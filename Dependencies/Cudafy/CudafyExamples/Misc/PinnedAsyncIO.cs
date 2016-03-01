/*
CUDAfy.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyExamples.Misc
{
    public class PinnedAsyncIO
    {
        public static int imin(float a, float b)
        {
            return (int)(a < b ? a : b);
        }

        public static float sum_squares(float x)
        {
            return (x * (x + 1) * (2 * x + 1) / 6);
        }

        public const int N = 1024 * 1024 * 4;
        public const int threadsPerBlock = 256;
        public static readonly int blocksPerGrid = Math.Min( 32, (N+threadsPerBlock-1) / threadsPerBlock );

        [Cudafy]
        public static void Dot(GThread thread, float[] a, float[] b, float[] c)
        {
            float[] cache = thread.AllocateShared<float>("cache", threadsPerBlock);

            int tid = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;
            int cacheIndex = thread.threadIdx.x;

            float temp = 0;
            while (tid < N)
            {
                temp += a[tid] * b[tid];
                tid += thread.blockDim.x * thread.gridDim.x;
            }

            // set the cache values
            cache[cacheIndex] = temp;

            // synchronize threads in this block
            thread.SyncThreads();

            // for reductions, threadsPerBlock must be a power of 2
            // because of the following code
            int i = thread.blockDim.x / 2;
            while (i != 0)
            {
                if (cacheIndex < i)
                    cache[cacheIndex] += cache[cacheIndex + i];
                thread.SyncThreads();
                i /= 2;
            }

            if (cacheIndex == 0)
                c[thread.blockIdx.x] = cache[0];
        }


        public static void Execute()
        {
            CudafyModule km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy();
                km.Serialize();
            }

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            gpu.LoadModule(km);

            float c = 0;

            int loops = 20;
            int batches = 12;

            // allocate memory on the cpu side
            float[] a = new float[N];
            float[] b = new float[N];
            float[] partial_c = new float[blocksPerGrid];

            // allocate the memory on the GPU
            float[] dev_a = gpu.Allocate<float>(N);
            float[] dev_b = gpu.Allocate<float>(N);
            float[] dev_partial_c = gpu.Allocate<float>(blocksPerGrid);

            float[] dev_test = gpu.Allocate<float>(blocksPerGrid * blocksPerGrid);

            // fill in the host memory with data
            for (int i = 0; i < N; i++)
            {
                a[i] = i;
                b[i] = i * 2;
            }

            // Synchronous Implementation
            Stopwatch sw = Stopwatch.StartNew();
            for (int l = 0; l < loops; l++)
            {
                for (int bat = 0; bat < batches; bat++)
                {
                    // copy the arrays 'a' and 'b' to the GPU
                    gpu.CopyToDevice(a, dev_a);
                    gpu.CopyToDevice(b, dev_b);

                    gpu.Launch(blocksPerGrid, threadsPerBlock, "Dot", dev_a, dev_b, dev_partial_c);

                    // copy the array 'c' back from the GPU to the CPU
                    gpu.CopyFromDevice(dev_partial_c, partial_c);
                    // finish up on the CPU side
                    c = 0;
                    for (int i = 0; i < blocksPerGrid; i++)
                    {
                        c += partial_c[i];
                    }
                }
            }
            long syncTime = sw.ElapsedMilliseconds;
            Console.WriteLine("Synchronous Time: {0}", syncTime);
            Console.WriteLine("Does GPU value {0} = {1}?\n", c, 2 * sum_squares((float)(N - 1)));

            // Asynchronous Pinned Memory Implementation
            IntPtr[] host_stages_a = new IntPtr[batches];
            IntPtr[] host_stages_b = new IntPtr[batches];
            IntPtr[] host_stages_c = new IntPtr[batches];
            for (int bat = 0; bat < batches; bat++)
            {
                host_stages_a[bat] = gpu.HostAllocate<float>(N);
                host_stages_b[bat] = gpu.HostAllocate<float>(N);
                host_stages_c[bat] = gpu.HostAllocate<float>(blocksPerGrid);
            }

            // Set GPU memory to zero
            gpu.Set(dev_a);
            gpu.Set(dev_b);
            gpu.Set(dev_partial_c);

            gpu.EnableSmartCopy();
            sw.Restart();
            for (int l = 0; l < loops; l++)
            {
                // Queue all the copying operations of the batch
                for (int bat = 0; bat < batches; bat++)
                {
                    // Finish processing the previous loop on CPU
                    if (l > 0)
                    {
                        gpu.SynchronizeStream(bat + 1);
                        c = 0;
                        for (int i = 0; i < blocksPerGrid; i++)
                        {
                            c += partial_c[i];
                        }
                    }
                    gpu.CopyToDeviceAsync(a, 0, dev_a, 0, N, bat + 1, host_stages_a[bat]);
                }
                // All copies to the GPU are put into a queue, so the different stream id's are abstract only
                // We are guaranteed that all previous copies with same stream id will be completed first.
                for (int bat = 0; bat < batches; bat++)
                    gpu.CopyToDeviceAsync(b, 0, dev_b, 0, N, bat + 1, host_stages_b[bat]);
                // Launch the kernels. These have same stream id as the copies and will take place as soon as 
                // the copy to the GPU with same stream id is complete. Hence kernels may be running in parallel
                // with copies fo higher stream id that are still running.
                for (int bat = 0; bat < batches; bat++)
                    gpu.LaunchAsync(blocksPerGrid, threadsPerBlock, bat + 1, "Dot", dev_a, dev_b, dev_partial_c);
                // Here we add to the copying from GPU queue. Copying will begin once the kernel with same stream
                // id is completed. If the GPU supports concurrent copying to and from at same time then the first 
                // copy from operations may be completed before all the copy to operations have.
                for (int bat = 0; bat < batches; bat++)
                    gpu.CopyFromDeviceAsync(dev_partial_c, 0, partial_c, 0, blocksPerGrid, bat + 1, host_stages_c[bat]);

            }
            // Finish processing the last loop on CPU
            for (int bat = 0; bat < batches; bat++)
            {
                gpu.SynchronizeStream(bat + 1);
                c = 0;
                for (int i = 0; i < blocksPerGrid; i++)
                {
                    c += partial_c[i];
                }
            }

            long asyncTime = sw.ElapsedMilliseconds;
            Console.WriteLine("Asynchronous Time: {0}", asyncTime);
            Console.WriteLine("Does GPU value {0} = {1}?\n", c, 2 * sum_squares((float)(N - 1)));
            gpu.DisableSmartCopy();

            // free memory on the gpu side
            gpu.FreeAll();

            // free memory on the cpu side
            gpu.HostFreeAll();

            // let's try and do this on the CPU in an straight forward fashion
            sw.Restart();
            c = 0;
            for (int l = 0; l < loops; l++)
                for (int bat = 0; bat < batches; bat++)
                    c = DotProduct(a, b);
            long cpuTime = sw.ElapsedMilliseconds;
            Console.WriteLine("CPU Time: {0}", cpuTime);
            Console.WriteLine("Does CPU value {0} = {1}?\n", c, 2 * sum_squares((float)(N - 1)));

            // let's try and do this on the CPU with Linq
            sw.Restart();
            c = 0;
            for (int l = 0; l < loops; l++)
                for (int bat = 0; bat < batches; bat++)
                    c = DotProductLinq(a, b);
            long cpuLinqTime = sw.ElapsedMilliseconds;
            Console.WriteLine("CPU Linq Time: {0}", cpuLinqTime);
            Console.WriteLine("Does CPU value {0} = {1}?\n", c, 2 * sum_squares((float)(N - 1)));

            // let's try and do this on the CPU with multiple threads
            DotProductDelegate dlgt = new DotProductDelegate(DotProduct);
            IAsyncResult[] res = new IAsyncResult[batches];
            for (int bat = 0; bat < batches; bat++)
                res[bat] = null;
            sw.Restart();
            c = 0;
            for (int l = 0; l < loops; l++)
                for (int bat = 0; bat < batches; bat++)
                {
                    if (res[bat] != null)
                        c = dlgt.EndInvoke(res[bat]);
                    res[bat] = dlgt.BeginInvoke(a, b, null, null);
                }
            for (int bat = 0; bat < batches; bat++)
                if (res[bat] != null)
                    c = dlgt.EndInvoke(res[bat]);
            long cpuMultiTime = sw.ElapsedMilliseconds;
            Console.WriteLine("CPU Multi Time: {0}", cpuMultiTime);
            Console.WriteLine("Does CPU value {0} = {1}?\n", c, 2 * sum_squares((float)(N - 1)));
        }

        private delegate float DotProductDelegate(float[] vec1, float[] vec2);

        private static float DotProduct(float[] vec1, float[] vec2)
        {
            if (vec1 == null)
                return 0;

            if (vec2 == null)
                return 0;

            if (vec1.Length != vec2.Length)
                return 0;

            double tVal = 0;
            for (int x = 0; x < vec1.Length; x++)
            {
                tVal += (double)(vec1[x] * vec2[x]);
            }

            return (float)tVal;
        }

        public static float DotProductLinq(float[] a, float[] b)
        {
            return a.Zip(b, (x, y) => x * y).Sum();
        }
    }
}

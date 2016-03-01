/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyByExample
{    
    public class dot
    {

        public static int imin(float a, float b)
        {
            return (int)(a < b ? a : b);
        }

        public static float sum_squares(float x)  
        {
            return (x*(x+1)*(2*x+1)/6);
        }

        public const int N = 33 * 1024;
        public const int threadsPerBlock = 256;
        public const int blocksPerGrid = 32;//imin( 32, (N+threadsPerBlock-1) / threadsPerBlock );

        [Cudafy]
        public static void Dot(GThread thread, float[] a, float[] b, float[] c ) 
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
            CudafyModule km = CudafyTranslator.Cudafy();

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(km);

            float c;

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
            for (int i=0; i<N; i++) 
            {
                a[i] = i;
                b[i] = i*2;
            }

            // copy the arrays 'a' and 'b' to the GPU
            gpu.CopyToDevice(a, dev_a);
            gpu.CopyToDevice(b, dev_b);

            gpu.Launch(blocksPerGrid, threadsPerBlock).Dot(dev_a, dev_b, dev_partial_c);

            // copy the array 'c' back from the GPU to the CPU
            gpu.CopyFromDevice(dev_partial_c, partial_c);

            // finish up on the CPU side
            c = 0;
            for (int i = 0; i < blocksPerGrid; i++)
            {
                c += partial_c[i];
            }

            Console.WriteLine("Does GPU value {0} = {1}?\n", c, 2 * sum_squares((float)(N - 1)));

            // free memory on the gpu side
            gpu.FreeAll();

            // free memory on the cpu side
            // No worries...
        }
    }
}

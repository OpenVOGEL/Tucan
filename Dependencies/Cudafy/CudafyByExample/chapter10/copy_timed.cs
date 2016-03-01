/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
using System;
using System.Collections.Generic;
using System.Text;
using Cudafy;
using Cudafy.Host;

namespace CudafyByExample
{
    public class copy_timed
    {
        public const int SIZE = 64*1024*1024;

        private GPGPU _gpu;

        private float cuda_malloc_test(int size, bool up) 
        {
            int[] a = new int[size];

            int[] dev_a = _gpu.Allocate<int>(size);
            
            _gpu.StartTimer();
            
            for (int i=0; i<100; i++) 
            {
                if (up)
                    _gpu.CopyToDevice(a, dev_a);
                else
                    _gpu.CopyFromDevice(dev_a, a);
            }

            float elapsedTime = _gpu.StopTimer();
            _gpu.FreeAll();
   
            GC.Collect();
            return elapsedTime;
        }

        private float cuda_host_alloc_test(int size, bool up) 
        {
            IntPtr a = _gpu.HostAllocate<int>(size);
            int[] dev_a = _gpu.Allocate<int>(size);
            
            _gpu.StartTimer();
            
            for (int i=0; i<100; i++) 
            {
                if (up)
                    _gpu.CopyToDevice(a, 0, dev_a, 0, size);
                else
                    _gpu.CopyFromDevice(dev_a, 0, a, 0, size);
            }

            float elapsedTime = _gpu.StopTimer();
            _gpu.FreeAll();
            _gpu.HostFree(a);
            GC.Collect();
            return elapsedTime;
        }

        private float cuda_host_alloc_copy_test(int size, bool up)
        {
            IntPtr a = _gpu.HostAllocate<int>(size);
            IntPtr b = _gpu.HostAllocate<int>(size);
            int[] dev_a = _gpu.Allocate<int>(size);
            int[] host_a = new int[size];
            _gpu.StartTimer();

            for (int i = 0; i < 50; i++) // 50 = two copies per loop
            {
                if (up)
                {
                    a.Write(host_a); 
                    _gpu.CopyToDeviceAsync(a, 0, dev_a, 0, size);
                    b.Write(host_a); 
                    _gpu.CopyToDeviceAsync(b, 0, dev_a, 0, size);
                }
                else
                {
                    _gpu.CopyFromDeviceAsync(dev_a, 0, a, 0, size);
                    b.Read(host_a); 
                    _gpu.CopyFromDeviceAsync(dev_a, 0, b, 0, size);
                    b.Read(host_a); 
                }
            }
            _gpu.SynchronizeStream();

            float elapsedTime = _gpu.StopTimer();
            _gpu.FreeAll();
            _gpu.HostFree(a);
            _gpu.HostFree(b);
            GC.Collect();
            return elapsedTime;
        }

        public void Execute() 
        {
            float elapsedTime;
            float MB = (float)100*SIZE*sizeof(int)/1024/1024;

            _gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            var props = _gpu.GetDeviceProperties();

            Console.WriteLine(props.Name);
            Console.WriteLine("Using {0}optimized driver.", props.HighPerformanceDriver ? "" : "non-");

            // try it with malloc
            elapsedTime = cuda_malloc_test(SIZE, true);
            Console.WriteLine("Time using cudaMalloc: {0} ms",
                    elapsedTime);
            Console.WriteLine("\tMB/s during copy up: {0}",
                    MB / (elapsedTime / 1000));

            elapsedTime = cuda_malloc_test(SIZE, false);
            Console.WriteLine("Time using cudaMalloc: {0} ms",
                    elapsedTime);
            Console.WriteLine("\tMB/s during copy down: {0}",
                    MB / (elapsedTime / 1000));

            // now try it with cudaHostAlloc
            elapsedTime = cuda_host_alloc_test(SIZE, true);
            Console.WriteLine("Time using cudaHostAlloc: {0} ms",
                    elapsedTime);
            Console.WriteLine("\tMB/s during copy up: {0}",
                    MB / (elapsedTime / 1000));

            elapsedTime = cuda_host_alloc_test(SIZE, false);
            Console.WriteLine("Time using cudaHostAlloc: {0} ms",
                    elapsedTime);
            Console.WriteLine("\tMB/s during copy down: {0}",
                    MB / (elapsedTime / 1000));

            #region 15-06-2011 Not working on laptop, works fine on workstation
            
            //// now try it with cudaHostAlloc copy
            //elapsedTime = cuda_host_alloc_copy_test(SIZE, true);
            //Console.WriteLine("Time using cudaHostAlloc + async copy: {0} ms",
            //        elapsedTime);
            //Console.WriteLine("\tMB/s during copy up: {0}",
            //        MB / (elapsedTime / 1000));

            //elapsedTime = cuda_host_alloc_copy_test(SIZE, false);
            //Console.WriteLine("Time using cudaHostAlloc + async copy: {0} ms",
            //        elapsedTime);
            //Console.WriteLine("\tMB/s during copy down: {0}",
            //        MB / (elapsedTime / 1000));

            #endregion
        }
    }
}

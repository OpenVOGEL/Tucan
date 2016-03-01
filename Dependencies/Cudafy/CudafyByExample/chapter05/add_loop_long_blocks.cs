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
    public class add_loop_long_blocks
    {
        public const int N = 33 * 1024;

        public static void Execute()
        {
            // Translates this class to CUDA C and then compliles
            CudafyModule km = CudafyTranslator.Cudafy();

            // Get the first GPU and load the module
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId);
            gpu.LoadModule(km);

            // Create some arrays on the host
            int[] a = new int[N];
            int[] b = new int[N];
            int[] c = new int[N];

            // allocate the memory on the GPU
            int[] dev_c = gpu.Allocate<int>(c);

            // fill the arrays 'a' and 'b' on the CPU
            for (int i = 0; i < N; i++)
            {
                a[i] = i;
                b[i] = 2 * i;
            }

            // copy the arrays 'a' and 'b' to the GPU
            int[] dev_a = gpu.CopyToDevice(a);
            int[] dev_b = gpu.CopyToDevice(b);

            // Launch 128 blocks of 128 threads each
            gpu.Launch(128, 128).add(dev_a, dev_b, dev_c);

            // copy the array 'c' back from the GPU to the CPU
            gpu.CopyFromDevice(dev_c, c);

            // verify that the GPU did the work we requested
            bool success = true;
            for (int i = 0; i < N; i++)
            {
                if ((a[i] + b[i]) != c[i])
                {
                    Console.WriteLine("{0} + {1} != {2}", a[i], b[i], c[i]);
                    success = false;
                    break;
                }
            }
            if (success)
                Console.WriteLine("We did it!");

            // free the memory allocated on the GPU
            gpu.FreeAll();
        }

        [Cudafy]
        public static void add(GThread thread, int[] a, int[] b, int[] c)
        {
            int tid = thread.threadIdx.x + thread.blockIdx.x * thread.blockDim.x;

            while (tid < N)
            {
                c[tid] = a[tid] + b[tid];
                tid += thread.blockDim.x * thread.gridDim.x;
            }
        }
    }
}

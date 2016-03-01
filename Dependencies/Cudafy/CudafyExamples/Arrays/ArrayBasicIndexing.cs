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
using System.Text;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyExamples.Arrays
{
    public class ArrayBasicIndexing
    {
        public const int N = 1 * 1024;

        public static void Execute()
        {
            GPGPU gpu = CudafyHost.GetDevice(eArchitecture.OpenCL12, 0);
            eArchitecture arch = gpu.GetArchitecture();
            CudafyModule km = CudafyTranslator.Cudafy(arch);
                                 
            gpu.LoadModule(km);

            int[] a = new int[N];
            int[] b = new int[N];
            int[] c = new int[N];

            // allocate the memory on the GPU
            int[] dev_a = gpu.Allocate<int>(a);
            int[] dev_b = gpu.Allocate<int>(b);
            int[] dev_c = gpu.Allocate<int>(c);
           
            // fill the arrays 'a' and 'b' on the CPU
            for (int i = 0; i < N; i++)
            {
                a[i] = i;
                b[i] = 2 * i;
            }

            for (int l = 0; l < km.Functions.Count; l++)
            {
                string function = "add_" + l.ToString();
                Console.WriteLine(function);
                
                // copy the arrays 'a' and 'b' to the GPU
                gpu.CopyToDevice(a, dev_a);
                gpu.CopyToDevice(b, dev_b);

                gpu.Launch(128, 1, function, dev_a, dev_b, dev_c);

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
            }

            // free the memory allocated on the GPU
            gpu.Free(dev_a);
            gpu.Free(dev_b);
            gpu.Free(dev_c);

            // free the memory we allocated on the CPU
            // Not necessary, this is .NET
        }

        [Cudafy]
        public static void add_0(GThread thread, int[] a, int[] b, int[] c)
        {
            int tid = thread.blockIdx.x;
            while (tid < N)
            {
                c[tid] = a[tid] + b[tid];
                tid += thread.gridDim.x;
            }
        }

        [Cudafy]
        public static void add_1(GThread thread, int[] a, int[] b, int[] c)
        {
            int tid = thread.blockIdx.x;
            while (tid < a.Length)
            {
                c[tid] = a[tid] + b[tid];
                tid += thread.gridDim.x;
            }
        }

        [Cudafy]
        public static void add_2(GThread thread, int[] a, int[] b, int[] c)
        {
            int tid = thread.blockIdx.x;
            while (tid < b.Length)
            {
                c[tid] = a[tid] + b[tid];
                tid += thread.gridDim.x;
            }
        }

        [Cudafy]
        public static void add_3(GThread thread, int[] a, int[] b, int[] c)
        {
            int tid = thread.blockIdx.x;
            while (tid < c.Length)
            {
                c[tid] = a[tid] + b[tid];
                tid += thread.gridDim.x;
            }
        }

        [Cudafy]
        public static void add_4(GThread thread, int[] a, int[] b, int[] c)
        {
            int tid = thread.blockIdx.x;
            int rank = a.Rank;
            while (tid < c.Length)
            {
                c[tid] = a[tid] + b[tid];
                tid += thread.gridDim.x;
            }
        }
    }
}

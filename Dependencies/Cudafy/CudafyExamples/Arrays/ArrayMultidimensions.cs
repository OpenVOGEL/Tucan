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

namespace CudafyExamples.Arrays
{
    public class ArrayMultidimensions
    {
        public const int XSIZE = 4;
        public const int YSIZE = 8;
        public const int ZSIZE = 16;

        [Cudafy]
        public static int[] Constant1D = new int[XSIZE];

        [Cudafy]
        public static int[,] Constant2D = new int[XSIZE, YSIZE];

        [Cudafy]
        public static int[, ,] Constant3D = new int[XSIZE, YSIZE, ZSIZE];

        public static void Execute()
        {
            CudafyModule km = CudafyTranslator.Cudafy();

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            gpu.LoadModule(km);

            // Set up host arrays for reading back results
            int[] result_array1 = new int[XSIZE];
            int[] result_array2 = new int[XSIZE * YSIZE];
            int[] result_array3 = new int[XSIZE * YSIZE * ZSIZE];

            // Set up GPU arrays
            int[] dev_array1 = gpu.Allocate<int>(XSIZE);
            int[] dev_array2 = gpu.Allocate<int>(XSIZE * YSIZE);
            int[] dev_array3 = gpu.Allocate<int>(XSIZE * YSIZE * ZSIZE);

            // Set up host arrays
            int[] host_constant1D = new int[XSIZE];
            int[,] host_constant2D = new int[XSIZE, YSIZE];
            int[, ,] host_constant3D = new int[XSIZE, YSIZE, ZSIZE];

            bool pass = true;

            Console.WriteLine("copy1D");
            InitializeHostConstants(host_constant1D, host_constant2D, host_constant3D);
            gpu.CopyToConstantMemory(host_constant1D, Constant1D);
            gpu.Launch(1, 1, "copy1D", dev_array1);
            gpu.CopyFromDevice(dev_array1, result_array1);
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                pass = result_array1[x] == host_constant1D[x];
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            
            Console.WriteLine("copy1D_alt");
            InitializeHostConstants(host_constant1D, host_constant2D, host_constant3D);            
            gpu.CopyToConstantMemory(host_constant1D, Constant1D);
            gpu.Launch(1, 1, "copy1D_alt", dev_array1);
            gpu.CopyFromDevice(dev_array1, result_array1);
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                pass = result_array1[x] == host_constant1D[x] * Constant1D.Rank; 
            }
            Console.WriteLine(pass ? "Pass" : "Fail");


            Console.WriteLine("copy2D");
            InitializeHostConstants(host_constant1D, host_constant2D, host_constant3D);
            gpu.CopyToConstantMemory(host_constant2D, Constant2D);
            gpu.Launch(XSIZE, 1, "copy2D", dev_array2);
            gpu.CopyFromDevice(dev_array2, result_array2);
            int i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE && pass; y++)
                {
                    pass = result_array2[x * YSIZE + y] == host_constant2D.Rank * i++;
                }
            Console.WriteLine(pass ? "Pass" : "Fail");


            Console.WriteLine("copy3D");
            InitializeHostConstants(host_constant1D, host_constant2D, host_constant3D);
            gpu.CopyToConstantMemory(host_constant3D, Constant3D);
            gpu.Launch(XSIZE, 1, "copy3D", dev_array3);
            gpu.CopyFromDevice(dev_array3, result_array3);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    for (int z = 0; z < ZSIZE && pass; z++)
                    {
                        pass = result_array3[x * YSIZE * ZSIZE + y * ZSIZE + z] == host_constant3D.Rank * i++;
                    }
            Console.WriteLine(pass ? "Pass" : "Fail");

            gpu.FreeAll();
        }

        [Cudafy]
        public static void copy1D(GThread thread, int[] result)
        {
            int tid = thread.blockIdx.x;
            while (tid < result.Length)
            {
                result[tid] = Constant1D[tid];
                tid++;
            }
        }

        [Cudafy]
        public static void copy1D_alt(GThread thread, int[] result)
        {
            int tid = thread.blockIdx.x;
            while (tid < Constant1D.Length)
            {
                result[tid] = Constant1D[tid] * Constant1D.Rank;
                tid++;
            }
        }

        [Cudafy]
        public static void copy2D(GThread thread, int[] result)
        {
            int[,] cache = thread.AllocateShared<int>("cache", XSIZE, YSIZE);
            int x = thread.blockIdx.x;
            int y = 0;
            while (y < YSIZE)
            {
                cache[x, y] = Constant2D[x, y] * Constant2D.Rank;
                result[x * YSIZE + y] = cache[x, y];
                y++;
            }
        }

        [Cudafy]
        public static void copy3D(GThread thread, int[] result)
        {
            int x = thread.blockIdx.x;
            int y = 0;
            int z = 0;
            while (y < YSIZE)
            {
                while (z < ZSIZE)
                {
                    result[x * YSIZE * ZSIZE + y * ZSIZE + z] = Constant3D[x, y, z] * Constant3D.Rank;
                    //Debug.WriteLine(string.Format("Index {0} = {1}", x * YSIZE * ZSIZE + y * ZSIZE + z, Constant3D[x, y, z] * Constant3D.Rank));
                    z++;
                }
                z = 0;
                y++;
            }
        }

        public static void InitializeHostConstants(int[] host_constant1D, int[,] host_constant2D, int[, ,] host_constant3D)
        {
            int i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    host_constant2D[x, y] = i++;
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    for (int z = 0; z < ZSIZE; z++)
                        host_constant3D[x, y, z] = i++;
        }
    }
}

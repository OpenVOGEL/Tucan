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
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyExamples.Arrays
{


    /// <summary>
    /// Is dependent on ComplexFloat type.
    /// </summary>
    public class GlobalArrays
    {
        public const int XSIZE = 4;
        public const int YSIZE = 8;
        public const int ZSIZE = 16;

        /// <summary>
        /// This type is used by GlobalArrays and must be selected for Cudafying.
        /// </summary>
        [Cudafy]
        public struct ComplexFloat
        {
            public ComplexFloat(float r, float i)
            {
                Real = r;
                Imag = i;
            }
            public float Real;
            public float Imag;
            public ComplexFloat Add(ComplexFloat c)
            {
                return new ComplexFloat(Real + c.Real, Imag + c.Imag);
            }
        }

        [Cudafy]
        public static ComplexFloat[] Constant1D = new ComplexFloat[8];

        public static void Execute()
        {
            CudafyModule km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy();//typeof(ComplexFloat), typeof(GlobalArrays));
                km.Serialize();
            }

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            gpu.LoadModule(km);

            bool pass = true;
            int i = 0;
            // 1D
            Console.WriteLine("global1DArray");
            int[] host_array1D = new int[XSIZE];
            int[] result_array1D = new int[XSIZE];
            int[] dev_array1D = gpu.Allocate<int>(XSIZE);
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                host_array1D[x] = i++;
            gpu.CopyToDevice(host_array1D, dev_array1D);
            gpu.Launch(XSIZE, 1, "global1DArray", dev_array1D);
            gpu.CopyFromDevice(dev_array1D, result_array1D);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                pass = result_array1D[x] == host_array1D[x] * host_array1D[x] * result_array1D.Rank;
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // 2D
            Console.WriteLine("global2DArray");
            int[,] host_array2D = new int[XSIZE, YSIZE];
            int[,] result_array2D = new int[XSIZE, YSIZE];
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    host_array2D[x, y] = i++;
            int[,] dev_array2D = gpu.CopyToDevice(host_array2D);
            gpu.Launch(XSIZE, 1, "global2DArray", dev_array2D);
            gpu.CopyFromDevice(dev_array2D, result_array2D);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE && pass; y++)
                {
                    pass = result_array2D[x, y] == host_array2D[x, y] * result_array2D.Rank;
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // 3D alt
            Console.WriteLine("global3DArrayAlt");
            int[,,] host_array3DAlt = new int[XSIZE, YSIZE, ZSIZE];
            int[,,] result_array3DAlt = new int[XSIZE, YSIZE, ZSIZE];
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    for (int z = 0; z < ZSIZE; z++)
                        host_array3DAlt[x, y, z] = i++;
            int[, ,] dev_array3DAlt = gpu.CopyToDevice(host_array3DAlt);
            gpu.Launch(1, 1, "global3DArrayAlt", dev_array3DAlt);
            gpu.CopyFromDevice(dev_array3DAlt, result_array3DAlt);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE; y++)
                {
                    for (int z = 0; z < ZSIZE && pass; z++, i++)
                    {
                        pass = result_array3DAlt[x, y, z] == i;
                    }
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // 3D
            Console.WriteLine("global3DArray");
            int[,,] host_array3D = new int[XSIZE, YSIZE, ZSIZE];
            int[,,] result_array3D = new int[XSIZE, YSIZE, ZSIZE];
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    for (int z = 0; z < ZSIZE; z++)
                        host_array3D[x, y, z] = i++;
            int[,,] dev_array3D = gpu.CopyToDevice(host_array3D);
            gpu.Launch(new dim3(XSIZE, YSIZE), 1, "global3DArray", dev_array3D);
            gpu.CopyFromDevice(dev_array3D, result_array3D);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE; y++)
                {
                    for (int z = 0; z < ZSIZE && pass; z++, i++)
                    {
                        pass = result_array3D[x, y, z] == host_array3D[x, y, z] * result_array3D.Rank;
                    }
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");
            
            // 1D Structs
            Console.WriteLine("global1DStructArray");
            ComplexFloat[] host_array1DS = new ComplexFloat[XSIZE];
            ComplexFloat[] result_array1DS = new ComplexFloat[XSIZE];
            for (int x = 0; x < XSIZE; x++)
                    host_array1DS[x] = new ComplexFloat(x * 2, x);
            ComplexFloat[] dev_array1DS = gpu.CopyToDevice(host_array1DS);
            gpu.Launch(XSIZE, 1, "global1DStructArray", dev_array1DS);
            gpu.CopyFromDevice(dev_array1DS, result_array1DS);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                ComplexFloat expected = host_array1DS[x].Add(host_array1DS[x]);
                ComplexFloat res = result_array1DS[x];
                pass = res.Real == expected.Real && res.Imag == expected.Imag;           
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // 2D Structs
            Console.WriteLine("global2DStructArray");
            ComplexFloat[,] host_array2DS = new ComplexFloat[XSIZE, YSIZE];
            ComplexFloat[,] result_array2DS = new ComplexFloat[XSIZE, YSIZE];
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++, i++)
                    host_array2DS[x, y] = new ComplexFloat(i * 2, i);
            ComplexFloat[,] dev_array2DS = gpu.CopyToDevice(host_array2DS);
            gpu.Launch(XSIZE, 1, "global2DStructArray", dev_array2DS);
            gpu.CopyFromDevice(dev_array2DS, result_array2DS);
            pass = true;
            i = 0;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE && pass; y++, i++)
                {
                    ComplexFloat expected = new ComplexFloat(i * 2, i).Add(new ComplexFloat(i * 2, i));
                    ComplexFloat res = result_array2DS[x, y];
                    pass = res.Real == expected.Real && res.Imag == expected.Imag; 
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // 3D Structs
            Console.WriteLine("global3DStructArray");
            ComplexFloat[,,] host_array3DS = new ComplexFloat[XSIZE, YSIZE, ZSIZE];
            ComplexFloat[,,] result_array3DS = new ComplexFloat[XSIZE, YSIZE, ZSIZE];
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                    for (int z = 0; z < ZSIZE; z++, i++)
                        host_array3DS[x, y, z] = new ComplexFloat(i * 2, i);
            ComplexFloat[,,] dev_array3DS = gpu.CopyToDevice(host_array3DS);
            gpu.Launch(new dim3(XSIZE, YSIZE), 1, "global3DStructArray", dev_array3DS);
            gpu.CopyFromDevice(dev_array3DS, result_array3DS);
            pass = true;
            i = 0;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE; y++)
                {
                    for (int z = 0; z < ZSIZE && pass; z++, i++)
                    {
                        ComplexFloat expected = new ComplexFloat(i * 2, i).Add(new ComplexFloat(i * 2, i));
                        ComplexFloat res = result_array3DS[x, y, z];
                        pass = res.Real == expected.Real && res.Imag == expected.Imag;
                    }
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // Foreach
            Console.WriteLine("foreach");
            host_array1DS = new ComplexFloat[XSIZE];
            result_array1DS = new ComplexFloat[XSIZE];
            ComplexFloat[] dev_array1D_2 = gpu.Allocate<ComplexFloat>(XSIZE);
            for (int x = 0; x < XSIZE; x++)
                host_array1DS[x] = new ComplexFloat(x * 2, x);
            dev_array1DS = gpu.CopyToDevice(host_array1DS);
            gpu.Launch(1, 1, "global1DStructArrayForeach", dev_array1DS, dev_array1D_2);
            gpu.CopyFromDevice(dev_array1D_2, result_array1DS);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                ComplexFloat expected = host_array1DS[x].Add(host_array1DS[x]);
                ComplexFloat res = result_array1DS[x];
                pass = res.Real == expected.Real && res.Imag == expected.Imag;
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            gpu.FreeAll();
        }

        [Cudafy(eCudafyType.Global)]
        public static void global1DArray(GThread thread, int[] result)
        {
            int x = thread.blockIdx.x;
            result[x] = result[x] * result[x] * result.Rank;
        }

        [Cudafy]
        public static void global2DArray(GThread thread, int[,] result)
        {
            int x = thread.blockIdx.x;
            int y = 0;

            while (y < YSIZE)
            {
                result[x, y] = result[x, y] * result.Rank;
                y++;
            }
        }

        [Cudafy]
        public static void global3DArrayAlt(GThread thread, int[,,] result)
        {
            int x = 0;
            int y = 0;
            int z = 0;
            for (x = 0; x < XSIZE; x++)
                for (y = 0; y < YSIZE; y++)
                    for (z = 0; z < ZSIZE; z++)
                        result[x, y, z] = x * YSIZE * ZSIZE + y * ZSIZE + z;
        }

        [Cudafy]
        public static void global3DArray(GThread thread, int[, ,] result)
        {
            int x = thread.blockIdx.x;
            int y = thread.blockIdx.y;
            int z = 0;
            while (z < ZSIZE)
            {
                result[x, y, z] = result[x, y, z] * result.Rank;
                z++;
            }
        }

        [Cudafy]
        public static void global1DStructArray(GThread thread, ComplexFloat[] result)
        {
            int x = thread.blockIdx.x;
            result[x] = result[x].Add(result[x]);
        }

        [Cudafy]
        public static void global1DStructArrayForeach(ComplexFloat[] input, ComplexFloat[] result)
        {
            int i = 0;
            foreach (ComplexFloat cf in input)
            {
                ComplexFloat t = cf.Add(cf);
                result[i++] = t;
            }

        }

        [Cudafy]
        public static void global2DStructArray(GThread thread, ComplexFloat[,] result)
        {
            int x = thread.blockIdx.x;
            int y = 0;

            while (y < result.GetLength(1))
            {
                result[x, y] = result[x, y].Add(result[x, y]);
                y++;
            }
        }

        [Cudafy]
        public static void global3DStructArray(GThread thread, ComplexFloat[, ,] result)
        {
            int x = thread.blockIdx.x;
            int y = thread.blockIdx.y;
            int z = 0;
            //while (z < ZSIZE)
            while (z < result.GetLength(2))
            {
                result[x, y, z] = result[x, y, z].Add(result[x, y, z]);
                z++;
            }            
        }
    }
}

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
using Cudafy.Types;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
//using Cudafy.ReflectorWrapper;
namespace CudafyExamples.Complex
{
    public class ComplexNumbersF
    {
        public const int XSIZE = 128;
        public const int YSIZE = 256;

        public static void Execute()
        {
            CudafyModule km = CudafyTranslator.Cudafy();

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            gpu.LoadModule(km);

            // 2D
            Console.WriteLine("kernel");
            ComplexF[,] host_A = new ComplexF[XSIZE, YSIZE];
            ComplexF[,] host_B = new ComplexF[XSIZE, YSIZE];
            ComplexF[,] host_C = new ComplexF[XSIZE, YSIZE];
            int i = 0;
            for (int x = 0; x < XSIZE; x++)
                for (int y = 0; y < YSIZE; y++)
                {
                    host_A[x, y] = new ComplexF(i, i);
                    host_B[x, y] = new ComplexF(2, 0); 
                    i++;
                }
            ComplexF[,] dev_A = gpu.CopyToDevice(host_A);
            ComplexF[,] dev_B = gpu.CopyToDevice(host_B);
            ComplexF[,] dev_C = gpu.Allocate<ComplexF>(XSIZE, YSIZE);

            Console.WriteLine("complexAdd");
            gpu.Launch(XSIZE, 1, "complexAdd", dev_A, dev_B, dev_C);
            gpu.CopyFromDevice(dev_C, host_C);
            i = 0;
            bool pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE && pass; y++)
                {
                    ComplexF expected = ComplexF.Add(host_A[x, y], host_B[x, y]);
                    pass = host_C[x, y].x == expected.x && host_C[x, y].y == expected.y;
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            Console.WriteLine("complexSub");
            gpu.Launch(XSIZE, 1, "complexSub", dev_A, dev_B, dev_C);
            gpu.CopyFromDevice(dev_C, host_C);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE && pass; y++)
                {
                    ComplexF expected = ComplexF.Subtract(host_A[x, y], host_B[x, y]);
                    pass = host_C[x, y].x == expected.x && host_C[x, y].y == expected.y;
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            Console.WriteLine("complexMpy");
            gpu.Launch(XSIZE, 1, "complexMpy", dev_A, dev_B, dev_C);
            gpu.CopyFromDevice(dev_C, host_C);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE && pass; y++)
                {
                    ComplexF expected = ComplexF.Multiply(host_A[x, y], host_B[x, y]);
                    //Console.WriteLine("{0} {1} : {2} {3}", host_C[x, y].R, host_C[x, y].I, expected.R, expected.I);
                    pass = Verify(host_C[x, y], expected, 1e-14F); 
                    i++;
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            Console.WriteLine("complexDiv");
            gpu.Launch(XSIZE, 1, "complexDiv", dev_A, dev_B, dev_C);
            gpu.CopyFromDevice(dev_C, host_C);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE && pass; y++)
                {
                    ComplexF expected = ComplexF.Divide(host_A[x, y], host_B[x, y]);
                    //Console.WriteLine("{0} {1} : {2} {3}", host_C[x, y].R, host_C[x, y].I, expected.R, expected.I);
                    if (i > 0)
                        pass = Verify(host_C[x, y], expected, 1e-13F);
                    i++;
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            Console.WriteLine("complexAbs");
            gpu.Launch(XSIZE, 1, "complexAbs", dev_A, dev_C);
            gpu.CopyFromDevice(dev_C, host_C);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE; x++)
            {
                for (int y = 0; y < YSIZE && pass; y++)
                {
                    float expected = ComplexF.Abs(host_A[x, y]);
                    pass = Verify(host_C[x, y].x, expected, 1e-2F);
                    //Console.WriteLine("{0} {1} : {2}", host_C[x, y].x, host_C[x, y].y, expected);
                    i++;
                }
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            gpu.FreeAll();
        }

        private static bool Verify(ComplexF x, ComplexF y, float delta)
        {
            if (Math.Abs(x.x - y.x) > delta || Math.Abs(x.y - y.y) > delta)
                return false;
            return true;
        }

        private static bool Verify(float x, float y, float delta)
        {
            if (Math.Abs(x - y) > delta)
                return false;
            return true;
        }

        [Cudafy]
        public static void complexAdd(GThread thread, ComplexF[,] a, ComplexF[,] b, ComplexF[,] c)
        {
            int x = thread.blockIdx.x;
            int y = 0;
            while (y < YSIZE)
            {
                c[x, y] = ComplexF.Add(a[x, y], b[x, y]);
                y++;
            }
        }

        [Cudafy]
        public static void complexSub(GThread thread, ComplexF[,] a, ComplexF[,] b, ComplexF[,] c)
        {
            int x = thread.blockIdx.x;
            int y = 0;
            while (y < YSIZE)
            {
                c[x, y] = ComplexF.Subtract(a[x, y], b[x, y]);
                y++;
            }
        }

        [Cudafy]
        public static void complexMpy(GThread thread, ComplexF[,] a, ComplexF[,] b, ComplexF[,] c)
        {
            int x = thread.blockIdx.x;
            int y = 0;
            while (y < YSIZE)
            {
                c[x, y] = ComplexF.Multiply(a[x, y], b[x, y]);
                y++;
            }
        }

        [Cudafy]
        public static void complexDiv(GThread thread, ComplexF[,] a, ComplexF[,] b, ComplexF[,] c)
        {
            int x = thread.blockIdx.x;
            int y = 0;
            while (y < YSIZE)
            {
                c[x, y] = ComplexF.Divide(a[x, y], b[x, y]);
                y++;
            }
        }

        [Cudafy]
        public static void complexAbs(GThread thread, ComplexF[,] a, ComplexF[,] c)
        {
            int x = thread.blockIdx.x;
            int y = 0;
            float j = 1.0F;
            float k = 2.0F;
            ComplexF cf = new ComplexF(3 + j, 7 + k);
            while (y < YSIZE)
            {
                c[x, y].x = ComplexF.Abs(a[x, y]);
                c[x, y].y = cf.y;// 0.0F;
                y++;
            }
        }
    }
}

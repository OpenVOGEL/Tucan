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

namespace CudafyExamples.Dummy
{
    [CudafyDummy]
    public struct DummyComplexFloat
    {
        
        public DummyComplexFloat(float r, float i)
        {
            Real = r;
            Imag = i;
        }
        public float Real;
        public float Imag;
        public DummyComplexFloat Add(DummyComplexFloat c)
        {
            return new DummyComplexFloat(Real + c.Real, Imag + c.Imag);
        }
    }
    //[CudafyDummy]
    //public struct DummyDefaultStruct
    //{
    //    public int X;
    //    public int Y;
    //}
    
    public class DummyFunctions
    {        
        public static void Execute()
        {
            CudafyModule km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy(typeof(DummyComplexFloat), typeof(DummyFunctions));
                km.Serialize();
            }

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            gpu.LoadModule(km);

            bool pass = true;
            int i = 0;
            // 1D
            Console.WriteLine("DummyFunction");
            int[] host_array1D = new int[XSIZE];
            int[] result_array1D = new int[XSIZE];
            int[] dev_array1D = gpu.Allocate<int>(XSIZE);
            i = 0;
            for (int x = 0; x < XSIZE; x++)
                host_array1D[x] = i++;
            gpu.CopyToDevice(host_array1D, dev_array1D);
            gpu.Launch(XSIZE, 1, "DummyFunction", dev_array1D);
            gpu.CopyFromDevice(dev_array1D, result_array1D);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                pass = result_array1D[x] == host_array1D[x] * host_array1D[x] * result_array1D.Rank;
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // DummyComplexFloatOp
            Console.WriteLine("DummyComplexFloatOp");
            DummyComplexFloat[] host_array1DS = new DummyComplexFloat[XSIZE];
            DummyComplexFloat[] result_array1DS = new DummyComplexFloat[XSIZE];
            for (int x = 0; x < XSIZE; x++)
                host_array1DS[x] = new DummyComplexFloat(x * 2, x);
            DummyComplexFloat[] dev_array1DS = gpu.CopyToDevice(host_array1DS);
            gpu.Launch(XSIZE, 1, "DummyComplexFloatOp", dev_array1DS);
            gpu.CopyFromDevice(dev_array1DS, result_array1DS);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                DummyComplexFloat expected = host_array1DS[x].Add(host_array1DS[x]);
                DummyComplexFloat res = result_array1DS[x];
                pass = res.Real == expected.Real && res.Imag == expected.Imag;
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            // DummyDummyComplexFloatOp
            Console.WriteLine("DummyDummyComplexFloatFunction");
            host_array1DS = new DummyComplexFloat[XSIZE];
            result_array1DS = new DummyComplexFloat[XSIZE];
            for (int x = 0; x < XSIZE; x++)
                host_array1DS[x] = new DummyComplexFloat(x * 2, x);
            dev_array1DS = gpu.CopyToDevice(host_array1DS);
            gpu.Launch(XSIZE, 1, "DummyDummyComplexFloatFunction", dev_array1DS);
            gpu.CopyFromDevice(dev_array1DS, result_array1DS);
            i = 0;
            pass = true;
            for (int x = 0; x < XSIZE && pass; x++)
            {
                DummyComplexFloat expected = host_array1DS[x].Add(host_array1DS[x]);
                DummyComplexFloat res = result_array1DS[x];
                pass = res.Real == expected.Real && res.Imag == expected.Imag;
            }
            Console.WriteLine(pass ? "Pass" : "Fail");

            gpu.FreeAll();
        }

        [CudafyDummy]
        public const int XSIZE = 4096;

        [Cudafy]
        public static int CONSTANT = 42;

        [CudafyDummy]
        public static void DummyFunction(int[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i] * a[i] * a.Rank;
            }
        }

        [Cudafy]
        public static void DummyComplexFloatOp(GThread thread, DummyComplexFloat[] result)
        {
            int x = thread.blockIdx.x;
            result[x] = result[x].Add(result[x]);
            DummyComplexFloat d = new DummyComplexFloat();

            result[0] = d;
        }

        [CudafyDummy]
        public static void DummyDummyComplexFloatFunction(DummyComplexFloat[] result)
        {
            for (int i = 0; i < XSIZE; i++)
            {
                result[i] = result[i].Add(result[i]);
            }

        }
    }
}

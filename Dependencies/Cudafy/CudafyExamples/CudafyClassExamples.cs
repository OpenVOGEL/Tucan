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
using System.Linq;

namespace CudafyExamples
{
    [Cudafy] 
    public class BaseClass
    {
        protected float field;

        protected int[] baseArray;
    }

    [Cudafy]
    public class MemberClass
    {
        public int Field;

        public float[] MemberClassArray;

        public float Test()
        {
            return MemberClassArray[0] * Field;
        }
    }

    [Cudafy]
    public class DerivedClass : BaseClass
    {
        float[] array1D;

        float[,] array2D;

        public MemberClass MemberClassInstance;

        [CudafyIgnore]
        public void PopulateMembers()
        {
            field = 7;
            array1D = new float[] { 2.3f, 5.6f, 7.6f, 3.4f };
            array2D = new float[,] { { 2.9f, 7.6f }, { 2.3f, 1.2f } };
            baseArray = new int[] { 1, 2, 3, 4 };
        }

        public float Sum()
        {
            float sum = 0;
            for (int i = 0; i < array1D.Length; ++i)
                sum += array1D[i];
            return sum;
        }

        public float SumDimension0()
        {
            float sum = 0;
            for (int i = 0; i < array2D.GetLength(0); ++i)
                sum += array2D[i, 1];
            return sum;
        }

        public int SumBase()
        {
            int sum = 0;
            for (int i = 0; i < baseArray.Length; ++i)
                sum += baseArray[i];
            return sum;
        }
    }

    [Cudafy]
    public class ArrayView
    {
        float[] arrayData;

        int arrayOffset;

        public void CreateView(float[] arrayData, int arrayOffset)
        {
            this.arrayData = arrayData;
            this.arrayOffset = arrayOffset;
        }

        public float GetElement(int index)
        {
            return arrayData[arrayOffset + index];
        }

        public void SetElement(int index, float value)
        {
            arrayData[arrayOffset + index] = value;
        }
    }

    /// <summary>
    /// Examples 
    /// </summary>
    public class CudafyClassExamples
    {
        public static void Execute()
        {
            bool previousValue = CudafyTranslator.AllowClasses;
            CudafyTranslator.AllowClasses = true;
            CudafyModule km = CudafyTranslator.Cudafy(new Type[] { typeof(BaseClass), typeof(MemberClass), typeof(DerivedClass), typeof(ArrayView), typeof(CudafyClassExamples) });
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, 0);
            gpu.LoadModule(km);
            Example1(gpu);
            Example2(gpu);
            CudafyTranslator.AllowClasses = previousValue;
        }

        public static void Example1(GPGPU gpu)
        {
            // Create instance and copy this to the GPU.
            // Includes a base class and another class as a member. 
            DerivedClass[] array = new DerivedClass[2];
            DerivedClass instance = new DerivedClass();
            instance.PopulateMembers();
            instance.MemberClassInstance = new MemberClass();
            instance.MemberClassInstance.Field = 5;
            instance.MemberClassInstance.MemberClassArray = new float[] { 3, 4, 5 };
            DerivedClass instance2 = new DerivedClass();
            instance2.PopulateMembers();
            instance2.MemberClassInstance = new MemberClass();
            instance2.MemberClassInstance.Field = 6;
            instance2.MemberClassInstance.MemberClassArray = new float[] { 6, 7, 8 };
            array[0] = instance;
            array[1] = instance2;
            DeviceClassHelper.CreateDeviceObject(gpu, array);
            // could also use 
            // var dev_array = DeviceClassHelper.CreateDeviceObject(gpu, array);
            // Note that this also copies reference type members to the GPU.

            // Create a result array and copy to the GPU:
            float[] result = new float[5];
            var dev_result = DeviceClassHelper.CreateDeviceObject(gpu, result);

            var dev_array = DeviceClassHelper.TryGetDeviceObjectFromHostObject(gpu, array);

            gpu.Launch(1, 1).Test(dev_array, dev_result);

            DeviceClassHelper.UpdateFromDevice(gpu, result);

            bool pass = (result[0] == 18.9f && result[1] == 8.8f && result[2] == 10f && result[3] == 36f);
            Console.WriteLine(pass ? "Pass" : "Fail");
        }

        public static void Example2(GPGPU gpu)
        {
            ArrayView view1 = new ArrayView();
            ArrayView view2 = new ArrayView();
            float[] data = Enumerable.Range(0, 1000).Select(t => (float)t).ToArray();
            // Two views of the array, simply applying an offset to the array; could slice instead for example.
            view1.CreateView(data, 100);
            view2.CreateView(data, 200);

            for (int i = 0; i < 1000; ++i) data[i] = data[i] * 10f;
            // Should copy the 'large' array to the device only once; this is referenced by each ArrayView instance.
            var dev_view1 = DeviceClassHelper.CreateDeviceObject(gpu, view1); 
            var dev_view2 = DeviceClassHelper.CreateDeviceObject(gpu, view2);

            var dev_result = gpu.Allocate<float>(5);
            var hostResult = new float[5];

            gpu.Launch(1, 1).Test2(dev_view1, dev_view2, dev_result);
            gpu.CopyFromDevice(dev_result, hostResult);

            bool pass = (hostResult[0] == 1050f && hostResult[1] == 7f);
            Console.WriteLine(pass ? "Pass" : "Fail");
        }

        [Cudafy]
        public static void Test(GThread thread, DerivedClass[] instance, float[] result)
        {
            result[0] = instance[1].Sum();
            result[1] = instance[1].SumDimension0();
            result[2] = instance[1].SumBase();
            result[3] = instance[1].MemberClassInstance.Test();
        }

        [Cudafy]
        public static void Test2(GThread thread, ArrayView arrayView1, ArrayView arrayView2, float[] result)
        {
            result[0] = arrayView1.GetElement(5);
            arrayView1.SetElement(106, 7f); // set element to 7 to verify that underlying array is same
            result[1] = arrayView2.GetElement(6); // check this is 7
        }
    }
}

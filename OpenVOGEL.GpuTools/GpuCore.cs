using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using System;
using System.Collections.Generic;

namespace OpenVOGEL.GpuTools
{
    public static class GpuCore
    {

        public static bool TestGpuDoublePrecision(int DeviceId)
        {
            if (DeviceId > CudafyHost.GetDeviceCount (eGPUType.OpenCL))
            {
                return false;
            }

            try
            {
                CudafyModes.Target = eGPUType.OpenCL;
                CudafyTranslator.Language = eLanguage.OpenCL;
                CudafyModule km = CudafyTranslator.Cudafy();
                GPGPU gpu = CudafyHost.GetDevice(eGPUType.OpenCL, DeviceId);
                gpu.LoadModule(km);

                double c;
                double[] dev_c = gpu.Allocate<double>(); 
                gpu.Launch().add_double(2.5d, 7.5d, dev_c);
                gpu.CopyFromDevice(dev_c, out c);                
                gpu.Free(dev_c);
                return c == 10.0d;
            }
            catch
            { return false; }
        }

        [Cudafy]
        public static void add_double(double a, double b, double[] c)
        {
            c[0] = a + b;
        }

    }
}

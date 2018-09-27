using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using System;

namespace OpenVOGEL.GpuTools
{
    public static class GpuCore
    {

        public static bool TestGpuDoublePresition()
        {
            try
            {
                CudafyModes.Target = eGPUType.OpenCL;
                CudafyTranslator.Language = eLanguage.OpenCL;
                CudafyModule km = CudafyTranslator.Cudafy();
                GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, 2);
                gpu.LoadModule(km);

                double c;
                double[] dev_c = gpu.Allocate<double>(); // cudaMalloc one Int32
                gpu.Launch().add_double(2.5d, 7.5d, dev_c); // or gpu.Launch(1, 1, "add", 2, 7, dev_c);
                gpu.CopyFromDevice(dev_c, out c);

                return c == 10.0d;
                
                gpu.Free(dev_c);
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

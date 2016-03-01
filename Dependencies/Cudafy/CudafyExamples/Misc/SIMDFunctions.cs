using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using Cudafy.SIMDFunctions;
namespace CudafyExamples.Misc
{
    public class SIMDFunctions
    {
        private static GPGPU _gpu;

        public static void Execute()
        {
            _gpu = CudafyHost.GetDevice(eGPUType.Cuda);

            CudafyModule km = CudafyTranslator.Cudafy(ePlatform.Auto, _gpu.GetArchitecture(), typeof(SIMDFunctions));
            //CudafyModule km = CudafyTranslator.Cudafy(ePlatform.Auto, eArchitecture.sm_12, typeof(SIMDFunctions));
            _gpu.LoadModule(km);
            int w = 1024;
            int h = 1024;

            for (int loop = 0; loop < 3; loop++)
            {
                uint[] a = new uint[w * h];
                Fill(a);
                uint[] dev_a = _gpu.CopyToDevice(a);
                uint[] b = new uint[w * h];
                Fill(b);
                uint[] dev_b = _gpu.CopyToDevice(b);
                uint[] c = new uint[w * h];
                uint[] dev_c = _gpu.Allocate(c);
                _gpu.StartTimer();
                _gpu.Launch(h, w, "SIMDFunctionTest", dev_a, dev_b, dev_c);
                _gpu.CopyFromDevice(dev_c, c);
                float time = _gpu.StopTimer();
                Console.WriteLine("Time: {0}", time);
                if (loop == 0)
                {
                    bool passed = true;
                    GThread thread = new GThread(1, 1, null);
                    for (int i = 0; i < w * h; i++)
                    {
                        uint exp = thread.vadd2(a[i], b[i]);
                        if (exp != c[i])
                            passed = false;
                    }                    
                    Console.WriteLine("Test {0}", passed ? "passed. " : "failed!");
                }
                _gpu.FreeAll();
            }
        }

        [Cudafy]
        private static void SIMDFunctionTest(GThread thread, uint[] a, uint[] b, uint[] c)
        {
            int i = thread.get_global_id(0);
            c[i] = thread.vadd2(a[i], b[i]);
        }

        private static void Fill(uint[] buffer)
        {
            Random r = new Random();
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (uint)r.Next();
            }
        }
    }
}

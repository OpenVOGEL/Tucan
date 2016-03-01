using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
namespace CudafyExamples.Misc
{
    public class TextInsertion
    {
        private static GPGPU _gpu;

        public static void Execute()
        {           
            _gpu = CudafyHost.GetDevice(eGPUType.Cuda);

            CudafyModule km = CudafyTranslator.Cudafy(ePlatform.Auto, _gpu.GetArchitecture(), typeof(TextInsertion));
            Console.WriteLine(km.CompilerOutput);
            _gpu.LoadModule(km);

            int[] data = new int[64];
            int[] data_d = _gpu.CopyToDevice(data);
            int[] res_d = _gpu.Allocate(data);
            int[] res = new int[64];
            _gpu.Launch(1, 1, "AHybridMethod", data_d, res_d);
            _gpu.CopyFromDevice(data_d, res);
            for(int i = 0; i < 64; i++)
                if (data[i] != res[i])
                {
                    Console.WriteLine("Failed");
                    break;
                }
        }

        [Cudafy]
        private static void AHybridMethod(Cudafy.GThread thread, int[] data, int[] results)
        {
            GThread.InsertCode("#pragma unroll 5");
            for (int h = 0; h < data.Length; h++)
                GThread.InsertCode("{0}[{2}] = {1}[{2}];", results, data, h);
        }
    }
}

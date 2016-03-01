using System;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyExamples.Generics
{
    [CudafyDummy]
    public struct DummyGeneric<T, U> where T : struct
    {
        public T A;
        public U B;
    }

    public class SimpleGeneric
    {
        [Cudafy]
        public struct Generic<T, U> where T : struct
        {
            public T A;
            public U B;
        }

        [Cudafy]
        public static void Kernel(GThread thread, Generic<ushort, ushort> input, int[] output)
        {
            var tid = thread.threadIdx.x;
            
            output[0] = input.A == 187 ? 1:0;
        }

        public static void Execute()
        {
            CudafyModule km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy(typeof(Generic<ushort, ushort>), typeof(SimpleGeneric));
                km.Serialize();
            }

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            gpu.LoadModule(km);

            var input = new Generic<ushort, ushort>();

            input.A = 187;

            int[] devoutput = gpu.Allocate<int>(1);
            gpu.Launch(1, 1, "Kernel", input, devoutput);

            int output;

            gpu.CopyFromDevice(devoutput, out output);

            Console.WriteLine("Simple Generic: " + ((output==1) ? "PASSED" : "FAILED"));
        }
    }
}

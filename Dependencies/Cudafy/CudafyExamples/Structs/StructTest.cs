using System;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyExamples.Structs
{
    public class StructTest
    {
        [Cudafy]
        public static void StructTestKernel(GThread thread, ValueA input, int[] output)
        {
            int value = input.valueB.value;
            output[0] = value;
        }

        public static void Execute()
        {
            var km = CudafyModule.TryDeserialize();
            if (km == null || !km.TryVerifyChecksums())
            {
                km = CudafyTranslator.Cudafy(ePlatform.Auto, eArchitecture.sm_20,
                    typeof(ValueB),
                    typeof(ValueA),
                    typeof(StructTest));

                km.Serialize();
            }
        
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target,0);
            gpu.LoadModule(km);

            var value = new ValueA();
            value.valueB = new ValueB();
            value.valueB.value = 56;

            var devOutput = gpu.Allocate<int>(1);

            gpu.Launch(1, 1, "StructTestKernel", value, devOutput);

            int output;

            // copy the array 'c' back from the GPU to the CPU
            gpu.CopyFromDevice(devOutput, out output);

            gpu.Free(devOutput);

            Console.WriteLine("Expected: {0} \t{1}", 56, 56 == output ? "PASSED" : "FAILED");
            
        }
    }
}

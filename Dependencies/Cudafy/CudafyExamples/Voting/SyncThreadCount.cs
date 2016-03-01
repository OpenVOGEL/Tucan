using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyExamples.Voting
{
    public class SyncThreadCount
    {
        [Cudafy]
        public static void SyncThreadCountKernel(GThread thread, int[] input, int[] output)
        {
            var tid = thread.threadIdx.x;

            int value = input[tid];
            bool predicate = value == 1;
            var count = thread.SyncThreadsCount(predicate);

            if (tid == 0)
                output[0] = count;
        }

        public static void Execute()
        {
            CudafyModule km = CudafyTranslator.Cudafy(eArchitecture.sm_20);
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target,0);
            gpu.LoadModule(km);

            const int count = 128;
            var random = new Random();
            var input = new int[count];
            int output = 0;
            int expectedOutput = 0;

            for (var i = 0; i < count; i++)
                input[i] = random.Next(16);

            for (var i = 0; i < count; i++)
                expectedOutput += (input[i]==1) ? 1 : 0;

            var devInput = gpu.Allocate<int>(count);
            var devOutput = gpu.Allocate<int>(1);

            gpu.CopyToDevice(input, devInput);

            gpu.Launch(1, count, "SyncThreadCountKernel", devInput, devOutput);

            // copy the array 'c' back from the GPU to the CPU
            gpu.CopyFromDevice(devOutput, out output);

            gpu.Free(devInput);
            gpu.Free(devOutput);

            
            Console.WriteLine("SyncThreadCount: {0}", output);
            Console.WriteLine("Expected: {0} \t{1}", expectedOutput, expectedOutput == output ? "PASSED" : "FAILED");
            
        }
    }
}

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
    public class Ballot
    {
        [Cudafy]
        public static void BallotKernel(GThread thread, int[] input, int[] output)
        {
            var tid = thread.threadIdx.x;
            var wid = thread.threadIdx.x / 32;
            var twid = thread.threadIdx.x % 32;

            int value = input[tid];
            bool predicate = value == 1;
            var ballot = thread.Ballot(predicate);

            if (twid == 0)
                output[wid] = ballot;
        }

        public static void Execute()
        {
            CudafyModule km = CudafyTranslator.Cudafy(eArchitecture.sm_20);
            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target,0);
            gpu.LoadModule(km);

            const int warps = 4;
            const int count = warps*32;
            var random = new Random();
            var input = new int[count];
            var output = new int[count/32];
            var expectedOutput = new int[count/32];

            for (var i = 0; i < warps; i++)
                expectedOutput[i] = 0;

            for (var i = 0; i < count; i++)
                input[i] = random.Next(2);

            for (var i = 0; i < count; i++)
                expectedOutput[i / 32] += input[i] << (i % 32);


            var devInput = gpu.Allocate<int>(count);
            var devOutput = gpu.Allocate<int>(warps);

            gpu.CopyToDevice(input, devInput);

            gpu.Launch(1, count, "BallotKernel", devInput, devOutput);

            // copy the array 'c' back from the GPU to the CPU
            gpu.CopyFromDevice(devOutput, output);

            gpu.Free(devInput);
            gpu.Free(devOutput);

            for (var i = 0; i < warps; i++)
            {
                Console.WriteLine("Warp {0} Ballot: {1}", i, output[i]);
                Console.WriteLine("Expected: {0} \t{1}", expectedOutput[i], expectedOutput[i] == output[i] ? "PASSED" : "FAILED");
            }
        }
    }
}

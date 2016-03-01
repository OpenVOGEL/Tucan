using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
namespace CudafyExamples.Misc
{


    public class TypeTest
    {
        // Count permutations
        static TypeTest()
        {
            ThreadsPerBlock = 256;
            BlocksPerGrid = 256;
            SetPermutations();
        }

        protected const int _cities = 12;	// Set this to control sample size
        protected static long _permutations = 1;
        public static int ThreadsPerBlock { get; set; }
        public static int BlocksPerGrid { get; set; }

        private static void SetPermutations()
        {
            _permutations = 1L;
            for (int i = 2; i <= _cities; i++) { _permutations *= i; }
        }

        public static void Execute()
        {
            Console.WriteLine("Compiling ...");
            RunTest(GetThreadInfo(), GetAnswer());
            ThreadsPerBlock /= 2;
            RunTest(GetThreadInfo(), GetAnswer());
            ThreadsPerBlock /= 2;
            RunTest(GetThreadInfo(), GetAnswer());
            BlocksPerGrid /= 2;
            RunTest(GetThreadInfo(), GetAnswer());

            Console.WriteLine("Done ... Press Enter to shutdown.");
            try { Console.Read(); }
            catch (InvalidOperationException) { ; }
            CudafyHost.GetDevice().FreeAll();
            CudafyHost.GetDevice().HostFreeAll();
        }

        private static string GetThreadInfo()
        {
            var target = string.Format(" ( {0,3} threads_per * {1,3} blocks input: ",
                ThreadsPerBlock, BlocksPerGrid);
            return target;
        }

        static void RunTest(string threadInfo, AnswerStruct answer)
        {
            Console.WriteLine(
                string.Format("{0} {1,3} threads * {2,3} blocks returned.",
                    threadInfo, answer.pathNo, answer.distance));
        }
        [Cudafy]
        public struct AnswerStruct
        {
            public float distance;
            public long pathNo;
        }
        internal static AnswerStruct GetAnswer()
        {
            using (var gpu = CudafyHost.GetDevice())
            {
                gpu.LoadModule(CudafyTranslator.Cudafy());

                var answer = new AnswerStruct[BlocksPerGrid]; ;
                var gpuAnswer = gpu.Allocate(answer);

                gpu.Launch(BlocksPerGrid, ThreadsPerBlock,
                   GpuFindPathDistance, gpuAnswer);

                gpu.Synchronize();
                gpu.CopyFromDevice(gpuAnswer, answer);
                gpu.FreeAll();

                var bestDistance = float.MaxValue;
                var bestPermutation = 0L;
                for (var i = 0; i < BlocksPerGrid; i++)
                {
                    if (answer[i].distance < bestDistance)
                    {
                        bestDistance = answer[i].distance;
                        bestPermutation = answer[i].pathNo;
                    }
                }

                return new AnswerStruct
                {
                    distance = bestDistance,
                    pathNo = bestPermutation
                };
            }
        }

        [Cudafy]
        public static void GpuFindPathDistance(GThread thread, AnswerStruct[] answer)
        {
            var answerLocal = thread.AllocateShared<AnswerStruct>("ansL", ThreadsPerBlock);

            var bestDistance = thread.gridDim.x;
            var bestPermutation = thread.blockDim.x;

            var sum = 0;
            for (int i = 0; i < thread.blockDim.x; i++) sum += i * thread.threadIdx.x;

            answerLocal[thread.threadIdx.x].distance = bestDistance;
            answerLocal[thread.threadIdx.x].pathNo = bestPermutation;
            thread.SyncThreads();

            if (thread.threadIdx.x == 0)
            {
                answer[thread.blockIdx.x] = answerLocal[0];
            }
        }
    }

}

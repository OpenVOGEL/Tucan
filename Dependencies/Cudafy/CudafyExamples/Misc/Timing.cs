using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
namespace CudafyExamples.Misc
{
    public class Timing
    {
        [Cudafy]
        public static void calc_e(GThread thread, int n, int[] dx, int[] dy, int[] e)
        {
            for (int i = 0; i < n; i++)
            {
                e[i] = 2 * dy[i] - dx[i];
            }
        }

        [Cudafy]
        public static void calc_e_v2(GThread thread, int n, int[] dx, int[] dy, int[] e)
        {
            int i = thread.blockDim.x * thread.blockIdx.x + thread.threadIdx.x;
            while(i < n)
            {
                e[i] = 2 * dy[i] - dx[i];
                i += (thread.blockDim.x * thread.gridDim.x);
            }
        }

        public static void Execute()
        {
            int n = 2000000;
            
            Random r = new Random();

            int[] dx = new int[n];
            int[] dy = new int[n];
            int[] e = new int[n]; int[] eh = new int[n];

            // fills massives by random
            for (int i = 0; i < n; i++)
            {
                dx[i] = r.Next();
                dy[i] = r.Next();
            }

            double t2 = MeasureTime(() =>
            {
                for (int i = 0; i < n; i++)
                {
                    eh[i] = 2 * dy[i] - dx[i];
                }
            });

            CudafyModule km = CudafyTranslator.Cudafy(eArchitecture.sm_20);

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target, 0);
            gpu.LoadModule(km);            

            int[] dev_dx = gpu.Allocate<int>(dx);
            int[] dev_dy = gpu.Allocate<int>(dy);
            int[] dev_e = gpu.Allocate<int>(e);
            double t3 = 0;
            gpu.CopyToDevice(dx, dev_dx);
            gpu.CopyToDevice(dy, dev_dy);
            for (int x = 0; x < 2; x++)
            {
                t3 = MeasureTime(() =>
                {
                    //gpu.Launch(1, 1, "calc_e", n, dev_dx, dev_dy, dev_e);
                    //gpu.CopyToDevice(dx, dev_dx);
                    //gpu.CopyToDevice(dy, dev_dy);
                    gpu.Launch(n / 512, 512, "calc_e_v2", n, dev_dx, dev_dy, dev_e);
                    gpu.Synchronize();
                    //gpu.CopyFromDevice(dev_e, e);
                });
            }

            double t4 = MeasureTime(() =>
            {
                gpu.CopyFromDevice(dev_e, e);
            });

            for (int i = 0; i < n; i++)
                Debug.Assert(e[i]== eh[i]);
            Console.WriteLine(string.Format("n = {0}", n));
            Console.WriteLine(string.Format("CPU ::: e = 2 * dy - dx ::: Excecution time: {0} ms", t2 * 1000));
            Console.WriteLine(string.Format("CUDA ::: e = 2 * dy - dx ::: Excecution time: {0} ms", t3 * 1000));
            //Console.WriteLine(string.Format("CUDA copy to host {0} ms", t4 * 1000));
            //Console.ReadKey();
        }

        static double MeasureTime(Action action)
        {
            Stopwatch watch = new Stopwatch();
            
            watch.Start();
            action.Invoke();
            watch.Stop();

            return watch.ElapsedTicks / (double)Stopwatch.Frequency;
        }
}
}

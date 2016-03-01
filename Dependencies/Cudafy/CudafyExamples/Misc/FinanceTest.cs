
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;

namespace CudafyExamples.Misc
{
    public class FinanceTest
    {
        private static GPGPU _gpu;
        
        public static void Execute()
        {
            CudafyModule km = CudafyTranslator.Cudafy(typeof(ParamsStruct), typeof(ImpliedVolatile));

            _gpu = CudafyHost.GetDevice(CudafyModes.Target);
            _gpu.LoadModule(km);

            ParamsStruct[] host_par = new ParamsStruct[1];
            ParamsStruct[] result = new ParamsStruct[1];
            host_par[0].OP = 96.95;
            host_par[0].Price = 1332.24;
            host_par[0].Strike = 1235;
            host_par[0].TD = 31;
            host_par[0].R = 0.0001355;
            host_par[0].Q = 0.0166;
            host_par[0].N = 100;// 1000;
            host_par[0].kind = 1;

            ParamsStruct[] dev_par = _gpu.CopyToDevice(host_par);
            float[] PA = _gpu.Allocate<float>(1001);
            _gpu.Launch(1,1, "impliedVolatile", dev_par, PA);

            _gpu.CopyFromDevice(dev_par, 0, result, 0, 1);

            Console.WriteLine("I={0}, B={1}", result[0].i, result[0].B);           
            //Console.ReadKey();
        }

    }

    [Cudafy]
    public struct ParamsStruct
    {
        //public double[] PA;
        public double OP;
        public double Price;
        public double Strike;
        public double TD;
        public double R;
        public double Q;
        public int N;
        public int i;
        public double B;
        public int kind;
    }        


    public class ImpliedVolatile
    {
        [Cudafy]
        public static double FCRR(float[] PA, int N, double S, double X, double sigma, double R, double Q, double delta, int kind)
        {
            double Up = Math.Exp(sigma * Math.Sqrt(delta));
            double p0 = (Math.Exp((R - Q) * delta) - 1 / Up) / (Up - 1 / Up);
            if (p0 > 1 || p0 < 0)
                return -1;
            double p1 = 1 - p0;

            int bm = 1;
            if (kind == 0 || kind == 3)
                bm = -1;

            for (int i = 0; i <= N; ++i)
            {
                PA[i] = (float)((S * Math.Pow(Up, N - 2 * i) - X) * bm);
                if (PA[i] < 0)
                    PA[i] = 0;
            }
            
            double mexp = Math.Exp(-R * delta);
            double exercize = 0;
            for (int k = N - 1; k >= 0; k--)
            {
                for (int i = 0; i <= k; ++i)
                {
                    PA[i] = (float)((p0 * PA[i] + p1 * PA[i + 1]) * mexp);
                    if (kind == 3 || kind == 2)
                    {
                        exercize = (S * Math.Pow(Up, k - 2 * i) - X) * bm;
                        if (PA[i] < exercize)
                            PA[i] = (float)exercize;
                    }
                }
            }
            //return p0;
            return PA[0];
        }

        [Cudafy]
        public static void impliedVolatile(ParamsStruct[] par, float[] PA)
        {
            double Ast;
            double S;
            double A = 0.001;
            par[0].B = 3;
            double C = A;
            double D = 0;
            double FF = par[0].OP;
            double delta = par[0].TD / 365 / par[0].N;
            double FA = FCRR(PA, par[0].N, par[0].Price, par[0].Strike, A, par[0].R, par[0].Q, delta, par[0].kind) - FF;


            while (FA == -1 - FF)
            {
                A += 0.001;
                FA = FCRR(PA, par[0].N, par[0].Price, par[0].Strike, A, par[0].R, par[0].Q, delta, par[0].kind) - FF;
            }
            double FB = FCRR(PA, par[0].N, par[0].Price, par[0].Strike, par[0].B, par[0].R, par[0].Q, delta, par[0].kind) - FF;
            while (FB == -1 - FF)
            {
                par[0].B -= 0.001;
                FB = FCRR(PA, par[0].N, par[0].Price, par[0].Strike, par[0].B, par[0].R, par[0].Q, delta, par[0].kind) - FF;
            }
            //
            double FC = FA;
            double FS = 1;
            bool MFlag = true;

            par[0].i = 0;

            while ((Math.Abs(FB) > 0.01) || (Math.Abs(FS) > 0.01) || (Math.Abs(par[0].B - C) > 0.001))
            {

                if ((FA != FC) && (FB != FC))
                    S = A * FB * FC / (FA - FB) / (FA - FC) + par[0].B * FA * FC / (FB - FA) / (FB - FC) + C * FA * FB / (FC - FA) * (FC - FB);
                else
                    S = par[0].B - FB * (par[0].B - A) / (FB - FA);


                if (
                    !(((3 * A + par[0].B) / 4 <= C) && (C <= par[0].B)) ||
                    ((Math.Abs(S - par[0].B) >= Math.Abs(par[0].B - C) / 2) && MFlag) ||
                    ((Math.Abs(S - par[0].B) >= Math.Abs(C - D) / 2) && MFlag) ||
                    ((Math.Abs(par[0].B - C) <= 0.001) && MFlag) ||
                    ((Math.Abs(C - D) <= 0.001) && MFlag)
                    )
                {
                    S = (A + par[0].B) / 2;
                    MFlag = true;
                }
                else
                    MFlag = false;


                FS = FCRR(PA, par[0].N, par[0].Price, par[0].Strike, S, par[0].R, par[0].Q, delta, par[0].kind) - FF;
                D = C;
                C = par[0].B;
                FC = FB;
                if ((FA * FS) < 0)
                {
                    par[0].B = S;
                    FB = FS;
                }
                else
                {
                    A = S;
                    FA = FS;
                }
                if (Math.Abs(FA) <= Math.Abs(FB))
                {
                    Ast = par[0].B;
                    par[0].B = A;
                    A = Ast;
                    Ast = FB;
                    FB = FA;
                    FA = Ast;
                }
                par[0].i++;
                if (par[0].i > 30)
                {
                    par[0].B = 0;
                    break;
                }
            }
        }
    }
}

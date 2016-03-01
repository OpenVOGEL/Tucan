/* 
 * This software is based upon the book CUDA By Example by Sanders and Kandrot
 * and source code provided by NVIDIA Corporation.
 * It is a good idea to read the book while studying the examples!
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cudafy;
namespace CudafyByExample
{
    public class add_loop_cpu
    {
        public const int N = 10;

        public static void Execute() 
        {
            int[] a = new int[N];
            int[] b = new int[N];
            int[] c = new int[N];

            // fill the arrays 'a' and 'b' on the CPU
            for (int i=0; i<N; i++) 
            {
                a[i] = -i;
                b[i] = i * i;
            }

            add( a, b, c );

            // display the results
            for (int i=0; i<N; i++) {
                Console.WriteLine("{0} + {1} = {2}", a[i], b[i], c[i] );
            }
        }

        public static void add(int[] a, int[] b, int[] c) 
        {
            int tid = 0;    // this is CPU zero, so we start at zero
            while (tid < N) {
                c[tid] = a[tid] + b[tid];
                tid += 1;   // we have one CPU, so we increment by one
            }
        }
    }

}

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
using Cudafy.Host;

namespace CudafyByExample
{
    public struct cuComplex
    {
        float r;
        float i;
        public cuComplex(float a, float b)
        {
            r = a;
            i = b;
        }
        public float magnitude2()
        {
            return r * r + i * i;
        }

        public static cuComplex operator *(cuComplex a, cuComplex b)
        {
            return new cuComplex(a.r * b.r - a.i * b.i, a.i * b.r + a.r * b.i);
        }

        public static cuComplex operator +(cuComplex a, cuComplex b)
        {
            return new cuComplex(b.r + a.r, b.i + a.i);
        }
    };
}

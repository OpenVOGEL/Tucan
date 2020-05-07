
using DotNumerics.LinearAlgebra.CSLapack;
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace DotNumerics.LinearAlgebra
{

   #region MKL wrappers

    // Imported LAPACK declarations from Intel Math Kernel Library
    [SuppressUnmanagedCodeSecurity]
    internal sealed class IntelMathKernel
    {
        private IntelMathKernel() { }

        /// <summary>
        /// Computes the LU decomposition of the given matrix a
        /// Original C header:
        /// void dgetrf (const MKL_INT* m, const MKL_INT* n, double* a, const MKL_INT* lda,
        ///              MKL_INT* ipiv, MKL_INT* info);
        /// </summary>
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern void dgetrf(ref int n, ref int m, [In, Out] double[] a, ref int lda, 
                                           [Out] int[] ipiv, ref int info);

        /// <summary>
        /// Computes the solution vector for the given LU matrix
        /// Original C header:
        /// void dgetrs (const char* trans, const MKL_INT* n, const MKL_INT* nrhs,
        ///              const double* a, const MKL_INT* lda, const MKL_INT* ipiv,
        ///              double* b, const MKL_INT* ldb, MKL_INT* info);
        /// </summary>
        [DllImport("mkl_rt.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, SetLastError = false)]
        internal static extern void dgetrs([In] char [] trans, ref int n, ref int nrhs,
                                           [In] double[] a, ref int lda, [In] int[] ipiv, 
                                           [In, Out] double[] b, ref int ldb, ref int info);

    }

    #endregion

    #region MKL tester

    public static class IntelMathKernelTest
    {
        public static bool Start()
        {
            Console.WriteLine("Testing LU decomposition");

            int n = 3;
            int info = 0;
            int[] ipiv = new int[3];
            
            Matrix Mtx = new Matrix(3);
            Mtx[0, 0] = 1;
            Mtx[0, 1] = 9;
            Mtx[0, 2] = 5;
            Mtx[1, 0] = 1;
            Mtx[1, 1] = 3;
            Mtx[1, 2] = 4;
            Mtx[2, 0] = 5;
            Mtx[2, 1] = 6;
            Mtx[2, 2] = 7;

            try
            {

                // Case 1
                Console.WriteLine("Computing LU with MKL dgetrf...");
                double[] A = (double[])Mtx.Data.Clone();
                IntelMathKernel.dgetrf(ref n, ref n, A, ref n, ipiv, ref info);

                // Case 2
                Console.WriteLine("Computing LU with with .NET dgetrf...");
                double[] B = (double[])Mtx.Data.Clone();
                DGETRF _dgetrf = new DGETRF();
                _dgetrf.Run(n, n, ref B, 0, n, ref ipiv, 0, ref info);

                Console.WriteLine("Comparing results...");
                for (int i = 0; i < 5; i++)
                {
                    if (A[i] != B[i] || (B[i] > 0.0 & (A[i] - B[i]) / B[i] > 1E-5))
                    {
                        Console.WriteLine("TEST NOT PASSED");
                        return false;
                    }
                }

                // Case 3
                Console.WriteLine("Solving linear equations with MKL dgetrs...");
                info = 0;
                int nrhs = 1;
                double[] R = new double[3];
                R[0] = 1;
                R[1] = 2;
                R[2] = 5;
                char[] trans = "No transponse".ToCharArray();
                IntelMathKernel.dgetrs(trans, ref n, ref nrhs, A, ref n, ipiv, R, ref n, ref info);

                Console.WriteLine("TEST PASSED");
                return true;

            }
            catch
            {
                Console.WriteLine("TEST NOT PASSED");
                return false;
            }

        }

    }

    #endregion
}

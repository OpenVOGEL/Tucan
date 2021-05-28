#region Copyright © 2009, De Santiago-Castillo JA. All rights reserved.

//Copyright © 2009 Jose Antonio De Santiago-Castillo 
//E-mail:JAntonioDeSantiago@gmail.com
//Web: www.DotNumerics.com
//
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using DotNumerics.LinearAlgebra.CSLapack;
using DotNumerics.LinearAlgebra.CSEispack;
using DotNumerics.FortranLibrary;

namespace DotNumerics.LinearAlgebra
{
    /// <summary>
    /// Computes the eigenvalues and the eigenvectors of a square matrix.
    /// </summary>
    public sealed class EigenSystem
    {

        #region Fields

        DGEEV _dgeev;
        DSYEV _dsyev;
        DSBEV _dsbev;
        CG _cg;

        #endregion


        /// <summary>
        /// Initializes a new instance of the EigenSystem class.
        /// </summary>
        public EigenSystem()
        {

        }

        #region General matrix 


        /// <summary>
        /// Computes the eigenvalues for an N-by-N real nonsymmetric matrix A.
        /// </summary>
        /// <param name="A">N-by-N real nonsymmetric matrix A.</param>
        /// <returns>The eigenvalues.</returns>
        public ComplexMatrix GetEigenvalues(Matrix A)
        {

            if (this._dgeev == null) this._dgeev = new DGEEV();

            this.CheckDimensions(A);


            Matrix ACopy = A.Clone();
            double[] ACopyData = ACopy.Data;
            Matrix RealEVectors = new Matrix(1, 1);  
            double[] EigenVectsData = RealEVectors.Data;
            ComplexMatrix EigenVals = new ComplexMatrix(A.RowCount, 1);

            double[] REigVal = new double[A.RowCount];
            double[] IEigVal = new double[A.RowCount];

            //double[] EigenValsData = EigenVals.Data;
            int Info = 0;

            double[] VL = new double[A.RowCount];

            double[] Work = new double[1];
            int LWork = -1;

            //Calculamos LWORK 
            _dgeev.Run("N", "N", A.RowCount, ref ACopyData, 0, ACopy.RowCount, ref REigVal, 0, ref IEigVal, 0, ref VL, 0, 1, ref  EigenVectsData, 0, A.RowCount, ref Work, 0, LWork, ref Info);

            LWork = Convert.ToInt32(Work[0]);
            if (LWork > 0)
            {
                Work = new double[LWork];
                _dgeev.Run("N", "N", A.RowCount, ref ACopyData, 0, ACopy.RowCount, ref REigVal, 0, ref IEigVal, 0, ref VL, 0, 1, ref  EigenVectsData, 0, A.RowCount, ref Work, 0, LWork, ref Info);
            }
            else
            {

                //Error
            }


            #region Error
            //= 0:  successful exit
            //.LT. 0:  if INFO = -i, the i-th argument had an illegal value.
            //.GT. 0:  if INFO = i, the QR algorithm failed to compute all the
            // eigenvalues, and no eigenvectors have been computed;
            // elements i+1:N of WR and WI contain eigenvalues which
            // have converged.

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("The QR algorithm failed to compute all the eigenvalues.");
            }

            #endregion


            for (int i = 0; i < EigenVals.RowCount; i++)
            {
                EigenVals[i, 0] = new Complex(REigVal[i], IEigVal[i]);
            }


            return EigenVals;

        }

        /// <summary>
        /// Computes for an N-by-N real nonsymmetric matrix A, the
        /// eigenvalues and eigenvectors.
        /// </summary>
        /// <param name="A">N-by-N real nonsymmetric matrix A.</param>
        /// <param name="EigenVectors">The eigenvectors.</param>
        /// <returns>The eigenvalues.</returns>
        public ComplexMatrix GetEigenvalues(Matrix A, out ComplexMatrix EigenVectors)
        {

            if (this._dgeev == null) this._dgeev = new DGEEV();

            this.CheckDimensions(A);


            Matrix ACopy = A.Clone();
            double[] ACopyData = ACopy.Data;
            EigenVectors = new ComplexMatrix(A.RowCount, A.ColumnCount);
            Matrix RealEVectors = new Matrix(A.RowCount, A.ColumnCount);
            double[] EigenVectsData = RealEVectors.Data;
            ComplexMatrix EigenVals = new ComplexMatrix(A.RowCount, 1);

            double[] REigVal = new double[A.RowCount];
            double[] IEigVal = new double[A.RowCount];

            //double[] EigenValsData = EigenVals.Data;
            int Info = 0;

            double[] VL = new double[A.RowCount];

            double[] Work = new double[1];
            int LWork = -1;

            //Calculamos LWORK 
            _dgeev.Run("N", "V", A.RowCount, ref ACopyData, 0, ACopy.RowCount, ref REigVal, 0, ref IEigVal, 0, ref VL, 0, 1, ref  EigenVectsData, 0, A.RowCount, ref Work, 0, LWork, ref Info);

            LWork = Convert.ToInt32(Work[0]);
            if (LWork > 0)
            {
                Work = new double[LWork];
                _dgeev.Run("N", "V", A.RowCount, ref ACopyData, 0, ACopy.RowCount, ref REigVal, 0, ref IEigVal, 0, ref VL, 0, 1, ref  EigenVectsData, 0, A.RowCount, ref Work, 0, LWork, ref Info);
            }
            else
            {

                //Error
            }


            #region Error
            //= 0:  successful exit
            //.LT. 0:  if INFO = -i, the i-th argument had an illegal value.
            //.GT. 0:  if INFO = i, the QR algorithm failed to compute all the
            // eigenvalues, and no eigenvectors have been computed;
            // elements i+1:N of WR and WI contain eigenvalues which
            // have converged.

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("The QR algorithm failed to compute all the eigenvalues.");
            }

            #endregion


            for (int i = 0; i < EigenVals.RowCount; i++)
            {
                EigenVals[i, 0] = new Complex(REigVal[i], IEigVal[i]);
            }

            for (int i = 0; i < EigenVals.RowCount; i++)
            {
                if (EigenVals[i, 0].Imaginary == 0.0)
                {
                    for (int j = 0; j < EigenVectors.RowCount; j++)
                    {
                        EigenVectors[j, i] = new Complex(RealEVectors[j, i], 0.0);
                    }
                }
                else
                {
                    if (EigenVals[i, 0].Imaginary > 0.0)
                    {
                        for (int j = 0; j < EigenVectors.RowCount; j++)
                        {
                            EigenVectors[j, i] = new Complex(RealEVectors[j, i], RealEVectors[j, i + 1]);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < EigenVectors.RowCount; j++)
                        {
                            EigenVectors[j, i] = new Complex(RealEVectors[j, i - 1], -RealEVectors[j, i]);

                        }
                    }
                }
            }

            return EigenVals;
        }

        #endregion


        #region SymmetricMatrix 

        /// <summary>
        /// Computes all eigenvalues of a real symmetric matrix A.
        /// </summary>
        /// <param name="A">The real symmetric matrix A.</param>
        /// <returns>The eigenvalues.</returns>
        public Matrix GetEigenvalues(SymmetricMatrix A)
        {
            if (this._dsyev == null) this._dsyev = new DSYEV();


            this.CheckDimensions(A);

            Matrix EigenVects = new Matrix(A.RowCount, A.ColumnCount, A.Data);
            double[] EigenVectsData = EigenVects.Data;
            Matrix EigenVals = new Matrix(A.RowCount, 1);
            double[] EigenValsData = EigenVals.Data;
            int Info = 0;

            double[] Work = new double[1];

            int LWork = -1;
            //Calculamos LWORK ideal

            _dsyev.Run("N", "U", A.RowCount, ref EigenVectsData, 0, A.RowCount, ref EigenValsData, 0, ref Work, 0, LWork, ref Info);

            LWork = Convert.ToInt32(Work[0]);
            if (LWork > 0)
            {
                Work = new double[LWork];
                _dsyev.Run("N", "U", A.RowCount, ref EigenVectsData, 0, A.RowCount, ref EigenValsData, 0, ref Work, 0, LWork, ref Info);
            }
            else
            {

                //Error
            }

            #region Error
            /// = 0:  successful exit
            /// .LT. 0:  if INFO = -i, the i-th argument had an illegal value
            /// .GT. 0:  if INFO = i, the algorithm failed to converge; i
            /// off-diagonal elements of an intermediate tridiagonal
            /// form did not converge to zero.

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("The algorithm failed to converge.");
            }

            #endregion


            return EigenVals;
        }

        /// <summary>
        /// Computes all eigenvalues and eigenvectors of a of a real symmetric matrix A.
        /// </summary>
        /// <param name="A">The real symmetric matrix A.</param>
        /// <param name="EigenVects">The eigenvectors.</param>
        /// <returns>The eigenvalues.</returns>
        public Matrix GetEigenvalues(SymmetricMatrix A, out Matrix EigenVects)
        {

            if (this._dsyev == null) this._dsyev = new DSYEV();
            this.CheckDimensions(A);


            EigenVects = new Matrix(A.RowCount, A.ColumnCount, A.Data);
            double[] EigenVectsData = EigenVects.Data;
            Matrix EigenVals = new Matrix(A.RowCount, 1);
            double[] EigenValsData = EigenVals.Data;
            int Info = 0;

            double[] Work = new double[1];

            int LWork = -1;
            //Calculamos LWORK ideal

            _dsyev.Run("V", "U", A.RowCount, ref EigenVectsData, 0, A.RowCount, ref EigenValsData, 0, ref Work, 0, LWork, ref Info);

            LWork = Convert.ToInt32(Work[0]);
            if (LWork > 0)
            {
                Work = new double[LWork];
                _dsyev.Run("V", "U", A.RowCount, ref EigenVectsData, 0, A.RowCount, ref EigenValsData, 0, ref Work, 0, LWork, ref Info);
            }
            else
            {

                //Error
            }

            #region Error
            /// = 0:  successful exit
            /// .LT. 0:  if INFO = -i, the i-th argument had an illegal value
            /// .GT. 0:  if INFO = i, the algorithm failed to converge; i
            /// off-diagonal elements of an intermediate tridiagonal
            /// form did not converge to zero.

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("The algorithm failed to converge.");
            }

            #endregion


            return EigenVals;
        }

        public double EVecError_Subspace = 1E-5;
        public double EValError_Subspace = 1E-7;
        public double EValError_Jacobi = 1E-7;

        /// <summary>
        /// Finds the eigen values and eigen vectors of a small system Kφ = λMφ. 
        /// K and M are dense symmetric matrices, K is non-singular and M is positive-definite. 
        /// </summary>
        /// <param name="K">K matrix</param>
        /// <param name="M">M matrix</param>
        /// <param name="Q">M-normalized eigen vectors (in columns)</param>
        /// <param name="D">Eigen values</param>
        /// <remarks>
        /// This method is suitable for small systems only, and will work better as matrices K and M have many off-diagonal zeros. 
        /// This is why this method can be succesfully implemented on the subspace iteration method (where projected matrices tend to diagonal form).
        /// If a lumped matrix is applied, this method will work even faster.
        /// This method has been brought here as a complement to the OpenVOGEL project publisehd on GPLv3.
        /// OpenVOGEL (openvogel.org)
        /// Open source software for aerodynamics
        /// Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)
        /// <http://www.gnu.org/licenses/>.
        /// This algorithm is a .NET interpretation of the original method published in "J. Bathe, Finite elements procedures",
        /// which was written in FORTRAN.
        /// </remarks>
        public void Jacobi(SymmetricMatrix K, SymmetricMatrix M, Matrix Q, Vector D)
        {

            int n = K.RowCount;
            
            if (M.RowCount != n | Q.RowCount != n | Q.ColumnCount != n | D.Length != n)
            {
                throw new Exception("Error on Jacobi solver: unconforming matrix sizes");
            }
                    
            Vector L = new Vector(n);
            Matrix X = new Matrix(n);

            for (int i = 0; i < n; i++) // < load identity
                X[i, i] = 1;

            double er_evec = 0.0;               // < eigen vectors convergence threshold
            double er_eval = EValError_Jacobi;  // < eigen values convergence threshold

            bool Converged = false;
            int Sweep = 0;

            SymmetricMatrix newM = M.Clone ();
            SymmetricMatrix newK = K.Clone ();

            while (!Converged && Sweep < 100)
            {

                er_evec = Math.Pow(0.01, 2 * (Sweep + 1)); // < update sweep threshold

                for (int i = 0; i < n - 1; i++)
                {

                    for (int j = i + 1; j < n; j++)
                    {

                        if ((newM[i, j] * newM[i, j]) / (newM[i, i] * newM[j, j]) < er_evec &&
                            (newK[i, j] * newK[i, j]) / (newK[i, i] * newK[j, j]) < er_evec)
                            continue; // < no zeroing required for this off-diagonal element

                        // calculate alpha and beta to zero off-diagonal elements {i, j} on K and M:

                        double kii = newK[i, i] * newM[i, j] - newM[i, i] * newK[i, j];
                        double kjj = newK[j, j] * newM[i, j] - newM[j, j] * newK[i, j];
                        double kdash = newK[i, i] * newM[j, j] - newK[j, j] * newM[i, i];

                        double check = 0.25 * kdash * kdash + kii * kjj;

                        double Den = 0.5 * kdash + System.Math.Sign(kdash) * System.Math.Sqrt(check);

                        double g = 0.0;
                        double a = 0.0;

                        if (System.Math.Abs(Den) > 1E-50)
                        {
                            g = -kii / Den;
                            a = kjj / Den;
                        }
                        else
                        {
                            g = -newK[i, j] / newK[j, j];
                            a = 0.0;
                        }

                        // Perform rotation to annihilate off-diagonal element ij:

                        for (int k = 0; k < i; k++)
                        {
                            double Kki = newK[k, i];
                            double Kkj = newK[k, j];
                            newK[k, i] = Kki + Kkj * g;
                            newK[k, j] = Kkj + Kki * a;

                            double Mki = newM[k, i];
                            double Mkj = newM[k, j];
                            newM[k, i] = Mki + Mkj * g;
                            newM[k, j] = Mkj + Mki * a;
                        }

                        for (int k = j + 1; k < n; k++)
                        {
                            double Kik = newK[i, k];
                            double Kjk = newK[j, k];
                            newK[i, k] = Kik + Kjk * g;
                            newK[j, k] = Kjk + Kik * a;

                            double Mik = newM[i, k];
                            double Mjk = newM[j, k];
                            newM[i, k] = Mik + Mjk * g;
                            newM[j, k] = Mjk + Mik * a;
                        }

                        for (int k = i + 1; k < j; k++)
                        {
                            double Kik = newK[i, k];
                            double Kkj = newK[k, j];
                            newK[i, k] = Kik + Kkj * g;
                            newK[k, j] = Kkj + Kik * a;

                            double Mik = newM[i, k];
                            double Mkj = newM[k, j];
                            newM[i, k] = Mik + Mkj * g;
                            newM[k, j] = Mkj + Mik * a;
                        }

                        double Kjj = newK[j, j];
                        double Mjj = newM[j, j];

                        newK[j, j] = Kjj + 2.0 * a * newK[i, j] + a * a * newK[i, i];
                        newM[j, j] = Mjj + 2.0 * a * newM[i, j] + a * a * newM[i, i];

                        newK[i, i] = newK[i, i] + 2.0 * g * newK[i, j] + g * g * Kjj;
                        newM[i, i] = newM[i, i] + 2.0 * g * newM[i, j] + g * g * Mjj;

                        newK[i, j] = 0.0;
                        newM[i, j] = 0.0;

                        // apply transformation to initial identity to get eigen vectors:

                        for (int k = 0; k < n; k++)
                        {
                            double Xki = X[k, i];
                            double Xkj = X[k, j];
                            X[k, i] = Xki + Xkj * g;
                            X[k, j] = Xkj + Xki * a;
                        }

                    }

                }

                // update eigen values and check convergence:

                Converged = true;

                for (int i = 0; i < n; i++)
                {
                    if (newK[i, i] > 0 && newM[i, i] > 0)
                    {
                        double l = newK[i, i] / newM[i, i];
                        Converged = Converged && System.Math.Abs((l - L[i]) / L[i]) < er_eval;
                        L[i] = l;
                    }
                    else
                        throw new Exception("Error on Jacobi solver: matrices are not positive definite.");
                }

                // if eigen values have converged: check if off-diagonal elements still need to be zeroed:

                if (Converged)
                {

                    er_evec = er_eval * er_eval;

                    for (int i = 0; i < n && Converged; i++)
                    {

                        for (int j = i + 1; j < n && Converged; j++)
                        {

                            if ((newM[i, j] * newM[i, j]) / (newM[i, i] * newM[j, j]) > er_evec ||
                                (newK[i, j] * newK[i, j]) / (newK[i, i] * newK[j, j]) > er_evec)
                            {
                                Converged = false;
                                continue;
                            }

                        }
                    }
                }

                Sweep++;
            }

            // sacale eigenvectors:

            for (int k = 0; k < n; k++)
            {
                for (int r = 0; r < n; r++)
                {
                    X[r, k] /= System.Math.Sqrt(newM[k, k]);
                }
            }

            // find incresing order:

            List<int> Ordered = new List<int>();
            List<int> NotOrdered = new List<int>();

            for (int i = 0; i < n; i++) { NotOrdered.Add(i); }

            while (NotOrdered.Count > 0)
            {
                int minp = NotOrdered[0];
                double minv = L[minp];
                int remp = 0;
                for (int i = 0; i < NotOrdered.Count; i++)
                {
                    if (L[NotOrdered[i]] < minv)
                    {
                        minv = L[NotOrdered[i]];
                        minp = NotOrdered[i];
                        remp = i;
                    }
                }
                Ordered.Add(minp);
                NotOrdered.RemoveAt(remp);
            }

            // set values and vectors in increasing order:
            
            int col = 0;

            foreach (int j in Ordered)
            {
                D[col] = L[j];
                for (int i = 0; i < n; i++)
                {
                    Q[i, col] = X[i, j];
                }

                col++;

            }

        }

        /// <summary>
        /// Solves an eigen value problem of the form Kφ = λMφ through the Lanczos iteration method. Both matrices should be positive definite.
        /// </summary>
        /// <param name="nSubSpace">Size of the modal subspace to converge</param>
        /// <remarks>
        /// This method uses the subspace iteration and Jacobi methods to aproach a limited number of eigen values of a large DOF system.
        /// This method has been presented by K.J. Bathe as a quite robust one. Convergence might be slower than through Lanczos method, but the implementation involves much simpler algorithms, and 
        /// convergence is much easier to control and checked.
        /// Gram-smith KM-ortogonalization occurs simultaneously for all vectors through a Ritz transformation. This is why  the method is very stable and less round-off errors sensitive.
        /// This method has been brought here as a complement to the OpenVOGEL project publisehd on GPLv3.
        /// OpenVOGEL (openvogel.org)
        /// Open source software for aerodynamics
        /// Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)
        /// <http://www.gnu.org/licenses/>.
        /// </remarks>
        public void SubspaceIteration(SymmetricMatrix M, 
                                      SymmetricMatrix K, 
                                      int nSubSpace, 
                                      int nModes, 
                                      out Vector D, 
                                      out Matrix V)
        {
            //------------------------------------------------------------------------------------
            // Check subspace dimension
            //------------------------------------------------------------------------------------

            int nDOF = M.RowCount;

            if (K.RowCount != nDOF)
            {
                throw new Exception("the size of the matrices does not match");
            }

            if (nSubSpace > nDOF)
            {
                throw new Exception("the subspace size is too large");
            }

            //------------------------------------------------------------------------------------
            // Set eigen values convergence threshold
            //------------------------------------------------------------------------------------

            double er_eval = EValError_Subspace;

            //------------------------------------------------------------------------------------
            // Decompose K in LU
            //------------------------------------------------------------------------------------

            LinearEquations K_LU = new LinearEquations ();
            K_LU.ComputeLU(K);

            //------------------------------------------------------------------------------------
            // Initialize temporal storage matrices
            //------------------------------------------------------------------------------------

            SymmetricMatrix K_p = new SymmetricMatrix(nSubSpace);
            SymmetricMatrix M_p = new SymmetricMatrix(nSubSpace);
            Matrix Q = new Matrix(nSubSpace);
            Vector L = new Vector(nSubSpace);

            D = new Vector(nSubSpace);
            V = new Matrix(nDOF, nSubSpace);

            bool Converged = false;
            int Step = 0;

            Matrix MV = new Matrix(nDOF, nSubSpace);
            Matrix MX = new Matrix(nDOF, nSubSpace);
            Matrix X = new Matrix(nDOF, nSubSpace);
            
            //------------------------------------------------------------------------------------
            // Set starting vectors
            //------------------------------------------------------------------------------------

            Random Rand = new Random();

            for (int i = 0; i < nDOF; i++)
            {
                V[i, 0] = M[i, i];
            }

            for (int i = 1; i < nSubSpace; i++)
            {
                if (i == nSubSpace - 1)
                {
                    for (int j = 0; j < nDOF; j++)
                    {
                        double Sign;
                        if (Rand.NextDouble() > 0.5)
                            Sign = 1.0;
                        else
                            Sign = -1.0;
                        V[j, i] = Rand.NextDouble() * Sign;
                    }
                }
                else
                    V[i, i] = 1.0;
            }

            //------------------------------------------------------------------------------------
            // Start sub space iteration loop 
            //------------------------------------------------------------------------------------

            while (Step < 15 && !Converged)
            {

                //--------------------------------------------------------------------------------
                // Find M.V
                //--------------------------------------------------------------------------------

                if (Step == 0)
                {
                    MV.Copy(V);
                }
                else
                {
                    // compute M.V

                    for (int i = 0; i < nDOF; i++)
                    {
                        for (int j = 0; j < nSubSpace; j++)
                        {
                            double p = 0;
                            for (int k = 0; k < nDOF; k++)
                            {
                                p += M[i, k] * V[k, j];
                            }
                            MV[i, j] = p;
                        }
                    }
                }

                //--------------------------------------------------------------------------------
                // Find new vector X:
                //--------------------------------------------------------------------------------

                K_LU.SolveLU(MV, X);

                //--------------------------------------------------------------------------------
                // Find projected K -> K_p = X_T.K.X (note that M.V = K.X, and then K_p = X_T.M.V)
                //--------------------------------------------------------------------------------

                for (int i = 0; i < nSubSpace; i++)
                {
                    for (int j = i; j < nSubSpace; j++)
                    {
                        double p = 0;
                        for (int k = 0; k < nDOF; k++)
                        {
                            p += X[k, i] * MV[k, j];
                        }
                        K_p[i, j] = p;
                    }
                }

                //--------------------------------------------------------------------------------
                // Find M.X  
                //--------------------------------------------------------------------------------          

                for (int i = 0; i < nDOF; i++)
                {
                    for (int j = 0; j < nSubSpace; j++)
                    {
                        double p = 0;
                        for (int k = 0; k < nDOF; k++)
                        {
                            p += M[i, k] * X[k, j];
                        }
                        MX[i, j] = p;
                    }
                }

                //--------------------------------------------------------------------------------
                // Find projected M -> M_p = X_T.M.X  
                //--------------------------------------------------------------------------------          

                for (int i = 0; i < nSubSpace; i++)
                {
                    for (int j = i; j < nSubSpace; j++)
                    {
                        double p = 0;
                        for (int k = 0; k < nDOF; k++)
                        {
                            p += X[k, i] * MX[k, j];
                        }
                        M_p[i, j] = p;
                    }
                }

                //--------------------------------------------------------------------------------
                // Save current values to check convergence
                //--------------------------------------------------------------------------------

                for (int i = 0; i < nSubSpace; i++)
                {
                    L[i] = D[i];
                }

                //--------------------------------------------------------------------------------
                // Solve reduced eigensystem in the projected space
                //--------------------------------------------------------------------------------

                Jacobi(K_p, M_p, Q, D);

                //--------------------------------------------------------------------------------
                // Apply Ritz transformation -> V = X.Q
                // V should tend to the truncated modal basis (subspace).
                //--------------------------------------------------------------------------------

                double deltaEvec = 0.0;

                for (int i = 0; i < nDOF; i++)
                {                    
                    for (int j = 0; j < nSubSpace; j++)
                    {
                        double p = 0;
                        for (int k = 0; k < nSubSpace; k++)
                        {
                            p += X[i, k] * Q[k, j];
                        }
                        if (Math.Abs (p) > 0) deltaEvec = Math.Max(deltaEvec, (p - V[i, j]) / p);
                        V[i, j] = p;
                    }
                }

                //--------------------------------------------------------------------------------
                // Check convergence
                // Only the required values are cheked for convergence (not the entire subspace)
                //--------------------------------------------------------------------------------

                Converged = deltaEvec <= EVecError_Subspace;

                for (int i = 0; i < nModes && Converged; i++)
                {
                    Converged = Converged && System.Math.Abs((D[i] - L[i]) / L[i]) < er_eval;
                }

                Step++;

            }
            
        }

        #endregion
        
        #region SymmetricBandMatrix

        /// <summary>
        ///Computes all the eigenvalues of
        /// a real symmetric band matrix A.
        /// </summary>
        /// <param name="A">The real symmetric band matrix A.</param>
        /// <returns>The eigenvalues.</returns>
        public Matrix GetEigenvalues(SymmetricBandMatrix A)
        {
            if (this._dsbev == null) this._dsbev = new DSBEV();

            this.CheckDimensions(A);

            Matrix SymmetricBand = A.GetSymmetricBandPackedMatrix();
            double[] SymmetricBandData = SymmetricBand.Data;
            Matrix EigenVects = new Matrix(1, 1);  //Se pone (1,1) pues nos e usaran
            double[] EigenVectsData = EigenVects.Data;
            Matrix EigenVals = new Matrix(A.RowCount, 1);
            double[] EigenValsData = EigenVals.Data;
            int Info = 0;

            double[] Work = new double[3 * A.RowCount - 2];


            _dsbev.Run("N", "U", A.RowCount, A.UpperBandWidth, ref SymmetricBandData, 0, SymmetricBand.RowCount, ref EigenValsData, 0, ref EigenVectsData, 0, A.RowCount, ref Work, 0, ref Info);


            #region Error
            /// = 0:  successful exit
            /// .LT. 0:  if INFO = -i, the i-th argument had an illegal value
            /// .GT. 0:  if INFO = i, the algorithm failed to converge; i
            /// off-diagonal elements of an intermediate tridiagonal
            /// form did not converge to zero.

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("The algorithm failed to converge.");
            }

            #endregion


            return EigenVals;
        }

        /// <summary>
        ///Computes all the eigenvalues and eigenvectors of
        /// a real symmetric band matrix A.
        /// </summary>
        /// <param name="A">The real symmetric band matrix A.</param>
        /// <param name="EigenVects">The eigenvectors.</param>
        /// <returns>The eigenvalues.</returns>
        public Matrix GetEigenvalues(SymmetricBandMatrix A, out Matrix EigenVects)
        {
            if (this._dsbev == null) this._dsbev = new DSBEV();
            this.CheckDimensions(A);

            Matrix SymmetricBand = A.GetSymmetricBandPackedMatrix();
            double[] SymmetricBandData = SymmetricBand.Data;
            EigenVects = new Matrix(A.RowCount, A.ColumnCount);
            double[] EigenVectsData = EigenVects.Data;
            Matrix EigenVals = new Matrix(A.RowCount, 1);
            double[] EigenValsData = EigenVals.Data;
            int Info = 0;

            double[] Work = new double[3 * A.RowCount - 2];


            _dsbev.Run("V", "U", A.RowCount, A.UpperBandWidth, ref SymmetricBandData, 0, SymmetricBand.RowCount, ref EigenValsData, 0, ref EigenVectsData, 0, A.RowCount, ref Work, 0, ref Info);


            #region Error
            /// = 0:  successful exit
            /// .LT. 0:  if INFO = -i, the i-th argument had an illegal value
            /// .GT. 0:  if INFO = i, the algorithm failed to converge; i
            /// off-diagonal elements of an intermediate tridiagonal
            /// form did not converge to zero.

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("The algorithm failed to converge.");
            }

            #endregion


            return EigenVals;
        }

        #endregion


        #region Complex General matrix

        /// <summary>
        /// Computes the eigenvalues for an complex general matrix A.
        /// </summary>
        /// <param name="A">The complex general matrix A.</param>
        /// <returns>The eigenvalues.</returns>
        public ComplexMatrix GetEigenvalues(ComplexMatrix A)
        {
            //Fortran Ejemplo
            //CG(NM,N,AR,AI,WR,WI,1,ZR,ZI,SCALE,ORTR,ORTI,ERROR)
            //C
            //C     THIS DRIVER TESTS  EISPACK  FOR THE CLASS OF COMPLEX GENERAL
            //C     MATRICES SUMMARIZING THE FIGURES OF MERIT FOR ALL PATHS.
            //C
            //C     THIS DRIVER IS CATALOGUED AS  EISPDRV4(CGSUMARY).
            //C
            //C     THE DIMENSION OF  AR,AI,ZR,ZI,ASAVER,ASAVEI,RM1,  AND  RM2 SHOULD
            //C     BE  NM  BY  NM.
            //C     THE DIMENSION OF  WR,WI,WR1,WI1,SELECT,SLHOLD,INT,SCALE,ORTR,ORTI,
            //C     RV1  AND  RV2  SHOULD BE  NM.
            //C     THE DIMENSION OF  ARHOLD  AND  AIHOLD  SHOULD BE  NM  BY  NM.
            //C     HERE NM = 20.


            if (this._cg == null) this._cg = new CG();

            this.CheckDimensions(A);

            Matrix AReal = A.GetReal();
            double[] ARealData = AReal.Data;
            Matrix AImag = A.GetImag();
            double[] AImagData = AImag.Data;

            ComplexMatrix EigenVals = new ComplexMatrix(A.RowCount, 1);
            Matrix RealEigenVals = new Matrix(A.RowCount, 1);
            double[] RealEigenValsData = RealEigenVals.Data;
            Matrix ImagEigenVals = new Matrix(A.RowCount, 1);
            double[] ImagEigenValsData = ImagEigenVals.Data;

            ComplexMatrix EigenVectors = new ComplexMatrix(1, 1);   
            Matrix RealEigVect = new Matrix(A.RowCount);
            double[] RealEigVectData = RealEigVect.Data;
            Matrix ImagEigVect = new Matrix(A.RowCount);
            double[] ImagEigVectData = ImagEigVect.Data;

            double[] SCALE = new double[A.RowCount];
            double[] ORTR = new double[A.RowCount];
            double[] ORTI = new double[A.RowCount];

            int Info = 0;

            int matz = 0; //No se calculan los eigenvectores

            _cg.Run(A.RowCount, A.RowCount, ref ARealData, 0, ref AImagData, 0, ref RealEigenValsData, 0, ref ImagEigenValsData, 0,
                matz, ref RealEigVectData, 0, ref ImagEigVectData, 0, ref SCALE, 0, ref ORTR, 0, ref ORTI, 0, ref Info);


            #region Error
            /// is set to
            /// zero       for normal return,
            /// j          if the limit of 30*n iterations is exhausted
            /// while the j-th eigenvalue is being sought.

            if (Info != 0)
            {
                throw new ArgumentException("The limit of 30*n iterations is exhausted");
            }

            #endregion



            EigenVals.SetReal(RealEigenVals);
            EigenVals.SetImag(ImagEigenVals);

            EigenVectors.SetReal(RealEigVect);
            EigenVectors.SetImag(ImagEigVect);

            return EigenVals;
        }

        /// <summary>
        /// Computes the eigenvalues and eigenvectors for an complex general matrix A.
        /// </summary>
        /// <param name="A">The complex general matrix A.</param>
        /// <param name="EigenVectors">The eigenvectors.</param>
        /// <returns>The eigenvalues.</returns>
        public ComplexMatrix GetEigenvalues(ComplexMatrix A, out ComplexMatrix EigenVectors)
        {
            //Fortran Ejemplo
            //CG(NM,N,AR,AI,WR,WI,1,ZR,ZI,SCALE,ORTR,ORTI,ERROR)
            //C
            //C     THIS DRIVER TESTS  EISPACK  FOR THE CLASS OF COMPLEX GENERAL
            //C     MATRICES SUMMARIZING THE FIGURES OF MERIT FOR ALL PATHS.
            //C
            //C     THIS DRIVER IS CATALOGUED AS  EISPDRV4(CGSUMARY).
            //C
            //C     THE DIMENSION OF  AR,AI,ZR,ZI,ASAVER,ASAVEI,RM1,  AND  RM2 SHOULD
            //C     BE  NM  BY  NM.
            //C     THE DIMENSION OF  WR,WI,WR1,WI1,SELECT,SLHOLD,INT,SCALE,ORTR,ORTI,
            //C     RV1  AND  RV2  SHOULD BE  NM.
            //C     THE DIMENSION OF  ARHOLD  AND  AIHOLD  SHOULD BE  NM  BY  NM.
            //C     HERE NM = 20.

            if (this._cg == null) this._cg = new CG();

            this.CheckDimensions(A);

            Matrix AReal = A.GetReal();
            double[] ARealData = AReal.Data;
            Matrix AImag = A.GetImag();
            double[] AImagData = AImag.Data;

            ComplexMatrix EigenVals = new ComplexMatrix(A.RowCount, 1);
            Matrix RealEigenVals = new Matrix(A.RowCount, 1);
            double[] RealEigenValsData = RealEigenVals.Data;
            Matrix ImagEigenVals = new Matrix(A.RowCount, 1);
            double[] ImagEigenValsData = ImagEigenVals.Data;

            EigenVectors = new ComplexMatrix(A.RowCount, A.ColumnCount);
            Matrix RealEigVect = new Matrix(A.RowCount);
            double[] RealEigVectData = RealEigVect.Data;
            Matrix ImagEigVect = new Matrix(A.RowCount);
            double[] ImagEigVectData = ImagEigVect.Data;

            double[] SCALE = new double[A.RowCount];
            double[] ORTR = new double[A.RowCount];
            double[] ORTI = new double[A.RowCount];

            int Info = 0;
            int matz = 1; //Se calculan los eigenvalores y los eigenvectores
            _cg.Run(A.RowCount, A.RowCount, ref ARealData, 0, ref AImagData, 0, ref RealEigenValsData, 0, ref ImagEigenValsData, 0,
                matz, ref RealEigVectData, 0, ref ImagEigVectData, 0, ref SCALE, 0, ref ORTR, 0, ref ORTI, 0, ref Info);


            #region Error
            /// is set to
            /// zero       for normal return,
            /// j          if the limit of 30*n iterations is exhausted
            /// while the j-th eigenvalue is being sought.

            if (Info != 0)
            {
                throw new ArgumentException("The limit of 30*n iterations is exhausted");
            }

            #endregion



            EigenVals.SetReal(RealEigenVals);
            EigenVals.SetImag(ImagEigenVals);

            EigenVectors.SetReal(RealEigVect);
            EigenVectors.SetImag(ImagEigVect);

            return EigenVals;
        }



        #endregion


        #region Private methods

        private void CheckDimensions(BaseMatrix matrixA)
        {
            if (matrixA.IsSquare != true)
            {
                throw new System.ArgumentException("Matrix A is not a square matrix.");
            }

        }

        private void CheckDimensions(ComplexMatrix matrixA)
        {
            if (matrixA.IsSquare != true)
            {
                throw new System.ArgumentException("Matrix A is not a square matrix.");
            }

        }

        #endregion


    }
}

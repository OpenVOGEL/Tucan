#region Copyright © 2009, De Santiago-Castillo JA. All rights reserved.

//Copyright © 2009 Jose Antonio De Santiago-Castillo 
//E-mail:JAntonioDeSantiago@gmail.com
//Web: www.DotNumerics.com
//
#endregion

using System;
using DotNumerics.LinearAlgebra.CSLapack;

namespace DotNumerics.LinearAlgebra
{

    /// <summary>
    /// Computes the solution to a system of linear equations.
    /// </summary>
    public sealed class LinearEquations
    {

        #region Fields

        DGESV _dgesv;
        DGBSV _dgbsv;
        DGTSV _dgtsv;

        #endregion
        
        /// <summary>
        /// Initializes a new instance of the LinearEquations class.
        /// </summary>
        public LinearEquations()
        {
        }

        #region Public LU Solver
        
        #region General Matrix
        
        /// <summary>
        /// Computes the solution to a real system of linear equations A * X = B, where A is a general matrix. 
        /// </summary>
        /// <param name="A">The square matrix.</param>
        /// <param name="B">The vector containing the right-hand side of the linear system.</param>
        /// <returns>A vector containing the solution to the linear system of equations.</returns>
        public Vector Solve(Matrix A, Vector B)
        {
            this.CheckDimensions(A, B);

            Matrix Solution = B.Clone();
            Matrix AClon = A.Clone();

            this.SolveInplace(AClon, Solution);

            return Solution.GetColumnVector(0);

        }

        /// <summary>
        /// Computes the solution to a real system of linear equations A * X = B, where A is a general matrix. 
        /// </summary>
        /// <param name="A">The square matrix.</param>
        /// <param name="B">The vector containing the right-hand side of the linear system.</param>
        /// <returns>A vector containing the solution to the linear system of equations.</returns>
        public double[] Solve(double[,] A, double[] B)
        {

            Vector Solution = new Vector(B);
            Matrix AClon = new Matrix(A);

            this.CheckDimensions(AClon, Solution);


            this.SolveInplace(AClon, Solution);

            double[] solutionDate = Solution.Data;

            return solutionDate;

        }
        
        /// <summary>
        /// Computes the solution to a real system of linear equations A * X = B, where A is a general matrix. 
        /// </summary>
        /// <param name="A">The square matrix.</param>
        /// <param name="B">The matrix containing the right-hand side of the linear system.</param>
        /// <returns>A matrix containing the solution to the linear system of equations.</returns>
        public Matrix Solve(Matrix A, Matrix B)
        {
            this.CheckDimensions(A, B);

            Matrix Solution = B.Clone();
            Matrix AClon = A.Clone();

            this.SolveInplace(AClon, Solution);
            

            return Solution;
        }
        
        /// <summary>
        /// In place, Computes the solution to a real system of linear equations A * X = B
        /// </summary>
        /// <param name="A">The square matrix.</param>
        /// <param name="B">The matrix containing the right-hand side of the linear system.</param>
        /// <returns>A matrix containing the solution to the linear system of equations.</returns>
        private void SolveInplace(Matrix A, Matrix B)
        {

            if (this._dgesv == null) this._dgesv = new DGESV();

            this.CheckDimensions(A, B);

            int numberEquations = A.RowCount;
            int numberSolutions = B.ColumnCount;
            double[] AMatrix = A.Data;
            double[] BMatrix = B.Data;

            int[] IPIV = new int[numberEquations];
            int Info = 0;

            this._dgesv.Run(numberEquations, numberSolutions, ref AMatrix, 0, numberEquations, ref IPIV, 0, ref BMatrix, 0, numberEquations, ref Info);
            
            #region Error
            // = 0:  successful exit
            // .LT. 0:  if INFO = -i, the i-th argument had an illegal value
            // .GT. 0:  if INFO = i, U(i,i) is exactly zero.  The factorization
            // has been completed, but the factor U is exactly
            // singular, so the solution could not be computed.

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("U(" + infoSTg + "," + infoSTg + ") is exactly zero.  The factorization has been completed, but the factor U is exactly singular, so the solution could not be computed.");
            }

            #endregion

        }
        
        /// <summary>
        /// LU cache
        /// </summary>
        private double[] _LU;

        /// <summary>
        /// Cached number of equations
        /// </summary>
        private int _N;

        /// <summary>
        /// The pivoting cache
        /// </summary>
        private int[] _IPIV;

        /// <summary>
        /// Result status given by LAPACK
        /// </summary>
        private int _Info = 0;

        /// <summary>
        /// Indicates if the Intel MKL should be used whenever possible.
        /// </summary>
        public static bool UseIntelMathKernel = false;

        /// <summary>
        /// Computes the LU and caches the results locally.
        /// Either the native .NET library or Intel MKL is used.
        /// </summary>
        /// <param name="A"></param>
        public void ComputeLU(Matrix A)
        {
            _N = A.RowCount;
            _LU = (double[]) A.Data.Clone();

            _IPIV = new int[_N];
            _Info = 0;
            
            if (UseIntelMathKernel)
            {
                IntelMathKernel.dgetrf(ref _N, ref _N, _LU, ref _N, _IPIV, ref _Info);
            }
            else                
            {
                DGETRF _dgetrf = new DGETRF();
                _dgetrf.Run(_N, _N, ref _LU, 0, _N, ref _IPIV, 0, ref _Info);
            }
                                
        }

        /// <summary>
        /// Solves the system of equations for right hand side vector B.
        /// </summary>
        /// <param name="B"></param>
        /// <param name="X"></param>
        public void SolveLU(Vector B, Vector X)
        {
            if (_LU != null)
            {
                // Make a copy of B on X:

                for (int i = 0; i < B.Length; i++) X[i] = B[i];

                // Solve the system A*X = B, overwriting B with X:

                if (UseIntelMathKernel)
                {
                    double[] _X = X.Data;
                    char[] trans = "No transponse".ToCharArray();
                    int one = 1;
                    IntelMathKernel.dgetrs(trans, ref _N, ref one, _LU, ref _N, _IPIV, _X, ref _N, ref _Info);
                }
                else
                {
                    DGETRS _dgetrs = new DGETRS();

                    double[] _X = X.Data;

                    _dgetrs.Run("No transpose", _N, 1, _LU, 0, _N, _IPIV, 0, ref _X, 0, _N, ref _Info);
                }
            }
        }

        /// <summary>
        /// Solves the system of equations for each column of matrix B.
        /// </summary>
        public void SolveLU(Matrix B, Matrix X)
        {
            if (_LU != null)
            {
                // TODO: implement direct MKL method

                if (B.RowCount == _N & X.RowCount == _N & B.ColumnCount == X.ColumnCount)
                {
                    for (int j = 0; j < B.ColumnCount; j++)
                    {
                        Vector _X = new Vector(_N);
                        SolveLU(B.Column(j), _X);
                        for (int i = 0; i < _N; i++)
                        {
                            X[i, j] = _X[i];
                        }
                    }
                }
                else
                {
                    throw new Exception("wrong sizes");
                }
            }
        }

        #endregion

            #region BandMatrix

            /// <summary>
            /// Computes the solution to a real system of linear equations
            /// A * X = B, where A is a band matrix.
            /// </summary>
            /// <param name="A">The band matrix.</param>
            /// <param name="B">The vector containing the right-hand side of the linear system.</param>
            /// <returns>A vector containing the solution to the linear system of equations.</returns>
        public Vector Solve(BandMatrix A, Vector B)
        {
            Matrix myB = B;
            Vector solution = this.Solve(A, myB).GetColumnVector(0);
            return this.Solve(A, B);
        }
        
        /// <summary>
        /// Computes the solution to a real system of linear equations
        /// A * X = B, where A is a band matrix.
        /// </summary>
        /// <param name="A">The band matrix.</param>
        /// <param name="B">The matrix containing the right-hand side of the linear system.</param>
        /// <returns>A matrix containing the solution to the linear system of equations.</returns>
        public Matrix Solve(BandMatrix A, Matrix B)
        {
            if (this._dgbsv == null) this._dgbsv = new DGBSV();

            this.CheckDimensions(A, B);

            Matrix Solution = B.Clone();
            Matrix ExtendedMatrix = A.GetBandPackedMatrix();
            double[] BAData = ExtendedMatrix.Data;
            double[] SolutionData = Solution.Data;

            int[] IPIV = new int[A.RowCount];
            int Info = 0;

            this._dgbsv.Run(A.RowCount, A.LowerBandWidth, A.UpperBandWidth, B.ColumnCount, ref BAData, 0, ExtendedMatrix.RowCount, ref IPIV, 0, ref SolutionData, 0, Solution.RowCount, ref Info);


            #region Error
            /// = 0:  successful exit
            /// .LT. 0:  if INFO = -i, the i-th argument had an illegal value
            /// .GT. 0:  if INFO = i, U(i,i) is exactly zero.  The factorization
            /// has been completed, but the factor U is exactly
            /// singular, and the solution has not been computed.
            ///</param>

            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("U(" + infoSTg + "," + infoSTg + ") is exactly zero.  The factorization has been completed, but the factor U is exactly singular, so the solution could not be computed.");
            }

            #endregion




            return Solution;
        }
        
        #endregion

        #region TridiagonalMatrix

        /// <summary>
        /// Computes the solution to a real system of linear equations
        /// A * X = B, where A is a tridiagonal matrix.
        /// </summary>
        /// <param name="A">The tridiagonal matrix.</param>
        /// <param name="B">The matrix containing the right-hand side of the linear system.</param>
        /// <returns>A matrix containing the solution to the linear system of equations.</returns>
        public Matrix Solve(TridiagonalMatrix A, Matrix B)
        {

            if (this._dgtsv == null) this._dgtsv = new DGTSV();

            this.CheckDimensions(A, B);

            Matrix Solution = B.Clone();
            double[] SolutionData = Solution.Data;
            double[] Diagonal;
            double[] SubDiagonal;
            double[] SuperDiagonal;
            A.GetPackedMatrix(out SubDiagonal, out SuperDiagonal, out Diagonal);

            int[] IPIV = new int[A.RowCount];
            int Info = 0;

            this._dgtsv.Run(A.RowCount, B.ColumnCount, ref SubDiagonal, 0, ref Diagonal, 0, ref SuperDiagonal, 0, ref SolutionData, 0, Solution.RowCount, ref Info);


            #region Error
            /// = 0: successful exit
            /// .LT. 0: if INFO = -i, the i-th argument had an illegal value
            /// .GT. 0: if INFO = i, U(i,i) is exactly zero, and the solution
            /// has not been computed.  The factorization has not been
            /// completed unless i = N.
            if (Info < 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new ArgumentException("the " + infoSTg + " -th argument had an illegal value");
            }
            else if (Info > 0)
            {
                string infoSTg = Math.Abs(Info).ToString();
                throw new Exception("U(" + infoSTg + "," + infoSTg + ") is exactly zero.  and the solution has not been computed.  The factorization has not been completed unless i = N.");
            }

            #endregion

            return Solution;
        }

        #endregion

        private void CheckDimensions(BaseMatrix matrixA, BaseMatrix matrixB)
        {
            if (matrixA.IsSquare!= true)
            {
                throw new System.ArgumentException("Matrix A is not a square matrix.");
            }


            if (matrixA.RowCount != matrixB.RowCount)
            {
                throw new System.ArgumentException("Matrix dimensions are not valid.");
            }
        }

        private void CheckDimensions(BaseMatrix matrixA, Vector vectorB)
        {
            if (matrixA.IsSquare != true)
            {
                throw new System.ArgumentException("Matrix A is not a square matrix.");
            }

            if (vectorB.Type != VectorType.Column)
            {
                throw new System.ArgumentException("The vector should be a column vector.");
            }

            if (matrixA.RowCount != vectorB.Length)
            {
                throw new System.ArgumentException("Matrix dimensions are not valid.");
            }
        }
        
        #endregion

        #region Public QR Solver

        #region General Matrix

        private double[] _QR;
        private double[] _TAU;
        private double[] _WORK;
        
        public void ComputeQR(Matrix A)
        {
            _N = A.RowCount;
            _QR = (double[])A.Data.Clone();

            _TAU = new double[_N];
            _WORK = new double[_N];
            _Info = 0;

            DGEQRF _dgeqrf = new DGEQRF();

            _dgeqrf.Run(_N, _N, ref _QR, 0, _N, ref _TAU, 0, ref _WORK, 0, _N, ref _Info);

        }

        public void SolveQR(Vector B, Vector X)
        {
            if (_QR != null)
            {
                // Solve the system A*X = B, overwriting B with X:

                double[] _B = B.Data;
                
                DORMQR _dormqr = new DORMQR();

                _dormqr.Run("L", "T", _N, 1, _N, ref _QR, 0, _N, _TAU, 0, ref _B, 0, _N, ref _WORK, 0, _N, ref _Info);

                DTRTRS _dtrtrs = new DTRTRS();

                _dtrtrs.Run("U", "N", "N", _N, 1, _QR, 0, _N, ref _B, 0, _N, ref _Info);

                for (int i = 0; i < B.Length; i++) X[i] = B[i];

            }
        }

        #endregion

        #endregion
        
    }

}

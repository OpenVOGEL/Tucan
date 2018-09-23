//Copyright (C) Microsoft Corporation.  All rights reserved.

using System;
using DotNumerics_Samples.Dumper;
using DotNumerics_Samples.Harness;

using DotNumerics.LinearAlgebra;
using DotNumerics;

namespace DotNumerics_Samples.Samples
{
    [Title("Linear Algebra")]
    [Prefix("LinearAlgebra")]
    class LinearAlgebra : SchemaInformationBasedSample
    {
        #region Solve

        [Category("LinearEquations")]
        [Title("Solve")]
        [Description("The LinearEquations class computes the solution to a real system of linear equations (general, band and tridiagonal matrices): A * X = B")]
        public void LinearAlgebraSolve()
        {
            //using DotNumerics.LinearAlgebra;
            //using DotNumerics;

            Matrix A = new Matrix(3, 3);
            A[0, 0] = 2; A[0, 1] = 5; A[0, 2] = 3;
            A[1, 0] = 1; A[1, 1] = 5; A[1, 2] = 7;
            A[2, 0] = 8; A[2, 1] = 2; A[2, 2] = 3;

            Matrix B = new Matrix(3, 1);
            B[0, 0] = 5;
            B[1, 0] = 3;
            B[2, 0] = 8;

            LinearEquations leq = new LinearEquations();
            Matrix X = leq.Solve(A, B);

            Matrix AXmB = A * X - B;

            ObjectDumper.Write("A=");
            ObjectDumper.Write(A.MatrixToString("0.000"));

            ObjectDumper.Write("B=");
            ObjectDumper.Write(B.MatrixToString("0.000"));

            ObjectDumper.Write("X=");
            ObjectDumper.Write(X.MatrixToString("0.000"));

            ObjectDumper.Write("A*X-B =");
            ObjectDumper.Write(AXmB.MatrixToString("0.000"));

        }

        [Category("LinearEquations")]
        [Title("Linear Least Squares")]
        [Description("The LinearLeastSquares class computes the minimum-norm solution to a real linear least squares problem: minimize 2-norm(|| A*X - B||) involving an M-by-N matrix A. ")]
        public void LinearAlgebraLLS()
        {
            //using DotNumerics.LinearAlgebra;
            //using DotNumerics;

            Matrix A = new Matrix(3, 2);
            A[0, 0] = 2; A[0, 1] = 5;
            A[1, 0] = 1; A[1, 1] = 5;
            A[2, 0] = 8; A[2, 1] = 2;

            Matrix B = new Matrix(3, 1);
            B[0, 0] = 5;
            B[1, 0] = 3;
            B[2, 0] = 8;

            LinearLeastSquares leastSquares = new LinearLeastSquares();

            Matrix Xcof = leastSquares.COFSolve(A, B);
            Matrix Xqr = leastSquares.QRorLQSolve(A, B);
            Matrix Xsvd = leastSquares.SVDdcSolve(A, B);

            ObjectDumper.Write("A=");
            ObjectDumper.Write(A.MatrixToString("0.000"));

            ObjectDumper.Write("B=");
            ObjectDumper.Write(B.MatrixToString("0.000"));

            ObjectDumper.Write("Using a omplete orthogonal factorization of A. X =");
            ObjectDumper.Write(Xcof.MatrixToString("0.000"));

            ObjectDumper.Write("Using a QR or LQ factorization of A. X = ");
            ObjectDumper.Write(Xqr.MatrixToString("0.000"));

            ObjectDumper.Write("Using the singular value decomposition of A. X = ");
            ObjectDumper.Write(Xsvd.MatrixToString("0.000"));

        }


        #endregion

        #region Matrix

        [Category("Matrix")]
        [Title("Matrix Inverse")]
        [Description("The Matrix class can be used to calculate the inverse matrix, determinant, transpose, Frobenius norm, one norm, infinity norm, trace.")]
        public void LinearAlgebraMatrixInverse()
        {
            //using DotNumerics.LinearAlgebra;
            //using DotNumerics;

            Matrix A = new Matrix(3, 3);
            A[0, 0] = 2; A[0, 1] = 5; A[0, 2] = 3;
            A[1, 0] = 1; A[1, 1] = 5; A[1, 2] = 7;
            A[2, 0] = 8; A[2, 1] = 2; A[2, 2] = 3;


            Matrix inverseA = A.Inverse();
            Matrix I = A * inverseA;

            ObjectDumper.Write("A=");
            ObjectDumper.Write(A.MatrixToString("0.000"));

            ObjectDumper.Write("Inverse matrix:");
            ObjectDumper.Write(inverseA.MatrixToString("0.000"));

            ObjectDumper.Write("A * inverseA = ");
            ObjectDumper.Write(I.MatrixToString("0.000"));
        }

        [Category("Matrix")]
        [Title("Singular Value Decomposition")]
        [Description("The SingularValueDecomposition class computes the singular value decomposition (SVD) of a real M-by-N matrix: A = U * S * transpose(V)")]
        public void LinearAlgebraSVD()
        {
            //using DotNumerics.LinearAlgebra;
            //using DotNumerics;

            Matrix A = new Matrix(3, 2);
            A[0, 0] = 2; A[0, 1] = 5; 
            A[1, 0] = 1; A[1, 1] = 5;
            A[2, 0] = 8; A[2, 1] = 2;


            SingularValueDecomposition svd = new SingularValueDecomposition();
            Matrix S;
            Matrix U;
            Matrix VT;
            svd.ComputeSVD(A, out S, out U, out VT);

            Matrix AmUSVT = A - U * S * VT;

            ObjectDumper.Write("A=");
            ObjectDumper.Write(A.MatrixToString("0.000"));

            ObjectDumper.Write("S =");
            ObjectDumper.Write(S.MatrixToString("0.000"));

            ObjectDumper.Write("U = ");
            ObjectDumper.Write(U.MatrixToString("0.000"));

            ObjectDumper.Write("VT = ");
            ObjectDumper.Write(VT.MatrixToString("0.000"));

            ObjectDumper.Write("A - U * S * VT - VT = ");
            ObjectDumper.Write(AmUSVT.MatrixToString("0.000"));
        }


        #endregion

        #region EigenSystem

        [Category("EigenSystem")]
        [Title("Eigenvalues")]
        [Description("The EigenSystem class computes the eigenvalues and the eigenvectors of a square matrix (general, symmetric, symmetric band and complex general matrices).")]
        public void LinearAlgebraEigenvalues()
        {
            //using DotNumerics.LinearAlgebra;
            //using DotNumerics;

            Matrix A = new Matrix(3, 3);
            A[0, 0] = 2; A[0, 1] = 5; A[0, 2] = 3;
            A[1, 0] = 1; A[1, 1] = 5; A[1, 2] = 7;
            A[2, 0] = 8; A[2, 1] = 2; A[2, 2] = 3;

            EigenSystem es = new EigenSystem();
            ComplexMatrix eigenvalues = es.GetEigenvalues(A);

            ObjectDumper.Write("A=");
            ObjectDumper.Write(A.MatrixToString("0.000"));

            ObjectDumper.Write("Eigenvalues:");
            ObjectDumper.Write(eigenvalues.MatrixToString("0.000"));
        }

        [Category("EigenSystem")]
        [Title("Eigenvalues And Eigenvectors")]
        [Description("The EigenSystem class computes the eigenvalues and the eigenvectors of a square matrix (general, symmetric, symmetric band and complex general matrices).")]
        public void LinearAlgebraEigenvaluesAndEigenvectors()
        {
            //using DotNumerics.LinearAlgebra;
            //using DotNumerics;

            Matrix A = new Matrix(3, 3);
            A[0, 0] = 2; A[0, 1] = 5; A[0, 2] = 3;
            A[1, 0] = 1; A[1, 1] = 5; A[1, 2] = 7;
            A[2, 0] = 8; A[2, 1] = 2; A[2, 2] = 3;

            EigenSystem es = new EigenSystem();
            ComplexMatrix eigenvectors;
            ComplexMatrix eigenvalues = es.GetEigenvalues(A, out eigenvectors);

            //Ax-lX=0
            Complex lambda = eigenvalues[0, 0];
            ComplexVector X = eigenvectors.GetColumnVectors()[0];
            ComplexMatrix AXmlambdaX = A.CopyToComplex() * X - lambda * X;

            ObjectDumper.Write("A=");
            ObjectDumper.Write(A.MatrixToString("0.000"));

            ObjectDumper.Write("Eigenvalues:");
            ObjectDumper.Write(eigenvalues.MatrixToString("0.000"));

            ObjectDumper.Write("Eigenvectors:");
            ObjectDumper.Write(eigenvectors.MatrixToString("0.000"));

            ObjectDumper.Write("A * X - lambda * X = ");
            ObjectDumper.Write(AXmlambdaX.MatrixToString("0.000"));
        }

        [Category("EigenSystem")]
        [Title("Complex Matrix")]
        [Description("The EigenSystem class computes the eigenvalues and the eigenvectors of a square matrix (general, symmetric, symmetric band and complex general matrices).")]
        public void LinearAlgebraComplexMatrix()
        {
            //using DotNumerics.LinearAlgebra;
            //using DotNumerics;

            ComplexMatrix A = new ComplexMatrix(3, 3);
            A[0, 0] = new Complex(3, 9); A[0, 1] = new Complex(4, 6); A[0, 2] = new Complex(1, 8);
            A[1, 0] = new Complex(8, 3); A[1, 1] = new Complex(2, 4); A[1, 2] = new Complex(5, 5);
            A[2, 0] = new Complex(2, 5); A[2, 1] = new Complex(7, 2); A[2, 2] = new Complex(5, 5);

            EigenSystem es = new EigenSystem();
            ComplexMatrix eigenvectors;
            ComplexMatrix eigenvalues = es.GetEigenvalues(A, out eigenvectors);

            //Ax-lX=0
            Complex lambda = eigenvalues[0, 0];
            ComplexVector X = eigenvectors.GetColumnVectors()[0];
            ComplexMatrix AXmlambdaX = A * X - lambda * X;

            ObjectDumper.Write("A=");
            ObjectDumper.Write(A.MatrixToString("0.000"));

            ObjectDumper.Write("Eigenvalues:");
            ObjectDumper.Write(eigenvalues.MatrixToString("0.000"));

            ObjectDumper.Write("Eigenvectors:");
            ObjectDumper.Write(eigenvectors.MatrixToString("0.000"));

            ObjectDumper.Write("A * X - lambda * X = ");
            ObjectDumper.Write(AXmlambdaX.MatrixToString("0.000"));
        }

        #endregion

    }
}

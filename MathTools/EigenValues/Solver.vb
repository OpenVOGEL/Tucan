'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program.  If Not, see < http:  //www.gnu.org/licenses/>.

Imports Meta.Numerics.Matrices
Imports System.IO

Namespace MathLibrary.EigenValues

    ''' <summary>
    ''' Provides methods to find eigen vectors and eigen values of systems with reduced number of DOF.
    ''' </summary>
    ''' <remarks>
    ''' This solver might not be fast for systems with large DOF
    ''' </remarks>
    Public Class EigenValuesSolver

        Private Mass As SymmetricMatrix
        Private Stiffness As SymmetricMatrix

        Private EShapes As List(Of ColumnVector) = New List(Of ColumnVector)
        Private EVals As List(Of Double) = New List(Of Double)

        Public EValError As Double = 0.005

        ''' <summary>
        ''' Finds modes and stores them on the database
        ''' </summary>
        ''' <param name="DataBasePath">File where matrices are to be found and results are to be written</param>
        ''' <param name="DOF">Number of degrees of freedom</param>
        ''' <param name="MaxIter">Maximum number of inverse iterations</param>
        ''' <param name="NModes">Number of wanted modes</param>
        ''' <remarks>This solver uses the inverse iteation method with Gram-Smith orthogonalization to find eigen vectors and values of the system KX = vMX.
        ''' Algorithm makes extensive use of Pardiso to solve systems of algabraic equations at each refining step.</remarks>
        Public Sub InverseIteration(ByVal DataBasePath As String, ByVal DataBasePathK As String, ByVal DataBasePathM As String, ByVal DOF As Integer, Optional ByVal MaxIter As Integer = 100, Optional ByVal NModes As Integer = 5)

            ReadMatrixM(DataBasePathM, DOF)
            ReadMatrixK(DataBasePathK, DOF)

            Dim CD As CholeskyDecomposition = Stiffness.CholeskyDecomposition()

            Dim Xi As ColumnVector '= New ColumnVector(DOF) ' Initial shape
            Dim Xk As ColumnVector '= New ColumnVector(DOF) ' Aproached shape
            Dim Bk As ColumnVector = New ColumnVector(DOF) ' Right hand side
            Dim MXk As ColumnVector '= New ColumnVector(DOF) ' Multiplication of aproached shape by mass matrix

            ' Find initial displacement

            For i = 0 To DOF - 1
                Bk(i) = Mass(i, i)
            Next

            Xi = CD.Solve(Bk)

            Dim AMax As Double = 1
            Dim AMin As Double = 1

            AMax = Xi.Max
            AMin = Xi.Min

            Dim Norm As Double = 1 / System.Math.Max(System.Math.Abs(AMax), System.Math.Abs(AMin))
            Xi = Norm * Xi

            ' Find first mode

            Xk = Xi.Copy

            For s = 0 To MaxIter

                If (s > 1) Then
                    Bk = Mass * Xk
                End If

                Xk = CD.Solve(Bk)

                AMax = Xk.Max
                AMin = Xk.Min

                Norm = 1 / System.Math.Max(System.Math.Abs(AMax), System.Math.Abs(AMin))
                Xk = Norm * Xk

            Next

            AddMode(Xk)

            ' Find higher modes

            ' Initialize deflation coeficients:

            Dim q As New List(Of Double)

            ' Start approach:

            For m = 1 To NModes - 1

                Xk = Xi.Copy()
                q.Add(0.0)

                For s = 0 To MaxIter

                    'If (True) Then ' (Step < 10 || Step / 2 == 0) Write here a conditionant to limite the number of deflations

                    MXk = Mass * Xk

                    ' Deflate current vector of already found mode shapes: 

                    For j = 0 To m - 1

                        q(j) = 0
                        For n = 0 To DOF - 1
                            q(j) += MXk(n) * EShapes(j)(n)
                        Next

                        For n = 0 To DOF - 1
                            Xk(n) -= q(j) * EShapes(j)(n)
                        Next

                    Next

                    'End If

                    Bk = Mass * Xk
                    Xk = CD.Solve(Bk)

                    AMax = Xk.Max
                    AMin = Xk.Min

                    Norm = 1 / System.Math.Max(System.Math.Abs(AMax), System.Math.Abs(AMin))
                    Xk = Norm * Xk

                Next

                AddMode(Xk)

            Next

            WriteShapes(DataBasePath)

        End Sub

        Public Sub SubspaceIteration(ByVal DataBasePath As String,
                                     ByVal PathK As String,
                                     ByVal PathM As String,
                                     ByVal nDOF As Integer,
                                     ByVal nModes As Integer,
                                     ByVal nSubSpace As Integer)

            ReadMatrixM(PathM, nDOF)
            ReadMatrixK(PathK, nDOF)

            ' Set eigen values convergence threshold

            Dim er_eval As Double = EValError

            '---------------------------------------------
            ' Check subspace dimension
            '---------------------------------------------

            If (nSubSpace > nDOF) Then nSubSpace = nDOF

            Dim Q As New SymmetricMatrix(nSubSpace)
            Dim D As New ColumnVector(nSubSpace)

            Jacobi(Stiffness, Mass, Q, D)

            Return

            Dim projected_K As New SymmetricMatrix(nSubSpace)
            Dim projected_M As New SymmetricMatrix(nSubSpace)
            Dim L As New ColumnVector(nSubSpace)
            Dim V = New RectangularMatrix(nDOF, nSubSpace)

            '---------------------------------------------
            ' Set starting vectors
            '---------------------------------------------

            Dim randGenerator As Random = New Random()

            For i = 0 To nDOF - 1
                V(i, 0) = Mass(i, i)
            Next

            For i = 1 To nSubSpace - 1

                If (i = nSubSpace - 1) Then
                    For j = 0 To nDOF - 1
                        Dim sign As Short
                        If (randGenerator.NextDouble() > 0.5) Then
                            sign = 1
                        Else
                            sign = -1
                        End If
                        V(j, i) = randGenerator.NextDouble() * sign
                    Next
                Else
                    V(i, i) = 1
                End If
            Next

            '---------------------------------------------
            ' Begin sub space iteration loop
            '---------------------------------------------

            Dim converged As Boolean = False
            Dim time_step As Integer = 0

            Dim MV As New RectangularMatrix(nDOF, nSubSpace)
            Dim MX As New RectangularMatrix(nDOF, nSubSpace)
            Dim X As New RectangularMatrix(nDOF, nSubSpace)
            Dim K_Decomposed As CholeskyDecomposition = Stiffness.CholeskyDecomposition

            While (time_step < 15 And Not converged)

                '---------------------------------------------
                ' Compute new vector X
                '---------------------------------------------

                If (time_step = 0) Then
                    MV = V.Copy
                Else
                    MV = Mass * V
                End If

                For i = 0 To nSubSpace - 1
                    Dim cX As ColumnVector = K_Decomposed.Solve(MV.Column(i))
                    For j = 0 To nDOF - 1
                        X(j, i) = cX(j)
                    Next
                Next

                '---------------------------------------------
                ' Get X transposed
                '---------------------------------------------

                Dim XT As RectangularMatrix = X.Transpose

                '---------------------------------------------
                ' Find stiffness projection matrix: (note that MV = KX)
                '---------------------------------------------

                Dim projected_K_0 As RectangularMatrix = XT * MV

                For i = 0 To projected_K_0.RowCount - 1
                    For j = i To projected_K_0.ColumnCount - 1
                        projected_K(i, j) = projected_K_0(i, j)
                    Next
                Next

                '---------------------------------------------
                ' Find mass projection matrix
                '---------------------------------------------

                MX = Mass * X

                Dim projected_M_0 As RectangularMatrix = XT * MX

                For i = 0 To projected_M_0.RowCount - 1
                    For j = i To projected_M_0.ColumnCount - 1
                        projected_M(i, j) = projected_M_0(i, j)
                    Next
                Next

                '---------------------------------------------
                ' Save current values to check convergence
                '---------------------------------------------

                For i = 0 To nSubSpace - 1
                    L(i) = D(i)
                Next

                '---------------------------------------------
                ' Solve reduced eigensystem using Jacobi method
                '---------------------------------------------

                Jacobi(projected_K, projected_M, Q, D)

                '---------------------------------------------
                ' Apply Ritz transformation (V should tend to the truncated modal basis)
                '---------------------------------------------

                V = X * Q

                '---------------------------------------------
                ' Check convergence
                '---------------------------------------------

                converged = True

                For i = 0 To nModes - 1 ' < only the required values are cheked for convergence (not the entire subspace)
                    converged = converged And System.Math.Abs((D(i) - L(i)) / L(i)) < er_eval
                Next

                time_step += 1

            End While

            For i = 0 To nModes - 1

                EShapes.Add(V.Column(i))
                EVals.Add(D(i))

            Next

            WriteShapes(DataBasePath)

        End Sub

        ''' <summary>
        ''' Finds the eigen values and eigen vectors of a small system Kφ = λMφ. K and M are dense symmetric matrices, K is non-singular and M is positive-definite. 
        ''' </summary>
        ''' <param name="K">K matrix</param>
        ''' <param name="M">M matrix</param>
        ''' <param name="Q">M-normalized eigen vectors (in columns)</param>
        ''' <param name="D">Eigen values</param>
        ''' <remarks>
        ''' This method is suitable for small systems only, and will work better as matrices K and M have many off-diagonal zeros. 
        ''' This is why this method can be succesfully implemented on the subspace iteration method (where projected matrices tend to diagonal form).
        ''' If a lumped matrix is applied, this method will work even faster.
        ''' </remarks>
        Public Sub Jacobi(ByRef K As SymmetricMatrix, ByRef M As SymmetricMatrix, ByRef Q As SymmetricMatrix, ByRef D As ColumnVector)

            Dim n As Integer = K.RowCount

            Dim L As New ColumnVector(n)
            Dim X As New SymmetricMatrix(n)

            For i = 0 To n - 1 ' < load identity
                X(i, i) = 1
            Next

            Dim er_evec As Double = 0.0       ' < eigen vectors convergence threshold
            Dim er_eval As Double = 0.000001  ' < eigen values convergence threshold

            Dim converged As Boolean = False
            Dim sweep As Integer = 0

            While (Not converged And sweep < 100)

                er_evec = System.Math.Pow(0.01, 2 * (sweep + 1)) ' < update sweep threshold

                For i = 0 To n - 2

                    For j = i + 1 To n - 1

                        If ((M(i, j) * M(i, j)) / (M(i, i) * M(j, j)) < er_evec And
                            (K(i, j) * K(i, j)) / (K(i, i) * K(j, j)) < er_evec) Then
                            Continue For ' < no zeroing required for this off-diagonal element
                        End If

                        ' calculate alpha and beta to zero off-diagonal elements {i, j} on K and M:

                        Dim kii As Double = K(i, i) * M(i, j) - M(i, i) * K(i, j)
                        Dim kjj As Double = K(j, j) * M(i, j) - M(j, j) * K(i, j)
                        Dim kdash As Double = K(i, i) * M(j, j) - K(j, j) * M(i, i)

                        Dim check As Double = 0.25 * kdash * kdash + kii * kjj

                        Dim den As Double = 0.5 * kdash + Math.Sign(kdash) * Math.Sqrt(check)

                        Dim g As Double = 0.0
                        Dim a As Double = 0.0

                        If (System.Math.Abs(den) > 1.0E-50) Then
                            g = -kii / den
                            a = kjj / den
                        Else
                            g = -K(i, j) / K(j, j)
                            a = 0.0
                        End If

                        ' Perform rotation to annihilate off-diagonal element ij:

                        For p As Integer = 0 To i - 1
                            Dim Kki As Double = K(p, i)
                            Dim Kkj As Double = K(p, j)
                            K(p, i) = Kki + Kkj * g
                            K(p, j) = Kkj + Kki * a

                            Dim Mki As Double = M(p, i)
                            Dim Mkj As Double = M(p, j)
                            M(p, i) = Mki + Mkj * g
                            M(p, j) = Mkj + Mki * a
                        Next

                        For p As Integer = j + 1 To n - 1
                            Dim Kik As Double = K(i, p)
                            Dim Kjk As Double = K(j, p)
                            K(i, p) = Kik + Kjk * g
                            K(j, p) = Kjk + Kik * a

                            Dim Mik As Double = M(i, p)
                            Dim Mjk As Double = M(j, p)
                            M(i, p) = Mik + Mjk * g
                            M(j, p) = Mjk + Mik * a
                        Next

                        For p As Integer = i + 1 To j - 1
                            Dim Kik As Double = K(i, p)
                            Dim Kkj As Double = K(p, j)
                            K(i, p) = Kik + Kkj * g
                            K(p, j) = Kkj + Kik * a

                            Dim Mik As Double = M(i, p)
                            Dim Mkj As Double = M(p, j)
                            M(i, p) = Mik + Mkj * g
                            M(p, j) = Mkj + Mik * a
                        Next

                        Dim Kjj_II As Double = K(j, j)
                        Dim Mjj_II As Double = M(j, j)

                        K(j, j) = Kjj_II + 2.0 * a * K(i, j) + a * a * K(i, i)
                        M(j, j) = Mjj_II + 2.0 * a * M(i, j) + a * a * M(i, i)

                        K(i, i) = K(i, i) + 2.0 * g * K(i, j) + g * g * Kjj_II
                        M(i, i) = M(i, i) + 2.0 * g * M(i, j) + g * g * Mjj_II

                        K(i, j) = 0.0
                        M(i, j) = 0.0

                        ' apply transformation to initial identity to get eigen vectors:

                        For p As Integer = 0 To n - 1
                            Dim Xki As Double = X(p, i)
                            Dim Xkj As Double = X(p, j)
                            X(p, i) = Xki + Xkj * g
                            X(p, j) = Xkj + Xki * a
                        Next

                    Next

                Next

                ' update eigen values and check convergence:

                converged = True

                For i = 0 To n - 1

                    If (K(i, i) > 0 And M(i, i) > 0) Then
                        Dim w As Double = K(i, i) / M(i, i)
                        converged = converged And System.Math.Abs((w - L(i)) / L(i)) < er_eval
                        L(i) = w
                    Else
                        Throw New Exception("Error on Jacobi solver: matrices are not positive definite.")
                    End If

                Next

                ' if eigen values have converged: check if off-diagonal elements still need to be zeroed:

                If converged Then

                    er_evec = er_eval * er_eval

                    For i = 0 To n - 1

                        For j = i + 1 To n - 1

                            If ((M(i, j) * M(i, j)) / (M(i, i) * M(j, j)) > er_evec Or
                            (K(i, j) * K(i, j)) / (K(i, i) * K(j, j)) > er_evec) Then
                                converged = False
                                Continue For
                            End If

                        Next

                    Next

                End If

                sweep += 1

            End While

            ' sacale eigenvectors:

            For p As Integer = 0 To n - 1
                For r As Integer = 0 To n - 1
                    X(r, p) /= System.Math.Sqrt(M(p, p))
                Next
            Next

            ' find incresing order:

            Dim Ordered As New List(Of Integer)
            Dim NotOrdered As New List(Of Integer)

            For i = 0 To n - 1
                NotOrdered.Add(i)
            Next

            While (NotOrdered.Count > 0)
                Dim minp As Integer = NotOrdered(0)
                Dim minv As Double = L(minp)
                Dim remp As Integer = 0
                For i = 0 To NotOrdered.Count - 1
                    If (L(NotOrdered(i)) < minv) Then
                        minv = L(NotOrdered(i))
                        minp = NotOrdered(i)
                        remp = i
                    End If
                Next
                Ordered.Add(minp)
                NotOrdered.RemoveAt(remp)
            End While

            ' set values and vectors in increasing order:

            Dim col As Integer = 0

            For Each j As Integer In Ordered

                D(col) = L(j)
                For i = 0 To n - 1
                    Q(i, col) = X(i, j)
                Next

                col += 1

            Next

        End Sub

        ''' <summary>
        ''' Adds normalizes the given eigen vector with respect to M and stores it on the list.
        ''' </summary>
        Private Sub AddMode(ByRef EShape As ColumnVector)

            Dim MXk As ColumnVector
            Dim KXk As ColumnVector

            MXk = Mass * EShape
            Dim EMass As Double = MXk.Transpose * EShape

            KXk = Stiffness * EShape
            Dim EStff As Double = KXk.Transpose * EShape

            EShape = (1 / System.Math.Sqrt(EMass)) * EShape

            Dim Mode As ColumnVector
            Mode = EShape.Copy
            EShapes.Add(Mode)
            EVals.Add(EStff / EMass)

        End Sub

        ' Binary exchange

        Private Sub ReadMatrixM(ByVal Path As String, ByVal DOF As Integer)

            Mass = New SymmetricMatrix(DOF)

            Dim r As BinaryReader = New BinaryReader(File.Open(Path, FileMode.Open))

            Dim n = r.ReadInt32()
            Dim e = r.ReadInt32()

            Dim row As Integer
            Dim col As Integer
            Dim val As Double

            For i = 0 To e - 1

                row = r.ReadInt32()
                col = r.ReadInt32()
                val = r.ReadDouble()
                'If col >= row Then

                'End If
                Mass(row, col) = val

            Next

            'Dim p As String = "C:\Users\Guillermo\Documents\Vogel tests\Aeroelasticity"
            'Dim f As Integer = 1
            'FileOpen(f, p & "\Mevals.txt", OpenMode.Output, OpenAccess.Write)
            'Print(f, Mass.__repr__())
            'FileClose(f)


        End Sub

        Private Sub ReadMatrixK(ByVal Path As String, ByVal DOF As Integer)

            Stiffness = New SymmetricMatrix(DOF)

            Dim r As BinaryReader = New BinaryReader(File.Open(Path, FileMode.Open))

            Dim n = r.ReadInt32()
            Dim e = r.ReadInt32()

            Dim row As Integer
            Dim col As Integer
            Dim val As Double

            For i = 0 To e - 1

                row = r.ReadInt32()
                col = r.ReadInt32()
                val = r.ReadDouble()
                'If col >= row Then

                'End If
                Stiffness(row, col) = val

            Next

            'Dim p As String = Path
            'Dim f As Integer = 1
            'FileOpen(f, p & "\Kevals.txt", OpenMode.Output, OpenAccess.Write)
            'Print(f, Stiffness.__repr__())
            'FileClose(f)

        End Sub

        ' Output

        Private Sub WriteShapes(ByVal DataBasePath As String)

            Dim sw As BinaryWriter = New BinaryWriter(File.Open(DataBasePath + "\x.bin", FileMode.Create))
            Dim vw As BinaryWriter = New BinaryWriter(File.Open(DataBasePath + "\e.bin", FileMode.Create))

            sw.Write(EShapes(0).RowCount)
            sw.Write(EShapes.Count)

            vw.Write(EVals.Count)
            vw.Write(1)

            For i = 0 To EShapes.Count - 1

                vw.Write(EVals(i))

                For j = 0 To EShapes(i).RowCount - 1

                    sw.Write(EShapes(i)(j))

                Next
            Next

            sw.Close()
            vw.Close()

        End Sub

    End Class

End Namespace


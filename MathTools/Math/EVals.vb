Imports Meta.Numerics.Matrices
Imports System.IO

Namespace MathLibrary.EVals

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

        ''' <summary>
        ''' Finds modes and stores them on the database
        ''' </summary>
        ''' <param name="DataBasePath">File where matrices are to be found and results are to be written</param>
        ''' <param name="DOF">Number of degrees of freedom</param>
        ''' <param name="MaxIter">Maximum number of inverse iterations</param>
        ''' <param name="NModes">Number of wanted modes</param>
        ''' <remarks>This solver uses the inverse iteation method with Gram-Smith orthogonalization to find eigen vectors and values of the system KX = vMX.
        ''' Algorithm makes extensive use of Pardiso to solve systems of algabraic equations at each refining step.</remarks>
        Public Sub Solve(ByVal DataBasePath As String, ByVal DOF As Integer, Optional ByVal MaxIter As Integer = 200, Optional ByVal NModes As Integer = 5)

            ReadMatrixM(DataBasePath + "\M.bin", DOF)
            ReadMatrixK(DataBasePath + "\K.bin", DOF)

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

            Dim p As String = "C:\Users\Guillermo\Documents\Vogel tests\Aeroelasticity"
            Dim f As Integer = 1
            FileOpen(f, p & "\Mevals.txt", OpenMode.Output, OpenAccess.Write)
            Print(f, Mass.__repr__())
            FileClose(f)


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

            Dim p As String = "C:\Users\Guillermo\Documents\Vogel tests\Aeroelasticity"
            Dim f As Integer = 1
            FileOpen(f, p & "\Kevals.txt", OpenMode.Output, OpenAccess.Write)
            Print(f, Stiffness.__repr__())
            FileClose(f)

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


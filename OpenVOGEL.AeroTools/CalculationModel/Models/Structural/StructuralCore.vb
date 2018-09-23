'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Elements
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes
Imports DotNumerics.LinearAlgebra

Namespace CalculationModel.Models.Structural

    ''' <summary>
    ''' The structural model containing nodes and elements
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralCore

        ''' <summary>
        ''' The structural nodes
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Nodes As New List(Of StructuralNode)

        ''' <summary>
        ''' The list of structural elements
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Elements As new List(Of BeamElement)

        ''' <summary>
        ''' The structural settings
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property StructuralSettings As New StructuralSettings

        ''' <summary>
        ''' The dynamic modes of the structure
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Modes As New List(Of Mode)

        ''' <summary>
        ''' The complete mass matrix (including constrained DOF)
        ''' </summary>
        Private M As SymmetricMatrix

        ''' <summary>
        ''' The complete stiffness matrix (including constrained static DOF)
        ''' </summary>
        Private K As SymmetricMatrix

        ''' <summary>
        ''' The complete damping matrix (including constrained static DOF)
        ''' </summary>
        Private C As SymmetricMatrix

        ''' <summary>
        ''' The number of degrees of freedom
        ''' </summary>
        Private DOF As Integer = 0

        ''' <summary>
        ''' Generates the structure global stiffness and mass matrices
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreateMatrices(ByVal Path As String, Optional ByVal PrintAsTxt As Boolean = False)

            '-----------------------------------------------------------------------------
            ' Generates structure mass and stiffness matrices
            '-----------------------------------------------------------------------------

            M = New SymmetricMatrix(6 * Nodes.Count)

            K = New SymmetricMatrix(6 * Nodes.Count)

            '-----------------------------------------------------------------------------
            ' Add element stiffnes
            '-----------------------------------------------------------------------------

            Dim ElementCount As Integer = 0

            For Each Element In Elements

                ElementCount += 1

                Element.GenerateGlobalMatrices()

                If PrintAsTxt Then

                    Try

                        Dim FileK As Integer = 1
                        FileOpen(FileK, Path & String.Format("\KG_{0}.txt", ElementCount), OpenMode.Output, OpenAccess.Write)
                        Print(FileK, String.Format("{0,12:F8};", Element.K.ToString))
                        FileClose(FileK)

                        Dim FileM As Integer = 2
                        FileOpen(FileM, Path & String.Format("\MG_{0}.txt", ElementCount), OpenMode.Output, OpenAccess.Write)
                        Print(FileM, String.Format("{0,12:F8};", Element.M.ToString))
                        FileClose(FileM)

                    Catch ex As Exception

                    End Try

                End If

                Dim BaseIndexA As Integer = 6 * Element.NodeA.Index
                Dim BaseIndexB As Integer = 6 * Element.NodeB.Index
                Dim r As Integer
                Dim c As Integer

                For i = 0 To 5

                    For j = 0 To 5

                        Dim rA = BaseIndexA + i
                        Dim cA = BaseIndexA + j
                        Dim rB = BaseIndexB + i
                        Dim cB = BaseIndexB + j

                        If cA >= rA Then
                            K(rA, cA) += Element.K(i, j)
                            M(rA, cA) += Element.M(i, j)
                        End If

                        If cB >= rA Then
                            K(rA, cB) += Element.K(i, j + 6)
                            M(rA, cB) += Element.M(i, j + 6)
                        End If

                        If cA >= rB Then
                            K(rB, cA) += Element.K(i + 6, j)
                            M(rB, cA) += Element.M(i + 6, j)
                        End If

                        If cB >= rB Then
                            K(rB, cB) += Element.K(i + 6, j + 6)
                            M(rB, cB) += Element.M(i + 6, j + 6)
                        End If
                    Next

                Next

            Next

            '-----------------------------------------------------------------------------
            ' Add nodal stiffness due to constrains (springs)
            '-----------------------------------------------------------------------------

            'For Each Node In Nodes

            '    For i = 0 To 5

            '        K(6 * Node.Index + i, 6 * Node.Index + i) += Node.Contrains.K(i)

            '    Next

            'Next

            If PrintAsTxt Then

                Try

                    Dim FileK As Integer = 1
                    FileOpen(FileK, Path & "\KG.txt", OpenMode.Output, OpenAccess.Write)
                    Print(FileK, K.ToString)
                    FileClose(FileK)

                    Dim FileM As Integer = 2
                    FileOpen(FileM, Path & "\MG.txt", OpenMode.Output, OpenAccess.Write)
                    Print(FileM, M.ToString)
                    FileClose(FileM)

                Catch ex As Exception

                End Try

            End If

        End Sub

        ''' <summary>
        ''' Calculates structure dynamic modes using Subspace iteration and/or Jacobi.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub FindModes(ByVal Path As String, ByVal LinkID As Integer)

            TestEigenValues()

            '-----------------------------------------------------------------------------
            ' Generate constrained matrices
            '-----------------------------------------------------------------------------

            Dim nDOF As Integer = 0

            For Each Node In Nodes
                For i = 0 To 5
                    If Not Node.Contrains.Fixed(i) Then
                        nDOF += 1
                    End If
                Next
            Next

            Dim _M_ As New SymmetricMatrix(nDOF)
            Dim _K_ As New SymmetricMatrix(nDOF)

            Dim r As Integer = -1
            Dim _r_ As Integer = -1

            For Each Node_i In Nodes
                For i = 0 To 5
                    r += 1
                    If Not Node_i.Contrains.Fixed(i) Then
                        _r_ += 1
                        Dim c As Integer = -1
                        Dim _c_ As Integer = -1
                        For Each Node_j In Nodes
                            For j = 0 To 5
                                c += 1
                                If Not Node_j.Contrains.Fixed(j) Then
                                    _c_ += 1
                                    If _c_ >= _r_ Then
                                        _M_(_r_, _c_) = M(r, c)
                                        _K_(_r_, _c_) = K(r, c)
                                    End If
                                End If
                            Next
                        Next
                    End If
                Next
            Next

            '-----------------------------------------------------------------------------
            ' Find eigen values using subspace iteration
            '-----------------------------------------------------------------------------

            Dim nModes As Integer = StructuralSettings.NumberOfModes
            Dim nSubspace As Integer = Math.Min(4 * StructuralSettings.NumberOfModes, nDOF)
            Dim EigenSolver As New EigenSystem
            Dim D As Vector = Nothing
            Dim V As Matrix = Nothing

            '/////////////////////////////////////
            ' Use Jacobi
            '/////////////////////////////////////
            'D = New Vector(nDOF)
            'V = New Matrix(nDOF, nDOF)
            'EigenSolver.Jacobi(_K_, _M_, V, D)
            '/////////////////////////////////////

            '/////////////////////////////////////
            ' Use subspace iteration
            '/////////////////////////////////////
            EigenSolver.SubspaceIteration(_M_, _K_, nSubspace, nModes, D, V)
            '/////////////////////////////////////

            '-----------------------------------------------------------------------------
            ' Convert data to modal info
            '-----------------------------------------------------------------------------

            Modes.Clear()

            For E = 0 To nModes - 1

                Dim Mode As New Mode(E)

                Mode.K = D(E)
                Mode.w = Math.Sqrt(Mode.K)
                Mode.M = 1.0
                Mode.Cc = 2 * Math.Sqrt(Mode.M * Mode.K)
                Mode.C = StructuralSettings.ModalDamping * Mode.Cc

                Dim n As Integer = -1

                For Each Node In Nodes
                    Dim ModalDisplacement As New NodalDisplacement
                    For i = 0 To 5
                        If Not Node.Contrains.Fixed(i) Then
                            n += 1
                            ModalDisplacement.Values(i) = V(n, E)
                        End If
                    Next
                    Mode.Shape.Add(ModalDisplacement)
                Next
                Modes.Add(Mode)

            Next

        End Sub

        ''' <summary>
        ''' Autotest that runs a couple of well known eigen-value examples.
        ''' </summary>
        Private Sub TestEigenValues()

            '-----------------------------------------------------------
            ' Test the Jacobi solver using a simple problem as reference
            '-----------------------------------------------------------

            Dim A As New SymmetricMatrix(5)
            Dim B As New SymmetricMatrix(5)

            For i = 0 To 4

                A(i, i) = i + 1
                B(i, i) = 1.0

                For j = 0 To 4

                    If j > i Then

                        A(i, j) = 1.0

                    End If

                Next

            Next

            Dim EigenSolver As New EigenSystem
            Dim D As Vector
            Dim V As Matrix

            D = New Vector(5)
            V = New Matrix(5, 5)
            EigenSolver.Jacobi(A, B, V, D)

            Dim L As New Vector(5)
            L(0) = 0.277696
            L(1) = 1.35663
            L(2) = 2.43474
            L(3) = 3.54039
            L(4) = 7.39054

            For i = 0 To 4
                If Math.Abs(L(i) - D(i)) > 0.00001 Then
                    Throw New Exception("Jacobi solver did not pass test")
                End If
            Next

            '-----------------------------------------------------------
            ' Try solving simple structural problem
            '-----------------------------------------------------------

            Dim Beam As New ConstantBeamElement(0)

            Beam.Basis.U.X = 1.0
            Beam.Basis.U.Y = 0.0
            Beam.Basis.U.Z = 0.0

            Beam.Basis.V.X = 0.0
            Beam.Basis.V.Y = 1.0
            Beam.Basis.V.Z = 0.0

            Beam.Basis.W.X = 0.0
            Beam.Basis.W.Y = 0.0
            Beam.Basis.W.Z = 1.0

            Beam.NodeA = New StructuralNode(0)
            Beam.NodeA.Position.X = 0.0
            Beam.NodeA.Position.Y = 0.0
            Beam.NodeA.Position.Z = 0.0
            Beam.NodeB = New StructuralNode(1)
            Beam.NodeB.Position.X = 1.0
            Beam.NodeB.Position.Y = 0.0
            Beam.NodeB.Position.Z = 0.0

            ' Basic flexional test

            Beam.Section.AE = 1000.0
            Beam.Section.EIy = 1.0
            Beam.Section.EIz = 1.0
            Beam.Section.GJ = 1000.0

            Beam.Section.m = 1.0
            Beam.Section.rIp = 1.0

            Beam.GenerateGlobalMatrices()

            Dim M As New SymmetricMatrix(6)
            Dim K As New SymmetricMatrix(6)

            For i = 0 To 5
                For j = i To 5
                    M(i, j) = Beam.M(i, j)
                    K(i, j) = Beam.K(i, j)
                Next
            Next

            D = New Vector(6)
            V = New Matrix(6, 6)
            EigenSolver.Jacobi(K, M, V, D)

            If Math.Abs(Math.Sqrt(D(1)) - 3.53273) > 0.00001 Then
                Throw New Exception("Structural solver did not pass basic test")
            End If

            If Math.Abs(Math.Sqrt(D(2)) - 34.80689) > 0.00001 Then
                Throw New Exception("Structural solver did not pass basic test")
            End If

            '-----------------------------------------------------------
            ' Check 90 degrees rotated beam
            '-----------------------------------------------------------

            Beam.Basis.U.X = 0.0
            Beam.Basis.U.Y = 1.0
            Beam.Basis.U.Z = 0.0

            Beam.Basis.V.X = -1.0
            Beam.Basis.V.Y = 0.0
            Beam.Basis.V.Z = 0.0

            Beam.Basis.W.X = 0.0
            Beam.Basis.W.Y = 0.0
            Beam.Basis.W.Z = 1.0

            Beam.NodeA.Position.X = 0.0
            Beam.NodeA.Position.Y = 0.0
            Beam.NodeA.Position.Z = 0.0

            Beam.NodeB.Position.X = 0.0
            Beam.NodeB.Position.Y = 1.0
            Beam.NodeB.Position.Z = 0.0

            Beam.GenerateGlobalMatrices()

            For i = 0 To 5
                For j = i To 5
                    M(i, j) = Beam.M(i, j)
                    K(i, j) = Beam.K(i, j)
                Next
            Next

            D = New Vector(6)
            V = New Matrix(6, 6)
            EigenSolver.Jacobi(K, M, V, D)

            If Math.Abs(Math.Sqrt(D(1)) - 3.53273) > 0.00001 Then
                Throw New Exception("Structural solver did not pass basic test")
            End If

            If Math.Abs(Math.Sqrt(D(2)) - 34.80689) > 0.00001 Then
                Throw New Exception("Structural solver did not pass basic test")
            End If

        End Sub

        ''' <summary>
        ''' Transfers the mode shape to the nodal displacement using the given scale factor.
        ''' </summary>
        ''' <param name="ModeIndex">The mode shape to be transferred.</param>
        ''' <param name="Scale">The amplitude of the displacemet relative to the modal shape.</param>
        Public Sub TransferModeShapeToNodes(ByVal ModeIndex As Integer, Optional ByVal Scale As Double = 1.0)

            For Each n As StructuralNode In Nodes

                n.Displacement.Dx = Scale * Modes(ModeIndex).Shape(n.Index).Dx
                n.Displacement.Dy = Scale * Modes(ModeIndex).Shape(n.Index).Dy
                n.Displacement.Dz = Scale * Modes(ModeIndex).Shape(n.Index).Dz
                n.Displacement.Rx = Scale * Modes(ModeIndex).Shape(n.Index).Rx
                n.Displacement.Ry = Scale * Modes(ModeIndex).Shape(n.Index).Ry
                n.Displacement.Rz = Scale * Modes(ModeIndex).Shape(n.Index).Rz

            Next

        End Sub

        ''' <summary>
        ''' Sets all nodal displacements to zero.
        ''' </summary>
        Public Sub ResetDisplacements()

            For Each n As StructuralNode In Nodes

                n.Displacement.Dx = 0.0
                n.Displacement.Dy = 0.0
                n.Displacement.Dz = 0.0
                n.Displacement.Rx = 0.0
                n.Displacement.Ry = 0.0
                n.Displacement.Rz = 0.0

            Next

        End Sub

    End Class

End Namespace

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

Imports System.IO
Imports Meta.Numerics.Matrices
Imports MathTools.MathLibrary.EigenValues
Imports AeroTools.CalculationModel.Models.Structural.Library

Namespace CalculationModel.Models.Structural

    ''' <summary>
    ''' Structural model
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralCore

        Public Nodes As New List(Of StructuralNode)
        Public Elements As New List(Of BeamElement)

        Private M As SparseSquareMatrix
        Private K As SparseSquareMatrix
        Private C As SparseSquareMatrix

        Public StructuralSettings As New StructuralSettings

        Private DOF As Integer = 0

        ''' <summary>
        ''' Structure dynamic modes
        ''' </summary>
        ''' <remarks></remarks>
        Public Modes As List(Of Mode)

        ''' <summary>
        ''' Generates the structure global stiffness and mass matrices
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CreateMatrices(ByVal Path As String, Optional ByVal asTXT As Boolean = False)

            ' Generates structure mass and stiffness matrices

            M = New SparseSquareMatrix(6 * Nodes.Count)
            K = New SparseSquareMatrix(6 * Nodes.Count)

            ' Add element stiffnes:

            For Each Element In Elements

                Element.GenerateGlobalMatrices()

                Dim BaseA As Integer = 6 * Element.NodeA.Index
                Dim BaseB As Integer = 6 * Element.NodeB.Index

                For i = 0 To 5

                    For j = 0 To 5

                        K(BaseA + i, BaseA + j) += Element.K(i, j)
                        M(BaseA + i, BaseA + j) += Element.M(i, j)

                        K(BaseA + i, BaseB + j) += Element.K(i, j + 6)
                        M(BaseA + i, BaseB + j) += Element.M(i, j + 6)

                        K(BaseB + i, BaseA + j) += Element.K(i + 6, j)
                        M(BaseB + i, BaseA + j) += Element.M(i + 6, j)

                        K(BaseB + i, BaseB + j) += Element.K(i + 6, j + 6)
                        M(BaseB + i, BaseB + j) += Element.M(i + 6, j + 6)

                    Next

                Next

            Next

            ' Add nodal stiffness due to constrains (springs):

            'For Each Node In Nodes

            '    For i = 0 To 5

            '        K(6 * Node.Index + i, 6 * Node.Index + i) += Node.Contrains.K(i)

            '    Next

            'Next

            If asTXT Then

                Try

                    Dim fK As Integer = 1
                    FileOpen(fK, Path & "\KG.txt", OpenMode.Output, OpenAccess.Write)

                    Dim fM As Integer = 2
                    FileOpen(fM, Path & "\MG.txt", OpenMode.Output, OpenAccess.Write)

                    Print(fK, String.Format("{0,12:F8};", K.__repr__))
                    Print(fM, String.Format("{0,12:F8};", M.__repr__))

                    FileClose(fK)
                    FileClose(fM)

                Catch ex As Exception

                End Try

            End If

        End Sub

        ''' <summary>
        ''' Writes constrained stiffness and mass matrices to binary files
        ''' </summary>
        ''' <param name="PathK"></param>
        ''' <param name="PathM"></param>
        ''' <remarks></remarks>
        Private Sub WriteConstrainedMatrices(ByVal PathK As String, ByVal PathM As String, Optional ByVal asTXT As Boolean = False)

            Dim fK As Integer = 1
            If asTXT Then FileOpen(fK, PathK & ".txt", OpenMode.Output, OpenAccess.Write)

            Dim fM As Integer = 2
            If asTXT Then FileOpen(fM, PathM & ".txt", OpenMode.Output, OpenAccess.Write)

            Dim rG As Integer = -1
            Dim cG As Integer = -1

            ' Write constrained matrices:

            Dim wK As BinaryWriter = New BinaryWriter(New FileStream(PathK, FileMode.OpenOrCreate, FileAccess.Write))
            Dim wM As BinaryWriter = New BinaryWriter(New FileStream(PathM, FileMode.OpenOrCreate, FileAccess.Write))

            'Reserve space for number of non ceros and dimension:

            wK.Write(0)
            wK.Write(0)
            Dim nK As Integer = 0

            wM.Write(0)
            wM.Write(0)
            Dim nM As Integer = 0

            Dim Row As Integer = -1

            For Each Node In Nodes

                For i = 0 To 5

                    rG += 1
                    cG = -1

                    If Not Node.Contrains.Fixed(i) Then

                        Row += 1
                        Dim Col As Integer = -1

                        For Each OtherNode In Nodes

                            For j = 0 To 5

                                cG += 1

                                If Not OtherNode.Contrains.Fixed(j) Then

                                    Col += 1

                                    If K(rG, cG) <> 0 Then
                                        wK.Write(Row)
                                        wK.Write(Col)
                                        wK.Write(K(rG, cG))
                                        nK += 1
                                    End If

                                    If M(rG, cG) <> 0 Then
                                        wM.Write(Row)
                                        wM.Write(Col)
                                        wM.Write(M(rG, cG))
                                        nM += 1
                                    End If

                                    If asTXT Then Print(fK, String.Format("{0,12:F8};", K(rG, cG)))
                                    If asTXT Then Print(fM, String.Format("{0,12:F8};", M(rG, cG)))

                                End If

                            Next

                        Next

                        If asTXT Then Print(fK, vbNewLine)
                        If asTXT Then Print(fM, vbNewLine)

                    End If

                Next

            Next

            wK.Seek(0, SeekOrigin.Begin)
            wK.Write(Row)   ' Dimension
            wK.Write(nK)    ' Written elements (non ceros).

            wM.Seek(0, SeekOrigin.Begin)
            wM.Write(Row)
            wM.Write(nM)

            DOF = Row + 1

            wK.Close()
            wM.Close()

            If asTXT Then FileClose(fK)
            If asTXT Then FileClose(fM)

        End Sub

        ''' <summary>
        ''' Calculates structure dynamic modes through inverse iteration method with Gram-Smith orthogonalization 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub FindModes(Optional ByVal Path As String = "C:\Users\Guillermo\Documents\Vogel tests\Aeroelasticity", Optional ByVal LinkID As Integer = 0)

            Dim PathM As String = String.Format("{0}\M{1}.bin", Path, LinkID)
            Dim PathK As String = String.Format("{0}\K{1}.bin", Path, LinkID)

            WriteConstrainedMatrices(PathK, PathM, False)

            Dim ESolver As EigenValuesSolver = New EigenValuesSolver

            ESolver.InverseIteration(Path, PathK, PathM, DOF, 100, StructuralSettings.NumberOfModes)

            'ESolver.SubspaceIteration(DOF, Quantity, Math.Max(4 * Quantity, 25), Path)

            ReadModes(Path)

        End Sub

        ''' <summary>
        ''' Read modes from specific path
        ''' </summary>
        ''' <param name="DataBasePath"></param>
        ''' <remarks></remarks>
        Private Sub ReadModes(ByVal DataBasePath As String)

            Modes = New List(Of Mode)

            Dim sr As New BinaryReader(File.Open(DataBasePath + "\x.bin", FileMode.Open)) ' Shapes reader
            Dim vr As New BinaryReader(File.Open(DataBasePath + "\e.bin", FileMode.Open)) ' Values reader

            Dim dof As Integer = sr.ReadInt32
            Dim nm As Integer = sr.ReadInt32

            vr.ReadInt32()
            vr.ReadInt32()

            For mindex = 0 To nm - 1

                Dim Mode As New Mode(mindex)
                Dim r As Integer = -1

                Mode.K = vr.ReadDouble
                Mode.w = Math.Sqrt(Mode.K)
                Mode.M = 1.0
                Mode.Cc = 2 * Math.Sqrt(Mode.M * Mode.K)
                Mode.C = StructuralSettings.ModalDamping * Mode.Cc

                For Each Node In Nodes

                    Dim ModalDisplacement As New NodalDisplacement

                    For i = 0 To 5

                        If Not Node.Contrains.Fixed(i) Then

                            r += 1

                            ModalDisplacement.Values(i) = sr.ReadDouble()

                        End If

                    Next

                    Mode.Shape.Add(ModalDisplacement)

                Next

                Modes.Add(Mode)

            Next

            sr.Close()
            vr.Close()

        End Sub

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

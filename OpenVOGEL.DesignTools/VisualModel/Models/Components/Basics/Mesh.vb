'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.MathTools.Algebra.CustomMatrices

Namespace VisualModel.Models.Components.Basics

    Public Class Mesh

        Public Nodes As New List(Of NodalPoint) ' Es la que se utiliza para el resto del calculo. Es igual a la original por la matriz de orientacion, mas un vector de translacion
        Public Panels As New List(Of Panel)
        Public Lattice As New List(Of LatticeSegment) ' Matriz de conexion de vórcies

        Public Sub Translate(ByVal Vector As Vector3)

            For Each Point In Nodes

                Point.Position.Add(Vector)

            Next

        End Sub

        Public Sub Rotate(ByVal ReferencePoint As Vector3, ByVal Ori As OrientationAngles)

            Dim M As New RotationMatrix
            M.Generate(Ori)

            For Each Point In Nodes

                Point.Position.Substract(ReferencePoint)
                Point.Position.Rotate(M)
                Point.Position.Add(ReferencePoint)

            Next

        End Sub

        Public Sub Rotate(ByVal ReferencePoint As Vector3, ByVal M As RotationMatrix)

            For Each Point In Nodes

                Point.Position.Substract(ReferencePoint)
                Point.Position.Rotate(M)
                Point.Position.Add(ReferencePoint)

            Next

        End Sub

        Public Sub Scale(ByVal Scale As Double)

            For Each Point In Nodes

                Point.Position.Scale(Scale)

            Next

        End Sub

        Public Sub Align()

        End Sub

        Public Sub GenerateLattice()

            If Panels.Count = 0 Then
                Exit Sub
            End If

            Try

                Lattice.Clear()

                ' Arma la matriz de conexiones de vortices:

                Dim N1 As Integer
                Dim N2 As Integer
                Dim Esta As Boolean

                Dim FirstSegment As New LatticeSegment

                FirstSegment.N1 = Panels(0).N1
                FirstSegment.N2 = Panels(0).N2

                Lattice.Add(FirstSegment)

                For i = 1 To Panels.Count

                    For k = 1 To 4

                        Select Case k
                            Case 1
                                N1 = Panels.Item(i - 1).N1
                                N2 = Panels.Item(i - 1).N2
                            Case 2
                                N1 = Panels.Item(i - 1).N2
                                N2 = Panels.Item(i - 1).N3
                            Case 3
                                N1 = Panels.Item(i - 1).N3
                                N2 = Panels.Item(i - 1).N4
                            Case 4
                                N1 = Panels.Item(i - 1).N4
                                N2 = Panels.Item(i - 1).N1
                        End Select

                        Esta = False

                        For m = 0 To Lattice.Count - 1

                            If Lattice.Item(m).N1 = N1 And Lattice.Item(m).N2 = N2 Then

                                Esta = True

                            ElseIf Lattice.Item(m).N1 = N2 And Lattice.Item(m).N2 = N1 Then

                                Esta = True

                            End If

                        Next

                        If Esta = False Then

                            Dim Segment As New LatticeSegment

                            Segment.N1 = N1
                            Segment.N2 = N2

                            Lattice.Add(Segment)

                        End If

                    Next

                Next

            Catch

                Throw New Exception("Error while generating lattice of segments.")

            End Try

        End Sub

    End Class

End Namespace

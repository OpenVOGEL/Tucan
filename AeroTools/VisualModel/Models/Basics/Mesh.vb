'Copyright (C) 2016 Guillermo Hazebrouck

Imports MathTools.Algebra.EuclideanSpace
Imports MathTools.Algebra.CustomMatrices

Namespace VisualModel.Models.Basics

    Public Class Mesh

        Public NodalPoints As New List(Of NodalPoint) ' Es la que se utiliza para el resto del calculo. Es igual a la original por la matriz de orientacion, mas un vector de translacion
        Public Panels As New List(Of Panel)
        Public Lattice As New List(Of LatticeSegment) ' Matriz de conexion de vórcies

        Public Sub Translate(ByVal Vector As EVector3)

            For Each Point In NodalPoints

                Point.Position.Add(Vector)

            Next

        End Sub

        Public Sub Rotate(ByVal ReferencePoint As EVector3, ByVal Ori As OrientationCoordinates)

            Dim M As New RotationMatrix
            M.Generate(Ori)

            For Each Point In NodalPoints

                Point.Position.Substract(ReferencePoint)
                Point.Position.Rotate(M)
                Point.Position.Add(ReferencePoint)

            Next

        End Sub

        Public Sub Rotate(ByVal ReferencePoint As EVector3, ByVal M As RotationMatrix)

            For Each Point In NodalPoints

                Point.Position.Substract(ReferencePoint)
                Point.Position.Rotate(M)
                Point.Position.Add(ReferencePoint)

            Next

        End Sub

        Public Sub Scale(ByVal Scale As Double)

            For Each Point In NodalPoints

                Point.Position.Scale(Scale)

            Next

        End Sub

        Public Sub Align()

        End Sub

        Public Sub GenerateLattice()

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

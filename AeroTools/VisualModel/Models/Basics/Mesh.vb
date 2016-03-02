'Copyright (C) 2016 Guillermo Hazebrouck

Imports MathTools.Algebra.EuclideanSpace
Imports MathTools.Algebra.CustomMatrices

Namespace VisualModel.Models.Basics

    Public Class Mesh

        Public NodalPoints As New List(Of NodalPoint) ' Es la que se utiliza para el resto del calculo. Es igual a la original por la matriz de orientacion, mas un vector de translacion
        Public Panels As New List(Of Panel)
        Public Vortices As New List(Of VortexSegment) ' Matriz de conexion de vórcies

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

        Public Sub GenerateVortices()

            Dim N1 As Integer
            Dim N2 As Integer
            Dim m As Integer
            Dim Found As Boolean

            Vortices.Clear()

            Dim VortexSegement As New VortexSegment

            VortexSegement.N1 = Panels.Item(0).N1
            VortexSegement.N2 = Panels.Item(0).N2

            Vortices.Add(VortexSegement)

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

                    Found = False

                    For m = 0 To Vortices.Count - 1

                        If Vortices.Item(m).N1 = N1 And Vortices.Item(m).N2 = N2 Then

                            Found = True

                        ElseIf Vortices.Item(m).N1 = N2 And Vortices.Item(m).N2 = N1 Then

                            Found = True

                        End If

                    Next

                    If Found = False Then

                        Dim Vortex As New VortexSegment

                        Vortex.N1 = N1
                        Vortex.N2 = N2

                        Vortices.Add(Vortex)

                    End If

                Next

            Next

        End Sub

    End Class

End Namespace

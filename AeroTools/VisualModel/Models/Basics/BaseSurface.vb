'Copyright (C) 2016 Guillermo Hazebrouck

Imports MathTools.Algebra.EuclideanSpace
Imports AeroTools.VisualModel.Interface
Imports SharpGL
Imports System.Xml

Namespace VisualModel.Models.Basics

    ''' <summary>
    ''' Basic definition of a base surface able to be operated and selected.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseSurface

        Implements IOperational
        Implements ISelectable

        Public Property Name As String
        Public Property Mesh As Mesh
        Public Property VisualProps As VisualizationProperties

        Public Property AccessPath As String
        Public Property IncludeInCalculation As Boolean = False
        Public Property LockContent As Boolean = True

        Public Property Position As New EVector3
        Public Property CenterOfRotation As New EVector3
        Public Property Orientation As New OrientationCoordinates
        Public Property Scales As Double = 1.0

        Public PressureRange As MathTools.LimitValues
        Public DisplacementRange As MathTools.LimitValues
        Public Property SerialNumber As String

#Region " Operations "

        Public Overridable Sub Translate(ByVal Vector As EVector3) Implements IOperational.Translate

            Mesh.Translate(Vector)

        End Sub

        Public Overridable Sub Orientate(ByVal Point As EVector3, ByVal Ori As OrientationCoordinates) Implements IOperational.Orientate

            Mesh.Rotate(Point, Ori)

        End Sub

        Public Overridable Sub Scale(ByVal Scale As Double) Implements IOperational.Scale

            Mesh.Scale(Scale)

        End Sub

        Public Overridable Sub Align(ByVal P1 As EVector3, ByVal P2 As EVector3, ByVal P3 As EVector3, ByVal P4 As EVector3) Implements IOperational.Align

            Mesh.Align()

        End Sub

#End Region

        ''' <summary>
        ''' Indicates if the surface is currently selected.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Selected As Boolean = False Implements ISelectable.Selected

        ''' <summary>
        ''' Unselects all nodal points.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UnselectAll() Implements ISelectable.UnselectAll

            For Each node In Mesh.NodalPoints
                node.Active = False
            Next

        End Sub

        Public Sub GenerateSerialNumber()

            Dim inst As Date = Now
            SerialNumber = Guid.NewGuid.ToString()

        End Sub

#Region " Meshing "

        ''' <summary>
        ''' Refresh the 3D model on a OpenGL control.
        ''' </summary>
        ''' <param name="gl"></param>
        ''' <param name="SelectionMode"></param>
        ''' <param name="ElementIndex"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub Refresh3DModel(ByRef gl As OpenGL, Optional ByVal SelectionMode As SelectionModes = SelectionModes.smNoSelection, Optional ByVal ElementIndex As Integer = 0)

        Public MustOverride Sub GenerateMesh()

        Public Function GenerateLattice() As Boolean

            Try

                Mesh.Vortices.Clear()

                ' Arma la matriz de conexiones de vortices:

                Dim N1 As Integer
                Dim N2 As Integer
                Dim Esta As Boolean

                Dim FirstVortex As New VortexSegment

                FirstVortex.N1 = Mesh.Panels(0).N1
                FirstVortex.N2 = Mesh.Panels(0).N2

                Mesh.Vortices.Add(FirstVortex)

                For i = 1 To Mesh.Panels.Count

                    For k = 1 To 4

                        Select Case k
                            Case 1
                                N1 = Mesh.Panels.Item(i - 1).N1
                                N2 = Mesh.Panels.Item(i - 1).N2
                            Case 2
                                N1 = Mesh.Panels.Item(i - 1).N2
                                N2 = Mesh.Panels.Item(i - 1).N3
                            Case 3
                                N1 = Mesh.Panels.Item(i - 1).N3
                                N2 = Mesh.Panels.Item(i - 1).N4
                            Case 4
                                N1 = Mesh.Panels.Item(i - 1).N4
                                N2 = Mesh.Panels.Item(i - 1).N1
                        End Select

                        Esta = False

                        For m = 0 To Mesh.Vortices.Count - 1

                            If Mesh.Vortices.Item(m).N1 = N1 And Mesh.Vortices.Item(m).N2 = N2 Then

                                Esta = True

                            ElseIf Mesh.Vortices.Item(m).N1 = N2 And Mesh.Vortices.Item(m).N2 = N1 Then

                                Esta = True

                            End If

                        Next

                        If Esta = False Then

                            Dim Vortex As New VortexSegment

                            Vortex.N1 = N1
                            Vortex.N2 = N2

                            Mesh.Vortices.Add(Vortex)

                        End If

                    Next

                Next

                Return True

            Catch ex As Exception

                Return False

            End Try

        End Function

#End Region

#Region " IO "

        Public MustOverride Sub WriteToXML(ByRef writes As XmlWriter)

        Public MustOverride Sub ReadFromXML(ByRef reader As XmlReader)

#End Region

    End Class

End Namespace
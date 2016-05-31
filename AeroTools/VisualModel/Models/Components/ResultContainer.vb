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

Imports SharpGL
Imports AeroTools.VisualModel.Interface
Imports AeroTools.VisualModel.Models.Basics
Imports AeroTools.CalculationModel.Models.Aero
Imports MathTools.Algebra.EuclideanSpace
Imports MathTools.Algebra.CustomMatrices
Imports System.IO
Imports AeroTools.CalculationModel.Settings
Imports System.Xml
Imports AeroTools.VisualModel.Environment.Colormaping
Imports MathTools

Namespace VisualModel.Models.Components

    ''' <summary>
    ''' Represents a multi-purpose surface for post-processing.
    ''' </summary>
    Public Class ResultContainer

        Inherits Surface

        Public Sub New()

            Mesh = New Mesh()
            VisualProperties = New VisualProperties(ComponentTypes.etFuselage)

        End Sub

        ''' <summary>
        ''' Clears the mesh.
        ''' </summary>
        Public Sub Clear()

            Mesh.Nodes.Clear()
            Mesh.Panels.Clear()
            Mesh.Lattice.Clear()

            _GeometryLoaded = False

        End Sub

        ''' <summary>
        ''' Location of this lattice in the database.
        ''' </summary>
        ''' <returns></returns>
        Public Property AccessPath As String

        ''' <summary>
        ''' Extreme values of the local pressure.
        ''' </summary>
        ''' <returns></returns>
        Public Property PressureRange As New LimitValues

        ''' <summary>
        ''' Maximum and minimum displacements.
        ''' </summary>
        ''' <returns></returns>
        Public Property DisplacementRange As New LimitValues

        Private _GeometryLoaded As Boolean = False

        Public ReadOnly Property GeometryLoaded As Boolean
            Get
                If _GeometryLoaded And Not IsNothing(Mesh.Nodes) And Not IsNothing(Mesh.Panels) Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Private _SelectedControlPoint As Integer = 0

        Public Sub SelectControlPoint(ByVal Punto As Integer)

            If Punto >= 1 And Punto <= Me.NumberOfPanels Then _SelectedControlPoint = Punto

        End Sub

#Region " Add elements "

        Public Overloads Sub AddNodalPoint(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)

            Dim Posicion As Integer = Mesh.Nodes.Count - 1

            Dim NodalPoint As New NodalPoint

            NodalPoint.ReferencePosition = New EVector3(X, Y, Z)
            NodalPoint.Position.X = X
            NodalPoint.Position.Y = Y
            NodalPoint.Position.Z = Z

            Me.Mesh.Nodes.Add(NodalPoint)

        End Sub

        Public Overloads Sub AddNodalPoint(ByVal Punto As EVector3, Optional ByVal Displacement As EVector3 = Nothing)

            Dim NodalPoint As New NodalPoint
            NodalPoint.ReferencePosition = New EVector3(Punto.X, Punto.Y, Punto.Z)
            NodalPoint.Position.Assign(Punto)
            If Not IsNothing(Displacement) Then
                NodalPoint.Displacement = New EVector3(Displacement)
                NodalPoint.Position.Add(Displacement)
            End If

            Me.Mesh.Nodes.Add(NodalPoint)

        End Sub

        Public Function AddPanel(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal N4 As Integer) As Integer

            Dim Panel As New Panel
            Panel.N1 = N1
            Panel.N2 = N2
            Panel.N3 = N3
            Panel.N4 = N4

            Mesh.Panels.Add(Panel)

            Return Mesh.Panels.Count

        End Function

        Public Function AddPanel(ByVal Panel As Panel) As Integer

            Mesh.Panels.Add(Panel)
            Return Mesh.Panels.Count

        End Function

        ''' <summary>
        ''' Loads the geometry from a calculation model lattice.
        ''' </summary>
        ''' <param name="Lattice"></param>
        Public Sub LoadFromLattice(ByVal Lattice As Lattice)

            For i = 0 To Lattice.Nodes.Count - 1
                AddNodalPoint(Lattice.Nodes(i).Position.X, Lattice.Nodes(i).Position.Y, Lattice.Nodes(i).Position.Z)
            Next

            For i = 0 To Lattice.VortexRings.Count - 1
                AddPanel(Lattice.VortexRings(i).Node(1).IndexL + 1, Lattice.VortexRings(i).Node(2).IndexL + 1, Lattice.VortexRings(i).Node(3).IndexL + 1, Lattice.VortexRings(i).Node(4).IndexL + 1)
            Next

            _GeometryLoaded = True

        End Sub

#End Region

#Region " 3D Functions "

        Public Overrides Sub Refresh3DModel(ByRef gl As OpenGL, Optional ByVal SelectionMode As SelectionModes = SelectionModes.smNoSelection, Optional ByVal ElementIndex As Integer = 0)

            'Version para OpenGL

            Dim i As Integer

            Dim Nodo As NodalPoint

            If VisualProperties.ShowSurface Then

                ' load homogeneous color:

                Dim cX As Double
                Dim cY As Double
                Dim cZ As Double

                If Not Selected Then

                    cX = VisualProperties.ColorSurface.R / 255
                    cY = VisualProperties.ColorSurface.G / 255
                    cZ = VisualProperties.ColorSurface.B / 255

                Else

                    ' default selected color is {255, 194, 14} (orange)

                    cX = 1
                    cY = 0.76078
                    cZ = 0.0549

                End If

                gl.Color(cX, cY, cZ, VisualProperties.Transparency)

                gl.InitNames()

                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etFuselage, ElementIndex, EntityTypes.etQuadPanel, 0)

                For i = 0 To NumberOfPanels - 1

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_TRIANGLES)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N1))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N2))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N3))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N3))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N4))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N1))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    gl.End()
                    gl.PopName()

                Next

            End If

            ' Show nodes:

            If VisualProperties.ShowNodes Then

                gl.InitNames()
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etFuselage, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(VisualProperties.SizeNodes)

                gl.Color(VisualProperties.ColorNodes.R / 255, VisualProperties.ColorNodes.G / 255, VisualProperties.ColorNodes.B / 255)

                For i = 0 To NumberOfNodes - 1

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_POINTS)
                    Nodo = Mesh.Nodes(i)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)
                    gl.End()
                    gl.PopName()

                Next

            End If

            ' Show lattice:

            If VisualProperties.ShowMesh Then

                gl.LineWidth(VisualProperties.ThicknessMesh)
                gl.Begin(OpenGL.GL_LINES)

                Dim Nodo1 As EVector3
                Dim Nodo2 As EVector3
                Dim Vector As EVector3
                Dim Carga As New EVector3

                gl.Color(VisualProperties.ColorMesh.R / 255, VisualProperties.ColorMesh.G / 255, VisualProperties.ColorMesh.B / 255)

                For i = 0 To NumberOfSegments - 1

                    Nodo1 = Mesh.Nodes(Mesh.Lattice(i).N1).Position
                    Nodo2 = Mesh.Nodes(Mesh.Lattice(i).N2).Position

                    gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                    gl.Vertex(Nodo2.X, Nodo2.Y, Nodo2.Z)

                Next

                gl.Color(VisualProperties.ColorVelocity.R / 255, VisualProperties.ColorVelocity.G / 255, VisualProperties.ColorVelocity.B / 255)

                If VisualProperties.ShowVelocityVectors Then

                    For i = 0 To NumberOfPanels - 1

                        Nodo1 = Mesh.Panels(i).ControlPoint
                        Vector = Mesh.Panels(i).LocalVelocity

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + VisualProperties.ScaleVelocity * Vector.X, Nodo1.Y + VisualProperties.ScaleVelocity * Vector.Y, Nodo1.Z + VisualProperties.ScaleVelocity * Vector.Z)

                    Next

                End If

                gl.Color(VisualProperties.ColorLoads.R / 255, VisualProperties.ColorLoads.G / 255, VisualProperties.ColorLoads.B / 255)

                If VisualProperties.ShowLoadVectors Then

                    For i = 0 To NumberOfPanels - 1

                        Nodo1 = Mesh.Panels(i).ControlPoint
                        Carga.Assign(Mesh.Panels(i).NormalVector)
                        Carga.Scale(VisualProperties.ScalePressure * Mesh.Panels(i).Cp * Mesh.Panels(i).Area)

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + Carga.X, Nodo1.Y + Carga.Y, Nodo1.Z + Carga.Z)

                    Next

                End If

                gl.End()

            End If

            If SelectionMode = SelectionModes.smNodePicking And _SelectedControlPoint >= 0 And _SelectedControlPoint < NumberOfPanels Then

                gl.PointSize(2 * VisualProperties.SizeNodes)
                gl.Color(VisualProperties.ColorNodes.R / 255, VisualProperties.ColorNodes.G / 255, VisualProperties.ColorNodes.B / 255)
                gl.Begin(OpenGL.GL_POINTS)

                gl.Vertex(Mesh.Panels(_SelectedControlPoint).ControlPoint.X, Mesh.Panels(_SelectedControlPoint).ControlPoint.Y, Mesh.Panels(_SelectedControlPoint).ControlPoint.Z)

                gl.End()

            End If

        End Sub

        ''' <summary>
        ''' Updates the position based on the reference nodal position and displacement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateDisplacement(Optional ByVal Scale As Double = 1.0)

            If NumberOfNodes > 0 Then

                For i = 0 To NumberOfNodes - 1

                    If (Not IsNothing(Mesh.Nodes(i).ReferencePosition)) And (Not IsNothing(Mesh.Nodes(i).Displacement)) Then

                        Mesh.Nodes(i).Position.X = Mesh.Nodes(i).ReferencePosition.X + Scale * Mesh.Nodes(i).Displacement.X
                        Mesh.Nodes(i).Position.Y = Mesh.Nodes(i).ReferencePosition.Y + Scale * Mesh.Nodes(i).Displacement.Y
                        Mesh.Nodes(i).Position.Z = Mesh.Nodes(i).ReferencePosition.Z + Scale * Mesh.Nodes(i).Displacement.Z

                    End If
                Next

            End If

        End Sub

        ''' <summary>
        ''' Obsolete sub.
        ''' </summary>
        Public Sub SearchForAdjacentPanels()

            ' Asigna los paneles adyacentes

            Dim N1 As Integer
            Dim N2 As Integer
            Dim m As Integer
            Dim p As Integer = 1

            Dim Ni1 As Integer
            Dim Ni2 As Integer
            Dim Nj1 As Integer
            Dim Nj2 As Integer

            Try

                For i = 0 To NumberOfPanels - 1

                    For k = 1 To 4

                        Select Case k
                            Case 1
                                N1 = Mesh.Panels(i).N1
                                N2 = Mesh.Panels(i).N2
                            Case 2
                                N1 = Mesh.Panels(i).N2
                                N2 = Mesh.Panels(i).N3
                            Case 3
                                N1 = Mesh.Panels(i).N3
                                N2 = Mesh.Panels(i).N4
                            Case 4
                                N1 = Mesh.Panels(i).N4
                                N2 = Mesh.Panels(i).N1
                        End Select

                        For m = 0 To Mesh.Lattice.Count - 1

                            If Mesh.Lattice(m).N1 = N1 And Mesh.Lattice(m).N2 = N2 Then

                                ' El panel adyacente 1 es el que sigue el mismo orden de nodos que el segmento

                                Mesh.Lattice(m).PanelAdyacente1 = Mesh.Panels(i)

                            ElseIf Mesh.Lattice(m).N1 = N2 And Mesh.Lattice(m).N2 = N1 Then

                                ' El panel adyacente 2 es el que sigue el orden de nodos opuesto al del segmento

                                Mesh.Lattice(m).PanelAdyacente2 = Mesh.Panels(i)

                            End If

                        Next

                    Next

                    For k = 1 To 4

                        Select Case k

                            Case 1

                                Ni1 = Mesh.Panels(i).N1
                                Ni2 = Mesh.Panels(i).N2

                                For j = 0 To NumberOfPanels - 1

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Mesh.Panels(j).N1
                                                    Nj2 = Mesh.Panels(j).N2
                                                Case 2
                                                    Nj1 = Mesh.Panels(j).N2
                                                    Nj2 = Mesh.Panels(j).N3
                                                Case 3
                                                    Nj1 = Mesh.Panels(j).N3
                                                    Nj2 = Mesh.Panels(j).N4
                                                Case 4
                                                    Nj1 = Mesh.Panels(j).N4
                                                    Nj2 = Mesh.Panels(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then

                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel1) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel1) = Sence.Positive

                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then

                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel1) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel1) = Sence.Negative

                                            End If

                                        Next

                                    End If

                                Next

                            Case 2

                                Ni1 = Mesh.Panels(i).N2
                                Ni2 = Mesh.Panels(i).N3

                                For j = 1 To Me.NumberOfPanels

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Mesh.Panels(j).N1
                                                    Nj2 = Mesh.Panels(j).N2
                                                Case 2
                                                    Nj1 = Mesh.Panels(j).N2
                                                    Nj2 = Mesh.Panels(j).N3
                                                Case 3
                                                    Nj1 = Mesh.Panels(j).N3
                                                    Nj2 = Mesh.Panels(j).N4
                                                Case 4
                                                    Nj1 = Mesh.Panels(j).N4
                                                    Nj2 = Mesh.Panels(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then

                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel2) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel2) = Sence.Positive

                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then

                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel2) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel2) = Sence.Negative

                                            End If

                                        Next

                                    End If

                                Next

                            Case 3

                                Ni1 = Mesh.Panels(i).N3
                                Ni2 = Mesh.Panels(i).N4

                                For j = 1 To Me.NumberOfPanels

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Mesh.Panels(j).N1
                                                    Nj2 = Mesh.Panels(j).N2
                                                Case 2
                                                    Nj1 = Mesh.Panels(j).N2
                                                    Nj2 = Mesh.Panels(j).N3
                                                Case 3
                                                    Nj1 = Mesh.Panels(j).N3
                                                    Nj2 = Mesh.Panels(j).N4
                                                Case 4
                                                    Nj1 = Mesh.Panels(j).N4
                                                    Nj2 = Mesh.Panels(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then
                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel3) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel3) = Sence.Positive
                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then
                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel3) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel3) = Sence.Negative
                                            End If

                                        Next

                                    End If

                                Next

                            Case 4

                                Ni1 = Mesh.Panels(i).N4
                                Ni2 = Mesh.Panels(i).N1

                                For j = 1 To Me.NumberOfPanels

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Mesh.Panels(j).N1
                                                    Nj2 = Mesh.Panels(j).N2
                                                Case 2
                                                    Nj1 = Mesh.Panels(j).N2
                                                    Nj2 = Mesh.Panels(j).N3
                                                Case 3
                                                    Nj1 = Mesh.Panels(j).N3
                                                    Nj2 = Mesh.Panels(j).N4
                                                Case 4
                                                    Nj1 = Mesh.Panels(j).N4
                                                    Nj2 = Mesh.Panels(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then
                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel4) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel4) = Sence.Positive
                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then
                                                Mesh.Panels(i).GetAdjacentPanel(AdjacentRing.Panel4) = Mesh.Panels(j)
                                                Mesh.Panels(i).GetSence(AdjacentRing.Panel4) = Sence.Negative
                                            End If

                                        Next

                                    End If

                                Next

                        End Select

                    Next

                Next

            Catch ex As Exception

                _GeometryLoaded = False

                Clear()

                MsgBox("Error while searching adjacent panels.")

            End Try

        End Sub

        Public Overrides Sub GenerateMesh()

            Throw New Exception("Cannot generate the mesh of a general surface.")

        End Sub

        ''' <summary>
        ''' Generates control points and normal vectors for each panel.
        ''' </summary>
        Public Sub GenerateControlPointsAndNormalVectors()

            Dim Nodo1 As EVector3
            Dim Nodo2 As EVector3
            Dim Nodo3 As EVector3
            Dim Nodo4 As EVector3

            Dim Vector1 As EVector3
            Dim Vector2 As EVector3
            Dim Vector3 As EVector3
            Dim Vector4 As EVector3

            Dim Diagonal1 As New EVector3
            Dim Diagonal2 As New EVector3

            For i = 0 To NumberOfPanels - 1

                If Mesh.Panels(i).IsTriangular Then

                    Nodo1 = Mesh.Nodes(Mesh.Panels(i).N1).Position
                    Nodo2 = Mesh.Nodes(Mesh.Panels(i).N2).Position
                    Nodo3 = Mesh.Nodes(Mesh.Panels(i).N3).Position

                    Vector1 = Nodo1.GetVectorToPoint(Nodo2)
                    Vector2 = Nodo2.GetVectorToPoint(Nodo3)
                    Vector3 = Nodo3.GetVectorToPoint(Nodo1)

                    Mesh.Panels(i).ControlPoint.X = (Nodo1.X + Nodo2.X + Nodo3.X) / 3
                    Mesh.Panels(i).ControlPoint.Y = (Nodo1.Y + Nodo2.Y + Nodo3.Y) / 3
                    Mesh.Panels(i).ControlPoint.Z = (Nodo1.Z + Nodo2.Z + Nodo3.Z) / 3

                    Diagonal1.X = Nodo2.X - Nodo1.X
                    Diagonal1.Y = Nodo2.Y - Nodo1.Y
                    Diagonal1.Z = Nodo2.Z - Nodo1.Z

                    Diagonal2.X = Nodo3.X - Nodo1.X
                    Diagonal2.Y = Nodo3.Y - Nodo1.Y
                    Diagonal2.Z = Nodo3.Z - Nodo1.Z

                    Mesh.Panels(i).NormalVector = Algebra.VectorProduct(Diagonal1, Diagonal2).NormalizedDirection
                    Mesh.Panels(i).Area = 0.5 * Algebra.VectorProduct(Vector1, Vector2).EuclideanNorm

                Else

                    Nodo1 = Mesh.Nodes(Mesh.Panels(i).N1).Position
                    Nodo2 = Mesh.Nodes(Mesh.Panels(i).N2).Position
                    Nodo3 = Mesh.Nodes(Mesh.Panels(i).N3).Position
                    Nodo4 = Mesh.Nodes(Mesh.Panels(i).N4).Position

                    Vector1 = Nodo1.GetVectorToPoint(Nodo2)
                    Vector2 = Nodo2.GetVectorToPoint(Nodo3)
                    Vector3 = Nodo3.GetVectorToPoint(Nodo4)
                    Vector4 = Nodo4.GetVectorToPoint(Nodo1)

                    Mesh.Panels(i).ControlPoint.X = 0.25 * (Nodo1.X + Nodo2.X + Nodo3.X + Nodo4.X)
                    Mesh.Panels(i).ControlPoint.Y = 0.25 * (Nodo1.Y + Nodo2.Y + Nodo3.Y + Nodo4.Y)
                    Mesh.Panels(i).ControlPoint.Z = 0.25 * (Nodo1.Z + Nodo2.Z + Nodo3.Z + Nodo4.Z)

                    Diagonal1.X = Nodo2.X - Nodo4.X
                    Diagonal1.Y = Nodo2.Y - Nodo4.Y
                    Diagonal1.Z = Nodo2.Z - Nodo4.Z

                    Diagonal2.X = Nodo3.X - Nodo1.X
                    Diagonal2.Y = Nodo3.Y - Nodo1.Y
                    Diagonal2.Z = Nodo3.Z - Nodo1.Z

                    Mesh.Panels(i).NormalVector = Algebra.VectorProduct(Diagonal1, Diagonal2).NormalizedDirection
                    Mesh.Panels(i).Area = 0.5 * Algebra.VectorProduct(Vector1, Vector2).EuclideanNorm + 0.5 * Algebra.VectorProduct(Vector3, Vector4).EuclideanNorm

                End If

                If Mesh.Panels(i).Reversed Then

                    Mesh.Panels(i).NormalVector.Scale(-1.0#)

                End If

            Next

        End Sub

#End Region

#Region " Posprocess UVLM "

        ''' <summary>
        ''' Finds the ragne of pressure over the surface.
        ''' </summary>
        ''' <param name="AbsoluteValue"></param>
        Public Sub FindPressureRange(Optional ByVal AbsoluteValue As Boolean = True)

            If Me.NumberOfPanels >= 0 And AbsoluteValue Then

                PressureRange.Maximum = Mesh.Panels(0).Cp
                PressureRange.Minimum = Mesh.Panels(0).Cp

                Dim Cp As Double

                For i = 0 To NumberOfPanels - 1

                    Cp = Mesh.Panels(i).Cp

                    If Mesh.Panels(i).IsSlender Then
                        Cp = Math.Abs(Cp)
                    End If

                    If Cp > PressureRange.Maximum Then PressureRange.Maximum = Cp
                    If Cp < PressureRange.Minimum Then PressureRange.Minimum = Cp

                Next

            End If

        End Sub

        ''' <summary>
        ''' Assignes an interpolated value to the nodal pressures (just for the colormap).
        ''' </summary>
        Public Sub DistributePressureOnNodes()

            ' Esta subrutina busca los paneles que rodea a un nodo y le asigna a ese nodo una presión promedio.

            Dim CpLocal As Double = 0.0#
            Dim nPanels As Integer

            If PressureRange.Maximum = 0 And PressureRange.Minimum = 0 Then FindPressureRange(True)

            For i = 0 To NumberOfNodes - 1

                CpLocal = 0.0#
                nPanels = 0

                For j = 0 To NumberOfPanels - 1

                    With Mesh.Panels(j)

                        If .N1 = i Or .N2 = i Or .N3 = i Or .N4 = i Then

                            nPanels = nPanels + 1

                            If .IsSlender Then
                                CpLocal = CpLocal + Math.Abs(.Cp)
                            Else
                                CpLocal = CpLocal + .Cp
                            End If

                        End If

                    End With

                Next

                ' Saca un promedio:

                If nPanels > 0 Then Mesh.Nodes(i).Pressure = CpLocal / nPanels

                ' Finalmente asigna un color:

                Mesh.Nodes(i).Color = Colormap.ScalarToColor(Math.Abs(Mesh.Nodes(i).Pressure), PressureRange.Maximum, PressureRange.Minimum)

            Next

        End Sub

        ''' <summary>
        ''' Updates the map with pressure.
        ''' </summary>
        Public Sub UpdateColormapWithPressure()

            For i = 0 To NumberOfNodes - 1

                Mesh.Nodes(i).Color = Colormap.ScalarToColor(Mesh.Nodes(i).Pressure, PressureRange.Maximum, PressureRange.Minimum)

            Next

        End Sub

        Public Sub FindDisplacementsRange()

            If NumberOfNodes >= 1 Then

                DisplacementRange.Maximum = Mesh.Nodes(0).Displacement.EuclideanNorm
                DisplacementRange.Minimum = DisplacementRange.Maximum

                Dim d As Double

                For i = 0 To NumberOfNodes - 1

                    d = Mesh.Nodes(i).Displacement.EuclideanNorm
                    If d > DisplacementRange.Maximum Then DisplacementRange.Maximum = d
                    If d < DisplacementRange.Minimum Then DisplacementRange.Minimum = d

                Next

            End If

        End Sub

        Public Sub UpdateColormapWithDisplacements()

            For i = 0 To NumberOfNodes - 1

                Mesh.Nodes(i).Color = Colormap.ScalarToColor(Mesh.Nodes(i).Displacement.EuclideanNorm, DisplacementRange.Maximum, DisplacementRange.Minimum)

            Next

        End Sub

        ''' <summary>
        ''' Obsolete sub.
        ''' </summary>
        Public Sub CalculateAerodynamiLoad(ByRef Force As EVector3, ByRef Moment As EVector3, ByRef Area As Double)

            Area = 0
            Force.SetToCero()
            Moment.SetToCero()

            Dim LocalF As New EVector3

            For Each Ring In Mesh.Panels

                Area += Ring.Area

                LocalF.X = Ring.Area * Ring.Cp * Ring.NormalVector.X
                LocalF.Y = Ring.Area * Ring.Cp * Ring.NormalVector.Y
                LocalF.Z = Ring.Area * Ring.Cp * Ring.NormalVector.Z

                Force.X += LocalF.X
                Force.Y += LocalF.Y
                Force.Z += LocalF.Z

                Moment.X += Ring.ControlPoint.Y * LocalF.Z - Ring.ControlPoint.Z * LocalF.Y
                Moment.Y += Ring.ControlPoint.Z * LocalF.X - Ring.ControlPoint.X * LocalF.Z
                Moment.Z += Ring.ControlPoint.X * LocalF.Y - Ring.ControlPoint.Y * LocalF.X

            Next

            Force.Scale(1 / Area)
            Moment.Scale(1 / Area)

        End Sub

#End Region

#Region " IO "

        Public Overrides Sub WriteToXML(ByRef writes As XmlWriter)

        End Sub

        Public Overrides Sub ReadFromXML(ByRef reader As XmlReader)

        End Sub

        Public Sub WriteToBinary()

            Dim bw As BinaryWriter = New BinaryWriter(New FileStream(AccessPath, FileMode.Create))

            bw.Write(NumberOfNodes)
            For Each Node In Me.Mesh.Nodes
                bw.Write(Node.Position.X)
                bw.Write(Node.Position.Y)
                bw.Write(Node.Position.Z)
            Next

            bw.Write(NumberOfPanels)
            For Each p In Mesh.Panels

                bw.Write(p.N1)
                bw.Write(p.N2)
                bw.Write(p.N3)
                bw.Write(p.N4)
                bw.Write(p.Circulation)
                bw.Write(p.Cp)
                bw.Write(p.LocalVelocity.X)
                bw.Write(p.LocalVelocity.Y)
                bw.Write(p.LocalVelocity.Z)

            Next

            bw.Close()

        End Sub

        Public Sub ReadFromBinary()

            If Not File.Exists(AccessPath) Then Throw New Exception("Result file could not have been found.")

            Dim br As BinaryReader = New BinaryReader(New FileStream(AccessPath, FileMode.Open))

            Dim n As Integer = br.ReadInt32()

            For i = 1 To n

                AddNodalPoint(br.ReadDouble, br.ReadDouble, br.ReadDouble)

            Next

            Mesh.Panels.Clear()

            n = br.ReadInt32()

            For i = 1 To n

                AddPanel(br.ReadInt32, br.ReadInt32, br.ReadInt32, br.ReadInt32)
                Mesh.Panels(i - 1).Circulation = br.ReadDouble
                Mesh.Panels(i - 1).Cp = br.ReadDouble
                Mesh.Panels(i - 1).LocalVelocity.X = br.ReadDouble
                Mesh.Panels(i - 1).LocalVelocity.Y = br.ReadDouble
                Mesh.Panels(i - 1).LocalVelocity.Z = br.ReadDouble

            Next

            br.Close()

            Mesh.GenerateLattice()

        End Sub

        ''' <summary>
        ''' Loads the data from a text file (using the access path).
        ''' </summary>
        ''' <returns></returns>
        Public Function LoadFromTextFile() As Boolean

            Try

                Dim Line As String

                FileOpen(25, AccessPath, OpenMode.Input, OpenAccess.Read)

                Line = LineInput(25)
                Me.Name = Line

                Line = LineInput(25)

                Line = LineInput(25)
                Dim NumeroDeNodos As Integer = CInt(Right(Line, 5))

                Line = LineInput(25)
                Dim NumeroDePaneles As Integer = CInt(Right(Line, 5))

                Do Until Trim(Line) = "## MATRICES"
                    Line = LineInput(25)
                Loop

                Line = LineInput(25) ' Lee el espacio

                Mesh.Nodes.Clear()
                Mesh.Panels.Clear()

                For i = 1 To NumeroDeNodos ' Comienza a leer la matriz de coordenadas

                    Line = LineInput(25)
                    AddNodalPoint(CDbl(Left(Line, 13)), CDbl(Mid(Line, 14, 12)), CDbl(Right(Line, 13)))

                Next

                For i = 1 To NumeroDePaneles ' Comienza a leer la matriz de conectividad

                    Line = LineInput(25)
                    AddPanel(CInt(Left(Line, 5)),
                                                                              CInt(Mid(Line, 6, 5)),
                                                                              CInt(Mid(Line, 11, 5)),
                                                                              CInt(Right(Line, 4)))
                Next

                FileClose(25)

                Me._GeometryLoaded = True

                Dim ErrorDeTamaño As Boolean = Not ((NumeroDeNodos = Mesh.Nodes.Count) And (NumeroDePaneles = Mesh.Panels.Count) Or Mesh.Nodes.Count >= 0 Or Mesh.Panels.Count >= 0)

                If ErrorDeTamaño Then

                    Clear()

                    Return False

                End If

                Mesh.GenerateLattice()

                MsgBox("Geometria cargada correctamente.", MsgBoxStyle.Information)

                Return True

            Catch ex1 As Exception

                _GeometryLoaded = False

                Clear()

                Try
                    FileClose(25)
                Catch

                End Try

                Return False

            End Try

        End Function

        Public Overrides Function Clone() As Surface

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
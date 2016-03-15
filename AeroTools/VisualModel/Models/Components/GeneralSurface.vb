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
Imports AeroTools.UVLM.Models.Aero
Imports MathTools.Algebra.EuclideanSpace
Imports MathTools.Algebra.CustomMatrices
Imports System.IO
Imports AeroTools.UVLM.Settings
Imports System.Xml
Imports AeroTools.VisualModel.Environment.Colormaping
Imports MathTools

Namespace VisualModel.Models.Components

    ''' <summary>
    ''' Represents a multi-purpose surface for post-processing.
    ''' </summary>
    Public Class GeneralSurface

        Inherits BaseSurface

        Public Sub New()

            Mesh = New Mesh()
            VisualProps = New VisualizationProperties(ComponentTypes.etBody)

        End Sub

        ''' <summary>
        ''' Clears the mesh.
        ''' </summary>
        Public Sub Clear()

            Mesh.NodalPoints.Clear()
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

        Public ReadOnly Property NumberOfSegments As Integer
            Get
                Return Mesh.Lattice.Count
            End Get
        End Property

        Public ReadOnly Property NumberOfNodes As Integer
            Get
                If Not IsNothing(Mesh.NodalPoints) Then
                    Return Mesh.NodalPoints.Count 'Me.FNN
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property NumberOfPanels As Integer
            Get
                If Not IsNothing(Mesh.Panels) Then
                    Return Mesh.Panels.Count 'Me.FNP
                Else
                    Return 0
                End If
            End Get
        End Property

        Private _GeometryLoaded As Boolean = False

        Public ReadOnly Property GeometryLoaded As Boolean
            Get
                If _GeometryLoaded And Not IsNothing(Mesh.NodalPoints) And Not IsNothing(Mesh.Panels) Then
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

#Region " Data properties "

        ''' <summary>
        ''' Position of a nodal point.
        ''' </summary>
        ''' <param name="Node"></param>
        ''' <returns></returns>
        Public Property NodalPosition(ByVal Node As Integer) As EVector3
            Get
                Dim EuNode As New EVector3

                If Node <= NumberOfNodes And Node > 0 Then
                    EuNode.X = Me.Mesh.NodalPoints.Item(Node - 1).Position.X 'Recordar que el nodo 1 corresponde al indice 0 en la matriz
                    EuNode.Y = Me.Mesh.NodalPoints.Item(Node - 1).Position.Y
                    EuNode.Z = Me.Mesh.NodalPoints.Item(Node - 1).Position.Z
                Else
                    EuNode.X = 0
                    EuNode.Y = 0
                    EuNode.Z = 0
                End If
                Return EuNode
            End Get
            Set(ByVal value As EVector3)
                If Node <= Me.Mesh.NodalPoints.Count Then
                    Me.Mesh.NodalPoints(Node).Position = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Nodal point.
        ''' </summary>
        ''' <param name="Node"></param>
        ''' <returns></returns>
        Public ReadOnly Property NodalPoint(ByVal Node As Integer) As NodalPoint
            Get
                Dim EuNode As New NodalPoint

                If Node <= NumberOfNodes And Node > 0 Then
                    Return Me.Mesh.NodalPoints.Item(Node - 1)
                Else
                    Return New NodalPoint
                End If

            End Get
        End Property

        ''' <summary>
        ''' Returns a clone of a panel.
        ''' </summary>
        ''' <param name="Node"></param>
        ''' <returns></returns>
        Public ReadOnly Property ClonedPanel(ByVal Node As Integer) As Panel
            Get
                Dim _QuadPanel As New Panel

                If Node <= Me.NumberOfPanels And Node > 0 Then

                    _QuadPanel.N1 = Mesh.Panels.Item(Node - 1).N1
                    _QuadPanel.N2 = Mesh.Panels.Item(Node - 1).N2
                    _QuadPanel.N3 = Mesh.Panels.Item(Node - 1).N3
                    _QuadPanel.N4 = Mesh.Panels.Item(Node - 1).N4

                    Return _QuadPanel

                Else

                    _QuadPanel.N1 = Mesh.Panels.Item(0).N1
                    _QuadPanel.N2 = Mesh.Panels.Item(0).N2
                    _QuadPanel.N3 = Mesh.Panels.Item(0).N3
                    _QuadPanel.N4 = Mesh.Panels.Item(0).N4

                    Return _QuadPanel

                End If
            End Get
        End Property

        ''' <summary>
        ''' Mesh panel.
        ''' </summary>
        ''' <param name="Node"></param>
        ''' <returns></returns>
        Public ReadOnly Property Panel(ByVal Node As Integer) As Panel
            Get
                If Node <= Me.NumberOfPanels And Node > 0 Then
                    Return Mesh.Panels.Item(Node - 1)
                Else
                    Return New Panel
                End If
            End Get
        End Property

        ''' <summary>
        ''' Lattice segment.
        ''' </summary>
        ''' <param name="SegmentNumber"></param>
        ''' <returns></returns>
        Public ReadOnly Property Segment(ByVal SegmentNumber As Integer) As LatticeSegment
            Get
                If SegmentNumber >= 1 And SegmentNumber <= NumberOfSegments Then
                    Return Mesh.Lattice.Item(SegmentNumber - 1)
                Else
                    Return New LatticeSegment
                End If
            End Get
        End Property

#End Region

#Region " Add elements "

        Public Overloads Sub AddNodalPoint(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)

            Dim Posicion As Integer = Mesh.NodalPoints.Count - 1

            Dim NodalPoint As New NodalPoint

            NodalPoint.ReferencePosition = New EVector3(X, Y, Z)
            NodalPoint.Position.X = X
            NodalPoint.Position.Y = Y
            NodalPoint.Position.Z = Z

            Me.Mesh.NodalPoints.Add(NodalPoint)

        End Sub

        Public Overloads Sub AddNodalPoint(ByVal Punto As EVector3, Optional ByVal Displacement As EVector3 = Nothing)

            Dim NodalPoint As New NodalPoint
            NodalPoint.ReferencePosition = New EVector3(Punto.X, Punto.Y, Punto.Z)
            NodalPoint.Position.Assign(Punto)
            If Not IsNothing(Displacement) Then
                NodalPoint.Displacement = New EVector3(Displacement)
                NodalPoint.Position.Add(Displacement)
            End If

            Me.Mesh.NodalPoints.Add(NodalPoint)

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

            If Me.VisualProps.ShowSurface Then

                ' load homogeneous color:
                Dim SColor As New EVector3
                If Not Selected Then
                    SColor.X = Me.VisualProps.ColorSurface.R / 255
                    SColor.Y = Me.VisualProps.ColorSurface.G / 255
                    SColor.Z = Me.VisualProps.ColorSurface.B / 255
                Else
                    ' default selected color is {255, 194, 14} (orange)
                    SColor.X = 1
                    SColor.Y = 0.76078
                    SColor.Z = 0.0549
                End If
                gl.Color(SColor.X, SColor.Y, SColor.Z, Me.VisualProps.Transparency)

                gl.InitNames()
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etBody, ElementIndex, EntityTypes.etQuadPanel, 0)

                For i = 1 To NumberOfPanels

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_TRIANGLES)

                    Nodo = NodalPoint((Panel(i).N1))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N2))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N3))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N3))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N4))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N1))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    gl.End()
                    gl.PopName()

                    'Dim p As EVector3 = Me.QuadPanel(i).ControlPoint
                    'Dim v As EVector3 = Me.QuadPanel(i).NormalVector

                    'gl.Begin(OpenGL.GL_LINES)
                    'gl.Color(1, 0.7, 0.05)
                    'gl.Vertex(p.X, p.Y, p.Z)
                    'gl.Vertex(p.X + v.X, p.Y + v.Y, p.Z + v.Z)
                    'gl.End()

                Next

            End If

            If SelectionMode = SelectionModes.smNodePicking Then

                gl.InitNames()
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etBody, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(VisualProps.SizeNodes)
                gl.Color(Me.VisualProps.ColorNodes.R / 255, Me.VisualProps.ColorNodes.G / 255, Me.VisualProps.ColorNodes.B / 255)

                For i = 1 To NumberOfNodes

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_POINTS)
                    Nodo = Me.NodalPoint(i)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)
                    gl.End()
                    gl.PopName()

                Next

            End If

            ' Genera el mallado:

            If Me.VisualProps.ShowMesh Then

                gl.LineWidth(VisualProps.ThicknessMesh)
                gl.Begin(OpenGL.GL_LINES)

                Dim Nodo1 As EVector3
                Dim Nodo2 As EVector3
                Dim Vector As EVector3
                Dim Carga As New EVector3

                gl.Color(Me.VisualProps.ColorMesh.R / 255, Me.VisualProps.ColorMesh.G / 255, Me.VisualProps.ColorMesh.B / 255)

                For i = 1 To NumberOfSegments

                    Nodo1 = Me.NodalPosition(Me.Segment(i).N1)
                    Nodo2 = Me.NodalPosition(Me.Segment(i).N2)

                    gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                    gl.Vertex(Nodo2.X, Nodo2.Y, Nodo2.Z)

                Next

                gl.Color(Me.VisualProps.ColorVelocity.R / 255, Me.VisualProps.ColorVelocity.G / 255, Me.VisualProps.ColorVelocity.B / 255)

                If VisualProps.ShowVelocityVectors Then

                    For i = 1 To NumberOfPanels

                        Nodo1 = Me.Panel(i).ControlPoint
                        Vector = Me.Panel(i).LocalVelocity

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + VisualProps.ScaleVelocity * Vector.X, Nodo1.Y + VisualProps.ScaleVelocity * Vector.Y, Nodo1.Z + VisualProps.ScaleVelocity * Vector.Z)

                    Next

                End If

                gl.Color(Me.VisualProps.ColorLoads.R / 255, Me.VisualProps.ColorLoads.G / 255, Me.VisualProps.ColorLoads.B / 255)

                If VisualProps.ShowLoadVectors Then

                    For i = 1 To NumberOfPanels

                        Nodo1 = Panel(i).ControlPoint
                        Carga.Assign(Panel(i).NormalVector)
                        Carga.Scale(VisualProps.ScalePressure * Panel(i).Cp * Panel(i).Area)

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + Carga.X, Nodo1.Y + Carga.Y, Nodo1.Z + Carga.Z)

                    Next

                End If

                gl.End()

            End If

            If _SelectedControlPoint >= 1 And _SelectedControlPoint <= Me.NumberOfPanels Then

                gl.PointSize(2 * Me.VisualProps.SizeNodes)
                gl.Color(VisualProps.ColorNodes.R / 255, VisualProps.ColorNodes.G / 255, VisualProps.ColorNodes.B / 255)
                gl.Begin(OpenGL.GL_POINTS)

                gl.Vertex(Me.Panel(_SelectedControlPoint).ControlPoint.X, Me.Panel(_SelectedControlPoint).ControlPoint.Y, Me.Panel(_SelectedControlPoint).ControlPoint.Z)

                gl.End()

            End If

        End Sub

        ''' <summary>
        ''' Position the model in relation to the reference position of the nodes (it won't work if there is no reference position). 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdatePosition()

            Dim M As New RotationMatrix

            M.Generate(Me.Orientation.ToRadians)

            If NumberOfNodes > 0 Then

                For i = 0 To NumberOfNodes - 1

                    If Not IsNothing(Me.Mesh.NodalPoints(i).ReferencePosition) Then

                        Me.Mesh.NodalPoints(i).Position.X = Me.Mesh.NodalPoints(i).ReferencePosition.X - Me.CenterOfRotation.X
                        Me.Mesh.NodalPoints(i).Position.Y = Me.Mesh.NodalPoints(i).ReferencePosition.Y - Me.CenterOfRotation.Y
                        Me.Mesh.NodalPoints(i).Position.Z = Me.Mesh.NodalPoints(i).ReferencePosition.Z - Me.CenterOfRotation.Z
                        Me.Mesh.NodalPoints(i).Position.Rotate(M)
                        Me.Mesh.NodalPoints(i).Position.Add(Me.Position.X + Me.CenterOfRotation.X,
                                                            Me.Position.Y + Me.CenterOfRotation.Y,
                                                            Me.Position.Y + Me.CenterOfRotation.Z)
                        Me.Mesh.NodalPoints(i).Position.Scale(SizeScale)

                    End If
                Next

            End If

        End Sub

        ''' <summary>
        ''' Updates the position based on the reference nodal position and displacement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateDisplacement(Optional ByVal Scale As Double = 1.0)

            If NumberOfNodes > 0 Then

                For i = 0 To NumberOfNodes - 1

                    If (Not IsNothing(Me.Mesh.NodalPoints(i).ReferencePosition)) And (Not IsNothing(Me.Mesh.NodalPoints(i).Displacement)) Then

                        Me.Mesh.NodalPoints(i).Position.X = Me.Mesh.NodalPoints(i).ReferencePosition.X + Scale * Me.Mesh.NodalPoints(i).Displacement.X
                        Me.Mesh.NodalPoints(i).Position.Y = Me.Mesh.NodalPoints(i).ReferencePosition.Y + Scale * Me.Mesh.NodalPoints(i).Displacement.Y
                        Me.Mesh.NodalPoints(i).Position.Z = Me.Mesh.NodalPoints(i).ReferencePosition.Z + Scale * Me.Mesh.NodalPoints(i).Displacement.Z

                    End If
                Next

            End If

        End Sub

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

                For i = 1 To Me.NumberOfPanels

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

                        For m = 0 To Me.Mesh.Lattice.Count - 1

                            If Mesh.Lattice(m).N1 = N1 And Mesh.Lattice(m).N2 = N2 Then

                                ' El panel adyacente 1 es el que sigue el mismo orden de nodos que el segmento
                                Me.Segment(m).PanelAdyacente1 = Me.Panel(i)

                            ElseIf Mesh.Lattice(m).N1 = N2 And Mesh.Lattice(m).N2 = N1 Then

                                ' El panel adyacente 2 es el que sigue el orden de nodos opuesto al del segmento
                                Me.Segment(m).PanelAdyacente2 = Me.Panel(i)

                            End If

                        Next

                    Next

                    For k = 1 To 4

                        Select Case k

                            Case 1

                                Ni1 = Me.Panel(i).N1
                                Ni2 = Me.Panel(i).N2

                                For j = 1 To Me.NumberOfPanels

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Me.Panel(j).N1
                                                    Nj2 = Me.Panel(j).N2
                                                Case 2
                                                    Nj1 = Me.Panel(j).N2
                                                    Nj2 = Me.Panel(j).N3
                                                Case 3
                                                    Nj1 = Me.Panel(j).N3
                                                    Nj2 = Me.Panel(j).N4
                                                Case 4
                                                    Nj1 = Me.Panel(j).N4
                                                    Nj2 = Me.Panel(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel1) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel1) = Sence.Positive
                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel1) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel1) = Sence.Negative
                                            End If

                                        Next

                                    End If

                                Next

                            Case 2

                                Ni1 = Me.Panel(i).N2
                                Ni2 = Me.Panel(i).N3

                                For j = 1 To Me.NumberOfPanels

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Me.Panel(j).N1
                                                    Nj2 = Me.Panel(j).N2
                                                Case 2
                                                    Nj1 = Me.Panel(j).N2
                                                    Nj2 = Me.Panel(j).N3
                                                Case 3
                                                    Nj1 = Me.Panel(j).N3
                                                    Nj2 = Me.Panel(j).N4
                                                Case 4
                                                    Nj1 = Me.Panel(j).N4
                                                    Nj2 = Me.Panel(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel2) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel2) = Sence.Positive
                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel2) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel2) = Sence.Negative
                                            End If

                                        Next

                                    End If

                                Next

                            Case 3

                                Ni1 = Me.Panel(i).N3
                                Ni2 = Me.Panel(i).N4

                                For j = 1 To Me.NumberOfPanels

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Me.Panel(j).N1
                                                    Nj2 = Me.Panel(j).N2
                                                Case 2
                                                    Nj1 = Me.Panel(j).N2
                                                    Nj2 = Me.Panel(j).N3
                                                Case 3
                                                    Nj1 = Me.Panel(j).N3
                                                    Nj2 = Me.Panel(j).N4
                                                Case 4
                                                    Nj1 = Me.Panel(j).N4
                                                    Nj2 = Me.Panel(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel3) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel3) = Sence.Positive
                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel3) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel3) = Sence.Negative
                                            End If

                                        Next

                                    End If

                                Next

                            Case 4

                                Ni1 = Me.Panel(i).N4
                                Ni2 = Me.Panel(i).N1

                                For j = 1 To Me.NumberOfPanels

                                    If j <> i Then

                                        For n = 1 To 4

                                            Select Case n

                                                Case 1
                                                    Nj1 = Me.Panel(j).N1
                                                    Nj2 = Me.Panel(j).N2
                                                Case 2
                                                    Nj1 = Me.Panel(j).N2
                                                    Nj2 = Me.Panel(j).N3
                                                Case 3
                                                    Nj1 = Me.Panel(j).N3
                                                    Nj2 = Me.Panel(j).N4
                                                Case 4
                                                    Nj1 = Me.Panel(j).N4
                                                    Nj2 = Me.Panel(j).N1

                                            End Select

                                            If Nj1 = Ni1 And Nj2 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel4) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel4) = Sence.Positive
                                            End If

                                            If Nj2 = Ni1 And Nj1 = Ni2 Then
                                                Me.Panel(i).ObtenerPanelAdyacente(AdjacentRing.Panel4) = Me.Panel(j)
                                                Me.Panel(i).ObtenerSentido(AdjacentRing.Panel4) = Sence.Negative
                                            End If

                                        Next

                                    End If

                                Next

                        End Select

                    Next

                Next

            Catch ex As Exception

                Me._GeometryLoaded = False

                Me.Clear()

                MsgBox("Error en el formato del archivo. Imposible de generar el mallado.")

            End Try

        End Sub

        Public Overrides Sub GenerateMesh()

            Throw New Exception("Cannot generate the mesh of a general surface.")

        End Sub

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

            For i = 1 To Me.NumberOfPanels

                If Panel(i).IsTriangular Then

                    Nodo1 = Me.NodalPosition(Me.Panel(i).N1)
                    Nodo2 = Me.NodalPosition(Me.Panel(i).N2)
                    Nodo3 = Me.NodalPosition(Me.Panel(i).N3)

                    Vector1 = Nodo1.GetVectorToPoint(Nodo2)
                    Vector2 = Nodo2.GetVectorToPoint(Nodo3)
                    Vector3 = Nodo3.GetVectorToPoint(Nodo1)

                    Me.Panel(i).ControlPoint.X = (Nodo1.X + Nodo2.X + Nodo3.X) / 3
                    Me.Panel(i).ControlPoint.Y = (Nodo1.Y + Nodo2.Y + Nodo3.Y) / 3
                    Me.Panel(i).ControlPoint.Z = (Nodo1.Z + Nodo2.Z + Nodo3.Z) / 3

                    Diagonal1.X = Nodo2.X - Nodo1.X
                    Diagonal1.Y = Nodo2.Y - Nodo1.Y
                    Diagonal1.Z = Nodo2.Z - Nodo1.Z

                    Diagonal2.X = Nodo3.X - Nodo1.X
                    Diagonal2.Y = Nodo3.Y - Nodo1.Y
                    Diagonal2.Z = Nodo3.Z - Nodo1.Z

                    Me.Panel(i).NormalVector = Algebra.VectorProduct(Diagonal1, Diagonal2).NormalizedDirection
                    Me.Panel(i).Area = 0.5 * Algebra.VectorProduct(Vector1, Vector2).EuclideanNorm

                Else

                    Nodo1 = Me.NodalPosition(Me.Panel(i).N1)
                    Nodo2 = Me.NodalPosition(Me.Panel(i).N2)
                    Nodo3 = Me.NodalPosition(Me.Panel(i).N3)
                    Nodo4 = Me.NodalPosition(Me.Panel(i).N4)

                    Vector1 = Nodo1.GetVectorToPoint(Nodo2)
                    Vector2 = Nodo2.GetVectorToPoint(Nodo3)
                    Vector3 = Nodo3.GetVectorToPoint(Nodo4)
                    Vector4 = Nodo4.GetVectorToPoint(Nodo1)

                    Me.Panel(i).ControlPoint.X = 0.25 * (Nodo1.X + Nodo2.X + Nodo3.X + Nodo4.X)
                    Me.Panel(i).ControlPoint.Y = 0.25 * (Nodo1.Y + Nodo2.Y + Nodo3.Y + Nodo4.Y)
                    Me.Panel(i).ControlPoint.Z = 0.25 * (Nodo1.Z + Nodo2.Z + Nodo3.Z + Nodo4.Z)

                    Diagonal1.X = Nodo2.X - Nodo4.X
                    Diagonal1.Y = Nodo2.Y - Nodo4.Y
                    Diagonal1.Z = Nodo2.Z - Nodo4.Z

                    Diagonal2.X = Nodo3.X - Nodo1.X
                    Diagonal2.Y = Nodo3.Y - Nodo1.Y
                    Diagonal2.Z = Nodo3.Z - Nodo1.Z

                    Me.Panel(i).NormalVector = Algebra.VectorProduct(Diagonal1, Diagonal2).NormalizedDirection
                    Me.Panel(i).Area = 0.5 * Algebra.VectorProduct(Vector1, Vector2).EuclideanNorm + 0.5 * Algebra.VectorProduct(Vector3, Vector4).EuclideanNorm

                End If

                If Me.Panel(i).Reversed Then
                    Me.Panel(i).NormalVector.Scale(-1.0#)
                End If

            Next

        End Sub

#End Region

#Region " Posprocess UVLM "

        Public Sub FindPressureRange(Optional ByVal AbsoluteValue As Boolean = True)

            If Me.NumberOfPanels >= 1 And AbsoluteValue Then

                PressureRange.Maximum = Panel(1).Cp
                PressureRange.Minimum = Panel(1).Cp

                Dim Cp As Double

                For i = 1 To NumberOfPanels

                    Cp = Panel(i).Cp

                    If Panel(i).IsSlender Then
                        Cp = Math.Abs(Cp)
                    End If

                    If Cp > PressureRange.Maximum Then PressureRange.Maximum = Cp
                    If Cp < PressureRange.Minimum Then PressureRange.Minimum = Cp

                Next

            End If

        End Sub

        Public Sub DistributePressureOnNodes()

            ' Esta subrutina busca los paneles que rodea a un nodo y le asigna a ese nodo una presión promedio.

            Dim CpLocal As Double = 0.0#
            Dim CantidadDePaneles As Integer

            If PressureRange.Maximum = 0 And PressureRange.Minimum = 0 Then FindPressureRange(True)

            For i = 1 To NumberOfNodes

                CpLocal = 0.0#
                CantidadDePaneles = 0

                For j = 1 To NumberOfPanels

                    With Panel(j)

                        If .N1 = i Or .N2 = i Or .N3 = i Or .N4 = i Then

                            CantidadDePaneles = CantidadDePaneles + 1

                            If .IsSlender Then
                                CpLocal = CpLocal + Math.Abs(.Cp)
                            Else
                                CpLocal = CpLocal + .Cp
                            End If

                        End If

                    End With

                Next

                ' Saca un promedio:

                If CantidadDePaneles > 0 Then Me.NodalPoint(i).Pressure = CpLocal / CantidadDePaneles

                ' Finalmente asigna un color:

                Me.NodalPoint(i).Color = Colormap.ScalarToColor(Math.Abs(Me.NodalPoint(i).Pressure), Me.PressureRange.Maximum, Me.PressureRange.Minimum)

            Next

        End Sub

        Public Sub UpdateColormapWithPressure()

            For i = 1 To Me.NumberOfNodes

                Me.NodalPoint(i).Color = Colormap.ScalarToColor(Me.NodalPoint(i).Pressure, Me.PressureRange.Maximum, Me.PressureRange.Minimum)

            Next

        End Sub

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

        Public Sub FindDisplacementsRange()

            If Me.NumberOfNodes >= 1 Then

                DisplacementRange.Maximum = Me.NodalPoint(1).Displacement.EuclideanNorm
                DisplacementRange.Minimum = DisplacementRange.Maximum

                Dim d As Double

                For i = 1 To Me.NumberOfNodes
                    d = Me.NodalPoint(i).Displacement.EuclideanNorm
                    If d > PressureRange.Maximum Then DisplacementRange.Maximum = d
                    If d < PressureRange.Minimum Then DisplacementRange.Minimum = d
                Next

            End If

        End Sub

        Public Sub UpdateColormapWithDisplacements()

            For i = 1 To Me.NumberOfNodes

                Me.NodalPoint(i).Color = Colormap.ScalarToColor(Math.Abs(Me.NodalPoint(i).Displacement.EuclideanNorm), Me.DisplacementRange.Maximum, Me.DisplacementRange.Minimum)

            Next

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
            For Each Node In Me.Mesh.NodalPoints
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

                Mesh.NodalPoints.Clear()
                Mesh.Panels.Clear()

                For i = 1 To NumeroDeNodos ' Comienza a leer la matriz de coordenadas

                    Line = LineInput(25)
                    Me.AddNodalPoint(CDbl(Left(Line, 13)), CDbl(Mid(Line, 14, 12)), CDbl(Right(Line, 13)))

                Next

                For i = 1 To NumeroDePaneles ' Comienza a leer la matriz de conectividad

                    Line = LineInput(25)
                    Me.AddPanel(CInt(Microsoft.VisualBasic.Left(Line, 5)),
                                                                              CInt(Mid(Line, 6, 5)),
                                                                              CInt(Mid(Line, 11, 5)),
                                                                              CInt(Right(Line, 4)))
                Next

                FileClose(25)

                Me._GeometryLoaded = True

                Dim ErrorDeTamaño As Boolean = Not ((NumeroDeNodos = Mesh.NodalPoints.Count) And (NumeroDePaneles = Mesh.Panels.Count) Or Mesh.NodalPoints.Count >= 0 Or Mesh.Panels.Count >= 0)

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

#End Region

    End Class

End Namespace
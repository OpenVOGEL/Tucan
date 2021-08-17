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

Imports System.Runtime.CompilerServices
Imports SharpGL
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.DesignTools.VisualModel.Models
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.ResultContainer
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace Tucan.Utility

    ''' <summary>
    ''' Implements the rendering of the surfaces declared in 
    ''' OpenVOGEL.DesignTools.VisualModel.Models.Components
    ''' NOTE: the rendering is based in the OpenGL compatibility mode
    ''' so it will not work for some graphics cards.
    ''' </summary>
    Module ModelRendering

        ''' <summary>
        ''' Redispatches the rendering method to the correct surface
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As Surface,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)

            If TypeOf This Is LiftingSurface Then
                Dim Surface As LiftingSurface = This
                Refresh3DModel(Surface, gl, ForSelection, ElementIndex)

            ElseIf TypeOf This Is Fuselage Then
                Dim Surface As Fuselage = This
                Refresh3DModel(Surface, gl, ForSelection, ElementIndex)

            ElseIf TypeOf This Is JetEngine Then
                Dim Surface As JetEngine = This
                Refresh3DModel(Surface, gl, ForSelection, ElementIndex)

            ElseIf TypeOf This Is Propeller Then
                Dim Surface As Propeller = This
                Refresh3DModel(Surface, gl, ForSelection, ElementIndex)

            ElseIf TypeOf This Is ImportedSurface Then
                Dim Surface As ImportedSurface = This
                Refresh3DModel(Surface, gl, ForSelection, ElementIndex)

            ElseIf TypeOf This Is ResultContainer Then
                Dim Surface As ResultContainer = This
                Refresh3DModel(Surface, gl, ForSelection, ElementIndex)
            End If

        End Sub

        ''' <summary>
        ''' Renders a ligting surface using SharpGL
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As LiftingSurface,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)

            With This

                Dim Code As Integer = 0

                ' Structure

                If .VisualProperties.VisualizationMode = VisualizationMode.Structural Then

                    gl.Color(0, 0, 0)

                    Code = Selection.GetSelectionCode(ComponentTypes.etLiftingSurface, ElementIndex, EntityTypes.etStructuralElement, 0)
                    Dim Code2 As Integer = Selection.GetSelectionCode(ComponentTypes.etLiftingSurface, ElementIndex, EntityTypes.etStructuralNode, 0)

                    For I = 0 To .StructuralPartition.Count - 1

                        If (I > 0) Then

                            ' Center of shear
                            '-------------------------------
                            gl.LineWidth(4.0)
                            gl.Color(0.0, 0.0, 0.0, 1.0)
                            gl.PushName(Code)
                            Code += 1
                            gl.Begin(OpenGL.GL_LINES)
                            With .StructuralPartition(I - 1).P
                                gl.Vertex(.X, .Y, .Z)
                            End With
                            With .StructuralPartition(I).P
                                gl.Vertex(.X, .Y, .Z)
                            End With
                            gl.End()
                            gl.PopName()

                            ' Center of mass
                            '-------------------------------
                            gl.Enable(OpenGL.GL_LINE_STIPPLE)
                            gl.LineStipple(2, &HC0F)

                            gl.LineWidth(3.0)
                            gl.Color(0.0, 0.0, 0.0, 1.0)
                            gl.Begin(OpenGL.GL_LINES)
                            With .StructuralPartition(I - 1).M
                                gl.Vertex(.X, .Y, .Z)
                            End With
                            With .StructuralPartition(I).M
                                gl.Vertex(.X, .Y, .Z)
                            End With
                            gl.End()

                            gl.LineWidth(1.0)
                            gl.Color(0.2, 1.0, 0.2, 1.0)
                            gl.Begin(OpenGL.GL_LINES)
                            With .StructuralPartition(I - 1).M
                                gl.Vertex(.X, .Y, .Z)
                            End With
                            With .StructuralPartition(I).M
                                gl.Vertex(.X, .Y, .Z)
                            End With
                            gl.End()

                            gl.Disable(OpenGL.GL_LINE_STIPPLE)

                        End If

                        gl.PointSize(4.0)
                        gl.Color(0.0, 0.0, 0.0, 1.0)

                        gl.Begin(OpenGL.GL_POINTS)

                        ' Structural node:

                        gl.PushName(Code2 + I + 1)

                        With .StructuralPartition(I).P
                            gl.Vertex(.X, .Y, .Z)
                        End With

                        gl.PopName()

                        ' Mass node:

                        gl.Color(0.0, 0.0, 0.0, 1.0)
                        gl.Vertex(.StructuralPartition(I).M.X,
                                  .StructuralPartition(I).M.Y,
                                  .StructuralPartition(I).M.Z)
                        gl.End()

                        If .VisualProperties.ShowLocalCoordinates Then

                            Dim Base As Base3 = .StructuralPartition(I).Basis
                            Dim l As Double = 0.2 * .StructuralPartition(I).LocalChord

                            gl.LineWidth(2.0)
                            gl.Begin(OpenGL.GL_LINES)

                            gl.Color(0.0, 1.0, 0.0, 1.0)

                            With .StructuralPartition(I).P

                                gl.Color(1.0, 0.0, 0.0, 1.0)
                                gl.Vertex(.X, .Y, .Z)
                                gl.Vertex(.X + l * Base.V.X, .Y + l * Base.V.Y, .Z + l * Base.V.Z)

                                gl.Color(0.0, 0.0, 1.0, 1.0)
                                gl.Vertex(.X, .Y, .Z)
                                gl.Vertex(.X + l * Base.W.X, .Y + l * Base.W.Y, .Z + l * Base.W.Z)

                            End With

                            gl.End()

                        End If

                    Next

                End If

                ' Panels:

                If .VisualProperties.VisualizationMode = VisualizationMode.Lattice Or
                   .VisualProperties.VisualizationMode = VisualizationMode.Structural Then

                    Dim Nodo As Vector3

                    gl.InitNames()
                    Code = Selection.GetSelectionCode(ComponentTypes.etLiftingSurface, ElementIndex, EntityTypes.etPanel, 0)
                    Dim p As Integer = 0

                    Dim PrimitiveColor As New Vector3
                    PrimitiveColor.X = .VisualProperties.ColorPrimitives.R / 255
                    PrimitiveColor.Y = .VisualProperties.ColorPrimitives.G / 255
                    PrimitiveColor.Z = .VisualProperties.ColorPrimitives.B / 255

                    Dim SurfaceColor As New Vector3
                    If Not .Active Then
                        SurfaceColor.X = .VisualProperties.ColorSurface.R / 255
                        SurfaceColor.Y = .VisualProperties.ColorSurface.G / 255
                        SurfaceColor.Z = .VisualProperties.ColorSurface.B / 255
                    Else
                        SurfaceColor.X = 1.0
                        SurfaceColor.Y = 0.8
                        SurfaceColor.Z = 0.0
                    End If

                    Dim Transparency As Double
                    If .VisualProperties.VisualizationMode = VisualizationMode.Lattice Then
                        Transparency = .VisualProperties.Transparency
                    ElseIf .VisualProperties.VisualizationMode = VisualizationMode.Structural Then
                        Transparency = 0.4
                    End If

                    Dim n As Integer = 0

                    For Each Panel In .Mesh.Panels

                        gl.PushName(Code)
                        Code += 1
                        n += 1

                        gl.Begin(OpenGL.GL_TRIANGLES)

                        If Panel.Active Then

                            gl.Color(1.0, 0.0, 0.5)

                        ElseIf Panel.IsPrimitive And .VisualProperties.ShowPrimitives Then

                            gl.Color(PrimitiveColor.X, PrimitiveColor.Y, PrimitiveColor.Z, Transparency)

                        Else

                            If .Active AndAlso This.OnCurrentRegion(n) Then
                                gl.Color(SurfaceColor.X, 0.5 * SurfaceColor.Y, 0.5 * SurfaceColor.Z, Transparency)
                            Else
                                gl.Color(SurfaceColor.X, SurfaceColor.Y, SurfaceColor.Z, Transparency)
                            End If

                        End If

                        ' First triangle:

                        Nodo = .Mesh.Nodes(Panel.N1).Position
                        gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                        Nodo = .Mesh.Nodes(Panel.N2).Position
                        gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                        Nodo = .Mesh.Nodes(Panel.N3).Position
                        gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                        ' Second triangle:

                        If Not Panel.IsTriangular Then

                            Nodo = .Mesh.Nodes(Panel.N3).Position
                            gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                            Nodo = .Mesh.Nodes(Panel.N4).Position
                            gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                            Nodo = .Mesh.Nodes(Panel.N1).Position
                            gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                        End If

                        If .Symmetric Then

                            ' First triangle:

                            Nodo = .Mesh.Nodes(Panel.N1).Position
                            gl.Vertex(Nodo.X, -Nodo.Y, Nodo.Z)

                            Nodo = .Mesh.Nodes(Panel.N2).Position
                            gl.Vertex(Nodo.X, -Nodo.Y, Nodo.Z)

                            Nodo = .Mesh.Nodes(Panel.N3).Position
                            gl.Vertex(Nodo.X, -Nodo.Y, Nodo.Z)

                            ' Second triangle:

                            If Not Panel.IsTriangular Then

                                Nodo = .Mesh.Nodes(Panel.N3).Position
                                gl.Vertex(Nodo.X, -Nodo.Y, Nodo.Z)

                                Nodo = .Mesh.Nodes(Panel.N4).Position
                                gl.Vertex(Nodo.X, -Nodo.Y, Nodo.Z)

                                Nodo = .Mesh.Nodes(Panel.N1).Position
                                gl.Vertex(Nodo.X, -Nodo.Y, Nodo.Z)

                            End If

                        End If

                        gl.End()
                        gl.PopName()

                    Next

                End If

                ' Nodes:

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etLiftingSurface, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(.VisualProperties.SizeNodes)
                gl.Color(.VisualProperties.ColorNodes.R / 255,
                         .VisualProperties.ColorNodes.G / 255,
                         .VisualProperties.ColorNodes.B / 255)

                For Each Node In .Mesh.Nodes

                    If ForSelection Or Node.Active Then

                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_POINTS)
                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)
                        If .Symmetric Then
                            gl.Vertex(Node.Position.X, -Node.Position.Y, Node.Position.Z)
                        End If
                        gl.End()
                        gl.PopName()

                    End If

                Next

                ' Create lattice:

                If ForSelection Or
                    .VisualProperties.VisualizationMode = VisualizationMode.Lattice Or
                    .VisualProperties.VisualizationMode = VisualizationMode.Structural Then

                    Dim SurfaceColor As New Vector3
                    SurfaceColor.X = 0.75
                    SurfaceColor.Y = 0.75
                    SurfaceColor.Z = 0.75
                    Dim Thickness As Double = 1.0

                    If (.VisualProperties.VisualizationMode = VisualizationMode.Lattice) Then
                        SurfaceColor.X = .VisualProperties.ColorMesh.R / 255
                        SurfaceColor.Y = .VisualProperties.ColorMesh.G / 255
                        SurfaceColor.Z = .VisualProperties.ColorMesh.B / 255
                        Thickness = .VisualProperties.ThicknessMesh
                    End If

                    If ForSelection Or .VisualProperties.ShowMesh Then

                        gl.InitNames()

                        Dim Nodo1 As Vector3
                        Dim Nodo2 As Vector3

                        gl.LineWidth(Thickness)
                        gl.Color(SurfaceColor.X, SurfaceColor.Y, SurfaceColor.Z)

                        Code = Selection.GetSelectionCode(ComponentTypes.etLiftingSurface, ElementIndex, EntityTypes.etSegment, 0)

                        For Each Segment In .Mesh.Lattice

                            gl.PushName(Code)
                            Code += 1

                            gl.Begin(OpenGL.GL_LINES)
                            Nodo1 = .Mesh.Nodes(Segment.N1).Position
                            Nodo2 = .Mesh.Nodes(Segment.N2).Position

                            gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                            gl.Vertex(Nodo2.X, Nodo2.Y, Nodo2.Z)

                            If .Symmetric Then

                                gl.Begin(OpenGL.GL_LINES)
                                Nodo1 = .Mesh.Nodes(Segment.N1).Position
                                Nodo2 = .Mesh.Nodes(Segment.N2).Position

                                gl.Vertex(Nodo1.X, -Nodo1.Y, Nodo1.Z)
                                gl.Vertex(Nodo2.X, -Nodo2.Y, Nodo2.Z)

                            End If

                            gl.End()
                            gl.PopName()

                        Next

                    End If

                End If

                ' Show local coordinates:

                If .VisualProperties.ShowLocalCoordinates Then

                    gl.LineWidth(2.0)
                    gl.Begin(OpenGL.GL_LINES)

                    gl.Color(0.0, 1.0, 0.0)
                    gl.Vertex(.LocalOrigin.X, .LocalOrigin.Y, .LocalOrigin.Z)
                    gl.Vertex(.MainDirections.U.X, .MainDirections.U.Y, .MainDirections.U.Z)

                    gl.Color(1.0, 0.0, 0.0)
                    gl.Vertex(.LocalOrigin.X, .LocalOrigin.Y, .LocalOrigin.Z)
                    gl.Vertex(.MainDirections.V.X, .MainDirections.V.Y, .MainDirections.V.Z)

                    gl.Color(0.0, 0.0, 1.0)
                    gl.Vertex(.LocalOrigin.X, .LocalOrigin.Y, .LocalOrigin.Z)
                    gl.Vertex(.MainDirections.W.X, .MainDirections.W.Y, .MainDirections.W.Z)

                    gl.End()

                End If

                ' Normals:

                If .VisualProperties.ShowNormalVectors Then

                    gl.Begin(OpenGL.GL_LINES)

                    gl.Color(.VisualProperties.ColorPositiveLoad.R / 255,
                             .VisualProperties.ColorPositiveLoad.G / 255,
                             .VisualProperties.ColorPositiveLoad.B / 255)

                    For Each Panel In .Mesh.Panels
                        gl.Vertex(Panel.ControlPoint.X, Panel.ControlPoint.Y, Panel.ControlPoint.Z)
                        gl.Vertex(Panel.ControlPoint.X + Panel.NormalVector.X,
                                  Panel.ControlPoint.Y + Panel.NormalVector.Y,
                                  Panel.ControlPoint.Z + Panel.NormalVector.Z)

                    Next

                    gl.End()

                End If

            End With

        End Sub

        ''' <summary>
        ''' Renders a fuselage using SharpGL
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As Fuselage,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)

            With This

                Dim Code As Integer = 0

                If ForSelection Or .VisualProperties.ShowSurface Then

                    ' Load homogeneous color:

                    Dim SurfaceColor As New Vector3

                    If Not .Active Then
                        SurfaceColor.X = .VisualProperties.ColorSurface.R / 255
                        SurfaceColor.Y = .VisualProperties.ColorSurface.G / 255
                        SurfaceColor.Z = .VisualProperties.ColorSurface.B / 255
                    Else
                        SurfaceColor.X = 1.0
                        SurfaceColor.Y = 0.8
                        SurfaceColor.Z = 0.0
                    End If

                    gl.InitNames()
                    Code = Selection.GetSelectionCode(ComponentTypes.etFuselage, ElementIndex, EntityTypes.etPanel, 0)

                    Dim Nodo As NodalPoint

                    For Each Panel In .Mesh.Panels

                        gl.PushName(Code)
                        Code += 1

                        gl.Begin(OpenGL.GL_TRIANGLES)

                        If Panel.Active Then
                            gl.Color(1.0, 0.0, 0.5)
                        Else
                            gl.Color(SurfaceColor.X, SurfaceColor.Y, SurfaceColor.Z, .VisualProperties.Transparency)
                        End If

                        Nodo = .Mesh.Nodes(Panel.N1)
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes(Panel.N2)
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes(Panel.N3)
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        If Not Panel.IsTriangular Then

                            Nodo = .Mesh.Nodes(Panel.N3)
                            gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                            Nodo = .Mesh.Nodes(Panel.N4)
                            gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                            Nodo = .Mesh.Nodes(Panel.N1)
                            gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        End If

                        gl.End()
                        gl.PopName()

                    Next

                End If

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etFuselage, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(.VisualProperties.SizeNodes)
                gl.Color(.VisualProperties.ColorNodes.R / 255,
                         .VisualProperties.ColorNodes.G / 255,
                         .VisualProperties.ColorNodes.B / 255)

                For Each Node In .Mesh.Nodes

                    If ForSelection Or Node.Active Then
                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_POINTS)
                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)
                        gl.End()
                        gl.PopName()
                    End If

                Next

                ' Represent lattice:

                If .VisualProperties.ShowMesh Then

                    gl.InitNames()
                    Code = Selection.GetSelectionCode(ComponentTypes.etFuselage, ElementIndex, EntityTypes.etSegment, 0)

                    gl.LineWidth(.VisualProperties.ThicknessMesh)

                    Dim Node1 As Vector3
                    Dim Node2 As Vector3

                    gl.Color(.VisualProperties.ColorMesh.R / 255,
                             .VisualProperties.ColorMesh.G / 255,
                             .VisualProperties.ColorMesh.B / 255)

                    If .Mesh.Nodes.Count > 0 Then

                        For Each Segment In .Mesh.Lattice

                            Node1 = .Mesh.Nodes(Segment.N1).Position
                            Node2 = .Mesh.Nodes(Segment.N2).Position

                            gl.PushName(Code)
                            Code += 1
                            gl.Begin(OpenGL.GL_LINES)
                            gl.Vertex(Node1.X, Node1.Y, Node1.Z)
                            gl.Vertex(Node2.X, Node2.Y, Node2.Z)
                            gl.End()
                            gl.PopName()

                        Next

                    End If

                End If

                ' Normals:

                If .VisualProperties.ShowNormalVectors Then

                    gl.Begin(OpenGL.GL_LINES)

                    gl.Color(.VisualProperties.ColorPositiveLoad.R / 255,
                             .VisualProperties.ColorPositiveLoad.G / 255,
                             .VisualProperties.ColorPositiveLoad.B / 255)

                    For Each Panel In .Mesh.Panels
                        gl.Vertex(Panel.ControlPoint.X, Panel.ControlPoint.Y, Panel.ControlPoint.Z)
                        gl.Vertex(Panel.ControlPoint.X + Panel.NormalVector.X,
                              Panel.ControlPoint.Y + Panel.NormalVector.Y,
                              Panel.ControlPoint.Z + Panel.NormalVector.Z)

                    Next

                    gl.End()

                End If

            End With

        End Sub

        ''' <summary>
        ''' Renders a nacelle using SharpGL
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As JetEngine,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)
            With This

                Dim Code As Integer = 0

                Dim Nodo As NodalPoint

                If .VisualProperties.ShowSurface Then

                    Dim SurfaceColor As New Vector3

                    If Not .Active Then
                        SurfaceColor.X = .VisualProperties.ColorSurface.R / 255
                        SurfaceColor.Y = .VisualProperties.ColorSurface.G / 255
                        SurfaceColor.Z = .VisualProperties.ColorSurface.B / 255
                    Else
                        SurfaceColor.X = 1.0
                        SurfaceColor.Y = 0.8
                        SurfaceColor.Z = 0.0
                    End If

                    gl.Color(SurfaceColor.X, SurfaceColor.Y, SurfaceColor.Z, .VisualProperties.Transparency)

                    gl.InitNames()
                    Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etPanel, 0)

                    For i = 0 To .NumberOfPanels - 1

                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_TRIANGLES)

                        Dim Panel As Panel = .Mesh.Panels(i)

                        Nodo = .Mesh.Nodes((Panel.N1))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N2))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N3))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N3))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N4))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N1))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        gl.End()
                        gl.PopName()

                    Next

                End If

                ' Nodes:

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(.VisualProperties.SizeNodes)
                gl.Color(.VisualProperties.ColorNodes.R / 255,
                         .VisualProperties.ColorNodes.G / 255,
                         .VisualProperties.ColorNodes.B / 255)

                For Each Node In .Mesh.Nodes

                    If ForSelection Or Node.Active Then

                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_POINTS)
                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)
                        gl.End()
                        gl.PopName()

                    End If

                Next

                ' Segments:

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etSegment, 0)

                If ForSelection Or .VisualProperties.ShowMesh Then

                    gl.LineWidth(.VisualProperties.ThicknessMesh)

                    Dim Node1 As Vector3
                    Dim Node2 As Vector3

                    gl.Color(.VisualProperties.ColorMesh.R / 255,
                             .VisualProperties.ColorMesh.G / 255,
                             .VisualProperties.ColorMesh.B / 255)

                    For Each Segment In .Mesh.Lattice

                        Node1 = .Mesh.Nodes(Segment.N1).Position
                        Node2 = .Mesh.Nodes(Segment.N2).Position

                        gl.Begin(OpenGL.GL_LINES)
                        gl.Vertex(Node1.X, Node1.Y, Node1.Z)
                        gl.Vertex(Node2.X, Node2.Y, Node2.Z)
                        gl.End()

                    Next

                End If

                ' Normals:

                If .VisualProperties.ShowNormalVectors Then

                    gl.Begin(OpenGL.GL_LINES)

                    gl.Color(.VisualProperties.ColorPositiveLoad.R / 255,
                             .VisualProperties.ColorPositiveLoad.G / 255,
                             .VisualProperties.ColorPositiveLoad.B / 255)

                    For Each Panel In .Mesh.Panels
                        gl.Vertex(Panel.ControlPoint.X, Panel.ControlPoint.Y, Panel.ControlPoint.Z)
                        gl.Vertex(Panel.ControlPoint.X + Panel.NormalVector.X,
                                  Panel.ControlPoint.Y + Panel.NormalVector.Y,
                                  Panel.ControlPoint.Z + Panel.NormalVector.Z)

                    Next

                    gl.End()

                End If

            End With

        End Sub

        ''' <summary>
        ''' Renders a propeller using SharpGL
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As Propeller,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)
            With This

                Dim Code As Integer = 0

                Dim Nodo As NodalPoint

                If .VisualProperties.ShowSurface Then

                    Dim SurfaceColor As New Vector3

                    If Not .Active Then
                        SurfaceColor.X = .VisualProperties.ColorSurface.R / 255
                        SurfaceColor.Y = .VisualProperties.ColorSurface.G / 255
                        SurfaceColor.Z = .VisualProperties.ColorSurface.B / 255
                    Else
                        SurfaceColor.X = 1.0
                        SurfaceColor.Y = 0.8
                        SurfaceColor.Z = 0.0
                    End If

                    gl.Color(SurfaceColor.X, SurfaceColor.Y, SurfaceColor.Z, .VisualProperties.Transparency)

                    gl.InitNames()
                    Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etPanel, 0)

                    For i = 0 To .NumberOfPanels - 1

                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_TRIANGLES)

                        Dim Panel As Panel = .Mesh.Panels(i)

                        Nodo = .Mesh.Nodes((Panel.N1))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N2))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N3))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N3))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N4))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N1))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        gl.End()
                        gl.PopName()

                    Next

                End If

                ' Nodes:

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(.VisualProperties.SizeNodes)
                gl.Color(.VisualProperties.ColorNodes.R / 255,
                         .VisualProperties.ColorNodes.G / 255,
                         .VisualProperties.ColorNodes.B / 255)

                For Each Node In .Mesh.Nodes

                    If ForSelection Or Node.Active Then

                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_POINTS)
                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)
                        gl.End()
                        gl.PopName()

                    End If

                Next

                ' Segments:

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etSegment, 0)

                If ForSelection Or .VisualProperties.ShowMesh Then

                    gl.LineWidth(.VisualProperties.ThicknessMesh)

                    Dim Node1 As Vector3
                    Dim Node2 As Vector3

                    gl.Color(.VisualProperties.ColorMesh.R / 255,
                             .VisualProperties.ColorMesh.G / 255,
                             .VisualProperties.ColorMesh.B / 255)

                    For Each Segment In .Mesh.Lattice

                        Node1 = .Mesh.Nodes(Segment.N1).Position
                        Node2 = .Mesh.Nodes(Segment.N2).Position

                        gl.Begin(OpenGL.GL_LINES)
                        gl.Vertex(Node1.X, Node1.Y, Node1.Z)
                        gl.Vertex(Node2.X, Node2.Y, Node2.Z)
                        gl.End()

                    Next

                End If

                ' Normals:

                If .VisualProperties.ShowNormalVectors Then

                    gl.Begin(OpenGL.GL_LINES)

                    gl.Color(.VisualProperties.ColorPositiveLoad.R / 255,
                             .VisualProperties.ColorPositiveLoad.G / 255,
                             .VisualProperties.ColorPositiveLoad.B / 255)

                    For Each Panel In .Mesh.Panels
                        gl.Vertex(Panel.ControlPoint.X, Panel.ControlPoint.Y, Panel.ControlPoint.Z)
                        gl.Vertex(Panel.ControlPoint.X + Panel.NormalVector.X,
                                  Panel.ControlPoint.Y + Panel.NormalVector.Y,
                                  Panel.ControlPoint.Z + Panel.NormalVector.Z)

                    Next

                    gl.End()

                End If

            End With

        End Sub

        ''' <summary>
        ''' Renders an imported surface using SharpGL
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As ImportedSurface,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)
            With This

                Dim Code As Integer = 0

                Dim Nodo As NodalPoint

                If .VisualProperties.ShowSurface Then

                    Dim SurfaceColor As New Vector3

                    If Not .Active Then
                        SurfaceColor.X = .VisualProperties.ColorSurface.R / 255
                        SurfaceColor.Y = .VisualProperties.ColorSurface.G / 255
                        SurfaceColor.Z = .VisualProperties.ColorSurface.B / 255
                    Else
                        SurfaceColor.X = 1.0
                        SurfaceColor.Y = 0.8
                        SurfaceColor.Z = 0.0
                    End If

                    gl.Color(SurfaceColor.X, SurfaceColor.Y, SurfaceColor.Z, .VisualProperties.Transparency)

                    gl.InitNames()
                    Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etPanel, 0)

                    For i = 0 To .NumberOfPanels - 1

                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_TRIANGLES)

                        Dim Panel As Panel = .Mesh.Panels(i)

                        Nodo = .Mesh.Nodes((Panel.N1))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N2))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N3))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N3))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N4))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = .Mesh.Nodes((Panel.N1))
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        gl.End()
                        gl.PopName()

                    Next

                End If

                ' Nodes:

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(.VisualProperties.SizeNodes)
                gl.Color(.VisualProperties.ColorNodes.R / 255,
                         .VisualProperties.ColorNodes.G / 255,
                         .VisualProperties.ColorNodes.B / 255)

                For Each Node In .Mesh.Nodes

                    If ForSelection Or Node.Active Then

                        gl.PushName(Code)
                        Code += 1
                        gl.Begin(OpenGL.GL_POINTS)
                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)
                        gl.End()
                        gl.PopName()

                    End If

                Next

                ' Segments:

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etSegment, 0)

                If ForSelection Or .VisualProperties.ShowMesh Then

                    gl.LineWidth(.VisualProperties.ThicknessMesh)

                    Dim Node1 As Vector3
                    Dim Node2 As Vector3

                    gl.Color(.VisualProperties.ColorMesh.R / 255,
                             .VisualProperties.ColorMesh.G / 255,
                             .VisualProperties.ColorMesh.B / 255)

                    For Each Segment In .Mesh.Lattice

                        Node1 = .Mesh.Nodes(Segment.N1).Position
                        Node2 = .Mesh.Nodes(Segment.N2).Position

                        gl.Begin(OpenGL.GL_LINES)
                        gl.Vertex(Node1.X, Node1.Y, Node1.Z)
                        gl.Vertex(Node2.X, Node2.Y, Node2.Z)
                        gl.End()

                    Next

                End If

                ' Normals:

                If .VisualProperties.ShowNormalVectors Then

                    gl.Begin(OpenGL.GL_LINES)

                    gl.Color(.VisualProperties.ColorPositiveLoad.R / 255,
                             .VisualProperties.ColorPositiveLoad.G / 255,
                             .VisualProperties.ColorPositiveLoad.B / 255)

                    For Each Panel In .Mesh.Panels
                        gl.Vertex(Panel.ControlPoint.X, Panel.ControlPoint.Y, Panel.ControlPoint.Z)
                        gl.Vertex(Panel.ControlPoint.X + Panel.NormalVector.X,
                                  Panel.ControlPoint.Y + Panel.NormalVector.Y,
                                  Panel.ControlPoint.Z + Panel.NormalVector.Z)

                    Next

                    gl.End()

                End If

            End With

        End Sub

        ''' <summary>
        ''' Renders a result container using SharpGL
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As ResultContainer,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)

            With This

                If .VisualProperties.ShowSurface Then

                    Dim Node As NodalPoint

                    Dim Index As Integer = 0

                    If ForSelection Or .VisualProperties.ShowSurface Then

                        ' Load homogeneous color:

                        Dim R As Double
                        Dim G As Double
                        Dim B As Double

                        R = .VisualProperties.ColorSurface.R / 255
                        G = .VisualProperties.ColorSurface.G / 255
                        B = .VisualProperties.ColorSurface.B / 255

                        gl.Color(R, G, B, .VisualProperties.Transparency)

                        gl.InitNames()

                        Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etResultContainer, ElementIndex, EntityTypes.etPanel, 0)

                        Index = 0

                        For Each Panel In .Mesh.Panels

                            gl.PushName(Code + Index)
                            gl.Begin(OpenGL.GL_TRIANGLES)

                            If Panel.Active Then

                                gl.Color(1.0, 0.0, 0.5)

                                Node = .Mesh.Nodes(Panel.N1)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N2)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N3)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N3)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N4)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N1)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                            ElseIf .VisualProperties.ShowColormap Then

                                Select Case .ActiveResult

                                    Case ResultKinds.PanelPressure

                                        gl.Color(Panel.CpColor.R, Panel.CpColor.G, Panel.CpColor.B)

                                        Node = .Mesh.Nodes(Panel.N1)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N2)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N3)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N3)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N4)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N1)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                    Case ResultKinds.NodalDisplacement

                                        Node = .Mesh.Nodes(Panel.N1)
                                        gl.Color(Node.DisplacementColor.R, Node.DisplacementColor.G, Node.DisplacementColor.B)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N2)
                                        gl.Color(Node.DisplacementColor.R, Node.DisplacementColor.G, Node.DisplacementColor.B)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N3)
                                        gl.Color(Node.DisplacementColor.R, Node.DisplacementColor.G, Node.DisplacementColor.B)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N3)
                                        gl.Color(Node.DisplacementColor.R, Node.DisplacementColor.G, Node.DisplacementColor.B)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N4)
                                        gl.Color(Node.DisplacementColor.R, Node.DisplacementColor.G, Node.DisplacementColor.B)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                        Node = .Mesh.Nodes(Panel.N1)
                                        gl.Color(Node.DisplacementColor.R, Node.DisplacementColor.G, Node.DisplacementColor.B)
                                        gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                End Select

                            Else

                                gl.Color(R, G, B, .VisualProperties.Transparency)

                                Node = .Mesh.Nodes(Panel.N1)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N2)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N3)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N3)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N4)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                                Node = .Mesh.Nodes(Panel.N1)
                                gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)

                            End If

                            gl.End()
                            gl.PopName()

                            Index += 1

                        Next

                    End If

                    ' Show nodes:

                    If ForSelection Or .VisualProperties.ShowNodes Then

                        gl.InitNames()
                        Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etResultContainer, ElementIndex, EntityTypes.etNode, 0)

                        gl.PointSize(.VisualProperties.SizeNodes)

                        gl.Color(.VisualProperties.ColorNodes.R / 255,
                                 .VisualProperties.ColorNodes.G / 255,
                                 .VisualProperties.ColorNodes.B / 255)

                        Index = 0

                        For Each Node In .Mesh.Nodes

                            gl.PushName(Code + Index)
                            gl.Begin(OpenGL.GL_POINTS)
                            gl.Vertex(Node.Position.X, Node.Position.Y, Node.Position.Z)
                            gl.End()
                            gl.PopName()

                            Index += 1
                        Next

                    End If

                    ' Show lattice:

                    If ForSelection Or .VisualProperties.ShowMesh Then

                        gl.LineWidth(.VisualProperties.ThicknessMesh)

                        Dim Node1 As Vector3
                        Dim Node2 As Vector3

                        gl.InitNames()
                        Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etResultContainer, ElementIndex, EntityTypes.etSegment, 0)

                        gl.Color(.VisualProperties.ColorMesh.R / 255,
                                 .VisualProperties.ColorMesh.G / 255,
                                 .VisualProperties.ColorMesh.B / 255)

                        Index = 0

                        For Each Segment In .Mesh.Lattice

                            Node1 = .Mesh.Nodes(Segment.N1).Position
                            Node2 = .Mesh.Nodes(Segment.N2).Position

                            gl.PushName(Code + Index)
                            gl.Begin(OpenGL.GL_LINES)
                            gl.Vertex(Node1.X, Node1.Y, Node1.Z)
                            gl.Vertex(Node2.X, Node2.Y, Node2.Z)
                            gl.End()
                            gl.PopName()

                            Index += 1

                        Next

                    End If

                    If .VisualProperties.ShowVelocityVectors Then

                        gl.Color(.VisualProperties.ColorVelocity.R / 255,
                                 .VisualProperties.ColorVelocity.G / 255,
                                 .VisualProperties.ColorVelocity.B / 255)

                        gl.Begin(OpenGL.GL_LINES)

                        For Each Panel In .Mesh.Panels

                            gl.Vertex(Panel.ControlPoint.X,
                                      Panel.ControlPoint.Y,
                                      Panel.ControlPoint.Z)
                            gl.Vertex(Panel.ControlPoint.X + .VisualProperties.ScaleVelocityVectors * Panel.LocalVelocity.X,
                                      Panel.ControlPoint.Y + .VisualProperties.ScaleVelocityVectors * Panel.LocalVelocity.Y,
                                      Panel.ControlPoint.Z + .VisualProperties.ScaleVelocityVectors * Panel.LocalVelocity.Z)

                        Next

                        gl.End()

                    End If

                End If

            End With

        End Sub

        ''' <summary>
        ''' Renders a result container using SharpGL
        ''' </summary>
        <Extension()>
        Public Sub Refresh3DModel(This As ResultFrame,
                                  ByRef gl As OpenGL,
                                  Optional ByVal ForSelection As Boolean = False,
                                  Optional ByVal ElementIndex As Integer = 0)

            With This.Model

                If .VisualProperties.ShowSurface AndAlso .VisualProperties.ShowLoadVectors Then

                    ' Local Cp
                    '---------------------------------------------
                    Dim Load As New Vector3

                    For Each Panel In .Mesh.Panels

                        Load.Assign(Panel.NormalVector)
                        If Panel.IsSlender Then
                            gl.Color(.VisualProperties.ColorPositiveLoad.R / 255,
                                     .VisualProperties.ColorPositiveLoad.G / 255,
                                     .VisualProperties.ColorPositiveLoad.B / 255)
                            Load.Scale(.VisualProperties.ScaleLoadVectors * Panel.Cp * Panel.Area)
                        Else
                            If Panel.Cp > 0 Then
                                gl.Color(.VisualProperties.ColorPositiveLoad.R / 255,
                                         .VisualProperties.ColorPositiveLoad.G / 255,
                                         .VisualProperties.ColorPositiveLoad.B / 255)
                                Load.Scale(.VisualProperties.ScaleLoadVectors * Panel.Cp * Panel.Area)
                            Else
                                gl.Color(.VisualProperties.ColorPositiveLoad.R,
                                         .VisualProperties.ColorPositiveLoad.G,
                                         .VisualProperties.ColorPositiveLoad.B)
                                Load.Scale(- .VisualProperties.ScaleLoadVectors * Panel.Cp * Panel.Area)
                            End If
                        End If

                        gl.Begin(OpenGL.GL_LINES)
                        gl.Vertex(Panel.ControlPoint.X,
                              Panel.ControlPoint.Y,
                              Panel.ControlPoint.Z)
                        gl.Vertex(Panel.ControlPoint.X + Load.X,
                              Panel.ControlPoint.Y + Load.Y,
                              Panel.ControlPoint.Z + Load.Z)
                        gl.End()

                    Next

                    ' Lift vectors
                    '---------------------------------------------

                    For Each AirLoad In This.PartialAirLoads

                        If AirLoad.MaximumLift > 0 Then

                            gl.Color(.VisualProperties.ColorPositiveLoad.R,
                                     .VisualProperties.ColorPositiveLoad.G,
                                     .VisualProperties.ColorPositiveLoad.B)

                            For Each LiftVector In AirLoad.LiftVectors

                                gl.Begin(OpenGL.GL_LINES)
                                gl.Vertex(LiftVector.Point.X,
                                          LiftVector.Point.Y,
                                          LiftVector.Point.Z)
                                gl.Vertex(LiftVector.Point.X + LiftVector.Vector.X / AirLoad.MaximumLift,
                                          LiftVector.Point.Y + LiftVector.Vector.Y / AirLoad.MaximumLift,
                                          LiftVector.Point.Z + LiftVector.Vector.Z / AirLoad.MaximumLift)
                                gl.End()

                            Next

                        End If

                    Next

                End If

            End With

        End Sub

        ''' <summary>
        ''' Represents the active operation
        ''' </summary>
        ''' <param name="This"></param>
        ''' <param name="gl"></param>
        <Extension()>
        Public Sub RepresentTaskOnGL(This As OperationsTool, ByRef gl As OpenGL)

            With This

                Select Case .Operation

                    Case Operations.NoOperation
                        Return

                    Case Operations.Translate

                        gl.Color(0, 0, 0)

                        For Each Point In .Points

                            If IsNothing(Point) Then Continue For

                            gl.Begin(OpenGL.GL_LINES)

                            gl.Vertex(Point.X - 0.1, Point.Y, Point.Z)
                            gl.Vertex(Point.X + 0.1, Point.Y, Point.Z)

                            gl.Vertex(Point.X, Point.Y - 0.1, Point.Z)
                            gl.Vertex(Point.X, Point.Y + 0.1, Point.Z)

                            gl.Vertex(Point.X, Point.Y, Point.Z - 0.1)
                            gl.Vertex(Point.X, Point.Y, Point.Z + 0.1)

                            gl.End()

                            gl.PointSize(4.0)

                            gl.Begin(OpenGL.GL_POINTS)
                            gl.Vertex(Point.X, Point.Y, Point.Z)
                            gl.End()

                        Next

                    Case Operations.Align

                        gl.Color(0, 0, 0)

                        For Each Point In .Points

                            If IsNothing(Point) Then Continue For

                            gl.Begin(OpenGL.GL_LINES)

                            gl.Vertex(Point.X - 0.1, Point.Y, Point.Z)
                            gl.Vertex(Point.X + 0.1, Point.Y, Point.Z)

                            gl.Vertex(Point.X, Point.Y - 0.1, Point.Z)
                            gl.Vertex(Point.X, Point.Y + 0.1, Point.Z)

                            gl.Vertex(Point.X, Point.Y, Point.Z - 0.1)
                            gl.Vertex(Point.X, Point.Y, Point.Z + 0.1)

                            gl.End()

                            gl.PointSize(4.0)

                            gl.Begin(OpenGL.GL_POINTS)
                            gl.Vertex(Point.X, Point.Y, Point.Z)
                            gl.End()

                        Next

                        gl.Color(0.5, 0.5, 0.5)

                        If .Points.Count > 1 Then

                            gl.LineWidth(3.0)

                            gl.Begin(OpenGL.GL_LINES)

                            gl.Vertex(.Points(0).X, .Points(0).Y, .Points(0).Z)
                            gl.Vertex(.Points(1).X, .Points(1).Y, .Points(1).Z)

                            gl.End()

                            gl.PointSize(4.0)

                        End If

                End Select

            End With

        End Sub

        ''' <summary>
        ''' Represents the velocity plane
        ''' </summary>
        <Extension()>
        Public Sub Updte3DModel(This As VelocityPlane, ByRef gl As OpenGL)

            With This

                If .Visible Then

                    gl.PointSize(.NodeSize)

                    gl.Begin(OpenGL.GL_POINTS)
                    gl.Color(.ColorNodes.R / 255,
                             .ColorNodes.G / 255,
                             .ColorNodes.B / 255, 1)

                    For i = 1 To .NumberOfNodes
                        gl.Vertex(.GetNode(i).X, .GetNode(i).Y, .GetNode(i).Z)
                    Next

                    gl.End()

                    gl.LineWidth(.VectorThickness)

                    gl.Begin(OpenGL.GL_LINES)
                    gl.Color(.ColorVectors.R / 255,
                             .ColorVectors.G / 255,
                             .ColorVectors.B / 255, 1)

                    For i = 1 To .NumberOfNodes
                        gl.Vertex(.GetNode(i).X, .GetNode(i).Y, .GetNode(i).Z)
                        gl.Vertex(.GetNode(i).X + .Scale * .GetInducedVelocity(i).X,
                                  .GetNode(i).Y + .Scale * .GetInducedVelocity(i).Y,
                                  .GetNode(i).Z + .Scale * .GetInducedVelocity(i).Z)
                    Next

                    gl.End()

                    gl.Color(.ColorSurface.R / 255,
                             .ColorSurface.G / 255,
                             .ColorSurface.B / 255, 0.3)

                    gl.Begin(OpenGL.GL_QUADS)

                    gl.Vertex(.Corner1.X, .Corner1.Y, .Corner1.Z)
                    gl.Vertex(.Corner2.X, .Corner2.Y, .Corner2.Z)
                    gl.Vertex(.Corner3.X, .Corner3.Y, .Corner3.Z)
                    gl.Vertex(.Corner4.X, .Corner4.Y, .Corner4.Z)

                    gl.End()

                End If

            End With

        End Sub

        ''' <summary>
        ''' Represents the velocity vector
        ''' </summary>
        ''' <param name="gl"></param>
        ''' <param name="StreamVelocity"></param>
        ''' <param name="Position"></param>
        Public Sub RepresentVelocityVector(ByVal gl As OpenGL, ByVal StreamVelocity As Vector3, ByVal Position As Vector3)

            Dim Velocity As New Vector3

            Velocity.Assign(StreamVelocity)

            Velocity.Normalize()

            gl.LineWidth(1.0F)
            gl.Begin(OpenGL.GL_LINES)

            gl.Color(0.1, 0.1, 0.8)

            gl.Vertex(Position.X, Position.Y, Position.Z)
            gl.Vertex(Position.X - Velocity.X, Position.Y - Velocity.Y, Position.Z - Velocity.Z)

            gl.End()

        End Sub

        ''' <summary>
        ''' Represents the transit state of the result model
        ''' </summary>
        <Extension()>
        Public Sub RepresentTransitState(This As ResultModel, ByRef GL As SharpGL.OpenGL, ByVal TimeStep As Integer)

            With This

                If (.Frames.Count <= 1) OrElse (TimeStep >= .Frames.Count) OrElse .Frames(TimeStep).FrameKind <> ResultFrameKinds.Transit Then Exit Sub

                This.Frames(TimeStep).Model.Refresh3DModel(GL)
                This.Frames(TimeStep).Wakes.Refresh3DModel(GL)

            End With

        End Sub

    End Module

End Namespace

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

Imports MathTools.Algebra.EuclideanSpace
Imports AeroTools.VisualModel.Interface
Imports SharpGL
Imports System.Xml

Namespace VisualModel.Models.Components.Basics

    ''' <summary>
    ''' Basic definition of a model surface. 
    ''' All model surfaces in the library must inherit from this class.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Surface

        Implements IOperational

        Implements ISelectable

        ''' <summary>
        ''' Surface identifier.
        ''' </summary>
        ''' <returns></returns>
        Public Property ID As Guid

        ''' <summary>
        ''' Surface name.
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String

        ''' <summary>
        ''' Mesh.
        ''' </summary>
        ''' <returns></returns>
        Public Property Mesh As Mesh

        ''' <summary>
        ''' Number of segments in the mesh.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NumberOfSegments As Integer
            Get
                Return Mesh.Lattice.Count
            End Get
        End Property

        ''' <summary>
        ''' Number of nodes in the mesh.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NumberOfNodes As Integer
            Get
                If Not IsNothing(Mesh.Nodes) Then
                    Return Mesh.Nodes.Count
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Number of panels in the mesh.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property NumberOfPanels As Integer
            Get
                If Not IsNothing(Mesh.Panels) Then
                    Return Mesh.Panels.Count
                Else
                    Return 0
                End If
            End Get
        End Property

        ''' <summary>
        ''' Surface visual properties.
        ''' </summary>
        ''' <returns></returns>
        Public Property VisualProperties As VisualProperties

        ''' <summary>
        ''' Indicates if the surface participates in the calculation model.
        ''' </summary>
        ''' <returns></returns>
        Public Property IncludeInCalculation As Boolean = False

        ''' <summary>
        ''' Indicate if the GUI has to block the content of this surface.
        ''' </summary>
        ''' <returns></returns>
        Public Property LockContent As Boolean = True

        ''' <summary>
        ''' Position of the surface in the global coordinates system.
        ''' </summary>
        ''' <returns></returns>
        Public Property Position As New EVector3

        ''' <summary>
        ''' Center of rotation of the surface in the local coordinates system.
        ''' </summary>
        ''' <returns></returns>
        Public Property CenterOfRotation As New EVector3

        ''' <summary>
        ''' Orientation of the surface.
        ''' </summary>
        ''' <returns></returns>
        Public Property Orientation As New EulerAngles

        ''' <summary>
        ''' Scale of this surface.
        ''' </summary>
        ''' <returns></returns>
        Public Property SizeScale As Double = 1.0

#Region " Operations "

        ''' <summary>
        ''' Local directions.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property LocalDirections As New EBase3

        ''' <summary>
        ''' Moves the origin of the local coordinates to a given point.
        ''' </summary>
        ''' <param name="Vector"></param>
        Public Overridable Sub MoveTo(ByVal Vector As EVector3) Implements IOperational.MoveTo

            Position.X = Vector.X
            Position.Y = Vector.Y
            Position.Z = Vector.Z

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Changes the orientation of the surface.
        ''' </summary>
        ''' <param name="Point"></param>
        ''' <param name="Ori"></param>
        Public Overridable Sub Orientate(ByVal Point As EVector3, ByVal Ori As EulerAngles) Implements IOperational.Orientate

            Orientation.Psi = Ori.Psi
            Orientation.Tita = Ori.Tita
            Orientation.Fi = Ori.Fi

            CenterOfRotation.X = Point.X
            CenterOfRotation.Y = Point.Y
            CenterOfRotation.Z = Point.Z

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Scales the coordinates.
        ''' </summary>
        ''' <param name="Scale"></param>
        Public Overridable Sub Scale(ByVal Scale As Double) Implements IOperational.Scale

            SizeScale = Scale

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Align the surface.
        ''' </summary>
        ''' <param name="P1"></param>
        ''' <param name="P2"></param>
        ''' <param name="P3"></param>
        ''' <param name="P4"></param>
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

            For Each node In Mesh.Nodes
                node.Active = False
            Next

        End Sub

        Public MustOverride Function Clone() As Surface

#Region " Meshing "

        ''' <summary>
        ''' Refresh the 3D model on a OpenGL control.
        ''' </summary>
        ''' <param name="gl"></param>
        ''' <param name="SelectionMode"></param>
        ''' <param name="ElementIndex"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub Refresh3DModel(ByRef gl As OpenGL, Optional ByVal SelectionMode As SelectionModes = SelectionModes.smNoSelection, Optional ByVal ElementIndex As Integer = 0)

        ''' <summary>
        ''' Generates the mesh.
        ''' </summary>
        Public Overridable Sub GenerateMesh()

            RaiseEvent MeshUpdated()

        End Sub

        Public Event MeshUpdated()

#End Region

#Region " IO "

        Public MustOverride Sub WriteToXML(ByRef writes As XmlWriter)

        Public MustOverride Sub ReadFromXML(ByRef reader As XmlReader)

#End Region

    End Class

End Namespace
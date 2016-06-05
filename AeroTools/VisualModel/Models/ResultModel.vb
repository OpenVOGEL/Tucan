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
Imports System.IO
Imports System.Xml
Imports AeroTools.CalculationModel.Settings
Imports AeroTools.VisualModel.Models.Components
Imports AeroTools.VisualModel.IO

Namespace VisualModel.Models

    ''' <summary>
    ''' Stores results for a given time step.
    ''' </summary>
    Public Class ResultModel

        Public Name As String
        Public TimeStep As Integer

        Private _Model As ResultContainer
        Private _Wakes As ResultContainer
        Private _DynamicModes As List(Of ResultContainer)

        Public Property AerodynamicForce As New EVector3
        Public Property AerodynamicMoment As New EVector3

        Public Property SimulationSettings As New SimulationSettings
        Public TotalArea As Double

        Public Property Loaded As Boolean = False

        Public Property VisualizeModes As Boolean

        Public Property SelectedModeIndex As Integer

        Public ReadOnly Property Model As ResultContainer
            Get
                Return _Model
            End Get
        End Property

        Public ReadOnly Property Wakes As ResultContainer
            Get
                Return _Wakes
            End Get
        End Property

        Public Property DynamicModes As List(Of ResultContainer)
            Get
                Return _DynamicModes
            End Get
            Set(ByVal value As List(Of ResultContainer))
                _DynamicModes = value
            End Set
        End Property

        Public ReadOnly Property SelectedMode As ResultContainer
            Get
                If SelectedModeIndex > -1 And SelectedModeIndex < DynamicModes.Count Then
                    Return _DynamicModes(SelectedModeIndex)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Sub InitializeResults()

            _Model = New ResultContainer
            _Wakes = New ResultContainer
            _DynamicModes = New List(Of ResultContainer)

            Me._Model.Name = "Modelo"
            Me._Model.VisualProperties.ColorMesh = System.Drawing.Color.Maroon
            Me._Model.VisualProperties.ColorSurface = System.Drawing.Color.Orange
            Me._Model.VisualProperties.Transparency = 1.0
            Me._Model.VisualProperties.ShowSurface = True
            Me._Model.VisualProperties.ShowMesh = True
            Me._Model.VisualProperties.ShowNodes = False
            Me._Model.VisualProperties.ThicknessMesh = 0.8
            Me._Model.VisualProperties.ShowNodes = False

            Me._Wakes.Name = "Estelas"
            Me._Wakes.VisualProperties.ColorMesh = System.Drawing.Color.Silver
            Me._Wakes.VisualProperties.ColorSurface = System.Drawing.Color.LightBlue
            Me._Wakes.VisualProperties.ColorNodes = Drawing.Color.Black
            Me._Wakes.VisualProperties.Transparency = 1.0
            Me._Wakes.VisualProperties.ShowSurface = False
            Me._Wakes.VisualProperties.ShowMesh = False
            Me._Wakes.VisualProperties.ThicknessMesh = 0.8
            Me._Wakes.VisualProperties.SizeNodes = 3.0#
            Me._Wakes.VisualProperties.ShowNodes = False

            _Model.Clear()
            _Wakes.Clear()

        End Sub

        Public Sub Clear()

            _Model.Clear()
            _Wakes.Clear()

        End Sub

        Public Sub CalculateLoads()

            _Model.CalculateAerodynamiLoad(AerodynamicForce, AerodynamicMoment, TotalArea)

        End Sub

#Region " IO "

        Public Sub ReadFromXML(ByVal FilePath As String)

            If Not File.Exists(FilePath) Then Throw New Exception("Results file could not have been found")

            Dim reader As XmlReader = XmlReader.Create(FilePath)

            If reader.ReadToFollowing("Result", "TResultados") Then

                Name = reader.GetAttribute("Name")

                _Model.AccessPath = reader.GetAttribute("ModelPath")
                _Model.ReadFromBinary()

                _Wakes.AccessPath = reader.GetAttribute("WakePath")
                _Wakes.ReadFromBinary()

                If reader.ReadToFollowing("Simulacion", "TSimulacion") Then
                    SimulationSettings.ReadFromXML(reader)
                End If

                Dim nModes As Integer = IOXML.ReadInteger(reader, "nModes", 0)

                For i = 0 To nModes - 1

                    Dim Mode As New ResultContainer()
                    Mode.AccessPath = IOXML.ReadString(reader, "Mode" + i, 0)
                    Mode.ReadFromBinary()

                Next

            End If

            reader.Close()

            Loaded = True

        End Sub

        Public Sub WriteToXML(ByVal FilePath As String)

            Dim writer As XmlWriter = XmlWriter.Create(FilePath)

            _Model.AccessPath = FilePath & ".mod"
            _Wakes.AccessPath = FilePath & ".wks"

            writer.WriteStartElement("Result", "TResultados")

            writer.WriteAttributeString("Name", Name)
            writer.WriteAttributeString("ModelPath", _Model.AccessPath)
            writer.WriteAttributeString("WakePath", _Wakes.AccessPath)

            writer.WriteStartElement("Simulacion", "TSimulacion")
            SimulationSettings.SaveToXML(writer)
            writer.WriteEndElement()

            writer.WriteAttributeString("nModes", DynamicModes.Count)
            For i = 0 To DynamicModes.Count - 1
                _DynamicModes(i).AccessPath = FilePath & ".dynmode" + i
                writer.WriteAttributeString("Mode" + i, _DynamicModes(i).AccessPath)
            Next

            writer.WriteEndElement() ' Project

            writer.Close()

            _Model.WriteToBinary()
            _Wakes.WriteToBinary()

        End Sub

#End Region

#Region " Transit simulation "

        ''' <summary>
        ''' Gathers all transit lattices.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TransitLattices As New List(Of ResultContainer)

        Private _TransitStages As Integer = 0

        ''' <summary>
        ''' Indicates the number of loaded stages
        ''' </summary>
        Public ReadOnly Property TransitStages As Integer
            Get
                Return TransitLattices.Count
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the simulation frames have been loaded
        ''' </summary>
        ''' <remarks></remarks>
        Public Property TransitLoaded As Boolean

        Public Sub ClearTransit()
            TransitLattices.Clear()
            _TransitLoaded = False
        End Sub

        Public Sub RepresentTransitState(ByRef GL As SharpGL.OpenGL, ByVal TimeStep As Integer)

            If (Not _TransitLoaded) Or (TimeStep >= TransitLattices.Count) Then Exit Sub

            GL.Color(0, 1.0#, 0)
            GL.Begin(SharpGL.OpenGL.GL_TRIANGLES)

            For Each Ring In TransitLattices(TimeStep).Mesh.Panels

                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Ring.N1).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N1).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N1).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Ring.N2).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N2).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N2).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Ring.N3).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N3).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N3).Position.Z)

                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Ring.N3).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N3).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N3).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Ring.N4).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N4).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N4).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Ring.N1).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N1).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Ring.N1).Position.Z)

            Next

            GL.End()

            'Surface velocity
            'GL.Begin(SharpGL.OpenGL.GL_LINES)

            'For Each Ring In TransitLattices(TimeStep).Mesh.Panels

            '    GL.Vertex(TransitLattices(TimeStep).Mesh.Panels(0).,
            '              TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.Y,
            '              TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.Z)
            '    GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N2 - 1).Position.X,
            '              TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N2 - 1).Position.Y,
            '              TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N2 - 1).Position.Z)

            'Next

            'GL.End()

            GL.Color(0.0#, 0.0#, 0.0#)
            GL.Begin(SharpGL.OpenGL.GL_LINES)

            For Each Vortex In TransitLattices(TimeStep).Mesh.Lattice

                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Vortex.N1).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Vortex.N1).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Vortex.N1).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.Nodes(Vortex.N2).Position.X,
                          TransitLattices(TimeStep).Mesh.Nodes(Vortex.N2).Position.Y,
                          TransitLattices(TimeStep).Mesh.Nodes(Vortex.N2).Position.Z)

            Next

            GL.End()

        End Sub

#End Region

    End Class

End Namespace
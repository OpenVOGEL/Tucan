'Copyright (C) 2016 Guillermo Hazebrouck

Imports MathTools.Algebra.EuclideanSpace
Imports System.IO
Imports System.Xml
Imports AeroTools.UVLM.Settings
Imports AeroTools.VisualModel.Models.Components
Imports AeroTools.VisualModel.IO

Namespace VisualModel.Interface.Results

    ''' <summary>
    ''' Stores results for a given time step.
    ''' </summary>
    Public Class CalculatedModel

        Public Name As String
        Public TimeStep As Integer

        Private _Model As GeneralSurface
        Private _Wakes As GeneralSurface
        Private _DynamicModes As List(Of GeneralSurface)

        Public Property AerodynamicForce As New EVector3
        Public Property AerodynamicMoment As New EVector3

        Public Property SimulationSettings As New SimulationSettings
        Public TotalArea As Double

        Public Property Loaded As Boolean = False

        Public Property VisualizeModes As Boolean

        Public Property SelectedModeIndex As Integer

        Public ReadOnly Property Model As GeneralSurface
            Get
                Return _Model
            End Get
        End Property

        Public ReadOnly Property Wakes As GeneralSurface
            Get
                Return _Wakes
            End Get
        End Property

        Public Property DynamicModes As List(Of GeneralSurface)
            Get
                Return _DynamicModes
            End Get
            Set(ByVal value As List(Of GeneralSurface))
                _DynamicModes = value
            End Set
        End Property

        Public ReadOnly Property SelectedMode As GeneralSurface
            Get
                If SelectedModeIndex > -1 And SelectedModeIndex < DynamicModes.Count Then
                    Return _DynamicModes(SelectedModeIndex)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Sub InitializeResults()

            _Model = New GeneralSurface
            _Wakes = New GeneralSurface
            _DynamicModes = New List(Of GeneralSurface)

            Me._Model.Name = "Modelo"
            Me._Model.VisualProps.ColorMesh = System.Drawing.Color.Maroon
            Me._Model.VisualProps.ColorSurface = System.Drawing.Color.Orange
            Me._Model.VisualProps.Transparency = 1.0
            Me._Model.VisualProps.ShowSurface = True
            Me._Model.VisualProps.ShowMesh = True
            Me._Model.VisualProps.ShowNodes = False
            Me._Model.VisualProps.ThicknessMesh = 0.8
            Me._Model.VisualProps.ShowNodes = False

            Me._Wakes.Name = "Estelas"
            Me._Wakes.VisualProps.ColorMesh = System.Drawing.Color.Silver
            Me._Wakes.VisualProps.ColorSurface = System.Drawing.Color.LightBlue
            Me._Wakes.VisualProps.ColorNodes = Drawing.Color.Black
            Me._Wakes.VisualProps.Transparency = 1.0
            Me._Wakes.VisualProps.ShowSurface = False
            Me._Wakes.VisualProps.ShowMesh = False
            Me._Wakes.VisualProps.ThicknessMesh = 0.8
            Me._Wakes.VisualProps.SizeNodes = 3.0#
            Me._Wakes.VisualProps.ShowNodes = True

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

                    Dim Mode As New GeneralSurface()
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
        Public Property TransitLattices As New List(Of GeneralSurface)

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

                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N2 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N2 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N2 - 1).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N3 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N3 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N3 - 1).Position.Z)

                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N3 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N3 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N3 - 1).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N4 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N4 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N4 - 1).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Ring.N1 - 1).Position.Z)

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

            For Each Vortex In TransitLattices(TimeStep).Mesh.Vortices

                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Vortex.N1 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Vortex.N1 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Vortex.N1 - 1).Position.Z)
                GL.Vertex(TransitLattices(TimeStep).Mesh.NodalPoints(Vortex.N2 - 1).Position.X,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Vortex.N2 - 1).Position.Y,
                          TransitLattices(TimeStep).Mesh.NodalPoints(Vortex.N2 - 1).Position.Z)

            Next

            GL.End()

        End Sub

#End Region

    End Class

End Namespace
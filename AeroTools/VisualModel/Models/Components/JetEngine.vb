'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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
Imports System.Xml
Imports AeroTools.VisualModel.Models.Components.Basics
Imports SharpGL
Imports AeroTools.VisualModel.Interface
Imports AeroTools.VisualModel.IO

Namespace VisualModel.Models.Components

    Public Class JetEngine

        Inherits Surface

        Public Sub New()

            Mesh = New Mesh()

            VisualProperties = New VisualProperties(ComponentTypes.etJetEngine)

            Length = 2
            FrontDiameter = 1
            BackDiameter = 0.6
            FrontLength = 0.6
            BackLength = 0.4
            Resolution = 15

            GenerateMesh()

            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True


        End Sub

        Public Property Length As Double

        Public Property FrontDiameter As Double

        Public Property BackDiameter As Double

        Public Property FrontLength As Double

        Public Property BackLength As Double

        Public Property ConvectWake As Boolean = True

        Public Property CuttingStep As Integer = 40

        ''' <summary>
        ''' The number of panels in radial direction
        ''' </summary>
        ''' <returns></returns>
        Public Property Resolution As Integer

        ''' <summary>
        ''' Generates a triangular or quadrilateral mesh.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub GenerateMesh()

            Mesh.Nodes.Clear()
            Mesh.Panels.Clear()

            For i = 0 To 3

                Dim x As Double
                Dim r As Double

                Select Case i

                    Case 0
                        x = 0.0#
                        r = 0.5 * FrontDiameter

                    Case 1
                        x = FrontLength
                        r = 0.5 * FrontDiameter

                    Case 2
                        x = Length - BackLength
                        r = 0.5 * BackDiameter

                    Case 3
                        x = Length
                        r = 0.5 * BackDiameter

                End Select

                For j = 0 To Resolution

                    Dim angle As Double = 2 * Math.PI * j / (Resolution + 1)

                    Dim p As New EVector3(x, r * Math.Cos(angle), r * Math.Sin(angle))

                    Mesh.Nodes.Add(New NodalPoint(p))

                    If i > 0 Then

                        Dim N1 As Integer
                        Dim N2 As Integer
                        Dim N3 As Integer
                        Dim N4 As Integer

                        If j < Resolution Then

                            N1 = (i - 1) * (Resolution + 1) + j + 0
                            N2 = (i - 1) * (Resolution + 1) + j + 1
                            N3 = i * (Resolution + 1) + j + 1
                            N4 = i * (Resolution + 1) + j + 0

                            Dim q As New Panel(N1, N4, N3, N2)

                            Mesh.Panels.Add(q)

                        Else

                            N1 = (i - 1) * (Resolution + 1) + j + 0
                            N2 = (i - 1) * (Resolution + 1) + 0
                            N3 = i * (Resolution + 1) + 0
                            N4 = i * (Resolution + 1) + j + 0

                            Dim q As New Panel(N1, N4, N3, N2)

                            Mesh.Panels.Add(q)

                        End If

                    End If

                Next

            Next

            Mesh.Rotate(CenterOfRotation, Orientation.ToRadians)

            Mesh.Translate(Position)

            Mesh.GenerateLattice()

            ' Launch base sub to raise update event.

            MyBase.GenerateMesh()

        End Sub

#Region " 3D Functions "

        Public Overrides Sub Refresh3DModel(ByRef gl As OpenGL, Optional ByVal SelectionMode As SelectionModes = SelectionModes.smNoSelection, Optional ByVal ElementIndex As Integer = 0)

            'Version para OpenGL

            Dim i As Integer

            Dim Nodo As NodalPoint

            If Me.VisualProperties.ShowSurface Then

                ' load homogeneous color:
                Dim SColor As New EVector3

                If Not Selected Then
                    SColor.X = Me.VisualProperties.ColorSurface.R / 255
                    SColor.Y = Me.VisualProperties.ColorSurface.G / 255
                    SColor.Z = Me.VisualProperties.ColorSurface.B / 255
                Else
                    ' default selected color is {255, 194, 14} (orange)
                    SColor.X = 1
                    SColor.Y = 0.76078
                    SColor.Z = 0.0549
                End If

                gl.Color(SColor.X, SColor.Y, SColor.Z, Me.VisualProperties.Transparency)

                gl.InitNames()

                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etQuadPanel, 0)

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
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(VisualProperties.SizeNodes)
                gl.Color(Me.VisualProperties.ColorNodes.R / 255, Me.VisualProperties.ColorNodes.G / 255, Me.VisualProperties.ColorNodes.B / 255)

                For i = 0 To NumberOfNodes - 1

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_POINTS)
                    Nodo = Mesh.Nodes(i)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)
                    gl.End()
                    gl.PopName()

                Next

            End If

            ' Genera el mallado:

            If VisualProperties.ShowMesh Then

                gl.LineWidth(VisualProperties.ThicknessMesh)
                gl.Begin(OpenGL.GL_LINES)

                Dim Nodo1 As EVector3
                Dim Nodo2 As EVector3
                Dim Vector As EVector3
                Dim Carga As New EVector3

                gl.Color(Me.VisualProperties.ColorMesh.R / 255, Me.VisualProperties.ColorMesh.G / 255, Me.VisualProperties.ColorMesh.B / 255)

                For i = 0 To NumberOfSegments - 1

                    Nodo1 = Mesh.Nodes(Mesh.Lattice(i).N1).Position
                    Nodo2 = Mesh.Nodes(Mesh.Lattice(i).N2).Position

                    gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                    gl.Vertex(Nodo2.X, Nodo2.Y, Nodo2.Z)

                Next

                gl.Color(Me.VisualProperties.ColorVelocity.R / 255, Me.VisualProperties.ColorVelocity.G / 255, Me.VisualProperties.ColorVelocity.B / 255)

                If VisualProperties.ShowVelocityVectors Then

                    For i = 0 To NumberOfPanels - 1

                        Nodo1 = Mesh.Panels(i).ControlPoint
                        Vector = Mesh.Panels(i).LocalVelocity

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + VisualProperties.ScaleVelocity * Vector.X, Nodo1.Y + VisualProperties.ScaleVelocity * Vector.Y, Nodo1.Z + VisualProperties.ScaleVelocity * Vector.Z)

                    Next

                End If

                gl.Color(Me.VisualProperties.ColorLoads.R / 255, Me.VisualProperties.ColorLoads.G / 255, Me.VisualProperties.ColorLoads.B / 255)

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

            'If _SelectedControlPoint >= 1 And _SelectedControlPoint <= Me.NumberOfPanels Then

            '    gl.PointSize(2 * Me.VisualizationProperties.SizeNodes)
            '    gl.Color(VisualizationProperties.ColorNodes.R / 255, VisualizationProperties.ColorNodes.G / 255, VisualizationProperties.ColorNodes.B / 255)
            '    gl.Begin(OpenGL.GL_POINTS)

            '    gl.Vertex(Me.QuadPanel(_SelectedControlPoint).ControlPoint.X, Me.QuadPanel(_SelectedControlPoint).ControlPoint.Y, Me.QuadPanel(_SelectedControlPoint).ControlPoint.Z)

            '    gl.End()

            'End If

        End Sub

#End Region

#Region " IO "

        ''' <summary>
        ''' Reads the wing from an XML file.
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <remarks></remarks>
        Public Overrides Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                Select Case reader.Name

                    Case "Identity"

                        Name = reader.GetAttribute("Name")
                        ID = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))
                        Resolution = IOXML.ReadInteger(reader, "RE", 10)
                        FrontDiameter = IOXML.ReadDouble(reader, "FD", 1)
                        BackDiameter = IOXML.ReadDouble(reader, "BD", 0.5)
                        FrontLength = IOXML.ReadDouble(reader, "FL", 1)
                        BackLength = IOXML.ReadDouble(reader, "BL", 0.5)
                        Length = IOXML.ReadDouble(reader, "TL", 0.5)
                        Resolution = Math.Max(11, IOXML.ReadInteger(reader, "RS", 15))
                        CuttingStep = IOXML.ReadInteger(reader, "CS", 20)

                        Position.X = IOXML.ReadDouble(reader, "X", 0.0)
                        Position.Y = IOXML.ReadDouble(reader, "Y", 0.0)
                        Position.Z = IOXML.ReadDouble(reader, "Z", 0.0)

                        Orientation.Psi = IOXML.ReadDouble(reader, "Psi", 0)
                        Orientation.Tita = IOXML.ReadDouble(reader, "Theta", 0)
                        Orientation.Fi = IOXML.ReadDouble(reader, "Phi", 0)

                    Case "VisualProperties"

                        VisualProperties.ReadFromXML(reader.ReadSubtree)

                End Select

            End While

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Writes the wing to an XML file.
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Public Overrides Sub WriteToXML(ByRef writer As XmlWriter)

            ' Identity:

            writer.WriteStartElement("Identity")
            writer.WriteAttributeString("Name", Name)
            writer.WriteAttributeString("ID", ID.ToString)
            writer.WriteAttributeString("FD", CDbl(FrontDiameter))
            writer.WriteAttributeString("BD", CDbl(BackDiameter))
            writer.WriteAttributeString("FL", CDbl(FrontLength))
            writer.WriteAttributeString("BL", CDbl(BackLength))
            writer.WriteAttributeString("TL", CDbl(Length))
            writer.WriteAttributeString("RS", CInt(Resolution))
            writer.WriteAttributeString("CS", CInt(CuttingStep))

            writer.WriteAttributeString("X", CDbl(Position.X))
            writer.WriteAttributeString("Y", CDbl(Position.Y))
            writer.WriteAttributeString("Z", CDbl(Position.Z))

            writer.WriteAttributeString("Psi", CDbl(Orientation.Psi))
            writer.WriteAttributeString("Theta", CDbl(Orientation.Tita))
            writer.WriteAttributeString("Phi", CDbl(Orientation.Fi))

            writer.WriteAttributeString("RE", CInt(Resolution))
            writer.WriteEndElement()

            ' Visual properties:

            writer.WriteStartElement("VisualProperties")
            VisualProperties.WriteToXML(writer)
            writer.WriteEndElement()

        End Sub

        Public Sub CopyFrom(Engine As JetEngine)

            Name = Engine.Name + " - Copy"
            Length = Engine.Length
            FrontDiameter = Engine.FrontDiameter
            BackDiameter = Engine.BackDiameter
            FrontLength = Engine.FrontLength
            BackLength = Engine.BackLength
            Resolution = Engine.Resolution

            Position.X = Engine.Position.X - Engine.Length
            Position.Y = Engine.Position.Y
            Position.Z = Engine.Position.Z

            Orientation.Psi = Engine.Orientation.Psi
            Orientation.Tita = Engine.Orientation.Tita
            Orientation.Fi = Engine.Orientation.Fi

            GenerateMesh()

            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True

        End Sub

        Public Overrides Function Clone() As Surface

            Dim ClonedEngine As New JetEngine
            ClonedEngine.CopyFrom(Me)
            ClonedEngine.Position.Y *= -1
            Return ClonedEngine

        End Function

#End Region

    End Class

End Namespace
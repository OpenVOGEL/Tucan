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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.AeroTools.IoHelper
Imports SharpGL
Imports System.Xml

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

        Public Overrides Sub Refresh3DModel(ByRef gl As OpenGL,
                                            Optional ByVal ForSelection As Boolean = False,
                                            Optional ByVal ElementIndex As Integer = 0)
            Dim Code As Integer = 0

            Dim Nodo As NodalPoint

            If VisualProperties.ShowSurface Then

                Dim SurfaceColor As New EVector3

                If Not Active Then
                    SurfaceColor.X = VisualProperties.ColorSurface.R / 255
                    SurfaceColor.Y = VisualProperties.ColorSurface.G / 255
                    SurfaceColor.Z = VisualProperties.ColorSurface.B / 255
                Else
                    SurfaceColor.X = 1.0
                    SurfaceColor.Y = 0.8
                    SurfaceColor.Z = 0.0
                End If

                gl.Color(SurfaceColor.X, SurfaceColor.Y, SurfaceColor.Z, Me.VisualProperties.Transparency)

                gl.InitNames()
                Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etPanel, 0)

                For i = 0 To NumberOfPanels - 1

                    gl.PushName(Code)
                    Code += 1
                    gl.Begin(OpenGL.GL_TRIANGLES)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N1))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.PressureDeltaColor.R, Nodo.PressureDeltaColor.G, Nodo.PressureDeltaColor.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N2))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.PressureDeltaColor.R, Nodo.PressureDeltaColor.G, Nodo.PressureDeltaColor.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N3))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.PressureDeltaColor.R, Nodo.PressureDeltaColor.G, Nodo.PressureDeltaColor.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N3))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.PressureDeltaColor.R, Nodo.PressureDeltaColor.G, Nodo.PressureDeltaColor.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N4))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.PressureDeltaColor.R, Nodo.PressureDeltaColor.G, Nodo.PressureDeltaColor.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes((Mesh.Panels(i).N1))
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.PressureDeltaColor.R, Nodo.PressureDeltaColor.G, Nodo.PressureDeltaColor.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    gl.End()
                    gl.PopName()

                Next

            End If

            ' Nodes:

            gl.InitNames()
            Code = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etNode, 0)

            gl.PointSize(VisualProperties.SizeNodes)
            gl.Color(VisualProperties.ColorNodes.R / 255, VisualProperties.ColorNodes.G / 255, VisualProperties.ColorNodes.B / 255)

            For Each Node In Mesh.Nodes

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

            If ForSelection Or VisualProperties.ShowMesh Then

                gl.LineWidth(VisualProperties.ThicknessMesh)

                Dim Node1 As EVector3
                Dim Node2 As EVector3

                gl.Color(VisualProperties.ColorMesh.R / 255, VisualProperties.ColorMesh.G / 255, VisualProperties.ColorMesh.B / 255)

                For Each Segment In Mesh.Lattice

                    Node1 = Mesh.Nodes(Segment.N1).Position
                    Node2 = Mesh.Nodes(Segment.N2).Position

                    gl.Begin(OpenGL.GL_LINES)
                    gl.Vertex(Node1.X, Node1.Y, Node1.Z)
                    gl.Vertex(Node2.X, Node2.Y, Node2.Z)
                    gl.End()

                Next

            End If

            ' Normals:

            If VisualProperties.ShowNormalVectors Then

                gl.Begin(OpenGL.GL_LINES)

                gl.Color(VisualProperties.ColorPositiveLoad.R / 255, VisualProperties.ColorPositiveLoad.G / 255, VisualProperties.ColorPositiveLoad.B / 255)

                For Each Panel In Mesh.Panels
                    gl.Vertex(Panel.ControlPoint.X, Panel.ControlPoint.Y, Panel.ControlPoint.Z)
                    gl.Vertex(Panel.ControlPoint.X + Panel.NormalVector.X,
                              Panel.ControlPoint.Y + Panel.NormalVector.Y,
                              Panel.ControlPoint.Z + Panel.NormalVector.Z)

                Next

                gl.End()

            End If

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
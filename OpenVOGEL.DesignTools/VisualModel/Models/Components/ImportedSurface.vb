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
Imports System.IO

Namespace VisualModel.Models.Components

    Public Class ImportedSurface

        Inherits Surface

        Public Sub New()

            Mesh = New Mesh()

            VisualProperties = New VisualProperties(ComponentTypes.etJetEngine)

            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True


        End Sub

        ''' <summary>
        ''' Generates a triangular or quadrilateral mesh.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Load(FilePath As String)

            If File.Exists(FilePath) Then

                Mesh.Nodes.Clear()
                Mesh.Panels.Clear()

                Dim FileNumber As Integer = FreeFile()

                FileOpen(FileNumber, FilePath, OpenAccess.Read)

                Do While Not EOF(FileNumber)

                    Dim Line As String = LineInput(FileNumber)

                    Dim Keywords As String() = Line.Split(" ")

                    If UBound(Keywords) >= 1 Then

                        If Keywords(0) = "NODES" Then

                            Dim N As Integer = Keywords(1)

                            For i = 1 To N

                                Line = LineInput(FileNumber).Trim

                                Dim Coordinates As String() = Line.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

                                If UBound(Coordinates) = 2 Then

                                    Dim Node As New NodalPoint
                                    Node.ReferencePosition = New Vector3(CDbl(Coordinates(0)),
                                                                         CDbl(Coordinates(1)),
                                                                         CDbl(Coordinates(2)))
                                    Mesh.Nodes.Add(Node)

                                End If

                            Next

                        ElseIf Keywords(0) = "PANELS" Then

                            Dim N As Integer = Keywords(1)

                            For i = 1 To N

                                Line = LineInput(FileNumber)

                                Dim Nodes As String() = Line.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

                                If UBound(Nodes) > 0 Then

                                    If Nodes(0) = "1" And UBound(Nodes) >= 4 Then

                                        Dim Panel As New Panel(CInt(Nodes(1)) - 1,
                                                               CInt(Nodes(2)) - 1,
                                                               CInt(Nodes(3)) - 1,
                                                               CInt(Nodes(4)) - 1)

                                        Panel.IsSlender = False
                                        Mesh.Panels.Add(Panel)

                                    ElseIf Nodes(0) = "2" And UBound(Nodes) >= 3 Then

                                        Dim Panel As New Panel(CInt(Nodes(1)) - 1,
                                                               CInt(Nodes(2)) - 1,
                                                               CInt(Nodes(3)) - 1)

                                        Panel.IsSlender = False
                                        Mesh.Panels.Add(Panel)

                                    End If

                                End If

                            Next

                        End If

                    End If

                Loop

                FileClose(FileNumber)

            End If

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Generates a triangular or quadrilateral mesh.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub GenerateMesh()

            For Each Node In Mesh.Nodes

                Node.Position.X = Node.ReferencePosition.X
                Node.Position.Y = Node.ReferencePosition.Y
                Node.Position.Z = Node.ReferencePosition.Z

            Next

            Mesh.Rotate(CenterOfRotation, Orientation.ToRadians)

            Mesh.Translate(Position)

            Mesh.GenerateLattice()

            ' Launch base sub to raise update event.

            MyBase.GenerateMesh()

        End Sub

#Region " IO "

        ''' <summary>
        ''' Reads the wing from an XML file.
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <remarks></remarks>
        Public Overrides Sub ReadFromXML(ByRef reader As XmlReader)

            Mesh.Nodes.Clear()
            Mesh.Panels.Clear()

            While reader.Read

                Select Case reader.Name

                    Case "Identity"

                        Name = reader.GetAttribute("Name")
                        ID = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))

                        Position.X = IOXML.ReadDouble(reader, "X", 0.0)
                        Position.Y = IOXML.ReadDouble(reader, "Y", 0.0)
                        Position.Z = IOXML.ReadDouble(reader, "Z", 0.0)

                        Orientation.Psi = IOXML.ReadDouble(reader, "Psi", 0)
                        Orientation.Tita = IOXML.ReadDouble(reader, "Theta", 0)
                        Orientation.Fi = IOXML.ReadDouble(reader, "Phi", 0)

                    Case "Mesh"

                        Dim NodesData As String = IOXML.ReadString(reader, "Nodes", "")

                        Dim Points As String() = NodesData.Split(";")

                        For Each Point In Points

                            Dim Coordinates As String() = Point.Split(":")

                            If Coordinates.Length = 3 Then
                                Dim Node As New NodalPoint()
                                Node.ReferencePosition = New Vector3(Coordinates(0),
                                                                     Coordinates(1),
                                                                     Coordinates(2))
                                Mesh.Nodes.Add(Node)
                            End If

                        Next

                        Dim PanelsData As String = IOXML.ReadString(reader, "Panels", "")

                        Dim Panels As String() = PanelsData.Split(";")

                        For Each PanelData In Panels

                            Dim Vertices As String() = PanelData.Split(":")

                            If Vertices.Length = 3 Then

                                Dim Panel As New Panel()

                                Panel = New Panel(Vertices(0),
                                                  Vertices(1),
                                                  Vertices(2))

                                Panel.IsSlender = False

                                Mesh.Panels.Add(Panel)

                            ElseIf Vertices.Length = 4 Then

                                Dim Panel As New Panel()
                                Panel = New Panel(Vertices(0),
                                                  Vertices(1),
                                                  Vertices(2),
                                                  Vertices(3))

                                Panel.IsSlender = False

                                Mesh.Panels.Add(Panel)

                            End If

                        Next

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

            writer.WriteAttributeString("X", CDbl(Position.X))
            writer.WriteAttributeString("Y", CDbl(Position.Y))
            writer.WriteAttributeString("Z", CDbl(Position.Z))

            writer.WriteAttributeString("Psi", CDbl(Orientation.Psi))
            writer.WriteAttributeString("Theta", CDbl(Orientation.Tita))
            writer.WriteAttributeString("Phi", CDbl(Orientation.Fi))

            writer.WriteEndElement()

            ' Original mesh:

            writer.WriteStartElement("Mesh")

            Dim Nodes As String = ""

            For Each Node In Mesh.Nodes

                Nodes = Nodes & String.Format("{0:F6}:{1:F6}:{2:F6};",
                                               Node.ReferencePosition.X,
                                               Node.ReferencePosition.Y,
                                               Node.ReferencePosition.Z)

            Next

            writer.WriteAttributeString("Nodes", Nodes)

            Dim Panels As String = ""

            For Each Panel In Mesh.Panels

                If Panel.IsTriangular Then

                    Panels = Panels & String.Format("{0}:{1}:{2};",
                                               Panel.N1,
                                               Panel.N2,
                                               Panel.N3)
                Else

                    Panels = Panels & String.Format("{0}:{1}:{2}:{3};",
                                               Panel.N1,
                                               Panel.N2,
                                               Panel.N3,
                                               Panel.N4)
                End If

            Next

            writer.WriteAttributeString("Panels", Panels)

            writer.WriteEndElement()

            ' Visual properties:

            writer.WriteStartElement("VisualProperties")

            VisualProperties.WriteToXML(writer)

            writer.WriteEndElement()

        End Sub

        Public Sub CopyFrom(Surface As ImportedSurface)

            Name = Surface.Name + " - Copy"

            Mesh.Nodes.Clear()
            Mesh.Panels.Clear()

            For Each Node In Surface.Mesh.Nodes
                Dim NewNode As New NodalPoint(Node.ReferencePosition)
                Mesh.Nodes.Add(NewNode)
            Next

            For Each Panel In Surface.Mesh.Panels
                If Panel.IsTriangular Then
                    Dim NewPanel As New Panel(Panel.N1, Panel.N2, Panel.N3)
                    Mesh.Panels.Add(NewPanel)
                Else
                    Dim NewPanel As New Panel(Panel.N1, Panel.N2, Panel.N3, Panel.N4)
                    Mesh.Panels.Add(NewPanel)
                End If
            Next

            Position.X = Surface.Position.X
            Position.Y = Surface.Position.Y
            Position.Z = Surface.Position.Z

            Orientation.Psi = Surface.Orientation.Psi
            Orientation.Tita = Surface.Orientation.Tita
            Orientation.Fi = Surface.Orientation.Fi

            GenerateMesh()

            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True

        End Sub

        Public Overrides Function Clone() As Surface

            Dim ClonedSurface As New ImportedSurface
            ClonedSurface.CopyFrom(Me)
            ClonedSurface.Position.Y *= -1
            Return ClonedSurface

        End Function

#End Region

    End Class

End Namespace
'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2020 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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
        ''' The different kinds of import formats
        ''' </summary>
        Public Enum ImportFormats As Integer

            ConnectivityFile
            StlFile

        End Enum

        ''' <summary>
        ''' Loads the mesh from a file
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Load(FilePath As String, Format As ImportFormats)

            Select Case Format

                Case ImportFormats.ConnectivityFile
                    LoadNativeFile(FilePath)

                Case ImportFormats.StlFile
                    LoadStlFile(FilePath)

            End Select

        End Sub

        ''' <summary>
        ''' The current entity
        ''' </summary>
        Enum EntityType As Integer

            EntitySurface

            EntityNode

            EntityPanel

        End Enum

        ''' <summary>
        ''' Loads the mesh from a connectivity file in native OpenVOGEL format.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadNativeFile(FilePath As String)

            Dim Section As EntityType = EntityType.EntitySurface

            If File.Exists(FilePath) Then

                Mesh.Nodes.Clear()
                Mesh.Panels.Clear()

                Dim FileId As Integer = FreeFile()
                Dim Offset As Integer = 0
                Dim BaseIndex As Integer = 0

                FileOpen(FileId, FilePath, OpenAccess.Read)

                Do While Not EOF(FileId)

                    Dim Line As String = LineInput(FileId).ToUpper

                    Dim Keywords As String() = Line.Split(" ")

                    ' Check if the entity has changed

                    If UBound(Keywords) >= 0 Then

                        Select Case Keywords(0)

                            Case "NODES"

                                Section = EntityType.EntityNode
                                GoTo NextLine

                            Case "BASE_INDEX"

                                If UBound(Keywords) >= 1 Then
                                    BaseIndex = CInt(Keywords(1))
                                End If
                                GoTo NextLine

                            Case "PANELS"

                                Section = EntityType.EntityPanel
                                GoTo NextLine

                            Case "SURFACE"

                                Section = EntityType.EntitySurface
                                Offset = Mesh.Nodes.Count
                                GoTo NextLine

                        End Select

                    End If

                    ' Read the line for the entitiy

                    Select Case Section

                        Case EntityType.EntityNode

                            Dim Data As String() = Line.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

                            If UBound(Data) = 2 Then

                                Dim Node As New NodalPoint
                                Node.ReferencePosition = New Vector3(CDbl(Data(0)),
                                                                     CDbl(Data(1)),
                                                                     CDbl(Data(2)))
                                Mesh.Nodes.Add(Node)

                            End If

                        Case EntityType.EntityPanel

                            Dim Data As String() = Line.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

                            If UBound(Data) > 0 Then

                                Dim Panel As Panel = Nothing
                                Dim IsSlender As Boolean = Data(0).Length > 0 AndAlso Data(0)(0) = "*"
                                Dim IsReversed As Boolean = Data(0).Length > 1 AndAlso Data(0)(1) = "*"
                                Dim IsPrimitive As Boolean = Data(0).Length > 2 AndAlso Data(0)(2) = "*"

                                If UBound(Data) = 4 Then

                                    ' Quadrilaterals

                                    Dim N1 As Integer = CInt(Data(1)) + Offset - BaseIndex
                                    Dim N2 As Integer = CInt(Data(2)) + Offset - BaseIndex
                                    Dim N3 As Integer = CInt(Data(3)) + Offset - BaseIndex
                                    Dim N4 As Integer = CInt(Data(4)) + Offset - BaseIndex

                                    If (N1 < 0 Or N1 >= Mesh.Nodes.Count) OrElse
                                       (N2 < 0 Or N2 >= Mesh.Nodes.Count) OrElse
                                       (N3 < 0 Or N3 >= Mesh.Nodes.Count) OrElse
                                       (N4 < 0 Or N4 >= Mesh.Nodes.Count) Then
                                        FileClose(FileId)
                                        Mesh.Nodes.Clear()
                                        Mesh.Panels.Clear()
                                        Exit Sub
                                    End If

                                    Panel = New Panel(N1, N2, N3, N4)

                                ElseIf UBound(Data) = 3 Then

                                    ' Triangles

                                    Dim N1 As Integer = CInt(Data(1)) + Offset - BaseIndex
                                    Dim N2 As Integer = CInt(Data(2)) + Offset - BaseIndex
                                    Dim N3 As Integer = CInt(Data(3)) + Offset - BaseIndex

                                    If (N1 < 0 Or N1 >= Mesh.Nodes.Count) OrElse
                                       (N2 < 0 Or N2 >= Mesh.Nodes.Count) OrElse
                                       (N3 < 0 Or N3 >= Mesh.Nodes.Count) Then
                                        FileClose(FileId)
                                        Mesh.Nodes.Clear()
                                        Mesh.Panels.Clear()
                                        Exit Sub
                                    End If

                                    Panel = New Panel(N1, N2, N3)

                                End If

                                If Panel IsNot Nothing Then

                                    Mesh.Panels.Add(Panel)
                                    Panel.IsSlender = IsSlender
                                    Panel.IsReversed = IsReversed
                                    Panel.IsPrimitive = IsPrimitive

                                End If

                            End If

                    End Select

NextLine:

                Loop

                FileClose(FileId)

            End If

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Loads the mesh from a connectivity file.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadStlFile(FilePath As String)

            Dim FileId As Integer = FreeFile()

            Dim InvalidData As Boolean = False

            Try

                If File.Exists(FilePath) Then

                    Mesh.Nodes.Clear()
                    Mesh.Panels.Clear()

                    FileOpen(FileId, FilePath, OpenAccess.Read)

                    Do While Not EOF(FileId)

                        Dim Line As String = LineInput(FileId)

                        Dim Keywords As String() = Line.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

                        ' Read a facet

                        If UBound(Keywords) >= 0 AndAlso Keywords(0).ToLower = "facet" Then

                            ' Disregard normal vector

                            Line = LineInput(FileId).Trim.ToLower

                            If Line = "outer loop" Then

                                Line = LineInput(FileId).ToLower

                                Dim Nodes(3) As Integer

                                Dim NodeCount As Integer = 0

                                ' Read until the en of the loop but maximum for nodes

                                Do While Line <> "endloop"

                                    ' Stop if there are already four nodes (only triangles or quads allowed)

                                    If NodeCount = 4 Then
                                        InvalidData = True
                                        GoTo CheckInvalid
                                    End If

                                    Dim Coordinates As String() = Line.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

                                    ' Read a vertex (the must be three coordinates prefixed by the word "vertex")

                                    If UBound(Coordinates) = 3 AndAlso Coordinates(0) = "vertex" Then

                                        NodeCount += 1

                                        Dim Vertex As New Vector3(CDbl(Coordinates(1)),
                                                                  CDbl(Coordinates(2)),
                                                                  CDbl(Coordinates(3)))

                                        ' Check if the vertex already exist

                                        Dim NodeFound As Boolean = False
                                        Dim NodeIndex As Integer = -1

                                        For Each Node In Mesh.Nodes

                                            NodeIndex += 1

                                            If Vertex.DistanceTo(Node.Position) < 0.00001 Then

                                                NodeFound = True

                                                Exit For

                                            End If

                                        Next

                                        ' If the node is not in the mesh, add it

                                        If Not NodeFound Then

                                            Dim NewNode As New NodalPoint
                                            NewNode.ReferencePosition = Vertex
                                            Mesh.Nodes.Add(NewNode)
                                            NodeIndex = Mesh.Nodes.Count - 1

                                        End If

                                        ' Store the node index for the panel

                                        Nodes(NodeCount - 1) = NodeIndex

                                    Else

                                        ' Stop if the vertex data format is wrong

                                        InvalidData = True
                                        GoTo CheckInvalid

                                    End If

                                    Line = LineInput(FileId).Trim.ToLower

                                Loop

                                ' TODO: use the normal to arrange the nodes in the correct order

                                Dim NewPanel As Panel = Nothing

                                If NodeCount = 3 Then
                                    NewPanel = New Panel(Nodes(0), Nodes(1), Nodes(2))
                                ElseIf NodeCount = 4 Then
                                    NewPanel = New Panel(Nodes(0), Nodes(1), Nodes(2), Nodes(3))
                                End If

                                If NewPanel IsNot Nothing Then
                                    NewPanel.IsSlender = False
                                    NewPanel.IsPrimitive = False
                                    Mesh.Panels.Add(NewPanel)
                                End If

                            End If

                        End If

                    Loop

                End If

CheckInvalid:

                If InvalidData Then

                    Mesh.Nodes.Clear()
                    Mesh.Panels.Clear()

                End If

            Catch

                Mesh.Nodes.Clear()
                Mesh.Panels.Clear()

            Finally

                FileClose(FileId)

                GenerateMesh()

            End Try

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

                            Dim Data As String() = Point.Split(":")

                            If Data.Length = 3 Then
                                Dim Node As New NodalPoint()
                                Node.ReferencePosition = New Vector3(Data(0),
                                                                     Data(1),
                                                                     Data(2))
                                Mesh.Nodes.Add(Node)
                            End If

                        Next

                        Dim PanelsData As String = IOXML.ReadString(reader, "Panels", "")

                        Dim Panels As String() = PanelsData.Split(";")

                        For Each PanelData In Panels

                            Dim Data As String() = PanelData.Split(":")

                            Dim Panel As Panel = Nothing
                            Dim IsSlender As Boolean = Data(0).Length > 0 AndAlso Data(0)(0) = "*"
                            Dim IsReversed As Boolean = Data(0).Length > 1 AndAlso Data(0)(1) = "*"
                            Dim IsPrimitive As Boolean = Data(0).Length > 2 AndAlso Data(0)(2) = "*"

                            If Data.Length = 4 Then

                                Panel = New Panel(Data(1),
                                                  Data(2),
                                                  Data(3))

                            ElseIf Data.Length = 5 Then

                                Panel = New Panel(Data(1),
                                                  Data(2),
                                                  Data(3),
                                                  Data(4))

                            End If

                            If Panel IsNot Nothing Then

                                Mesh.Panels.Add(Panel)
                                Panel.IsSlender = IsSlender
                                Panel.IsReversed = IsReversed
                                Panel.IsPrimitive = IsPrimitive

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

                    Panels = Panels & String.Format("{0}{1}{2}:{3:D}:{4:D}:{5:D};",
                                                    GetCharFlag(Panel.IsSlender),
                                                    GetCharFlag(Panel.IsReversed),
                                                    GetCharFlag(Panel.IsPrimitive),
                                                    Panel.N1,
                                                    Panel.N2,
                                                    Panel.N3)
                Else

                    Panels = Panels & String.Format("{0}{1}{2}:{3:D}:{4:D}:{5:D}:{6:D};",
                                                    GetCharFlag(Panel.IsSlender),
                                                    GetCharFlag(Panel.IsReversed),
                                                    GetCharFlag(Panel.IsPrimitive),
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
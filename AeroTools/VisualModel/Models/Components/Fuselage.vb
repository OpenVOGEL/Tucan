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
Imports OpenVOGEL.AeroTools.VisualModel.Models.Components.Basics
Imports SharpGL
Imports OpenVOGEL.AeroTools.VisualModel.Interface
Imports System.Xml
Imports OpenVOGEL.AeroTools.VisualModel.IO

Namespace VisualModel.Models.Components

    ''' <summary>
    ''' Contains information about a parent wing.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class WingAnchorInfo

        Public Property ParentID As Guid
        Public Property AnchorFromTip = False
        Public Property AnchorFromRoot As Boolean = False

    End Class

    ''' <summary>
    ''' Represents a point in the surface where a vertex has to be added.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AnchorPoint

        ''' <summary>
        ''' Curvilinear coordinate.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property s As Double

        ''' <summary>
        ''' Longitudinal coordinate.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Z As Double

        ''' <summary>
        ''' Perimeter of the section associated to this anchor point.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Used to find back the cartesian coordinates.</remarks>
        Public Property Perimeter As Double

    End Class

    ''' <summary>
    ''' Represents a series of segments that have to be projected on a extruded surface.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AnchorLine

        ''' <summary>
        ''' Lines of extrusion.
        ''' </summary>
        Public Property Lines As New List(Of ELine3)

        ''' <summary>
        ''' Surface coordinates anchor points.
        ''' </summary>
        Public Property Projections As New List(Of AnchorPoint)

        ''' <summary>
        ''' Indicates the initial position in the mesh where the points were located.
        ''' </summary>
        Public Property BeginIndex As Integer

        ''' <summary>
        ''' Indicates the end position in the mesh where the points were located.
        ''' </summary>
        Public Property EndIndex As Integer

        ''' <summary>
        ''' Gather information about the parent lifting surface.
        ''' </summary>
        Public Property WingAnchorInfo As WingAnchorInfo

        ''' <summary>
        ''' Inidcates if the projections where created.
        ''' </summary>
        Public ReadOnly Property Generated As Boolean
            Get
                Return Projections.Count > 1
            End Get
        End Property

        ''' <summary>
        ''' Moves all points the given displacements.
        ''' </summary>
        Public Sub TranslatePoints(ByVal dX As Double, ByVal dY As Double, ByVal dZ As Double)

            For Each Line In Lines

                Line.Point.X += dX
                Line.Point.Y += dY
                Line.Point.Z += dZ

            Next

        End Sub

        ''' <summary>
        ''' Creates a new instance with values read from an xml file.
        ''' </summary>
        Public Sub New(ByVal reader As XmlReader)
            ReadFromXML(reader)
        End Sub

        ''' <summary>
        ''' Creates a new instance.
        ''' </summary>
        Public Sub New()

        End Sub

#Region "IO"

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.IsStartElement Then

                    Select Case reader.Name

                        Case "Line"

                            Dim Line As New ELine3

                            Line.Point.X = IOXML.ReadDouble(reader, "Xp", 0.0)
                            Line.Point.Y = IOXML.ReadDouble(reader, "Yp", 0.0)
                            Line.Point.Z = IOXML.ReadDouble(reader, "Zp", 0.0)

                            Line.Direction.X = IOXML.ReadDouble(reader, "Xd", 0.0)
                            Line.Direction.Y = IOXML.ReadDouble(reader, "Yd", 0.0)
                            Line.Direction.Z = IOXML.ReadDouble(reader, "Zd", 0.0)

                            Lines.Add(Line)

                        Case "Info"

                            WingAnchorInfo = New WingAnchorInfo()

                            WingAnchorInfo.ParentID = New Guid(IOXML.ReadString(reader, "ParentID", Guid.NewGuid.ToString))
                            WingAnchorInfo.AnchorFromRoot = IOXML.ReadBoolean(reader, "Root", False)
                            WingAnchorInfo.AnchorFromTip = IOXML.ReadBoolean(reader, "Tip", False)

                    End Select

                End If

            End While

        End Sub

        Public Sub WriteToXML(ByRef writer As XmlWriter)

            For Each Line In Lines

                writer.WriteStartElement("Line")

                writer.WriteAttributeString("Xp", Line.Point.X.ToString)
                writer.WriteAttributeString("Yp", Line.Point.Y.ToString)
                writer.WriteAttributeString("Zp", Line.Point.Z.ToString)

                writer.WriteAttributeString("Xd", Line.Direction.X.ToString)
                writer.WriteAttributeString("Yd", Line.Direction.Y.ToString)
                writer.WriteAttributeString("Zd", Line.Direction.Z.ToString)
                writer.WriteEndElement()

            Next

            If Not IsNothing(WingAnchorInfo) Then

                writer.WriteStartElement("Info")

                writer.WriteAttributeString("ParentID", WingAnchorInfo.ParentID.ToString)
                writer.WriteAttributeString("Root", WingAnchorInfo.AnchorFromRoot)
                writer.WriteAttributeString("Tip", WingAnchorInfo.AnchorFromTip)
                writer.WriteEndElement()

            End If

        End Sub

#End Region

    End Class

    Public Class FeatureGrid

        Public e As Integer

        Public nns As Integer

        Public nnz As Integer

        Public zo As Double

        Public zf As Double

        Public Nodes As New List(Of EVector2)

        Public ParentAnchor As AnchorLine

        Public AnchorIndices() As Integer

    End Class

    ''' <summary>
    ''' Represents a planar cross section.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CrossSection

        Implements IComparable

        Public Property Z As Double = 0.0#

        Public Property Vertices As New List(Of EVector2)

        Private _Perimeter As Double

        Public Sub New()

        End Sub

        Public Sub New(ByRef Reader As XmlReader)
            ReadFromXML(Reader)
        End Sub

        Public Sub CalculatePerimeter()

            _Perimeter = 0

            For i = 0 To Vertices.Count - 2

                _Perimeter += Vertices(i).DistanceTo(Vertices(i + 1))

            Next

        End Sub

        ''' <summary>
        ''' Perimeter of the section.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Perimeter As Double
            Get
                Return _Perimeter
            End Get
        End Property

        ''' <summary>
        ''' Returns an interpolated point at the given line coordinate s.
        ''' </summary>
        ''' <param name="s">Curvilinear coordinate from 0 to 1</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPoint(ByVal s As Double) As EVector2

            If Vertices.Count > 1 Then

                Dim s_i As Double = 0
                Dim s_f As Double = 0
                Dim i_i As Integer = -1

                If s < 0 Then s = 0
                If s > 1 Then s = 1

                For i = 0 To Vertices.Count - 2

                    s_i = s_f
                    s_f += Vertices(i).DistanceTo(Vertices(i + 1))

                    If s_f / Perimeter >= s Then
                        i_i = i
                        Exit For
                    End If

                Next

                ' If it has not been assigned:

                If i_i = -1 Then

                    i_i = Vertices.Count - 2

                End If

                s_f /= Perimeter
                s_i /= Perimeter

                Dim f As Double = (s_f - s) / (s_f - s_i) ' 0 if s = s_f; 1 if s = s_o

                Dim point As EVector2 = New EVector2()

                point.X = f * Vertices(i_i).X + (1 - f) * Vertices(i_i + 1).X
                point.Y = f * Vertices(i_i).Y + (1 - f) * Vertices(i_i + 1).Y

                Return point

            ElseIf Vertices.Count = 1 Then

                Dim point As EVector2 = New EVector2()

                point.X = Vertices(0).X
                point.Y = Vertices(0).Y

                Return point

            Else

                Return Nothing

            End If

        End Function

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo

            Dim otherSection As CrossSection = obj

            If otherSection.Z > Z Then
                Return -1
            ElseIf otherSection.Z = Z
                Return 0
            Else
                Return 1
            End If

        End Function

#Region " IO "

        ''' <summary>
        ''' Writes the section to an XML file.
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Public Sub WriteToXML(ByRef writer As XmlWriter)

            writer.WriteAttributeString("Z", String.Format("{0}", Z))

            For Each Vertex In Vertices

                writer.WriteStartElement("Vertex")
                writer.WriteAttributeString("X", String.Format("{0}", Vertex.X))
                writer.WriteAttributeString("Y", String.Format("{0}", Vertex.Y))
                writer.WriteEndElement()

            Next

        End Sub

        ''' <summary>
        ''' Reads the wing from an XML file.
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <remarks></remarks>
        Public Sub ReadFromXML(ByRef reader As XmlReader)

            Vertices.Clear()

            Z = IOXML.ReadDouble(reader, "Z", 0.0)

            Dim subtree As XmlReader = reader.ReadSubtree()

            While subtree.ReadToFollowing("Vertex")

                Dim Vertex As New EVector2
                Vertex.X = IOXML.ReadDouble(reader, "X", 0.0)
                Vertex.Y = IOXML.ReadDouble(reader, "Y", 0.0)
                Vertices.Add(Vertex)

            End While

            CalculatePerimeter()

        End Sub

#End Region

    End Class

    ''' <summary>
    ''' All types of meshes.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MeshTypes

        UnstructuredTriangles = 0
        StructuredQuadrilaterals = 1

    End Enum

    ''' <summary>
    ''' Represents a surface defined by parallel planar cross sections.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Fuselage

        Inherits Surface

        Public Sub New()

            Mesh = New Mesh()

            ID = Guid.NewGuid
            Name = "Fuselage"
            CrossSections = New List(Of CrossSection)
            VisualProperties = New VisualProperties(ComponentTypes.etFuselage)
            VisualProperties.ThicknessMesh = 1.0
            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True
            AnchorLines = New List(Of AnchorLine)
            CrossRefinement = 10

        End Sub

        ''' <summary>
        ''' List containing cross sections used to extrude the surface.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property CrossSections As List(Of CrossSection)

        ''' <summary>
        ''' Gathers all ankor lines.
        ''' </summary>
        Public Property AnchorLines As List(Of AnchorLine)

        ''' <summary>
        ''' Defines the type of mesh
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MeshType As MeshTypes = MeshTypes.StructuredQuadrilaterals

        Private _LongitudinalRefinement As Integer = 2

        ''' <summary>
        ''' Indicates the  number of paneles in the transverse direction.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LongitudinalRefinement As Integer
            Set(value As Integer)
                If value >= 2 Then _LongitudinalRefinement = value
            End Set
            Get
                Return _LongitudinalRefinement
            End Get
        End Property

        Private _CrossRefinement As Integer = 2

        ''' <summary>
        ''' Indicates the  number of paneles in the transverse direction.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CrossRefinement As Integer
            Set(value As Integer)
                If value >= 2 Then _CrossRefinement = value
            End Set
            Get
                Return _CrossRefinement
            End Get
        End Property

        ''' <summary>
        ''' List containing cross sections used to extrude the surface.
        ''' </summary>
        ''' <remarks></remarks>
        Private _CrossSectionsToDisplay(0)() As EVector3

        Private Function GetPoint(ByVal z As Double, ByVal s As Double) As EVector3

            Dim i_f As Integer = 1
            Dim _z As Double = z

            If _z < CrossSections(0).Z Then
                _z = CrossSections(0).Z
            ElseIf _z > CrossSections(CrossSections.Count - 1).Z
                _z = CrossSections(CrossSections.Count - 1).Z
            End If

            For i = 1 To CrossSections.Count - 1

                If CrossSections(i).Z >= _z Then
                    i_f = i
                    Exit For
                End If

            Next

            Dim point_i = CrossSections(i_f).GetPoint(s)
            Dim point_f = CrossSections(i_f - 1).GetPoint(s)

            Dim f As Double = (CrossSections(i_f).Z - _z) / (CrossSections(i_f).Z - CrossSections(i_f - 1).Z)

            Return New EVector3((1 - f) * point_i.X + f * point_f.X, (1 - f) * point_i.Y + f * point_f.Y, _z)

        End Function

        ''' <summary>
        ''' Generates a triangular or quadrilateral mesh.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub GenerateMesh()

            Select Case MeshType
                Case MeshTypes.UnstructuredTriangles
                    GenerateTriangularMesh()
                Case MeshTypes.StructuredQuadrilaterals
                    GenerateQuadrilateralMesh()
            End Select

            ' Launch base sub to raise update event.

            MyBase.GenerateMesh()

        End Sub

        ''' <summary>
        ''' Creates a mesh of triangular panels.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub GenerateTriangularMesh()

            Throw New NotImplementedException

        End Sub

        ''' <summary>
        ''' Generates a structured mesh of quadrilaterals.
        ''' </summary>
        ''' <remarks>Cannot handle "Z supperposed" anchors.</remarks>
        Public Sub GenerateQuadrilateralMesh()

            Mesh.Panels.Clear()
            Mesh.Nodes.Clear()

            Try

                UpdateAnchors()

                ' First check: get valid anchors

                Dim ValidAnchors As New List(Of AnchorLine)

                For Each ank In AnchorLines

                    If ank.Projections.Count > 1 Then

                        Dim valid As Boolean = True

                        For i = 1 To ank.Projections.Count - 1

                            If ank.Projections(i).Z <= ank.Projections(i - 1).Z Then

                                valid = False

                            End If

                            If ank.Projections(i - 1).Z < CrossSections(0).Z Or ank.Projections(i).Z > CrossSections(CrossSections.Count - 1).Z Then

                                valid = False

                            End If

                        Next

                        If valid Then

                            ValidAnchors.Add(ank)

                        End If

                    End If

                Next

                ' Second check: remove supperposed anchors

                Dim supperposed As Boolean = True

                While supperposed

                    supperposed = False

                    For i = 0 To ValidAnchors.Count - 1

                        For j = 0 To ValidAnchors.Count - 1

                            If j <> i Then

                                Dim Zi0 As Integer = ValidAnchors(i).Projections(0).Z
                                Dim Zin As Integer = ValidAnchors(i).Projections(ValidAnchors(i).Projections.Count - 1).Z

                                Dim Zj0 As Integer = ValidAnchors(j).Projections(0).Z
                                Dim Zjn As Integer = ValidAnchors(j).Projections(ValidAnchors(j).Projections.Count - 1).Z

                                If (Zi0 >= Zj0 And Zi0 <= Zjn) Or (Zin >= Zj0 And Zin <= Zjn) Then
                                    supperposed = True
                                    ValidAnchors.RemoveAt(j)
                                    Exit For
                                End If

                            End If

                        Next

                        If supperposed Then Exit For

                    Next

                End While

                ' Generate grids:

                Dim npz As Integer = LongitudinalRefinement
                Dim nps As Integer = CrossRefinement ' number of panels in s direction
                Dim l As Double = CrossSections(CrossSections.Count - 1).Z - CrossSections(0).Z
                Dim addTrailingWake As Boolean = False

                Dim Grids As New List(Of FeatureGrid)

                For Each ValidAnchor In ValidAnchors

                    Dim Grid As New FeatureGrid

                    Dim e As Integer
                    Dim s_mean As Double = 0.0#

                    For Each point In ValidAnchor.Projections

                        s_mean += point.s / point.Perimeter

                    Next

                    s_mean /= ValidAnchor.Projections.Count

                    e = Math.Round(s_mean * nps)

                    If e < 0 Then e = 0

                    If e > nps Then e = nps - 1

                    Grid.e = e
                    Grid.nns = nps + 1
                    Grid.nnz = ValidAnchor.Projections.Count
                    Grid.zo = ValidAnchor.Projections(0).Z
                    Grid.zf = ValidAnchor.Projections(ValidAnchor.Projections.Count - 1).Z
                    Grid.ParentAnchor = ValidAnchor
                    ReDim Grid.AnchorIndices(Grid.nnz)

                    For i = 0 To ValidAnchor.Projections.Count - 1

                        Dim z As Double = ValidAnchor.Projections(i).Z

                        For j = 0 To nps

                            Dim s As Double = 0.0#
                            Dim ps As Double = ValidAnchor.Projections(i).s / ValidAnchor.Projections(i).Perimeter

                            If j <= e Then
                                s = ps * j / e
                            Else
                                s = ps + (1 - ps) * (j - e) / (nps - e)
                            End If

                            If j = 0 Then s = 0

                            If j = nps Then s = 1

                            Grid.Nodes.Add(New EVector2(z, s))

                        Next

                    Next

                    Grids.Add(Grid)

                Next

                ' Generate mesh progressively:

                If Grids.Count = 0 Then

                    For i = 0 To npz

                        Dim z As Double = CrossSections(0).Z + l * i / npz

                        For j = 0 To nps

                            Dim s As Double = j / nps

                            Mesh.Nodes.Add(New NodalPoint(GetPoint(z, s)))

                            If (i > 0) And (j > 0) Then

                                Dim N1 As Integer = (i - 1) * (nps + 1) + j
                                Dim N2 As Integer = i * (nps + 1) + j
                                Dim N3 As Integer = N2 + 1
                                Dim N4 As Integer = N1 + 1

                                Dim q4 As New Panel(N1, N2, N3, N4)

                                q4.IsPrimitive = False
                                q4.IsSlender = False

                                Mesh.Panels.Add(q4)

                            End If

                        Next

                    Next

                Else

                    ' If there are feature lines:

                    Dim dz As Double = l / npz
                    Dim p As Integer = 0
                    Dim g As Integer = Grids.Count - 1

                    For i = 0 To g

                        ' Add panels between first section and first grid

                        Dim zo As Double
                        Dim lz As Double

                        If i = 0 Then

                            zo = CrossSections(0).Z
                            lz = Grids(0).zo - zo

                            Dim nz As Integer = Math.Ceiling(lz / dz)

                            For k = 0 To nz - 1

                                Dim z As Double = zo + lz * k / nz

                                For j = 0 To nps

                                    Dim s As Double = j / nps + (Grids(0).Nodes(j).Y - j / nps) * k / nz

                                    Mesh.Nodes.Add(New NodalPoint(GetPoint(z, s)))

                                    If (p > 0) And (j > 0) Then

                                        Dim N1 As Integer = (p - 1) * (nps + 1) + j
                                        Dim N2 As Integer = p * (nps + 1) + j
                                        Dim N3 As Integer = N2 + 1
                                        Dim N4 As Integer = N1 + 1

                                        Dim q4 As New Panel(N1, N2, N3, N4)

                                        q4.IsPrimitive = False
                                        q4.IsSlender = False

                                        Mesh.Panels.Add(q4)

                                    End If

                                Next

                                p += 1

                            Next

                        End If

                        ' Add the grid:

                        Dim q As Integer = 0

                        For k = 0 To Grids(i).nnz - 1

                            For j = 0 To nps

                                Mesh.Nodes.Add(New NodalPoint(GetPoint(Grids(i).Nodes(q).X, Grids(i).Nodes(q).Y)))

                                ' Save the position p when j = e here!

                                If j = Grids(i).e Then

                                    Grids(i).AnchorIndices(k) = Mesh.Nodes.Count

                                End If

                                q += 1

                                If (p > 0) And (j > 0) Then

                                    Dim N1 As Integer = (p - 1) * (nps + 1) + j
                                    Dim N2 As Integer = p * (nps + 1) + j
                                    Dim N3 As Integer = N2 + 1
                                    Dim N4 As Integer = N1 + 1

                                    Dim q4 As New Panel(N1, N2, N3, N4)

                                    q4.IsPrimitive = False
                                    q4.IsSlender = False

                                    Mesh.Panels.Add(q4)

                                End If

                            Next

                            p += 1

                        Next

                        ' Add region between two grids:

                        If i < g Then

                            zo = Grids(i).zf
                            lz = Grids(i + 1).zo - zo

                            Dim nz As Integer = Math.Ceiling(lz / dz)

                            For k = 1 To nz - 1

                                For j = 0 To nps

                                    Dim z As Double = k / nz * lz + zo
                                    Dim s As Double = Grids(i).Nodes(j).Y + (Grids(i + 1).Nodes(j).Y - Grids(i).Nodes(j).Y) * j / nps

                                    Mesh.Nodes.Add(New NodalPoint(GetPoint(z, s)))

                                    If (p > 0) And (j > 0) Then

                                        Dim N1 As Integer = (p - 1) * (nps + 1) + j
                                        Dim N2 As Integer = p * (nps + 1) + j
                                        Dim N3 As Integer = N2 + 1
                                        Dim N4 As Integer = N1 + 1

                                        Dim q4 As New Panel(N1, N2, N3, N4)

                                        q4.IsPrimitive = False
                                        q4.IsSlender = False

                                        Mesh.Panels.Add(q4)

                                    End If

                                Next

                                p += 1

                            Next

                        End If

                        ' Add region at the end:

                        If i = g Then

                            zo = Grids(i).zf
                            lz = CrossSections(CrossSections.Count - 1).Z - zo

                            Dim nz As Integer = Math.Ceiling(lz / dz)
                            Dim nn As Integer = Grids(i).Nodes.Count - nps - 1

                            For k = 1 To nz

                                For j = 0 To nps

                                    Dim z As Double = k / nz * lz + zo
                                    Dim s As Double = Grids(i).Nodes(nn + j).Y + (j / nps - Grids(i).Nodes(nn + j).Y) * k / nz

                                    Mesh.Nodes.Add(New NodalPoint(GetPoint(z, s)))

                                    If (p > 0) And (j > 0) Then

                                        Dim N1 As Integer = (p - 1) * (nps + 1) + j
                                        Dim N2 As Integer = p * (nps + 1) + j
                                        Dim N3 As Integer = N2 + 1
                                        Dim N4 As Integer = N1 + 1

                                        Dim q4 As New Panel(N1, N2, N3, N4)

                                        q4.IsPrimitive = False
                                        q4.IsSlender = False

                                        Mesh.Panels.Add(q4)

                                    End If

                                Next

                                p += 1

                            Next

                        End If

                    Next

                End If

                ' If the last section has one node, replace rear quad panels by triangles:

                If CrossSections(CrossSections.Count - 1).Vertices.Count = 1 Then

                    For i = 0 To nps - 1

                        Mesh.Nodes.RemoveAt(Mesh.Nodes.Count - 1)
                        Mesh.Panels.RemoveAt(Mesh.Panels.Count - 1)

                    Next

                    Dim m As Integer = Mesh.Nodes.Count

                    For i = 1 To nps

                        Dim panel3 As New Panel(m, m - i, m - i - 1)

                        panel3.IsSlender = False
                        panel3.IsPrimitive = False

                        Mesh.Panels.Add(panel3)

                    Next

                End If

                ' add the anchors:

                For Each Grid In Grids

                    For i = 0 To Grid.nnz - 1

                        Mesh.Nodes.Add(New NodalPoint(Grid.ParentAnchor.Lines(i).Point.Clone))

                        If i > 0 Then

                            Dim N1 As Integer = Grid.AnchorIndices(i - 1)
                            Dim N2 As Integer = Grid.AnchorIndices(i)
                            Dim N3 As Integer = Mesh.Nodes.Count
                            Dim N4 As Integer = Mesh.Nodes.Count - 1

                            Dim q4 As New Panel(N1, N2, N3, N4)

                            q4.IsPrimitive = i = Grid.nnz - 1
                            q4.IsSlender = True

                            Mesh.Panels.Add(q4)

                        End If

                    Next

                Next

                ' If the first section only has one node, replace frontal quad panels by triangular panels:

                If CrossSections(0).Vertices.Count = 1 Then

                    For i = 0 To nps - 1

                        Mesh.Nodes.RemoveAt(0)
                        Mesh.Panels.RemoveAt(0)

                    Next

                    For i = 0 To Mesh.Panels.Count - 1

                        Mesh.Panels(i).N1 -= nps
                        Mesh.Panels(i).N2 -= nps
                        Mesh.Panels(i).N3 -= nps
                        Mesh.Panels(i).N4 -= nps

                    Next

                    For i = 0 To nps - 1

                        Dim panel3 As New Panel(1, i + 2, i + 3)

                        panel3.IsSlender = False
                        panel3.IsPrimitive = False

                        Mesh.Panels.Insert(0, panel3)

                    Next

                End If

                ' add symmetric part: new panels are inserted so that symmetric panels are consecutive.

                Dim n As Integer = Mesh.Nodes.Count - 1

                For i = 0 To n

                    Dim point As New NodalPoint(-Mesh.Nodes(i).Position.X, Mesh.Nodes(i).Position.Y, Mesh.Nodes(i).Position.Z) '  vertex.X, vertex.Y, 0

                    Mesh.Nodes.Add(point)

                Next

                Dim r As Integer = Mesh.Panels.Count - 1

                For i = 0 To r

                    Dim pnl As Panel

                    Dim index As Integer = 2 * i

                    If Mesh.Panels(index).IsTriangular Then

                        Dim N1 As Integer = Mesh.Panels(index).N1
                        Dim N2 As Integer = Mesh.Panels(index).N2
                        Dim N3 As Integer = Mesh.Panels(index).N3

                        Mesh.Panels(index).N1 = N1
                        Mesh.Panels(index).N2 = N2
                        Mesh.Panels(index).N3 = N3

                        pnl = New Panel(N1 + n + 1, N2 + n + 1, N3 + n + 1)

                    Else

                        Dim N1 As Integer = Mesh.Panels(index).N1
                        Dim N2 As Integer = Mesh.Panels(index).N2
                        Dim N3 As Integer = Mesh.Panels(index).N3
                        Dim N4 As Integer = Mesh.Panels(index).N4

                        Mesh.Panels(index).N1 = N1
                        Mesh.Panels(index).N2 = N2
                        Mesh.Panels(index).N3 = N3
                        Mesh.Panels(index).N4 = N4

                        pnl = New Panel(N1 + n + 1, N2 + n + 1, N3 + n + 1, N4 + n + 1)

                    End If

                    pnl.IsSlender = Mesh.Panels(index).IsSlender
                    pnl.IsPrimitive = Mesh.Panels(index).IsPrimitive

                    ' We don't reverse slender panels (because the same is done in wings, and this simplifies the canvection):

                    pnl.Reversed = Not pnl.IsSlender

                    Mesh.Panels.Insert(2 * i + 1, pnl)

                Next

                ' Change indices to 0-based

                For i = 0 To Mesh.Panels.Count - 1

                    Mesh.Panels(i).GlobalIndex = i

                Next

                For i = 0 To NumberOfPanels - 1

                    Mesh.Panels(i).N1 -= 1
                    Mesh.Panels(i).N2 -= 1
                    Mesh.Panels(i).N3 -= 1
                    Mesh.Panels(i).N4 -= 1

                Next

                ' Generate lattice

                Mesh.GenerateLattice()

                ' Rotate to align with global XYZ

                For Each p In Mesh.Nodes

                    Dim x As Double = p.Position.X
                    Dim y As Double = p.Position.Y
                    Dim z As Double = p.Position.Z

                    p.Position.X = z
                    p.Position.Y = x
                    p.Position.Z = y

                Next

                Mesh.Rotate(CenterOfRotation, Orientation.ToRadians)
                Mesh.Translate(Position)

                ' Generate cross sections to display:

                ReDim _CrossSectionsToDisplay(CrossSections.Count - 1)

                For i = 0 To CrossSections.Count - 1
                    ReDim _CrossSectionsToDisplay(i)(2 * CrossSections(i).Vertices.Count - 1)
                Next

                Dim csIndex As Integer = 0

                For Each CrossSection In CrossSections

                    Dim vIndex As Integer = 0

                    For Each Vertex In CrossSection.Vertices

                        _CrossSectionsToDisplay(csIndex)(vIndex) = New EVector3(CrossSection.Z, Vertex.X, Vertex.Y)

                        vIndex += 1

                    Next

                    vIndex = _CrossSectionsToDisplay(csIndex).Length - 1

                    For Each Vertex In CrossSection.Vertices

                        _CrossSectionsToDisplay(csIndex)(vIndex) = New EVector3(CrossSection.Z, -Vertex.X, Vertex.Y)

                        vIndex -= 1

                    Next

                    csIndex += 1

                Next

            Catch

                Mesh.Panels.Clear()
                Mesh.Nodes.Clear()

            End Try

        End Sub

        ''' <summary>
        ''' Regenerates the 3D model in Open GL.
        ''' </summary>
        ''' <param name="gl"></param>
        ''' <param name="SelectionMode"></param>
        ''' <param name="ElementIndex"></param>
        ''' <remarks></remarks>
        Public Overrides Sub Refresh3DModel(ByRef gl As OpenGL, Optional ByVal SelectionMode As SelectionModes = SelectionModes.smNoSelection, Optional ByVal ElementIndex As Integer = 0)

            'Version para OpenGL

            Dim i As Integer

            Dim Nodo As NodalPoint

            If VisualProperties.ShowSurface Then

                ' load homogeneous color:

                Dim SColor As New EVector3

                If Not Selected Then

                    SColor.X = VisualProperties.ColorSurface.R / 255
                    SColor.Y = VisualProperties.ColorSurface.G / 255
                    SColor.Z = VisualProperties.ColorSurface.B / 255
                    gl.Color(SColor.X, SColor.Y, SColor.Z, VisualProperties.Transparency)

                Else

                    ' default selected color is {255, 194, 14} (orange)

                    SColor.X = 1
                    SColor.Y = 0.76078
                    SColor.Z = 0.0549
                    gl.Color(SColor.X, SColor.Y, SColor.Z, 1.0)

                End If

                gl.InitNames()
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etFuselage, ElementIndex, EntityTypes.etQuadPanel, 0)
                Dim Count As Integer = 0

                For Each p In Mesh.Panels

                    gl.PushName(Code + Count)
                    gl.Begin(OpenGL.GL_TRIANGLES)

                    Nodo = Mesh.Nodes(p.N1)
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes(p.N2)
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = Mesh.Nodes(p.N3)
                    If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    If Not p.IsTriangular Then

                        Nodo = Mesh.Nodes(p.N3)
                        If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = Mesh.Nodes(p.N4)
                        If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                        Nodo = Mesh.Nodes(p.N1)
                        If VisualProperties.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                        gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    End If

                    gl.End()
                    gl.PopName()

                Next

            End If

            If SelectionMode = SelectionModes.smNodePicking Then

                gl.InitNames()
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etFuselage, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(VisualProperties.SizeNodes)
                gl.Color(VisualProperties.ColorNodes.R / 255, VisualProperties.ColorNodes.G / 255, VisualProperties.ColorNodes.B / 255)

                For i = 0 To Mesh.Nodes.Count - 1

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_POINTS)
                    Nodo = Mesh.Nodes(i)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)
                    gl.End()
                    gl.PopName()

                Next

            End If

            ' Represent lattice:

            If VisualProperties.ShowMesh Then

                gl.LineWidth(VisualProperties.ThicknessMesh)
                gl.Begin(OpenGL.GL_LINES)

                Dim Nodo1 As EVector3
                Dim Nodo2 As EVector3
                Dim Vector As EVector3
                Dim Carga As New EVector3

                gl.Color(VisualProperties.ColorMesh.R / 255, VisualProperties.ColorMesh.G / 255, VisualProperties.ColorMesh.B / 255)

                If Mesh.Nodes.Count > 0 Then

                    For i = 0 To Mesh.Lattice.Count - 1

                        Nodo1 = Mesh.Nodes(Mesh.Lattice(i).N1).Position
                        Nodo2 = Mesh.Nodes(Mesh.Lattice(i).N2).Position

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo2.X, Nodo2.Y, Nodo2.Z)

                    Next

                End If

                ' Normals:

                For i = 0 To Mesh.Panels.Count - 1

                    Nodo1 = Mesh.Panels(i).ControlPoint
                    Carga.Assign(Mesh.Panels(i).NormalVector)

                    gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                    gl.Vertex(Nodo1.X + Carga.X, Nodo1.Y + Carga.Y, Nodo1.Z + Carga.Z)

                Next

                gl.Color(VisualProperties.ColorVelocity.R / 255, VisualProperties.ColorVelocity.G / 255, VisualProperties.ColorVelocity.B / 255)

                If VisualProperties.ShowVelocityVectors Then

                    For i = 0 To Mesh.Panels.Count - 1

                        Nodo1 = Mesh.Panels(i).ControlPoint
                        Vector = Mesh.Panels(i).LocalVelocity

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + VisualProperties.ScaleVelocity * Vector.X, Nodo1.Y + VisualProperties.ScaleVelocity * Vector.Y, Nodo1.Z + VisualProperties.ScaleVelocity * Vector.Z)

                    Next

                End If

                gl.Color(VisualProperties.ColorLoads.R / 255, VisualProperties.ColorLoads.G / 255, VisualProperties.ColorLoads.B / 255)

                If VisualProperties.ShowLoadVectors Then

                    For i = 0 To Mesh.Panels.Count - 1

                        Nodo1 = Mesh.Panels(i).ControlPoint
                        Carga.Assign(Mesh.Panels(i).NormalVector)
                        Carga.Scale(VisualProperties.ScalePressure * Mesh.Panels(i).Cp)

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + Carga.X, Nodo1.Y + Carga.Y, Nodo1.Z + Carga.Z)

                    Next

                End If

                gl.End()

            End If

            ' Sections:

            'For i = 0 To _CrossSectionsToDisplay.Length - 1

            '    If _CrossSectionsToDisplay(i) IsNot Nothing Then

            '        gl.LineWidth(2 * VisualizationProperties.ThicknessMesh)
            '        gl.Begin(OpenGL.GL_LINES)

            '        gl.Color(0, 0, 0)

            '        For j = 1 To _CrossSectionsToDisplay(i).Length - 1

            '            gl.Vertex(_CrossSectionsToDisplay(i)(j - 1).X, _CrossSectionsToDisplay(i)(j - 1).Y, _CrossSectionsToDisplay(i)(j - 1).Z)
            '            gl.Vertex(_CrossSectionsToDisplay(i)(j).X, _CrossSectionsToDisplay(i)(j).Y, _CrossSectionsToDisplay(i)(j).Z)

            '        Next

            '        gl.End()

            '        gl.PointSize(VisualizationProperties.SizeNodes)
            '        gl.Color(Me.VisualizationProperties.ColorNodes.R / 255, Me.VisualizationProperties.ColorNodes.G / 255, Me.VisualizationProperties.ColorNodes.B / 255)

            '        gl.Begin(OpenGL.GL_POINTS)

            '        For j = 0 To _CrossSectionsToDisplay(i).Length - 1

            '            gl.Vertex(_CrossSectionsToDisplay(i)(j).X, _CrossSectionsToDisplay(i)(j).Y, _CrossSectionsToDisplay(i)(j).Z)

            '        Next

            '        gl.End()

            '    End If

            'Next

            'If _SelectedControlPoint >= 1 And _SelectedControlPoint <= Me.NumberOfPanels Then

            '    gl.PointSize(2 * Me.VisualizationProperties.SizeNodes)
            '    gl.Color(VisualizationProperties.ColorNodes.R / 255, VisualizationProperties.ColorNodes.G / 255, VisualizationProperties.ColorNodes.B / 255)
            '    gl.Begin(OpenGL.GL_POINTS)

            '    gl.Vertex(Me.QuadPanel(_SelectedControlPoint).PuntoDeControl.X, Me.QuadPanel(_SelectedControlPoint).PuntoDeControl.Y, Me.QuadPanel(_SelectedControlPoint).PuntoDeControl.Z)

            '    gl.End()

            'End If

        End Sub

        Public Overrides Function Clone() As Surface

            Return Nothing

        End Function

#Region " Anchors "

        Private Sub UpdateAnchors()

            For Each Anchor In AnchorLines

                Anchor.Projections.Clear()

                Dim OutOfBody As Boolean = False

                For Each Line In Anchor.Lines

                    Dim Point As EVector3 = Line.Point

                    Dim currentZ As Double = Point.Z

                    Dim currentP As EVector2 = Nothing

                    Dim lastP As New EVector2(Point.X, Point.Y)

                    ' Find intersection iteratively:

                    Dim iterations = 10

                    For k = 1 To iterations

                        If Not OutOfBody Then

                            If k > 1 Then

                                ' Adjust Zn so that it is closer to the real intersection:

                                Dim dx As Double = currentP.X - lastP.X
                                Dim dy As Double = currentP.Y - lastP.Y

                                currentZ += Line.Direction.Z * Math.Sqrt(dx * dx + dy * dy)

                                lastP.X = currentP.X
                                lastP.Y = currentP.Y

                            End If

                            ' generate a local section:

                            Dim bIndex As Integer = 0 ' begin section index
                            Dim eIndex As Integer = 0 ' end section index

                            OutOfBody = True

                            For i = 0 To CrossSections.Count - 2

                                If currentZ >= CrossSections(i).Z And currentZ < CrossSections(i + 1).Z Then

                                    OutOfBody = False
                                    bIndex = i
                                    eIndex = i + 1
                                    Exit For

                                End If

                            Next

                            ' generate an intermediate section:

                            Dim InterSection As New CrossSection

                            InterSection.Z = currentZ

                            Dim s As Double = 0
                            Dim z As Double = (currentZ - CrossSections(bIndex).Z) / (CrossSections(eIndex).Z - CrossSections(bIndex).Z)
                            Dim Resolution As Integer = Math.Ceiling(0.5 * (CrossSections(bIndex).Vertices.Count + CrossSections(eIndex).Vertices.Count))

                            For i = 0 To Resolution

                                s = i / Resolution

                                Dim bPoint As EVector2 = CrossSections(bIndex).GetPoint(s)
                                Dim ePoint As EVector2 = CrossSections(eIndex).GetPoint(s)

                                Dim iPoint As New EVector2((1 - z) * bPoint.X + z * ePoint.X, (1 - z) * bPoint.Y + z * ePoint.Y)

                                InterSection.Vertices.Add(iPoint)

                            Next

                            InterSection.CalculatePerimeter()

                            ' search the crossing point on the intermediate section:

                            Dim inPlanePoint As New EVector2(Point.X, Point.Y)
                            Dim inPlaneDirection As New EVector2(Line.Direction.X, Line.Direction.Y)
                            Dim intersectionCoordinates As New List(Of Double)
                            Dim intersectionPoints As New List(Of EVector2)
                            Dim indices As New List(Of Integer)
                            s = 0

                            For i = 0 To InterSection.Vertices.Count - 2

                                Dim coordinate As Double = EVector2.IntersectionCoordinate(inPlanePoint, inPlaneDirection, InterSection.Vertices(i), InterSection.Vertices(i + 1))
                                Dim increment As Double = InterSection.Vertices(i).DistanceTo(InterSection.Vertices(i + 1))

                                If Not Double.IsNaN(coordinate) Then

                                    intersectionCoordinates.Add(s + coordinate * increment)
                                    intersectionPoints.Add(New EVector2(InterSection.Vertices(i).X + coordinate * (InterSection.Vertices(i + 1).X - InterSection.Vertices(i).X),
                                                                        InterSection.Vertices(i).Y + coordinate * (InterSection.Vertices(i + 1).Y - InterSection.Vertices(i).Y)))
                                    indices.Add(i)

                                End If

                                s += increment

                            Next

                            ' pick the intersection closest to the point to be projected:

                            Dim currentS As Double = Double.NaN
                            Dim distance As Double = 0
                            Dim shortestDistance As Double = Double.MaxValue

                            For i = 0 To indices.Count - 1

                                distance = InterSection.Vertices(indices(i)).DistanceTo(inPlanePoint)
                                If distance < shortestDistance Then
                                    shortestDistance = distance
                                    currentS = intersectionCoordinates(i)
                                    currentP = intersectionPoints(i)
                                End If

                            Next

                            If Double.IsNaN(currentS) Then

                                OutOfBody = True
                                Exit For

                            End If

                            If Not OutOfBody And k = iterations Then

                                Dim anchorPoint As New AnchorPoint
                                anchorPoint.s = currentS
                                anchorPoint.Z = currentZ
                                anchorPoint.Perimeter = InterSection.Perimeter
                                Anchor.Projections.Add(anchorPoint)

                            End If

                        Else

                            Exit For

                        End If

                    Next

                Next

                If OutOfBody Then

                    Anchor.Projections.Clear()

                End If

            Next

            ' Updates the anchors in increasing order:

            AnchorLines.Sort(Function(x As AnchorLine, y As AnchorLine)
                                 If x.Projections.Count = 0 Then
                                     Return 0
                                 ElseIf y.Projections.Count = 0 Then
                                     Return 0
                                 ElseIf x.Projections(0).Z > y.Projections(0).Z Then
                                     Return 1
                                 Else
                                     Return -1
                                 End If
                             End Function)

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
                        MeshType = IOXML.ReadInteger(reader, "MeshType", MeshTypes.StructuredQuadrilaterals)
                        CrossRefinement = IOXML.ReadInteger(reader, "NPS", 10)
                        LongitudinalRefinement = IOXML.ReadInteger(reader, "NPZ", 10)

                    Case "CrossSection"

                        CrossSections.Add(New CrossSection(reader))

                    Case "VisualProperties"

                        VisualProperties.ReadFromXML(reader.ReadSubtree)

                    Case "Anchor"

                        AnchorLines.Add(New AnchorLine(reader.ReadSubtree))

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
            writer.WriteAttributeString("MeshType", CInt(MeshType))
            writer.WriteAttributeString("NPS", CInt(CrossRefinement))
            writer.WriteAttributeString("NPZ", CInt(LongitudinalRefinement))
            writer.WriteEndElement()

            ' Sections:

            For Each Section In CrossSections

                writer.WriteStartElement("CrossSection")
                Section.WriteToXML(writer)
                writer.WriteEndElement()

            Next

            ' Anchors

            For Each Anchor In AnchorLines

                writer.WriteStartElement("Anchor")
                Anchor.WriteToXML(writer)
                writer.WriteEndElement()

            Next

            ' Visual properties:

            writer.WriteStartElement("VisualProperties")
            VisualProperties.WriteToXML(writer)
            writer.WriteEndElement()

        End Sub

#End Region

#Region " Examples "

        Public Sub GenerateExample()

            CrossSections.Clear()

            ' section 0:

            Dim Section0 As New CrossSection
            Section0.Z = 0
            Dim nSegments As Integer = 15

            For i = 0 To nSegments

                Dim angle As Double = i / nSegments * Math.PI
                Section0.Vertices.Add(New EVector2(0.1 * Math.Sin(angle), 0.1 * Math.Cos(angle)))

            Next

            Section0.CalculatePerimeter()

            CrossSections.Add(Section0)

            ' section 1:

            Dim Section1 As New CrossSection
            Section1.Z = 1.5
            nSegments = 15

            For i = 0 To nSegments

                Dim angle As Double = i / nSegments * Math.PI
                Section1.Vertices.Add(New EVector2(1.0 * Math.Sin(angle), 1.0 * Math.Cos(angle)))

            Next

            Section1.CalculatePerimeter()

            CrossSections.Add(Section1)

            ' section 2:

            Dim Section2 As New CrossSection
            Section2.Z = 3
            nSegments = 15

            For i = 0 To nSegments

                Dim angle As Double = i / nSegments * Math.PI
                Section2.Vertices.Add(New EVector2(1.0 * Math.Sin(angle), 1.0 * Math.Cos(angle)))

            Next

            Section2.CalculatePerimeter()

            CrossSections.Add(Section2)

            ' section 3:

            Dim Section3 As New CrossSection
            Section3.Z = 4.5
            nSegments = 15

            For i = 0 To nSegments

                Dim angle As Double = i / nSegments * Math.PI
                Section3.Vertices.Add(New EVector2(0.1 * Math.Sin(angle), 0.1 * Math.Cos(angle)))

            Next

            Section3.CalculatePerimeter()

            CrossSections.Add(Section3)

        End Sub

        Public Sub GenerateModelForTesting_1()

            Mesh.Nodes.Clear()
            Mesh.Nodes.Add(New NodalPoint(2, 0, 0))
            Mesh.Nodes.Add(New NodalPoint(0, 1, 1))
            Mesh.Nodes.Add(New NodalPoint(0, -1, 1))
            Mesh.Nodes.Add(New NodalPoint(0, -1, -1))
            Mesh.Nodes.Add(New NodalPoint(0, 1, -1))
            Mesh.Nodes.Add(New NodalPoint(-2, 0, 0))

            Mesh.Panels.Clear()
            Mesh.Panels.Add(New Panel(1, 2, 3))
            Mesh.Panels.Add(New Panel(1, 3, 4))
            Mesh.Panels.Add(New Panel(1, 4, 5))
            Mesh.Panels.Add(New Panel(1, 5, 2))
            Mesh.Panels.Add(New Panel(6, 3, 2))
            Mesh.Panels.Add(New Panel(6, 4, 3))
            Mesh.Panels.Add(New Panel(6, 5, 4))
            Mesh.Panels.Add(New Panel(6, 2, 5))

            For Each p In Mesh.Panels
                p.IsPrimitive = False
                p.IsSlender = False
            Next

            Mesh.GenerateLattice()

        End Sub

        Public Sub GenerateModelForTesting_2()

            Mesh.Nodes.Clear()

            Dim n As Integer = 26

            Mesh.Nodes.Add(New NodalPoint(2, 0, 0))

            For i = 0 To n - 1
                Dim angle = 2 * i / (n - 1) * Math.PI
                Mesh.Nodes.Add(New NodalPoint(0, Math.Cos(angle), Math.Sin(angle)))
            Next

            Mesh.Nodes.Add(New NodalPoint(-2, 0, 0))

            Mesh.Panels.Clear()

            For i = 2 To n - 1

                Mesh.Panels.Add(New Panel(1, i, i + 1))

            Next

            Mesh.Panels.Add(New Panel(1, n, 2))

            For i = 2 To n - 1

                Mesh.Panels.Add(New Panel(n + 2, i + 1, i))

            Next

            Mesh.Panels.Add(New Panel(n + 2, 2, n))

            For Each p In Mesh.Panels
                p.IsPrimitive = False
                p.IsSlender = False
            Next

            Mesh.GenerateLattice()

        End Sub

        Public Sub GenerateModelForTesting_3()

            Mesh.Nodes.Clear()
            Mesh.Nodes.Add(New NodalPoint(0, 0, 0))

            Dim n As Integer = 4
            Dim r As Double = 1

            For i = 0 To n - 1
                Dim angle As Double = 2 * Math.PI * i / n
                Mesh.Nodes.Add(New NodalPoint(2, r * Math.Cos(angle), r * Math.Sin(angle)))
            Next

            For i = 0 To n - 1
                Dim angle As Double = 2 * Math.PI * i / n
                Mesh.Nodes.Add(New NodalPoint(4, r * Math.Cos(angle), r * Math.Sin(angle)))
            Next

            Mesh.Nodes.Add(New NodalPoint(6, 0, 0))

            Mesh.Panels.Clear()

            For i = 1 To n

                If i < n Then
                    Mesh.Panels.Add(New Panel(1, i + 1, i + 2))
                Else
                    Mesh.Panels.Add(New Panel(1, n + 1, 2))
                End If

            Next

            For i = 1 To n

                If i < n Then
                    Mesh.Panels.Add(New Panel(i + 1, i + n + 1, i + n + 2, i + 2))
                Else
                    Mesh.Panels.Add(New Panel(n + 1, 2 * n + 1, n + 2, 2))
                End If

            Next

            For i = 1 To n

                If i < n Then
                    Mesh.Panels.Add(New Panel(2 * n + 2, i + n + 2, i + n + 1))
                Else
                    Mesh.Panels.Add(New Panel(2 * n + 2, n + 2, 2 * n + 1))
                End If

            Next

            'Mesh.Panels.Add(New Panel(2, 6, 7, 3))
            'Mesh.Panels.Add(New Panel(3, 7, 8, 4))
            'Mesh.Panels.Add(New Panel(4, 8, 9, 5))
            'Mesh.Panels.Add(New Panel(5, 9, 6, 2))

            'Mesh.Panels.Add(New Panel(10, 7, 6))
            'Mesh.Panels.Add(New Panel(10, 8, 7))
            'Mesh.Panels.Add(New Panel(10, 9, 8))
            'Mesh.Panels.Add(New Panel(10, 6, 9))

            For Each p In Mesh.Panels
                p.IsPrimitive = False
                p.IsSlender = False
            Next

            Mesh.GenerateLattice()

        End Sub

#End Region

    End Class

End Namespace

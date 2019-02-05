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

Imports System.Drawing
Imports System.Xml
Imports OpenVOGEL.AeroTools.CalculationModel
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports SharpGL

Namespace VisualModel.Models.Components

    ''' <summary>
    ''' Represents a flat 3D rectangular surface where the airfield velocity is computed at
    ''' regular nodes.
    ''' </summary>
    Public Class VelocityPlane

        Private XYZVP(0) As Vector3
        Private XYZVI(0) As Vector3

        Public Origin As New Vector3

        Private Direction1 As New Vector3
        Private Direction2 As New Vector3

        Private Corner1 As New Vector3
        Private Corner2 As New Vector3
        Private Corner3 As New Vector3
        Private Corner4 As New Vector3

        Public NormalVector As New Vector3

        Public Extension1 As Double = 1.0F
        Public Extension2 As Double = 1.0F

        Public _Psi As Double = 0.5 * Math.PI
        Public _Tita As Double = 0.0#

        Private _NodesInDirection1 As Integer = 10
        Private _NodesInDirection2 As Integer = 10

        Public ColorNodes As Color = Color.DarkGray
        Public ColorVectors As Color = Color.LightBlue
        Public ColorSurface As Color = Color.LightGray

        Public Scale As Double = 0.1
        Public VectorThickness As Double = 1.0
        Public NodeSize As Double = 3

        Public Visible As Boolean = True
        Public InducedVelocity As Boolean = False ' Defines whether the total or induced velocity is represented.

        Public TreftSegments As New List(Of Solver.Solver.TrefftzSegment)

        Public Property Psi As Double
            Set(ByVal value As Double)
                _Psi = value
                NormalVector = Direction1.VectorProduct(Direction2)
            End Set
            Get
                Return _Psi
            End Get
        End Property

        Public Property Tita As Double
            Set(ByVal value As Double)
                _Tita = value
                NormalVector = Direction1.VectorProduct(Direction2)
            End Set
            Get
                Return _Tita
            End Get
        End Property

        Public ReadOnly Property GetNode(ByVal Node As Integer) As Vector3
            Get
                If 0 < Node <= XYZVP.Length Then
                    Return XYZVP(Node)
                Else
                    Return New Vector3
                End If
            End Get
        End Property

        Public Property GetInducedVelocity(ByVal Node As Integer) As Vector3
            Get
                If 0 < Node <= XYZVI.Length Then
                    Return XYZVI(Node)
                Else
                    Return New Vector3
                End If
            End Get
            Set(ByVal value As Vector3)
                If 0 < Node <= XYZVI.Length Then
                    XYZVI(Node) = value
                End If
            End Set
        End Property

        Public Property NodesInDirection1 As Integer
            Set(ByVal value As Integer)
                If value >= 2 Then
                    _NodesInDirection1 = value
                    GenerateMesh()
                End If
            End Set
            Get
                Return _NodesInDirection1
            End Get
        End Property

        Public Property NodesInDirection2 As Integer
            Set(ByVal value As Integer)
                If value >= 2 Then
                    _NodesInDirection2 = value
                    GenerateMesh()
                End If
            End Set
            Get
                Return _NodesInDirection2
            End Get
        End Property

        Public ReadOnly Property NumberOfNodes As Integer
            Get
                If Not IsNothing(XYZVP) Then
                    Return XYZVP.Length - 1
                Else
                    Return 0
                End If
            End Get
        End Property

        Private Sub AddControlPoint(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)

            Dim Dimension As Integer = XYZVP.Length

            ReDim Preserve Me.XYZVP(Dimension)
            ReDim Preserve Me.XYZVI(Dimension)

            Me.XYZVI(Dimension) = New Vector3

            Me.XYZVP(Dimension) = New Vector3
            Me.XYZVP(Dimension).X = X
            Me.XYZVP(Dimension).Y = Y
            Me.XYZVP(Dimension).Z = Z

        End Sub

        Private Sub AddControlPoint(ByVal Point As Vector3)

            Dim Dimension As Integer = XYZVP.Length

            ReDim Preserve Me.XYZVP(Dimension)
            ReDim Preserve Me.XYZVI(Dimension)

            Me.XYZVI(Dimension) = New Vector3

            Me.XYZVP(Dimension) = New Vector3
            Me.XYZVP(Dimension).X = Point.X
            Me.XYZVP(Dimension).Y = Point.Y
            Me.XYZVP(Dimension).Z = Point.Z

        End Sub

        Public Sub GenerateMesh()

            ReDim Preserve Me.XYZVP(0)

            Direction1.X = Math.Cos(Psi)
            Direction1.Y = Math.Sin(Psi)
            Direction1.Z = 0.0#

            Direction2.X = -Math.Sin(Tita) * Math.Sin(Psi)
            Direction2.Y = Math.Sin(Tita) * Math.Cos(Psi)
            Direction2.Z = Math.Cos(Tita)

            Dim Coordinate1 As Double
            Dim Coordinate2 As Double
            Dim Punto As New Vector3

            Direction1.Normalize()
            Direction2.Normalize()

            For i = 1 To NodesInDirection1

                Coordinate1 = Extension1 * ((i - 1) / (NodesInDirection1 - 1) - 0.5) 'De 0.5 a -0.5

                For j = 1 To NodesInDirection2

                    Coordinate2 = Extension2 * ((j - 1) / (NodesInDirection2 - 1) - 0.5) 'De 0.5 a -0.5

                    Punto.X = Coordinate1 * Direction1.X + Coordinate2 * Direction2.X + Origin.X
                    Punto.Y = Coordinate1 * Direction1.Y + Coordinate2 * Direction2.Y + Origin.Y
                    Punto.Z = Coordinate1 * Direction1.Z + Coordinate2 * Direction2.Z + Origin.Z

                    Me.AddControlPoint(Punto)

                Next

            Next

            Corner1.X = 0.5 * Extension1 * Direction1.X + 0.5 * Extension2 * Direction2.X + Origin.X
            Corner1.Y = 0.5 * Extension1 * Direction1.Y + 0.5 * Extension2 * Direction2.Y + Origin.Y
            Corner1.Z = 0.5 * Extension1 * Direction1.Z + 0.5 * Extension2 * Direction2.Z + Origin.Z

            Corner2.X = 0.5 * Extension1 * Direction1.X - 0.5 * Extension2 * Direction2.X + Origin.X
            Corner2.Y = 0.5 * Extension1 * Direction1.Y - 0.5 * Extension2 * Direction2.Y + Origin.Y
            Corner2.Z = 0.5 * Extension1 * Direction1.Z - 0.5 * Extension2 * Direction2.Z + Origin.Z

            Corner3.X = -0.5 * Extension1 * Direction1.X - 0.5 * Extension2 * Direction2.X + Origin.X
            Corner3.Y = -0.5 * Extension1 * Direction1.Y - 0.5 * Extension2 * Direction2.Y + Origin.Y
            Corner3.Z = -0.5 * Extension1 * Direction1.Z - 0.5 * Extension2 * Direction2.Z + Origin.Z

            Corner4.X = -0.5 * Extension1 * Direction1.X + 0.5 * Extension2 * Direction2.X + Origin.X
            Corner4.Y = -0.5 * Extension1 * Direction1.Y + 0.5 * Extension2 * Direction2.Y + Origin.Y
            Corner4.Z = -0.5 * Extension1 * Direction1.Z + 0.5 * Extension2 * Direction2.Z + Origin.Z

            NormalVector = Direction1.VectorProduct(Direction2)

        End Sub

        Public Sub Updte3DModel(ByRef gl As OpenGL)

            If Me.Visible Then

                gl.PointSize(Me.NodeSize)

                gl.Begin(OpenGL.GL_POINTS)
                gl.Color(ColorNodes.R / 255, ColorNodes.G / 255, ColorNodes.B / 255, 1)

                For i = 1 To NodesInDirection1 * NodesInDirection2
                    gl.Vertex(Me.GetNode(i).X, Me.GetNode(i).Y, Me.GetNode(i).Z)
                Next

                gl.End()

                gl.LineWidth(Me.VectorThickness)

                gl.Begin(OpenGL.GL_LINES)
                gl.Color(ColorVectors.R / 255, ColorVectors.G / 255, ColorVectors.B / 255, 1)

                For i = 1 To NodesInDirection1 * NodesInDirection2
                    gl.Vertex(Me.GetNode(i).X, Me.GetNode(i).Y, Me.GetNode(i).Z)
                    gl.Vertex(Me.GetNode(i).X + Me.Scale * Me.GetInducedVelocity(i).X, Me.GetNode(i).Y + Me.Scale * Me.GetInducedVelocity(i).Y, Me.GetNode(i).Z + Me.Scale * Me.GetInducedVelocity(i).Z)
                Next

                gl.End()

                gl.Color(ColorSurface.R / 255, ColorSurface.G / 255, ColorSurface.B / 255, 0.3)

                gl.Begin(OpenGL.GL_QUADS)

                gl.Vertex(Corner1.X, Corner1.Y, Corner1.Z)
                gl.Vertex(Corner2.X, Corner2.Y, Corner2.Z)
                gl.Vertex(Corner3.X, Corner3.Y, Corner3.Z)
                gl.Vertex(Corner4.X, Corner4.Y, Corner4.Z)

                gl.End()

                If TreftSegments.Count > 0 Then

                    gl.Begin(OpenGL.GL_LINES)
                    gl.Color(ColorVectors.R / 255, ColorVectors.G / 255, ColorVectors.B / 255, 1)

                    For i = 0 To TreftSegments.Count - 1

                        gl.Vertex(TreftSegments(i).Point1.X, TreftSegments(i).Point1.Y, TreftSegments(i).Point1.Z)
                        gl.Vertex(TreftSegments(i).Point2.X, TreftSegments(i).Point2.Y, TreftSegments(i).Point2.Z)

                        gl.Vertex(TreftSegments(i).Point1.X, TreftSegments(i).Point1.Y, TreftSegments(i).Point1.Z)
                        gl.Vertex(TreftSegments(i).Point1.X + Scale * TreftSegments(i).Velocity.X, TreftSegments(i).Point1.Y + Scale * TreftSegments(i).Velocity.Y, TreftSegments(i).Point1.Z + Scale * TreftSegments(i).Velocity.Z)

                    Next

                    gl.End()

                End If

            End If

        End Sub

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

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Origin")
            writer.WriteAttributeString("X", String.Format("{0}", Origin.X))
            writer.WriteAttributeString("Y", String.Format("{0}", Origin.Y))
            writer.WriteAttributeString("Z", String.Format("{0}", Origin.Z))
            writer.WriteEndElement()

            writer.WriteStartElement("Exstension")
            writer.WriteAttributeString("Extension1", String.Format("{0}", Extension1))
            writer.WriteAttributeString("Extension2", String.Format("{0}", Extension2))
            writer.WriteAttributeString("Nodes1", String.Format("{0}", _NodesInDirection1))
            writer.WriteAttributeString("Nodes2", String.Format("{0}", _NodesInDirection2))
            writer.WriteEndElement()

            writer.WriteStartElement("Orientation")
            writer.WriteAttributeString("Psi", String.Format("{0}", Psi))
            writer.WriteAttributeString("Tita", String.Format("{0}", Tita))
            writer.WriteEndElement()

            writer.WriteStartElement("VisualProperties")
            writer.WriteAttributeString("Scale", String.Format("{0}", Scale))
            writer.WriteAttributeString("VectorThickess", String.Format("{0}", VectorThickness))
            writer.WriteAttributeString("NodeSize", String.Format("{0}", NodeSize))

            writer.WriteAttributeString("NodeColorR", String.Format("{0}", ColorNodes.R))
            writer.WriteAttributeString("NodeColorG", String.Format("{0}", ColorNodes.G))
            writer.WriteAttributeString("NodeColorB", String.Format("{0}", ColorNodes.B))

            writer.WriteAttributeString("VectorColorR", String.Format("{0}", ColorVectors.R))
            writer.WriteAttributeString("VectorColorG", String.Format("{0}", ColorVectors.G))
            writer.WriteAttributeString("VectorColorB", String.Format("{0}", ColorVectors.B))

            writer.WriteAttributeString("Show", String.Format("{0}", CInt(Visible)))
            writer.WriteAttributeString("InducedVelocity", String.Format("{0}", CInt(InducedVelocity)))
            writer.WriteEndElement()

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "Origin"

                            Origin.X = CDbl(reader.GetAttribute("X"))
                            Origin.Y = CDbl(reader.GetAttribute("Y"))
                            Origin.Z = CDbl(reader.GetAttribute("Z"))

                        Case "Extension"

                            Extension1 = CDbl(reader.GetAttribute("Extension1"))
                            Extension2 = CDbl(reader.GetAttribute("Extension2"))
                            NodesInDirection1 = CInt(reader.GetAttribute("Nodes1"))
                            NodesInDirection2 = CInt(reader.GetAttribute("Nodes2"))

                        Case "Orientation"

                            Psi = CDbl(reader.GetAttribute("Psi"))
                            Tita = CDbl(reader.GetAttribute("Tita"))

                        Case "VisualProperties"

                            Scale = CDbl(reader.GetAttribute("Escale"))
                            VectorThickness = CDbl(reader.GetAttribute("VectorThickess"))
                            NodeSize = CDbl(reader.GetAttribute("NodeSize"))

                            Dim R As Integer = CDbl(reader.GetAttribute("NodeColorR"))
                            Dim G As Integer = CDbl(reader.GetAttribute("NodeColorG"))
                            Dim B As Integer = CDbl(reader.GetAttribute("NodeColorB"))

                            ColorNodes = Color.FromArgb(R, G, B)

                            R = CDbl(reader.GetAttribute("VectorColorR"))
                            G = CDbl(reader.GetAttribute("VectorColorG"))
                            B = CDbl(reader.GetAttribute("VectorColorB"))

                            ColorVectors = Color.FromArgb(R, G, B)

                            Visible = CBool(CInt(reader.GetAttribute("Show")))
                            InducedVelocity = CBool(CInt(reader.GetAttribute("InducedVelocity")))

                    End Select

                End If

            End While

        End Sub

    End Class

End Namespace
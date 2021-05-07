'Open VOGEL (openvogel.org)
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

        Private Points As New List(Of Vector3)
        Private Vectors As New List(Of Vector3)

        Public Origin As New Vector3

        Private Direction1 As New Vector3
        Private Direction2 As New Vector3

        Public ReadOnly Corner1 As New Vector3
        Public ReadOnly Corner2 As New Vector3
        Public ReadOnly Corner3 As New Vector3
        Public ReadOnly Corner4 As New Vector3

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
                Return Points(Node - 1)
            End Get
        End Property

        Public Property GetInducedVelocity(ByVal Node As Integer) As Vector3
            Get
                Return Vectors(Node - 1)
            End Get
            Set(ByVal value As Vector3)
                Vectors(Node - 1) = value
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
                Return Points.Count
            End Get
        End Property

        Private Sub AddControlPoint(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)

            Dim Point As New Vector3(X, Y, Z)
            Points.Add(Point)

            Dim Vector As New Vector3
            Vectors.Add(Vector)

        End Sub

        Private Sub AddControlPoint(ByVal Point As Vector3)

            Dim NewPoint As New Vector3(Point)
            Points.Add(NewPoint)

            Dim Vector As New Vector3
            Vectors.Add(Vector)

        End Sub

        Public Sub GenerateMesh()

            Points.Clear()
            Vectors.Clear()

            Direction1.X = Math.Cos(Psi)
            Direction1.Y = Math.Sin(Psi)
            Direction1.Z = 0.0#

            Direction2.X = -Math.Sin(Tita) * Math.Sin(Psi)
            Direction2.Y = Math.Sin(Tita) * Math.Cos(Psi)
            Direction2.Z = Math.Cos(Tita)

            Dim Coordinate1 As Double
            Dim Coordinate2 As Double
            Dim Point As New Vector3

            Direction1.Normalize()
            Direction2.Normalize()

            For i = 1 To NodesInDirection1

                Coordinate1 = Extension1 * ((i - 1) / (NodesInDirection1 - 1) - 0.5) 'from 0.5 to -0.5

                For j = 1 To NodesInDirection2

                    Coordinate2 = Extension2 * ((j - 1) / (NodesInDirection2 - 1) - 0.5) 'from 0.5 to -0.5

                    Point.X = Coordinate1 * Direction1.X + Coordinate2 * Direction2.X + Origin.X
                    Point.Y = Coordinate1 * Direction1.Y + Coordinate2 * Direction2.Y + Origin.Y
                    Point.Z = Coordinate1 * Direction1.Z + Coordinate2 * Direction2.Z + Origin.Z

                    AddControlPoint(Point)

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

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Origin")
            writer.WriteAttributeString("X", String.Format("{0}", Origin.X))
            writer.WriteAttributeString("Y", String.Format("{0}", Origin.Y))
            writer.WriteAttributeString("Z", String.Format("{0}", Origin.Z))
            writer.WriteEndElement()

            writer.WriteStartElement("Extension")
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
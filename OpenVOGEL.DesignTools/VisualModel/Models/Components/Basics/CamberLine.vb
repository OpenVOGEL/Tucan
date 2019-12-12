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

Imports System.Xml
Imports OpenVOGEL.AeroTools.IoHelper
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace VisualModel.Models.Components.Basics

    Public Class CamberLine

        Public Property Name As String = "Camber line"

        ''' <summary>
        ''' The identification number of this camber line.
        ''' </summary>
        ''' <returns></returns>
        Public Property ID As Guid

        ''' <summary>
        ''' The nodes if this camber line.
        ''' </summary>
        ''' <returns></returns>
        Public Property Nodes As New List(Of Vector2)

        ''' <summary>
        ''' Returns a NACA chamber point provided the position as fraction of the chord.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Returns the Y coordinate of the basic camber function given a chordwise coordinate.
        ''' </summary>
        ''' <param name="X">Chordwise coordinate</param>
        Public Function Y(ByVal X As Single) As Single

            For i = 1 To Nodes.Count - 1

                If Nodes(i - 1).X <= X AndAlso Nodes(i).X > X Then

                    Dim dy As Single = Nodes(i).Y - Nodes(i - 1).Y
                    Dim dx As Single = Nodes(i).X - Nodes(i - 1).X
                    Dim px As Single = X - Nodes(i - 1).X

                    Return Nodes(i - 1).Y + dy / dx * px

                End If

            Next

            Return 0.0

        End Function

        Public Sub EvaluatePoint(ByRef point As Vector2, ByVal x As Double, Optional Flapped As Boolean = False, Optional FlapChord As Single = 0.2, Optional FlapDeflection As Single = 0.0)

            point.X = x
            point.Y = Y(x)

            If Flapped And 1 - x <= FlapChord + 0.01 Then

                Dim y As Double = Me.Y(1 - FlapChord)
                point.Rotate(FlapDeflection, 1 - FlapChord, y)

            End If

        End Sub

        Public Sub Assign(ByVal Camber As CamberLine)

            Nodes.Clear()

            For i = 0 To Camber.Nodes.Count - 1

                Nodes.Add(New Vector2(Camber.Nodes(i).X, Camber.Nodes(i).Y))

            Next

        End Sub

        Public Function Clone() As CamberLine

            Dim newLine As New CamberLine
            newLine.Assign(Me)
            Return newLine

        End Function

        Public Sub GenerateNaca(MaxCamber As Single, MaxCamberX As Single, Optional n As Integer = 20)

            If (MaxCamberX > 0) AndAlso (MaxCamberX < 1) Then

                Nodes.Clear()

                For i = 0 To n

                    Dim _x As Single = i / n
                    Dim _y As Single = 0

                    If _x <= MaxCamberX Then

                        _y = MaxCamber * _x / MaxCamberX ^ 2 * (2 * MaxCamberX - _x)

                    ElseIf _x > MaxCamberX Then

                        _y = MaxCamber * (1.0 - _x) / (1.0 - MaxCamberX) ^ 2 * (1 + _x - 2 * MaxCamberX)

                    Else

                        _y = 0

                    End If

                    Nodes.Add(New Vector2(_x, _y))

                Next

            End If

        End Sub

        Public Sub WriteToXML(writer As XmlWriter)

            writer.WriteStartElement("Identity")
            writer.WriteAttributeString("Name", Name)
            writer.WriteAttributeString("ID", ID.ToString)
            writer.WriteEndElement()

            For Each node In Nodes

                writer.WriteStartElement("Node")

                writer.WriteAttributeString("X", node.X)
                writer.WriteAttributeString("Y", node.Y)

                writer.WriteEndElement()

            Next

        End Sub

        Public Sub ReadFromXML(reader As XmlReader)

            Nodes.Clear()

            While reader.Read

                If Not reader.NodeType = XmlNodeType.Element Then Continue While

                Select Case reader.Name

                    Case "Identity"

                        Name = reader.GetAttribute("Name")
                        ID = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))

                    Case "Node"

                        Dim x As Double = IOXML.ReadDouble(reader, "X", 0.0)
                        Dim y As Double = IOXML.ReadDouble(reader, "Y", 0.0)

                        Nodes.Add(New Vector2(x, y))

                End Select

            End While

        End Sub

    End Class

End Namespace


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

Imports System.Xml
Imports AeroTools.VisualModel.Models.Components.Basics

Namespace DataStore

    Public Module CamberLinesDatabase

        Public CamberLines As New List(Of CamberLine)

        Public Sub Initialize()

            CamberLines.Clear()
            Dim symmetric As New CamberLine
            symmetric.Name = "Symmetric"
            CamberLines.Add(symmetric)
            CamberLines(0).ID = Guid.Empty

        End Sub

        Public Function GetCamberLineFromID(ID As Guid) As CamberLine

            For i = 0 To CamberLines.Count - 1

                If CamberLines(i).ID.Equals(ID) Then
                    Return CamberLines(i)
                End If

            Next

            If CamberLines.Count > 0 Then
                Return CamberLines(0)
            Else
                Return Nothing
            End If

        End Function

        Public Sub RemoveCamberLine(ID As Guid)

            If ID <> Guid.Empty Then

                For i = 0 To CamberLines.Count - 1

                    If CamberLines(i).ID.Equals(ID) Then

                        CamberLines.RemoveAt(i)

                        Exit For

                    End If

                Next

            End If

        End Sub

        Public Sub WriteToXML(writer As XmlWriter)

            For Each line In CamberLines

                writer.WriteStartElement("CamberLine")

                line.WriteToXML(writer)

                writer.WriteEndElement()

            Next

        End Sub

        Public Sub ReadFromXML(reader As XmlReader)

            CamberLines.Clear()

            While reader.Read

                If Not reader.NodeType = XmlNodeType.Element Then Continue While

                Select Case reader.Name

                    Case "CamberLine"

                        Dim line As New CamberLine()

                        line.ReadFromXML(reader.ReadSubtree())

                        CamberLines.Add(line)

                End Select

            End While

        End Sub

    End Module

End Namespace


'#############################################################################
'OpenVOGEL (openvogel.org)
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

'' Standard .NET frameworks
'-----------------------------------------------------------------------------
Imports System.Xml

'' OpenVOGEL
'-----------------------------------------------------------------------------
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics

'#############################################################################
' Unit: CamberLineDatabase
'
' This unit provides a central storage for camber lines to be used throughout  
' the design process.
'#############################################################################
Namespace DataStore

    ''' <summary>
    '''  The database of chamber lines used during design.
    ''' </summary>
    Public Module CamberLinesDatabase

        ''' <summary>
        ''' The actual stack containing all camber lines in the project.
        ''' </summary>
        Public CamberLines As New List(Of CamberLine)

        ''' <summary>
        ''' Initializes the module.
        ''' </summary>
        Sub New()

            CamberLines.Clear()
            Dim symmetric As New CamberLine
            symmetric.Name = "Symmetric"
            CamberLines.Add(symmetric)
            CamberLines(0).ID = Guid.Empty

        End Sub

        ''' <summary>
        ''' Returns a camber line of the given unique Id.
        ''' </summary>
        ''' <param name="ID"></param>
        ''' <returns></returns>
        Public Function GetCamberLineFromId(ID As Guid) As CamberLine

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

        ''' <summary>
        ''' Removes the camberline given its unique Id.
        ''' </summary>
        ''' <param name="ID"></param>
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

        ''' <summary>
        ''' Writes the database in an XML file
        ''' </summary>
        ''' <param name="writer"></param>
        Public Sub WriteToXML(writer As XmlWriter)

            For Each Line In CamberLines

                writer.WriteStartElement("CamberLine")

                Line.WriteToXML(writer)

                writer.WriteEndElement()

            Next

        End Sub

        ''' <summary>
        ''' Writes the database in an XML file
        ''' </summary>
        ''' <param name="reader"></param>
        Public Sub ReadFromXML(reader As XmlReader)

            CamberLines.Clear()

            While reader.Read

                If Not reader.NodeType = XmlNodeType.Element Then Continue While

                Select Case reader.Name

                    Case "CamberLine"

                        Dim Line As New CamberLine()

                        Line.ReadFromXML(reader.ReadSubtree())

                        CamberLines.Add(Line)

                End Select

            End While

        End Sub

    End Module

End Namespace


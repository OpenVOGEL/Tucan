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

Namespace IoHelper

    Public Class IOXML

        ''' <summary>
        ''' Reads an attribute and converts it into integer.
        ''' </summary>
        ''' <param name="r"></param>
        ''' <param name="Name"></param>
        ''' <param name="DefValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReadInteger(ByRef r As XmlReader, ByVal Name As String, ByVal DefValue As Integer) As Integer

            Try
                Return CInt(r.GetAttribute(Name))
            Catch
                Return DefValue
            End Try

        End Function

        ''' <summary>
        ''' Reads an attribute and converts it into double taking into account the local decimal separator.
        ''' </summary>
        ''' <param name="r"></param>
        ''' <param name="Name"></param>
        ''' <param name="DefValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReadDouble(ByRef r As XmlReader, ByVal Name As String, ByVal DefValue As Double) As Double
            Try
                Dim value As String = r.GetAttribute(Name)
                If value.Contains(".") Then
                    value = value.Replace(".", My.Application.Culture.NumberFormat.NumberDecimalSeparator)
                ElseIf value.Contains(",") Then
                    value = value.Replace(",", My.Application.Culture.NumberFormat.NumberDecimalSeparator)
                End If
                Return CDbl(value)
            Catch
                Return DefValue
            End Try

        End Function

        ''' <summary>
        ''' Reads an attribute.
        ''' </summary>
        ''' <param name="r"></param>
        ''' <param name="Name"></param>
        ''' <param name="DefValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReadString(ByRef r As XmlReader, ByVal Name As String, ByVal DefValue As String) As String

            Try
                Dim value As String = r.GetAttribute(Name)
                If value IsNot Nothing Then
                    Return value
                End If
                Return DefValue
            Catch
                Return DefValue
            End Try

        End Function

        ''' <summary>
        ''' Reads an attribute and converts it into boolean.
        ''' </summary>
        ''' <param name="r"></param>
        ''' <param name="Name"></param>
        ''' <param name="DefValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ReadBoolean(ByRef r As XmlReader, ByVal Name As String, ByVal DefValue As Boolean) As Boolean

            Try
                Return CBool(r.GetAttribute(Name))
            Catch
                Return DefValue
            End Try

        End Function

    End Class

End Namespace


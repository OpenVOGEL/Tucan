Imports System.Xml

Namespace VisualModel.IO

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
            Catch ex As Exception
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
            Catch ex As Exception
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
                Return r.GetAttribute(Name)
            Catch ex As Exception
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
            Catch ex As Exception
                Return DefValue
            End Try

        End Function

    End Class

End Namespace


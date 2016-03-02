'Copyright (C) 2016 Guillermo Hazebrouck

Namespace UVLM.Models.Aero

    ''' <summary>
    ''' Represents a border from which wakes will be convected.
    ''' Nodes and rings should be provided in adyacent order.
    ''' </summary>
    Public Class Primitive

        Public Nodes As New List(Of Integer)
        Public Rings As New List(Of Integer)

    End Class


End Namespace
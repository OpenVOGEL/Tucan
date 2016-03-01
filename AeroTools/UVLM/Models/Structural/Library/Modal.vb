
Namespace UVLM.Models.Structural.Library

    Public Class Mode

        Public Shape As List(Of NodalDisplacement)

        Public w As Double
        Public K As Double
        Public M As Double
        Public C As Double
        Public Cc As Double

        Private _Index As Integer = 0

        ''' <summary>
        ''' Mode index
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Index As Integer
            Get
                Return _Index
            End Get
        End Property

        Public Sub New(ByVal Index As Integer)
            Shape = New List(Of NodalDisplacement)
            _Index = Index
        End Sub

    End Class

    Public Class ModalCoordinate

        ''' <summary>
        ''' Position
        ''' </summary>
        ''' <remarks></remarks>
        Public p As Double

        ''' <summary>
        ''' Velocity
        ''' </summary>
        ''' <remarks></remarks>
        Public v As Double

        ''' <summary>
        ''' Acceleration
        ''' </summary>
        ''' <remarks></remarks>
        Public a As Double

    End Class

    Public Class ModalCoordinates

        Private _ModalCoordinates As List(Of ModalCoordinate)

        ''' <summary>
        ''' Modal coordinates for mode a given mode
        ''' </summary>
        ''' <param name="mode"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Default Public Property Item(ByVal mode As Integer) As ModalCoordinate
            Get
                Return _ModalCoordinates(mode)
            End Get
            Set(ByVal value As ModalCoordinate)
                _ModalCoordinates(mode) = value
            End Set
        End Property

        Public ReadOnly Property Count As Integer
            Get
                Return _ModalCoordinates.Count
            End Get
        End Property

        Public Sub New(ByVal modes As Integer)

            _ModalCoordinates = New List(Of ModalCoordinate)(modes)

            For i = 1 To modes
                _ModalCoordinates.Add(New ModalCoordinate)
            Next

        End Sub

    End Class

End Namespace
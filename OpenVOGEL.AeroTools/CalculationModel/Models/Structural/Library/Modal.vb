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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes

Namespace CalculationModel.Models.Structural.Library

    Public Class Mode

        Public Shape As List(Of NodalDisplacement)

        Public W As Double
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
        Public P As Double

        ''' <summary>
        ''' Velocity
        ''' </summary>
        ''' <remarks></remarks>
        Public V As Double

        ''' <summary>
        ''' Acceleration
        ''' </summary>
        ''' <remarks></remarks>
        Public A As Double

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
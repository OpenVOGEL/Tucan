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

'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes

'#############################################################################
' Unit: Modal
'
' This unit provides a representation of modal shapes.
'#############################################################################
Namespace CalculationModel.Models.Structural.Library

    ''' <summary>
    ''' Represents a modal shape.
    ''' </summary>
    Public Class Mode

        ''' <summary>
        ''' The displacements of the shape at each node.
        ''' </summary>
        Public Shape As List(Of NodalDisplacement)

        ''' <summary>
        ''' The angular frequency
        ''' </summary>
        Public W As Double

        ''' <summary>
        ''' The modal stiffness.
        ''' </summary>
        Public K As Double

        ''' <summary>
        ''' The modal mass.
        ''' </summary>
        Public M As Double

        ''' <summary>
        ''' The modal damping (absolute)
        ''' </summary>
        Public C As Double

        ''' <summary>
        ''' The critical modal damping
        ''' </summary>
        Public Cc As Double

        ''' <summary>
        ''' The mode index
        ''' </summary>
        Public ReadOnly Property Index As Integer

        ''' <summary>
        ''' Generates a new mode with the given index.
        ''' </summary>
        ''' <param name="Index">Index of the mode in the external stack.</param>
        Public Sub New(ByVal Index As Integer)
            Shape = New List(Of NodalDisplacement)
            _Index = Index
        End Sub

    End Class

    ''' <summary>
    ''' Represents an instantaneous state of the modal shape.
    ''' </summary>
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

        ''' <summary>
        ''' The virtual work
        ''' </summary>
        ''' <remarks></remarks>
        Public W As Double

    End Class

    ''' <summary>
    ''' Represents a set of modal coordinates (one for each mode at a given time step).
    ''' </summary>
    Public Class ModalCoordinates

        Private _ModalCoordinates As List(Of ModalCoordinate)

        ''' <summary>
        ''' Modal coordinates for mode a given mode.
        ''' </summary>
        ''' <param name="Size">The number of modes</param>
        Default Public Property Item(ByVal Size As Integer) As ModalCoordinate
            Get
                Return _ModalCoordinates(Size)
            End Get
            Set(ByVal value As ModalCoordinate)
                _ModalCoordinates(Size) = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the size of the stack (the number of tracked modes).
        ''' </summary>
        Public ReadOnly Property Count As Integer
            Get
                Return _ModalCoordinates.Count
            End Get
        End Property

        ''' <summary>
        ''' Generates a stack of modal coordinates (one for each mode)
        ''' </summary>
        ''' <param name="Size">The number of modes</param>
        Public Sub New(ByVal Size As Integer)

            _ModalCoordinates = New List(Of ModalCoordinate)(Size)

            For I = 1 To Size
                _ModalCoordinates.Add(New ModalCoordinate)
            Next

        End Sub

    End Class

End Namespace
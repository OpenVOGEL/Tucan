'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

Namespace CalculationModel.Models.Structural.Library.Nodes

    ''' <summary>
    ''' Represents a nodal displacement.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NodalDisplacement

        ''' <summary>
        ''' Contains the nodal displacements (Dx, Dy, Dz, Rx, Ry, Rz)
        ''' </summary>
        ''' <remarks></remarks>
        Public Values(5) As Double

        Public Property Dx As Double
            Set(ByVal value As Double)
                Values(0) = value
            End Set
            Get
                Return Values(0)
            End Get
        End Property

        Public Property Dy As Double
            Set(ByVal value As Double)
                Values(1) = value
            End Set
            Get
                Return Values(1)
            End Get
        End Property

        Public Property Dz As Double
            Set(ByVal value As Double)
                Values(2) = value
            End Set
            Get
                Return Values(2)
            End Get
        End Property

        Public Property Rx As Double
            Set(ByVal value As Double)
                Values(3) = value
            End Set
            Get
                Return Values(3)
            End Get
        End Property

        Public Property Ry As Double
            Set(ByVal value As Double)
                Values(4) = value
            End Set
            Get
                Return Values(4)
            End Get
        End Property

        Public Property Rz As Double
            Set(ByVal value As Double)
                Values(5) = value
            End Set
            Get
                Return Values(5)
            End Get
        End Property

        Public Sub Clear()
            Dx = 0.0#
            Dy = 0.0#
            Dz = 0.0#
            Rx = 0.0#
            Ry = 0.0#
            Rz = 0.0#
        End Sub

        ''' <summary>
        ''' Calculates the virtual work of this displacement with the given load
        ''' </summary>
        ''' <param name="Load">Nodal load</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualWork(ByVal Load As NodalLoad) As Double

            Dim vw As Double = 0.0#
            For i = 0 To 5
                vw += Me.Values(i) * Load.Values(i)
            Next

            Return vw

        End Function

    End Class

End Namespace
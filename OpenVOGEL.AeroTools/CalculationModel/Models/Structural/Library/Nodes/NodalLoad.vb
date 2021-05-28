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

'#############################################################################
' Unit: NodalLoads
'
' This unit declares the components of a nodal load
'#############################################################################
Namespace CalculationModel.Models.Structural.Library.Nodes

    ''' <summary>
    ''' Represents a load on a structural node.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NodalLoad

        ''' <summary>
        ''' Contains the nodal loads (Px, Py, Pz, Tx, Ty, Tz)
        ''' </summary>
        ''' <remarks></remarks>
        Public Values(5) As Double

        Public Property Px As Double
            Set(ByVal value As Double)
                Values(0) = value
            End Set
            Get
                Return Values(0)
            End Get
        End Property

        Public Property Py As Double
            Set(ByVal value As Double)
                Values(1) = value
            End Set
            Get
                Return Values(1)
            End Get
        End Property

        Public Property Pz As Double
            Set(ByVal value As Double)
                Values(2) = value
            End Set
            Get
                Return Values(2)
            End Get
        End Property

        Public Property Tx As Double
            Set(ByVal value As Double)
                Values(3) = value
            End Set
            Get
                Return Values(3)
            End Get
        End Property

        Public Property Ty As Double
            Set(ByVal value As Double)
                Values(4) = value
            End Set
            Get
                Return Values(4)
            End Get
        End Property

        Public Property Tz As Double
            Set(ByVal value As Double)
                Values(5) = value
            End Set
            Get
                Return Values(5)
            End Get
        End Property

        Public Sub Clear()
            Px = 0.0#
            Py = 0.0#
            Pz = 0.0#
            Tx = 0.0#
            Ty = 0.0#
            Tz = 0.0#
        End Sub

        ''' <summary>
        ''' Calculates the virtual work of this load with the given displacement.
        ''' </summary>
        ''' <param name="Displacement">Nodal displacement</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualWork(ByVal Displacement As NodalDisplacement) As Double

            Dim Work As Double = 0.0#
            For I = 0 To 5
                Work += Values(I) * Displacement.Values(I)
            Next

            Return Work

        End Function


    End Class

End Namespace

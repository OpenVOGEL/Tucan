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
' Unit: Primitives
'
' This unit provides the definition of a sheding edge.
'#############################################################################
Namespace CalculationModel.Models.Aero

    ''' <summary>
    ''' Represents a border from which wakes will be convected (sheding edge).
    ''' Nodes and rings should be provided in adyacent order.
    ''' </summary>
    Public Class Primitive

        Public Nodes As New List(Of Integer)
        Public Rings As New List(Of Integer)

    End Class

End Namespace
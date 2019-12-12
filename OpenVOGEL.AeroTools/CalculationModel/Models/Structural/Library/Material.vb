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

Namespace CalculationModel.Models.Structural.Library

    ''' <summary>
    ''' Represents a material.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Material

        ''' <summary>
        ''' Young module [N/m²]
        ''' </summary>
        ''' <remarks></remarks>
        Public E As Double ' 210GPa

        ''' <summary>
        ''' Transverse module [N/m²]
        ''' </summary>
        ''' <remarks></remarks>
        Public G As Double ' 100GPa

        ''' <summary>
        ''' Poisson coefficient
        ''' </summary>
        ''' <remarks></remarks>
        Public v As Double = 0.3

        ''' <summary>
        ''' Material density [kg/m³]
        ''' </summary>
        ''' <remarks></remarks>
        Public Density As Double = 7800

        Public Sub New()
            E = 69000000000 ' Aluminium
            v = 0.3
            G = 0.5 * E / (1 + v)
        End Sub

        Public Sub Assign(ByVal Material As Material)

            E = Material.E
            G = Material.G
            v = Material.v
            Density = Material.Density

        End Sub

    End Class

End Namespace
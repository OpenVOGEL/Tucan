'## Open VOGEL ##
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Imports MathTools.Algebra.EuclideanSpace

Namespace UVLM.Models.Aero

    ''' <summary>
    ''' Gathers the resultants of aerodynamic forces per type.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TotalAirLoads

        Public Property Area As Double

        Public Property Force As New EVector3
        Public Property Moment As New EVector3

        Public Property CL As Double
        Public Property CDi As Double
        Public Property CDp As Double

        ' old:

        Public Property SlenderForce As New EVector3
        Public Property SlenderMoment As New EVector3

        Public Property InducedDrag As New EVector3
        Public Property InducedMoment As New EVector3

        Public Property SkinDrag As New EVector3
        Public Property SkinMoment As New EVector3

        Public Property BodyForce As New EVector3
        Public Property BodyMoment As New EVector3

    End Class

End Namespace
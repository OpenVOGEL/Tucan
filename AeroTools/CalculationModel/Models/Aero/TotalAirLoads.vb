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

Imports MathTools.Algebra.EuclideanSpace

Namespace CalculationModel.Models.Aero

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

        ''' <summary>
        ''' Adds the given total load scaled by the surface.
        ''' </summary>
        ''' <param name="Airloads"></param>
        Public Sub Add(Airloads As TotalAirLoads)

            Force.Add(Airloads.Area * Airloads.Force)
            Moment.Add(Airloads.Area * Airloads.Moment)

            CL += Airloads.Area * Airloads.CL
            CDi += Airloads.Area * Airloads.CDi
            CDp += Airloads.Area * Airloads.CDp

            SlenderForce.Add(Airloads.Area * Airloads.SlenderForce)
            SlenderMoment.Add(Airloads.Area * Airloads.SlenderMoment)

            InducedDrag.Add(Airloads.Area * Airloads.InducedDrag)
            InducedMoment.Add(Airloads.Area * Airloads.InducedMoment)

            SkinDrag.Add(Airloads.Area * Airloads.SkinDrag)
            SkinMoment.Add(Airloads.Area * Airloads.SkinMoment)

            BodyForce.Add(Airloads.Area * Airloads.BodyForce)
            BodyMoment.Add(Airloads.Area * Airloads.BodyMoment)

        End Sub

        Public Sub Clear()

            Force.SetToCero()
            Moment.SetToCero()

            CL = 0.0
            CDi = 0.0
            CDp = 0.0

            SlenderForce.SetToCero()
            SlenderMoment.SetToCero()

            InducedDrag.SetToCero()
            InducedMoment.SetToCero()

            SkinDrag.SetToCero()
            SkinMoment.SetToCero()

            BodyForce.SetToCero()
            BodyMoment.SetToCero()

        End Sub

    End Class

End Namespace
'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2020 George Lazarou (george.sp.lazarou@gmail.com)

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

Imports System.Math

Namespace CalculationModel.Settings

    Public Class StandardAtmosphere

        Public Property Temperature As Double
        Public Property Pressure As Double
        Public Property Density As Double
        Public Property DynamicVisc As Double
        Public Property KinematicVisc As Double
        Public Property SoundSpeed As Double

        Public Sub New()
            CalculateAirProperties(0.0#)
        End Sub

        Public Sub New(Altitude As Double)
            CalculateAirProperties(Altitude)
        End Sub

        Public Sub CalculateAirProperties(Altitude As Double)

            Temperature = 288.16 - 0.0065 * Altitude 'Unit: [K]
            Pressure = 101325 * (1 - 2.25569 * 10 ^ (-5) * Altitude) ^ 5.25616 'Unit: [Pa]
            Density = 1.225 * (Temperature / 288.16) ^ 4.2561 'Unit: [kg/m3]
            DynamicVisc = (1.458 * 10 ^ (-6) * Temperature ^ 1.5) / (Temperature + 110.4) 'Unit: [kg/m*s]
            KinematicVisc = DynamicVisc / Density 'Unit: [m2/s]
            SoundSpeed = Sqrt(401.874018 * Temperature) 'Unit: [m/s]

        End Sub

    End Class

End Namespace
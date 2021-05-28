'#############################################################################
'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 George Lazarou (george.sp.lazarou@gmail.com)

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

' This unit has been incorporated by Georgios Lazarou.

'#############################################################################
' Unit: AirProperties
'
' This unit provides model of the standard atmosphere
'#############################################################################
Namespace CalculationModel.Settings

    ''' <summary>
    ''' The standard atmosphere
    ''' </summary>
    Public Class StandardAtmosphere

        ''' <summary>
        ''' The temperature [K]
        ''' </summary>
        Public ReadOnly Property Temperature As Double

        ''' <summary>
        ''' The static pressure [Pa]
        ''' </summary>
        Public ReadOnly Property Pressure As Double

        ''' <summary>
        ''' The density [kg/m³]
        ''' </summary>
        Public ReadOnly Property Density As Double

        ''' <summary>
        ''' The dynamic viscosity [N/m.s]
        ''' </summary>
        Public ReadOnly Property DynamicVisc As Double

        ''' <summary>
        '''  The dynamic viscosity [m²/s]
        ''' </summary>
        Public ReadOnly Property KinematicVisc As Double

        ''' <summary>
        ''' The speed of sound [m/s]
        ''' </summary>
        Public ReadOnly Property SoundSpeed As Double

        ''' <summary>
        ''' The altitude [m]
        ''' </summary>
        Public ReadOnly Property Altitude As Double

        ''' <summary>
        ''' Generates the properties at sea level (altitude = 0m).
        ''' </summary>
        Public Sub New()
            CalculateAirProperties(0.0#)
        End Sub

        ''' <summary>
        ''' Generates the properties at the given altitude
        ''' </summary>
        Public Sub New(Altitude As Double)
            CalculateAirProperties(Altitude)
        End Sub

        ''' <summary>
        ''' Calculates the properties for a given altitude
        ''' </summary>
        ''' <param name="Altitude"></param>
        Public Sub CalculateAirProperties(Altitude As Double)

            _Altitude = Altitude
            _Temperature = 288.16 - 0.0065 * Altitude 'Unit: [K]
            _Pressure = 101325 * (1 - 2.25569 * 10 ^ (-5) * Altitude) ^ 5.25616 'Unit: [Pa]
            _Density = 1.225 * (_Temperature / 288.16) ^ 4.2561 'Unit: [kg/m3]
            _DynamicVisc = (1.458 * 10 ^ (-6) * _Temperature ^ 1.5) / (_Temperature + 110.4) 'Unit: [kg/m*s]
            _KinematicVisc = _DynamicVisc / _Density 'Unit: [m2/s]
            _SoundSpeed = Math.Sqrt(401.874018 * _Temperature) 'Unit: [m/s]

        End Sub

    End Class

End Namespace
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

Namespace Magnitudes

    Public Enum Magnitudes As Short

        Time
        'Mass
        Length
        Area
        Velocity
        Density
        'Acceleration
        Force
        Moment
        Angular
        Pressure
        Dimensionless

    End Enum

    Public Interface IPhysicalMagnitude

        Property Value As Double

        Property DefaultUnitValue As Double

        ReadOnly Property Label As String

        ReadOnly Property Magnitude As Magnitudes

        Sub Assign(Magnitude As IPhysicalMagnitude)

    End Interface

    Public Class Units

        Public Shared Function GetInstanceOf(Magnitude As Magnitudes) As IPhysicalMagnitude

            Select Case Magnitude
                Case Magnitudes.Time
                    Return New TimeMagnitude()
                Case Magnitudes.Length
                    Return New LengthMagnitude()
                Case Magnitudes.Area
                    Return New AreaMagnitude()
                Case Magnitudes.Density
                    Return New DensityMagnitude()
                Case Magnitudes.Force
                    Return New ForceMagnitude()
                Case Magnitudes.Pressure
                    Return New PressureMagnitude()
                Case Magnitudes.Moment
                    Return New MomentMagnitude()
                Case Magnitudes.Velocity
                    Return New VelocityMagnitude()
                Case Magnitudes.Angular
                    Return New AngularMagnitude()
                Case Magnitudes.Dimensionless
                    Return New DimensionlessMagnitude()
                Case Else
                    Return Nothing
            End Select

        End Function

    End Class

    ''' <summary>
    ''' Represents a time magnitude.
    ''' </summary>
    Public Class TimeMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            Seconds
            Minutes
            Hours
            Days

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.Seconds

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Time Then
                Dim Mag As TimeMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.Seconds
                    _Value = _DefaultUnitValue
                Case Units.Minutes
                    _Value = _DefaultUnitValue / 60.0#
                Case Units.Hours
                    _Value = _DefaultUnitValue / 3600.0#
                Case Units.Days
                    _Value = _DefaultUnitValue / 86400.0#
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.Seconds
                    _DefaultUnitValue = _Value
                Case Units.Minutes
                    _DefaultUnitValue = _Value * 60.0#
                Case Units.Hours
                    _DefaultUnitValue = _Value * 3600.0#
                Case Units.Days
                    _DefaultUnitValue = _Value * 86400.0#
                Case Else
                    _DefaultUnitValue = _Value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Time
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.Days
                        Return "days"
                    Case Units.Hours
                        Return "hrs"
                    Case Units.Minutes
                        Return "min"
                    Case Units.Seconds
                        Return "sec"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a length magnitude.
    ''' </summary>
    Public Class LengthMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            Meters
            Milimeters
            Centimeters
            Kilometers
            Feet
            Inches
            Miles
            NauticMiles

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.Meters

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Length Then
                Dim Mag As LengthMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.Meters
                    _Value = _DefaultUnitValue
                Case Units.Milimeters
                    _Value = _DefaultUnitValue * 1000
                Case Units.Centimeters
                    _Value = _DefaultUnitValue * 100
                Case Units.Kilometers
                    _Value = _DefaultUnitValue / 1000
                Case Units.Feet
                    _Value = _DefaultUnitValue / 0.3048
                Case Units.Inches
                    _Value = _DefaultUnitValue / 0.0254
                Case Units.Miles
                    _Value = _DefaultUnitValue / 1609.344
                Case Units.NauticMiles
                    _Value = _DefaultUnitValue / 1853.184
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.Meters
                    _DefaultUnitValue = value
                Case Units.Milimeters
                    _DefaultUnitValue = value / 1000
                Case Units.Centimeters
                    _DefaultUnitValue = value / 100
                Case Units.Kilometers
                    _DefaultUnitValue = value * 1000
                Case Units.Feet
                    _DefaultUnitValue = value * 0.3048
                Case Units.Inches
                    _DefaultUnitValue = value * 0.0254
                Case Units.Miles
                    _DefaultUnitValue = value * 1609.344
                Case Units.NauticMiles
                    _DefaultUnitValue = value * 1853.184
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Length
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.Meters
                        Return "m"
                    Case Units.Feet
                        Return "ft"
                    Case Units.Inches
                        Return "in"
                    Case Units.Milimeters
                        Return "mm"
                    Case Units.Centimeters
                        Return "cm"
                    Case Units.Kilometers
                        Return "km"
                    Case Units.Miles
                        Return "miles"
                    Case Units.NauticMiles
                        Return "kts"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents an area magnitude.
    ''' </summary>
    Public Class AreaMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            SquareMeters
            SquareMilimeters
            SquareCentimeters
            SquareKilometers
            SquareFeet
            SquareInches
            SquareMiles
            SquareNauticMiles

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.SquareMeters

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Area Then
                Dim Mag As AreaMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.SquareMeters
                    _Value = _DefaultUnitValue
                Case Units.SquareMilimeters
                    _Value = _DefaultUnitValue * 1000000
                Case Units.SquareCentimeters
                    _Value = _DefaultUnitValue * 10000
                Case Units.SquareKilometers
                    _Value = _DefaultUnitValue / 1000000
                Case Units.SquareFeet
                    _Value = _DefaultUnitValue / 0.09290304
                Case Units.SquareInches
                    _Value = _DefaultUnitValue / 0.00064516
                Case Units.SquareMiles
                    _Value = _DefaultUnitValue / 2589988.110336
                Case Units.SquareNauticMiles
                    _Value = _DefaultUnitValue / 3434290.937856
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.SquareMeters
                    _DefaultUnitValue = value
                Case Units.SquareMilimeters
                    _DefaultUnitValue = value / 1000000
                Case Units.SquareCentimeters
                    _DefaultUnitValue = value / 10000
                Case Units.SquareKilometers
                    _DefaultUnitValue = value * 1000000
                Case Units.SquareFeet
                    _DefaultUnitValue = value * 0.09290304
                Case Units.SquareInches
                    _DefaultUnitValue = value * 0.00064516
                Case Units.SquareMiles
                    _DefaultUnitValue = value * 2589988.110336
                Case Units.SquareNauticMiles
                    _DefaultUnitValue = value * 3434290.937856
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Area
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.SquareMeters
                        Return "m²"
                    Case Units.SquareFeet
                        Return "ft²"
                    Case Units.SquareInches
                        Return "in²"
                    Case Units.SquareMilimeters
                        Return "mm²"
                    Case Units.SquareCentimeters
                        Return "cm²"
                    Case Units.SquareKilometers
                        Return "km²"
                    Case Units.SquareMiles
                        Return "miles²"
                    Case Units.SquareNauticMiles
                        Return "kts²"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a length magnitude.
    ''' </summary>
    Public Class VelocityMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            MetersPerSecond
            KilometersPerHour
            MilesPerHour
            Knots

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.MetersPerSecond

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Length Then
                Dim Mag As VelocityMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.MetersPerSecond
                    _Value = _DefaultUnitValue
                Case Units.KilometersPerHour
                    _Value = _DefaultUnitValue * 3.6
                Case Units.MilesPerHour
                    _Value = _DefaultUnitValue * 2.23693629
                Case Units.Knots
                    _Value = _DefaultUnitValue * 1.94384449244
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.MetersPerSecond
                    _DefaultUnitValue = value
                Case Units.KilometersPerHour
                    _DefaultUnitValue = value / 3.6
                Case Units.MilesPerHour
                    _DefaultUnitValue = value / 2.23693629
                Case Units.Knots
                    _DefaultUnitValue = value / 1.94384449244
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Velocity
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.MetersPerSecond
                        Return "m/s"
                    Case Units.KilometersPerHour
                        Return "km/h"
                    Case Units.MilesPerHour
                        Return "mph"
                    Case Units.Knots
                        Return "kts"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a density magnitude.
    ''' </summary>
    Public Class DensityMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            kg_m3
            lb_ft3

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.kg_m3

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Length Then
                Dim Mag As DensityMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.kg_m3
                    _Value = _DefaultUnitValue
                Case Units.lb_ft3
                    _Value = _DefaultUnitValue * 0.0624279606
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.kg_m3
                    _DefaultUnitValue = value
                Case Units.lb_ft3
                    _DefaultUnitValue = value / 0.0624279606
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Density
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.kg_m3
                        Return "kg/m³"
                    Case Units.lb_ft3
                        Return "lb/ft³"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a force magnitude.
    ''' </summary>
    Public Class ForceMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            N
            Tonf
            kgf

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.N

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Force Then
                Dim Mag As ForceMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.N
                    _Value = _DefaultUnitValue
                Case Units.kgf
                    _Value = _DefaultUnitValue / 9.8
                Case Units.Tonf
                    _Value = _DefaultUnitValue / 9800
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.N
                    _DefaultUnitValue = value
                Case Units.kgf
                    _DefaultUnitValue = value * 9.8
                Case Units.Tonf
                    _DefaultUnitValue = value * 9800
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Force
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.N
                        Return "N"
                    Case Units.kgf
                        Return "kgf"
                    Case Units.Tonf
                        Return "Tonf"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a pressure magnitude.
    ''' </summary>
    Public Class PressureMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            Pa
            Atm
            Psi
            mmHg

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.Pa

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Pressure Then
                Dim Mag As PressureMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.Pa
                    _Value = _DefaultUnitValue
                Case Units.Atm
                    _Value = _DefaultUnitValue / 101.325
                Case Units.Psi
                    _Value = _DefaultUnitValue / 6894.757
                Case Units.mmHg
                    _DefaultUnitValue = value / 133.3
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.Pa
                    _DefaultUnitValue = value
                Case Units.Atm
                    _DefaultUnitValue = value * 101.325
                Case Units.Psi
                    _DefaultUnitValue = value * 6894.757
                Case Units.mmHg
                    _DefaultUnitValue = value * 133.3
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Pressure
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.Pa
                        Return "Pa"
                    Case Units.Atm
                        Return "Atm"
                    Case Units.Psi
                        Return "Psi"
                    Case Units.mmHg
                        Return "mmHg"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a force magnitude.
    ''' </summary>
    Public Class MomentMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            Nm
            kgfm

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.Nm

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Length Then
                Dim Mag As MomentMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.Nm
                    _Value = _DefaultUnitValue
                Case Units.kgfm
                    _Value = _DefaultUnitValue / 9.8
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.Nm
                    _DefaultUnitValue = value
                Case Units.kgfm
                    _DefaultUnitValue = value * 9.8
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Moment
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.Nm
                        Return "Nm"
                    Case Units.kgfm
                        Return "kgfm"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents an angular magnitude.
    ''' </summary>
    Public Class AngularMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            Degrees
            Radians

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.Radians

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Length Then
                Dim Mag As AngularMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            _DefaultUnitValue = value

            Select Case Unit
                Case Units.Radians
                    _Value = _DefaultUnitValue
                Case Units.Degrees
                    _Value = _DefaultUnitValue / Math.PI * 180
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.Radians
                    _DefaultUnitValue = value
                Case Units.Degrees
                    _DefaultUnitValue = value * Math.PI / 180
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Angular
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.Degrees
                        Return "°"
                    Case Units.Radians
                        Return "rad"
                    Case Else
                        Return ""
                End Select
            End Get
        End Property

    End Class

    ''' <summary>
    ''' Represents a dimensionless magnitude.
    ''' </summary>
    Public Class DimensionlessMagnitude

        Implements IPhysicalMagnitude

        ''' <summary>
        ''' Time units.
        ''' </summary>
        Public Enum Units As Short

            None

        End Enum

        ''' <summary>
        ''' Unit used as reference.
        ''' </summary>
        Public Const DefaultUnit As Units = Units.None

        Private _DefaultUnitValue As Double
        Private _Value As Double
        Private _Unit As Units

        Public Sub Assign(Magnitude As IPhysicalMagnitude) Implements IPhysicalMagnitude.Assign

            If Magnitude.Magnitude = Magnitudes.Length Then
                Dim Mag As DimensionlessMagnitude = Magnitude
                _DefaultUnitValue = Mag._DefaultUnitValue
                _Value = Mag._Value
                _Unit = Mag._Unit
            End If

        End Sub

        Private Sub SetValueFromDafaultUnitValue(value As Double)

            Select Case Unit
                Case Units.None
                    _Value = _DefaultUnitValue
                Case Else
                    _Value = _DefaultUnitValue
            End Select

        End Sub

        Private Sub SetDafaultUnitValueFromValue(value As Double)

            Select Case Unit
                Case Units.None
                    _DefaultUnitValue = value
                Case Else
                    _DefaultUnitValue = value
            End Select

        End Sub

        Public Property DefaultUnitValue As Double Implements IPhysicalMagnitude.DefaultUnitValue
            Set(value As Double)

                _DefaultUnitValue = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _DefaultUnitValue
            End Get
        End Property

        Public Property Unit As Units
            Set(value As Units)

                _Unit = value

                SetValueFromDafaultUnitValue(value)

            End Set
            Get
                Return _Unit
            End Get
        End Property

        Public Property Value As Double Implements IPhysicalMagnitude.Value
            Set(value As Double)

                _Value = value

                SetDafaultUnitValueFromValue(value)

            End Set
            Get
                Return _Value
            End Get
        End Property

        Public ReadOnly Property Magnitude As Magnitudes Implements IPhysicalMagnitude.Magnitude
            Get
                Return Magnitudes.Dimensionless
            End Get
        End Property

        Public ReadOnly Property Label As String Implements IPhysicalMagnitude.Label
            Get
                Select Case Unit
                    Case Units.None
                        Return "-"
                    Case Else
                        Return "-"
                End Select
            End Get
        End Property

    End Class

End Namespace
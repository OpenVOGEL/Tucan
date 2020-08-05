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

Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.MathTools.Magnitudes

Public Class FormAtmosphere

    Private rbPressure As ResultBox
    Private rbDensity As ResultBox
    Private rbViscocity As ResultBox
    Private rbSoundSpeed As ResultBox
    Private rbTemperature As ResultBox

    Public Sub New()

        InitializeComponent()

        rbPressure = New ResultBox(UserMagnitudes(Magnitudes.Pressure))
        rbDensity = New ResultBox(UserMagnitudes(Magnitudes.Density))
        rbViscocity = New ResultBox(UserMagnitudes(Magnitudes.Viscosity))
        rbSoundSpeed = New ResultBox(UserMagnitudes(Magnitudes.Velocity))
        rbTemperature = New ResultBox(UserMagnitudes(Magnitudes.Temperature))

        rbTemperature.Name = "T"
        rbTemperature.Top = 40
        rbTemperature.Left = 10
        rbTemperature.Width = 200
        rbTemperature.NameWidth = 20
        rbTemperature.UnitWidth = 50
        rbTemperature.Parent = Me
        rbTemperature.Decimals = GlobalDecimals(Magnitudes.Temperature)

        rbPressure.Name = "P"
        rbPressure.Top = rbTemperature.Bottom + 5
        rbPressure.Left = 10
        rbPressure.Width = 200
        rbPressure.NameWidth = 20
        rbPressure.UnitWidth = 50
        rbPressure.Parent = Me
        rbPressure.Decimals = GlobalDecimals(Magnitudes.Pressure)

        rbDensity.GreekLetter = True
        rbDensity.Name = "r"
        rbDensity.Top = rbPressure.Bottom + 5
        rbDensity.Left = 10
        rbDensity.Width = 200
        rbDensity.NameWidth = 20
        rbDensity.UnitWidth = 50
        rbDensity.Parent = Me
        rbDensity.Decimals = GlobalDecimals(Magnitudes.Density)

        rbViscocity.GreekLetter = True
        rbViscocity.Name = "m"
        rbViscocity.Top = rbDensity.Bottom + 5
        rbViscocity.Left = 10
        rbViscocity.Width = 200
        rbViscocity.NameWidth = 20
        rbViscocity.UnitWidth = 50
        rbViscocity.Parent = Me
        rbViscocity.Decimals = GlobalDecimals(Magnitudes.Viscosity)
        rbViscocity.Scientific = True

        rbSoundSpeed.Name = "a"
        rbSoundSpeed.Top = rbViscocity.Bottom + 5
        rbSoundSpeed.Left = 10
        rbSoundSpeed.Width = 200
        rbSoundSpeed.NameWidth = 20
        rbSoundSpeed.UnitWidth = 50
        rbSoundSpeed.Parent = Me
        rbSoundSpeed.Decimals = GlobalDecimals(Magnitudes.Velocity)

        LoadData()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Close()

    End Sub

    Private Sub LoadData()

        Dim ISA As New AeroTools.CalculationModel.Settings.StandardAtmosphere(nudAltitude.Value)

        rbTemperature.Value = ISA.Temperature
        rbPressure.Value = ISA.Pressure
        rbDensity.Value = ISA.Density
        rbViscocity.Value = ISA.DynamicVisc
        rbSoundSpeed.Value = ISA.SoundSpeed

    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click

        Try
            LoadData()

        Catch ex As Exception

            MessageBox.Show("Enter a valid Altitude setting")

        End Try

    End Sub

    Public Sub GetSettings(ByRef Settings As SimulationSettings)

        Try
            Settings.AssignStandardAtmosphere(nudAltitude.Value)
        Catch ex As Exception
            MessageBox.Show("Enter a valid Altitude setting")
        End Try

    End Sub

    Private Sub FormAtmosphere_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Hide()
        End If

    End Sub

End Class
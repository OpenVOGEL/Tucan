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

Public Class FormISA

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Close()

    End Sub

    Private Sub btnCheck_Click(sender As Object, e As EventArgs) Handles btnCheck.Click

        Try
            Dim ISA As New AeroTools.CalculationModel.Settings.StandardAtmosphere(nudAltitude.Value)
            txtTemp.Text = CStr(ISA.Temperature)
            txtPressure.Text = CStr(ISA.Pressure)
            txtDensity.Text = CStr(ISA.Density)
            txtViscosity.Text = CStr(ISA.DynamicVisc)
            txtSoundSpeed.Text = CStr(ISA.SoundSpeed)

        Catch ex As Exception

            MessageBox.Show("Enter a valid Altitude setting")

        End Try

    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub GetSettings(ByRef Settings As SimulationSettings)

        Try
            Settings.AssignStandardAtmosphere(nudAltitude.Value)
            DesignTools.DataStore.ProjectRoot.SimulationSettings = Settings
        Catch ex As Exception
            MessageBox.Show("Enter a valid Altitude setting")
        End Try

    End Sub

End Class
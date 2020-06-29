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

Imports OpenVOGEL.AeroTools.CalculationModel.Settings

Public Class FormSettings

    Private Made As Boolean = False
    Private LocalSettings As SimulationSettings

    Public Sub SetSettings()

        Made = False

        nudVx.Value = LocalSettings.StreamVelocity.X
        nudVy.Value = LocalSettings.StreamVelocity.Y
        nudVz.Value = LocalSettings.StreamVelocity.Z
        nudDensity.Value = LocalSettings.Density
        nudViscosity.Value = LocalSettings.Viscocity
        nudStaticPressure.Value = LocalSettings.StaticPressure
        nudSimulationSteps.Value = LocalSettings.SimulationSteps
        nudInterval.Value = LocalSettings.Interval
        nudCutoff.Value = LocalSettings.Cutoff
        nudAdjacencyTolerance.Value = LocalSettings.SurveyTolerance

        If Not _AllowFlowRotation Then
            LocalSettings.StreamOmega.SetToCero()
        End If
        nudOmegax.Value = LocalSettings.StreamOmega.X
        nudOmegay.Value = LocalSettings.StreamOmega.Y
        nudOmagaz.Value = LocalSettings.StreamOmega.Z
        nudOmegax.Enabled = _AllowFlowRotation
        nudOmegay.Enabled = _AllowFlowRotation
        nudOmagaz.Enabled = _AllowFlowRotation

        cbAutoCutOff.Checked = LocalSettings.CalculateCutoff
        cbExtendWakes.Checked = LocalSettings.ExtendWakes

        ' GPU

        cbUseGpu.Checked = LocalSettings.UseGpu
        nudDeviceId.Value = LocalSettings.GpuDeviceId

        ' Structural:

        If LocalSettings.AnalysisType = CalculationType.ctAeroelastic Then

            nudStructureStart.Enabled = True
            nudnModes.Enabled = True
            nudDamping.Enabled = True

        Else

            nudStructureStart.Enabled = False
            nudnModes.Enabled = False
            nudDamping.Enabled = False

        End If

        nudStructureStart.Value = LocalSettings.StructuralSettings.StructuralLinkingStep
        nudnModes.Value = LocalSettings.StructuralSettings.NumberOfModes
        nudDamping.Value = LocalSettings.StructuralSettings.ModalDamping

        Made = True

    End Sub

    Public Sub GetSettings()

        If Not Made Then Return

        LocalSettings.StreamVelocity.X = nudVx.Value
        LocalSettings.StreamVelocity.Y = nudVy.Value
        LocalSettings.StreamVelocity.Z = nudVz.Value
        LocalSettings.Density = nudDensity.Value
        LocalSettings.Viscocity = nudViscosity.Value
        LocalSettings.StaticPressure = nudStaticPressure.Value
        LocalSettings.SimulationSteps = nudSimulationSteps.Value
        LocalSettings.Interval = nudInterval.Value
        LocalSettings.Cutoff = nudCutoff.Value
        LocalSettings.SurveyTolerance = nudAdjacencyTolerance.Value
        If _AllowFlowRotation Then
            LocalSettings.StreamOmega.X = nudOmegax.Value
            LocalSettings.StreamOmega.Y = nudOmegay.Value
            LocalSettings.StreamOmega.Z = nudOmagaz.Value
        Else
            LocalSettings.StreamOmega.SetToCero()
        End If
        LocalSettings.CalculateCutoff = cbAutoCutOff.Checked
        LocalSettings.ExtendWakes = cbExtendWakes.Checked

        LocalSettings.UseGpu = cbUseGpu.Checked
        LocalSettings.GpuDeviceId = nudDeviceId.Value

        LocalSettings.StructuralSettings.StructuralLinkingStep = nudStructureStart.Value
        LocalSettings.StructuralSettings.NumberOfModes = nudnModes.Value
        LocalSettings.StructuralSettings.ModalDamping = nudDamping.Value

    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    Public WriteOnly Property Settings As SimulationSettings
        Set(ByVal value As SimulationSettings)
            LocalSettings = value
            SetSettings()
        End Set
    End Property

    Private _AllowFlowRotation As Boolean = True

    Public WriteOnly Property AllowFlowRotation As Boolean
        Set(ByVal value As Boolean)
            _AllowFlowRotation = value
        End Set
    End Property

    Private Sub btnISA_Click(sender As Object, e As EventArgs) Handles btnISA.Click

        If FormISA.ShowDialog = DialogResult.OK Then
            FormISA.GetSettings(LocalSettings)
            SetSettings()
        End If

    End Sub
End Class
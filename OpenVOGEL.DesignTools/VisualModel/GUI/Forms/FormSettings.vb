'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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
        STEPSBox.Value = LocalSettings.SimulationSteps
        CSTEPSBox.Value = LocalSettings.ClippingStep
        DTBox.Value = LocalSettings.Interval
        CUTOFFBox.Value = LocalSettings.Cutoff
        nudAdjacencyTolerance.Value = LocalSettings.SurveyTolerance

        If Not _AllowFlowRotation Then
            LocalSettings.Omega.SetToCero()
        End If
        nudOmegax.Value = LocalSettings.Omega.X
        nudOmegay.Value = LocalSettings.Omega.Y
        nudOmagaz.Value = LocalSettings.Omega.Z
        nudOmegax.Enabled = _AllowFlowRotation
        nudOmegay.Enabled = _AllowFlowRotation
        nudOmagaz.Enabled = _AllowFlowRotation

        cbAutoCutOff.Checked = LocalSettings.CalculateCutoff
        cbGlobalSurvey.Checked = LocalSettings.GlobalPanelSurvey
        cbExtendWakes.Checked = LocalSettings.ExtendWakes

        ' GPU

        cbUseGpu.Checked = LocalSettings.UseGpu
        nudDeviceId.Value = LocalSettings.GpuDeviceId

        ' Structural:

        nudStructureStart.Value = LocalSettings.StructuralSettings.StructuralLinkingStep
        nudnModes.Value = LocalSettings.StructuralSettings.NumberOfModes
        nudDamping.Value = LocalSettings.StructuralSettings.ModalDamping

        nudStructureStart.Enabled = _AllowStructuralSettings
        nudnModes.Enabled = _AllowStructuralSettings
        nudDamping.Enabled = _AllowStructuralSettings

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
        LocalSettings.SimulationSteps = STEPSBox.Value
        LocalSettings.ClippingStep = CSTEPSBox.Value
        LocalSettings.Interval = DTBox.Value
        LocalSettings.Cutoff = CUTOFFBox.Value
        LocalSettings.SurveyTolerance = nudAdjacencyTolerance.Value
        If _AllowFlowRotation Then
            LocalSettings.Omega.X = nudOmegax.Value
            LocalSettings.Omega.Y = nudOmegay.Value
            LocalSettings.Omega.Z = nudOmagaz.Value
        Else
            LocalSettings.Omega.SetToCero()
        End If
        LocalSettings.CalculateCutoff = cbAutoCutOff.Checked
        LocalSettings.GlobalPanelSurvey = cbGlobalSurvey.Checked
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

    Private _AllowStructuralSettings As Boolean = False

    Public WriteOnly Property AllowStructuralSettings As Boolean
        Set(ByVal value As Boolean)
            _AllowStructuralSettings = value
        End Set
    End Property

End Class
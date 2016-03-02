'Copyright (C) 2016 Guillermo Hazebrouck

Imports AeroTools.UVLM.Settings

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
        LongCaracteristricaBox.Value = LocalSettings.CharacteristicLenght

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
        LocalSettings.CharacteristicLenght = LongCaracteristricaBox.Value

        LocalSettings.GlobalPosition.X = PxBox.Value
        LocalSettings.GlobalPosition.Y = PyBox.Value
        LocalSettings.GlobalPosition.Z = PzBox.Value

        LocalSettings.GlobalRotationCenter.X = PxoBox.Value
        LocalSettings.GlobalRotationCenter.Y = PyoBox.Value
        LocalSettings.GlobalRotationCenter.Z = PzoBox.Value

        LocalSettings.GlobalOrientation.Psi = PsioBox.Value
        LocalSettings.GlobalOrientation.Tita = TitaoBox.Value
        LocalSettings.GlobalOrientation.Fi = FioBox.Value

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

    Private _AllowFlowRotation As Boolean = False

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
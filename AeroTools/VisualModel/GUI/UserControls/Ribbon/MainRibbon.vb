
Imports System.Windows.Forms
Imports AeroTools.CalculationModel.Settings
Imports AeroTools.VisualModel.Interface
Imports AeroTools.VisualModel.Models.Basics

Public Class MainRibbon

    Private _Project As ProjectRoot

    Public Property Project As ProjectRoot
        Set(value As ProjectRoot)
            _Project = value
            RefreshListOfObjects()
            LoadVisualization()
        End Set
        Get
            Return _Project
        End Get
    End Property

    Public Event SwitchToDesignMode()
    Public Event SwitchToResultsMode()
    Public Event PushMessage(msg As String)
    Public Event EditSurface(ByRef Surface As Surface)
    Public Event EditVelocityPlane()

    Public Sub New()

        InitializeComponent()

        cbxSimulationMode.Items.Add("Steady")
        cbxSimulationMode.Items.Add("Unsteady")
        cbxSimulationMode.Items.Add("Aeroelastic")
        cbxSimulationMode.SelectedIndex = 0

        AddHandler _timer.Tick, AddressOf _timer_Tick

    End Sub

    Private Sub tcRibbon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcRibbon.SelectedIndexChanged

        If tcRibbon.SelectedIndex = 0 Or tcRibbon.SelectedIndex = 1 Then

            If Project IsNot Nothing Then

                Project.DesignMode()

                StopTransit()

                RaiseEvent SwitchToDesignMode()

            End If

        ElseIf tcRibbon.SelectedIndex = 2

            If Project IsNot Nothing Then

                Project.PostprocessMode()

                RaiseEvent SwitchToResultsMode()

            End If

        End If

    End Sub

#Region "Add, remove and clone objects"

    Private Sub btnAddObject_Click(sender As Object, e As EventArgs) Handles btnAddObject.Click

        Dim dialog As New FormSelectObject

        If dialog.ShowDialog() = DialogResult.OK Then

            If dialog.rbFuselage.Checked Then
                Project.Model.AddExtrudedBody()
            End If

            If dialog.rbLiftingSurface.Checked Then
                Project.Model.AddLiftingSurface()
            End If

            If dialog.rbJetEngine.Checked Then
                Project.Model.AddJetEngine()
            End If

        End If

        RefreshListOfObjects()

    End Sub

    Public Sub RefreshListOfObjects()

        If Project IsNot Nothing Then

            Dim index As Integer = cbxSurfaces.SelectedIndex

            cbxSurfaces.Items.Clear()

            For Each Surface In Project.Model.Objects

                cbxSurfaces.Items.Add(Surface.Name)

            Next

            If (index >= 0 And index < cbxSurfaces.Items.Count) Then

                cbxSurfaces.SelectedIndex = index

            End If

            Project.RefreshOnGL()

        End If

    End Sub

#End Region

#Region "Open and save project"

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click

        GenerateNewProject()

    End Sub

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click

        OpenProject()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        SaveProject()

    End Sub

    Private Sub btnSaveAs_Click(sender As Object, e As EventArgs) Handles btnSaveAs.Click

        SaveProjectAs()

    End Sub

    Private Sub OpenProject()

        Dim Respuesta1 As MsgBoxResult = MsgBox("The current project will be closed. Do you wish to save it?", vbYesNoCancel, "Opening exsisting project")

        Select Case Respuesta1

            Case MsgBoxResult.Yes

                ' If the project has never been saved call save as.

                Dim Saved As Boolean = True

                If Project.ExistsOnDatabase Then
                    SaveProject()
                Else
                    Saved = SaveProjectAs()
                End If

                If Saved Then
                    Project.RestartProject()
                Else
                    Exit Sub
                End If

            Case MsgBoxResult.Cancel
                Exit Sub

            Case MsgBoxResult.No

        End Select

        Try

            Dim dlgOpenFile As New OpenFileDialog()

            dlgOpenFile.Filter = "Vogel proyect files (*.vog)|*.vog"

            Dim Respuesta2 As MsgBoxResult = dlgOpenFile.ShowDialog()

            If Respuesta2 = MsgBoxResult.Ok Then

                Project.RestartProject()
                Project.FilePath = dlgOpenFile.FileName
                Project.ReadFromXML()

                LoadVisualization()
                LoadSettings()

            End If

        Catch ex As Exception
            MsgBox("Error while reading proyect data file. File data might be corrupted.", MsgBoxStyle.OkOnly, "Error")
            FileClose(200)
        End Try

        Project.RefreshOnGL()
        RefreshListOfObjects()

    End Sub

    Private Sub SaveProject()

        Try

            If Project.ExistsOnDatabase Then
                Project.WriteToXML()
            Else
                SaveProjectAs()
            End If

            RaiseEvent PushMessage("The proyect has been saved")

        Catch ex As Exception

            Dim GuardarConOtroNombre As MsgBoxResult = MsgBox("An exception was raised while saving the project! Do you wish to save it under a different name?", MsgBoxStyle.OkCancel, "Error!")
            If GuardarConOtroNombre = MsgBoxResult.Ok Then SaveProjectAs()

        End Try

    End Sub

    Public Function SaveProjectAs() As Boolean

        Dim Result As Boolean = False

        Try
            Dim Respuesta As DialogResult

            Dim dlgSaveFile As New SaveFileDialog()

            dlgSaveFile.Filter = "Vogel proyect files (*.vog)|*.vog"
            Respuesta = dlgSaveFile.ShowDialog()
            If Respuesta = DialogResult.OK Then
                Project.FilePath = dlgSaveFile.FileName
                Project.WriteToXML()
                Result = True
                RaiseEvent PushMessage("The proyect has been saved")
            End If
        Catch ex As Exception
            MsgBox("Error while saving project!", MsgBoxStyle.OkOnly, "Error!")
            Return False
        End Try

        Return Result

    End Function

    Public Sub GenerateNewProject()

        Dim Respuesta As MsgBoxResult = MsgBox("Do you wish to save the current project before starting a new one?", vbYesNoCancel, "Start new project from scratch")

        On Error GoTo ErrSub

        Select Case Respuesta

            Case MsgBoxResult.Yes

                SaveProject()
                Project.RestartProject()
                RefreshListOfObjects()
                RaiseEvent SwitchToDesignMode()

            Case MsgBoxResult.No

                Project.RestartProject()
                RefreshListOfObjects()
                RaiseEvent SwitchToDesignMode()

            Case MsgBoxResult.Cancel

        End Select

        Exit Sub

ErrSub:

        MsgBox("Error while creating new project!", MsgBoxStyle.Exclamation, "Error")

    End Sub

    Private Sub btnClone_Click(sender As Object, e As EventArgs) Handles btnClone.Click

        If _SelectedSurface IsNot Nothing Then

            Dim Clone As Surface = _SelectedSurface.Clone()

            If Clone IsNot Nothing Then

                Project.Model.Objects.Add(Clone)

                RefreshListOfObjects()

            End If

        End If

    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        If cbxSurfaces.SelectedIndex >= 0 And cbxSurfaces.SelectedIndex < Project.Model.Objects.Count Then

            Project.Model.Objects.RemoveAt(cbxSurfaces.SelectedIndex)

            RefreshListOfObjects()

        End If

    End Sub

#End Region

#Region "Load ad manage screen properties"

    Private _LockScreenPropEvents As Boolean

    Private Sub LoadVisualization()

        If Project IsNot Nothing Then

            _LockScreenPropEvents = True

            pnlScreenColor.BackColor = Project.Visualization.ScreenColor
            cbxShowRulers.Checked = Project.Visualization.ReferenceFrame.Visible
            nudXmax.Value = Project.Visualization.ReferenceFrame.Xmax
            nudXmin.Value = Project.Visualization.ReferenceFrame.Xmin
            nudYmax.Value = Project.Visualization.ReferenceFrame.Ymax
            nudYmin.Value = Project.Visualization.ReferenceFrame.Ymin

            _LockScreenPropEvents = False

        End If

    End Sub

    Private Sub pnlScreenColor_Click(sender As Object, e As EventArgs) Handles pnlScreenColor.Click

        If Project IsNot Nothing AndAlso Not _LockScreenPropEvents Then

            Dim dialog As New ColorDialog

            ' load custom surface colors (colors from all other surfaces):

            Dim colors(0) As Integer

            Dim color As Drawing.Color = Project.Visualization.ScreenColor

            colors(0) = (CInt(color.B) << 16) + (CInt(color.G) << 8) + color.R

            dialog.CustomColors = colors

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Visualization.ScreenColor = dialog.Color

                pnlScreenColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowRulers_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowRulers.CheckedChanged

        If Project IsNot Nothing AndAlso Not _LockScreenPropEvents Then

            Project.Visualization.ReferenceFrame.Visible = cbxShowRulers.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudXmax_ValueChanged(sender As Object, e As EventArgs) Handles nudXmax.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockScreenPropEvents Then

            Project.Visualization.ReferenceFrame.Xmax = nudXmax.Value

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudXmin_ValueChanged(sender As Object, e As EventArgs) Handles nudXmin.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockScreenPropEvents Then

            Project.Visualization.ReferenceFrame.Xmin = nudXmin.Value

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudYmax_ValueChanged(sender As Object, e As EventArgs) Handles nudYmax.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockScreenPropEvents Then

            Project.Visualization.ReferenceFrame.Ymax = nudYmax.Value

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudYmin_ValueChanged(sender As Object, e As EventArgs) Handles nudYmin.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockScreenPropEvents Then

            Project.Visualization.ReferenceFrame.Xmin = nudYmin.Value

            Project.RefreshOnGL()

        End If

    End Sub

#End Region

#Region "Load settigs and manage calculation"

    Private niNotification As New NotifyIcon()

    Public Sub Calculate(Optional ByVal CalculationType As CalculationType = CalculationType.ctSteady)

        Dim FormSettings = New FormSettings()

        FormSettings.Settings = Project.SimulationSettings

        If (FormSettings.ShowDialog()) = DialogResult.OK Then

            FormSettings.GetSettings()

            RaiseEvent PushMessage("Calculation started.")

            niNotification.Text = "Calculating..."

            Select Case CalculationType

                Case CalculationType.ctSteady

                    niNotification.BalloonTipText = "Calculating steady state"
                    niNotification.ShowBalloonTip(3000)

                Case CalculationType.ctUnsteady

                    niNotification.BalloonTipText = "Calculating unsteady transit"
                    niNotification.ShowBalloonTip(3000)

                Case CalculationType.ctAeroelastic

                    niNotification.BalloonTipText = "Calculating aeroelastic transit"
                    niNotification.ShowBalloonTip(3000)

            End Select

            AddHandler Project.CalculationDone, AddressOf PostCalculationActions

            Project.StartCalculation(CalculationType, Parent)

        End If

    End Sub

    Private Sub PostCalculationActions()

        If InvokeRequired Then
            BeginInvoke(New Action(AddressOf PostCalculationActions))
        Else
            tcRibbon.SelectedIndex = 2
            niNotification.Text = "Ready"
            niNotification.BalloonTipText = "Calculation done!"
            niNotification.ShowBalloonTip(3000)
            LoadResultProperties()
            LoadModes()
            RaiseEvent PushMessage("Calculation done!")
        End If

    End Sub

    Private Sub btnStartCalculation_Click(sender As Object, e As EventArgs) Handles btnStartCalculation.Click

        If Project IsNot Nothing Then

            Calculate(cbxSimulationMode.SelectedIndex)

        End If

    End Sub

    Private _LockSettingsEvents As Boolean = True

    Private Sub LoadSettings()

        If Project IsNot Nothing Then

            _LockSettingsEvents = True

            nudVx.Value = Project.SimulationSettings.StreamVelocity.X
            nudVy.Value = Project.SimulationSettings.StreamVelocity.Y
            nudVz.Value = Project.SimulationSettings.StreamVelocity.Z

            nudOx.Value = Project.SimulationSettings.Omega.X
            nudOy.Value = Project.SimulationSettings.Omega.Y
            nudOz.Value = Project.SimulationSettings.Omega.Z

            nudDensity.Value = Project.SimulationSettings.Density
            nudViscosity.Value = Project.SimulationSettings.Viscocity
            nudSteps.Value = Project.SimulationSettings.SimulationSteps
            nudCuttingStep.Value = Project.SimulationSettings.ClippingStep
            nudIncrement.Value = Project.SimulationSettings.Interval

            _LockSettingsEvents = False

        End If

    End Sub

    Private Sub nudVx_ValueChanged(sender As Object, e As EventArgs) Handles nudVx.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.StreamVelocity.X = nudVx.Value

        End If

    End Sub

    Private Sub nudVy_ValueChanged(sender As Object, e As EventArgs) Handles nudVy.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.StreamVelocity.Y = nudVy.Value

        End If

    End Sub

    Private Sub nudVz_ValueChanged(sender As Object, e As EventArgs) Handles nudVz.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.StreamVelocity.Z = nudVz.Value

        End If

    End Sub

    Private Sub nudOx_ValueChanged(sender As Object, e As EventArgs) Handles nudOx.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.Omega.X = nudOx.Value

        End If

    End Sub

    Private Sub nudOy_ValueChanged(sender As Object, e As EventArgs) Handles nudOy.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.Omega.Y = nudOy.Value

        End If

    End Sub

    Private Sub nudOz_ValueChanged(sender As Object, e As EventArgs) Handles nudOz.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.Omega.Z = nudOz.Value

        End If

    End Sub

    Private Sub nudDensity_ValueChanged(sender As Object, e As EventArgs) Handles nudDensity.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.Density = nudDensity.Value

        End If

    End Sub

    Private Sub nudViscosity_ValueChanged(sender As Object, e As EventArgs) Handles nudViscosity.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.Viscocity = nudViscosity.Value

        End If

    End Sub

    Private Sub nudSteps_ValueChanged(sender As Object, e As EventArgs) Handles nudSteps.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.SimulationSteps = nudSteps.Value

        End If

    End Sub

    Private Sub nudIncrement_ValueChanged(sender As Object, e As EventArgs) Handles nudIncrement.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.Interval = nudIncrement.Value

        End If

    End Sub

    Private Sub nudCuttingStep_ValueChanged(sender As Object, e As EventArgs) Handles nudCuttingStep.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.ClippingStep = nudCuttingStep.Value

        End If

    End Sub

    Private Sub nudCutoff_ValueChanged(sender As Object, e As EventArgs) Handles nudCutoff.ValueChanged

        If Project IsNot Nothing AndAlso Not _LockSettingsEvents Then

            Project.SimulationSettings.Cutoff = nudCutoff.Value

        End If

    End Sub

#End Region

#Region "Surface selection and loading of surface data"

    Private _LockPropsEvents As Boolean
    Private _SelectedSurface As Surface

    Public Sub ChangeSurfaceIndex(newIndex As Integer)

        If newIndex >= 0 And newIndex < cbxSurfaces.Items.Count Then

            cbxSurfaces.SelectedIndex = newIndex

        End If

    End Sub

    Private Sub cbxSurfaces_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxSurfaces.SelectedIndexChanged

        If _SelectedSurface IsNot Nothing Then RemoveHandler _SelectedSurface.MeshUpdated, AddressOf UpdateMeshInfo

        For i = 0 To Project.Model.Objects.Count - 1

            Project.Model.Objects(i).Selected = i = cbxSurfaces.SelectedIndex

            If Project.Model.Objects(i).Selected Then

                _SelectedSurface = Project.Model.Objects(i)

                If _SelectedSurface IsNot Nothing Then AddHandler _SelectedSurface.MeshUpdated, AddressOf UpdateMeshInfo

                LoadSelectedSurface()

            End If

        Next

        Project.RefreshOnGL()

    End Sub

    Private Sub LoadSelectedSurface()

        _LockPropsEvents = True

        If _SelectedSurface IsNot Nothing Then

            tbxName.Text = _SelectedSurface.Name
            cbxShowMesh.Checked = _SelectedSurface.VisualProperties.ShowMesh
            cbxShowSurface.Checked = _SelectedSurface.VisualProperties.ShowSurface
            pnlMeshColor.BackColor = _SelectedSurface.VisualProperties.ColorMesh
            pnlSurfaceColor.BackColor = _SelectedSurface.VisualProperties.ColorSurface

            nudPx.Value = _SelectedSurface.Position.X
            nudPy.Value = _SelectedSurface.Position.Y
            nudPz.Value = _SelectedSurface.Position.Z

            nudCRx.Value = _SelectedSurface.CenterOfRotation.X
            nudCRy.Value = _SelectedSurface.CenterOfRotation.Y
            nudCRz.Value = _SelectedSurface.CenterOfRotation.Z

            nudPsi.Value = _SelectedSurface.Orientation.Psi
            nudTita.Value = _SelectedSurface.Orientation.Tita
            nudFi.Value = _SelectedSurface.Orientation.Fi

            cbSecuence.SelectedIndex = _SelectedSurface.Orientation.Secuence

        End If

        _LockPropsEvents = False

        UpdateMeshInfo()

    End Sub

    Private Sub UpdateMeshInfo()

        If _SelectedSurface IsNot Nothing Then

            lblMeshInfo.Text = String.Format("{0} nodes, {1} panels", _SelectedSurface.NumberOfNodes, _SelectedSurface.NumberOfPanels)

        End If

    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click

        RaiseEvent EditSurface(_SelectedSurface)

        LoadSelectedSurface()

    End Sub

    Private Sub tbxName_TextChanged(sender As Object, e As EventArgs) Handles tbxName.TextChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Name = tbxName.Text

            RefreshListOfObjects()

        End If

    End Sub

    Private Sub cbxShowSurface_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowSurface.CheckedChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.VisualProperties.ShowSurface = cbxShowSurface.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowMesh.CheckedChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.VisualProperties.ShowMesh = cbxShowMesh.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlSurfaceColor.MouseClick

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            Dim dialog As New ColorDialog

            ' load custom surface colors (colors from all other surfaces):

            Dim colors(Project.Model.Objects.Count - 1) As Integer

            For i = 0 To Project.Model.Objects.Count - 1

                Dim color As Drawing.Color = Project.Model.Objects(i).VisualProperties.ColorSurface

                colors(i) = (CInt(color.B) << 16) + (CInt(color.G) << 8) + color.R

            Next

            dialog.Color = _SelectedSurface.VisualProperties.ColorSurface

            dialog.CustomColors = colors

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                _SelectedSurface.VisualProperties.ColorSurface = dialog.Color

                pnlSurfaceColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlMeshColor.MouseClick

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            Dim dialog As New ColorDialog

            ' load custom surface colors (colors from all other meshes):

            Dim colors(Project.Model.Objects.Count - 1) As Integer

            For i = 0 To Project.Model.Objects.Count - 1

                Dim color As Drawing.Color = Project.Model.Objects(i).VisualProperties.ColorMesh

                colors(i) = (CInt(color.B) << 16) + (CInt(color.G) << 8) + color.R

            Next

            dialog.Color = _SelectedSurface.VisualProperties.ColorMesh

            dialog.CustomColors = colors

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                _SelectedSurface.VisualProperties.ColorMesh = dialog.Color

                pnlMeshColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If
    End Sub

    Private Sub nudPx_ValueChanged(sender As Object, e As EventArgs) Handles nudPx.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Position.X = nudPx.Value

            _SelectedSurface.MoveTo(_SelectedSurface.Position)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudPy_ValueChanged(sender As Object, e As EventArgs) Handles nudPy.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Position.Y = nudPy.Value

            _SelectedSurface.MoveTo(_SelectedSurface.Position)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudPz_ValueChanged(sender As Object, e As EventArgs) Handles nudPz.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Position.Z = nudPz.Value

            _SelectedSurface.MoveTo(_SelectedSurface.Position)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCRx_ValueChanged(sender As Object, e As EventArgs) Handles nudCRx.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.CenterOfRotation.X = nudCRx.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCRy_ValueChanged(sender As Object, e As EventArgs) Handles nudCRy.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.CenterOfRotation.Y = nudCRy.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCRz_ValueChanged(sender As Object, e As EventArgs) Handles nudCRz.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.CenterOfRotation.Z = nudCRz.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudPsi_ValueChanged(sender As Object, e As EventArgs) Handles nudPsi.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Psi = nudPsi.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudTita_ValueChanged(sender As Object, e As EventArgs) Handles nudTita.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Tita = nudTita.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudFi_ValueChanged(sender As Object, e As EventArgs) Handles nudFi.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Fi = nudFi.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbSecuence_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSecuence.SelectedIndexChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Secuence = Math.Max(0, cbSecuence.SelectedIndex)

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            Project.RefreshOnGL()

        End If

    End Sub

#End Region

#Region "Load results and transit"

    Private Sub btnLoadResults_Click(sender As Object, e As EventArgs) Handles btnLoadResults.Click

        Try

            Dim dlgOpenFile As New OpenFileDialog

            dlgOpenFile.Filter = "Vogel result files (*.res)|*.res"

            Dim Respuesta2 As MsgBoxResult = dlgOpenFile.ShowDialog()

            If Respuesta2 = MsgBoxResult.Ok Then

                Project.ReadResults(dlgOpenFile.FileName)

                Project.PostprocessMode()

                LoadResultProperties()

                LoadModes()

            End If

            Project.RefreshOnGL()

        Catch

            MsgBox("Could not open the selected result file!")

        End Try

    End Sub

    Private Sub LoadModes()

        cbxModes.Items.Clear()

        If Project IsNot Nothing Then

            If Not IsNothing(Project.Results.DynamicModes) Then

                If Project.Results.DynamicModes.Count > 0 Then

                    nudModeScale.Visible = True

                    cbxModes.Enabled = True
                    nudModeScale.Enabled = True

                    cbxModes.Items.Add("Displacements")

                    For i = 0 To Project.Results.DynamicModes.Count - 1
                        cbxModes.Items.Add(Project.Results.DynamicModes(i).Name)
                    Next

                    cbxModes.SelectedIndex = 0

                Else

                    cbxModes.Items.Add("Results")
                    cbxModes.SelectedIndex = 0

                    cbxModes.Enabled = False
                    nudModeScale.Enabled = False

                End If

            End If

            btnPlayStop.Enabled = Project.Results.TransitLoaded

        End If

    End Sub

    Private Sub cbxModes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxModes.SelectedIndexChanged

        If Project IsNot Nothing Then

            If Not IsNothing(Project.Results.DynamicModes) Then

                If cbxModes.SelectedIndex > -1 And cbxModes.SelectedIndex < Project.Results.DynamicModes.Count + 1 Then

                    If (cbxModes.SelectedIndex = 0) Then
                        Project.Results.VisualizeModes = False
                    Else
                        Project.Results.VisualizeModes = True
                        Project.Results.SelectedModeIndex = cbxModes.SelectedIndex - 1
                        Project.Results.SelectedMode.UpdateDisplacement(nudModeScale.Value)
                    End If

                    Project.RefreshOnGL()

                End If

            End If

        End If

    End Sub

    Private Sub nudModeScale_ValueChanged(sender As Object, e As EventArgs) Handles nudModeScale.ValueChanged

        If Project IsNot Nothing Then

            If Not IsNothing(Project.Results.DynamicModes) Then

                If cbxModes.SelectedIndex > 0 And cbxModes.SelectedIndex < Project.Results.DynamicModes.Count + 1 Then

                    Project.Results.SelectedMode.UpdateDisplacement(nudModeScale.Value)

                    Project.RefreshOnGL()

                End If

            End If

        End If

    End Sub

    Private _timer As New Timer
    Private _Simulating As Boolean = False
    Private _CurrentFrame As Integer = 0

    Private Sub btnPlayStop_Click(sender As Object, e As EventArgs) Handles btnPlayStop.Click

        If Project IsNot Nothing Then

            If Project.Results.TransitLoaded Then

                If Not Project.Simulating Then
                    _timer.Interval = Project.Results.SimulationSettings.Interval * 1000 / Project.Results.SimulationSettings.StructuralSettings.SubSteps
                    Project.Simulating = True
                    _timer.Start()
                    btnPlayStop.Text = "Stop"
                    RaiseEvent PushMessage(String.Format("Simulating. Rate {0}f/{1}s", Project.Results.SimulationSettings.StructuralSettings.SubSteps, Project.Results.SimulationSettings.Interval))
                Else
                    Project.Simulating = False
                    _timer.Stop()
                    btnPlayStop.Text = "Play"
                    RaiseEvent PushMessage("Simulation stopped")
                End If

            Else

                Project.Simulating = False

            End If

        End If

    End Sub

    Private Sub _timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If Project IsNot Nothing Then
            If _CurrentFrame > Project.Results.TransitStages Then _CurrentFrame = 0
            Project.RepresentResultsTransitWithOpenGL(_CurrentFrame)
            _CurrentFrame += 1
        End If

    End Sub

    Private Sub StopTransit()

        If Project IsNot Nothing Then
            Project.Simulating = False
            _timer.Stop()
        End If

    End Sub

#End Region

#Region "Postprocess properties"

    Private _LockResultPropsEvents As Boolean

    Private Sub LoadResultProperties()

        _LockResultPropsEvents = True

        If Project IsNot Nothing Then

            cbxShowResMesh.Checked = Project.Results.Model.VisualProperties.ShowMesh
            cbxShowResSurface.Checked = Project.Results.Model.VisualProperties.ShowSurface
            pnlResultMeshColor.BackColor = Project.Results.Model.VisualProperties.ColorMesh
            pnlResultSurfaceColor.BackColor = Project.Results.Model.VisualProperties.ColorSurface

            pnlForceColor.BackColor = Project.Results.Model.VisualProperties.ColorLoads
            pnlVelocityColor.BackColor = Project.Results.Model.VisualProperties.ColorVelocity
            cbxShowForce.Checked = Project.Results.Model.VisualProperties.ShowLoadVectors
            cbxShowVelocity.Checked = Project.Results.Model.VisualProperties.ShowVelocityVectors

            nudScaleForce.Value = Project.Results.Model.VisualProperties.ScalePressure
            nudScaleVelocity.Value = Project.Results.Model.VisualProperties.ScaleVelocity

            cbxShowColormap.Checked = Project.Results.Model.VisualProperties.ShowColormap
            nudCpmax.Value = Project.Results.Model.PressureRange.Maximum
            nudCpmin.Value = Project.Results.Model.PressureRange.Minimum

            cbxShowWakeSurface.Checked = Project.Results.Wakes.VisualProperties.ShowSurface
            cbxShowWakeMesh.Checked = Project.Results.Wakes.VisualProperties.ShowMesh
            cbxShowWakeNodes.Checked = Project.Results.Wakes.VisualProperties.ShowNodes

            pnlWakeSurfaceColor.BackColor = Project.Results.Wakes.VisualProperties.ColorSurface
            pnlWakeMeshColor.BackColor = Project.Results.Wakes.VisualProperties.ColorMesh
            pnlWakeNodeColor.BackColor = Project.Results.Wakes.VisualProperties.ColorNodes

            cbxShowVelocityPlane.Checked = Project.VelocityPlane.Visible

            gbxAeroelastic.Enabled = Project.Results.DynamicModes.Count > 0

        End If

        _LockResultPropsEvents = False

    End Sub

    Private Sub cbxShowColormap_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowColormap.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.VisualProperties.ShowColormap = cbxShowColormap.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlResultSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlResultSurfaceColor.MouseClick

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = Project.Results.Model.VisualProperties.ColorSurface

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Results.Model.VisualProperties.ColorSurface = dialog.Color

                pnlResultSurfaceColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlResultMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlResultMeshColor.MouseClick

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = Project.Results.Model.VisualProperties.ColorMesh

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Results.Model.VisualProperties.ColorMesh = dialog.Color

                pnlResultMeshColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlVelocityColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlVelocityColor.MouseClick

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = Project.Results.Model.VisualProperties.ColorVelocity

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Results.Model.VisualProperties.ColorVelocity = dialog.Color

                pnlVelocityColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlForceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlForceColor.MouseClick

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = Project.Results.Model.VisualProperties.ColorLoads

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Results.Model.VisualProperties.ColorLoads = dialog.Color

                pnlForceColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudScaleVelocity_ValueChanged(sender As Object, e As EventArgs) Handles nudScaleVelocity.ValueChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.VisualProperties.ScaleVelocity = nudScaleVelocity.Value

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudScaleForce_ValueChanged(sender As Object, e As EventArgs) Handles nudScaleForce.ValueChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.VisualProperties.ScalePressure = nudScaleForce.Value

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeSurface_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeSurface.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Wakes.VisualProperties.ShowSurface = cbxShowWakeSurface.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeMesh.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Wakes.VisualProperties.ShowMesh = cbxShowWakeMesh.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeNodes_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeNodes.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Wakes.VisualProperties.ShowNodes = cbxShowWakeNodes.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeSurfaceColor.MouseClick

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = Project.Results.Wakes.VisualProperties.ColorSurface

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Results.Wakes.VisualProperties.ColorSurface = dialog.Color

                pnlWakeSurfaceColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeMeshColor.MouseClick

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = Project.Results.Wakes.VisualProperties.ColorMesh

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Results.Wakes.VisualProperties.ColorMesh = dialog.Color

                pnlWakeMeshColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeNodeColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeNodeColor.MouseClick

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = Project.Results.Wakes.VisualProperties.ColorNodes

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                Project.Results.Wakes.VisualProperties.ColorNodes = dialog.Color

                pnlWakeNodeColor.BackColor = dialog.Color

            End If

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowResSurface_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowResSurface.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.VisualProperties.ShowSurface = cbxShowResSurface.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowResMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowResMesh.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.VisualProperties.ShowMesh = cbxShowResMesh.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowVelocity_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowVelocity.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.VisualProperties.ShowVelocityVectors = cbxShowVelocity.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowForce_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowForce.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.VisualProperties.ShowLoadVectors = cbxShowForce.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowVelocityPlane_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowVelocityPlane.CheckedChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.VelocityPlane.Visible = cbxShowVelocityPlane.Checked

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub btnEditVelocityPlane_Click(sender As Object, e As EventArgs) Handles btnEditVelocityPlane.Click

        RaiseEvent EditVelocityPlane()

    End Sub

    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click

        If Project IsNot Nothing Then

            If (Not Project.CalculationCore Is Nothing) Then
                Dim FormReport As New FormReport
                FormReport.ReportResults(Project.CalculationCore)
                FormReport.ShowDialog()
            Else
                MsgBox("Results are not available. Load a results file or execute the calculation first.")
            End If

        End If

    End Sub

    Private Sub nudCpmax_ValueChanged(sender As Object, e As EventArgs) Handles nudCpmax.ValueChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.PressureRange.Maximum = nudCpmax.Value

            Project.Results.Model.UpdateColormapWithPressure()

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCpmin_ValueChanged(sender As Object, e As EventArgs) Handles nudCpmin.ValueChanged

        If (Not _LockResultPropsEvents) And Project IsNot Nothing Then

            Project.Results.Model.PressureRange.Minimum = nudCpmin.Value

            Project.Results.Model.UpdateColormapWithPressure()

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub btnResetColormap_Click(sender As Object, e As EventArgs) Handles btnResetColormap.Click

        If Project IsNot Nothing Then

            Project.Results.Model.FindPressureRange()

            Project.Results.Model.UpdateColormapWithPressure()

            _LockResultPropsEvents = True

            nudCpmax.Value = Project.Results.Model.PressureRange.Maximum
            nudCpmin.Value = Project.Results.Model.PressureRange.Minimum

            _LockResultPropsEvents = False

            Project.RefreshOnGL()

        End If

    End Sub

#End Region

#Region " Representation "

    Private Sub LoadViewParameters(Optional ByVal Vista As String = "Free")

        If Project IsNot Nothing Then

            Select Case Vista

                Case "XY"
                    Project.Visualization.CameraOrientation.Psi = 0
                    Project.Visualization.CameraOrientation.Fi = 0
                    RaiseEvent PushMessage("XY view")

                Case "ZY"
                    Project.Visualization.CameraOrientation.Psi = 90
                    Project.Visualization.CameraOrientation.Fi = -90
                    RaiseEvent PushMessage("ZY view")

                Case "ZX"
                    Project.Visualization.CameraOrientation.Psi = 0
                    Project.Visualization.CameraOrientation.Fi = -90
                    RaiseEvent PushMessage("ZX view")

                Case "Isometrica"
                    Project.Visualization.CameraOrientation.Psi = 30
                    Project.Visualization.CameraOrientation.Fi = -60
                    RaiseEvent PushMessage("Free view")

                Case "Center"
                    Project.Visualization.CameraPosition.X = 0
                    Project.Visualization.CameraPosition.Y = 0
                    Project.Visualization.CameraPosition.Z = 0

            End Select

            Project.RefreshOnGL()

        End If

    End Sub

    Private Sub btnTopView_Click(sender As Object, e As EventArgs) Handles btnTopView.Click

        LoadViewParameters("XY")

    End Sub

    Private Sub btnFrontView_Click(sender As Object, e As EventArgs) Handles btnFrontView.Click

        LoadViewParameters("ZY")

    End Sub

    Private Sub btnSideView_Click(sender As Object, e As EventArgs) Handles btnSideView.Click

        LoadViewParameters("ZX")

    End Sub

    Private Sub btnMove_Click(sender As Object, e As EventArgs) Handles btnMove.Click

        Project.Model.OperationsTool.CancelOperation()
        Project.Model.OperationsTool.Operation = Operations.Translate
        RaiseEvent PushMessage("Translate surface")

    End Sub

    Private Sub bntAlign_Click(sender As Object, e As EventArgs) Handles bntAlign.Click

        Project.Model.OperationsTool.CancelOperation()
        Project.Model.OperationsTool.Operation = Operations.Align
        RaiseEvent PushMessage("Align surface")

    End Sub

    Private Sub btnHistogram_Click(sender As Object, e As EventArgs) Handles btnHistogram.Click

        If cbxSimulationMode.SelectedIndex = 1 Then

            Dim Dialog As New FormUnsteadyVelocity

            Dialog.StartPosition = FormStartPosition.CenterParent

            Dialog.ShowProfile(Project.SimulationSettings, True)

        ElseIf cbxSimulationMode.SelectedIndex = 2 Then

            Dim Dialog As New FormHistogram(Project.SimulationSettings)

            Dialog.StartPosition = FormStartPosition.CenterParent

            Dialog.ShowDialog()

        End If

    End Sub

#End Region

End Class

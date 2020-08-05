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
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.Tucan.Utility
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

Public Class MainRibbon

    Public Event SwitchToDesignMode()
    Public Event SwitchToResultsMode()
    Public Event PushMessage(msg As String)
    Public Event EditSurface(ByRef Surface As Surface)
    Public Event EditVelocityPlane()
    Public Event ProjectCleared()

    Private CalculationBussy As Boolean = False

    Public Sub New()

        InitializeComponent()

        cbxSimulationMode.Items.Add("Steady state")
        cbxSimulationMode.Items.Add("Free flight")
        cbxSimulationMode.Items.Add("Aeroelastic")
        cbxSimulationMode.SelectedIndex = 0

        AddHandler _timer.Tick, AddressOf _timer_Tick

        RefreshListOfObjects()

        LoadVisualization()

    End Sub

    Private Sub tcRibbon_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tcRibbon.SelectedIndexChanged

        If tcRibbon.SelectedIndex = 0 Or tcRibbon.SelectedIndex = 1 Then

            If ModelInterface.Initialized Then

                ModelInterface.DesignMode()

                StopTransit()

                FormReport.Hide()

                RaiseEvent SwitchToDesignMode()

            End If

        ElseIf tcRibbon.SelectedIndex = 2 Then

            If ModelInterface.Initialized Then

                ModelInterface.PostprocessMode()

                RaiseEvent SwitchToResultsMode()

            End If

        End If

    End Sub

#Region "Add, remove and clone objects"

    Private Sub btnAddObject_Click(sender As Object, e As EventArgs) Handles btnAddObject.Click

        Dim Dialog As New FormSelectObject

        If Dialog.ShowDialog() = DialogResult.OK Then

            If Dialog.rbFuselage.Checked Then
                ProjectRoot.Model.AddExtrudedBody()
            End If

            If Dialog.rbLiftingSurface.Checked Then
                ProjectRoot.Model.AddLiftingSurface()
            End If

            If Dialog.rbJetEngine.Checked Then
                ProjectRoot.Model.AddJetEngine()
            End If

            If Dialog.rbImported.Checked Then
                ProjectRoot.Model.AddImportedSurface()
            End If

        End If

        RefreshListOfObjects()

    End Sub

    Public Sub RefreshListOfObjects()

        If ModelInterface.Initialized Then

            Dim index As Integer = cbxSurfaces.SelectedIndex

            cbxSurfaces.Items.Clear()

            For Each Surface In ProjectRoot.Model.Objects

                cbxSurfaces.Items.Add(Surface.Name)

            Next

            If (index >= 0 And index < cbxSurfaces.Items.Count) Then

                cbxSurfaces.SelectedIndex = index

            End If

            ModelInterface.RefreshOnGL()

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

    Public Sub OpenProject(FilePath As String)

        Try

            ModelInterface.RestartProject()
            ProjectRoot.FilePath = FilePath
            ModelInterface.ReadFromXML()

            LoadVisualization()
            LoadSettings()

        Catch ex As Exception
            MsgBox("Error while reading proyect data file. File data might be corrupted.", MsgBoxStyle.OkOnly, "Error")
            FileClose(200)
        End Try

        ModelInterface.RefreshOnGL()
        RefreshListOfObjects()

    End Sub

    Private Sub OpenProject()

        Dim SaveBeforeClose As MsgBoxResult = MsgBox("The current project will be closed. Do you wish to save it?", vbYesNoCancel, "Opening exsisting project")

        Select Case SaveBeforeClose

            Case MsgBoxResult.Yes

                ' If the project has never been saved call save as.

                Dim Saved As Boolean = True

                If ProjectRoot.ExistsOnDatabase Then
                    SaveProject()
                Else
                    Saved = SaveProjectAs()
                End If

                If Saved Then
                    ModelInterface.RestartProject()
                Else
                    Exit Sub
                End If

            Case MsgBoxResult.Cancel
                Exit Sub

            Case MsgBoxResult.No

        End Select

        RaiseEvent ProjectCleared()

        Try

            Dim dlgOpenFile As New OpenFileDialog()

            dlgOpenFile.Filter = "Vogel proyect files (*.vog)|*.vog"

            Dim AcceptFile As MsgBoxResult = dlgOpenFile.ShowDialog()

            If AcceptFile = MsgBoxResult.Ok Then

                ModelInterface.RestartProject()
                ProjectRoot.FilePath = dlgOpenFile.FileName
                ModelInterface.ReadFromXML()

                LoadVisualization()
                LoadSettings()

            End If

        Catch ex As Exception
            MsgBox("Error while reading proyect data file. File data might be corrupted.", MsgBoxStyle.OkOnly, "Error")
            FileClose(200)
        End Try

        ModelInterface.RefreshOnGL()
        RefreshListOfObjects()

    End Sub

    Private Sub SaveProject()

        Try

            If ProjectRoot.ExistsOnDatabase Then
                ModelInterface.WriteToXML()
            Else
                SaveProjectAs()
            End If

            RaiseEvent PushMessage("The proyect has been saved")

        Catch E As Exception

            Dim SaveUnderDifferentName As MsgBoxResult = MsgBox("An exception was raised while saving the project! Do you wish to save it under a different name?", MsgBoxStyle.OkCancel, "Error!")
            If SaveUnderDifferentName = MsgBoxResult.Ok Then SaveProjectAs()

        End Try

    End Sub

    Public Function SaveProjectAs() As Boolean

        Dim Result As Boolean = False

        Try
            Dim Response As DialogResult

            Dim dlgSaveFile As New SaveFileDialog()

            dlgSaveFile.Filter = "Vogel proyect files (*.vog)|*.vog"
            Response = dlgSaveFile.ShowDialog()
            If Response = DialogResult.OK Then
                ProjectRoot.FilePath = dlgSaveFile.FileName
                ModelInterface.WriteToXML()
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
                ProjectRoot.RestartProject()
                RefreshListOfObjects()
                RaiseEvent ProjectCleared()
                RaiseEvent SwitchToDesignMode()

            Case MsgBoxResult.No

                ProjectRoot.RestartProject()
                RefreshListOfObjects()
                RaiseEvent ProjectCleared()
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

                ProjectRoot.Model.Objects.Add(Clone)

                RefreshListOfObjects()

            End If

        End If

    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        If cbxSurfaces.SelectedIndex >= 0 And cbxSurfaces.SelectedIndex < ProjectRoot.Model.Objects.Count Then

            ProjectRoot.Model.Objects.RemoveAt(cbxSurfaces.SelectedIndex)

            RefreshListOfObjects()

        End If

    End Sub

#End Region

#Region "Load ad manage screen properties"

    Private _LockScreenPropEvents As Boolean

    Private Sub LoadVisualization()

        _LockScreenPropEvents = True

        pnlScreenColor.BackColor = ModelInterface.Visualization.ScreenColor
        cbxShowRulers.Checked = ModelInterface.Visualization.ReferenceFrame.Visible
        nudXmax.Value = ModelInterface.Visualization.ReferenceFrame.Xmax
        nudXmin.Value = ModelInterface.Visualization.ReferenceFrame.Xmin
        nudYmax.Value = ModelInterface.Visualization.ReferenceFrame.Ymax
        nudYmin.Value = ModelInterface.Visualization.ReferenceFrame.Ymin

        _LockScreenPropEvents = False

    End Sub

    Private Sub pnlScreenColor_Click(sender As Object, e As EventArgs) Handles pnlScreenColor.Click

        If ModelInterface.Initialized AndAlso Not _LockScreenPropEvents Then

            Dim dialog As New ColorDialog

            ' load custom surface colors (colors from all other surfaces):

            Dim colors(0) As Integer

            Dim color As Drawing.Color = ModelInterface.Visualization.ScreenColor

            colors(0) = (CInt(color.B) << 16) + (CInt(color.G) << 8) + color.R

            dialog.CustomColors = colors

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                ModelInterface.Visualization.ScreenColor = dialog.Color

                pnlScreenColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowRulers_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowRulers.CheckedChanged

        If ModelInterface.Initialized AndAlso Not _LockScreenPropEvents Then

            ModelInterface.Visualization.ReferenceFrame.Visible = cbxShowRulers.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudXmax_ValueChanged(sender As Object, e As EventArgs) Handles nudXmax.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockScreenPropEvents Then

            ModelInterface.Visualization.ReferenceFrame.Xmax = nudXmax.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudXmin_ValueChanged(sender As Object, e As EventArgs) Handles nudXmin.ValueChanged

        If ModelInterface.Initialized AndAlso Not _LockScreenPropEvents Then

            ModelInterface.Visualization.ReferenceFrame.Xmin = nudXmin.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudYmax_ValueChanged(sender As Object, e As EventArgs) Handles nudYmax.ValueChanged

        If ModelInterface.Initialized AndAlso Not _LockScreenPropEvents Then

            ModelInterface.Visualization.ReferenceFrame.Ymax = nudYmax.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudYmin_ValueChanged(sender As Object, e As EventArgs) Handles nudYmin.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockScreenPropEvents Then

            ModelInterface.Visualization.ReferenceFrame.Ymin = nudYmin.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

#End Region

#Region "Load settigs and manage calculation"

    Private niNotification As New NotifyIcon()

    Public Sub Calculate(Optional ByVal CalculationType As CalculationType = CalculationType.SteadyState)

        If Not System.IO.File.Exists(ProjectRoot.FilePath) Then

            MsgBox("Please, save the project first.")

        End If

        Dim FormSettings = New FormSettings()

        FormSettings.Settings = ProjectRoot.SimulationSettings

        If (FormSettings.ShowDialog()) = DialogResult.OK Then

            FormSettings.GetSettings()

            RaiseEvent PushMessage("Calculation started.")

            niNotification.Text = "Calculating..."
            CalculationBussy = True

            Select Case CalculationType

                Case CalculationType.SteadyState

                    niNotification.BalloonTipText = "Calculating steady state"
                    niNotification.ShowBalloonTip(3000)

                Case CalculationType.FreeFlight

                    niNotification.BalloonTipText = "Calculating unsteady transit"
                    niNotification.ShowBalloonTip(3000)

                Case CalculationType.Aeroelastic

                    niNotification.BalloonTipText = "Calculating aeroelastic transit"
                    niNotification.ShowBalloonTip(3000)

            End Select

            AddHandler CalculationManager.CalculationDone, AddressOf PostCalculationActions

            CalculationManager.StartCalculation(CalculationType, cbxOnServer.Checked, Me.Parent)

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
            LoadFrames()
            FormReport.ReportResults()
            CalculationBussy = False
            RaiseEvent PushMessage("Calculation done!")
        End If

    End Sub

    Private Sub btnStartCalculation_Click(sender As Object, e As EventArgs) Handles btnStartCalculation.Click

        If ProjectRoot.Initialized Then

            FormReport.Hide()

            Calculate(cbxSimulationMode.SelectedIndex)

        End If

    End Sub

    Private _LockSettingsEvents As Boolean = False

    Private Sub LoadSettings()

        If ProjectRoot.Initialized Then

            _LockSettingsEvents = True

            cbxSimulationMode.SelectedIndex = ProjectRoot.SimulationSettings.AnalysisType
            nudVx.Value = ProjectRoot.SimulationSettings.StreamVelocity.X
            nudVy.Value = ProjectRoot.SimulationSettings.StreamVelocity.Y
            nudVz.Value = ProjectRoot.SimulationSettings.StreamVelocity.Z

            nudOx.Value = ProjectRoot.SimulationSettings.StreamRotation.X
            nudOy.Value = ProjectRoot.SimulationSettings.StreamRotation.Y
            nudOz.Value = ProjectRoot.SimulationSettings.StreamRotation.Z

            nudDensity.Value = ProjectRoot.SimulationSettings.Density
            nudViscosity.Value = ProjectRoot.SimulationSettings.Viscocity
            nudSteps.Value = ProjectRoot.SimulationSettings.SimulationSteps
            nudIncrement.Value = ProjectRoot.SimulationSettings.Interval

            _LockSettingsEvents = False

        End If

    End Sub

    Private Sub nudVx_ValueChanged(sender As Object, e As EventArgs) Handles nudVx.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.StreamVelocity.X = nudVx.Value

        End If

    End Sub

    Private Sub nudVy_ValueChanged(sender As Object, e As EventArgs) Handles nudVy.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.StreamVelocity.Y = nudVy.Value

        End If

    End Sub

    Private Sub nudVz_ValueChanged(sender As Object, e As EventArgs) Handles nudVz.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.StreamVelocity.Z = nudVz.Value

        End If

    End Sub

    Private Sub nudOx_ValueChanged(sender As Object, e As EventArgs) Handles nudOx.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.StreamRotation.X = nudOx.Value

        End If

    End Sub

    Private Sub nudOy_ValueChanged(sender As Object, e As EventArgs) Handles nudOy.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.StreamRotation.Y = nudOy.Value

        End If

    End Sub

    Private Sub nudOz_ValueChanged(sender As Object, e As EventArgs) Handles nudOz.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.StreamRotation.Z = nudOz.Value

        End If

    End Sub

    Private Sub nudDensity_ValueChanged(sender As Object, e As EventArgs) Handles nudDensity.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.Density = nudDensity.Value

        End If

    End Sub

    Private Sub nudViscosity_ValueChanged(sender As Object, e As EventArgs) Handles nudViscosity.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.Viscocity = nudViscosity.Value

        End If

    End Sub

    Private Sub nudSteps_ValueChanged(sender As Object, e As EventArgs) Handles nudSteps.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.SimulationSteps = nudSteps.Value

        End If

    End Sub

    Private Sub nudIncrement_ValueChanged(sender As Object, e As EventArgs) Handles nudIncrement.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.Interval = nudIncrement.Value

        End If

    End Sub

    Private Sub cbxSimulationMode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxSimulationMode.SelectedIndexChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.AnalysisType = cbxSimulationMode.SelectedIndex

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

        For i = 0 To ProjectRoot.Model.Objects.Count - 1

            ProjectRoot.Model.Objects(i).Active = i = cbxSurfaces.SelectedIndex

            If ProjectRoot.Model.Objects(i).Active Then

                _SelectedSurface = ProjectRoot.Model.Objects(i)

                If _SelectedSurface IsNot Nothing Then AddHandler _SelectedSurface.MeshUpdated, AddressOf UpdateMeshInfo

                LoadSelectedSurface()

            End If

        Next

        ModelInterface.RefreshOnGL()

    End Sub

    Private Sub LoadSelectedSurface()

        _LockPropsEvents = True

        If _SelectedSurface IsNot Nothing Then

            tbxName.Text = _SelectedSurface.Name
            cbxShowMesh.Checked = _SelectedSurface.VisualProperties.ShowMesh
            cbxShowSurface.Checked = _SelectedSurface.VisualProperties.ShowSurface
            pnlMeshColor.BackColor = _SelectedSurface.VisualProperties.ColorMesh
            pnlSurfaceColor.BackColor = _SelectedSurface.VisualProperties.ColorSurface
            cbxInclude.Checked = _SelectedSurface.IncludeInCalculation

            nudPx.Value = _SelectedSurface.Position.X
            nudPy.Value = _SelectedSurface.Position.Y
            nudPz.Value = _SelectedSurface.Position.Z

            nudCRx.Value = _SelectedSurface.CenterOfRotation.X
            nudCRy.Value = _SelectedSurface.CenterOfRotation.Y
            nudCRz.Value = _SelectedSurface.CenterOfRotation.Z

            nudPsi.Value = _SelectedSurface.Orientation.R1
            nudTita.Value = _SelectedSurface.Orientation.R2
            nudFi.Value = _SelectedSurface.Orientation.R3

            cbSecuence.SelectedIndex = _SelectedSurface.Orientation.Sequence

        End If

        _LockPropsEvents = False

        UpdateMeshInfo()

        ModelInterface.RefreshOnGL()

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

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowMesh.CheckedChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.VisualProperties.ShowMesh = cbxShowMesh.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlSurfaceColor.MouseClick

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            Dim dialog As New ColorDialog

            ' load custom surface colors (colors from all other surfaces):

            Dim colors(ProjectRoot.Model.Objects.Count - 1) As Integer

            For i = 0 To ProjectRoot.Model.Objects.Count - 1

                Dim color As Drawing.Color = ProjectRoot.Model.Objects(i).VisualProperties.ColorSurface

                colors(i) = (CInt(color.B) << 16) + (CInt(color.G) << 8) + color.R

            Next

            dialog.Color = _SelectedSurface.VisualProperties.ColorSurface

            dialog.CustomColors = colors

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                _SelectedSurface.VisualProperties.ColorSurface = dialog.Color

                pnlSurfaceColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlMeshColor.MouseClick

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            Dim dialog As New ColorDialog

            ' load custom surface colors (colors from all other meshes):

            Dim colors(ProjectRoot.Model.Objects.Count - 1) As Integer

            For i = 0 To ProjectRoot.Model.Objects.Count - 1

                Dim color As Drawing.Color = ProjectRoot.Model.Objects(i).VisualProperties.ColorMesh

                colors(i) = (CInt(color.B) << 16) + (CInt(color.G) << 8) + color.R

            Next

            dialog.Color = _SelectedSurface.VisualProperties.ColorMesh

            dialog.CustomColors = colors

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                _SelectedSurface.VisualProperties.ColorMesh = dialog.Color

                pnlMeshColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If
    End Sub

    Private Sub nudPx_ValueChanged(sender As Object, e As EventArgs) Handles nudPx.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Position.X = nudPx.Value

            _SelectedSurface.MoveTo(_SelectedSurface.Position)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudPy_ValueChanged(sender As Object, e As EventArgs) Handles nudPy.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Position.Y = nudPy.Value

            _SelectedSurface.MoveTo(_SelectedSurface.Position)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudPz_ValueChanged(sender As Object, e As EventArgs) Handles nudPz.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Position.Z = nudPz.Value

            _SelectedSurface.MoveTo(_SelectedSurface.Position)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCRx_ValueChanged(sender As Object, e As EventArgs) Handles nudCRx.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.CenterOfRotation.X = nudCRx.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCRy_ValueChanged(sender As Object, e As EventArgs) Handles nudCRy.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.CenterOfRotation.Y = nudCRy.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCRz_ValueChanged(sender As Object, e As EventArgs) Handles nudCRz.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.CenterOfRotation.Z = nudCRz.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudPsi_ValueChanged(sender As Object, e As EventArgs) Handles nudPsi.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.R1 = nudPsi.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudTita_ValueChanged(sender As Object, e As EventArgs) Handles nudTita.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.R2 = nudTita.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudFi_ValueChanged(sender As Object, e As EventArgs) Handles nudFi.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.R3 = nudFi.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbSecuence_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSecuence.SelectedIndexChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Sequence = Math.Max(0, cbSecuence.SelectedIndex)

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxInclude_CheckedChanged(sender As Object, e As EventArgs) Handles cbxInclude.CheckedChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.IncludeInCalculation = cbxInclude.Checked

        End If

    End Sub

#End Region

#Region "Load results and transit"

    Public Sub LoadResults(FilePath As String)

        Try
            ProjectRoot.ReadResults(FilePath)
            ModelInterface.PostprocessMode()
            FormReport.ReportResults()
            LoadResultProperties()
            LoadFrames()
            ModelInterface.RefreshOnGL()
        Catch

            MsgBox("Could not open the selected result file!")

        End Try

    End Sub

    Private Sub btnLoadResults_Click(sender As Object, e As EventArgs) Handles btnLoadResults.Click

        Dim dlgOpenFile As New OpenFileDialog

        dlgOpenFile.Filter = "Vogel result info (*.vri)|*.vri"

        Dim Respuesta2 As MsgBoxResult = dlgOpenFile.ShowDialog()

        If Respuesta2 = MsgBoxResult.Ok Then

            LoadResults(dlgOpenFile.FileName)

        End If
    End Sub

    Private Sub LoadFrames()

        cbxFrames.Items.Clear()

        If ProjectRoot.Initialized Then

            If ProjectRoot.Results.Frames.Count > 0 Then

                nudModeScale.Visible = True

                cbxFrames.Enabled = True
                nudModeScale.Enabled = True

                For i = 0 To ProjectRoot.Results.Frames.Count - 1
                    cbxFrames.Items.Add(ProjectRoot.Results.Frames(i).Model.Name)
                Next

                cbxFrames.SelectedIndex = ProjectRoot.Results.Frames.Count - 1

            Else

                cbxFrames.Items.Add("(no results)")
                cbxFrames.SelectedIndex = 0

                cbxFrames.Enabled = False
                nudModeScale.Enabled = False

            End If

            btnPlayStop.Enabled = ProjectRoot.Results.Frames.Count > 1

        End If

    End Sub

    Private Sub cbxFrames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxFrames.SelectedIndexChanged

        If ProjectRoot.Initialized Then

            If cbxFrames.SelectedIndex >= 0 And cbxFrames.SelectedIndex < ProjectRoot.Results.Frames.Count Then

                ProjectRoot.Results.ActiveFrame = ProjectRoot.Results.Frames(cbxFrames.SelectedIndex)
                LoadResultProperties()
                FormReport.UpdateLoads()
                ModelInterface.RefreshOnGL()
                FormAttitude.Refresh()

            End If

        End If

    End Sub

    Private Sub nudModeScale_ValueChanged(sender As Object, e As EventArgs) Handles nudModeScale.ValueChanged

        If ProjectRoot.Initialized Then

            If cbxFrames.SelectedIndex >= 0 And cbxFrames.SelectedIndex < ProjectRoot.Results.Frames.Count Then

                If ProjectRoot.Results.ActiveFrame.FrameKind = DesignTools.VisualModel.Models.ResultFrameKinds.DynamicMode Then

                    ProjectRoot.Results.ActiveFrame.Model.UpdateDisplacement(nudModeScale.Value)

                    ModelInterface.RefreshOnGL()

                End If

            End If

        End If

    End Sub

    Private _timer As New Timer
    Private _Simulating As Boolean = False
    Private _CurrentFrame As Integer = 0

    Private Sub btnPlayStop_Click(sender As Object, e As EventArgs) Handles btnPlayStop.Click

        If ModelInterface.Initialized Then

            If ProjectRoot.Results.Frames.Count > 1 Then

                If Not ModelInterface.Simulating Then

                    _timer.Interval = ProjectRoot.Results.Settings.Interval * 1000
                    ModelInterface.Simulating = True
                    _timer.Start()
                    btnPlayStop.Text = "Stop"
                    RaiseEvent PushMessage(String.Format("Simulating. Rate {0}f/{1}s",
                                                         ProjectRoot.Results.Settings.StructuralSettings.SubSteps,
                                                         ProjectRoot.Results.Settings.Interval))
                Else
                    ModelInterface.Simulating = False
                    _timer.Stop()
                    btnPlayStop.Text = "Play"
                    RaiseEvent PushMessage("Simulation stopped")
                End If

            Else

                ModelInterface.Simulating = False

            End If

        End If

    End Sub

    Private Sub _timer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If ModelInterface.Initialized And ProjectRoot.Results.Frames.Count > 0 Then

            If _CurrentFrame >= ProjectRoot.Results.Frames.Count Then _CurrentFrame = 0
            ProjectRoot.Results.ActiveFrame = ProjectRoot.Results.Frames(_CurrentFrame)
            ModelInterface.RepresentResultsTransitWithOpenGL(_CurrentFrame)
            FormAttitude.Refresh()
            _CurrentFrame += 1

        End If

    End Sub

    Private Sub StopTransit()

        If ModelInterface.Initialized Then
            ModelInterface.Simulating = False
            _timer.Stop()
        End If

    End Sub

#End Region

#Region "Postprocess properties"

    Private _LockResultPropsEvents As Boolean

    Private Sub LoadResultProperties()

        _LockResultPropsEvents = True

        If ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Model As ResultContainer = ProjectRoot.Results.ActiveFrame.Model

            cbxShowResMesh.Checked = Model.VisualProperties.ShowMesh
            cbxShowResSurface.Checked = Model.VisualProperties.ShowSurface
            pnlResultMeshColor.BackColor = Model.VisualProperties.ColorMesh
            pnlResultSurfaceColor.BackColor = Model.VisualProperties.ColorSurface

            pnlForceColor.BackColor = Model.VisualProperties.ColorPositiveLoad
            pnlVelocityColor.BackColor = Model.VisualProperties.ColorVelocity
            cbxShowForce.Checked = Model.VisualProperties.ShowLoadVectors
            cbxShowVelocity.Checked = Model.VisualProperties.ShowVelocityVectors

            nudScaleForce.Value = Model.VisualProperties.ScaleLoadVectors
            nudScaleVelocity.Value = Model.VisualProperties.ScaleVelocityVectors

            cbxShowColormap.Checked = Model.VisualProperties.ShowColormap
            nudDCpmax.Value = Model.PressureDeltaRange.Maximum
            nudDCpmin.Value = Model.PressureDeltaRange.Minimum
            nudCpmax.Value = Model.PressureRange.Maximum
            nudCpmin.Value = Model.PressureRange.Minimum

            Dim Wakes As ResultContainer = ProjectRoot.Results.ActiveFrame.Wakes

            cbxShowWakeSurface.Checked = Wakes.VisualProperties.ShowSurface
            cbxShowWakeMesh.Checked = Wakes.VisualProperties.ShowMesh
            cbxShowWakeNodes.Checked = Wakes.VisualProperties.ShowNodes

            pnlWakeSurfaceColor.BackColor = Wakes.VisualProperties.ColorSurface
            pnlWakeMeshColor.BackColor = Wakes.VisualProperties.ColorMesh
            pnlWakeNodeColor.BackColor = Wakes.VisualProperties.ColorNodes

            gbFrames.Enabled = True
            nudModeScale.Enabled = ProjectRoot.Results.ActiveFrame.FrameKind = DesignTools.VisualModel.Models.ResultFrameKinds.DynamicMode

        End If

        _LockResultPropsEvents = False

    End Sub

    Private Sub cbxShowColormap_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowColormap.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ShowColormap = cbxShowColormap.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlResultSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlResultSurfaceColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorSurface

            ' Show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorSurface = Dialog.Color

                pnlResultSurfaceColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlResultMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlResultMeshColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorMesh

            ' show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorMesh = Dialog.Color

                pnlResultMeshColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlVelocityColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlVelocityColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorVelocity

            ' show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorVelocity = Dialog.Color

                pnlVelocityColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlForceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlForceColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorPositiveLoad

            ' show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ColorPositiveLoad = Dialog.Color

                pnlForceColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudScaleVelocity_ValueChanged(sender As Object, e As EventArgs) Handles nudScaleVelocity.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ScaleVelocityVectors = nudScaleVelocity.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudScaleForce_ValueChanged(sender As Object, e As EventArgs) Handles nudScaleForce.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ScaleLoadVectors = nudScaleForce.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeSurface_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeSurface.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ShowSurface = cbxShowWakeSurface.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeMesh.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ShowMesh = cbxShowWakeMesh.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeNodes_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeNodes.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ShowNodes = cbxShowWakeNodes.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeSurfaceColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ColorSurface

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ColorSurface = dialog.Color

                pnlWakeSurfaceColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeMeshColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ColorMesh

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ColorMesh = dialog.Color

                pnlWakeMeshColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeNodeColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeNodeColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim dialog As New ColorDialog

            dialog.Color = ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ColorNodes

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.ActiveFrame.Wakes.VisualProperties.ColorNodes = dialog.Color

                pnlWakeNodeColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowResSurface_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowResSurface.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ShowSurface = cbxShowResSurface.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowResMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowResMesh.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ShowMesh = cbxShowResMesh.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowVelocity_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowVelocity.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ShowVelocityVectors = cbxShowVelocity.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowForce_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowForce.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.VisualProperties.ShowLoadVectors = cbxShowForce.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub btnEditVelocityPlane_Click(sender As Object, e As EventArgs) Handles btnShowAttitude.Click

        FormAttitude.Show()

    End Sub

    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click

        If ProjectRoot.Initialized Then

            If FormReport.Visible Then
                FormReport.Hide()
            Else

                If CalculationBussy Then
                    MsgBox("Please, wait until the calculation is done.")
                Else
                    If Results.Frames.Count = 0 Then

                        MsgBox("Results are not available. Load a results file or execute the calculation first.")

                    ElseIf Results.ActiveFrame IsNot Nothing AndAlso
                           Results.ActiveFrame.FrameKind <> DesignTools.VisualModel.Models.ResultFrameKinds.DynamicMode Then

                        FormReport.Show(ParentForm)

                    Else

                        MsgBox("Results are not available for the selected frame.")

                    End If

                End If

            End If

        End If

    End Sub

    Private Sub nudDCpmax_ValueChanged(sender As Object, e As EventArgs) Handles nudDCpmax.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.PressureDeltaRange.Maximum = nudDCpmax.Value

            ProjectRoot.Results.ActiveFrame.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudDCpmin_ValueChanged(sender As Object, e As EventArgs) Handles nudDCpmin.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.PressureDeltaRange.Minimum = nudDCpmin.Value

            ProjectRoot.Results.ActiveFrame.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCpmin_ValueChanged(sender As Object, e As EventArgs) Handles nudCpmin.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.PressureRange.Minimum = nudCpmin.Value

            ProjectRoot.Results.ActiveFrame.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCpmax_ValueChanged(sender As Object, e As EventArgs) Handles nudCpmax.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.PressureRange.Maximum = nudCpmax.Value

            ProjectRoot.Results.ActiveFrame.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub btnResetColormap_Click(sender As Object, e As EventArgs) Handles btnResetColormap.Click

        If ProjectRoot.Initialized And ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            ProjectRoot.Results.ActiveFrame.Model.FindPressureRange()

            ProjectRoot.Results.ActiveFrame.Model.UpdatePressureColormap()

            _LockResultPropsEvents = True

            nudDCpmax.Value = ProjectRoot.Results.ActiveFrame.Model.PressureDeltaRange.Maximum
            nudDCpmin.Value = ProjectRoot.Results.ActiveFrame.Model.PressureDeltaRange.Minimum

            nudCpmax.Value = ProjectRoot.Results.ActiveFrame.Model.PressureRange.Maximum
            nudCpmin.Value = ProjectRoot.Results.ActiveFrame.Model.PressureRange.Minimum

            _LockResultPropsEvents = False

            ModelInterface.RefreshOnGL()

        End If

    End Sub

#End Region

#Region " Representation "

    Private Sub LoadViewParameters(Optional ByVal Vista As String = "Free")

        If ProjectRoot.Initialized Then

            Select Case Vista

                Case "XY"
                    ModelInterface.Visualization.CameraOrientation.R1 = 0
                    ModelInterface.Visualization.CameraOrientation.R3 = 0
                    RaiseEvent PushMessage("XY view")

                Case "ZY"
                    ModelInterface.Visualization.CameraOrientation.R1 = 90
                    ModelInterface.Visualization.CameraOrientation.R3 = -90
                    RaiseEvent PushMessage("ZY view")

                Case "ZX"
                    ModelInterface.Visualization.CameraOrientation.R1 = 0
                    ModelInterface.Visualization.CameraOrientation.R3 = -90
                    RaiseEvent PushMessage("ZX view")

                Case "Isometrica"
                    ModelInterface.Visualization.CameraOrientation.R1 = 30
                    ModelInterface.Visualization.CameraOrientation.R3 = -60
                    RaiseEvent PushMessage("Free view")

                Case "Center"
                    ModelInterface.Visualization.CameraPosition.X = 0
                    ModelInterface.Visualization.CameraPosition.Y = 0
                    ModelInterface.Visualization.CameraPosition.Z = 0

            End Select

            ModelInterface.RefreshOnGL()

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

    Private Sub btnMove_Click(sender As Object, e As EventArgs)

        ProjectRoot.Model.OperationsTool.CancelOperation()
        ProjectRoot.Model.OperationsTool.Operation = Operations.Translate
        RaiseEvent PushMessage("Translate surface")

    End Sub

    Private Sub bntAlign_Click(sender As Object, e As EventArgs)

        ProjectRoot.Model.OperationsTool.CancelOperation()
        ProjectRoot.Model.OperationsTool.Operation = Operations.Align
        RaiseEvent PushMessage("Align surface")

    End Sub

    Private Sub btnHistogram_Click(sender As Object, e As EventArgs) Handles btnHistogram.Click

        If cbxSimulationMode.SelectedIndex = 2 Then

            Dim Dialog As New FormHistogram

            Dialog.StartPosition = FormStartPosition.CenterParent

            Dialog.ShowDialog()

        End If

    End Sub

    Private Sub rbNode_CheckedChanged(sender As Object, e As EventArgs) Handles rbNode.CheckedChanged

        ModelInterface.Selection.EntityToSelect = EntityTypes.etNode

    End Sub

    Private Sub rnPanel_CheckedChanged(sender As Object, e As EventArgs) Handles rnPanel.CheckedChanged

        ModelInterface.Selection.EntityToSelect = EntityTypes.etPanel

    End Sub

    Private Sub cbMultiselect_CheckedChanged(sender As Object, e As EventArgs) Handles cbMultiselect.CheckedChanged

        ModelInterface.Selection.MultipleSelection = cbMultiselect.Checked

    End Sub

    Private Sub rbStructure_CheckedChanged(sender As Object, e As EventArgs) Handles rbStructure.CheckedChanged

        ModelInterface.Selection.EntityToSelect = EntityTypes.etStructuralElement

    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        If System.IO.File.Exists(FilePath) Then

            FormExport.ShowDialog()

        Else
            MessageBox.Show("Please, save the project first")

        End If

    End Sub

    Private Sub btnISA_Click(sender As Object, e As EventArgs) Handles btnISA.Click

        Dim FormISA As New FormAtmosphere
        If (FormISA.ShowDialog()) = DialogResult.OK Then

            FormISA.GetSettings(ProjectRoot.SimulationSettings)
            LoadSettings()

        End If

    End Sub

    Private Sub butInertia_Click(sender As Object, e As EventArgs) Handles butInertia.Click

        If Model IsNot Nothing Then
            FormInertia.SetReadOnly(True)
            FormInertia.SetInertia(Model.GetGlobalInertia)
            FormInertia.ShowDialog()
            FormInertia.SetReadOnly(False)
        End If

    End Sub

#End Region

End Class

﻿'Open VOGEL (openvogel.org)
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

Public Class MainRibbon

    Public Event SwitchToDesignMode()
    Public Event SwitchToResultsMode()
    Public Event PushMessage(msg As String)
    Public Event EditSurface(ByRef Surface As Surface)
    Public Event EditVelocityPlane()
    Public Event ProjectCleared()

    Private CalculationBussy As Boolean = False
    Private FormReport As New FormReport

    Public Sub New()

        InitializeComponent()

        cbxSimulationMode.Items.Add("Steady")
        cbxSimulationMode.Items.Add("Unsteady")
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

        ElseIf tcRibbon.SelectedIndex = 2

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

    Public Sub Calculate(Optional ByVal CalculationType As CalculationType = CalculationType.ctSteady)

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
            LoadModes()
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

            nudOx.Value = ProjectRoot.SimulationSettings.Omega.X
            nudOy.Value = ProjectRoot.SimulationSettings.Omega.Y
            nudOz.Value = ProjectRoot.SimulationSettings.Omega.Z

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

            ProjectRoot.SimulationSettings.Omega.X = nudOx.Value

        End If

    End Sub

    Private Sub nudOy_ValueChanged(sender As Object, e As EventArgs) Handles nudOy.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.Omega.Y = nudOy.Value

        End If

    End Sub

    Private Sub nudOz_ValueChanged(sender As Object, e As EventArgs) Handles nudOz.ValueChanged

        If ProjectRoot.Initialized AndAlso Not _LockSettingsEvents Then

            ProjectRoot.SimulationSettings.Omega.Z = nudOz.Value

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

            nudPsi.Value = _SelectedSurface.Orientation.Psi
            nudTita.Value = _SelectedSurface.Orientation.Tita
            nudFi.Value = _SelectedSurface.Orientation.Fi

            cbSecuence.SelectedIndex = _SelectedSurface.Orientation.Secuence

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

            _SelectedSurface.Orientation.Psi = nudPsi.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudTita_ValueChanged(sender As Object, e As EventArgs) Handles nudTita.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Tita = nudTita.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudFi_ValueChanged(sender As Object, e As EventArgs) Handles nudFi.ValueChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Fi = nudFi.Value

            _SelectedSurface.Orientate(_SelectedSurface.CenterOfRotation, _SelectedSurface.Orientation)

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbSecuence_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbSecuence.SelectedIndexChanged

        If _SelectedSurface IsNot Nothing AndAlso Not _LockPropsEvents Then

            _SelectedSurface.Orientation.Secuence = Math.Max(0, cbSecuence.SelectedIndex)

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

    Private Sub btnLoadResults_Click(sender As Object, e As EventArgs) Handles btnLoadResults.Click

        Try

            Dim dlgOpenFile As New OpenFileDialog

            dlgOpenFile.Filter = "Vogel result files (*.res)|*.res"

            Dim Respuesta2 As MsgBoxResult = dlgOpenFile.ShowDialog()

            If Respuesta2 = MsgBoxResult.Ok Then

                ProjectRoot.ReadResults(dlgOpenFile.FileName)

                ModelInterface.PostprocessMode()

                FormReport.ReportResults()

                LoadResultProperties()

                LoadModes()

            End If

            ModelInterface.RefreshOnGL()

        Catch

            MsgBox("Could not open the selected result file!")

        End Try

    End Sub

    Private Sub LoadModes()

        cbxModes.Items.Clear()

        If ProjectRoot.Initialized Then

            If Not IsNothing(ProjectRoot.Results.DynamicModes) Then

                If ProjectRoot.Results.DynamicModes.Count > 0 Then

                    nudModeScale.Visible = True

                    cbxModes.Enabled = True
                    nudModeScale.Enabled = True

                    cbxModes.Items.Add("Displacements")

                    For i = 0 To ProjectRoot.Results.DynamicModes.Count - 1
                        cbxModes.Items.Add(ProjectRoot.Results.DynamicModes(i).Name)
                    Next

                    cbxModes.SelectedIndex = 0

                Else

                    cbxModes.Items.Add("Results")
                    cbxModes.SelectedIndex = 0

                    cbxModes.Enabled = False
                    nudModeScale.Enabled = False

                End If

            End If

            btnPlayStop.Enabled = ProjectRoot.Results.TransitLoaded

        End If

    End Sub

    Private Sub cbxModes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxModes.SelectedIndexChanged

        If ProjectRoot.Initialized Then

            If Not IsNothing(ProjectRoot.Results.DynamicModes) Then

                If cbxModes.SelectedIndex > -1 And cbxModes.SelectedIndex < ProjectRoot.Results.DynamicModes.Count + 1 Then

                    If (cbxModes.SelectedIndex = 0) Then
                        ProjectRoot.Results.VisualizeModes = False
                    Else
                        ProjectRoot.Results.VisualizeModes = True
                        ProjectRoot.Results.SelectedModeIndex = cbxModes.SelectedIndex - 1
                        ProjectRoot.Results.SelectedMode.UpdateDisplacement(nudModeScale.Value)
                    End If

                    ModelInterface.RefreshOnGL()

                End If

            End If

        End If

    End Sub

    Private Sub nudModeScale_ValueChanged(sender As Object, e As EventArgs) Handles nudModeScale.ValueChanged

        If ProjectRoot.Initialized Then

            If Not IsNothing(ProjectRoot.Results.DynamicModes) Then

                If cbxModes.SelectedIndex > 0 And cbxModes.SelectedIndex < ProjectRoot.Results.DynamicModes.Count + 1 Then

                    ProjectRoot.Results.SelectedMode.UpdateDisplacement(nudModeScale.Value)

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

            If ProjectRoot.Results.TransitLoaded Then

                If Not ModelInterface.Simulating Then
                    _timer.Interval = ProjectRoot.Results.SimulationSettings.Interval * 1000 / ProjectRoot.Results.SimulationSettings.StructuralSettings.SubSteps
                    ModelInterface.Simulating = True
                    _timer.Start()
                    btnPlayStop.Text = "Stop"
                    RaiseEvent PushMessage(String.Format("Simulating. Rate {0}f/{1}s", ProjectRoot.Results.SimulationSettings.StructuralSettings.SubSteps, ProjectRoot.Results.SimulationSettings.Interval))
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

        If ModelInterface.Initialized Then
            If _CurrentFrame > ProjectRoot.Results.TransitStages Then _CurrentFrame = 0
            ModelInterface.RepresentResultsTransitWithOpenGL(_CurrentFrame)
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

        If ProjectRoot.Initialized Then

            cbxShowResMesh.Checked = ProjectRoot.Results.Model.VisualProperties.ShowMesh
            cbxShowResSurface.Checked = ProjectRoot.Results.Model.VisualProperties.ShowSurface
            pnlResultMeshColor.BackColor = ProjectRoot.Results.Model.VisualProperties.ColorMesh
            pnlResultSurfaceColor.BackColor = ProjectRoot.Results.Model.VisualProperties.ColorSurface

            pnlForceColor.BackColor = ProjectRoot.Results.Model.VisualProperties.ColorPositiveLoad
            pnlVelocityColor.BackColor = ProjectRoot.Results.Model.VisualProperties.ColorVelocity
            cbxShowForce.Checked = ProjectRoot.Results.Model.VisualProperties.ShowLoadVectors
            cbxShowVelocity.Checked = ProjectRoot.Results.Model.VisualProperties.ShowVelocityVectors

            nudScaleForce.Value = ProjectRoot.Results.Model.VisualProperties.ScaleLoadVectors
            nudScaleVelocity.Value = ProjectRoot.Results.Model.VisualProperties.ScaleVelocityVectors

            cbxShowColormap.Checked = ProjectRoot.Results.Model.VisualProperties.ShowColormap
            nudDCpmax.Value = ProjectRoot.Results.Model.PressureDeltaRange.Maximum
            nudDCpmin.Value = ProjectRoot.Results.Model.PressureDeltaRange.Minimum
            nudCpmax.Value = ProjectRoot.Results.Model.PressureRange.Maximum
            nudCpmin.Value = ProjectRoot.Results.Model.PressureRange.Minimum

            cbxShowWakeSurface.Checked = ProjectRoot.Results.Wakes.VisualProperties.ShowSurface
            cbxShowWakeMesh.Checked = ProjectRoot.Results.Wakes.VisualProperties.ShowMesh
            cbxShowWakeNodes.Checked = ProjectRoot.Results.Wakes.VisualProperties.ShowNodes

            pnlWakeSurfaceColor.BackColor = ProjectRoot.Results.Wakes.VisualProperties.ColorSurface
            pnlWakeMeshColor.BackColor = ProjectRoot.Results.Wakes.VisualProperties.ColorMesh
            pnlWakeNodeColor.BackColor = ProjectRoot.Results.Wakes.VisualProperties.ColorNodes

            cbxShowVelocityPlane.Checked = ProjectRoot.VelocityPlane.Visible

            gbxAeroelastic.Enabled = ProjectRoot.Results.DynamicModes.Count > 0

        End If

        _LockResultPropsEvents = False

    End Sub

    Private Sub cbxShowColormap_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowColormap.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.VisualProperties.ShowColormap = cbxShowColormap.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlResultSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlResultSurfaceColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.Model.VisualProperties.ColorSurface

            ' show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.Model.VisualProperties.ColorSurface = Dialog.Color

                pnlResultSurfaceColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlResultMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlResultMeshColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.Model.VisualProperties.ColorMesh

            ' show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.Model.VisualProperties.ColorMesh = Dialog.Color

                pnlResultMeshColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlVelocityColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlVelocityColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.Model.VisualProperties.ColorVelocity

            ' show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.Model.VisualProperties.ColorVelocity = Dialog.Color

                pnlVelocityColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlForceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlForceColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            Dim Dialog As New ColorDialog

            Dialog.Color = ProjectRoot.Results.Model.VisualProperties.ColorPositiveLoad

            ' show dialog:

            If Dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.Model.VisualProperties.ColorPositiveLoad = Dialog.Color

                pnlForceColor.BackColor = Dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudScaleVelocity_ValueChanged(sender As Object, e As EventArgs) Handles nudScaleVelocity.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.VisualProperties.ScaleVelocityVectors = nudScaleVelocity.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudScaleForce_ValueChanged(sender As Object, e As EventArgs) Handles nudScaleForce.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.VisualProperties.ScaleLoadVectors = nudScaleForce.Value

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeSurface_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeSurface.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Wakes.VisualProperties.ShowSurface = cbxShowWakeSurface.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeMesh.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Wakes.VisualProperties.ShowMesh = cbxShowWakeMesh.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowWakeNodes_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowWakeNodes.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Wakes.VisualProperties.ShowNodes = cbxShowWakeNodes.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeSurfaceColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeSurfaceColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            Dim dialog As New ColorDialog

            dialog.Color = ProjectRoot.Results.Wakes.VisualProperties.ColorSurface

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.Wakes.VisualProperties.ColorSurface = dialog.Color

                pnlWakeSurfaceColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeMeshColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeMeshColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            Dim dialog As New ColorDialog

            dialog.Color = ProjectRoot.Results.Wakes.VisualProperties.ColorMesh

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.Wakes.VisualProperties.ColorMesh = dialog.Color

                pnlWakeMeshColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub pnlWakeNodeColor_MouseClick(sender As Object, e As MouseEventArgs) Handles pnlWakeNodeColor.MouseClick

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            Dim dialog As New ColorDialog

            dialog.Color = ProjectRoot.Results.Wakes.VisualProperties.ColorNodes

            ' show dialog:

            If dialog.ShowDialog = DialogResult.OK Then

                ProjectRoot.Results.Wakes.VisualProperties.ColorNodes = dialog.Color

                pnlWakeNodeColor.BackColor = dialog.Color

            End If

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowResSurface_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowResSurface.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.VisualProperties.ShowSurface = cbxShowResSurface.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowResMesh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowResMesh.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.VisualProperties.ShowMesh = cbxShowResMesh.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowVelocity_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowVelocity.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.VisualProperties.ShowVelocityVectors = cbxShowVelocity.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowForce_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowForce.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.VisualProperties.ShowLoadVectors = cbxShowForce.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub cbxShowVelocityPlane_CheckedChanged(sender As Object, e As EventArgs) Handles cbxShowVelocityPlane.CheckedChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.VelocityPlane.Visible = cbxShowVelocityPlane.Checked

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub btnEditVelocityPlane_Click(sender As Object, e As EventArgs) Handles btnEditVelocityPlane.Click

        RaiseEvent EditVelocityPlane()

    End Sub

    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click

        If ProjectRoot.Initialized Then
            If FormReport.Visible Then
                FormReport.Hide()
            Else
                If CalculationBussy Then
                    MsgBox("Please, wait until the calculation is done.")
                Else
                    If (Not ProjectRoot.CalculationCore Is Nothing) Then
                        FormReport.Show(ParentForm)
                    Else
                        MsgBox("Results are not available. Load a results file or execute the calculation first.")
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub nudDCpmax_ValueChanged(sender As Object, e As EventArgs) Handles nudDCpmax.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.PressureDeltaRange.Maximum = nudDCpmax.Value

            ProjectRoot.Results.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudDCpmin_ValueChanged(sender As Object, e As EventArgs) Handles nudDCpmin.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.PressureDeltaRange.Minimum = nudDCpmin.Value

            ProjectRoot.Results.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCpmin_ValueChanged(sender As Object, e As EventArgs) Handles nudCpmin.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.PressureRange.Minimum = nudCpmin.Value

            ProjectRoot.Results.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub nudCpmax_ValueChanged(sender As Object, e As EventArgs) Handles nudCpmax.ValueChanged

        If (Not _LockResultPropsEvents) And ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.PressureRange.Maximum = nudCpmax.Value

            ProjectRoot.Results.Model.UpdatePressureColormap()

            ModelInterface.RefreshOnGL()

        End If

    End Sub

    Private Sub btnResetColormap_Click(sender As Object, e As EventArgs) Handles btnResetColormap.Click

        If ProjectRoot.Initialized Then

            ProjectRoot.Results.Model.FindPressureRange()

            ProjectRoot.Results.Model.UpdatePressureColormap()

            _LockResultPropsEvents = True

            nudDCpmax.Value = ProjectRoot.Results.Model.PressureDeltaRange.Maximum
            nudDCpmin.Value = ProjectRoot.Results.Model.PressureDeltaRange.Minimum

            nudCpmax.Value = ProjectRoot.Results.Model.PressureRange.Maximum
            nudCpmin.Value = ProjectRoot.Results.Model.PressureRange.Minimum

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
                    ModelInterface.Visualization.CameraOrientation.Psi = 0
                    ModelInterface.Visualization.CameraOrientation.Fi = 0
                    RaiseEvent PushMessage("XY view")

                Case "ZY"
                    ModelInterface.Visualization.CameraOrientation.Psi = 90
                    ModelInterface.Visualization.CameraOrientation.Fi = -90
                    RaiseEvent PushMessage("ZY view")

                Case "ZX"
                    ModelInterface.Visualization.CameraOrientation.Psi = 0
                    ModelInterface.Visualization.CameraOrientation.Fi = -90
                    RaiseEvent PushMessage("ZX view")

                Case "Isometrica"
                    ModelInterface.Visualization.CameraOrientation.Psi = 30
                    ModelInterface.Visualization.CameraOrientation.Fi = -60
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

        If cbxSimulationMode.SelectedIndex = 1 Then

            Dim Dialog As New FormUnsteadyVelocity

            Dialog.StartPosition = FormStartPosition.CenterParent

            Dialog.ShowProfile(ProjectRoot.SimulationSettings, True)

        ElseIf cbxSimulationMode.SelectedIndex = 2 Then

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

#End Region

End Class

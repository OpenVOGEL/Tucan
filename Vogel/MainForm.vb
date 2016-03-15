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

Imports MathTools.Algebra.EuclideanSpace
Imports AeroTools
Imports AeroTools.VisualModel.Interface
Imports AeroTools.VisualModel.Models.Components
Imports AeroTools.UVLM.Settings

Public Class MainForm

#Region " Initialize program "

    Public Project As New AircraftProject
    Public FormCargado As Boolean = False
    Public PostProcesoCargado As Boolean = False

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Initialize OpenGL control:

        Text = "Open VOGEL 2016 Beta"

        ControlOpenGL.Dock = DockStyle.Fill
        Project.SetControlGL(ControlOpenGL.OpenGL)

        Contenedor.ContentPanel.Height = 0

        ' Force design mode:

        SwitchToDesignMode()

        FormCargado = True

        AddHandler tsmFieldEvaluation.Click, AddressOf ShowVelocityPlane
        AddHandler tsbFieldEvaluation.Click, AddressOf ShowVelocityPlane

        ' Read command line arguments:

        Try

            Dim arguments As String() = Environment.GetCommandLineArgs

            If arguments.Length > 1 Then


                For Each argument In arguments

                    MsgBox(String.Format("Argument found: {0}", argument))

                    If IO.File.Exists(argument) And IO.Path.GetExtension(argument) = ".vog" Then

                        MsgBox(String.Format("Trying to open file {0}", argument))
                        Me.OpenProject(argument)
                        Exit For

                    End If

                Next

            End If

        Finally

        End Try

    End Sub

    Public Sub ReportState(ByVal Mensaje As String)

        lblStatus.Text = Mensaje

    End Sub

#End Region

#Region " Save project "

    Private Sub GuardarToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveProject.Click

        SaveProject()

    End Sub

    Private Sub SaveProject()

        Try

            If Me.Project.ExistsOnDatabase Then
                Project.WriteToXML()
            Else
                SaveProjectAs()
            End If

            lblStatus.Text = "The proyect has been saved"

        Catch ex As Exception

            Dim GuardarConOtroNombre As MsgBoxResult = MsgBox("An exception was raised while saving the project! Do you wish to save it under a different name?", MsgBoxStyle.OkCancel, "Error!")
            If GuardarConOtroNombre = MsgBoxResult.Ok Then Me.SaveProjectAs()

        End Try

    End Sub

    Public Function SaveProjectAs() As Boolean

        Dim Result As Boolean = False

        Try
            Dim Respuesta As DialogResult

            dlgSaveFile.Filter = "Vogel proyect files (*.vog)|*.vog"
            Respuesta = dlgSaveFile.ShowDialog()
            If Respuesta = Windows.Forms.DialogResult.OK Then
                Me.Project.FilePath = dlgSaveFile.FileName
                Project.WriteToXML()
                Result = True
            End If
        Catch ex As Exception
            MsgBox("Error while saving project!", MsgBoxStyle.OkOnly, "Error!")
            Result = False
        End Try

        Return Result

    End Function

    Private Sub GuardarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveAs.Click
        Me.SaveProjectAs()
    End Sub

#End Region

#Region " Open project "

    Private Overloads Sub OpenProject()

        Dim Respuesta1 As MsgBoxResult = MsgBox("Current project will be closed. Do you wish to save it?", vbYesNoCancel, "Opening exsisting project")

        Select Case Respuesta1

            Case MsgBoxResult.Yes

                ' If the project has never been saved call save as.

                Dim Saved As Boolean = True

                If Me.Project.ExistsOnDatabase Then
                    Me.SaveProject()
                Else
                    Saved = SaveProjectAs()
                End If

                If Saved Then
                    Me.Project.RestartProject()
                    Me.WingControlPanel.Hide()
                    Me.SwitchToDesignMode()
                Else
                    Exit Sub
                End If

            Case MsgBoxResult.Cancel
                Exit Sub

            Case MsgBoxResult.No
                Me.WingControlPanel.Hide()
                Me.SwitchToDesignMode()

        End Select

        Try

            dlgOpenFile.Filter = "Vogel proyect files (*.vog)|*.vog"

            Dim Respuesta2 As MsgBoxResult = dlgOpenFile.ShowDialog()

            If Respuesta2 = MsgBoxResult.Ok Then

                Project.RestartProject()
                Project.FilePath = dlgOpenFile.FileName
                Project.ReadFromXML()

                SwitchToDesignMode()

            End If

        Catch ex As Exception
            MsgBox("Error while reading proyect data file. File data might be corrupted.", MsgBoxStyle.OkOnly, "Error")
            FileClose(200)
        End Try

    End Sub

    Private Overloads Sub OpenProject(ByVal Path As String)

        Try

            Me.Project.RestartProject()
            Me.Project.FilePath = Path
            Project.ReadFromXML()
            Me.SwitchToDesignMode()
            Me.Text = "UVLM Solver - " & Me.Project.Name

        Catch ex As Exception
            MsgBox("Error while reading proyect data file. File data might be corrupted.", MsgBoxStyle.OkOnly, "Error")
            FileClose(200)
        End Try

    End Sub

    Private Sub AbrirToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.OpenProject()

    End Sub

    Private Sub AbrirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenFile.Click, btnOpenProject.Click
        Me.OpenProject()
    End Sub

#End Region

#Region " Open results file "

    Private Sub btnLoadResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadResults.Click

        Try

            dlgOpenFile.Filter = "Vogel result files (*.res)|*.res"

            Dim Respuesta2 As MsgBoxResult = dlgOpenFile.ShowDialog()

            If Respuesta2 = MsgBoxResult.Ok Then

                Project.ReadResults(dlgOpenFile.FileName)
                SwitchToPostprocessMode()

            End If

            Project.RepresentOnGL()

        Catch

            MsgBox("Could not open the selected result file!")

        End Try

    End Sub

#End Region

#Region " New project "

    Private Sub btnNewProject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewProject.Click

        Me.GenerateNewProject()

    End Sub

    Public Sub GenerateNewProject()

        Dim Respuesta As MsgBoxResult = MsgBox("¿Desea guardar el proyecto actual antes de crear o abrir uno nuevo?", vbYesNoCancel, "Comenzar un nuevo proyecto")

        On Error GoTo ErrSub

        Select Case Respuesta

            Case MsgBoxResult.Yes

                SaveProject()
                Project.RestartProject()
                SwitchToDesignMode()
                WingControlPanel.Hide()

            Case MsgBoxResult.No

                Project.RestartProject()
                SwitchToDesignMode()
                WingControlPanel.Hide()

            Case MsgBoxResult.Cancel

        End Select

        Me.PostProcesoCargado = False

        Exit Sub

ErrSub:

        MsgBox("Error al crear el nuevo proyecto!", MsgBoxStyle.Exclamation, "Error")

    End Sub

#End Region

#Region " Start calculation "

    Public Sub Calculate(Optional ByVal CalculationType As CalculationType = CalculationType.ctSteady)

        Dim FormSettings = New FormSettings()

        FormSettings.Settings = Project.SimulationSettings
        If (FormSettings.ShowDialog()) = Windows.Forms.DialogResult.OK Then
            FormSettings.GetSettings()

            niNotificationTool.Text = "Calculating..."

            Select Case CalculationType

                Case CalculationType.ctSteady

                    niNotificationTool.BalloonTipText = "Calculating steady state"
                    niNotificationTool.ShowBalloonTip(2000)

                Case CalculationType.ctUnsteady

                    niNotificationTool.BalloonTipText = "Calculating unsteady transit"
                    niNotificationTool.ShowBalloonTip(2000)

                Case CalculationType.ctAeroelastic

                    niNotificationTool.BalloonTipText = "Calculating aeroelastic transit"
                    niNotificationTool.ShowBalloonTip(2000)

            End Select

            AddHandler Project.CalculationDone, AddressOf PostCalculationActions

            Project.StartCalculation(CalculationType, Me)

        End If

    End Sub

    Private Sub PostCalculationActions()

        If InvokeRequired Then
            BeginInvoke(New Action(AddressOf PostCalculationActions))
        Else
            SwitchToPostprocessMode()
            niNotificationTool.Text = "Ready"
            niNotificationTool.BalloonTipText = "Calculation done!"
            niNotificationTool.ShowBalloonTip(3000)
        End If

    End Sub

    Private Sub btnStartSteady_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartSteady.Click
        Me.Calculate(CalculationType.ctSteady)
    End Sub

    Private Sub btnStartUnsteady_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartUnsteady.Click
        Me.Calculate(CalculationType.ctUnsteady)
    End Sub

    Private Sub btnStartAeroelastic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartAeroelastic.Click
        Me.Calculate(CalculationType.ctAeroelastic)
    End Sub

#End Region

#Region " Switching modes "

    Private Sub btnDesign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDesign.Click
        Me.SwitchToDesignMode()
    End Sub

    Private Sub btnPostprocess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostprocess.Click
        Me.SwitchToPostprocessMode()
    End Sub

    ''' <summary>
    ''' Changes project mode and window state to design mode.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SwitchToDesignMode()

        SetUpLiftingSurfaceEditor()
        SetUpVelocityPlaneFrame()

        Project.DesignMode()

        btnDesign.Checked = True
        btnPostprocess.Checked = False

        ' Hide/show tool strip buttons:

        btnOpenObjectsManager.Enabled = True
        btnPlayStop.Visible = False
        btnFieldEvaluation.Visible = False
        btTranslate.Visible = True
        btnAlign.Visible = False ' this operation needs to be reviewed
        VelocityControlPanel.Visible = False
        btnPlayStop.Visible = False
        tbDisplacementScale.Visible = False
        cbModes.Visible = False
        lblResultScale.Visible = False
        lblResultType.Visible = False
        btnPlayStop.Visible = False
        tsbFieldEvaluation.Visible = False
        tss10.Visible = False
        sepResults.Visible = False

        ' Stop the transit (if it was on)

        StopTransit()

        ' Hide show user controls:

        Me.VelocityControlPanel.Visible = False
        Me.WingControlPanel.Visible = False
        ' Represent:

        Me.Project.RepresentOnGL()

    End Sub

    ''' <summary>
    ''' Changes project mode and window state to post process mode.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SwitchToPostprocessMode()

        Try

            If Project.Results.Loaded Then

                SetUpLiftingSurfaceEditor()
                SetUpVelocityPlaneFrame()

                Project.PostprocessMode()
                btnPlayStop.Visible = True
                btnFieldEvaluation.Enabled = True
                btnFieldEvaluation.Visible = True
                WingControlPanel.Visible = False
                btTranslate.Visible = False
                btnAlign.Visible = False
                btnDesign.Checked = False
                btnPostprocess.Checked = True
                btnPlayStop.Enabled = Project.Results.TransitLoaded
                tbDisplacementScale.Enabled = Project.Results.TransitLoaded
                btnPlayStop.Visible = Project.Results.TransitLoaded
                tsbFieldEvaluation.Visible = True
                tss10.Visible = True

                cbModes.Items.Clear()

                If Not IsNothing(Project.Results.DynamicModes) Then

                    If Project.Results.DynamicModes.Count > 0 Then

                        Me.tbDisplacementScale.Visible = True

                        cbModes.Visible = True
                        cbModes.Items.Add("Results")
                        lblResultType.Visible = True
                        lblResultScale.Visible = True
                        tbDisplacementScale.Visible = True
                        sepResults.Visible = True

                        For i = 0 To Project.Results.DynamicModes.Count - 1
                            cbModes.Items.Add(Project.Results.DynamicModes(i).Name)
                        Next

                        cbModes.SelectedIndex = 0

                    Else

                        cbModes.Visible = False
                        lblResultType.Visible = False
                        lblResultScale.Visible = False
                        tbDisplacementScale.Visible = False
                        sepResults.Visible = False

                    End If

                End If

            Else

                MsgBox("No results have been loaded.",
                        MsgBoxStyle.Information, "Results are not available")

            End If

        Catch ex As Exception

            MsgBox("Unable to switch to post-processing mode! Results data might be corrupted.", MsgBoxStyle.Critical, "Error")
            Me.Project.Results.Clear()
            Me.SwitchToDesignMode()

        End Try

    End Sub

#End Region

#Region " Show dialogs "

    Private Sub ShowVelocityPlane(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ShowVelocityPlaneFrame()
    End Sub

    Private Sub btnLocalVelocity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLocalVelocity.Click
        FormAskVelocity.ShowDialog()
    End Sub

    Private Sub btnSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettings.Click

        If Project.InterfaceMode = InterfaceModes.Design Then

            Dim FormSettings = New FormSettings()

            FormSettings.Settings = Project.SimulationSettings

            If (FormSettings.ShowDialog()) = Windows.Forms.DialogResult.OK Then
                FormSettings.GetSettings()
            End If

        Else
            Try

                Dim FormSettings = New FormSettings()
                FormSettings.Settings = Project.Results.SimulationSettings
                FormSettings.ShowDialog()

            Catch ex As Exception
                MsgBox("Unable to load the simulation parameters of the results", MsgBoxStyle.Exclamation)
            End Try

        End If

    End Sub

    Private Sub btnShowResults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowResults.Click

        If (Not Project.CalculationCore Is Nothing) Then
            Dim FormReport As New FormReport
            FormReport.Owner = Me
            FormReport.ReportResults(Project.CalculationCore)
            FormReport.ShowDialog()
        Else
            MsgBox("Results are not available. Load a results file or execute the calculation first.")
        End If

    End Sub

    Private Sub btnOpenObjectsManager_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenObjectsManager.Click

        FormObjects.ShowDialog()

    End Sub

#End Region

#Region " User controls "

    Private WingControlPanel As WingControl

    Private VelocityControlPanel As VelocityControl

    Private Sub SetUpLiftingSurfaceEditor()

        If IsNothing(WingControlPanel) Then
            Me.WingControlPanel = New WingControl
            AddHandler WingControlPanel.RefreshGL, AddressOf Project.RefreshOnGL
            AddHandler WingControlPanel.OnClose, AddressOf ContractLeftPanel
        End If

    End Sub

    Public Sub ShowLiftingSurfaceEditor()

        SetUpLiftingSurfaceEditor()
        WingControlPanel.InitializeControl(Me.Project.Model.CurrentLiftingSurface, Project.Model.PolarDataBase)
        WingControlPanel.Parent = scMain.Panel1
        scMain.Panel1Collapsed = False
        scMain.SplitterDistance = WingControlPanel.Width
        WingControlPanel.Dock = DockStyle.Top
        WingControlPanel.Show()

    End Sub

    Private Sub SetUpVelocityPlaneFrame()

        If IsNothing(VelocityControlPanel) Then
            VelocityControlPanel = New VelocityControl
            AddHandler VelocityControlPanel.RefreshGL, AddressOf Project.RefreshOnGL
            AddHandler VelocityControlPanel.OnClose, AddressOf ContractLeftPanel
        End If

    End Sub

    Public Sub ShowVelocityPlaneFrame()

        SetUpVelocityPlaneFrame()
        VelocityControlPanel.IniciarControl(Me.Project)
        VelocityControlPanel.Parent = scMain.Panel1
        scMain.Panel1Collapsed = False
        scMain.SplitterDistance = VelocityControlPanel.Width
        VelocityControlPanel.Dock = DockStyle.Top
        VelocityControlPanel.Show()

    End Sub

    Public Sub ShowFuselageEditor()

        If TypeOf (Project.Model.CurrentBody) Is Fuselage Then
            Dim Fuselage As Fuselage = Project.Model.CurrentBody
            Dim FuselageForm As New FormFuselageEditor(Fuselage, Project.Model.LiftingSurfaces)
            FuselageForm.ShowDialog()
            Fuselage.GenerateMesh()
        End If

    End Sub

    Public Sub ShowJetEngineDialog()

        Dim JetEngine As JetEngine = Project.Model.CurrentJetEngine
        Dim JetEngineForm As New FormJetEngine(JetEngine)
        AddHandler JetEngineForm.UpdateModel, AddressOf Project.RepresentOnGL
        JetEngineForm.ShowDialog()
        JetEngine.GenerateMesh()

    End Sub

    Private Sub ContractLeftPanel()

        scMain.Panel1Collapsed = True
        Project.RefreshOnGL()
        Refresh()

    End Sub

#End Region

#Region " Exit "

    Private Sub SalirToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

#End Region

#Region " Control GL "

    Private PosicionInicialDelMouse As New Drawing.Point
    Private PosicionInicialDelLaCamara As New EVector3
    Private OricentacionInicialDeLaCamara As New EulerAngles

    Private Sub ControlOpenGL_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseMove

        If Project.VisualizationParameters.Panning Then
            Project.VisualizationParameters.CameraPosition.X = PosicionInicialDelLaCamara.X + (e.Location.X - PosicionInicialDelMouse.X)
            Project.VisualizationParameters.CameraPosition.Y = PosicionInicialDelLaCamara.Y + (PosicionInicialDelMouse.Y - e.Location.Y)
            'Proyecto.RepresentOnGL()
        End If

        If Project.VisualizationParameters.Rotating Then
            Project.VisualizationParameters.CameraOrientation.Psi = OricentacionInicialDeLaCamara.Psi + 0.25 * (e.Location.X - PosicionInicialDelMouse.X)
            Project.VisualizationParameters.CameraOrientation.Fi = OricentacionInicialDeLaCamara.Fi + 0.25 * (e.Location.Y - PosicionInicialDelMouse.Y)
            'Proyecto.RepresentOnGL()
        End If

    End Sub

    Private Sub ControlOpenGL_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseDown

        If e.Button = MouseButtons.Middle Then
            Project.VisualizationParameters.Panning = True
            PosicionInicialDelMouse = e.Location
            PosicionInicialDelLaCamara.Assign(Project.VisualizationParameters.CameraPosition)
        End If

        If e.Button = MouseButtons.Right Then
            Project.VisualizationParameters.Rotating = True
            PosicionInicialDelMouse = e.Location
            OricentacionInicialDeLaCamara.Psi = Project.VisualizationParameters.CameraOrientation.Psi
            OricentacionInicialDeLaCamara.Fi = Project.VisualizationParameters.CameraOrientation.Fi
        End If

        If e.Button = MouseButtons.Left Then

            SelectAndProcessItems(e.X, e.Y)

        End If

        If e.Button = MouseButtons.Right Then
            Project.Model.OperationsTool.CancelOperation()
        End If

        Project.RefreshOnGL()

    End Sub

    Private Sub SelectAndProcessItems(ByVal X As Integer, ByVal Y As Integer)

        With Project.Model

            Select Case Project.InterfaceMode

                Case InterfaceModes.Design

                    .Selection.SelectionMode = SelectionModes.smNodePicking
                    Project.SelectOnGL(X, Y, ControlOpenGL.Width, ControlOpenGL.Height)

                    ' there is a priority list to show the item info: structural elements, nodes and lattice rings

                    Dim KeepSearching As Boolean = True

                    ' search for a structural element:

                    For Each sr As SelectionRecord In .Selection.SelectionList

                        If sr.EntityType = EntityTypes.etStructuralElement Then

                            If sr.ComponentType = ComponentTypes.etLiftingSurface Then

                                Project.Model.OperationsTool.SetDestinationObject(.CurrentLiftingSurface)

                                If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                    ShowLiftingSurfaceEditor()
                                End If

                            End If

                            If Not IsNothing(.CurrentLiftingSurface) Then

                                ttSelectedEntity.Show(String.Format("Structural element {0}", sr.EntityIndex), ControlOpenGL)
                                lblStatus.Text = String.Format("Structural element {0} at {1}; AE = {2,6:F2}; GJ = {3,6:F2}; EIx = {4,6:F2}; EIy = {4,6:F2}", sr.EntityIndex,
                                                                          .CurrentLiftingSurface.Name,
                                                                          .CurrentLiftingSurface.StructuralPartition(sr.EntityIndex).LocalSection.AE,
                                                                          .CurrentLiftingSurface.StructuralPartition(sr.EntityIndex).LocalSection.GJ,
                                                                          .CurrentLiftingSurface.StructuralPartition(sr.EntityIndex).LocalSection.EIy,
                                                                          .CurrentLiftingSurface.StructuralPartition(sr.EntityIndex).LocalSection.EIz)

                                'Proyecto.Modelo.OperationsTool.SetEntityToQueue(.CurrentLiftingSurface.ObtenerPuntoNodal(sr.EntityIndex))

                                KeepSearching = False

                                Exit For

                            End If

                        End If

                    Next

                    ' search for a lattice node:

                    If KeepSearching Then

                        For Each sr As SelectionRecord In .Selection.SelectionList

                            If sr.EntityType = EntityTypes.etNode Then

                                If sr.ComponentType = ComponentTypes.etLiftingSurface Then

                                    Project.Model.OperationsTool.SetDestinationObject(.CurrentLiftingSurface)

                                    If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                        ShowLiftingSurfaceEditor()
                                    End If

                                    If Not IsNothing(.CurrentLiftingSurface) Then

                                        ttSelectedEntity.Show(String.Format("Lattice node {0}", sr.EntityIndex), ControlOpenGL)
                                        lblStatus.Text = String.Format("{1}: Node {0} ({2:F2}, {3:F2}, {4:F2})", sr.EntityIndex,
                                                                                  .CurrentLiftingSurface.Name,
                                                                                  .CurrentLiftingSurface.GetNodalPoint(sr.EntityIndex).X,
                                                                                  .CurrentLiftingSurface.GetNodalPoint(sr.EntityIndex).Y,
                                                                                  .CurrentLiftingSurface.GetNodalPoint(sr.EntityIndex).Z)
                                        Project.Model.OperationsTool.SetEntityToQueue(.CurrentLiftingSurface.GetNodalPoint(sr.EntityIndex))

                                        KeepSearching = False

                                        Exit For

                                    End If

                                End If

                                If sr.ComponentType = ComponentTypes.etBody Then

                                    Project.Model.OperationsTool.SetDestinationObject(.CurrentBody)

                                    If Not IsNothing(.CurrentBody) Then

                                        If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                            ShowFuselageEditor()
                                        End If

                                        ttSelectedEntity.Show(String.Format("Lattice node {0}", sr.EntityIndex), ControlOpenGL)
                                        lblStatus.Text = String.Format("{1}: Node {0} ({2:F2}, {3:F2}, {4:F2})", sr.EntityIndex,
                                                                                  .CurrentBody.Name,
                                                                                  .CurrentBody.NodalPosition(sr.EntityIndex).X,
                                                                                  .CurrentBody.NodalPosition(sr.EntityIndex).Y,
                                                                                  .CurrentBody.NodalPosition(sr.EntityIndex).Z)
                                        Project.Model.OperationsTool.SetEntityToQueue(.CurrentBody.NodalPosition(sr.EntityIndex))

                                        KeepSearching = False
                                        Exit For

                                    End If

                                End If

                            End If
                        Next

                    End If

                    ' search for a lattice ring:

                    If KeepSearching Then

                        For Each sr As SelectionRecord In .Selection.SelectionList

                            If sr.EntityType = EntityTypes.etQuadPanel Then

                                If sr.EntityType = EntityTypes.etQuadPanel Then

                                    ' show associated surface info

                                    If sr.ComponentType = ComponentTypes.etLiftingSurface Then

                                        Project.Model.OperationsTool.SetDestinationObject(.CurrentLiftingSurface)

                                        If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                            ShowLiftingSurfaceEditor()
                                        End If

                                        If Not IsNothing(.CurrentLiftingSurface) Then

                                            ttSelectedEntity.Show(String.Format("Vortex ring {0}", sr.EntityIndex), ControlOpenGL)
                                            lblStatus.Text = String.Format("{1}: Vortex ring {0}", sr.EntityIndex,
                                                                                      .CurrentLiftingSurface.Name)

                                            KeepSearching = False
                                            Exit For

                                        End If

                                    End If

                                    If sr.ComponentType = ComponentTypes.etBody Then

                                        Project.Model.OperationsTool.SetDestinationObject(.CurrentBody)

                                        If Not IsNothing(.CurrentBody) Then

                                            If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                                ShowFuselageEditor()
                                            End If

                                            ttSelectedEntity.Show(String.Format("Vortex ring {0}", sr.EntityIndex), ControlOpenGL)
                                            lblStatus.Text = String.Format("{1}: Vortex ring {0}", sr.EntityIndex,
                                                                                      .CurrentBody.Name)

                                            KeepSearching = False
                                            Exit For

                                        End If

                                    End If

                                    If sr.ComponentType = ComponentTypes.etJetEngine Then

                                        Project.Model.OperationsTool.SetDestinationObject(.CurrentJetEngine)

                                        If Not IsNothing(.CurrentJetEngine) Then

                                            If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                                ShowJetEngineDialog()
                                            End If

                                            ttSelectedEntity.Show(String.Format("Vortex ring {0}", sr.EntityIndex), ControlOpenGL)
                                            lblStatus.Text = String.Format("{1}: Vortex ring {0}", sr.EntityIndex,
                                                                                      .CurrentJetEngine.Name)

                                            KeepSearching = False

                                            Exit For

                                        End If

                                    End If

                                End If

                            End If

                        Next

                    End If

                    ' It didnt find anything:

                    If KeepSearching Then

                        ContractLeftPanel()

                    End If

            End Select

        End With

    End Sub

    Private Sub ControlOpenGL_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseUp
        Project.VisualizationParameters.Panning = False
        Project.VisualizationParameters.Rotating = False
        Project.RepresentOnGL()
    End Sub

    Private Sub ControlOpenGL_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.M Then
            If Project.Model.Selection.MultipleSelection = False Then
                Project.Model.Selection.MultipleSelection = True
                ReportState("Selección múltiple activada")
            Else
                Project.Model.Selection.MultipleSelection = False
                ReportState("Selección múltiple desactivada")
            End If
        End If
    End Sub

    Private Sub ControlOpenGL_OpenGLDraw(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ControlOpenGL.OpenGLDraw

        If FormCargado Then Project.RepresentOnGL()

    End Sub

    Private Sub ControlOpenGL_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ControlOpenGL.MouseLeave

        ttSelectedEntity.Hide(ControlOpenGL)

    End Sub

    Private Sub ControlOpenGL_Resized(sender As Object, e As EventArgs) Handles ControlOpenGL.Resized
        If Project IsNot Nothing Then
            Project.ControlGLWidth = ControlOpenGL.Width
            Project.ControlGLHeight = ControlOpenGL.Height
        End If
    End Sub

#End Region

#Region " Representation "

    Private Sub LoadViewParameters(Optional ByVal Vista As String = "Free")

        Select Case Vista

            Case "XY"
                Project.VisualizationParameters.CameraOrientation.Psi = 0
                Project.VisualizationParameters.CameraOrientation.Fi = 0
                lblStatus.Text = "XY view"

            Case "ZY"
                Project.VisualizationParameters.CameraOrientation.Psi = 90
                Project.VisualizationParameters.CameraOrientation.Fi = -90
                lblStatus.Text = "ZY view"

            Case "ZX"
                Project.VisualizationParameters.CameraOrientation.Psi = 0
                Project.VisualizationParameters.CameraOrientation.Fi = -90
                lblStatus.Text = "ZX view"

            Case "Isometrica"
                Project.VisualizationParameters.CameraOrientation.Psi = 30
                Project.VisualizationParameters.CameraOrientation.Fi = -60
                lblStatus.Text = "Free view"

            Case "Center"
                Project.VisualizationParameters.CameraPosition.X = 0
                Project.VisualizationParameters.CameraPosition.Y = 0
                Project.VisualizationParameters.CameraPosition.Z = 0

        End Select

        Project.RepresentOnGL()

    End Sub

#End Region

#Region " Other event handlers "

    Private Sub btnViewTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewTop.Click
        LoadViewParameters("XY")
    End Sub

    Private Sub btnViewFront_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewFront.Click
        LoadViewParameters("ZY")
    End Sub

    Private Sub btnViewLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewLeft.Click
        LoadViewParameters("ZX")
    End Sub

    Private Sub btnView3D_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView3D.Click
        LoadViewParameters("Isometrica")
    End Sub

    Private Sub btnViewCenter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewCenter.Click
        LoadViewParameters("Center")
    End Sub

    Private Sub sbHorizontal_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles sbHorizontal.Scroll
        Project.VisualizationParameters.CameraOrientation.Psi = Me.sbHorizontal.Value
        Project.RepresentOnGL()
    End Sub

    Private Sub sbVertical_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles sbVertical.Scroll
        Project.VisualizationParameters.CameraOrientation.Fi = Me.sbVertical.Value
        Project.RepresentOnGL()
    End Sub

    Private Sub MainForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseWheel
        Project.VisualizationParameters.Proximity = Project.VisualizationParameters.Proximity + 0.05 * Project.SimulationSettings.CharacteristicLenght * e.Delta
        Project.RepresentOnGL()
    End Sub

    Private Sub btnViewOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnViewOptions.Click
        FormOptions.ShowDialog()
    End Sub

    Private Sub btnImportSurfaces_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImportSurfaces.Click

        Dim dialog As New OpenFileDialog
        Dim result As DialogResult = dialog.ShowDialog(Me)

        If result = DialogResult.OK Then
            Project.ImportSurfacesFromXML(dialog.FileName)
        End If

    End Sub

    Private Sub btTranslate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btTranslate.Click
        Project.Model.OperationsTool.Operation = Operations.Translate
    End Sub

    Private Sub btnAlign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlign.Click
        Project.Model.OperationsTool.Operation = Operations.Align
    End Sub

    Private Sub MainForm_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Escape Then
            Project.Model.OperationsTool.CancelOperation()
            MsgBox(Project.Model.OperationsTool.StatusFlag)
        End If
    End Sub

    Private Sub tsbHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbHelp.Click
        AboutDialog.ShowDialog()
    End Sub

    Private Sub cbModels_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbModes.SelectedIndexChanged

        If cbModes.SelectedIndex > -1 And cbModes.SelectedIndex < Project.Results.DynamicModes.Count + 1 Then
            If (cbModes.SelectedIndex = 0) Then
                Project.Results.VisualizeModes = False
            Else
                Project.Results.VisualizeModes = True
                Project.Results.SelectedModeIndex = cbModes.SelectedIndex - 1
                Dim Scale As Double = Convert.ToDouble(tbDisplacementScale.Text)
                Project.Results.SelectedMode.UpdateDisplacement(Scale)
            End If
            Project.RefreshOnGL()
        End If

    End Sub

    Private Sub tbDisplacementScale_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbDisplacementScale.TextChanged

        Try
            Dim Scale As Double = Convert.ToDouble(tbDisplacementScale.Text)
            If Not IsNothing(Project.Results.SelectedMode) Then
                Project.Results.SelectedMode.UpdateDisplacement(Scale)
                Project.RefreshOnGL()
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnUnsteadyHistogram_Click(sender As Object, e As EventArgs) Handles btnUnsteadyHistogram.Click

        Select Case Project.InterfaceMode
            Case InterfaceModes.Design
                Dim Dialog As New FormUnsteadyVelocity
                Dialog.ShowProfile(Project.SimulationSettings, True)
            Case InterfaceModes.Postprocess
                Dim Dialog As New FormUnsteadyVelocity
                Dialog.ShowProfile(Project.Results.SimulationSettings, False)
        End Select

    End Sub

    Private Sub btnAeroelasticHistogram_Click(sender As Object, e As EventArgs) Handles btnAeroelasticHistogram.Click

        Select Case Project.InterfaceMode

            Case InterfaceModes.Design

                Dim Dialog As New FormHistogram(Project.SimulationSettings)

                Dialog.StartPosition = FormStartPosition.CenterParent

                Dialog.ShowDialog()

            Case InterfaceModes.Postprocess

        End Select

    End Sub

#End Region

#Region " Simulation "

    Private _Simulating As Boolean = False
    Private _CurrentFrame As Integer = 0
    Private _timer As New Timer

    Private Sub tsbPlayStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlayStop.Click

        If Project.Results.TransitLoaded Then

            If Not Project.Simulating Then
                _timer.Interval = Project.Results.SimulationSettings.Interval * 1000 / Project.Results.SimulationSettings.StructuralSettings.SubSteps
                Project.Simulating = True
                _timer.Start()
                btnPlayStop.Text = "Stop"
                ReportState(String.Format("Simulating. Rate {0}f/{1}s", Project.Results.SimulationSettings.StructuralSettings.SubSteps, Project.Results.SimulationSettings.Interval))
            Else
                Project.Simulating = False
                _timer.Stop()
                btnPlayStop.Text = "Play"
                ReportState("Simulation stopped")
            End If

        Else

            Project.Simulating = False

        End If

    End Sub

    Private Sub SimulationTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If _CurrentFrame > Project.Results.TransitStages Then _CurrentFrame = 0
        Project.RepresentResultsTransitWithOpenGL(_CurrentFrame)
        _CurrentFrame += 1
    End Sub

    Private Sub StopTransit()
        Project.Simulating = False
        _timer.Stop()
    End Sub

#End Region

End Class

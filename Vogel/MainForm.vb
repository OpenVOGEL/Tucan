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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.AeroTools
Imports OpenVOGEL.AeroTools.VisualModel.Interface
Imports OpenVOGEL.AeroTools.VisualModel.Models.Components
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.AeroTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.AeroTools.DataStore

Public Class MainForm

#Region " Initialize program "

    Public FormCargado As Boolean = False
    Public PostProcesoCargado As Boolean = False

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Initialize project root module

        ProjectRoot.Initialize()

        Text = Application.ProductName & "/" & Application.ProductVersion

        ' Initialize OpenGL control:

        ControlOpenGL.Dock = DockStyle.Fill

        ProjectRoot.SetControlGL(ControlOpenGL.OpenGL)

        ' Initialize the camber line database:

        CamberLinesDatabase.Initialize()

        ' Initialize declaration of global magitudes:

        GlobalMagnitudes.Initialize()

        ' Force design mode:

        SwitchToDesignMode()

        FormCargado = True

        AddHandler mrRibbon.PushMessage, AddressOf PushMessage
        AddHandler mrRibbon.EditSurface, AddressOf ShowEditor
        AddHandler mrRibbon.EditVelocityPlane, AddressOf ShowVelocityPlaneFrame
        AddHandler mrRibbon.SwitchToDesignMode, AddressOf SwitchToDesignMode
        AddHandler mrRibbon.SwitchToResultsMode, AddressOf SwitchToPostprocessMode

        ' Read command line arguments:

        Try

            Dim arguments As String() = Environment.GetCommandLineArgs

            If arguments.Length > 1 Then


                For Each argument In arguments

                    MsgBox(String.Format("Argument found: {0}", argument))

                    If IO.File.Exists(argument) And IO.Path.GetExtension(argument) = ".vog" Then

                        MsgBox(String.Format("Trying to open file {0}", argument))
                        'Me.OpenProject(argument)
                        Exit For

                    End If

                Next

            End If

        Finally

        End Try

    End Sub

    Public Sub PushMessage(ByVal Mensaje As String)

        lblStatus.Text = Mensaje

    End Sub

#End Region

#Region " Start calculation "

    Public Sub Calculate(Optional ByVal CalculationType As CalculationType = CalculationType.ctSteady)

        Dim FormSettings = New FormSettings()

        FormSettings.Settings = ProjectRoot.SimulationSettings
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

            AddHandler ProjectRoot.CalculationDone, AddressOf PostCalculationActions

            ProjectRoot.StartCalculation(CalculationType, Me)

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

#End Region

#Region " Switching modes "

    ''' <summary>
    ''' Changes project mode and window state to design mode.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SwitchToDesignMode()

        SetUpLiftingSurfaceEditor()

        If VelocityControlPanel IsNot Nothing Then
            VelocityControlPanel.Visible = False
        End If

        WingControlPanel.Visible = False

        ContractLeftPanel()

        ' Represent:

        ProjectRoot.RepresentOnGL()

    End Sub

    ''' <summary>
    ''' Changes project mode and window state to post process mode.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SwitchToPostprocessMode()

        Try

            SetUpVelocityPlaneFrame()

            If WingControlPanel IsNot Nothing Then
                WingControlPanel.Visible = False
            End If

            VelocityControlPanel.Visible = False

            ContractLeftPanel()

        Catch ex As Exception

            MsgBox("Unable to switch to post-processing mode! Results data might be corrupted.", MsgBoxStyle.Critical, "Error")
            ProjectRoot.Results.Clear()
            SwitchToDesignMode()

        End Try

    End Sub

#End Region

#Region " User controls "

    Private WingControlPanel As WingControl

    Private VelocityControlPanel As VelocityControl

    Private Sub SetUpLiftingSurfaceEditor()

        If IsNothing(WingControlPanel) Then
            WingControlPanel = New WingControl
            AddHandler WingControlPanel.RefreshGL, AddressOf ProjectRoot.RefreshOnGL
            AddHandler WingControlPanel.OnClose, AddressOf ContractLeftPanel
        End If

    End Sub

    Private Sub SetUpVelocityPlaneFrame()

        If IsNothing(VelocityControlPanel) Then
            VelocityControlPanel = New VelocityControl
            AddHandler VelocityControlPanel.RefreshGL, AddressOf ProjectRoot.RefreshOnGL
            AddHandler VelocityControlPanel.OnClose, AddressOf ContractLeftPanel
        End If

    End Sub

    Public Sub ShowVelocityPlaneFrame()

        SetUpVelocityPlaneFrame()
        VelocityControlPanel.Initialize()
        VelocityControlPanel.Parent = scMain.Panel1
        scMain.Panel1Collapsed = False
        scMain.SplitterDistance = VelocityControlPanel.Width
        VelocityControlPanel.Dock = DockStyle.Top
        VelocityControlPanel.Show()

    End Sub

    Public Sub ShowLiftingSurfaceEditor(Surface As LiftingSurface)

        SetUpLiftingSurfaceEditor()
        WingControlPanel.InitializeControl(Surface, ProjectRoot.Model.PolarDataBase)
        WingControlPanel.Parent = scMain.Panel1
        scMain.Panel1Collapsed = False
        scMain.SplitterDistance = WingControlPanel.Width
        WingControlPanel.Dock = DockStyle.Top
        WingControlPanel.Show()

    End Sub

    Public Sub ShowFuselageEditor(Fuselage As Fuselage)

        Dim Wings As New List(Of LiftingSurface)

        For Each otherSurface In ProjectRoot.Model.Objects
            If TypeOf otherSurface Is LiftingSurface Then
                Wings.Add(otherSurface)
            End If
        Next

        Dim FuselageForm As New FormFuselageEditor(Fuselage, Wings)
        FuselageForm.ShowDialog()
        Fuselage.GenerateMesh()

    End Sub

    Public Sub ShowJetEngineEditor(JetEngine As JetEngine)

        Dim JetEngineForm As New FormJetEngine(JetEngine)
        AddHandler JetEngineForm.UpdateModel, AddressOf ProjectRoot.RepresentOnGL
        JetEngineForm.ShowDialog()
        JetEngine.GenerateMesh()

    End Sub

    Public Sub ShowEditor(ByRef Surface As Surface)

        If Surface IsNot Nothing Then

            If TypeOf Surface Is LiftingSurface Then

                ShowLiftingSurfaceEditor(Surface)

            ElseIf TypeOf Surface Is Fuselage

                ShowFuselageEditor(Surface)

            ElseIf TypeOf Surface Is JetEngine

                ShowJetEngineEditor(Surface)

            End If

        End If

    End Sub

    Private Sub ContractLeftPanel()

        scMain.Panel1Collapsed = True
        ProjectRoot.RefreshOnGL()
        Refresh()

    End Sub

#End Region

#Region " Control GL "

    Private MouseDownPosition As New Drawing.Point
    Private CameraStartPosition As New EVector3
    Private CameraOrientation As New EulerAngles

    Private Sub ControlOpenGL_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseMove

        If ProjectRoot.Visualization.Panning Then
            ProjectRoot.Visualization.CameraPosition.X = CameraStartPosition.X + (e.Location.X - MouseDownPosition.X)
            ProjectRoot.Visualization.CameraPosition.Y = CameraStartPosition.Y + (MouseDownPosition.Y - e.Location.Y)
            'Proyecto.RepresentOnGL()
        End If

        If ProjectRoot.Visualization.Rotating Then
            ProjectRoot.Visualization.CameraOrientation.Psi = CameraOrientation.Psi + 0.25 * (e.Location.X - MouseDownPosition.X)
            ProjectRoot.Visualization.CameraOrientation.Fi = CameraOrientation.Fi + 0.25 * (e.Location.Y - MouseDownPosition.Y)
            'Proyecto.RepresentOnGL()
        End If

    End Sub

    Private Sub ControlOpenGL_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseDown

        If e.Button = MouseButtons.Middle Then
            ProjectRoot.Visualization.Panning = True
            MouseDownPosition = e.Location
            CameraStartPosition.Assign(ProjectRoot.Visualization.CameraPosition)
        End If

        If e.Button = MouseButtons.Right Then
            ProjectRoot.Visualization.Rotating = True
            MouseDownPosition = e.Location
            CameraOrientation.Psi = ProjectRoot.Visualization.CameraOrientation.Psi
            CameraOrientation.Fi = ProjectRoot.Visualization.CameraOrientation.Fi
        End If

        If e.Button = MouseButtons.Left Then

            SelectAndProcessItems(e.X, e.Y)

        End If

        If e.Button = MouseButtons.Right Then
            ProjectRoot.Model.OperationsTool.CancelOperation()
        End If

        ProjectRoot.RefreshOnGL()

    End Sub

    Private Sub SelectAndProcessItems(ByVal X As Integer, ByVal Y As Integer)

        With ProjectRoot.Model

            Select Case ProjectRoot.InterfaceMode

                Case InterfaceModes.Design

                    .Selection.SelectionMode = SelectionModes.smNodePicking
                    ProjectRoot.SelectOnGL(X, Y, ControlOpenGL.Width, ControlOpenGL.Height)

                    ' there is a priority list to show the item info: structural elements, nodes and lattice rings

                    Dim KeepSearching As Boolean = True

                    ' search for a structural element:

                    For Each sr As SelectionRecord In .Selection.SelectionList

                        If sr.EntityType = EntityTypes.etStructuralElement Then

                            If sr.ComponentType = ComponentTypes.etLiftingSurface Then

                                Dim wing As LiftingSurface = .Objects(sr.ComponentIndex)

                                ProjectRoot.Model.OperationsTool.SetDestinationObject(wing)

                                mrRibbon.ChangeSurfaceIndex(sr.ComponentIndex)

                                'If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                '    ShowLiftingSurfaceEditor(wing)
                                'End If

                                ttSelectedEntity.Show(String.Format("Structural element {0}", sr.EntityIndex), ControlOpenGL)
                                lblStatus.Text = String.Format("Structural element {0} at {1}; AE = {2,6:F2}; GJ = {3,6:F2}; EIx = {4,6:F2}; EIy = {4,6:F2}", sr.EntityIndex,
                                                                              wing.Name,
                                                                              wing.StructuralPartition(sr.EntityIndex).LocalSection.AE,
                                                                              wing.StructuralPartition(sr.EntityIndex).LocalSection.GJ,
                                                                              wing.StructuralPartition(sr.EntityIndex).LocalSection.EIy,
                                                                              wing.StructuralPartition(sr.EntityIndex).LocalSection.EIz)

                            End If

                            KeepSearching = False

                            Exit For

                        End If

                    Next

                    ' search for a lattice node:

                    If KeepSearching Then

                        For Each sr As SelectionRecord In .Selection.SelectionList

                            If sr.EntityType = EntityTypes.etNode Then

                                If sr.ComponentType = ComponentTypes.etLiftingSurface Then

                                    Dim wing As LiftingSurface = .Objects(sr.ComponentIndex)

                                    ProjectRoot.Model.OperationsTool.SetDestinationObject(wing)

                                    mrRibbon.ChangeSurfaceIndex(sr.ComponentIndex)

                                    'If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                    '    ShowLiftingSurfaceEditor(wing)
                                    'End If

                                    ttSelectedEntity.Show(String.Format("Lattice node {0}", sr.EntityIndex), ControlOpenGL)
                                    lblStatus.Text = String.Format("{1}: Node {0} ({2:F2}, {3:F2}, {4:F2})", sr.EntityIndex,
                                                                                  wing.Name,
                                                                                  wing.Mesh.Nodes(sr.EntityIndex).Position.X,
                                                                                  wing.Mesh.Nodes(sr.EntityIndex).Position.Y,
                                                                                  wing.Mesh.Nodes(sr.EntityIndex).Position.Z)

                                    ProjectRoot.Model.OperationsTool.SetEntityToQueue(wing.Mesh.Nodes(sr.EntityIndex))

                                    KeepSearching = False

                                    Exit For

                                End If

                                If sr.ComponentType = ComponentTypes.etFuselage Then

                                    Dim body As Fuselage = .Objects(sr.ComponentIndex)

                                    ProjectRoot.Model.OperationsTool.SetDestinationObject(body)

                                    mrRibbon.ChangeSurfaceIndex(sr.ComponentIndex)

                                    'If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                    '    ShowFuselageEditor(body)
                                    'End If

                                    ttSelectedEntity.Show(String.Format("Lattice node {0}", sr.EntityIndex), ControlOpenGL)
                                    lblStatus.Text = String.Format("{1}: Node {0} ({2:F2}, {3:F2}, {4:F2})", sr.EntityIndex,
                                                                                  body.Name,
                                                                                  body.Mesh.Nodes(sr.EntityIndex).Position.X,
                                                                                  body.Mesh.Nodes(sr.EntityIndex).Position.Y,
                                                                                  body.Mesh.Nodes(sr.EntityIndex).Position.Z)
                                    ProjectRoot.Model.OperationsTool.SetEntityToQueue(body.Mesh.Nodes(sr.EntityIndex).Position)

                                    KeepSearching = False

                                    Exit For

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

                                        Dim wing As LiftingSurface = .Objects(sr.ComponentIndex)

                                        ProjectRoot.Model.OperationsTool.SetDestinationObject(wing)

                                        mrRibbon.ChangeSurfaceIndex(sr.ComponentIndex)

                                        'If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                        '    ShowLiftingSurfaceEditor(wing)
                                        'End If

                                        ttSelectedEntity.Show(String.Format("Vortex ring {0}", sr.EntityIndex), ControlOpenGL)
                                        lblStatus.Text = String.Format("{1}: Vortex ring {0}", sr.EntityIndex,
                                                                                      wing.Name)

                                        KeepSearching = False

                                        Exit For

                                    End If

                                    If sr.ComponentType = ComponentTypes.etFuselage Then

                                        Dim body As Fuselage = .Objects(sr.ComponentIndex)

                                        ProjectRoot.Model.OperationsTool.SetDestinationObject(body)

                                        mrRibbon.ChangeSurfaceIndex(sr.ComponentIndex)

                                        'If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                        '    ShowFuselageEditor(body)
                                        'End If

                                        ttSelectedEntity.Show(String.Format("Vortex ring {0}", sr.EntityIndex), ControlOpenGL)
                                        lblStatus.Text = String.Format("{1}: Vortex ring {0}", sr.EntityIndex,
                                                                                      body.Name)

                                        KeepSearching = False

                                        Exit For

                                    End If

                                    If sr.ComponentType = ComponentTypes.etJetEngine Then

                                        Dim nacelle As JetEngine = .Objects(sr.ComponentIndex)

                                        ProjectRoot.Model.OperationsTool.SetDestinationObject(nacelle)

                                        mrRibbon.ChangeSurfaceIndex(sr.ComponentIndex)

                                        'If Project.Model.OperationsTool.Operation = Operations.NoOperation Then
                                        '    ShowJetEngineEditor(nacelle)
                                        'End If

                                        ttSelectedEntity.Show(String.Format("Vortex ring {0}", sr.EntityIndex), ControlOpenGL)
                                        lblStatus.Text = String.Format("{1}: Vortex ring {0}", sr.EntityIndex,
                                                                                     nacelle.Name)

                                        KeepSearching = False

                                        Exit For

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
        ProjectRoot.Visualization.Panning = False
        ProjectRoot.Visualization.Rotating = False
        ProjectRoot.RepresentOnGL()
    End Sub

    Private Sub ControlOpenGL_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.M Then
            If ProjectRoot.Model.Selection.MultipleSelection = False Then
                ProjectRoot.Model.Selection.MultipleSelection = True
                PushMessage("Selección múltiple activada")
            Else
                ProjectRoot.Model.Selection.MultipleSelection = False
                PushMessage("Selección múltiple desactivada")
            End If
        End If
    End Sub

    Private Sub ControlOpenGL_OpenGLDraw(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ControlOpenGL.OpenGLDraw

        If FormCargado Then ProjectRoot.RepresentOnGL()

    End Sub

    Private Sub ControlOpenGL_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ControlOpenGL.MouseLeave

        ttSelectedEntity.Hide(ControlOpenGL)

    End Sub

    Private Sub ControlOpenGL_Resized(sender As Object, e As EventArgs) Handles ControlOpenGL.Resized

        If ProjectRoot.Initialized Then
            ProjectRoot.ControlGLWidth = ControlOpenGL.Width
            ProjectRoot.ControlGLHeight = ControlOpenGL.Height
        End If

    End Sub

#End Region

#Region " Representation "

    Private Sub LoadViewParameters(Optional ByVal Vista As String = "Free")

        Select Case Vista

            Case "XY"
                ProjectRoot.Visualization.CameraOrientation.Psi = 0
                ProjectRoot.Visualization.CameraOrientation.Fi = 0
                lblStatus.Text = "XY view"

            Case "ZY"
                ProjectRoot.Visualization.CameraOrientation.Psi = 90
                ProjectRoot.Visualization.CameraOrientation.Fi = -90
                lblStatus.Text = "ZY view"

            Case "ZX"
                ProjectRoot.Visualization.CameraOrientation.Psi = 0
                ProjectRoot.Visualization.CameraOrientation.Fi = -90
                lblStatus.Text = "ZX view"

            Case "Isometrica"
                ProjectRoot.Visualization.CameraOrientation.Psi = 30
                ProjectRoot.Visualization.CameraOrientation.Fi = -60
                lblStatus.Text = "Free view"

            Case "Center"
                ProjectRoot.Visualization.CameraPosition.X = 0
                ProjectRoot.Visualization.CameraPosition.Y = 0
                ProjectRoot.Visualization.CameraPosition.Z = 0

        End Select

        ProjectRoot.RepresentOnGL()

    End Sub

#End Region

#Region " Other event handlers "

    Private Sub sbHorizontal_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles sbHorizontal.Scroll
        ProjectRoot.Visualization.CameraOrientation.Psi = Me.sbHorizontal.Value
        ProjectRoot.RepresentOnGL()
    End Sub

    Private Sub sbVertical_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles sbVertical.Scroll
        ProjectRoot.Visualization.CameraOrientation.Fi = Me.sbVertical.Value
        ProjectRoot.RepresentOnGL()
    End Sub

    Private Sub MainForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseWheel
        ProjectRoot.Visualization.Proximity = ProjectRoot.Visualization.Proximity + 0.05 * ProjectRoot.SimulationSettings.CharacteristicLenght * e.Delta
        ProjectRoot.RepresentOnGL()
    End Sub

    Private Sub MainForm_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Escape Then
            ProjectRoot.Model.OperationsTool.CancelOperation()
            MsgBox(ProjectRoot.Model.OperationsTool.StatusFlag)
        End If
    End Sub

    Private Sub btnUnsteadyHistogram_Click(sender As Object, e As EventArgs)

        Select Case ProjectRoot.InterfaceMode
            Case InterfaceModes.Design
                Dim Dialog As New FormUnsteadyVelocity
                Dialog.ShowProfile(ProjectRoot.SimulationSettings, True)
            Case InterfaceModes.Postprocess
                Dim Dialog As New FormUnsteadyVelocity
                Dialog.ShowProfile(ProjectRoot.Results.SimulationSettings, False)
        End Select

    End Sub

    Private Sub btnAeroelasticHistogram_Click(sender As Object, e As EventArgs)

        Select Case ProjectRoot.InterfaceMode

            Case InterfaceModes.Design

                Dim Dialog As New FormHistogram(ProjectRoot.SimulationSettings)

                Dialog.StartPosition = FormStartPosition.CenterParent

                Dialog.ShowDialog()

            Case InterfaceModes.Postprocess

        End Select

    End Sub

    Private Sub lblWebSite_Click(sender As Object, e As EventArgs) Handles lblWebSite.Click

        Process.Start("https://sites.google.com/site/gahvogel/main")

    End Sub

#End Region

End Class

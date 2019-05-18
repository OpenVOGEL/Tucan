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
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.Tucan.Utility

Public Class MainForm

#Region " Initialize program "

    Public FormLoaded As Boolean = False
    Public PostProcesoCargado As Boolean = False

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Initialize project root module

        ModelInterface.Initialize()

        Text = Application.ProductName

        ' Initialize OpenGL control:

        ControlOpenGL.Dock = DockStyle.Fill

        ModelInterface.SetControlGL(ControlOpenGL.OpenGL)

        ' Force design mode:

        SwitchToDesignMode()

        FormLoaded = True

        AddHandler mrRibbon.PushMessage, AddressOf PushMessage
        AddHandler mrRibbon.EditSurface, AddressOf ShowEditor
        AddHandler mrRibbon.EditVelocityPlane, AddressOf ShowVelocityPlaneFrame
        AddHandler mrRibbon.SwitchToDesignMode, AddressOf SwitchToDesignMode
        AddHandler mrRibbon.SwitchToResultsMode, AddressOf SwitchToPostprocessMode
        AddHandler mrRibbon.ProjectCleared, AddressOf CloseEditors
        AddHandler ModelInterface.InputOutputDone, AddressOf ChangeTitle

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

    Private Sub ChangeTitle(Text As String)

        Me.Text = Application.ProductName & " - " & Text

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

        ModelInterface.RepresentOnGL()

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

        If WingControlPanel Is Nothing Then
            WingControlPanel = New WingControl
            AddHandler WingControlPanel.RefreshGL, AddressOf ModelInterface.RefreshOnGL
            AddHandler WingControlPanel.OnClose, AddressOf ContractLeftPanel
        End If

    End Sub

    Private Sub SetUpVelocityPlaneFrame()

        If VelocityControlPanel Is Nothing Then
            VelocityControlPanel = New VelocityControl
            AddHandler VelocityControlPanel.RefreshGL, AddressOf ModelInterface.RefreshOnGL
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
        AddHandler JetEngineForm.UpdateModel, AddressOf RefreshOnGL
        JetEngineForm.ShowDialog()
        JetEngine.GenerateMesh()

    End Sub

    Public Sub ShowSurfaceLoader(Surface As ImportedSurface)

        Dim Dialog As New OpenFileDialog()
        If Dialog.ShowDialog = DialogResult.OK Then
            Surface.Load(Dialog.FileName)
        End If

    End Sub

    Public Sub ShowEditor(ByRef Surface As Surface)

        If Surface IsNot Nothing Then

            If TypeOf Surface Is LiftingSurface Then

                ShowLiftingSurfaceEditor(Surface)

            ElseIf TypeOf Surface Is Fuselage

                ShowFuselageEditor(Surface)

            ElseIf TypeOf Surface Is JetEngine

                ShowJetEngineEditor(Surface)

            ElseIf TypeOf Surface Is ImportedSurface

                ShowSurfaceLoader(Surface)

            End If

        End If

    End Sub

    Public Sub CloseEditors()
        ContractLeftPanel()
    End Sub

    Private Sub ContractLeftPanel()

        scMain.Panel1Collapsed = True
        ModelInterface.RefreshOnGL()
        Refresh()

    End Sub

#End Region

#Region " Control GL "

    Private MouseDownPosition As New Drawing.Point
    Private CameraStartPosition As New Vector3
    Private CameraOrientation As New EulerAngles

    Private Sub ControlOpenGL_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseMove

        If ModelInterface.Visualization.Panning Then
            ModelInterface.Visualization.CameraPosition.X = CameraStartPosition.X + (e.Location.X - MouseDownPosition.X)
            ModelInterface.Visualization.CameraPosition.Y = CameraStartPosition.Y + (MouseDownPosition.Y - e.Location.Y)
        End If

        If ModelInterface.Visualization.Rotating Then
            ModelInterface.Visualization.CameraOrientation.Psi = CameraOrientation.Psi + 0.25 * (e.Location.X - MouseDownPosition.X)
            ModelInterface.Visualization.CameraOrientation.Fi = CameraOrientation.Fi + 0.25 * (e.Location.Y - MouseDownPosition.Y)
        End If

    End Sub

    Private Sub ControlOpenGL_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ControlOpenGL.MouseDown

        If e.Button = MouseButtons.Middle Then
            ModelInterface.Visualization.Panning = True
            MouseDownPosition = e.Location
            CameraStartPosition.Assign(ModelInterface.Visualization.CameraPosition)
        End If

        If e.Button = MouseButtons.Right Then
            ModelInterface.Visualization.Rotating = True
            MouseDownPosition = e.Location
            CameraOrientation.Psi = ModelInterface.Visualization.CameraOrientation.Psi
            CameraOrientation.Fi = ModelInterface.Visualization.CameraOrientation.Fi
        End If

        If e.Button = MouseButtons.Left Then

            SelectAndProcessItems(e.X, e.Y)

        End If

        If e.Button = MouseButtons.Right Then
            ProjectRoot.Model.OperationsTool.CancelOperation()
        End If

        ModelInterface.RefreshOnGL()

    End Sub

    Private Sub SelectAndProcessItems(ByVal X As Integer, ByVal Y As Integer)

        ModelInterface.SelectOnGL(X, Y, ControlOpenGL.Width, ControlOpenGL.Height)

        Select Case ModelInterface.InterfaceMode

            Case Tucan.Utility.InterfaceModes.Design

                ' There is a priority list to show the item info: structural elements, nodes and lattice rings

                Dim KeepSearching As Boolean = True

                ' Search a structural element:

                For Each SelectedItem As SelectionRecord In ModelInterface.Selection.SelectionList

                    If SelectedItem.EntityType = EntityTypes.etStructuralElement Then

                        If SelectedItem.ComponentType = ComponentTypes.etLiftingSurface Then

                            Dim Wing As LiftingSurface = ProjectRoot.Model.Objects(SelectedItem.ComponentIndex)

                            ProjectRoot.Model.OperationsTool.SetDestinationObject(Wing)

                            mrRibbon.ChangeSurfaceIndex(SelectedItem.ComponentIndex)

                            lblStatus.Text = String.Format("Structural element {0} at {1}; AE = {2,6:F2}; GJ = {3,6:F2}; EIx = {4,6:F2}; EIy = {4,6:F2}", SelectedItem.EntityIndex,
                                                                          Wing.Name,
                                                                          Wing.StructuralPartition(SelectedItem.EntityIndex).LocalSection.AE,
                                                                          Wing.StructuralPartition(SelectedItem.EntityIndex).LocalSection.GJ,
                                                                          Wing.StructuralPartition(SelectedItem.EntityIndex).LocalSection.EIy,
                                                                          Wing.StructuralPartition(SelectedItem.EntityIndex).LocalSection.EIz)

                        End If

                        KeepSearching = False

                        Exit For

                    End If

                Next

                ' Search for a node:

                If KeepSearching Then

                    For Each SelectedItem As SelectionRecord In ModelInterface.Selection.SelectionList

                        If SelectedItem.EntityType = EntityTypes.etNode Then

                            Dim SelectedObject As Surface = ProjectRoot.Model.Objects(SelectedItem.ComponentIndex)

                            ProjectRoot.Model.OperationsTool.SetDestinationObject(SelectedObject)

                            mrRibbon.ChangeSurfaceIndex(SelectedItem.ComponentIndex)

                            lblStatus.Text = String.Format("{1}: Node {0} ({2:F2}, {3:F2}, {4:F2})", SelectedItem.EntityIndex,
                                                                              SelectedObject.Name,
                                                                              SelectedObject.Mesh.Nodes(SelectedItem.EntityIndex).Position.X,
                                                                              SelectedObject.Mesh.Nodes(SelectedItem.EntityIndex).Position.Y,
                                                                              SelectedObject.Mesh.Nodes(SelectedItem.EntityIndex).Position.Z)

                            ProjectRoot.Model.OperationsTool.SetEntityToQueue(SelectedObject.Mesh.Nodes(SelectedItem.EntityIndex))

                            KeepSearching = False

                        End If
                    Next

                End If

                ' Search for a panel:

                If KeepSearching Then

                    For Each SelectedItem As SelectionRecord In ModelInterface.Selection.SelectionList

                        If SelectedItem.EntityType = EntityTypes.etPanel Then

                            ' show associated surface info

                            ProjectRoot.Model.OperationsTool.SetDestinationObject(ProjectRoot.Model.Objects(SelectedItem.ComponentIndex))

                            mrRibbon.ChangeSurfaceIndex(SelectedItem.ComponentIndex)

                            lblStatus.Text = String.Format("{1}: Panel {0}", SelectedItem.EntityIndex,
                                                                              ProjectRoot.Model.Objects(SelectedItem.ComponentIndex).Name)

                            Exit For

                        End If

                    Next

                End If

                ' It didnt find anything:

                If KeepSearching Then

                    ContractLeftPanel()

                End If

            Case Tucan.Utility.InterfaceModes.Postprocess

                For Each SelectedItem As SelectionRecord In ModelInterface.Selection.SelectionList

                    If SelectedItem.ComponentType = ComponentTypes.etResultContainer Then

                        Select Case SelectedItem.EntityType

                            Case EntityTypes.etPanel

                                ' Show associated surface info

                                Dim Panel As Panel = Results.Model.Mesh.Panels(SelectedItem.EntityIndex)

                                If Panel.IsSlender Then

                                    lblStatus.Text = String.Format("Panel {0}: ΔCp={1:F5}; V={2:F5}m/s; G={3:F5}; A={4:F5}m²", SelectedItem.EntityIndex,
                                                               Panel.Cp,
                                                               Panel.LocalVelocity.EuclideanNorm,
                                                               Panel.Circulation,
                                                               Panel.Area)

                                Else

                                    lblStatus.Text = String.Format("Panel {0}: Cp={1:F5}; V={2:F5}m/s; G={3:F5}; S={3:F5}; A={4:F5}m²", SelectedItem.EntityIndex,
                                                               Panel.Cp,
                                                               Panel.LocalVelocity.EuclideanNorm,
                                                               Panel.Circulation,
                                                               Panel.SourceStrength,
                                                               Panel.Area)

                                End If

                                Exit For

                            Case EntityTypes.etNode

                                ' Show associated surface info

                                lblStatus.Text = String.Format("Node {0}: ", SelectedItem.EntityIndex,
                                                                              Results.Model.Mesh.Nodes(SelectedItem.EntityIndex).Position.ToString)

                                Exit For

                        End Select

                    End If

                Next

        End Select

    End Sub

    Private Sub ControlOpenGL_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseUp
        ModelInterface.Visualization.Panning = False
        ModelInterface.Visualization.Rotating = False
        ModelInterface.RepresentOnGL()
    End Sub

    Private Sub ControlOpenGL_OpenGLDraw(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ControlOpenGL.OpenGLDraw

        If FormLoaded Then ModelInterface.RepresentOnGL()

    End Sub

    Private Sub ControlOpenGL_MouseLeave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ControlOpenGL.MouseLeave

    End Sub

    Private Sub ControlOpenGL_Resized(sender As Object, e As EventArgs) Handles ControlOpenGL.Resized

        If ModelInterface.Initialized Then
            ModelInterface.ControlGLWidth = ControlOpenGL.Width
            ModelInterface.ControlGLHeight = ControlOpenGL.Height
        End If

    End Sub

#End Region

#Region " Representation "

    Private Sub LoadViewParameters(Optional ByVal Vista As String = "Free")

        Select Case Vista

            Case "XY"
                ModelInterface.Visualization.CameraOrientation.Psi = 0
                ModelInterface.Visualization.CameraOrientation.Fi = 0
                lblStatus.Text = "XY view"

            Case "ZY"
                ModelInterface.Visualization.CameraOrientation.Psi = 90
                ModelInterface.Visualization.CameraOrientation.Fi = -90
                lblStatus.Text = "ZY view"

            Case "ZX"
                ModelInterface.Visualization.CameraOrientation.Psi = 0
                ModelInterface.Visualization.CameraOrientation.Fi = -90
                lblStatus.Text = "ZX view"

            Case "Isometrica"
                ModelInterface.Visualization.CameraOrientation.Psi = 30
                ModelInterface.Visualization.CameraOrientation.Fi = -60
                lblStatus.Text = "Free view"

            Case "Center"
                ModelInterface.Visualization.CameraPosition.X = 0
                ModelInterface.Visualization.CameraPosition.Y = 0
                ModelInterface.Visualization.CameraPosition.Z = 0

        End Select

        ModelInterface.RepresentOnGL()

    End Sub

#End Region

#Region " Other event handlers "

    Private Sub sbHorizontal_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles sbHorizontal.Scroll
        ModelInterface.Visualization.CameraOrientation.Psi = Me.sbHorizontal.Value
        ModelInterface.RepresentOnGL()
    End Sub

    Private Sub sbVertical_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles sbVertical.Scroll
        ModelInterface.Visualization.CameraOrientation.Fi = Me.sbVertical.Value
        ModelInterface.RepresentOnGL()
    End Sub

    Private Sub MainForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseWheel
        ModelInterface.Visualization.Proximity = ModelInterface.Visualization.Proximity + 0.05 * e.Delta
        ModelInterface.RepresentOnGL()
    End Sub

    Private Sub MainForm_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Escape Then
            ProjectRoot.Model.OperationsTool.CancelOperation()
            MsgBox(ProjectRoot.Model.OperationsTool.StatusFlag)
        End If
    End Sub

    Private Sub btnUnsteadyHistogram_Click(sender As Object, e As EventArgs)

        Select Case ModelInterface.InterfaceMode
            Case Tucan.Utility.InterfaceModes.Design
                Dim Dialog As New FormUnsteadyVelocity
                Dialog.ShowProfile(ProjectRoot.SimulationSettings, True)
            Case Tucan.Utility.InterfaceModes.Postprocess
                Dim Dialog As New FormUnsteadyVelocity
                Dialog.ShowProfile(ProjectRoot.Results.SimulationSettings, False)
        End Select

    End Sub

    Private Sub btnAeroelasticHistogram_Click(sender As Object, e As EventArgs)

        Select Case ModelInterface.InterfaceMode

            Case Tucan.Utility.InterfaceModes.Design

                Dim Dialog As New FormHistogram

                Dialog.StartPosition = FormStartPosition.CenterParent

                Dialog.ShowDialog()

            Case Tucan.Utility.InterfaceModes.Postprocess

        End Select

    End Sub

    Private Sub lblWebSite_Click(sender As Object, e As EventArgs)

        Process.Start("https://sites.google.com/site/gahvogel/main")

    End Sub

#End Region

End Class

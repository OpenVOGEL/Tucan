﻿'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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
    Public ResultsLoaded As Boolean = False

    Private Sub Main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Initialize project root module
        '-------------------------------------------------------------

        ModelInterface.Initialize()

        Text = Application.ProductName

        ' Initialize OpenGL control:
        '-------------------------------------------------------------

        ControlOpenGL.Dock = DockStyle.Fill

        ModelInterface.SetControlGL(ControlOpenGL.OpenGL)

        ' Force design mode:
        '-------------------------------------------------------------

        SwitchToDesignMode()

        FormLoaded = True

        AddHandler mrRibbon.PushMessage, AddressOf PushMessage
        AddHandler mrRibbon.EditSurface, AddressOf ShowEditor
        AddHandler mrRibbon.SwitchToDesignMode, AddressOf SwitchToDesignMode
        AddHandler mrRibbon.SwitchToResultsMode, AddressOf SwitchToPostprocessMode
        AddHandler mrRibbon.ProjectCleared, AddressOf CloseEditors
        AddHandler ModelInterface.InputOutputDone, AddressOf ChangeTitle

        ' Read command line arguments:
        '-------------------------------------------------------------

        Try

            Dim Arguments As String() = Environment.GetCommandLineArgs

            If Arguments.Length > 1 Then

                For Each Argument In Arguments

                    If IO.File.Exists(Argument) Then

                        If IO.Path.GetExtension(Argument) = ".vog" Then

                            mrRibbon.OpenProject(Argument)

                            Exit For

                        ElseIf IO.Path.GetExtension(Argument) = ".vri" Then

                            mrRibbon.LoadResults(Argument)

                            Exit For

                        End If

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

            If WingControlPanel IsNot Nothing Then
                WingControlPanel.Visible = False
            End If

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

    Private Sub SetUpLiftingSurfaceEditor()

        If WingControlPanel Is Nothing Then
            WingControlPanel = New WingControl
            AddHandler WingControlPanel.RefreshModelView, AddressOf ModelInterface.RefreshOnGL
            AddHandler WingControlPanel.OnClose, AddressOf ContractLeftPanel
        End If

    End Sub

    Public Sub ShowLiftingSurfaceEditor(Surface As LiftingSurface)

        SetUpLiftingSurfaceEditor()
        WingControlPanel.InitializeControl(Surface)
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

    Public Sub ShowPropellerEditor(Propeller As Propeller)

        Dim PropellerForm As New FormPropeller(Propeller)
        AddHandler PropellerForm.UpdateModel, AddressOf RefreshOnGL
        PropellerForm.ShowDialog()
        Propeller.GenerateMesh()

    End Sub

    Public Sub ShowSurfaceLoader(Surface As ImportedSurface)

        Dim Dialog As New OpenFileDialog()
        Dialog.Filter = "Stereolithography (*.stl)|*.stl|Native (*.dat)|*.dat"

        If Dialog.ShowDialog = DialogResult.OK Then
            Select Case Dialog.FilterIndex
                Case 1
                    Surface.Load(Dialog.FileName, ImportedSurface.ImportFormats.StlFile)
                Case 2
                    Surface.Load(Dialog.FileName, ImportedSurface.ImportFormats.ConnectivityFile)
            End Select
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

            ElseIf TypeOf Surface Is Propeller

                ShowPropellerEditor(Surface)

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
    Private CameraOrientation As New OrientationAngles

    Private Sub ControlOpenGL_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ControlOpenGL.MouseMove

        If ModelInterface.Visualization.Panning Then
            ModelInterface.Visualization.CameraPosition.X = CameraStartPosition.X + (e.Location.X - MouseDownPosition.X)
            ModelInterface.Visualization.CameraPosition.Y = CameraStartPosition.Y + (MouseDownPosition.Y - e.Location.Y)
        End If

        If ModelInterface.Visualization.Rotating Then
            ModelInterface.Visualization.CameraOrientation.R1 = CameraOrientation.R1 + 0.25 * (e.Location.X - MouseDownPosition.X)
            ModelInterface.Visualization.CameraOrientation.R3 = CameraOrientation.R3 + 0.25 * (e.Location.Y - MouseDownPosition.Y)
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
            CameraOrientation.R1 = ModelInterface.Visualization.CameraOrientation.R1
            CameraOrientation.R3 = ModelInterface.Visualization.CameraOrientation.R3
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

                For Each SelectedItem As SelectionInfo In ModelInterface.Selection.SelectionList

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

                    For Each SelectedItem As SelectionInfo In ModelInterface.Selection.SelectionList

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

                    For Each SelectedItem As SelectionInfo In ModelInterface.Selection.SelectionList

                        If SelectedItem.EntityType = EntityTypes.etPanel Then

                            ' show associated surface info

                            ProjectRoot.Model.OperationsTool.SetDestinationObject(ProjectRoot.Model.Objects(SelectedItem.ComponentIndex))

                            mrRibbon.ChangeSurfaceIndex(SelectedItem.ComponentIndex)

                            lblStatus.Text = String.Format("{1}: Panel {0} -> Slender={2}; Primitive={3}",
                                                           SelectedItem.EntityIndex,
                                                           ProjectRoot.Model.Objects(SelectedItem.ComponentIndex).Name,
                                                           ProjectRoot.Model.Objects(SelectedItem.ComponentIndex).Mesh.Panels(SelectedItem.EntityIndex).IsSlender,
                                                           ProjectRoot.Model.Objects(SelectedItem.ComponentIndex).Mesh.Panels(SelectedItem.EntityIndex).IsPrimitive)

                            If SelectedItem.ComponentType = ComponentTypes.etLiftingSurface And WingControlPanel IsNot Nothing Then

                                WingControlPanel.InitializeControl(ProjectRoot.Model.Objects(SelectedItem.ComponentIndex))

                            Else

                                ContractLeftPanel()

                            End If

                            KeepSearching = False

                            Exit For

                        End If

                    Next

                End If

                ' It didnt find anything:

                If KeepSearching Then

                    ContractLeftPanel()

                End If

            Case Tucan.Utility.InterfaceModes.Postprocess

                If Results.ActiveFrame IsNot Nothing Then

                    For Each SelectedItem As SelectionInfo In ModelInterface.Selection.SelectionList

                        If SelectedItem.ComponentType = ComponentTypes.etResultContainer Then

                            Select Case SelectedItem.EntityType

                                Case EntityTypes.etPanel

                                    ' Show associated surface info

                                    Dim Panel As Panel = Results.ActiveFrame.Model.Mesh.Panels(SelectedItem.EntityIndex)

                                    If Panel.IsSlender Then

                                        lblStatus.Text = String.Format("Panel {0}: ΔCp={1:F5}; V={2:F5}m/s; G={3:F5}; A={4:F5}m²",
                                                                       SelectedItem.EntityIndex,
                                                                       Panel.Cp,
                                                                       Panel.LocalVelocity.EuclideanNorm,
                                                                       Panel.Circulation,
                                                                       Panel.Area)

                                    Else

                                        lblStatus.Text = String.Format("Panel {0}: Cp={1:F5}; V={2:F5}m/s; G={3:F5}; S={4:F5}; A={5:F5}m²",
                                                                       SelectedItem.EntityIndex,
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
                                                                                  Results.ActiveFrame.Model.Mesh.Nodes(SelectedItem.EntityIndex).Position.ToString)

                                    Exit For

                            End Select

                        End If

                    Next

                End If

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
                ModelInterface.Visualization.CameraOrientation.R1 = 0
                ModelInterface.Visualization.CameraOrientation.R3 = 0
                lblStatus.Text = "XY view"

            Case "ZY"
                ModelInterface.Visualization.CameraOrientation.R1 = 90
                ModelInterface.Visualization.CameraOrientation.R3 = -90
                lblStatus.Text = "ZY view"

            Case "ZX"
                ModelInterface.Visualization.CameraOrientation.R1 = 0
                ModelInterface.Visualization.CameraOrientation.R3 = -90
                lblStatus.Text = "ZX view"

            Case "Isometrica"
                ModelInterface.Visualization.CameraOrientation.R1 = 30
                ModelInterface.Visualization.CameraOrientation.R3 = -60
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
        ModelInterface.Visualization.CameraOrientation.R1 = Me.sbHorizontal.Value
        ModelInterface.RepresentOnGL()
    End Sub

    Private Sub sbVertical_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles sbVertical.Scroll
        ModelInterface.Visualization.CameraOrientation.R3 = Me.sbVertical.Value
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

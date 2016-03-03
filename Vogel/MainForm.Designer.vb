<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.Contenedor = New System.Windows.Forms.ToolStripContainer()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.ControlOpenGL = New SharpGL.OpenGLControl()
        Me.sbHorizontal = New System.Windows.Forms.HScrollBar()
        Me.sbVertical = New System.Windows.Forms.VScrollBar()
        Me.tsTools = New System.Windows.Forms.ToolStrip()
        Me.btnNewProject = New System.Windows.Forms.ToolStripButton()
        Me.btnOpenProject = New System.Windows.Forms.ToolStripButton()
        Me.btnSaveProject = New System.Windows.Forms.ToolStripButton()
        Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnOpenObjectsManager = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnAlign = New System.Windows.Forms.ToolStripButton()
        Me.btTranslate = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator12 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnViewTop = New System.Windows.Forms.ToolStripButton()
        Me.btnViewFront = New System.Windows.Forms.ToolStripButton()
        Me.btnViewLeft = New System.Windows.Forms.ToolStripButton()
        Me.btnView3D = New System.Windows.Forms.ToolStripButton()
        Me.btnViewCenter = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnDesign = New System.Windows.Forms.ToolStripButton()
        Me.btnPostprocess = New System.Windows.Forms.ToolStripButton()
        Me.tss10 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbFieldEvaluation = New System.Windows.Forms.ToolStripButton()
        Me.sepResults = New System.Windows.Forms.ToolStripSeparator()
        Me.lblResultType = New System.Windows.Forms.ToolStripLabel()
        Me.cbModes = New System.Windows.Forms.ToolStripComboBox()
        Me.lblResultScale = New System.Windows.Forms.ToolStripLabel()
        Me.tbDisplacementScale = New System.Windows.Forms.ToolStripTextBox()
        Me.btnPlayStop = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbHelp = New System.Windows.Forms.ToolStripButton()
        Me.ssStatus = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.dlgSaveFile = New System.Windows.Forms.SaveFileDialog()
        Me.dlgOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.msMenuBar = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnOpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnSaveAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator11 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnImportSurfaces = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.ModoPostprocesoBtn = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnViewOptions = New System.Windows.Forms.ToolStripMenuItem()
        Me.CalcularToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnSteadyAnalysis = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnStartSteady = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnUnsteadyAnalysis = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnStartUnsteady = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnUnsteadyHistogram = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnAeroelastic = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnStartAeroelastic = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnAeroelasticHistogram = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnFieldEvaluation = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmFieldEvaluation = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnLocalVelocity = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnSettings = New System.Windows.Forms.ToolStripMenuItem()
        Me.PostprocesoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnShowResults = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnLoadResults = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTipS = New System.Windows.Forms.ToolTip(Me.components)
        Me.niNotificationTool = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.miniToolStrip = New System.Windows.Forms.ToolStrip()
        Me.ttSelectedEntity = New System.Windows.Forms.ToolTip(Me.components)
        Me.Contenedor.ContentPanel.SuspendLayout()
        Me.Contenedor.TopToolStripPanel.SuspendLayout()
        Me.Contenedor.SuspendLayout()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.ControlOpenGL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsTools.SuspendLayout()
        Me.ssStatus.SuspendLayout()
        Me.msMenuBar.SuspendLayout()
        Me.SuspendLayout()
        '
        'Contenedor
        '
        Me.Contenedor.BottomToolStripPanelVisible = False
        '
        'Contenedor.ContentPanel
        '
        Me.Contenedor.ContentPanel.BackColor = System.Drawing.SystemColors.Control
        Me.Contenedor.ContentPanel.Controls.Add(Me.scMain)
        resources.ApplyResources(Me.Contenedor.ContentPanel, "Contenedor.ContentPanel")
        resources.ApplyResources(Me.Contenedor, "Contenedor")
        Me.Contenedor.LeftToolStripPanelVisible = False
        Me.Contenedor.Name = "Contenedor"
        Me.Contenedor.RightToolStripPanelVisible = False
        '
        'Contenedor.TopToolStripPanel
        '
        Me.Contenedor.TopToolStripPanel.Controls.Add(Me.tsTools)
        '
        'scMain
        '
        resources.ApplyResources(Me.scMain, "scMain")
        Me.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scMain.Name = "scMain"
        Me.scMain.Panel1Collapsed = True
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.Controls.Add(Me.ControlOpenGL)
        Me.scMain.Panel2.Controls.Add(Me.sbHorizontal)
        Me.scMain.Panel2.Controls.Add(Me.sbVertical)
        Me.scMain.TabStop = False
        '
        'ControlOpenGL
        '
        Me.ControlOpenGL.BitDepth = 24
        Me.ControlOpenGL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ControlOpenGL.DrawFPS = False
        Me.ControlOpenGL.FrameRate = 30
        resources.ApplyResources(Me.ControlOpenGL, "ControlOpenGL")
        Me.ControlOpenGL.Name = "ControlOpenGL"
        Me.ControlOpenGL.RenderContextType = SharpGL.RenderContextType.DIBSection
        '
        'sbHorizontal
        '
        resources.ApplyResources(Me.sbHorizontal, "sbHorizontal")
        Me.sbHorizontal.Maximum = 180
        Me.sbHorizontal.Minimum = -180
        Me.sbHorizontal.Name = "sbHorizontal"
        '
        'sbVertical
        '
        resources.ApplyResources(Me.sbVertical, "sbVertical")
        Me.sbVertical.Maximum = 90
        Me.sbVertical.Minimum = -90
        Me.sbVertical.Name = "sbVertical"
        '
        'tsTools
        '
        Me.tsTools.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Me.tsTools, "tsTools")
        Me.tsTools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnNewProject, Me.btnOpenProject, Me.btnSaveProject, Me.toolStripSeparator1, Me.btnOpenObjectsManager, Me.ToolStripSeparator7, Me.btnAlign, Me.btTranslate, Me.ToolStripSeparator12, Me.btnViewTop, Me.btnViewFront, Me.btnViewLeft, Me.btnView3D, Me.btnViewCenter, Me.ToolStripSeparator2, Me.btnDesign, Me.btnPostprocess, Me.tss10, Me.tsbFieldEvaluation, Me.sepResults, Me.lblResultType, Me.cbModes, Me.lblResultScale, Me.tbDisplacementScale, Me.btnPlayStop, Me.ToolStripSeparator4, Me.tsbHelp})
        Me.tsTools.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.tsTools.Name = "tsTools"
        '
        'btnNewProject
        '
        Me.btnNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnNewProject, "btnNewProject")
        Me.btnNewProject.ForeColor = System.Drawing.Color.DimGray
        Me.btnNewProject.Name = "btnNewProject"
        '
        'btnOpenProject
        '
        Me.btnOpenProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnOpenProject, "btnOpenProject")
        Me.btnOpenProject.ForeColor = System.Drawing.Color.DimGray
        Me.btnOpenProject.Name = "btnOpenProject"
        '
        'btnSaveProject
        '
        Me.btnSaveProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnSaveProject, "btnSaveProject")
        Me.btnSaveProject.ForeColor = System.Drawing.Color.DimGray
        Me.btnSaveProject.Name = "btnSaveProject"
        '
        'toolStripSeparator1
        '
        Me.toolStripSeparator1.ForeColor = System.Drawing.Color.DimGray
        Me.toolStripSeparator1.Name = "toolStripSeparator1"
        resources.ApplyResources(Me.toolStripSeparator1, "toolStripSeparator1")
        '
        'btnOpenObjectsManager
        '
        Me.btnOpenObjectsManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnOpenObjectsManager, "btnOpenObjectsManager")
        Me.btnOpenObjectsManager.ForeColor = System.Drawing.Color.DimGray
        Me.btnOpenObjectsManager.Name = "btnOpenObjectsManager"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.ForeColor = System.Drawing.Color.DimGray
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        resources.ApplyResources(Me.ToolStripSeparator7, "ToolStripSeparator7")
        '
        'btnAlign
        '
        Me.btnAlign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnAlign, "btnAlign")
        Me.btnAlign.ForeColor = System.Drawing.Color.DimGray
        Me.btnAlign.Name = "btnAlign"
        '
        'btTranslate
        '
        Me.btTranslate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btTranslate, "btTranslate")
        Me.btTranslate.ForeColor = System.Drawing.Color.DimGray
        Me.btTranslate.Name = "btTranslate"
        '
        'ToolStripSeparator12
        '
        Me.ToolStripSeparator12.ForeColor = System.Drawing.Color.DimGray
        Me.ToolStripSeparator12.Name = "ToolStripSeparator12"
        resources.ApplyResources(Me.ToolStripSeparator12, "ToolStripSeparator12")
        '
        'btnViewTop
        '
        Me.btnViewTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnViewTop.ForeColor = System.Drawing.Color.DimGray
        resources.ApplyResources(Me.btnViewTop, "btnViewTop")
        Me.btnViewTop.Name = "btnViewTop"
        '
        'btnViewFront
        '
        Me.btnViewFront.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnViewFront.ForeColor = System.Drawing.Color.DimGray
        resources.ApplyResources(Me.btnViewFront, "btnViewFront")
        Me.btnViewFront.Name = "btnViewFront"
        '
        'btnViewLeft
        '
        Me.btnViewLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnViewLeft.ForeColor = System.Drawing.Color.DimGray
        resources.ApplyResources(Me.btnViewLeft, "btnViewLeft")
        Me.btnViewLeft.Name = "btnViewLeft"
        '
        'btnView3D
        '
        Me.btnView3D.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnView3D.ForeColor = System.Drawing.Color.DimGray
        resources.ApplyResources(Me.btnView3D, "btnView3D")
        Me.btnView3D.Name = "btnView3D"
        '
        'btnViewCenter
        '
        Me.btnViewCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnViewCenter, "btnViewCenter")
        Me.btnViewCenter.ForeColor = System.Drawing.Color.DimGray
        Me.btnViewCenter.Name = "btnViewCenter"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.ForeColor = System.Drawing.Color.DimGray
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        resources.ApplyResources(Me.ToolStripSeparator2, "ToolStripSeparator2")
        '
        'btnDesign
        '
        Me.btnDesign.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnDesign, "btnDesign")
        Me.btnDesign.ForeColor = System.Drawing.Color.DimGray
        Me.btnDesign.Name = "btnDesign"
        '
        'btnPostprocess
        '
        Me.btnPostprocess.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnPostprocess, "btnPostprocess")
        Me.btnPostprocess.ForeColor = System.Drawing.Color.DimGray
        Me.btnPostprocess.Name = "btnPostprocess"
        '
        'tss10
        '
        Me.tss10.ForeColor = System.Drawing.Color.DimGray
        Me.tss10.Name = "tss10"
        resources.ApplyResources(Me.tss10, "tss10")
        '
        'tsbFieldEvaluation
        '
        Me.tsbFieldEvaluation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.tsbFieldEvaluation, "tsbFieldEvaluation")
        Me.tsbFieldEvaluation.ForeColor = System.Drawing.Color.DimGray
        Me.tsbFieldEvaluation.Name = "tsbFieldEvaluation"
        '
        'sepResults
        '
        Me.sepResults.Name = "sepResults"
        resources.ApplyResources(Me.sepResults, "sepResults")
        '
        'lblResultType
        '
        resources.ApplyResources(Me.lblResultType, "lblResultType")
        Me.lblResultType.ForeColor = System.Drawing.Color.DimGray
        Me.lblResultType.Name = "lblResultType"
        '
        'cbModes
        '
        Me.cbModes.BackColor = System.Drawing.Color.WhiteSmoke
        Me.cbModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cbModes, "cbModes")
        Me.cbModes.ForeColor = System.Drawing.Color.DimGray
        Me.cbModes.Name = "cbModes"
        '
        'lblResultScale
        '
        resources.ApplyResources(Me.lblResultScale, "lblResultScale")
        Me.lblResultScale.ForeColor = System.Drawing.Color.DimGray
        Me.lblResultScale.Name = "lblResultScale"
        '
        'tbDisplacementScale
        '
        resources.ApplyResources(Me.tbDisplacementScale, "tbDisplacementScale")
        Me.tbDisplacementScale.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbDisplacementScale.ForeColor = System.Drawing.Color.DimGray
        Me.tbDisplacementScale.Name = "tbDisplacementScale"
        '
        'btnPlayStop
        '
        Me.btnPlayStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        resources.ApplyResources(Me.btnPlayStop, "btnPlayStop")
        Me.btnPlayStop.ForeColor = System.Drawing.Color.DimGray
        Me.btnPlayStop.Name = "btnPlayStop"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        resources.ApplyResources(Me.ToolStripSeparator4, "ToolStripSeparator4")
        '
        'tsbHelp
        '
        Me.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        resources.ApplyResources(Me.tsbHelp, "tsbHelp")
        Me.tsbHelp.Name = "tsbHelp"
        '
        'ssStatus
        '
        Me.ssStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
        resources.ApplyResources(Me.ssStatus, "ssStatus")
        Me.ssStatus.Name = "ssStatus"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.ForeColor = System.Drawing.Color.Black
        Me.lblStatus.Name = "lblStatus"
        resources.ApplyResources(Me.lblStatus, "lblStatus")
        '
        'dlgSaveFile
        '
        resources.ApplyResources(Me.dlgSaveFile, "dlgSaveFile")
        '
        'dlgOpenFile
        '
        resources.ApplyResources(Me.dlgOpenFile, "dlgOpenFile")
        '
        'msMenuBar
        '
        Me.msMenuBar.BackColor = System.Drawing.Color.Teal
        Me.msMenuBar.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ModoPostprocesoBtn, Me.CalcularToolStripMenuItem, Me.PostprocesoToolStripMenuItem})
        resources.ApplyResources(Me.msMenuBar, "msMenuBar")
        Me.msMenuBar.Name = "msMenuBar"
        Me.msMenuBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnOpenFile, Me.btnSaveAs, Me.ToolStripSeparator11, Me.btnImportSurfaces, Me.ToolStripSeparator3, Me.btnExit})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
        '
        'btnOpenFile
        '
        resources.ApplyResources(Me.btnOpenFile, "btnOpenFile")
        Me.btnOpenFile.ForeColor = System.Drawing.Color.Black
        Me.btnOpenFile.Name = "btnOpenFile"
        '
        'btnSaveAs
        '
        resources.ApplyResources(Me.btnSaveAs, "btnSaveAs")
        Me.btnSaveAs.ForeColor = System.Drawing.Color.Black
        Me.btnSaveAs.Name = "btnSaveAs"
        '
        'ToolStripSeparator11
        '
        Me.ToolStripSeparator11.Name = "ToolStripSeparator11"
        resources.ApplyResources(Me.ToolStripSeparator11, "ToolStripSeparator11")
        '
        'btnImportSurfaces
        '
        Me.btnImportSurfaces.Name = "btnImportSurfaces"
        resources.ApplyResources(Me.btnImportSurfaces, "btnImportSurfaces")
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        resources.ApplyResources(Me.ToolStripSeparator3, "ToolStripSeparator3")
        '
        'btnExit
        '
        Me.btnExit.Name = "btnExit"
        resources.ApplyResources(Me.btnExit, "btnExit")
        '
        'ModoPostprocesoBtn
        '
        Me.ModoPostprocesoBtn.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnViewOptions})
        Me.ModoPostprocesoBtn.Name = "ModoPostprocesoBtn"
        resources.ApplyResources(Me.ModoPostprocesoBtn, "ModoPostprocesoBtn")
        '
        'btnViewOptions
        '
        resources.ApplyResources(Me.btnViewOptions, "btnViewOptions")
        Me.btnViewOptions.ForeColor = System.Drawing.Color.Black
        Me.btnViewOptions.Name = "btnViewOptions"
        '
        'CalcularToolStripMenuItem
        '
        Me.CalcularToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.CalcularToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnSteadyAnalysis, Me.btnUnsteadyAnalysis, Me.btnAeroelastic, Me.btnFieldEvaluation, Me.ToolStripSeparator9, Me.btnSettings})
        Me.CalcularToolStripMenuItem.Name = "CalcularToolStripMenuItem"
        resources.ApplyResources(Me.CalcularToolStripMenuItem, "CalcularToolStripMenuItem")
        '
        'btnSteadyAnalysis
        '
        Me.btnSteadyAnalysis.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnStartSteady})
        Me.btnSteadyAnalysis.Name = "btnSteadyAnalysis"
        resources.ApplyResources(Me.btnSteadyAnalysis, "btnSteadyAnalysis")
        '
        'btnStartSteady
        '
        resources.ApplyResources(Me.btnStartSteady, "btnStartSteady")
        Me.btnStartSteady.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnStartSteady.Name = "btnStartSteady"
        '
        'btnUnsteadyAnalysis
        '
        Me.btnUnsteadyAnalysis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.btnUnsteadyAnalysis.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnStartUnsteady, Me.btnUnsteadyHistogram})
        Me.btnUnsteadyAnalysis.Name = "btnUnsteadyAnalysis"
        resources.ApplyResources(Me.btnUnsteadyAnalysis, "btnUnsteadyAnalysis")
        '
        'btnStartUnsteady
        '
        resources.ApplyResources(Me.btnStartUnsteady, "btnStartUnsteady")
        Me.btnStartUnsteady.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnStartUnsteady.Name = "btnStartUnsteady"
        '
        'btnUnsteadyHistogram
        '
        Me.btnUnsteadyHistogram.Name = "btnUnsteadyHistogram"
        resources.ApplyResources(Me.btnUnsteadyHistogram, "btnUnsteadyHistogram")
        '
        'btnAeroelastic
        '
        Me.btnAeroelastic.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnStartAeroelastic, Me.btnAeroelasticHistogram})
        Me.btnAeroelastic.Name = "btnAeroelastic"
        resources.ApplyResources(Me.btnAeroelastic, "btnAeroelastic")
        '
        'btnStartAeroelastic
        '
        resources.ApplyResources(Me.btnStartAeroelastic, "btnStartAeroelastic")
        Me.btnStartAeroelastic.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnStartAeroelastic.Name = "btnStartAeroelastic"
        '
        'btnAeroelasticHistogram
        '
        Me.btnAeroelasticHistogram.Name = "btnAeroelasticHistogram"
        resources.ApplyResources(Me.btnAeroelasticHistogram, "btnAeroelasticHistogram")
        '
        'btnFieldEvaluation
        '
        Me.btnFieldEvaluation.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmFieldEvaluation, Me.btnLocalVelocity})
        Me.btnFieldEvaluation.Name = "btnFieldEvaluation"
        resources.ApplyResources(Me.btnFieldEvaluation, "btnFieldEvaluation")
        '
        'tsmFieldEvaluation
        '
        Me.tsmFieldEvaluation.Name = "tsmFieldEvaluation"
        resources.ApplyResources(Me.tsmFieldEvaluation, "tsmFieldEvaluation")
        '
        'btnLocalVelocity
        '
        Me.btnLocalVelocity.Name = "btnLocalVelocity"
        resources.ApplyResources(Me.btnLocalVelocity, "btnLocalVelocity")
        '
        'ToolStripSeparator9
        '
        Me.ToolStripSeparator9.Name = "ToolStripSeparator9"
        resources.ApplyResources(Me.ToolStripSeparator9, "ToolStripSeparator9")
        '
        'btnSettings
        '
        resources.ApplyResources(Me.btnSettings, "btnSettings")
        Me.btnSettings.ForeColor = System.Drawing.Color.Black
        Me.btnSettings.Name = "btnSettings"
        '
        'PostprocesoToolStripMenuItem
        '
        Me.PostprocesoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnShowResults, Me.btnLoadResults})
        Me.PostprocesoToolStripMenuItem.Name = "PostprocesoToolStripMenuItem"
        resources.ApplyResources(Me.PostprocesoToolStripMenuItem, "PostprocesoToolStripMenuItem")
        '
        'btnShowResults
        '
        Me.btnShowResults.Name = "btnShowResults"
        resources.ApplyResources(Me.btnShowResults, "btnShowResults")
        '
        'btnLoadResults
        '
        Me.btnLoadResults.Name = "btnLoadResults"
        resources.ApplyResources(Me.btnLoadResults, "btnLoadResults")
        '
        'niNotificationTool
        '
        resources.ApplyResources(Me.niNotificationTool, "niNotificationTool")
        '
        'BottomToolStripPanel
        '
        resources.ApplyResources(Me.BottomToolStripPanel, "BottomToolStripPanel")
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'TopToolStripPanel
        '
        resources.ApplyResources(Me.TopToolStripPanel, "TopToolStripPanel")
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'RightToolStripPanel
        '
        resources.ApplyResources(Me.RightToolStripPanel, "RightToolStripPanel")
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'LeftToolStripPanel
        '
        resources.ApplyResources(Me.LeftToolStripPanel, "LeftToolStripPanel")
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        '
        'ContentPanel
        '
        resources.ApplyResources(Me.ContentPanel, "ContentPanel")
        '
        'miniToolStrip
        '
        Me.miniToolStrip.BackColor = System.Drawing.SystemColors.Control
        Me.miniToolStrip.CanOverflow = False
        resources.ApplyResources(Me.miniToolStrip, "miniToolStrip")
        Me.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.miniToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.miniToolStrip.Name = "miniToolStrip"
        '
        'ttSelectedEntity
        '
        Me.ttSelectedEntity.AutomaticDelay = 100
        Me.ttSelectedEntity.UseAnimation = False
        '
        'MainForm
        '
        Me.AllowDrop = True
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Contenedor)
        Me.Controls.Add(Me.ssStatus)
        Me.Controls.Add(Me.msMenuBar)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.msMenuBar
        Me.Name = "MainForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Contenedor.ContentPanel.ResumeLayout(False)
        Me.Contenedor.TopToolStripPanel.ResumeLayout(False)
        Me.Contenedor.TopToolStripPanel.PerformLayout()
        Me.Contenedor.ResumeLayout(False)
        Me.Contenedor.PerformLayout()
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        CType(Me.ControlOpenGL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsTools.ResumeLayout(False)
        Me.tsTools.PerformLayout()
        Me.ssStatus.ResumeLayout(False)
        Me.ssStatus.PerformLayout()
        Me.msMenuBar.ResumeLayout(False)
        Me.msMenuBar.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ssStatus As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents dlgSaveFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents dlgOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents msMenuBar As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CalcularToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PostprocesoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOpenFile As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSaveAs As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnExit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSteadyAnalysis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnUnsteadyAnalysis As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnShowResults As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTipS As System.Windows.Forms.ToolTip
    Friend WithEvents btnFieldEvaluation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents niNotificationTool As System.Windows.Forms.NotifyIcon
    Friend WithEvents tsmFieldEvaluation As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnLocalVelocity As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnStartSteady As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnLoadResults As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModoPostprocesoBtn As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnStartUnsteady As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSettings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents miniToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnPlayStop As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnOpenObjectsManager As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnDesign As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnPostprocess As System.Windows.Forms.ToolStripButton
    Friend WithEvents tss10 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsTools As System.Windows.Forms.ToolStrip
    Friend WithEvents ContenedorDeBarras As System.Windows.Forms.ToolStripPanel
    Friend WithEvents Contenedor As System.Windows.Forms.ToolStripContainer
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents ControlOpenGL As SharpGL.OpenGLControl
    Friend WithEvents sbHorizontal As System.Windows.Forms.HScrollBar
    Friend WithEvents sbVertical As System.Windows.Forms.VScrollBar
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents btnNewProject As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnOpenProject As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnSaveProject As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbHelp As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnViewOptions As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnAeroelastic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnStartAeroelastic As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator11 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnImportSurfaces As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btTranslate As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnAlign As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator12 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents btnViewTop As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnViewFront As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnViewLeft As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnView3D As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnViewCenter As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbFieldEvaluation As System.Windows.Forms.ToolStripButton
    Friend WithEvents ttSelectedEntity As System.Windows.Forms.ToolTip
    Friend WithEvents cbModes As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents tbDisplacementScale As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents lblResultType As ToolStripLabel
    Friend WithEvents sepResults As ToolStripSeparator
    Friend WithEvents lblResultScale As ToolStripLabel
    Friend WithEvents btnUnsteadyHistogram As ToolStripMenuItem
    Friend WithEvents btnAeroelasticHistogram As ToolStripMenuItem
End Class

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
        Me.ControlOpenGL = New SharpGL.OpenGLControl()
        Me.dlgSaveFile = New System.Windows.Forms.SaveFileDialog()
        Me.dlgOpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.ToolTipS = New System.Windows.Forms.ToolTip(Me.components)
        Me.niNotificationTool = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.miniToolStrip = New System.Windows.Forms.ToolStrip()
        Me.sbVertical = New System.Windows.Forms.VScrollBar()
        Me.scMain = New System.Windows.Forms.SplitContainer()
        Me.sbHorizontal = New System.Windows.Forms.HScrollBar()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lblWebSite = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ssStatus = New System.Windows.Forms.StatusStrip()
        Me.mrRibbon = New OpenVOGEL.DesignTools.MainRibbon()
        CType(Me.ControlOpenGL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.ssStatus.SuspendLayout()
        Me.SuspendLayout()
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
        'dlgSaveFile
        '
        resources.ApplyResources(Me.dlgSaveFile, "dlgSaveFile")
        '
        'dlgOpenFile
        '
        resources.ApplyResources(Me.dlgOpenFile, "dlgOpenFile")
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
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        '
        'LeftToolStripPanel
        '
        resources.ApplyResources(Me.LeftToolStripPanel, "LeftToolStripPanel")
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(0, 3, 0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.BackColor = System.Drawing.SystemColors.Control
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
        'sbVertical
        '
        resources.ApplyResources(Me.sbVertical, "sbVertical")
        Me.sbVertical.Maximum = 90
        Me.sbVertical.Minimum = -90
        Me.sbVertical.Name = "sbVertical"
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
        Me.scMain.TabStop = False
        '
        'sbHorizontal
        '
        resources.ApplyResources(Me.sbHorizontal, "sbHorizontal")
        Me.sbHorizontal.Maximum = 180
        Me.sbHorizontal.Minimum = -180
        Me.sbHorizontal.Name = "sbHorizontal"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        resources.ApplyResources(Me.lblStatus, "lblStatus")
        Me.lblStatus.ForeColor = System.Drawing.Color.Black
        Me.lblStatus.Name = "lblStatus"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.BackColor = System.Drawing.Color.Transparent
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        resources.ApplyResources(Me.ToolStripStatusLabel2, "ToolStripStatusLabel2")
        '
        'lblWebSite
        '
        Me.lblWebSite.BackColor = System.Drawing.Color.Transparent
        Me.lblWebSite.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        resources.ApplyResources(Me.lblWebSite, "lblWebSite")
        Me.lblWebSite.IsLink = True
        Me.lblWebSite.Name = "lblWebSite"
        '
        'ssStatus
        '
        Me.ssStatus.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus, Me.ToolStripStatusLabel2, Me.lblWebSite})
        resources.ApplyResources(Me.ssStatus, "ssStatus")
        Me.ssStatus.Name = "ssStatus"
        '
        'mrRibbon
        '
        resources.ApplyResources(Me.mrRibbon, "mrRibbon")
        Me.mrRibbon.Name = "mrRibbon"
        '
        'MainForm
        '
        Me.AllowDrop = True
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.sbVertical)
        Me.Controls.Add(Me.sbHorizontal)
        Me.Controls.Add(Me.mrRibbon)
        Me.Controls.Add(Me.ssStatus)
        Me.IsMdiContainer = True
        Me.Name = "MainForm"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.ControlOpenGL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.Panel2.ResumeLayout(False)
        CType(Me.scMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scMain.ResumeLayout(False)
        Me.ssStatus.ResumeLayout(False)
        Me.ssStatus.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents dlgSaveFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents dlgOpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolTipS As System.Windows.Forms.ToolTip
    Friend WithEvents niNotificationTool As System.Windows.Forms.NotifyIcon
    Friend WithEvents miniToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents ContenedorDeBarras As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ControlOpenGL As SharpGL.OpenGLControl
    Friend WithEvents mrRibbon As DesignTools.MainRibbon
    Friend WithEvents sbVertical As VScrollBar
    Friend WithEvents scMain As SplitContainer
    Friend WithEvents BottomToolStripPanel As ToolStripPanel
    Friend WithEvents TopToolStripPanel As ToolStripPanel
    Friend WithEvents RightToolStripPanel As ToolStripPanel
    Friend WithEvents LeftToolStripPanel As ToolStripPanel
    Friend WithEvents ContentPanel As ToolStripContentPanel
    Friend WithEvents sbHorizontal As HScrollBar
    Friend WithEvents lblStatus As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents lblWebSite As ToolStripStatusLabel
    Friend WithEvents ssStatus As StatusStrip
End Class

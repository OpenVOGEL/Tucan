<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GroupPanel
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblSelection = New System.Windows.Forms.Label()
        Me.lbxPanels = New System.Windows.Forms.ListBox()
        Me.lblPanel = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblPanelArea = New System.Windows.Forms.Label()
        Me.lblPanelLoad = New System.Windows.Forms.Label()
        Me.lblPanelCount = New System.Windows.Forms.Label()
        Me.lblTotalArea = New System.Windows.Forms.Label()
        Me.lblTotalLoad = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSelection
        '
        Me.lblSelection.AutoSize = True
        Me.lblSelection.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelection.Location = New System.Drawing.Point(5, 5)
        Me.lblSelection.Name = "lblSelection"
        Me.lblSelection.Size = New System.Drawing.Size(94, 15)
        Me.lblSelection.TabIndex = 0
        Me.lblSelection.Text = "Selected panels"
        '
        'lbxPanels
        '
        Me.lbxPanels.FormattingEnabled = True
        Me.lbxPanels.Location = New System.Drawing.Point(8, 21)
        Me.lbxPanels.Name = "lbxPanels"
        Me.lbxPanels.Size = New System.Drawing.Size(122, 264)
        Me.lbxPanels.TabIndex = 1
        '
        'lblPanel
        '
        Me.lblPanel.AutoSize = True
        Me.lblPanel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPanel.Location = New System.Drawing.Point(136, 21)
        Me.lblPanel.Name = "lblPanel"
        Me.lblPanel.Size = New System.Drawing.Size(80, 13)
        Me.lblPanel.TabIndex = 2
        Me.lblPanel.Text = "Panel number"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(136, 117)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(372, 168)
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        '
        'lblPanelArea
        '
        Me.lblPanelArea.AutoSize = True
        Me.lblPanelArea.Location = New System.Drawing.Point(136, 36)
        Me.lblPanelArea.Name = "lblPanelArea"
        Me.lblPanelArea.Size = New System.Drawing.Size(60, 13)
        Me.lblPanelArea.TabIndex = 4
        Me.lblPanelArea.Text = "Panel area"
        '
        'lblPanelLoad
        '
        Me.lblPanelLoad.AutoSize = True
        Me.lblPanelLoad.Location = New System.Drawing.Point(136, 49)
        Me.lblPanelLoad.Name = "lblPanelLoad"
        Me.lblPanelLoad.Size = New System.Drawing.Size(61, 13)
        Me.lblPanelLoad.TabIndex = 5
        Me.lblPanelLoad.Text = "Panel load"
        '
        'lblPanelCount
        '
        Me.lblPanelCount.AutoSize = True
        Me.lblPanelCount.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPanelCount.Location = New System.Drawing.Point(136, 73)
        Me.lblPanelCount.Name = "lblPanelCount"
        Me.lblPanelCount.Size = New System.Drawing.Size(69, 13)
        Me.lblPanelCount.TabIndex = 6
        Me.lblPanelCount.Text = "Panel count"
        '
        'lblTotalArea
        '
        Me.lblTotalArea.AutoSize = True
        Me.lblTotalArea.Location = New System.Drawing.Point(137, 89)
        Me.lblTotalArea.Name = "lblTotalArea"
        Me.lblTotalArea.Size = New System.Drawing.Size(57, 13)
        Me.lblTotalArea.TabIndex = 7
        Me.lblTotalArea.Text = "Total area"
        '
        'lblTotalLoad
        '
        Me.lblTotalLoad.AutoSize = True
        Me.lblTotalLoad.Location = New System.Drawing.Point(137, 101)
        Me.lblTotalLoad.Name = "lblTotalLoad"
        Me.lblTotalLoad.Size = New System.Drawing.Size(58, 13)
        Me.lblTotalLoad.TabIndex = 8
        Me.lblTotalLoad.Text = "Total load"
        '
        'GroupPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.lblTotalLoad)
        Me.Controls.Add(Me.lblTotalArea)
        Me.Controls.Add(Me.lblPanelCount)
        Me.Controls.Add(Me.lblPanelLoad)
        Me.Controls.Add(Me.lblPanelArea)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblPanel)
        Me.Controls.Add(Me.lbxPanels)
        Me.Controls.Add(Me.lblSelection)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ForeColor = System.Drawing.Color.Black
        Me.Name = "GroupPanel"
        Me.Size = New System.Drawing.Size(519, 297)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblSelection As Label
    Friend WithEvents lbxPanels As ListBox
    Friend WithEvents lblPanel As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents lblPanelArea As Label
    Friend WithEvents lblPanelLoad As Label
    Friend WithEvents lblPanelCount As Label
    Friend WithEvents lblTotalArea As Label
    Friend WithEvents lblTotalLoad As Label
End Class

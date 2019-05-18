<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CustomPolarControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.btnRemovePoint = New System.Windows.Forms.Button()
        Me.btnAddPoint = New System.Windows.Forms.Button()
        Me.dgvNodes = New System.Windows.Forms.DataGridView()
        Me.cmsIO = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.btnGetFromClipboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudReynolds = New System.Windows.Forms.NumericUpDown()
        Me.tbPolarName = New System.Windows.Forms.TextBox()
        CType(Me.dgvNodes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsIO.SuspendLayout()
        CType(Me.nudReynolds, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnRemovePoint
        '
        Me.btnRemovePoint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemovePoint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemovePoint.Location = New System.Drawing.Point(92, 258)
        Me.btnRemovePoint.Name = "btnRemovePoint"
        Me.btnRemovePoint.Size = New System.Drawing.Size(78, 24)
        Me.btnRemovePoint.TabIndex = 11
        Me.btnRemovePoint.Text = "Remove"
        Me.btnRemovePoint.UseVisualStyleBackColor = True
        '
        'btnAddPoint
        '
        Me.btnAddPoint.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddPoint.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddPoint.Location = New System.Drawing.Point(3, 258)
        Me.btnAddPoint.Name = "btnAddPoint"
        Me.btnAddPoint.Size = New System.Drawing.Size(78, 24)
        Me.btnAddPoint.TabIndex = 10
        Me.btnAddPoint.Text = "Add"
        Me.btnAddPoint.UseVisualStyleBackColor = True
        '
        'dgvNodes
        '
        Me.dgvNodes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvNodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNodes.ContextMenuStrip = Me.cmsIO
        Me.dgvNodes.Location = New System.Drawing.Point(0, 26)
        Me.dgvNodes.Name = "dgvNodes"
        Me.dgvNodes.Size = New System.Drawing.Size(174, 200)
        Me.dgvNodes.TabIndex = 12
        '
        'cmsIO
        '
        Me.cmsIO.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnGetFromClipboard})
        Me.cmsIO.Name = "cmsIO"
        Me.cmsIO.Size = New System.Drawing.Size(175, 26)
        '
        'btnGetFromClipboard
        '
        Me.btnGetFromClipboard.Name = "btnGetFromClipboard"
        Me.btnGetFromClipboard.Size = New System.Drawing.Size(174, 22)
        Me.btnGetFromClipboard.Text = "Get from clipboard"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1, 234)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(24, 13)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Re:"
        '
        'nudReynolds
        '
        Me.nudReynolds.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.nudReynolds.Increment = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudReynolds.Location = New System.Drawing.Point(26, 231)
        Me.nudReynolds.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
        Me.nudReynolds.Name = "nudReynolds"
        Me.nudReynolds.Size = New System.Drawing.Size(86, 20)
        Me.nudReynolds.TabIndex = 18
        Me.nudReynolds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudReynolds.Value = New Decimal(New Integer() {200000, 0, 0, 0})
        '
        'tbPolarName
        '
        Me.tbPolarName.Dock = System.Windows.Forms.DockStyle.Top
        Me.tbPolarName.Location = New System.Drawing.Point(0, 0)
        Me.tbPolarName.Name = "tbPolarName"
        Me.tbPolarName.Size = New System.Drawing.Size(174, 20)
        Me.tbPolarName.TabIndex = 20
        '
        'CustomPolarControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tbPolarName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.nudReynolds)
        Me.Controls.Add(Me.dgvNodes)
        Me.Controls.Add(Me.btnRemovePoint)
        Me.Controls.Add(Me.btnAddPoint)
        Me.MinimumSize = New System.Drawing.Size(165, 200)
        Me.Name = "CustomPolarControl"
        Me.Size = New System.Drawing.Size(174, 289)
        CType(Me.dgvNodes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsIO.ResumeLayout(False)
        CType(Me.nudReynolds, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRemovePoint As System.Windows.Forms.Button
    Friend WithEvents btnAddPoint As System.Windows.Forms.Button
    Friend WithEvents dgvNodes As System.Windows.Forms.DataGridView
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents nudReynolds As System.Windows.Forms.NumericUpDown
    Friend WithEvents tbPolarName As System.Windows.Forms.TextBox
    Friend WithEvents cmsIO As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents btnGetFromClipboard As System.Windows.Forms.ToolStripMenuItem
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCamberLine
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.lbLines = New System.Windows.Forms.ListBox()
        Me.lblCamberLines = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.tbxCamberName = New System.Windows.Forms.TextBox()
        Me.btnAddNode = New System.Windows.Forms.Button()
        Me.btnDelNode = New System.Windows.Forms.Button()
        Me.pbPlot = New System.Windows.Forms.PictureBox()
        Me.gbxNacaGenerate = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudMaxCamber = New System.Windows.Forms.NumericUpDown()
        Me.nudXmax = New System.Windows.Forms.NumericUpDown()
        Me.btnGenerateNaca = New System.Windows.Forms.Button()
        Me.gbxNode = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudX = New System.Windows.Forms.NumericUpDown()
        Me.nudY = New System.Windows.Forms.NumericUpDown()
        Me.btnImportTable = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        CType(Me.pbPlot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbxNacaGenerate.SuspendLayout()
        CType(Me.nudMaxCamber, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudXmax, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbxNode.SuspendLayout()
        CType(Me.nudX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbLines
        '
        Me.lbLines.FormattingEnabled = True
        Me.lbLines.Location = New System.Drawing.Point(12, 24)
        Me.lbLines.Name = "lbLines"
        Me.lbLines.Size = New System.Drawing.Size(280, 108)
        Me.lbLines.TabIndex = 0
        '
        'lblCamberLines
        '
        Me.lblCamberLines.AutoSize = True
        Me.lblCamberLines.Location = New System.Drawing.Point(9, 6)
        Me.lblCamberLines.Name = "lblCamberLines"
        Me.lblCamberLines.Size = New System.Drawing.Size(164, 13)
        Me.lblCamberLines.TabIndex = 1
        Me.lblCamberLines.Text = "List of generated camber lines:"
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(298, 24)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(298, 47)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnRemove.TabIndex = 3
        Me.btnRemove.Text = "Remove"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'tbxCamberName
        '
        Me.tbxCamberName.Location = New System.Drawing.Point(12, 136)
        Me.tbxCamberName.Name = "tbxCamberName"
        Me.tbxCamberName.Size = New System.Drawing.Size(199, 22)
        Me.tbxCamberName.TabIndex = 8
        '
        'btnAddNode
        '
        Me.btnAddNode.Location = New System.Drawing.Point(217, 136)
        Me.btnAddNode.Name = "btnAddNode"
        Me.btnAddNode.Size = New System.Drawing.Size(75, 23)
        Me.btnAddNode.TabIndex = 9
        Me.btnAddNode.Text = "Add node"
        Me.btnAddNode.UseVisualStyleBackColor = True
        '
        'btnDelNode
        '
        Me.btnDelNode.Location = New System.Drawing.Point(298, 136)
        Me.btnDelNode.Name = "btnDelNode"
        Me.btnDelNode.Size = New System.Drawing.Size(75, 23)
        Me.btnDelNode.TabIndex = 10
        Me.btnDelNode.Text = "Del. node"
        Me.btnDelNode.UseVisualStyleBackColor = True
        '
        'pbPlot
        '
        Me.pbPlot.Location = New System.Drawing.Point(12, 164)
        Me.pbPlot.Name = "pbPlot"
        Me.pbPlot.Size = New System.Drawing.Size(361, 99)
        Me.pbPlot.TabIndex = 11
        Me.pbPlot.TabStop = False
        '
        'gbxNacaGenerate
        '
        Me.gbxNacaGenerate.Controls.Add(Me.Label2)
        Me.gbxNacaGenerate.Controls.Add(Me.Label1)
        Me.gbxNacaGenerate.Controls.Add(Me.nudMaxCamber)
        Me.gbxNacaGenerate.Controls.Add(Me.nudXmax)
        Me.gbxNacaGenerate.Controls.Add(Me.btnGenerateNaca)
        Me.gbxNacaGenerate.Location = New System.Drawing.Point(12, 333)
        Me.gbxNacaGenerate.Name = "gbxNacaGenerate"
        Me.gbxNacaGenerate.Size = New System.Drawing.Size(361, 55)
        Me.gbxNacaGenerate.TabIndex = 12
        Me.gbxNacaGenerate.TabStop = False
        Me.gbxNacaGenerate.Text = "NACA camber line generator"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(129, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 13)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "x/C:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(27, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "e/C:"
        '
        'nudMaxCamber
        '
        Me.nudMaxCamber.DecimalPlaces = 3
        Me.nudMaxCamber.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudMaxCamber.Location = New System.Drawing.Point(40, 24)
        Me.nudMaxCamber.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMaxCamber.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudMaxCamber.Name = "nudMaxCamber"
        Me.nudMaxCamber.Size = New System.Drawing.Size(71, 22)
        Me.nudMaxCamber.TabIndex = 14
        Me.nudMaxCamber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudXmax
        '
        Me.nudXmax.DecimalPlaces = 3
        Me.nudXmax.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudXmax.Location = New System.Drawing.Point(159, 24)
        Me.nudXmax.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudXmax.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudXmax.Name = "nudXmax"
        Me.nudXmax.Size = New System.Drawing.Size(71, 22)
        Me.nudXmax.TabIndex = 13
        Me.nudXmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnGenerateNaca
        '
        Me.btnGenerateNaca.Location = New System.Drawing.Point(280, 24)
        Me.btnGenerateNaca.Name = "btnGenerateNaca"
        Me.btnGenerateNaca.Size = New System.Drawing.Size(75, 23)
        Me.btnGenerateNaca.TabIndex = 6
        Me.btnGenerateNaca.Text = "Generate"
        Me.btnGenerateNaca.UseVisualStyleBackColor = True
        '
        'gbxNode
        '
        Me.gbxNode.Controls.Add(Me.Label3)
        Me.gbxNode.Controls.Add(Me.Label4)
        Me.gbxNode.Controls.Add(Me.nudX)
        Me.gbxNode.Controls.Add(Me.nudY)
        Me.gbxNode.Controls.Add(Me.btnImportTable)
        Me.gbxNode.Location = New System.Drawing.Point(12, 269)
        Me.gbxNode.Name = "gbxNode"
        Me.gbxNode.Size = New System.Drawing.Size(361, 55)
        Me.gbxNode.TabIndex = 13
        Me.gbxNode.TabStop = False
        Me.gbxNode.Text = "Node"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(129, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 13)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "y/C:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 13)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "x/C:"
        '
        'nudX
        '
        Me.nudX.DecimalPlaces = 3
        Me.nudX.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudX.Location = New System.Drawing.Point(40, 24)
        Me.nudX.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudX.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudX.Name = "nudX"
        Me.nudX.Size = New System.Drawing.Size(71, 22)
        Me.nudX.TabIndex = 14
        Me.nudX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudY
        '
        Me.nudY.DecimalPlaces = 3
        Me.nudY.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudY.Location = New System.Drawing.Point(159, 24)
        Me.nudY.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudY.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudY.Name = "nudY"
        Me.nudY.Size = New System.Drawing.Size(71, 22)
        Me.nudY.TabIndex = 13
        Me.nudY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnImportTable
        '
        Me.btnImportTable.Enabled = False
        Me.btnImportTable.Location = New System.Drawing.Point(280, 23)
        Me.btnImportTable.Name = "btnImportTable"
        Me.btnImportTable.Size = New System.Drawing.Size(75, 23)
        Me.btnImportTable.TabIndex = 6
        Me.btnImportTable.Text = "Import tbl"
        Me.btnImportTable.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOK.Location = New System.Drawing.Point(295, 394)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(78, 23)
        Me.btnOK.TabIndex = 14
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'FormCamberLine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(380, 426)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbxNode)
        Me.Controls.Add(Me.gbxNacaGenerate)
        Me.Controls.Add(Me.pbPlot)
        Me.Controls.Add(Me.btnDelNode)
        Me.Controls.Add(Me.btnAddNode)
        Me.Controls.Add(Me.tbxCamberName)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lblCamberLines)
        Me.Controls.Add(Me.lbLines)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormCamberLine"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Camber lines data base"
        CType(Me.pbPlot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbxNacaGenerate.ResumeLayout(False)
        Me.gbxNacaGenerate.PerformLayout()
        CType(Me.nudMaxCamber, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudXmax, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbxNode.ResumeLayout(False)
        Me.gbxNode.PerformLayout()
        CType(Me.nudX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lbLines As Windows.Forms.ListBox
    Friend WithEvents lblCamberLines As Windows.Forms.Label
    Friend WithEvents btnAdd As Windows.Forms.Button
    Friend WithEvents btnRemove As Windows.Forms.Button
    Friend WithEvents tbxCamberName As Windows.Forms.TextBox
    Friend WithEvents btnAddNode As Windows.Forms.Button
    Friend WithEvents btnDelNode As Windows.Forms.Button
    Friend WithEvents pbPlot As Windows.Forms.PictureBox
    Friend WithEvents gbxNacaGenerate As Windows.Forms.GroupBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents nudMaxCamber As Windows.Forms.NumericUpDown
    Friend WithEvents nudXmax As Windows.Forms.NumericUpDown
    Friend WithEvents btnGenerateNaca As Windows.Forms.Button
    Friend WithEvents gbxNode As Windows.Forms.GroupBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents nudX As Windows.Forms.NumericUpDown
    Friend WithEvents nudY As Windows.Forms.NumericUpDown
    Friend WithEvents btnImportTable As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
End Class

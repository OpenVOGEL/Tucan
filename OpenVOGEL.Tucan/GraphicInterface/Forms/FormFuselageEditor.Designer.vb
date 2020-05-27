<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormFuselageEditor
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lbSections = New System.Windows.Forms.ListBox()
        Me.btnAddSection = New System.Windows.Forms.Button()
        Me.btnRemoveSection = New System.Windows.Forms.Button()
        Me.pbSections = New System.Windows.Forms.PictureBox()
        Me.nudPosition = New System.Windows.Forms.NumericUpDown()
        Me.lblPosition = New System.Windows.Forms.Label()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.clbWingsToAnchor = New System.Windows.Forms.CheckedListBox()
        Me.btnAddPoint = New System.Windows.Forms.Button()
        Me.btnRemovePoint = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.lblNode = New System.Windows.Forms.Label()
        Me.nudX = New System.Windows.Forms.NumericUpDown()
        Me.nudY = New System.Windows.Forms.NumericUpDown()
        Me.lblX = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudNPS = New System.Windows.Forms.NumericUpDown()
        Me.lblNPS = New System.Windows.Forms.Label()
        Me.lblNPZ = New System.Windows.Forms.Label()
        Me.nudNPZ = New System.Windows.Forms.NumericUpDown()
        Me.tbName = New System.Windows.Forms.TextBox()
        Me.pbSideView = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.btnCenter = New System.Windows.Forms.Button()
        Me.cbxBrokenEdge = New System.Windows.Forms.CheckBox()
        CType(Me.pbSections, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPosition, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNPS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNPZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbSideView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbSections
        '
        Me.lbSections.FormattingEnabled = True
        Me.lbSections.Location = New System.Drawing.Point(6, 29)
        Me.lbSections.Name = "lbSections"
        Me.lbSections.Size = New System.Drawing.Size(189, 134)
        Me.lbSections.TabIndex = 1
        '
        'btnAddSection
        '
        Me.btnAddSection.BackColor = System.Drawing.Color.White
        Me.btnAddSection.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnAddSection.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnAddSection.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnAddSection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnAddSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddSection.Location = New System.Drawing.Point(196, 29)
        Me.btnAddSection.Name = "btnAddSection"
        Me.btnAddSection.Size = New System.Drawing.Size(58, 23)
        Me.btnAddSection.TabIndex = 103
        Me.btnAddSection.Text = "Add"
        Me.btnAddSection.UseVisualStyleBackColor = False
        '
        'btnRemoveSection
        '
        Me.btnRemoveSection.BackColor = System.Drawing.Color.White
        Me.btnRemoveSection.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnRemoveSection.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnRemoveSection.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnRemoveSection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnRemoveSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveSection.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveSection.Location = New System.Drawing.Point(196, 53)
        Me.btnRemoveSection.Name = "btnRemoveSection"
        Me.btnRemoveSection.Size = New System.Drawing.Size(58, 23)
        Me.btnRemoveSection.TabIndex = 104
        Me.btnRemoveSection.Text = "Remove"
        Me.btnRemoveSection.UseVisualStyleBackColor = False
        '
        'pbSections
        '
        Me.pbSections.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbSections.BackColor = System.Drawing.Color.White
        Me.pbSections.Cursor = System.Windows.Forms.Cursors.Cross
        Me.pbSections.Location = New System.Drawing.Point(260, 25)
        Me.pbSections.Name = "pbSections"
        Me.pbSections.Size = New System.Drawing.Size(482, 296)
        Me.pbSections.TabIndex = 107
        Me.pbSections.TabStop = False
        '
        'nudPosition
        '
        Me.nudPosition.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPosition.Location = New System.Drawing.Point(164, 168)
        Me.nudPosition.Name = "nudPosition"
        Me.nudPosition.Size = New System.Drawing.Size(67, 22)
        Me.nudPosition.TabIndex = 108
        '
        'lblPosition
        '
        Me.lblPosition.AutoSize = True
        Me.lblPosition.Location = New System.Drawing.Point(8, 171)
        Me.lblPosition.Name = "lblPosition"
        Me.lblPosition.Size = New System.Drawing.Size(150, 13)
        Me.lblPosition.TabIndex = 109
        Me.lblPosition.Text = "Current section position (Z):"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.BackColor = System.Drawing.Color.White
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(665, 480)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 111
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 285)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 122
        Me.Label1.Text = "Anchor to:"
        '
        'clbWingsToAnchor
        '
        Me.clbWingsToAnchor.FormattingEnabled = True
        Me.clbWingsToAnchor.Location = New System.Drawing.Point(6, 300)
        Me.clbWingsToAnchor.Name = "clbWingsToAnchor"
        Me.clbWingsToAnchor.Size = New System.Drawing.Size(248, 72)
        Me.clbWingsToAnchor.TabIndex = 121
        '
        'btnAddPoint
        '
        Me.btnAddPoint.BackColor = System.Drawing.Color.White
        Me.btnAddPoint.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnAddPoint.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnAddPoint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnAddPoint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnAddPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddPoint.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddPoint.Location = New System.Drawing.Point(196, 221)
        Me.btnAddPoint.Name = "btnAddPoint"
        Me.btnAddPoint.Size = New System.Drawing.Size(58, 23)
        Me.btnAddPoint.TabIndex = 126
        Me.btnAddPoint.Text = "Add"
        Me.btnAddPoint.UseVisualStyleBackColor = False
        '
        'btnRemovePoint
        '
        Me.btnRemovePoint.BackColor = System.Drawing.Color.White
        Me.btnRemovePoint.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnRemovePoint.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnRemovePoint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnRemovePoint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnRemovePoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemovePoint.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemovePoint.Location = New System.Drawing.Point(196, 245)
        Me.btnRemovePoint.Name = "btnRemovePoint"
        Me.btnRemovePoint.Size = New System.Drawing.Size(58, 23)
        Me.btnRemovePoint.TabIndex = 127
        Me.btnRemovePoint.Text = "Remove"
        Me.btnRemovePoint.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.White
        Me.Button1.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.Button1.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.Button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.Button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(196, 30)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(58, 23)
        Me.Button1.TabIndex = 104
        Me.Button1.Text = "Remove"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'lblNode
        '
        Me.lblNode.AutoSize = True
        Me.lblNode.Location = New System.Drawing.Point(8, 221)
        Me.lblNode.Name = "lblNode"
        Me.lblNode.Size = New System.Drawing.Size(124, 13)
        Me.lblNode.TabIndex = 128
        Me.lblNode.Text = "Current node location:"
        '
        'nudX
        '
        Me.nudX.DecimalPlaces = 4
        Me.nudX.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudX.Location = New System.Drawing.Point(11, 256)
        Me.nudX.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudX.Name = "nudX"
        Me.nudX.Size = New System.Drawing.Size(75, 22)
        Me.nudX.TabIndex = 129
        '
        'nudY
        '
        Me.nudY.DecimalPlaces = 4
        Me.nudY.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudY.Location = New System.Drawing.Point(92, 256)
        Me.nudY.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudY.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudY.Name = "nudY"
        Me.nudY.Size = New System.Drawing.Size(75, 22)
        Me.nudY.TabIndex = 130
        '
        'lblX
        '
        Me.lblX.AutoSize = True
        Me.lblX.Location = New System.Drawing.Point(11, 240)
        Me.lblX.Name = "lblX"
        Me.lblX.Size = New System.Drawing.Size(16, 13)
        Me.lblX.TabIndex = 131
        Me.lblX.Text = "X:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(93, 240)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(15, 13)
        Me.Label2.TabIndex = 132
        Me.Label2.Text = "Y:"
        '
        'nudNPS
        '
        Me.nudNPS.Location = New System.Drawing.Point(110, 401)
        Me.nudNPS.Name = "nudNPS"
        Me.nudNPS.Size = New System.Drawing.Size(67, 22)
        Me.nudNPS.TabIndex = 135
        '
        'lblNPS
        '
        Me.lblNPS.AutoSize = True
        Me.lblNPS.Location = New System.Drawing.Point(6, 403)
        Me.lblNPS.Name = "lblNPS"
        Me.lblNPS.Size = New System.Drawing.Size(97, 13)
        Me.lblNPS.TabIndex = 136
        Me.lblNPS.Text = "Cross refinement:"
        '
        'lblNPZ
        '
        Me.lblNPZ.AutoSize = True
        Me.lblNPZ.Location = New System.Drawing.Point(6, 380)
        Me.lblNPZ.Name = "lblNPZ"
        Me.lblNPZ.Size = New System.Drawing.Size(98, 13)
        Me.lblNPZ.TabIndex = 138
        Me.lblNPZ.Text = "Long. refinement:"
        '
        'nudNPZ
        '
        Me.nudNPZ.Location = New System.Drawing.Point(110, 378)
        Me.nudNPZ.Name = "nudNPZ"
        Me.nudNPZ.Size = New System.Drawing.Size(67, 22)
        Me.nudNPZ.TabIndex = 137
        '
        'tbName
        '
        Me.tbName.Location = New System.Drawing.Point(6, 6)
        Me.tbName.Name = "tbName"
        Me.tbName.Size = New System.Drawing.Size(248, 22)
        Me.tbName.TabIndex = 139
        '
        'pbSideView
        '
        Me.pbSideView.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbSideView.BackColor = System.Drawing.Color.White
        Me.pbSideView.Cursor = System.Windows.Forms.Cursors.Cross
        Me.pbSideView.Location = New System.Drawing.Point(260, 340)
        Me.pbSideView.Name = "pbSideView"
        Me.pbSideView.Size = New System.Drawing.Size(482, 134)
        Me.pbSideView.TabIndex = 140
        Me.pbSideView.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(257, 324)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(55, 13)
        Me.Label3.TabIndex = 141
        Me.Label3.Text = "Side view"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(260, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 13)
        Me.Label4.TabIndex = 142
        Me.Label4.Text = "Cross section"
        '
        'btnCenter
        '
        Me.btnCenter.BackColor = System.Drawing.Color.White
        Me.btnCenter.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCenter.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnCenter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnCenter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCenter.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCenter.Location = New System.Drawing.Point(263, 29)
        Me.btnCenter.Name = "btnCenter"
        Me.btnCenter.Size = New System.Drawing.Size(58, 23)
        Me.btnCenter.TabIndex = 143
        Me.btnCenter.Text = "Center"
        Me.btnCenter.UseVisualStyleBackColor = False
        '
        'cbxBrokenEdge
        '
        Me.cbxBrokenEdge.AutoSize = True
        Me.cbxBrokenEdge.Location = New System.Drawing.Point(12, 193)
        Me.cbxBrokenEdge.Name = "cbxBrokenEdge"
        Me.cbxBrokenEdge.Size = New System.Drawing.Size(91, 17)
        Me.cbxBrokenEdge.TabIndex = 144
        Me.cbxBrokenEdge.Text = "Broken edge"
        Me.cbxBrokenEdge.UseVisualStyleBackColor = True
        '
        'FormFuselageEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 512)
        Me.Controls.Add(Me.cbxBrokenEdge)
        Me.Controls.Add(Me.btnCenter)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pbSideView)
        Me.Controls.Add(Me.tbName)
        Me.Controls.Add(Me.lblNPZ)
        Me.Controls.Add(Me.nudNPZ)
        Me.Controls.Add(Me.lblNPS)
        Me.Controls.Add(Me.nudNPS)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.lblX)
        Me.Controls.Add(Me.nudY)
        Me.Controls.Add(Me.nudX)
        Me.Controls.Add(Me.lblNode)
        Me.Controls.Add(Me.btnRemovePoint)
        Me.Controls.Add(Me.btnAddPoint)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.clbWingsToAnchor)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblPosition)
        Me.Controls.Add(Me.nudPosition)
        Me.Controls.Add(Me.pbSections)
        Me.Controls.Add(Me.btnRemoveSection)
        Me.Controls.Add(Me.btnAddSection)
        Me.Controls.Add(Me.lbSections)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MinimumSize = New System.Drawing.Size(668, 447)
        Me.Name = "FormFuselageEditor"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fuselage editor"
        CType(Me.pbSections, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPosition, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNPS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNPZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbSideView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbSections As System.Windows.Forms.ListBox
    Friend WithEvents btnAddSection As System.Windows.Forms.Button
    Friend WithEvents btnRemoveSection As System.Windows.Forms.Button
    Friend WithEvents pbSections As System.Windows.Forms.PictureBox
    Friend WithEvents nudPosition As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPosition As System.Windows.Forms.Label
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents clbWingsToAnchor As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnAddPoint As System.Windows.Forms.Button
    Friend WithEvents btnRemovePoint As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lblNode As System.Windows.Forms.Label
    Friend WithEvents nudX As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudY As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblX As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudNPS As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblNPS As System.Windows.Forms.Label
    Friend WithEvents lblNPZ As System.Windows.Forms.Label
    Friend WithEvents nudNPZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents tbName As System.Windows.Forms.TextBox
    Friend WithEvents pbSideView As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btnCenter As System.Windows.Forms.Button
    Friend WithEvents cbxBrokenEdge As CheckBox
End Class

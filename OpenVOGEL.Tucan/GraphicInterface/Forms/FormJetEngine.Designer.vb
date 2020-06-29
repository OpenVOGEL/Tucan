<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormJetEngine
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblX = New System.Windows.Forms.Label()
        Me.nudY = New System.Windows.Forms.NumericUpDown()
        Me.nudX = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudZ = New System.Windows.Forms.NumericUpDown()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.txtFrontL = New System.Windows.Forms.Label()
        Me.nudFrontL = New System.Windows.Forms.NumericUpDown()
        Me.txtRearD = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudRearD = New System.Windows.Forms.NumericUpDown()
        Me.nudFrontD = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.nudTotalL = New System.Windows.Forms.NumericUpDown()
        Me.nudRearL = New System.Windows.Forms.NumericUpDown()
        Me.tbxName = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.gbPosition = New System.Windows.Forms.GroupBox()
        Me.gbDimensions = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.nudResolution = New System.Windows.Forms.NumericUpDown()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.nudCuttingStep = New System.Windows.Forms.NumericUpDown()
        Me.gbOrientation = New System.Windows.Forms.GroupBox()
        Me.nudPsi = New System.Windows.Forms.NumericUpDown()
        Me.nudTita = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.nudPhi = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnInertia = New System.Windows.Forms.Button()
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudFrontL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRearD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudFrontD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTotalL, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRearL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbPosition.SuspendLayout()
        Me.gbDimensions.SuspendLayout()
        CType(Me.nudResolution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCuttingStep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbOrientation.SuspendLayout()
        CType(Me.nudPsi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTita, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPhi, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 42)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(12, 13)
        Me.Label2.TabIndex = 136
        Me.Label2.Text = "Y"
        '
        'lblX
        '
        Me.lblX.AutoSize = True
        Me.lblX.Location = New System.Drawing.Point(13, 21)
        Me.lblX.Name = "lblX"
        Me.lblX.Size = New System.Drawing.Size(13, 13)
        Me.lblX.TabIndex = 135
        Me.lblX.Text = "X"
        '
        'nudY
        '
        Me.nudY.DecimalPlaces = 4
        Me.nudY.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudY.Location = New System.Drawing.Point(37, 40)
        Me.nudY.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudY.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudY.Name = "nudY"
        Me.nudY.Size = New System.Drawing.Size(75, 22)
        Me.nudY.TabIndex = 134
        '
        'nudX
        '
        Me.nudX.DecimalPlaces = 4
        Me.nudX.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudX.Location = New System.Drawing.Point(37, 19)
        Me.nudX.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudX.Name = "nudX"
        Me.nudX.Size = New System.Drawing.Size(75, 22)
        Me.nudX.TabIndex = 133
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 138
        Me.Label1.Text = "Z"
        '
        'nudZ
        '
        Me.nudZ.DecimalPlaces = 4
        Me.nudZ.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudZ.Location = New System.Drawing.Point(37, 61)
        Me.nudZ.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudZ.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudZ.Name = "nudZ"
        Me.nudZ.Size = New System.Drawing.Size(75, 22)
        Me.nudZ.TabIndex = 137
        '
        'btnOK
        '
        Me.btnOK.BackColor = System.Drawing.Color.White
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(264, 244)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 139
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'txtFrontL
        '
        Me.txtFrontL.AutoSize = True
        Me.txtFrontL.Location = New System.Drawing.Point(7, 46)
        Me.txtFrontL.Name = "txtFrontL"
        Me.txtFrontL.Size = New System.Drawing.Size(88, 13)
        Me.txtFrontL.TabIndex = 146
        Me.txtFrontL.Text = "Diffuser length:"
        Me.txtFrontL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudFrontL
        '
        Me.nudFrontL.DecimalPlaces = 4
        Me.nudFrontL.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudFrontL.Location = New System.Drawing.Point(126, 40)
        Me.nudFrontL.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudFrontL.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudFrontL.Name = "nudFrontL"
        Me.nudFrontL.Size = New System.Drawing.Size(75, 22)
        Me.nudFrontL.TabIndex = 145
        '
        'txtRearD
        '
        Me.txtRearD.AutoSize = True
        Me.txtRearD.Location = New System.Drawing.Point(7, 67)
        Me.txtRearD.Name = "txtRearD"
        Me.txtRearD.Size = New System.Drawing.Size(92, 13)
        Me.txtRearD.TabIndex = 144
        Me.txtRearD.Text = "Nozzle diameter:"
        Me.txtRearD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(99, 13)
        Me.Label5.TabIndex = 143
        Me.Label5.Text = "Diffuser diameter:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudRearD
        '
        Me.nudRearD.DecimalPlaces = 4
        Me.nudRearD.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudRearD.Location = New System.Drawing.Point(126, 61)
        Me.nudRearD.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudRearD.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudRearD.Name = "nudRearD"
        Me.nudRearD.Size = New System.Drawing.Size(75, 22)
        Me.nudRearD.TabIndex = 142
        '
        'nudFrontD
        '
        Me.nudFrontD.DecimalPlaces = 4
        Me.nudFrontD.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudFrontD.Location = New System.Drawing.Point(126, 19)
        Me.nudFrontD.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudFrontD.Name = "nudFrontD"
        Me.nudFrontD.Size = New System.Drawing.Size(75, 22)
        Me.nudFrontD.TabIndex = 141
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 150
        Me.Label4.Text = "Total lenght:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 88)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 149
        Me.Label6.Text = "Nozzle lenght:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudTotalL
        '
        Me.nudTotalL.DecimalPlaces = 4
        Me.nudTotalL.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudTotalL.Location = New System.Drawing.Point(126, 103)
        Me.nudTotalL.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudTotalL.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudTotalL.Name = "nudTotalL"
        Me.nudTotalL.Size = New System.Drawing.Size(75, 22)
        Me.nudTotalL.TabIndex = 148
        '
        'nudRearL
        '
        Me.nudRearL.DecimalPlaces = 4
        Me.nudRearL.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudRearL.Location = New System.Drawing.Point(126, 82)
        Me.nudRearL.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudRearL.Name = "nudRearL"
        Me.nudRearL.Size = New System.Drawing.Size(75, 22)
        Me.nudRearL.TabIndex = 147
        '
        'tbxName
        '
        Me.tbxName.Location = New System.Drawing.Point(55, 7)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(286, 22)
        Me.tbxName.TabIndex = 151
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(10, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 152
        Me.Label3.Text = "Name:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'gbPosition
        '
        Me.gbPosition.Controls.Add(Me.nudX)
        Me.gbPosition.Controls.Add(Me.nudY)
        Me.gbPosition.Controls.Add(Me.lblX)
        Me.gbPosition.Controls.Add(Me.Label2)
        Me.gbPosition.Controls.Add(Me.nudZ)
        Me.gbPosition.Controls.Add(Me.Label1)
        Me.gbPosition.Location = New System.Drawing.Point(4, 35)
        Me.gbPosition.Name = "gbPosition"
        Me.gbPosition.Size = New System.Drawing.Size(121, 98)
        Me.gbPosition.TabIndex = 153
        Me.gbPosition.TabStop = False
        Me.gbPosition.Text = "Position"
        '
        'gbDimensions
        '
        Me.gbDimensions.Controls.Add(Me.Label10)
        Me.gbDimensions.Controls.Add(Me.nudResolution)
        Me.gbDimensions.Controls.Add(Me.Label5)
        Me.gbDimensions.Controls.Add(Me.nudFrontD)
        Me.gbDimensions.Controls.Add(Me.nudRearD)
        Me.gbDimensions.Controls.Add(Me.txtRearD)
        Me.gbDimensions.Controls.Add(Me.Label4)
        Me.gbDimensions.Controls.Add(Me.nudFrontL)
        Me.gbDimensions.Controls.Add(Me.Label6)
        Me.gbDimensions.Controls.Add(Me.txtFrontL)
        Me.gbDimensions.Controls.Add(Me.nudTotalL)
        Me.gbDimensions.Controls.Add(Me.nudRearL)
        Me.gbDimensions.Location = New System.Drawing.Point(131, 35)
        Me.gbDimensions.Name = "gbDimensions"
        Me.gbDimensions.Size = New System.Drawing.Size(210, 156)
        Me.gbDimensions.TabIndex = 154
        Me.gbDimensions.TabStop = False
        Me.gbDimensions.Text = "Geometry"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(7, 130)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(66, 13)
        Me.Label10.TabIndex = 152
        Me.Label10.Text = "Resolution:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudResolution
        '
        Me.nudResolution.Location = New System.Drawing.Point(126, 124)
        Me.nudResolution.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudResolution.Name = "nudResolution"
        Me.nudResolution.Size = New System.Drawing.Size(75, 22)
        Me.nudResolution.TabIndex = 151
        Me.nudResolution.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(7, 17)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(74, 13)
        Me.Label11.TabIndex = 154
        Me.Label11.Text = "Cutting step:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudCuttingStep
        '
        Me.nudCuttingStep.Location = New System.Drawing.Point(126, 15)
        Me.nudCuttingStep.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudCuttingStep.Name = "nudCuttingStep"
        Me.nudCuttingStep.Size = New System.Drawing.Size(75, 22)
        Me.nudCuttingStep.TabIndex = 153
        Me.nudCuttingStep.Value = New Decimal(New Integer() {20, 0, 0, 0})
        '
        'gbOrientation
        '
        Me.gbOrientation.Controls.Add(Me.nudPsi)
        Me.gbOrientation.Controls.Add(Me.nudTita)
        Me.gbOrientation.Controls.Add(Me.Label7)
        Me.gbOrientation.Controls.Add(Me.Label8)
        Me.gbOrientation.Controls.Add(Me.nudPhi)
        Me.gbOrientation.Controls.Add(Me.Label9)
        Me.gbOrientation.Location = New System.Drawing.Point(4, 134)
        Me.gbOrientation.Name = "gbOrientation"
        Me.gbOrientation.Size = New System.Drawing.Size(121, 104)
        Me.gbOrientation.TabIndex = 155
        Me.gbOrientation.TabStop = False
        Me.gbOrientation.Text = "Orientation"
        '
        'nudPsi
        '
        Me.nudPsi.DecimalPlaces = 4
        Me.nudPsi.Location = New System.Drawing.Point(37, 20)
        Me.nudPsi.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudPsi.Name = "nudPsi"
        Me.nudPsi.Size = New System.Drawing.Size(75, 22)
        Me.nudPsi.TabIndex = 133
        '
        'nudTita
        '
        Me.nudTita.DecimalPlaces = 4
        Me.nudTita.Location = New System.Drawing.Point(37, 41)
        Me.nudTita.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudTita.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudTita.Name = "nudTita"
        Me.nudTita.Size = New System.Drawing.Size(75, 22)
        Me.nudTita.TabIndex = 134
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label7.Location = New System.Drawing.Point(13, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(14, 13)
        Me.Label7.TabIndex = 135
        Me.Label7.Text = "y"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label8.Location = New System.Drawing.Point(14, 43)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(13, 13)
        Me.Label8.TabIndex = 136
        Me.Label8.Text = "q"
        '
        'nudPhi
        '
        Me.nudPhi.DecimalPlaces = 4
        Me.nudPhi.Location = New System.Drawing.Point(37, 62)
        Me.nudPhi.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudPhi.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudPhi.Name = "nudPhi"
        Me.nudPhi.Size = New System.Drawing.Size(75, 22)
        Me.nudPhi.TabIndex = 137
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label9.Location = New System.Drawing.Point(14, 64)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(12, 13)
        Me.Label9.TabIndex = 138
        Me.Label9.Text = "f"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.nudCuttingStep)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Location = New System.Drawing.Point(131, 193)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(210, 45)
        Me.GroupBox1.TabIndex = 156
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Wake"
        '
        'btnInertia
        '
        Me.btnInertia.BackColor = System.Drawing.Color.White
        Me.btnInertia.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnInertia.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnInertia.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnInertia.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnInertia.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnInertia.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInertia.Location = New System.Drawing.Point(6, 244)
        Me.btnInertia.Name = "btnInertia"
        Me.btnInertia.Size = New System.Drawing.Size(80, 23)
        Me.btnInertia.TabIndex = 157
        Me.btnInertia.Text = "Inertia..."
        Me.btnInertia.UseVisualStyleBackColor = False
        '
        'FormJetEngine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(347, 274)
        Me.Controls.Add(Me.btnInertia)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbOrientation)
        Me.Controls.Add(Me.gbDimensions)
        Me.Controls.Add(Me.gbPosition)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbxName)
        Me.Controls.Add(Me.btnOK)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormJetEngine"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Jet engine editor"
        CType(Me.nudY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudFrontL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRearD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudFrontD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTotalL, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRearL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbPosition.ResumeLayout(False)
        Me.gbPosition.PerformLayout()
        Me.gbDimensions.ResumeLayout(False)
        Me.gbDimensions.PerformLayout()
        CType(Me.nudResolution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCuttingStep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbOrientation.ResumeLayout(False)
        Me.gbOrientation.PerformLayout()
        CType(Me.nudPsi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTita, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPhi, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblX As System.Windows.Forms.Label
    Friend WithEvents nudY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudX As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtFrontL As System.Windows.Forms.Label
    Friend WithEvents nudFrontL As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtRearD As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents nudRearD As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudFrontD As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents nudTotalL As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudRearL As System.Windows.Forms.NumericUpDown
    Friend WithEvents tbxName As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents gbPosition As System.Windows.Forms.GroupBox
    Friend WithEvents gbDimensions As System.Windows.Forms.GroupBox
    Friend WithEvents gbOrientation As System.Windows.Forms.GroupBox
    Friend WithEvents nudPsi As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudTita As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents nudPhi As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents nudCuttingStep As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents nudResolution As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents btnInertia As Button
End Class

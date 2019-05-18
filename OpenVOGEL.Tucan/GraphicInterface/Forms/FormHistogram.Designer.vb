<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormHistogram
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
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblHiperdampingSpan = New System.Windows.Forms.Label()
        Me.nudGustSpan = New System.Windows.Forms.NumericUpDown()
        Me.nudGustX = New System.Windows.Forms.NumericUpDown()
        Me.gbDamping = New System.Windows.Forms.GroupBox()
        Me.nudHyperdampingSpan = New System.Windows.Forms.NumericUpDown()
        Me.nudHyperDamping = New System.Windows.Forms.NumericUpDown()
        Me.nudNormalDamping = New System.Windows.Forms.NumericUpDown()
        Me.lblNormalDamping = New System.Windows.Forms.Label()
        Me.lblHyperDamping = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblGustSpan = New System.Windows.Forms.Label()
        Me.gbGust = New System.Windows.Forms.GroupBox()
        Me.lblGustZ = New System.Windows.Forms.Label()
        Me.lblGustY = New System.Windows.Forms.Label()
        Me.lblGustX = New System.Windows.Forms.Label()
        Me.nudGustY = New System.Windows.Forms.NumericUpDown()
        Me.nudGustZ = New System.Windows.Forms.NumericUpDown()
        Me.pbPLot = New System.Windows.Forms.PictureBox()
        CType(Me.nudGustSpan, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudGustX, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDamping.SuspendLayout()
        CType(Me.nudHyperdampingSpan, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHyperDamping, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNormalDamping, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbGust.SuspendLayout()
        CType(Me.nudGustY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudGustZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPLot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.White
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(465, 236)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 23)
        Me.btnCancel.TabIndex = 114
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
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
        Me.btnOK.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(548, 236)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 113
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(182, 75)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(12, 13)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "s"
        '
        'lblHiperdampingSpan
        '
        Me.lblHiperdampingSpan.AutoSize = True
        Me.lblHiperdampingSpan.Location = New System.Drawing.Point(8, 75)
        Me.lblHiperdampingSpan.Name = "lblHiperdampingSpan"
        Me.lblHiperdampingSpan.Size = New System.Drawing.Size(61, 13)
        Me.lblHiperdampingSpan.TabIndex = 8
        Me.lblHiperdampingSpan.Text = "Time span:"
        '
        'nudGustSpan
        '
        Me.nudGustSpan.Location = New System.Drawing.Point(89, 16)
        Me.nudGustSpan.Name = "nudGustSpan"
        Me.nudGustSpan.Size = New System.Drawing.Size(87, 22)
        Me.nudGustSpan.TabIndex = 2
        '
        'nudGustX
        '
        Me.nudGustX.Location = New System.Drawing.Point(89, 42)
        Me.nudGustX.Name = "nudGustX"
        Me.nudGustX.Size = New System.Drawing.Size(87, 22)
        Me.nudGustX.TabIndex = 3
        '
        'gbDamping
        '
        Me.gbDamping.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbDamping.Controls.Add(Me.Label1)
        Me.gbDamping.Controls.Add(Me.nudHyperdampingSpan)
        Me.gbDamping.Controls.Add(Me.lblHiperdampingSpan)
        Me.gbDamping.Controls.Add(Me.nudHyperDamping)
        Me.gbDamping.Controls.Add(Me.nudNormalDamping)
        Me.gbDamping.Controls.Add(Me.lblNormalDamping)
        Me.gbDamping.Controls.Add(Me.lblHyperDamping)
        Me.gbDamping.Location = New System.Drawing.Point(406, 4)
        Me.gbDamping.Name = "gbDamping"
        Me.gbDamping.Size = New System.Drawing.Size(219, 105)
        Me.gbDamping.TabIndex = 117
        Me.gbDamping.TabStop = False
        Me.gbDamping.Text = "Damping"
        '
        'nudHyperdampingSpan
        '
        Me.nudHyperdampingSpan.Location = New System.Drawing.Point(104, 73)
        Me.nudHyperdampingSpan.Name = "nudHyperdampingSpan"
        Me.nudHyperdampingSpan.Size = New System.Drawing.Size(72, 22)
        Me.nudHyperdampingSpan.TabIndex = 7
        '
        'nudHyperDamping
        '
        Me.nudHyperDamping.Location = New System.Drawing.Point(104, 21)
        Me.nudHyperDamping.Name = "nudHyperDamping"
        Me.nudHyperDamping.Size = New System.Drawing.Size(72, 22)
        Me.nudHyperDamping.TabIndex = 0
        '
        'nudNormalDamping
        '
        Me.nudNormalDamping.Location = New System.Drawing.Point(104, 47)
        Me.nudNormalDamping.Name = "nudNormalDamping"
        Me.nudNormalDamping.Size = New System.Drawing.Size(72, 22)
        Me.nudNormalDamping.TabIndex = 1
        '
        'lblNormalDamping
        '
        Me.lblNormalDamping.AutoSize = True
        Me.lblNormalDamping.Location = New System.Drawing.Point(8, 49)
        Me.lblNormalDamping.Name = "lblNormalDamping"
        Me.lblNormalDamping.Size = New System.Drawing.Size(57, 13)
        Me.lblNormalDamping.TabIndex = 6
        Me.lblNormalDamping.Text = "Damping:"
        '
        'lblHyperDamping
        '
        Me.lblHyperDamping.AutoSize = True
        Me.lblHyperDamping.Location = New System.Drawing.Point(8, 23)
        Me.lblHyperDamping.Name = "lblHyperDamping"
        Me.lblHyperDamping.Size = New System.Drawing.Size(86, 13)
        Me.lblHyperDamping.TabIndex = 5
        Me.lblHyperDamping.Text = "Hyperdamping:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(182, 86)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 13)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "m/s"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(182, 65)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(25, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "m/s"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(182, 44)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(25, 13)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "m/s"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(182, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(12, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "s"
        '
        'lblGustSpan
        '
        Me.lblGustSpan.AutoSize = True
        Me.lblGustSpan.Location = New System.Drawing.Point(8, 18)
        Me.lblGustSpan.Name = "lblGustSpan"
        Me.lblGustSpan.Size = New System.Drawing.Size(75, 13)
        Me.lblGustSpan.TabIndex = 7
        Me.lblGustSpan.Text = "Gust interval:"
        '
        'gbGust
        '
        Me.gbGust.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbGust.Controls.Add(Me.Label5)
        Me.gbGust.Controls.Add(Me.Label4)
        Me.gbGust.Controls.Add(Me.Label3)
        Me.gbGust.Controls.Add(Me.Label2)
        Me.gbGust.Controls.Add(Me.nudGustSpan)
        Me.gbGust.Controls.Add(Me.lblGustZ)
        Me.gbGust.Controls.Add(Me.nudGustX)
        Me.gbGust.Controls.Add(Me.lblGustY)
        Me.gbGust.Controls.Add(Me.lblGustSpan)
        Me.gbGust.Controls.Add(Me.lblGustX)
        Me.gbGust.Controls.Add(Me.nudGustY)
        Me.gbGust.Controls.Add(Me.nudGustZ)
        Me.gbGust.Location = New System.Drawing.Point(406, 109)
        Me.gbGust.Name = "gbGust"
        Me.gbGust.Size = New System.Drawing.Size(219, 114)
        Me.gbGust.TabIndex = 116
        Me.gbGust.TabStop = False
        Me.gbGust.Text = "Gust"
        '
        'lblGustZ
        '
        Me.lblGustZ.AutoSize = True
        Me.lblGustZ.Location = New System.Drawing.Point(59, 86)
        Me.lblGustZ.Name = "lblGustZ"
        Me.lblGustZ.Size = New System.Drawing.Size(22, 13)
        Me.lblGustZ.TabIndex = 12
        Me.lblGustZ.Text = "Vz:"
        '
        'lblGustY
        '
        Me.lblGustY.AutoSize = True
        Me.lblGustY.Location = New System.Drawing.Point(59, 65)
        Me.lblGustY.Name = "lblGustY"
        Me.lblGustY.Size = New System.Drawing.Size(22, 13)
        Me.lblGustY.TabIndex = 11
        Me.lblGustY.Text = "Vy:"
        '
        'lblGustX
        '
        Me.lblGustX.AutoSize = True
        Me.lblGustX.Location = New System.Drawing.Point(59, 44)
        Me.lblGustX.Name = "lblGustX"
        Me.lblGustX.Size = New System.Drawing.Size(22, 13)
        Me.lblGustX.TabIndex = 10
        Me.lblGustX.Text = "Vx:"
        '
        'nudGustY
        '
        Me.nudGustY.Location = New System.Drawing.Point(89, 63)
        Me.nudGustY.Name = "nudGustY"
        Me.nudGustY.Size = New System.Drawing.Size(87, 22)
        Me.nudGustY.TabIndex = 8
        '
        'nudGustZ
        '
        Me.nudGustZ.Location = New System.Drawing.Point(89, 84)
        Me.nudGustZ.Name = "nudGustZ"
        Me.nudGustZ.Size = New System.Drawing.Size(87, 22)
        Me.nudGustZ.TabIndex = 9
        '
        'pbPLot
        '
        Me.pbPLot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPLot.BackColor = System.Drawing.Color.White
        Me.pbPLot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbPLot.Location = New System.Drawing.Point(4, 4)
        Me.pbPLot.Name = "pbPLot"
        Me.pbPLot.Size = New System.Drawing.Size(396, 255)
        Me.pbPLot.TabIndex = 115
        Me.pbPLot.TabStop = False
        '
        'FormHistogram
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(630, 264)
        Me.Controls.Add(Me.gbDamping)
        Me.Controls.Add(Me.gbGust)
        Me.Controls.Add(Me.pbPLot)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormHistogram"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Aeroelastic histogram"
        CType(Me.nudGustSpan, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudGustX, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDamping.ResumeLayout(False)
        Me.gbDamping.PerformLayout()
        CType(Me.nudHyperdampingSpan, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHyperDamping, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNormalDamping, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbGust.ResumeLayout(False)
        Me.gbGust.PerformLayout()
        CType(Me.nudGustY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudGustZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPLot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents lblHiperdampingSpan As Windows.Forms.Label
    Friend WithEvents nudGustSpan As Windows.Forms.NumericUpDown
    Friend WithEvents nudGustX As Windows.Forms.NumericUpDown
    Friend WithEvents gbDamping As Windows.Forms.GroupBox
    Friend WithEvents nudHyperdampingSpan As Windows.Forms.NumericUpDown
    Friend WithEvents nudHyperDamping As Windows.Forms.NumericUpDown
    Friend WithEvents nudNormalDamping As Windows.Forms.NumericUpDown
    Friend WithEvents lblNormalDamping As Windows.Forms.Label
    Friend WithEvents lblHyperDamping As Windows.Forms.Label
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents lblGustSpan As Windows.Forms.Label
    Friend WithEvents gbGust As Windows.Forms.GroupBox
    Friend WithEvents lblGustZ As Windows.Forms.Label
    Friend WithEvents lblGustY As Windows.Forms.Label
    Friend WithEvents lblGustX As Windows.Forms.Label
    Friend WithEvents nudGustY As Windows.Forms.NumericUpDown
    Friend WithEvents nudGustZ As Windows.Forms.NumericUpDown
    Friend WithEvents pbPLot As Windows.Forms.PictureBox
End Class

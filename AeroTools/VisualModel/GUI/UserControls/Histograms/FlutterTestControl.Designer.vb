<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FlutterTestControl
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
        Me.nudHyperDamping = New System.Windows.Forms.NumericUpDown()
        Me.nudNormalDamping = New System.Windows.Forms.NumericUpDown()
        Me.nudGustSpan = New System.Windows.Forms.NumericUpDown()
        Me.nudGustX = New System.Windows.Forms.NumericUpDown()
        Me.pbPLot = New System.Windows.Forms.PictureBox()
        Me.lblHyperDamping = New System.Windows.Forms.Label()
        Me.lblNormalDamping = New System.Windows.Forms.Label()
        Me.lblGustSpan = New System.Windows.Forms.Label()
        Me.nudGustY = New System.Windows.Forms.NumericUpDown()
        Me.nudGustZ = New System.Windows.Forms.NumericUpDown()
        Me.lblGustX = New System.Windows.Forms.Label()
        Me.lblGustY = New System.Windows.Forms.Label()
        Me.lblGustZ = New System.Windows.Forms.Label()
        Me.gbGust = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.gbDamping = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudHyperdampingSpan = New System.Windows.Forms.NumericUpDown()
        Me.lblHiperdampingSpan = New System.Windows.Forms.Label()
        CType(Me.nudHyperDamping, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudNormalDamping, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudGustSpan, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudGustX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPLot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudGustY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudGustZ, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbGust.SuspendLayout()
        Me.gbDamping.SuspendLayout()
        CType(Me.nudHyperdampingSpan, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        'pbPLot
        '
        Me.pbPLot.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbPLot.BackColor = System.Drawing.Color.White
        Me.pbPLot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbPLot.Location = New System.Drawing.Point(3, 3)
        Me.pbPLot.Name = "pbPLot"
        Me.pbPLot.Size = New System.Drawing.Size(394, 333)
        Me.pbPLot.TabIndex = 4
        Me.pbPLot.TabStop = False
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
        'lblNormalDamping
        '
        Me.lblNormalDamping.AutoSize = True
        Me.lblNormalDamping.Location = New System.Drawing.Point(8, 49)
        Me.lblNormalDamping.Name = "lblNormalDamping"
        Me.lblNormalDamping.Size = New System.Drawing.Size(57, 13)
        Me.lblNormalDamping.TabIndex = 6
        Me.lblNormalDamping.Text = "Damping:"
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
        'lblGustX
        '
        Me.lblGustX.AutoSize = True
        Me.lblGustX.Location = New System.Drawing.Point(59, 44)
        Me.lblGustX.Name = "lblGustX"
        Me.lblGustX.Size = New System.Drawing.Size(22, 13)
        Me.lblGustX.TabIndex = 10
        Me.lblGustX.Text = "Vx:"
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
        'lblGustZ
        '
        Me.lblGustZ.AutoSize = True
        Me.lblGustZ.Location = New System.Drawing.Point(59, 86)
        Me.lblGustZ.Name = "lblGustZ"
        Me.lblGustZ.Size = New System.Drawing.Size(22, 13)
        Me.lblGustZ.TabIndex = 12
        Me.lblGustZ.Text = "Vz:"
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
        Me.gbGust.Location = New System.Drawing.Point(403, 114)
        Me.gbGust.Name = "gbGust"
        Me.gbGust.Size = New System.Drawing.Size(219, 114)
        Me.gbGust.TabIndex = 13
        Me.gbGust.TabStop = False
        Me.gbGust.Text = "Gust"
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
        Me.gbDamping.Location = New System.Drawing.Point(403, 3)
        Me.gbDamping.Name = "gbDamping"
        Me.gbDamping.Size = New System.Drawing.Size(219, 105)
        Me.gbDamping.TabIndex = 14
        Me.gbDamping.TabStop = False
        Me.gbDamping.Text = "Damping"
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
        'nudHyperdampingSpan
        '
        Me.nudHyperdampingSpan.Location = New System.Drawing.Point(104, 73)
        Me.nudHyperdampingSpan.Name = "nudHyperdampingSpan"
        Me.nudHyperdampingSpan.Size = New System.Drawing.Size(72, 22)
        Me.nudHyperdampingSpan.TabIndex = 7
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
        'FlutterTestControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.gbDamping)
        Me.Controls.Add(Me.gbGust)
        Me.Controls.Add(Me.pbPLot)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "FlutterTestControl"
        Me.Size = New System.Drawing.Size(627, 339)
        CType(Me.nudHyperDamping, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudNormalDamping, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudGustSpan, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudGustX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPLot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudGustY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudGustZ, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbGust.ResumeLayout(False)
        Me.gbGust.PerformLayout()
        Me.gbDamping.ResumeLayout(False)
        Me.gbDamping.PerformLayout()
        CType(Me.nudHyperdampingSpan, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents nudHyperDamping As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudNormalDamping As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudGustSpan As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudGustX As System.Windows.Forms.NumericUpDown
    Friend WithEvents pbPLot As System.Windows.Forms.PictureBox
    Friend WithEvents lblHyperDamping As System.Windows.Forms.Label
    Friend WithEvents lblNormalDamping As System.Windows.Forms.Label
    Friend WithEvents lblGustSpan As System.Windows.Forms.Label
    Friend WithEvents nudGustY As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudGustZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblGustX As System.Windows.Forms.Label
    Friend WithEvents lblGustY As System.Windows.Forms.Label
    Friend WithEvents lblGustZ As System.Windows.Forms.Label
    Friend WithEvents gbGust As System.Windows.Forms.GroupBox
    Friend WithEvents gbDamping As System.Windows.Forms.GroupBox
    Friend WithEvents nudHyperdampingSpan As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblHiperdampingSpan As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class QuadraticPolarControl
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
        Me.tbPolarName = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudReynolds = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nudB = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudA = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudCD0 = New System.Windows.Forms.NumericUpDown()
        CType(Me.nudReynolds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCD0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tbPolarName
        '
        Me.tbPolarName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPolarName.Location = New System.Drawing.Point(3, 3)
        Me.tbPolarName.Name = "tbPolarName"
        Me.tbPolarName.Size = New System.Drawing.Size(163, 20)
        Me.tbPolarName.TabIndex = 18
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(34, 124)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(24, 13)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Re:"
        '
        'nudReynolds
        '
        Me.nudReynolds.Increment = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudReynolds.Location = New System.Drawing.Point(64, 122)
        Me.nudReynolds.Maximum = New Decimal(New Integer() {100000000, 0, 0, 0})
        Me.nudReynolds.Name = "nudReynolds"
        Me.nudReynolds.Size = New System.Drawing.Size(86, 20)
        Me.nudReynolds.TabIndex = 16
        Me.nudReynolds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudReynolds.Value = New Decimal(New Integer() {200000, 0, 0, 0})
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(128, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "CD = CDo + A.CL + B.CL²"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(35, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(23, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "B ="
        '
        'nudB
        '
        Me.nudB.DecimalPlaces = 4
        Me.nudB.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudB.Location = New System.Drawing.Point(64, 96)
        Me.nudB.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
        Me.nudB.Name = "nudB"
        Me.nudB.Size = New System.Drawing.Size(61, 20)
        Me.nudB.TabIndex = 6
        Me.nudB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(35, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(23, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "A ="
        '
        'nudA
        '
        Me.nudA.DecimalPlaces = 4
        Me.nudA.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudA.Location = New System.Drawing.Point(64, 75)
        Me.nudA.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
        Me.nudA.Minimum = New Decimal(New Integer() {9, 0, 0, -2147483648})
        Me.nudA.Name = "nudA"
        Me.nudA.Size = New System.Drawing.Size(61, 20)
        Me.nudA.TabIndex = 4
        Me.nudA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(21, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "CDo ="
        '
        'nudCD0
        '
        Me.nudCD0.DecimalPlaces = 5
        Me.nudCD0.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.nudCD0.Location = New System.Drawing.Point(64, 54)
        Me.nudCD0.Maximum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudCD0.Name = "nudCD0"
        Me.nudCD0.Size = New System.Drawing.Size(61, 20)
        Me.nudCD0.TabIndex = 0
        Me.nudCD0.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'QuadraticPolarControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tbPolarName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.nudReynolds)
        Me.Controls.Add(Me.nudCD0)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.nudA)
        Me.Controls.Add(Me.nudB)
        Me.Controls.Add(Me.Label2)
        Me.Name = "QuadraticPolarControl"
        Me.Size = New System.Drawing.Size(170, 182)
        CType(Me.nudReynolds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCD0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents nudB As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudA As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudCD0 As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tbPolarName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents nudReynolds As System.Windows.Forms.NumericUpDown

End Class

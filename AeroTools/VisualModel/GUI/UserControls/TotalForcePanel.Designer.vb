<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TotalForcePanel
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
        Me.components = New System.ComponentModel.Container()
        Me.gbReferencePoint = New System.Windows.Forms.GroupBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nudRz = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudRy = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudRx = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.nudSurface = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.nudLength = New System.Windows.Forms.NumericUpDown()
        Me.cbDimensionless = New System.Windows.Forms.CheckBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.gbReferencePoint.SuspendLayout()
        CType(Me.nudRz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSurface, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLength, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbReferencePoint
        '
        Me.gbReferencePoint.Controls.Add(Me.Label6)
        Me.gbReferencePoint.Controls.Add(Me.Label5)
        Me.gbReferencePoint.Controls.Add(Me.Label4)
        Me.gbReferencePoint.Controls.Add(Me.Label3)
        Me.gbReferencePoint.Controls.Add(Me.nudRz)
        Me.gbReferencePoint.Controls.Add(Me.Label2)
        Me.gbReferencePoint.Controls.Add(Me.nudRy)
        Me.gbReferencePoint.Controls.Add(Me.Label1)
        Me.gbReferencePoint.Controls.Add(Me.nudRx)
        Me.gbReferencePoint.Location = New System.Drawing.Point(3, 3)
        Me.gbReferencePoint.Name = "gbReferencePoint"
        Me.gbReferencePoint.Size = New System.Drawing.Size(144, 100)
        Me.gbReferencePoint.TabIndex = 0
        Me.gbReferencePoint.TabStop = False
        Me.gbReferencePoint.Text = "Reference point"
        Me.ToolTip.SetToolTip(Me.gbReferencePoint, "This point is used as reference to compute the moments." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "When this point is locat" &
        "ed at the CG, mass forces" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "don't need to be included in the analysis.")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(106, 69)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 13)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "[unit]"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(106, 46)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "[unit]"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(106, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "[unit]"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 69)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Z"
        '
        'nudRz
        '
        Me.nudRz.DecimalPlaces = 4
        Me.nudRz.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudRz.Location = New System.Drawing.Point(25, 67)
        Me.nudRz.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudRz.Name = "nudRz"
        Me.nudRz.Size = New System.Drawing.Size(75, 22)
        Me.nudRz.TabIndex = 5
        Me.nudRz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(12, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Y"
        '
        'nudRy
        '
        Me.nudRy.DecimalPlaces = 4
        Me.nudRy.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudRy.Location = New System.Drawing.Point(25, 44)
        Me.nudRy.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudRy.Name = "nudRy"
        Me.nudRy.Size = New System.Drawing.Size(75, 22)
        Me.nudRy.TabIndex = 3
        Me.nudRy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(13, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "X"
        '
        'nudRx
        '
        Me.nudRx.DecimalPlaces = 4
        Me.nudRx.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudRx.Location = New System.Drawing.Point(25, 21)
        Me.nudRx.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudRx.Name = "nudRx"
        Me.nudRx.Size = New System.Drawing.Size(75, 22)
        Me.nudRx.TabIndex = 1
        Me.nudRx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(253, 49)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(34, 13)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "[unit]"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(150, 49)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(13, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "S"
        '
        'nudSurface
        '
        Me.nudSurface.DecimalPlaces = 4
        Me.nudSurface.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudSurface.Location = New System.Drawing.Point(169, 47)
        Me.nudSurface.Name = "nudSurface"
        Me.nudSurface.Size = New System.Drawing.Size(75, 22)
        Me.nudSurface.TabIndex = 8
        Me.nudSurface.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip.SetToolTip(Me.nudSurface, "Select the value of the reference area")
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(253, 72)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(34, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "[unit]"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(150, 72)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(12, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "L"
        '
        'nudLength
        '
        Me.nudLength.DecimalPlaces = 4
        Me.nudLength.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudLength.Location = New System.Drawing.Point(169, 70)
        Me.nudLength.Name = "nudLength"
        Me.nudLength.Size = New System.Drawing.Size(75, 22)
        Me.nudLength.TabIndex = 11
        Me.nudLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip.SetToolTip(Me.nudLength, "Select the value of the reference length")
        '
        'cbDimensionless
        '
        Me.cbDimensionless.AutoSize = True
        Me.cbDimensionless.Location = New System.Drawing.Point(153, 24)
        Me.cbDimensionless.Name = "cbDimensionless"
        Me.cbDimensionless.Size = New System.Drawing.Size(100, 17)
        Me.cbDimensionless.TabIndex = 13
        Me.cbDimensionless.Text = "Dimensionless"
        Me.ToolTip.SetToolTip(Me.cbDimensionless, "Represent the data in magnitudes or " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "in dimensionless coefficients")
        Me.cbDimensionless.UseVisualStyleBackColor = True
        '
        'TotalForcePanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.cbDimensionless)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.nudLength)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.nudSurface)
        Me.Controls.Add(Me.gbReferencePoint)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "TotalForcePanel"
        Me.Size = New System.Drawing.Size(616, 371)
        Me.gbReferencePoint.ResumeLayout(False)
        Me.gbReferencePoint.PerformLayout()
        CType(Me.nudRz, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSurface, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLength, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents gbReferencePoint As Windows.Forms.GroupBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents nudRz As Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents nudRy As Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents nudRx As Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents nudSurface As Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As Windows.Forms.Label
    Friend WithEvents Label10 As Windows.Forms.Label
    Friend WithEvents nudLength As Windows.Forms.NumericUpDown
    Friend WithEvents cbDimensionless As Windows.Forms.CheckBox
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents ToolTip As Windows.Forms.ToolTip
End Class

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
        Me.lblRzUnit = New System.Windows.Forms.Label()
        Me.lblRyUnit = New System.Windows.Forms.Label()
        Me.lblRxUnit = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nudRz = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudRy = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.nudRx = New System.Windows.Forms.NumericUpDown()
        Me.lblSurfaceUnit = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.nudSurface = New System.Windows.Forms.NumericUpDown()
        Me.lblLengthUnit = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.nudLength = New System.Windows.Forms.NumericUpDown()
        Me.cbDimensionless = New System.Windows.Forms.CheckBox()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.cbSlenderForces = New System.Windows.Forms.CheckBox()
        Me.cbInducedForces = New System.Windows.Forms.CheckBox()
        Me.cbSkinDrag = New System.Windows.Forms.CheckBox()
        Me.cbBodyForces = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.rbAeroCoordinates = New System.Windows.Forms.RadioButton()
        Me.rbBodyCoordinates = New System.Windows.Forms.RadioButton()
        Me.gbReferencePoint.SuspendLayout()
        CType(Me.nudRz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSurface, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLength, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbReferencePoint
        '
        Me.gbReferencePoint.Controls.Add(Me.lblRzUnit)
        Me.gbReferencePoint.Controls.Add(Me.lblRyUnit)
        Me.gbReferencePoint.Controls.Add(Me.lblRxUnit)
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
        'lblRzUnit
        '
        Me.lblRzUnit.AutoSize = True
        Me.lblRzUnit.Location = New System.Drawing.Point(106, 69)
        Me.lblRzUnit.Name = "lblRzUnit"
        Me.lblRzUnit.Size = New System.Drawing.Size(34, 13)
        Me.lblRzUnit.TabIndex = 12
        Me.lblRzUnit.Text = "[unit]"
        '
        'lblRyUnit
        '
        Me.lblRyUnit.AutoSize = True
        Me.lblRyUnit.Location = New System.Drawing.Point(106, 46)
        Me.lblRyUnit.Name = "lblRyUnit"
        Me.lblRyUnit.Size = New System.Drawing.Size(34, 13)
        Me.lblRyUnit.TabIndex = 11
        Me.lblRyUnit.Text = "[unit]"
        '
        'lblRxUnit
        '
        Me.lblRxUnit.AutoSize = True
        Me.lblRxUnit.Location = New System.Drawing.Point(106, 23)
        Me.lblRxUnit.Name = "lblRxUnit"
        Me.lblRxUnit.Size = New System.Drawing.Size(34, 13)
        Me.lblRxUnit.TabIndex = 10
        Me.lblRxUnit.Text = "[unit]"
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
        'lblSurfaceUnit
        '
        Me.lblSurfaceUnit.AutoSize = True
        Me.lblSurfaceUnit.Location = New System.Drawing.Point(253, 34)
        Me.lblSurfaceUnit.Name = "lblSurfaceUnit"
        Me.lblSurfaceUnit.Size = New System.Drawing.Size(34, 13)
        Me.lblSurfaceUnit.TabIndex = 9
        Me.lblSurfaceUnit.Text = "[unit]"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(150, 34)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(13, 13)
        Me.Label8.TabIndex = 7
        Me.Label8.Text = "S"
        '
        'nudSurface
        '
        Me.nudSurface.DecimalPlaces = 4
        Me.nudSurface.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudSurface.Location = New System.Drawing.Point(169, 32)
        Me.nudSurface.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudSurface.Name = "nudSurface"
        Me.nudSurface.Size = New System.Drawing.Size(75, 22)
        Me.nudSurface.TabIndex = 8
        Me.nudSurface.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip.SetToolTip(Me.nudSurface, "Select the value of the characteristic area")
        Me.nudSurface.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblLengthUnit
        '
        Me.lblLengthUnit.AutoSize = True
        Me.lblLengthUnit.Location = New System.Drawing.Point(253, 57)
        Me.lblLengthUnit.Name = "lblLengthUnit"
        Me.lblLengthUnit.Size = New System.Drawing.Size(34, 13)
        Me.lblLengthUnit.TabIndex = 12
        Me.lblLengthUnit.Text = "[unit]"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(150, 57)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(12, 13)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "L"
        '
        'nudLength
        '
        Me.nudLength.DecimalPlaces = 4
        Me.nudLength.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudLength.Location = New System.Drawing.Point(169, 55)
        Me.nudLength.Name = "nudLength"
        Me.nudLength.Size = New System.Drawing.Size(75, 22)
        Me.nudLength.TabIndex = 11
        Me.nudLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip.SetToolTip(Me.nudLength, "Select the value of the characteristic length")
        Me.nudLength.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cbDimensionless
        '
        Me.cbDimensionless.AutoSize = True
        Me.cbDimensionless.Location = New System.Drawing.Point(153, 9)
        Me.cbDimensionless.Name = "cbDimensionless"
        Me.cbDimensionless.Size = New System.Drawing.Size(100, 17)
        Me.cbDimensionless.TabIndex = 13
        Me.cbDimensionless.Text = "Dimensionless"
        Me.ToolTip.SetToolTip(Me.cbDimensionless, "Represent the data in magnitudes or " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "in dimensionless coefficients")
        Me.cbDimensionless.UseVisualStyleBackColor = True
        '
        'cbSlenderForces
        '
        Me.cbSlenderForces.AutoSize = True
        Me.cbSlenderForces.Checked = True
        Me.cbSlenderForces.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSlenderForces.Location = New System.Drawing.Point(293, 9)
        Me.cbSlenderForces.Name = "cbSlenderForces"
        Me.cbSlenderForces.Size = New System.Drawing.Size(99, 17)
        Me.cbSlenderForces.TabIndex = 14
        Me.cbSlenderForces.Text = "Slender forces"
        Me.ToolTip.SetToolTip(Me.cbSlenderForces, "Indicate if the primary lift forces " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "on slender surfaces have to be included")
        Me.cbSlenderForces.UseVisualStyleBackColor = True
        '
        'cbInducedForces
        '
        Me.cbInducedForces.AutoSize = True
        Me.cbInducedForces.Checked = True
        Me.cbInducedForces.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbInducedForces.Location = New System.Drawing.Point(293, 30)
        Me.cbInducedForces.Name = "cbInducedForces"
        Me.cbInducedForces.Size = New System.Drawing.Size(102, 17)
        Me.cbInducedForces.TabIndex = 15
        Me.cbInducedForces.Text = "Induced forces"
        Me.ToolTip.SetToolTip(Me.cbInducedForces, "Indicate if the induced drag on slender surfaces" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "have to be included")
        Me.cbInducedForces.UseVisualStyleBackColor = True
        '
        'cbSkinDrag
        '
        Me.cbSkinDrag.AutoSize = True
        Me.cbSkinDrag.Checked = True
        Me.cbSkinDrag.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbSkinDrag.Location = New System.Drawing.Point(293, 50)
        Me.cbSkinDrag.Name = "cbSkinDrag"
        Me.cbSkinDrag.Size = New System.Drawing.Size(109, 17)
        Me.cbSkinDrag.TabIndex = 16
        Me.cbSkinDrag.Text = "Skin drag forces"
        Me.ToolTip.SetToolTip(Me.cbSkinDrag, "Indicate if the friction drag on slender surfaces" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "have to be included")
        Me.cbSkinDrag.UseVisualStyleBackColor = True
        '
        'cbBodyForces
        '
        Me.cbBodyForces.AutoSize = True
        Me.cbBodyForces.Checked = True
        Me.cbBodyForces.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbBodyForces.Location = New System.Drawing.Point(293, 70)
        Me.cbBodyForces.Name = "cbBodyForces"
        Me.cbBodyForces.Size = New System.Drawing.Size(85, 17)
        Me.cbBodyForces.TabIndex = 17
        Me.cbBodyForces.Text = "Body forces"
        Me.ToolTip.SetToolTip(Me.cbBodyForces, "Indicate if the resultant of the pressure over fuselages" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "have to be included")
        Me.cbBodyForces.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbAeroCoordinates)
        Me.GroupBox1.Controls.Add(Me.rbBodyCoordinates)
        Me.GroupBox1.Location = New System.Drawing.Point(408, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(130, 100)
        Me.GroupBox1.TabIndex = 21
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Coordinates system"
        Me.ToolTip.SetToolTip(Me.GroupBox1, "Select a coordinate system to project the components" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of the total aerodynamic fo" &
        "rce")
        '
        'rbAeroCoordinates
        '
        Me.rbAeroCoordinates.AutoSize = True
        Me.rbAeroCoordinates.Checked = True
        Me.rbAeroCoordinates.Location = New System.Drawing.Point(9, 21)
        Me.rbAeroCoordinates.Name = "rbAeroCoordinates"
        Me.rbAeroCoordinates.Size = New System.Drawing.Size(91, 17)
        Me.rbAeroCoordinates.TabIndex = 20
        Me.rbAeroCoordinates.TabStop = True
        Me.rbAeroCoordinates.Text = "Aerodynamic"
        Me.rbAeroCoordinates.UseVisualStyleBackColor = True
        '
        'rbBodyCoordinates
        '
        Me.rbBodyCoordinates.AutoSize = True
        Me.rbBodyCoordinates.Location = New System.Drawing.Point(9, 42)
        Me.rbBodyCoordinates.Name = "rbBodyCoordinates"
        Me.rbBodyCoordinates.Size = New System.Drawing.Size(76, 17)
        Me.rbBodyCoordinates.TabIndex = 19
        Me.rbBodyCoordinates.Text = "Body (XYZ)"
        Me.rbBodyCoordinates.UseVisualStyleBackColor = True
        '
        'TotalForcePanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cbBodyForces)
        Me.Controls.Add(Me.cbSkinDrag)
        Me.Controls.Add(Me.cbInducedForces)
        Me.Controls.Add(Me.cbSlenderForces)
        Me.Controls.Add(Me.cbDimensionless)
        Me.Controls.Add(Me.lblLengthUnit)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.nudLength)
        Me.Controls.Add(Me.lblSurfaceUnit)
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
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents gbReferencePoint As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents nudRz As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudRy As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents nudRx As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblSurfaceUnit As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents nudSurface As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblLengthUnit As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents nudLength As System.Windows.Forms.NumericUpDown
    Friend WithEvents cbDimensionless As System.Windows.Forms.CheckBox
    Friend WithEvents lblRzUnit As System.Windows.Forms.Label
    Friend WithEvents lblRyUnit As System.Windows.Forms.Label
    Friend WithEvents lblRxUnit As System.Windows.Forms.Label
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents cbSlenderForces As System.Windows.Forms.CheckBox
    Friend WithEvents cbInducedForces As System.Windows.Forms.CheckBox
    Friend WithEvents cbSkinDrag As System.Windows.Forms.CheckBox
    Friend WithEvents cbBodyForces As System.Windows.Forms.CheckBox
    Friend WithEvents rbBodyCoordinates As System.Windows.Forms.RadioButton
    Friend WithEvents rbAeroCoordinates As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class

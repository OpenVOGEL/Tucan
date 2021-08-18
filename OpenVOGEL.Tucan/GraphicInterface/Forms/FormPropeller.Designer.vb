<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPropeller
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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.tbxName = New System.Windows.Forms.TextBox()
        Me.gbDimensions = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudDiameter = New System.Windows.Forms.NumericUpDown()
        Me.nudCentral = New System.Windows.Forms.NumericUpDown()
        Me.txtRearD = New System.Windows.Forms.Label()
        Me.nudPitch = New System.Windows.Forms.NumericUpDown()
        Me.txtFrontL = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.nudChordPanels = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.nudWakeLength = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.nudSpanPanels = New System.Windows.Forms.NumericUpDown()
        Me.btnInertia = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.pbxPlot = New System.Windows.Forms.PictureBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnLoad = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.nudBlades = New System.Windows.Forms.NumericUpDown()
        Me.gbDimensions.SuspendLayout()
        CType(Me.nudDiameter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCentral, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPitch, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudChordPanels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.nudWakeLength, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpanPanels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbxPlot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudBlades, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 154
        Me.Label3.Text = "Name:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbxName
        '
        Me.tbxName.Location = New System.Drawing.Point(52, 5)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(176, 22)
        Me.tbxName.TabIndex = 153
        '
        'gbDimensions
        '
        Me.gbDimensions.Controls.Add(Me.Label4)
        Me.gbDimensions.Controls.Add(Me.Label2)
        Me.gbDimensions.Controls.Add(Me.Label1)
        Me.gbDimensions.Controls.Add(Me.Label5)
        Me.gbDimensions.Controls.Add(Me.nudDiameter)
        Me.gbDimensions.Controls.Add(Me.nudCentral)
        Me.gbDimensions.Controls.Add(Me.txtRearD)
        Me.gbDimensions.Controls.Add(Me.nudPitch)
        Me.gbDimensions.Controls.Add(Me.txtFrontL)
        Me.gbDimensions.Location = New System.Drawing.Point(4, 31)
        Me.gbDimensions.Name = "gbDimensions"
        Me.gbDimensions.Size = New System.Drawing.Size(224, 94)
        Me.gbDimensions.TabIndex = 155
        Me.gbDimensions.TabStop = False
        Me.gbDimensions.Text = "Geometry"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(189, 67)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(11, 13)
        Me.Label4.TabIndex = 155
        Me.Label4.Text = "°"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(189, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 13)
        Me.Label2.TabIndex = 154
        Me.Label2.Text = "%"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(189, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 13)
        Me.Label1.TabIndex = 153
        Me.Label1.Text = "m"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 13)
        Me.Label5.TabIndex = 143
        Me.Label5.Text = "Diameter:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudDiameter
        '
        Me.nudDiameter.DecimalPlaces = 4
        Me.nudDiameter.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudDiameter.Location = New System.Drawing.Point(108, 23)
        Me.nudDiameter.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudDiameter.Name = "nudDiameter"
        Me.nudDiameter.Size = New System.Drawing.Size(75, 22)
        Me.nudDiameter.TabIndex = 141
        '
        'nudCentral
        '
        Me.nudCentral.DecimalPlaces = 4
        Me.nudCentral.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudCentral.Location = New System.Drawing.Point(108, 44)
        Me.nudCentral.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudCentral.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudCentral.Name = "nudCentral"
        Me.nudCentral.Size = New System.Drawing.Size(75, 22)
        Me.nudCentral.TabIndex = 142
        '
        'txtRearD
        '
        Me.txtRearD.AutoSize = True
        Me.txtRearD.Location = New System.Drawing.Point(7, 67)
        Me.txtRearD.Name = "txtRearD"
        Me.txtRearD.Size = New System.Drawing.Size(88, 13)
        Me.txtRearD.TabIndex = 144
        Me.txtRearD.Text = "Collective pitch:"
        Me.txtRearD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudPitch
        '
        Me.nudPitch.DecimalPlaces = 4
        Me.nudPitch.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudPitch.Location = New System.Drawing.Point(108, 65)
        Me.nudPitch.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudPitch.Minimum = New Decimal(New Integer() {1410065408, 2, 0, -2147483648})
        Me.nudPitch.Name = "nudPitch"
        Me.nudPitch.Size = New System.Drawing.Size(75, 22)
        Me.nudPitch.TabIndex = 145
        '
        'txtFrontL
        '
        Me.txtFrontL.AutoSize = True
        Me.txtFrontL.Location = New System.Drawing.Point(7, 46)
        Me.txtFrontL.Name = "txtFrontL"
        Me.txtFrontL.Size = New System.Drawing.Size(95, 13)
        Me.txtFrontL.TabIndex = 146
        Me.txtFrontL.Text = "Central diameter:"
        Me.txtFrontL.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(6, 26)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(79, 13)
        Me.Label10.TabIndex = 152
        Me.Label10.Text = "Chord panels:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudChordPanels
        '
        Me.nudChordPanels.Location = New System.Drawing.Point(91, 23)
        Me.nudChordPanels.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudChordPanels.Name = "nudChordPanels"
        Me.nudChordPanels.Size = New System.Drawing.Size(75, 22)
        Me.nudChordPanels.TabIndex = 151
        Me.nudChordPanels.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.nudWakeLength)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.nudSpanPanels)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.nudChordPanels)
        Me.GroupBox1.Location = New System.Drawing.Point(234, 31)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(211, 94)
        Me.GroupBox1.TabIndex = 156
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Mesh"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(6, 68)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(73, 13)
        Me.Label7.TabIndex = 156
        Me.Label7.Text = "Wake length"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudWakeLength
        '
        Me.nudWakeLength.Location = New System.Drawing.Point(91, 65)
        Me.nudWakeLength.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudWakeLength.Name = "nudWakeLength"
        Me.nudWakeLength.Size = New System.Drawing.Size(75, 22)
        Me.nudWakeLength.TabIndex = 155
        Me.nudWakeLength.Value = New Decimal(New Integer() {4, 0, 0, 0})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 13)
        Me.Label6.TabIndex = 154
        Me.Label6.Text = "Span panels:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudSpanPanels
        '
        Me.nudSpanPanels.Location = New System.Drawing.Point(91, 44)
        Me.nudSpanPanels.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudSpanPanels.Name = "nudSpanPanels"
        Me.nudSpanPanels.Size = New System.Drawing.Size(75, 22)
        Me.nudSpanPanels.TabIndex = 153
        Me.nudSpanPanels.Value = New Decimal(New Integer() {4, 0, 0, 0})
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
        Me.btnInertia.Location = New System.Drawing.Point(452, 37)
        Me.btnInertia.Name = "btnInertia"
        Me.btnInertia.Size = New System.Drawing.Size(80, 23)
        Me.btnInertia.TabIndex = 158
        Me.btnInertia.Text = "Inertia..."
        Me.btnInertia.UseVisualStyleBackColor = False
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
        Me.Button1.Location = New System.Drawing.Point(452, 66)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(80, 23)
        Me.Button1.TabIndex = 159
        Me.Button1.Text = "Polars..."
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.White
        Me.Button2.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.Button2.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.Button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.Button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(452, 95)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(80, 23)
        Me.Button2.TabIndex = 160
        Me.Button2.Text = "Airfoil..."
        Me.Button2.UseVisualStyleBackColor = False
        '
        'pbxPlot
        '
        Me.pbxPlot.Location = New System.Drawing.Point(13, 131)
        Me.pbxPlot.Name = "pbxPlot"
        Me.pbxPlot.Size = New System.Drawing.Size(513, 251)
        Me.pbxPlot.TabIndex = 161
        Me.pbxPlot.TabStop = False
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
        Me.btnOK.Location = New System.Drawing.Point(455, 402)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 162
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'btnLoad
        '
        Me.btnLoad.BackColor = System.Drawing.Color.White
        Me.btnLoad.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnLoad.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnLoad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnLoad.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoad.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoad.Location = New System.Drawing.Point(4, 402)
        Me.btnLoad.Name = "btnLoad"
        Me.btnLoad.Size = New System.Drawing.Size(80, 23)
        Me.btnLoad.TabIndex = 163
        Me.btnLoad.Text = "Load"
        Me.btnLoad.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(240, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(102, 13)
        Me.Label8.TabIndex = 165
        Me.Label8.Text = "Number of blades:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudBlades
        '
        Me.nudBlades.Location = New System.Drawing.Point(348, 6)
        Me.nudBlades.Maximum = New Decimal(New Integer() {1410065408, 2, 0, 0})
        Me.nudBlades.Name = "nudBlades"
        Me.nudBlades.Size = New System.Drawing.Size(75, 22)
        Me.nudBlades.TabIndex = 164
        Me.nudBlades.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'FormPropeller
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(539, 430)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.nudBlades)
        Me.Controls.Add(Me.btnLoad)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.pbxPlot)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btnInertia)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.gbDimensions)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.tbxName)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormPropeller"
        Me.ShowInTaskbar = False
        Me.Text = "Propeller editor"
        Me.gbDimensions.ResumeLayout(False)
        Me.gbDimensions.PerformLayout()
        CType(Me.nudDiameter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCentral, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPitch, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudChordPanels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.nudWakeLength, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpanPanels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbxPlot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudBlades, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label3 As Label
    Friend WithEvents tbxName As TextBox
    Friend WithEvents gbDimensions As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents nudDiameter As NumericUpDown
    Friend WithEvents nudCentral As NumericUpDown
    Friend WithEvents txtRearD As Label
    Friend WithEvents nudPitch As NumericUpDown
    Friend WithEvents txtFrontL As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents nudChordPanels As NumericUpDown
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label6 As Label
    Friend WithEvents nudSpanPanels As NumericUpDown
    Friend WithEvents btnInertia As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents pbxPlot As PictureBox
    Friend WithEvents btnOK As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents nudWakeLength As NumericUpDown
    Friend WithEvents btnLoad As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents nudBlades As NumericUpDown
End Class

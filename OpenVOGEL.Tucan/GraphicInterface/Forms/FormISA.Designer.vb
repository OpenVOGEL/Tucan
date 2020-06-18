<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormISA
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
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.nudAltitude = New System.Windows.Forms.NumericUpDown()
        Me.lblAltitude = New System.Windows.Forms.Label()
        Me.txtTemp = New System.Windows.Forms.TextBox()
        Me.lblTemp = New System.Windows.Forms.Label()
        Me.lblPressure = New System.Windows.Forms.Label()
        Me.lblDensity = New System.Windows.Forms.Label()
        Me.lblDynamicViscosity = New System.Windows.Forms.Label()
        Me.lblSoundSpeed = New System.Windows.Forms.Label()
        Me.txtPressure = New System.Windows.Forms.TextBox()
        Me.txtDensity = New System.Windows.Forms.TextBox()
        Me.txtViscosity = New System.Windows.Forms.TextBox()
        Me.txtSoundSpeed = New System.Windows.Forms.TextBox()
        Me.btnCheck = New System.Windows.Forms.Button()
        Me.lblK = New System.Windows.Forms.Label()
        Me.lblPa = New System.Windows.Forms.Label()
        Me.lbldns = New System.Windows.Forms.Label()
        Me.lblVisc = New System.Windows.Forms.Label()
        Me.lblSS = New System.Windows.Forms.Label()
        Me.lblAlt = New System.Windows.Forms.Label()
        CType(Me.nudAltitude, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOk.BackColor = System.Drawing.Color.White
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOk.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Location = New System.Drawing.Point(258, 206)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(75, 23)
        Me.btnOk.TabIndex = 102
        Me.btnOk.Text = "Ok"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.BackColor = System.Drawing.Color.White
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCoral
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Location = New System.Drawing.Point(177, 206)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 103
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'nudAltitude
        '
        Me.nudAltitude.DecimalPlaces = 1
        Me.nudAltitude.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudAltitude.Location = New System.Drawing.Point(92, 19)
        Me.nudAltitude.Maximum = New Decimal(New Integer() {15000, 0, 0, 0})
        Me.nudAltitude.Name = "nudAltitude"
        Me.nudAltitude.Size = New System.Drawing.Size(79, 22)
        Me.nudAltitude.TabIndex = 104
        Me.nudAltitude.ThousandsSeparator = True
        '
        'lblAltitude
        '
        Me.lblAltitude.AutoSize = True
        Me.lblAltitude.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAltitude.Location = New System.Drawing.Point(24, 21)
        Me.lblAltitude.Name = "lblAltitude"
        Me.lblAltitude.Size = New System.Drawing.Size(48, 13)
        Me.lblAltitude.TabIndex = 105
        Me.lblAltitude.Text = "Altitude"
        '
        'txtTemp
        '
        Me.txtTemp.Location = New System.Drawing.Point(122, 56)
        Me.txtTemp.Name = "txtTemp"
        Me.txtTemp.Size = New System.Drawing.Size(79, 22)
        Me.txtTemp.TabIndex = 107
        '
        'lblTemp
        '
        Me.lblTemp.AutoSize = True
        Me.lblTemp.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTemp.Location = New System.Drawing.Point(24, 59)
        Me.lblTemp.Name = "lblTemp"
        Me.lblTemp.Size = New System.Drawing.Size(71, 13)
        Me.lblTemp.TabIndex = 108
        Me.lblTemp.Text = "Temperature"
        '
        'lblPressure
        '
        Me.lblPressure.AutoSize = True
        Me.lblPressure.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPressure.Location = New System.Drawing.Point(24, 87)
        Me.lblPressure.Name = "lblPressure"
        Me.lblPressure.Size = New System.Drawing.Size(53, 13)
        Me.lblPressure.TabIndex = 109
        Me.lblPressure.Text = "Pressure "
        '
        'lblDensity
        '
        Me.lblDensity.AutoSize = True
        Me.lblDensity.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDensity.Location = New System.Drawing.Point(24, 115)
        Me.lblDensity.Name = "lblDensity"
        Me.lblDensity.Size = New System.Drawing.Size(48, 13)
        Me.lblDensity.TabIndex = 110
        Me.lblDensity.Text = "Density "
        '
        'lblDynamicViscosity
        '
        Me.lblDynamicViscosity.AutoSize = True
        Me.lblDynamicViscosity.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDynamicViscosity.Location = New System.Drawing.Point(24, 143)
        Me.lblDynamicViscosity.Name = "lblDynamicViscosity"
        Me.lblDynamicViscosity.Size = New System.Drawing.Size(97, 13)
        Me.lblDynamicViscosity.TabIndex = 111
        Me.lblDynamicViscosity.Text = "Dynamic Viscosity"
        '
        'lblSoundSpeed
        '
        Me.lblSoundSpeed.AutoSize = True
        Me.lblSoundSpeed.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSoundSpeed.Location = New System.Drawing.Point(24, 171)
        Me.lblSoundSpeed.Name = "lblSoundSpeed"
        Me.lblSoundSpeed.Size = New System.Drawing.Size(79, 13)
        Me.lblSoundSpeed.TabIndex = 112
        Me.lblSoundSpeed.Text = "Sound Speed "
        '
        'txtPressure
        '
        Me.txtPressure.Location = New System.Drawing.Point(122, 84)
        Me.txtPressure.Name = "txtPressure"
        Me.txtPressure.Size = New System.Drawing.Size(79, 22)
        Me.txtPressure.TabIndex = 113
        '
        'txtDensity
        '
        Me.txtDensity.Location = New System.Drawing.Point(122, 112)
        Me.txtDensity.Name = "txtDensity"
        Me.txtDensity.Size = New System.Drawing.Size(79, 22)
        Me.txtDensity.TabIndex = 114
        '
        'txtViscosity
        '
        Me.txtViscosity.Location = New System.Drawing.Point(122, 140)
        Me.txtViscosity.Name = "txtViscosity"
        Me.txtViscosity.Size = New System.Drawing.Size(79, 22)
        Me.txtViscosity.TabIndex = 115
        '
        'txtSoundSpeed
        '
        Me.txtSoundSpeed.Location = New System.Drawing.Point(122, 168)
        Me.txtSoundSpeed.Name = "txtSoundSpeed"
        Me.txtSoundSpeed.Size = New System.Drawing.Size(79, 22)
        Me.txtSoundSpeed.TabIndex = 116
        '
        'btnCheck
        '
        Me.btnCheck.Location = New System.Drawing.Point(221, 16)
        Me.btnCheck.Name = "btnCheck"
        Me.btnCheck.Size = New System.Drawing.Size(75, 23)
        Me.btnCheck.TabIndex = 117
        Me.btnCheck.Text = "Check"
        Me.btnCheck.UseVisualStyleBackColor = True
        '
        'lblK
        '
        Me.lblK.AutoSize = True
        Me.lblK.Location = New System.Drawing.Point(203, 59)
        Me.lblK.Name = "lblK"
        Me.lblK.Size = New System.Drawing.Size(13, 13)
        Me.lblK.TabIndex = 118
        Me.lblK.Text = "K"
        '
        'lblPa
        '
        Me.lblPa.AutoSize = True
        Me.lblPa.Location = New System.Drawing.Point(203, 87)
        Me.lblPa.Name = "lblPa"
        Me.lblPa.Size = New System.Drawing.Size(19, 13)
        Me.lblPa.TabIndex = 119
        Me.lblPa.Text = "Pa"
        '
        'lbldns
        '
        Me.lbldns.AutoSize = True
        Me.lbldns.Location = New System.Drawing.Point(203, 115)
        Me.lbldns.Name = "lbldns"
        Me.lbldns.Size = New System.Drawing.Size(39, 13)
        Me.lbldns.TabIndex = 120
        Me.lbldns.Text = "kg/m3"
        '
        'lblVisc
        '
        Me.lblVisc.AutoSize = True
        Me.lblVisc.Location = New System.Drawing.Point(203, 143)
        Me.lblVisc.Name = "lblVisc"
        Me.lblVisc.Size = New System.Drawing.Size(38, 13)
        Me.lblVisc.TabIndex = 121
        Me.lblVisc.Text = "kg/ms"
        '
        'lblSS
        '
        Me.lblSS.AutoSize = True
        Me.lblSS.Location = New System.Drawing.Point(203, 171)
        Me.lblSS.Name = "lblSS"
        Me.lblSS.Size = New System.Drawing.Size(25, 13)
        Me.lblSS.TabIndex = 122
        Me.lblSS.Text = "m/s"
        '
        'lblAlt
        '
        Me.lblAlt.AutoSize = True
        Me.lblAlt.Location = New System.Drawing.Point(174, 21)
        Me.lblAlt.Name = "lblAlt"
        Me.lblAlt.Size = New System.Drawing.Size(16, 13)
        Me.lblAlt.TabIndex = 123
        Me.lblAlt.Text = "m"
        '
        'FormISA
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(341, 239)
        Me.Controls.Add(Me.nudAltitude)
        Me.Controls.Add(Me.txtTemp)
        Me.Controls.Add(Me.lblAlt)
        Me.Controls.Add(Me.lblSS)
        Me.Controls.Add(Me.lblVisc)
        Me.Controls.Add(Me.lbldns)
        Me.Controls.Add(Me.lblPa)
        Me.Controls.Add(Me.lblK)
        Me.Controls.Add(Me.btnCheck)
        Me.Controls.Add(Me.txtSoundSpeed)
        Me.Controls.Add(Me.txtViscosity)
        Me.Controls.Add(Me.txtDensity)
        Me.Controls.Add(Me.txtPressure)
        Me.Controls.Add(Me.lblSoundSpeed)
        Me.Controls.Add(Me.lblDynamicViscosity)
        Me.Controls.Add(Me.lblDensity)
        Me.Controls.Add(Me.lblPressure)
        Me.Controls.Add(Me.lblTemp)
        Me.Controls.Add(Me.lblAltitude)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormISA"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Standard Atmosphere"
        CType(Me.nudAltitude, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnOk As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents nudAltitude As NumericUpDown
    Friend WithEvents lblAltitude As Label
    Friend WithEvents txtTemp As TextBox
    Friend WithEvents lblTemp As Label
    Friend WithEvents lblPressure As Label
    Friend WithEvents lblDensity As Label
    Friend WithEvents lblDynamicViscosity As Label
    Friend WithEvents lblSoundSpeed As Label
    Friend WithEvents txtPressure As TextBox
    Friend WithEvents txtDensity As TextBox
    Friend WithEvents txtViscosity As TextBox
    Friend WithEvents txtSoundSpeed As TextBox
    Friend WithEvents btnCheck As Button
    Friend WithEvents lblK As Label
    Friend WithEvents lblPa As Label
    Friend WithEvents lbldns As Label
    Friend WithEvents lblVisc As Label
    Friend WithEvents lblSS As Label
    Friend WithEvents lblAlt As Label
End Class

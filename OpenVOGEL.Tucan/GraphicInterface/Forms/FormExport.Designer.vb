<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormExport
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
        Me.cbxFormats = New System.Windows.Forms.ComboBox()
        Me.lblTarget = New System.Windows.Forms.Label()
        Me.btnExport = New System.Windows.Forms.Button()
        Me.cbxOneFile = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cbxFormats
        '
        Me.cbxFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxFormats.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxFormats.FormattingEnabled = True
        Me.cbxFormats.Location = New System.Drawing.Point(11, 24)
        Me.cbxFormats.Name = "cbxFormats"
        Me.cbxFormats.Size = New System.Drawing.Size(172, 21)
        Me.cbxFormats.TabIndex = 64
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTarget.Location = New System.Drawing.Point(11, 6)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(98, 13)
        Me.lblTarget.TabIndex = 66
        Me.lblTarget.Text = "Target file format:"
        '
        'btnExport
        '
        Me.btnExport.BackColor = System.Drawing.Color.Gainsboro
        Me.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnExport.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnExport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnExport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExport.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnExport.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnExport.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnExport.Location = New System.Drawing.Point(123, 51)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(60, 22)
        Me.btnExport.TabIndex = 67
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = False
        '
        'cbxOneFile
        '
        Me.cbxOneFile.AutoSize = True
        Me.cbxOneFile.Checked = True
        Me.cbxOneFile.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxOneFile.Location = New System.Drawing.Point(11, 53)
        Me.cbxOneFile.Name = "cbxOneFile"
        Me.cbxOneFile.Size = New System.Drawing.Size(62, 17)
        Me.cbxOneFile.TabIndex = 74
        Me.cbxOneFile.Text = "One file"
        Me.cbxOneFile.UseVisualStyleBackColor = True
        '
        'FormExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(193, 84)
        Me.Controls.Add(Me.cbxOneFile)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lblTarget)
        Me.Controls.Add(Me.cbxFormats)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormExport"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Export model"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cbxFormats As ComboBox
    Friend WithEvents lblTarget As Label
    Friend WithEvents btnExport As Button
    Friend WithEvents cbxOneFile As CheckBox
End Class

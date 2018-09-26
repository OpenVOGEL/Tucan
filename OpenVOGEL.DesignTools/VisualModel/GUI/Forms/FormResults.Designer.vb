<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormResults
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.cbLattices = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'cbLattices
        '
        Me.cbLattices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLattices.FormattingEnabled = True
        Me.cbLattices.Location = New System.Drawing.Point(12, 12)
        Me.cbLattices.Name = "cbLattices"
        Me.cbLattices.Size = New System.Drawing.Size(180, 21)
        Me.cbLattices.TabIndex = 3
        '
        'FormResults
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(661, 417)
        Me.Controls.Add(Me.cbLattices)
        Me.Name = "FormResults"
        Me.Text = "FormResults"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cbLattices As System.Windows.Forms.ComboBox
End Class

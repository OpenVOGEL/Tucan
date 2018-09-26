<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ForcesPanel
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.cbResultType = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'cbLattices
        '
        Me.cbLattices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLattices.FormattingEnabled = True
        Me.cbLattices.Location = New System.Drawing.Point(3, 3)
        Me.cbLattices.Name = "cbLattices"
        Me.cbLattices.Size = New System.Drawing.Size(180, 21)
        Me.cbLattices.TabIndex = 4
        '
        'cbResultType
        '
        Me.cbResultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbResultType.FormattingEnabled = True
        Me.cbResultType.Location = New System.Drawing.Point(3, 244)
        Me.cbResultType.Name = "cbResultType"
        Me.cbResultType.Size = New System.Drawing.Size(180, 21)
        Me.cbResultType.TabIndex = 5
        '
        'ForcesPanel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cbResultType)
        Me.Controls.Add(Me.cbLattices)
        Me.DoubleBuffered = True
        Me.Name = "ForcesPanel"
        Me.Size = New System.Drawing.Size(659, 433)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cbLattices As System.Windows.Forms.ComboBox
    Friend WithEvents cbResultType As System.Windows.Forms.ComboBox
End Class

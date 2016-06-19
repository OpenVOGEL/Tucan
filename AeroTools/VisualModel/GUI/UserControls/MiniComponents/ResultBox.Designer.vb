<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ResultBox
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
        Me.lblUnit = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.tbValue = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lblUnit
        '
        Me.lblUnit.Dock = System.Windows.Forms.DockStyle.Right
        Me.lblUnit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnit.Location = New System.Drawing.Point(108, 0)
        Me.lblUnit.Name = "lblUnit"
        Me.lblUnit.Size = New System.Drawing.Size(53, 27)
        Me.lblUnit.TabIndex = 27
        Me.lblUnit.Text = "[N]"
        Me.lblUnit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblName
        '
        Me.lblName.Dock = System.Windows.Forms.DockStyle.Left
        Me.lblName.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblName.Location = New System.Drawing.Point(0, 0)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(60, 27)
        Me.lblName.TabIndex = 25
        Me.lblName.Text = "LBL"
        Me.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tbValue
        '
        Me.tbValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbValue.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbValue.Location = New System.Drawing.Point(46, 0)
        Me.tbValue.Name = "tbValue"
        Me.tbValue.Size = New System.Drawing.Size(76, 27)
        Me.tbValue.TabIndex = 26
        Me.tbValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ResultBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tbValue)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblUnit)
        Me.Name = "ResultBox"
        Me.Size = New System.Drawing.Size(161, 27)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblUnit As Windows.Forms.Label
    Friend WithEvents lblName As Windows.Forms.Label
    Friend WithEvents tbValue As Windows.Forms.TextBox
End Class

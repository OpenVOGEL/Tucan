<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormCamberLine
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
        Me.lbLines = New System.Windows.Forms.ListBox()
        Me.lblCamberLines = New System.Windows.Forms.Label()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.tbxCamberName = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.pbPlot = New System.Windows.Forms.PictureBox()
        CType(Me.pbPlot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbLines
        '
        Me.lbLines.FormattingEnabled = True
        Me.lbLines.Location = New System.Drawing.Point(12, 24)
        Me.lbLines.Name = "lbLines"
        Me.lbLines.Size = New System.Drawing.Size(199, 108)
        Me.lbLines.TabIndex = 0
        '
        'lblCamberLines
        '
        Me.lblCamberLines.AutoSize = True
        Me.lblCamberLines.Location = New System.Drawing.Point(9, 8)
        Me.lblCamberLines.Name = "lblCamberLines"
        Me.lblCamberLines.Size = New System.Drawing.Size(76, 13)
        Me.lblCamberLines.TabIndex = 1
        Me.lblCamberLines.Text = "Camber lines:"
        '
        'btnAdd
        '
        Me.btnAdd.Location = New System.Drawing.Point(217, 24)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Location = New System.Drawing.Point(217, 47)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnRemove.TabIndex = 3
        Me.btnRemove.Text = "Remove"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(304, 333)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 5
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'tbxCamberName
        '
        Me.tbxCamberName.Location = New System.Drawing.Point(12, 136)
        Me.tbxCamberName.Name = "tbxCamberName"
        Me.tbxCamberName.Size = New System.Drawing.Size(199, 22)
        Me.tbxCamberName.TabIndex = 8
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(217, 136)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Add node"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(298, 136)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 10
        Me.Button2.Text = "Del. node"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'pbPlot
        '
        Me.pbPlot.Location = New System.Drawing.Point(12, 164)
        Me.pbPlot.Name = "pbPlot"
        Me.pbPlot.Size = New System.Drawing.Size(361, 163)
        Me.pbPlot.TabIndex = 11
        Me.pbPlot.TabStop = False
        '
        'FormCamberLine
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(391, 365)
        Me.Controls.Add(Me.pbPlot)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.tbxCamberName)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.lblCamberLines)
        Me.Controls.Add(Me.lbLines)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormCamberLine"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Camber lines DB"
        CType(Me.pbPlot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lbLines As Windows.Forms.ListBox
    Friend WithEvents lblCamberLines As Windows.Forms.Label
    Friend WithEvents btnAdd As Windows.Forms.Button
    Friend WithEvents btnRemove As Windows.Forms.Button
    Friend WithEvents btnOK As Windows.Forms.Button
    Friend WithEvents tbxCamberName As Windows.Forms.TextBox
    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents Button2 As Windows.Forms.Button
    Friend WithEvents pbPlot As Windows.Forms.PictureBox
End Class

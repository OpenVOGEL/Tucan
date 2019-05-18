<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAnchors
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cbBodies = New System.Windows.Forms.ComboBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.clbWingsToAnchor = New System.Windows.Forms.CheckedListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblBody = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.cbFromRoot = New System.Windows.Forms.CheckBox()
        Me.cbFromTip = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cbBodies
        '
        Me.cbBodies.FormattingEnabled = True
        Me.cbBodies.Location = New System.Drawing.Point(12, 25)
        Me.cbBodies.Name = "cbBodies"
        Me.cbBodies.Size = New System.Drawing.Size(177, 21)
        Me.cbBodies.TabIndex = 0
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
        Me.btnOK.Location = New System.Drawing.Point(195, 173)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(77, 23)
        Me.btnOK.TabIndex = 112
        Me.btnOK.Text = "&OK"
        Me.btnOK.UseVisualStyleBackColor = False
        '
        'clbWingsToAnchor
        '
        Me.clbWingsToAnchor.FormattingEnabled = True
        Me.clbWingsToAnchor.Location = New System.Drawing.Point(12, 65)
        Me.clbWingsToAnchor.Name = "clbWingsToAnchor"
        Me.clbWingsToAnchor.Size = New System.Drawing.Size(177, 94)
        Me.clbWingsToAnchor.TabIndex = 115
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 116
        Me.Label1.Text = "Anchor to:"
        '
        'lblBody
        '
        Me.lblBody.AutoSize = True
        Me.lblBody.Location = New System.Drawing.Point(9, 9)
        Me.lblBody.Name = "lblBody"
        Me.lblBody.Size = New System.Drawing.Size(34, 13)
        Me.lblBody.TabIndex = 117
        Me.lblBody.Text = "Body:"
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.White
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(112, 173)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(77, 23)
        Me.btnCancel.TabIndex = 118
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'cbFromRoot
        '
        Me.cbFromRoot.AutoSize = True
        Me.cbFromRoot.Location = New System.Drawing.Point(195, 65)
        Me.cbFromRoot.Name = "cbFromRoot"
        Me.cbFromRoot.Size = New System.Drawing.Size(70, 17)
        Me.cbFromRoot.TabIndex = 119
        Me.cbFromRoot.Text = "From root"
        Me.cbFromRoot.UseVisualStyleBackColor = True
        '
        'cbFromTip
        '
        Me.cbFromTip.AutoSize = True
        Me.cbFromTip.Location = New System.Drawing.Point(195, 84)
        Me.cbFromTip.Name = "cbFromTip"
        Me.cbFromTip.Size = New System.Drawing.Size(63, 17)
        Me.cbFromTip.TabIndex = 120
        Me.cbFromTip.Text = "From tip"
        Me.cbFromTip.UseVisualStyleBackColor = True
        '
        'FormAnchors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(278, 202)
        Me.Controls.Add(Me.cbFromTip)
        Me.Controls.Add(Me.cbFromRoot)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblBody)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.clbWingsToAnchor)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.cbBodies)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormAnchors"
        Me.ShowInTaskbar = False
        Me.Text = "Wing-Body anchors"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cbBodies As System.Windows.Forms.ComboBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents clbWingsToAnchor As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblBody As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cbFromRoot As System.Windows.Forms.CheckBox
    Friend WithEvents cbFromTip As System.Windows.Forms.CheckBox
End Class

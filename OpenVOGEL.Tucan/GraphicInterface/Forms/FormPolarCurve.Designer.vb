<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormPolarCurve
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
        Me.btnAddFamily = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnRemoveFamily = New System.Windows.Forms.Button()
        Me.lbFamilies = New System.Windows.Forms.ListBox()
        Me.btnSavePolarDB = New System.Windows.Forms.Button()
        Me.sfdSavePolarDB = New System.Windows.Forms.SaveFileDialog()
        Me.bnLoadPolarDB = New System.Windows.Forms.Button()
        Me.ofdLoadPolarDB = New System.Windows.Forms.OpenFileDialog()
        Me.lblFamilies = New System.Windows.Forms.Label()
        Me.lbPolars = New System.Windows.Forms.ListBox()
        Me.btnRemovePolar = New System.Windows.Forms.Button()
        Me.btnAddQuadratic = New System.Windows.Forms.Button()
        Me.lblPolars = New System.Windows.Forms.Label()
        Me.btnAddCustom = New System.Windows.Forms.Button()
        Me.tbxFamilyName = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnAddFamily
        '
        Me.btnAddFamily.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddFamily.Location = New System.Drawing.Point(4, 237)
        Me.btnAddFamily.Name = "btnAddFamily"
        Me.btnAddFamily.Size = New System.Drawing.Size(78, 24)
        Me.btnAddFamily.TabIndex = 4
        Me.btnAddFamily.Text = "Add"
        Me.btnAddFamily.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOK.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnOK.Location = New System.Drawing.Point(745, 329)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(78, 23)
        Me.btnOK.TabIndex = 8
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnRemoveFamily
        '
        Me.btnRemoveFamily.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveFamily.Location = New System.Drawing.Point(4, 262)
        Me.btnRemoveFamily.Name = "btnRemoveFamily"
        Me.btnRemoveFamily.Size = New System.Drawing.Size(78, 24)
        Me.btnRemoveFamily.TabIndex = 9
        Me.btnRemoveFamily.Text = "Remove"
        Me.btnRemoveFamily.UseVisualStyleBackColor = True
        '
        'lbFamilies
        '
        Me.lbFamilies.FormattingEnabled = True
        Me.lbFamilies.Location = New System.Drawing.Point(4, 20)
        Me.lbFamilies.Name = "lbFamilies"
        Me.lbFamilies.Size = New System.Drawing.Size(162, 186)
        Me.lbFamilies.TabIndex = 10
        '
        'btnSavePolarDB
        '
        Me.btnSavePolarDB.Location = New System.Drawing.Point(88, 237)
        Me.btnSavePolarDB.Name = "btnSavePolarDB"
        Me.btnSavePolarDB.Size = New System.Drawing.Size(78, 24)
        Me.btnSavePolarDB.TabIndex = 11
        Me.btnSavePolarDB.Text = "Export"
        Me.btnSavePolarDB.UseVisualStyleBackColor = True
        '
        'sfdSavePolarDB
        '
        Me.sfdSavePolarDB.Title = "Save polars database"
        '
        'bnLoadPolarDB
        '
        Me.bnLoadPolarDB.Location = New System.Drawing.Point(88, 262)
        Me.bnLoadPolarDB.Name = "bnLoadPolarDB"
        Me.bnLoadPolarDB.Size = New System.Drawing.Size(78, 24)
        Me.bnLoadPolarDB.TabIndex = 12
        Me.bnLoadPolarDB.Text = "Import"
        Me.bnLoadPolarDB.UseVisualStyleBackColor = True
        '
        'ofdLoadPolarDB
        '
        Me.ofdLoadPolarDB.Title = "Load polars database"
        '
        'lblFamilies
        '
        Me.lblFamilies.AutoSize = True
        Me.lblFamilies.Location = New System.Drawing.Point(2, 4)
        Me.lblFamilies.Name = "lblFamilies"
        Me.lblFamilies.Size = New System.Drawing.Size(51, 13)
        Me.lblFamilies.TabIndex = 13
        Me.lblFamilies.Text = "Families:"
        '
        'lbPolars
        '
        Me.lbPolars.FormattingEnabled = True
        Me.lbPolars.Location = New System.Drawing.Point(172, 20)
        Me.lbPolars.Name = "lbPolars"
        Me.lbPolars.Size = New System.Drawing.Size(162, 186)
        Me.lbPolars.TabIndex = 14
        '
        'btnRemovePolar
        '
        Me.btnRemovePolar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemovePolar.Location = New System.Drawing.Point(172, 262)
        Me.btnRemovePolar.Name = "btnRemovePolar"
        Me.btnRemovePolar.Size = New System.Drawing.Size(78, 24)
        Me.btnRemovePolar.TabIndex = 16
        Me.btnRemovePolar.Text = "Remove"
        Me.btnRemovePolar.UseVisualStyleBackColor = True
        '
        'btnAddQuadratic
        '
        Me.btnAddQuadratic.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddQuadratic.Location = New System.Drawing.Point(172, 212)
        Me.btnAddQuadratic.Name = "btnAddQuadratic"
        Me.btnAddQuadratic.Size = New System.Drawing.Size(78, 24)
        Me.btnAddQuadratic.TabIndex = 15
        Me.btnAddQuadratic.Text = "+ Parabolic"
        Me.btnAddQuadratic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddQuadratic.UseVisualStyleBackColor = True
        '
        'lblPolars
        '
        Me.lblPolars.AutoSize = True
        Me.lblPolars.Location = New System.Drawing.Point(169, 4)
        Me.lblPolars.Name = "lblPolars"
        Me.lblPolars.Size = New System.Drawing.Size(41, 13)
        Me.lblPolars.TabIndex = 17
        Me.lblPolars.Text = "Polars:"
        '
        'btnAddCustom
        '
        Me.btnAddCustom.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddCustom.Location = New System.Drawing.Point(172, 237)
        Me.btnAddCustom.Name = "btnAddCustom"
        Me.btnAddCustom.Size = New System.Drawing.Size(78, 24)
        Me.btnAddCustom.TabIndex = 18
        Me.btnAddCustom.Text = "+ Custom"
        Me.btnAddCustom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddCustom.UseVisualStyleBackColor = True
        '
        'tbxFamilyName
        '
        Me.tbxFamilyName.Location = New System.Drawing.Point(4, 212)
        Me.tbxFamilyName.Name = "tbxFamilyName"
        Me.tbxFamilyName.Size = New System.Drawing.Size(162, 22)
        Me.tbxFamilyName.TabIndex = 19
        '
        'FormPolarCurve
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(828, 357)
        Me.Controls.Add(Me.tbxFamilyName)
        Me.Controls.Add(Me.btnAddCustom)
        Me.Controls.Add(Me.lblPolars)
        Me.Controls.Add(Me.btnRemovePolar)
        Me.Controls.Add(Me.btnAddQuadratic)
        Me.Controls.Add(Me.lbPolars)
        Me.Controls.Add(Me.lblFamilies)
        Me.Controls.Add(Me.bnLoadPolarDB)
        Me.Controls.Add(Me.btnSavePolarDB)
        Me.Controls.Add(Me.lbFamilies)
        Me.Controls.Add(Me.btnRemoveFamily)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnAddFamily)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormPolarCurve"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Polar curves for airfoil skin drag"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAddFamily As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnRemoveFamily As System.Windows.Forms.Button
    Friend WithEvents lbFamilies As System.Windows.Forms.ListBox
    Friend WithEvents btnSavePolarDB As System.Windows.Forms.Button
    Friend WithEvents sfdSavePolarDB As System.Windows.Forms.SaveFileDialog
    Friend WithEvents bnLoadPolarDB As System.Windows.Forms.Button
    Friend WithEvents ofdLoadPolarDB As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblFamilies As System.Windows.Forms.Label
    Friend WithEvents lbPolars As System.Windows.Forms.ListBox
    Friend WithEvents btnRemovePolar As System.Windows.Forms.Button
    Friend WithEvents btnAddQuadratic As System.Windows.Forms.Button
    Friend WithEvents lblPolars As System.Windows.Forms.Label
    Friend WithEvents btnAddCustom As System.Windows.Forms.Button
    Friend WithEvents tbxFamilyName As System.Windows.Forms.TextBox
End Class

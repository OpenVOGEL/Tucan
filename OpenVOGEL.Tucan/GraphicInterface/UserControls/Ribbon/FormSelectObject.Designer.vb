<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSelectObject
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
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnAddObject = New System.Windows.Forms.Button()
        Me.gbElementsToAdd = New System.Windows.Forms.GroupBox()
        Me.rbJetEngine = New System.Windows.Forms.RadioButton()
        Me.rbFuselage = New System.Windows.Forms.RadioButton()
        Me.rbLiftingSurface = New System.Windows.Forms.RadioButton()
        Me.rbImported = New System.Windows.Forms.RadioButton()
        Me.gbElementsToAdd.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.White
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 6.75!)
        Me.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnCancel.Location = New System.Drawing.Point(151, 94)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(60, 22)
        Me.btnCancel.TabIndex = 66
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnAddObject
        '
        Me.btnAddObject.BackColor = System.Drawing.Color.White
        Me.btnAddObject.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnAddObject.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnAddObject.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAddObject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnAddObject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnAddObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddObject.Font = New System.Drawing.Font("Segoe UI", 6.75!)
        Me.btnAddObject.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAddObject.Location = New System.Drawing.Point(217, 94)
        Me.btnAddObject.Name = "btnAddObject"
        Me.btnAddObject.Size = New System.Drawing.Size(60, 22)
        Me.btnAddObject.TabIndex = 65
        Me.btnAddObject.Text = "Add"
        Me.btnAddObject.UseVisualStyleBackColor = False
        '
        'gbElementsToAdd
        '
        Me.gbElementsToAdd.Controls.Add(Me.rbImported)
        Me.gbElementsToAdd.Controls.Add(Me.rbJetEngine)
        Me.gbElementsToAdd.Controls.Add(Me.rbFuselage)
        Me.gbElementsToAdd.Controls.Add(Me.rbLiftingSurface)
        Me.gbElementsToAdd.Location = New System.Drawing.Point(7, 7)
        Me.gbElementsToAdd.Name = "gbElementsToAdd"
        Me.gbElementsToAdd.Size = New System.Drawing.Size(270, 81)
        Me.gbElementsToAdd.TabIndex = 64
        Me.gbElementsToAdd.TabStop = False
        Me.gbElementsToAdd.Text = "Which type of object do you wish to add?"
        '
        'rbJetEngine
        '
        Me.rbJetEngine.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbJetEngine.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.rbJetEngine.Location = New System.Drawing.Point(144, 25)
        Me.rbJetEngine.Name = "rbJetEngine"
        Me.rbJetEngine.Size = New System.Drawing.Size(104, 20)
        Me.rbJetEngine.TabIndex = 5
        Me.rbJetEngine.Text = "Nacelle"
        Me.rbJetEngine.UseVisualStyleBackColor = True
        '
        'rbFuselage
        '
        Me.rbFuselage.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbFuselage.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.rbFuselage.Location = New System.Drawing.Point(14, 49)
        Me.rbFuselage.Name = "rbFuselage"
        Me.rbFuselage.Size = New System.Drawing.Size(242, 20)
        Me.rbFuselage.TabIndex = 4
        Me.rbFuselage.Text = "Fuselage"
        Me.rbFuselage.UseVisualStyleBackColor = True
        '
        'rbLiftingSurface
        '
        Me.rbLiftingSurface.Checked = True
        Me.rbLiftingSurface.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbLiftingSurface.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.rbLiftingSurface.Location = New System.Drawing.Point(14, 21)
        Me.rbLiftingSurface.Name = "rbLiftingSurface"
        Me.rbLiftingSurface.Size = New System.Drawing.Size(170, 24)
        Me.rbLiftingSurface.TabIndex = 3
        Me.rbLiftingSurface.TabStop = True
        Me.rbLiftingSurface.Text = "Lifting surface"
        Me.rbLiftingSurface.UseVisualStyleBackColor = True
        '
        'rbImported
        '
        Me.rbImported.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.rbImported.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.rbImported.Location = New System.Drawing.Point(144, 51)
        Me.rbImported.Name = "rbImported"
        Me.rbImported.Size = New System.Drawing.Size(104, 20)
        Me.rbImported.TabIndex = 6
        Me.rbImported.Text = "Imported mesh"
        Me.rbImported.UseVisualStyleBackColor = True
        '
        'FormSelectObject
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 123)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAddObject)
        Me.Controls.Add(Me.gbElementsToAdd)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormSelectObject"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select object type"
        Me.gbElementsToAdd.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnAddObject As System.Windows.Forms.Button
    Friend WithEvents gbElementsToAdd As System.Windows.Forms.GroupBox
    Friend WithEvents rbJetEngine As System.Windows.Forms.RadioButton
    Friend WithEvents rbFuselage As System.Windows.Forms.RadioButton
    Friend WithEvents rbLiftingSurface As System.Windows.Forms.RadioButton
    Friend WithEvents rbImported As Windows.Forms.RadioButton
End Class

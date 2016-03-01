<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectObject
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SelectObject))
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.bntOK = New System.Windows.Forms.Button()
        Me.rbLiftingSurface = New System.Windows.Forms.RadioButton()
        Me.rbFuselage = New System.Windows.Forms.RadioButton()
        Me.gbElementsToAdd = New System.Windows.Forms.GroupBox()
        Me.rbJetEngine = New System.Windows.Forms.RadioButton()
        Me.gbElementsToAdd.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Tomato
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'bntOK
        '
        Me.bntOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bntOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.bntOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DodgerBlue
        resources.ApplyResources(Me.bntOK, "bntOK")
        Me.bntOK.Name = "bntOK"
        Me.bntOK.UseVisualStyleBackColor = True
        '
        'rbLiftingSurface
        '
        Me.rbLiftingSurface.Checked = True
        resources.ApplyResources(Me.rbLiftingSurface, "rbLiftingSurface")
        Me.rbLiftingSurface.Name = "rbLiftingSurface"
        Me.rbLiftingSurface.TabStop = True
        Me.rbLiftingSurface.UseVisualStyleBackColor = True
        '
        'rbFuselage
        '
        resources.ApplyResources(Me.rbFuselage, "rbFuselage")
        Me.rbFuselage.Name = "rbFuselage"
        Me.rbFuselage.UseVisualStyleBackColor = True
        '
        'gbElementsToAdd
        '
        Me.gbElementsToAdd.Controls.Add(Me.rbJetEngine)
        Me.gbElementsToAdd.Controls.Add(Me.rbFuselage)
        Me.gbElementsToAdd.Controls.Add(Me.rbLiftingSurface)
        resources.ApplyResources(Me.gbElementsToAdd, "gbElementsToAdd")
        Me.gbElementsToAdd.Name = "gbElementsToAdd"
        Me.gbElementsToAdd.TabStop = False
        '
        'rbJetEngine
        '
        resources.ApplyResources(Me.rbJetEngine, "rbJetEngine")
        Me.rbJetEngine.Name = "rbJetEngine"
        Me.rbJetEngine.UseVisualStyleBackColor = True
        '
        'SelectObject
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.gbElementsToAdd)
        Me.Controls.Add(Me.bntOK)
        Me.Controls.Add(Me.btnCancel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectObject"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.gbElementsToAdd.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents bntOK As System.Windows.Forms.Button
    Friend WithEvents rbLiftingSurface As System.Windows.Forms.RadioButton
    Friend WithEvents rbFuselage As System.Windows.Forms.RadioButton
    Friend WithEvents gbElementsToAdd As System.Windows.Forms.GroupBox
    Friend WithEvents rbJetEngine As System.Windows.Forms.RadioButton
End Class

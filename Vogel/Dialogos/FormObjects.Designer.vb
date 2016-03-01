<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormObjects
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.dgvObjects = New System.Windows.Forms.DataGridView()
        Me.cmOtherActions = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmEditObject = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmChangeStyle = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnAddObject = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnCopyObject = New System.Windows.Forms.Button()
        Me.btnRemoveObject = New System.Windows.Forms.Button()
        CType(Me.dgvObjects, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmOtherActions.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvObjects
        '
        Me.dgvObjects.AllowUserToAddRows = False
        Me.dgvObjects.AllowUserToDeleteRows = False
        Me.dgvObjects.AllowUserToResizeRows = False
        Me.dgvObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvObjects.ContextMenuStrip = Me.cmOtherActions
        Me.dgvObjects.Dock = System.Windows.Forms.DockStyle.Top
        Me.dgvObjects.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.dgvObjects.Location = New System.Drawing.Point(4, 4)
        Me.dgvObjects.MultiSelect = False
        Me.dgvObjects.Name = "dgvObjects"
        Me.dgvObjects.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.GrayText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvObjects.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvObjects.RowHeadersWidth = 26
        Me.dgvObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvObjects.Size = New System.Drawing.Size(627, 237)
        Me.dgvObjects.TabIndex = 0
        '
        'cmOtherActions
        '
        Me.cmOtherActions.BackColor = System.Drawing.Color.White
        Me.cmOtherActions.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmEditObject, Me.tsmChangeStyle})
        Me.cmOtherActions.Name = "ContextMenuStrip1"
        Me.cmOtherActions.ShowImageMargin = False
        Me.cmOtherActions.Size = New System.Drawing.Size(118, 48)
        '
        'tsmEditObject
        '
        Me.tsmEditObject.Name = "tsmEditObject"
        Me.tsmEditObject.Size = New System.Drawing.Size(117, 22)
        Me.tsmEditObject.Text = "Edit model"
        '
        'tsmChangeStyle
        '
        Me.tsmChangeStyle.Name = "tsmChangeStyle"
        Me.tsmChangeStyle.Size = New System.Drawing.Size(117, 22)
        Me.tsmChangeStyle.Text = "Change style"
        '
        'btnAddObject
        '
        Me.btnAddObject.BackColor = System.Drawing.Color.White
        Me.btnAddObject.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnAddObject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnAddObject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnAddObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddObject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddObject.Location = New System.Drawing.Point(6, 19)
        Me.btnAddObject.Name = "btnAddObject"
        Me.btnAddObject.Size = New System.Drawing.Size(80, 23)
        Me.btnAddObject.TabIndex = 1
        Me.btnAddObject.Text = "Add"
        Me.btnAddObject.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnCopyObject)
        Me.GroupBox1.Controls.Add(Me.btnRemoveObject)
        Me.GroupBox1.Controls.Add(Me.btnAddObject)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.GroupBox1.Location = New System.Drawing.Point(4, 245)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(627, 55)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Actions"
        '
        'btnCopyObject
        '
        Me.btnCopyObject.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopyObject.BackColor = System.Drawing.Color.White
        Me.btnCopyObject.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCopyObject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnCopyObject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCopyObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCopyObject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCopyObject.Location = New System.Drawing.Point(541, 19)
        Me.btnCopyObject.Name = "btnCopyObject"
        Me.btnCopyObject.Size = New System.Drawing.Size(80, 23)
        Me.btnCopyObject.TabIndex = 3
        Me.btnCopyObject.Text = "Copy"
        Me.btnCopyObject.UseVisualStyleBackColor = False
        '
        'btnRemoveObject
        '
        Me.btnRemoveObject.BackColor = System.Drawing.Color.White
        Me.btnRemoveObject.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnRemoveObject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnRemoveObject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnRemoveObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemoveObject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveObject.Location = New System.Drawing.Point(92, 19)
        Me.btnRemoveObject.Name = "btnRemoveObject"
        Me.btnRemoveObject.Size = New System.Drawing.Size(80, 23)
        Me.btnRemoveObject.TabIndex = 2
        Me.btnRemoveObject.Text = "Remove"
        Me.btnRemoveObject.UseVisualStyleBackColor = False
        '
        'ManagerDeObjetos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(635, 304)
        Me.Controls.Add(Me.dgvObjects)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ManagerDeObjetos"
        Me.Padding = New System.Windows.Forms.Padding(4)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Objects manager"
        CType(Me.dgvObjects, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmOtherActions.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvObjects As System.Windows.Forms.DataGridView
    Friend WithEvents btnAddObject As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemoveObject As System.Windows.Forms.Button
    Friend WithEvents cmOtherActions As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmEditObject As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnCopyObject As System.Windows.Forms.Button
    Friend WithEvents tsmChangeStyle As System.Windows.Forms.ToolStripMenuItem
End Class

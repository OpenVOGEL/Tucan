<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormProgress
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
        Me.pgCalculationProgress = New System.Windows.Forms.ProgressBar()
        Me.lblStatusLabel = New System.Windows.Forms.Label()
        Me.BarraDeProgresoµ = New System.Windows.Forms.ProgressBar()
        Me.lbState = New System.Windows.Forms.Label()
        Me.tbOperationsList = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'pgCalculationProgress
        '
        Me.pgCalculationProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pgCalculationProgress.Location = New System.Drawing.Point(12, 286)
        Me.pgCalculationProgress.MarqueeAnimationSpeed = 1000
        Me.pgCalculationProgress.Name = "pgCalculationProgress"
        Me.pgCalculationProgress.Size = New System.Drawing.Size(411, 18)
        Me.pgCalculationProgress.TabIndex = 0
        '
        'lblStatusLabel
        '
        Me.lblStatusLabel.AutoSize = True
        Me.lblStatusLabel.Location = New System.Drawing.Point(9, 9)
        Me.lblStatusLabel.Name = "lblStatusLabel"
        Me.lblStatusLabel.Size = New System.Drawing.Size(42, 13)
        Me.lblStatusLabel.TabIndex = 1
        Me.lblStatusLabel.Text = "Status:"
        '
        'BarraDeProgresoµ
        '
        Me.BarraDeProgresoµ.Location = New System.Drawing.Point(12, 129)
        Me.BarraDeProgresoµ.Name = "BarraDeProgresoµ"
        Me.BarraDeProgresoµ.Size = New System.Drawing.Size(326, 23)
        Me.BarraDeProgresoµ.TabIndex = 0
        '
        'lbState
        '
        Me.lbState.AutoSize = True
        Me.lbState.Location = New System.Drawing.Point(9, 270)
        Me.lbState.Name = "lbState"
        Me.lbState.Size = New System.Drawing.Size(43, 13)
        Me.lbState.TabIndex = 3
        Me.lbState.Text = "lbState"
        '
        'tbOperationsList
        '
        Me.tbOperationsList.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbOperationsList.Location = New System.Drawing.Point(12, 25)
        Me.tbOperationsList.Multiline = True
        Me.tbOperationsList.Name = "tbOperationsList"
        Me.tbOperationsList.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbOperationsList.Size = New System.Drawing.Size(411, 242)
        Me.tbOperationsList.TabIndex = 4
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(348, 310)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Stop"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'FormProgress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(435, 338)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.tbOperationsList)
        Me.Controls.Add(Me.lbState)
        Me.Controls.Add(Me.lblStatusLabel)
        Me.Controls.Add(Me.pgCalculationProgress)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormProgress"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Calculating"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pgCalculationProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lblStatusLabel As System.Windows.Forms.Label
    Friend WithEvents BarraDeProgresoµ As System.Windows.Forms.ProgressBar
    Friend WithEvents lbState As System.Windows.Forms.Label
    Friend WithEvents tbOperationsList As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class

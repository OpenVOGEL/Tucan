<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormAskVelocity
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormAskVelocity))
        Me.Label13 = New System.Windows.Forms.Label()
        Me.ZBox = New System.Windows.Forms.NumericUpDown()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.YBox = New System.Windows.Forms.NumericUpDown()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.XBox = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Reporte = New System.Windows.Forms.TextBox()
        Me.EnPuntoDeControl = New System.Windows.Forms.RadioButton()
        Me.EnPuntoGeneral = New System.Windows.Forms.RadioButton()
        Me.PuntoDeControl = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Calcular = New System.Windows.Forms.Button()
        CType(Me.ZBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.YBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.XBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PuntoDeControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.ForeColor = System.Drawing.Color.Gray
        Me.Label13.Name = "Label13"
        '
        'ZBox
        '
        Me.ZBox.DecimalPlaces = 1
        Me.ZBox.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.ZBox, "ZBox")
        Me.ZBox.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.ZBox.Name = "ZBox"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.ForeColor = System.Drawing.Color.Gray
        Me.Label12.Name = "Label12"
        '
        'YBox
        '
        Me.YBox.DecimalPlaces = 1
        Me.YBox.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.YBox, "YBox")
        Me.YBox.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.YBox.Name = "YBox"
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.ForeColor = System.Drawing.Color.Gray
        Me.Label11.Name = "Label11"
        '
        'XBox
        '
        Me.XBox.DecimalPlaces = 1
        Me.XBox.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.XBox, "XBox")
        Me.XBox.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.XBox.Name = "XBox"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Reporte)
        Me.GroupBox1.Controls.Add(Me.EnPuntoDeControl)
        Me.GroupBox1.Controls.Add(Me.EnPuntoGeneral)
        Me.GroupBox1.Controls.Add(Me.PuntoDeControl)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.ZBox)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.YBox)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.XBox)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'Reporte
        '
        resources.ApplyResources(Me.Reporte, "Reporte")
        Me.Reporte.Name = "Reporte"
        '
        'EnPuntoDeControl
        '
        resources.ApplyResources(Me.EnPuntoDeControl, "EnPuntoDeControl")
        Me.EnPuntoDeControl.Name = "EnPuntoDeControl"
        Me.EnPuntoDeControl.UseVisualStyleBackColor = True
        '
        'EnPuntoGeneral
        '
        resources.ApplyResources(Me.EnPuntoGeneral, "EnPuntoGeneral")
        Me.EnPuntoGeneral.Checked = True
        Me.EnPuntoGeneral.Name = "EnPuntoGeneral"
        Me.EnPuntoGeneral.TabStop = True
        Me.EnPuntoGeneral.UseVisualStyleBackColor = True
        '
        'PuntoDeControl
        '
        resources.ApplyResources(Me.PuntoDeControl, "PuntoDeControl")
        Me.PuntoDeControl.Name = "PuntoDeControl"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.Color.Gray
        Me.Label1.Name = "Label1"
        '
        'Button1
        '
        Me.Button1.DialogResult = System.Windows.Forms.DialogResult.OK
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Calcular
        '
        resources.ApplyResources(Me.Calcular, "Calcular")
        Me.Calcular.Name = "Calcular"
        Me.Calcular.UseVisualStyleBackColor = True
        '
        'PreguntarVelocidad
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Calcular)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "PreguntarVelocidad"
        Me.ShowInTaskbar = False
        CType(Me.ZBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.YBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.XBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PuntoDeControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ZBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents YBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents XBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents PuntoDeControl As System.Windows.Forms.NumericUpDown
    Friend WithEvents EnPuntoDeControl As System.Windows.Forms.RadioButton
    Friend WithEvents EnPuntoGeneral As System.Windows.Forms.RadioButton
    Friend WithEvents Calcular As System.Windows.Forms.Button
    Friend WithEvents Reporte As System.Windows.Forms.TextBox
End Class

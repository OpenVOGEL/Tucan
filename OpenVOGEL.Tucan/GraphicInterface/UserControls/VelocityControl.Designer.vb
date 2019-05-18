<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VelocityControl
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
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
        Me.BarraDeEstado = New System.Windows.Forms.ProgressBar()
        Me.gbViewOptions = New System.Windows.Forms.GroupBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.NodeSampleColor = New System.Windows.Forms.PictureBox()
        Me.ColorNodo = New System.Windows.Forms.Button()
        Me.NodeSize = New System.Windows.Forms.NumericUpDown()
        Me.LineSize = New System.Windows.Forms.NumericUpDown()
        Me.VectorsSampleColor = New System.Windows.Forms.PictureBox()
        Me.Formato = New System.Windows.Forms.Button()
        Me.EscalaBox = New System.Windows.Forms.NumericUpDown()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.rdInducedVelocity = New System.Windows.Forms.RadioButton()
        Me.rdTotalVelocity = New System.Windows.Forms.RadioButton()
        Me.VisualizarPlano = New System.Windows.Forms.CheckBox()
        Me.Calcular = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.NyBox = New System.Windows.Forms.NumericUpDown()
        Me.NxBox = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ExtensionY = New System.Windows.Forms.NumericUpDown()
        Me.ExtensionX = New System.Windows.Forms.NumericUpDown()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.OrigenZ = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OrigenY = New System.Windows.Forms.NumericUpDown()
        Me.OrigenX = New System.Windows.Forms.NumericUpDown()
        Me.TitaBox = New System.Windows.Forms.NumericUpDown()
        Me.PsiBox = New System.Windows.Forms.NumericUpDown()
        Me.ColorsDialog = New System.Windows.Forms.ColorDialog()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.gbViewOptions.SuspendLayout()
        CType(Me.NodeSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NodeSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LineSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VectorsSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EscalaBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.NyBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NxBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtensionY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ExtensionX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OrigenZ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OrigenY, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.OrigenX, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TitaBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PsiBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BarraDeEstado
        '
        Me.BarraDeEstado.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BarraDeEstado.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.BarraDeEstado.Location = New System.Drawing.Point(0, 480)
        Me.BarraDeEstado.Name = "BarraDeEstado"
        Me.BarraDeEstado.Size = New System.Drawing.Size(216, 13)
        Me.BarraDeEstado.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.BarraDeEstado.TabIndex = 126
        '
        'gbViewOptions
        '
        Me.gbViewOptions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbViewOptions.Controls.Add(Me.Label14)
        Me.gbViewOptions.Controls.Add(Me.Label13)
        Me.gbViewOptions.Controls.Add(Me.NodeSampleColor)
        Me.gbViewOptions.Controls.Add(Me.ColorNodo)
        Me.gbViewOptions.Controls.Add(Me.NodeSize)
        Me.gbViewOptions.Controls.Add(Me.LineSize)
        Me.gbViewOptions.Controls.Add(Me.VectorsSampleColor)
        Me.gbViewOptions.Controls.Add(Me.Formato)
        Me.gbViewOptions.Controls.Add(Me.EscalaBox)
        Me.gbViewOptions.Controls.Add(Me.Label12)
        Me.gbViewOptions.Location = New System.Drawing.Point(0, 230)
        Me.gbViewOptions.Name = "gbViewOptions"
        Me.gbViewOptions.Size = New System.Drawing.Size(215, 244)
        Me.gbViewOptions.TabIndex = 125
        Me.gbViewOptions.TabStop = False
        Me.gbViewOptions.Text = "View options"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label14.Location = New System.Drawing.Point(12, 78)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(42, 13)
        Me.Label14.TabIndex = 129
        Me.Label14.Text = "Points:"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label13.Location = New System.Drawing.Point(12, 52)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(47, 13)
        Me.Label13.TabIndex = 128
        Me.Label13.Text = "Vectors:"
        '
        'NodeSampleColor
        '
        Me.NodeSampleColor.BackColor = System.Drawing.Color.Lime
        Me.NodeSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.NodeSampleColor.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.NodeSampleColor.Location = New System.Drawing.Point(124, 74)
        Me.NodeSampleColor.Name = "NodeSampleColor"
        Me.NodeSampleColor.Size = New System.Drawing.Size(27, 26)
        Me.NodeSampleColor.TabIndex = 127
        Me.NodeSampleColor.TabStop = False
        '
        'ColorNodo
        '
        Me.ColorNodo.BackColor = System.Drawing.Color.White
        Me.ColorNodo.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ColorNodo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.ColorNodo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.ColorNodo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ColorNodo.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ColorNodo.ForeColor = System.Drawing.Color.Black
        Me.ColorNodo.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ColorNodo.Location = New System.Drawing.Point(159, 76)
        Me.ColorNodo.Name = "ColorNodo"
        Me.ColorNodo.Size = New System.Drawing.Size(39, 22)
        Me.ColorNodo.TabIndex = 126
        Me.ColorNodo.Text = "Color"
        Me.ColorNodo.UseVisualStyleBackColor = False
        '
        'NodeSize
        '
        Me.NodeSize.DecimalPlaces = 2
        Me.NodeSize.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.NodeSize.Location = New System.Drawing.Point(68, 76)
        Me.NodeSize.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.NodeSize.Name = "NodeSize"
        Me.NodeSize.Size = New System.Drawing.Size(44, 22)
        Me.NodeSize.TabIndex = 124
        Me.NodeSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NodeSize.Value = New Decimal(New Integer() {25, 0, 0, 65536})
        '
        'LineSize
        '
        Me.LineSize.DecimalPlaces = 2
        Me.LineSize.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.LineSize.Location = New System.Drawing.Point(68, 48)
        Me.LineSize.Maximum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.LineSize.Name = "LineSize"
        Me.LineSize.Size = New System.Drawing.Size(44, 22)
        Me.LineSize.TabIndex = 122
        Me.LineSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.LineSize.Value = New Decimal(New Integer() {2, 0, 0, 65536})
        '
        'VectorsSampleColor
        '
        Me.VectorsSampleColor.BackColor = System.Drawing.Color.Lime
        Me.VectorsSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VectorsSampleColor.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.VectorsSampleColor.Location = New System.Drawing.Point(124, 46)
        Me.VectorsSampleColor.Name = "VectorsSampleColor"
        Me.VectorsSampleColor.Size = New System.Drawing.Size(27, 26)
        Me.VectorsSampleColor.TabIndex = 121
        Me.VectorsSampleColor.TabStop = False
        '
        'Formato
        '
        Me.Formato.BackColor = System.Drawing.Color.White
        Me.Formato.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.Formato.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.Formato.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.Formato.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Formato.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Formato.ForeColor = System.Drawing.Color.Black
        Me.Formato.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Formato.Location = New System.Drawing.Point(159, 48)
        Me.Formato.Name = "Formato"
        Me.Formato.Size = New System.Drawing.Size(39, 22)
        Me.Formato.TabIndex = 120
        Me.Formato.Text = "Color"
        Me.Formato.UseVisualStyleBackColor = False
        '
        'EscalaBox
        '
        Me.EscalaBox.DecimalPlaces = 3
        Me.EscalaBox.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.EscalaBox.Location = New System.Drawing.Point(52, 18)
        Me.EscalaBox.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.EscalaBox.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.EscalaBox.Name = "EscalaBox"
        Me.EscalaBox.Size = New System.Drawing.Size(61, 22)
        Me.EscalaBox.TabIndex = 117
        Me.EscalaBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label12.Location = New System.Drawing.Point(12, 20)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(36, 13)
        Me.Label12.TabIndex = 118
        Me.Label12.Text = "Scale:"
        '
        'rdInducedVelocity
        '
        Me.rdInducedVelocity.AutoSize = True
        Me.rdInducedVelocity.Location = New System.Drawing.Point(11, 205)
        Me.rdInducedVelocity.Name = "rdInducedVelocity"
        Me.rdInducedVelocity.Size = New System.Drawing.Size(129, 17)
        Me.rdInducedVelocity.TabIndex = 131
        Me.rdInducedVelocity.TabStop = True
        Me.rdInducedVelocity.Text = "Induced component"
        Me.rdInducedVelocity.UseVisualStyleBackColor = True
        '
        'rdTotalVelocity
        '
        Me.rdTotalVelocity.AutoSize = True
        Me.rdTotalVelocity.Location = New System.Drawing.Point(11, 186)
        Me.rdTotalVelocity.Name = "rdTotalVelocity"
        Me.rdTotalVelocity.Size = New System.Drawing.Size(90, 17)
        Me.rdTotalVelocity.TabIndex = 130
        Me.rdTotalVelocity.TabStop = True
        Me.rdTotalVelocity.Text = "Total velocity"
        Me.rdTotalVelocity.UseVisualStyleBackColor = True
        '
        'VisualizarPlano
        '
        Me.VisualizarPlano.AutoSize = True
        Me.VisualizarPlano.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.VisualizarPlano.Location = New System.Drawing.Point(152, 187)
        Me.VisualizarPlano.Name = "VisualizarPlano"
        Me.VisualizarPlano.Size = New System.Drawing.Size(60, 17)
        Me.VisualizarPlano.TabIndex = 116
        Me.VisualizarPlano.Text = "Visible"
        Me.VisualizarPlano.UseVisualStyleBackColor = True
        '
        'Calcular
        '
        Me.Calcular.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Calcular.BackColor = System.Drawing.Color.White
        Me.Calcular.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.Calcular.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.Calcular.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.Calcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Calcular.ForeColor = System.Drawing.Color.Black
        Me.Calcular.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Calcular.Location = New System.Drawing.Point(150, 501)
        Me.Calcular.Name = "Calcular"
        Me.Calcular.Size = New System.Drawing.Size(65, 23)
        Me.Calcular.TabIndex = 124
        Me.Calcular.Text = "Calculate"
        Me.Calcular.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.NyBox)
        Me.GroupBox1.Controls.Add(Me.NxBox)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.ExtensionY)
        Me.GroupBox1.Controls.Add(Me.ExtensionX)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.OrigenZ)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.OrigenY)
        Me.GroupBox1.Controls.Add(Me.OrigenX)
        Me.GroupBox1.Controls.Add(Me.TitaBox)
        Me.GroupBox1.Controls.Add(Me.PsiBox)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox1.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(218, 180)
        Me.GroupBox1.TabIndex = 123
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Geometry"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label9.Location = New System.Drawing.Point(124, 131)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(11, 12)
        Me.Label9.TabIndex = 115
        Me.Label9.Text = "H"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label10.Location = New System.Drawing.Point(123, 110)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(13, 12)
        Me.Label10.TabIndex = 114
        Me.Label10.Text = "W"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label11.Location = New System.Drawing.Point(108, 89)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 13)
        Me.Label11.TabIndex = 113
        Me.Label11.Text = "Control points:"
        '
        'NyBox
        '
        Me.NyBox.Location = New System.Drawing.Point(143, 129)
        Me.NyBox.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.NyBox.Name = "NyBox"
        Me.NyBox.Size = New System.Drawing.Size(61, 22)
        Me.NyBox.TabIndex = 112
        Me.NyBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NyBox.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'NxBox
        '
        Me.NxBox.Location = New System.Drawing.Point(143, 108)
        Me.NxBox.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.NxBox.Name = "NxBox"
        Me.NxBox.Size = New System.Drawing.Size(61, 22)
        Me.NxBox.TabIndex = 111
        Me.NxBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.NxBox.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label6.Location = New System.Drawing.Point(124, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(11, 12)
        Me.Label6.TabIndex = 110
        Me.Label6.Text = "H"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label7.Location = New System.Drawing.Point(123, 42)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(13, 12)
        Me.Label7.TabIndex = 109
        Me.Label7.Text = "W"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label8.Location = New System.Drawing.Point(108, 20)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(60, 13)
        Me.Label8.TabIndex = 108
        Me.Label8.Text = "Extension:"
        '
        'ExtensionY
        '
        Me.ExtensionY.DecimalPlaces = 2
        Me.ExtensionY.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.ExtensionY.Location = New System.Drawing.Point(143, 60)
        Me.ExtensionY.Name = "ExtensionY"
        Me.ExtensionY.Size = New System.Drawing.Size(61, 22)
        Me.ExtensionY.TabIndex = 107
        Me.ExtensionY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ExtensionY.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ExtensionX
        '
        Me.ExtensionX.DecimalPlaces = 2
        Me.ExtensionX.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.ExtensionX.Location = New System.Drawing.Point(143, 39)
        Me.ExtensionX.Name = "ExtensionX"
        Me.ExtensionX.Size = New System.Drawing.Size(61, 22)
        Me.ExtensionX.TabIndex = 106
        Me.ExtensionX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ExtensionX.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.BackColor = System.Drawing.Color.Transparent
        Me.Label19.Font = New System.Drawing.Font("Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label19.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label19.Location = New System.Drawing.Point(19, 64)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(13, 13)
        Me.Label19.TabIndex = 104
        Me.Label19.Text = "q"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.BackColor = System.Drawing.Color.Transparent
        Me.Label18.Font = New System.Drawing.Font("Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label18.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label18.Location = New System.Drawing.Point(18, 42)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(14, 13)
        Me.Label18.TabIndex = 105
        Me.Label18.Text = "y"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label5.Location = New System.Drawing.Point(19, 153)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(10, 12)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Z"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label4.Location = New System.Drawing.Point(19, 132)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(10, 12)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Y"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label3.Location = New System.Drawing.Point(19, 111)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(10, 12)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "X"
        '
        'OrigenZ
        '
        Me.OrigenZ.DecimalPlaces = 2
        Me.OrigenZ.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.OrigenZ.Location = New System.Drawing.Point(37, 151)
        Me.OrigenZ.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.OrigenZ.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.OrigenZ.Name = "OrigenZ"
        Me.OrigenZ.Size = New System.Drawing.Size(61, 22)
        Me.OrigenZ.TabIndex = 6
        Me.OrigenZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(9, 89)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Center:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(9, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Orientation:"
        '
        'OrigenY
        '
        Me.OrigenY.DecimalPlaces = 2
        Me.OrigenY.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.OrigenY.Location = New System.Drawing.Point(37, 130)
        Me.OrigenY.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.OrigenY.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.OrigenY.Name = "OrigenY"
        Me.OrigenY.Size = New System.Drawing.Size(61, 22)
        Me.OrigenY.TabIndex = 3
        Me.OrigenY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'OrigenX
        '
        Me.OrigenX.DecimalPlaces = 2
        Me.OrigenX.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.OrigenX.Location = New System.Drawing.Point(37, 109)
        Me.OrigenX.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.OrigenX.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.OrigenX.Name = "OrigenX"
        Me.OrigenX.Size = New System.Drawing.Size(61, 22)
        Me.OrigenX.TabIndex = 2
        Me.OrigenX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TitaBox
        '
        Me.TitaBox.Location = New System.Drawing.Point(37, 61)
        Me.TitaBox.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        Me.TitaBox.Name = "TitaBox"
        Me.TitaBox.Size = New System.Drawing.Size(61, 22)
        Me.TitaBox.TabIndex = 1
        Me.TitaBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PsiBox
        '
        Me.PsiBox.Location = New System.Drawing.Point(37, 40)
        Me.PsiBox.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        Me.PsiBox.Name = "PsiBox"
        Me.PsiBox.Size = New System.Drawing.Size(61, 22)
        Me.PsiBox.TabIndex = 0
        Me.PsiBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnOk
        '
        Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOk.BackColor = System.Drawing.Color.White
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(3, 501)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(65, 23)
        Me.btnOk.TabIndex = 128
        Me.btnOk.Text = "Close"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'VelocityControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.rdInducedVelocity)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.BarraDeEstado)
        Me.Controls.Add(Me.rdTotalVelocity)
        Me.Controls.Add(Me.gbViewOptions)
        Me.Controls.Add(Me.Calcular)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.VisualizarPlano)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "VelocityControl"
        Me.Size = New System.Drawing.Size(218, 530)
        Me.gbViewOptions.ResumeLayout(False)
        Me.gbViewOptions.PerformLayout()
        CType(Me.NodeSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NodeSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LineSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VectorsSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EscalaBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.NyBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NxBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtensionY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ExtensionX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OrigenZ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OrigenY, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.OrigenX, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TitaBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PsiBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BarraDeEstado As System.Windows.Forms.ProgressBar
    Friend WithEvents gbViewOptions As System.Windows.Forms.GroupBox
    Friend WithEvents NodeSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents ColorNodo As System.Windows.Forms.Button
    Friend WithEvents NodeSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents VisualizarPlano As System.Windows.Forms.CheckBox
    Friend WithEvents LineSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents VectorsSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents Formato As System.Windows.Forms.Button
    Friend WithEvents EscalaBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Calcular As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents NyBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents NxBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ExtensionY As System.Windows.Forms.NumericUpDown
    Friend WithEvents ExtensionX As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents OrigenZ As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OrigenY As System.Windows.Forms.NumericUpDown
    Friend WithEvents OrigenX As System.Windows.Forms.NumericUpDown
    Friend WithEvents TitaBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents PsiBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents ColorsDialog As System.Windows.Forms.ColorDialog
    Friend WithEvents rdTotalVelocity As System.Windows.Forms.RadioButton
    Friend WithEvents rdInducedVelocity As System.Windows.Forms.RadioButton
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label

End Class

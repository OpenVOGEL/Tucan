<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormOptions
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormOptions))
        Me.Ocultar = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.Aparence = New System.Windows.Forms.TabPage()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.MarcacionFina = New System.Windows.Forms.CheckBox()
        Me.PantallaSample = New System.Windows.Forms.PictureBox()
        Me.PantallaButton = New System.Windows.Forms.Button()
        Me.MarcacionGruesa = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Zmed_Box = New System.Windows.Forms.NumericUpDown()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Ymin_Box = New System.Windows.Forms.NumericUpDown()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Ymax_Box = New System.Windows.Forms.NumericUpDown()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Xmin_Box = New System.Windows.Forms.NumericUpDown()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Xmax_Box = New System.Windows.Forms.NumericUpDown()
        Me.WireframeSample = New System.Windows.Forms.PictureBox()
        Me.ReferenceFrame = New System.Windows.Forms.Button()
        Me.GrupoDiseño = New System.Windows.Forms.GroupBox()
        Me.ListaDeCuerpos = New System.Windows.Forms.ComboBox()
        Me.IncMalladoCuerpos = New System.Windows.Forms.CheckBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.TransparenciaCuerpos = New System.Windows.Forms.NumericUpDown()
        Me.MalladoCuerposSampleColor = New System.Windows.Forms.PictureBox()
        Me.CuerposSampleColor = New System.Windows.Forms.PictureBox()
        Me.MalladoCuerposColor = New System.Windows.Forms.Button()
        Me.CuerposColor = New System.Windows.Forms.Button()
        Me.ListeDeSuperfices = New System.Windows.Forms.ComboBox()
        Me.IncMallado = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TransparenciaSuperficie = New System.Windows.Forms.NumericUpDown()
        Me.MalladoSampleColor = New System.Windows.Forms.PictureBox()
        Me.SuperficieSampleColor = New System.Windows.Forms.PictureBox()
        Me.MalladoPalaColor = New System.Windows.Forms.Button()
        Me.SuperficiesColor = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.AlphaBlendBox = New System.Windows.Forms.CheckBox()
        Me.Antialias2D = New System.Windows.Forms.CheckBox()
        Me.SmothLines = New System.Windows.Forms.CheckBox()
        Me.Postprocess = New System.Windows.Forms.TabPage()
        Me.GrupoPostProceso = New System.Windows.Forms.GroupBox()
        Me.MapaDePresión = New System.Windows.Forms.CheckBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.nudWakesNodesSize = New System.Windows.Forms.NumericUpDown()
        Me.nudWakeMeshSize = New System.Windows.Forms.NumericUpDown()
        Me.cbShowWakeNodes = New System.Windows.Forms.CheckBox()
        Me.cbWakeColorNodes = New System.Windows.Forms.Button()
        Me.NodoEstSampleColor = New System.Windows.Forms.PictureBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cbShowWakeMesh = New System.Windows.Forms.CheckBox()
        Me.ModeloColor = New System.Windows.Forms.Button()
        Me.cbWakeColorMesh = New System.Windows.Forms.Button()
        Me.btnWakeColorSurface = New System.Windows.Forms.Button()
        Me.MalladoEstSampleColor = New System.Windows.Forms.PictureBox()
        Me.ModeloSampleColor = New System.Windows.Forms.PictureBox()
        Me.IncluirMalladoMod = New System.Windows.Forms.CheckBox()
        Me.EstelasSampleColor = New System.Windows.Forms.PictureBox()
        Me.cbShowWakeSurface = New System.Windows.Forms.CheckBox()
        Me.TransparenciaModelo = New System.Windows.Forms.NumericUpDown()
        Me.MalladoColor = New System.Windows.Forms.Button()
        Me.nudWakesSurfaceTrans = New System.Windows.Forms.NumericUpDown()
        Me.MalladoModeloSampleColor = New System.Windows.Forms.PictureBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ResetMapaDePresiones = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.MinPresBox = New System.Windows.Forms.NumericUpDown()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.MaxPresBox = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.RepresentarDV = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.EscalaDV = New System.Windows.Forms.NumericUpDown()
        Me.DVColor = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.SaltoVColor = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.EscalaVmedia = New System.Windows.Forms.NumericUpDown()
        Me.VmeanColor = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.VmediaColor = New System.Windows.Forms.PictureBox()
        Me.RepresentarVMedia = New System.Windows.Forms.CheckBox()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.EscalaCarga = New System.Windows.Forms.NumericUpDown()
        Me.LoadColor = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ColorCarga = New System.Windows.Forms.PictureBox()
        Me.RepresentarCarga = New System.Windows.Forms.CheckBox()
        Me.Colores = New System.Windows.Forms.ColorDialog()
        Me.ToolTipS = New System.Windows.Forms.ToolTip(Me.components)
        Me.FontDialog1 = New System.Windows.Forms.FontDialog()
        Me.TabControl1.SuspendLayout()
        Me.Aparence.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.PantallaSample, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Zmed_Box, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Ymin_Box, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Ymax_Box, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Xmin_Box, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Xmax_Box, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.WireframeSample, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GrupoDiseño.SuspendLayout()
        CType(Me.TransparenciaCuerpos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MalladoCuerposSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CuerposSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TransparenciaSuperficie, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MalladoSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SuperficieSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.Postprocess.SuspendLayout()
        Me.GrupoPostProceso.SuspendLayout()
        CType(Me.nudWakesNodesSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudWakeMeshSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NodoEstSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MalladoEstSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ModeloSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EstelasSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TransparenciaModelo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudWakesSurfaceTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MalladoModeloSampleColor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.MinPresBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MaxPresBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox7.SuspendLayout()
        CType(Me.EscalaDV, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SaltoVColor, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.EscalaVmedia, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VmediaColor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox6.SuspendLayout()
        CType(Me.EscalaCarga, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ColorCarga, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Ocultar
        '
        Me.Ocultar.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.Ocultar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.Ocultar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.Ocultar, "Ocultar")
        Me.Ocultar.Name = "Ocultar"
        Me.ToolTipS.SetToolTip(Me.Ocultar, resources.GetString("Ocultar.ToolTip"))
        Me.Ocultar.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.Aparence)
        Me.TabControl1.Controls.Add(Me.Postprocess)
        resources.ApplyResources(Me.TabControl1, "TabControl1")
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        '
        'Aparence
        '
        Me.Aparence.Controls.Add(Me.GroupBox5)
        Me.Aparence.Controls.Add(Me.GrupoDiseño)
        Me.Aparence.Controls.Add(Me.GroupBox2)
        resources.ApplyResources(Me.Aparence, "Aparence")
        Me.Aparence.Name = "Aparence"
        Me.Aparence.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.MarcacionFina)
        Me.GroupBox5.Controls.Add(Me.PantallaSample)
        Me.GroupBox5.Controls.Add(Me.PantallaButton)
        Me.GroupBox5.Controls.Add(Me.MarcacionGruesa)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.Zmed_Box)
        Me.GroupBox5.Controls.Add(Me.Label14)
        Me.GroupBox5.Controls.Add(Me.Ymin_Box)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.Ymax_Box)
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Controls.Add(Me.Xmin_Box)
        Me.GroupBox5.Controls.Add(Me.Label11)
        Me.GroupBox5.Controls.Add(Me.Xmax_Box)
        Me.GroupBox5.Controls.Add(Me.WireframeSample)
        Me.GroupBox5.Controls.Add(Me.ReferenceFrame)
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.TabStop = False
        '
        'MarcacionFina
        '
        resources.ApplyResources(Me.MarcacionFina, "MarcacionFina")
        Me.MarcacionFina.Name = "MarcacionFina"
        Me.MarcacionFina.UseVisualStyleBackColor = True
        '
        'PantallaSample
        '
        Me.PantallaSample.BackColor = System.Drawing.Color.White
        Me.PantallaSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.PantallaSample, "PantallaSample")
        Me.PantallaSample.Name = "PantallaSample"
        Me.PantallaSample.TabStop = False
        '
        'PantallaButton
        '
        Me.PantallaButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.PantallaButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.PantallaButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.PantallaButton, "PantallaButton")
        Me.PantallaButton.Name = "PantallaButton"
        Me.PantallaButton.UseVisualStyleBackColor = True
        '
        'MarcacionGruesa
        '
        resources.ApplyResources(Me.MarcacionGruesa, "MarcacionGruesa")
        Me.MarcacionGruesa.Name = "MarcacionGruesa"
        Me.MarcacionGruesa.UseVisualStyleBackColor = True
        '
        'Label15
        '
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.ForeColor = System.Drawing.Color.Gray
        Me.Label15.Name = "Label15"
        '
        'Zmed_Box
        '
        Me.Zmed_Box.DecimalPlaces = 1
        Me.Zmed_Box.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.Zmed_Box, "Zmed_Box")
        Me.Zmed_Box.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Zmed_Box.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.Zmed_Box.Name = "Zmed_Box"
        Me.ToolTipS.SetToolTip(Me.Zmed_Box, resources.GetString("Zmed_Box.ToolTip"))
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.ForeColor = System.Drawing.Color.Gray
        Me.Label14.Name = "Label14"
        '
        'Ymin_Box
        '
        Me.Ymin_Box.DecimalPlaces = 1
        Me.Ymin_Box.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.Ymin_Box, "Ymin_Box")
        Me.Ymin_Box.Maximum = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Ymin_Box.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.Ymin_Box.Name = "Ymin_Box"
        Me.ToolTipS.SetToolTip(Me.Ymin_Box, resources.GetString("Ymin_Box.ToolTip"))
        Me.Ymin_Box.Value = New Decimal(New Integer() {5, 0, 0, -2147483648})
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.ForeColor = System.Drawing.Color.Gray
        Me.Label13.Name = "Label13"
        '
        'Ymax_Box
        '
        Me.Ymax_Box.DecimalPlaces = 1
        Me.Ymax_Box.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.Ymax_Box, "Ymax_Box")
        Me.Ymax_Box.Name = "Ymax_Box"
        Me.ToolTipS.SetToolTip(Me.Ymax_Box, resources.GetString("Ymax_Box.ToolTip"))
        Me.Ymax_Box.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.ForeColor = System.Drawing.Color.Gray
        Me.Label12.Name = "Label12"
        '
        'Xmin_Box
        '
        Me.Xmin_Box.DecimalPlaces = 1
        Me.Xmin_Box.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.Xmin_Box, "Xmin_Box")
        Me.Xmin_Box.Maximum = New Decimal(New Integer() {0, 0, 0, 0})
        Me.Xmin_Box.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.Xmin_Box.Name = "Xmin_Box"
        Me.ToolTipS.SetToolTip(Me.Xmin_Box, resources.GetString("Xmin_Box.ToolTip"))
        Me.Xmin_Box.Value = New Decimal(New Integer() {5, 0, 0, -2147483648})
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.ForeColor = System.Drawing.Color.Gray
        Me.Label11.Name = "Label11"
        '
        'Xmax_Box
        '
        Me.Xmax_Box.DecimalPlaces = 1
        Me.Xmax_Box.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.Xmax_Box, "Xmax_Box")
        Me.Xmax_Box.Name = "Xmax_Box"
        Me.ToolTipS.SetToolTip(Me.Xmax_Box, resources.GetString("Xmax_Box.ToolTip"))
        Me.Xmax_Box.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'WireframeSample
        '
        Me.WireframeSample.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.WireframeSample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.WireframeSample, "WireframeSample")
        Me.WireframeSample.Name = "WireframeSample"
        Me.WireframeSample.TabStop = False
        '
        'ReferenceFrame
        '
        Me.ReferenceFrame.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ReferenceFrame.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.ReferenceFrame.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.ReferenceFrame, "ReferenceFrame")
        Me.ReferenceFrame.Name = "ReferenceFrame"
        Me.ReferenceFrame.UseVisualStyleBackColor = True
        '
        'GrupoDiseño
        '
        Me.GrupoDiseño.Controls.Add(Me.ListaDeCuerpos)
        Me.GrupoDiseño.Controls.Add(Me.IncMalladoCuerpos)
        Me.GrupoDiseño.Controls.Add(Me.Label16)
        Me.GrupoDiseño.Controls.Add(Me.Label17)
        Me.GrupoDiseño.Controls.Add(Me.TransparenciaCuerpos)
        Me.GrupoDiseño.Controls.Add(Me.MalladoCuerposSampleColor)
        Me.GrupoDiseño.Controls.Add(Me.CuerposSampleColor)
        Me.GrupoDiseño.Controls.Add(Me.MalladoCuerposColor)
        Me.GrupoDiseño.Controls.Add(Me.CuerposColor)
        Me.GrupoDiseño.Controls.Add(Me.ListeDeSuperfices)
        Me.GrupoDiseño.Controls.Add(Me.IncMallado)
        Me.GrupoDiseño.Controls.Add(Me.Label8)
        Me.GrupoDiseño.Controls.Add(Me.Label7)
        Me.GrupoDiseño.Controls.Add(Me.TransparenciaSuperficie)
        Me.GrupoDiseño.Controls.Add(Me.MalladoSampleColor)
        Me.GrupoDiseño.Controls.Add(Me.SuperficieSampleColor)
        Me.GrupoDiseño.Controls.Add(Me.MalladoPalaColor)
        Me.GrupoDiseño.Controls.Add(Me.SuperficiesColor)
        resources.ApplyResources(Me.GrupoDiseño, "GrupoDiseño")
        Me.GrupoDiseño.Name = "GrupoDiseño"
        Me.GrupoDiseño.TabStop = False
        '
        'ListaDeCuerpos
        '
        Me.ListaDeCuerpos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ListaDeCuerpos, "ListaDeCuerpos")
        Me.ListaDeCuerpos.FormattingEnabled = True
        Me.ListaDeCuerpos.Name = "ListaDeCuerpos"
        '
        'IncMalladoCuerpos
        '
        resources.ApplyResources(Me.IncMalladoCuerpos, "IncMalladoCuerpos")
        Me.IncMalladoCuerpos.Checked = True
        Me.IncMalladoCuerpos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.IncMalladoCuerpos.Name = "IncMalladoCuerpos"
        Me.ToolTipS.SetToolTip(Me.IncMalladoCuerpos, resources.GetString("IncMalladoCuerpos.ToolTip"))
        Me.IncMalladoCuerpos.UseVisualStyleBackColor = True
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.ForeColor = System.Drawing.Color.Gray
        Me.Label16.Name = "Label16"
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.ForeColor = System.Drawing.Color.Gray
        Me.Label17.Name = "Label17"
        '
        'TransparenciaCuerpos
        '
        Me.TransparenciaCuerpos.DecimalPlaces = 1
        Me.TransparenciaCuerpos.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.TransparenciaCuerpos, "TransparenciaCuerpos")
        Me.TransparenciaCuerpos.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TransparenciaCuerpos.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.TransparenciaCuerpos.Name = "TransparenciaCuerpos"
        Me.ToolTipS.SetToolTip(Me.TransparenciaCuerpos, resources.GetString("TransparenciaCuerpos.ToolTip"))
        Me.TransparenciaCuerpos.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'MalladoCuerposSampleColor
        '
        Me.MalladoCuerposSampleColor.BackColor = System.Drawing.Color.Green
        Me.MalladoCuerposSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.MalladoCuerposSampleColor, "MalladoCuerposSampleColor")
        Me.MalladoCuerposSampleColor.Name = "MalladoCuerposSampleColor"
        Me.MalladoCuerposSampleColor.TabStop = False
        '
        'CuerposSampleColor
        '
        Me.CuerposSampleColor.BackColor = System.Drawing.Color.Lime
        Me.CuerposSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.CuerposSampleColor, "CuerposSampleColor")
        Me.CuerposSampleColor.Name = "CuerposSampleColor"
        Me.CuerposSampleColor.TabStop = False
        '
        'MalladoCuerposColor
        '
        Me.MalladoCuerposColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.MalladoCuerposColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.MalladoCuerposColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.MalladoCuerposColor, "MalladoCuerposColor")
        Me.MalladoCuerposColor.Name = "MalladoCuerposColor"
        Me.ToolTipS.SetToolTip(Me.MalladoCuerposColor, resources.GetString("MalladoCuerposColor.ToolTip"))
        Me.MalladoCuerposColor.UseVisualStyleBackColor = True
        '
        'CuerposColor
        '
        Me.CuerposColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.CuerposColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.CuerposColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.CuerposColor, "CuerposColor")
        Me.CuerposColor.Name = "CuerposColor"
        Me.ToolTipS.SetToolTip(Me.CuerposColor, resources.GetString("CuerposColor.ToolTip"))
        Me.CuerposColor.UseVisualStyleBackColor = True
        '
        'ListeDeSuperfices
        '
        Me.ListeDeSuperfices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.ListeDeSuperfices, "ListeDeSuperfices")
        Me.ListeDeSuperfices.FormattingEnabled = True
        Me.ListeDeSuperfices.Name = "ListeDeSuperfices"
        '
        'IncMallado
        '
        resources.ApplyResources(Me.IncMallado, "IncMallado")
        Me.IncMallado.Checked = True
        Me.IncMallado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.IncMallado.Name = "IncMallado"
        Me.ToolTipS.SetToolTip(Me.IncMallado, resources.GetString("IncMallado.ToolTip"))
        Me.IncMallado.UseVisualStyleBackColor = True
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.ForeColor = System.Drawing.Color.Gray
        Me.Label8.Name = "Label8"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.ForeColor = System.Drawing.Color.Gray
        Me.Label7.Name = "Label7"
        '
        'TransparenciaSuperficie
        '
        Me.TransparenciaSuperficie.DecimalPlaces = 1
        Me.TransparenciaSuperficie.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.TransparenciaSuperficie, "TransparenciaSuperficie")
        Me.TransparenciaSuperficie.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TransparenciaSuperficie.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.TransparenciaSuperficie.Name = "TransparenciaSuperficie"
        Me.ToolTipS.SetToolTip(Me.TransparenciaSuperficie, resources.GetString("TransparenciaSuperficie.ToolTip"))
        Me.TransparenciaSuperficie.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'MalladoSampleColor
        '
        Me.MalladoSampleColor.BackColor = System.Drawing.Color.Green
        Me.MalladoSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.MalladoSampleColor, "MalladoSampleColor")
        Me.MalladoSampleColor.Name = "MalladoSampleColor"
        Me.MalladoSampleColor.TabStop = False
        '
        'SuperficieSampleColor
        '
        Me.SuperficieSampleColor.BackColor = System.Drawing.Color.Lime
        Me.SuperficieSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.SuperficieSampleColor, "SuperficieSampleColor")
        Me.SuperficieSampleColor.Name = "SuperficieSampleColor"
        Me.SuperficieSampleColor.TabStop = False
        '
        'MalladoPalaColor
        '
        Me.MalladoPalaColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.MalladoPalaColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.MalladoPalaColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.MalladoPalaColor, "MalladoPalaColor")
        Me.MalladoPalaColor.Name = "MalladoPalaColor"
        Me.ToolTipS.SetToolTip(Me.MalladoPalaColor, resources.GetString("MalladoPalaColor.ToolTip"))
        Me.MalladoPalaColor.UseVisualStyleBackColor = True
        '
        'SuperficiesColor
        '
        Me.SuperficiesColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.SuperficiesColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.SuperficiesColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.SuperficiesColor, "SuperficiesColor")
        Me.SuperficiesColor.Name = "SuperficiesColor"
        Me.ToolTipS.SetToolTip(Me.SuperficiesColor, resources.GetString("SuperficiesColor.ToolTip"))
        Me.SuperficiesColor.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.AlphaBlendBox)
        Me.GroupBox2.Controls.Add(Me.Antialias2D)
        Me.GroupBox2.Controls.Add(Me.SmothLines)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'AlphaBlendBox
        '
        resources.ApplyResources(Me.AlphaBlendBox, "AlphaBlendBox")
        Me.AlphaBlendBox.Name = "AlphaBlendBox"
        Me.AlphaBlendBox.UseVisualStyleBackColor = True
        '
        'Antialias2D
        '
        resources.ApplyResources(Me.Antialias2D, "Antialias2D")
        Me.Antialias2D.Checked = True
        Me.Antialias2D.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Antialias2D.Name = "Antialias2D"
        Me.Antialias2D.UseVisualStyleBackColor = True
        '
        'SmothLines
        '
        resources.ApplyResources(Me.SmothLines, "SmothLines")
        Me.SmothLines.Name = "SmothLines"
        Me.SmothLines.UseVisualStyleBackColor = True
        '
        'Postprocess
        '
        Me.Postprocess.Controls.Add(Me.GrupoPostProceso)
        Me.Postprocess.Controls.Add(Me.GroupBox1)
        Me.Postprocess.Controls.Add(Me.GroupBox7)
        Me.Postprocess.Controls.Add(Me.GroupBox6)
        resources.ApplyResources(Me.Postprocess, "Postprocess")
        Me.Postprocess.Name = "Postprocess"
        Me.Postprocess.UseVisualStyleBackColor = True
        '
        'GrupoPostProceso
        '
        Me.GrupoPostProceso.Controls.Add(Me.MapaDePresión)
        Me.GrupoPostProceso.Controls.Add(Me.Label9)
        Me.GrupoPostProceso.Controls.Add(Me.nudWakesNodesSize)
        Me.GrupoPostProceso.Controls.Add(Me.nudWakeMeshSize)
        Me.GrupoPostProceso.Controls.Add(Me.cbShowWakeNodes)
        Me.GrupoPostProceso.Controls.Add(Me.cbWakeColorNodes)
        Me.GrupoPostProceso.Controls.Add(Me.NodoEstSampleColor)
        Me.GrupoPostProceso.Controls.Add(Me.Label10)
        Me.GrupoPostProceso.Controls.Add(Me.cbShowWakeMesh)
        Me.GrupoPostProceso.Controls.Add(Me.ModeloColor)
        Me.GrupoPostProceso.Controls.Add(Me.cbWakeColorMesh)
        Me.GrupoPostProceso.Controls.Add(Me.btnWakeColorSurface)
        Me.GrupoPostProceso.Controls.Add(Me.MalladoEstSampleColor)
        Me.GrupoPostProceso.Controls.Add(Me.ModeloSampleColor)
        Me.GrupoPostProceso.Controls.Add(Me.IncluirMalladoMod)
        Me.GrupoPostProceso.Controls.Add(Me.EstelasSampleColor)
        Me.GrupoPostProceso.Controls.Add(Me.cbShowWakeSurface)
        Me.GrupoPostProceso.Controls.Add(Me.TransparenciaModelo)
        Me.GrupoPostProceso.Controls.Add(Me.MalladoColor)
        Me.GrupoPostProceso.Controls.Add(Me.nudWakesSurfaceTrans)
        Me.GrupoPostProceso.Controls.Add(Me.MalladoModeloSampleColor)
        resources.ApplyResources(Me.GrupoPostProceso, "GrupoPostProceso")
        Me.GrupoPostProceso.Name = "GrupoPostProceso"
        Me.GrupoPostProceso.TabStop = False
        '
        'MapaDePresión
        '
        Me.MapaDePresión.Checked = True
        Me.MapaDePresión.CheckState = System.Windows.Forms.CheckState.Checked
        resources.ApplyResources(Me.MapaDePresión, "MapaDePresión")
        Me.MapaDePresión.Name = "MapaDePresión"
        Me.ToolTipS.SetToolTip(Me.MapaDePresión, resources.GetString("MapaDePresión.ToolTip"))
        Me.MapaDePresión.UseVisualStyleBackColor = True
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.ForeColor = System.Drawing.Color.Gray
        Me.Label9.Name = "Label9"
        '
        'nudWakesNodesSize
        '
        Me.nudWakesNodesSize.DecimalPlaces = 1
        Me.nudWakesNodesSize.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.nudWakesNodesSize, "nudWakesNodesSize")
        Me.nudWakesNodesSize.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.nudWakesNodesSize.Name = "nudWakesNodesSize"
        Me.ToolTipS.SetToolTip(Me.nudWakesNodesSize, resources.GetString("nudWakesNodesSize.ToolTip"))
        Me.nudWakesNodesSize.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'nudWakeMeshSize
        '
        Me.nudWakeMeshSize.DecimalPlaces = 1
        Me.nudWakeMeshSize.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.nudWakeMeshSize, "nudWakeMeshSize")
        Me.nudWakeMeshSize.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.nudWakeMeshSize.Name = "nudWakeMeshSize"
        Me.ToolTipS.SetToolTip(Me.nudWakeMeshSize, resources.GetString("nudWakeMeshSize.ToolTip"))
        Me.nudWakeMeshSize.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'cbShowWakeNodes
        '
        resources.ApplyResources(Me.cbShowWakeNodes, "cbShowWakeNodes")
        Me.cbShowWakeNodes.Name = "cbShowWakeNodes"
        Me.ToolTipS.SetToolTip(Me.cbShowWakeNodes, resources.GetString("cbShowWakeNodes.ToolTip"))
        Me.cbShowWakeNodes.UseVisualStyleBackColor = True
        '
        'cbWakeColorNodes
        '
        Me.cbWakeColorNodes.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.cbWakeColorNodes.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.cbWakeColorNodes.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.cbWakeColorNodes, "cbWakeColorNodes")
        Me.cbWakeColorNodes.Name = "cbWakeColorNodes"
        Me.ToolTipS.SetToolTip(Me.cbWakeColorNodes, resources.GetString("cbWakeColorNodes.ToolTip"))
        Me.cbWakeColorNodes.UseVisualStyleBackColor = True
        '
        'NodoEstSampleColor
        '
        Me.NodoEstSampleColor.BackColor = System.Drawing.Color.Black
        Me.NodoEstSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.NodoEstSampleColor, "NodoEstSampleColor")
        Me.NodoEstSampleColor.Name = "NodoEstSampleColor"
        Me.NodoEstSampleColor.TabStop = False
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.ForeColor = System.Drawing.Color.Gray
        Me.Label10.Name = "Label10"
        '
        'cbShowWakeMesh
        '
        resources.ApplyResources(Me.cbShowWakeMesh, "cbShowWakeMesh")
        Me.cbShowWakeMesh.Name = "cbShowWakeMesh"
        Me.ToolTipS.SetToolTip(Me.cbShowWakeMesh, resources.GetString("cbShowWakeMesh.ToolTip"))
        Me.cbShowWakeMesh.UseVisualStyleBackColor = True
        '
        'ModeloColor
        '
        Me.ModeloColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ModeloColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.ModeloColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.ModeloColor, "ModeloColor")
        Me.ModeloColor.Name = "ModeloColor"
        Me.ToolTipS.SetToolTip(Me.ModeloColor, resources.GetString("ModeloColor.ToolTip"))
        Me.ModeloColor.UseVisualStyleBackColor = True
        '
        'cbWakeColorMesh
        '
        Me.cbWakeColorMesh.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.cbWakeColorMesh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.cbWakeColorMesh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.cbWakeColorMesh, "cbWakeColorMesh")
        Me.cbWakeColorMesh.Name = "cbWakeColorMesh"
        Me.ToolTipS.SetToolTip(Me.cbWakeColorMesh, resources.GetString("cbWakeColorMesh.ToolTip"))
        Me.cbWakeColorMesh.UseVisualStyleBackColor = True
        '
        'btnWakeColorSurface
        '
        Me.btnWakeColorSurface.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnWakeColorSurface.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnWakeColorSurface.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.btnWakeColorSurface, "btnWakeColorSurface")
        Me.btnWakeColorSurface.Name = "btnWakeColorSurface"
        Me.ToolTipS.SetToolTip(Me.btnWakeColorSurface, resources.GetString("btnWakeColorSurface.ToolTip"))
        Me.btnWakeColorSurface.UseVisualStyleBackColor = True
        '
        'MalladoEstSampleColor
        '
        Me.MalladoEstSampleColor.BackColor = System.Drawing.Color.Silver
        Me.MalladoEstSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.MalladoEstSampleColor, "MalladoEstSampleColor")
        Me.MalladoEstSampleColor.Name = "MalladoEstSampleColor"
        Me.MalladoEstSampleColor.TabStop = False
        '
        'ModeloSampleColor
        '
        Me.ModeloSampleColor.BackColor = System.Drawing.Color.Lime
        Me.ModeloSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.ModeloSampleColor, "ModeloSampleColor")
        Me.ModeloSampleColor.Name = "ModeloSampleColor"
        Me.ModeloSampleColor.TabStop = False
        '
        'IncluirMalladoMod
        '
        resources.ApplyResources(Me.IncluirMalladoMod, "IncluirMalladoMod")
        Me.IncluirMalladoMod.Checked = True
        Me.IncluirMalladoMod.CheckState = System.Windows.Forms.CheckState.Checked
        Me.IncluirMalladoMod.Name = "IncluirMalladoMod"
        Me.ToolTipS.SetToolTip(Me.IncluirMalladoMod, resources.GetString("IncluirMalladoMod.ToolTip"))
        Me.IncluirMalladoMod.UseVisualStyleBackColor = True
        '
        'EstelasSampleColor
        '
        Me.EstelasSampleColor.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.EstelasSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.EstelasSampleColor, "EstelasSampleColor")
        Me.EstelasSampleColor.Name = "EstelasSampleColor"
        Me.EstelasSampleColor.TabStop = False
        '
        'cbShowWakeSurface
        '
        resources.ApplyResources(Me.cbShowWakeSurface, "cbShowWakeSurface")
        Me.cbShowWakeSurface.Checked = True
        Me.cbShowWakeSurface.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbShowWakeSurface.Name = "cbShowWakeSurface"
        Me.ToolTipS.SetToolTip(Me.cbShowWakeSurface, resources.GetString("cbShowWakeSurface.ToolTip"))
        Me.cbShowWakeSurface.UseVisualStyleBackColor = True
        '
        'TransparenciaModelo
        '
        Me.TransparenciaModelo.DecimalPlaces = 1
        Me.TransparenciaModelo.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.TransparenciaModelo, "TransparenciaModelo")
        Me.TransparenciaModelo.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TransparenciaModelo.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.TransparenciaModelo.Name = "TransparenciaModelo"
        Me.ToolTipS.SetToolTip(Me.TransparenciaModelo, resources.GetString("TransparenciaModelo.ToolTip"))
        Me.TransparenciaModelo.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'MalladoColor
        '
        Me.MalladoColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.MalladoColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.MalladoColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.MalladoColor, "MalladoColor")
        Me.MalladoColor.Name = "MalladoColor"
        Me.ToolTipS.SetToolTip(Me.MalladoColor, resources.GetString("MalladoColor.ToolTip"))
        Me.MalladoColor.UseVisualStyleBackColor = True
        '
        'nudWakesSurfaceTrans
        '
        Me.nudWakesSurfaceTrans.DecimalPlaces = 1
        Me.nudWakesSurfaceTrans.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.nudWakesSurfaceTrans, "nudWakesSurfaceTrans")
        Me.nudWakesSurfaceTrans.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudWakesSurfaceTrans.Name = "nudWakesSurfaceTrans"
        Me.ToolTipS.SetToolTip(Me.nudWakesSurfaceTrans, resources.GetString("nudWakesSurfaceTrans.ToolTip"))
        Me.nudWakesSurfaceTrans.Value = New Decimal(New Integer() {7, 0, 0, 65536})
        '
        'MalladoModeloSampleColor
        '
        Me.MalladoModeloSampleColor.BackColor = System.Drawing.Color.Green
        Me.MalladoModeloSampleColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.MalladoModeloSampleColor, "MalladoModeloSampleColor")
        Me.MalladoModeloSampleColor.Name = "MalladoModeloSampleColor"
        Me.MalladoModeloSampleColor.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ResetMapaDePresiones)
        Me.GroupBox1.Controls.Add(Me.Label19)
        Me.GroupBox1.Controls.Add(Me.MinPresBox)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.MaxPresBox)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'ResetMapaDePresiones
        '
        Me.ResetMapaDePresiones.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.ResetMapaDePresiones.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.ResetMapaDePresiones.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.ResetMapaDePresiones, "ResetMapaDePresiones")
        Me.ResetMapaDePresiones.Name = "ResetMapaDePresiones"
        Me.ResetMapaDePresiones.UseVisualStyleBackColor = True
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        '
        'MinPresBox
        '
        Me.MinPresBox.DecimalPlaces = 6
        Me.MinPresBox.Increment = New Decimal(New Integer() {5, 0, 0, 196608})
        resources.ApplyResources(Me.MinPresBox, "MinPresBox")
        Me.MinPresBox.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.MinPresBox.Name = "MinPresBox"
        Me.MinPresBox.Tag = ""
        Me.ToolTipS.SetToolTip(Me.MinPresBox, resources.GetString("MinPresBox.ToolTip"))
        Me.MinPresBox.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label18
        '
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.Name = "Label18"
        '
        'MaxPresBox
        '
        Me.MaxPresBox.DecimalPlaces = 6
        Me.MaxPresBox.Increment = New Decimal(New Integer() {5, 0, 0, 196608})
        resources.ApplyResources(Me.MaxPresBox, "MaxPresBox")
        Me.MaxPresBox.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.MaxPresBox.Name = "MaxPresBox"
        Me.MaxPresBox.Tag = ""
        Me.ToolTipS.SetToolTip(Me.MaxPresBox, resources.GetString("MaxPresBox.ToolTip"))
        Me.MaxPresBox.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.RepresentarDV)
        Me.GroupBox7.Controls.Add(Me.Label5)
        Me.GroupBox7.Controls.Add(Me.EscalaDV)
        Me.GroupBox7.Controls.Add(Me.DVColor)
        Me.GroupBox7.Controls.Add(Me.Label6)
        Me.GroupBox7.Controls.Add(Me.SaltoVColor)
        Me.GroupBox7.Controls.Add(Me.Label3)
        Me.GroupBox7.Controls.Add(Me.EscalaVmedia)
        Me.GroupBox7.Controls.Add(Me.VmeanColor)
        Me.GroupBox7.Controls.Add(Me.Label4)
        Me.GroupBox7.Controls.Add(Me.VmediaColor)
        Me.GroupBox7.Controls.Add(Me.RepresentarVMedia)
        resources.ApplyResources(Me.GroupBox7, "GroupBox7")
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.TabStop = False
        '
        'RepresentarDV
        '
        resources.ApplyResources(Me.RepresentarDV, "RepresentarDV")
        Me.RepresentarDV.Name = "RepresentarDV"
        Me.ToolTipS.SetToolTip(Me.RepresentarDV, resources.GetString("RepresentarDV.ToolTip"))
        Me.RepresentarDV.UseVisualStyleBackColor = True
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'EscalaDV
        '
        Me.EscalaDV.DecimalPlaces = 3
        Me.EscalaDV.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        resources.ApplyResources(Me.EscalaDV, "EscalaDV")
        Me.EscalaDV.Name = "EscalaDV"
        Me.EscalaDV.Tag = ""
        Me.ToolTipS.SetToolTip(Me.EscalaDV, resources.GetString("EscalaDV.ToolTip"))
        Me.EscalaDV.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'DVColor
        '
        Me.DVColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.DVColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.DVColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.DVColor, "DVColor")
        Me.DVColor.Name = "DVColor"
        Me.ToolTipS.SetToolTip(Me.DVColor, resources.GetString("DVColor.ToolTip"))
        Me.DVColor.UseVisualStyleBackColor = True
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'SaltoVColor
        '
        Me.SaltoVColor.BackColor = System.Drawing.Color.Crimson
        resources.ApplyResources(Me.SaltoVColor, "SaltoVColor")
        Me.SaltoVColor.Name = "SaltoVColor"
        Me.SaltoVColor.TabStop = False
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'EscalaVmedia
        '
        Me.EscalaVmedia.DecimalPlaces = 3
        Me.EscalaVmedia.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        resources.ApplyResources(Me.EscalaVmedia, "EscalaVmedia")
        Me.EscalaVmedia.Name = "EscalaVmedia"
        Me.EscalaVmedia.Tag = ""
        Me.ToolTipS.SetToolTip(Me.EscalaVmedia, resources.GetString("EscalaVmedia.ToolTip"))
        Me.EscalaVmedia.Value = New Decimal(New Integer() {5, 0, 0, 131072})
        '
        'VmeanColor
        '
        Me.VmeanColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.VmeanColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.VmeanColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.VmeanColor, "VmeanColor")
        Me.VmeanColor.Name = "VmeanColor"
        Me.ToolTipS.SetToolTip(Me.VmeanColor, resources.GetString("VmeanColor.ToolTip"))
        Me.VmeanColor.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'VmediaColor
        '
        Me.VmediaColor.BackColor = System.Drawing.Color.DarkOrchid
        resources.ApplyResources(Me.VmediaColor, "VmediaColor")
        Me.VmediaColor.Name = "VmediaColor"
        Me.VmediaColor.TabStop = False
        '
        'RepresentarVMedia
        '
        resources.ApplyResources(Me.RepresentarVMedia, "RepresentarVMedia")
        Me.RepresentarVMedia.Name = "RepresentarVMedia"
        Me.ToolTipS.SetToolTip(Me.RepresentarVMedia, resources.GetString("RepresentarVMedia.ToolTip"))
        Me.RepresentarVMedia.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.Label2)
        Me.GroupBox6.Controls.Add(Me.EscalaCarga)
        Me.GroupBox6.Controls.Add(Me.LoadColor)
        Me.GroupBox6.Controls.Add(Me.Label1)
        Me.GroupBox6.Controls.Add(Me.ColorCarga)
        Me.GroupBox6.Controls.Add(Me.RepresentarCarga)
        resources.ApplyResources(Me.GroupBox6, "GroupBox6")
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.TabStop = False
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'EscalaCarga
        '
        Me.EscalaCarga.DecimalPlaces = 2
        Me.EscalaCarga.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        resources.ApplyResources(Me.EscalaCarga, "EscalaCarga")
        Me.EscalaCarga.Name = "EscalaCarga"
        Me.EscalaCarga.Tag = ""
        Me.ToolTipS.SetToolTip(Me.EscalaCarga, resources.GetString("EscalaCarga.ToolTip"))
        Me.EscalaCarga.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'LoadColor
        '
        Me.LoadColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.LoadColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.LoadColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        resources.ApplyResources(Me.LoadColor, "LoadColor")
        Me.LoadColor.Name = "LoadColor"
        Me.ToolTipS.SetToolTip(Me.LoadColor, resources.GetString("LoadColor.ToolTip"))
        Me.LoadColor.UseVisualStyleBackColor = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'ColorCarga
        '
        Me.ColorCarga.BackColor = System.Drawing.Color.RoyalBlue
        resources.ApplyResources(Me.ColorCarga, "ColorCarga")
        Me.ColorCarga.Name = "ColorCarga"
        Me.ColorCarga.TabStop = False
        '
        'RepresentarCarga
        '
        resources.ApplyResources(Me.RepresentarCarga, "RepresentarCarga")
        Me.RepresentarCarga.Name = "RepresentarCarga"
        Me.ToolTipS.SetToolTip(Me.RepresentarCarga, resources.GetString("RepresentarCarga.ToolTip"))
        Me.RepresentarCarga.UseVisualStyleBackColor = True
        '
        'OptionsDialog
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.Ocultar)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "OptionsDialog"
        Me.ShowInTaskbar = False
        Me.TabControl1.ResumeLayout(False)
        Me.Aparence.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.PantallaSample, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Zmed_Box, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Ymin_Box, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Ymax_Box, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Xmin_Box, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Xmax_Box, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.WireframeSample, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GrupoDiseño.ResumeLayout(False)
        Me.GrupoDiseño.PerformLayout()
        CType(Me.TransparenciaCuerpos, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MalladoCuerposSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CuerposSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TransparenciaSuperficie, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MalladoSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SuperficieSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Postprocess.ResumeLayout(False)
        Me.GrupoPostProceso.ResumeLayout(False)
        Me.GrupoPostProceso.PerformLayout()
        CType(Me.nudWakesNodesSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudWakeMeshSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NodoEstSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MalladoEstSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ModeloSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EstelasSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TransparenciaModelo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudWakesSurfaceTrans, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MalladoModeloSampleColor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.MinPresBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MaxPresBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        CType(Me.EscalaDV, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SaltoVColor, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.EscalaVmedia, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VmediaColor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.EscalaCarga, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ColorCarga, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Ocultar As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents Aparence As System.Windows.Forms.TabPage
    Friend WithEvents SmothLines As System.Windows.Forms.CheckBox
    Friend WithEvents Antialias2D As System.Windows.Forms.CheckBox
    Friend WithEvents ReferenceFrame As System.Windows.Forms.Button
    Friend WithEvents Postprocess As System.Windows.Forms.TabPage
    Friend WithEvents btnWakeColorSurface As System.Windows.Forms.Button
    Friend WithEvents ModeloColor As System.Windows.Forms.Button
    Friend WithEvents Colores As System.Windows.Forms.ColorDialog
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents MalladoPalaColor As System.Windows.Forms.Button
    Friend WithEvents SuperficiesColor As System.Windows.Forms.Button
    Friend WithEvents EstelasSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents ModeloSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents MalladoSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents SuperficieSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents WireframeSample As System.Windows.Forms.PictureBox
    Friend WithEvents nudWakesSurfaceTrans As System.Windows.Forms.NumericUpDown
    Friend WithEvents TransparenciaSuperficie As System.Windows.Forms.NumericUpDown
    Friend WithEvents TransparenciaModelo As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents GrupoDiseño As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents EscalaCarga As System.Windows.Forms.NumericUpDown
    Friend WithEvents LoadColor As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ColorCarga As System.Windows.Forms.PictureBox
    Friend WithEvents RepresentarCarga As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents EscalaVmedia As System.Windows.Forms.NumericUpDown
    Friend WithEvents VmeanColor As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents VmediaColor As System.Windows.Forms.PictureBox
    Friend WithEvents RepresentarVMedia As System.Windows.Forms.CheckBox
    Friend WithEvents RepresentarDV As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents EscalaDV As System.Windows.Forms.NumericUpDown
    Friend WithEvents DVColor As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents SaltoVColor As System.Windows.Forms.PictureBox
    Friend WithEvents MalladoColor As System.Windows.Forms.Button
    Friend WithEvents MalladoModeloSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents cbShowWakeSurface As System.Windows.Forms.CheckBox
    Friend WithEvents cbShowWakeMesh As System.Windows.Forms.CheckBox
    Friend WithEvents cbWakeColorMesh As System.Windows.Forms.Button
    Friend WithEvents MalladoEstSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents IncluirMalladoMod As System.Windows.Forms.CheckBox
    Friend WithEvents GrupoPostProceso As System.Windows.Forms.GroupBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents IncMallado As System.Windows.Forms.CheckBox
    Friend WithEvents ToolTipS As System.Windows.Forms.ToolTip
    Friend WithEvents ListeDeSuperfices As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Zmed_Box As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Ymin_Box As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Ymax_Box As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Xmin_Box As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Xmax_Box As System.Windows.Forms.NumericUpDown
    Friend WithEvents PantallaSample As System.Windows.Forms.PictureBox
    Friend WithEvents PantallaButton As System.Windows.Forms.Button
    Friend WithEvents MarcacionFina As System.Windows.Forms.CheckBox
    Friend WithEvents MarcacionGruesa As System.Windows.Forms.CheckBox
    Friend WithEvents ListaDeCuerpos As System.Windows.Forms.ComboBox
    Friend WithEvents IncMalladoCuerpos As System.Windows.Forms.CheckBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TransparenciaCuerpos As System.Windows.Forms.NumericUpDown
    Friend WithEvents MalladoCuerposSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents CuerposSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents MalladoCuerposColor As System.Windows.Forms.Button
    Friend WithEvents CuerposColor As System.Windows.Forms.Button
    Friend WithEvents cbShowWakeNodes As System.Windows.Forms.CheckBox
    Friend WithEvents cbWakeColorNodes As System.Windows.Forms.Button
    Friend WithEvents NodoEstSampleColor As System.Windows.Forms.PictureBox
    Friend WithEvents nudWakesNodesSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudWakeMeshSize As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents MapaDePresión As System.Windows.Forms.CheckBox
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents MinPresBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents MaxPresBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents ResetMapaDePresiones As System.Windows.Forms.Button
    Friend WithEvents AlphaBlendBox As System.Windows.Forms.CheckBox
End Class

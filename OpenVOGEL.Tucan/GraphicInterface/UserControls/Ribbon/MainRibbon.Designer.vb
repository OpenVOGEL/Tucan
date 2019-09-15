<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainRibbon
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.tcRibbon = New System.Windows.Forms.TabControl()
        Me.tpModel = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbSecuence = New System.Windows.Forms.ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.nudFi = New System.Windows.Forms.NumericUpDown()
        Me.nudTita = New System.Windows.Forms.NumericUpDown()
        Me.nudPsi = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.nudCRz = New System.Windows.Forms.NumericUpDown()
        Me.nudCRy = New System.Windows.Forms.NumericUpDown()
        Me.nudCRx = New System.Windows.Forms.NumericUpDown()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.nudPz = New System.Windows.Forms.NumericUpDown()
        Me.nudPy = New System.Windows.Forms.NumericUpDown()
        Me.nudPx = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbxInclude = New System.Windows.Forms.CheckBox()
        Me.btnClone = New System.Windows.Forms.Button()
        Me.btnAddObject = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.cbxSurfaces = New System.Windows.Forms.ComboBox()
        Me.tbxName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.lblMeshInfo = New System.Windows.Forms.Label()
        Me.cbxShowMesh = New System.Windows.Forms.CheckBox()
        Me.cbxShowSurface = New System.Windows.Forms.CheckBox()
        Me.pnlMeshColor = New System.Windows.Forms.Panel()
        Me.pnlSurfaceColor = New System.Windows.Forms.Panel()
        Me.pnlIO = New System.Windows.Forms.GroupBox()
        Me.btnNew = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnSaveAs = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.tpCalculation = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox12 = New System.Windows.Forms.GroupBox()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.nudIncrement = New System.Windows.Forms.NumericUpDown()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.nudSteps = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox11 = New System.Windows.Forms.GroupBox()
        Me.btnHistogram = New System.Windows.Forms.Button()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.nudViscosity = New System.Windows.Forms.NumericUpDown()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.nudDensity = New System.Windows.Forms.NumericUpDown()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.nudOz = New System.Windows.Forms.NumericUpDown()
        Me.nudOy = New System.Windows.Forms.NumericUpDown()
        Me.nudOx = New System.Windows.Forms.NumericUpDown()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.nudVz = New System.Windows.Forms.NumericUpDown()
        Me.nudVy = New System.Windows.Forms.NumericUpDown()
        Me.nudVx = New System.Windows.Forms.NumericUpDown()
        Me.gbxCalculationType = New System.Windows.Forms.GroupBox()
        Me.btnStartCalculation = New System.Windows.Forms.Button()
        Me.cbxSimulationMode = New System.Windows.Forms.ComboBox()
        Me.tpResults = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.gbxAeroelastic = New System.Windows.Forms.GroupBox()
        Me.btnPlayStop = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.nudModeScale = New System.Windows.Forms.NumericUpDown()
        Me.cbxModes = New System.Windows.Forms.ComboBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.btnEditVelocityPlane = New System.Windows.Forms.Button()
        Me.cbxShowVelocityPlane = New System.Windows.Forms.CheckBox()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.nudScaleForce = New System.Windows.Forms.NumericUpDown()
        Me.cbxShowVelocity = New System.Windows.Forms.CheckBox()
        Me.nudScaleVelocity = New System.Windows.Forms.NumericUpDown()
        Me.pnlVelocityColor = New System.Windows.Forms.Panel()
        Me.cbxShowForce = New System.Windows.Forms.CheckBox()
        Me.pnlForceColor = New System.Windows.Forms.Panel()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.nudCpmin = New System.Windows.Forms.NumericUpDown()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.nudCpmax = New System.Windows.Forms.NumericUpDown()
        Me.cbxShowColormap = New System.Windows.Forms.CheckBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.btnResetColormap = New System.Windows.Forms.Button()
        Me.nudDCpmin = New System.Windows.Forms.NumericUpDown()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.nudDCpmax = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox8 = New System.Windows.Forms.GroupBox()
        Me.cbxShowWakeNodes = New System.Windows.Forms.CheckBox()
        Me.pnlWakeNodeColor = New System.Windows.Forms.Panel()
        Me.cbxShowWakeMesh = New System.Windows.Forms.CheckBox()
        Me.cbxShowWakeSurface = New System.Windows.Forms.CheckBox()
        Me.pnlWakeMeshColor = New System.Windows.Forms.Panel()
        Me.pnlWakeSurfaceColor = New System.Windows.Forms.Panel()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.cbxShowResMesh = New System.Windows.Forms.CheckBox()
        Me.cbxShowResSurface = New System.Windows.Forms.CheckBox()
        Me.pnlResultMeshColor = New System.Windows.Forms.Panel()
        Me.pnlResultSurfaceColor = New System.Windows.Forms.Panel()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.btnReport = New System.Windows.Forms.Button()
        Me.btnLoadResults = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.rbStructure = New System.Windows.Forms.RadioButton()
        Me.cbMultiselect = New System.Windows.Forms.CheckBox()
        Me.rnPanel = New System.Windows.Forms.RadioButton()
        Me.rbNode = New System.Windows.Forms.RadioButton()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.btnFrontView = New System.Windows.Forms.Button()
        Me.btnTopView = New System.Windows.Forms.Button()
        Me.btnSideView = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudYmin = New System.Windows.Forms.NumericUpDown()
        Me.nudYmax = New System.Windows.Forms.NumericUpDown()
        Me.nudXmin = New System.Windows.Forms.NumericUpDown()
        Me.nudXmax = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cbxShowRulers = New System.Windows.Forms.CheckBox()
        Me.pnlScreenColor = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.tcRibbon.SuspendLayout()
        Me.tpModel.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.nudFi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTita, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPsi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCRz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCRy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCRx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPx, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.pnlIO.SuspendLayout()
        Me.tpCalculation.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        CType(Me.nudIncrement, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSteps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox11.SuspendLayout()
        CType(Me.nudViscosity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDensity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudOz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudOy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudOx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudVz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudVy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudVx, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbxCalculationType.SuspendLayout()
        Me.tpResults.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.gbxAeroelastic.SuspendLayout()
        CType(Me.nudModeScale, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        CType(Me.nudScaleForce, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudScaleVelocity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox9.SuspendLayout()
        CType(Me.nudCpmin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCpmax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDCpmin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDCpmax, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.nudYmin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudYmax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudXmin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudXmax, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'tcRibbon
        '
        Me.tcRibbon.Controls.Add(Me.tpModel)
        Me.tcRibbon.Controls.Add(Me.tpCalculation)
        Me.tcRibbon.Controls.Add(Me.tpResults)
        Me.tcRibbon.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcRibbon.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcRibbon.Location = New System.Drawing.Point(3, 3)
        Me.tcRibbon.MinimumSize = New System.Drawing.Size(0, 130)
        Me.tcRibbon.Name = "tcRibbon"
        Me.tcRibbon.SelectedIndex = 0
        Me.tcRibbon.Size = New System.Drawing.Size(912, 169)
        Me.tcRibbon.TabIndex = 0
        '
        'tpModel
        '
        Me.tpModel.Controls.Add(Me.TableLayoutPanel1)
        Me.tpModel.Location = New System.Drawing.Point(4, 22)
        Me.tpModel.Name = "tpModel"
        Me.tpModel.Size = New System.Drawing.Size(904, 143)
        Me.tpModel.TabIndex = 1
        Me.tpModel.Text = "Model"
        Me.tpModel.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox2, 2, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.pnlIO, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(904, 143)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.cbSecuence)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.Label16)
        Me.GroupBox2.Controls.Add(Me.nudFi)
        Me.GroupBox2.Controls.Add(Me.nudTita)
        Me.GroupBox2.Controls.Add(Me.nudPsi)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.nudCRz)
        Me.GroupBox2.Controls.Add(Me.nudCRy)
        Me.GroupBox2.Controls.Add(Me.nudCRx)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.nudPz)
        Me.GroupBox2.Controls.Add(Me.nudPy)
        Me.GroupBox2.Controls.Add(Me.nudPx)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(504, 3)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(397, 139)
        Me.GroupBox2.TabIndex = 506
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Position"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label2.Location = New System.Drawing.Point(237, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 12)
        Me.Label2.TabIndex = 107
        Me.Label2.Text = "Sequence:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbSecuence
        '
        Me.cbSecuence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSecuence.FormattingEnabled = True
        Me.cbSecuence.Items.AddRange(New Object() {"ZYX", "XYZ"})
        Me.cbSecuence.Location = New System.Drawing.Point(237, 34)
        Me.cbSecuence.Name = "cbSecuence"
        Me.cbSecuence.Size = New System.Drawing.Size(50, 20)
        Me.cbSecuence.TabIndex = 106
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Symbol", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label14.Location = New System.Drawing.Point(165, 54)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(10, 11)
        Me.Label14.TabIndex = 101
        Me.Label14.Text = "f"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Symbol", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label15.Location = New System.Drawing.Point(165, 37)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(10, 11)
        Me.Label15.TabIndex = 100
        Me.Label15.Text = "q"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Symbol", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label16.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label16.Location = New System.Drawing.Point(165, 19)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(12, 11)
        Me.Label16.TabIndex = 99
        Me.Label16.Text = "y"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudFi
        '
        Me.nudFi.DecimalPlaces = 1
        Me.nudFi.Location = New System.Drawing.Point(181, 51)
        Me.nudFi.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
        Me.nudFi.Minimum = New Decimal(New Integer() {180, 0, 0, -2147483648})
        Me.nudFi.Name = "nudFi"
        Me.nudFi.Size = New System.Drawing.Size(50, 19)
        Me.nudFi.TabIndex = 98
        Me.nudFi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudTita
        '
        Me.nudTita.DecimalPlaces = 1
        Me.nudTita.Location = New System.Drawing.Point(181, 35)
        Me.nudTita.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
        Me.nudTita.Minimum = New Decimal(New Integer() {180, 0, 0, -2147483648})
        Me.nudTita.Name = "nudTita"
        Me.nudTita.Size = New System.Drawing.Size(50, 19)
        Me.nudTita.TabIndex = 97
        Me.nudTita.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudPsi
        '
        Me.nudPsi.DecimalPlaces = 1
        Me.nudPsi.Location = New System.Drawing.Point(181, 17)
        Me.nudPsi.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
        Me.nudPsi.Minimum = New Decimal(New Integer() {180, 0, 0, -2147483648})
        Me.nudPsi.Name = "nudPsi"
        Me.nudPsi.Size = New System.Drawing.Size(50, 19)
        Me.nudPsi.TabIndex = 96
        Me.nudPsi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label3.Location = New System.Drawing.Point(86, 54)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(13, 12)
        Me.Label3.TabIndex = 95
        Me.Label3.Text = "Zr"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label12.Location = New System.Drawing.Point(86, 37)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(12, 12)
        Me.Label12.TabIndex = 94
        Me.Label12.Text = "Yr"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label13.Location = New System.Drawing.Point(86, 19)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(13, 12)
        Me.Label13.TabIndex = 93
        Me.Label13.Text = "Xr"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudCRz
        '
        Me.nudCRz.DecimalPlaces = 2
        Me.nudCRz.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudCRz.Location = New System.Drawing.Point(102, 51)
        Me.nudCRz.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudCRz.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.nudCRz.Name = "nudCRz"
        Me.nudCRz.Size = New System.Drawing.Size(50, 19)
        Me.nudCRz.TabIndex = 92
        Me.nudCRz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudCRy
        '
        Me.nudCRy.DecimalPlaces = 2
        Me.nudCRy.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudCRy.Location = New System.Drawing.Point(102, 35)
        Me.nudCRy.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudCRy.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.nudCRy.Name = "nudCRy"
        Me.nudCRy.Size = New System.Drawing.Size(50, 19)
        Me.nudCRy.TabIndex = 91
        Me.nudCRy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudCRx
        '
        Me.nudCRx.DecimalPlaces = 2
        Me.nudCRx.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudCRx.Location = New System.Drawing.Point(102, 17)
        Me.nudCRx.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudCRx.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.nudCRx.Name = "nudCRx"
        Me.nudCRx.Size = New System.Drawing.Size(50, 19)
        Me.nudCRx.TabIndex = 90
        Me.nudCRx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label9.Location = New System.Drawing.Point(8, 54)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(10, 12)
        Me.Label9.TabIndex = 89
        Me.Label9.Text = "Z"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label10.Location = New System.Drawing.Point(8, 37)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(10, 12)
        Me.Label10.TabIndex = 88
        Me.Label10.Text = "Y"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label11.Location = New System.Drawing.Point(8, 19)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(10, 12)
        Me.Label11.TabIndex = 87
        Me.Label11.Text = "X"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudPz
        '
        Me.nudPz.DecimalPlaces = 2
        Me.nudPz.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPz.Location = New System.Drawing.Point(24, 51)
        Me.nudPz.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPz.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.nudPz.Name = "nudPz"
        Me.nudPz.Size = New System.Drawing.Size(50, 19)
        Me.nudPz.TabIndex = 86
        Me.nudPz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudPy
        '
        Me.nudPy.DecimalPlaces = 2
        Me.nudPy.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPy.Location = New System.Drawing.Point(24, 35)
        Me.nudPy.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPy.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.nudPy.Name = "nudPy"
        Me.nudPy.Size = New System.Drawing.Size(50, 19)
        Me.nudPy.TabIndex = 85
        Me.nudPy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudPx
        '
        Me.nudPx.DecimalPlaces = 2
        Me.nudPx.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudPx.Location = New System.Drawing.Point(24, 17)
        Me.nudPx.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPx.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.nudPx.Name = "nudPx"
        Me.nudPx.Size = New System.Drawing.Size(50, 19)
        Me.nudPx.TabIndex = 84
        Me.nudPx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cbxInclude)
        Me.GroupBox1.Controls.Add(Me.btnClone)
        Me.GroupBox1.Controls.Add(Me.btnAddObject)
        Me.GroupBox1.Controls.Add(Me.btnRemove)
        Me.GroupBox1.Controls.Add(Me.cbxSurfaces)
        Me.GroupBox1.Controls.Add(Me.tbxName)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnEdit)
        Me.GroupBox1.Controls.Add(Me.lblMeshInfo)
        Me.GroupBox1.Controls.Add(Me.cbxShowMesh)
        Me.GroupBox1.Controls.Add(Me.cbxShowSurface)
        Me.GroupBox1.Controls.Add(Me.pnlMeshColor)
        Me.GroupBox1.Controls.Add(Me.pnlSurfaceColor)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(143, 3)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(355, 139)
        Me.GroupBox1.TabIndex = 505
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Components"
        '
        'cbxInclude
        '
        Me.cbxInclude.AutoSize = True
        Me.cbxInclude.Checked = True
        Me.cbxInclude.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxInclude.Location = New System.Drawing.Point(250, 63)
        Me.cbxInclude.Name = "cbxInclude"
        Me.cbxInclude.Size = New System.Drawing.Size(52, 16)
        Me.cbxInclude.TabIndex = 73
        Me.cbxInclude.Text = "Include"
        Me.cbxInclude.UseVisualStyleBackColor = True
        '
        'btnClone
        '
        Me.btnClone.BackColor = System.Drawing.Color.White
        Me.btnClone.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnClone.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnClone.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnClone.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnClone.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnClone.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClone.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnClone.Location = New System.Drawing.Point(6, 40)
        Me.btnClone.Name = "btnClone"
        Me.btnClone.Size = New System.Drawing.Size(60, 22)
        Me.btnClone.TabIndex = 67
        Me.btnClone.Text = "Clone"
        Me.btnClone.UseVisualStyleBackColor = False
        '
        'btnAddObject
        '
        Me.btnAddObject.BackColor = System.Drawing.Color.White
        Me.btnAddObject.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnAddObject.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAddObject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnAddObject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnAddObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddObject.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddObject.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAddObject.Location = New System.Drawing.Point(6, 17)
        Me.btnAddObject.Name = "btnAddObject"
        Me.btnAddObject.Size = New System.Drawing.Size(60, 22)
        Me.btnAddObject.TabIndex = 61
        Me.btnAddObject.Text = "Add"
        Me.btnAddObject.UseVisualStyleBackColor = False
        '
        'btnRemove
        '
        Me.btnRemove.BackColor = System.Drawing.Color.White
        Me.btnRemove.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnRemove.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnRemove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRemove.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnRemove.Location = New System.Drawing.Point(6, 63)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(60, 22)
        Me.btnRemove.TabIndex = 62
        Me.btnRemove.Text = "Remove"
        Me.btnRemove.UseVisualStyleBackColor = False
        '
        'cbxSurfaces
        '
        Me.cbxSurfaces.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxSurfaces.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxSurfaces.FormattingEnabled = True
        Me.cbxSurfaces.Location = New System.Drawing.Point(72, 17)
        Me.cbxSurfaces.Name = "cbxSurfaces"
        Me.cbxSurfaces.Size = New System.Drawing.Size(172, 21)
        Me.cbxSurfaces.TabIndex = 63
        '
        'tbxName
        '
        Me.tbxName.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbxName.Location = New System.Drawing.Point(106, 41)
        Me.tbxName.Name = "tbxName"
        Me.tbxName.Size = New System.Drawing.Size(138, 19)
        Me.tbxName.TabIndex = 64
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(70, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 12)
        Me.Label1.TabIndex = 65
        Me.Label1.Text = "Name"
        '
        'btnEdit
        '
        Me.btnEdit.BackColor = System.Drawing.Color.White
        Me.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnEdit.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnEdit.Location = New System.Drawing.Point(184, 63)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(60, 22)
        Me.btnEdit.TabIndex = 66
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.UseVisualStyleBackColor = False
        '
        'lblMeshInfo
        '
        Me.lblMeshInfo.AutoSize = True
        Me.lblMeshInfo.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblMeshInfo.Location = New System.Drawing.Point(72, 68)
        Me.lblMeshInfo.Name = "lblMeshInfo"
        Me.lblMeshInfo.Size = New System.Drawing.Size(73, 12)
        Me.lblMeshInfo.TabIndex = 72
        Me.lblMeshInfo.Text = "0 nodes, 0 panels"
        Me.lblMeshInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbxShowMesh
        '
        Me.cbxShowMesh.AutoSize = True
        Me.cbxShowMesh.Checked = True
        Me.cbxShowMesh.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowMesh.Location = New System.Drawing.Point(250, 41)
        Me.cbxShowMesh.Name = "cbxShowMesh"
        Me.cbxShowMesh.Size = New System.Drawing.Size(70, 16)
        Me.cbxShowMesh.TabIndex = 71
        Me.cbxShowMesh.Text = "Show mesh"
        Me.cbxShowMesh.UseVisualStyleBackColor = True
        '
        'cbxShowSurface
        '
        Me.cbxShowSurface.AutoSize = True
        Me.cbxShowSurface.Checked = True
        Me.cbxShowSurface.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowSurface.Location = New System.Drawing.Point(250, 19)
        Me.cbxShowSurface.Name = "cbxShowSurface"
        Me.cbxShowSurface.Size = New System.Drawing.Size(77, 16)
        Me.cbxShowSurface.TabIndex = 69
        Me.cbxShowSurface.Text = "Show surface"
        Me.cbxShowSurface.UseVisualStyleBackColor = True
        '
        'pnlMeshColor
        '
        Me.pnlMeshColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMeshColor.Location = New System.Drawing.Point(328, 39)
        Me.pnlMeshColor.Name = "pnlMeshColor"
        Me.pnlMeshColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlMeshColor.TabIndex = 70
        '
        'pnlSurfaceColor
        '
        Me.pnlSurfaceColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSurfaceColor.Location = New System.Drawing.Point(328, 17)
        Me.pnlSurfaceColor.Name = "pnlSurfaceColor"
        Me.pnlSurfaceColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlSurfaceColor.TabIndex = 67
        '
        'pnlIO
        '
        Me.pnlIO.BackColor = System.Drawing.Color.Transparent
        Me.pnlIO.Controls.Add(Me.btnNew)
        Me.pnlIO.Controls.Add(Me.btnOpen)
        Me.pnlIO.Controls.Add(Me.btnSaveAs)
        Me.pnlIO.Controls.Add(Me.btnSave)
        Me.pnlIO.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlIO.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlIO.Location = New System.Drawing.Point(3, 3)
        Me.pnlIO.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.pnlIO.Name = "pnlIO"
        Me.pnlIO.Size = New System.Drawing.Size(134, 139)
        Me.pnlIO.TabIndex = 504
        Me.pnlIO.TabStop = False
        Me.pnlIO.Text = "IO"
        '
        'btnNew
        '
        Me.btnNew.BackColor = System.Drawing.Color.Gainsboro
        Me.btnNew.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnNew.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNew.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNew.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnNew.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnNew.Location = New System.Drawing.Point(6, 17)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(60, 22)
        Me.btnNew.TabIndex = 63
        Me.btnNew.Text = "New"
        Me.btnNew.UseVisualStyleBackColor = False
        '
        'btnOpen
        '
        Me.btnOpen.BackColor = System.Drawing.Color.Gainsboro
        Me.btnOpen.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOpen.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOpen.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpen.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnOpen.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnOpen.Location = New System.Drawing.Point(68, 17)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(60, 22)
        Me.btnOpen.TabIndex = 60
        Me.btnOpen.Text = "Open"
        Me.btnOpen.UseVisualStyleBackColor = False
        '
        'btnSaveAs
        '
        Me.btnSaveAs.BackColor = System.Drawing.Color.Gainsboro
        Me.btnSaveAs.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnSaveAs.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnSaveAs.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnSaveAs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSaveAs.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSaveAs.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnSaveAs.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSaveAs.Location = New System.Drawing.Point(68, 41)
        Me.btnSaveAs.Name = "btnSaveAs"
        Me.btnSaveAs.Size = New System.Drawing.Size(60, 22)
        Me.btnSaveAs.TabIndex = 62
        Me.btnSaveAs.Text = "Save as..."
        Me.btnSaveAs.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.Gainsboro
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnSave.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSave.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSave.Location = New System.Drawing.Point(6, 41)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(60, 22)
        Me.btnSave.TabIndex = 61
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'tpCalculation
        '
        Me.tpCalculation.Controls.Add(Me.TableLayoutPanel2)
        Me.tpCalculation.Location = New System.Drawing.Point(4, 22)
        Me.tpCalculation.Name = "tpCalculation"
        Me.tpCalculation.Size = New System.Drawing.Size(904, 143)
        Me.tpCalculation.TabIndex = 2
        Me.tpCalculation.Text = "Simulation"
        Me.tpCalculation.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 3
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox12, 3, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.GroupBox11, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.gbxCalculationType, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(904, 143)
        Me.TableLayoutPanel2.TabIndex = 522
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.Label28)
        Me.GroupBox12.Controls.Add(Me.nudIncrement)
        Me.GroupBox12.Controls.Add(Me.Label35)
        Me.GroupBox12.Controls.Add(Me.nudSteps)
        Me.GroupBox12.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox12.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox12.Location = New System.Drawing.Point(391, 3)
        Me.GroupBox12.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(178, 139)
        Me.GroupBox12.TabIndex = 522
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Simulation"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label28.Location = New System.Drawing.Point(7, 36)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(42, 12)
        Me.Label28.TabIndex = 97
        Me.Label28.Text = "Incement"
        Me.Label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudIncrement
        '
        Me.nudIncrement.DecimalPlaces = 5
        Me.nudIncrement.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.nudIncrement.Location = New System.Drawing.Point(66, 34)
        Me.nudIncrement.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIncrement.Name = "nudIncrement"
        Me.nudIncrement.Size = New System.Drawing.Size(64, 19)
        Me.nudIncrement.TabIndex = 96
        Me.nudIncrement.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label35.Location = New System.Drawing.Point(7, 18)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(27, 12)
        Me.Label35.TabIndex = 93
        Me.Label35.Text = "Steps"
        Me.Label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudSteps
        '
        Me.nudSteps.Location = New System.Drawing.Point(66, 16)
        Me.nudSteps.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudSteps.Name = "nudSteps"
        Me.nudSteps.Size = New System.Drawing.Size(64, 19)
        Me.nudSteps.TabIndex = 90
        Me.nudSteps.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.btnHistogram)
        Me.GroupBox11.Controls.Add(Me.Label27)
        Me.GroupBox11.Controls.Add(Me.nudViscosity)
        Me.GroupBox11.Controls.Add(Me.Label26)
        Me.GroupBox11.Controls.Add(Me.nudDensity)
        Me.GroupBox11.Controls.Add(Me.Label23)
        Me.GroupBox11.Controls.Add(Me.Label24)
        Me.GroupBox11.Controls.Add(Me.Label25)
        Me.GroupBox11.Controls.Add(Me.nudOz)
        Me.GroupBox11.Controls.Add(Me.nudOy)
        Me.GroupBox11.Controls.Add(Me.nudOx)
        Me.GroupBox11.Controls.Add(Me.Label20)
        Me.GroupBox11.Controls.Add(Me.Label21)
        Me.GroupBox11.Controls.Add(Me.Label22)
        Me.GroupBox11.Controls.Add(Me.nudVz)
        Me.GroupBox11.Controls.Add(Me.nudVy)
        Me.GroupBox11.Controls.Add(Me.nudVx)
        Me.GroupBox11.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox11.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox11.Location = New System.Drawing.Point(137, 3)
        Me.GroupBox11.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(248, 139)
        Me.GroupBox11.TabIndex = 521
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "Stream"
        '
        'btnHistogram
        '
        Me.btnHistogram.BackColor = System.Drawing.Color.White
        Me.btnHistogram.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnHistogram.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnHistogram.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnHistogram.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnHistogram.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHistogram.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHistogram.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnHistogram.Location = New System.Drawing.Point(182, 59)
        Me.btnHistogram.Name = "btnHistogram"
        Me.btnHistogram.Size = New System.Drawing.Size(60, 22)
        Me.btnHistogram.TabIndex = 108
        Me.btnHistogram.Text = "Histogram"
        Me.btnHistogram.UseVisualStyleBackColor = False
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label27.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label27.Location = New System.Drawing.Point(172, 36)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(13, 13)
        Me.Label27.TabIndex = 105
        Me.Label27.Text = "m"
        Me.Label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudViscosity
        '
        Me.nudViscosity.DecimalPlaces = 4
        Me.nudViscosity.Location = New System.Drawing.Point(192, 34)
        Me.nudViscosity.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudViscosity.Name = "nudViscosity"
        Me.nudViscosity.Size = New System.Drawing.Size(50, 19)
        Me.nudViscosity.TabIndex = 104
        Me.nudViscosity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Symbol", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.Label26.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label26.Location = New System.Drawing.Point(172, 18)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(14, 13)
        Me.Label26.TabIndex = 103
        Me.Label26.Text = "r"
        Me.Label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudDensity
        '
        Me.nudDensity.DecimalPlaces = 3
        Me.nudDensity.Location = New System.Drawing.Point(192, 16)
        Me.nudDensity.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudDensity.Name = "nudDensity"
        Me.nudDensity.Size = New System.Drawing.Size(50, 19)
        Me.nudDensity.TabIndex = 102
        Me.nudDensity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label23.Location = New System.Drawing.Point(89, 53)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(16, 12)
        Me.Label23.TabIndex = 101
        Me.Label23.Text = "Ωz"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label24.Location = New System.Drawing.Point(89, 36)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(16, 12)
        Me.Label24.TabIndex = 100
        Me.Label24.Text = "Ωy"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label25.Location = New System.Drawing.Point(89, 18)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(16, 12)
        Me.Label25.TabIndex = 99
        Me.Label25.Text = "Ωx"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudOz
        '
        Me.nudOz.DecimalPlaces = 1
        Me.nudOz.Location = New System.Drawing.Point(108, 50)
        Me.nudOz.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudOz.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudOz.Name = "nudOz"
        Me.nudOz.Size = New System.Drawing.Size(50, 19)
        Me.nudOz.TabIndex = 98
        Me.nudOz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudOy
        '
        Me.nudOy.DecimalPlaces = 1
        Me.nudOy.Location = New System.Drawing.Point(108, 34)
        Me.nudOy.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudOy.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudOy.Name = "nudOy"
        Me.nudOy.Size = New System.Drawing.Size(50, 19)
        Me.nudOy.TabIndex = 97
        Me.nudOy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudOx
        '
        Me.nudOx.DecimalPlaces = 1
        Me.nudOx.Location = New System.Drawing.Point(108, 16)
        Me.nudOx.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudOx.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudOx.Name = "nudOx"
        Me.nudOx.Size = New System.Drawing.Size(50, 19)
        Me.nudOx.TabIndex = 96
        Me.nudOx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label20.Location = New System.Drawing.Point(8, 53)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(15, 12)
        Me.Label20.TabIndex = 95
        Me.Label20.Text = "Vz"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label21.Location = New System.Drawing.Point(8, 36)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(15, 12)
        Me.Label21.TabIndex = 94
        Me.Label21.Text = "Vy"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label22.Location = New System.Drawing.Point(8, 18)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(15, 12)
        Me.Label22.TabIndex = 93
        Me.Label22.Text = "Vx"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudVz
        '
        Me.nudVz.DecimalPlaces = 1
        Me.nudVz.Location = New System.Drawing.Point(27, 50)
        Me.nudVz.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudVz.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudVz.Name = "nudVz"
        Me.nudVz.Size = New System.Drawing.Size(50, 19)
        Me.nudVz.TabIndex = 92
        Me.nudVz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudVy
        '
        Me.nudVy.DecimalPlaces = 1
        Me.nudVy.Location = New System.Drawing.Point(27, 34)
        Me.nudVy.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudVy.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudVy.Name = "nudVy"
        Me.nudVy.Size = New System.Drawing.Size(50, 19)
        Me.nudVy.TabIndex = 91
        Me.nudVy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudVx
        '
        Me.nudVx.DecimalPlaces = 1
        Me.nudVx.Location = New System.Drawing.Point(27, 16)
        Me.nudVx.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudVx.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudVx.Name = "nudVx"
        Me.nudVx.Size = New System.Drawing.Size(50, 19)
        Me.nudVx.TabIndex = 90
        Me.nudVx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gbxCalculationType
        '
        Me.gbxCalculationType.Controls.Add(Me.btnStartCalculation)
        Me.gbxCalculationType.Controls.Add(Me.cbxSimulationMode)
        Me.gbxCalculationType.Dock = System.Windows.Forms.DockStyle.Left
        Me.gbxCalculationType.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbxCalculationType.Location = New System.Drawing.Point(3, 3)
        Me.gbxCalculationType.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.gbxCalculationType.Name = "gbxCalculationType"
        Me.gbxCalculationType.Size = New System.Drawing.Size(128, 139)
        Me.gbxCalculationType.TabIndex = 520
        Me.gbxCalculationType.TabStop = False
        Me.gbxCalculationType.Text = "Mode"
        '
        'btnStartCalculation
        '
        Me.btnStartCalculation.BackColor = System.Drawing.Color.White
        Me.btnStartCalculation.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnStartCalculation.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnStartCalculation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnStartCalculation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnStartCalculation.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnStartCalculation.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnStartCalculation.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnStartCalculation.Location = New System.Drawing.Point(6, 45)
        Me.btnStartCalculation.Name = "btnStartCalculation"
        Me.btnStartCalculation.Size = New System.Drawing.Size(60, 22)
        Me.btnStartCalculation.TabIndex = 107
        Me.btnStartCalculation.Text = "Start"
        Me.btnStartCalculation.UseVisualStyleBackColor = False
        '
        'cbxSimulationMode
        '
        Me.cbxSimulationMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxSimulationMode.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxSimulationMode.FormattingEnabled = True
        Me.cbxSimulationMode.Location = New System.Drawing.Point(6, 18)
        Me.cbxSimulationMode.Name = "cbxSimulationMode"
        Me.cbxSimulationMode.Size = New System.Drawing.Size(116, 21)
        Me.cbxSimulationMode.TabIndex = 63
        '
        'tpResults
        '
        Me.tpResults.BackColor = System.Drawing.Color.White
        Me.tpResults.Controls.Add(Me.TableLayoutPanel3)
        Me.tpResults.Location = New System.Drawing.Point(4, 22)
        Me.tpResults.Name = "tpResults"
        Me.tpResults.Size = New System.Drawing.Size(904, 143)
        Me.tpResults.TabIndex = 3
        Me.tpResults.Text = "Results"
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 7
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel3.Controls.Add(Me.gbxAeroelastic, 6, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox5, 5, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox10, 4, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox9, 3, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox8, 2, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox4, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.GroupBox6, 0, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 143.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(904, 143)
        Me.TableLayoutPanel3.TabIndex = 519
        '
        'gbxAeroelastic
        '
        Me.gbxAeroelastic.Controls.Add(Me.btnPlayStop)
        Me.gbxAeroelastic.Controls.Add(Me.Label19)
        Me.gbxAeroelastic.Controls.Add(Me.nudModeScale)
        Me.gbxAeroelastic.Controls.Add(Me.cbxModes)
        Me.gbxAeroelastic.Dock = System.Windows.Forms.DockStyle.Left
        Me.gbxAeroelastic.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbxAeroelastic.Location = New System.Drawing.Point(771, 3)
        Me.gbxAeroelastic.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.gbxAeroelastic.Name = "gbxAeroelastic"
        Me.gbxAeroelastic.Size = New System.Drawing.Size(128, 139)
        Me.gbxAeroelastic.TabIndex = 519
        Me.gbxAeroelastic.TabStop = False
        Me.gbxAeroelastic.Text = "Aeroelastic"
        '
        'btnPlayStop
        '
        Me.btnPlayStop.BackColor = System.Drawing.Color.White
        Me.btnPlayStop.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnPlayStop.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnPlayStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnPlayStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnPlayStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPlayStop.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPlayStop.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnPlayStop.Location = New System.Drawing.Point(62, 68)
        Me.btnPlayStop.Name = "btnPlayStop"
        Me.btnPlayStop.Size = New System.Drawing.Size(60, 22)
        Me.btnPlayStop.TabIndex = 107
        Me.btnPlayStop.Text = "Play"
        Me.btnPlayStop.UseVisualStyleBackColor = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label19.Location = New System.Drawing.Point(40, 46)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(26, 12)
        Me.Label19.TabIndex = 104
        Me.Label19.Text = "Scale"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudModeScale
        '
        Me.nudModeScale.DecimalPlaces = 1
        Me.nudModeScale.Location = New System.Drawing.Point(72, 44)
        Me.nudModeScale.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudModeScale.Name = "nudModeScale"
        Me.nudModeScale.Size = New System.Drawing.Size(50, 19)
        Me.nudModeScale.TabIndex = 98
        Me.nudModeScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudModeScale.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cbxModes
        '
        Me.cbxModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbxModes.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbxModes.FormattingEnabled = True
        Me.cbxModes.Location = New System.Drawing.Point(6, 18)
        Me.cbxModes.Name = "cbxModes"
        Me.cbxModes.Size = New System.Drawing.Size(116, 21)
        Me.cbxModes.TabIndex = 63
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.btnEditVelocityPlane)
        Me.GroupBox5.Controls.Add(Me.cbxShowVelocityPlane)
        Me.GroupBox5.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox5.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(690, 3)
        Me.GroupBox5.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(75, 139)
        Me.GroupBox5.TabIndex = 518
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Velocity plane"
        '
        'btnEditVelocityPlane
        '
        Me.btnEditVelocityPlane.BackColor = System.Drawing.Color.White
        Me.btnEditVelocityPlane.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnEditVelocityPlane.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnEditVelocityPlane.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnEditVelocityPlane.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnEditVelocityPlane.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnEditVelocityPlane.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEditVelocityPlane.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnEditVelocityPlane.Location = New System.Drawing.Point(6, 17)
        Me.btnEditVelocityPlane.Name = "btnEditVelocityPlane"
        Me.btnEditVelocityPlane.Size = New System.Drawing.Size(60, 22)
        Me.btnEditVelocityPlane.TabIndex = 66
        Me.btnEditVelocityPlane.Text = "Edit"
        Me.btnEditVelocityPlane.UseVisualStyleBackColor = False
        '
        'cbxShowVelocityPlane
        '
        Me.cbxShowVelocityPlane.AutoSize = True
        Me.cbxShowVelocityPlane.Checked = True
        Me.cbxShowVelocityPlane.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowVelocityPlane.Location = New System.Drawing.Point(6, 45)
        Me.cbxShowVelocityPlane.Name = "cbxShowVelocityPlane"
        Me.cbxShowVelocityPlane.Size = New System.Drawing.Size(46, 16)
        Me.cbxShowVelocityPlane.TabIndex = 69
        Me.cbxShowVelocityPlane.Text = "Show"
        Me.cbxShowVelocityPlane.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.nudScaleForce)
        Me.GroupBox10.Controls.Add(Me.cbxShowVelocity)
        Me.GroupBox10.Controls.Add(Me.nudScaleVelocity)
        Me.GroupBox10.Controls.Add(Me.pnlVelocityColor)
        Me.GroupBox10.Controls.Add(Me.cbxShowForce)
        Me.GroupBox10.Controls.Add(Me.pnlForceColor)
        Me.GroupBox10.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox10.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox10.Location = New System.Drawing.Point(516, 3)
        Me.GroupBox10.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(168, 139)
        Me.GroupBox10.TabIndex = 517
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Vectors"
        '
        'nudScaleForce
        '
        Me.nudScaleForce.DecimalPlaces = 2
        Me.nudScaleForce.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.nudScaleForce.Location = New System.Drawing.Point(110, 39)
        Me.nudScaleForce.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudScaleForce.Name = "nudScaleForce"
        Me.nudScaleForce.Size = New System.Drawing.Size(50, 19)
        Me.nudScaleForce.TabIndex = 98
        Me.nudScaleForce.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cbxShowVelocity
        '
        Me.cbxShowVelocity.AutoSize = True
        Me.cbxShowVelocity.Checked = True
        Me.cbxShowVelocity.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowVelocity.Location = New System.Drawing.Point(6, 18)
        Me.cbxShowVelocity.Name = "cbxShowVelocity"
        Me.cbxShowVelocity.Size = New System.Drawing.Size(77, 16)
        Me.cbxShowVelocity.TabIndex = 73
        Me.cbxShowVelocity.Text = "Show velocity"
        Me.cbxShowVelocity.UseVisualStyleBackColor = True
        '
        'nudScaleVelocity
        '
        Me.nudScaleVelocity.DecimalPlaces = 3
        Me.nudScaleVelocity.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudScaleVelocity.Location = New System.Drawing.Point(110, 17)
        Me.nudScaleVelocity.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudScaleVelocity.Name = "nudScaleVelocity"
        Me.nudScaleVelocity.Size = New System.Drawing.Size(50, 19)
        Me.nudScaleVelocity.TabIndex = 97
        Me.nudScaleVelocity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'pnlVelocityColor
        '
        Me.pnlVelocityColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlVelocityColor.Location = New System.Drawing.Point(84, 16)
        Me.pnlVelocityColor.Name = "pnlVelocityColor"
        Me.pnlVelocityColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlVelocityColor.TabIndex = 72
        '
        'cbxShowForce
        '
        Me.cbxShowForce.AutoSize = True
        Me.cbxShowForce.Checked = True
        Me.cbxShowForce.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowForce.Location = New System.Drawing.Point(6, 40)
        Me.cbxShowForce.Name = "cbxShowForce"
        Me.cbxShowForce.Size = New System.Drawing.Size(68, 16)
        Me.cbxShowForce.TabIndex = 75
        Me.cbxShowForce.Text = "Show force"
        Me.cbxShowForce.UseVisualStyleBackColor = True
        '
        'pnlForceColor
        '
        Me.pnlForceColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlForceColor.Location = New System.Drawing.Point(84, 38)
        Me.pnlForceColor.Name = "pnlForceColor"
        Me.pnlForceColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlForceColor.TabIndex = 74
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.Label29)
        Me.GroupBox9.Controls.Add(Me.nudCpmin)
        Me.GroupBox9.Controls.Add(Me.Label30)
        Me.GroupBox9.Controls.Add(Me.nudCpmax)
        Me.GroupBox9.Controls.Add(Me.cbxShowColormap)
        Me.GroupBox9.Controls.Add(Me.Label18)
        Me.GroupBox9.Controls.Add(Me.btnResetColormap)
        Me.GroupBox9.Controls.Add(Me.nudDCpmin)
        Me.GroupBox9.Controls.Add(Me.Label17)
        Me.GroupBox9.Controls.Add(Me.nudDCpmax)
        Me.GroupBox9.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox9.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox9.Location = New System.Drawing.Point(312, 3)
        Me.GroupBox9.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(198, 139)
        Me.GroupBox9.TabIndex = 516
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Colormap"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label29.Location = New System.Drawing.Point(107, 46)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(33, 12)
        Me.Label29.TabIndex = 107
        Me.Label29.Text = "Cp min"
        Me.Label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudCpmin
        '
        Me.nudCpmin.DecimalPlaces = 3
        Me.nudCpmin.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.nudCpmin.Location = New System.Drawing.Point(142, 44)
        Me.nudCpmin.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudCpmin.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudCpmin.Name = "nudCpmin"
        Me.nudCpmin.Size = New System.Drawing.Size(50, 19)
        Me.nudCpmin.TabIndex = 105
        Me.nudCpmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label30.Location = New System.Drawing.Point(107, 67)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(35, 12)
        Me.Label30.TabIndex = 106
        Me.Label30.Text = "Cp max"
        Me.Label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudCpmax
        '
        Me.nudCpmax.DecimalPlaces = 3
        Me.nudCpmax.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.nudCpmax.Location = New System.Drawing.Point(142, 65)
        Me.nudCpmax.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudCpmax.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudCpmax.Name = "nudCpmax"
        Me.nudCpmax.Size = New System.Drawing.Size(50, 19)
        Me.nudCpmax.TabIndex = 104
        Me.nudCpmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cbxShowColormap
        '
        Me.cbxShowColormap.AutoSize = True
        Me.cbxShowColormap.Checked = True
        Me.cbxShowColormap.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowColormap.Location = New System.Drawing.Point(12, 18)
        Me.cbxShowColormap.Name = "cbxShowColormap"
        Me.cbxShowColormap.Size = New System.Drawing.Size(85, 16)
        Me.cbxShowColormap.TabIndex = 99
        Me.cbxShowColormap.Text = "Show colormap"
        Me.cbxShowColormap.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label18.Location = New System.Drawing.Point(5, 46)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(39, 12)
        Me.Label18.TabIndex = 103
        Me.Label18.Text = "ΔCp min"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnResetColormap
        '
        Me.btnResetColormap.BackColor = System.Drawing.Color.White
        Me.btnResetColormap.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnResetColormap.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnResetColormap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnResetColormap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnResetColormap.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnResetColormap.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnResetColormap.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnResetColormap.Location = New System.Drawing.Point(132, 14)
        Me.btnResetColormap.Name = "btnResetColormap"
        Me.btnResetColormap.Size = New System.Drawing.Size(60, 22)
        Me.btnResetColormap.TabIndex = 66
        Me.btnResetColormap.Text = "Reset"
        Me.btnResetColormap.UseVisualStyleBackColor = False
        '
        'nudDCpmin
        '
        Me.nudDCpmin.DecimalPlaces = 3
        Me.nudDCpmin.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.nudDCpmin.Location = New System.Drawing.Point(46, 44)
        Me.nudDCpmin.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudDCpmin.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudDCpmin.Name = "nudDCpmin"
        Me.nudDCpmin.Size = New System.Drawing.Size(50, 19)
        Me.nudDCpmin.TabIndex = 101
        Me.nudDCpmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label17.Location = New System.Drawing.Point(5, 67)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(41, 12)
        Me.Label17.TabIndex = 102
        Me.Label17.Text = "ΔCp max"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudDCpmax
        '
        Me.nudDCpmax.DecimalPlaces = 3
        Me.nudDCpmax.Increment = New Decimal(New Integer() {1, 0, 0, 196608})
        Me.nudDCpmax.Location = New System.Drawing.Point(46, 65)
        Me.nudDCpmax.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudDCpmax.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudDCpmax.Name = "nudDCpmax"
        Me.nudDCpmax.Size = New System.Drawing.Size(50, 19)
        Me.nudDCpmax.TabIndex = 100
        Me.nudDCpmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.cbxShowWakeNodes)
        Me.GroupBox8.Controls.Add(Me.pnlWakeNodeColor)
        Me.GroupBox8.Controls.Add(Me.cbxShowWakeMesh)
        Me.GroupBox8.Controls.Add(Me.cbxShowWakeSurface)
        Me.GroupBox8.Controls.Add(Me.pnlWakeMeshColor)
        Me.GroupBox8.Controls.Add(Me.pnlWakeSurfaceColor)
        Me.GroupBox8.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox8.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox8.Location = New System.Drawing.Point(197, 3)
        Me.GroupBox8.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(109, 139)
        Me.GroupBox8.TabIndex = 513
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "Wakes"
        '
        'cbxShowWakeNodes
        '
        Me.cbxShowWakeNodes.AutoSize = True
        Me.cbxShowWakeNodes.Checked = True
        Me.cbxShowWakeNodes.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowWakeNodes.Location = New System.Drawing.Point(6, 62)
        Me.cbxShowWakeNodes.Name = "cbxShowWakeNodes"
        Me.cbxShowWakeNodes.Size = New System.Drawing.Size(72, 16)
        Me.cbxShowWakeNodes.TabIndex = 73
        Me.cbxShowWakeNodes.Text = "Show nodes"
        Me.cbxShowWakeNodes.UseVisualStyleBackColor = True
        '
        'pnlWakeNodeColor
        '
        Me.pnlWakeNodeColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlWakeNodeColor.Location = New System.Drawing.Point(84, 60)
        Me.pnlWakeNodeColor.Name = "pnlWakeNodeColor"
        Me.pnlWakeNodeColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlWakeNodeColor.TabIndex = 72
        '
        'cbxShowWakeMesh
        '
        Me.cbxShowWakeMesh.AutoSize = True
        Me.cbxShowWakeMesh.Checked = True
        Me.cbxShowWakeMesh.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowWakeMesh.Location = New System.Drawing.Point(6, 40)
        Me.cbxShowWakeMesh.Name = "cbxShowWakeMesh"
        Me.cbxShowWakeMesh.Size = New System.Drawing.Size(70, 16)
        Me.cbxShowWakeMesh.TabIndex = 71
        Me.cbxShowWakeMesh.Text = "Show mesh"
        Me.cbxShowWakeMesh.UseVisualStyleBackColor = True
        '
        'cbxShowWakeSurface
        '
        Me.cbxShowWakeSurface.AutoSize = True
        Me.cbxShowWakeSurface.Checked = True
        Me.cbxShowWakeSurface.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowWakeSurface.Location = New System.Drawing.Point(6, 18)
        Me.cbxShowWakeSurface.Name = "cbxShowWakeSurface"
        Me.cbxShowWakeSurface.Size = New System.Drawing.Size(77, 16)
        Me.cbxShowWakeSurface.TabIndex = 69
        Me.cbxShowWakeSurface.Text = "Show surface"
        Me.cbxShowWakeSurface.UseVisualStyleBackColor = True
        '
        'pnlWakeMeshColor
        '
        Me.pnlWakeMeshColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlWakeMeshColor.Location = New System.Drawing.Point(84, 38)
        Me.pnlWakeMeshColor.Name = "pnlWakeMeshColor"
        Me.pnlWakeMeshColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlWakeMeshColor.TabIndex = 70
        '
        'pnlWakeSurfaceColor
        '
        Me.pnlWakeSurfaceColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlWakeSurfaceColor.Location = New System.Drawing.Point(84, 16)
        Me.pnlWakeSurfaceColor.Name = "pnlWakeSurfaceColor"
        Me.pnlWakeSurfaceColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlWakeSurfaceColor.TabIndex = 67
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.cbxShowResMesh)
        Me.GroupBox4.Controls.Add(Me.cbxShowResSurface)
        Me.GroupBox4.Controls.Add(Me.pnlResultMeshColor)
        Me.GroupBox4.Controls.Add(Me.pnlResultSurfaceColor)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox4.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox4.Location = New System.Drawing.Point(82, 3)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(109, 139)
        Me.GroupBox4.TabIndex = 512
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Bounded lattices"
        '
        'cbxShowResMesh
        '
        Me.cbxShowResMesh.AutoSize = True
        Me.cbxShowResMesh.Checked = True
        Me.cbxShowResMesh.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowResMesh.Location = New System.Drawing.Point(6, 40)
        Me.cbxShowResMesh.Name = "cbxShowResMesh"
        Me.cbxShowResMesh.Size = New System.Drawing.Size(70, 16)
        Me.cbxShowResMesh.TabIndex = 71
        Me.cbxShowResMesh.Text = "Show mesh"
        Me.cbxShowResMesh.UseVisualStyleBackColor = True
        '
        'cbxShowResSurface
        '
        Me.cbxShowResSurface.AutoSize = True
        Me.cbxShowResSurface.Checked = True
        Me.cbxShowResSurface.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowResSurface.Location = New System.Drawing.Point(6, 18)
        Me.cbxShowResSurface.Name = "cbxShowResSurface"
        Me.cbxShowResSurface.Size = New System.Drawing.Size(77, 16)
        Me.cbxShowResSurface.TabIndex = 69
        Me.cbxShowResSurface.Text = "Show surface"
        Me.cbxShowResSurface.UseVisualStyleBackColor = True
        '
        'pnlResultMeshColor
        '
        Me.pnlResultMeshColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlResultMeshColor.Location = New System.Drawing.Point(84, 38)
        Me.pnlResultMeshColor.Name = "pnlResultMeshColor"
        Me.pnlResultMeshColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlResultMeshColor.TabIndex = 70
        '
        'pnlResultSurfaceColor
        '
        Me.pnlResultSurfaceColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlResultSurfaceColor.Location = New System.Drawing.Point(84, 16)
        Me.pnlResultSurfaceColor.Name = "pnlResultSurfaceColor"
        Me.pnlResultSurfaceColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlResultSurfaceColor.TabIndex = 67
        '
        'GroupBox6
        '
        Me.GroupBox6.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox6.Controls.Add(Me.btnReport)
        Me.GroupBox6.Controls.Add(Me.btnLoadResults)
        Me.GroupBox6.Dock = System.Windows.Forms.DockStyle.Left
        Me.GroupBox6.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox6.Margin = New System.Windows.Forms.Padding(3, 3, 3, 1)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(73, 139)
        Me.GroupBox6.TabIndex = 511
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "IO"
        '
        'btnReport
        '
        Me.btnReport.BackColor = System.Drawing.Color.Gainsboro
        Me.btnReport.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnReport.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnReport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnReport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReport.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReport.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnReport.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnReport.Location = New System.Drawing.Point(6, 41)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(60, 22)
        Me.btnReport.TabIndex = 61
        Me.btnReport.Text = "Report"
        Me.btnReport.UseVisualStyleBackColor = False
        '
        'btnLoadResults
        '
        Me.btnLoadResults.BackColor = System.Drawing.Color.Gainsboro
        Me.btnLoadResults.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnLoadResults.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnLoadResults.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnLoadResults.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnLoadResults.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLoadResults.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLoadResults.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.btnLoadResults.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnLoadResults.Location = New System.Drawing.Point(6, 17)
        Me.btnLoadResults.Name = "btnLoadResults"
        Me.btnLoadResults.Size = New System.Drawing.Size(60, 22)
        Me.btnLoadResults.TabIndex = 60
        Me.btnLoadResults.Text = "Load"
        Me.btnLoadResults.UseVisualStyleBackColor = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbStructure)
        Me.GroupBox3.Controls.Add(Me.cbMultiselect)
        Me.GroupBox3.Controls.Add(Me.rnPanel)
        Me.GroupBox3.Controls.Add(Me.rbNode)
        Me.GroupBox3.Controls.Add(Me.Label31)
        Me.GroupBox3.Controls.Add(Me.btnFrontView)
        Me.GroupBox3.Controls.Add(Me.btnTopView)
        Me.GroupBox3.Controls.Add(Me.btnSideView)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.Label6)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.nudYmin)
        Me.GroupBox3.Controls.Add(Me.nudYmax)
        Me.GroupBox3.Controls.Add(Me.nudXmin)
        Me.GroupBox3.Controls.Add(Me.nudXmax)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.cbxShowRulers)
        Me.GroupBox3.Controls.Add(Me.pnlScreenColor)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox3.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(921, 17)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(3, 17, 3, 0)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(280, 158)
        Me.GroupBox3.TabIndex = 507
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Screen"
        '
        'rbStructure
        '
        Me.rbStructure.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbStructure.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.rbStructure.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.rbStructure.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime
        Me.rbStructure.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.rbStructure.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.rbStructure.Location = New System.Drawing.Point(54, 90)
        Me.rbStructure.Name = "rbStructure"
        Me.rbStructure.Size = New System.Drawing.Size(22, 22)
        Me.rbStructure.TabIndex = 97
        Me.rbStructure.Text = "S"
        Me.rbStructure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbStructure.UseVisualStyleBackColor = True
        '
        'cbMultiselect
        '
        Me.cbMultiselect.AutoSize = True
        Me.cbMultiselect.Location = New System.Drawing.Point(82, 94)
        Me.cbMultiselect.Name = "cbMultiselect"
        Me.cbMultiselect.Size = New System.Drawing.Size(56, 16)
        Me.cbMultiselect.TabIndex = 96
        Me.cbMultiselect.Text = "multiple"
        Me.cbMultiselect.UseVisualStyleBackColor = True
        '
        'rnPanel
        '
        Me.rnPanel.Appearance = System.Windows.Forms.Appearance.Button
        Me.rnPanel.Checked = True
        Me.rnPanel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.rnPanel.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.rnPanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime
        Me.rnPanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.rnPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.rnPanel.Location = New System.Drawing.Point(31, 90)
        Me.rnPanel.Name = "rnPanel"
        Me.rnPanel.Size = New System.Drawing.Size(22, 22)
        Me.rnPanel.TabIndex = 94
        Me.rnPanel.TabStop = True
        Me.rnPanel.Text = "P"
        Me.rnPanel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rnPanel.UseVisualStyleBackColor = True
        '
        'rbNode
        '
        Me.rbNode.Appearance = System.Windows.Forms.Appearance.Button
        Me.rbNode.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.rbNode.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.rbNode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime
        Me.rbNode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.rbNode.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.rbNode.Location = New System.Drawing.Point(8, 90)
        Me.rbNode.Name = "rbNode"
        Me.rbNode.Size = New System.Drawing.Size(22, 22)
        Me.rbNode.TabIndex = 92
        Me.rbNode.Text = "N"
        Me.rbNode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.rbNode.UseVisualStyleBackColor = True
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label31.Location = New System.Drawing.Point(6, 75)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(41, 12)
        Me.Label31.TabIndex = 91
        Me.Label31.Text = "Selection"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnFrontView
        '
        Me.btnFrontView.BackColor = System.Drawing.Color.White
        Me.btnFrontView.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnFrontView.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnFrontView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnFrontView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnFrontView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFrontView.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFrontView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnFrontView.Location = New System.Drawing.Point(182, 36)
        Me.btnFrontView.Name = "btnFrontView"
        Me.btnFrontView.Size = New System.Drawing.Size(40, 22)
        Me.btnFrontView.TabIndex = 87
        Me.btnFrontView.Text = "Front"
        Me.btnFrontView.UseVisualStyleBackColor = False
        '
        'btnTopView
        '
        Me.btnTopView.BackColor = System.Drawing.Color.White
        Me.btnTopView.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnTopView.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnTopView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnTopView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnTopView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnTopView.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnTopView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnTopView.Location = New System.Drawing.Point(182, 13)
        Me.btnTopView.Name = "btnTopView"
        Me.btnTopView.Size = New System.Drawing.Size(40, 22)
        Me.btnTopView.TabIndex = 85
        Me.btnTopView.Text = "Top"
        Me.btnTopView.UseVisualStyleBackColor = False
        '
        'btnSideView
        '
        Me.btnSideView.BackColor = System.Drawing.Color.White
        Me.btnSideView.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnSideView.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnSideView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Red
        Me.btnSideView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnSideView.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSideView.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSideView.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSideView.Location = New System.Drawing.Point(182, 59)
        Me.btnSideView.Name = "btnSideView"
        Me.btnSideView.Size = New System.Drawing.Size(40, 22)
        Me.btnSideView.TabIndex = 86
        Me.btnSideView.Text = "Side"
        Me.btnSideView.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label7.Location = New System.Drawing.Point(93, 68)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(24, 12)
        Me.Label7.TabIndex = 84
        Me.Label7.Text = "Ymin"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label8.Location = New System.Drawing.Point(93, 50)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(26, 12)
        Me.Label8.TabIndex = 83
        Me.Label8.Text = "Ymax"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label6.Location = New System.Drawing.Point(93, 33)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(25, 12)
        Me.Label6.TabIndex = 82
        Me.Label6.Text = "Xmin"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label5.Location = New System.Drawing.Point(93, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(27, 12)
        Me.Label5.TabIndex = 81
        Me.Label5.Text = "Xmax"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'nudYmin
        '
        Me.nudYmin.DecimalPlaces = 1
        Me.nudYmin.Location = New System.Drawing.Point(126, 65)
        Me.nudYmin.Maximum = New Decimal(New Integer() {0, 0, 0, 0})
        Me.nudYmin.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudYmin.Name = "nudYmin"
        Me.nudYmin.Size = New System.Drawing.Size(50, 19)
        Me.nudYmin.TabIndex = 80
        Me.nudYmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudYmax
        '
        Me.nudYmax.DecimalPlaces = 1
        Me.nudYmax.Location = New System.Drawing.Point(126, 47)
        Me.nudYmax.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudYmax.Name = "nudYmax"
        Me.nudYmax.Size = New System.Drawing.Size(50, 19)
        Me.nudYmax.TabIndex = 79
        Me.nudYmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudXmin
        '
        Me.nudXmin.DecimalPlaces = 1
        Me.nudXmin.Location = New System.Drawing.Point(126, 31)
        Me.nudXmin.Maximum = New Decimal(New Integer() {0, 0, 0, 0})
        Me.nudXmin.Minimum = New Decimal(New Integer() {1000, 0, 0, -2147483648})
        Me.nudXmin.Name = "nudXmin"
        Me.nudXmin.Size = New System.Drawing.Size(50, 19)
        Me.nudXmin.TabIndex = 78
        Me.nudXmin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudXmax
        '
        Me.nudXmax.DecimalPlaces = 1
        Me.nudXmax.Location = New System.Drawing.Point(126, 13)
        Me.nudXmax.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudXmax.Name = "nudXmax"
        Me.nudXmax.Size = New System.Drawing.Size(50, 19)
        Me.nudXmax.TabIndex = 77
        Me.nudXmax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label4.Location = New System.Drawing.Point(6, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 12)
        Me.Label4.TabIndex = 76
        Me.Label4.Text = "Screen color"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cbxShowRulers
        '
        Me.cbxShowRulers.AutoSize = True
        Me.cbxShowRulers.Checked = True
        Me.cbxShowRulers.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbxShowRulers.Location = New System.Drawing.Point(8, 41)
        Me.cbxShowRulers.Name = "cbxShowRulers"
        Me.cbxShowRulers.Size = New System.Drawing.Size(70, 16)
        Me.cbxShowRulers.TabIndex = 75
        Me.cbxShowRulers.Text = "Show rulers"
        Me.cbxShowRulers.UseVisualStyleBackColor = True
        '
        'pnlScreenColor
        '
        Me.pnlScreenColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlScreenColor.Location = New System.Drawing.Point(65, 17)
        Me.pnlScreenColor.Name = "pnlScreenColor"
        Me.pnlScreenColor.Size = New System.Drawing.Size(20, 20)
        Me.pnlScreenColor.TabIndex = 72
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.24585!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.75415!))
        Me.TableLayoutPanel4.Controls.Add(Me.GroupBox3, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.tcRibbon, 0, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(2, 2)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(1204, 175)
        Me.TableLayoutPanel4.TabIndex = 508
        '
        'MainRibbon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel4)
        Me.Name = "MainRibbon"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.Size = New System.Drawing.Size(1208, 179)
        Me.tcRibbon.ResumeLayout(False)
        Me.tpModel.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.nudFi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTita, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPsi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCRz, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCRy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCRx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPz, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPx, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlIO.ResumeLayout(False)
        Me.tpCalculation.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        CType(Me.nudIncrement, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSteps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        CType(Me.nudViscosity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDensity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudOz, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudOy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudOx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudVz, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudVy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudVx, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbxCalculationType.ResumeLayout(False)
        Me.tpResults.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.gbxAeroelastic.ResumeLayout(False)
        Me.gbxAeroelastic.PerformLayout()
        CType(Me.nudModeScale, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        CType(Me.nudScaleForce, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudScaleVelocity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        CType(Me.nudCpmin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCpmax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDCpmin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDCpmax, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox8.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.nudYmin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudYmax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudXmin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudXmax, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tcRibbon As System.Windows.Forms.TabControl
    Friend WithEvents tpModel As System.Windows.Forms.TabPage
    Friend WithEvents tpCalculation As System.Windows.Forms.TabPage
    Friend WithEvents tpResults As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnFrontView As System.Windows.Forms.Button
    Friend WithEvents btnTopView As System.Windows.Forms.Button
    Friend WithEvents btnSideView As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents nudYmin As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudYmax As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudXmin As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudXmax As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbxShowRulers As System.Windows.Forms.CheckBox
    Friend WithEvents pnlScreenColor As System.Windows.Forms.Panel
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbSecuence As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents nudFi As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudTita As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudPsi As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents nudCRz As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudCRy As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudCRx As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents nudPz As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudPy As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudPx As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnClone As System.Windows.Forms.Button
    Friend WithEvents btnAddObject As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents cbxSurfaces As System.Windows.Forms.ComboBox
    Friend WithEvents tbxName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents lblMeshInfo As System.Windows.Forms.Label
    Friend WithEvents cbxShowMesh As System.Windows.Forms.CheckBox
    Friend WithEvents cbxShowSurface As System.Windows.Forms.CheckBox
    Friend WithEvents pnlMeshColor As System.Windows.Forms.Panel
    Friend WithEvents pnlSurfaceColor As System.Windows.Forms.Panel
    Friend WithEvents pnlIO As System.Windows.Forms.GroupBox
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnSaveAs As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents nudIncrement As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents nudSteps As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents btnHistogram As System.Windows.Forms.Button
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents nudViscosity As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents nudDensity As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents nudOz As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudOy As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudOx As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents nudVz As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudVy As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudVx As System.Windows.Forms.NumericUpDown
    Friend WithEvents gbxCalculationType As System.Windows.Forms.GroupBox
    Friend WithEvents btnStartCalculation As System.Windows.Forms.Button
    Friend WithEvents cbxSimulationMode As System.Windows.Forms.ComboBox
    Friend WithEvents TableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents gbxAeroelastic As System.Windows.Forms.GroupBox
    Friend WithEvents btnPlayStop As System.Windows.Forms.Button
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents nudModeScale As System.Windows.Forms.NumericUpDown
    Friend WithEvents cbxModes As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents btnEditVelocityPlane As System.Windows.Forms.Button
    Friend WithEvents cbxShowVelocityPlane As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents nudScaleForce As System.Windows.Forms.NumericUpDown
    Friend WithEvents cbxShowVelocity As System.Windows.Forms.CheckBox
    Friend WithEvents nudScaleVelocity As System.Windows.Forms.NumericUpDown
    Friend WithEvents pnlVelocityColor As System.Windows.Forms.Panel
    Friend WithEvents cbxShowForce As System.Windows.Forms.CheckBox
    Friend WithEvents pnlForceColor As System.Windows.Forms.Panel
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents cbxShowColormap As System.Windows.Forms.CheckBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents btnResetColormap As System.Windows.Forms.Button
    Friend WithEvents nudDCpmin As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents nudDCpmax As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents cbxShowWakeNodes As System.Windows.Forms.CheckBox
    Friend WithEvents pnlWakeNodeColor As System.Windows.Forms.Panel
    Friend WithEvents cbxShowWakeMesh As System.Windows.Forms.CheckBox
    Friend WithEvents cbxShowWakeSurface As System.Windows.Forms.CheckBox
    Friend WithEvents pnlWakeMeshColor As System.Windows.Forms.Panel
    Friend WithEvents pnlWakeSurfaceColor As System.Windows.Forms.Panel
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents cbxShowResMesh As System.Windows.Forms.CheckBox
    Friend WithEvents cbxShowResSurface As System.Windows.Forms.CheckBox
    Friend WithEvents pnlResultMeshColor As System.Windows.Forms.Panel
    Friend WithEvents pnlResultSurfaceColor As System.Windows.Forms.Panel
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents btnReport As System.Windows.Forms.Button
    Friend WithEvents btnLoadResults As System.Windows.Forms.Button
    Friend WithEvents cbxInclude As System.Windows.Forms.CheckBox
    Friend WithEvents Label29 As Windows.Forms.Label
    Friend WithEvents nudCpmin As Windows.Forms.NumericUpDown
    Friend WithEvents Label30 As Windows.Forms.Label
    Friend WithEvents nudCpmax As Windows.Forms.NumericUpDown
    Friend WithEvents TableLayoutPanel4 As Windows.Forms.TableLayoutPanel
    Friend WithEvents Label31 As Windows.Forms.Label
    Friend WithEvents rnPanel As Windows.Forms.RadioButton
    Friend WithEvents rbNode As Windows.Forms.RadioButton
    Friend WithEvents cbMultiselect As Windows.Forms.CheckBox
    Friend WithEvents rbStructure As Windows.Forms.RadioButton
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WingControl
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
        Me.btnLockSurface = New System.Windows.Forms.Button()
        Me.btnHideSurface = New System.Windows.Forms.Button()
        Me.PanelDeEdicion = New System.Windows.Forms.TabControl()
        Me.tbDesign = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnDeletePanel = New System.Windows.Forms.Button()
        Me.btnAddPanel = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudSelectRegion = New System.Windows.Forms.NumericUpDown()
        Me.btnInsertPanel = New System.Windows.Forms.Button()
        Me.tcMacroPanelProperties = New System.Windows.Forms.TabControl()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.nudChordwisePanels = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cbSymetricWing = New System.Windows.Forms.CheckBox()
        Me.rbConstantSpacement = New System.Windows.Forms.RadioButton()
        Me.rbLinearSpacement = New System.Windows.Forms.RadioButton()
        Me.Label40 = New System.Windows.Forms.Label()
        Me.Label39 = New System.Windows.Forms.Label()
        Me.nudTwistingAxis = New System.Windows.Forms.NumericUpDown()
        Me.nudTwist = New System.Windows.Forms.NumericUpDown()
        Me.nudDihedral = New System.Windows.Forms.NumericUpDown()
        Me.nudSweepback = New System.Windows.Forms.NumericUpDown()
        Me.nudLength = New System.Windows.Forms.NumericUpDown()
        Me.nudTipChord = New System.Windows.Forms.NumericUpDown()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.nudSpanwiseRings = New System.Windows.Forms.NumericUpDown()
        Me.nudRootChord = New System.Windows.Forms.NumericUpDown()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.lblPolarName = New System.Windows.Forms.Label()
        Me.lblCamberLineName = New System.Windows.Forms.Label()
        Me.btnCamberLines = New System.Windows.Forms.Button()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.btnPolarCurves = New System.Windows.Forms.Button()
        Me.Label72 = New System.Windows.Forms.Label()
        Me.pbProfileSketch = New System.Windows.Forms.PictureBox()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.cbTrailingEdge = New System.Windows.Forms.CheckBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.nudCuttingStep = New System.Windows.Forms.NumericUpDown()
        Me.cbConvectWake = New System.Windows.Forms.CheckBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.cbShowPriitives = New System.Windows.Forms.CheckBox()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.nudLastPrimitive = New System.Windows.Forms.NumericUpDown()
        Me.nudFirstPrimitive = New System.Windows.Forms.NumericUpDown()
        Me.TabPage10 = New System.Windows.Forms.TabPage()
        Me.Label73 = New System.Windows.Forms.Label()
        Me.nudFlapPanels = New System.Windows.Forms.NumericUpDown()
        Me.Label67 = New System.Windows.Forms.Label()
        Me.Label71 = New System.Windows.Forms.Label()
        Me.nudRootFlap = New System.Windows.Forms.NumericUpDown()
        Me.Label66 = New System.Windows.Forms.Label()
        Me.Label65 = New System.Windows.Forms.Label()
        Me.Label61 = New System.Windows.Forms.Label()
        Me.Label60 = New System.Windows.Forms.Label()
        Me.nudFlapDeflection = New System.Windows.Forms.NumericUpDown()
        Me.nudFlapChord = New System.Windows.Forms.NumericUpDown()
        Me.cbFlapped = New System.Windows.Forms.CheckBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.rbTipSection = New System.Windows.Forms.RadioButton()
        Me.rbRootSection = New System.Windows.Forms.RadioButton()
        Me.Label38 = New System.Windows.Forms.Label()
        Me.nudCMz = New System.Windows.Forms.NumericUpDown()
        Me.Label41 = New System.Windows.Forms.Label()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.nudCMy = New System.Windows.Forms.NumericUpDown()
        Me.Label37 = New System.Windows.Forms.Label()
        Me.Label52 = New System.Windows.Forms.Label()
        Me.Label76 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.nudCS = New System.Windows.Forms.NumericUpDown()
        Me.Label75 = New System.Windows.Forms.Label()
        Me.Label54 = New System.Windows.Forms.Label()
        Me.nudM = New System.Windows.Forms.NumericUpDown()
        Me.Label55 = New System.Windows.Forms.Label()
        Me.nudJ = New System.Windows.Forms.NumericUpDown()
        Me.Label53 = New System.Windows.Forms.Label()
        Me.nudIw = New System.Windows.Forms.NumericUpDown()
        Me.nudIv = New System.Windows.Forms.NumericUpDown()
        Me.nudIu = New System.Windows.Forms.NumericUpDown()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.nudArea = New System.Windows.Forms.NumericUpDown()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.tbInquire = New System.Windows.Forms.TabPage()
        Me.tbSurfaceData = New System.Windows.Forms.TextBox()
        Me.btSurfaceData = New System.Windows.Forms.Button()
        Me.tbSurfaceName = New System.Windows.Forms.TextBox()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.PanelDeEdicion.SuspendLayout()
        Me.tbDesign.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.nudSelectRegion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tcMacroPanelProperties.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        CType(Me.nudChordwisePanels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTwistingAxis, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTwist, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudDihedral, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSweepback, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLength, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudTipChord, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudSpanwiseRings, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRootChord, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage7.SuspendLayout()
        CType(Me.pbProfileSketch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage8.SuspendLayout()
        CType(Me.nudCuttingStep, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudLastPrimitive, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudFirstPrimitive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage10.SuspendLayout()
        CType(Me.nudFlapPanels, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRootFlap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudFlapDeflection, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudFlapChord, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.nudCMz, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCMy, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudJ, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIv, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudArea, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbInquire.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnLockSurface
        '
        Me.btnLockSurface.BackColor = System.Drawing.Color.White
        Me.btnLockSurface.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnLockSurface.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gold
        Me.btnLockSurface.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnLockSurface.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnLockSurface.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLockSurface.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnLockSurface.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnLockSurface.Location = New System.Drawing.Point(249, 29)
        Me.btnLockSurface.Name = "btnLockSurface"
        Me.btnLockSurface.Size = New System.Drawing.Size(59, 22)
        Me.btnLockSurface.TabIndex = 69
        Me.btnLockSurface.Text = "Lock"
        Me.btnLockSurface.UseVisualStyleBackColor = False
        '
        'btnHideSurface
        '
        Me.btnHideSurface.BackColor = System.Drawing.Color.White
        Me.btnHideSurface.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnHideSurface.FlatAppearance.CheckedBackColor = System.Drawing.Color.Gold
        Me.btnHideSurface.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnHideSurface.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnHideSurface.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHideSurface.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHideSurface.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnHideSurface.Location = New System.Drawing.Point(249, 6)
        Me.btnHideSurface.Name = "btnHideSurface"
        Me.btnHideSurface.Size = New System.Drawing.Size(59, 22)
        Me.btnHideSurface.TabIndex = 68
        Me.btnHideSurface.Text = "Hide"
        Me.btnHideSurface.UseVisualStyleBackColor = False
        '
        'PanelDeEdicion
        '
        Me.PanelDeEdicion.Controls.Add(Me.tbDesign)
        Me.PanelDeEdicion.Controls.Add(Me.tbInquire)
        Me.PanelDeEdicion.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PanelDeEdicion.Location = New System.Drawing.Point(5, 34)
        Me.PanelDeEdicion.Margin = New System.Windows.Forms.Padding(8)
        Me.PanelDeEdicion.Name = "PanelDeEdicion"
        Me.PanelDeEdicion.SelectedIndex = 0
        Me.PanelDeEdicion.Size = New System.Drawing.Size(307, 370)
        Me.PanelDeEdicion.TabIndex = 65
        '
        'tbDesign
        '
        Me.tbDesign.BackColor = System.Drawing.Color.White
        Me.tbDesign.Controls.Add(Me.Panel1)
        Me.tbDesign.Controls.Add(Me.tcMacroPanelProperties)
        Me.tbDesign.Location = New System.Drawing.Point(4, 22)
        Me.tbDesign.Name = "tbDesign"
        Me.tbDesign.Padding = New System.Windows.Forms.Padding(3)
        Me.tbDesign.Size = New System.Drawing.Size(299, 344)
        Me.tbDesign.TabIndex = 0
        Me.tbDesign.Text = "Design"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnDeletePanel)
        Me.Panel1.Controls.Add(Me.btnAddPanel)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.nudSelectRegion)
        Me.Panel1.Controls.Add(Me.btnInsertPanel)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(293, 29)
        Me.Panel1.TabIndex = 60
        '
        'btnDeletePanel
        '
        Me.btnDeletePanel.BackColor = System.Drawing.Color.White
        Me.btnDeletePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnDeletePanel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnDeletePanel.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnDeletePanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnDeletePanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnDeletePanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDeletePanel.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDeletePanel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnDeletePanel.Location = New System.Drawing.Point(247, 3)
        Me.btnDeletePanel.Name = "btnDeletePanel"
        Me.btnDeletePanel.Size = New System.Drawing.Size(42, 22)
        Me.btnDeletePanel.TabIndex = 57
        Me.btnDeletePanel.Text = "Delete"
        Me.btnDeletePanel.UseVisualStyleBackColor = False
        '
        'btnAddPanel
        '
        Me.btnAddPanel.BackColor = System.Drawing.Color.White
        Me.btnAddPanel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnAddPanel.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnAddPanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnAddPanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnAddPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAddPanel.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddPanel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnAddPanel.Location = New System.Drawing.Point(159, 3)
        Me.btnAddPanel.Name = "btnAddPanel"
        Me.btnAddPanel.Size = New System.Drawing.Size(42, 22)
        Me.btnAddPanel.TabIndex = 59
        Me.btnAddPanel.Text = "Add"
        Me.btnAddPanel.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.DimGray
        Me.Label4.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label4.Location = New System.Drawing.Point(3, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Region:"
        '
        'nudSelectRegion
        '
        Me.nudSelectRegion.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.nudSelectRegion.ForeColor = System.Drawing.Color.Black
        Me.nudSelectRegion.Location = New System.Drawing.Point(63, 2)
        Me.nudSelectRegion.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.nudSelectRegion.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudSelectRegion.Name = "nudSelectRegion"
        Me.nudSelectRegion.Size = New System.Drawing.Size(48, 25)
        Me.nudSelectRegion.TabIndex = 51
        Me.nudSelectRegion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudSelectRegion.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'btnInsertPanel
        '
        Me.btnInsertPanel.BackColor = System.Drawing.Color.White
        Me.btnInsertPanel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnInsertPanel.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ButtonFace
        Me.btnInsertPanel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnInsertPanel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnInsertPanel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnInsertPanel.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnInsertPanel.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnInsertPanel.Location = New System.Drawing.Point(203, 3)
        Me.btnInsertPanel.Name = "btnInsertPanel"
        Me.btnInsertPanel.Size = New System.Drawing.Size(42, 22)
        Me.btnInsertPanel.TabIndex = 56
        Me.btnInsertPanel.Text = "Insert"
        Me.btnInsertPanel.UseVisualStyleBackColor = False
        '
        'tcMacroPanelProperties
        '
        Me.tcMacroPanelProperties.Controls.Add(Me.TabPage6)
        Me.tcMacroPanelProperties.Controls.Add(Me.TabPage7)
        Me.tcMacroPanelProperties.Controls.Add(Me.TabPage8)
        Me.tcMacroPanelProperties.Controls.Add(Me.TabPage10)
        Me.tcMacroPanelProperties.Controls.Add(Me.TabPage3)
        Me.tcMacroPanelProperties.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.tcMacroPanelProperties.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcMacroPanelProperties.Location = New System.Drawing.Point(3, 34)
        Me.tcMacroPanelProperties.Name = "tcMacroPanelProperties"
        Me.tcMacroPanelProperties.SelectedIndex = 0
        Me.tcMacroPanelProperties.Size = New System.Drawing.Size(293, 307)
        Me.tcMacroPanelProperties.TabIndex = 48
        '
        'TabPage6
        '
        Me.TabPage6.BackColor = System.Drawing.Color.White
        Me.TabPage6.Controls.Add(Me.Label14)
        Me.TabPage6.Controls.Add(Me.Label13)
        Me.TabPage6.Controls.Add(Me.Label1)
        Me.TabPage6.Controls.Add(Me.nudChordwisePanels)
        Me.TabPage6.Controls.Add(Me.Label2)
        Me.TabPage6.Controls.Add(Me.rbLinearSpacement)
        Me.TabPage6.Controls.Add(Me.rbConstantSpacement)
        Me.TabPage6.Controls.Add(Me.cbSymetricWing)
        Me.TabPage6.Controls.Add(Me.Label40)
        Me.TabPage6.Controls.Add(Me.Label39)
        Me.TabPage6.Controls.Add(Me.nudTwistingAxis)
        Me.TabPage6.Controls.Add(Me.nudTwist)
        Me.TabPage6.Controls.Add(Me.nudDihedral)
        Me.TabPage6.Controls.Add(Me.nudSweepback)
        Me.TabPage6.Controls.Add(Me.nudLength)
        Me.TabPage6.Controls.Add(Me.nudTipChord)
        Me.TabPage6.Controls.Add(Me.Label5)
        Me.TabPage6.Controls.Add(Me.Label27)
        Me.TabPage6.Controls.Add(Me.Label7)
        Me.TabPage6.Controls.Add(Me.Label8)
        Me.TabPage6.Controls.Add(Me.Label9)
        Me.TabPage6.Controls.Add(Me.Label15)
        Me.TabPage6.Controls.Add(Me.Label10)
        Me.TabPage6.Controls.Add(Me.Label16)
        Me.TabPage6.Controls.Add(Me.Label11)
        Me.TabPage6.Controls.Add(Me.Label17)
        Me.TabPage6.Controls.Add(Me.Label12)
        Me.TabPage6.Controls.Add(Me.nudSpanwiseRings)
        Me.TabPage6.Controls.Add(Me.nudRootChord)
        Me.TabPage6.Controls.Add(Me.Label6)
        Me.TabPage6.Controls.Add(Me.Label3)
        Me.TabPage6.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Margin = New System.Windows.Forms.Padding(0)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(285, 281)
        Me.TabPage6.TabIndex = 0
        Me.TabPage6.Text = "Geometry"
        '
        'nudChordwisePanels
        '
        Me.nudChordwisePanels.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.nudChordwisePanels.ForeColor = System.Drawing.Color.Black
        Me.nudChordwisePanels.Location = New System.Drawing.Point(109, 26)
        Me.nudChordwisePanels.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudChordwisePanels.Name = "nudChordwisePanels"
        Me.nudChordwisePanels.Size = New System.Drawing.Size(82, 22)
        Me.nudChordwisePanels.TabIndex = 58
        Me.nudChordwisePanels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudChordwisePanels.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label2.Location = New System.Drawing.Point(4, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(102, 13)
        Me.Label2.TabIndex = 42
        Me.Label2.Text = "Chordwise panels:"
        '
        'cbSymetricWing
        '
        Me.cbSymetricWing.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cbSymetricWing.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbSymetricWing.Location = New System.Drawing.Point(7, 251)
        Me.cbSymetricWing.Name = "cbSymetricWing"
        Me.cbSymetricWing.Size = New System.Drawing.Size(212, 24)
        Me.cbSymetricWing.TabIndex = 59
        Me.cbSymetricWing.Text = "Symmetric about [ Y = 0 ] plane"
        Me.cbSymetricWing.UseVisualStyleBackColor = True
        '
        'rbConstantSpacement
        '
        Me.rbConstantSpacement.AutoSize = True
        Me.rbConstantSpacement.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.rbConstantSpacement.Location = New System.Drawing.Point(201, 44)
        Me.rbConstantSpacement.Name = "rbConstantSpacement"
        Me.rbConstantSpacement.Size = New System.Drawing.Size(72, 17)
        Me.rbConstantSpacement.TabIndex = 52
        Me.rbConstantSpacement.Text = "Constant"
        Me.rbConstantSpacement.UseVisualStyleBackColor = True
        '
        'rbCuadraticSpacement
        '
        Me.rbLinearSpacement.AutoSize = True
        Me.rbLinearSpacement.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.rbLinearSpacement.Location = New System.Drawing.Point(201, 60)
        Me.rbLinearSpacement.Name = "rbCuadraticSpacement"
        Me.rbLinearSpacement.Size = New System.Drawing.Size(56, 17)
        Me.rbLinearSpacement.TabIndex = 53
        Me.rbLinearSpacement.Text = "Linear"
        Me.rbLinearSpacement.UseVisualStyleBackColor = True
        '
        'Label40
        '
        Me.Label40.AutoSize = True
        Me.Label40.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label40.Location = New System.Drawing.Point(194, 118)
        Me.Label40.Name = "Label40"
        Me.Label40.Size = New System.Drawing.Size(16, 13)
        Me.Label40.TabIndex = 57
        Me.Label40.Text = "m"
        '
        'Label39
        '
        Me.Label39.AutoSize = True
        Me.Label39.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label39.Location = New System.Drawing.Point(194, 139)
        Me.Label39.Name = "Label39"
        Me.Label39.Size = New System.Drawing.Size(16, 13)
        Me.Label39.TabIndex = 56
        Me.Label39.Text = "m"
        '
        'nudTwistingAxis
        '
        Me.nudTwistingAxis.DecimalPlaces = 4
        Me.nudTwistingAxis.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudTwistingAxis.Location = New System.Drawing.Point(109, 221)
        Me.nudTwistingAxis.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudTwistingAxis.Name = "nudTwistingAxis"
        Me.nudTwistingAxis.Size = New System.Drawing.Size(82, 22)
        Me.nudTwistingAxis.TabIndex = 51
        Me.nudTwistingAxis.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudTwistingAxis.Value = New Decimal(New Integer() {5, 0, 0, 65536})
        '
        'nudTwist
        '
        Me.nudTwist.DecimalPlaces = 4
        Me.nudTwist.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudTwist.Location = New System.Drawing.Point(109, 200)
        Me.nudTwist.Maximum = New Decimal(New Integer() {90, 0, 0, 0})
        Me.nudTwist.Minimum = New Decimal(New Integer() {90, 0, 0, -2147483648})
        Me.nudTwist.Name = "nudTwist"
        Me.nudTwist.Size = New System.Drawing.Size(82, 22)
        Me.nudTwist.TabIndex = 50
        Me.nudTwist.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudDihedral
        '
        Me.nudDihedral.DecimalPlaces = 4
        Me.nudDihedral.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.nudDihedral.Location = New System.Drawing.Point(109, 179)
        Me.nudDihedral.Maximum = New Decimal(New Integer() {360, 0, 0, 0})
        Me.nudDihedral.Minimum = New Decimal(New Integer() {90, 0, 0, -2147483648})
        Me.nudDihedral.Name = "nudDihedral"
        Me.nudDihedral.Size = New System.Drawing.Size(82, 22)
        Me.nudDihedral.TabIndex = 49
        Me.nudDihedral.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudSweepback
        '
        Me.nudSweepback.DecimalPlaces = 4
        Me.nudSweepback.Location = New System.Drawing.Point(109, 158)
        Me.nudSweepback.Maximum = New Decimal(New Integer() {90, 0, 0, 0})
        Me.nudSweepback.Minimum = New Decimal(New Integer() {90, 0, 0, -2147483648})
        Me.nudSweepback.Name = "nudSweepback"
        Me.nudSweepback.Size = New System.Drawing.Size(82, 22)
        Me.nudSweepback.TabIndex = 48
        Me.nudSweepback.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudLength
        '
        Me.nudLength.DecimalPlaces = 4
        Me.nudLength.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudLength.Location = New System.Drawing.Point(109, 137)
        Me.nudLength.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.nudLength.Name = "nudLength"
        Me.nudLength.Size = New System.Drawing.Size(82, 22)
        Me.nudLength.TabIndex = 47
        Me.nudLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudLength.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudTipChord
        '
        Me.nudTipChord.DecimalPlaces = 4
        Me.nudTipChord.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudTipChord.Location = New System.Drawing.Point(109, 116)
        Me.nudTipChord.Name = "nudTipChord"
        Me.nudTipChord.Size = New System.Drawing.Size(82, 22)
        Me.nudTipChord.TabIndex = 46
        Me.nudTipChord.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudTipChord.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label5.Location = New System.Drawing.Point(7, 49)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 13)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Spanwise panels:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label27.Location = New System.Drawing.Point(194, 160)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(11, 13)
        Me.Label27.TabIndex = 45
        Me.Label27.Text = "°"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label7.Location = New System.Drawing.Point(45, 118)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(58, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Tip chord:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label8.Location = New System.Drawing.Point(57, 139)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Length:"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label9.Location = New System.Drawing.Point(35, 160)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 13)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Sweepback:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label15.Location = New System.Drawing.Point(194, 223)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(49, 13)
        Me.Label15.TabIndex = 40
        Me.Label15.Text = "% chord"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label10.Location = New System.Drawing.Point(49, 181)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(54, 13)
        Me.Label10.TabIndex = 20
        Me.Label10.Text = "Dihedral:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label16.Location = New System.Drawing.Point(194, 202)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(11, 13)
        Me.Label16.TabIndex = 39
        Me.Label16.Text = "°"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label11.Location = New System.Drawing.Point(29, 202)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(74, 13)
        Me.Label11.TabIndex = 22
        Me.Label11.Text = "Overall twist:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label17.Location = New System.Drawing.Point(194, 181)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(11, 13)
        Me.Label17.TabIndex = 38
        Me.Label17.Text = "°"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label12.Location = New System.Drawing.Point(28, 223)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(74, 13)
        Me.Label12.TabIndex = 24
        Me.Label12.Text = "Twisting axis:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudSpanwiseRings
        '
        Me.nudSpanwiseRings.Location = New System.Drawing.Point(109, 47)
        Me.nudSpanwiseRings.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudSpanwiseRings.Name = "nudSpanwiseRings"
        Me.nudSpanwiseRings.Size = New System.Drawing.Size(82, 22)
        Me.nudSpanwiseRings.TabIndex = 58
        Me.nudSpanwiseRings.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudSpanwiseRings.Value = New Decimal(New Integer() {5, 0, 0, 0})
        '
        'nudRootChord
        '
        Me.nudRootChord.DecimalPlaces = 4
        Me.nudRootChord.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.nudRootChord.ForeColor = System.Drawing.Color.Black
        Me.nudRootChord.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudRootChord.Location = New System.Drawing.Point(109, 95)
        Me.nudRootChord.Maximum = New Decimal(New Integer() {99, 0, 0, 0})
        Me.nudRootChord.Name = "nudRootChord"
        Me.nudRootChord.Size = New System.Drawing.Size(82, 22)
        Me.nudRootChord.TabIndex = 44
        Me.nudRootChord.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRootChord.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label6.Location = New System.Drawing.Point(34, 97)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(68, 13)
        Me.Label6.TabIndex = 43
        Me.Label6.Text = "Root chord:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label3.Location = New System.Drawing.Point(195, 97)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(15, 13)
        Me.Label3.TabIndex = 45
        Me.Label3.Text = "m"
        '
        'TabPage7
        '
        Me.TabPage7.BackColor = System.Drawing.Color.White
        Me.TabPage7.Controls.Add(Me.lblPolarName)
        Me.TabPage7.Controls.Add(Me.lblCamberLineName)
        Me.TabPage7.Controls.Add(Me.btnCamberLines)
        Me.TabPage7.Controls.Add(Me.Label23)
        Me.TabPage7.Controls.Add(Me.btnPolarCurves)
        Me.TabPage7.Controls.Add(Me.Label72)
        Me.TabPage7.Controls.Add(Me.pbProfileSketch)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(285, 281)
        Me.TabPage7.TabIndex = 1
        Me.TabPage7.Text = "Profile"
        '
        'lblPolarName
        '
        Me.lblPolarName.AutoEllipsis = True
        Me.lblPolarName.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblPolarName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblPolarName.Location = New System.Drawing.Point(85, 13)
        Me.lblPolarName.Name = "lblPolarName"
        Me.lblPolarName.Size = New System.Drawing.Size(183, 13)
        Me.lblPolarName.TabIndex = 45
        Me.lblPolarName.Text = "MyPolarCurve"
        '
        'lblCamberLineName
        '
        Me.lblCamberLineName.AutoEllipsis = True
        Me.lblCamberLineName.Font = New System.Drawing.Font("Segoe UI", 8.25!)
        Me.lblCamberLineName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.lblCamberLineName.Location = New System.Drawing.Point(85, 69)
        Me.lblCamberLineName.Name = "lblCamberLineName"
        Me.lblCamberLineName.Size = New System.Drawing.Size(183, 13)
        Me.lblCamberLineName.TabIndex = 44
        Me.lblCamberLineName.Text = "MyCamberLine"
        '
        'btnCamberLines
        '
        Me.btnCamberLines.FlatAppearance.BorderColor = System.Drawing.Color.Silver
        Me.btnCamberLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnCamberLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCamberLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCamberLines.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCamberLines.Location = New System.Drawing.Point(10, 89)
        Me.btnCamberLines.Margin = New System.Windows.Forms.Padding(0)
        Me.btnCamberLines.Name = "btnCamberLines"
        Me.btnCamberLines.Size = New System.Drawing.Size(72, 22)
        Me.btnCamberLines.TabIndex = 43
        Me.btnCamberLines.Text = "Camber lines"
        Me.btnCamberLines.UseVisualStyleBackColor = True
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label23.Location = New System.Drawing.Point(7, 69)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(72, 13)
        Me.Label23.TabIndex = 42
        Me.Label23.Text = "Camber line:"
        '
        'btnPolarCurves
        '
        Me.btnPolarCurves.FlatAppearance.BorderColor = System.Drawing.Color.Silver
        Me.btnPolarCurves.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnPolarCurves.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnPolarCurves.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnPolarCurves.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPolarCurves.Location = New System.Drawing.Point(10, 36)
        Me.btnPolarCurves.Margin = New System.Windows.Forms.Padding(0)
        Me.btnPolarCurves.Name = "btnPolarCurves"
        Me.btnPolarCurves.Size = New System.Drawing.Size(72, 22)
        Me.btnPolarCurves.TabIndex = 41
        Me.btnPolarCurves.Text = "Polar curves"
        Me.btnPolarCurves.UseVisualStyleBackColor = True
        '
        'Label72
        '
        Me.Label72.AutoSize = True
        Me.Label72.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label72.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label72.Location = New System.Drawing.Point(7, 13)
        Me.Label72.Name = "Label72"
        Me.Label72.Size = New System.Drawing.Size(72, 13)
        Me.Label72.TabIndex = 40
        Me.Label72.Text = "Polar family:"
        '
        'pbProfileSketch
        '
        Me.pbProfileSketch.BackColor = System.Drawing.Color.White
        Me.pbProfileSketch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbProfileSketch.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.pbProfileSketch.Location = New System.Drawing.Point(10, 119)
        Me.pbProfileSketch.Name = "pbProfileSketch"
        Me.pbProfileSketch.Size = New System.Drawing.Size(262, 77)
        Me.pbProfileSketch.TabIndex = 29
        Me.pbProfileSketch.TabStop = False
        '
        'TabPage8
        '
        Me.TabPage8.BackColor = System.Drawing.Color.White
        Me.TabPage8.Controls.Add(Me.cbTrailingEdge)
        Me.TabPage8.Controls.Add(Me.Label18)
        Me.TabPage8.Controls.Add(Me.nudCuttingStep)
        Me.TabPage8.Controls.Add(Me.cbConvectWake)
        Me.TabPage8.Controls.Add(Me.Label29)
        Me.TabPage8.Controls.Add(Me.cbShowPriitives)
        Me.TabPage8.Controls.Add(Me.Label32)
        Me.TabPage8.Controls.Add(Me.Label31)
        Me.TabPage8.Controls.Add(Me.nudLastPrimitive)
        Me.TabPage8.Controls.Add(Me.nudFirstPrimitive)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(285, 281)
        Me.TabPage8.TabIndex = 2
        Me.TabPage8.Text = "Primitives"
        Me.TabPage8.ToolTipText = "Primitive panels"
        '
        'cbTrailingEdge
        '
        Me.cbTrailingEdge.AutoSize = True
        Me.cbTrailingEdge.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbTrailingEdge.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbTrailingEdge.Location = New System.Drawing.Point(9, 33)
        Me.cbTrailingEdge.Name = "cbTrailingEdge"
        Me.cbTrailingEdge.Size = New System.Drawing.Size(119, 17)
        Me.cbTrailingEdge.TabIndex = 61
        Me.cbTrailingEdge.Text = "Trailing edge only"
        Me.cbTrailingEdge.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label18.Location = New System.Drawing.Point(43, 128)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(74, 13)
        Me.Label18.TabIndex = 60
        Me.Label18.Text = "Cutting step:"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudCuttingStep
        '
        Me.nudCuttingStep.Location = New System.Drawing.Point(120, 126)
        Me.nudCuttingStep.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudCuttingStep.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudCuttingStep.Name = "nudCuttingStep"
        Me.nudCuttingStep.Size = New System.Drawing.Size(57, 22)
        Me.nudCuttingStep.TabIndex = 59
        Me.nudCuttingStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudCuttingStep.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'cbConvectWake
        '
        Me.cbConvectWake.AutoSize = True
        Me.cbConvectWake.Checked = True
        Me.cbConvectWake.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbConvectWake.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbConvectWake.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbConvectWake.Location = New System.Drawing.Point(9, 10)
        Me.cbConvectWake.Name = "cbConvectWake"
        Me.cbConvectWake.Size = New System.Drawing.Size(98, 17)
        Me.cbConvectWake.TabIndex = 58
        Me.cbConvectWake.Text = "Convect wake"
        Me.cbConvectWake.UseVisualStyleBackColor = True
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.Transparent
        Me.Label29.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label29.Location = New System.Drawing.Point(6, 57)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(197, 13)
        Me.Label29.TabIndex = 48
        Me.Label29.Text = "Primitive panels for wake convection:"
        '
        'cbShowPriitives
        '
        Me.cbShowPriitives.AutoSize = True
        Me.cbShowPriitives.Checked = True
        Me.cbShowPriitives.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbShowPriitives.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbShowPriitives.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.cbShowPriitives.Location = New System.Drawing.Point(9, 162)
        Me.cbShowPriitives.Name = "cbShowPriitives"
        Me.cbShowPriitives.Size = New System.Drawing.Size(109, 17)
        Me.cbShowPriitives.TabIndex = 54
        Me.cbShowPriitives.Text = "Show primitives"
        Me.cbShowPriitives.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label32.Location = New System.Drawing.Point(49, 100)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(68, 13)
        Me.Label32.TabIndex = 52
        Me.Label32.Text = "to segment:"
        Me.Label32.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label31.Location = New System.Drawing.Point(34, 79)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(83, 13)
        Me.Label31.TabIndex = 51
        Me.Label31.Text = "From segment:"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudLastPrimitive
        '
        Me.nudLastPrimitive.Location = New System.Drawing.Point(120, 98)
        Me.nudLastPrimitive.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.nudLastPrimitive.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudLastPrimitive.Name = "nudLastPrimitive"
        Me.nudLastPrimitive.Size = New System.Drawing.Size(57, 22)
        Me.nudLastPrimitive.TabIndex = 50
        Me.nudLastPrimitive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudLastPrimitive.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudFirstPrimitive
        '
        Me.nudFirstPrimitive.Location = New System.Drawing.Point(120, 77)
        Me.nudFirstPrimitive.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudFirstPrimitive.Name = "nudFirstPrimitive"
        Me.nudFirstPrimitive.Size = New System.Drawing.Size(57, 22)
        Me.nudFirstPrimitive.TabIndex = 49
        Me.nudFirstPrimitive.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudFirstPrimitive.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'TabPage10
        '
        Me.TabPage10.BackColor = System.Drawing.Color.White
        Me.TabPage10.Controls.Add(Me.Label73)
        Me.TabPage10.Controls.Add(Me.nudFlapPanels)
        Me.TabPage10.Controls.Add(Me.Label67)
        Me.TabPage10.Controls.Add(Me.Label71)
        Me.TabPage10.Controls.Add(Me.nudRootFlap)
        Me.TabPage10.Controls.Add(Me.Label66)
        Me.TabPage10.Controls.Add(Me.Label65)
        Me.TabPage10.Controls.Add(Me.Label61)
        Me.TabPage10.Controls.Add(Me.Label60)
        Me.TabPage10.Controls.Add(Me.nudFlapDeflection)
        Me.TabPage10.Controls.Add(Me.nudFlapChord)
        Me.TabPage10.Controls.Add(Me.cbFlapped)
        Me.TabPage10.Location = New System.Drawing.Point(4, 22)
        Me.TabPage10.Name = "TabPage10"
        Me.TabPage10.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage10.Size = New System.Drawing.Size(285, 281)
        Me.TabPage10.TabIndex = 4
        Me.TabPage10.Text = "Flaps"
        '
        'Label73
        '
        Me.Label73.AutoSize = True
        Me.Label73.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label73.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label73.Location = New System.Drawing.Point(27, 122)
        Me.Label73.Name = "Label73"
        Me.Label73.Size = New System.Drawing.Size(81, 13)
        Me.Label73.TabIndex = 60
        Me.Label73.Text = "Flapped rings:"
        Me.Label73.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudFlapPanels
        '
        Me.nudFlapPanels.Location = New System.Drawing.Point(111, 120)
        Me.nudFlapPanels.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudFlapPanels.Name = "nudFlapPanels"
        Me.nudFlapPanels.Size = New System.Drawing.Size(72, 22)
        Me.nudFlapPanels.TabIndex = 59
        Me.nudFlapPanels.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label67
        '
        Me.Label67.AutoSize = True
        Me.Label67.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label67.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label67.Location = New System.Drawing.Point(185, 101)
        Me.Label67.Name = "Label67"
        Me.Label67.Size = New System.Drawing.Size(21, 13)
        Me.Label67.TabIndex = 58
        Me.Label67.Text = "%c"
        '
        'Label71
        '
        Me.Label71.AutoSize = True
        Me.Label71.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label71.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label71.Location = New System.Drawing.Point(22, 101)
        Me.Label71.Name = "Label71"
        Me.Label71.Size = New System.Drawing.Size(86, 13)
        Me.Label71.TabIndex = 57
        Me.Label71.Text = "Wing root flap:"
        Me.Label71.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudRootFlap
        '
        Me.nudRootFlap.DecimalPlaces = 4
        Me.nudRootFlap.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudRootFlap.Location = New System.Drawing.Point(111, 99)
        Me.nudRootFlap.Name = "nudRootFlap"
        Me.nudRootFlap.Size = New System.Drawing.Size(72, 22)
        Me.nudRootFlap.TabIndex = 56
        Me.nudRootFlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label66
        '
        Me.Label66.AutoSize = True
        Me.Label66.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label66.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label66.Location = New System.Drawing.Point(185, 60)
        Me.Label66.Name = "Label66"
        Me.Label66.Size = New System.Drawing.Size(27, 13)
        Me.Label66.TabIndex = 55
        Me.Label66.Text = "deg"
        '
        'Label65
        '
        Me.Label65.AutoSize = True
        Me.Label65.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label65.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label65.Location = New System.Drawing.Point(185, 39)
        Me.Label65.Name = "Label65"
        Me.Label65.Size = New System.Drawing.Size(21, 13)
        Me.Label65.TabIndex = 54
        Me.Label65.Text = "%c"
        '
        'Label61
        '
        Me.Label61.AutoSize = True
        Me.Label61.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label61.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label61.Location = New System.Drawing.Point(21, 60)
        Me.Label61.Name = "Label61"
        Me.Label61.Size = New System.Drawing.Size(87, 13)
        Me.Label61.TabIndex = 53
        Me.Label61.Text = "Flap deflection:"
        Me.Label61.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label60
        '
        Me.Label60.AutoSize = True
        Me.Label60.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label60.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label60.Location = New System.Drawing.Point(13, 39)
        Me.Label60.Name = "Label60"
        Me.Label60.Size = New System.Drawing.Size(95, 13)
        Me.Label60.TabIndex = 52
        Me.Label60.Text = "Segment tip flap:"
        Me.Label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudFlapDeflection
        '
        Me.nudFlapDeflection.Location = New System.Drawing.Point(111, 58)
        Me.nudFlapDeflection.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudFlapDeflection.Name = "nudFlapDeflection"
        Me.nudFlapDeflection.Size = New System.Drawing.Size(72, 22)
        Me.nudFlapDeflection.TabIndex = 2
        Me.nudFlapDeflection.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudFlapChord
        '
        Me.nudFlapChord.DecimalPlaces = 4
        Me.nudFlapChord.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        Me.nudFlapChord.Location = New System.Drawing.Point(111, 37)
        Me.nudFlapChord.Name = "nudFlapChord"
        Me.nudFlapChord.Size = New System.Drawing.Size(72, 22)
        Me.nudFlapChord.TabIndex = 1
        Me.nudFlapChord.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cbFlapped
        '
        Me.cbFlapped.AutoSize = True
        Me.cbFlapped.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cbFlapped.Location = New System.Drawing.Point(12, 10)
        Me.cbFlapped.Name = "cbFlapped"
        Me.cbFlapped.Size = New System.Drawing.Size(68, 17)
        Me.cbFlapped.TabIndex = 0
        Me.cbFlapped.Text = "Flapped"
        Me.cbFlapped.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.BackColor = System.Drawing.Color.White
        Me.TabPage3.Controls.Add(Me.rbTipSection)
        Me.TabPage3.Controls.Add(Me.rbRootSection)
        Me.TabPage3.Controls.Add(Me.Label38)
        Me.TabPage3.Controls.Add(Me.nudCMz)
        Me.TabPage3.Controls.Add(Me.Label41)
        Me.TabPage3.Controls.Add(Me.Label36)
        Me.TabPage3.Controls.Add(Me.nudCMy)
        Me.TabPage3.Controls.Add(Me.Label37)
        Me.TabPage3.Controls.Add(Me.Label52)
        Me.TabPage3.Controls.Add(Me.Label76)
        Me.TabPage3.Controls.Add(Me.Label30)
        Me.TabPage3.Controls.Add(Me.Label33)
        Me.TabPage3.Controls.Add(Me.Label34)
        Me.TabPage3.Controls.Add(Me.Label35)
        Me.TabPage3.Controls.Add(Me.Label21)
        Me.TabPage3.Controls.Add(Me.Label22)
        Me.TabPage3.Controls.Add(Me.Label26)
        Me.TabPage3.Controls.Add(Me.nudCS)
        Me.TabPage3.Controls.Add(Me.Label75)
        Me.TabPage3.Controls.Add(Me.Label54)
        Me.TabPage3.Controls.Add(Me.nudM)
        Me.TabPage3.Controls.Add(Me.Label55)
        Me.TabPage3.Controls.Add(Me.nudJ)
        Me.TabPage3.Controls.Add(Me.Label53)
        Me.TabPage3.Controls.Add(Me.nudIw)
        Me.TabPage3.Controls.Add(Me.nudIv)
        Me.TabPage3.Controls.Add(Me.nudIu)
        Me.TabPage3.Controls.Add(Me.Label20)
        Me.TabPage3.Controls.Add(Me.nudArea)
        Me.TabPage3.Controls.Add(Me.Label19)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(285, 281)
        Me.TabPage3.TabIndex = 3
        Me.TabPage3.Text = "Structure"
        '
        'rbTipSection
        '
        Me.rbTipSection.AutoSize = True
        Me.rbTipSection.Location = New System.Drawing.Point(99, 5)
        Me.rbTipSection.Name = "rbTipSection"
        Me.rbTipSection.Size = New System.Drawing.Size(80, 17)
        Me.rbTipSection.TabIndex = 84
        Me.rbTipSection.Text = "Tip section"
        Me.rbTipSection.UseVisualStyleBackColor = True
        '
        'rbRootSection
        '
        Me.rbRootSection.AutoSize = True
        Me.rbRootSection.Checked = True
        Me.rbRootSection.Location = New System.Drawing.Point(5, 5)
        Me.rbRootSection.Name = "rbRootSection"
        Me.rbRootSection.Size = New System.Drawing.Size(90, 17)
        Me.rbRootSection.TabIndex = 83
        Me.rbRootSection.TabStop = True
        Me.rbRootSection.Text = "Root section"
        Me.rbRootSection.UseVisualStyleBackColor = True
        '
        'Label38
        '
        Me.Label38.AutoSize = True
        Me.Label38.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label38.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label38.Location = New System.Drawing.Point(192, 231)
        Me.Label38.Name = "Label38"
        Me.Label38.Size = New System.Drawing.Size(16, 13)
        Me.Label38.TabIndex = 82
        Me.Label38.Text = "m"
        '
        'nudCMz
        '
        Me.nudCMz.DecimalPlaces = 5
        Me.nudCMz.Location = New System.Drawing.Point(40, 229)
        Me.nudCMz.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCMz.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.nudCMz.Name = "nudCMz"
        Me.nudCMz.Size = New System.Drawing.Size(146, 22)
        Me.nudCMz.TabIndex = 81
        Me.nudCMz.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label41
        '
        Me.Label41.AutoSize = True
        Me.Label41.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label41.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label41.Location = New System.Drawing.Point(5, 231)
        Me.Label41.Name = "Label41"
        Me.Label41.Size = New System.Drawing.Size(32, 13)
        Me.Label41.TabIndex = 80
        Me.Label41.Text = "CMz:"
        Me.Label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label36.Location = New System.Drawing.Point(192, 210)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(16, 13)
        Me.Label36.TabIndex = 79
        Me.Label36.Text = "m"
        '
        'nudCMy
        '
        Me.nudCMy.DecimalPlaces = 5
        Me.nudCMy.Location = New System.Drawing.Point(40, 208)
        Me.nudCMy.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudCMy.Minimum = New Decimal(New Integer() {100000, 0, 0, -2147483648})
        Me.nudCMy.Name = "nudCMy"
        Me.nudCMy.Size = New System.Drawing.Size(146, 22)
        Me.nudCMy.TabIndex = 78
        Me.nudCMy.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label37
        '
        Me.Label37.AutoSize = True
        Me.Label37.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label37.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label37.Location = New System.Drawing.Point(5, 210)
        Me.Label37.Name = "Label37"
        Me.Label37.Size = New System.Drawing.Size(32, 13)
        Me.Label37.TabIndex = 77
        Me.Label37.Text = "CMy:"
        Me.Label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label52
        '
        Me.Label52.AutoSize = True
        Me.Label52.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label52.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label52.Location = New System.Drawing.Point(192, 182)
        Me.Label52.Name = "Label52"
        Me.Label52.Size = New System.Drawing.Size(33, 13)
        Me.Label52.TabIndex = 76
        Me.Label52.Text = "Kgm²"
        '
        'Label76
        '
        Me.Label76.AutoSize = True
        Me.Label76.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label76.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label76.Location = New System.Drawing.Point(8, 140)
        Me.Label76.Name = "Label76"
        Me.Label76.Size = New System.Drawing.Size(102, 13)
        Me.Label76.TabIndex = 75
        Me.Label76.Text = "Inertial properties:"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label30.Location = New System.Drawing.Point(192, 115)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(28, 13)
        Me.Label30.TabIndex = 74
        Me.Label30.Text = "Nm²"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label33.Location = New System.Drawing.Point(192, 94)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(28, 13)
        Me.Label33.TabIndex = 73
        Me.Label33.Text = "Nm²"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label34.Location = New System.Drawing.Point(192, 73)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(28, 13)
        Me.Label34.TabIndex = 72
        Me.Label34.Text = "Nm²"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label35.Location = New System.Drawing.Point(192, 52)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(34, 13)
        Me.Label35.TabIndex = 71
        Me.Label35.Text = "kN/m"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label21.Location = New System.Drawing.Point(13, 115)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(24, 13)
        Me.Label21.TabIndex = 70
        Me.Label21.Text = "EIz:"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label22.Location = New System.Drawing.Point(13, 94)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(24, 13)
        Me.Label22.TabIndex = 69
        Me.Label22.Text = "EIy:"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label26.Location = New System.Drawing.Point(13, 73)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(22, 13)
        Me.Label26.TabIndex = 68
        Me.Label26.Text = "GJ:"
        '
        'nudCS
        '
        Me.nudCS.DecimalPlaces = 2
        Me.nudCS.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudCS.Location = New System.Drawing.Point(128, 255)
        Me.nudCS.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudCS.Name = "nudCS"
        Me.nudCS.Size = New System.Drawing.Size(58, 22)
        Me.nudCS.TabIndex = 67
        Me.nudCS.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudCS.Value = New Decimal(New Integer() {3, 0, 0, 65536})
        '
        'Label75
        '
        Me.Label75.AutoSize = True
        Me.Label75.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label75.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label75.Location = New System.Drawing.Point(83, 257)
        Me.Label75.Name = "Label75"
        Me.Label75.Size = New System.Drawing.Size(39, 13)
        Me.Label75.TabIndex = 66
        Me.Label75.Text = "CSy/C:"
        Me.Label75.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label54
        '
        Me.Label54.AutoSize = True
        Me.Label54.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label54.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label54.Location = New System.Drawing.Point(192, 161)
        Me.Label54.Name = "Label54"
        Me.Label54.Size = New System.Drawing.Size(33, 13)
        Me.Label54.TabIndex = 65
        Me.Label54.Text = "kg/m"
        '
        'nudM
        '
        Me.nudM.DecimalPlaces = 2
        Me.nudM.Location = New System.Drawing.Point(40, 159)
        Me.nudM.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
        Me.nudM.Name = "nudM"
        Me.nudM.Size = New System.Drawing.Size(146, 22)
        Me.nudM.TabIndex = 64
        Me.nudM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label55
        '
        Me.Label55.AutoSize = True
        Me.Label55.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label55.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label55.Location = New System.Drawing.Point(17, 161)
        Me.Label55.Name = "Label55"
        Me.Label55.Size = New System.Drawing.Size(19, 13)
        Me.Label55.TabIndex = 63
        Me.Label55.Text = "m:"
        Me.Label55.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudJ
        '
        Me.nudJ.DecimalPlaces = 2
        Me.nudJ.Increment = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudJ.Location = New System.Drawing.Point(40, 180)
        Me.nudJ.Maximum = New Decimal(New Integer() {-559939584, 902409669, 54, 0})
        Me.nudJ.Name = "nudJ"
        Me.nudJ.Size = New System.Drawing.Size(146, 22)
        Me.nudJ.TabIndex = 61
        Me.nudJ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label53
        '
        Me.Label53.AutoSize = True
        Me.Label53.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label53.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label53.Location = New System.Drawing.Point(10, 182)
        Me.Label53.Name = "Label53"
        Me.Label53.Size = New System.Drawing.Size(27, 13)
        Me.Label53.TabIndex = 60
        Me.Label53.Text = "ρIp:"
        Me.Label53.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'nudIw
        '
        Me.nudIw.DecimalPlaces = 2
        Me.nudIw.Increment = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudIw.Location = New System.Drawing.Point(40, 113)
        Me.nudIw.Maximum = New Decimal(New Integer() {-559939584, 902409669, 54, 0})
        Me.nudIw.Name = "nudIw"
        Me.nudIw.Size = New System.Drawing.Size(146, 22)
        Me.nudIw.TabIndex = 46
        Me.nudIw.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudIv
        '
        Me.nudIv.DecimalPlaces = 2
        Me.nudIv.Increment = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudIv.Location = New System.Drawing.Point(40, 92)
        Me.nudIv.Maximum = New Decimal(New Integer() {-559939584, 902409669, 54, 0})
        Me.nudIv.Name = "nudIv"
        Me.nudIv.Size = New System.Drawing.Size(146, 22)
        Me.nudIv.TabIndex = 44
        Me.nudIv.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudIu
        '
        Me.nudIu.DecimalPlaces = 2
        Me.nudIu.Increment = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudIu.Location = New System.Drawing.Point(40, 71)
        Me.nudIu.Maximum = New Decimal(New Integer() {-559939584, 902409669, 54, 0})
        Me.nudIu.Name = "nudIu"
        Me.nudIu.Size = New System.Drawing.Size(146, 22)
        Me.nudIu.TabIndex = 42
        Me.nudIu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label20.Location = New System.Drawing.Point(8, 28)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(98, 13)
        Me.Label20.TabIndex = 40
        Me.Label20.Text = "Elastic properties:"
        '
        'nudArea
        '
        Me.nudArea.DecimalPlaces = 2
        Me.nudArea.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.nudArea.Location = New System.Drawing.Point(40, 50)
        Me.nudArea.Maximum = New Decimal(New Integer() {-559939584, 902409669, 54, 0})
        Me.nudArea.Name = "nudArea"
        Me.nudArea.Size = New System.Drawing.Size(146, 22)
        Me.nudArea.TabIndex = 39
        Me.nudArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label19.Location = New System.Drawing.Point(13, 52)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(23, 13)
        Me.Label19.TabIndex = 38
        Me.Label19.Text = "EA:"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tbInquire
        '
        Me.tbInquire.Controls.Add(Me.tbSurfaceData)
        Me.tbInquire.Controls.Add(Me.btSurfaceData)
        Me.tbInquire.Location = New System.Drawing.Point(4, 22)
        Me.tbInquire.Name = "tbInquire"
        Me.tbInquire.Padding = New System.Windows.Forms.Padding(3)
        Me.tbInquire.Size = New System.Drawing.Size(299, 344)
        Me.tbInquire.TabIndex = 2
        Me.tbInquire.Text = "Inquire"
        Me.tbInquire.UseVisualStyleBackColor = True
        '
        'tbSurfaceData
        '
        Me.tbSurfaceData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbSurfaceData.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbSurfaceData.Location = New System.Drawing.Point(6, 34)
        Me.tbSurfaceData.Multiline = True
        Me.tbSurfaceData.Name = "tbSurfaceData"
        Me.tbSurfaceData.Size = New System.Drawing.Size(287, 304)
        Me.tbSurfaceData.TabIndex = 86
        '
        'btSurfaceData
        '
        Me.btSurfaceData.BackColor = System.Drawing.Color.White
        Me.btSurfaceData.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btSurfaceData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btSurfaceData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btSurfaceData.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btSurfaceData.Location = New System.Drawing.Point(6, 6)
        Me.btSurfaceData.Name = "btSurfaceData"
        Me.btSurfaceData.Size = New System.Drawing.Size(107, 22)
        Me.btSurfaceData.TabIndex = 85
        Me.btSurfaceData.Text = "Request surface data"
        Me.btSurfaceData.UseVisualStyleBackColor = False
        '
        'tbSurfaceName
        '
        Me.tbSurfaceName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbSurfaceName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.tbSurfaceName.Location = New System.Drawing.Point(5, 6)
        Me.tbSurfaceName.Name = "tbSurfaceName"
        Me.tbSurfaceName.Size = New System.Drawing.Size(238, 22)
        Me.tbSurfaceName.TabIndex = 66
        Me.tbSurfaceName.Text = "Name"
        '
        'btnOk
        '
        Me.btnOk.BackColor = System.Drawing.Color.White
        Me.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOk.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btnOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnOk.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOk.Location = New System.Drawing.Point(235, 406)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(72, 23)
        Me.btnOk.TabIndex = 70
        Me.btnOk.Text = "Close"
        Me.btnOk.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label1.Location = New System.Drawing.Point(199, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 60
        Me.Label1.Text = "Spacement:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label13.Location = New System.Drawing.Point(4, 8)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(39, 13)
        Me.Label13.TabIndex = 61
        Me.Label13.Text = "Mesh:"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label14.Location = New System.Drawing.Point(4, 76)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(71, 13)
        Me.Label14.TabIndex = 62
        Me.Label14.Text = "Dimensions:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'WingControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.btnLockSurface)
        Me.Controls.Add(Me.btnHideSurface)
        Me.Controls.Add(Me.PanelDeEdicion)
        Me.Controls.Add(Me.tbSurfaceName)
        Me.Name = "WingControl"
        Me.Size = New System.Drawing.Size(312, 434)
        Me.PanelDeEdicion.ResumeLayout(False)
        Me.tbDesign.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudSelectRegion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tcMacroPanelProperties.ResumeLayout(False)
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        CType(Me.nudChordwisePanels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTwistingAxis, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTwist, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudDihedral, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSweepback, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLength, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudTipChord, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudSpanwiseRings, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRootChord, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        CType(Me.pbProfileSketch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage8.ResumeLayout(False)
        Me.TabPage8.PerformLayout()
        CType(Me.nudCuttingStep, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudLastPrimitive, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudFirstPrimitive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage10.ResumeLayout(False)
        Me.TabPage10.PerformLayout()
        CType(Me.nudFlapPanels, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRootFlap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudFlapDeflection, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudFlapChord, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        CType(Me.nudCMz, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCMy, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudJ, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIv, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudArea, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbInquire.ResumeLayout(False)
        Me.tbInquire.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnLockSurface As System.Windows.Forms.Button
    Friend WithEvents btnHideSurface As System.Windows.Forms.Button
    Friend WithEvents PanelDeEdicion As System.Windows.Forms.TabControl
    Friend WithEvents tbDesign As System.Windows.Forms.TabPage
    Friend WithEvents btnAddPanel As System.Windows.Forms.Button
    Friend WithEvents nudChordwisePanels As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnDeletePanel As System.Windows.Forms.Button
    Friend WithEvents btnInsertPanel As System.Windows.Forms.Button
    Friend WithEvents nudSelectRegion As System.Windows.Forms.NumericUpDown
    Friend WithEvents tcMacroPanelProperties As System.Windows.Forms.TabControl
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents cbSymetricWing As System.Windows.Forms.CheckBox
    Friend WithEvents rbConstantSpacement As System.Windows.Forms.RadioButton
    Friend WithEvents rbLinearSpacement As System.Windows.Forms.RadioButton
    Friend WithEvents nudSpanwiseRings As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label40 As System.Windows.Forms.Label
    Friend WithEvents Label39 As System.Windows.Forms.Label
    Friend WithEvents nudTwistingAxis As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudTwist As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudDihedral As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudSweepback As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudLength As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudTipChord As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents pbProfileSketch As System.Windows.Forms.PictureBox
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents cbConvectWake As System.Windows.Forms.CheckBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents cbShowPriitives As System.Windows.Forms.CheckBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents nudLastPrimitive As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudFirstPrimitive As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents nudRootChord As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents tbSurfaceName As System.Windows.Forms.TextBox
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents nudCuttingStep As System.Windows.Forms.NumericUpDown
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents nudIw As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudIv As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudIu As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents nudArea As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents tbInquire As System.Windows.Forms.TabPage
    Friend WithEvents btSurfaceData As System.Windows.Forms.Button
    Friend WithEvents tbSurfaceData As System.Windows.Forms.TextBox
    Friend WithEvents nudJ As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label53 As System.Windows.Forms.Label
    Friend WithEvents Label54 As System.Windows.Forms.Label
    Friend WithEvents nudM As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label55 As System.Windows.Forms.Label
    Friend WithEvents TabPage10 As System.Windows.Forms.TabPage
    Friend WithEvents cbFlapped As System.Windows.Forms.CheckBox
    Friend WithEvents nudFlapDeflection As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudFlapChord As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label61 As System.Windows.Forms.Label
    Friend WithEvents Label60 As System.Windows.Forms.Label
    Friend WithEvents Label66 As System.Windows.Forms.Label
    Friend WithEvents Label65 As System.Windows.Forms.Label
    Friend WithEvents Label67 As System.Windows.Forms.Label
    Friend WithEvents Label71 As System.Windows.Forms.Label
    Friend WithEvents nudRootFlap As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label73 As System.Windows.Forms.Label
    Friend WithEvents nudFlapPanels As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label72 As System.Windows.Forms.Label
    Friend WithEvents btnPolarCurves As System.Windows.Forms.Button
    Friend WithEvents nudCS As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label75 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents Label33 As System.Windows.Forms.Label
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents Label76 As System.Windows.Forms.Label
    Friend WithEvents Label52 As System.Windows.Forms.Label
    Friend WithEvents Label38 As System.Windows.Forms.Label
    Friend WithEvents nudCMz As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label41 As System.Windows.Forms.Label
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents nudCMy As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label37 As System.Windows.Forms.Label
    Friend WithEvents cbTrailingEdge As System.Windows.Forms.CheckBox
    Friend WithEvents btnCamberLines As System.Windows.Forms.Button
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents lblPolarName As System.Windows.Forms.Label
    Friend WithEvents lblCamberLineName As System.Windows.Forms.Label
    Friend WithEvents Panel1 As Windows.Forms.Panel
    Friend WithEvents rbTipSection As Windows.Forms.RadioButton
    Friend WithEvents rbRootSection As Windows.Forms.RadioButton
    Friend WithEvents Label14 As Windows.Forms.Label
    Friend WithEvents Label13 As Windows.Forms.Label
    Friend WithEvents Label1 As Windows.Forms.Label
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormUnsteadyVelocity
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
        Dim ChartArea7 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend7 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Series19 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series20 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Series21 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Dim Title7 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormUnsteadyVelocity))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.rbtImpulsive = New System.Windows.Forms.RadioButton()
        Me.rbtPerturbation = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.EjeZ = New System.Windows.Forms.RadioButton()
        Me.EjeY = New System.Windows.Forms.RadioButton()
        Me.EjeX = New System.Windows.Forms.RadioButton()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.nudFinalValue = New System.Windows.Forms.NumericUpDown()
        Me.tbxInterval = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.nudInterval = New System.Windows.Forms.NumericUpDown()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.nudIntensity = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.nudPeak = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.nudStart = New System.Windows.Forms.NumericUpDown()
        Me.tbxVelocity = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.gVelocity = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btOK = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.nudFinalValue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudInterval, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIntensity, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPeak, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudStart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gVelocity, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.Controls.Add(Me.btnCancel)
        Me.GroupBox1.Controls.Add(Me.btOK)
        Me.GroupBox1.Controls.Add(Me.Panel2)
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.nudFinalValue)
        Me.GroupBox1.Controls.Add(Me.tbxInterval)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.nudInterval)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.nudIntensity)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.nudPeak)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.nudStart)
        Me.GroupBox1.Controls.Add(Me.tbxVelocity)
        Me.GroupBox1.Controls.Add(Me.Label1)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.rbtImpulsive)
        Me.Panel2.Controls.Add(Me.rbtPerturbation)
        resources.ApplyResources(Me.Panel2, "Panel2")
        Me.Panel2.Name = "Panel2"
        '
        'rbtImpulsive
        '
        resources.ApplyResources(Me.rbtImpulsive, "rbtImpulsive")
        Me.rbtImpulsive.Name = "rbtImpulsive"
        Me.rbtImpulsive.TabStop = True
        Me.rbtImpulsive.UseVisualStyleBackColor = True
        '
        'rbtPerturbation
        '
        resources.ApplyResources(Me.rbtPerturbation, "rbtPerturbation")
        Me.rbtPerturbation.Name = "rbtPerturbation"
        Me.rbtPerturbation.TabStop = True
        Me.rbtPerturbation.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.EjeZ)
        Me.Panel1.Controls.Add(Me.EjeY)
        Me.Panel1.Controls.Add(Me.EjeX)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'EjeZ
        '
        resources.ApplyResources(Me.EjeZ, "EjeZ")
        Me.EjeZ.Name = "EjeZ"
        Me.EjeZ.TabStop = True
        Me.EjeZ.UseVisualStyleBackColor = True
        '
        'EjeY
        '
        resources.ApplyResources(Me.EjeY, "EjeY")
        Me.EjeY.Name = "EjeY"
        Me.EjeY.TabStop = True
        Me.EjeY.UseVisualStyleBackColor = True
        '
        'EjeX
        '
        resources.ApplyResources(Me.EjeX, "EjeX")
        Me.EjeX.Name = "EjeX"
        Me.EjeX.TabStop = True
        Me.EjeX.UseVisualStyleBackColor = True
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'nudFinalValue
        '
        Me.nudFinalValue.DecimalPlaces = 3
        Me.nudFinalValue.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        resources.ApplyResources(Me.nudFinalValue, "nudFinalValue")
        Me.nudFinalValue.Name = "nudFinalValue"
        '
        'tbxInterval
        '
        resources.ApplyResources(Me.tbxInterval, "tbxInterval")
        Me.tbxInterval.Name = "tbxInterval"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'nudInterval
        '
        resources.ApplyResources(Me.nudInterval, "nudInterval")
        Me.nudInterval.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudInterval.Name = "nudInterval"
        Me.nudInterval.Value = New Decimal(New Integer() {3, 0, 0, 0})
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'nudIntensity
        '
        Me.nudIntensity.DecimalPlaces = 3
        Me.nudIntensity.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        resources.ApplyResources(Me.nudIntensity, "nudIntensity")
        Me.nudIntensity.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.nudIntensity.Name = "nudIntensity"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'nudPeak
        '
        resources.ApplyResources(Me.nudPeak, "nudPeak")
        Me.nudPeak.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudPeak.Name = "nudPeak"
        Me.nudPeak.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'nudStart
        '
        resources.ApplyResources(Me.nudStart, "nudStart")
        Me.nudStart.Maximum = New Decimal(New Integer() {500, 0, 0, 0})
        Me.nudStart.Name = "nudStart"
        Me.nudStart.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'tbxVelocity
        '
        resources.ApplyResources(Me.tbxVelocity, "tbxVelocity")
        Me.tbxVelocity.Name = "tbxVelocity"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'gVelocity
        '
        resources.ApplyResources(Me.gVelocity, "gVelocity")
        ChartArea7.AxisX.IsLabelAutoFit = False
        ChartArea7.AxisX.LabelStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea7.AxisX.LineWidth = 2
        ChartArea7.AxisX.MinorGrid.Enabled = True
        ChartArea7.AxisX.MinorGrid.IntervalOffset = 1.0R
        ChartArea7.AxisX.MinorGrid.LineColor = System.Drawing.Color.LightGray
        ChartArea7.AxisX.Title = "t/Δt"
        ChartArea7.AxisX.TitleFont = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea7.AxisY.Interval = 0.2R
        ChartArea7.AxisY.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea7.AxisY.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number
        ChartArea7.AxisY.IsLabelAutoFit = False
        ChartArea7.AxisY.LabelStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea7.AxisY.LineWidth = 2
        ChartArea7.AxisY.MajorGrid.Interval = 1.0R
        ChartArea7.AxisY.Maximum = 2.0R
        ChartArea7.AxisY.Minimum = -2.0R
        ChartArea7.AxisY.MinorGrid.Enabled = True
        ChartArea7.AxisY.MinorGrid.Interval = 0.2R
        ChartArea7.AxisY.MinorGrid.LineColor = System.Drawing.Color.LightGray
        ChartArea7.AxisY.Title = "V(t)/Vo"
        ChartArea7.AxisY.TitleFont = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea7.BackColor = System.Drawing.Color.Transparent
        ChartArea7.Name = "AreaDelGrafico"
        ChartArea7.ShadowColor = System.Drawing.Color.White
        Me.gVelocity.ChartAreas.Add(ChartArea7)
        Me.gVelocity.Cursor = System.Windows.Forms.Cursors.Cross
        Legend7.BackColor = System.Drawing.Color.White
        Legend7.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Legend7.ForeColor = System.Drawing.Color.DimGray
        Legend7.IsTextAutoFit = False
        Legend7.ItemColumnSpacing = 30
        Legend7.MaximumAutoSize = 40.0!
        Legend7.Name = "Leyenda"
        Legend7.Position.Auto = False
        Legend7.Position.Height = 6.0!
        Legend7.Position.Width = 26.0!
        Legend7.Position.X = 70.0!
        Legend7.Position.Y = 79.0!
        Me.gVelocity.Legends.Add(Legend7)
        Me.gVelocity.Name = "gVelocity"
        Series19.BorderWidth = 2
        Series19.ChartArea = "AreaDelGrafico"
        Series19.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series19.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Series19.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Series19.Legend = "Leyenda"
        Series19.MarkerBorderColor = System.Drawing.Color.Black
        Series19.MarkerColor = System.Drawing.Color.White
        Series19.MarkerSize = 3
        Series19.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle
        Series19.Name = "Vx"
        Series20.BorderWidth = 2
        Series20.ChartArea = "AreaDelGrafico"
        Series20.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series20.Color = System.Drawing.Color.Red
        Series20.Legend = "Leyenda"
        Series20.MarkerBorderColor = System.Drawing.Color.Black
        Series20.MarkerColor = System.Drawing.Color.White
        Series20.MarkerSize = 3
        Series20.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle
        Series20.Name = "Vy"
        Series21.BorderWidth = 2
        Series21.ChartArea = "AreaDelGrafico"
        Series21.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline
        Series21.Color = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Series21.Legend = "Leyenda"
        Series21.MarkerBorderColor = System.Drawing.Color.Black
        Series21.MarkerColor = System.Drawing.Color.White
        Series21.MarkerSize = 3
        Series21.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle
        Series21.Name = "Vz"
        Me.gVelocity.Series.Add(Series19)
        Me.gVelocity.Series.Add(Series20)
        Me.gVelocity.Series.Add(Series21)
        Title7.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Title7.Name = "Title1"
        Title7.Text = "Unsteady velocity"
        Me.gVelocity.Titles.Add(Title7)
        '
        'btnCancel
        '
        resources.ApplyResources(Me.btnCancel, "btnCancel")
        Me.btnCancel.BackColor = System.Drawing.Color.White
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btnCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightCoral
        Me.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btOK
        '
        resources.ApplyResources(Me.btOK, "btOK")
        Me.btOK.BackColor = System.Drawing.Color.White
        Me.btOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btOK.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btOK.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.btOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.PaleGreen
        Me.btOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DeepSkyBlue
        Me.btOK.Name = "btOK"
        Me.btOK.UseVisualStyleBackColor = False
        '
        'FormUnsteadyVelocity
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.gVelocity)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "FormUnsteadyVelocity"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.nudFinalValue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudInterval, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIntensity, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPeak, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudStart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gVelocity, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents tbxVelocity As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents gVelocity As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents nudPeak As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents nudStart As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents nudInterval As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents nudIntensity As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents EjeZ As System.Windows.Forms.RadioButton
    Friend WithEvents EjeY As System.Windows.Forms.RadioButton
    Friend WithEvents EjeX As System.Windows.Forms.RadioButton
    Friend WithEvents tbxInterval As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents nudFinalValue As System.Windows.Forms.NumericUpDown
    Friend WithEvents rbtPerturbation As System.Windows.Forms.RadioButton
    Friend WithEvents rbtImpulsive As System.Windows.Forms.RadioButton
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btOK As System.Windows.Forms.Button
End Class

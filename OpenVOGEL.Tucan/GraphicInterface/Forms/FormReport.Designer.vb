<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormReport
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormReport))
        Me.DialogoGuardarReporte = New System.Windows.Forms.SaveFileDialog()
        Me.ChrtModalResponse = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.DialogoAbrir = New System.Windows.Forms.OpenFileDialog()
        Me.pPlot = New System.Windows.Forms.Panel()
        Me.CbLink = New System.Windows.Forms.ComboBox()
        Me.CbModes = New System.Windows.Forms.ComboBox()
        Me.tcResults = New System.Windows.Forms.TabControl()
        Me.tbTotalLoads = New System.Windows.Forms.TabPage()
        Me.tbLoads = New System.Windows.Forms.TabPage()
        Me.tbRaw = New System.Windows.Forms.TabPage()
        Me.TbRawData = New System.Windows.Forms.RichTextBox()
        Me.TbModalResponse = New System.Windows.Forms.TabPage()
        CType(Me.ChrtModalResponse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pPlot.SuspendLayout()
        Me.tcResults.SuspendLayout()
        Me.tbRaw.SuspendLayout()
        Me.TbModalResponse.SuspendLayout()
        Me.SuspendLayout()
        '
        'cModalResponse
        '
        Me.ChrtModalResponse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea1.AxisX.LineWidth = 2
        ChartArea1.AxisX.Title = "Time [s]"
        ChartArea1.AxisX.TitleFont = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY.LineWidth = 2
        ChartArea1.AxisY.TitleFont = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.Name = "ModalResponse"
        Me.ChrtModalResponse.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.ChrtModalResponse.Legends.Add(Legend1)
        Me.ChrtModalResponse.Location = New System.Drawing.Point(0, 30)
        Me.ChrtModalResponse.Name = "cModalResponse"
        Me.ChrtModalResponse.Size = New System.Drawing.Size(613, 351)
        Me.ChrtModalResponse.TabIndex = 0
        Me.ChrtModalResponse.Text = "Chart1"
        '
        'DialogoAbrir
        '
        Me.DialogoAbrir.FileName = "Open file"
        '
        'pPlot
        '
        Me.pPlot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pPlot.Controls.Add(Me.CbLink)
        Me.pPlot.Controls.Add(Me.CbModes)
        Me.pPlot.Controls.Add(Me.ChrtModalResponse)
        Me.pPlot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pPlot.Location = New System.Drawing.Point(3, 3)
        Me.pPlot.Name = "pPlot"
        Me.pPlot.Size = New System.Drawing.Size(618, 382)
        Me.pPlot.TabIndex = 7
        '
        'cbLink
        '
        Me.CbLink.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbLink.FormattingEnabled = True
        Me.CbLink.Location = New System.Drawing.Point(3, 3)
        Me.CbLink.Name = "cbLink"
        Me.CbLink.Size = New System.Drawing.Size(180, 21)
        Me.CbLink.TabIndex = 2
        '
        'cbModes
        '
        Me.CbModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CbModes.FormattingEnabled = True
        Me.CbModes.Location = New System.Drawing.Point(189, 3)
        Me.CbModes.Name = "cbModes"
        Me.CbModes.Size = New System.Drawing.Size(180, 21)
        Me.CbModes.TabIndex = 1
        '
        'tcResults
        '
        Me.tcResults.Controls.Add(Me.tbTotalLoads)
        Me.tcResults.Controls.Add(Me.tbLoads)
        Me.tcResults.Controls.Add(Me.tbRaw)
        Me.tcResults.Controls.Add(Me.TbModalResponse)
        Me.tcResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcResults.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcResults.Location = New System.Drawing.Point(2, 2)
        Me.tcResults.Name = "tcResults"
        Me.tcResults.SelectedIndex = 0
        Me.tcResults.Size = New System.Drawing.Size(632, 414)
        Me.tcResults.TabIndex = 8
        '
        'tbTotalLoads
        '
        Me.tbTotalLoads.Location = New System.Drawing.Point(4, 22)
        Me.tbTotalLoads.Name = "tbTotalLoads"
        Me.tbTotalLoads.Size = New System.Drawing.Size(624, 388)
        Me.tbTotalLoads.TabIndex = 3
        Me.tbTotalLoads.Text = "Total aerodynamic loads"
        Me.tbTotalLoads.UseVisualStyleBackColor = True
        '
        'tbLoads
        '
        Me.tbLoads.BackColor = System.Drawing.Color.White
        Me.tbLoads.ForeColor = System.Drawing.Color.Black
        Me.tbLoads.Location = New System.Drawing.Point(4, 22)
        Me.tbLoads.Name = "tbLoads"
        Me.tbLoads.Padding = New System.Windows.Forms.Padding(3)
        Me.tbLoads.Size = New System.Drawing.Size(624, 388)
        Me.tbLoads.TabIndex = 0
        Me.tbLoads.Text = "By component"
        '
        'tbRaw
        '
        Me.tbRaw.Controls.Add(Me.TbRawData)
        Me.tbRaw.Location = New System.Drawing.Point(4, 22)
        Me.tbRaw.Name = "tbRaw"
        Me.tbRaw.Padding = New System.Windows.Forms.Padding(3)
        Me.tbRaw.Size = New System.Drawing.Size(624, 388)
        Me.tbRaw.TabIndex = 2
        Me.tbRaw.Text = "Raw data"
        Me.tbRaw.UseVisualStyleBackColor = True
        '
        'tbRawData
        '
        Me.TbRawData.BackColor = System.Drawing.Color.White
        Me.TbRawData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TbRawData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TbRawData.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TbRawData.Location = New System.Drawing.Point(3, 3)
        Me.TbRawData.Margin = New System.Windows.Forms.Padding(60)
        Me.TbRawData.Name = "tbRawData"
        Me.TbRawData.Size = New System.Drawing.Size(618, 382)
        Me.TbRawData.TabIndex = 3
        Me.TbRawData.Text = ""
        '
        'tbModalResponse
        '
        Me.TbModalResponse.Controls.Add(Me.pPlot)
        Me.TbModalResponse.Location = New System.Drawing.Point(4, 22)
        Me.TbModalResponse.Name = "tbModalResponse"
        Me.TbModalResponse.Padding = New System.Windows.Forms.Padding(3)
        Me.TbModalResponse.Size = New System.Drawing.Size(624, 388)
        Me.TbModalResponse.TabIndex = 1
        Me.TbModalResponse.Text = "Modal response"
        Me.TbModalResponse.UseVisualStyleBackColor = True
        '
        'FormReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(636, 418)
        Me.Controls.Add(Me.tcResults)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormReport"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Output panel"
        CType(Me.ChrtModalResponse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pPlot.ResumeLayout(False)
        Me.tcResults.ResumeLayout(False)
        Me.tbRaw.ResumeLayout(False)
        Me.TbModalResponse.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DialogoGuardarReporte As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ChrtModalResponse As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents DialogoAbrir As System.Windows.Forms.OpenFileDialog
    Friend WithEvents pPlot As System.Windows.Forms.Panel
    Friend WithEvents tcResults As System.Windows.Forms.TabControl
    Friend WithEvents tbLoads As System.Windows.Forms.TabPage
    Friend WithEvents TbModalResponse As System.Windows.Forms.TabPage
    Friend WithEvents CbModes As System.Windows.Forms.ComboBox
    Friend WithEvents CbLink As System.Windows.Forms.ComboBox
    Friend WithEvents tbRaw As System.Windows.Forms.TabPage
    Friend WithEvents TbRawData As System.Windows.Forms.RichTextBox
    Friend WithEvents tbTotalLoads As System.Windows.Forms.TabPage
End Class

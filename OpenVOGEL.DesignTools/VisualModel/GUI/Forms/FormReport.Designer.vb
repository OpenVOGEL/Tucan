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
        Me.cModalResponse = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.DialogoAbrir = New System.Windows.Forms.OpenFileDialog()
        Me.pPlot = New System.Windows.Forms.Panel()
        Me.cbLink = New System.Windows.Forms.ComboBox()
        Me.cbModes = New System.Windows.Forms.ComboBox()
        Me.tcResults = New System.Windows.Forms.TabControl()
        Me.tbTotalLoads = New System.Windows.Forms.TabPage()
        Me.tbLoads = New System.Windows.Forms.TabPage()
        Me.tbRaw = New System.Windows.Forms.TabPage()
        Me.tbRawData = New System.Windows.Forms.RichTextBox()
        Me.tbModalResponse = New System.Windows.Forms.TabPage()
        CType(Me.cModalResponse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pPlot.SuspendLayout()
        Me.tcResults.SuspendLayout()
        Me.tbRaw.SuspendLayout()
        Me.tbModalResponse.SuspendLayout()
        Me.SuspendLayout()
        '
        'cModalResponse
        '
        Me.cModalResponse.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ChartArea1.AxisX.TitleFont = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.AxisY.TitleFont = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        ChartArea1.Name = "ChartArea1"
        Me.cModalResponse.ChartAreas.Add(ChartArea1)
        Legend1.Name = "Legend1"
        Me.cModalResponse.Legends.Add(Legend1)
        Me.cModalResponse.Location = New System.Drawing.Point(0, 30)
        Me.cModalResponse.Name = "cModalResponse"
        Me.cModalResponse.Size = New System.Drawing.Size(784, 397)
        Me.cModalResponse.TabIndex = 0
        Me.cModalResponse.Text = "Chart1"
        '
        'DialogoAbrir
        '
        Me.DialogoAbrir.FileName = "Open file"
        '
        'pPlot
        '
        Me.pPlot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pPlot.Controls.Add(Me.cbLink)
        Me.pPlot.Controls.Add(Me.cbModes)
        Me.pPlot.Controls.Add(Me.cModalResponse)
        Me.pPlot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pPlot.Location = New System.Drawing.Point(3, 3)
        Me.pPlot.Name = "pPlot"
        Me.pPlot.Size = New System.Drawing.Size(789, 428)
        Me.pPlot.TabIndex = 7
        '
        'cbLink
        '
        Me.cbLink.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLink.FormattingEnabled = True
        Me.cbLink.Location = New System.Drawing.Point(3, 3)
        Me.cbLink.Name = "cbLink"
        Me.cbLink.Size = New System.Drawing.Size(180, 21)
        Me.cbLink.TabIndex = 2
        '
        'cbModes
        '
        Me.cbModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbModes.FormattingEnabled = True
        Me.cbModes.Location = New System.Drawing.Point(189, 3)
        Me.cbModes.Name = "cbModes"
        Me.cbModes.Size = New System.Drawing.Size(180, 21)
        Me.cbModes.TabIndex = 1
        '
        'tcResults
        '
        Me.tcResults.Controls.Add(Me.tbTotalLoads)
        Me.tcResults.Controls.Add(Me.tbLoads)
        Me.tcResults.Controls.Add(Me.tbRaw)
        Me.tcResults.Controls.Add(Me.tbModalResponse)
        Me.tcResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcResults.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcResults.Location = New System.Drawing.Point(2, 2)
        Me.tcResults.Name = "tcResults"
        Me.tcResults.SelectedIndex = 0
        Me.tcResults.Size = New System.Drawing.Size(803, 460)
        Me.tcResults.TabIndex = 8
        '
        'tbTotalLoads
        '
        Me.tbTotalLoads.Location = New System.Drawing.Point(4, 22)
        Me.tbTotalLoads.Name = "tbTotalLoads"
        Me.tbTotalLoads.Size = New System.Drawing.Size(795, 434)
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
        Me.tbLoads.Size = New System.Drawing.Size(795, 434)
        Me.tbLoads.TabIndex = 0
        Me.tbLoads.Text = "By component"
        '
        'tbRaw
        '
        Me.tbRaw.Controls.Add(Me.tbRawData)
        Me.tbRaw.Location = New System.Drawing.Point(4, 22)
        Me.tbRaw.Name = "tbRaw"
        Me.tbRaw.Padding = New System.Windows.Forms.Padding(3)
        Me.tbRaw.Size = New System.Drawing.Size(795, 434)
        Me.tbRaw.TabIndex = 2
        Me.tbRaw.Text = "Raw data"
        Me.tbRaw.UseVisualStyleBackColor = True
        '
        'tbRawData
        '
        Me.tbRawData.BackColor = System.Drawing.Color.White
        Me.tbRawData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tbRawData.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tbRawData.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tbRawData.Location = New System.Drawing.Point(3, 3)
        Me.tbRawData.Margin = New System.Windows.Forms.Padding(60)
        Me.tbRawData.Name = "tbRawData"
        Me.tbRawData.Size = New System.Drawing.Size(789, 428)
        Me.tbRawData.TabIndex = 3
        Me.tbRawData.Text = ""
        '
        'tbModalResponse
        '
        Me.tbModalResponse.Controls.Add(Me.pPlot)
        Me.tbModalResponse.Location = New System.Drawing.Point(4, 22)
        Me.tbModalResponse.Name = "tbModalResponse"
        Me.tbModalResponse.Padding = New System.Windows.Forms.Padding(3)
        Me.tbModalResponse.Size = New System.Drawing.Size(795, 434)
        Me.tbModalResponse.TabIndex = 1
        Me.tbModalResponse.Text = "Modal response"
        Me.tbModalResponse.UseVisualStyleBackColor = True
        '
        'FormReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(807, 464)
        Me.Controls.Add(Me.tcResults)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FormReport"
        Me.Padding = New System.Windows.Forms.Padding(2)
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Output panel"
        CType(Me.cModalResponse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pPlot.ResumeLayout(False)
        Me.tcResults.ResumeLayout(False)
        Me.tbRaw.ResumeLayout(False)
        Me.tbModalResponse.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DialogoGuardarReporte As System.Windows.Forms.SaveFileDialog
    Friend WithEvents cModalResponse As System.Windows.Forms.DataVisualization.Charting.Chart
    Friend WithEvents DialogoAbrir As System.Windows.Forms.OpenFileDialog
    Friend WithEvents pPlot As System.Windows.Forms.Panel
    Friend WithEvents tcResults As System.Windows.Forms.TabControl
    Friend WithEvents tbLoads As System.Windows.Forms.TabPage
    Friend WithEvents tbModalResponse As System.Windows.Forms.TabPage
    Friend WithEvents cbModes As System.Windows.Forms.ComboBox
    Friend WithEvents cbLink As System.Windows.Forms.ComboBox
    Friend WithEvents tbRaw As System.Windows.Forms.TabPage
    Friend WithEvents tbRawData As System.Windows.Forms.RichTextBox
    Friend WithEvents tbTotalLoads As System.Windows.Forms.TabPage
End Class

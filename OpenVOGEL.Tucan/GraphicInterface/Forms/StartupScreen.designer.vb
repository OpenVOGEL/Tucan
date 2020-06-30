<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StartupScreen
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StartupScreen))
        Me.LblInfo = New System.Windows.Forms.Label()
        Me.LblVersion = New System.Windows.Forms.Label()
        Me.PbxLogo = New System.Windows.Forms.PictureBox()
        Me.LblKernel = New System.Windows.Forms.Label()
        CType(Me.PbxLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LblInfo
        '
        resources.ApplyResources(Me.LblInfo, "LblInfo")
        Me.LblInfo.ForeColor = System.Drawing.Color.Gray
        Me.LblInfo.Name = "LblInfo"
        '
        'LblVersion
        '
        resources.ApplyResources(Me.LblVersion, "LblVersion")
        Me.LblVersion.ForeColor = System.Drawing.Color.Gray
        Me.LblVersion.Name = "LblVersion"
        '
        'PbxLogo
        '
        Me.PbxLogo.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.PbxLogo, "PbxLogo")
        Me.PbxLogo.Name = "PbxLogo"
        Me.PbxLogo.TabStop = False
        '
        'LblKernel
        '
        resources.ApplyResources(Me.LblKernel, "LblKernel")
        Me.LblKernel.ForeColor = System.Drawing.Color.Gray
        Me.LblKernel.Name = "LblKernel"
        '
        'StartupScreen
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.Controls.Add(Me.LblKernel)
        Me.Controls.Add(Me.LblVersion)
        Me.Controls.Add(Me.LblInfo)
        Me.Controls.Add(Me.PbxLogo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "StartupScreen"
        Me.ShowInTaskbar = False
        CType(Me.PbxLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblInfo As System.Windows.Forms.Label
    Friend WithEvents LblVersion As System.Windows.Forms.Label
    Friend WithEvents PbxLogo As PictureBox
    Friend WithEvents LblKernel As Label
End Class

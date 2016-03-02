'Copyright (C) 2016 Guillermo Hazebrouck

Imports AeroTools.VisualModel.Models.Components

Public Class FormJetEngine

    Private _JetEngine As JetEngine
    Private AllowEvents As Boolean = False

    Public Sub New(ByRef JetEngine As JetEngine)

        InitializeComponent()

        _JetEngine = JetEngine

        SetUpControls()

        AllowEvents = True

    End Sub

    Private Sub SetUpControls()

        nudX.DecimalPlaces = 3
        nudX.Minimum = -10000000
        nudX.Maximum = 10000000
        nudX.Value = _JetEngine.Position.X

        nudY.DecimalPlaces = 3
        nudY.Minimum = -10000000
        nudY.Maximum = 10000000
        nudY.Value = _JetEngine.Position.Y

        nudZ.DecimalPlaces = 3
        nudZ.Minimum = -10000000
        nudZ.Maximum = 10000000
        nudZ.Value = _JetEngine.Position.Z

        nudPsi.DecimalPlaces = 3
        nudPsi.Minimum = -180
        nudPsi.Maximum = 180
        nudPsi.Value = _JetEngine.Orientation.Psi / Math.PI * 180

        nudTita.DecimalPlaces = 3
        nudTita.Minimum = -180
        nudTita.Maximum = 180
        nudTita.Value = _JetEngine.Orientation.Tita / Math.PI * 180

        nudPhi.DecimalPlaces = 3
        nudPhi.Minimum = -180
        nudPhi.Maximum = 180
        nudPhi.Value = _JetEngine.Orientation.Fi / Math.PI * 180

        nudFrontD.DecimalPlaces = 3
        nudFrontD.Value = _JetEngine.FrontDiameter

        nudFrontL.DecimalPlaces = 3
        nudFrontL.Value = _JetEngine.FrontLength

        nudRearD.DecimalPlaces = 3
        nudRearD.Value = _JetEngine.BackDiameter

        nudRearL.DecimalPlaces = 3
        nudRearL.Value = _JetEngine.BackLength

        nudTotalL.DecimalPlaces = 3
        nudTotalL.Value = _JetEngine.Length

        tbxName.Text = _JetEngine.Name

    End Sub

    Private Sub nudX_ValueChanged(sender As Object, e As EventArgs) Handles nudX.ValueChanged
        _JetEngine.Position.X = nudX.Value
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudY_ValueChanged(sender As Object, e As EventArgs) Handles nudY.ValueChanged
        _JetEngine.Position.Y = nudY.Value
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudZ_ValueChanged(sender As Object, e As EventArgs) Handles nudZ.ValueChanged
        _JetEngine.Position.Z = nudZ.Value
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudFrontD_ValueChanged(sender As Object, e As EventArgs) Handles nudFrontD.ValueChanged
        _JetEngine.FrontDiameter = nudFrontD.Value
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudRearD_ValueChanged(sender As Object, e As EventArgs) Handles nudRearD.ValueChanged
        _JetEngine.BackDiameter = nudRearD.Value
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudFrontL_ValueChanged(sender As Object, e As EventArgs) Handles nudFrontL.ValueChanged
        _JetEngine.FrontLength = nudFrontL.Value
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudRearL_ValueChanged(sender As Object, e As EventArgs) Handles nudRearL.ValueChanged
        _JetEngine.BackLength = nudRearL.Value
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudTotalL_ValueChanged(sender As Object, e As EventArgs) Handles nudTotalL.ValueChanged
        _JetEngine.Length = nudTotalL.Value
        RaiseEvent UpdateModel()
    End Sub

    Public Event UpdateModel()

    Private Sub nudPsi_ValueChanged(sender As Object, e As EventArgs) Handles nudPsi.ValueChanged
        _JetEngine.Orientation.Psi = nudPsi.Value * Math.PI / 180
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudTita_ValueChanged(sender As Object, e As EventArgs) Handles nudTita.ValueChanged
        _JetEngine.Orientation.Tita = nudTita.Value * Math.PI / 180
        RaiseEvent UpdateModel()
    End Sub

    Private Sub nudPhi_ValueChanged(sender As Object, e As EventArgs) Handles nudPhi.ValueChanged
        _JetEngine.Orientation.Fi = nudPhi.Value * Math.PI / 180
        RaiseEvent UpdateModel()
    End Sub

    Private Sub tbxName_TextChanged(sender As Object, e As EventArgs) Handles tbxName.TextChanged
        _JetEngine.Name = tbxName.Text
        RaiseEvent UpdateModel()
    End Sub

End Class
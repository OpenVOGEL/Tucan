
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

Public Class FormPropeller

    Private _Propeller As Propeller

    Public Event UpdateModel()

    Public Sub New(ByRef Propeller As Propeller)

        InitializeComponent()

        _Propeller = Propeller

        SetUpControls()

        AddHandler nudDiameter.ValueChanged, AddressOf RequestUpdate
        AddHandler nudCentral.ValueChanged, AddressOf RequestUpdate
        AddHandler nudPitch.ValueChanged, AddressOf RequestUpdate
        AddHandler nudChordPanels.ValueChanged, AddressOf RequestUpdate
        AddHandler nudWakeLength.ValueChanged, AddressOf RequestUpdate
        AddHandler nudBlades.ValueChanged, AddressOf RequestUpdate
        AddHandler tbxName.TextChanged, AddressOf RequestUpdate

    End Sub

    Private Sub SetUpControls()

        tbxName.Text = _Propeller.Name

        nudDiameter.DecimalPlaces = 3
        nudDiameter.Minimum = 0.0
        nudDiameter.Value = _Propeller.Diameter

        nudCentral.DecimalPlaces = 1
        nudCentral.Minimum = 0.0
        nudCentral.Maximum = 100.0
        nudCentral.Increment = 1.0
        nudCentral.Value = _Propeller.CentralRing

        nudPitch.DecimalPlaces = 1
        nudPitch.Minimum = -180.0
        nudPitch.Maximum = 180.0
        nudPitch.Increment = 1.0
        nudPitch.Value = _Propeller.CollectivePitch

        nudBlades.DecimalPlaces = 0
        nudBlades.Minimum = 1
        nudBlades.Value = _Propeller.NumberOfBlades

        nudChordPanels.DecimalPlaces = 0
        nudChordPanels.Minimum = 1
        nudChordPanels.Value = _Propeller.NumberOfChordPanels

        nudSpanPanels.DecimalPlaces = 0
        nudSpanPanels.Minimum = 1
        nudSpanPanels.Value = _Propeller.NumberOfSpanPanels

    End Sub

    Private Sub RequestUpdate()

        If _Propeller IsNot Nothing Then

            _Propeller.Diameter = nudDiameter.Value
            _Propeller.CentralRing = nudCentral.Value
            _Propeller.CollectivePitch = nudPitch.Value
            _Propeller.Name = tbxName.Text
            _Propeller.NumberOfBlades = nudBlades.Value
            _Propeller.NumberOfSpanPanels = nudSpanPanels.Value
            _Propeller.NumberOfChordPanels = nudChordPanels.Value

            _Propeller.GenerateMesh()

            RaiseEvent UpdateModel()

        End If

    End Sub

    Private Background As SolidBrush = Brushes.Black
    Private AxisPen As New Pen(Color.DimGray, 2)
    Private GridPen As New Pen(Color.DimGray, 1)
    Private TwistPen As New Pen(Color.Red, 3)
    Private ChordPen As New Pen(Color.Blue, 3)

    Private Sub pbxPlot_Paint(sender As Object, e As PaintEventArgs) Handles pbxPlot.Paint

        Dim G As Graphics = e.Graphics
        G.TranslateTransform(0.0, pbxPlot.Height)
        G.ScaleTransform(1.0, -1.0)

        G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim X_Scale As Single = pbxPlot.Width
        Dim Y_Scale As Single = pbxPlot.Height / 90.0#

        For I = 1 To _Propeller.TwistFunction.Count - 1

            G.DrawLine(TwistPen,
                      X_Scale * CSng(_Propeller.TwistFunction(I - 1).X),
                      Y_Scale * CSng(_Propeller.TwistFunction(I - 1).Y),
                      X_Scale * CSng(_Propeller.TwistFunction(I).X),
                      Y_Scale * CSng(_Propeller.TwistFunction(I).Y))

        Next

        Y_Scale = pbxPlot.Height

        For I = 1 To _Propeller.ChordFunction.Count - 1

            G.DrawLine(ChordPen,
                       X_Scale * CSng(_Propeller.ChordFunction(I - 1).X),
                       Y_Scale * CSng(_Propeller.ChordFunction(I - 1).Y),
                       X_Scale * CSng(_Propeller.ChordFunction(I).X),
                       Y_Scale * CSng(_Propeller.ChordFunction(I).Y))

        Next

    End Sub

    Private Sub btnLoad_Click(sender As Object, e As EventArgs) Handles btnLoad.Click

        If _Propeller IsNot Nothing Then

            Dim Dialog As New OpenFileDialog
            Dim Result = Dialog.ShowDialog()

            If Result = DialogResult.OK Then

                _Propeller.LoadFromFile(Dialog.FileName)

                pbxPlot.Refresh()

                RaiseEvent UpdateModel()

            End If

        End If

    End Sub

End Class
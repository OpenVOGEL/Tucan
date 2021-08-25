'#############################################################################
'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program.  If Not, see < http:  //www.gnu.org/licenses/>.

'' Standard .NET frameworks dependencies
'-----------------------------------------------------------------------------


'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

Public Class FormPropeller

    Private _Propeller As Propeller

    Public Event UpdateModel()

    Public Sub New(ByRef Propeller As Propeller)

        InitializeComponent()

        _Propeller = Propeller

        SetUpControls()

        AddHandler nudDiameter.ValueChanged, AddressOf RequestUpdate
        AddHandler nudPitch.ValueChanged, AddressOf RequestUpdate
        AddHandler nudDeflection.ValueChanged, AddressOf RequestUpdate
        AddHandler nudAxisPosition.ValueChanged, AddressOf RequestUpdate
        AddHandler nudChordPanels.ValueChanged, AddressOf RequestUpdate
        AddHandler nudSpanPanels.ValueChanged, AddressOf RequestUpdate
        AddHandler nudBlades.ValueChanged, AddressOf RequestUpdate
        AddHandler tbxName.TextChanged, AddressOf RequestUpdate

    End Sub

    Private Sub SetUpControls()

        tbxName.Text = _Propeller.Name

        nudDiameter.DecimalPlaces = 3
        nudDiameter.Minimum = 0.0
        nudDiameter.Value = _Propeller.Diameter

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

        nudWakeLength.DecimalPlaces = 0
        nudWakeLength.Minimum = 1
        nudWakeLength.Value = _Propeller.CuttingStep

        nudDeflection.DecimalPlaces = 3
        nudDeflection.Minimum = -0.5
        nudDeflection.Maximum = 0.5
        nudDeflection.Increment = 0.01
        nudDeflection.Value = _Propeller.AxisDeflection

        nudAxisPosition.DecimalPlaces = 3
        nudAxisPosition.Minimum = 0.0
        nudAxisPosition.Maximum = 1.0
        nudAxisPosition.Increment = 0.01
        nudAxisPosition.Value = _Propeller.AxisPosition

    End Sub

    Private Sub RequestUpdate()

        If _Propeller IsNot Nothing Then

            _Propeller.Diameter = nudDiameter.Value
            _Propeller.CollectivePitch = nudPitch.Value
            _Propeller.AxisDeflection = nudDeflection.Value
            _Propeller.AxisPosition = nudAxisPosition.Value
            _Propeller.Name = tbxName.Text
            _Propeller.NumberOfBlades = nudBlades.Value
            _Propeller.NumberOfSpanPanels = nudSpanPanels.Value
            _Propeller.NumberOfChordPanels = nudChordPanels.Value
            _Propeller.CuttingStep = nudWakeLength.Value

            _Propeller.GenerateMesh()

            RaiseEvent UpdateModel()

        End If

    End Sub

    Private Background As SolidBrush = Brushes.DarkGray
    Private AxisPen As New Pen(Color.DimGray, 2)
    Private GridPen As New Pen(Color.DimGray, 1)
    Private TwistPen As New Pen(Color.Red, 3)
    Private ChordPen As New Pen(Color.Blue, 3)
    Private FontLabel As New Font("Segoe UI", 6.5)
    Private FontAxis As New Font("Segoe UI", 10.0)

    Private Sub pbxPlot_Paint(sender As Object, e As PaintEventArgs) Handles pbxPlot.Paint

        Dim G As Graphics = e.Graphics

        G.FillRectangle(Background, pbxPlot.DisplayRectangle)

        G.DrawString("β [°]", FontAxis, Brushes.Red, 30, 2)

        G.DrawString("C/R [°]", FontAxis, Brushes.Blue, pbxPlot.Width - 50, 2)

        For I = 1 To 9

            Dim Y As Single = 1.0 - I / 10

            Dim H As Single = Y * pbxPlot.Height

            G.DrawLine(GridPen, 20, H, pbxPlot.Width - 20, H)

            G.DrawString(String.Format("{0:F0}°", 100.0 * I / 10), FontLabel, Brushes.Red, 4, H - 6)

            G.DrawString(String.Format("{0:F1}", I / 10), FontLabel, Brushes.Blue, pbxPlot.Width - 16, H - 6)

        Next

        G.TranslateTransform(0.0, pbxPlot.Height)

        G.ScaleTransform(1.0, -1.0)

        G.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim X_Scale As Single = pbxPlot.Width
        Dim Y_Scale As Single = pbxPlot.Height / 100.0#

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

    Private Sub btnAirfoil_Click(sender As Object, e As EventArgs) Handles btnAirfoil.Click

        If _Propeller IsNot Nothing Then

            Dim Form As New FormCamberLine(_Propeller.CamberLineId)

            Select Case Form.ShowDialog()

                Case DialogResult.OK

                    _Propeller.CamberLineId = Form.SelectedCamberID
                    _Propeller.GenerateMesh()

                    RaiseEvent UpdateModel()

            End Select

        End If

    End Sub

    Private Sub btnInertia_Click(sender As Object, e As EventArgs) Handles btnInertia.Click

        If _Propeller IsNot Nothing Then

            FormInertia.SetInertia(_Propeller.Inertia)

            If FormInertia.ShowDialog() = DialogResult.OK Then
                _Propeller.Inertia = FormInertia.GetInertia
            End If

        End If

    End Sub

    Private Sub btnPolar_Click(sender As Object, e As EventArgs) Handles btnPolar.Click

        If Model.PolarDataBase IsNot Nothing And _Propeller IsNot Nothing Then

            Dim Id As Guid = Guid.Empty
            If _Propeller.PolarFamiliy IsNot Nothing Then
                Id = _Propeller.PolarFamiliy.Id
            End If

            Dim Form As New FormPolarCurve(Model.PolarDataBase, Id)

            If Form.ShowDialog() = vbOK Then

                If Not Form.SelectedFamilyId.Equals(Guid.Empty) Then
                    _Propeller.PolarFamiliy = Model.PolarDataBase.GetFamilyFromId(Form.SelectedFamilyId)
                    _Propeller.PolarId = Form.SelectedFamilyId
                End If

            End If

        End If

    End Sub

End Class
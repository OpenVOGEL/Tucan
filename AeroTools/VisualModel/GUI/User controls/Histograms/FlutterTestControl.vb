Imports AeroTools.UVLM.SimulationTools
Imports System.Windows.Forms
Imports System.Drawing
Imports MathTools.Algebra.EuclideanSpace

Public Class FlutterTestControl

    Private _Histogram As FlutterTestHistogram
    Private _SimSteps As Integer
    Private _SimStep As Double
    Private _SimSpan As Double
    Private _v As EVector3

    Public Sub New(Histogram As FlutterTestHistogram, SimulationSteps As Integer, SimulationStep As Double, v As EVector3)

        InitializeComponent()

        DoubleBuffered = True

        _Histogram = Histogram
        _SimStep = SimulationStep
        _SimSteps = SimulationSteps
        _SimSpan = SimulationStep * SimulationSteps
        _v = v.Clone

        BindData()

        UpdateHistogram()

        AddHandler _Histogram.ValueChanged, AddressOf UpdateHistogram

    End Sub

    Public Sub BindData()

        If Not IsNothing(_Histogram) Then

            nudHyperDamping.DataBindings.Add("Value", _Histogram, "HyperDamping", False, DataSourceUpdateMode.OnPropertyChanged)
            nudHyperDamping.DecimalPlaces = 4
            nudHyperDamping.Increment = 0.01
            nudHyperDamping.Minimum = 0

            nudHyperdampingSpan.DataBindings.Add("Value", _Histogram, "HyperDampingSpan", False, DataSourceUpdateMode.OnPropertyChanged)
            nudHyperdampingSpan.DecimalPlaces = 4
            nudHyperdampingSpan.Increment = 0.01
            nudHyperdampingSpan.Minimum = 0

            nudNormalDamping.DataBindings.Add("Value", _Histogram, "NormalDamping", False, DataSourceUpdateMode.OnPropertyChanged)
            nudNormalDamping.DecimalPlaces = 4
            nudNormalDamping.Increment = 0.01
            nudNormalDamping.Minimum = 0

            nudGustSpan.DataBindings.Add("Value", _Histogram, "GustSpan", False, DataSourceUpdateMode.OnPropertyChanged)
            nudGustSpan.DecimalPlaces = 4
            nudGustSpan.Increment = 0.01
            nudGustSpan.Minimum = 0

            nudGustX.DataBindings.Add("Value", _Histogram, "GustX", False, DataSourceUpdateMode.OnPropertyChanged)
            nudGustX.Increment = 0.5
            nudGustX.DecimalPlaces = 1

            nudGustY.DataBindings.Add("Value", _Histogram, "GustY", False, DataSourceUpdateMode.OnPropertyChanged)
            nudGustY.Increment = 0.5
            nudGustY.DecimalPlaces = 1

            nudGustZ.DataBindings.Add("Value", _Histogram, "GustZ", False, DataSourceUpdateMode.OnPropertyChanged)
            nudGustZ.Increment = 0.5
            nudGustZ.DecimalPlaces = 1

        End If

    End Sub

    Private _GustX_max As Double
    Private _GustX_min As Double

    Private _GustY_max As Double
    Private _GustY_min As Double

    Private _GustZ_max As Double
    Private _GustZ_min As Double

    Private Sub UpdateHistogram()

        _Histogram.Generate(_v, _SimStep, _SimSteps)

        _GustX_max = Math.Max(_Histogram.GustX + _v.X, _v.X)
        _GustX_min = Math.Min(_Histogram.GustX + _v.X, _v.X)

        _GustY_max = Math.Max(_Histogram.GustY + _v.Y, _v.Y)
        _GustY_min = Math.Min(_Histogram.GustY + _v.Y, _v.Y)

        _GustZ_max = Math.Max(_Histogram.GustZ + _v.Z, _v.Z)
        _GustZ_min = Math.Min(_Histogram.GustZ + _v.Z, _v.Z)

        Refresh()

    End Sub

    Private Sub Plot(obj As Object, e As PaintEventArgs) Handles pbPLot.Paint

        Dim g As Graphics = e.Graphics

        g.FillRectangle(Brushes.White, e.ClipRectangle)
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        Dim m1 As New Drawing2D.Matrix(1, 0, 0, -1, 0, e.ClipRectangle.Height)
        g.Transform = m1

        Dim margin As Integer = 15
        Dim o As New Point(margin, 3 * margin)
        Dim h As Integer = pbPLot.Height - margin - o.Y
        Dim w As Integer = pbPLot.Width - 2 * margin

        Dim nGrid As Integer = 11

        For i = 0 To nGrid

            Dim p0 As New Point(o.X, o.Y + i / nGrid * h)
            Dim p1 As New Point(o.X + w, p0.Y)
            g.DrawLine(Pens.Gainsboro, p0, p1)

            p0 = New Point(o.X + i / nGrid * w, o.Y)
            p1 = New Point(p0.X, o.Y + h)
            g.DrawLine(Pens.Gainsboro, p0, p1)

        Next

        Dim pa As New Point(0, 0)
        Dim pb As New Point(0, 0)

        Dim Gust_max As Double = Math.Max(_Histogram.GustX, Math.Max(_Histogram.GustY, _Histogram.GustZ))
        Dim green_Pen As New Pen(Brushes.LimeGreen, 2)
        Dim red_Pen As New Pen(Brushes.Red, 2)
        Dim magenta_Pen As New Pen(Brushes.DarkViolet, 2)

        Dim f As New Font("Segoe UI", 7)

        For i = 1 To _SimSteps - 1

            Dim step0 As StepData = _Histogram.State(i - 1)
            Dim step1 As StepData = _Histogram.State(i)

            Dim ta As Double = step0.Time / _SimSpan
            Dim tb As Double = step1.Time / _SimSpan

            pa.X = o.X + ta * w
            pb.X = o.X + tb * w

            ' Lines:

            If (_GustX_max > _GustX_min) Then
                pa.Y = ((step0.Velocity.X - _GustX_min) / Gust_max) * h + o.Y
                pb.Y = ((step1.Velocity.X - _GustX_min) / Gust_max) * h + o.Y
                g.DrawLine(green_Pen, pa, pb)
            End If

            If (_GustY_max > _GustY_min) Then
                pa.Y = ((step0.Velocity.Y - _GustY_min) / Gust_max) * h + o.Y
                pb.Y = ((step1.Velocity.Y - _GustY_min) / Gust_max) * h + o.Y
                g.DrawLine(red_Pen, pa, pb)
            End If

            If (_GustZ_max > _GustZ_min) Then
                pa.Y = ((step0.Velocity.Z - _GustZ_min) / Gust_max) * h + o.Y
                pb.Y = ((step1.Velocity.Z - _GustZ_min) / Gust_max) * h + o.Y
                g.DrawLine(magenta_Pen, pa, pb)
            End If

        Next

        For i = 0 To _SimSteps - 1

            Dim step0 As StepData = _Histogram.State(i)

            Dim ta As Double = step0.Time / _SimSpan

            pa.X = o.X + ta * w

            ' Markers:

            If (_GustX_max > _GustX_min) Then
                pa.Y = ((step0.Velocity.X - _GustX_min) / Gust_max) * h + o.Y
                Dim r As New Rectangle(pa.X - 2, pa.Y - 2, 4, 4)
                g.DrawEllipse(Pens.Black, r)
                g.FillEllipse(Brushes.White, r)
            End If

            If (_GustY_max > _GustY_min) Then
                pa.Y = ((step0.Velocity.Y - _GustY_min) / Gust_max) * h + o.Y
                Dim r As New Rectangle(pa.X - 2, pa.Y - 2, 4, 4)
                g.DrawEllipse(Pens.Black, r)
                g.FillEllipse(Brushes.White, r)
            End If

            If (_GustZ_max > _GustZ_min) Then
                pa.Y = ((step0.Velocity.Z - _GustZ_min) / Gust_max) * h + o.Y
                Dim r As New Rectangle(pa.X - 2, pa.Y - 2, 4, 4)
                g.DrawEllipse(Pens.Black, r)
                g.FillEllipse(Brushes.White, r)
            End If

        Next

        ' Hyper damping span strip:

        Dim rhd As New Rectangle(o.X, margin, _Histogram.HyperDampingSpan / _SimSpan * w, margin)
        g.FillRectangle(Brushes.SkyBlue, rhd)

        Dim s As String = String.Format("Hyper damping {0}s - ξ={1}", _Histogram.HyperDampingSpan, _Histogram.HyperDamping)
        Dim s_size As SizeF = g.MeasureString(s, f)

        Dim m2 As New Drawing2D.Matrix(1, 0, 0, 1, 0, 0)
        g.Transform = m2

        Dim p As Point

        If rhd.Width > s_size.Width Then
            p.Y = e.ClipRectangle.Height - 1.5 * margin - 0.5 * s_size.Height
            p.X = o.X + Math.Min(0.5 * (rhd.Width - s_size.Width), 0.5 * (w - s_size.Width))
        Else
            p.Y = h + 1.5 * margin - 0.5 * s_size.Height
            p.X = o.X
        End If

        g.DrawString(s, f, Brushes.Blue, p)

    End Sub

End Class

Imports System.Drawing
Imports GaugesTools.IndicationTools.GeometricTools
Imports System.Drawing.Drawing2D

Namespace IndicationTools.Indicators

    ''' <summary>
    ''' Provides a graphical representation of an attitude indicator with primary flight data
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AttitudeIndicator

        Implements IIndicator

        Private _Region As Rectangle

        Public Property AirSpeed As Single
        Public Property Roll As Single
        Public Property Pitch As Single
        Public Property Yaw As Single
        Public Property Slide As Single

        Public Property StaticPressure As Single       ' [Pa]
        Public Property Altitude As Single             ' [m]
        Public Property VerticalSpeed As Single        ' [m/s]

        Public Property AltitudeResolution As Single = 5 ' [m]
        Public Property AltitudMarks As Short = 4
        Public Property RelativeAltitude As Boolean = True
        Public Property VarioResolution As Single = 2
        Public Property PitchResolution As Single = 5   ' [⁰]
        Public Property PitchMarks As Short = 4
        Public Property GroundAltitude As Single = 0.0

        Public Temperature As Single

        Private f1 As New Font("Vrinda", 11)
        Private f2 As New Font("Vrinda", 10)
        Private f3 As New Font("Vrinda", 15)
        Private f4 As New Font("Vrinda", 7)
        Private pen1 As New Pen(Brushes.White, 1)
        Private pen2 As New Pen(Brushes.Black, 8)
        Private pen3 As New Pen(Brushes.White, 5)
        Private pen4 As New Pen(Brushes.White, 1.5)
        Private brush1 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(55, 55, 55))
        Private brush2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.White, Color.Red)
        Private r_alt As Rectangle
        Private r_att As Rectangle
        Private r_asp As Rectangle
        Private r_hor As Rectangle
        Private r_var As Rectangle
        Private center As PointF
        Private SkyBrush As New SolidBrush(Color.FromArgb(33, 151, 248))
        Private unit As Single
        Private bipperAltitude As New Timers.Timer(2000)
        Private bipperSinkRate As New Timers.Timer(500)

        Public Sub New()

            StaticPressure = 101325
            AddHandler bipperAltitude.Elapsed, AddressOf PlayAltitudeAlarm
            AddHandler bipperSinkRate.Elapsed, AddressOf PlaySinkRateAlarm

        End Sub

        Public Property Region As Rectangle Implements IIndicator.Region
            Set(ByVal value As Rectangle)

                _Region = value

                r_att = Region

                center.X = r_att.Left + 0.5 * r_att.Width
                center.Y = r_att.Top + 0.5 * r_att.Height
                unit = 0.4 * r_att.Height
                r_hor.Width = Math.Sqrt(r_att.Width ^ 2 + r_att.Height ^ 2)
                r_hor.Height = r_hor.Width
                r_hor.X = -0.5 * r_hor.Width

                r_asp.X = Region.X + Math.Round(0.05 * Region.Width)
                r_asp.Y = Region.Y + Math.Round(0.15 * Region.Height)
                r_asp.Height = Math.Round(0.7 * Region.Height)
                r_asp.Width = Math.Round(0.15 * Region.Width)

                r_alt.X = Region.X + Math.Round(0.8 * Region.Width)
                r_alt.Y = Region.Y + Math.Round(0.15 * Region.Height)
                r_alt.Height = Math.Round(0.7 * Region.Height)
                r_alt.Width = Math.Round(0.15 * Region.Width)

                r_var.X = Region.X + Math.Round(0.72 * Region.Width)
                r_var.Y = Region.Y + Math.Round(0.6 * Region.Height)
                r_var.Height = Math.Round(0.18 * Region.Width)
                r_var.Width = r_var.Height

            End Set
            Get
                Return _Region
            End Get
        End Property

        Public Sub DrawControl(ByRef g As Graphics) Implements IIndicator.DrawControl

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.FillRectangle(Brushes.Black, Region)

            ' attitude (horizon):

            g.DrawRectangle(pen1, r_att)
            If Pitch < -50 Then
                g.FillRectangle(Brushes.DarkOrange, r_att)
            Else
                g.FillRectangle(SkyBrush, r_att)
            End If

            g.SetClip(r_att)
            g.TranslateTransform(center.X, center.Y)
            g.RotateTransform(Roll)
            r_hor.Y = Pitch * unit / (PitchResolution * PitchMarks)
            g.FillRectangle(Brushes.DarkOrange, r_hor)
            g.DrawRectangle(pen1, r_hor)

            Dim rec As Rectangle
            Dim c1, c2 As Point
            Dim delta As Single

            ' pitching angle:

            Dim delta_p As Single

            If Pitch > 0 Then
                delta_p = (Pitch - Math.Floor(Pitch / PitchResolution) * PitchResolution) / PitchResolution
            Else
                delta_p = (Pitch - Math.Ceiling(Pitch / PitchResolution) * PitchResolution) / PitchResolution
            End If

            delta = 0.8 * r_att.Height / (2 * PitchMarks)

            Dim p As Single
            Dim dp As Single = Math.Truncate(Pitch / PitchResolution)
            c1.X = 0.2 * unit
            c2.X = -c1.X
            Dim sfr As New StringFormat()
            sfr.LineAlignment = StringAlignment.Center
            sfr.Alignment = StringAlignment.Center

            Dim lowerL As Integer
            If delta_p > 0 Then lowerL = -PitchMarks + 1 Else lowerL = -PitchMarks + 2

            Dim upperL As Integer
            If delta_p > 0 Then upperL = PitchMarks - 1 Else upperL = PitchMarks '- 1

            For i = lowerL To upperL
                p = Math.Abs(PitchResolution * (dp - i))
                If p < 0.01 Then Continue For
                c1.Y = (i + delta_p) * delta
                c2.Y = c1.Y
                g.DrawLine(pen1, c1, c2)
                g.DrawString(String.Format("{0}", p), f3, Brushes.White, c2.X - 18, c2.Y, sfr)
                g.DrawString(String.Format("{0}", p), f3, Brushes.White, c1.X + 18, c1.Y, sfr)
            Next

            c1.X = 0.1 * unit
            c2.X = -c1.X

            For i = lowerL To upperL
                c1.Y = (i - 0.5 + delta_p) * delta
                c2.Y = c1.Y
                g.DrawLine(pen1, c1, c2)
            Next

            'pen1.Width = 2
            'c1.X = 0
            'c1.Y = unit
            'c2.X = 0
            'c2.Y = -unit
            'g.DrawLine(pen1, c1, c2)

            pen1.Color = Color.Black
            c1.X = unit
            c1.Y = 0
            c2.X = -unit
            c2.Y = 0
            pen1.Width = 1
            rec.Location = c2
            rec.Width = 2 * delta
            rec.Height = 2 * f1.Size
            delta = 0.1 * unit
            pen2.StartCap = Drawing2D.LineCap.ArrowAnchor
            g.DrawLine(pen2, 0, -unit, 0, -unit + delta)
            pen1.Color = Color.White

            g.RotateTransform(-Roll)

            Dim pa As PointF
            Dim pb As PointF

            ' sliding (side velocity):

            g.TranslateTransform(-center.X, -center.Y)
            g.SetClip(_Region)

            pen1.Color = Color.White
            pen1.Width = 1
            delta = 0.1 * unit
            g.DrawArc(pen1, center.X - unit, center.Y - unit, 2 * unit, 2 * unit, 210, 120)
            g.DrawLine(pen2, center.X, center.Y - unit, center.X, center.Y - unit - delta)
            Dim sf As New StringFormat()
            sf.Alignment = StringAlignment.Center
            g.DrawString(String.Format("{0:F0}⁰", Yaw), f2, Brushes.White, center.X, center.Y - unit - 2.1 * delta, sf)

            For i = 1 To 4
                Dim sin As Single = Math.Sin(i * Math.PI / 12)
                Dim cos As Single = Math.Cos(i * Math.PI / 12)
                pa.X = center.X + unit * sin
                pa.Y = center.Y - unit * cos
                pb.X = center.X + (unit + 12) * sin
                pb.Y = center.Y - (unit + 12) * cos
                g.DrawLine(pen1, pa, pb)
                pa.X = center.X - unit * sin
                pb.X = center.X - (unit + 12) * sin
                g.DrawLine(pen1, pa, pb)
            Next

            Dim sfh As New StringFormat
            sfh.Alignment = StringAlignment.Far

            ' velocity:

            g.SetClip(r_asp)
            rec.X = r_asp.X + 0.1 * r_asp.Width
            rec.Y = r_asp.Y + 0.45 * r_asp.Height
            rec.Width = 0.8 * r_asp.Width
            rec.Height = 0.4 * r_asp.Width
            g.FillRectangle(brush1, rec)
            g.DrawString(String.Format("{0:F1}", AirSpeed), f1, Brushes.White, rec.Right, rec.Top, sfh)

            ' altitude:

            g.SetClip(r_alt)
            g.TranslateTransform(r_alt.X, r_alt.Y)

            Dim p1 As Point
            Dim p2 As Point
            Dim pf As Point
            Dim cy As Integer = Math.Round(0.5 * r_alt.Height)
            Dim s As Integer = Math.Round(r_alt.Height / (2 * AltitudMarks))
            p1.X = Math.Round(0.8 * r_alt.Width)
            p2.X = r_alt.Width
            pf.X = Math.Round(0.2 * r_alt.Width)

            If _Altitude - _GroundAltitude < 6 * AltitudeResolution Then
                rec.X = 0.1 * r_alt.Width
                rec.Y = Math.Max(cy + s * (_Altitude - _GroundAltitude) / AltitudeResolution, 0)
                rec.Width = 0.8 * r_alt.Width
                rec.Height = r_alt.Height
                g.FillRectangle(brush2, rec)
            End If

            Dim dispAlt As Single
            If RelativeAltitude Then
                dispAlt = _Altitude - _GroundAltitude
            Else
                dispAlt = _Altitude
            End If

            Dim delta_h As Single
            If dispAlt > 0 Then
                delta_h = (dispAlt - Math.Floor(dispAlt / AltitudeResolution) * AltitudeResolution) / AltitudeResolution
            Else
                delta_h = (dispAlt - Math.Ceiling(dispAlt / AltitudeResolution) * AltitudeResolution) / AltitudeResolution
            End If

            Dim h As Single
            Dim dh As Single = Math.Truncate(dispAlt / AltitudeResolution)

            pf.X = Math.Round(0.8 * r_alt.Width)
            For i As Short = -AltitudMarks To AltitudMarks
                p1.Y = cy + s * (i + delta_h)
                p2.Y = p1.Y
                pf.Y = p1.Y - Math.Round(0.5 * f1.Height)
                g.DrawLine(pen1, p1, p2)
                h = AltitudeResolution * (dh - i)
                If h >= 0 Then
                    g.DrawString(String.Format("{0}", h), f2, Brushes.White, pf, sfh)
                End If
            Next

            rec.X = 0.1 * r_alt.Width
            rec.Y = 0.45 * r_alt.Height
            rec.Width = 0.8 * r_alt.Width
            rec.Height = 0.4 * r_alt.Width
            g.FillRectangle(brush1, rec)
            g.DrawString(String.Format("{0:F0}", dispAlt), f1, Brushes.White, rec.Right, rec.Top, sfh)

            ' wing reference bars:

            g.TranslateTransform(-r_alt.X, -r_alt.Y)
            g.SetClip(r_att)

            g.FillEllipse(Brushes.Black, center.X - 5, center.Y - 5, 10, 10)
            g.DrawEllipse(pen1, center.X - 5, center.Y - 5, 10, 10)

            pen2.StartCap = LineCap.Round
            pen2.EndCap = LineCap.Round
            pen3.StartCap = LineCap.Round
            pen3.EndCap = LineCap.Round

            p1.X = center.X - 0.7 * unit
            p1.Y = center.Y
            p2.X = center.X - 0.2 * unit
            p2.Y = center.Y
            g.DrawLine(pen2, p1, p2)
            g.DrawLine(pen3, p1, p2)

            p1.X = center.X + 0.7 * unit
            p1.Y = center.Y
            p2.X = center.X + 0.2 * unit
            p2.Y = center.Y
            g.DrawLine(pen2, p1, p2)
            g.DrawLine(pen3, p1, p2)

            ' variometer:

            g.TranslateTransform(r_var.X, r_var.Y)

            g.DrawArc(pen1, 0, 0, r_var.Width, r_var.Height, 105, 150)
            s = 0.5 * r_var.Width
            p1.X = s
            p1.Y = s
            Dim angle As Double = _VerticalSpeed / VarioResolution / 12 * Math.PI
            p2.X = p1.X - s * Math.Cos(angle) ' 15deg/(step)
            p2.Y = p1.Y - s * Math.Sin(angle)
            pen4.Width = 2
            pen4.StartCap = LineCap.RoundAnchor
            pen4.EndCap = LineCap.ArrowAnchor
            g.DrawLine(pen4, p1, p2)

            Dim p3 As Point

            For i = -5 To 5
                angle = i / 12 * Math.PI
                p2.X = p1.X - s * Math.Cos(angle)
                p2.Y = p1.Y - s * Math.Sin(angle)
                p3.X = p2.X - 0.15 * s * Math.Cos(angle)
                p3.Y = p2.Y - 0.15 * s * Math.Sin(angle)
                g.DrawLine(pen1, p3, p2)
            Next

            rec.X = p1.X
            rec.Y = p1.Y
            rec.Width = 0.7 * r_var.Width
            rec.Height = 0.2 * r_var.Width
            g.FillRectangle(brush1, rec)
            g.DrawString(String.Format("{0:F1}", _VerticalSpeed), f2, Brushes.White, rec.Right, rec.Top, sfh)

            g.TranslateTransform(-r_var.X, -r_var.Y)

            ' current atmospheric info:

            Dim Baro As Single = 101325 * Math.Pow(1 - _GroundAltitude / 44330, 5.255)
            Dim text As String = String.Format("TEMP {0:F0}°C - BARO {1:F2}hPa", Temperature, 0.01 * Baro)
            rec.X = r_att.X
            rec.Y = r_att.Y - 20
            rec.Height = 20
            rec.Width = g.MeasureString(text, f2).Width
            g.SetClip(rec)
            g.DrawString(text, f2, Brushes.LimeGreen, rec.X, rec.Y)


        End Sub

        Private Sub PlayAltitudeAlarm(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
            'If AltitudeAlarm Then My.Computer.Audio.Play(My.Resources.OUTERMK, AudioPlayMode.WaitToComplete)
        End Sub

        Private Sub PlaySinkRateAlarm(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
            'If SinkRateAlarm Then My.Computer.Audio.Play(My.Resources.STALLHRN, AudioPlayMode.WaitToComplete)
        End Sub

    End Class

End Namespace

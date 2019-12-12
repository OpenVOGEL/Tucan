'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2020 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports System.Drawing

Public Class PolarPlotter

    Private _Family As PolarFamily
    Private _Polar As IPolarCurve

    Private AxisPen As New Pen(Color.DarkGray, 2)
    Private GridPen As New Pen(Color.Gainsboro, 1)
    Private OtherCurvePen As New Pen(Color.Gray, 2)
    Private ActiveCurvePen As New Pen(Color.Blue, 2)
    Private MarkerPen As New Pen(Color.Black, 1)
    Private FontLabel As New Font("Segoe UI", 6.5)
    Private MarginX As Integer = 10
    Private MarginY As Integer = 10

    Private HighlightedPointIndex As Integer
    Private DragPoint As Boolean = False
    Private DragOrigin As Boolean = False

    Private Xrange = 3.0
    Private Ymax As Double = 0.1#
    Private Xmin As Double = -0.5 * Xrange
    Private Xmax As Double = 0.5 * Xrange

    Private MousePoint As New PointF(Single.MinValue, Single.MinValue)
    Private DownPoint As New PointF(Single.MinValue, Single.MinValue)
    Private XminDown As Double = Xmin
    Private XmaxDown As Double = Xmax

    Public Sub SetPolars(Family As PolarFamily, Curve As IPolarCurve)

        If Curve IsNot Nothing AndAlso Family.Polars.Contains(Curve) Then
            _Family = Family
            _Polar = Curve
            Refresh()
        Else
            ClearPlotter()
        End If

    End Sub

    Public Sub ClearPlotter()

        _Family = Nothing
        _Polar = Nothing
        Refresh()

    End Sub

    Public Sub ClearPolar()

        _Polar = Nothing
        Refresh()

    End Sub

    Public ReadOnly Property Polar As IPolarCurve
        Get
            Return _Polar
        End Get
    End Property

    Private Sub DrawCurve(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        ' Gridlines and axes

        DrawGrid(e.Graphics)

        ' All curves of the family in the background 

        If _Family IsNot Nothing Then

            For Each OtherPolar In _Family.Polars

                If OtherPolar IsNot _Polar Then
                    DrawCurve(OtherPolar, OtherCurvePen, False, e.Graphics, e.ClipRectangle.Width, e.ClipRectangle.Height)
                End If

            Next

        End If

        ' Selected polar

        DrawCurve(_Polar, ActiveCurvePen, True, e.Graphics, e.ClipRectangle.Width, e.ClipRectangle.Height)

    End Sub

    Private Sub DrawGrid(g As Graphics)

        Dim nx As Integer = 10
        Dim x As Single
        Dim dx As Double = Xmax - Xmin
        Dim GraphW As Integer = Width - 2 * MarginX
        Dim GraphH As Integer = Height - 2 * MarginY
        Dim LimitXmax = MarginX + GraphW
        Dim LimitXmin = MarginX
        Dim LimitYmax = MarginY + GraphH
        Dim LimitYmin = MarginY

        Dim ny As Integer = 10

        For i = 1 To ny - 1

            Dim y As Single = MarginY + i * GraphH / ny
            g.DrawLine(GridPen, LimitXmin, y, LimitXmax, y)

        Next

        g.DrawLine(AxisPen, LimitXmin, LimitYmin, LimitXmin, LimitYmax)
        g.DrawLine(AxisPen, LimitXmin, LimitYmin, LimitXmax, LimitYmin)
        g.DrawLine(AxisPen, LimitXmax, LimitYmin, LimitXmax, LimitYmax)
        g.DrawLine(AxisPen, LimitXmax, LimitYmax, LimitXmin, LimitYmax)

        For i = 0 To nx

            x = MarginX + ((i + Math.Round(Xmin / dx * nx)) / nx - Xmin / dx) * GraphW

            If x > LimitXmin And x < LimitXmax Then
                g.DrawLine(GridPen, x, LimitYmin, x, LimitYmax)
            End If

        Next

        If Xmax > 0 And Xmin < 0 Then
            x = CSng(MarginX - GraphW * Xmin / dx)
            g.DrawLine(AxisPen, x, MarginY, x, MarginY + GraphH)
        End If

    End Sub

    Private Sub DrawCurve(Curve As IPolarCurve, CurvePen As Pen, WithNodes As Boolean, g As Graphics, W As Integer, H As Integer)

        If Curve IsNot Nothing Then

            g.DrawRectangle(Pens.DarkGray, 0, 0, W - 1, H - 1)

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Dim gW As Integer = Width - 2 * MarginX
            Dim gH As Integer = Height - 2 * MarginY

            ' Function:

            If TypeOf Curve Is CustomPolar Then

                Dim Custom As CustomPolar = Curve

                If Custom.Nodes.Count > 0 Then

                    Dim dx As Double = Xmax - Xmin

                    If dx > 0 And Ymax > 0 Then

                        ' update position of selected point:

                        If WithNodes Then

                            If DragPoint AndAlso HighlightedPointIndex >= 0 AndAlso HighlightedPointIndex < Custom.Nodes.Count Then

                                Custom.Nodes(HighlightedPointIndex).X = (MousePoint.X - MarginX) / gW * dx + Xmin
                                Custom.Nodes(HighlightedPointIndex).Y = (1 - (MousePoint.Y - MarginY) / gH) * Ymax

                            End If

                        End If

                        Dim pnts(Custom.Nodes.Count) As Point

                            ' convert to screen coordinates

                            For i = 0 To Custom.Nodes.Count - 1

                                pnts(i).X = MarginX + gW * (Custom.Nodes(i).X - Xmin) / dx
                                pnts(i).Y = MarginY + gH * (1 - Custom.Nodes(i).Y / Ymax)

                            Next

                            ' draw lines:

                            For i = 1 To Custom.Nodes.Count - 1

                                g.DrawLine(CurvePen, pnts(i - 1), pnts(i))

                            Next

                            ' draw points:

                            If WithNodes Then

                                HighlightedPointIndex = -1
                                Dim pnt As Point

                                For i = 0 To Custom.Nodes.Count - 1

                                    pnt = pnts(i)

                                    Dim dmx As Single = pnt.X - MousePoint.X
                                    Dim dmy As Single = pnt.Y - MousePoint.Y

                                    ' draw marker:

                                    If dmx * dmx + dmy * dmy < 9 Then

                                        HighlightedPointIndex = i

                                        g.FillEllipse(Brushes.Orange, pnt.X - 3, pnt.Y - 3, 6, 6)
                                        g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                                    Else

                                        g.FillEllipse(Brushes.White, pnt.X - 3, pnt.Y - 3, 6, 6)
                                        g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                                    End If

                                Next

                                ' draw label on selected point:

                                If HighlightedPointIndex >= 0 Then

                                    pnt = pnts(HighlightedPointIndex)

                                    Dim lblPoint As String = String.Format("CL = {0:F3} / CD = {1:F4}", Custom.Nodes(HighlightedPointIndex).X, Custom.Nodes(HighlightedPointIndex).Y)

                                    DrawLabel(g, lblPoint, pnt, FontLabel)

                                End If

                            End If

                        End If

                    End If

            ElseIf TypeOf Curve Is QuadraticPolar Then

                Dim np As Integer = 30
                Dim dx As Double = Xmax - Xmin

                Dim pnts(np) As Point
                Dim vals(np) As PointF

                For i = 0 To np

                    vals(i).X = Xmin + dx * i / np
                    vals(i).Y = Curve.SkinDrag(vals(i).X)

                    pnts(i).X = MarginX + i / np * gW
                    pnts(i).Y = MarginY + gH * (1 - vals(i).Y / Ymax)

                    If i > 0 Then g.DrawLine(CurvePen, pnts(i - 1), pnts(i))

                Next

                ' draw points:

                If WithNodes Then

                    HighlightedPointIndex = -1
                    Dim pnt As Point

                    For i = 0 To np

                        pnt = pnts(i)

                        Dim dmx As Single = pnt.X - MousePoint.X
                        Dim dmy As Single = pnt.Y - MousePoint.Y

                        ' draw marker:

                        If dmx * dmx + dmy * dmy < 9 Then

                            HighlightedPointIndex = i

                            g.FillEllipse(Brushes.Orange, pnt.X - 3, pnt.Y - 3, 6, 6)
                            g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                        Else

                            g.FillEllipse(Brushes.White, pnt.X - 3, pnt.Y - 3, 6, 6)
                            g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                        End If

                    Next

                    ' draw label on selected point:

                    If HighlightedPointIndex >= 0 Then

                        pnt = pnts(HighlightedPointIndex)

                        Dim lblPoint As String = String.Format("CL = {0:F3} / CD = {1:F4}", vals(HighlightedPointIndex).X, vals(HighlightedPointIndex).Y)

                        DrawLabel(g, lblPoint, pnt, FontLabel)

                    End If

                End If

            End If

        End If

    End Sub

    Private Sub DrawLabel(g As Graphics, Content As String, Point As Point, Font As Font, Optional Leg As Integer = 10, Optional Mrg As Integer = 2)

        Dim lblSize As SizeF = g.MeasureString(Content, Font)

        If Point.X + Leg + Mrg + lblSize.Width < Width Then

            If Point.Y + Leg + lblSize.Height + Mrg < Height Then

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y + Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y + Leg - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y + Leg)

            Else

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y - Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y - Leg - lblSize.Height - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y - Leg - lblSize.Height)

            End If

        Else

            If Point.Y + Leg + lblSize.Height + Mrg < Height Then

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X - Leg, Point.Y + Leg)
                g.FillRectangle(Brushes.Orange, Point.X - Leg - Mrg - lblSize.Width, Point.Y + Leg - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X - Leg - lblSize.Width, Point.Y + Leg)

            Else

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X - Leg, Point.Y - Leg)
                g.FillRectangle(Brushes.Orange, Point.X - Leg - Mrg - lblSize.Width, Point.Y - Leg - lblSize.Height - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X - Leg - lblSize.Width, Point.Y - Leg - lblSize.Height)

            End If

        End If

    End Sub

    Private Sub PolarPlotter_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

        Select Case e.Button

            Case Windows.Forms.MouseButtons.Left

                DragPoint = True

                MousePoint.X = e.X
                MousePoint.Y = e.Y

                Refresh()

            Case Windows.Forms.MouseButtons.Middle

                DragOrigin = True

                DownPoint.X = e.X
                DownPoint.Y = e.Y
                XminDown = Xmin
                XmaxDown = Xmax

                Refresh()

        End Select

    End Sub

    Private Sub PolarPlotter_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        MousePoint.X = e.X
        MousePoint.Y = e.Y

        If DragOrigin Then

            Dim DeltaX As Double = Xrange * (DownPoint.X - e.X) / (Width - 2.0 * MarginX)
            Dim DeltaY As Double = Ymax * (DownPoint.Y - e.Y) / (Height - 2.0 * MarginY)

            Xmax = XmaxDown + DeltaX
            Xmin = XminDown + DeltaX

        End If

        Refresh()

    End Sub

    Private Sub PolarPlotter_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp

        DragPoint = False
        DragOrigin = False

        RaiseEvent PointChanged()

    End Sub

    Private Sub PolarPlotter_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseWheel

        Ymax = Math.Min(0.2, Math.Max(0.01, Ymax - 0.01 * e.Delta / Windows.Forms.SystemInformation.MouseWheelScrollDelta))

        Refresh()

    End Sub

    Public Event PointChanged()

End Class

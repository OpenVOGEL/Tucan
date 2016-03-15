'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

Imports AeroTools.UVLM.Models.Aero.Components
Imports System.Drawing

Public Class PolarPlotter

    Private _Polar As IPolarCurve

    Public Property Polar As IPolarCurve
        Set(value As IPolarCurve)
            _Polar = value
            Refresh()
        End Set
        Get
            Return _Polar
        End Get
    End Property

    Private AxisPen As New Pen(Color.DarkGray, 2)
    Private GridPen As New Pen(Color.Gainsboro, 1)
    Private CurvePen As New Pen(Color.Blue, 2)
    Private MarkerPen As New Pen(Color.Black, 1)
    Private FontLabel As New Font("Segoe UI", 6.5)
    Private mX As Integer = 10
    Private mY As Integer = 10

    Private SelectedPointIndex As Integer
    Private DragPoint As Boolean = False

    Private Sub DrawCurve(sender As Object, e As Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        If Polar IsNot Nothing Then

            Dim g As Graphics = e.Graphics

            g.DrawRectangle(Pens.DarkGray, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Dim gW As Integer = Width - 2 * mX
            Dim gH As Integer = Height - 2 * mY

            ' Gridlines:

            Dim ny As Integer = 10

            For i = 1 To ny - 1

                Dim y As Single = mY + i * gH / ny
                g.DrawLine(GridPen, mX, y, mX + gW, y)

            Next

            g.DrawLine(AxisPen, mX, mY + gH, mX + gW, mY + gH)
            g.DrawLine(AxisPen, mX, mY, mX, mY + gH)
            g.DrawLine(AxisPen, mX + gW, mY, mX + gW, mY + gH)

            ' Function:

            Dim np As Integer = 30
            Dim ymax As Double = 0
            Dim xmin As Double = -1
            Dim xmax As Double = 1

            If TypeOf Polar Is CustomPolar Then

                Dim cPolar As CustomPolar = Polar

                If cPolar.Nodes.Count > 0 Then

                    ymax = cPolar.Nodes(0).Y
                    xmin = cPolar.Nodes(0).X
                    xmax = cPolar.Nodes(0).X

                    For i = 0 To cPolar.Nodes.Count - 1

                        ymax = Math.Max(ymax, cPolar.Nodes(i).Y)
                        xmin = Math.Min(xmin, cPolar.Nodes(i).X)
                        xmax = Math.Max(xmax, cPolar.Nodes(i).X)

                    Next

                    Dim dx As Double = xmax - xmin

                    If dx > 0 Then

                        ' update position of selected point:

                        If DragPoint AndAlso SelectedPointIndex >= 0 AndAlso SelectedPointIndex < cPolar.Nodes.Count Then

                            cPolar.Nodes(SelectedPointIndex).X = (MousePoint.X - mX) / gW * dx + xmin
                            cPolar.Nodes(SelectedPointIndex).Y = (1 - (MousePoint.Y - mY) / gH) * ymax

                        End If

                        ' draw grid:

                        Dim nx As Integer = 10
                        Dim x As Single

                        For i = 1 To nx - 1

                            x = mX + ((i + Math.Round(xmin / dx * nx)) / nx - xmin / dx) * gW
                            g.DrawLine(GridPen, x, mY, x, mY + gH)

                        Next

                        x = CSng(mX + gW * Math.Abs(xmin) / dx)
                        g.DrawLine(AxisPen, x, mY, x, mY + gH)
                        g.DrawLine(AxisPen, mX, mY, mX + gW, mY)

                        Dim pnts(cPolar.Nodes.Count) As Point

                        ' convert to screen coordinates

                        For i = 0 To cPolar.Nodes.Count - 1

                            pnts(i).X = mX + gW * (cPolar.Nodes(i).X - xmin) / dx
                            pnts(i).Y = mY + gH * (1 - cPolar.Nodes(i).Y / ymax)

                        Next

                        ' draw lines:

                        For i = 1 To cPolar.Nodes.Count - 1

                            g.DrawLine(CurvePen, pnts(i - 1), pnts(i))

                        Next

                        ' draw points:

                        SelectedPointIndex = -1
                        Dim pnt As Point

                        For i = 0 To cPolar.Nodes.Count - 1

                            pnt = pnts(i)

                            Dim dmx As Single = pnt.X - MousePoint.X
                            Dim dmy As Single = pnt.Y - MousePoint.Y

                            ' draw marker:

                            If dmx * dmx + dmy * dmy < 9 Then

                                SelectedPointIndex = i

                                g.FillEllipse(Brushes.Orange, pnt.X - 3, pnt.Y - 3, 6, 6)
                                g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                            Else

                                g.FillEllipse(Brushes.White, pnt.X - 3, pnt.Y - 3, 6, 6)
                                g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                            End If

                        Next

                        ' draw label on selected point:

                        If SelectedPointIndex >= 0 Then

                            pnt = pnts(SelectedPointIndex)

                            Dim lblPoint As String = String.Format("CL = {0:F3} / CD = {1:F4}", cPolar.Nodes(SelectedPointIndex).X, cPolar.Nodes(SelectedPointIndex).Y)

                            DrawLabel(g, lblPoint, pnt, FontLabel)

                        End If

                    End If

                End If

            ElseIf TypeOf Polar Is QuadraticPolar Then

                xmin = -1

                Dim nx As Integer = 10

                For i = 1 To nx - 1

                    Dim x As Single = mX + i / nx * gW
                    g.DrawLine(GridPen, x, 0, x, gH)

                Next

                g.DrawLine(AxisPen, mX, mY + gH, mX + gW, mY + gH)
                g.DrawLine(AxisPen, mX, mY, mX + gW, mY)

                Dim dx As Double = xmax - xmin

                For i = 0 To np

                    ymax = Math.Max(ymax, Polar.SkinDrag(xmin + dx * i / np))

                Next

                Dim pnts(np) As Point
                Dim vals(np) As PointF

                For i = 0 To np

                    vals(i).X = xmin + dx * i / np
                    vals(i).Y = Polar.SkinDrag(vals(i).X)

                    pnts(i).X = mX + i / np * gW
                    pnts(i).Y = mY + gH * (1 - vals(i).Y / ymax)

                    If i > 0 Then g.DrawLine(CurvePen, pnts(i - 1), pnts(i))

                Next

                ' draw points:

                SelectedPointIndex = -1
                Dim pnt As Point

                For i = 0 To np

                    pnt = pnts(i)

                    Dim dmx As Single = pnt.X - MousePoint.X
                    Dim dmy As Single = pnt.Y - MousePoint.Y

                    ' draw marker:

                    If dmx * dmx + dmy * dmy < 9 Then

                        SelectedPointIndex = i

                        g.FillEllipse(Brushes.Orange, pnt.X - 3, pnt.Y - 3, 6, 6)
                        g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                    Else

                        g.FillEllipse(Brushes.White, pnt.X - 3, pnt.Y - 3, 6, 6)
                        g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                    End If

                Next

                ' draw label on selected point:

                If SelectedPointIndex >= 0 Then

                    pnt = pnts(SelectedPointIndex)

                    Dim lblPoint As String = String.Format("CL = {0:F3} / CD = {1:F4}", vals(SelectedPointIndex).X, vals(SelectedPointIndex).Y)

                    DrawLabel(g, lblPoint, pnt, FontLabel)

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

    Private MousePoint As New PointF(Single.MinValue, Single.MinValue)

    Private Sub PolarPlotter_MouseDown(sender As Object, e As Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown

        DragPoint = True

        MousePoint.X = e.X
        MousePoint.Y = e.Y

        Refresh()

    End Sub

    Private Sub PolarPlotter_MouseMove(sender As Object, e As Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove

        MousePoint.X = e.X
        MousePoint.Y = e.Y

        Refresh()

    End Sub

    Private Sub PolarPlotter_MouseUp(sender As Object, e As Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp

        DragPoint = False

        RaiseEvent PointChanged()

    End Sub

    Public Event PointChanged()

End Class

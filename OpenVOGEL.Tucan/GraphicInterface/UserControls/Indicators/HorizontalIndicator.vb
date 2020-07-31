Imports GaugesTools.IndicationTools.Indicators
Imports System.Drawing
Imports MathTools.GeodesicCoordinates
Imports MathTools.Algebra.EuclideanSpace
Imports GaugesTools.GaugesTools.Maps
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.IndicationTools.Indicators

Public Class HorizontalIndicator

    Implements IIndicator

    Public Property HeadingAircraft As Single

    ''' <summary>
    ''' Zoom in Km
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Zoom As Single = 0.5

    Public Property MapRotation As Single = 0

    Private _Region As Rectangle

    Public Sub New()

    End Sub

    Public Property Region As Rectangle Implements IIndicator.Region
        Set(ByVal value As Rectangle)

            _Region = value

        End Set
        Get
            Return _Region
        End Get
    End Property

    Dim pen_1 As New Pen(Color.FromArgb(42, 42, 42), 2)
    Dim brush_1 As New SolidBrush(Color.FromArgb(42, 42, 42))
    Dim font_1 As New Font("Vrinda", 10)
    Dim pen_2 As New Pen(Brushes.LimeGreen, 2)
    Dim pen_3 As New Pen(Brushes.White, 2)
    Dim pen_4 As New Pen(Brushes.Blue, 2)
    Dim pen_5 As New Pen(Brushes.Gainsboro, 8)

    Public Sub DrawControl(ByRef g As Graphics) Implements IIndicator.DrawControl

        g.DrawRectangle(Pens.White, _Region)
        g.SetClip(_Region)
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.TranslateTransform(_Region.Left, _Region.Top)

        ' Draw circles:

        Dim center As New Point(0.5 * Region.Width, 0.5 * _Region.Height)

        Dim r_1 As Single = 0.45 * _Region.Width
        Dim c_1 As New Rectangle(center.X - r_1, center.Y - r_1, 2 * r_1, 2 * r_1)
        pen_1.DashStyle = Drawing2D.DashStyle.Dash
        pen_1.DashCap = Drawing2D.DashCap.Round
        pen_1.DashPattern = {6.0F, 4.0F}

        g.DrawEllipse(pen_1, c_1)

        If Zoom > 1 Then
            g.DrawString(String.Format("{0}km", Zoom), font_1, brush_1, center.X + r_1, center.Y)
        Else
            g.DrawString(String.Format("{0}m", Zoom * 1000), font_1, brush_1, center.X + r_1, center.Y)
        End If

        Dim r_2 As Single = 0.25 * _Region.Width
        Dim c_2 As New Rectangle(center.X - r_2, center.Y - r_2, 2 * r_2, 2 * r_2)
        g.DrawEllipse(pen_1, c_2)

        If 0.5 * Zoom > 1 Then
            g.DrawString(String.Format("{0}km", 0.5 * Zoom), font_1, brush_1, center.X + r_2, center.Y)
        Else
            g.DrawString(String.Format("{0}m", Zoom * 500), font_1, brush_1, center.X + r_2, center.Y)
        End If

        Dim c As New Rectangle(center.X - 5, center.Y - 5, 10, 10)
        g.FillEllipse(Brushes.Red, c)
        g.DrawEllipse(pen_3, c)

        ' Rotate the map

        g.TranslateTransform(center.X, center.Y)
        g.RotateTransform(MapRotation)

        Dim scale As Double = 1 / Zoom * r_1 / 1000

        ' Draw the aircraft:

    End Sub

End Class


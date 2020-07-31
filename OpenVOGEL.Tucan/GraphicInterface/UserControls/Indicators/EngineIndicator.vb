Imports System.Drawing
Imports GaugesTools.IndicationTools.GeometricTools
Imports System.Drawing.Drawing2D

Namespace IndicationTools.Indicators

    Public Class EngineIndicator

        Implements IIndicator

        Private _Region As Rectangle

        Public Property Region As Rectangle Implements IIndicator.Region
            Set(ByVal value As Rectangle)

                _Region = value
                r_gauge.Width = 0.7 * _Region.Width
                r_gauge.Height = 0.7 * _Region.Width
                r_gauge.X = 0.15 * _Region.Width
                r_gauge.Y = 0.15 * _Region.Width
                r_middle.Width = 0.5 * _Region.Width
                r_middle.Height = 0.5 * _Region.Width
                r_middle.X = 0.25 * _Region.Width
                r_middle.Y = 0.25 * _Region.Width
                r_frame.X = r_gauge.X
                r_frame.Y = r_gauge.Y
                r_frame.Width = 0.5 * r_gauge.Width
                r_frame.Height = 0.5 * r_gauge.Width

                throttleTimer.Interval = 6000

            End Set
            Get
                Return _Region
            End Get
        End Property

        ''' <summary>
        ''' Engine temperature
        ''' </summary>
        ''' <remarks></remarks>
        Public EngineTemperature As Single = 0

        Private _EngineN As Single = 0

        ''' <summary>
        ''' Engine rotational speed (RPM)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property EngineRPM As Single
            Get
                Return _EngineN
            End Get
            Set(ByVal value As Single)
                If value > 0 Then _EngineN = value
            End Set
        End Property

        ''' <summary>
        ''' Indicates whether the engine is running or not
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Running As Boolean
            Get
                Return EngineRPM > 100
            End Get
        End Property

        ''' <summary>
        ''' Vertical load factor.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LoadFactor As Single = 0

        ''' <summary>
        ''' Indicates the percentage of throttle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Throttle As Single
            Get
                Return EngineRPM / MaxRPM * 100
            End Get
        End Property

        ''' <summary>
        ''' Stablishes the maximum number of RPM
        ''' </summary>
        ''' <remarks></remarks>
        Public MaxRPM As Single = 14000

        ''' <summary>
        ''' Minimum RPM to keep running
        ''' </summary>
        ''' <remarks></remarks>
        Public MinRPM As Single = 1000

        ''' <summary>
        ''' Maximum engine temperature
        ''' </summary>
        ''' <remarks></remarks>
        Public MaxTemperature As Single = 80

        Private f1 As New Font("Segoe UI", 20)
        Private f2 As New Font("Segoe UI", 8)
        Private f3 As New Font("Segoe UI", 12)
        Private pen1 As New Pen(Brushes.White, 1)
        Private pen2 As New Pen(Brushes.Black, 8)
        Private pen3 As New Pen(Brushes.White, 5)
        Private pen4 As New Pen(Brushes.White, 2)
        Private brush1 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(55, 55, 55))
        Private brush2 As New HatchBrush(HatchStyle.BackwardDiagonal, Color.White, Color.Red)
        Private r_gauge As Rectangle
        Private r_middle As Rectangle
        Private r_frame As Rectangle

        Private Shared WithEvents throttleTimer As System.Windows.Forms.Timer = New System.Windows.Forms.Timer()
        Private Shared _warnThrottle As Boolean = False

        Public Sub DrawControl(ByRef g As Graphics) Implements IIndicator.DrawControl

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            g.FillRectangle(Brushes.Black, Region)

            If Running Then
                g.DrawString("ENGINE ON", f3, Brushes.Lime, r_frame.X, Region.Y)
            Else
                g.DrawString("ENGINE OFF", f3, Brushes.Red, r_frame.X, Region.Y)
            End If

            If Throttle < 90 Then
                If Throttle < 80 Then
                    g.FillPie(Brushes.Lime, r_gauge, 270, Throttle / 100 * 270)
                Else
                    g.FillPie(Brushes.Orange, r_gauge, 270, Throttle / 100 * 270)
                End If
                throttleTimer.Stop()
                _warnThrottle = False
            Else
                g.FillPie(Brushes.Red, r_gauge, 270, Throttle / 100 * 270)
                throttleTimer.Start()
            End If

            g.DrawPie(pen4, r_gauge, 270, 270)
            g.FillEllipse(Brushes.Black, r_middle)
            g.DrawPie(pen4, r_middle, 270, 270)
            g.FillRectangle(brush1, r_frame)
            g.DrawRectangle(pen4, r_frame)
            g.DrawString(String.Format("{0:F0}", Throttle), f1, Brushes.White, r_gauge)
            g.DrawString(String.Format("{0:F0} RPM", EngineRPM), f2, Brushes.White, r_gauge.X, r_gauge.Y + f1.Height)
            g.DrawString(String.Format("{0:F0}⁰C", EngineTemperature), f2, Brushes.White, r_gauge.X, r_gauge.Y + f1.Height + f2.Height)

            If EngineRPM < MinRPM Then
                g.DrawString("WARNING: LOW THROTTLE", f2, Brushes.Red, r_gauge.X, r_gauge.Bottom + f2.Height)
            End If

            If _warnThrottle Then
                g.DrawString("WARNING: HIGH THROTTLE", f2, Brushes.Red, r_gauge.X, r_gauge.Bottom + f2.Height)
            End If

            If EngineTemperature > MaxTemperature Then
                g.DrawString("WARNING: HIGH TEMPERATURE", f2, Brushes.Red, r_gauge.X, r_gauge.Bottom + 2 * f2.Height)
            End If

        End Sub

        Private Shared Sub TimerEventProcessor(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles throttleTimer.Tick
            throttleTimer.Stop()
            _warnThrottle = True
        End Sub

    End Class

End Namespace
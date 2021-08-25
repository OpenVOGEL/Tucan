Imports OpenVOGEL.IndicationTools.Indicators

Public Class FormAttitude

    Public ReadOnly Property AttitudeIndicator As AttitudeIndicator

    Public Sub New()

        InitializeComponent()

        DoubleBuffered = True

        _AttitudeIndicator = New AttitudeIndicator

        Dim Area As Rectangle
        Area.X = 0
        Area.Y = 0
        Area.Width = Width
        Area.Height = Height

        _AttitudeIndicator.Region = Area

    End Sub

    Private Sub FormAttitude_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint

        If DesignTools.DataStore.Results.ActiveFrame IsNot Nothing Then

            Dim Frame = DesignTools.DataStore.Results.ActiveFrame

            'NOTE: the angles are reversed because the verctors are reverted
            ' I points to the rear
            ' J points to the right wingtip
            '--------------------------------------------------------------------

            _AttitudeIndicator.Pitch = -Frame.Orientation.Angle2
            _AttitudeIndicator.Roll = -Frame.Orientation.Angle3
            _AttitudeIndicator.AirSpeed = Frame.StreamVelocity.Norm2
            _AttitudeIndicator.Slide = Frame.StreamVelocity.Y
            _AttitudeIndicator.Altitude = Frame.Position.Z

        End If

        If Visible Then

            _AttitudeIndicator.DrawControl(e.Graphics)

        End If

    End Sub

    Private Sub FormAttitude_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Hide()
        End If

    End Sub

End Class
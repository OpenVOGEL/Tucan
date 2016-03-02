'Copyright (C) 2016 Guillermo Hazebrouck

Public Class Conversion

    Public Shared Function DegToRad(ByVal value As Double) As Double

        Return value * Math.PI / 180

    End Function

    Public Shared Function RadToDeg(ByVal value As Double) As Double

        Return value / Math.PI * 180

    End Function

End Class

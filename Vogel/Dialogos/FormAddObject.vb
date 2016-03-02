'Copyright (C) 2016 Guillermo Hazebrouck

Public Class SelectObject

    Public ReadOnly Property WingSelected As Boolean
        Get
            Return rbLiftingSurface.Enabled And rbLiftingSurface.Checked
        End Get
    End Property

    Public ReadOnly Property FuselageSelected As Boolean
        Get
            Return rbFuselage.Enabled And rbFuselage.Checked
        End Get
    End Property

    Public ReadOnly Property JetEngineSelected As Boolean
        Get
            Return rbJetEngine.Enabled And rbJetEngine.Checked
        End Get
    End Property

End Class
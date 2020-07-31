Imports System.Drawing

Namespace IndicationTools.Indicators

    Public Interface IIndicator

        Property Region As Rectangle
        Sub DrawControl(ByRef g As Drawing.Graphics)

    End Interface

End Namespace

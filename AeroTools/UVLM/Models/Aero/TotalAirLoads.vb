Imports MathTools.Algebra.EuclideanSpace

Namespace UVLM.Models.Aero

    ''' <summary>
    ''' Gathers the resultants of aerodynamic forces per type.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TotalAirLoads

        Public Property Area As Double

        Public Property Force As New EVector3
        Public Property Moment As New EVector3

        Public Property CL As Double
        Public Property CDi As Double
        Public Property CDp As Double

        ' old:

        Public Property SlenderForce As New EVector3
        Public Property SlenderMoment As New EVector3

        Public Property InducedDrag As New EVector3
        Public Property InducedMoment As New EVector3

        Public Property SkinDrag As New EVector3
        Public Property SkinMoment As New EVector3

        Public Property BodyForce As New EVector3
        Public Property BodyMoment As New EVector3

    End Class

End Namespace
Imports MathTools.Algebra.EuclideanSpace

Namespace MathLibrary.ODEs

    Public Class TEstado

        Private Z1 As New EVector3
        Private Z2 As New EVector3

    End Class

    ''' <summary>
    ''' Provides methods to solve ODEs of the type M d²y/dt² + C dy/dt + K y = P(t) being M, C and K real constants.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TimeDomainSolver

        'Public Shared Sub Solve_I(ByVal M As Double, ByVal C As Double, ByVal K As Double, ByVal P As Double, ByVal Rm2 As TModalCoordinate, ByVal Rm1 As TModalCoordinate, ByRef Output As TModalCoordinate)

        '    Output.a = P / M
        '    Output.v = 1
        '    Output.p = 1

        'End Sub

    End Class

End Namespace

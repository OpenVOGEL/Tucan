'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

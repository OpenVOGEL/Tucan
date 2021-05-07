'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Namespace Integration

    Public Class NewmarkIntegrator

        Public A(2, 2) As Double
        Public L(2) As Double

        ''' <summary>
        ''' Initializes the integrator with the given coefficients.
        ''' This algorithm is descrived in [Finite element procedures, Bathe].
        ''' </summary>
        ''' <param name="a">Alpha</param>
        ''' <param name="d">Delta</param>
        ''' <param name="z">The damping ratio</param>
        ''' <param name="w">The natural frequency</param>
        ''' <param name="Dt">The fixed time step</param>
        Public Sub Load(a As Double, d As Double, z As Double, w As Double, Dt As Double)

            Dim b As Double = 1 / (1 / (w * w * Dt * Dt) + 2 * z * d / (w * Dt) + a)
            Dim k As Double = z * b / (w * Dt)

            Me.A(0, 0) = -(0.5 - a) * b - 2 * (1 - d) * k
            Me.A(0, 1) = -(b + 2 * k) / Dt
            Me.A(0, 2) = -b / (Dt * Dt)

            Me.A(1, 0) = Dt * (1 - d - (0.5 - a) * d * b - 2 * (1 - d) * d * k)
            Me.A(1, 1) = 1 - b * d - 2 * d * k
            Me.A(1, 2) = -b * d / Dt

            Me.A(2, 0) = Dt * Dt * (0.5 - a - (0.5 - a) * a * b - 2 * (1 - d) * a * k)
            Me.A(2, 1) = Dt * (1 - a * b - 2 * a * k)
            Me.A(2, 2) = 1 - a * b

            L(0) = b / (w * w * Dt * Dt)
            L(1) = b * d / (w * w * Dt)
            L(2) = a * b / (w * w)

        End Sub

        Public Sub Integrate(ByVal P As Double, ByVal A0 As Double, V0 As Double, P0 As Double, ByRef A1 As Double, ByRef V1 As Double, ByRef P1 As Double)

            A1 = A(0, 0) * A0 + A(0, 1) * V0 + A(0, 2) * P0 + L(0) * P
            V1 = A(1, 0) * A0 + A(1, 1) * V0 + A(1, 2) * P0 + L(1) * P
            P1 = A(2, 0) * A0 + A(2, 1) * V0 + A(2, 2) * P0 + L(2) * P

        End Sub

    End Class

End Namespace
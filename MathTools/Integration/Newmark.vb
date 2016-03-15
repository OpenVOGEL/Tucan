'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

        Public _A(2, 2) As Double
        Public _L(2) As Double

        Public Sub Load(a As Double, d As Double, z As Double, w As Double, dt As Double)

            Dim b As Double = 1 / (1 / (w * w * dt * dt) + 2 * z * d / (w * dt) + a)
            Dim k As Double = z * b / (w * dt)

            _A(0, 0) = -(0.5 - a) * b - 2 * (1 - d) * k
            _A(0, 1) = -(b + 2 * k) / dt
            _A(0, 2) = -b / (dt * dt)

            _A(1, 0) = dt * (1 - d - (0.5 - a) * d * b - 2 * (1 - d) * d * k)
            _A(1, 1) = 1 - b * d - 2 * d * k
            _A(1, 2) = -b * d / dt

            _A(2, 0) = dt * dt * (0.5 - a - (0.5 - a) * a * b - 2 * (1 - d) * a * k)
            _A(2, 1) = dt * (1 - a * b - 2 * a * k)
            _A(2, 2) = 1 - a * b

            _L(0) = b / (w * w * dt * dt)
            _L(1) = b * d / (w * w * dt)
            _L(2) = a * b / (w * w)

        End Sub

        Public Sub Integrate(ByVal p As Double, ByVal a0 As Double, v0 As Double, p0 As Double, ByRef a1 As Double, ByRef v1 As Double, ByRef p1 As Double)

            a1 = _A(0, 0) * a0 + _A(0, 1) * v0 + _A(0, 2) * p0 + _L(0) * p
            v1 = _A(1, 0) * a0 + _A(1, 1) * v0 + _A(1, 2) * p0 + _L(1) * p
            p1 = _A(2, 0) * a0 + _A(2, 1) * v0 + _A(2, 2) * p0 + _L(2) * p

        End Sub

    End Class

End Namespace
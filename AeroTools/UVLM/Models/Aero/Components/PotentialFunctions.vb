'## Open VOGEL ##
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Namespace UVLM.Models.Aero.Components

    Public Class PotentialFunctions

        ''' <summary>
        ''' Four times PI
        ''' </summary>
        ''' <remarks></remarks>
        Const FourPi As Double = 4 * Math.PI

        ''' <summary>
        ''' Minimum value of the Biot-Savart denominator
        ''' </summary>
        ''' <remarks></remarks>
        Const Epsilon As Double = 0.00000001

        ' Triangle functions:

        ''' <summary>
        ''' Returns the potential associated to a uniform unit distribution of sources.
        ''' </summary>
        ''' <param name="p">Evaluation point</param>
        ''' <param name="p0">Point 0</param>
        ''' <param name="p1">Point 1</param>
        ''' <param name="p2">Point 2</param>
        ''' <returns>The unit doublets distribution</returns>
        ''' <remarks></remarks>
        Public Shared Function GetTriangularUnitSourcePotential(ByVal p As EVector3, ByVal p0 As EVector3, ByVal p1 As EVector3, ByVal p2 As EVector3, Optional ByVal WithDiagonal As Boolean = True) As Double

            ' Directional versors:

            Dim ux, uy, uz As Double
            Dim vx, vy, vz As Double
            Dim wx, wy, wz As Double

            ux = p1.X - p0.X
            uy = p1.Y - p0.Y
            uz = p1.Z - p0.Z

            Dim d01 As Double = Math.Sqrt(ux * ux + uy * uy + uz * uz)

            ux /= d01
            uy /= d01
            uz /= d01

            Dim d02x = p2.X - p0.X
            Dim d02y = p2.Y - p0.Y
            Dim d02z = p2.Z - p0.Z

            wx = uy * d02z - uz * d02y
            wy = uz * d02x - ux * d02z
            wz = ux * d02y - uy * d02x

            Dim w As Double = Math.Sqrt(wx * wx + wy * wy + wz * wz)

            wx /= w
            wy /= w
            wz /= w

            vx = uz * wy - uy * wz
            vy = ux * wz - uz * wx
            vz = uy * wx - ux * wy

            ' distances:

            Dim r0px = p.X - p0.X
            Dim r0py = p.Y - p0.Y
            Dim r0pz = p.Z - p0.Z

            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

            Dim r1px = p.X - p1.X
            Dim r1py = p.Y - p1.Y
            Dim r1pz = p.Z - p1.Z

            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

            Dim r2px = p.X - p2.X
            Dim r2py = p.Y - p2.Y
            Dim r2pz = p.Z - p2.Z

            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

            ' projected vectors to point:

            Dim d0pu As Double = r0px * ux + r0py * uy + r0pz * uz
            Dim d0pv As Double = r0px * vx + r0py * vy + r0pz * vz

            Dim d1pu As Double = r1px * ux + r1py * uy + r1pz * uz
            Dim d1pv As Double = r1px * vx + r1py * vy + r1pz * vz

            Dim d2pu As Double = r2px * ux + r2py * uy + r2pz * uz
            Dim d2pv As Double = r2px * vx + r2py * vy + r2pz * vz

            ' all points have the same w coordinate since they lay in plane {(u, v), p}:

            Dim pw As Double = (p.X - p0.X) * wx + (p.Y - p0.Y) * wy + (p.Z - p0.Z) * wz

            pw = Math.Abs(pw)

            ' projected segments:

            Dim d01u As Double = d01  ' (p1.X - p0.X) * ux + (p1.Y - p0.Y) * uy + (p1.Z - p0.Z) * uz
            Dim d01v As Double = 0.0# ' (p1.X - p0.X) * vx + (p1.Y - p0.Y) * vy + (p1.Z - p0.Z) * vz

            Dim d12u As Double = (p2.X - p1.X) * ux + (p2.Y - p1.Y) * uy + (p2.Z - p1.Z) * uz
            Dim d12v As Double = (p2.X - p1.X) * vx + (p2.Y - p1.Y) * vy + (p2.Z - p1.Z) * vz

            Dim d20u As Double = (p0.X - p2.X) * ux + (p0.Y - p2.Y) * uy + (p0.Z - p2.Z) * uz
            Dim d20v As Double = (p0.X - p2.X) * vx + (p0.Y - p2.Y) * vy + (p0.Z - p2.Z) * vz

            ' segments length:

            Dim d12 As Double = Math.Sqrt(d12u * d12u + d12v * d12v)
            Dim d20 As Double = Math.Sqrt(d20u * d20u + d20v * d20v)

            ' logarithms:

            Dim ln01 As Double = (d0pu * d01v - d0pv * d01u) / d01 * Math.Log((r0p + r1p + d01) / (r0p + r1p - d01))
            Dim ln12 As Double = (d1pu * d12v - d1pv * d12u) / d12 * Math.Log((r1p + r2p + d12) / (r1p + r2p - d12))
            Dim ln20 As Double = 0.0

            If WithDiagonal Then ln20 = (d2pu * d20v - d2pv * d20u) / d20 * Math.Log((r2p + r0p + d20) / (r2p + r0p - d20))

            ' entities for evaluation of arctangents:

            Dim e0 As Double = d0pu * d0pu + pw * pw
            Dim e1 As Double = d1pu * d1pu + pw * pw
            Dim e2 As Double = d2pu * d2pu + pw * pw

            Dim h0 As Double = d0pu * d0pv
            Dim h1 As Double = d1pu * d1pv
            Dim h2 As Double = d2pu * d2pv

            Dim m01 As Double = d01v / d01u
            Dim m12 As Double = d12v / d12u
            Dim m20 As Double = d20v / d20u

            Dim tn01 As Double = Math.Atan((m01 * e0 - h0) / (pw * r0p)) - Math.Atan((m01 * e1 - h1) / (pw * r1p))
            Dim tn12 As Double = Math.Atan((m12 * e1 - h1) / (pw * r1p)) - Math.Atan((m12 * e2 - h2) / (pw * r2p))
            Dim tn20 As Double = 0.0

            If WithDiagonal Then tn20 = Math.Atan((m20 * e2 - h2) / (pw * r2p)) - Math.Atan((m20 * e0 - h0) / (pw * r0p))

            Return -(ln01 + ln12 + ln20 - pw * (tn01 + tn12 + tn20)) / FourPi

        End Function

        ''' <summary>
        ''' Returns the potential associated to a uniform unit distribution of doublets.
        ''' </summary>
        ''' <param name="p">Evaluation point</param>
        ''' <param name="p0">Point 0</param>
        ''' <param name="p1">Point 1</param>
        ''' <param name="p2">Point 2</param>
        ''' <returns>The unit doublets distribution</returns>
        ''' <remarks></remarks>
        Public Shared Function GetTriangularUnitDoubletPotential(ByVal p As EVector3, ByVal p0 As EVector3, ByVal p1 As EVector3, ByVal p2 As EVector3, _
                                                                 Optional ByVal WithDiagonal20 As Boolean = True, _
                                                                 Optional ByVal WithDiagonal12 As Boolean = True) As Double

            ' Directional versors:

            Dim ux, uy, uz As Double
            Dim vx, vy, vz As Double
            Dim wx, wy, wz As Double

            ux = p1.X - p0.X
            uy = p1.Y - p0.Y
            uz = p1.Z - p0.Z

            Dim d01 As Double = Math.Sqrt(ux * ux + uy * uy + uz * uz)

            ux /= d01
            uy /= d01
            uz /= d01

            Dim d02x = p2.X - p0.X
            Dim d02y = p2.Y - p0.Y
            Dim d02z = p2.Z - p0.Z

            wx = uy * d02z - uz * d02y
            wy = uz * d02x - ux * d02z
            wz = ux * d02y - uy * d02x

            Dim w As Double = Math.Sqrt(wx * wx + wy * wy + wz * wz)

            wx /= w
            wy /= w
            wz /= w

            vx = uz * wy - uy * wz
            vy = ux * wz - uz * wx
            vz = uy * wx - ux * wy

            ' distances:

            Dim r0px = p.X - p0.X
            Dim r0py = p.Y - p0.Y
            Dim r0pz = p.Z - p0.Z

            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

            Dim r1px = p.X - p1.X
            Dim r1py = p.Y - p1.Y
            Dim r1pz = p.Z - p1.Z

            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

            Dim r2px = p.X - p2.X
            Dim r2py = p.Y - p2.Y
            Dim r2pz = p.Z - p2.Z

            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

            ' projected vectors to point:

            Dim d0pu As Double = r0px * ux + r0py * uy + r0pz * uz
            Dim d0pv As Double = r0px * vx + r0py * vy + r0pz * vz

            Dim d1pu As Double = r1px * ux + r1py * uy + r1pz * uz
            Dim d1pv As Double = r1px * vx + r1py * vy + r1pz * vz

            Dim d2pu As Double = r2px * ux + r2py * uy + r2pz * uz
            Dim d2pv As Double = r2px * vx + r2py * vy + r2pz * vz

            ' all points have the same w coordinate since they lay in plane {(u, v), p}:

            Dim pw As Double = (p.X - p0.X) * wx + (p.Y - p0.Y) * wy + (p.Z - p0.Z) * wz
            'pw = (p.X - p1.X) * wx + (p.Y - p1.Y) * wy + (p.Z - p1.Z) * wz
            'pw = (p.X - p2.X) * wx + (p.Y - p2.Y) * wy + (p.Z - p2.Z) * wz

            Dim s As Double = Math.Sign(pw)

            pw = Math.Abs(pw)

            ' projected segments:

            Dim d01u As Double = d01  ' (p1.X - p0.X) * ux + (p1.Y - p0.Y) * uy + (p1.Z - p0.Z) * uz
            Dim d01v As Double = 0.0# ' (p1.X - p0.X) * vx + (p1.Y - p0.Y) * vy + (p1.Z - p0.Z) * vz

            Dim d12u As Double = (p2.X - p1.X) * ux + (p2.Y - p1.Y) * uy + (p2.Z - p1.Z) * uz
            Dim d12v As Double = (p2.X - p1.X) * vx + (p2.Y - p1.Y) * vy + (p2.Z - p1.Z) * vz

            Dim d20u As Double = (p0.X - p2.X) * ux + (p0.Y - p2.Y) * uy + (p0.Z - p2.Z) * uz
            Dim d20v As Double = (p0.X - p2.X) * vx + (p0.Y - p2.Y) * vy + (p0.Z - p2.Z) * vz

            Dim d12 As Double = Math.Sqrt(d12u * d12u + d12v * d12v)
            Dim d20 As Double = Math.Sqrt(d20u * d20u + d20v * d20v)

            ' entities for evaluation of arctangents:

            Dim e0 As Double = d0pu * d0pu + pw * pw
            Dim e1 As Double = d1pu * d1pu + pw * pw
            Dim e2 As Double = d2pu * d2pu + pw * pw

            Dim h0 As Double = d0pu * d0pv
            Dim h1 As Double = d1pu * d1pv
            Dim h2 As Double = d2pu * d2pv

            Dim m01 As Double = d01v / d01u
            Dim m12 As Double = d12v / d12u
            Dim m20 As Double = d20v / d20u

            Dim tn01 As Double = Math.Atan((m01 * e0 - h0) / (pw * r0p)) - Math.Atan((m01 * e1 - h1) / (pw * r1p))
            Dim tn12 As Double = 0.0
            Dim tn20 As Double = 0.0

            If WithDiagonal12 Then tn12 = Math.Atan((m12 * e1 - h1) / (pw * r1p)) - Math.Atan((m12 * e2 - h2) / (pw * r2p))
            If WithDiagonal20 Then tn20 = Math.Atan((m20 * e2 - h2) / (pw * r2p)) - Math.Atan((m20 * e0 - h0) / (pw * r0p))

            Return s * (tn01 + tn12 + tn20) / FourPi

        End Function

        ''' <summary>
        ''' Adds the velocity associated to a unifor distribution of sources.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <param name="p0"></param>
        ''' <param name="p1"></param>
        ''' <param name="p2"></param>
        ''' <param name="Velocity"></param>
        ''' <param name="factor"></param>
        ''' <remarks></remarks>
        Public Shared Sub AddTriangularSourceVelocity(ByVal p As EVector3, ByVal p0 As EVector3, ByVal p1 As EVector3, ByVal p2 As EVector3, ByRef Velocity As EVector3, ByVal factor As Double, Optional ByVal WithDiagonal As Boolean = True)

            ' Directional versors:

            Dim ux, uy, uz As Double
            Dim vx, vy, vz As Double
            Dim wx, wy, wz As Double

            ux = p1.X - p0.X
            uy = p1.Y - p0.Y
            uz = p1.Z - p0.Z

            Dim d01 As Double = Math.Sqrt(ux * ux + uy * uy + uz * uz)

            ux /= d01
            uy /= d01
            uz /= d01

            Dim d02x = p2.X - p0.X
            Dim d02y = p2.Y - p0.Y
            Dim d02z = p2.Z - p0.Z

            wx = uy * d02z - uz * d02y
            wy = uz * d02x - ux * d02z
            wz = ux * d02y - uy * d02x

            Dim w As Double = Math.Sqrt(wx * wx + wy * wy + wz * wz)

            wx /= w
            wy /= w
            wz /= w

            vx = uz * wy - uy * wz
            vy = ux * wz - uz * wx
            vz = uy * wx - ux * wy

            ' distances:

            Dim r0px = p.X - p0.X
            Dim r0py = p.Y - p0.Y
            Dim r0pz = p.Z - p0.Z

            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

            Dim r1px = p.X - p1.X
            Dim r1py = p.Y - p1.Y
            Dim r1pz = p.Z - p1.Z

            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

            Dim r2px = p.X - p2.X
            Dim r2py = p.Y - p2.Y
            Dim r2pz = p.Z - p2.Z

            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

            ' projected vectors to point:

            Dim d0pu As Double = r0px * ux + r0py * uy + r0pz * uz
            Dim d0pv As Double = r0px * vx + r0py * vy + r0pz * vz

            Dim d1pu As Double = r1px * ux + r1py * uy + r1pz * uz
            Dim d1pv As Double = r1px * vx + r1py * vy + r1pz * vz

            Dim d2pu As Double = r2px * ux + r2py * uy + r2pz * uz
            Dim d2pv As Double = r2px * vx + r2py * vy + r2pz * vz

            ' all points have the same w coordinate since they lay in plane {(u, v), p}:

            Dim pw As Double = (p.X - p0.X) * wx + (p.Y - p0.Y) * wy + (p.Z - p0.Z) * wz

            ' projected segments:

            Dim d01u As Double = d01  ' (p1.X - p0.X) * ux + (p1.Y - p0.Y) * uy + (p1.Z - p0.Z) * uz
            Dim d01v As Double = 0.0# ' (p1.X - p0.X) * vx + (p1.Y - p0.Y) * vy + (p1.Z - p0.Z) * vz

            Dim d12u As Double = (p2.X - p1.X) * ux + (p2.Y - p1.Y) * uy + (p2.Z - p1.Z) * uz
            Dim d12v As Double = (p2.X - p1.X) * vx + (p2.Y - p1.Y) * vy + (p2.Z - p1.Z) * vz

            Dim d20u As Double = (p0.X - p2.X) * ux + (p0.Y - p2.Y) * uy + (p0.Z - p2.Z) * uz
            Dim d20v As Double = (p0.X - p2.X) * vx + (p0.Y - p2.Y) * vy + (p0.Z - p2.Z) * vz

            ' segments length:

            Dim d12 As Double = Math.Sqrt(d12u * d12u + d12v * d12v)
            Dim d20 As Double = Math.Sqrt(d20u * d20u + d20v * d20v)

            ' u and v components:

            Dim ln01 As Double = Math.Log((r0p + r1p - d01) / (r0p + r1p + d01))
            Dim ln12 As Double = Math.Log((r1p + r2p - d12) / (r1p + r2p + d12))
            Dim ln20 As Double = 0.0

            If WithDiagonal Then ln20 = Math.Log((r2p + r0p - d20) / (r2p + r0p + d20))

            Dim Vu As Double = d01v / d01 * ln01 + d12v / d12 * ln12 + d20v / d20 * ln20

            Dim Vv As Double = -d01u / d01 * ln01 - d12u / d12 * ln12 - d20u / d20 * ln20

            ' entities for evaluation of arctangents:

            Dim e0 As Double = d0pu * d0pu + pw * pw
            Dim e1 As Double = d1pu * d1pu + pw * pw
            Dim e2 As Double = d2pu * d2pu + pw * pw

            Dim h0 As Double = d0pu * d0pv
            Dim h1 As Double = d1pu * d1pv
            Dim h2 As Double = d2pu * d2pv

            Dim m01 As Double = d01v / d01u
            Dim m12 As Double = d12v / d12u
            Dim m20 As Double = d20v / d20u

            Dim tn01 As Double = Math.Atan((m01 * e0 - h0) / (pw * r0p)) - Math.Atan((m01 * e1 - h1) / (pw * r1p))
            Dim tn12 As Double = Math.Atan((m12 * e1 - h1) / (pw * r1p)) - Math.Atan((m12 * e2 - h2) / (pw * r2p))
            Dim tn20 As Double = 0.0

            If WithDiagonal Then tn20 = Math.Atan((m20 * e2 - h2) / (pw * r2p)) - Math.Atan((m20 * e0 - h0) / (pw * r0p))

            Dim Vw As Double = tn01 + tn12 + tn20

            Vu *= -factor / FourPi
            Vv *= -factor / FourPi
            Vw *= -factor / FourPi

#If DEBUG Then
            Dim v_x As Double = ux * Vu + vx * Vv + wx * Vw
            Dim v_y As Double = uy * Vu + vy * Vv + wy * Vw
            Dim v_z As Double = uz * Vu + vz * Vv + wz * Vw

            Velocity.X += v_x
            Velocity.Y += v_y
            Velocity.Z += v_z
#Else
            Velocity.X += ux * Vu + vx * Vv + wx * Vw
            Velocity.Y += uy * Vu + vy * Vv + wy * Vw
            Velocity.Z += uz * Vu + vz * Vv + wz * Vw
#End If

        End Sub

        ' Quad functions:

        ''' <summary>
        ''' Returns the potential associated to a uniform unit distribution of sources.
        ''' </summary>
        ''' <param name="p">Evaluation point</param>
        ''' <param name="p0">Point 0</param>
        ''' <param name="p1">Point 1</param>
        ''' <param name="p2">Point 2</param>
        ''' <param name="p3">Point 3</param>
        ''' <param name="n">Normal</param>
        ''' <param name="reversed">Indicates if the normal has been reversed</param>
        ''' <returns>The unit doublets distribution</returns>
        ''' <remarks></remarks>
        Public Shared Function GetQuadUnitSourcePotential_MeanPlane(p As EVector3, p0 As EVector3, p1 As EVector3, p2 As EVector3, p3 As EVector3, n As EVector3, reversed As Boolean) As Double

            Dim cX As Double = 0.25 * (p0.X + p1.X + p2.X + p3.X)
            Dim cY As Double = 0.25 * (p0.Y + p1.Y + p2.Y + p3.Y)
            Dim cZ As Double = 0.25 * (p0.Z + p1.Z + p2.Z + p3.Z)

            ' Directional unit vectors:

            ' Vector u points from the center to node 0
            ' Vector w points in the direction of the normal
            ' Vector v is orthogonal to u and w

            Dim c0x, c0y, c0z As Double

            c0x = p0.X - cX
            c0y = p0.Y - cY
            c0z = p0.Z - cZ

            Dim ux, uy, uz As Double
            Dim vx, vy, vz As Double
            Dim wx, wy, wz As Double

            If reversed Then
                wx = -n.X
                wy = -n.Y
                wz = -n.Z
            Else
                wx = n.X
                wy = n.Y
                wz = n.Z
            End If

            vx = wy * c0z - wz * c0y
            vy = wz * c0x - wx * c0z
            vz = wx * c0y - wy * c0x

            Dim v As Double = Math.Sqrt(vx * vx + vy * vy + vz * vz)

            vx /= v
            vy /= v
            vz /= v

            ux = vy * wz - vz * wy
            uy = vz * wx - vx * wz
            uz = vx * wy - vy * wx

            ' distances:

            Dim r0px = p.X - p0.X
            Dim r0py = p.Y - p0.Y
            Dim r0pz = p.Z - p0.Z

            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

            Dim r1px = p.X - p1.X
            Dim r1py = p.Y - p1.Y
            Dim r1pz = p.Z - p1.Z

            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

            Dim r2px = p.X - p2.X
            Dim r2py = p.Y - p2.Y
            Dim r2pz = p.Z - p2.Z

            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

            Dim r3px = p.X - p3.X
            Dim r3py = p.Y - p3.Y
            Dim r3pz = p.Z - p3.Z

            Dim r3p As Double = Math.Sqrt(r3px * r3px + r3py * r3py + r3pz * r3pz)

            ' projected vectors to point:

            Dim d0pu As Double = r0px * ux + r0py * uy + r0pz * uz
            Dim d0pv As Double = r0px * vx + r0py * vy + r0pz * vz

            Dim d1pu As Double = r1px * ux + r1py * uy + r1pz * uz
            Dim d1pv As Double = r1px * vx + r1py * vy + r1pz * vz

            Dim d2pu As Double = r2px * ux + r2py * uy + r2pz * uz
            Dim d2pv As Double = r2px * vx + r2py * vy + r2pz * vz

            Dim d3pu As Double = r3px * ux + r3py * uy + r3pz * uz
            Dim d3pv As Double = r3px * vx + r3py * vy + r3pz * vz

            ' all points have the same w coordinate since they lay in plane {(u, v), p}:

            Dim pw As Double = (p.X - cX) * wx + (p.Y - cY) * wy + (p.Z - cZ) * wz

            pw = Math.Abs(pw)

            ' projected segments:

            Dim d01u As Double = (p1.X - p0.X) * ux + (p1.Y - p0.Y) * uy + (p1.Z - p0.Z) * uz
            Dim d01v As Double = (p1.X - p0.X) * vx + (p1.Y - p0.Y) * vy + (p1.Z - p0.Z) * vz

            Dim d12u As Double = (p2.X - p1.X) * ux + (p2.Y - p1.Y) * uy + (p2.Z - p1.Z) * uz
            Dim d12v As Double = (p2.X - p1.X) * vx + (p2.Y - p1.Y) * vy + (p2.Z - p1.Z) * vz

            Dim d23u As Double = (p3.X - p2.X) * ux + (p3.Y - p2.Y) * uy + (p3.Z - p2.Z) * uz
            Dim d23v As Double = (p3.X - p2.X) * vx + (p3.Y - p2.Y) * vy + (p3.Z - p2.Z) * vz

            Dim d30u As Double = (p0.X - p3.X) * ux + (p0.Y - p3.Y) * uy + (p0.Z - p3.Z) * uz
            Dim d30v As Double = (p0.X - p3.X) * vx + (p0.Y - p3.Y) * vy + (p0.Z - p3.Z) * vz

            ' segments length:

            Dim d01 As Double = Math.Sqrt(d01u * d01u + d01v * d01v)
            Dim d12 As Double = Math.Sqrt(d12u * d12u + d12v * d12v)
            Dim d23 As Double = Math.Sqrt(d23u * d23u + d23v * d23v)
            Dim d30 As Double = Math.Sqrt(d30u * d30u + d30v * d30v)

            ' logarithms:

            Dim ln01 As Double = (d0pu * d01v - d0pv * d01u) / d01 * Math.Log((r0p + r1p + d01) / (r0p + r1p - d01))
            Dim ln12 As Double = (d1pu * d12v - d1pv * d12u) / d12 * Math.Log((r1p + r2p + d12) / (r1p + r2p - d12))
            Dim ln23 As Double = (d2pu * d23v - d2pv * d23u) / d23 * Math.Log((r2p + r3p + d23) / (r2p + r3p - d23))
            Dim ln30 As Double = (d3pu * d30v - d3pv * d30u) / d30 * Math.Log((r3p + r0p + d30) / (r3p + r0p - d30))

            ' entities for evaluation of arctangents:

            Dim e0 As Double = d0pu * d0pu + pw * pw
            Dim e1 As Double = d1pu * d1pu + pw * pw
            Dim e2 As Double = d2pu * d2pu + pw * pw
            Dim e3 As Double = d3pu * d3pu + pw * pw

            Dim h0 As Double = d0pu * d0pv
            Dim h1 As Double = d1pu * d1pv
            Dim h2 As Double = d2pu * d2pv
            Dim h3 As Double = d3pu * d3pv

            Dim m01 As Double = d01v / d01u
            Dim m12 As Double = d12v / d12u
            Dim m23 As Double = d23v / d23u
            Dim m30 As Double = d30v / d30u

            Dim tn01 As Double = Math.Atan((m01 * e0 - h0) / (pw * r0p)) - Math.Atan((m01 * e1 - h1) / (pw * r1p))
            Dim tn12 As Double = Math.Atan((m12 * e1 - h1) / (pw * r1p)) - Math.Atan((m12 * e2 - h2) / (pw * r2p))
            Dim tn23 As Double = Math.Atan((m23 * e2 - h2) / (pw * r2p)) - Math.Atan((m23 * e3 - h3) / (pw * r3p))
            Dim tn30 As Double = Math.Atan((m30 * e3 - h3) / (pw * r3p)) - Math.Atan((m30 * e0 - h0) / (pw * r0p))

            Return -(ln01 + ln12 + ln23 + ln30 - pw * (tn01 + tn12 + tn23 + tn30)) / FourPi

        End Function

        ''' <summary>
        ''' Returns the potential associated to a uniform unit distribution of doublets.
        ''' </summary>
        ''' <param name="p">Evaluation point</param>
        ''' <param name="p0">Point 0</param>
        ''' <param name="p1">Point 1</param>
        ''' <param name="p2">Point 2</param>
        ''' <param name="p3">Point 3</param>
        ''' <param name="n">Normal</param>
        ''' <param name="reversed">Indicates if the normal has been reversed</param>
        ''' <returns>The unit doublets distribution</returns>
        ''' <remarks></remarks>
        Public Shared Function GetQuadUnitDoubletPotential_MeanPlane(p As EVector3, p0 As EVector3, p1 As EVector3, p2 As EVector3, p3 As EVector3, n As EVector3, reversed As Boolean) As Double

            Dim cX As Double = 0.25 * (p0.X + p1.X + p2.X + p3.X)
            Dim cY As Double = 0.25 * (p0.Y + p1.Y + p2.Y + p3.Y)
            Dim cZ As Double = 0.25 * (p0.Z + p1.Z + p2.Z + p3.Z)

            ' Directional versors:

            ' Vector u points from the center to node 0
            ' Vector w points in the direction of the normal
            ' Vector v is orthogonal to u and w

            Dim c0x, c0y, c0z As Double

            c0x = p0.X - cX
            c0y = p0.Y - cY
            c0z = p0.Z - cZ

            Dim ux, uy, uz As Double
            Dim vx, vy, vz As Double
            Dim wx, wy, wz As Double

            If reversed Then
                wx = -n.X
                wy = -n.Y
                wz = -n.Z
            Else
                wx = n.X
                wy = n.Y
                wz = n.Z
            End If

            vx = wy * c0z - wz * c0y
            vy = wz * c0x - wx * c0z
            vz = wx * c0y - wy * c0x

            Dim v As Double = Math.Sqrt(vx * vx + vy * vy + vz * vz)

            vx /= v
            vy /= v
            vz /= v

            wx = n.X
            wy = n.Y
            wz = n.Z

            ux = vy * wz - vz * wy
            uy = vz * wx - vx * wz
            uz = vx * wy - vy * wx

            ' distances:

            Dim r0px = p.X - p0.X
            Dim r0py = p.Y - p0.Y
            Dim r0pz = p.Z - p0.Z

            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

            Dim r1px = p.X - p1.X
            Dim r1py = p.Y - p1.Y
            Dim r1pz = p.Z - p1.Z

            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

            Dim r2px = p.X - p2.X
            Dim r2py = p.Y - p2.Y
            Dim r2pz = p.Z - p2.Z

            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

            Dim r3px = p.X - p3.X
            Dim r3py = p.Y - p3.Y
            Dim r3pz = p.Z - p3.Z

            Dim r3p As Double = Math.Sqrt(r3px * r3px + r3py * r3py + r3pz * r3pz)

            ' projected vectors to point:

            Dim d0pu As Double = r0px * ux + r0py * uy + r0pz * uz
            Dim d0pv As Double = r0px * vx + r0py * vy + r0pz * vz

            Dim d1pu As Double = r1px * ux + r1py * uy + r1pz * uz
            Dim d1pv As Double = r1px * vx + r1py * vy + r1pz * vz

            Dim d2pu As Double = r2px * ux + r2py * uy + r2pz * uz
            Dim d2pv As Double = r2px * vx + r2py * vy + r2pz * vz

            Dim d3pu As Double = r3px * ux + r3py * uy + r3pz * uz
            Dim d3pv As Double = r3px * vx + r3py * vy + r3pz * vz

            ' use center point as referece to compute the altitude:

            Dim pw As Double = (p.X - cX) * wx + (p.Y - cY) * wy + (p.Z - cZ) * wz

            Dim s As Double = Math.Sign(pw)

            pw = Math.Abs(pw)

            ' projected segments:

            Dim d01u As Double = (p1.X - p0.X) * ux + (p1.Y - p0.Y) * uy + (p1.Z - p0.Z) * uz
            Dim d01v As Double = (p1.X - p0.X) * vx + (p1.Y - p0.Y) * vy + (p1.Z - p0.Z) * vz

            Dim d12u As Double = (p2.X - p1.X) * ux + (p2.Y - p1.Y) * uy + (p2.Z - p1.Z) * uz
            Dim d12v As Double = (p2.X - p1.X) * vx + (p2.Y - p1.Y) * vy + (p2.Z - p1.Z) * vz

            Dim d23u As Double = (p3.X - p2.X) * ux + (p3.Y - p2.Y) * uy + (p3.Z - p2.Z) * uz
            Dim d23v As Double = (p3.X - p2.X) * vx + (p3.Y - p2.Y) * vy + (p3.Z - p2.Z) * vz

            Dim d30u As Double = (p0.X - p3.X) * ux + (p0.Y - p3.Y) * uy + (p0.Z - p3.Z) * uz
            Dim d30v As Double = (p0.X - p3.X) * vx + (p0.Y - p3.Y) * vy + (p0.Z - p3.Z) * vz

            ' entities for evaluation of arctangents:

            Dim e0 As Double = d0pu * d0pu + pw * pw
            Dim e1 As Double = d1pu * d1pu + pw * pw
            Dim e2 As Double = d2pu * d2pu + pw * pw
            Dim e3 As Double = d3pu * d3pu + pw * pw

            Dim h0 As Double = d0pu * d0pv
            Dim h1 As Double = d1pu * d1pv
            Dim h2 As Double = d2pu * d2pv
            Dim h3 As Double = d3pu * d3pv

            Dim m01 As Double = d01v / d01u
            Dim m12 As Double = d12v / d12u
            Dim m23 As Double = d23v / d23u
            Dim m30 As Double = d30v / d30u

            Dim tn01 As Double = Math.Atan((m01 * e0 - h0) / (pw * r0p)) - Math.Atan((m01 * e1 - h1) / (pw * r1p))
            Dim tn12 As Double = Math.Atan((m12 * e1 - h1) / (pw * r1p)) - Math.Atan((m12 * e2 - h2) / (pw * r2p))
            Dim tn23 As Double = Math.Atan((m23 * e2 - h2) / (pw * r2p)) - Math.Atan((m23 * e3 - h3) / (pw * r3p))
            Dim tn30 As Double = Math.Atan((m30 * e3 - h3) / (pw * r3p)) - Math.Atan((m30 * e0 - h0) / (pw * r0p))

            If reversed Then s *= -1

            Return s * (tn01 + tn12 + tn23 + tn30) / FourPi

        End Function

        ''' <summary>
        ''' Adds the velocity associated to a unifor distribution of sources.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <param name="p0"></param>
        ''' <param name="p1"></param>
        ''' <param name="p2"></param>
        ''' <param name="p3"></param>
        ''' <param name="n"></param>
        ''' <param name="Velocity"></param>
        ''' <param name="factor"></param>
        ''' <param name="reversed"></param>
        ''' <remarks></remarks>
        Public Shared Sub AddQuadSourceVelocity_MeanPlane(p As EVector3, p0 As EVector3, p1 As EVector3, p2 As EVector3, p3 As EVector3, n As EVector3, ByRef Velocity As EVector3, factor As Double, reversed As Boolean)

            Dim cX As Double = 0.25 * (p0.X + p1.X + p2.X + p3.X)
            Dim cY As Double = 0.25 * (p0.Y + p1.Y + p2.Y + p3.Y)
            Dim cZ As Double = 0.25 * (p0.Z + p1.Z + p2.Z + p3.Z)

            ' Calculate distance:

            'Dim dx As Double = (p.X - cX)
            'Dim dy As Double = (p.Y - cY)
            'Dim dz As Double = (p.Z - cZ)

            'Dim d_sqr = dx * dx + dy * dy + dz * dz

            ' Directional versors:

            ' Vector u points from the center to node 0
            ' Vector w points in the direction of the normal
            ' Vector v is orthogonal to u and w

            Dim c0x, c0y, c0z As Double

            c0x = p0.X - cX
            c0y = p0.Y - cY
            c0z = p0.Z - cZ

            Dim ux, uy, uz As Double
            Dim vx, vy, vz As Double
            Dim wx, wy, wz As Double

            If reversed Then
                wx = -n.X
                wy = -n.Y
                wz = -n.Z
            Else
                wx = n.X
                wy = n.Y
                wz = n.Z
            End If

            vx = wy * c0z - wz * c0y
            vy = wz * c0x - wx * c0z
            vz = wx * c0y - wy * c0x

            Dim v As Double = Math.Sqrt(vx * vx + vy * vy + vz * vz)

            vx /= v
            vy /= v
            vz /= v

            wx = n.X
            wy = n.Y
            wz = n.Z

            ux = vy * wz - vz * wy
            uy = vz * wx - vx * wz
            uz = vx * wy - vy * wx

            ' distances:

            Dim r0px = p.X - p0.X
            Dim r0py = p.Y - p0.Y
            Dim r0pz = p.Z - p0.Z

            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

            Dim r1px = p.X - p1.X
            Dim r1py = p.Y - p1.Y
            Dim r1pz = p.Z - p1.Z

            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

            Dim r2px = p.X - p2.X
            Dim r2py = p.Y - p2.Y
            Dim r2pz = p.Z - p2.Z

            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

            Dim r3px = p.X - p3.X
            Dim r3py = p.Y - p3.Y
            Dim r3pz = p.Z - p3.Z

            Dim r3p As Double = Math.Sqrt(r3px * r3px + r3py * r3py + r3pz * r3pz)

            ' projected vectors to point:

            Dim d0pu As Double = r0px * ux + r0py * uy + r0pz * uz
            Dim d0pv As Double = r0px * vx + r0py * vy + r0pz * vz

            Dim d1pu As Double = r1px * ux + r1py * uy + r1pz * uz
            Dim d1pv As Double = r1px * vx + r1py * vy + r1pz * vz

            Dim d2pu As Double = r2px * ux + r2py * uy + r2pz * uz
            Dim d2pv As Double = r2px * vx + r2py * vy + r2pz * vz

            Dim d3pu As Double = r3px * ux + r3py * uy + r3pz * uz
            Dim d3pv As Double = r3px * vx + r3py * vy + r3pz * vz

            ' all points have the same w coordinate since they lay in plane {(u, v), p}:

            Dim pw As Double = (p.X - cX) * wx + (p.Y - cY) * wy + (p.Z - cZ) * wz

            ' projected segments:

            Dim d01u As Double = (p1.X - p0.X) * ux + (p1.Y - p0.Y) * uy + (p1.Z - p0.Z) * uz
            Dim d01v As Double = (p1.X - p0.X) * vx + (p1.Y - p0.Y) * vy + (p1.Z - p0.Z) * vz

            Dim d12u As Double = (p2.X - p1.X) * ux + (p2.Y - p1.Y) * uy + (p2.Z - p1.Z) * uz
            Dim d12v As Double = (p2.X - p1.X) * vx + (p2.Y - p1.Y) * vy + (p2.Z - p1.Z) * vz

            Dim d23u As Double = (p3.X - p2.X) * ux + (p3.Y - p2.Y) * uy + (p3.Z - p2.Z) * uz
            Dim d23v As Double = (p3.X - p2.X) * vx + (p3.Y - p2.Y) * vy + (p3.Z - p2.Z) * vz

            Dim d30u As Double = (p0.X - p3.X) * ux + (p0.Y - p3.Y) * uy + (p0.Z - p3.Z) * uz
            Dim d30v As Double = (p0.X - p3.X) * vx + (p0.Y - p3.Y) * vy + (p0.Z - p3.Z) * vz

            ' segments length:

            Dim d01 As Double = Math.Sqrt(d01u * d01u + d01v * d01v)
            Dim d12 As Double = Math.Sqrt(d12u * d12u + d12v * d12v)
            Dim d23 As Double = Math.Sqrt(d23u * d23u + d23v * d23v)
            Dim d30 As Double = Math.Sqrt(d30u * d30u + d30v * d30v)

            ' logarithms:

            Dim ln01 As Double = Math.Log((r0p + r1p - d01) / (r0p + r1p + d01))
            Dim ln12 As Double = Math.Log((r1p + r2p - d12) / (r1p + r2p + d12))
            Dim ln23 As Double = Math.Log((r2p + r3p - d23) / (r2p + r3p + d23))
            Dim ln30 As Double = Math.Log((r3p + r0p - d30) / (r3p + r0p + d30))

            ' planar velocity componets:

            Dim Vu As Double = d01v / d01 * ln01 + d12v / d12 * ln12 + d23v / d23 * ln23 + d30v / d30 * ln30

            Dim Vv As Double = -d01u / d01 * ln01 - d12u / d12 * ln12 - d23u / d23 * ln23 - d30u / d30 * ln30

            ' entities for evaluation of arctangents:

            Dim e0 As Double = d0pu * d0pu + pw * pw
            Dim e1 As Double = d1pu * d1pu + pw * pw
            Dim e2 As Double = d2pu * d2pu + pw * pw
            Dim e3 As Double = d3pu * d3pu + pw * pw

            Dim h0 As Double = d0pu * d0pv
            Dim h1 As Double = d1pu * d1pv
            Dim h2 As Double = d2pu * d2pv
            Dim h3 As Double = d3pu * d3pv

            Dim m01 As Double = d01v / d01u
            Dim m12 As Double = d12v / d12u
            Dim m23 As Double = d23v / d23u
            Dim m30 As Double = d30v / d30u

            Dim tn01 As Double = Math.Atan((m01 * e0 - h0) / (pw * r0p)) - Math.Atan((m01 * e1 - h1) / (pw * r1p))
            Dim tn12 As Double = Math.Atan((m12 * e1 - h1) / (pw * r1p)) - Math.Atan((m12 * e2 - h2) / (pw * r2p))
            Dim tn23 As Double = Math.Atan((m23 * e2 - h2) / (pw * r2p)) - Math.Atan((m23 * e3 - h3) / (pw * r3p))
            Dim tn30 As Double = Math.Atan((m30 * e3 - h3) / (pw * r3p)) - Math.Atan((m30 * e0 - h0) / (pw * r0p))

            Dim Vw As Double = tn01 + tn12 + tn23 + tn30

            If reversed Then
                Vu *= factor / FourPi
                Vv *= factor / FourPi
                Vw *= factor / FourPi
            Else
                Vu *= -factor / FourPi
                Vv *= -factor / FourPi
                Vw *= -factor / FourPi
            End If

#If DEBUG Then
            Dim v_x As Double = ux * Vu + vx * Vv + wx * Vw
            Dim v_y As Double = uy * Vu + vy * Vv + wy * Vw
            Dim v_z As Double = uz * Vu + vz * Vv + wz * Vw

            Velocity.X += v_x
            Velocity.Y += v_y
            Velocity.Z += v_z
#Else
            Velocity.X += ux * Vu + vx * Vv + wx * Vw
            Velocity.Y += uy * Vu + vy * Vv + wy * Vw
            Velocity.Z += uz * Vu + vz * Vv + wz * Vw
#End If

        End Sub

    End Class

End Namespace
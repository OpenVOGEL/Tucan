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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.MathTools.Algebra.CustomMatrices
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Public Module Aero

    ''' <summary>
    ''' Performs some tests to check the consistency of the solver
    ''' </summary>
    Public Sub TestAerodynamicSolver()

        TestSphere()

    End Sub

    Private Sub TestSphere()

        Dim Core As New AeroTools.CalculationModel.Solver.Solver

        Core.ReadFromXML("C:\Users\Guillermo\Documents\Vogel\Examples\Apame\Sphere_Steady\Steady.res")

        Dim Panel As VortexRing
        Dim Point As Vector3

        Panel = Core.Lattices(0).VortexRings(0)
        Point = Core.Lattices(0).VortexRings(1).ControlPoint

        Dim P1o As Double = DoubletPotential(Point,
                                             Panel.Node(1).Position,
                                             Panel.Node(2).Position,
                                             Panel.Node(3).Position,
                                             Panel.Node(4).Position,
                                             False)

        Dim P1n As Double = Panel.GetDoubletPotentialInfluence(Point, False)

        Panel = Core.Lattices(0).VortexRings(1)
        Point = Core.Lattices(0).VortexRings(0).ControlPoint

        Dim P2o As Double = DoubletPotential(Point,
                                             Panel.Node(1).Position,
                                             Panel.Node(2).Position,
                                             Panel.Node(3).Position,
                                             Panel.Node(4).Position,
                                             False)

        Dim P2n As Double = Panel.GetDoubletPotentialInfluence(Point, False)

        Console.WriteLine("Case 1 new = {0:F14}", (P1n))
        Console.WriteLine("Case 1 old = {0:F14}", (P1o))
        Console.WriteLine("Case 2 new = {0:F14}", (P2n))
        Console.WriteLine("Case 2 old = {0:F14}", (P2o))

        Core.WithSources = True
        Core.SteadyState("C:\Users\Guillermo\Documents\Vogel\Examples\Apame\Sphere_Steady\Test\")

    End Sub

    Private Sub TestPanelOrientation()

        Console.WriteLine("Testing influence of panel orientation")

        Dim N1 As New Node
        Dim N2 As New Node
        Dim N3 As New Node
        Dim N4 As New Node
        Dim P As New Vector3

        Dim M As New RotationMatrix
        Dim A As New EulerAngles
        Dim F As Double = Math.PI / 180.0

        Dim SourcePotentials1(180) As Double
        Dim DoubletPotentials1(180) As Double

        Dim SourcePotentials2(180) As Double
        Dim DoubletPotentials2(180) As Double

        N1.Position.X = 0.0
        N1.Position.Y = 0.0
        N1.Position.Z = 0.0

        N2.Position.X = 1.0
        N2.Position.Y = 0.0
        N2.Position.Z = 0.0

        N3.Position.X = 1.0
        N3.Position.Y = 1.0
        N3.Position.Z = 0.0

        N4.Position.X = 0.0
        N4.Position.Y = 1.0
        N4.Position.Z = 0.0

        P.X = 0.5
        P.Y = 0.5
        P.Z = 1.0

        A.Psi = Math.PI / 5
        M.Generate(A)
        N1.Position.Rotate(M)
        N2.Position.Rotate(M)
        N3.Position.Rotate(M)
        N4.Position.Rotate(M)
        P.Rotate(M)

        Console.WriteLine("Doublet potentials")

        Dim Panel1 As New VortexRing4(N1, N2, N3, N4, 0, False, False)

        Dim P1n As Double = Panel1.GetDoubletPotentialInfluence(P, False)
        'Dim P1o As Double = PotentialFunctions.GetQuadUnitDoubletPotential_MeanPlane(P,
        '                                                                             N1.Position,
        '                                                                             N2.Position,
        '                                                                             N3.Position,
        '                                                                             N4.Position,
        '                                                                             Panel1.Normal,
        '                                                                             False)

        Console.WriteLine("Case 1 new = {0:F14}", (P1n))
        'Console.WriteLine("Case 1 old = {0:F14}", (P1o))

        Dim Panel2 As New VortexRing4(N2, N3, N4, N1, 0, False, False)

        Dim P2n As Double = Panel2.GetDoubletPotentialInfluence(P, False)

        Console.WriteLine("Case 2 = {0:F14}", (P2n))

        Console.WriteLine("Source potentials")

        Dim S1n As Double = Panel1.GetSourcePotentialInfluence(P, False)
        'Dim S1o As Double = PotentialFunctions.GetQuadUnitSourcePotential_MeanPlane(P,
        '                                                                             N1.Position,
        '                                                                             N2.Position,
        '                                                                             N3.Position,
        '                                                                             N4.Position,
        '                                                                             Panel1.Normal,
        '                                                                             False)

        Console.WriteLine("Source new = {0:F14}", (S1n))
        'Console.WriteLine("Source old = {0:F14}", (S1o))

    End Sub

    Public Function GetDoubletPotential(p As Vector3, p0 As Vector2, p1 As Vector2, p2 As Vector2, p3 As Vector2, reversed As Boolean) As Double

        ' distances:

        Dim r0px = p.X - p0.X
        Dim r0py = p.Y - p0.Y
        Dim r0pz = p.Z

        Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

        Dim r1px = p.X - p1.X
        Dim r1py = p.Y - p1.Y
        Dim r1pz = p.Z

        Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

        Dim r2px = p.X - p2.X
        Dim r2py = p.Y - p2.Y
        Dim r2pz = p.Z

        Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

        Dim r3px = p.X - p3.X
        Dim r3py = p.Y - p3.Y
        Dim r3pz = p.Z

        Dim r3p As Double = Math.Sqrt(r3px * r3px + r3py * r3py + r3pz * r3pz)

        ' use center point as referece to compute the altitude:

        Dim s As Double = Math.Sign(p.Z)
        Dim z As Double = Math.Abs(z)

        ' projected segments:

        Dim d01x As Double = p1.X - p0.X
        Dim d01y As Double = p1.Y - p0.Y

        Dim d12x As Double = p2.X - p1.X
        Dim d12y As Double = p2.Y - p1.Y

        Dim d23x As Double = p3.X - p2.X
        Dim d23y As Double = p3.Y - p2.Y

        Dim d30x As Double = p0.X - p3.X
        Dim d30y As Double = p0.X - p3.X

        ' entities for evaluation of arctangents:

        Dim z2 As Double = z * z

        Dim e0 As Double = r0px * r0px + z2
        Dim e1 As Double = r1px * r1px + z2
        Dim e2 As Double = r2px * r2px + z2
        Dim e3 As Double = r3px * r3px + z2

        Dim h0 As Double = r0px * r0py
        Dim h1 As Double = r1px * r1py
        Dim h2 As Double = r2px * r2py
        Dim h3 As Double = r3px * r3py

        ' These are the Katz-Plotkin fotran formulas, which are based in only one Atan2 instead of two

        Dim f0 As Double = d01y * e0 - d01x * h0
        Dim g0 As Double = d01y * e1 - d01x * h1
        Dim tn01 As Double = Math.Atan2(z * d01x * (f0 * r1p - g0 * r0p), z2 * d01x * d01x * r0p * r1p + f0 * g0)

        Dim f1 As Double = d12y * e1 - d12x * h1
        Dim g1 As Double = d12y * e2 - d12x * h2
        Dim tn12 As Double = Math.Atan2(z * d12x * (f1 * r2p - g1 * r1p), z2 * d12x * d12x * r1p * r2p + f1 * g1)

        Dim f2 As Double = d23y * e2 - d23x * h2
        Dim g2 As Double = d23y * e3 - d23x * h3
        Dim tn23 As Double = Math.Atan2(z * d23x * (f2 * r3p - g2 * r2p), z2 * d23x * d23x * r2p * r3p + f2 * g2)

        Dim f3 As Double = d30y * e3 - d30x * h3
        Dim g3 As Double = d30y * e0 - d30x * h0
        Dim tn30 As Double = Math.Atan2(z * d30x * (f3 * r0p - g3 * r3p), z2 * d30x * d30x * r3p * r0p + f3 * g3)

        If reversed Then s *= -1

        Return s * (tn01 + tn12 + tn23 + tn30) / (4.0# * Math.PI)

    End Function

    Public Function DoubletPotential(p As Vector3, p0 As Vector3, p1 As Vector3, p2 As Vector3, p3 As Vector3, reversed As Boolean) As Double

        Dim cX As Double = 0.25 * (p0.X + p1.X + p2.X + p3.X)
        Dim cY As Double = 0.25 * (p0.Y + p1.Y + p2.Y + p3.Y)
        Dim cZ As Double = 0.25 * (p0.Z + p1.Z + p2.Z + p3.Z)

        ' Directional versors:

        ' Vector u points from node 0 to node 2
        ' Vector w points in the direction that is normal to the two diagonals
        ' Vector v is orthogonal to u and w

        Dim ux, uy, uz As Double
        Dim vx, vy, vz As Double
        Dim wx, wy, wz As Double

        ux = p2.X - p0.X
        uy = p2.Y - p0.Y
        uz = p2.Z - p0.Z
        Dim u As Double = Math.Sqrt(ux * ux + uy * uy + uz * uz)
        ux /= u
        uy /= u
        uz /= u

        vx = p3.X - p1.X
        vy = p3.Y - p1.Y
        vz = p3.Z - p1.Z

        wx = uy * vz - uz * vy
        wy = uz * vx - ux * vz
        wz = ux * vy - uy * vx

        If reversed Then
            wx = -wx
            wy = -wy
            wz = -wz
        End If

        Dim w As Double = Math.Sqrt(wx * wx + wy * wy + wz * wz)
        wx /= w
        wy /= w
        wz /= w

        vx = wy * uz - wz * uy
        vy = wz * ux - wx * uz
        vz = wx * uy - wy * ux

        Dim v As Double = Math.Sqrt(vx * vx + vy * vy + vz * vz)

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
        Dim d0 As Double = Math.Sqrt(d0pu * d0pu + d0pv * d0pv)

        Dim d1pu As Double = r1px * ux + r1py * uy + r1pz * uz
        Dim d1pv As Double = r1px * vx + r1py * vy + r1pz * vz
        Dim d1 As Double = Math.Sqrt(d1pu * d1pu + d1pv * d1pv)

        Dim d2pu As Double = r2px * ux + r2py * uy + r2pz * uz
        Dim d2pv As Double = r2px * vx + r2py * vy + r2pz * vz
        Dim d2 As Double = Math.Sqrt(d2pu * d2pu + d2pv * d2pv)

        Dim d3pu As Double = r3px * ux + r3py * uy + r3pz * uz
        Dim d3pv As Double = r3px * vx + r3py * vy + r3pz * vz
        Dim d3 As Double = Math.Sqrt(d3pu * d3pu + d3pv * d3pv)

        ' use center point as referece to compute the altitude:

        Dim pw As Double = (p.X - cX) * wx + (p.Y - cY) * wy + (p.Z - cZ) * wz

        Dim s As Double = Math.Sign(pw)

        pw = Math.Abs(pw)

        ' projected segments:

        Dim d01u As Double = (p1.X - p0.X) * ux + (p1.Y - p0.Y) * uy + (p1.Z - p0.Z) * uz
        Dim d01v As Double = (p1.X - p0.X) * vx + (p1.Y - p0.Y) * vy + (p1.Z - p0.Z) * vz
        Dim d01 As Double = Math.Sqrt(d01u * d01u + d01v * d01v)

        Dim d12u As Double = (p2.X - p1.X) * ux + (p2.Y - p1.Y) * uy + (p2.Z - p1.Z) * uz
        Dim d12v As Double = (p2.X - p1.X) * vx + (p2.Y - p1.Y) * vy + (p2.Z - p1.Z) * vz
        Dim d12 As Double = Math.Sqrt(d12u * d12u + d12v * d12v)

        Dim d23u As Double = (p3.X - p2.X) * ux + (p3.Y - p2.Y) * uy + (p3.Z - p2.Z) * uz
        Dim d23v As Double = (p3.X - p2.X) * vx + (p3.Y - p2.Y) * vy + (p3.Z - p2.Z) * vz
        Dim d23 As Double = Math.Sqrt(d23u * d23u + d23v * d23v)

        Dim d30u As Double = (p0.X - p3.X) * ux + (p0.Y - p3.Y) * uy + (p0.Z - p3.Z) * uz
        Dim d30v As Double = (p0.X - p3.X) * vx + (p0.Y - p3.Y) * vy + (p0.Z - p3.Z) * vz
        Dim d30 As Double = Math.Sqrt(d30u * d30u + d30v * d30v)

        ' entities for evaluation of arctangents:

        Dim e0 As Double = d0pu * d0pu + pw * pw
        Dim e1 As Double = d1pu * d1pu + pw * pw
        Dim e2 As Double = d2pu * d2pu + pw * pw
        Dim e3 As Double = d3pu * d3pu + pw * pw

        Dim h0 As Double = d0pu * d0pv
        Dim h1 As Double = d1pu * d1pv
        Dim h2 As Double = d2pu * d2pv
        Dim h3 As Double = d3pu * d3pv

        ' This are the Katz-Plotkin fotran formulas

        Dim pw2 As Double = pw * pw

        Dim f0 As Double = d01v * e0 - d01u * h0
        Dim g0 As Double = d01v * e1 - d01u * h1
        Dim tn01 As Double = Math.Atan2(pw * d01u * (f0 * r1p - g0 * r0p), pw2 * d01u * d01u * r0p * r1p + f0 * g0)

        Dim f1 As Double = d12v * e1 - d12u * h1
        Dim g1 As Double = d12v * e2 - d12u * h2
        Dim tn12 As Double = Math.Atan2(pw * d12u * (f1 * r2p - g1 * r1p), pw2 * d12u * d12u * r1p * r2p + f1 * g1)

        Dim f2 As Double = d23v * e2 - d23u * h2
        Dim g2 As Double = d23v * e3 - d23u * h3
        Dim tn23 As Double = Math.Atan2(pw * d23u * (f2 * r3p - g2 * r2p), pw2 * d23u * d23u * r2p * r3p + f2 * g2)

        Dim f3 As Double = d30v * e3 - d30u * h3
        Dim g3 As Double = d30v * e0 - d30u * h0
        Dim tn30 As Double = Math.Atan2(pw * d30u * (f3 * r0p - g3 * r3p), pw2 * d30u * d30u * r3p * r0p + f3 * g3)

        ' This are the Katz-Plotkin printed formulas

        'Dim m01 As Double = d01v / d01u
        'Dim m12 As Double = d12v / d12u
        'Dim m23 As Double = d23v / d23u
        'Dim m30 As Double = d30v / d30u

        'Dim tn01 As Double = Math.Atan((m01 * e0 - h0) / (pw * r0p)) - Math.Atan((m01 * e1 - h1) / (pw * r1p))
        'Dim tn12 As Double = Math.Atan((m12 * e1 - h1) / (pw * r1p)) - Math.Atan((m12 * e2 - h2) / (pw * r2p))
        'Dim tn23 As Double = Math.Atan((m23 * e2 - h2) / (pw * r2p)) - Math.Atan((m23 * e3 - h3) / (pw * r3p))
        'Dim tn30 As Double = Math.Atan((m30 * e3 - h3) / (pw * r3p)) - Math.Atan((m30 * e0 - h0) / (pw * r0p))

        If reversed Then s *= -1

        Return s * (tn01 + tn12 + tn23 + tn30) / (4.0# * Math.PI)

    End Function

End Module


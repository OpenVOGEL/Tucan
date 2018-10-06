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

'Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
'Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
'Imports Cudafy
'Imports Cudafy.Translator
'Imports Cudafy.Host

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components

Namespace CalculationModel.Solver

    Partial Public Class Solver

        Private GpuVortexSolver As GpuTools.VortexSolver

        Private Sub TestOpenCL()

            If Settings.UseGpu Then

                RaiseEvent PushMessage("Testing GPU double precision capability...")

                If GpuTools.GpuCore.TestGpuDoublePrecision(Settings.GpuDeviceId) Then

                    RaiseEvent PushMessage("Double precision enabled")

                Else

                    RaiseEvent PushMessage("Double precision disabled")

                    Settings.UseGpu = False

                End If

            Else

                RaiseEvent PushMessage("Hardware acceleration disabled")

            End If

        End Sub

        Private Sub CalculateVelocityInducedByTheWakesOnBoundedLatticesWithOpenCL(ByVal SlenderRingsOnly As Boolean)

            ' Count number of vortices in the wakes

            Dim nVortices As Integer = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Wake As Wake In Lattice.Wakes

                    nVortices += Wake.Vortices.Count

                Next

            Next

            ' Generate array

            Dim G(nVortices - 1) As Double
            Dim Ax(nVortices - 1) As Double
            Dim Ay(nVortices - 1) As Double
            Dim Az(nVortices - 1) As Double
            Dim Bx(nVortices - 1) As Double
            Dim By(nVortices - 1) As Double
            Dim Bz(nVortices - 1) As Double

            ' Make a list of vortices

            Dim i As Integer = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Wake As Wake In Lattice.Wakes

                    For Each Vortex As Vortex In Wake.Vortices

                        Ax(i) = Vortex.Node1.Position.X
                        Ay(i) = Vortex.Node1.Position.Y
                        Az(i) = Vortex.Node1.Position.Z

                        Bx(i) = Vortex.Node2.Position.X
                        By(i) = Vortex.Node2.Position.Y
                        Bz(i) = Vortex.Node2.Position.Z

                        G(i) = Vortex.G

                        i += 1

                    Next

                Next

            Next

            Dim nRings As Integer = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Ring As VortexRing In Lattice.VortexRings

                    If (Not SlenderRingsOnly) OrElse (Ring.IsSlender) Then

                        Ring.VelocityW.X = 0.0#
                        Ring.VelocityW.Y = 0.0#
                        Ring.VelocityW.Z = 0.0#

                        nRings += 1

                    End If

                Next

            Next

            Dim Vx(nRings - 1) As Double
            Dim Vy(nRings - 1) As Double
            Dim Vz(nRings - 1) As Double

            Dim Px(nRings - 1) As Double
            Dim Py(nRings - 1) As Double
            Dim Pz(nRings - 1) As Double

            i = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Ring As VortexRing In Lattice.VortexRings

                    If (Not SlenderRingsOnly) OrElse (Ring.IsSlender) Then

                        Px(i) = Ring.ControlPoint.X
                        Py(i) = Ring.ControlPoint.Y
                        Pz(i) = Ring.ControlPoint.Z

                        i += 1

                    End If

                Next

            Next

            GpuVortexSolver.CalculateVelocity(G,
                                              Ax, Ay, Az,
                                              Bx, By, Bz,
                                              Px, Py, Pz,
                                              Vx, Vx, Vz,
                                              Settings.Cutoff)

            ' Set information to lattice:

            i = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Ring As VortexRing In Lattice.VortexRings

                    If (Not SlenderRingsOnly) OrElse (Ring.IsSlender) Then

                        Ring.VelocityW.X += Vx(i)
                        Ring.VelocityW.Y += Vy(i)
                        Ring.VelocityW.Z += Vz(i)

                        i += 1

                    End If

                Next

            Next

        End Sub

        Private Sub CalculateTotalVelocityOnBoundedLatticesWithOpenCL(ByVal WithStreamOmega As Boolean)

            ' Count number of vortices

            Dim nVortices As Integer = 0

            For Each Lattice As BoundedLattice In Lattices

                nVortices += Lattice.Vortices.Count

                For Each Wake As Wake In Lattice.Wakes

                    nVortices += Wake.Vortices.Count

                Next

            Next

            ' Generate array

            Dim G(nVortices - 1) As Double
            Dim Ax(nVortices - 1) As Double
            Dim Ay(nVortices - 1) As Double
            Dim Az(nVortices - 1) As Double
            Dim Bx(nVortices - 1) As Double
            Dim By(nVortices - 1) As Double
            Dim Bz(nVortices - 1) As Double

            ' Make a list of vortices

            Dim i As Integer = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Vortex As Vortex In Lattice.Vortices

                    Ax(i) = Vortex.Node1.Position.X
                    Ay(i) = Vortex.Node1.Position.Y
                    Az(i) = Vortex.Node1.Position.Z

                    Bx(i) = Vortex.Node2.Position.X
                    By(i) = Vortex.Node2.Position.Y
                    Bz(i) = Vortex.Node2.Position.Z

                    G(i) = Vortex.G

                    i += 1

                Next

                For Each Wake As Wake In Lattice.Wakes

                    For Each Vortex As Vortex In Wake.Vortices

                        Ax(i) = Vortex.Node1.Position.X
                        Ay(i) = Vortex.Node1.Position.Y
                        Az(i) = Vortex.Node1.Position.Z

                        Bx(i) = Vortex.Node2.Position.X
                        By(i) = Vortex.Node2.Position.Y
                        Bz(i) = Vortex.Node2.Position.Z

                        G(i) = Vortex.G

                        i += 1

                    Next

                Next

            Next

            Dim nRings As Integer = 0

            For Each Lattice As BoundedLattice In Lattices

                nRings += Lattice.VortexRings.Count

            Next

            Dim Vx(nRings - 1) As Double
            Dim Vy(nRings - 1) As Double
            Dim Vz(nRings - 1) As Double

            Dim Px(nRings - 1) As Double
            Dim Py(nRings - 1) As Double
            Dim Pz(nRings - 1) As Double

            i = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Ring In Lattice.VortexRings

                    Px(i) = Ring.ControlPoint.X
                    Py(i) = Ring.ControlPoint.Y
                    Pz(i) = Ring.ControlPoint.Z

                    i += 1

                Next

            Next

            Dim VortexSolver As New GpuTools.VortexSolver

            VortexSolver.CalculateVelocity(G,
                                           Ax, Ay, Az,
                                           Bx, By, Bz,
                                           Px, Py, Pz,
                                           Vx, Vx, Vz,
                                           Settings.Cutoff)

            ' Set information to lattice:

            i = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Ring In Lattice.VortexRings

                    Ring.VelocityT.X = _StreamVelocity.X
                    Ring.VelocityT.Y = _StreamVelocity.Y
                    Ring.VelocityT.Z = _StreamVelocity.Z

                    Ring.VelocityT.X += Ring.VelocityS.X
                    Ring.VelocityT.Y += Ring.VelocityS.Y
                    Ring.VelocityT.Z += Ring.VelocityS.Z

                    If WithStreamOmega Then

                        Ring.VelocityT.AddCrossProduct(_StreamOmega, Ring.ControlPoint) ' Add stream angular velocity

                    End If

                    Ring.VelocityT.X += Vx(i)
                    Ring.VelocityT.Y += Vy(i)
                    Ring.VelocityT.Z += Vz(i)

                    i += 1

                Next

            Next

        End Sub

        '        ''' <summary>
        '        ''' General Kernel to calculate the velocities on the given points.
        '        ''' </summary>
        '        ''' <param name="thread">Thread information.</param>
        '        ''' <param name="VELOCITY_INFO">Array containing the components of the velocity vector.</param>
        '        ''' <param name="LOCATION_INFO">Array containing the components of the evaluation points.</param>
        '        ''' <param name="VORTEX_INFO">Array containing the vortex information.</param>
        '        ''' <param name="CutOff"></param>
        '        ''' <remarks>VELOCITY_INFO should be of the same lenght as LOCATION_INFO</remarks>
        '        <Cudafy()>
        '        Public Shared Sub AddVelocities(ByVal thread As GThread, VELOCITY_INFO(,) As Double, LOCATION_INFO(,) As Double, VORTEX_INFO(,) As Double, CutOff As Double())

        '            Dim i As Integer = thread.threadIdx.x + thread.blockDim.x * thread.blockIdx.x

        '            If i > VELOCITY_INFO.GetLength(0) Then Return

        '            VELOCITY_INFO(i, 0) = 0.0
        '            VELOCITY_INFO(i, 1) = 0.0
        '            VELOCITY_INFO(i, 2) = 0.0

        '            Dim j As Integer = 0

        '            While j < VORTEX_INFO.GetLength(0)

        '                Dim Lx As Double = VORTEX_INFO(j, 3) - VORTEX_INFO(j, 0) ' p2x - p1x
        '                Dim Ly As Double = VORTEX_INFO(j, 4) - VORTEX_INFO(j, 1) ' p2y - p1y
        '                Dim Lz As Double = VORTEX_INFO(j, 5) - VORTEX_INFO(j, 2) ' p2z - p1z

        '                Dim R1x As Double = LOCATION_INFO(i, 0) - VORTEX_INFO(j, 0) ' px - p1x
        '                Dim R1y As Double = LOCATION_INFO(i, 1) - VORTEX_INFO(j, 1) ' py - p1y
        '                Dim R1z As Double = LOCATION_INFO(i, 2) - VORTEX_INFO(j, 2) ' pz - p1z

        '                Dim vx As Double = Ly * R1z - Lz * R1y
        '                Dim vy As Double = Lz * R1x - Lx * R1z
        '                Dim vz As Double = Lx * R1y - Ly * R1x

        '                Dim Den As Double = FourPi * (vx * vx + vy * vy + vz * vz)

        '                If Den > CutOff(0) Then

        '                    ' Calculate the rest of the geometrical parameters:

        '                    Dim R2x As Double = LOCATION_INFO(i, 0) - VORTEX_INFO(j, 3)
        '                    Dim R2y As Double = LOCATION_INFO(i, 1) - VORTEX_INFO(j, 4)
        '                    Dim R2z As Double = LOCATION_INFO(i, 2) - VORTEX_INFO(j, 5)

        '                    Dim NR1 As Double = 1 / Cudafy.GMath.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
        '                    Dim NR2 As Double = 1 / Cudafy.GMath.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

        '                    Dim dx As Double = NR1 * R1x - NR2 * R2x
        '                    Dim dy As Double = NR1 * R1y - NR2 * R2y
        '                    Dim dz As Double = NR1 * R1z - NR2 * R2z

        '                    Dim Factor As Double = VORTEX_INFO(j, 6) * (Lx * dx + Ly * dy + Lz * dz) / Den

        '                    VELOCITY_INFO(i, 0) += Factor * vx
        '                    VELOCITY_INFO(i, 1) += Factor * vy
        '                    VELOCITY_INFO(i, 2) += Factor * vz

        '                End If

        '                j += 1

        '            End While

        '        End Sub

    End Class

End Namespace


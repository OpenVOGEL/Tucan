'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2020 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components

Namespace CalculationModel.Solver

    Partial Public Class Solver

        Private GpuVortexSolver As GpuTools.VortexSolver

        ''' <summary>
        ''' Test if the GPU is able to handle double precision.
        ''' </summary>
        ''' <returns></returns>
        Private Function TestOpenCL() As Boolean

            RaiseEvent PushMessage("Testing GPU double precision capability...")

            If GpuTools.GpuCore.TestGpuDoublePrecision(Settings.GpuDeviceId) Then

                RaiseEvent PushMessage("Double precision enabled")

                Return True

            Else

                RaiseEvent PushMessage("Double precision disabled")

                Return False

            End If

        End Function

        ''' <summary>
        ''' Calculates the velocity induced by the wakes on the bounded lattices using the GPU.
        ''' This procedure can only be used when there are only slender panels.
        ''' </summary>
        Private Sub CalculateVelocityInducedByTheWakesOnBoundedLatticesWithOpenCL()

            Dim Start As DateTime = Now

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

                    Ring.VelocityW.X = 0.0#
                    Ring.VelocityW.Y = 0.0#
                    Ring.VelocityW.Z = 0.0#

                    nRings += 1

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

                    Px(i) = Ring.ControlPoint.X
                    Py(i) = Ring.ControlPoint.Y
                    Pz(i) = Ring.ControlPoint.Z

                    i += 1

                Next

            Next

            RaiseEvent PushMessage(String.Format("{0} -> GPU assignments", (Now - Start).ToString))

            GpuVortexSolver.CalculateVelocity(G,
                                              Ax, Ay, Az,
                                              Bx, By, Bz,
                                              Px, Py, Pz,
                                              Vx, Vx, Vz,
                                              Settings.Cutoff)

            RaiseEvent PushMessage(String.Format("{0} -> actual GPU task", (Now - Start).ToString))

            ' Set information to lattice:

            Start = Now

            i = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Ring As VortexRing In Lattice.VortexRings

                    Ring.VelocityW.X = Vx(i)
                    Ring.VelocityW.Y = Vy(i)
                    Ring.VelocityW.Z = Vz(i)

                    i += 1

                Next

            Next

            RaiseEvent PushMessage(String.Format("{0} -> retriving GPU data", (Now - Start).ToString))

        End Sub

        Private Sub CalculateTotalVelocityOnBoundedLatticesWithOpenCL(ByVal WithStreamOmega As Boolean)

            Dim Start As DateTime = Now

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

            RaiseEvent PushMessage(String.Format("{0} -> GPU assignments", (Now - Start).ToString))

            Start = Now

            Dim VortexSolver As New GpuTools.VortexSolver

            VortexSolver.CalculateVelocity(G,
                                           Ax, Ay, Az,
                                           Bx, By, Bz,
                                           Px, Py, Pz,
                                           Vx, Vx, Vz,
                                           Settings.Cutoff)

            RaiseEvent PushMessage(String.Format("{0} -> actual GPU task", (Now - Start).ToString))

            ' Set information to lattice:

            Start = Now

            i = 0

            For Each Lattice As BoundedLattice In Lattices

                For Each Ring In Lattice.VortexRings

                    Ring.VelocityT.X = Stream.Velocity.X
                    Ring.VelocityT.Y = Stream.Velocity.Y
                    Ring.VelocityT.Z = Stream.Velocity.Z

                    Ring.VelocityT.X += Ring.VelocityS.X
                    Ring.VelocityT.Y += Ring.VelocityS.Y
                    Ring.VelocityT.Z += Ring.VelocityS.Z

                    If WithStreamOmega Then

                        Ring.VelocityT.AddCrossProduct(Stream.Omega, Ring.ControlPoint) ' Add stream angular velocity

                    End If

                    Ring.VelocityT.X += Vx(i)
                    Ring.VelocityT.Y += Vy(i)
                    Ring.VelocityT.Z += Vz(i)

                    i += 1

                Next

            Next

            RaiseEvent PushMessage(String.Format("{0} -> retriving GPU data", (Now - Start).ToString))

        End Sub

    End Class

End Namespace


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

Namespace CalculationModel.Solver

    Partial Public Class Solver

        Private Sub TestOpenCL()

            RaiseEvent PushMessage("Testing GPU double presition capability...")

            If GpuTools.GpuCore.TestGpuDoublePresition Then

                RaiseEvent PushMessage("Double presition enabled")

            Else

                RaiseEvent PushMessage("Double presition disabled")

            End If

        End Sub

        '        Public Const FourPi As Double = 4 * Math.PI

        '        Private Sub TestCuda()

        '            RaiseEvent PushMessage("Testing cuda")

        '            CudafyModes.Target = eGPUType.OpenCL
        '            CudafyModes.DeviceId = 2
        '            CudafyTranslator.Language = eLanguage.OpenCL

        '            Dim km As CudafyModule = CudafyTranslator.Cudafy
        '            Dim GPU As GPGPU = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId)
        '            GPU.LoadModule(km)

        '            Dim a(3) As Double
        '            a(0) = 1
        '            a(1) = 2
        '            a(2) = 3
        '            a(3) = 4
        '            Dim a_dev() As Double = GPU.Allocate(Of Double)(a)
        '            GPU.CopyToDevice(a, a_dev)

        '            Dim s(4) As Double
        '            Dim s_dev() As Double = GPU.Allocate(Of Double)(s)
        '            GPU.CopyToDevice(s, s_dev)

        '            GPU.Launch(4, 1, "TestCudaMethod", a_dev, s_dev)

        '            GPU.CopyFromDevice(s_dev, s)

        '            RaiseEvent PushMessage("Cuda test done")

        '        End Sub

        '        <Cudafy()>
        '        Public Shared Sub TestCudaMethod(thread As GThread, a As Double(), s As Double())

        '            Dim tid As Integer = thread.blockIdx.x

        '            If tid < 4 Then
        '                For i = 0 To tid
        '                    s(tid) = a(i)
        '                Next
        '            End If

        '        End Sub

        '        Private Sub CalculateVelocityI_CUDA(Optional ByVal WithStreamOmega As Boolean = False)

        '            ' Generate and load module:

        '            CudafyModes.Target = eGPUType.Cuda
        '            CudafyModes.DeviceId = 0
        '            CudafyTranslator.Language = eLanguage.Cuda

        '            Dim GPU As GPGPU = CudafyHost.GetDevice(CudafyModes.Target, CudafyModes.DeviceId)
        '            Dim km As CudafyModule = CudafyTranslator.Cudafy
        '            GPU.LoadModule(km)

        '            ' Count number of vortices

        '            Dim nVortices As Integer = 0

        '            For Each Lattice As BoundedLattice In Lattices

        '                nVortices += Lattice.Vortices.Count

        '                For Each Wake As Wake In Lattice.Wakes

        '                    nVortices += Wake.Vortices.Count

        '                Next

        '            Next

        '            ' Generate array

        '            Dim VORTEX_INFO(nVortices - 1, 6) As Double ' pAx, pAy, pAz, pBx, pBy, pBz, G

        '            Dim VORTEX_INFO_D(,) As Double = GPU.Allocate(Of Double)(VORTEX_INFO)

        '            ' Make a list of vortices

        '            Dim i As Integer = 0

        '            For Each Lattice As BoundedLattice In Lattices

        '                For Each Vortex As Vortex In Lattice.Vortices

        '                    VORTEX_INFO(i, 0) = Vortex.Node1.Position.X
        '                    VORTEX_INFO(i, 1) = Vortex.Node1.Position.Y
        '                    VORTEX_INFO(i, 2) = Vortex.Node1.Position.Z

        '                    VORTEX_INFO(i, 3) = Vortex.Node2.Position.X
        '                    VORTEX_INFO(i, 4) = Vortex.Node2.Position.Y
        '                    VORTEX_INFO(i, 5) = Vortex.Node2.Position.Z

        '                    VORTEX_INFO(i, 6) = Vortex.G

        '                    i += 1

        '                Next

        '                For Each Wake As Wake In Lattice.Wakes

        '                    For Each Vortex As Vortex In Wake.Vortices

        '                        VORTEX_INFO(i, 0) = Vortex.Node1.Position.X
        '                        VORTEX_INFO(i, 1) = Vortex.Node1.Position.Y
        '                        VORTEX_INFO(i, 2) = Vortex.Node1.Position.Z

        '                        VORTEX_INFO(i, 3) = Vortex.Node2.Position.X
        '                        VORTEX_INFO(i, 4) = Vortex.Node2.Position.Y
        '                        VORTEX_INFO(i, 5) = Vortex.Node2.Position.Z

        '                        VORTEX_INFO(i, 6) = Vortex.G

        '                        i += 1

        '                    Next

        '                Next

        '            Next

        '            Dim nRings As Integer = 0

        '            For Each Lattice As BoundedLattice In Lattices

        '                nRings += Lattice.VortexRings.Count

        '            Next

        '            Dim nThreads As Integer = 1
        '            Dim nBlocks As Integer = Math.Ceiling(nRings / nThreads)

        '            Dim VELOCITY_INFO(nRings - 1, 2) As Double
        '            Dim VELOCITY_INFO_D(,) As Double = GPU.Allocate(Of Double)(VELOCITY_INFO)

        '            Dim LOCATION_INFO(nRings - 1, 2) As Double
        '            Dim LOCATION_INFO_D(,) As Double = GPU.Allocate(Of Double)(LOCATION_INFO)

        '            i = 0

        '            For Each Lattice As BoundedLattice In Lattices

        '                For Each Ring In Lattice.VortexRings

        '                    LOCATION_INFO(i, 0) = Ring.ControlPoint.X
        '                    LOCATION_INFO(i, 1) = Ring.ControlPoint.Y
        '                    LOCATION_INFO(i, 2) = Ring.ControlPoint.Z

        '                    i += 1

        '                Next

        '            Next

        '            GPU.CopyToDevice(VORTEX_INFO, VORTEX_INFO_D)
        '            GPU.CopyToDevice(LOCATION_INFO, LOCATION_INFO_D)

        '            Dim Cutoff(0) As Double
        '            Cutoff(0) = Settings.Cutoff
        '            Dim Cutoff_D() As Double = GPU.Allocate(Of Double)(Cutoff)
        '            GPU.CopyToDevice(Cutoff, Cutoff_D)

        '            Try

        '                GPU.Launch(nBlocks, nThreads).AddVelocities(VELOCITY_INFO_D, LOCATION_INFO_D, VORTEX_INFO_D, Cutoff_D)
        '                GPU.CopyFromDevice(VELOCITY_INFO_D, VELOCITY_INFO)

        '            Finally

        '                GPU.Free(VORTEX_INFO_D)
        '                GPU.Free(VELOCITY_INFO_D)
        '                GPU.Free(LOCATION_INFO_D)
        '                GPU.Free(Cutoff_D)

        '            End Try

        '            ' Set information to lattice:

        '            i = 0

        '            For Each Lattice As BoundedLattice In Lattices

        '                For Each Ring In Lattice.VortexRings

        '                    Ring.VelocityW.X = _StreamVelocity.X
        '                    Ring.VelocityW.Y = _StreamVelocity.Y
        '                    Ring.VelocityW.Z = _StreamVelocity.Z

        '                    Ring.VelocityW.X += Ring.VelocityS.X
        '                    Ring.VelocityW.Y += Ring.VelocityS.Y
        '                    Ring.VelocityW.Z += Ring.VelocityS.Z

        '                    If WithStreamOmega Then

        '                        Ring.VelocityW.AddCrossProduct(_StreamOmega, Ring.ControlPoint) ' Add stream angular velocity

        '                    End If

        '                    Ring.VelocityW.X += VELOCITY_INFO(i, 0)
        '                    Ring.VelocityW.Y += VELOCITY_INFO(i, 1)
        '                    Ring.VelocityW.Z += VELOCITY_INFO(i, 2)

        '                    i += 1

        '                Next

        '            Next

        '        End Sub

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


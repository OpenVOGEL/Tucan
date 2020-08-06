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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.MathTools.Algebra.CustomMatrices
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.MathTools.Integration

Public Module AeroTests

    ''' <summary>
    ''' Performs some tests to check the consistency of the solver
    ''' </summary>
    Public Sub TestAerodynamicSolver()

        TestPanelOrientation()
        TestTriangularPanels()
        TestSourcePanelVelocity()

    End Sub

    ''' <summary>
    ''' This test verifies that the source and doublet potentials are the same
    ''' when for a non flat 4-nodes panel only the order of the nodes is modified.
    ''' </summary>
    Private Sub TestPanelOrientation()

        System.Console.WriteLine("Testing influence coeficients in non flat panel")

        Dim N1 As New Node
        Dim N2 As New Node
        Dim N3 As New Node
        Dim N4 As New Node
        Dim P As New Vector3
        Dim M As New RotationMatrix
        Dim A As New OrientationAngles

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
        N4.Position.Z = 0.1

        P.X = 0.5
        P.Y = 0.5
        P.Z = 1.0

        A.R1 = Math.PI / 5
        M.Generate(A)
        N1.Position.Rotate(M)
        N2.Position.Rotate(M)
        N3.Position.Rotate(M)
        N4.Position.Rotate(M)
        P.Rotate(M)

        '--------------------------------------------------------------
        ' Doublet potential
        '--------------------------------------------------------------

        System.Console.WriteLine("Doublet potentials")

        Dim Panel1 As New VortexRing4(N1, N2, N3, N4, 0, False, False)

        Dim P1 As Double = Panel1.GetDoubletPotentialInfluence(P, False)

        System.Console.WriteLine("P1 = {0:F14}", (P1))

        Dim Panel2 As New VortexRing4(N2, N3, N4, N1, 0, False, False)

        Dim P2 As Double = Panel2.GetDoubletPotentialInfluence(P, False)

        System.Console.WriteLine("P2 = {0:F14}", (P2))

        If (P1 - P2) / P1 < 0.0000000001 Then
            System.Console.WriteLine("Doublets OK")
        End If

        '--------------------------------------------------------------
        ' Source potential
        '--------------------------------------------------------------

        System.Console.WriteLine("Source potentials")

        Dim S1 As Double = Panel1.GetSourcePotentialInfluence(P, False)

        System.Console.WriteLine("S1 = {0:F14}", (S1))

        Dim S2 As Double = Panel2.GetSourcePotentialInfluence(P, False)

        System.Console.WriteLine("S2 = {0:F14}", (S2))

        If (S1 - S2) / S1 < 0.0000000001 Then
            System.Console.WriteLine("Sources OK")
        End If

    End Sub

    ''' <summary>
    ''' This test verifies that the source and doublet potentials derivatives
    ''' are equivalent to their induced velocity.
    ''' </summary>
    Private Sub TestSourcePanelVelocity()

        System.Console.WriteLine("Testing influence velocity against potential derivative in quad panel")

        Dim N1 As New Node
        Dim N2 As New Node
        Dim N3 As New Node
        Dim N4 As New Node
        Dim P As New Vector3

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

        Dim Panel As New VortexRing4(N1, N2, N3, N4, 0, False, False)

        For I = 1 To 2

            System.Console.WriteLine("CASE " & I.ToString)

            Select Case I
                Case 1
                    P.X = 0.0
                    P.Y = 0.0
                    P.Z = 0.5
                Case 2
                    P.X = 0.0
                    P.Y = 0.0
                    P.Z = -0.5
            End Select

            Dim V As New Vector3

            Dim Rx As New Vector3(P)
            Rx.X += 0.00001
            Dim Ry As New Vector3(P)
            Ry.Y += 0.00001
            Dim Rz As New Vector3(P)
            Rz.Z += 0.00001

            ' Source

            System.Console.WriteLine("Source:")

            Panel.AddSourceVelocityInfluence(V, P, False)

            Dim Qx As Double = (Panel.GetSourcePotentialInfluence(Rx, False) - Panel.GetSourcePotentialInfluence(P, False)) / 0.00001
            System.Console.WriteLine("dQ/dx = {0:F14}", (Qx))
            System.Console.WriteLine("Vx    = {0:F14}", (V.X))

            Dim Qy As Double = (Panel.GetSourcePotentialInfluence(Ry, False) - Panel.GetSourcePotentialInfluence(P, False)) / 0.00001
            System.Console.WriteLine("dQ/dy = {0:F14}", (Qy))
            System.Console.WriteLine("Vy    = {0:F14}", (V.Y))

            Dim Qz As Double = (Panel.GetSourcePotentialInfluence(Rz, False) - Panel.GetSourcePotentialInfluence(P, False)) / 0.00001
            System.Console.WriteLine("dQ/dz = {0:F14}", (Qz))
            System.Console.WriteLine("Vz    = {0:F14}", (V.Z))

            V.SetToCero()

            ' Dipole

            System.Console.WriteLine("Doublet:")

            Panel.AddDoubletVelocityInfluence(V, P, False, False)

            Qx = (Panel.GetDoubletPotentialInfluence(Rx, False) - Panel.GetDoubletPotentialInfluence(P, False)) / 0.00001
            System.Console.WriteLine("dQ/dx = {0:F14}", (Qx))
            System.Console.WriteLine("Vx    = {0:F14}", (V.X))

            Qy = (Panel.GetDoubletPotentialInfluence(Ry, False) - Panel.GetDoubletPotentialInfluence(P, False)) / 0.00001
            System.Console.WriteLine("dQ/dy = {0:F14}", (Qy))
            System.Console.WriteLine("Vy    = {0:F14}", (V.Y))

            Qz = (Panel.GetDoubletPotentialInfluence(Rz, False) - Panel.GetDoubletPotentialInfluence(P, False)) / 0.00001
            System.Console.WriteLine("dQ/dz = {0:F14}", (Qz))
            System.Console.WriteLine("Vz    = {0:F14}", (V.Z))

        Next

    End Sub

    ''' <summary>
    ''' This test checks that the source and doublet potentials are the same when 
    ''' using a single 4-nodes panels as when using two 3-nodes panels.
    ''' </summary>
    Private Sub TestTriangularPanels()

        System.Console.WriteLine("Testing consistency of triangular and quadrilateral panels")

        Dim N1 As New Node
        Dim N2 As New Node
        Dim N3 As New Node
        Dim N4 As New Node
        Dim P As New Vector3
        Dim M As New RotationMatrix
        Dim A As New OrientationAngles

        Dim SourcePotentials1(180) As Double
        Dim DoubletPotentials1(180) As Double

        Dim SourcePotentials2(180) As Double
        Dim DoubletPotentials2(180) As Double

        N1.Position.X = 0.1
        N1.Position.Y = 0.0
        N1.Position.Z = 0.0
        N1.IndexG = 0

        N2.Position.X = 1.0
        N2.Position.Y = 0.0
        N2.Position.Z = 0.0
        N2.IndexG = 1

        N3.Position.X = 1.0
        N3.Position.Y = 1.0
        N3.Position.Z = 0.0
        N3.IndexG = 2

        N4.Position.X = 0.0
        N4.Position.Y = 1.0
        N4.Position.Z = 0.0
        N4.IndexG = 3

        P.X = 0.5
        P.Y = 0.5
        P.Z = 1.0

        'A.Psi = Math.PI / 5
        'A.Tita = Math.PI / 5
        'A.Fi = Math.PI / 5
        'M.Generate(A)
        'N1.Position.Rotate(M)
        'N2.Position.Rotate(M)
        'N3.Position.Rotate(M)
        'N4.Position.Rotate(M)
        'P.Rotate(M)

        Dim Epsilon As Double = 0.0000000001

        '--------------------------------------------------------------
        ' Doublet potential
        '--------------------------------------------------------------

        System.Console.WriteLine("Doublet potentials")

        Dim Panel4 As New VortexRing4(N1, N2, N3, N4, 0, False, False)

        Dim P4 As Double = Panel4.GetDoubletPotentialInfluence(P, False)

        System.Console.WriteLine("4-nodes = {0:F14}", (P4))

        Dim Panel3a As New VortexRing3(N1, N2, N3, 0, False, False)
        Dim Panel3b As New VortexRing3(N3, N4, N1, 0, False, False)

        Dim P3 As Double = 0.0#
        P3 += Panel3a.GetDoubletPotentialInfluence(P, False)
        P3 += Panel3b.GetDoubletPotentialInfluence(P, False)

        System.Console.WriteLine("3-nodes = {0:F14}", (P3))

        If (P4 - P3) / P4 < Epsilon Then
            System.Console.WriteLine("Doublets OK")
        Else
            System.Console.WriteLine("Doublets NOT OK!")
        End If

        '--------------------------------------------------------------
        ' Source potential
        '--------------------------------------------------------------

        System.Console.WriteLine("Source potentials")

        Dim S4 As Double = Panel4.GetSourcePotentialInfluence(P, False)

        System.Console.WriteLine("4-nodes = {0:F14}", (S4))

        Dim S3 As Double = 0.0#
        S3 += Panel3a.GetSourcePotentialInfluence(P, False)
        S3 += Panel3b.GetSourcePotentialInfluence(P, False)

        System.Console.WriteLine("3-nodes = {0:F14}", (S3))

        If (S4 - S3) / S4 < Epsilon Then
            System.Console.WriteLine("Sources OK")
        Else
            System.Console.WriteLine("Sources NOT OK!")
        End If

        '--------------------------------------------------------------
        ' Source velocity
        '--------------------------------------------------------------

        System.Console.WriteLine("Source velocity")

        Dim VS4 As New Vector3
        Panel4.AddSourceVelocityInfluence(VS4, P, False)

        System.Console.WriteLine("4-nodes = {0:F14}", (VS4.EuclideanNorm))

        Dim VS3 As New Vector3
        Panel3a.AddSourceVelocityInfluence(VS3, P, False)
        Panel3b.AddSourceVelocityInfluence(VS3, P, False)

        System.Console.WriteLine("3-nodes = {0:F14}", (VS3.EuclideanNorm))

        If (VS4.EuclideanNorm - VS3.EuclideanNorm) / VS4.EuclideanNorm < Epsilon Then
            System.Console.WriteLine("Source velocity OK")
        Else
            System.Console.WriteLine("Source velocity NOT OK!")
        End If

        If (VS4.X - VS3.X) / VS4.X < Epsilon Then
            System.Console.WriteLine("Source velocity X OK")
        Else
            System.Console.WriteLine("Source velocity X NOT OK!")
        End If

        If (VS4.Y - VS3.Y) / VS4.Y < Epsilon Then
            System.Console.WriteLine("Source velocity Y OK")
        Else
            System.Console.WriteLine("Source velocity Y NOT OK!")
        End If

        If (VS4.Z - VS3.Z) / VS4.Z < Epsilon Then
            System.Console.WriteLine("Source velocity Z OK")
        Else
            System.Console.WriteLine("Source velocity Z NOT OK!")
        End If

        '--------------------------------------------------------------
        ' Doublet velocity
        '--------------------------------------------------------------

        System.Console.WriteLine("Doublet velocity")

        Dim VD4 As New Vector3
        Panel4.AddDoubletVelocityInfluence(VD4, P, 0.0001, False)

        System.Console.WriteLine("4-nodes = {0:F14}", (VD4.EuclideanNorm))

        Dim VD3 As New Vector3
        Panel3a.AddDoubletVelocityInfluence(VD3, P, 0.0001, False)
        Panel3b.AddDoubletVelocityInfluence(VD3, P, 0.0001, False)

        System.Console.WriteLine("3-nodes = {0:F14}", (VD3.EuclideanNorm))

        If (VD4.EuclideanNorm - VD3.EuclideanNorm) / VD4.EuclideanNorm < Epsilon Then
            System.Console.WriteLine("Doublet velocity OK")
        Else
            System.Console.WriteLine("Doublet velocity NOT OK!")
        End If

        If (VD4.X - VD3.X) / VD4.X < Epsilon Then
            System.Console.WriteLine("Doublet velocity X OK")
        Else
            System.Console.WriteLine("Doublet velocity X NOT OK!")
        End If

        If (VD4.Y - VD3.Y) / VD4.Y < Epsilon Then
            System.Console.WriteLine("Doublet velocity Y OK")
        Else
            System.Console.WriteLine("Doublet velocity Y NOT OK!")
        End If

        If (VD4.Z - VD3.Z) / VD4.Z < Epsilon Then
            System.Console.WriteLine("Doublet velocity Z OK")
        Else
            System.Console.WriteLine("Doublet velocity Z NOT OK!")
        End If

    End Sub

    ''' <summary>
    ''' Tests the Hamming ODES solver
    ''' </summary>
    Public Sub TestHammingSolver()

        System.Console.WriteLine("Testing Hamming ODEs solver")

        TestParabolicShot()

        TestHarmonicOscillator()

    End Sub

    ''' <summary>
    ''' Tests the Hamming ODES solver
    ''' </summary>
    Private Sub TestParabolicShot()

        '' Simple 2D parabolic shot
        '-------------------------------------------------------

        System.Console.WriteLine("Test case: parabolic shot")

        Dim V0 As New Vector3(100.0#, 0.0#, 100.0#)
        Dim O0 As New Vector3(0.0#, 0.0#, 0.0#)
        Dim g As Double = -10.0#
        Dim B As New Base3
        B.CanonicalBase()

        Dim N As Integer = 100
        Dim Dt As Double = 0.1
        Dim T As Double = 0.0#

        Dim Solver As New MotionIntegrator(N, Dt, V0, O0, g, B)

        Solver.Mass = 1.0#
        Solver.Ixx = 1.0#
        Solver.Iyy = 1.0#
        Solver.Izz = 1.0#

        Dim F0 As New Vector3(0.0#, 0.0#, 0.0#)
        Dim M0 As New Vector3(0.0#, 0.0#, 0.0#)
        Solver.SetInitialForces(F0, M0)

        For I = 1 To N

            T += Dt

            Dim AnaliticSolution As Variable
            AnaliticSolution.Vx = V0.X
            AnaliticSolution.Vz = V0.Z + g * T
            AnaliticSolution.Px = V0.X * T
            AnaliticSolution.Pz = V0.Z * T + 0.5 * g * T ^ 2

            Solver.Predict()

            Dim Converged As Boolean = False

            For J = 1 To 10

                If Solver.Correct(F0, M0) Then
                    Converged = True
                    Exit For
                End If

            Next

            If Not Converged Then
                System.Console.WriteLine("FAILED to converge")
                Return
            Else
                Dim Err As Variable = (AnaliticSolution - Solver.State(I)) / AnaliticSolution
                System.Console.WriteLine(String.Format("{0,10:F6} | {1,10:F6} | {2,10:F6} | {3,10:F6}", Err.Px, Err.Pz, Err.Vx, Err.Vz))

            End If

        Next

        System.Console.WriteLine("TEST PASSED")

    End Sub

    ''' <summary>
    ''' Tests the Hamming ODES solver
    ''' </summary>
    Private Sub TestHarmonicOscillator()

        '' Simple 1D harmonic oscillator
        '-------------------------------------------------------

        System.Console.WriteLine("Test case: harmonic oscillator")

        Dim FileId As Integer = FreeFile()
        FileOpen(FileId, "\oscillator.txt", OpenMode.Output)

        Dim V0 As New Vector3(1.0#, 0.0#, 0.0#)
        Dim O0 As New Vector3(0.0#, 0.0#, 0.0#)
        Dim B As New Base3
        B.CanonicalBase()

        Dim N As Integer = 400
        Dim Dt As Double = 0.025
        Dim T As Double = 0.0#

        Dim M As Double = 1.0#
        Dim K As Double = 1.0#
        Dim C As Double = 0.1#

        Dim Cc As Double = 2.0# * Math.Sqrt(K * M)
        Dim Psi As Double = C / Cc
        Dim W As Double = Math.Sqrt(K / M)
        Dim Wd As Double = W * Math.Sqrt(1 - Psi ^ 2.0#)
        Dim Td As Double = 2.0# * Math.PI / Wd

        System.Console.WriteLine(String.Format("Damping ratio      = {0,10:E6}", Psi))
        System.Console.WriteLine(String.Format("Oscillation period = {0,10:E6}s", Td))
        System.Console.WriteLine(String.Format("Test period        = {0,10:E6}s ({1,4:F2} cicles)", N * Dt, N * Dt / Td))

        Dim Solver As New MotionIntegrator(N, Dt, V0, O0, 0.0#, B)

        Solver.Mass = M
        Solver.Ixx = 1.0#
        Solver.Iyy = 1.0#
        Solver.Izz = 1.0#

        Dim F As New Vector3(0.0#, 0.0#, 0.0#)
        Dim Q As New Vector3(0.0#, 0.0#, 0.0#)
        Solver.SetInitialForces(F, Q)

        Dim Epsilon As Variable
        Epsilon.Px = 0.005
        Epsilon.Vx = 0.005
        Solver.Epsilon = Epsilon

        For I = 1 To N

            T += Dt

            Dim AnaliticSolution As Variable
            AnaliticSolution.Vx = V0.X * Math.Exp(-Psi * W * T) * (Math.Cos(Wd * T) - Psi / Math.Sqrt(1 - Psi ^ 2) * Math.Sin(Wd * T))
            AnaliticSolution.Px = Math.Exp(-Psi * W * T) * (V0.X / Wd) * Math.Sin(Wd * T)

            Solver.Predict()

            Dim Converged As Boolean = False

            For J = 1 To 20

                Dim State As Variable = Solver.State(I)

                F.X = -K * State.Px - C * State.Vx

                If Solver.Correct(F, Q) Then
                    Converged = True
                    Exit For
                End If

            Next

            If Not Converged Then
                System.Console.WriteLine("FAILED to converge")
                Return
            Else
                Dim State As Variable = Solver.State(I)
                Dim Err As Variable = (AnaliticSolution - State) / AnaliticSolution
                PrintLine(FileId, String.Format("{0,11:F6} | {1,11:F6} | {2,11:F6} | {3,11:F6} | {4,11:F6} | {5,11:F6}", State.Px, AnaliticSolution.Px, Err.Px, State.Vx, AnaliticSolution.Vx, Err.Vx))
            End If

        Next

        FileClose(FileId)

        System.Console.WriteLine("TEST PASSED")

    End Sub

End Module


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

Public Module AeroTests

    ''' <summary>
    ''' Performs some tests to check the consistency of the solver
    ''' </summary>
    Public Sub TestAerodynamicSolver()

        TestPanelOrientation()
        TestTriangularPanels()

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
        Dim A As New EulerAngles

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

        A.Psi = Math.PI / 5
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
        Dim A As New EulerAngles

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

End Module


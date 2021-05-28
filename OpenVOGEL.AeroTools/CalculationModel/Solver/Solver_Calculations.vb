'#############################################################################
'OpenVOGEL (openvogel.org)
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

'' Standard .NET frameworks dependencies
'-----------------------------------------------------------------------------
Imports System.Threading.Tasks

'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports DotNumerics.LinearAlgebra
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components

'#############################################################################
' Unit: Solver_Calculations
'
' This unit does general calculations on the kernel that are used on different
' simulations.
'#############################################################################
Namespace CalculationModel.Solver

    ''' <summary>
    ''' This is the OpenVOGEL kernel class. This class contains the whole
    ''' calculation model and is able of doing several simulation types.
    ''' </summary>
    Partial Public Class Solver

        ' This is the main part containing the UVLM method algorithm.

#Region " General calculations "

        ''' <summary>
        ''' Checks if the model contains sources
        ''' </summary>
        Public Sub CheckForSources()

            WithSources = False
            For Each Lattice In Lattices
                For Each Ring In Lattice.VortexRings
                    If Not Ring.IsSlender Then
                        WithSources = True
                        Return
                    End If
                Next
            Next

        End Sub

        ''' <summary>
        ''' Gives to each vortex its corresponding global index on vectors and matrices and returns the number of bounded vortex rings.
        ''' </summary>
        Public Function IndexateLattices() As Integer

            Dim NodeIndex As Integer = 0
            Dim RingIndex As Integer = 0

            For Each Lattice In Lattices
                For Each Node In Lattice.Nodes
                    Node.IndexG = NodeIndex
                    NodeIndex += 1
                Next
                For Each Ring In Lattice.VortexRings
                    Ring.IndexG = RingIndex
                    RingIndex += 1
                Next
            Next

            Return RingIndex

        End Function

        ''' <summary>
        ''' Assign surrounding rings to each panel by scanning all lattices.
        ''' Additionally, it marks primitive panels as such.
        ''' </summary>
        Public Sub FindSurroundingRingsGlobally()

            RaiseEvent PushMessage("Finding all adjacent panels")

            Dim Tolerance As Double = Settings.SurveyTolerance

            Dim Order4(4, 2) As Integer

            Order4(0, 0) = 1
            Order4(0, 1) = 2

            Order4(1, 0) = 2
            Order4(1, 1) = 3

            Order4(2, 0) = 3
            Order4(2, 1) = 4

            Order4(3, 0) = 4
            Order4(3, 1) = 1

            Dim Order3(3, 2) As Integer

            Order3(0, 0) = 1
            Order3(0, 1) = 2

            Order3(1, 0) = 2
            Order3(1, 1) = 3

            Order3(2, 0) = 3
            Order3(2, 1) = 1

            For Each Lattice In Lattices

                ' Run each ring in parallel to increase speed

                Parallel.ForEach(Lattice.VortexRings,
                Sub(Ring As VortexRing)

                    Dim Pi As Vector3
                    Dim Pj As Vector3

                    Dim Pm As Vector3
                    Dim Pn As Vector3

                    Ring.InitializeSurroundingRings()

                    If Ring.Type = VortexRingType.VR4 Then

                        For m = 0 To 3 ' For each local segment:

                            Pi = Ring.Node(Order4(m, 0)).Position
                            Pj = Ring.Node(Order4(m, 1)).Position

                            For Each OtherLattice In Lattices

                                For Each OtherRing In OtherLattice.VortexRings

                                    If Ring.IndexG <> OtherRing.IndexG Then

                                        If OtherRing.Type = VortexRingType.VR4 Then

                                            For n = 0 To 3 ' For each other segment:

                                                If OtherRing.Reversed Then
                                                    Pm = OtherRing.Node(Order4(n, 1)).Position
                                                    Pn = OtherRing.Node(Order4(n, 0)).Position
                                                Else
                                                    Pm = OtherRing.Node(Order4(n, 0)).Position
                                                    Pn = OtherRing.Node(Order4(n, 1)).Position
                                                End If

                                                If (Pi.DistanceTo(Pm) < Tolerance AndAlso Pj.DistanceTo(Pn) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (Pi.DistanceTo(Pn) < Tolerance AndAlso Pj.DistanceTo(Pm) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        ElseIf OtherRing.Type = VortexRingType.VR3 Then

                                            For n = 0 To 2 ' For each other segment:

                                                If OtherRing.Reversed Then
                                                    Pm = OtherRing.Node(Order3(n, 1)).Position
                                                    Pn = OtherRing.Node(Order3(n, 0)).Position
                                                Else
                                                    Pm = OtherRing.Node(Order3(n, 0)).Position
                                                    Pn = OtherRing.Node(Order3(n, 1)).Position
                                                End If

                                                If (Pi.DistanceTo(Pm) < Tolerance AndAlso Pj.DistanceTo(Pn) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (Pi.DistanceTo(Pn) < Tolerance AndAlso Pj.DistanceTo(Pm) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        End If

                                    End If

                                Next

                            Next

                        Next

                    ElseIf Ring.Type = VortexRingType.VR3 Then

                        For m = 0 To 2 ' For each local segment:

                            Pi = Ring.Node(Order3(m, 0)).Position
                            Pj = Ring.Node(Order3(m, 1)).Position

                            For Each OtherLattice In Lattices

                                For Each OtherRing In OtherLattice.VortexRings

                                    If Ring.IndexG <> OtherRing.IndexG Then

                                        If OtherRing.Type = VortexRingType.VR4 Then

                                            For n = 0 To 3 ' For each other segment:

                                                If OtherRing.Reversed Then
                                                    Pm = OtherRing.Node(Order4(n, 1)).Position
                                                    Pn = OtherRing.Node(Order4(n, 0)).Position
                                                Else
                                                    Pm = OtherRing.Node(Order4(n, 0)).Position
                                                    Pn = OtherRing.Node(Order4(n, 1)).Position
                                                End If

                                                If (Pi.DistanceTo(Pm) < Tolerance And Pj.DistanceTo(Pn) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (Pi.DistanceTo(Pn) < Tolerance And Pj.DistanceTo(Pm) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        ElseIf OtherRing.Type = VortexRingType.VR3 Then

                                            For n = 0 To 2 ' For each other segment:

                                                If OtherRing.Reversed Then
                                                    Pm = OtherRing.Node(Order3(n, 1)).Position
                                                    Pn = OtherRing.Node(Order3(n, 0)).Position
                                                Else
                                                    Pm = OtherRing.Node(Order3(n, 0)).Position
                                                    Pn = OtherRing.Node(Order3(n, 1)).Position
                                                End If

                                                If (Pi.DistanceTo(Pm) < Tolerance And Pj.DistanceTo(Pn) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (Pi.DistanceTo(Pn) < Tolerance And Pj.DistanceTo(Pm) < Tolerance) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        End If

                                    End If

                                Next

                            Next

                        Next

                    End If

                End Sub)

                ' Set primitive flag on primitive panels:

                For Each Wake In Lattice.Wakes

                    For Each Primitive In Wake.Primitive.Rings
                        Lattice.VortexRings(Primitive).IsPrimitive = True
                    Next

                Next

            Next

        End Sub

        ''' <summary>
        ''' Constructs the matrix of mutual influence coeficients.
        ''' </summary>
        ''' <param name="Indexate">Indicates if nodes should be globaly indexated (this is only required once).
        ''' In that case indexation will occur and matrix and RHS vector will be created.</param>
        Private Sub BuildMatrixForDoublets(Optional ByVal Indexate As Boolean = True)

            If Indexate Then

                Dimension = IndexateLattices()
                MatrixDoublets = New Matrix(Dimension)
                RHS = New Vector(Dimension)

            End If

            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice As Lattice In Lattices

                For Each VortexRing As VortexRing In Lattice.VortexRings

                    Dim Point As Vector3 = VortexRing.ControlPoint
                    Dim Normal As Vector3 = VortexRing.Normal
                    Dim Row As Integer = VortexRing.IndexG

                    For Each OtherLattice As Lattice In Lattices

                        If VortexRing.IsSlender Then

                            ' Impose Neumann boundary conditions

                            Parallel.ForEach(OtherLattice.VortexRings,
                            Sub(OtherVortexRing As VortexRing)

                                Dim Induced As Vector3 = OtherVortexRing.GetDoubletVelocityInfluence(Point, CutOff, False)

                                MatrixDoublets(Row, OtherVortexRing.IndexG) = Induced.X * Normal.X + Induced.Y * Normal.Y + Induced.Z * Normal.Z

                            End Sub)

                        Else

                            ' Impose Dirichlet boundary condition

                            For Each OtherVortexRing In OtherLattice.VortexRings

                                If OtherVortexRing.IndexG = VortexRing.IndexG Then

                                    ' Force autoinfluence to 1/2 (exact solution)

                                    MatrixDoublets(Row, OtherVortexRing.IndexG) = 0.5#

                                Else

                                    MatrixDoublets(Row, OtherVortexRing.IndexG) = OtherVortexRing.GetDoubletPotentialInfluence(Point, False)

                                End If

                            Next

                        End If

                    Next

                Next

            Next

#If DEBUG Then

            'MatrixDoublets.WriteTXT(String.Format("{0}\Matrix_Doublets.txt", Steady_Path))

#End If

        End Sub

        ''' <summary>
        ''' Constructs the matrix of source influence.
        ''' </summary>
        Private Sub BuildMatrixForSources()

            Dim n As Double = 0

            For Each Lattice In Lattices

                For Each Ring In Lattice.VortexRings

                    If Not Ring.IsSlender Then

                        n += 1

                    End If

                Next

            Next

            MatrixSources = New Matrix(MatrixDoublets.RowCount, n)
            S = New Vector(n)

            For Each Lattice As Lattice In Lattices

                For Each VortexRing As VortexRing In Lattice.VortexRings

                    Dim Point As Vector3 = VortexRing.ControlPoint
                    Dim Normal As Vector3 = VortexRing.Normal
                    Dim Row As Integer = VortexRing.IndexG

                    For Each OtherLattice As Lattice In Lattices

                        If VortexRing.IsSlender Then

                            ' Neumman boundary conditions in this panel

                            n = -1

                            For Each OtherVortexRing As VortexRing In OtherLattice.VortexRings

                                If Not OtherVortexRing.IsSlender Then

                                    n += 1

                                    Dim Induced As New Vector3

                                    OtherVortexRing.AddSourceVelocityInfluence(Induced, Point, False)

                                    MatrixSources(Row, n) = Induced.X * Normal.X + Induced.Y * Normal.Y + Induced.Z * Normal.Z

                                End If

                            Next

                        Else

                            ' Dirchlet boundary conditions in this panel

                            n = -1

                            For Each OtherVortexRing As VortexRing In OtherLattice.VortexRings

                                If Not OtherVortexRing.IsSlender Then

                                    n += 1

                                    MatrixSources(Row, n) = OtherVortexRing.GetSourcePotentialInfluence(Point, False)

                                End If

                            Next

                        End If

                    Next

                Next

            Next

        End Sub

        ''' <summary>
        ''' Calculates the right hand side without influence of the wake and without surface motion.
        ''' </summary>
        Private Sub BuildRightHandSide1()

            For Each Lattice As Lattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    If VortexRing.IsSlender Then

                        ' For Neumann boundary conditions:

                        Dim Vx As Double = Stream.Velocity.X
                        Dim Vy As Double = Stream.Velocity.Y
                        Dim Vz As Double = Stream.Velocity.Z

                        If WithStreamRotation Then

                            Vx += Stream.Rotation.Y * VortexRing.ControlPoint.Z - Stream.Rotation.Z * VortexRing.ControlPoint.Y
                            Vy += Stream.Rotation.Z * VortexRing.ControlPoint.X - Stream.Rotation.X * VortexRing.ControlPoint.Z
                            Vz += Stream.Rotation.X * VortexRing.ControlPoint.Y - Stream.Rotation.Y * VortexRing.ControlPoint.X

                        End If

                        RHS(VortexRing.IndexG) = -Vx * VortexRing.Normal.X - Vy * VortexRing.Normal.Y - Vz * VortexRing.Normal.Z

                    Else

                        ' For Dirichlet boundary conditions:

                        RHS(VortexRing.IndexG) = 0

                        For i = 0 To MatrixSources.ColumnCount - 1

                            RHS(VortexRing.IndexG) -= MatrixSources(VortexRing.IndexG, i) * S(i)

                        Next

                    End If

                Next

            Next

        End Sub

        ''' <summary>
        ''' Calculates the right hand side considering the influence of the wake and the surface motion.
        ''' </summary> 
        Private Sub BuildRightHandSide2()

            For Each Lattice As Lattice In Lattices

                Parallel.ForEach(Lattice.VortexRings,
                Sub(VortexRing As VortexRing)

                    If VortexRing.IsSlender Then

                        ' Neumann boundary conditions
                        ' NOTE: missing sumation of sources
                        '-------------------------------------------------------------

                        Dim Vx As Double = VortexRing.VelocityW.X + Stream.Velocity.X
                        Dim Vy As Double = VortexRing.VelocityW.Y + Stream.Velocity.Y
                        Dim Vz As Double = VortexRing.VelocityW.Z + Stream.Velocity.Z

                        If WithStreamRotation Then

                            Vx += Stream.Rotation.Y * VortexRing.ControlPoint.Z - Stream.Rotation.Z * VortexRing.ControlPoint.Y
                            Vy += Stream.Rotation.Z * VortexRing.ControlPoint.X - Stream.Rotation.X * VortexRing.ControlPoint.Z
                            Vz += Stream.Rotation.X * VortexRing.ControlPoint.Y - Stream.Rotation.Y * VortexRing.ControlPoint.X

                        End If

                        RHS(VortexRing.IndexG) = (VortexRing.VelocityS.X - Vx) * VortexRing.Normal.X +
                                                 (VortexRing.VelocityS.Y - Vy) * VortexRing.Normal.Y +
                                                 (VortexRing.VelocityS.Z - Vz) * VortexRing.Normal.Z

                    Else

                        ' Dirichlet boundary conditions
                        ' NOTE: stream rotation already taken into account in vector S
                        '-------------------------------------------------------------

                        RHS(VortexRing.IndexG) = -VortexRing.PotentialW

                        For i = 0 To MatrixSources.ColumnCount - 1

                            RHS(VortexRing.IndexG) -= MatrixSources(VortexRing.IndexG, i) * S(i)

                        Next

                    End If

                End Sub)

            Next

        End Sub

        ''' <summary>
        ''' Sets starting nodes at each wake.
        ''' </summary>
        Private Sub InitializeWakes()

            For Each Lattice As BoundedLattice In Lattices

                Lattice.InitializeWakeVortices()

            Next

        End Sub

        ''' <summary>
        ''' Gives to each vortex ring its corresponding circulation.
        ''' </summary>
        Private Sub AssignDoublets()

            For Each Lattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    VortexRing.DGdt = (G(VortexRing.IndexG) - VortexRing.G) / Settings.Interval

                    VortexRing.G = G(VortexRing.IndexG)

                Next

                For Each Vortex In Lattice.Vortices

                    Vortex.G = 0

                    For i = 0 To 2

                        If Not IsNothing(Vortex.Rings(i)) Then

                            If Vortex.Rings(i).Reversed Then

                                Vortex.G -= Vortex.Sence(i) * Vortex.Rings(i).G

                            Else

                                Vortex.G += Vortex.Sence(i) * Vortex.Rings(i).G

                            End If

                        End If
                    Next

                Next

            Next

        End Sub

        ''' <summary>
        ''' Gives to each non-slender vortex ring its corresponding source intensity based on the current stream velocity.
        ''' </summary>
        Private Sub AssignSources()

            Dim i As Integer = -1

            For Each Lattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    If Not VortexRing.IsSlender Then

                        i += 1

                        ' Remember that the normal points to the outside of the body, therefore the minus sign.

                        Dim Vx As Double = Stream.Velocity.X
                        Dim Vy As Double = Stream.Velocity.Y
                        Dim Vz As Double = Stream.Velocity.Z

                        If WithStreamRotation Then

                            Vx += Stream.Rotation.Y * VortexRing.ControlPoint.Z - Stream.Rotation.Z * VortexRing.ControlPoint.Y
                            Vy += Stream.Rotation.Z * VortexRing.ControlPoint.X - Stream.Rotation.X * VortexRing.ControlPoint.Z
                            Vz += Stream.Rotation.X * VortexRing.ControlPoint.Y - Stream.Rotation.Y * VortexRing.ControlPoint.X

                        End If

                        VortexRing.S = VortexRing.Normal.X * Vx + VortexRing.Normal.Y * Vy + VortexRing.Normal.Z * Vz

                        S(i) = VortexRing.S

                    End If

                Next

            Next

        End Sub

#End Region

#Region " Calculation of velocities "

        ''' <summary>
        ''' Calculates rings VelocityW by adding to the local stream velocity the velocity induced by the wakes.
        ''' </summary>
        Private Sub CalculateVelocityInducedByTheWakesOnBoundedLattices(Optional ByVal SlenderRingsOnly As Boolean = True)

            If Settings.UseGpu Then

                CalculateVelocityInducedByTheWakesOnBoundedLatticesWithOpenCL()

            Else

                Dim CutOff As Double = Settings.Cutoff

                For Each Lattice As BoundedLattice In Lattices

                    Parallel.ForEach(Lattice.VortexRings,
                                     Sub(VortexRing As VortexRing)

                                         If (SlenderRingsOnly And VortexRing.IsSlender) Or Not SlenderRingsOnly Then

                                             VortexRing.VelocityW.X = 0.0#
                                             VortexRing.VelocityW.Y = 0.0#
                                             VortexRing.VelocityW.Z = 0.0#

                                             For Each OtherLattice As BoundedLattice In Lattices

                                                 For Each Wake As Wake In OtherLattice.Wakes

                                                     Wake.AddInducedVelocity(VortexRing.VelocityW, VortexRing.ControlPoint, CutOff)

                                                 Next

                                             Next

                                         End If

                                     End Sub)

                Next

            End If

        End Sub

        ''' <summary>
        ''' Calculates rings PotentialW by adding the influence of the wakes doublets. Only non-slender rings are accepted.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CalculatePotentialInducedByTheWakeOnThickBoundedLattices()

            ' Warning! If this is done, we have to convect wake rings appart from vortices.

            For Each OtherLattice As BoundedLattice In Lattices

                For Each Wake As Wake In OtherLattice.Wakes

                    For Each VortexRing In Wake.VortexRings

                        'This needs to be fixed

                        VortexRing.RecalculateBasis()

                    Next

                Next

            Next

            For Each Lattice As BoundedLattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    VortexRing.PotentialW = 0

                    If Not VortexRing.IsSlender Then

                        For Each OtherLattice As BoundedLattice In Lattices

                            For Each Wake As Wake In OtherLattice.Wakes

                                For Each WakeVortexRing In Wake.VortexRings

                                    VortexRing.PotentialW += WakeVortexRing.GetDoubletPotentialInfluence(VortexRing.ControlPoint, True)

                                Next

                            Next

                        Next

                    End If

                Next

            Next

        End Sub

        ''' <summary>
        ''' Calculates rings VelocityT (total local velocity) by adding the StreamVelocity, VelocityW and the velocity induced by the bounded lattices.
        ''' </summary>
        Private Sub CalculateTotalVelocityOnBoundedLattices()

            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice In Lattices

                For Each Ring In Lattice.VortexRings

                    If Ring.IsSlender Then

                        Ring.VelocityT.X = Stream.Velocity.X
                        Ring.VelocityT.Y = Stream.Velocity.Y
                        Ring.VelocityT.Z = Stream.Velocity.Z

                        If WithStreamRotation Then

                            ' Add stream angular velocity

                            Ring.VelocityT.AddCrossProduct(Stream.Rotation, Ring.ControlPoint)

                        End If

                        Ring.VelocityT.X += Ring.VelocityW.X
                        Ring.VelocityT.Y += Ring.VelocityW.Y
                        Ring.VelocityT.Z += Ring.VelocityW.Z

                        For Each OtherLattice In Lattices

                            ' Use the common control point:

                            OtherLattice.AddInducedVelocity(Ring.VelocityT, Ring.ControlPoint, CutOff)

                        Next

                    Else

                        Dim Velocity As New Vector3(Stream.Velocity)

                        If WithStreamRotation Then

                            ' Add stream angular velocity

                            Velocity.AddCrossProduct(Stream.Rotation, Ring.ControlPoint)

                        End If

                        Ring.CalculateLocalVelocity(Velocity)

                    End If

                Next

            Next

        End Sub

        ''' <summary>
        ''' Calculates the total local velocity at each wake nodal point.
        ''' </summary>
        Private Sub CalculateVelocityOnWakes()

            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice As BoundedLattice In Lattices

                For Each Wake As Wake In Lattice.Wakes

                    Parallel.ForEach(Wake.Nodes,
                    Sub(NodalPoint As Node)

                        NodalPoint.Velocity.Assign(Stream.Velocity)

                        If WithStreamRotation Then

                            NodalPoint.Velocity.AddCrossProduct(Stream.Rotation, NodalPoint.Position)

                        End If

                        For Each OtherLattice As BoundedLattice In Lattices

                            OtherLattice.AddInducedVelocity(NodalPoint.Velocity, NodalPoint.Position, CutOff)

                            For Each OtherWake As Wake In OtherLattice.Wakes

                                OtherWake.AddInducedVelocity(NodalPoint.Velocity, NodalPoint.Position, CutOff)

                            Next

                        Next

                    End Sub)

                Next

            Next

        End Sub

        ''' <summary>
        ''' Returns the total induced velocity at the given point (serial computation).
        ''' </summary>
        Public Function CalculateVelocityAtPoint(ByVal Point As Vector3, ByVal Total As Boolean) As Vector3

            Dim CutOff As Double = Settings.Cutoff
            Dim Velocity As New Vector3

            If Total Then

                Velocity.Assign(Stream.Velocity)

                If WithStreamRotation Then

                    Velocity.AddCrossProduct(Stream.Rotation, Point)

                End If

            End If

            For Each Lattice As BoundedLattice In Lattices

                Lattice.AddInducedVelocity(Velocity, Point, CutOff)

                For Each Wake As Wake In Lattice.Wakes

                    Wake.AddInducedVelocity(Velocity, Point, CutOff)

                Next

            Next

            Return Velocity

        End Function

#End Region

#Region "Cancellation request"

        Public Property CancellationPending As Boolean = False

            Public Sub RequestCancellation()

                CancellationPending = True

            End Sub

            Public Sub CancelProcess()

                RaiseEvent PushMessage("Calculation canceled")
                RaiseEvent CalculationDone()

            End Sub

#End Region

        End Class

End Namespace

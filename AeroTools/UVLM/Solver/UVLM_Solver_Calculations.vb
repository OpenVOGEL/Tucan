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

Imports DotNumerics.LinearAlgebra
Imports System.Threading.Tasks
Imports MathTools.Algebra.EuclideanSpace
Imports MathTools.Extensions
Imports AeroTools.UVLM.Models.Aero
Imports AeroTools.UVLM.Models.Aero.Components

#Const WITH_BOUNDED_VORTICES = False
#Const WITH_PARALLEL_LOOPS = False

Namespace UVLM.Solver

    ''' <summary>
    ''' Provides methods to solve aerodynamic problems through the Unsteady Vortex Lattice Method.
    ''' Programmed by Guillermo A. Hazebrouck
    ''' guillermo.hazebrouck@outlook.be
    ''' </summary> 
    Partial Public Class UVLMSolver

        ' This is the main part containing the UVLM method algorithm.

#Region " General calculations "

        ''' <summary>
        ''' Gives to each vortex its corresponding global index on vectors and matrices and returns the number of bounded vortex rings.
        ''' </summary>
        Private Function IndexateLattices() As Integer

            Dim nIndex As Integer = 0
            Dim eIndex As Integer = 0

            For Each Lattice In Lattices
                For Each Node In Lattice.Nodes
                    Node.IndexG = nIndex
                    nIndex += 1
                Next
                For Each Ring In Lattice.VortexRings
                    Ring.IndexG = eIndex
                    eIndex += 1
                Next
            Next

            Return eIndex

        End Function

        ''' <summary>
        ''' Constructs the matrix of mutual influence coeficients.
        ''' </summary>
        ''' <param name="Indexate">Indicates if nodes should be globaly indexated (this is only required once).
        ''' In that case indexation will occur and matrix and RHS vector will be created.</param>
        Private Sub BuildMatrixForDoublets(Optional ByVal Indexate As Boolean = True)

            If Indexate Then

                Dimension = IndexateLattices()
                MatrixDoublets = New Matrix(Dimension) 'SquareMatrix(Dimension)
                RHS = New Vector(Dimension) 'ColumnVector(Dimension)

            End If

            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice As Lattice In Lattices

                For Each VortexRing As VortexRing In Lattice.VortexRings

                    Dim Point As EVector3 = VortexRing.ControlPoint
                    Dim Normal As EVector3 = VortexRing.Normal
                    Dim Row As Integer = VortexRing.IndexG

                    For Each OtherLattice As Lattice In Lattices

                        If VortexRing.IsSlender Then

                            ' Impose Neumann boundary conditions

                            Parallel.ForEach(OtherLattice.VortexRings, Sub(OtherVortexRing As VortexRing)

                                                                           Dim Induced As EVector3 = OtherVortexRing.GiveDoubletVelocityInfluence(Point, CutOff, False)

                                                                           MatrixDoublets(Row, OtherVortexRing.IndexG) = Induced.X * Normal.X + Induced.Y * Normal.Y + Induced.Z * Normal.Z

                                                                       End Sub)

                        Else

                            ' Impose Dirichlet boundary condition

                            For Each OtherVortexRing In OtherLattice.VortexRings

                                If OtherVortexRing.IndexG = VortexRing.IndexG Then

                                    'principal value:

                                    MatrixDoublets(Row, OtherVortexRing.IndexG) = 0.5#

                                Else

                                    MatrixDoublets(Row, OtherVortexRing.IndexG) = OtherVortexRing.GiveDoubletPotentialInfluence(Point, False)

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

                    Dim Point As EVector3 = VortexRing.ControlPoint
                    Dim Normal As EVector3 = VortexRing.Normal
                    Dim Row As Integer = VortexRing.IndexG

                    For Each OtherLattice As Lattice In Lattices

                        If VortexRing.IsSlender Then

                            n = -1

                            For Each OtherVortexRing As VortexRing In OtherLattice.VortexRings

                                If Not OtherVortexRing.IsSlender Then

                                    n += 1

                                    Dim Induced As New EVector3

                                    OtherVortexRing.AddSourceVelocityInfluence(Point, Induced, False)

                                    MatrixSources(Row, n) = Induced.X * Normal.X + Induced.Y * Normal.Y + Induced.Z * Normal.Z

                                End If

                            Next

                        Else

                            n = -1

                            For Each OtherVortexRing As VortexRing In OtherLattice.VortexRings

                                If Not OtherVortexRing.IsSlender Then

                                    n += 1

                                    MatrixSources(Row, n) = OtherVortexRing.GiveSourcePotentialInfluence(Point, False)

                                End If

                            Next

                        End If

                    Next

                Next

            Next

#If DEBUG Then

            MatrixSources.WriteTXT(String.Format("{0}\Matrix_Sources.txt", Steady_Path))

#End If

        End Sub

        ''' <summary>
        ''' Calculates the right hand side without influence of the wake and without surface motion.
        ''' </summary>
        Private Sub BuildRHS_I()

            Dim Velocity As EVector3 = Settings.StreamVelocity

            For Each Lattice As Lattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    If VortexRing.IsSlender Then

                        ' For Neumann boundary conditions:

                        RHS(VortexRing.IndexG) = -Velocity.X * VortexRing.Normal.X - Velocity.Y * VortexRing.Normal.Y - Velocity.Z * VortexRing.Normal.Z

                    Else

                        ' For Dirichlet boundary conditions:

                        RHS(VortexRing.IndexG) = 0

                        For i = 0 To MatrixSources.ColumnCount - 1

                            RHS(VortexRing.IndexG) += MatrixSources(VortexRing.IndexG, i) * S(i)

                        Next

                    End If

                Next

            Next

        End Sub

        ''' <summary>
        ''' Calculates the right hand side considering the influence of the wake and the surface motion.
        ''' </summary> 
        Private Sub BuildRHS_II(Optional ByVal WithStreamOmega As Boolean = False)

            For Each Lattice As Lattice In Lattices

                Parallel.ForEach(Lattice.VortexRings, Sub(VortexRing As VortexRing)

                                                          If VortexRing.IsSlender Then

                                                              ' Neumann boundary conditions:

                                                              Dim Vx As Double = VortexRing.VelocityW.X + _StreamVelocity.X
                                                              Dim Vy As Double = VortexRing.VelocityW.Y + _StreamVelocity.Y
                                                              Dim Vz As Double = VortexRing.VelocityW.Z + _StreamVelocity.Z

                                                              If WithStreamOmega Then
                                                                  Vx += _StreamOmega.Y * VortexRing.ControlPoint.Z - _StreamOmega.Z * VortexRing.ControlPoint.Y
                                                                  Vy += _StreamOmega.Z * VortexRing.ControlPoint.X - _StreamOmega.X * VortexRing.ControlPoint.Z
                                                                  Vz += _StreamOmega.X * VortexRing.ControlPoint.Y - _StreamOmega.Y * VortexRing.ControlPoint.X
                                                              End If

                                                              RHS(VortexRing.IndexG) = (VortexRing.VelocityS.X - Vx) * VortexRing.Normal.X + _
                                                                                       (VortexRing.VelocityS.Y - Vy) * VortexRing.Normal.Y + _
                                                                                       (VortexRing.VelocityS.Z - Vz) * VortexRing.Normal.Z

                                                          Else

                                                              ' Dirichlet boundary conditions:

                                                              RHS(VortexRing.IndexG) = -VortexRing.PotentialW

                                                              For i = 0 To MatrixSources.ColumnCount - 1

                                                                  RHS(VortexRing.IndexG) += MatrixSources(VortexRing.IndexG, i) * S(i)

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

#If WITH_PARALLEL_LOOPS Then

            Parallel.ForEach(Lattices, Sub(Lattice As BoundedLattice)

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

            End Sub)

#Else

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

#End If

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

                        VortexRing.S = -VortexRing.Normal.X * _StreamVelocity.X - VortexRing.Normal.Y * _StreamVelocity.Y - VortexRing.Normal.Z * _StreamVelocity.Z

                        S(i) = VortexRing.S

                    End If

                Next

            Next

        End Sub

        ''' <summary>
        ''' Checks if there are bounded vortex rings defined as non-slender.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckIfThereAreSources() As Boolean

            For Each Lattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    If Not VortexRing.IsSlender Then

                        Return True

                    End If

                Next

            Next

            Return False

        End Function

#End Region

#Region " Calculation of velocities "

        ''' <summary>
        ''' Calculates rings VelocityW by adding to the local stream velocity the velocity induced by the wakes.
        ''' </summary>
        Private Sub CalculateVelocityInducedByTheWakesOnBoundedLattices(Optional ByVal SlenderRingsOnly As Boolean = True)

            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice As BoundedLattice In Lattices

                Parallel.ForEach(Lattice.VortexRings, Sub(VortexRing As VortexRing)

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

                        VortexRing.RecalculateNormal()

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

                                    VortexRing.PotentialW += WakeVortexRing.GiveDoubletPotentialInfluence(VortexRing.ControlPoint, True)

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
        Private Sub CalculateTotalVelocityOnBoundedLattices(Optional ByVal WithStreamOmega As Boolean = False)

            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice In Lattices

#If WITH_PARALLEL_LOOPS Then

                Parallel.ForEach(Lattice.VortexRings, Sub(Ring As VortexRing)

                    For Each Ring In Lattice.VortexRings

                        Ring.VelocityT.X = _StreamVelocity.X
                        Ring.VelocityT.Y = _StreamVelocity.Y
                        Ring.VelocityT.Z = _StreamVelocity.Z

                        If WithStreamOmega Then

                            Ring.VelocityT.AddCrossProduct(_StreamOmega, Ring.ControlPoint) ' Add stream angular velocity

                        End If

                        Ring.VelocityT.X += Ring.VelocityW.X
                        Ring.VelocityT.Y += Ring.VelocityW.Y
                        Ring.VelocityT.Z += Ring.VelocityW.Z

                        For Each OtherLattice In Lattices

                            If Not Ring.IsSlender Then

                                ' Use the outer control point:

                                OtherLattice.AddInducedVelocity(Ring.VelocityT, Ring.OuterControlPoint, CutOff)

                            Else

                                ' Use the common control point:

                                OtherLattice.AddInducedVelocity(Ring.VelocityT, Ring.ControlPoint, CutOff)

                            End If

                        Next

                    Next

               End Sub)

#Else

                For Each Ring In Lattice.VortexRings

                    Ring.VelocityT.X = _StreamVelocity.X
                    Ring.VelocityT.Y = _StreamVelocity.Y
                    Ring.VelocityT.Z = _StreamVelocity.Z

                    If WithStreamOmega Then

                        Ring.VelocityT.AddCrossProduct(_StreamOmega, Ring.ControlPoint) ' Add stream angular velocity

                    End If

                    Ring.VelocityT.X += Ring.VelocityW.X
                    Ring.VelocityT.Y += Ring.VelocityW.Y
                    Ring.VelocityT.Z += Ring.VelocityW.Z

                    For Each OtherLattice In Lattices

                        If Not Ring.IsSlender Then

                            ' Use the outer control point:

                            OtherLattice.AddInducedVelocity(Ring.VelocityT, Ring.OuterControlPoint, CutOff)

                        Else

                            ' Use the common control point:

                            OtherLattice.AddInducedVelocity(Ring.VelocityT, Ring.ControlPoint, CutOff)

                        End If

                    Next

                Next

#End If

            Next



        End Sub

        ''' <summary>
        ''' Calculates the total local velocity at each wake nodal point.
        ''' </summary>
        Private Sub CalculateVelocityOnWakes(Optional ByVal WithStreamOmega As Boolean = False)

            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice As BoundedLattice In Lattices

                For Each Wake As Wake In Lattice.Wakes

                    Parallel.ForEach(Wake.Nodes, Sub(NodalPoint As Node)

                                                     NodalPoint.Velocity.Assign(_StreamVelocity)

                                                     If WithStreamOmega Then NodalPoint.Velocity.AddCrossProduct(_StreamOmega, NodalPoint.Position)

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
        Public Function CalculateVelocityAtPoint(ByVal Point As EVector3, ByVal Total As Boolean, Optional ByVal WithStreamOmega As Boolean = False) As EVector3

            Dim CutOff As Double = Settings.Cutoff
            Dim Velocity As New EVector3

            If Total Then

                Velocity.Assign(_StreamVelocity)
                If WithStreamOmega Then Velocity.AddCrossProduct(_StreamOmega, Point)

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

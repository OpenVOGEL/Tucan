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
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace CalculationModel.Solver

    Partial Public Class Solver

        'This part constains several methods used to calculate the total airloads

#Region " Total aerodynamic force through modified surface integral "

        ''' <summary>
        ''' Computes the total aerodynamic loads on each bounded lattice
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CalculateAirloads()

            ComputeInducedDrag()

            ComputeForcesAndMoments()

        End Sub

        ''' <summary>
        ''' Computes the induced drag on vortex rings through surface integral (only for slender panels)
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ComputeInducedDrag()

            Dim StreamDirection As New Vector3
            StreamDirection.Assign(Stream.Velocity)
            StreamDirection.Normalize()
            Dim Projection As New Vector3
            Dim V As Double = Stream.Velocity.EuclideanNorm
            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    If Not VortexRing.IsSlender Then Continue For

                    Dim Point As Vector3 = VortexRing.ControlPoint
                    Dim InducedVelocity As New Vector3

                    ' Calculate the total induced velocity at the control point by streamwise segments only:

                    For Each OtherLattice In Lattices

                        For Each OtherRing In OtherLattice.VortexRings

                            If OtherRing.IsSlender Then

                                InducedVelocity.Add(OtherRing.StreamwiseInfluence(Point, 1, 3, CutOff))

                            Else

                                OtherRing.AddDoubletVelocityInfluence(InducedVelocity, Point, CutOff)

                            End If

                        Next

                        For Each Wake In OtherLattice.Wakes

                            For Each Vortex In Wake.Vortices

                                If Vortex.Streamwise Then

                                    Vortex.AddBiotSavartVector(InducedVelocity, Point, CutOff, True)

                                End If

                            Next

                        Next

                    Next

                    ' Take the component of iVelocity in the direction of the projection of the normal vector to the normal plane

                    Dim NormalVelocity As Double = Stream.Velocity.InnerProduct(VortexRing.Normal)

                    Projection.SetToCero()
                    Projection.X = VortexRing.Normal.X - NormalVelocity * StreamDirection.X
                    Projection.Y = VortexRing.Normal.Y - NormalVelocity * StreamDirection.Y
                    Projection.Z = VortexRing.Normal.Z - NormalVelocity * StreamDirection.Z

                    Dim ProjectionNorm As Double = Projection.EuclideanNorm

                    Projection.Normalize()

                    Dim Deflection As Double = Math.Abs(InducedVelocity.InnerProduct(Projection))
                    VortexRing.Cdi = ProjectionNorm * Math.Abs(VortexRing.Cp) * Deflection / V

                Next

            Next

        End Sub

        ''' <summary>
        ''' Computes all forces and moments. If chordwise stripes are defined, parasitic drag is calculated
        ''' based on the local lift and polar curve.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ComputeForcesAndMoments()

            For Each Lattice In Lattices

                Lattice.AirLoads.CL = 0
                Lattice.AirLoads.CDi = 0
                Lattice.AirLoads.CDp = 0
                Lattice.AirLoads.Area = 0

                Lattice.AirLoads.SlenderForce.SetToCero()
                Lattice.AirLoads.InducedDrag.SetToCero()
                Lattice.AirLoads.SkinDrag.SetToCero()
                Lattice.AirLoads.Force.SetToCero()
                Lattice.AirLoads.SlenderMoment.SetToCero()
                Lattice.AirLoads.InducedMoment.SetToCero()
                Lattice.AirLoads.SkinMoment.SetToCero()
                Lattice.AirLoads.BodyForce.SetToCero()
                Lattice.AirLoads.BodyMoment.SetToCero()

                Dim FirstNode As Node = Lattice.Nodes.First

                For Each Ring In Lattice.VortexRings

                    Lattice.AirLoads.Area += Ring.Area

                    If Not Ring.IsSlender Then

                        Dim Cf As Double = -Ring.Cp * Ring.Area

                        Lattice.AirLoads.BodyForce.Add(Ring.Normal, Cf)
                        Lattice.AirLoads.BodyMoment.AddCrossProduct(Ring.ControlPoint, Ring.Normal, Cf)

                        If Settings.IncludeAproximateBodyFriction Then
                            'NOTE:
                            'We estimate the local friction using a flat plate analogy.
                            'This is simplified method has the next restrictions:
                            '> It does not take into account the pressure gradient.
                            '> It assumes everywhere a turbulent layer
                            '> It approaches the reynolds number using a diagonal (which only works for low incidence angle)
                            Dim Direction As New Vector3(Ring.VelocityT)
                            Direction.ProjectOnPlane(Ring.Normal)
                            Dim SurfaceVelocity As Double = Direction.EuclideanNorm
                            Direction.Normalize()
                            Dim Distance As Double = Ring.ControlPoint.DistanceTo(FirstNode.Position)
                            Dim LocalReynolds As Double = SurfaceVelocity * Distance * Settings.Density / Settings.Viscocity
                            Dim Stress As Double = 0.0576 * 0.5 * SurfaceVelocity ^ 2.0 * Settings.Density / Math.Pow(LocalReynolds, 0.2)
                            Cf = Ring.Area * Stress / Settings.DynamicPressure
                            Lattice.AirLoads.SkinDrag.Add(Direction, Cf)
                            Lattice.AirLoads.SkinMoment.AddCrossProduct(Ring.ControlPoint, Direction, Cf)
                        End If

                    End If

                Next

                For Each Stripe In Lattice.ChordWiseStripes

                    Stripe.Compute(Stream.Velocity, Stream.Omega, Settings.Density, Settings.Viscocity)

                    Lattice.AirLoads.SlenderForce.Add(Stripe.L, Stripe.Area)
                    Lattice.AirLoads.InducedDrag.Add(Stripe.Di, Stripe.Area)
                    Lattice.AirLoads.SkinDrag.Add(Stripe.Dp, Stripe.Area)
                    Lattice.AirLoads.SlenderMoment.Add(Stripe.ML, Stripe.Area)
                    Lattice.AirLoads.InducedMoment.Add(Stripe.MDi, Stripe.Area)
                    Lattice.AirLoads.SkinMoment.Add(Stripe.MDp, Stripe.Area)

                Next

                Dim InverseArea = 1 / Lattice.AirLoads.Area

                Lattice.AirLoads.SlenderForce.Scale(InverseArea)
                Lattice.AirLoads.InducedDrag.Scale(InverseArea)
                Lattice.AirLoads.SkinDrag.Scale(InverseArea)
                Lattice.AirLoads.BodyForce.Scale(InverseArea)

                Lattice.AirLoads.SlenderMoment.Scale(InverseArea)
                Lattice.AirLoads.InducedMoment.Scale(InverseArea)
                Lattice.AirLoads.SkinMoment.Scale(InverseArea)
                Lattice.AirLoads.BodyMoment.Scale(InverseArea)

                Lattice.AirLoads.Force.SetToCero()

                Lattice.AirLoads.Force.Add(Lattice.AirLoads.SlenderForce)
                Lattice.AirLoads.Force.Add(Lattice.AirLoads.InducedDrag)
                Lattice.AirLoads.Force.Add(Lattice.AirLoads.SkinDrag)
                Lattice.AirLoads.Force.Add(Lattice.AirLoads.BodyForce)

                Lattice.AirLoads.Moment.SetToCero()

                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.SlenderMoment)
                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.InducedMoment)
                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.SkinMoment)
                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.BodyMoment)

                Lattice.AirLoads.CL = Lattice.AirLoads.SlenderForce.EuclideanNorm
                Lattice.AirLoads.CDi = Lattice.AirLoads.InducedDrag.EuclideanNorm
                Lattice.AirLoads.CDp = Lattice.AirLoads.SkinDrag.EuclideanNorm

            Next

        End Sub

#End Region

    End Class

End Namespace




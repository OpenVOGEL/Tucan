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

        ''' <summary>
        ''' A snapshot on the total airloads (summed from all lattices)
        ''' </summary>
        ''' <returns></returns>
        Public Property GlobalAirloads As New Models.Aero.AirLoads

#Region " Total aerodynamic force "

        ''' <summary>
        ''' Computes the total aerodynamic loads on each bounded lattice.
        ''' The prerequisite is to have calculated the total velocity and the
        ''' pressure coefficients.
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

                    ' Calculate the total induced velocity at the control point
                    '----------------------------------------------------------

                    For Each OtherLattice In Lattices

                        ' Streamwise wing segments:

                        For Each OtherRing In OtherLattice.VortexRings

                            If OtherRing.IsSlender Then

                                InducedVelocity.Add(OtherRing.StreamwiseInfluence(Point, 1, 3, CutOff))

                            Else

                                OtherRing.AddDoubletVelocityInfluence(InducedVelocity, Point, CutOff)

                            End If

                        Next

                        ' Streamwise wake vortex filaments:

                        For Each Wake In OtherLattice.Wakes

                            For Each Vortex In Wake.Vortices

                                If Vortex.Streamwise Then

                                    Vortex.AddInducedVelocity(InducedVelocity, Point, CutOff, True)

                                End If

                            Next

                        Next

                    Next

                    ' Compute the local downwash (the component of the induced velocity in the direction 
                    ' of the projection of the normal vector to the normal plane).
                    '-----------------------------------------------------------------------------------

                    Projection.X = VortexRing.Normal.X * (1.0 - StreamDirection.X)
                    Projection.Y = VortexRing.Normal.Y * (1.0 - StreamDirection.Y)
                    Projection.Z = VortexRing.Normal.Z * (1.0 - StreamDirection.Z)

                    Projection.Normalize()

                    Dim Downwash As Double = Math.Abs(InducedVelocity.InnerProduct(Projection))

                    VortexRing.Cdi = Math.Abs(VortexRing.Cp) * Downwash / V

                Next

            Next

        End Sub

        ''' <summary>
        ''' Computes all forces and moments. If chordwise stripes are defined, parasitic drag is calculated
        ''' based on the local lift and polar curve.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ComputeForcesAndMoments()

            GlobalAirloads.Clear()
            GlobalAirloads.DynamicPressure = Settings.DynamicPressure
            GlobalAirloads.Area = 0.0
            GlobalAirloads.Length = 1.0
            Dim qS As Double = 0.0#

            For Each Lattice In Lattices

                Lattice.AirLoads.DynamicPressure = Settings.DynamicPressure
                Lattice.AirLoads.Area = 0.0
                Lattice.AirLoads.Length = 1.0

                Lattice.AirLoads.LiftForce.SetToCero()
                Lattice.AirLoads.InducedDragForce.SetToCero()
                Lattice.AirLoads.SkinDragForce.SetToCero()
                Lattice.AirLoads.Force.SetToCero()
                Lattice.AirLoads.LiftMoment.SetToCero()
                Lattice.AirLoads.InducedDragMoment.SetToCero()
                Lattice.AirLoads.SkinDragMoment.SetToCero()
                Lattice.AirLoads.BodyForce.SetToCero()
                Lattice.AirLoads.BodyMoment.SetToCero()

                ' Body force
                '-------------------------------------------------------------

                Dim FirstNode As Node = Lattice.Nodes.First

                For Each Ring In Lattice.VortexRings

                    Lattice.AirLoads.Area += Ring.Area

                    If Not Ring.IsSlender Then

                        Dim Cf As Double = -Ring.Cp * Ring.Area

                        Lattice.AirLoads.BodyForce.Add(Ring.Normal, Cf)
                        Lattice.AirLoads.BodyMoment.AddCrossProduct(Ring.ControlPoint, Ring.Normal, Cf)

                        If Settings.IncludeAproximateBodyFriction Then
                            'NOTE:
                            'We estimate the local friction using a flat plate analogy (extremely simplistic approach).
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
                            Lattice.AirLoads.SkinDragForce.Add(Direction, Cf)
                            Lattice.AirLoads.SkinDragMoment.AddCrossProduct(Ring.ControlPoint, Direction, Cf)
                        End If

                    End If

                Next

                Lattice.AirLoads.BodyForce.Scale(Settings.DynamicPressure)
                Lattice.AirLoads.BodyMoment.Scale(Settings.DynamicPressure)

                ' Slender surface forces (from the stripes)
                '-------------------------------------------------------------

                For Each Stripe In Lattice.ChordWiseStripes

                    Stripe.Compute(Stream.Velocity, Stream.Omega, Settings.Density, Settings.Viscocity)

                    Lattice.AirLoads.LiftForce.Add(Stripe.Lift)
                    Lattice.AirLoads.InducedDragForce.Add(Stripe.InducedDrag)
                    Lattice.AirLoads.SkinDragForce.Add(Stripe.SkinDrag)
                    Lattice.AirLoads.LiftMoment.Add(Stripe.LiftMoment)
                    Lattice.AirLoads.InducedDragMoment.Add(Stripe.InducedDragMoment)
                    Lattice.AirLoads.SkinDragMoment.Add(Stripe.SkinDragMoment)

                Next

                ' Traditional coefficients:

                qS = StreamDynamicPressure * Lattice.AirLoads.Area
                Lattice.AirLoads.LiftCoefficient = Lattice.AirLoads.LiftForce.EuclideanNorm / qS
                Lattice.AirLoads.InducedDragCoefficient = Lattice.AirLoads.InducedDragForce.EuclideanNorm / qS
                Lattice.AirLoads.SkinDragCoefficient = Lattice.AirLoads.SkinDragForce.EuclideanNorm / qS

                ' Total force
                '-------------------------------------------------------------

                Lattice.AirLoads.Force.SetToCero()

                Lattice.AirLoads.Force.Add(Lattice.AirLoads.LiftForce)
                Lattice.AirLoads.Force.Add(Lattice.AirLoads.InducedDragForce)
                Lattice.AirLoads.Force.Add(Lattice.AirLoads.SkinDragForce)
                Lattice.AirLoads.Force.Add(Lattice.AirLoads.BodyForce)

                ' Total moment
                '-------------------------------------------------------------

                Lattice.AirLoads.Moment.SetToCero()

                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.LiftMoment)
                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.InducedDragMoment)
                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.SkinDragMoment)
                Lattice.AirLoads.Moment.Add(Lattice.AirLoads.BodyMoment)

                ' Add the contribution of the lattice to the global loads
                '-------------------------------------------------------------

                GlobalAirloads.Area += Lattice.AirLoads.Area
                GlobalAirloads.Add(Lattice.AirLoads)

            Next

            ' Traditional coefficients (total):
            '-----------------------------------------------------------------

            qS = StreamDynamicPressure * GlobalAirloads.Area
            GlobalAirloads.LiftCoefficient = GlobalAirloads.LiftForce.EuclideanNorm / qS
            GlobalAirloads.InducedDragCoefficient = GlobalAirloads.InducedDragForce.EuclideanNorm / qS
            GlobalAirloads.SkinDragCoefficient = GlobalAirloads.SkinDragForce.EuclideanNorm / qS
            GlobalAirloads.Alfa = Math.Asin(StreamVelocity.Z / StreamVelocity.EuclideanNorm)
            GlobalAirloads.Beta = Math.Asin(StreamVelocity.Y / StreamVelocity.EuclideanNorm)

        End Sub

#End Region

    End Class

End Namespace




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

Imports MathTools.Algebra.EuclideanSpace

Namespace UVLM.Solver

    Partial Public Class UVLMSolver

        'This part constains several methods used to calculate the total airloads

#Region " Treftz integral "

        Public Class TrefftzSegment

            Public Point1 As EVector3
            Public Point2 As EVector3
            Public G As Double = 0.0#
            Public Velocity As EVector3
            Public PA As EVector2
            Public PB As EVector2
            Public PC As EVector2

        End Class

        ''' <summary>
        ''' Computes treftz line integral provided a plane cutting the wake
        ''' </summary>
        ''' <param name="Normal"></param>
        ''' <param name="RPoint"></param>
        ''' <remarks></remarks>
        Public Sub ComputeTrefftzIntegral(ByVal Normal As EVector3, ByVal RPoint As EVector3, ByRef TrefftzSegments As List(Of TrefftzSegment), Optional ByVal S As Double = 1.0)

            Dim Basis As New EBase3
            Basis.U.Assign(_StreamVelocity)
            Basis.U.Normalize()
            Basis.V.X = -Basis.U.Y
            Basis.V.Y = Basis.U.X
            Basis.V.Normalize()
            Basis.W.FromVectorProduct(Basis.U, Basis.V)

            Dim V1 As New EVector3
            Dim V2 As New EVector3
            Dim D1 As Double
            Dim D2 As Double
            Dim nA As Integer
            Dim nB As Integer

            Dim Found1 As Boolean = False
            Dim Found2 As Boolean = False

            TrefftzSegments.Clear()

            ' Find the trefftz line and asociated circulation

            For Each Lattice In Lattices

                For Each Wake In Lattice.Wakes

                    For Each Ring In Wake.VortexRings

                        Found1 = False
                        Found2 = False

                        Dim Segment As New TrefftzSegment

                        For i = 1 To 4

                            Select Case i
                                Case 1
                                    nA = 1
                                    nB = 2
                                Case 2
                                    nA = 2
                                    nB = 3
                                Case 3
                                    nA = 3
                                    nB = 4
                                Case 4
                                    nA = 4
                                    nB = 1
                            End Select

                            V1.X = Ring.Node(nA).Position.X - RPoint.X
                            V1.Y = Ring.Node(nA).Position.Y - RPoint.Y
                            V1.Z = Ring.Node(nA).Position.Z - RPoint.Z
                            V2.X = Ring.Node(nB).Position.X - RPoint.X
                            V2.Y = Ring.Node(nB).Position.Y - RPoint.Y
                            V2.Z = Ring.Node(nB).Position.Z - RPoint.Z

                            D1 = Normal.X * V1.X + Normal.Y * V1.Y + Normal.Z * V1.Z
                            D2 = Normal.X * V2.X + Normal.Y * V2.Y + Normal.Z * V2.Z

                            If Math.Sign(D1) <> Math.Sign(D2) Then

                                If Found1 Then Found2 = True

                                Dim mPoint As New EVector3
                                Dim mVelocity As New EVector3
                                Dim Dist As Double = Math.Abs(D1) + Math.Abs(D2)
                                Dim ScaA As Double = (Math.Abs(D2) / Dist)
                                Dim ScaB As Double = (Math.Abs(D1) / Dist)

                                mPoint.X = ScaA * Ring.Node(nA).Position.X + ScaB * Ring.Node(nB).Position.X
                                mPoint.Y = ScaA * Ring.Node(nA).Position.Y + ScaB * Ring.Node(nB).Position.Y
                                mPoint.Z = ScaA * Ring.Node(nA).Position.Z + ScaB * Ring.Node(nB).Position.Z

                                mVelocity.X = ScaA * Ring.Node(nA).Velocity.X + ScaB * Ring.Node(nB).Velocity.X
                                mVelocity.Y = ScaA * Ring.Node(nA).Velocity.Y + ScaB * Ring.Node(nB).Velocity.Y
                                mVelocity.Z = ScaA * Ring.Node(nA).Velocity.Z + ScaB * Ring.Node(nB).Velocity.Z

                                If Not Found1 Then
                                    Segment.Point1 = mPoint
                                    Segment.PA = New EVector2
                                    Segment.PA.X = mPoint.InnerProduct(Basis.V)
                                    Segment.PA.Y = mPoint.InnerProduct(Basis.W)
                                    Segment.Velocity = mVelocity
                                    Segment.G = Ring.G
                                Else
                                    Segment.Point2 = mPoint
                                    Segment.PB = New EVector2
                                    Segment.PB.X = mPoint.InnerProduct(Basis.V)
                                    Segment.PB.Y = mPoint.InnerProduct(Basis.W)
                                    Segment.Velocity.Add(mVelocity)
                                    Segment.Velocity.Scale(0.5)
                                End If

                                Found1 = True

                            End If

                        Next

                        If Found1 And Found2 Then
                            TrefftzSegments.Add(Segment)
                        End If

                    Next

                Next

            Next

            ' Now that vortex stripes have been found, calculate the induced drag as a 2D problem

            Dim nV As Double
            Dim n As New EVector2
            Dim V As New EVector2
            Dim dL As Double
            Dim CDi As Double = 0.0#

            Dim d As New Windows.Forms.SaveFileDialog
            Dim r As Windows.Forms.DialogResult = d.ShowDialog()
            Dim file As String = ""
            Dim write As Boolean = False
            If r = Windows.Forms.DialogResult.OK Then
                file = d.FileName
                write = True
                FileOpen(120, file, OpenMode.Output)
                PrintLine(120, "PA.X, PA.Y, PB.X, PB.Y, Δl, Δφ, dφ/dn")
            End If

            For Each Segment In TrefftzSegments

                ' Calculate PC:

                Segment.PC = New EVector2
                Segment.PC.X = 0.5 * (Segment.PA.X + Segment.PB.X)
                Segment.PC.Y = 0.5 * (Segment.PA.Y + Segment.PB.Y)

                ' Calculate induced velocity on PC:

                V.SetToCero()

                For Each OtherSegment In TrefftzSegments

                    V.Y += 0.5 / Math.PI * OtherSegment.G * (Segment.PC.X - OtherSegment.PA.X) _
                        / (Segment.PC.DistanceTo(OtherSegment.PA)) ^ 2

                    V.X -= 0.5 / Math.PI * OtherSegment.G * (Segment.PC.Y - OtherSegment.PA.Y) _
                        / (Segment.PC.DistanceTo(OtherSegment.PA)) ^ 2

                    V.Y -= 0.5 / Math.PI * OtherSegment.G * (Segment.PC.X - OtherSegment.PB.X) _
                        / (Segment.PC.DistanceTo(OtherSegment.PB)) ^ 2

                    V.X += 0.5 / Math.PI * OtherSegment.G * (Segment.PC.Y - OtherSegment.PB.Y) _
                        / (Segment.PC.DistanceTo(OtherSegment.PB)) ^ 2

                Next

                n.X = Segment.PA.Y - Segment.PB.Y
                n.Y = Segment.PB.X - Segment.PA.X
                n.Normalize()

                nV = V.X * n.X + V.Y * n.Y
                dL = Segment.PA.DistanceTo(Segment.PB)

                CDi += Segment.G * nV * dL

                If write Then
                    PrintLine(120, String.Format("{0,12:F8}, {1,12:F8}, {2,12:F8}, {3,12:F8}, {4,12:F8}, {5,12:F8}, {6,12:F8}", Segment.PA.X, Segment.PA.Y, Segment.PB.X, Segment.PB.Y, dL, Segment.G, nV))
                End If

            Next

            If write Then
                CDi /= Math.Sign(CDi) * _StreamVelocity.SquareEuclideanNorm * S
                PrintLine(120, String.Format("CDi = {0,12:F8}", CDi))
                FileClose(120)
            End If

            'MsgBox(String.Format("Computed S.CDi = {0,12:F8}m²", CDi))

            ComputeInducedDrag()

        End Sub

#End Region

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

            Dim StreamDirection As New EVector3
            StreamDirection.Assign(_StreamVelocity)
            StreamDirection.Normalize()
            Dim Projection As New EVector3
            Dim V As Double = _StreamVelocity.EuclideanNorm
            Dim CutOff As Double = Settings.Cutoff

            For Each Lattice In Lattices

                For Each VortexRing In Lattice.VortexRings

                    If Not VortexRing.IsSlender Then Continue For

                    Dim Point As EVector3 = VortexRing.ControlPoint
                    Dim iVelocity As New EVector3

                    ' Calculate the total induced velocity at the control point by streamwise segments only:

                    For Each OtherLattice In Lattices

                        For Each OtherRing In OtherLattice.VortexRings

                            If OtherRing.IsSlender Then

                                iVelocity.Add(OtherRing.StreamwiseInfluence(Point, 1, 3, CutOff))

                            Else

                                OtherRing.AddDoubletVelocityInfluence(iVelocity, Point, CutOff)

                            End If

                        Next

                        For Each Wake In OtherLattice.Wakes

                            For Each Vortex In Wake.Vortices

                                If Vortex.Streamwise Then

                                    Vortex.AddBiotSavartVector(iVelocity, Point, CutOff, True)

                                End If

                            Next

                        Next

                    Next

                    ' Take the component of iVelocity in the direction of the projection of the normal vector to the normal plane

                    Dim nv As Double = _StreamVelocity.InnerProduct(VortexRing.Normal)

                    Projection.SetToCero()
                    Projection.X = VortexRing.Normal.X - nv * StreamDirection.X
                    Projection.Y = VortexRing.Normal.Y - nv * StreamDirection.Y
                    Projection.Z = VortexRing.Normal.Z - nv * StreamDirection.Z

                    Dim nu As Double = Projection.EuclideanNorm

                    Projection.Normalize()

                    Dim wi As Double = Math.Abs(iVelocity.InnerProduct(Projection))
                    VortexRing.Cdi = nu * Math.Abs(VortexRing.Cp) * wi / V

                Next

            Next

        End Sub

        ''' <summary>
        ''' Computes all forces and moments. If chordwise stripes are defined, parasitic drag is calculated
        ''' based on the local lift and polar curve.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ComputeForcesAndMoments()

            Dim StreamDirection As New EVector3
            StreamDirection.Assign(_StreamVelocity)
            StreamDirection.Normalize()

            Dim V As Double = _StreamVelocity.EuclideanNorm

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

                Dim FreeForce As New EVector3
                Dim StreamForce As New EVector3

                For Each Ring In Lattice.VortexRings

                    Lattice.AirLoads.Area += Ring.Area

                    If Not Ring.IsSlender Then

                        Dim f As Double = -Ring.Area

                        FreeForce.X += f * Ring.Normal.X
                        FreeForce.Y += f * Ring.Normal.Y
                        FreeForce.Z += f * Ring.Normal.Z

                        f *= Ring.Cp

                        StreamForce.X += f * Ring.Normal.X
                        StreamForce.Y += f * Ring.Normal.Y
                        StreamForce.Z += f * Ring.Normal.Z

                    End If

                Next

                Dim FreeForceDirection As New EVector3(FreeForce)

                If FreeForce.EuclideanNorm > 0 Then
                    FreeForceDirection.Normalize()
                End If

                Dim FreePressure As Double = -StreamForce.InnerProduct(FreeForceDirection)

                For Each Ring In Lattice.VortexRings

                    If Not Ring.IsSlender Then

                        Dim f As Double = -(Ring.Cp + FreePressure) * Ring.Area

                        Lattice.AirLoads.BodyForce.X += f * Ring.Normal.X
                        Lattice.AirLoads.BodyForce.Y += f * Ring.Normal.Y
                        Lattice.AirLoads.BodyForce.Z += f * Ring.Normal.Z

                        Lattice.AirLoads.BodyMoment.X += f * (Ring.ControlPoint.Y * Ring.Normal.Z - Ring.ControlPoint.Z * Ring.Normal.Y)
                        Lattice.AirLoads.BodyMoment.Y += f * (Ring.ControlPoint.Z * Ring.Normal.X - Ring.ControlPoint.X * Ring.Normal.Z)
                        Lattice.AirLoads.BodyMoment.Z += f * (Ring.ControlPoint.X * Ring.Normal.Y - Ring.ControlPoint.Y * Ring.Normal.X)

                    End If

                Next

                For Each Stripe In Lattice.ChordWiseStripes

                    Stripe.Compute(StreamDirection, V, Settings.Density, Settings.Viscocity)

                    Lattice.AirLoads.SlenderForce.X += Stripe.Area * Stripe.L.X
                    Lattice.AirLoads.SlenderForce.Y += Stripe.Area * Stripe.L.Y
                    Lattice.AirLoads.SlenderForce.Z += Stripe.Area * Stripe.L.Z

                    Lattice.AirLoads.InducedDrag.Add(Stripe.Di, Stripe.Area)
                    Lattice.AirLoads.SkinDrag.Add(Stripe.Dp, Stripe.Area)
                    Lattice.AirLoads.SlenderMoment.Add(Stripe.ML, Stripe.Area)
                    Lattice.AirLoads.InducedMoment.Add(Stripe.MDi, Stripe.Area)
                    Lattice.AirLoads.SkinMoment.Add(Stripe.MDp, Stripe.Area)

                Next

                Dim Sm1 = 1 / Lattice.AirLoads.Area

                Lattice.AirLoads.SlenderForce.Scale(Sm1)
                Lattice.AirLoads.InducedDrag.Scale(Sm1)
                Lattice.AirLoads.SkinDrag.Scale(Sm1)
                Lattice.AirLoads.BodyForce.Scale(Sm1)

                Lattice.AirLoads.SlenderMoment.Scale(Sm1)
                Lattice.AirLoads.InducedMoment.Scale(Sm1)
                Lattice.AirLoads.SkinMoment.Scale(Sm1)
                Lattice.AirLoads.BodyMoment.Scale(Sm1)

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




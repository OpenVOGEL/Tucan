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

Imports System.IO
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace CalculationModel.Models.Aero.Components

    ''' <summary>
    ''' Represents a vortex ring stripe where lift and drag can be locally computed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ChorwiseStripe

        ''' <summary>
        ''' Chordwise stripe of vortex rings (from the leading edge to the trailing edge).
        ''' </summary>
        ''' <remarks></remarks>
        Public Rings As List(Of VortexRing)

        ''' <summary>
        ''' Polar curve used to compute the local drag.
        ''' </summary>
        ''' <remarks></remarks>
        Public Polars As PolarFamily

        Public Sub New()
            Rings = New List(Of VortexRing)
        End Sub

        ''' <summary>
        ''' Returns the area of this portion of wing in m².
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Area As Double

        ''' <summary>
        ''' Local lift coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property LiftCoefficient As Double

        ''' <summary>
        ''' Local induced drag coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InducedDragCoefficient As Double

        ''' <summary>
        ''' Skin drag coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SkinDragCoefficient As Double

        ''' <summary>
        ''' Total stripe lift in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Lift As New Vector3

        ''' <summary>
        ''' Total stripe induced drag in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InducedDrag As New Vector3

        ''' <summary>
        ''' Total stripe induced drag in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SkinDrag As New Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property LiftMoment As New Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InducedDragMoment As New Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SkinDragMoment As New Vector3

        ''' <summary>
        ''' Stripe chord in meters.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Chord As Double

        ''' <summary>
        ''' Vector having the direction of the chord.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ChordWiseVector As New Vector3

        ''' <summary>
        ''' Point located at the geometric center of the chordwise stripe.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Coordinates are always in meters.</remarks>
        Public ReadOnly Property CenterPoint As New Vector3

        ''' <summary>
        ''' Calculates the stripe lift, drag and area. 
        ''' The Cp (pressure coefficient) and the Cdi (local induced component) of each panel should be calculated before calling this sub.
        ''' NOTE:
        ''' This method corrects the missing leading edge pressure decay by substracting the projection of the total force in 
        ''' the direction of the local stream velocity (this is why the stream direction is requested). This is an extention of 
        ''' the 2D potential theory in which 2D airfoils do not introduce drag. This is arguable, but at least the correction 
        ''' does provide more consistent results, specially in the case of rotating wings where the incidence varies considerably.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Compute(StreamVelocity As Vector3,
                           StreamRotation As Vector3,
                           Density As Double,
                           Viscosity As Double)

            ' Calculate local chordwise direction and chord:

            Dim n As Integer = Rings.Count - 1

            _ChordWiseVector.X = 0.5 * ((Rings(n).Node(2).Position.X + Rings(n).Node(3).Position.X) - (Rings(0).Node(1).Position.X + Rings(0).Node(4).Position.X))
            _ChordWiseVector.Y = 0.5 * ((Rings(n).Node(2).Position.Y + Rings(n).Node(3).Position.Y) - (Rings(0).Node(1).Position.Y + Rings(0).Node(4).Position.Y))
            _ChordWiseVector.Z = 0.5 * ((Rings(n).Node(2).Position.Z + Rings(n).Node(3).Position.Z) - (Rings(0).Node(1).Position.Z + Rings(0).Node(4).Position.Z))

            _CenterPoint.X = 0.5 * (Rings(0).Node(1).Position.X + Rings(0).Node(4).Position.X + _ChordWiseVector.X)
            _CenterPoint.Y = 0.5 * (Rings(0).Node(1).Position.Y + Rings(0).Node(4).Position.Y + _ChordWiseVector.Y)
            _CenterPoint.Z = 0.5 * (Rings(0).Node(1).Position.Z + Rings(0).Node(4).Position.Z + _ChordWiseVector.Z)

            _Chord = _ChordWiseVector.EuclideanNorm
            _ChordWiseVector.Normalize()

            Dim DynamicPressure As Double = 0.5 * StreamVelocity.SquareEuclideanNorm * Density
            Dim StreamDirection As New Vector3(StreamVelocity)
            StreamDirection.AddCrossProduct(StreamRotation, _CenterPoint)
            StreamDirection.Normalize()

            _Area = 0.0#
            _LiftCoefficient = 0.0#
            _SkinDragCoefficient = 0.0#
            _InducedDragCoefficient = 0.0#

            Dim Projection As Double
            Dim Force As Double

            Dim LocalLift = New Vector3
            Dim LocalInducedDrag = New Vector3
            Dim qS As Double = 0.0#

            _Lift.SetToCero()
            _InducedDrag.SetToCero()
            _SkinDrag.SetToCero()
            _LiftMoment.SetToCero()
            _InducedDragMoment.SetToCero()
            _SkinDragMoment.SetToCero()

            ' Sum contributions to lift and induced drag:

            For Each VortexRing In Rings

                _Area += VortexRing.Area

                qS = VortexRing.Area * DynamicPressure

                ' Lift
                '-------------------------------------------------------

                Force = VortexRing.Cp * qS
                Projection = VortexRing.Normal.InnerProduct(StreamDirection)

                LocalLift.X = Force * (VortexRing.Normal.X - Projection * StreamDirection.X)
                LocalLift.Y = Force * (VortexRing.Normal.Y - Projection * StreamDirection.Y)
                LocalLift.Z = Force * (VortexRing.Normal.Z - Projection * StreamDirection.Z)

                _Lift.X += LocalLift.X
                _Lift.Y += LocalLift.Y
                _Lift.Z += LocalLift.Z

                _LiftMoment.AddCrossProduct(VortexRing.ControlPoint, LocalLift)

                ' Induced drag 
                '-------------------------------------------------------

                Dim qS_Cdi As Double = qS * VortexRing.Cdi
                LocalInducedDrag.X = qS_Cdi * StreamDirection.X
                LocalInducedDrag.Y = qS_Cdi * StreamDirection.Y
                LocalInducedDrag.Z = qS_Cdi * StreamDirection.Z

                _InducedDrag.X += LocalInducedDrag.X
                _InducedDrag.Y += LocalInducedDrag.Y
                _InducedDrag.Z += LocalInducedDrag.Z

                _InducedDragMoment.AddCrossProduct(VortexRing.ControlPoint, LocalInducedDrag)

            Next

            qS = _Area * DynamicPressure

            _LiftCoefficient = _Lift.EuclideanNorm / qS
            _InducedDragCoefficient = InducedDrag.EuclideanNorm / qS

            ' Skin drag from polar curve using lift coefficient and the Reynolds number 
            ' If there is no polar curve it stays zero.
            '--------------------------------------------------------------------------

            Dim Reynolds As Double = StreamVelocity.EuclideanNorm * Density * Chord / Viscosity

            If Not IsNothing(Polars) Then
                _SkinDragCoefficient = Polars.SkinDrag(LiftCoefficient, Reynolds)
            End If

            _SkinDrag.X = _SkinDragCoefficient * ChordWiseVector.X * qS
            _SkinDrag.Y = _SkinDragCoefficient * ChordWiseVector.Y * qS
            _SkinDrag.Z = _SkinDragCoefficient * ChordWiseVector.Z * qS

            _SkinDragMoment.FromVectorProduct(CenterPoint, SkinDrag)

        End Sub

        ''' <summary>
        ''' Reads the chordwise link data from a binary stream.
        ''' </summary>
        ''' <param name="Reader"></param>
        ''' <param name="Rings"></param>
        ''' <param name="PolarData"></param>
        Sub ReadBinary(ByRef Reader As BinaryReader, ByRef Rings As List(Of VortexRing), ByRef PolarData As PolarDatabase)
            Try
                ' Rings
                '-----------------------------------
                For i = 1 To Reader.ReadInt32
                    Me.Rings.Add(Rings(Reader.ReadInt32))
                Next

                ' Polar id
                '-----------------------------------
                Dim PolarId = New Guid(Reader.ReadString())
                Polars = PolarData.GetFamilyFromID(PolarId)

                ' Geometric properties
                '-----------------------------------

                _Chord = Reader.ReadDouble
                _Area = Reader.ReadDouble

                _ChordWiseVector.X = Reader.ReadDouble
                _ChordWiseVector.Y = Reader.ReadDouble
                _ChordWiseVector.Z = Reader.ReadDouble

                _CenterPoint.X = Reader.ReadDouble
                _CenterPoint.Y = Reader.ReadDouble
                _CenterPoint.Z = Reader.ReadDouble

                ' Forces and moments
                '-----------------------------------

                _Lift.X = Reader.ReadDouble
                _Lift.Y = Reader.ReadDouble
                _Lift.Z = Reader.ReadDouble
                _LiftCoefficient = Reader.ReadDouble

                _InducedDrag.X = Reader.ReadDouble
                _InducedDrag.Y = Reader.ReadDouble
                _InducedDrag.Z = Reader.ReadDouble
                _InducedDragCoefficient = Reader.ReadDouble

                _SkinDrag.X = Reader.ReadDouble
                _SkinDrag.Y = Reader.ReadDouble
                _SkinDrag.Z = Reader.ReadDouble
                _SkinDragCoefficient = Reader.ReadDouble

                _LiftMoment.X = Reader.ReadDouble
                _LiftMoment.Y = Reader.ReadDouble
                _LiftMoment.Z = Reader.ReadDouble

                _InducedDragMoment.X = Reader.ReadDouble
                _InducedDragMoment.Y = Reader.ReadDouble
                _InducedDragMoment.Z = Reader.ReadDouble

                _SkinDragMoment.X = Reader.ReadDouble
                _SkinDragMoment.Y = Reader.ReadDouble
                _SkinDragMoment.Z = Reader.ReadDouble

            Catch ex As Exception
                Me.Rings.Clear()
            End Try

        End Sub

        ''' <summary>
        ''' Writes the chordwise link data to a binary stream.
        ''' </summary>
        ''' <param name="Writer"></param>
        Sub WriteBinary(ByRef Writer As BinaryWriter)

            ' Rings
            '-----------------------------------
            Writer.Write(Rings.Count)

            For Each Ring In Rings
                Writer.Write(Ring.IndexL)
            Next

            ' Polar id
            '-----------------------------------
            If IsNothing(Polars) Then
                Writer.Write(Guid.Empty.ToString)
            Else
                Writer.Write(Polars.ID.ToString)
            End If

            ' Geometric properties
            '-----------------------------------

            Writer.Write(Chord)
            Writer.Write(Area)

            Writer.Write(ChordWiseVector.X)
            Writer.Write(ChordWiseVector.Y)
            Writer.Write(ChordWiseVector.Z)

            Writer.Write(CenterPoint.X)
            Writer.Write(CenterPoint.Y)
            Writer.Write(CenterPoint.Z)

            ' Forces and moments
            '-----------------------------------

            Writer.Write(Lift.X)
            Writer.Write(Lift.Y)
            Writer.Write(Lift.Z)
            Writer.Write(LiftCoefficient)

            Writer.Write(InducedDrag.X)
            Writer.Write(InducedDrag.Y)
            Writer.Write(InducedDrag.Z)
            Writer.Write(InducedDragCoefficient)

            Writer.Write(SkinDrag.X)
            Writer.Write(SkinDrag.Y)
            Writer.Write(SkinDrag.Z)
            Writer.Write(SkinDragCoefficient)

            Writer.Write(LiftMoment.X)
            Writer.Write(LiftMoment.Y)
            Writer.Write(LiftMoment.Z)

            Writer.Write(InducedDragMoment.X)
            Writer.Write(InducedDragMoment.Y)
            Writer.Write(InducedDragMoment.Z)

            Writer.Write(SkinDragMoment.X)
            Writer.Write(SkinDragMoment.Y)
            Writer.Write(SkinDragMoment.Z)

        End Sub

    End Class

End Namespace
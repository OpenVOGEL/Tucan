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

        Private _Area As Double

        ''' <summary>
        ''' Returns the area of this portion of wing in m².
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Area As Double
            Get
                Return _Area
            End Get
        End Property

        Private _LiftCoefficient As Double

        ''' <summary>
        ''' Local lift coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property LiftCoefficient As Double
            Get
                Return _LiftCoefficient
            End Get
        End Property

        Private _InducedDragCoefficient As Double

        ''' <summary>
        ''' Local induced drag coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InducedDragCoefficient As Double
            Get
                Return _InducedDragCoefficient
            End Get
        End Property

        Private _SkinDragCoefficient As Double

        ''' <summary>
        ''' Skin drag coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SkinDragCoefficient As Double
            Get
                Return _SkinDragCoefficient
            End Get
        End Property

        Public _Lift As New Vector3

        ''' <summary>
        ''' Total stripe lift in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Lift As Vector3
            Get
                Return _Lift
            End Get
        End Property

        Public _InducedDrag As New Vector3

        ''' <summary>
        ''' Total stripe induced drag in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InducedDrag As Vector3
            Get
                Return _InducedDrag
            End Get
        End Property

        Public _SkinDrag As New Vector3

        ''' <summary>
        ''' Total stripe induced drag in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SkinDrag As Vector3
            Get
                Return _SkinDrag
            End Get
        End Property

        Public _LiftMoment As New Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property LiftMoment As Vector3
            Get
                Return _LiftMoment
            End Get
        End Property

        Public _InducedDragMoment As New Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property InducedDragMoment As Vector3
            Get
                Return _InducedDragMoment
            End Get
        End Property

        Public _SkinDragMoment As New Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property SkinDragMoment As Vector3
            Get
                Return _SkinDragMoment
            End Get
        End Property

        Private _Chord As Double

        ''' <summary>
        ''' Stripe chord in meters.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Chord As Double
            Get
                Return _Chord
            End Get
        End Property

        Private _ChordWiseVector As New Vector3

        ''' <summary>
        ''' Vector having the direction of the chord.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ChordWiseVector
            Get
                Return _ChordWiseVector
            End Get
        End Property

        Private _CenterPoint As New Vector3

        ''' <summary>
        ''' Point located at the geometric center of the chordwise stripe.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>Coordinates are always in meters.</remarks>
        Public ReadOnly Property CenterPoint As Vector3
            Get
                Return _CenterPoint
            End Get
        End Property

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
        ''' <param name="r"></param>
        ''' <param name="Rings"></param>
        ''' <param name="PolarDB"></param>
        Sub ReadBinary(ByRef r As BinaryReader, ByRef Rings As List(Of VortexRing), ByRef PolarDB As PolarDatabase)
            Try
                ' Rings
                '-----------------------------------
                For i = 1 To r.ReadInt32
                    Me.Rings.Add(Rings(r.ReadInt32))
                Next

                ' Polar id
                '-----------------------------------
                Dim polarID = New Guid(r.ReadString())
                Polars = PolarDB.GetFamilyFromID(polarID)

                ' Forces
                '-----------------------------------

            Catch ex As Exception
                Me.Rings.Clear()
            End Try
        End Sub

        ''' <summary>
        ''' Writes the chordwise link data to a binary stream.
        ''' </summary>
        ''' <param name="w"></param>
        Sub WriteBinary(ByRef w As BinaryWriter)

            ' Rings
            '-----------------------------------
            w.Write(Rings.Count)

            For Each Ring In Rings
                w.Write(Ring.IndexL)
            Next

            ' Polar id
            '-----------------------------------
            If IsNothing(Polars) Then
                w.Write(Guid.Empty.ToString)
            Else
                w.Write(Polars.ID.ToString)
            End If

            ' Forces
            '-----------------------------------

            'w.Write(CenterPoint.X)
            'w.Write(CenterPoint.Y)
            'w.Write(CenterPoint.Z)
            '
            'w.Write(Lift.X)
            'w.Write(Lift.Y)
            'w.Write(Lift.Z)
            'w.Write(LiftCoefficient)
            '
            'w.Write(InducedDrag.X)
            'w.Write(InducedDrag.Y)
            'w.Write(InducedDrag.Z)
            'w.Write(InducedDragCoefficient)
            '
            'w.Write(SkinDrag.X)
            'w.Write(SkinDrag.Y)
            'w.Write(SkinDrag.Z)
            'w.Write(SkinDragCoefficient)
            '
            'w.Write(LiftMoment.X)
            'w.Write(LiftMoment.Y)
            'w.Write(LiftMoment.Z)
            '
            'w.Write(InducedDragMoment.X)
            'w.Write(InducedDragMoment.Y)
            'w.Write(InducedDragMoment.Z)
            '
            'w.Write(SkinDragMoment.X)
            'w.Write(SkinDragMoment.Y)
            'w.Write(SkinDragMoment.Z)

        End Sub

    End Class

End Namespace
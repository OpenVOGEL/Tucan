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

        Private _CL As Double

        ''' <summary>
        ''' Local lift coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property CL As Double
            Get
                Return _CL
            End Get
        End Property

        Private _CDi As Double

        ''' <summary>
        ''' Local induced drag coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property CDi As Double
            Get
                Return _CDi
            End Get
        End Property

        Private _CDp As Double

        ''' <summary>
        ''' Skin drag coefficient in this portion of wing.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property CDp As Double
            Get
                Return _CDp
            End Get
        End Property

        Public _L As Vector3

        ''' <summary>
        ''' Total stripe lift in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property L As Vector3
            Get
                Return _L
            End Get
        End Property

        Public _Di As Vector3

        ''' <summary>
        ''' Total stripe induced drag in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Di As Vector3
            Get
                Return _Di
            End Get
        End Property

        Public _Dp As Vector3

        ''' <summary>
        ''' Total stripe induced drag in N.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property Dp As Vector3
            Get
                Return _Dp
            End Get
        End Property

        Public _ML As Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property ML As Vector3
            Get
                Return _ML
            End Get
        End Property

        Public _MDi As Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property MDi As Vector3
            Get
                Return _MDi
            End Get
        End Property

        Public _MDp As Vector3

        ''' <summary>
        ''' Total stripe moment (with respect to the origin) in N.m.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property MDp As Vector3
            Get
                Return _MDp
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
        ''' The Cp (pressure coefficient) and the Cdi (local induced component) should be calculated before calling this sub.
        ''' NOTE:
        ''' This method corrects the missing leading edge pressure decay by substracting the projection of the total force in 
        ''' the direction of the local stream velocity (this is why the stream direction is requested). This is an extention of 
        ''' the 2D potential theory in which 2D airfoils do not introduce drag. This is arguable, but at least the correction 
        ''' does provide more consistent results, specially in the case of rotating wings where the incidence varies considerably.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Compute(Velocity As Vector3, Omega As Vector3, Rho As Double, Mu As Double)

            ' Calculate local chordwise direction and chord:

            Dim n As Integer = Rings.Count - 1

            _ChordWiseVector.X = 0.5 * ((Rings(n).Node(1).Position.X - Rings(0).Node(2).Position.X) + (Rings(n).Node(4).Position.X - Rings(0).Node(3).Position.X))
            _ChordWiseVector.Y = 0.5 * ((Rings(n).Node(1).Position.Y - Rings(0).Node(2).Position.Y) + (Rings(n).Node(4).Position.Y - Rings(0).Node(3).Position.Y))
            _ChordWiseVector.Z = 0.5 * ((Rings(n).Node(1).Position.Z - Rings(0).Node(2).Position.Z) + (Rings(n).Node(4).Position.Z - Rings(0).Node(3).Position.Z))

            _CenterPoint.X = 0.5 * (Rings(0).Node(1).Position.X + Rings(0).Node(4).Position.X) + _ChordWiseVector.X
            _CenterPoint.Y = 0.5 * (Rings(0).Node(1).Position.Y + Rings(0).Node(4).Position.Y) + _ChordWiseVector.Y
            _CenterPoint.Z = 0.5 * (Rings(0).Node(1).Position.Z + Rings(0).Node(4).Position.Z) + _ChordWiseVector.Z

            _Chord = _ChordWiseVector.EuclideanNorm
            _ChordWiseVector.Normalize()

            Dim StreamDirection As New Vector3(Velocity)
            StreamDirection.AddCrossProduct(Omega, _CenterPoint)
            StreamDirection.Normalize()

            _CL = 0.0#
            _Area = 0.0#
            _CDp = 0
            _CDi = 0

            Dim Projection As Double
            Dim Force As Double
            Dim InducedDrag As Double = 0

            Dim LocalL = New Vector3
            Dim LocalDi = New Vector3
            Dim LocalML As New Vector3
            Dim LocalMDi As New Vector3
            Dim LocalMDp As New Vector3

            _L = New Vector3
            _Di = New Vector3
            _Dp = New Vector3
            _ML = New Vector3
            _MDi = New Vector3
            _MDp = New Vector3

            ' Sum contributions to CL and CDi:

            For Each VortexRing In Rings

                _Area += VortexRing.Area
                InducedDrag += VortexRing.Cdi * VortexRing.Area

                Force = VortexRing.Cp * VortexRing.Area
                Projection = VortexRing.Normal.InnerProduct(StreamDirection)

                LocalL.X = Force * (VortexRing.Normal.X - Projection * StreamDirection.X)
                LocalL.Y = Force * (VortexRing.Normal.Y - Projection * StreamDirection.Y)
                LocalL.Z = Force * (VortexRing.Normal.Z - Projection * StreamDirection.Z)

                LocalDi.X = VortexRing.Cdi * VortexRing.Area * StreamDirection.X
                LocalDi.Y = VortexRing.Cdi * VortexRing.Area * StreamDirection.Y
                LocalDi.Z = VortexRing.Cdi * VortexRing.Area * StreamDirection.Z

                LocalML.FromVectorProduct(VortexRing.ControlPoint, LocalL)
                LocalMDi.FromVectorProduct(VortexRing.ControlPoint, LocalDi)

                _ML.Add(LocalML)
                _MDi.Add(LocalMDi)

                _L.X += LocalL.X
                _L.Y += LocalL.Y
                _L.Z += LocalL.Z

                _Di.X += LocalDi.X
                _Di.Y += LocalDi.Y
                _Di.Z += LocalDi.Z

            Next

            _L.X /= _Area
            _L.Y /= _Area
            _L.Z /= _Area

            _Di.X /= _Area
            _Di.Y /= _Area
            _Di.Z /= _Area

            _ML.X /= _Area
            _ML.Y /= _Area
            _ML.Z /= _Area

            _MDi.X /= _Area
            _MDi.Y /= _Area
            _MDi.Z /= _Area

            _MDp.X /= _Area
            _MDp.Y /= _Area
            _MDp.Z /= _Area

            _CL = _L.EuclideanNorm
            _CDi = InducedDrag / _Area

            ' Calculate _CDp from CL (if there is a polar curve):

            Dim Re As Double = Velocity.EuclideanNorm * Rho * Chord / Mu

            If Not IsNothing(Polars) Then
                _CDp = Polars.SkinDrag(CL, Re)
            End If

            _Dp.X = _CDp * ChordWiseVector.X
            _Dp.Y = _CDp * ChordWiseVector.Y
            _Dp.Z = _CDp * ChordWiseVector.Z

            LocalMDp.FromVectorProduct(CenterPoint, Dp)
            _MDp.Add(LocalMDi)

        End Sub

        ''' <summary>
        ''' Reads the chordwise link data from a binary stream.
        ''' </summary>
        ''' <param name="r"></param>
        ''' <param name="Rings"></param>
        ''' <param name="PolarDB"></param>
        Sub ReadBinary(ByRef r As BinaryReader, ByRef Rings As List(Of VortexRing), ByRef PolarDB As PolarDatabase)
            Try
                For i = 1 To r.ReadInt32
                    Me.Rings.Add(Rings(r.ReadInt32))
                Next
                Dim polarID = New Guid(r.ReadString())
                Polars = PolarDB.GetFamilyFromID(polarID)
            Catch ex As Exception
                Me.Rings.Clear()
            End Try
        End Sub

        ''' <summary>
        ''' Writes the chordwise link data to a binary stream.
        ''' </summary>
        ''' <param name="w"></param>
        Sub WriteBinary(ByRef w As BinaryWriter)

            w.Write(Rings.Count)

            For Each Ring In Rings
                w.Write(Ring.IndexL)
            Next

            If IsNothing(Polars) Then
                w.Write(Guid.Empty.ToString)
            Else
                w.Write(Polars.ID.ToString)
            End If

        End Sub

    End Class

End Namespace
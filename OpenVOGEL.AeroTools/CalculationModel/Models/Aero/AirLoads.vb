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

Namespace CalculationModel.Models.Aero

    ''' <summary>
    ''' Gathers the resultants of aerodynamic forces per type. 
    ''' All forces are normalized by normalized by qS (using the properties Area and DynamicPressure).
    ''' </summary>
    ''' <remarks></remarks>
    Public Class AirLoads

        ' Related data
        '------------------------------------------

        ''' <summary>
        ''' The reference area (the default value is 1.0) [m²].
        ''' </summary>
        ''' <returns></returns>
        Public Property Area As Double = 1.0

        ''' <summary>
        ''' The reference length (the default value is 1.0) [m].
        ''' </summary>
        ''' <returns></returns>
        Public Property Length As Double = 1.0

        ''' <summary>
        ''' The reference dynamic pressure [Pa].
        ''' </summary>
        ''' <returns></returns>
        Public Property DynamicPressure As Double = 0.0

        ' Total loads
        '------------------------------------------

        ''' <summary>
        ''' The force.
        ''' </summary>
        Public Property Force As New Vector3

        ''' <summary>
        ''' The moment about the origin.
        ''' </summary>
        Public Property Moment As New Vector3

        ' Force and moment components classified by their nature
        ' (Units are shown in international system)
        '-------------------------------------------------------

        ''' <summary>
        ''' The lift force from the slender panels [N]
        ''' </summary>
        Public Property LiftForce As New Vector3

        ''' <summary>
        ''' The lift moment about the origin from the slender panels [Nm]
        ''' </summary>
        Public Property LiftMoment As New Vector3

        ''' <summary>
        ''' The induced drag force from the slender panels [N]
        ''' </summary>
        Public Property InducedDragForce As New Vector3

        ''' <summary>
        ''' The induced drag moment about the origin from the slender panels [Nm]
        ''' </summary>
        Public Property InducedDragMoment As New Vector3

        ''' <summary>
        ''' The skin drag force from the slender panels extracted from the polar 
        ''' curves using the lift coefficient and the local Reynolds number [N].
        ''' There is an option to include here the body skin drag using a very 
        ''' simplistic model (under development).
        ''' </summary>
        Public Property SkinDragForce As New Vector3

        ''' <summary>
        ''' The skin drag moment about the origin from the slender panels [Nm]
        ''' </summary>
        Public Property SkinDragMoment As New Vector3

        ''' <summary>
        ''' The force from the closed surface panels [Nm]
        ''' </summary>
        Public Property BodyForce As New Vector3

        ''' <summary>
        ''' The body moment about the origin from the closed surface panels [Nm]
        ''' </summary>
        Public Property BodyMoment As New Vector3

        ' Classic dimensionless force components
        '------------------------------------------

        ''' <summary>
        ''' The lift coefficient.
        ''' </summary>
        Public Property LiftCoefficient As Double

        ''' <summary>
        ''' The induced drag coefficient.
        ''' </summary>
        Public Property InducedDragCoefficient As Double

        ''' <summary>
        ''' The skin drag coefficient.
        ''' </summary>
        Public Property SkinDragCoefficient As Double

        ''' <summary>
        ''' The normal incidence angle
        ''' </summary>
        ''' <returns></returns>
        Public Property Alfa As Double

        ''' <summary>
        ''' The lateral incidence angle
        ''' </summary>
        ''' <returns></returns>
        Public Property Beta As Double

        ''' <summary>
        ''' Adds the given load scaled by the surface.
        ''' </summary>
        ''' <param name="Airloads"></param>
        Public Sub Add(Airloads As AirLoads)

            Force.Add(Airloads.Force)
            Moment.Add(Airloads.Moment)

            LiftForce.Add(Airloads.LiftForce)
            LiftMoment.Add(Airloads.LiftMoment)

            InducedDragForce.Add(Airloads.InducedDragForce)
            InducedDragMoment.Add(Airloads.InducedDragMoment)

            SkinDragForce.Add(Airloads.SkinDragForce)
            SkinDragMoment.Add(Airloads.SkinDragMoment)

            BodyForce.Add(Airloads.BodyForce)
            BodyMoment.Add(Airloads.BodyMoment)

        End Sub

        ''' <summary>
        ''' Clears the airloads to zero.
        ''' </summary>
        Public Sub Clear()

            Force.SetToCero()
            Moment.SetToCero()

            LiftCoefficient = 0.0
            InducedDragCoefficient = 0.0
            SkinDragCoefficient = 0.0

            LiftForce.SetToCero()
            LiftMoment.SetToCero()

            InducedDragForce.SetToCero()
            InducedDragMoment.SetToCero()

            SkinDragForce.SetToCero()
            SkinDragMoment.SetToCero()

            BodyForce.SetToCero()
            BodyMoment.SetToCero()

        End Sub

        Public Sub Transform(Base As Base3)

            Force.Transform(Base)
            Moment.Transform(Base)

            LiftForce.Transform(Base)
            LiftMoment.Transform(Base)

            InducedDragForce.Transform(Base)
            InducedDragMoment.Transform(Base)

            SkinDragForce.Transform(Base)
            SkinDragMoment.Transform(Base)

            BodyForce.Transform(Base)
            BodyMoment.Transform(Base)

        End Sub

        Public Sub AntiTransform(Base As Base3)

            Force.AntiTransform(Base)
            Moment.AntiTransform(Base)

            LiftForce.AntiTransform(Base)
            LiftMoment.AntiTransform(Base)

            InducedDragForce.AntiTransform(Base)
            InducedDragMoment.AntiTransform(Base)

            SkinDragForce.AntiTransform(Base)
            SkinDragMoment.AntiTransform(Base)

            BodyForce.AntiTransform(Base)
            BodyMoment.AntiTransform(Base)

        End Sub

        ''' <summary>
        ''' Writes the airloads data to a binary stream.
        ''' </summary>
        ''' <param name="w"></param>
        Public Sub WriteBinary(ByRef w As BinaryWriter)

            w.Write(Force.X)
            w.Write(Force.Y)
            w.Write(Force.Z)

            w.Write(Moment.X)
            w.Write(Moment.Y)
            w.Write(Moment.Z)

            w.Write(LiftCoefficient)
            w.Write(InducedDragCoefficient)
            w.Write(SkinDragCoefficient)

            w.Write(LiftForce.X)
            w.Write(LiftForce.Y)
            w.Write(LiftForce.Z)

            w.Write(LiftMoment.X)
            w.Write(LiftMoment.Y)
            w.Write(LiftMoment.Z)

            w.Write(InducedDragForce.X)
            w.Write(InducedDragForce.Y)
            w.Write(InducedDragForce.Z)

            w.Write(InducedDragMoment.X)
            w.Write(InducedDragMoment.Y)
            w.Write(InducedDragMoment.Z)

            w.Write(SkinDragForce.X)
            w.Write(SkinDragForce.Y)
            w.Write(SkinDragForce.Z)

            w.Write(SkinDragMoment.X)
            w.Write(SkinDragMoment.Y)
            w.Write(SkinDragMoment.Z)

            w.Write(BodyForce.X)
            w.Write(BodyForce.Y)
            w.Write(BodyForce.Z)

            w.Write(BodyMoment.X)
            w.Write(BodyMoment.Y)
            w.Write(BodyMoment.Z)

            w.Write(Area)
            w.Write(Length)
            w.Write(DynamicPressure)
            w.Write(Alfa)
            w.Write(Beta)

        End Sub

        ''' <summary>
        ''' Reads the airloads data from a binary stream.
        ''' </summary>
        ''' <param name="w"></param>
        Public Sub ReadBinary(ByRef r As System.IO.BinaryReader)

            Force.X = r.ReadDouble
            Force.Y = r.ReadDouble
            Force.Z = r.ReadDouble

            Moment.X = r.ReadDouble
            Moment.Y = r.ReadDouble
            Moment.Z = r.ReadDouble

            LiftCoefficient = r.ReadDouble
            InducedDragCoefficient = r.ReadDouble
            SkinDragCoefficient = r.ReadDouble

            LiftForce.X = r.ReadDouble
            LiftForce.Y = r.ReadDouble
            LiftForce.Z = r.ReadDouble

            LiftMoment.X = r.ReadDouble
            LiftMoment.Y = r.ReadDouble
            LiftMoment.Z = r.ReadDouble

            InducedDragForce.X = r.ReadDouble
            InducedDragForce.Y = r.ReadDouble
            InducedDragForce.Z = r.ReadDouble

            InducedDragMoment.X = r.ReadDouble
            InducedDragMoment.Y = r.ReadDouble
            InducedDragMoment.Z = r.ReadDouble

            SkinDragForce.X = r.ReadDouble
            SkinDragForce.Y = r.ReadDouble
            SkinDragForce.Z = r.ReadDouble

            SkinDragMoment.X = r.ReadDouble
            SkinDragMoment.Y = r.ReadDouble
            SkinDragMoment.Z = r.ReadDouble

            BodyForce.X = r.ReadDouble
            BodyForce.Y = r.ReadDouble
            BodyForce.Z = r.ReadDouble

            BodyMoment.X = r.ReadDouble
            BodyMoment.Y = r.ReadDouble
            BodyMoment.Z = r.ReadDouble

            Area = r.ReadDouble
            Length = r.ReadDouble
            DynamicPressure = r.ReadDouble
            Alfa = r.ReadDouble
            Beta = r.ReadDouble

        End Sub

    End Class

End Namespace
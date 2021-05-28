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

'' Standard .NET dependencies
'-----------------------------------------------------------------------------
Imports System.IO

'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components

'#############################################################################
' Unit: BoundedLattice
'
' This unit provides a declaration of a bounded lattice.
' Bounded lattices are lattices that represent solid boundaries and can 
' contain edges from where wakes are shed.
'#############################################################################
Namespace CalculationModel.Models.Aero

    ''' <summary>
    ''' Represents a lattice bounded to a solid surface where several wakes might be convected.
    ''' </summary>
    Public Class BoundedLattice

        Inherits Lattice

        Public Wakes As New List(Of Wake)

        Public Sub New()
            Wakes = New List(Of Wake)
        End Sub

        ''' <summary>
        ''' This sub repositions the primitive points at the wake. It is used on the aeroelastic analysis.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReEstablishWakes()

            For Each Wake In Wakes

                Dim StartingIndex As Integer = Wake.Nodes.Count - Wake.Primitive.Nodes.Count  ' < Last local index

                For Each PrimNode In Wake.Primitive.Nodes

                    Wake.Nodes(StartingIndex).Position.Assign(Nodes(PrimNode).Position)

                    StartingIndex += 1

                Next

            Next

        End Sub

        ''' <summary>
        ''' Reasigns velocity at each control point.
        ''' </summary>
        Public Overloads Overrides Sub ClearVelocity(ByVal Velocity As Vector3)

            For Each VortexRing In VortexRings
                VortexRing.VelocityW.Assign(Velocity)
            Next

        End Sub

#Region " Rings on wakes "

        Public Sub InitializeWakeRings()

            For Each Wake As Wake In Wakes

                For Each PrimNode In Wake.Primitive.Nodes

                    Wake.AddNode(Nodes(PrimNode).Position)

                Next

            Next

        End Sub

        ''' <summary>
        ''' Convect all wakes on the primiteve edges of the bound lattice and adds new nodes and panels at primitive positions.
        ''' If RemoveOldest is marked as true, oldest wake panels will be removed.
        ''' </summary>
        Public Sub PopulateWakeRings(Dt As Double,
                                     TimeStep As Integer,
                                     ExtendWakes As Boolean,
                                     Extension As Vector3)

            Dim N1 As Integer
            Dim N2 As Integer
            Dim N3 As Integer
            Dim N4 As Integer

            For Each Wake As Wake In Wakes

                Wake.Convect(Dt)

                ' Add new nodes:

                Dim nn As Integer = Wake.Nodes.Count
                Dim n As Integer = Wake.Primitive.Nodes.Count
                Dim p As Integer = Wake.Primitive.Rings.Count
                Dim LocalIndex As Integer = Wake.Nodes(Wake.Nodes.Count - 1).IndexL ' < Last local index

                For Each PrimNode In Wake.Primitive.Nodes

                    LocalIndex += 1
                    Wake.AddNode(Nodes(PrimNode).Position, LocalIndex)

                    ' Index has been corrected to follow consecutive order. 
                    ' If the wake has been cut, index wont start from 0. Nevertheless it will always follow a consecutive order.

                Next

                ' Add new vortex rings:

                nn = Wake.Nodes.Count
                N4 = nn - 2 * n
                N3 = nn - n
                N2 = N3 + 1
                N1 = N4 + 1

                ' N4 ## N1 < convected nodes
                ' #     #
                ' #     #
                ' N3 ## N2 < primitive edge >

                For i = 0 To p - 1

                    ' Add wake ring i:

                    Wake.AddVortexRing4(N1 + i, N2 + i, N3 + i, N4 + i, VortexRings(Wake.Primitive.Rings(i)).G)

                Next

                ' Remove old rings, nodes and vortices

                If TimeStep > Wake.CuttingStep Then

                    Wake.VortexRings.RemoveRange(0, p)
                    Wake.Nodes.RemoveRange(0, n)

                    ' Extend the trailing part of the wake if requested

                    If ExtendWakes AndAlso TimeStep > Wake.CuttingStep + 5 Then

                        Wake.Extend(Extension)

                    End If

                End If

            Next

        End Sub

        ''' <summary>
        ''' Convect all wakes on the primiteve edges of the bound lattice and adds new nodes and panels at primitive positions.
        ''' If RemoveOldest is marked as true, oldest wake panels will be removed.
        ''' </summary>
        Public Sub PopulateWakeRingsAndVortices(Dt As Double,
                                                TimeStep As Integer,
                                                ExtendWakes As Boolean,
                                                Extension As Vector3)

            Dim N1 As Integer
            Dim N2 As Integer
            Dim N3 As Integer
            Dim N4 As Integer

            For Each Wake As Wake In Wakes

                Wake.Convect(Dt)

                ' Add new nodes:

                Dim nn As Integer = Wake.Nodes.Count
                Dim nv As Integer = Wake.Vortices.Count
                Dim n As Integer = Wake.Primitive.Nodes.Count
                Dim p As Integer = Wake.Primitive.Rings.Count
                Dim LocalIndex As Integer = Wake.Nodes(nn - 1).IndexL ' < Last local index

                ' Add nodes:

                For i = 0 To n - 1

                    LocalIndex += 1
                    Wake.AddNode(Nodes(Wake.Primitive.Nodes(i)).Position, LocalIndex)

                Next

                ' Add streamwise vortices:

                For i = 0 To n - 1

                    Dim StreamWiseVortex As New Vortex

                    StreamWiseVortex.Node1 = Wake.Nodes(nn - n + i)
                    StreamWiseVortex.Node2 = Wake.Nodes(nn + i)
                    StreamWiseVortex.Streamwise = True
                    Wake.Vortices.Add(StreamWiseVortex)

                Next

                ' Add transverse vortices:

                For i = 1 To n - 1

                    Dim TransverseVortex As New Vortex

                    TransverseVortex.Node1 = Wake.Nodes(nn + i - 1)
                    TransverseVortex.Node2 = Wake.Nodes(nn + i)

                    Wake.Vortices.Add(TransverseVortex)

                Next

                ' Assign circulation

                For i = 0 To p - 1

                    Dim G As Double = VortexRings(Wake.Primitive.Rings(i)).G

                    Wake.Vortices(nv - p + i).G += G
                    Wake.Vortices(nv + i).G -= G
                    Wake.Vortices(nv + i + 1).G += G
                    Wake.Vortices(nv + n + i).G -= G

                Next

                ' If TameInnerCirculation Then Wake.Vortices(nv).G = 0

                ' Remove old vortices and nodes

                If TimeStep > Wake.CuttingStep Then

                    Wake.Nodes.RemoveRange(0, n)
                    Wake.Vortices.RemoveRange(0, n + p)

                End If

                ' Add new vortex rings:

                nn = Wake.Nodes.Count
                N4 = nn - 2 * n
                N3 = nn - n
                N2 = N3 + 1
                N1 = N4 + 1

                ' N4 ## N1 < convected nodes
                ' #     #
                ' #     #
                ' N3 ## N2 < primitive edge >

                Dim nr As Integer = Wake.VortexRings.Count

                For i = 0 To p - 1

                    ' Add wake ring i:

                    Wake.AddVortexRing4(N1 + i, N2 + i, N3 + i, N4 + i, VortexRings(Wake.Primitive.Rings(i)).G)

                Next

                'If TameInnerCirculation Then Wake.VortexRings(nr).G = 0.0#

                ' Remove old rings, nodes and vortices

                If TimeStep > Wake.CuttingStep Then

                    Wake.VortexRings.RemoveRange(0, p)

                    ' Extend the trailing part of the wake if requested

                    If ExtendWakes AndAlso TimeStep > Wake.CuttingStep + 5 Then

                        Wake.Extend(Extension)

                    End If

                End If

            Next

        End Sub

#End Region

#Region " Vortices on wakes "

        Public Sub InitializeWakeVortices()

            For Each Wake As Wake In Wakes

                For Each PrimNode In Wake.Primitive.Nodes

                    Wake.AddNode(Nodes(PrimNode).Position)

                Next

                For i = 1 To Wake.Primitive.Nodes.Count - 1

                    Dim Vortex As New Vortex
                    Vortex.Node1 = Wake.Nodes(i - 1)
                    Vortex.Node2 = Wake.Nodes(i)
                    Vortex.G = 0.0#
                    Wake.Vortices.Add(Vortex)

                Next

            Next

        End Sub

        ''' <summary>
        ''' Convect all wakes on the primiteve edges of the bound lattice and adds new nodes and panels at primitive positions.
        ''' </summary>
        Public Sub PopulateWakeVortices(Dt As Double,
                                        TimeStep As Integer,
                                        ExtendWakes As Boolean,
                                        Extension As Vector3)

            For Each Wake As Wake In Wakes

                Wake.Convect(Dt)

                ' Add new nodes:

                Dim nn As Integer = Wake.Nodes.Count
                Dim nv As Integer = Wake.Vortices.Count
                Dim n As Integer = Wake.Primitive.Nodes.Count
                Dim p As Integer = Wake.Primitive.Rings.Count
                Dim LocalIndex As Integer = Wake.Nodes(nn - 1).IndexL ' < Last local index

                ' Add nodes:

                For i = 0 To n - 1

                    LocalIndex += 1
                    Wake.AddNode(Nodes(Wake.Primitive.Nodes(i)).Position, LocalIndex)

                Next

                ' Add streamwise vortices:

                For i = 0 To n - 1

                    Dim StreamWiseVortex As New Vortex

                    StreamWiseVortex.Node1 = Wake.Nodes(nn - n + i)
                    StreamWiseVortex.Node2 = Wake.Nodes(nn + i)
                    StreamWiseVortex.Streamwise = True
                    Wake.Vortices.Add(StreamWiseVortex)

                Next

                ' Add transverse vortices:

                For i = 1 To n - 1

                    Dim TransverseVortex As New Vortex

                    TransverseVortex.Node1 = Wake.Nodes(nn + i - 1)
                    TransverseVortex.Node2 = Wake.Nodes(nn + i)
                    Wake.Vortices.Add(TransverseVortex)

                Next

                ' Assign circulation

                For i = 0 To p - 1

                    Dim G As Double = VortexRings(Wake.Primitive.Rings(i)).G

                    Wake.Vortices(nv - p + i).G += G
                    Wake.Vortices(nv + i).G -= G
                    Wake.Vortices(nv + i + 1).G += G
                    Wake.Vortices(nv + n + i).G -= G

                Next

                ' Do this when there is a fuselage to avoid wake rollup next to the fuselage:

                If Wake.SupressInnerCircuation Then Wake.Vortices(nv).G = 0

                ' Remove old vortices and nodes

                If TimeStep > Wake.CuttingStep Then

                    Wake.Nodes.RemoveRange(0, n)
                    Wake.Vortices.RemoveRange(0, n + p)

                    ' Extend the trailing part of the wake if requested

                    If ExtendWakes AndAlso TimeStep > Wake.CuttingStep + 5 Then

                        Wake.Extend(Extension)

                    End If

                End If

            Next

        End Sub

        ''' <summary>
        ''' Stores the current position of the wake for later use.
        ''' </summary>
        Public Sub CacheWakePosition()

            For Each Wake As Wake In Wakes
                Wake.CachePosition
            Next

        End Sub

        ''' <summary>
        ''' Reconvects the nodes having an original position.
        ''' </summary>
        Public Sub ReconvectWakes(Dt)

            For Each Wake As Wake In Wakes
                Wake.ReConvect(Dt)
            Next

        End Sub

#End Region

#Region " Loads "

        ''' <summary>
        ''' Links to chordwise ring stripes used to calculate skin drag and local CL
        ''' </summary>
        ''' <remarks></remarks>
        Public ChordWiseStripes As New List(Of ChorwiseStripe)

        ''' <summary>
        ''' Calculates pressure coeficient at each bounded vortex ring.
        ''' </summary>
        ''' <param name="SquareVelocity">Square of reference velocity used to adimensionalize pressure</param>
        Public Sub CalculatePressure(ByVal SquareVelocity As Double)

            'Parallel.ForEach(VortexRings, Sub(Ring As VortexRing)

            '                                  Ring.CalculateCP(SquareVelocity)

            '                              End Sub)

            For Each Ring In VortexRings

                Ring.CalculateCp(SquareVelocity)

            Next

        End Sub

#End Region

#Region " IO "

        Public Overrides Sub ReadBinary(ByRef r As System.IO.BinaryReader)

            MyBase.ReadBinary(r)

            For i = 0 To VortexRings.Count - 1
                VortexRings(i).Cp = r.ReadDouble
                VortexRings(i).DGdt = r.ReadDouble
                VortexRings(i).VelocityT.X = r.ReadDouble
                VortexRings(i).VelocityT.Y = r.ReadDouble
                VortexRings(i).VelocityT.Z = r.ReadDouble
                VortexRings(i).Cdi = r.ReadDouble
            Next

        End Sub

        Public Overrides Sub WriteBinary(ByRef w As System.IO.BinaryWriter, Optional ByVal NodalVelocity As Boolean = False, Optional ByVal ReferencePosition As Boolean = False)

            MyBase.WriteBinary(w, NodalVelocity, True)

            ' Write vortex rings properties after geometry:

            For i = 0 To VortexRings.Count - 1
                w.Write(VortexRings(i).Cp)
                w.Write(VortexRings(i).DGdt)
                w.Write(VortexRings(i).VelocityT.X)
                w.Write(VortexRings(i).VelocityT.Y)
                w.Write(VortexRings(i).VelocityT.Z)
                w.Write(VortexRings(i).Cdi)
            Next

        End Sub

        Public Overloads Sub WriteBinary(ByVal FilePath As String, Optional ByVal WakesNodalVelocity As Boolean = False)

            Dim w As New BinaryWriter(New FileStream(FilePath, FileMode.Create))

            ' Base
            '--------------------------

            Me.WriteBinary(w, True)

            ' Wakes
            '--------------------------

            w.Write(Wakes.Count)

            For i = 0 To Wakes.Count - 1
                Wakes(i).WriteBinary(w, WakesNodalVelocity)
            Next

            ' Chordwise links
            '--------------------------

            w.Write(ChordWiseStripes.Count)

            For i = 0 To ChordWiseStripes.Count - 1
                ChordWiseStripes(i).WriteBinary(w)
            Next

            ' Air-loads
            '--------------------------

            AirLoads.WriteBinary(w)

            w.Close()

        End Sub

        Public Overloads Sub ReadBinary(ByVal FilePath As String, Optional ByVal Polars As PolarDatabase = Nothing, Optional ByVal WithWakes As Boolean = True)

            If File.Exists(FilePath) Then

                Dim r As New BinaryReader(New FileStream(FilePath, FileMode.Open))

                ' Base
                '--------------------------

                Me.ReadBinary(r)

                CalculateLatticeParameters()

                ' Wakes
                '--------------------------

                Wakes.Clear()

                If WithWakes Then

                    For i = 0 To r.ReadInt32 - 1
                        Wakes.Add(New Wake)
                        Wakes(i).ReadBinary(r)
                    Next

                End If

                ' Chordwise links
                '--------------------------

                Try
                    For i = 0 To r.ReadInt32 - 1
                        ChordWiseStripes.Add(New ChorwiseStripe)
                        ChordWiseStripes(i).ReadBinary(r, VortexRings, Polars)
                    Next
                Catch
                    ' No links were found
                    ChordWiseStripes.Clear()
                End Try

                ' Air-loads
                '--------------------------

                AirLoads.ReadBinary(r)

                r.Close()

            End If

        End Sub

#End Region

    End Class

End Namespace

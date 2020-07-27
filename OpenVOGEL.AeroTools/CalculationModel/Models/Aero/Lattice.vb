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
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components

Namespace CalculationModel.Models.Aero

    ''' <summary>
    ''' Base clas for modelling lattices.
    ''' </summary>
    Public Class Lattice

        Public Name As String
        Public Nodes As New List(Of Node)
        Public VortexRings As New List(Of VortexRing)
        Public Vortices As New List(Of Vortex)

        ''' <summary>
        ''' Adds a node to the lattice with an specific local index
        ''' </summary>
        ''' <param name="Position"></param>
        Public Overloads Sub AddNode(ByVal Position As Vector3, ByVal LocalIndex As Integer)
            Dim NodeToAdd As New Node(LocalIndex, Position) ' < Solucionar! si la estela se corta el numero cambia! (controlar si esta bien)
            Nodes.Add(NodeToAdd)
        End Sub

        ''' <summary>
        ''' Adds a node to the lattice with a local index equal to the current number of nodes
        ''' </summary>
        ''' <param name="Position"></param>
        Public Overloads Sub AddNode(ByVal Position As Vector3)
            Dim NodeToAdd As New Node(Nodes.Count, Position) ' < Solucionar! si la estela se corta el numero cambia! (controlar si esta bien)
            Nodes.Add(NodeToAdd)
        End Sub

        ''' <summary>
        ''' Adds a vortex ring to the lattice
        ''' </summary>
        ''' <param name="N1">Local index of node 3</param>
        ''' <param name="N2">Local index of node 3</param>
        ''' <param name="N3">Local index of node 3</param>
        Public Sub AddVortexRing3(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal Reversed As Boolean, ByVal Slender As Boolean)
            Dim VortexRingToAdd As New VortexRing3(Nodes(N1), Nodes(N2), Nodes(N3), VortexRings.Count, Reversed, Slender)
            VortexRings.Add(VortexRingToAdd)
        End Sub

        ''' <summary>
        ''' Adds a vortex ring to the lattice
        ''' </summary>
        ''' <param name="N1">Local index of node 3</param>
        ''' <param name="N2">Local index of node 3</param>
        ''' <param name="N3">Local index of node 3</param>
        ''' <param name="N4">Local index of node 3</param>
        Public Sub AddVortexRing4(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal N4 As Integer, ByVal Reversed As Boolean, ByVal Slender As Boolean)
            Dim VortexRingToAdd As New VortexRing4(Nodes(N1), Nodes(N2), Nodes(N3), Nodes(N4), VortexRings.Count, Reversed, Slender)
            VortexRings.Add(VortexRingToAdd)
        End Sub
        ''' <summary>
        ''' Reasigns velocity at each nodal point.
        ''' </summary>
        Public Overridable Overloads Sub ClearVelocity(ByVal Velocity As Vector3)

            For Each Node In Nodes

                Node.Velocity.Assign(Velocity)

            Next

        End Sub

        ''' <summary>
        ''' Computed velocity induced by this lattice at Point.
        ''' </summary>
        ''' <param name="Point"></param>
        ''' <remarks></remarks>
        Public Sub AddInducedVelocity(ByRef Velocity As Vector3, ByVal Point As Vector3, ByVal CutOff As Double)

            ' Add velocity associated to doublets (using the associated vortices)

            For Each Vortex In Vortices

                Vortex.AddInducedVelocity(Velocity, Point, CutOff, True)

            Next

            ' Add velocity associated with sources

            For Each Ring In VortexRings

                If Not Ring.IsSlender Then

                    Ring.AddSourceVelocityInfluence(Velocity, Point)

                End If

            Next

        End Sub

        ''' <summary>
        ''' Finds all vortices associated to this lattice.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub PopulateVortices()

            Vortices.Clear()

            ' Scan each of the four vortices and, if they have not been added, add them to the list:

            Dim Order4(3, 1) As Integer

            Order4(0, 0) = 1
            Order4(0, 1) = 2

            Order4(1, 0) = 2
            Order4(1, 1) = 3

            Order4(2, 0) = 3
            Order4(2, 1) = 4

            Order4(3, 0) = 4
            Order4(3, 1) = 1

            Dim Order3(2, 1) As Integer

            Order3(0, 0) = 1
            Order3(0, 1) = 2

            Order3(1, 0) = 2
            Order3(1, 1) = 3

            Order3(2, 0) = 3
            Order3(2, 1) = 1

            Dim mi As Integer
            Dim mj As Integer

            Dim ni As Integer
            Dim nj As Integer

            For Each Ring In VortexRings

                Select Ring.Type

                    Case VortexRingType.VR4

                        For m = 0 To 3 ' For each local segment:

                            mi = Ring.Node(Order4(m, 0)).IndexL
                            mj = Ring.Node(Order4(m, 1)).IndexL

                            Dim Foud As Boolean = False

                            For Each Vortex In Vortices

                                ni = Vortex.Node1.IndexL
                                nj = Vortex.Node2.IndexL

                                ' If this segments already exist in the stack, there must be an adjacent ring to add:

                                If (mi = ni And mj = nj) Then
                                    For i = 0 To 2
                                        If IsNothing(Vortex.Rings(i)) Then
                                            Vortex.Rings(i) = Ring
                                            Vortex.Sence(i) = 1
                                            Foud = True
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                ElseIf (mi = nj And mj = ni) Then
                                    For i = 0 To 2
                                        If IsNothing(Vortex.Rings(i)) Then
                                            Vortex.Rings(i) = Ring
                                            Vortex.Sence(i) = -1
                                            Foud = True
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If

                            Next

                            If Not Foud Then

                                ' If it came up to here, the segment is new.

                                Dim Vortex As New Vortex
                                Vortex.Node1 = Nodes(mi)
                                Vortex.Node2 = Nodes(mj)
                                Vortex.Rings(0) = Ring
                                Vortex.Sence(0) = 1
                                Vortices.Add(Vortex)

                            End If

                        Next

                    Case VortexRingType.VR3

                        For m = 0 To 2 ' For each local segment:

                            mi = Ring.Node(Order3(m, 0)).IndexL
                            mj = Ring.Node(Order3(m, 1)).IndexL

                            Dim Foud As Boolean = False

                            For Each Vortex In Vortices

                                ni = Vortex.Node1.IndexL
                                nj = Vortex.Node2.IndexL

                                ' If this segments already exist in the stack, there must be an adjacent ring to add:

                                If (mi = ni And mj = nj) Then
                                    For i = 0 To 2
                                        If IsNothing(Vortex.Rings(i)) Then
                                            Vortex.Rings(i) = Ring
                                            Vortex.Sence(i) = 1
                                            Foud = True
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                ElseIf (mi = nj And mj = ni) Then
                                    For i = 0 To 2
                                        If IsNothing(Vortex.Rings(i)) Then
                                            Vortex.Rings(i) = Ring
                                            Vortex.Sence(i) = -1
                                            Foud = True
                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If

                            Next

                            If Not Foud Then

                                ' If it came up to here, the segment is new.

                                Dim Vortex As New Vortex
                                Vortex.Node1 = Nodes(mi)
                                Vortex.Node2 = Nodes(mj)
                                Vortex.Rings(0) = Ring
                                Vortex.Sence(0) = 1
                                Vortices.Add(Vortex)

                            End If

                        Next

                End Select

            Next

        End Sub

#Region " Forces and moments "

        ''' <summary>
        ''' The lattice airloads
        ''' </summary>
        ''' <returns></returns>
        Public Property AirLoads As New AirLoads

#End Region

#Region " Geometric operations "

        ''' <summary>
        ''' Calculate vortex rings geometric parameters (control point, normal vector)
        ''' </summary>
        Public Sub CalculateLatticeParameters()

            For Each Ring In VortexRings
                Ring.CalculateGeometricEntities()
            Next

        End Sub

        ''' <summary>
        ''' Rotates all nodal poins as specified in Orientation about the origin {0,0,0}
        ''' </summary>
        ''' <param name="Orientation">Euler angles providing the orientation</param>
        ''' <remarks></remarks>
        Public Sub Rotate(ByVal Orientation As EulerAngles)

            Dim m As New MathTools.Algebra.CustomMatrices.RotationMatrix
            m.Generate(Orientation)

            For Each Node In Nodes

                Node.Position.Rotate(m)

            Next

        End Sub

        ''' <summary>
        ''' Rotates all nodal points as specified in Orientation about an specific point.
        ''' </summary>
        ''' <param name="Orientation"></param>
        ''' <param name="Point"></param>
        ''' <remarks></remarks>
        Public Sub RotateAbout(ByVal Orientation As EulerAngles, ByVal Point As Vector3)

            Dim m As New MathTools.Algebra.CustomMatrices.RotationMatrix
            m.Generate(Orientation)

            For Each Node In Nodes

                Node.Position.Substract(Point)
                Node.Position.Rotate(m)
                Node.Position.Add(Point)

            Next

        End Sub

#End Region

#Region "I/O"

        Public Overridable Sub WriteBinary(ByRef w As BinaryWriter, Optional ByVal NodalVelocity As Boolean = False, Optional ByVal ReferencePosition As Boolean = False)

            w.Write(NodalVelocity)
            w.Write(ReferencePosition)

            Dim lb As Integer = Nodes(0).IndexL ' Write the lower bound

            w.Write(lb)

            w.Write(Nodes.Count)

            For i = 0 To Nodes.Count - 1

                w.Write(Nodes(i).Position.X)
                w.Write(Nodes(i).Position.Y)
                w.Write(Nodes(i).Position.Z)

                If NodalVelocity Then

                    w.Write(Nodes(i).Velocity.X)
                    w.Write(Nodes(i).Velocity.Y)
                    w.Write(Nodes(i).Velocity.Z)

                End If

                If ReferencePosition Then

                    If Nodes(i).OriginalPosition Is Nothing Then
                        w.Write(Nodes(i).Position.X)
                        w.Write(Nodes(i).Position.Y)
                        w.Write(Nodes(i).Position.Z)
                    Else
                        w.Write(Nodes(i).OriginalPosition.X)
                        w.Write(Nodes(i).OriginalPosition.Y)
                        w.Write(Nodes(i).OriginalPosition.Z)
                    End If

                    If Nodes(i).Displacement Is Nothing Then
                        w.Write(0.0#)
                        w.Write(0.0#)
                        w.Write(0.0#)
                    Else
                        w.Write(Nodes(i).Displacement.X)
                        w.Write(Nodes(i).Displacement.Y)
                        w.Write(Nodes(i).Displacement.Z)
                    End If

                End If

            Next

            w.Write(VortexRings.Count)

            Dim IndexL As Integer = 0

            For i = 0 To VortexRings.Count - 1

                w.Write(VortexRings(i).Type)
                w.Write(VortexRings(i).IsSlender)
                w.Write(VortexRings(i).Reversed)

                IndexL = VortexRings(i).Node(1).IndexL - lb  ' < Indices have been corrected in case wake has been cut
                w.Write(IndexL)

                IndexL = VortexRings(i).Node(2).IndexL - lb
                w.Write(IndexL)

                IndexL = VortexRings(i).Node(3).IndexL - lb
                w.Write(IndexL)

                If VortexRings(i).Type = VortexRingType.VR4 Then

                    IndexL = VortexRings(i).Node(4).IndexL - lb
                    w.Write(IndexL)

                End If

                w.Write(VortexRings(i).G)

            Next

            w.Write(Vortices.Count)

            For i = 0 To Vortices.Count - 1

                w.Write(Vortices(i).Node1.IndexL)
                w.Write(Vortices(i).Node2.IndexL)
                w.Write(Vortices(i).G)

                If (IsNothing(Vortices(i).Rings(0))) Then
                    w.Write(-1)
                Else
                    w.Write(Vortices(i).Rings(0).IndexL)
                End If
                w.Write(Vortices(i).Sence(0))

                If (IsNothing(Vortices(i).Rings(1))) Then
                    w.Write(-1)
                Else
                    w.Write(Vortices(i).Rings(1).IndexL)
                End If
                w.Write(Vortices(i).Sence(1))

                w.Write(Vortices(i).Streamwise)

            Next

        End Sub

        Public Overridable Sub ReadBinary(ByRef r As BinaryReader)

            Nodes.Clear()
            VortexRings.Clear()

            Dim Position As New Vector3

            Dim NodalVelocity As Boolean = r.ReadBoolean
            Dim ReferenceAndDisplacement As Boolean = r.ReadBoolean

            Dim lb As Integer = r.ReadInt32

            Dim n As Integer = r.ReadInt32

            For i = 0 To n - 1

                Position.X = r.ReadDouble()
                Position.Y = r.ReadDouble()
                Position.Z = r.ReadDouble()
                Nodes.Add(New Node(i, Position))

                If NodalVelocity Then

                    Nodes(i).Velocity.X = r.ReadDouble()
                    Nodes(i).Velocity.Y = r.ReadDouble()
                    Nodes(i).Velocity.Z = r.ReadDouble()

                End If

                If ReferenceAndDisplacement Then

                    Nodes(i).OriginalPosition = New Vector3(r.ReadDouble(), r.ReadDouble(), r.ReadDouble())

                    Nodes(i).Displacement = New Vector3(r.ReadDouble(), r.ReadDouble(), r.ReadDouble())

                End If

            Next

            Dim inodes(3) As Integer

            n = r.ReadInt32

            For i = 0 To n - 1

                Dim vrType As VortexRingType = r.ReadByte
                Dim vrSlender As Boolean = r.ReadBoolean
                Dim vrReversed As Boolean = r.ReadBoolean

                inodes(0) = r.ReadInt32
                inodes(1) = r.ReadInt32
                inodes(2) = r.ReadInt32

                If vrType = VortexRingType.VR4 Then

                    inodes(3) = r.ReadInt32
                    VortexRings.Add(New VortexRing4(Nodes(inodes(0)), Nodes(inodes(1)), Nodes(inodes(2)), Nodes(inodes(3)), r.ReadDouble, i, vrReversed, vrSlender))

                Else

                    VortexRings.Add(New VortexRing3(Nodes(inodes(0)), Nodes(inodes(1)), Nodes(inodes(2)), r.ReadDouble, i, vrReversed, vrSlender))

                End If

            Next

            n = r.ReadInt32

            For i = 0 To n - 1

                Dim vortex As New Vortex

                inodes(0) = r.ReadInt32 - lb
                inodes(1) = r.ReadInt32 - lb

                vortex.Node1 = Nodes(inodes(0))
                vortex.Node2 = Nodes(inodes(1))
                vortex.G = r.ReadDouble

                inodes(0) = r.ReadInt32
                If inodes(0) >= 0 And inodes(0) < Nodes.Count Then
                    vortex.Rings(0) = VortexRings(inodes(0))
                End If
                vortex.Sence(0) = r.ReadSByte

                inodes(1) = r.ReadInt32
                If inodes(1) >= 0 And inodes(1) < Nodes.Count Then
                    vortex.Rings(1) = VortexRings(inodes(1))
                End If
                vortex.Sence(1) = r.ReadSByte

                vortex.Streamwise = r.ReadBoolean

                Vortices.Add(vortex)

            Next

        End Sub

#End Region

    End Class

End Namespace

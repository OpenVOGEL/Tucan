'Open VOGEL (openvogel.org)
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

Imports DotNumerics.LinearAlgebra
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace CalculationModel.Models.Aero.Components

    ''' <summary>
    ''' Represents a ring of 4 vortex segments.
    ''' Nodes are passed by refference and used to on calculation, they do not belong to this class.
    ''' </summary>
    Public Class VortexRing4

        Implements VortexRing

#Region " Constructors "

        ''' <summary>
        ''' Use this constructor to add bounded panels.
        ''' Geometric entities will be imediatly calculated.
        ''' </summary>
        Public Sub New(ByVal N1 As Node, ByVal N2 As Node, ByVal N3 As Node, ByVal N4 As Node, ByVal IndexL As Integer, ByVal Reversed As Boolean, IsSlender As Boolean)

            VelocityW = New Vector3
            VelocityT = New Vector3
            VelocityS = New Vector3

            For i = 0 To 3
                _LocalNodes(i) = New Vector2
                _LocalEdges(i) = New Vector2
            Next

            _Nodes(0) = N1
            _Nodes(1) = N2
            _Nodes(2) = N3
            _Nodes(3) = N4

            Me.IndexL = IndexL

            _Reversed = Reversed
            _IsSlender = IsSlender

            CalculateGeometricEntities()

        End Sub

        ''' <summary>
        ''' Use this constructor to add wake panels. Geometric entities will not be calculated here.
        ''' </summary>
        Public Sub New(ByVal N1 As Node, ByVal N2 As Node, ByVal N3 As Node, ByVal N4 As Node, ByVal G As Double, ByVal IndexL As Integer, ByVal Reversed As Boolean, ByVal IsSlender As Boolean)

            VelocityW = New Vector3
            VelocityT = New Vector3
            VelocityS = New Vector3

            For i = 0 To 3
                _LocalNodes(i) = New Vector2
                _LocalEdges(i) = New Vector2
            Next

            _Nodes(0) = N1
            _Nodes(1) = N2
            _Nodes(2) = N3
            _Nodes(3) = N4

            _Reversed = Reversed
            _IsSlender = IsSlender

            Me.IndexL = IndexL
            Me.G = G

        End Sub

#End Region

#Region " Fields & Properties "

        Public ReadOnly Property Type As VortexRingType Implements VortexRing.Type
            Get
                Return VortexRingType.VR4
            End Get
        End Property

        Private _Nodes(3) As Node ' Contains refference to existing corner nodes. Four nodes always available.
        Private _LocalNodes(3) As Vector2
        Private _LocalEdges(3) As Vector2
        Private _LocalSides(3) As Double
        Private _Basis As New Base3
        Private _InnerControlPoint As New Vector3
        Private _MidleControlPoint As New Vector3
        Private _OuterControlPoint As New Vector3
        Private _Area As New Double

        ''' <summary>
        ''' This is the velocity induced by the wakes plus the stream velocity.
        ''' </summary>
        Public Property VelocityW As Vector3 Implements VortexRing.VelocityW

        ''' <summary>
        ''' Total velocity at the control point.
        ''' </summary>
        Public Property VelocityT As Vector3 Implements VortexRing.VelocityT

        ''' <summary>
        ''' Surface velocity at the control point
        ''' </summary>
        Public Property VelocityS As Vector3 Implements VortexRing.VelocityS

        ''' <summary>
        ''' Local circulation.
        ''' </summary>
        Public Property G As Double Implements VortexRing.G

        ''' <summary>
        ''' First derivative of circulation in time.
        ''' </summary>
        Public Property DGdt As Double Implements VortexRing.DGdt

        ''' <summary>
        ''' Local source intensity.
        ''' </summary>
        Public Property S As Double Implements VortexRing.S

        ''' <summary>
        ''' Potential induced by wake doublets.
        ''' </summary>
        Public Property PotentialW As Double Implements VortexRing.PotentialW

        Private _Cp As Double

        ''' <summary>
        ''' Local pressure coeficient. When parent lattice "IsSlender" field is true it represents the coeficient of local jump of pressure.
        ''' </summary>
        Public Property Cp As Double Implements VortexRing.Cp
            Set(ByVal value As Double)
                _Cp = value
            End Set
            Get
                Return _Cp
            End Get
        End Property

        Private _Cdi As Double

        ''' <summary>
        ''' Local component of induced drag (only valid for slender rings).
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Cdi As Double Implements VortexRing.Cdi
            Set(ByVal value As Double)
                _Cdi = value
            End Set
            Get
                Return _Cdi
            End Get
        End Property

        Private _IndexL As Integer

        ''' <summary>
        ''' Local index: represents the position of this panel on the local storage.
        ''' </summary>
        Public Property IndexL As Integer Implements VortexRing.IndexL
            Set(ByVal value As Integer)
                _IndexL = value
            End Set
            Get
                Return _IndexL
            End Get
        End Property

        Private _IndexG As Integer

        ''' <summary>
        ''' Global index: represents the index of this panel on the influence matrix and G and RHS vectors.
        ''' </summary>
        Public Property IndexG As Integer Implements VortexRing.IndexG
            Set(ByVal value As Integer)
                _IndexG = value
            End Set
            Get
                Return _IndexG
            End Get
        End Property

        ''' <summary>
        ''' Four times PI
        ''' </summary>
        ''' <remarks></remarks>
        Const FourPi As Double = 4.0# * Math.PI

        ''' <summary>
        ''' Four times PI
        ''' </summary>
        ''' <remarks></remarks>
        Const InverseFourPi As Double = 1.0# / (4.0# * Math.PI)

        ''' <summary>
        ''' Collocation point delta
        ''' </summary>
        ''' <remarks></remarks>
        Const Delta As Double = 0.0000001

        ''' <summary>
        ''' Minimum value of the Biot-Savart denominator
        ''' </summary>
        ''' <remarks></remarks>
        Const Epsilon As Double = 0.00000001

#End Region

#Region " Geometry "

        ''' <summary>
        ''' Sets or gets a corner node. This property is 1-based.
        ''' </summary>
        Public Property Node(ByVal Index As Integer) As Node Implements VortexRing.Node
            Get
                Return _Nodes(Index - 1)
            End Get
            Set(ByVal value As Node)
                _Nodes(Index - 1) = value
            End Set
        End Property

        ''' <summary>
        ''' Normal vector at the control point.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Normal As Vector3 Implements VortexRing.Normal
            Get
                Return _Basis.W
            End Get
        End Property

        ''' <summary>
        ''' Control point used to impose local boundary conditions.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ControlPoint As Vector3 Implements VortexRing.ControlPoint
            Get
                Return _InnerControlPoint
            End Get
        End Property

        ''' <summary>
        ''' Control point used to compute velocity at non slender rings.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property OuterControlPoint As Vector3 Implements VortexRing.OuterControlPoint
            Get
                Return _OuterControlPoint
            End Get
        End Property

        ''' <summary>
        ''' Ring area.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Area As Double Implements VortexRing.Area
            Get
                Return _Area
            End Get
        End Property

        Private _Reversed As Boolean = False

        Public ReadOnly Property Reversed As Boolean Implements VortexRing.Reversed
            Get
                Return _Reversed
            End Get
        End Property

        ''' <summary>
        ''' Calculates all geometric entities associated to the vortex ring.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CalculateGeometricEntities() Implements VortexRing.CalculateGeometricEntities

            ' Middle control point:

            _MidleControlPoint.X = (_Nodes(0).Position.X + _Nodes(1).Position.X + _Nodes(2).Position.X + _Nodes(3).Position.X) * 0.25#
            _MidleControlPoint.Y = (_Nodes(0).Position.Y + _Nodes(1).Position.Y + _Nodes(2).Position.Y + _Nodes(3).Position.Y) * 0.25#
            _MidleControlPoint.Z = (_Nodes(0).Position.Z + _Nodes(1).Position.Z + _Nodes(2).Position.Z + _Nodes(3).Position.Z) * 0.25#

            ' Local basis:

            RecalculateBasis()

            ' Inner and outer control points

            _OuterControlPoint.X = _MidleControlPoint.X + Delta * _Basis.W.X
            _OuterControlPoint.Y = _MidleControlPoint.Y + Delta * _Basis.W.Y
            _OuterControlPoint.Z = _MidleControlPoint.Z + Delta * _Basis.W.Z

            _InnerControlPoint.X = _MidleControlPoint.X
            _InnerControlPoint.Y = _MidleControlPoint.Y
            _InnerControlPoint.Z = _MidleControlPoint.Z

            If Not _IsSlender Then
                _InnerControlPoint.X -= Delta * _Basis.W.X
                _InnerControlPoint.Y -= Delta * _Basis.W.Y
                _InnerControlPoint.Z -= Delta * _Basis.W.Z
            End If

            ' Surface velocity:

            VelocityS.X = (_Nodes(0).Velocity.X + _Nodes(1).Velocity.X + _Nodes(2).Velocity.X + _Nodes(3).Velocity.X) * 0.25#
            VelocityS.Y = (_Nodes(0).Velocity.Y + _Nodes(1).Velocity.Y + _Nodes(2).Velocity.Y + _Nodes(3).Velocity.Y) * 0.25#
            VelocityS.Z = (_Nodes(0).Velocity.Z + _Nodes(1).Velocity.Z + _Nodes(2).Velocity.Z + _Nodes(3).Velocity.Z) * 0.25#

        End Sub

        ''' <summary>
        ''' Calculates the local coordinates of the vortices.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RecalculateLocalCoordinates() Implements VortexRing.RecalculateLocalCoordinates

            Dim dx As Double
            Dim dy As Double
            Dim dz As Double

            For i = 0 To 3

                dx = _Nodes(i).Position.X - _MidleControlPoint.X
                dy = _Nodes(i).Position.Y - _MidleControlPoint.Y
                dz = _Nodes(i).Position.Z - _MidleControlPoint.Z

                _LocalNodes(i).X = dx * _Basis.U.X + dy * _Basis.U.Y + dz * _Basis.U.Z
                _LocalNodes(i).Y = dx * _Basis.V.X + dy * _Basis.V.Y + dz * _Basis.V.Z

            Next

            _LocalEdges(0).X = _LocalNodes(1).X - _LocalNodes(0).X
            _LocalEdges(0).Y = _LocalNodes(1).Y - _LocalNodes(0).Y

            _LocalEdges(1).X = _LocalNodes(2).X - _LocalNodes(1).X
            _LocalEdges(1).Y = _LocalNodes(2).Y - _LocalNodes(1).Y

            _LocalEdges(2).X = _LocalNodes(3).X - _LocalNodes(2).X
            _LocalEdges(2).Y = _LocalNodes(3).Y - _LocalNodes(2).Y

            _LocalEdges(3).X = _LocalNodes(0).X - _LocalNodes(3).X
            _LocalEdges(3).Y = _LocalNodes(0).Y - _LocalNodes(3).Y

            For i = 0 To 3
                _LocalSides(i) = _LocalEdges(i).Norm2
            Next

        End Sub

        ''' <summary>
        ''' Calculates all geometric entities associated to the vortex ring.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RecalculateBasis() Implements VortexRing.RecalculateBasis

            _Basis.U.X = _Nodes(2).Position.X - _Nodes(0).Position.X
            _Basis.U.Y = _Nodes(2).Position.Y - _Nodes(0).Position.Y
            _Basis.U.Z = _Nodes(2).Position.Z - _Nodes(0).Position.Z

            Dim V2X = _Nodes(3).Position.X - _Nodes(1).Position.X
            Dim V2Y = _Nodes(3).Position.Y - _Nodes(1).Position.Y
            Dim V2Z = _Nodes(3).Position.Z - _Nodes(1).Position.Z

            _Basis.W.X = _Basis.U.Y * V2Z - _Basis.U.Z * V2Y
            _Basis.W.Y = _Basis.U.Z * V2X - _Basis.U.X * V2Z
            _Basis.W.Z = _Basis.U.X * V2Y - _Basis.U.Y * V2X

            _Area = 0.5 * _Basis.W.EuclideanNorm

            _Basis.U.Normalize()
            _Basis.W.Normalize()

            _Basis.V.FromVectorProduct(_Basis.W, _Basis.U)

            If _Reversed Then
                _Basis.U.Oppose()
                _Basis.V.Oppose()
                _Basis.W.Oppose()
            End If

            RecalculateLocalCoordinates()

        End Sub

#End Region

#Region " Velocity field "

        ''' <summary>
        ''' Returns the influence of the doublet distribution in the velocity.
        ''' If WidthG is true vector is scaled by G.
        ''' </summary>
        Public Function GetDoubletVelocityInfluence(ByVal Point As Vector3,
                                                     Optional ByVal CutOff As Double = 0.0001,
                                                     Optional ByVal WithG As Boolean = True) As Vector3 Implements VortexRing.GetDoubletVelocityInfluence

            Dim Vector As New Vector3

            AddDoubletVelocityInfluence(Vector, Point, CutOff, WithG)

            Return Vector

        End Function

        ''' <summary>
        ''' Adds the influence of the doublet distribution in the velocity.
        ''' If WidthG is true vector is scaled by G.
        ''' </summary>
        Public Sub AddDoubletVelocityInfluence(ByRef Vector As Vector3,
                                               ByVal Point As Vector3,
                                               Optional ByVal CutOff As Double = 0.0001,
                                               Optional ByVal WithG As Boolean = True) Implements VortexRing.AddDoubletVelocityInfluence

            Dim Den As Double
            Dim Num As Double

            Dim Node1 As Vector3 = Nothing
            Dim Node2 As Vector3 = Nothing

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim Vx, Vy, Vz As Double
            Dim Dx, Dy, Dz As Double

            Dim NR1 As Double
            Dim NR2 As Double
            Dim Factor As Double

            For i = 1 To 4

                Select Case i
                    Case 1
                        Node1 = Node(1).Position
                        Node2 = Node(2).Position
                    Case 2
                        Node1 = Node(2).Position
                        Node2 = Node(3).Position
                    Case 3
                        Node1 = Node(3).Position
                        Node2 = Node(4).Position
                    Case 4
                        Node1 = Node(4).Position
                        Node2 = Node(1).Position
                End Select

                Lx = Node2.X - Node1.X
                Ly = Node2.Y - Node1.Y
                Lz = Node2.Z - Node1.Z

                R1x = Point.X - Node1.X
                R1y = Point.Y - Node1.Y
                R1z = Point.Z - Node1.Z

                Vx = Ly * R1z - Lz * R1y
                Vy = Lz * R1x - Lx * R1z
                Vz = Lx * R1y - Ly * R1x

                Den = FourPi * (Vx * Vx + Vy * Vy + Vz * Vz)

                If Den > CutOff Then

                    ' Calculate the rest of the geometrical parameters:

                    R2x = Point.X - Node2.X
                    R2y = Point.Y - Node2.Y
                    R2z = Point.Z - Node2.Z

                    NR1 = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                    NR2 = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                    Dx = NR1 * R1x - NR2 * R2x
                    Dy = NR1 * R1y - NR2 * R2y
                    Dz = NR1 * R1z - NR2 * R2z

                    If WithG Then
                        Num = G * (Lx * Dx + Ly * Dy + Lz * Dz)
                    Else
                        Num = (Lx * Dx + Ly * Dy + Lz * Dz)
                    End If

                    Factor = Num / Den

                    If Reversed Then Factor *= -1

                    Vector.X += Factor * Vx
                    Vector.Y += Factor * Vy
                    Vector.Z += Factor * Vz

                End If

            Next

        End Sub

        ''' <summary>
        ''' Adds the influence of the source distribution in the velocity.
        ''' </summary>
        Sub AddSourceVelocityInfluence(ByRef Vector As Vector3,
                                       ByVal Point As Vector3,
                                       Optional ByVal WithS As Boolean = True) Implements VortexRing.AddSourceVelocityInfluence

            ' Convert the point to local coordinates (center on the control point + using the local basis)

            Dim dx = Point.X - _MidleControlPoint.X
            Dim dy = Point.Y - _MidleControlPoint.Y
            Dim dz = Point.Z - _MidleControlPoint.Z

            Dim p As New Vector3(dx * _Basis.U.X + dy * _Basis.U.Y + dz * _Basis.U.Z,
                                 dx * _Basis.V.X + dy * _Basis.V.Y + dz * _Basis.V.Z,
                                 dx * _Basis.W.X + dy * _Basis.W.Y + dz * _Basis.W.Z)

            ' Distances:

            Dim r0px = p.X - _LocalNodes(0).X
            Dim r0py = p.Y - _LocalNodes(0).Y
            Dim r0pz = p.Z
            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + r0pz * r0pz)

            Dim r1px = p.X - _LocalNodes(1).X
            Dim r1py = p.Y - _LocalNodes(1).Y
            Dim r1pz = p.Z
            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + r1pz * r1pz)

            Dim r2px = p.X - _LocalNodes(2).X
            Dim r2py = p.Y - _LocalNodes(2).Y
            Dim r2pz = p.Z
            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + r2pz * r2pz)

            Dim r3px = p.X - _LocalNodes(3).X
            Dim r3py = p.Y - _LocalNodes(3).Y
            Dim r3pz = p.Z
            Dim r3p As Double = Math.Sqrt(r3px * r3px + r3py * r3py + r3pz * r3pz)

            ' Projected segments:

            Dim d01x As Double = _LocalEdges(0).X
            Dim d01y As Double = _LocalEdges(0).Y

            Dim d12x As Double = _LocalEdges(1).X
            Dim d12y As Double = _LocalEdges(1).Y

            Dim d23x As Double = _LocalEdges(2).X
            Dim d23y As Double = _LocalEdges(2).Y

            Dim d30x As Double = _LocalEdges(3).X
            Dim d30y As Double = _LocalEdges(3).Y

            ' Segments length:

            Dim d01 As Double = _LocalSides(0)
            Dim d12 As Double = _LocalSides(1)
            Dim d23 As Double = _LocalSides(2)
            Dim d30 As Double = _LocalSides(3)

            ' Use center point as referece to compute the altitude:

            Dim z As Double = Math.Abs(p.Z)

            ' Entities for evaluation of arctangents:

            Dim z2 As Double = z * z

            Dim e0 As Double = r0px * r0px + z2
            Dim e1 As Double = r1px * r1px + z2
            Dim e2 As Double = r2px * r2px + z2
            Dim e3 As Double = r3px * r3px + z2

            Dim h0 As Double = r0px * r0py
            Dim h1 As Double = r1px * r1py
            Dim h2 As Double = r2px * r2py
            Dim h3 As Double = r3px * r3py

            ' Normal component:
            ' These are the Katz-Plotkin fotran formulas, which are based in only one Atan2 instead of two

            Dim f0 As Double = d01y * e0 - d01x * h0
            Dim g0 As Double = d01y * e1 - d01x * h1
            Dim tn01 As Double = Math.Atan2(z * d01x * (f0 * r1p - g0 * r0p), z2 * d01x * d01x * r0p * r1p + f0 * g0)

            Dim f1 As Double = d12y * e1 - d12x * h1
            Dim g1 As Double = d12y * e2 - d12x * h2
            Dim tn12 As Double = Math.Atan2(z * d12x * (f1 * r2p - g1 * r1p), z2 * d12x * d12x * r1p * r2p + f1 * g1)

            Dim f2 As Double = d23y * e2 - d23x * h2
            Dim g2 As Double = d23y * e3 - d23x * h3
            Dim tn23 As Double = Math.Atan2(z * d23x * (f2 * r3p - g2 * r2p), z2 * d23x * d23x * r2p * r3p + f2 * g2)

            Dim f3 As Double = d30y * e3 - d30x * h3
            Dim g3 As Double = d30y * e0 - d30x * h0
            Dim tn30 As Double = Math.Atan2(z * d30x * (f3 * r0p - g3 * r3p), z2 * d30x * d30x * r3p * r0p + f3 * g3)

            Dim Vw As Double = Math.Sign(p.Z) * (tn01 + tn12 + tn23 + tn30)

            ' Logarithmic factors

            Dim ln01 As Double = Math.Log((r0p + r1p - d01) / (r0p + r1p + d01))
            Dim ln12 As Double = Math.Log((r1p + r2p - d12) / (r1p + r2p + d12))
            Dim ln23 As Double = Math.Log((r2p + r3p - d23) / (r2p + r3p + d23))
            Dim ln30 As Double = Math.Log((r3p + r0p - d30) / (r3p + r0p + d30))

            ' Planar velocity componets:

            Dim Vu As Double = d01y / d01 * ln01 + d12y / d12 * ln12 + d23y / d23 * ln23 + d30y / d30 * ln30
            Dim Vv As Double = d01x / d01 * ln01 + d12x / d12 * ln12 + d23x / d23 * ln23 + d30x / d30 * ln30

            ' Recompose vector in global coordinates

            Dim Factor As Double = InverseFourPi

            If WithS Then
                Factor *= S
            End If

            Vu *= Factor
            Vv *= Factor
            Vw *= Factor

            Vector.X += _Basis.U.X * Vu - _Basis.V.X * Vv + _Basis.W.X * Vw
            Vector.Y += _Basis.U.Y * Vu - _Basis.V.Y * Vv + _Basis.W.Y * Vw
            Vector.Z += _Basis.U.Z * Vu - _Basis.V.Z * Vv + _Basis.W.Z * Vw

        End Sub

#Region " Other functions "

        ''' <summary>
        ''' Computes the induced velocity at a given point by counting only the stramwise vortices
        ''' </summary>
        ''' <param name="Point">Point where influence is to be calculated</param>
        ''' <param name="N1">First streamwise segment</param>
        ''' <param name="N2">Second streamwise segment</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function StreamwiseInfluence(ByVal Point As Vector3, ByVal N1 As Integer, ByVal N2 As Integer, Optional ByVal CutOff As Double = 0.0001) As Vector3 Implements VortexRing.StreamwiseInfluence

            Dim BSVector As New Vector3

            Dim Den As Double
            Dim Num As Double

            Dim Node1 As Vector3 = Nothing
            Dim Node2 As Vector3 = Nothing

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim vx, vy, vz As Double
            Dim dx, dy, dz As Double

            Dim NR1 As Double
            Dim NR2 As Double
            Dim Factor As Double

            For Each i In {N1, N2}

                Select Case i
                    Case 1
                        Node1 = Node(1).Position
                        Node2 = Node(2).Position
                    Case 2
                        Node1 = Node(2).Position
                        Node2 = Node(3).Position
                    Case 3
                        Node1 = Node(3).Position
                        Node2 = Node(4).Position
                    Case 4
                        Node1 = Node(4).Position
                        Node2 = Node(1).Position
                End Select

                Lx = Node2.X - Node1.X
                Ly = Node2.Y - Node1.Y
                Lz = Node2.Z - Node1.Z

                R1x = Point.X - Node1.X
                R1y = Point.Y - Node1.Y
                R1z = Point.Z - Node1.Z

                R2x = Point.X - Node2.X
                R2y = Point.Y - Node2.Y
                R2z = Point.Z - Node2.Z

                NR1 = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                NR2 = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                dx = NR1 * R1x - NR2 * R2x
                dy = NR1 * R1y - NR2 * R2y
                dz = NR1 * R1z - NR2 * R2z

                vx = Ly * R1z - Lz * R1y
                vy = Lz * R1x - Lx * R1z
                vz = Lx * R1y - Ly * R1x

                Den = FourPi * (vx * vx + vy * vy + vz * vz)

                If Den > CutOff Then

                    Num = G * (Lx * dx + Ly * dy + Lz * dz)

                    Factor = Num / Den

                    BSVector.X += Factor * vx
                    BSVector.Y += Factor * vy
                    BSVector.Z += Factor * vz

                End If

            Next

            Return BSVector

        End Function

#End Region

#End Region

#Region " Velocity potential "

        ''' <summary>
        ''' Returns the influence of the doublet distribution in the velocity potential.
        ''' </summary>
        Public Function GetDoubletPotentialInfluence(ByVal Point As Vector3,
                                                     Optional ByVal WithG As Boolean = True) As Double Implements VortexRing.GetDoubletPotentialInfluence

            ' Convert the point to local coordinates (center on the control point and using the local basis)

            Dim dx = Point.X - _MidleControlPoint.X
            Dim dy = Point.Y - _MidleControlPoint.Y
            Dim dz = Point.Z - _MidleControlPoint.Z

            Dim p As New Vector3(dx * _Basis.U.X + dy * _Basis.U.Y + dz * _Basis.U.Z,
                                 dx * _Basis.V.X + dy * _Basis.V.Y + dz * _Basis.V.Z,
                                 dx * _Basis.W.X + dy * _Basis.W.Y + dz * _Basis.W.Z)

            Dim pzsq = p.Z * p.Z

            ' Distances:

            Dim r0px = p.X - _LocalNodes(0).X
            Dim r0py = p.Y - _LocalNodes(0).Y
            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + pzsq)

            Dim r1px = p.X - _LocalNodes(1).X
            Dim r1py = p.Y - _LocalNodes(1).Y
            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + pzsq)

            Dim r2px = p.X - _LocalNodes(2).X
            Dim r2py = p.Y - _LocalNodes(2).Y
            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + pzsq)

            Dim r3px = p.X - _LocalNodes(3).X
            Dim r3py = p.Y - _LocalNodes(3).Y
            Dim r3p As Double = Math.Sqrt(r3px * r3px + r3py * r3py + pzsq)

            ' Use center point as referece to compute the altitude:

            Dim z As Double = p.Z

            ' Projected segments:

            Dim d01x As Double = _LocalEdges(0).X
            Dim d01y As Double = _LocalEdges(0).Y

            Dim d12x As Double = _LocalEdges(1).X
            Dim d12y As Double = _LocalEdges(1).Y

            Dim d23x As Double = _LocalEdges(2).X
            Dim d23y As Double = _LocalEdges(2).Y

            Dim d30x As Double = _LocalEdges(3).X
            Dim d30y As Double = _LocalEdges(3).Y

            ' Entities for evaluation of arctangents:

            Dim z2 As Double = z * z

            Dim e0 As Double = r0px * r0px + z2
            Dim e1 As Double = r1px * r1px + z2
            Dim e2 As Double = r2px * r2px + z2
            Dim e3 As Double = r3px * r3px + z2

            Dim h0 As Double = r0px * r0py
            Dim h1 As Double = r1px * r1py
            Dim h2 As Double = r2px * r2py
            Dim h3 As Double = r3px * r3py

            ' These are the Katz-Plotkin fortran formulas, which are based in only one Atan2 instead of two

            Dim f0 As Double = d01y * e0 - d01x * h0
            Dim g0 As Double = d01y * e1 - d01x * h1
            Dim tn01 As Double = Math.Atan2(z * d01x * (f0 * r1p - g0 * r0p), z2 * d01x * d01x * r0p * r1p + f0 * g0)

            Dim f1 As Double = d12y * e1 - d12x * h1
            Dim g1 As Double = d12y * e2 - d12x * h2
            Dim tn12 As Double = Math.Atan2(z * d12x * (f1 * r2p - g1 * r1p), z2 * d12x * d12x * r1p * r2p + f1 * g1)

            Dim f2 As Double = d23y * e2 - d23x * h2
            Dim g2 As Double = d23y * e3 - d23x * h3
            Dim tn23 As Double = Math.Atan2(z * d23x * (f2 * r3p - g2 * r2p), z2 * d23x * d23x * r2p * r3p + f2 * g2)

            Dim f3 As Double = d30y * e3 - d30x * h3
            Dim g3 As Double = d30y * e0 - d30x * h0
            Dim tn30 As Double = Math.Atan2(z * d30x * (f3 * r0p - g3 * r3p), z2 * d30x * d30x * r3p * r0p + f3 * g3)

            Dim Potential As Double = (tn01 + tn12 + tn23 + tn30) / FourPi

            If WithG Then Potential *= G

            Return Potential

        End Function

        ''' <summary>
        ''' Returns the influence of the source distribution in the velocity potential.
        ''' </summary>
        Public Function GetSourcePotentialInfluence(ByVal Point As Vector3, Optional ByVal WithS As Boolean = True) As Double Implements VortexRing.GetSourcePotentialInfluence

            ' Convert the point to local coordinates (center on the control point and using the local basis)

            Dim dx = Point.X - _MidleControlPoint.X
            Dim dy = Point.Y - _MidleControlPoint.Y
            Dim dz = Point.Z - _MidleControlPoint.Z

            Dim p As New Vector3(dx * _Basis.U.X + dy * _Basis.U.Y + dz * _Basis.U.Z,
                                 dx * _Basis.V.X + dy * _Basis.V.Y + dz * _Basis.V.Z,
                                 dx * _Basis.W.X + dy * _Basis.W.Y + dz * _Basis.W.Z)

            Dim pzsq = p.Z * p.Z

            ' Distances:

            Dim r0px = p.X - _LocalNodes(0).X
            Dim r0py = p.Y - _LocalNodes(0).Y
            Dim r0p As Double = Math.Sqrt(r0px * r0px + r0py * r0py + pzsq)

            Dim r1px = p.X - _LocalNodes(1).X
            Dim r1py = p.Y - _LocalNodes(1).Y
            Dim r1p As Double = Math.Sqrt(r1px * r1px + r1py * r1py + pzsq)

            Dim r2px = p.X - _LocalNodes(2).X
            Dim r2py = p.Y - _LocalNodes(2).Y
            Dim r2p As Double = Math.Sqrt(r2px * r2px + r2py * r2py + pzsq)

            Dim r3px = p.X - _LocalNodes(3).X
            Dim r3py = p.Y - _LocalNodes(3).Y
            Dim r3p As Double = Math.Sqrt(r3px * r3px + r3py * r3py + pzsq)

            ' Use center point as referece to compute the altitude:

            Dim z As Double = Math.Abs(p.Z)

            ' Projected segments:

            Dim d01x As Double = _LocalEdges(0).X
            Dim d01y As Double = _LocalEdges(0).Y

            Dim d12x As Double = _LocalEdges(1).X
            Dim d12y As Double = _LocalEdges(1).Y

            Dim d23x As Double = _LocalEdges(2).X
            Dim d23y As Double = _LocalEdges(2).Y

            Dim d30x As Double = _LocalEdges(3).X
            Dim d30y As Double = _LocalEdges(3).Y

            ' Segments length:

            Dim d01 As Double = _LocalSides(0)
            Dim d12 As Double = _LocalSides(1)
            Dim d23 As Double = _LocalSides(2)
            Dim d30 As Double = _LocalSides(3)

            ' Logarithms:

            Dim ln01 As Double = (r0px * d01y - r0py * d01x) / d01 * Math.Log((r0p + r1p + d01) / (r0p + r1p - d01))
            Dim ln12 As Double = (r1px * d12y - r1py * d12x) / d12 * Math.Log((r1p + r2p + d12) / (r1p + r2p - d12))
            Dim ln23 As Double = (r2px * d23y - r2py * d23x) / d23 * Math.Log((r2p + r3p + d23) / (r2p + r3p - d23))
            Dim ln30 As Double = (r3px * d30y - r3py * d30x) / d30 * Math.Log((r3p + r0p + d30) / (r3p + r0p - d30))

            ' Entities for evaluation of arctangents:

            Dim z2 As Double = z * z

            Dim e0 As Double = r0px * r0px + z2
            Dim e1 As Double = r1px * r1px + z2
            Dim e2 As Double = r2px * r2px + z2
            Dim e3 As Double = r3px * r3px + z2

            Dim h0 As Double = r0px * r0py
            Dim h1 As Double = r1px * r1py
            Dim h2 As Double = r2px * r2py
            Dim h3 As Double = r3px * r3py

            ' These are the Katz-Plotkin fortran formulas, which are based in only one Atan2 instead of two

            Dim f0 As Double = d01y * e0 - d01x * h0
            Dim g0 As Double = d01y * e1 - d01x * h1
            Dim tn01 As Double = Math.Atan2(z * d01x * (f0 * r1p - g0 * r0p), z2 * d01x * d01x * r0p * r1p + f0 * g0)

            Dim f1 As Double = d12y * e1 - d12x * h1
            Dim g1 As Double = d12y * e2 - d12x * h2
            Dim tn12 As Double = Math.Atan2(z * d12x * (f1 * r2p - g1 * r1p), z2 * d12x * d12x * r1p * r2p + f1 * g1)

            Dim f2 As Double = d23y * e2 - d23x * h2
            Dim g2 As Double = d23y * e3 - d23x * h3
            Dim tn23 As Double = Math.Atan2(z * d23x * (f2 * r3p - g2 * r2p), z2 * d23x * d23x * r2p * r3p + f2 * g2)

            Dim f3 As Double = d30y * e3 - d30x * h3
            Dim g3 As Double = d30y * e0 - d30x * h0
            Dim tn30 As Double = Math.Atan2(z * d30x * (f3 * r0p - g3 * r3p), z2 * d30x * d30x * r3p * r0p + f3 * g3)

            Dim Potential As Double = -(ln01 + ln12 + ln23 + ln30 - z * (tn01 + tn12 + tn23 + tn30)) / FourPi

            If WithS Then Potential *= S

            Return Potential

        End Function

#End Region

#Region " Loads "

        ' These fields are only used to calculate local Cp

        Private _IsPrimitive As Boolean

        ''' <summary>
        ''' Indicates whether the panel is used to convect wake or not. This conditionates the local circulation.
        ''' </summary>
        Public Property IsPrimitive As Boolean Implements VortexRing.IsPrimitive
            Set(ByVal value As Boolean)
                _IsPrimitive = value
            End Set
            Get
                Return _IsPrimitive
            End Get
        End Property

        Private _IsSlender As Boolean

        ''' <summary>
        ''' Indicates whether the surface is slender or not. This will conditionate the calculation of the local pressure coeficient.
        ''' </summary>
        Public ReadOnly Property IsSlender As Boolean Implements VortexRing.IsSlender
            Get
                Return _IsSlender
            End Get
        End Property

        Private _SurroundingRings(3, 1) As VortexRing

        ''' <summary>
        ''' Provides refference to the local neighbor rings. This field is driven by "FindSurroundingRings" of parent lattice.
        ''' </summary>
        ''' <remarks>
        '''  If there is no ring at the given position "nothing" will be returned.
        ''' </remarks>
        Public Property SurroundingRing(ByVal side As UInt16, ByVal location As UInt16) As VortexRing Implements VortexRing.SurroundingRing
            Set(ByVal value As VortexRing)
                _SurroundingRings(side, location) = value
            End Set
            Get
                Return _SurroundingRings(side, location)
            End Get
        End Property

        Private _SurroundingRingsSence(3, 1) As Int16

        ''' <summary>
        ''' Sence of the adjacent ring. 1 if same as this ring, -1 if oposite and 0 if there is no assigned ring on that location.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SurroundingRingsSence(ByVal side As UInt16, ByVal location As UInt16) As Int16 Implements VortexRing.SurroundingRingsSence
            Set(ByVal value As Int16)
                _SurroundingRingsSence(side, location) = value
            End Set
            Get
                Return _SurroundingRingsSence(side, location)
            End Get
        End Property

        ''' <summary>
        ''' Indicates whether this ring has a neighbor ring at the given position or not.
        ''' </summary>
        ''' <param name="side">
        ''' 1-based index indicating the local position of the boundary line.
        ''' </param>
        Public ReadOnly Property HasNeighborAt(ByVal side As UInt16, ByVal location As UInt16) Implements VortexRing.HasNeighborAt
            Get
                Return Not IsNothing(_SurroundingRings(side, location))
            End Get
        End Property

        ''' <summary>
        ''' Attaches a neighbour ring at the following location on the given side.
        ''' </summary>
        ''' <param name="side">0-based index indicating the local position of the boundary line.</param>
        ''' <remarks></remarks>
        Sub AttachNeighbourAtSide(ByVal side As Int16, ByRef Ring As VortexRing, ByVal Sence As Int16) Implements VortexRing.AttachNeighbourAtSide
            If IsNothing(_SurroundingRings(side, 0)) Then
                _SurroundingRings(side, 0) = Ring
                _SurroundingRingsSence(side, 0) = Sence
            Else
                _SurroundingRings(side, 1) = Ring
                _SurroundingRingsSence(side, 1) = Sence
            End If
        End Sub

        Private _GSence As Int16 = 1

        ''' <summary>
        ''' Indicates the local circulation sence (1 or -1).
        ''' </summary>
        Public Property CirculationSence As Int16 Implements VortexRing.CirculationSence
            Set(ByVal value As Int16)
                _GSence = value
            End Set
            Get
                Return _GSence
            End Get
        End Property

        Private _Gammas(3) As Double

        ''' <summary>
        ''' Stores the circulation of each vortex segment composing this ring. Driven by "CalculateGammas".
        ''' </summary>
        Public Property Gamma(ByVal index As Integer) As Double Implements VortexRing.Gamma
            Get
                Return _Gammas(index)
            End Get
            Set(ByVal value As Double)
                _Gammas(index) = value
            End Set
        End Property

        ''' <summary>
        ''' Calculates the jump of pressure through the vortex ring.
        ''' </summary>
        ''' <param name="SquaredVelocity">
        ''' Square of reference velocity Norm2.
        ''' </param>
        Public Sub CalculateCp(ByVal SquaredVelocity As Double) Implements VortexRing.CalculateCp

            If IsSlender Then

                ' Calculates vortices circulation (Gamma):

                For k = 0 To 3

                    If HasNeighborAt(k, 0) Then
                        Gamma(k) = G - SurroundingRingsSence(k, 0) * SurroundingRing(k, 0).G
                        If HasNeighborAt(k, 1) Then
                            Gamma(k) -= SurroundingRingsSence(k, 1) * SurroundingRing(k, 1).G
                        End If
                    Else
                        If IsPrimitive Then
                            Gamma(k) = 0.0#
                        Else
                            Gamma(k) = 2.0 * G
                        End If
                    End If

                Next

                Dim GammaX, GammaY, GammaZ As Double

                GammaX = 0.5 * ((Node(2).Position.X - Node(1).Position.X) * Gamma(0) +
                                (Node(3).Position.X - Node(2).Position.X) * Gamma(1) +
                                (Node(4).Position.X - Node(3).Position.X) * Gamma(2) +
                                (Node(1).Position.X - Node(4).Position.X) * Gamma(3))

                GammaY = 0.5 * ((Node(2).Position.Y - Node(1).Position.Y) * Gamma(0) +
                                (Node(3).Position.Y - Node(2).Position.Y) * Gamma(1) +
                                (Node(4).Position.Y - Node(3).Position.Y) * Gamma(2) +
                                (Node(1).Position.Y - Node(4).Position.Y) * Gamma(3))

                GammaZ = 0.5 * ((Node(2).Position.Z - Node(1).Position.Z) * Gamma(0) +
                                (Node(3).Position.Z - Node(2).Position.Z) * Gamma(1) +
                                (Node(4).Position.Z - Node(3).Position.Z) * Gamma(2) +
                                (Node(1).Position.Z - Node(4).Position.Z) * Gamma(3))

                ' Calculates dV as Gamma x n (not divided by the area):

                Dim dVx, dVy, dVz As Double

                dVx = GammaY * Normal.Z - GammaZ * Normal.Y
                dVy = GammaZ * Normal.X - GammaX * Normal.Z
                dVz = GammaX * Normal.Y - GammaY * Normal.X

                Cp = 2 * ((dVx * (VelocityT.X - VelocityS.X) + dVy * (VelocityT.Y - VelocityS.Y) + dVz * (VelocityT.Z - VelocityS.Z)) / Area + DGdt) / SquaredVelocity

                ' Correction: the component associated to the lattice motion was not being included.
                ' The local variation of the velocity potential should include the therm (-Vs.dV), and not only DG/dt. This is stated in equation 2.125.

                ' Cp = 2 * ((dVx * (VelocityII.X - VelocityS) + dVy * VelocityII.Y + dVz * VelocityII.Z) / Area + DGdt) / SquaredVelocity

            Else

                Cp = 1 - ((VelocityT.X * VelocityT.X + VelocityT.Y * VelocityT.Y + VelocityT.Z * VelocityT.Z)) / SquaredVelocity ' + 2 * DGdt

            End If

        End Sub

        ''' <summary>
        ''' Sets surrounding rings to nothing.
        ''' </summary>
        ''' <remarks></remarks>
        Sub InitializeSurroundingRings() Implements VortexRing.InitializeSurroundingRings
            _SurroundingRings(0, 0) = Nothing
            _SurroundingRings(1, 0) = Nothing
            _SurroundingRings(2, 0) = Nothing
            _SurroundingRings(3, 0) = Nothing
            _SurroundingRings(0, 1) = Nothing
            _SurroundingRings(1, 1) = Nothing
            _SurroundingRings(2, 1) = Nothing
            _SurroundingRings(3, 1) = Nothing
        End Sub

        ''' <summary>
        ''' Calculates the local tangent velocity by analyzing the gradient of circulation.
        ''' </summary>
        ''' <remarks>
        ''' This is only intenderd for thick bodies. It works by computing the best fit
        ''' to the circulation using least squares.
        ''' </remarks>
        Public Sub CalculateLocalVelocity(StreamVelocity As Vector3) Implements VortexRing.CalculateLocalVelocity

            VelocityT.SetToCero()

            If Not _IsSlender Then

                Dim M As New Matrix(3)
                Dim B As New Vector(3)

                ' Local circulation at (0,0)

                M(2, 2) += 1
                B(2) += G

                ' Add circulation of adjacent panels

                For i = _SurroundingRings.GetLowerBound(0) To _SurroundingRings.GetUpperBound(0)

                    ' Do not include the panels behind a shared edge

                    If _SurroundingRings(i, 0) IsNot Nothing AndAlso _SurroundingRings(i, 1) Is Nothing Then

                        Dim Delta As New Vector3

                        Delta.X = _SurroundingRings(i, 0).ControlPoint.X - ControlPoint.X
                        Delta.Y = _SurroundingRings(i, 0).ControlPoint.Y - ControlPoint.Y
                        Delta.Z = _SurroundingRings(i, 0).ControlPoint.Z - ControlPoint.Z

                        Dim OtherU As Double = Delta.InnerProduct(_Basis.U)
                        Dim OtherV As Double = Delta.InnerProduct(_Basis.V)
                        Dim OtherG = _SurroundingRings(i, 0).G

                        M(0, 0) += OtherU * OtherU
                        M(0, 1) += OtherU * OtherV
                        M(0, 2) += OtherU

                        M(1, 0) += OtherU * OtherV
                        M(1, 1) += OtherV * OtherV
                        M(1, 2) += OtherV

                        M(2, 0) += OtherU
                        M(2, 1) += OtherV
                        M(2, 2) += 1

                        B(0) += OtherU * OtherG
                        B(1) += OtherV * OtherG
                        B(2) += OtherG

                    End If

                Next

                ' Calculate the circulation derivatives on each tangent directions
                ' Vector A will contain the circulation slopes and the local mean circulation

                Dim Equations As New LinearEquations
                Dim A As Vector

                Try
                    A = Equations.Solve(M, B)
                Catch ex As Exception
                    A = New Vector(3)
                End Try

                Dim StreamU As Double = _Basis.U.InnerProduct(StreamVelocity)
                Dim StreamV As Double = _Basis.V.InnerProduct(StreamVelocity)

                VelocityT.Add(_Basis.U, -A(0) + StreamU)
                VelocityT.Add(_Basis.V, -A(1) + StreamV)

            End If

        End Sub

#End Region

    End Class

End Namespace
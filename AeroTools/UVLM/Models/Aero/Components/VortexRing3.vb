Imports MathTools.Algebra.EuclideanSpace

Namespace UVLM.Models.Aero.Components

    ''' <summary>
    ''' Represents a ring of 3 vortex segments.
    ''' Nodes are passed by refference and used to on calculation, they do not belong to this class.
    ''' </summary>
    Public Class VortexRing3

        Implements VortexRing

#Region " Constructors "

        ''' <summary>
        ''' Use this constructor to add bounded panels.
        ''' Geometric entities will be imediatly calculated.
        ''' </summary>
        Public Sub New(ByVal N1 As Node, ByVal N2 As Node, ByVal N3 As Node, ByVal IndexL As Integer, ByVal Reversed As Boolean, IsSlender As Boolean)

            VelocityW = New EVector3
            VelocityT = New EVector3
            VelocityS = New EVector3

            _Nodes(0) = N1
            _Nodes(1) = N2
            _Nodes(2) = N3
            Me.IndexL = IndexL

            _Reversed = Reversed
            _IsSlender = IsSlender

            CalculateGeometricEntities()

        End Sub

        ''' <summary>
        ''' Use this constructor to add wake panels. Geometric entities will not be calculated here.
        ''' </summary>
        Public Sub New(ByVal N1 As Node, ByVal N2 As Node, ByVal N3 As Node, ByVal G As Double, ByVal IndexL As Integer, ByVal Reversed As Boolean, IsSlender As Boolean)

            VelocityW = New EVector3
            VelocityT = New EVector3
            VelocityS = New EVector3

            _Nodes(0) = N1
            _Nodes(1) = N2
            _Nodes(2) = N3

            _Reversed = Reversed
            _IsSlender = IsSlender

            Me.IndexL = IndexL
            Me.G = G

        End Sub

#End Region

#Region " Fields & Properties "

        Public ReadOnly Property Type As VortexRingType Implements VortexRing.Type
            Get
                Return VortexRingType.VR3
            End Get
        End Property

        Private _Nodes(2) As Node ' Contains refference to existing corner nodes. Four nodes always available.
        Private _Normal As New EVector3
        Private _ControlPoint As New EVector3
        Private _OuterControlPoint As EVector3
        Private _Area As New Double

        ''' <summary>
        ''' This is the velocity induced by the wakes plus the stream velocity.
        ''' </summary>
        Public Property VelocityW As EVector3 Implements VortexRing.VelocityW

        ''' <summary>
        ''' Total velocity at the control point.
        ''' </summary>
        Public Property VelocityT As EVector3 Implements VortexRing.VelocityT

        ''' <summary>
        ''' Surface velocity at the control point
        ''' </summary>
        Public Property VelocityS As EVector3 Implements VortexRing.VelocityS

        ''' <summary>
        ''' Potential induced by wake doublets.
        ''' </summary>
        Public Property PotentialW As Double Implements VortexRing.PotentialW

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
        Const FourPi As Double = 4 * Math.PI

        ''' <summary>
        ''' Minimum value of the Biot-Savart denominator
        ''' </summary>
        ''' <remarks></remarks>
        Const Epsilon As Double = 0.0001 ^ 2

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
        Public ReadOnly Property Normal As EVector3 Implements VortexRing.Normal
            Get
                Return _Normal
            End Get
        End Property

        ''' <summary>
        ''' Control point used to impose local boundary conditions.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ControlPoint As EVector3 Implements VortexRing.ControlPoint
            Get
                Return _ControlPoint
            End Get
        End Property

        ''' <summary>
        ''' Control point used to compute velocity at non slender rings.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property OuterControlPoint As EVector3 Implements VortexRing.OuterControlPoint
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

            ' Control point:

            _ControlPoint.X = (_Nodes(0).Position.X + _Nodes(1).Position.X + _Nodes(2).Position.X) / 3
            _ControlPoint.Y = (_Nodes(0).Position.Y + _Nodes(1).Position.Y + _Nodes(2).Position.Y) / 3
            _ControlPoint.Z = (_Nodes(0).Position.Z + _Nodes(1).Position.Z + _Nodes(2).Position.Z) / 3

            If Not IsSlender Then

                _OuterControlPoint = New EVector3
                _OuterControlPoint.X = _ControlPoint.X
                _OuterControlPoint.Y = _ControlPoint.Y
                _OuterControlPoint.Z = _ControlPoint.Z

            End If

            ' Normal:

            Dim V1x As Double = _Nodes(1).Position.X - _Nodes(0).Position.X
            Dim V1y As Double = _Nodes(1).Position.Y - _Nodes(0).Position.Y
            Dim V1z As Double = _Nodes(1).Position.Z - _Nodes(0).Position.Z

            Dim V2x As Double = _Nodes(2).Position.X - _Nodes(0).Position.X
            Dim V2y As Double = _Nodes(2).Position.Y - _Nodes(0).Position.Y
            Dim V2z As Double = _Nodes(2).Position.Z - _Nodes(0).Position.Z

            _Normal.X = V1y * V2z - V1z * V2y
            _Normal.Y = V1z * V2x - V1x * V2z
            _Normal.Z = V1x * V2y - V1y * V2x

            ' Area:

            Dim norm As Double = _Normal.EuclideanNorm

            _Area = 0.5 * norm

            _Normal.X /= norm
            _Normal.Y /= norm
            _Normal.Z /= norm

            If _Reversed Then _Normal.Oppose()

            If Not IsSlender Then

                _ControlPoint.X -= 0.0001 * _Normal.X
                _ControlPoint.Y -= 0.0001 * _Normal.Y
                _ControlPoint.Z -= 0.0001 * _Normal.Z

                _OuterControlPoint.X += 0.0001 * _Normal.X
                _OuterControlPoint.Y += 0.0001 * _Normal.Y
                _OuterControlPoint.Z += 0.0001 * _Normal.Z

            End If

            VelocityS.X = (_Nodes(0).Velocity.X + _Nodes(1).Velocity.X + _Nodes(2).Velocity.X) / 3
            VelocityS.Y = (_Nodes(0).Velocity.Y + _Nodes(1).Velocity.Y + _Nodes(2).Velocity.Y) / 3
            VelocityS.Z = (_Nodes(0).Velocity.Z + _Nodes(1).Velocity.Z + _Nodes(2).Velocity.Z) / 3

        End Sub

        ''' <summary>
        ''' Calculates the normal vector associated to the vortex ring.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RecalculateNormal() Implements VortexRing.RecalculateNormal

            ' Normal:

            Dim V1x As Double = _Nodes(1).Position.X - _Nodes(0).Position.X
            Dim V1y As Double = _Nodes(1).Position.Y - _Nodes(0).Position.Y
            Dim V1z As Double = _Nodes(1).Position.Z - _Nodes(0).Position.Z

            Dim V2x As Double = _Nodes(2).Position.X - _Nodes(0).Position.X
            Dim V2y As Double = _Nodes(2).Position.Y - _Nodes(0).Position.Y
            Dim V2z As Double = _Nodes(2).Position.Z - _Nodes(0).Position.Z

            _Normal.X = V1y * V2z - V1z * V2y
            _Normal.Y = V1z * V2x - V1x * V2z
            _Normal.Z = V1x * V2y - V1y * V2x

            ' Area:

            Dim norm As Double = _Normal.EuclideanNorm

            _Normal.X /= norm
            _Normal.Y /= norm
            _Normal.Z /= norm

            If _Reversed Then _Normal.Oppose()

        End Sub

#End Region

#Region " Velocity field "

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Function GiveDoubletVelocityInfluence(ByVal Point As EVector3, Optional ByVal CutOff As Double = 0.0001, Optional ByVal WithG As Boolean = True) As EVector3 Implements VortexRing.GiveDoubletVelocityInfluence

            Dim Vector As New EVector3

            AddDoubletVelocityInfluence(Vector, Point, CutOff, WithG)

            Return Vector

        End Function

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Sub AddDoubletVelocityInfluence(ByRef Vector As EVector3, ByVal Point As EVector3, Optional ByVal CutOff As Double = 0.0001, Optional ByVal WithG As Boolean = True) Implements VortexRing.AddDoubletVelocityInfluence

            Dim Den As Double
            Dim Num As Double

            Dim Node1 As EVector3 = Nothing
            Dim Node2 As EVector3 = Nothing

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim vx, vy, vz As Double
            Dim dx, dy, dz As Double

            Dim NR1 As Double
            Dim NR2 As Double
            Dim Factor As Double

            For i = 1 To 3

                Select Case i
                    Case 1
                        Node1 = Node(1).Position
                        Node2 = Node(2).Position
                    Case 2
                        Node1 = Node(2).Position
                        Node2 = Node(3).Position
                    Case 3
                        Node1 = Node(3).Position
                        Node2 = Node(1).Position
                End Select

                Lx = Node2.X - Node1.X
                Ly = Node2.Y - Node1.Y
                Lz = Node2.Z - Node1.Z

                R1x = Point.X - Node1.X
                R1y = Point.Y - Node1.Y
                R1z = Point.Z - Node1.Z

                vx = Ly * R1z - Lz * R1y
                vy = Lz * R1x - Lx * R1z
                vz = Lx * R1y - Ly * R1x

                Den = FourPi * (vx * vx + vy * vy + vz * vz)

                If Den > CutOff Then

                    ' Calculate the rest of the geometrical parameters:

                    R2x = Point.X - Node2.X
                    R2y = Point.Y - Node2.Y
                    R2z = Point.Z - Node2.Z

                    NR1 = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                    NR2 = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                    dx = NR1 * R1x - NR2 * R2x
                    dy = NR1 * R1y - NR2 * R2y
                    dz = NR1 * R1z - NR2 * R2z

                    If WithG Then
                        Num = G * (Lx * dx + Ly * dy + Lz * dz)
                    Else
                        Num = (Lx * dx + Ly * dy + Lz * dz)
                    End If

                    Factor = Num / Den

                    If Reversed Then Factor *= -1

                    Vector.X += Factor * vx
                    Vector.Y += Factor * vy
                    Vector.Z += Factor * vz

                End If

            Next

        End Sub

        ''' <summary>
        ''' Adds the influence of the source distribution in the velocity.
        ''' </summary>
        ''' <remarks></remarks>
        Sub AddSourceVelocityInfluence(ByRef Vector As EVector3, ByVal Point As EVector3, Optional ByVal CutOff As Double = 0.0001, Optional ByVal WithS As Boolean = True) Implements VortexRing.AddSourceVelocityInfluence

            Dim factor As Double = 1.0#

            If WithS Then
                factor = S
            End If

            PotentialFunctions.AddTriangularSourceVelocity(Point, _Nodes(0).Position, _Nodes(1).Position, _Nodes(2).Position, Vector, factor, True)

        End Sub

#Region " Other functions "

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Function BiotSavart_ViscousCore(ByVal Point As EVector3, Optional ByVal SquareCoreRadius As Double = 0.0001, Optional ByVal WithG As Boolean = True) As EVector3

            Dim BSVector As New EVector3

            Dim sqr_normLxR1 As Double
            Dim normL As Double
            Dim Num As Double

            Dim Node1 As EVector3 = Nothing
            Dim Node2 As EVector3 = Nothing

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim LxR1x, LxR1y, LxR1z As Double
            Dim dx, dy, dz As Double

            Dim normR1inv As Double
            Dim normR2inv As Double
            Dim Factor As Double

            For i = 1 To 3

                Select Case i
                    Case 1
                        Node1 = Node(1).Position
                        Node2 = Node(2).Position
                    Case 2
                        Node1 = Node(2).Position
                        Node2 = Node(3).Position
                    Case 3
                        Node1 = Node(3).Position
                        Node2 = Node(1).Position
                End Select

                Lx = Node2.X - Node1.X
                Ly = Node2.Y - Node1.Y
                Lz = Node2.Z - Node1.Z

                normL = Math.Sqrt(Lx * Lx + Ly * Ly + Lz * Lz)

                Lx /= normL
                Ly /= normL
                Lz /= normL

                R1x = Point.X - Node1.X
                R1y = Point.Y - Node1.Y
                R1z = Point.Z - Node1.Z

                LxR1x = Ly * R1z - Lz * R1y
                LxR1y = Lz * R1x - Lx * R1z
                LxR1z = Lx * R1y - Ly * R1x

                sqr_normLxR1 = LxR1x * LxR1x + LxR1y * LxR1y + LxR1z * LxR1z ' this is the square of the radius.

                If sqr_normLxR1 < VortexRing3.Epsilon Then ' when vortex and point are aligned, don't do anything.

                    Continue For

                Else

                    ' calculate the rest of the geometrical parameters:

                    R2x = Point.X - Node2.X
                    R2y = Point.Y - Node2.Y
                    R2z = Point.Z - Node2.Z

                    normR1inv = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                    normR2inv = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                    dx = normR1inv * R1x - normR2inv * R2x
                    dy = normR1inv * R1y - normR2inv * R2y
                    dz = normR1inv * R1z - normR2inv * R2z

                    If WithG Then
                        Num = G * (Lx * dx + Ly * dy + Lz * dz)
                    Else
                        Num = (Lx * dx + Ly * dy + Lz * dz)
                    End If

                    If sqr_normLxR1 > SquareCoreRadius Then ' when point is outside the viscous core, apply the Biot & Savart law

                        Factor = Num / (VortexRing3.FourPi * sqr_normLxR1)

                        BSVector.X += Factor * LxR1x
                        BSVector.Y += Factor * LxR1y
                        BSVector.Z += Factor * LxR1z

                    Else ' when inside the visvous-core, apply linear function

                        Factor = Num / (VortexRing3.FourPi * SquareCoreRadius)

                        BSVector.X += Factor * LxR1x
                        BSVector.Y += Factor * LxR1y
                        BSVector.Z += Factor * LxR1z

                    End If

                End If

            Next

            Return BSVector

        End Function

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Sub AddBiotSavartVector_ViscousCore(ByRef Vector As EVector3, ByVal Point As EVector3, Optional ByVal SquareCoreRadius As Double = 0.0001, Optional ByVal WithG As Boolean = True)

            Dim sqr_normLxR1 As Double
            Dim normL As Double
            Dim Num As Double

            Dim Node1 As EVector3 = Nothing
            Dim Node2 As EVector3 = Nothing

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim LxR1x, LxR1y, LxR1z As Double
            Dim dx, dy, dz As Double

            Dim normR1inv As Double
            Dim normR2inv As Double
            Dim Factor As Double

            For i = 1 To 3

                Select Case i
                    Case 1
                        Node1 = Node(1).Position
                        Node2 = Node(2).Position
                    Case 2
                        Node1 = Node(2).Position
                        Node2 = Node(3).Position
                    Case 3
                        Node1 = Node(3).Position
                        Node2 = Node(1).Position
                End Select

                Lx = Node2.X - Node1.X
                Ly = Node2.Y - Node1.Y
                Lz = Node2.Z - Node1.Z

                normL = Math.Sqrt(Lx * Lx + Ly * Ly + Lz * Lz)

                Lx /= normL
                Ly /= normL
                Lz /= normL

                R1x = Point.X - Node1.X
                R1y = Point.Y - Node1.Y
                R1z = Point.Z - Node1.Z

                LxR1x = Ly * R1z - Lz * R1y
                LxR1y = Lz * R1x - Lx * R1z
                LxR1z = Lx * R1y - Ly * R1x

                sqr_normLxR1 = LxR1x * LxR1x + LxR1y * LxR1y + LxR1z * LxR1z ' this is the square of the radius.

                If sqr_normLxR1 < VortexRing3.Epsilon Then ' when vortex and point are aligned, don't do anything.

                    Continue For

                Else

                    ' calculate the rest of the geometrical parameters:

                    R2x = Point.X - Node2.X
                    R2y = Point.Y - Node2.Y
                    R2z = Point.Z - Node2.Z

                    normR1inv = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                    normR2inv = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                    dx = normR1inv * R1x - normR2inv * R2x
                    dy = normR1inv * R1y - normR2inv * R2y
                    dz = normR1inv * R1z - normR2inv * R2z

                    If WithG Then
                        Num = G * (Lx * dx + Ly * dy + Lz * dz)
                    Else
                        Num = (Lx * dx + Ly * dy + Lz * dz)
                    End If

                    If sqr_normLxR1 > SquareCoreRadius Then ' when point is outside the viscous core, apply the Biot & Savart law

                        Factor = Num / (VortexRing3.FourPi * sqr_normLxR1)

                        Vector.X += Factor * LxR1x
                        Vector.Y += Factor * LxR1y
                        Vector.Z += Factor * LxR1z

                    Else ' when inside the visvous-core, apply linear function

                        Factor = Num / VortexRing3.FourPi * SquareCoreRadius

                        Vector.X += Factor * LxR1x
                        Vector.Y += Factor * LxR1y
                        Vector.Z += Factor * LxR1z

                    End If

                End If

            Next

        End Sub

        ''' <summary>
        ''' Computes the induced velocity at a given point by counting only the stramwise vortices
        ''' </summary>
        ''' <param name="Point">Point where influence is to be calculated</param>
        ''' <param name="N1">First streamwise segment</param>
        ''' <param name="N2">Second streamwise segment</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function StreamwiseInfluence(ByVal Point As EVector3, ByVal N1 As Integer, ByVal N2 As Integer, Optional ByVal CutOff As Double = 0.0001) As EVector3 Implements VortexRing.StreamwiseInfluence

            Return New EVector3

        End Function

#End Region

#End Region

#Region " Velocity potential "

        ''' <summary>
        ''' Returns the influence of the velocity in the potential.
        ''' </summary>
        ''' <param name="Point">Point influence wants to be calculated.</param>
        ''' <returns>The velocity potential influence coefficient.</returns>
        ''' <remarks></remarks>
        Public Function GiveDoubletPotentialInfluence(ByVal Point As EVector3, Optional ByVal WithG As Boolean = True) As Double Implements VortexRing.GiveDoubletPotentialInfluence

            Dim Potential As Double = PotentialFunctions.GetTriangularUnitDoubletPotential(Point, _Nodes(0).Position, _Nodes(1).Position, _Nodes(2).Position)

            If WithG Then Potential *= G

            If Me.Reversed Then Potential *= -1

            Return Potential

        End Function

        ''' <summary>
        ''' Returns the influence coefficient of the velocity potential.
        ''' </summary>
        ''' <param name="Point">Point influence wants to be calculated.</param>
        ''' <returns>The velocity potential influence coefficient.</returns>
        ''' <remarks></remarks>
        Public Function GiveSourcePotentialInfluence(ByVal Point As EVector3, Optional ByVal WithS As Boolean = True) As Double Implements VortexRing.GiveSourcePotentialInfluence

            Dim Potential As Double = PotentialFunctions.GetTriangularUnitSourcePotential(Point, _Nodes(0).Position, _Nodes(1).Position, _Nodes(2).Position)

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

        Private _SurroundingRings(2, 1) As VortexRing

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

        Private _SurroundingRingsSence(2, 1) As Int16

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

        Private _Gammas(2) As Double

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
        ''' <param name="VSqr">
        ''' Square of reference velocity Norm2.
        ''' </param>
        Public Sub CalculateCP(ByVal VSqr As Double) Implements VortexRing.CalculateCP

            If IsSlender Then

                ' Calculates vortices circulation (Gamma):

                For k = 0 To 2

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

                GammaX = 0.5 * ((Node(2).Position.X - Node(1).Position.X) * Gamma(0) + _
                                (Node(3).Position.X - Node(2).Position.X) * Gamma(1) + _
                                (Node(4).Position.X - Node(3).Position.X) * Gamma(2) + _
                                (Node(1).Position.X - Node(4).Position.X) * Gamma(3))

                GammaY = 0.5 * ((Node(2).Position.Y - Node(1).Position.Y) * Gamma(0) + _
                                (Node(3).Position.Y - Node(2).Position.Y) * Gamma(1) + _
                                (Node(4).Position.Y - Node(3).Position.Y) * Gamma(2) + _
                                (Node(1).Position.Y - Node(4).Position.Y) * Gamma(3))

                GammaZ = 0.5 * ((Node(2).Position.Z - Node(1).Position.Z) * Gamma(0) + _
                                (Node(3).Position.Z - Node(2).Position.Z) * Gamma(1) + _
                                (Node(4).Position.Z - Node(3).Position.Z) * Gamma(2) + _
                                (Node(1).Position.Z - Node(4).Position.Z) * Gamma(3))

                ' Calculates DV (not divided by the area):

                Dim dVx, dVy, dVz As Double

                dVx = GammaY * Normal.Z - GammaZ * Normal.Y
                dVy = GammaZ * Normal.X - GammaX * Normal.Z
                dVz = GammaX * Normal.Y - GammaY * Normal.X

                Cp = 2 * ((dVx * VelocityT.X + dVy * VelocityT.Y + dVz * VelocityT.Z) / Area + DGdt) / VSqr

            Else

                Cp = 1 - ((VelocityT.X * VelocityT.X + VelocityT.Y * VelocityT.Y + VelocityT.Z * VelocityT.Z)) / VSqr ' + 2 * DGdt

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
            _SurroundingRings(0, 1) = Nothing
            _SurroundingRings(1, 1) = Nothing
            _SurroundingRings(2, 1) = Nothing
        End Sub

#End Region

    End Class

End Namespace
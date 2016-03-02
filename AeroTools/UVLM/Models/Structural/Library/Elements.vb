'Copyright (C) 2016 Guillermo Hazebrouck

Imports MathTools.Algebra.EuclideanSpace
Imports Meta.Numerics.Matrices

Namespace UVLM.Models.Structural.Library

    ''' <summary>
    ''' Nodal constrains
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Constrains

        Public Fixed(5) As Boolean
        Public K(5) As Double

        Public Property FixedDx As Boolean
            Set(ByVal value As Boolean)
                Fixed(0) = value
            End Set
            Get
                Return Fixed(0)
            End Get
        End Property

        Public Property FixedDy As Boolean
            Set(ByVal value As Boolean)
                Fixed(1) = value
            End Set
            Get
                Return Fixed(1)
            End Get
        End Property

        Public Property FixedDz As Boolean
            Set(ByVal value As Boolean)
                Fixed(2) = value
            End Set
            Get
                Return Fixed(2)
            End Get
        End Property

        Public Property FixedRx As Boolean
            Set(ByVal value As Boolean)
                Fixed(3) = value
            End Set
            Get
                Return Fixed(3)
            End Get
        End Property

        Public Property FixedRy As Boolean
            Set(ByVal value As Boolean)
                Fixed(4) = value
            End Set
            Get
                Return Fixed(4)
            End Get
        End Property

        Public Property FixedRz As Boolean
            Set(ByVal value As Boolean)
                Fixed(5) = value
            End Set
            Get
                Return Fixed(5)
            End Get
        End Property

        ''' <summary>
        ''' Stiffness in x direction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KDx As Double
            Set(ByVal value As Double)
                K(0) = value
            End Set
            Get
                Return K(0)
            End Get
        End Property

        ''' <summary>
        ''' Stiffness in y direction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KDy As Double
            Set(ByVal value As Double)
                K(1) = value
            End Set
            Get
                Return K(1)
            End Get
        End Property

        ''' <summary>
        ''' Stiffness in z direction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KDz As Double
            Set(ByVal value As Double)
                K(2) = value
            End Set
            Get
                Return K(2)
            End Get
        End Property

        ''' <summary>
        ''' Stiffness arround x direction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KRx As Double
            Set(ByVal value As Double)
                K(3) = value
            End Set
            Get
                Return K(3)
            End Get
        End Property

        ''' <summary>
        ''' Stiffness arround y direction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KRy As Double
            Set(ByVal value As Double)
                K(4) = value
            End Set
            Get
                Return K(4)
            End Get
        End Property

        ''' <summary>
        ''' Stiffness arround z direction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property KRz As Double
            Set(ByVal value As Double)
                K(5) = value
            End Set
            Get
                Return K(5)
            End Get
        End Property

        ''' <summary>
        ''' Fixes all degrees of freedom to the base reference frame
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Clamped()

            For i = 0 To 5
                Fixed(i) = True
            Next

        End Sub

    End Class

    ''' <summary>
    ''' Structural node
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralNode

        Public Position As EVector3

        Public Contrains As Constrains
        Public Load As NodalLoad
        Public Displacement As NodalDisplacement
        Public Velocity As NodalDisplacement

        Private _Index As Integer

        ''' <summary>
        ''' Node global index
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Index As Integer
            Get
                Return _Index
            End Get
        End Property

        ''' <summary>
        ''' Creates a new structural node
        ''' </summary>
        ''' <param name="Index">Global index</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Index As Integer)
            _Index = Index
            Position = New EVector3
            Displacement = New NodalDisplacement
            Velocity = New NodalDisplacement
            Contrains = New Constrains
            Load = New NodalLoad
        End Sub

    End Class

    ''' <summary>
    ''' Defines a beam element
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BeamElement

        Public Property NodeA As StructuralNode
        Public Property NodeB As StructuralNode
        Public Property Section As Section
        Public Property Basis As EBase3
        Public Property Index As Integer

        Public Property M As SquareMatrix
        Public Property K As SquareMatrix

        Public MustOverride Sub GenerateLocalMass()
        Public MustOverride Sub GenerateLocalStiffness()
        Public MustOverride Sub GenerateGlobalMatrices()

    End Class

    ''' <summary>
    ''' Models a structural beam element of constant section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class GeneralBeamElement

        Inherits BeamElement

        Public Sub New(ByVal Index As Integer)
            Me.Index = Index
            M = New SquareMatrix(12)
            K = New SquareMatrix(12)
            Section = New Section()
            Basis = New EBase3()
        End Sub

        ''' <summary>
        ''' Generates the element mass matrix in local coordinates
        ''' </summary>
        ''' <remarks>Current version only supports non-excentrical section</remarks>
        Public Overrides Sub GenerateLocalMass()

            ' Assemblies consistent mass matrix: axes crossing CG (first moments of inertia = 0)

            Dim L As Double = NodeA.Position.DistanceTo(NodeB.Position)

            Dim m_L1 As Double = Section.m * L
            Dim m_L2 As Double = Section.m * L ^ 2
            Dim m_L3 As Double = Section.m * L ^ 3
            Dim r_J_L As Double = Section.rIp * L

            ' Traction on local X

            M(0, 0) = m_L1 / 3
            M(0, 6) = m_L1 / 6

            M(6, 6) = m_L1 / 3

            ' Torsion on local X

            M(3, 3) = r_J_L / 3
            M(3, 9) = r_J_L / 6

            M(9, 9) = r_J_L / 3

            ' Flexion on local YX

            M(1, 1) = 13 * m_L1 / 35
            M(1, 5) = 11 * m_L2 / 210
            M(1, 7) = 9 * m_L1 / 70
            M(1, 11) = -13 * m_L2 / 420

            M(5, 5) = m_L3 / 105
            M(5, 7) = 13 * m_L2 / 410
            M(5, 11) = -m_L3 / 140

            M(7, 1) = 9 * m_L1 / 70
            M(7, 5) = 13 * m_L2 / 410
            M(7, 7) = 13 * m_L1 / 35
            M(7, 11) = -11 * m_L2 / 210

            M(11, 1) = -13 * m_L2 / 420
            M(11, 5) = m_L3 / 140
            M(11, 7) = -11 * m_L2 / 210
            M(11, 11) = m_L3 / 105

            ' Flexion on local ZX

            M(2, 2) = 13 * m_L1 / 35
            M(2, 4) = -11 * m_L2 / 210
            M(2, 8) = 9 * m_L1 / 70
            M(2, 10) = 13 * m_L2 / 420

            M(4, 4) = m_L3 / 105
            M(4, 8) = -13 * m_L2 / 410
            M(4, 10) = -m_L3 / 140

            M(8, 8) = 13 * m_L1 / 35
            M(8, 10) = -11 * m_L2 / 210

            M(10, 10) = m_L3 / 105

            ' Excentrical mass:

            Dim Sy As Double = Section.m * Section.CMy
            Dim Sy_L1 As Double = Section.m * Section.CMy * L
            Dim Sy_L2 As Double = Section.m * Section.CMy * L * L

            Dim Sz As Double = Section.m * Section.CMz
            Dim Sz_L1 As Double = Section.m * Section.CMz * L
            Dim Sz_L2 As Double = Section.m * Section.CMz * L * L

            M(0, 1) = Sy / 2
            M(0, 2) = -Sz / 2
            M(0, 4) = Sz_L1 / 12
            M(0, 5) = -Sy_L1 / 12
            M(0, 7) = -Sy / 2
            M(0, 8) = Sz / 2
            M(0, 10) = -Sz_L1 / 12
            M(0, 11) = Sy_L1 / 12

            M(1, 3) = -7 * Sz_L1 / 20
            M(1, 6) = Sy / 2
            M(1, 9) = -3 * Sz_L1 / 20

            M(2, 3) = 7 * Sy_L1 / 20
            M(2, 6) = -Sz / 2
            M(2, 9) = 3 * Sy_L1 / 20

            M(3, 4) = -Sy_L2 / 20
            M(3, 5) = -Sz_L2 / 20
            M(3, 7) = -3 * Sz_L1 / 20
            M(3, 8) = 3 * Sy_L1 / 20
            M(3, 10) = Sy_L2 / 30
            M(3, 11) = Sz_L2 / 30

            M(4, 6) = -Sz_L1 / 12
            M(4, 9) = Sy_L2 / 30

            M(5, 6) = Sy_L1 / 12
            M(5, 9) = -Sz_L2 / 30

            M(6, 7) = -Sy / 2
            M(6, 8) = Sz / 2
            M(6, 10) = Sz_L1 / 12
            M(6, 11) = -Sy_L1 / 12

            M(7, 9) = -7 * Sz_L1 / 20

            M(8, 9) = 7 * Sy_L1 / 20

            M(9, 10) = Sy_L2 / 20
            M(9, 11) = Sz_L2 / 20

            ' Generate symetric matrix:

            For i = 0 To 11

                For j = i + 1 To 11

                    M(j, i) = M(i, j)

                Next

            Next

        End Sub

        ''' <summary>
        ''' Generates the element stiffness matrix in local coordinates
        ''' </summary>
        ''' <remarks>Current version only supports non-excentrical section</remarks>
        Public Overrides Sub GenerateLocalStiffness()

            Dim L As Double = NodeA.Position.DistanceTo(NodeB.Position)
            Dim EA_L As Double = Section.AE / L
            Dim GJ_L As Double = Section.GJ / L
            Dim EIw_L1 As Double = Section.EIz / L
            Dim EIw_L2 As Double = Section.EIz / L ^ 2
            Dim EIw_L3 As Double = Section.EIz / L ^ 3
            Dim EIv_L1 As Double = Section.EIy / L
            Dim EIv_L2 As Double = Section.EIy / L ^ 2
            Dim EIv_L3 As Double = Section.EIy / L ^ 3

            ' Traction on local X

            K(0, 0) = EA_L
            K(0, 6) = -EA_L

            K(6, 6) = EA_L

            ' Torsion on local X

            K(3, 3) = GJ_L
            K(3, 9) = -GJ_L

            K(9, 9) = GJ_L

            ' Flexion on local YX

            K(1, 1) = 12 * EIw_L3
            K(1, 5) = 6 * EIw_L2
            K(1, 7) = -12 * EIw_L3
            K(1, 11) = 6 * EIw_L2

            K(5, 5) = 4 * EIw_L1
            K(5, 7) = -6 * EIw_L2
            K(5, 11) = 2 * EIw_L1

            K(7, 7) = 12 * EIw_L3
            K(7, 11) = -6 * EIw_L2

            K(11, 11) = 4 * EIw_L1

            ' Flexion on local ZX

            K(2, 2) = 12 * EIv_L3
            K(2, 4) = -6 * EIv_L2
            K(2, 8) = -12 * EIv_L3
            K(2, 10) = -6 * EIv_L2

            K(4, 4) = 4 * EIv_L1
            K(4, 8) = 6 * EIv_L2
            K(4, 10) = 2 * EIv_L1

            K(8, 8) = 12 * EIv_L3
            K(8, 10) = 6 * EIv_L2

            K(10, 10) = 4 * EIv_L1

            ' Generate symetric matrix:

            For i = 0 To 11

                For j = i + 1 To 11

                    K(j, i) = K(i, j)

                Next

            Next

        End Sub

        ''' <summary>
        ''' Orientates local mass and stiffness matrices to global directions
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub GenerateGlobalMatrices()

            ' The local basis has been loaded before starting the calculation.

            GenerateLocalMass()
            GenerateLocalStiffness()

            ' Transform coordinates:

            Dim T As SquareMatrix = New SquareMatrix(12)

            For i = 0 To 1

                Dim bIndx As Integer = 6 * i

                T(0 + bIndx, 0 + bIndx) = Basis.U.X
                T(0 + bIndx, 1 + bIndx) = Basis.U.Y
                T(0 + bIndx, 2 + bIndx) = Basis.U.Z
                T(1 + bIndx, 0 + bIndx) = Basis.V.X
                T(1 + bIndx, 1 + bIndx) = Basis.V.Y
                T(1 + bIndx, 2 + bIndx) = Basis.V.Z
                T(2 + bIndx, 0 + bIndx) = Basis.W.X
                T(2 + bIndx, 1 + bIndx) = Basis.W.Y
                T(2 + bIndx, 2 + bIndx) = Basis.W.Z

                T(3 + bIndx, 3 + bIndx) = Basis.U.X
                T(3 + bIndx, 4 + bIndx) = Basis.U.Y
                T(3 + bIndx, 5 + bIndx) = Basis.U.Z
                T(4 + bIndx, 3 + bIndx) = Basis.V.X
                T(4 + bIndx, 4 + bIndx) = Basis.V.Y
                T(4 + bIndx, 5 + bIndx) = Basis.V.Z
                T(5 + bIndx, 3 + bIndx) = Basis.W.X
                T(5 + bIndx, 4 + bIndx) = Basis.W.Y
                T(5 + bIndx, 5 + bIndx) = Basis.W.Z

            Next

            K = T.Transpose * (K * T)
            M = T.Transpose * (M * T)

            'Dim p As String = "C:\Users\Guillermo\Documents\Vogel tests\Aeroelasticity"
            'Dim f As Integer = 101
            'FileOpen(f, p & "\K_element_" & ID & ".txt", OpenMode.Output, OpenAccess.Write)
            'Print(f, K.__repr__())
            'FileClose(f)

        End Sub

    End Class

End Namespace

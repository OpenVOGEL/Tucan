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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports DotNumerics.LinearAlgebra

Namespace CalculationModel.Models.Structural.Library.Elements

    ''' <summary>
    ''' Gathers global section porperties for a beam element.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Section

        ''' <summary>
        ''' Longitudinal stiffness [N/m]
        ''' </summary>
        ''' <remarks></remarks>
        Public AE As Double = 1

        ''' <summary>
        ''' Torsional rigidity [Nm/rad]
        ''' </summary>
        ''' <remarks></remarks>
        Public GJ As Double = 10

        ''' <summary>
        ''' Flexional rigidity of inertia around local axis y [Nm/rad]
        ''' </summary>
        ''' <remarks></remarks>
        Public EIy As Double = 10

        ''' <summary>
        ''' Flexional rigidity of inertia around local axis z [Nm/rad]
        ''' </summary>
        ''' <remarks></remarks>
        Public EIz As Double = 10

        ''' <summary>
        ''' Torsional moment of inertia [kgm²]
        ''' </summary>
        ''' <remarks></remarks>
        Public rIp As Double = 1.0

        ''' <summary>
        ''' Mass per unit length [kg/m]
        ''' </summary>
        ''' <remarks></remarks>
        Public m As Double = 1.0

        ''' <summary>
        ''' Y coordinate of center of mass [m]
        ''' </summary>
        ''' <remarks></remarks>
        Public CMy As Double = 0.0

        ''' <summary>
        ''' Z coordinate of center of mass [m]
        ''' </summary>
        ''' <remarks></remarks>
        Public CMz As Double = 0.0

        Public Sub Assign(ByVal Section As Section)

            AE = Section.AE
            CMy = Section.CMy
            CMz = Section.CMz
            GJ = Section.GJ
            EIy = Section.EIy
            EIz = Section.EIz
            rIp = Section.rIp
            m = Section.m

        End Sub

    End Class

    ''' <summary>
    ''' Represents a beam element
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BeamElement

        Implements IFiniteElement

        Public Property Nodes As StructuralNode() Implements IFiniteElement.Nodes

        Public Property NodeA As StructuralNode
            Set(value As StructuralNode)
                Nodes(0) = value
            End Set
            Get
                Return Nodes(0)
            End Get
        End Property

        Public Property NodeB As StructuralNode
            Set(value As StructuralNode)
                Nodes(1) = value
            End Set
            Get
                Return Nodes(1)
            End Get
        End Property

        Public Property Section As Section
        Public Property Basis As Base3
        Public Property Index As Integer

        Public Property M As SymmetricMatrix Implements IFiniteElement.M
        Public Property K As SymmetricMatrix Implements IFiniteElement.K

        Public MustOverride Sub GenerateLocalMass() Implements IFiniteElement.GenerateLocalMass
        Public MustOverride Sub GenerateLocalStiffness() Implements IFiniteElement.GenerateLocalStiffness
        Public MustOverride Sub GenerateGlobalMatrices() Implements IFiniteElement.GenerateGlobalMatrices

        Public Sub New()

            ReDim Nodes(1)

        End Sub

    End Class

    ''' <summary>
    ''' Models a structural beam element of constant section
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ConstantBeamElement

        Inherits BeamElement

        Public Sub New(ByVal Index As Integer)
            Me.Index = Index
            M = New SymmetricMatrix(12)
            K = New SymmetricMatrix(12)
            Section = New Section()
            Basis = New Base3()
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

            M(0, 0) = m_L1 / 3
            M(0, 6) = m_L1 / 6

            M(1, 1) = 13 * m_L1 / 35
            M(1, 5) = 11 * m_L2 / 210
            M(1, 7) = 9 * m_L1 / 70
            M(1, 11) = -13 * m_L2 / 420

            M(2, 2) = 13 * m_L1 / 35
            M(2, 4) = -11 * m_L2 / 210
            M(2, 8) = 9 * m_L1 / 70
            M(2, 10) = 13 * m_L2 / 420

            M(3, 3) = r_J_L / 3
            M(3, 9) = r_J_L / 6

            M(4, 4) = m_L3 / 105
            M(4, 8) = -13 * m_L2 / 420
            M(4, 10) = -m_L3 / 140

            M(5, 5) = m_L3 / 105
            M(5, 7) = 13 * m_L2 / 420
            M(5, 11) = -m_L3 / 140

            M(6, 6) = m_L1 / 3

            M(7, 7) = 13 * m_L1 / 35
            M(7, 11) = -11 * m_L2 / 210

            M(8, 8) = 13 * m_L1 / 35
            M(8, 10) = 11 * m_L2 / 210

            M(9, 9) = r_J_L / 3

            M(10, 10) = m_L3 / 105

            M(11, 11) = m_L3 / 105

            ' Excentrical mass:

            If Math.Abs(Section.CMy) > 0 Or Math.Abs(Section.CMz) > 0 Then

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

            End If

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

            Dim T As Matrix = New Matrix(12)

            For i = 0 To 3

                Dim bIndx As Integer = 3 * i

                T(0 + bIndx, 0 + bIndx) = Basis.U.X
                T(0 + bIndx, 1 + bIndx) = Basis.U.Y
                T(0 + bIndx, 2 + bIndx) = Basis.U.Z
                T(1 + bIndx, 0 + bIndx) = Basis.V.X
                T(1 + bIndx, 1 + bIndx) = Basis.V.Y
                T(1 + bIndx, 2 + bIndx) = Basis.V.Z
                T(2 + bIndx, 0 + bIndx) = Basis.W.X
                T(2 + bIndx, 1 + bIndx) = Basis.W.Y
                T(2 + bIndx, 2 + bIndx) = Basis.W.Z

            Next

            K = K.SymmetricTransformation(T)
            M = M.SymmetricTransformation(T)

            'Dim p As String = "C:\Users\Guillermo\Documents\Vogel tests\Aeroelasticity"
            'Dim f As Integer = 101
            'FileOpen(f, p & "\K_element_" & ID & ".txt", OpenMode.Output, OpenAccess.Write)
            'Print(f, K.__repr__())
            'FileClose(f)

        End Sub

    End Class

End Namespace

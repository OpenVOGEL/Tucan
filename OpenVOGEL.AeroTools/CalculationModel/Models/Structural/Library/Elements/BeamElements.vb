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

'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports DotNumerics.LinearAlgebra

'#############################################################################
' Unit: BeamElements
'
' This unit provides a complete structural dynamic module based on beam
' elements.
'#############################################################################
Namespace CalculationModel.Models.Structural.Library.Elements

    ''' <summary>
    ''' Gathers the global section porperties for a beam element.
    ''' Some of the propertis are elastic and other inertial. The information is used to generate
    ''' the mass and stiffness matrices.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Section

        ''' <summary>
        ''' Longitudinal stiffness [N/m]
        ''' </summary>
        Public AE As Double = 1

        ''' <summary>
        ''' Torsional rigidity [N.m/rad]
        ''' </summary>
        Public GJ As Double = 10

        ''' <summary>
        ''' Flexional rigidity of inertia around local axis y [N.m/rad]
        ''' </summary>
        Public EIy As Double = 10

        ''' <summary>
        ''' Flexional rigidity of inertia around local axis z [N.m/rad]
        ''' </summary>
        Public EIz As Double = 10

        ''' <summary>
        ''' Torsional moment of inertia [kg.m]
        ''' </summary>
        Public Ip As Double = 1.0

        ''' <summary>
        ''' Mass per unit length [kg/m]
        ''' </summary>
        Public M As Double = 1.0

        ''' <summary>
        ''' Y coordinate of center of mass in relation to the beam axis (center of shear) [m]
        ''' The local Y direction follows the basis vector V.
        ''' </summary>
        Public Cmy As Double = 0.0

        ''' <summary>
        ''' Z coordinate of center of mass in relation to the beam axis (center of shear) [m]
        ''' The local Z direction follows the basis vector W.
        ''' </summary>
        Public Cmz As Double = 0.0

        ''' <summary>
        ''' Copies the section properties from another object.
        ''' </summary>
        ''' <param name="Section"></param>
        Public Sub Assign(ByVal Section As Section)

            AE = Section.AE
            GJ = Section.GJ
            EIy = Section.EIy
            EIz = Section.EIz
            Ip = Section.Ip
            M = Section.M
            Cmy = Section.Cmy
            Cmz = Section.Cmz

        End Sub

        ''' <summary>
        ''' Obtains the mean properties of two sections.
        ''' </summary>
        ''' <param name="Section1"></param>
        ''' <param name="Section2"></param>
        Public Sub Combine(Section1 As Section, Section2 As Section)

            AE = 0.5 * (Section1.AE + Section2.AE)
            GJ = 0.5 * (Section1.GJ + Section2.GJ)
            EIy = 0.5 * (Section1.EIy + Section2.EIy)
            EIz = 0.5 * (Section1.EIz + Section2.EIz)
            Ip = 0.5 * (Section1.Ip + Section2.Ip)
            M = 0.5 * (Section1.M + Section2.M)
            Cmy = 0.5 * (Section1.Cmy + Section2.Cmy)
            Cmz = 0.5 * (Section1.Cmz + Section2.Cmz)

        End Sub

    End Class

    ''' <summary>
    ''' Represents the internal loads in a beam element
    ''' </summary>
    Public Class InternalLoads

        ''' <summary>
        ''' The force along axis X
        ''' </summary>
        ''' <returns></returns>
        Public Property Fx As Double

        ''' <summary>
        ''' The shear force along axis Y
        ''' </summary>
        ''' <returns></returns>
        Public Property Fy As Double

        ''' <summary>
        ''' The shear force along axis X
        ''' </summary>
        ''' <returns></returns>
        Public Property Fz As Double

        ''' <summary>
        ''' The torsional moment along axis X at node A
        ''' </summary>
        ''' <returns></returns>
        Public Property MAx As Double

        ''' <summary>
        ''' The bending moment along axis Y at node A
        ''' </summary>
        ''' <returns></returns>
        Public Property MAy As Double

        ''' <summary>
        ''' The bending moment along axis Z at node A
        ''' </summary>
        ''' <returns></returns>
        Public Property MAz As Double

        ''' <summary>
        ''' The torsional moment along axis X at node B
        ''' </summary>
        ''' <returns></returns>
        Public Property MBx As Double

        ''' <summary>
        ''' The bending moment along axis Y at node B
        ''' </summary>
        ''' <returns></returns>
        Public Property MBy As Double

        ''' <summary>
        ''' The bending moment along axis Z at node B
        ''' </summary>
        ''' <returns></returns>
        Public Property MBz As Double

    End Class

    ''' <summary>
    ''' Represents a generic beam element.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BeamElement

        Implements IFiniteElement

        ''' <summary>
        ''' A given structural node.
        ''' </summary>
        Public Property Nodes As StructuralNode() Implements IFiniteElement.Nodes

        ''' <summary>
        ''' The first node.
        ''' </summary>
        Public Property NodeA As StructuralNode
            Set(value As StructuralNode)
                Nodes(0) = value
            End Set
            Get
                Return Nodes(0)
            End Get
        End Property

        ''' <summary>
        ''' The second node.
        ''' </summary>
        Public Property NodeB As StructuralNode
            Set(value As StructuralNode)
                Nodes(1) = value
            End Set
            Get
                Return Nodes(1)
            End Get
        End Property

        ''' <summary>
        ''' The beam section properties.
        ''' </summary>
        Public Property Section As Section

        ''' <summary>
        ''' The basis providing the local directions as follows:
        ''' X -> U
        ''' Y -> V
        ''' Z -> W
        ''' </summary>
        Public Property Basis As Base3

        ''' <summary>
        ''' The index of the element.
        ''' </summary>
        Public Property Index As Integer

        ''' <summary>
        ''' The mass matrix.
        ''' </summary>
        Public Property M As SymmetricMatrix Implements IFiniteElement.M

        ''' <summary>
        ''' The stiffness matrix.
        ''' </summary>
        Public Property K As SymmetricMatrix Implements IFiniteElement.K

        ''' <summary>
        ''' The generic procedure that generates the local mass matrix.
        ''' </summary>
        Public MustOverride Sub GenerateLocalMass() Implements IFiniteElement.GenerateLocalMass

        ''' <summary>
        ''' The generic procedure that generates the local stiffness matrix.
        ''' </summary>
        Public MustOverride Sub GenerateLocalStiffness() Implements IFiniteElement.GenerateLocalStiffness

        ''' <summary>
        ''' The generic procedure that generates the global mass and stiffness matrices.
        ''' </summary>
        Public MustOverride Sub GenerateGlobalMatrices() Implements IFiniteElement.GenerateGlobalMatrices

        Public Sub New()

            ReDim Nodes(1)

        End Sub

    End Class

    ''' <summary>
    ''' Represents a hing (a rigid connection between two nodes with no rotational stiffness)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class HingedLink

        Inherits BeamElement

        Public Sub New()
            M = New SymmetricMatrix(12)
            K = New SymmetricMatrix(12)
        End Sub

        ''' <summary>
        ''' The normal stiffness of the connection
        ''' </summary>
        ''' <returns></returns>
        Public Property Stiffness As Double = 1000000000.0 ' [N/m]

        ''' <summary>
        ''' This does nothing because this element has no mass
        ''' </summary>
        Public Overrides Sub GenerateLocalMass()

            ' This element has no mass

        End Sub

        ''' <summary>
        ''' This does nothing because this element has no mass
        ''' </summary>
        Public Overrides Sub GenerateLocalStiffness()

            K(0, 0) = Stiffness
            K(0, 6) = -Stiffness
            K(6, 6) = Stiffness

            K(1, 1) = Stiffness
            K(1, 7) = -Stiffness
            K(7, 7) = Stiffness

            K(2, 2) = Stiffness
            K(2, 8) = -Stiffness
            K(8, 8) = Stiffness

        End Sub

        Public Overrides Sub GenerateGlobalMatrices()

            GenerateLocalMass()
            GenerateLocalStiffness()

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
            InternalLoads = New InternalLoads
        End Sub

        ''' <summary>
        ''' The internal loads
        ''' </summary>
        ''' <returns></returns>
        Public Property InternalLoads As InternalLoads

        ''' <summary>
        ''' Returns the length of the beam
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property Length As Double
            Get
                If NodeA IsNot Nothing And NodeB IsNot Nothing Then
                    Return NodeA.Position.DistanceTo(NodeB.Position)
                Else
                    Return 0#
                End If
            End Get
        End Property

        ''' <summary>
        ''' Generates a standard basis for this element (not enforced)
        ''' </summary>
        Public Sub GenerateStandardBasis()

            If NodeA IsNot Nothing And NodeB IsNot Nothing Then

                ' U goes from A to B
                Basis.U.FromSubstraction(NodeB.Position, NodeA.Position)
                Basis.U.Normalize()

                ' V is normal to the projection of U on the 'horizontal' plane (XY)
                ' NOTE: this is singular for vertical beams
                Basis.V.X = -Basis.U.Y
                Basis.V.Y = Basis.U.X

                If Basis.V.EuclideanNorm < 0.001 Then
                    Basis.V.X = 1.0
                    Basis.V.Y = 0.0
                End If

                Basis.V.Normalize()

                ' W is normal to U and V
                Basis.W.FromVectorProduct(Basis.U, Basis.V)
                Basis.W.Normalize()

            End If

        End Sub

        ''' <summary>
        ''' Generates the element mass matrix in local coordinates.
        ''' The model is consistent (not lumped), which means that the kinematics of the beam is described by the shape functions. 
        ''' The second moment of inertia in flexion are disregarded (Izz=Iyy=Iyz=0). Because Iyz does not participate, the main
        ''' elastic and inertial directions always coincide. Iyy and Izz might be added in the future if necessary.
        ''' The eccentricty of the center of mass in relation to the beam axis (center of shear) is taken into account by the
        ''' Cmy and Cmz coordinates.
        ''' </summary>
        Public Overrides Sub GenerateLocalMass()

            ' Concentrical component
            '-------------------------------------------

            Dim L As Double = NodeA.Position.DistanceTo(NodeB.Position)

            Dim m_L1 As Double = Section.M * L
            Dim m_L2 As Double = Section.M * L ^ 2
            Dim m_L3 As Double = Section.M * L ^ 3
            Dim r_J_L As Double = Section.Ip * L

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

            ' Eccentrical component
            '-------------------------------------------

            If Math.Abs(Section.Cmy) > 0 Or Math.Abs(Section.Cmz) > 0 Then

                Dim Sy As Double = Section.M * Section.Cmz
                Dim Sy_L1 As Double = Section.M * Section.Cmz * L
                Dim Sy_L2 As Double = Section.M * Section.Cmz * L * L

                Dim Sz As Double = Section.M * Section.Cmy
                Dim Sz_L1 As Double = Section.M * Section.Cmy * L
                Dim Sz_L2 As Double = Section.M * Section.Cmy * L * L

                M(0, 1) = Sz / 2.0
                M(0, 2) = Sy / 2.0
                M(0, 4) = Sy_L1 / 12.0
                M(0, 5) = -Sz_L1 / 12.0
                M(0, 7) = -Sz / 2.0
                M(0, 8) = -Sy / 2.0
                M(0, 10) = -Sy_L1 / 12.0
                M(0, 11) = Sz_L1 / 12.0

                M(1, 3) = -Sy_L1 * 7.0 / 20.0
                M(1, 6) = Sz / 2.0
                M(1, 9) = -Sy_L1 * 3.0 / 20.0

                M(2, 3) = Sz_L1 * 7.0 / 20.0
                M(2, 6) = Sy / 2.0
                M(2, 9) = Sz_L1 * 3.0 / 20.0

                M(3, 4) = -Sz_L2 / 20.0
                M(3, 5) = -Sy_L2
                M(3, 7) = -Sy_L1 * 3.0 / 20.0
                M(3, 8) = Sz_L1 * 3.0 / 20.0
                M(3, 10) = Sz_L2 / 30.0
                M(3, 11) = Sy_L2 / 30.0

                M(4, 6) = -Sy_L1 / 12.0
                M(4, 9) = -Sz_L2 / 30.0

                M(5, 6) = Sz_L1 / 12.0
                M(5, 9) = -Sy_L2 / 30.0

                M(6, 7) = -Sz / 2.0
                M(6, 8) = -Sy / 2.0
                M(6, 10) = Sy_L1 / 12.0
                M(6, 11) = -Sz_L1 / 12.0

                M(7, 9) = -Sy_L1 * 7.0 / 20.0

                M(9, 10) = Sz_L2 / 20.0
                M(9, 11) = Sy_L2 / 20.0

            End If

        End Sub

        ''' <summary>
        ''' Generates the element stiffness matrix in local coordinates.
        ''' The kinematics of the beam is described using the standard shape functions.
        ''' The second cross-moments of inertia are disregarded, which means that the local axes coincide with the main elastic directions.
        ''' The first moments of inertia are also disregarded, which means that the beam axis is at the center of shear.
        ''' </summary>
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
            '-------------------------------------------

            K(0, 0) = EA_L
            K(0, 6) = -EA_L

            K(6, 6) = EA_L

            ' Torsion on local X
            '-------------------------------------------

            K(3, 3) = GJ_L
            K(3, 9) = -GJ_L

            K(9, 9) = GJ_L

            ' Flexion on local YX
            '-------------------------------------------

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
            '-------------------------------------------

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
        ''' Generates the glbal mass and stiffness matrices.
        ''' This procedure first generates the local matrices and immediatly after converts them to global
        ''' by means of a linear transformation using the local basis.
        ''' </summary>
        Public Overrides Sub GenerateGlobalMatrices()

            ' The local basis has been loaded before starting the calculation
            '----------------------------------------------------------------

            GenerateLocalMass()

            GenerateLocalStiffness()

            ' Transform coordinates to the global system
            '----------------------------------------------------------------

            Dim T As Matrix = New Matrix(12)

            For I = 0 To 3

                Dim K As Integer = 3 * I

                T(0 + K, 0 + K) = Basis.U.X
                T(0 + K, 1 + K) = Basis.V.X
                T(0 + K, 2 + K) = Basis.W.X
                T(1 + K, 0 + K) = Basis.U.Y
                T(1 + K, 1 + K) = Basis.V.Y
                T(1 + K, 2 + K) = Basis.W.Y
                T(2 + K, 0 + K) = Basis.U.Z
                T(2 + K, 1 + K) = Basis.V.Z
                T(2 + K, 2 + K) = Basis.W.Z

            Next

            K = K.SymmetricTransformation(T)

            M = M.SymmetricTransformation(T)

        End Sub

    End Class

End Namespace

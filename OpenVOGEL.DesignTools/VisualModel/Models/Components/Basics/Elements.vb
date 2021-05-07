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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.VisualModel.Tools.Colormaping

Namespace VisualModel.Models.Components.Basics

    ''' <summary>
    ''' Represents a panel (both, triangular or quadrialteral) on the model.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Panel

        ''' <summary>
        '''  Creates a new panel
        ''' </summary>
        Public Sub New()
            ControlPoint = New Vector3
            NormalVector = New Vector3
            LocalVelocity = New Vector3
            Area = 0.0#
            Circulation = 0.0#
            Cp = 0.0#
            GlobalIndex = 0
            IsReversed = False
            IsPrimitive = False
        End Sub

        ''' <summary>
        ''' The index of the first node
        ''' </summary>
        ''' <returns></returns>
        Public Property N1 As Integer

        ''' <summary>
        ''' The index of the second node
        ''' </summary>
        ''' <returns></returns>
        Public Property N2 As Integer

        ''' <summary>
        ''' The index of the third node
        ''' </summary>
        ''' <returns></returns>
        Public Property N3 As Integer

        ''' <summary>
        ''' The index of the fourth node (only for quadrilateral panels)
        ''' </summary>
        ''' <returns></returns>
        Public Property N4 As Integer

        ''' <summary>
        ''' The global index in the model
        ''' </summary>
        ''' <returns></returns>
        Public Property GlobalIndex As Integer

        ''' <summary>
        ''' The control point
        ''' </summary>
        ''' <returns></returns>
        Public Property ControlPoint As Vector3

        ''' <summary>
        ''' The normal vector
        ''' </summary>
        ''' <returns></returns>
        Public Property NormalVector As Vector3

        ''' <summary>
        ''' The area of the panel
        ''' </summary>
        ''' <returns></returns>
        Public Property Area As Double

        ''' <summary>
        ''' Indicates if this panel has a reversed singularity behavior
        ''' </summary>
        ''' <returns></returns>
        Public Property IsReversed As Boolean

        ''' <summary>
        ''' The local velocity vector at the control point
        ''' </summary>
        ''' <returns></returns>
        Public Property LocalVelocity As Vector3

        ''' <summary>
        ''' The local circulation
        ''' </summary>
        ''' <returns></returns>
        Public Property Circulation As Double

        ''' <summary>
        ''' The local sink/source strenght
        ''' </summary>
        ''' <returns></returns>
        Public Property SourceStrength As Double

        ''' <summary>
        ''' The panel pressure coefficient
        ''' </summary>
        ''' <returns></returns>
        Public Property Cp As Double

        ''' <summary>
        ''' The color associated to te pressure coefficient
        ''' </summary>
        ''' <returns></returns>
        Public Property CpColor As ColorSharpGL

        ''' <summary>
        ''' Indicates if this panel is a primitive convection area
        ''' </summary>
        ''' <returns></returns>
        Public Property IsPrimitive As Boolean

        ''' <summary>
        ''' Indicates if this panel represents a slender (zero thickness) surface
        ''' </summary>
        ''' <returns></returns>
        Public Property IsSlender As Boolean = True

        ''' <summary>
        ''' Indicates if this panel is active (for selection)
        ''' </summary>
        ''' <returns></returns>
        Public Property Active As Boolean = False

        Private _IsTriangular As Boolean

        ''' <summary>
        ''' Indicates if this panel is represented by only the first three nodes.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsTriangular As Boolean
            Get
                Return _IsTriangular
            End Get
        End Property

        ''' <summary>
        ''' The velocity induced on this panel
        ''' </summary>
        Private _InducedVelocity As New Vector3

        ''' <summary>
        ''' Generates a quadrilateral element
        ''' </summary>
        ''' <param name="N1"></param>
        ''' <param name="N2"></param>
        ''' <param name="N3"></param>
        ''' <param name="N4"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal N4 As Integer)

            Me.N1 = N1
            Me.N2 = N2
            Me.N3 = N3
            Me.N4 = N4
            _IsTriangular = False

        End Sub

        ''' <summary>
        ''' Generates a triangular element
        ''' </summary>
        ''' <param name="N1"></param>
        ''' <param name="N2"></param>
        ''' <param name="N3"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer)

            Me.N1 = N1
            Me.N2 = N2
            Me.N3 = N3
            Me.N4 = N1
            _IsTriangular = True

        End Sub

        ''' <summary>
        ''' The velocity induced at the control point of this panel
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property InducedVelocity As Vector3
            Get
                Return _InducedVelocity
            End Get
        End Property

        ''' <summary>
        ''' Deep copy of the given object into this one
        ''' </summary>
        ''' <param name="Panel"></param>
        Public Sub Assign(ByVal Panel As Panel)

            N1 = Panel.N1
            N2 = Panel.N2
            N3 = Panel.N3
            N4 = Panel.N4

            _IsTriangular = Panel.IsTriangular

            GlobalIndex = Panel.GlobalIndex
            ControlPoint.Assign(Panel.ControlPoint)
            NormalVector.Assign(Panel.NormalVector)
            Area = Panel.Area
            IsReversed = Panel.IsReversed

            LocalVelocity.Assign(Panel.LocalVelocity)
            Circulation = Panel.Circulation
            Cp = Panel.Cp

            IsPrimitive = Panel.IsPrimitive
            IsSlender = Panel.IsSlender

            _InducedVelocity.Assign(Panel.InducedVelocity)

        End Sub

    End Class

    ''' <summary>
    ''' Represents a point in a view model. This object is used for modelling and representation of results.
    ''' </summary>
    Public Class NodalPoint

        ''' <summary>
        ''' Indicqtes if this node is active (selection)
        ''' </summary>
        ''' <returns></returns>
        Public Property Active As Boolean = False

        ''' <summary>
        ''' The original position of this node (used to represent the undeformed state)
        ''' </summary>
        ''' <returns></returns>
        Public Property ReferencePosition As Vector3

        ''' <summary>
        ''' The position of this node
        ''' </summary>
        ''' <returns></returns>
        Public Property Position As Vector3

        ''' <summary>
        ''' The displacement (for modes or aeroelastic results)
        ''' </summary>
        ''' <returns></returns>
        Public Property Displacement As Vector3

        ''' <summary>
        ''' The color of the displacement
        ''' </summary>
        ''' <returns></returns>
        Public Property DisplacementColor As ColorSharpGL

        ''' <summary>
        ''' The interpolated pressure coefficient
        ''' </summary>
        ''' <returns></returns>
        Public Property Cp As Double

        ''' <summary>
        ''' The color of the pressure coefficient
        ''' </summary>
        ''' <returns></returns>
        Public Property CpColor As ColorSharpGL

        ''' <summary>
        ''' The interpolated pressure jump
        ''' </summary>
        ''' <returns></returns>
        Public Property CpDelta As Double

        ''' <summary>
        ''' The color of the interpolated pressure jump
        ''' </summary>
        ''' <returns></returns>
        Public Property CpDeltaColor As ColorSharpGL

        ''' <summary>
        ''' Creates a new node with coordinates (0,0,0)
        ''' </summary>
        Public Sub New()
            Position = New Vector3
        End Sub

        ''' <summary>
        ''' Creates a new node at the given coordinates
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Z"></param>
        Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
            Position = New Vector3(X, Y, Z)
        End Sub

        ''' <summary>
        ''' Creates a new node at the position of point P
        ''' </summary>
        ''' <param name="P"></param>
        Public Sub New(ByRef P As Vector3)
            Position = New Vector3(P)
        End Sub

    End Class

    ''' <summary>
    ''' Represents a segment in the lattice
    ''' </summary>
    Public Class LatticeSegment

        ''' <summary>
        ''' The index of the first node
        ''' </summary>
        Public N1 As Integer

        ''' <summary>
        ''' The index of the second node
        ''' </summary>
        Public N2 As Integer

        ''' <summary>
        ''' Creates a new segment referencing to node 0
        ''' </summary>
        Public Sub New()
            N1 = 0
            N2 = 0
        End Sub

        ''' <summary>
        ''' Creates a new segment linked to nodes N1 and N2 
        ''' </summary>
        ''' <param name="N1"></param>
        ''' <param name="N2"></param>
        Public Sub New(ByVal N1 As Integer, ByVal N2 As Integer)
            Me.N1 = N1
            Me.N2 = N2
        End Sub

    End Class

End Namespace

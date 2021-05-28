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

'#############################################################################
' Unit: Constraints
'
' This unit declares the contrains that can be imposed on a structural node
'#############################################################################
Namespace CalculationModel.Models.Structural.Library.Nodes

    ''' <summary>
    ''' Nodal constrains
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Constrains

        ''' <summary>
        ''' Indicates which degrees of freedom in the node are fixed to the inertial reference frame.
        ''' </summary>
        Public Fixed(5) As Boolean

        ''' <summary>
        ''' Indicates which degrees of freedom in the node are attached to a spring
        ''' on the inertial reference frame.
        ''' </summary>
        Public K(5) As Double

        ''' <summary>
        ''' Indicates if the displacement in the X direction is restrained.
        ''' </summary>
        Public Property FixedDx As Boolean
            Set(ByVal value As Boolean)
                Fixed(0) = value
            End Set
            Get
                Return Fixed(0)
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the displacement in the Y direction is restrained.
        ''' </summary>
        Public Property FixedDy As Boolean
            Set(ByVal value As Boolean)
                Fixed(1) = value
            End Set
            Get
                Return Fixed(1)
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the displacement in the Z direction is restrained.
        ''' </summary>
        Public Property FixedDz As Boolean
            Set(ByVal value As Boolean)
                Fixed(2) = value
            End Set
            Get
                Return Fixed(2)
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the rotation about the X axis is restrained.
        ''' </summary>
        Public Property FixedRx As Boolean
            Set(ByVal value As Boolean)
                Fixed(3) = value
            End Set
            Get
                Return Fixed(3)
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the rotation about the Y axis is restrained.
        ''' </summary>
        Public Property FixedRy As Boolean
            Set(ByVal value As Boolean)
                Fixed(4) = value
            End Set
            Get
                Return Fixed(4)
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the rotation about the Z axis is restrained.
        ''' </summary>
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

End Namespace

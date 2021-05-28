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
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

'#############################################################################
' Unit: StructualNode
'
' This unit declares a 6-DOF nodal point in the structual model.
'#############################################################################
Namespace CalculationModel.Models.Structural.Library.Nodes

    ''' <summary>
    ''' Structural node
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralNode

        ''' <summary>
        ''' The position of the node.
        ''' </summary>
        Public Position As Vector3

        ''' <summary>
        ''' The contrains (or stiffness)
        ''' </summary>
        Public Contrains As Constrains

        ''' <summary>
        ''' The point load.
        ''' </summary>
        Public Load As NodalLoad

        ''' <summary>
        ''' The displacement relative to the original position.
        ''' </summary>
        Public Displacement As NodalDisplacement

        ''' <summary>
        ''' The velocity of the node.
        ''' </summary>
        Public Velocity As NodalDisplacement

        Private _Index As Integer

        ''' <summary>
        ''' Node global index (used to build the finite element matrices).
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
            Position = New Vector3
            Displacement = New NodalDisplacement
            Velocity = New NodalDisplacement
            Contrains = New Constrains
            Load = New NodalLoad
        End Sub

    End Class

End Namespace
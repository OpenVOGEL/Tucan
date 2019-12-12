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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace CalculationModel.Models.Structural.Library.Nodes

    ''' <summary>
    ''' Structural node
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralNode

        Public Position As Vector3

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
            Position = New Vector3
            Displacement = New NodalDisplacement
            Velocity = New NodalDisplacement
            Contrains = New Constrains
            Load = New NodalLoad
        End Sub

    End Class

End Namespace
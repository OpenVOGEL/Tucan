'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Namespace CalculationModel.Models.Aero.Components

    ''' <summary>
    ''' Represents a node on a wake or a bounded lattice.
    ''' This class does not only hold the position, but also the substantial velocity.
    ''' </summary>
    Public Class Node

        ''' <summary>
        ''' Position used as reference for translating or rotating (not always used)
        ''' </summary>
        ''' <remarks></remarks>
        Public OriginalPosition As EVector3

        ''' <summary>
        ''' Current position
        ''' </summary>
        ''' <remarks></remarks>
        Public Position As EVector3

        ''' <summary>
        ''' Represents a displacement (used on aeroelastic calculation)
        ''' </summary>
        ''' <remarks></remarks>
        Public Displacement As EVector3

        ''' <summary>
        ''' Absolute velocity
        ''' </summary>
        ''' <remarks></remarks>
        Public Velocity As EVector3

        ''' <summary>
        ''' Local index.
        ''' </summary>
        Public IndexL As Integer

        ''' <summary>
        ''' Global index.
        ''' </summary>
        Public IndexG As Integer

        Public Sub New()
            Position = New EVector3
            Velocity = New EVector3
        End Sub

        Public Sub New(ByVal IndexL As Integer, ByVal Position As EVector3)
            Me.Position = New EVector3(Position)
            Velocity = New EVector3
            Me.IndexL = IndexL
        End Sub

        Public Sub New(ByVal NodeToCopy As Node)
            Position = New EVector3
            Velocity = New EVector3
            Me.Assign(NodeToCopy)
        End Sub

        Public Sub Assign(ByVal Node As Node)
            Me.Position.Assign(Node.Position)
            Me.Velocity.Assign(Node.Velocity)
            Me.IndexL = Node.IndexL
            Me.IndexG = Node.IndexG
        End Sub

        Public Sub IntegratePosition(ByVal Dt As Double)
            Me.Position.X += Dt * Velocity.X
            Me.Position.Y += Dt * Velocity.Y
            Me.Position.Z += Dt * Velocity.Z
        End Sub

    End Class

End Namespace
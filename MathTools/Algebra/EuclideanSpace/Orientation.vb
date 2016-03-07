'## Open VOGEL ##
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Namespace Algebra.EuclideanSpace

    ''' <summary>
    ''' Represents an orientation in Euler angles
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OrientationCoordinates

        Public Psi As Double
        Public Tita As Double
        Public Fi As Double

        Public Sub SetToCero()
            Me.Psi = 0
            Me.Tita = 0
            Me.Fi = 0
        End Sub

        Public Function ToRadians() As OrientationCoordinates

            Dim NuevaOrientacion As New OrientationCoordinates
            Dim Conversion As Double = Math.PI / 180
            NuevaOrientacion.Psi = Me.Psi * Conversion
            NuevaOrientacion.Tita = Me.Tita * Conversion
            NuevaOrientacion.Fi = Me.Fi * Conversion
            Return NuevaOrientacion

        End Function

        Public Sub Assign(ByVal Ori As OrientationCoordinates)

            Me.Fi = Ori.Fi
            Me.Psi = Ori.Psi
            Me.Tita = Ori.Tita

        End Sub

    End Class

End Namespace

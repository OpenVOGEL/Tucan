'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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
    Public Class EulerAngles

        Public Property Psi As Double
        Public Property Tita As Double
        Public Property Fi As Double
        Public Property Secuence As RotationSecuence = RotationSecuence.ZYX

        Public Enum RotationSecuence As Byte

            ZYX = 0
            XYZ = 1

            ' Other secuences can be added here, but you have to add the transformation matrix.

        End Enum

        Public Sub SetToCero()
            Psi = 0
            Tita = 0
            Fi = 0
        End Sub

        Public Function ToRadians() As EulerAngles

            Dim OrientationInRadians As New EulerAngles
            Dim Conversion As Double = Math.PI / 180
            OrientationInRadians.Psi = Psi * Conversion
            OrientationInRadians.Tita = Tita * Conversion
            OrientationInRadians.Fi = Fi * Conversion
            OrientationInRadians.Secuence = Secuence
            Return OrientationInRadians

        End Function

        Public Sub Assign(ByVal Euler As EulerAngles)

            Fi = Euler.Fi
            Psi = Euler.Psi
            Tita = Euler.Tita
            Secuence = Euler.Secuence

        End Sub

    End Class

End Namespace

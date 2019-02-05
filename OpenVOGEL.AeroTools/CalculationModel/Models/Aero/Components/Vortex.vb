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
    ''' Represents a straight constant-circulation vortex segment
    ''' </summary>
    Public Class Vortex

        ' Comment regarding this class:
        ' Vortices might be useful to replace the complete wake lattice, but not to work together with vortex rings.

        Const FourPi As Double = 4 * Math.PI

        ''' <summary>
        ''' First vortex node
        ''' </summary>
        Public Node1 As Node

        ''' <summary>
        ''' Second vortex node
        ''' </summary>
        Public Node2 As Node

        ''' <summary>
        ''' Adjacent rings.
        ''' </summary>
        ''' <remarks></remarks>
        Public Rings(2) As VortexRing

        ''' <summary>
        ''' Sence of the adjacent rings.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sence(2) As SByte

        ''' <summary>
        ''' Intensity
        ''' </summary>
        Public G As Double

        ' ''' <summary>
        ' ''' Indicates if the vortex is free or bounded.
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Public Free As Boolean

        ''' <summary>
        ''' Indicates if the vortex has been emitted in streamwise direction.
        ''' </summary>
        ''' <remarks></remarks>
        Public Streamwise As Boolean = False

#Region "Field evaluation"

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Function BiotSavart(ByVal Point As Vector3,
                                   Optional ByVal CutOff As Double = 0.0001,
                                   Optional ByVal WithG As Boolean = True) As Vector3

            Dim BSVector As New Vector3

            Dim Den As Double
            Dim Num As Double

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim vx, vy, vz As Double
            Dim dx, dy, dz As Double

            Dim NR1 As Double
            Dim NR2 As Double
            Dim Factor As Double

            Lx = Node2.Position.X - Node1.Position.X
            Ly = Node2.Position.Y - Node1.Position.Y
            Lz = Node2.Position.Z - Node1.Position.Z

            R1x = Point.X - Node1.Position.X
            R1y = Point.Y - Node1.Position.Y
            R1z = Point.Z - Node1.Position.Z

            vx = Ly * R1z - Lz * R1y
            vy = Lz * R1x - Lx * R1z
            vz = Lx * R1y - Ly * R1x

            Den = FourPi * (vx * vx + vy * vy + vz * vz)

            If Den > CutOff Then

                ' Calculate the rest of the geometrical parameters:

                R2x = Point.X - Node2.Position.X
                R2y = Point.Y - Node2.Position.Y
                R2z = Point.Z - Node2.Position.Z

                NR1 = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                NR2 = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                dx = NR1 * R1x - NR2 * R2x
                dy = NR1 * R1y - NR2 * R2y
                dz = NR1 * R1z - NR2 * R2z

                If WithG Then
                    Num = G * (Lx * dx + Ly * dy + Lz * dz)
                Else
                    Num = (Lx * dx + Ly * dy + Lz * dz)
                End If

                Factor = Num / Den

                BSVector.X += Factor * vx
                BSVector.Y += Factor * vy
                BSVector.Z += Factor * vz

            Else

                BSVector.X += 0
                BSVector.Y += 0
                BSVector.Z += 0

            End If

            Return BSVector

        End Function

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Sub AddBiotSavartVector(ByRef Vector As Vector3,
                                       Point As Vector3,
                                       CutOff As Double,
                                       WithG As Boolean)

            Dim Den As Double
            Dim Num As Double

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim vx, vy, vz As Double
            Dim dx, dy, dz As Double

            Dim NR1 As Double
            Dim NR2 As Double
            Dim Factor As Double

            Lx = Node2.Position.X - Node1.Position.X
            Ly = Node2.Position.Y - Node1.Position.Y
            Lz = Node2.Position.Z - Node1.Position.Z

            R1x = Point.X - Node1.Position.X
            R1y = Point.Y - Node1.Position.Y
            R1z = Point.Z - Node1.Position.Z

            vx = Ly * R1z - Lz * R1y
            vy = Lz * R1x - Lx * R1z
            vz = Lx * R1y - Ly * R1x

            Den = FourPi * (vx * vx + vy * vy + vz * vz)

            If Den > CutOff Then

                ' Calculate the rest of the geometrical parameters:

                R2x = Point.X - Node2.Position.X
                R2y = Point.Y - Node2.Position.Y
                R2z = Point.Z - Node2.Position.Z

                NR1 = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                NR2 = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                dx = NR1 * R1x - NR2 * R2x
                dy = NR1 * R1y - NR2 * R2y
                dz = NR1 * R1z - NR2 * R2z

                If WithG Then
                    Num = G * (Lx * dx + Ly * dy + Lz * dz)
                Else
                    Num = (Lx * dx + Ly * dy + Lz * dz)
                End If

                Factor = Num / Den

                Vector.X += Factor * vx
                Vector.Y += Factor * vy
                Vector.Z += Factor * vz

            End If

        End Sub

#End Region

    End Class

End Namespace
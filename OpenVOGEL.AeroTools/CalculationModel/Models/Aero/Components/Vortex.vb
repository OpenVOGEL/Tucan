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
' Unit: Vortex
'
' This unit provides the definition of a vortex.
' Vortices are used as alternative to vortex rings to reduce the calculation
' effort (in a lattice, the number of vortices is larger than the number of
' rings, but all rings together require 2 or 3 times more computations for the
' influence functions).
' Vortices are also used to simulate the kutta condition.
'#############################################################################
Namespace CalculationModel.Models.Aero.Components

    ''' <summary>
    ''' Represents a straight constant-circulation vortex segment
    ''' </summary>
    Public Class Vortex

        ''' <summary>
        ''' Four times Pi
        ''' </summary>
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

        '' Possible optimization: compute the L vector only once every time step

        ''' <summary>
        ''' The vortex segment
        ''' </summary>
        'Public L As New Vector3

        ''' <summary>
        ''' Calculates the L vector as Node2 - Node1.
        ''' This procedure could be used to lower the calculation time
        ''' </summary>
        'Public Sub Refresh()

        '    L.X = Node2.Position.X - Node1.Position.X
        '    L.Y = Node2.Position.Y - Node1.Position.Y
        '    L.Z = Node2.Position.Z - Node1.Position.Z

        'End Sub

#Region "Field evaluation"

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Function InducedVelocity(ByVal Point As Vector3,
                                        Optional ByVal CutOff As Double = 0.0001,
                                        Optional ByVal WithG As Boolean = True) As Vector3

            Dim Vector As New Vector3

            Dim D As Double
            Dim F As Double

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim vx, vy, vz As Double
            Dim dx, dy, dz As Double

            Dim NR1 As Double
            Dim NR2 As Double

            Lx = Node2.Position.X - Node1.Position.X
            Ly = Node2.Position.Y - Node1.Position.Y
            Lz = Node2.Position.Z - Node1.Position.Z

            R1x = Point.X - Node1.Position.X
            R1y = Point.Y - Node1.Position.Y
            R1z = Point.Z - Node1.Position.Z

            vx = Ly * R1z - Lz * R1y
            vy = Lz * R1x - Lx * R1z
            vz = Lx * R1y - Ly * R1x

            D = FourPi * (vx * vx + vy * vy + vz * vz)

            If D > CutOff Then

                ' Calculate the rest of the geometrical parameters:

                R2x = Point.X - Node2.Position.X
                R2y = Point.Y - Node2.Position.Y
                R2z = Point.Z - Node2.Position.Z

                NR1 = 1 / Math.Sqrt(R1x * R1x + R1y * R1y + R1z * R1z)
                NR2 = 1 / Math.Sqrt(R2x * R2x + R2y * R2y + R2z * R2z)

                dx = NR1 * R1x - NR2 * R2x
                dy = NR1 * R1y - NR2 * R2y
                dz = NR1 * R1z - NR2 * R2z

                F = (Lx * dx + Ly * dy + Lz * dz) / D

                If WithG Then
                    F *= G
                End If

                Vector.X += F * vx
                Vector.Y += F * vy
                Vector.Z += F * vz

            Else

                Vector.X += 0
                Vector.Y += 0
                Vector.Z += 0

            End If

            Return Vector

        End Function

        ''' <summary>
        ''' Calculates BiotSavart vector at a given point. If WidthG is true vector is scaled by G.
        ''' </summary>
        ''' <remarks>
        ''' Calculation has been optimized by replacing object subs by local code.
        ''' Value types are used on internal calculations (other versions used reference type EVector3).
        ''' </remarks>
        Public Sub AddInducedVelocity(ByRef Vector As Vector3,
                                       Point As Vector3,
                                       CutOff As Double,
                                       WithG As Boolean)

            Dim D As Double
            Dim F As Double

            Dim Lx, Ly, Lz As Double
            Dim R1x, R1y, R1z, R2x, R2y, R2z As Double
            Dim vx, vy, vz As Double
            Dim dx, dy, dz As Double

            Dim NR1 As Double
            Dim NR2 As Double

            Lx = Node2.Position.X - Node1.Position.X
            Ly = Node2.Position.Y - Node1.Position.Y
            Lz = Node2.Position.Z - Node1.Position.Z

            R1x = Point.X - Node1.Position.X
            R1y = Point.Y - Node1.Position.Y
            R1z = Point.Z - Node1.Position.Z

            vx = Ly * R1z - Lz * R1y
            vy = Lz * R1x - Lx * R1z
            vz = Lx * R1y - Ly * R1x

            D = FourPi * (vx * vx + vy * vy + vz * vz)

            If D > CutOff Then

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
                    F = G * (Lx * dx + Ly * dy + Lz * dz) / D
                Else
                    F = (Lx * dx + Ly * dy + Lz * dz) / D
                End If

                Vector.X += F * vx
                Vector.Y += F * vy
                Vector.Z += F * vz

            End If

        End Sub

#End Region

    End Class

End Namespace
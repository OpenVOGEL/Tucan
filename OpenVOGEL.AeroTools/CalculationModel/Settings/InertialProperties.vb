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
Imports DotNumerics.LinearAlgebra

Namespace CalculationModel.Settings

    ''' <summary>
    ''' Represent the inertial properties of a body
    ''' </summary>
    Public Structure InertialProperties

        ''' <summary>
        ''' Center of uniform force field.
        ''' </summary>
        ''' <returns></returns>
        Public Property Xcg As Double

        ''' <summary>
        ''' Center of uniform force field.
        ''' </summary>
        ''' <returns></returns>
        Public Property Ycg As Double

        ''' <summary>
        ''' Center of uniform force field.
        ''' </summary>
        ''' <returns></returns>
        Public Property Zcg As Double

        ''' <summary>
        ''' The mass of the system.
        ''' </summary>
        ''' <returns></returns>
        Public Property Mass As Double

        ''' <summary>
        ''' Moment of ineratia about the X axis.
        ''' </summary>
        ''' <returns></returns>
        Public Property Ixx As Double

        ''' <summary>
        ''' Moment of ineratia about the Y axis.
        ''' </summary>
        ''' <returns></returns>
        Public Property Iyy As Double

        ''' <summary>
        ''' Moment of ineratia about the Z axis.
        ''' </summary>
        ''' <returns></returns>
        Public Property Izz As Double

        ''' <summary>
        ''' Cross moment of ineratia about the XY axes.
        ''' </summary>
        ''' <returns></returns>
        Public Property Ixy As Double

        ''' <summary>
        ''' Cross moment of ineratia about the XZ axes.
        ''' </summary>
        ''' <returns></returns>
        Public Property Ixz As Double

        ''' <summary>
        ''' Cross moment of ineratia about the YZ axes.
        ''' </summary>
        ''' <returns></returns>
        Public Property Iyz As Double

        ''' <summary>
        ''' Adds the two inertial properties using the transport theorem.
        ''' </summary>
        ''' <param name="I1"></param>
        ''' <param name="I2"></param>
        ''' <returns></returns>
        Public Shared Operator +(I1 As InertialProperties, I2 As InertialProperties)

            Dim I As InertialProperties

            I.Mass = I1.Mass + I2.Mass

            I.Xcg = (I1.Xcg * I1.Mass + I2.Xcg * I2.Mass) / I.Mass
            I.Ycg = (I1.Ycg * I1.Mass + I2.Ycg * I2.Mass) / I.Mass
            I.Zcg = (I1.Zcg * I1.Mass + I2.Zcg * I2.Mass) / I.Mass

            Dim I1_Xcg As Double = I1.Xcg - I.Xcg
            Dim I1_Ycg As Double = I1.Ycg - I.Ycg
            Dim I1_Zcg As Double = I1.Zcg - I.Zcg

            Dim I2_Xcg As Double = I2.Xcg - I.Xcg
            Dim I2_Ycg As Double = I2.Ycg - I.Ycg
            Dim I2_Zcg As Double = I2.Zcg - I.Zcg

            Dim I1_Ixx As Double = (I1_Ycg ^ 2 + I1_Zcg ^ 2) * I1.Mass
            Dim I2_Ixx As Double = (I2_Ycg ^ 2 + I2_Zcg ^ 2) * I2.Mass
            I.Ixx = I1.Ixx + I2.Ixx + I1_Ixx + I2_Ixx

            Dim I1_Iyy As Double = (I1_Xcg ^ 2 + I1.Zcg ^ 2) * I1.Mass
            Dim I2_Iyy As Double = (I2_Xcg ^ 2 + I2_Zcg ^ 2) * I2.Mass
            I.Iyy = I1.Iyy + I2.Iyy + I1_Iyy + I2_Iyy

            Dim I1_Izz As Double = (I1_Xcg ^ 2 + I1_Ycg ^ 2) * I1.Mass
            Dim I2_Izz As Double = (I2_Xcg ^ 2 + I2_Ycg ^ 2) * I2.Mass
            I.Izz = I1.Izz + I2.Izz + I1_Izz + I2_Izz

            Dim I1_Ixy As Double = I1_Xcg * I1_Ycg * I1.Mass
            Dim I2_Ixy As Double = I2_Xcg * I2_Ycg * I2.Mass
            I.Ixy = I1.Ixy + I2.Ixy + I1_Ixy + I2_Ixy

            Dim I1_Ixz As Double = I1_Xcg * I1_Zcg * I1.Mass
            Dim I2_Ixz As Double = I2_Xcg * I2_Zcg * I2.Mass
            I.Ixz = I1.Ixz + I2.Ixz + I1_Ixz + I2_Ixz

            Dim I1_Iyz As Double = I1_Ycg * I1_Zcg * I1.Mass
            Dim I2_Iyz As Double = I2_Ycg * I2_Zcg * I2.Mass
            I.Iyz = I1.Iyz + I2.Iyz + I1_Iyz + I2_Iyz

            Return I

        End Operator

        ''' <summary>
        ''' Convertes the general tensor of inertia to a the main axes of inertia
        ''' </summary>
        ''' <param name="Basis"></param>
        ''' <param name="I_xx"></param>
        ''' <param name="I_yy"></param>
        ''' <param name="I_zz"></param>
        Public Sub ToMainInertia(ByRef Basis As Base3,
                                 ByRef I_xx As Double,
                                 ByRef I_yy As Double,
                                 ByRef I_zz As Double)

            Dim E As New EigenSystem()
            Dim M As New SymmetricMatrix(3)

            M(0, 0) = Ixx
            M(0, 1) = Ixy
            M(0, 2) = Ixz
            M(1, 1) = Iyy
            M(1, 2) = Iyz
            M(2, 2) = Izz

            ' Solve the eigen system
            '--------------------------------------

            Dim V As New Matrix(3)
            Dim A As Matrix = E.GetEigenvalues(M, V)

            I_xx = A(0, 0)
            I_yy = A(1, 0)
            I_zz = A(2, 0)

            Basis.U.X = V(0, 0)
            Basis.U.Y = V(1, 0)
            Basis.U.Z = V(2, 0)

            Basis.V.X = V(0, 1)
            Basis.V.Y = V(1, 1)
            Basis.V.Z = V(2, 1)

            ' Force the basis to be dextro-rotation
            '--------------------------------------

            Basis.W.FromVectorProduct(Basis.U, Basis.V)

        End Sub

    End Structure

End Namespace


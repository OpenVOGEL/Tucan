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

Namespace Integration

    ''' <summary>
    ''' A 6-DOF dynamics variable
    ''' </summary>
    Public Structure Variable

        ' Gravity 
        '------------------
        Public Gx As Double
        Public Gy As Double
        Public Gz As Double

        ' Position 
        '------------------
        Public Px As Double
        Public Py As Double
        Public Pz As Double

        ' Velocity 
        '------------------
        Public Vx As Double
        Public Vy As Double
        Public Vz As Double

        ' Angular velocity 
        '------------------
        Public Ox As Double
        Public Oy As Double
        Public Oz As Double

        Public Shared Operator +(X1 As Variable, X2 As Variable) As Variable

            Dim S As Variable

            S.Gx = X1.Gx + X2.Gx
            S.Gy = X1.Gy + X2.Gy
            S.Gz = X1.Gz + X2.Gz

            S.Px = X1.Px + X2.Px
            S.Py = X1.Py + X2.Py
            S.Pz = X1.Pz + X2.Pz

            S.Vx = X1.Vx + X2.Vx
            S.Vy = X1.Vy + X2.Vy
            S.Vz = X1.Vz + X2.Vz

            S.Ox = X1.Ox + X2.Ox
            S.Oy = X1.Oy + X2.Oy
            S.Oz = X1.Oz + X2.Oz

            Return S

        End Operator

        Public Shared Operator -(X1 As Variable, X2 As Variable) As Variable

            Dim S As Variable

            S.Gx = X1.Gx - X2.Gx
            S.Gy = X1.Gy - X2.Gy
            S.Gz = X1.Gz - X2.Gz

            S.Px = X1.Px - X2.Px
            S.Py = X1.Py - X2.Py
            S.Pz = X1.Pz - X2.Pz

            S.Vx = X1.Vx - X2.Vx
            S.Vy = X1.Vy - X2.Vy
            S.Vz = X1.Vz - X2.Vz

            S.Ox = X1.Ox - X2.Ox
            S.Oy = X1.Oy - X2.Oy
            S.Oz = X1.Oz - X2.Oz

            Return S

        End Operator

        Public Shared Operator *(K As Double, X As Variable) As Variable

            Dim P As Variable

            P.Gx = K * X.Gx
            P.Gy = K * X.Gy
            P.Gz = K * X.Gz

            P.Px = K * X.Px
            P.Py = K * X.Py
            P.Pz = K * X.Pz

            P.Vx = K * X.Vx
            P.Vy = K * X.Vy
            P.Vz = K * X.Vz

            P.Ox = K * X.Ox
            P.Oy = K * X.Oy
            P.Oz = K * X.Oz

            Return P

        End Operator

        Public Shared Operator /(X1 As Variable, X2 As Variable) As Variable

            Dim D As Variable

            D.Gx = X1.Gx / X2.Gx
            D.Gy = X1.Gy / X2.Gy
            D.Gz = X1.Gz / X2.Gz

            D.Px = X1.Px / X2.Px
            D.Py = X1.Py / X2.Py
            D.Pz = X1.Pz / X2.Pz

            D.Vx = X1.Vx / X2.Vx
            D.Vy = X1.Vy / X2.Vy
            D.Vz = X1.Vz / X2.Vz

            D.Ox = X1.Ox / X2.Ox
            D.Oy = X1.Oy / X2.Oy
            D.Oz = X1.Oz / X2.Oz

            Return D

        End Operator

        Public Shared Operator <=(X1 As Variable, X2 As Variable) As Boolean

            Return _
            X1.Gx <= X2.Gx AndAlso
            X1.Gy <= X2.Gy AndAlso
            X1.Gz <= X2.Gz AndAlso
            X1.Px <= X2.Px AndAlso
            X1.Py <= X2.Py AndAlso
            X1.Pz <= X2.Pz AndAlso
            X1.Vx <= X2.Vx AndAlso
            X1.Vy <= X2.Vy AndAlso
            X1.Vz <= X2.Vz AndAlso
            X1.Ox <= X2.Ox AndAlso
            X1.Oy <= X2.Oy AndAlso
            X1.Oz <= X2.Oz

        End Operator

        Public Shared Operator >=(X1 As Variable, X2 As Variable) As Boolean

            Return _
            X1.Gx >= X2.Gx AndAlso
            X1.Gy >= X2.Gy AndAlso
            X1.Gz >= X2.Gz AndAlso
            X1.Px >= X2.Px AndAlso
            X1.Py >= X2.Py AndAlso
            X1.Pz >= X2.Pz AndAlso
            X1.Vx >= X2.Vx AndAlso
            X1.Vy >= X2.Vy AndAlso
            X1.Vz >= X2.Vz AndAlso
            X1.Ox >= X2.Ox AndAlso
            X1.Oy >= X2.Oy AndAlso
            X1.Oz >= X2.Oz

        End Operator

        ''' <summary>
        ''' Convertes all variables to absolute values
        ''' </summary>
        ''' <returns></returns>
        Public Sub Absolute()

            Gx = Math.Abs(Gx)
            Gy = Math.Abs(Gy)
            Gz = Math.Abs(Gz)

            Px = Math.Abs(Px)
            Py = Math.Abs(Py)
            Pz = Math.Abs(Pz)

            Vx = Math.Abs(Vx)
            Vy = Math.Abs(Vy)
            Vz = Math.Abs(Vz)

            Ox = Math.Abs(Ox)
            Oy = Math.Abs(Oy)
            Oz = Math.Abs(Oz)

        End Sub

    End Structure

    ''' <summary>
    ''' This class represents a predictor corrector method specifically tailored for
    ''' solving rigid body motion in 6 degrees of freedom under a uniform graviational field.
    ''' The algorithm starts with the Euler method and then uses two Adams/Bashford 
    ''' and Adams/Moulton steps of increasing accuracy.
    ''' The Hamming method is used from step 4 on.
    ''' Remarks:
    ''' > All steps compensate the truncation errors.
    ''' > The gravity field is implicitly taken into account and initialized as (0.0, 0.0, -g).
    ''' How to use:
    ''' 1) Instantiate the class and initialize the inertial properties.
    '''    The X, Y and Z axis are the main intertial axes and the
    '''    origin is at the CG.
    ''' 2) Provide the initial external forces.
    ''' 3) Enter the time iteration loop:
    '''    > "Predict" with the current forces
    '''    > Enter the refinement loop:
    '''       > Adapt system and calculate the external forces using the 
    '''         predicted or already corrected state variables.
    '''       > "Correct" with the new forces.
    ''' </summary>
    Public Class HammingIntegrator

        ''' <summary>
        ''' Creates a new integrator for N time steps and the given initial conditions.
        ''' </summary>
        ''' <param name="N">Number of time steps</param>
        ''' <param name="V0"></param>
        ''' <param name="O0"></param>
        Public Sub New(N As Integer, T As Double, V0 As Vector3, O0 As Vector3, Gravity As Vector3)

            _Velocity = New Vector3
            _Rotation = New Vector3

            ReDim X(N)
            ReDim TE(N)
            ReDim DX(N)

            Dt = T

            X(0).Vx = V0.X
            X(0).Vy = V0.Y
            X(0).Vz = V0.Z

            X(0).Ox = O0.X
            X(0).Oy = O0.Y
            X(0).Oz = O0.Z

            X(0).Gx = Gravity.X
            X(0).Gy = Gravity.Y
            X(0).Gz = Gravity.Z

            S = 0
            I = 0

            Dim E As Variable

            ' Absolute errors
            '------------------

            E.Gx = 0.01
            E.Gy = 0.01
            E.Gz = 0.01

            E.Px = 0.01
            E.Py = 0.01
            E.Pz = 0.01

            E.Vx = 0.01
            E.Vy = 0.01
            E.Vz = 0.01

            E.Ox = 0.001
            E.Oy = 0.001
            E.Oz = 0.001

            Epsilon = E

        End Sub

        ''' <summary>
        ''' The current velocity
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Velocity As Vector3

        ''' <summary>
        ''' The current angular velocity
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Rotation As Vector3

        ''' <summary>
        ''' Stores the current state in the output variables
        ''' </summary>
        Private Sub CacheState()

            _Velocity.X = X(S).Vx
            _Velocity.Y = X(S).Vy
            _Velocity.Z = X(S).Vz

            _Rotation.X = X(S).Ox
            _Rotation.Y = X(S).Oy
            _Rotation.Z = X(S).Oz

        End Sub

        ''' <summary>
        ''' Stores the current state in the output variables
        ''' </summary>
        Private Sub CacheDerivatives(J As Integer, F As Vector3, ByRef M As Vector3)

            ' Gravity
            '-------------------

            DX(J).Gx = X(J).Oy * X(J).Gz - X(J).Oz * X(J).Gy
            DX(J).Gy = X(J).Oz * X(J).Gx - X(J).Ox * X(J).Gz
            DX(J).Gz = X(J).Ox * X(J).Gy - X(J).Oy * X(J).Gx

            ' Position
            '-------------------

            DX(J).Px = X(J).Vx + X(J).Oy * X(J).Vz - X(J).Oz * X(J).Vy
            DX(J).Py = X(J).Vy + X(J).Oz * X(J).Vx - X(J).Ox * X(J).Vz
            DX(J).Pz = X(J).Vz + X(J).Ox * X(J).Vy - X(J).Oy * X(J).Vx

            ' Velocity
            '-------------------

            DX(J).Vx = F.X / Mass + X(J).Gx - X(J).Oy * X(J).Vz + X(J).Oz * X(J).Vy
            DX(J).Vy = F.Y / Mass + X(J).Gy - X(J).Oz * X(J).Vx + X(J).Ox * X(J).Vz
            DX(J).Vz = F.Z / Mass + X(J).Gz - X(J).Ox * X(J).Vy + X(J).Oy * X(J).Vx

            ' Angular velocity
            '-------------------

            DX(J).Ox = (M.X + (Iyy - Izz) * X(J).Oy * X(J).Oz) / Ixx
            DX(J).Oy = (M.Y + (Izz - Ixx) * X(J).Oz * X(J).Ox) / Iyy
            DX(J).Oz = (M.Z + (Ixx - Iyy) * X(J).Ox * X(J).Oy) / Iyy

        End Sub

        Private S As Integer

        Private I As Integer

        Private Dt As Double

        ''' <summary>
        ''' The truncation error
        ''' </summary>
        Private TE() As Variable

        ''' <summary>
        ''' The state variables
        ''' </summary>
        Private X() As Variable

        ''' <summary>
        ''' The Hamming predicted variable
        ''' </summary>
        Private XP As Variable

        ''' <summary>
        ''' The derivative of the state variables
        ''' </summary>
        Private DX() As Variable

        ''' <summary>
        ''' The maximum absolute step change allowed
        ''' </summary>
        ''' <returns></returns>
        Public Property Epsilon As Variable

        ''' <summary>
        ''' The total mass of the system
        ''' </summary>
        ''' <returns></returns>
        Public Property Mass As Double

        ''' <summary>
        ''' The second moment of inertia about the X axis
        ''' </summary>
        ''' <returns></returns>
        Public Property Ixx As Double

        ''' <summary>
        ''' The second moment of inertia about the Y axis
        ''' </summary>
        ''' <returns></returns>
        Public Property Iyy As Double

        ''' <summary>
        ''' The second moment of inertia about the Z axis
        ''' </summary>
        ''' <returns></returns>
        Public Property Izz As Double

        ''' <summary>
        ''' Sets the startup forces
        ''' </summary>
        ''' <param name="F"></param>
        ''' <param name="M"></param>
        Public Sub SetInitialForces(F As Vector3, ByRef M As Vector3)

            CacheDerivatives(0, F, M)

        End Sub

        ''' <summary>
        ''' Advances one time step and produces a tentative predicted solution.
        ''' </summary>
        ''' <param name="F"></param>
        ''' <param name="M"></param>
        Public Sub Predict()

            S += 1
            I = 0

            ' Use the appropriate integrator for each time step
            '--------------------------------------------------

            Select Case S

                Case 1

                    ' Euler method
                    '------------------------------------------

                    X(1) = X(0) + Dt * DX(0)

                Case 2

                    ' 2-steps Adams-Bashford
                    '------------------------------------------

                    X(2) = X(1) + (0.5# * Dt) * (3.0# * DX(1) - DX(0))

                Case 3

                    ' 3-steps Adams-Bashford
                    '------------------------------------------

                    X(3) = X(2) + (Dt / 12.0#) * (23.0# * DX(2) - 16.0# * DX(1) + 5.0# * DX(0))

                Case Else

                    ' Hamming's predictor/corrector algorithm
                    '------------------------------------------

                    XP = X(S - 4) + (Dt * 4.0# / 3.0#) * (2.0 * DX(S - 1) - DX(S - 2) + 2.0 * DX(S - 3))

                    X(S) = XP + 112.0# / 9.0# * TE(S - 1)

            End Select

            CacheState()

        End Sub

        ''' <summary>
        ''' Refines the current step by advancing one implicit iteration.
        ''' If the function returns False, use the state to recalculate the 
        ''' external forces and call the function again. Repeat the loop
        ''' until the function returns True, or until a maximum amount of
        ''' corrective steps is reached.
        ''' </summary>
        ''' <param name="F">The force predicted with at the last state</param>
        ''' <param name="M">The moment predicted with at the last state</param>
        ''' <returns></returns>
        Public Function Correct(F As Vector3, ByRef M As Vector3) As Boolean

            I += 1

            ' Use the appropriate iterator for each time step
            '--------------------------------------------------

            Dim XS As Variable = X(S)

            Select Case S

                Case 1

                    ' Modified Euler method
                    '------------------------------------------

                    CacheDerivatives(1, F, M)

                    X(1) = X(0) + (0.5 * Dt) * (DX(0) + DX(1))

                    TE(1) = (9.0# / 121.0#) * (X(1) - XS)

                Case 2

                    ' 2-steps Adams-Moulton
                    '------------------------------------------

                    CacheDerivatives(2, F, M)

                    X(2) = X(1) + (Dt / 12.0#) * (5.0# * DX(2) + 8.0# * DX(1) - DX(0))

                    TE(2) = (9.0# / 121.0#) * (X(2) - XS)

                Case 3

                    ' 3-steps Adams-Moulton
                    '------------------------------------------

                    CacheDerivatives(3, F, M)

                    X(3) = X(2) + Dt / 24.0# * (9.0# * DX(3) + 19.0# * DX(2) - 5.0# * DX(1) + DX(0))

                    TE(3) = (9.0# / 121.0#) * (X(3) - XS)

                Case Else

                    ' Hamming's predictor/corrector algorithm
                    '------------------------------------------

                    CacheDerivatives(S, F, M)

                    X(S) = (1.0# / 8.0#) * (9.0# * X(S - 1) - X(S - 3) + 3.0# * Dt * (DX(S) + 2.0# * DX(S - 1) - DX(S - 2)))

                    TE(S) = (9.0# / 121.0#) * ((X(S) - XP))

                    X(S) = X(S) - TE(S)

            End Select

            CacheState()

            ' Evaluate change
            '--------------------------------------------------

            Dim E As Variable = X(S) - XS

            E.Absolute()

            Return E <= Epsilon

        End Function

        ''' <summary>
        ''' Sets the startup forces
        ''' </summary>
        ''' <param name="F"></param>
        ''' <param name="M"></param>
        Public Function State(I As Integer) As Variable

            If I >= LBound(X) And I <= UBound(X) Then
                Return X(I)
            Else
                Dim NoVariable As Variable
                Return NoVariable
            End If

        End Function

    End Class

End Namespace

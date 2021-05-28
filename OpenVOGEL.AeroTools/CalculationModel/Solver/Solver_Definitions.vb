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
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.MathTools.Integration
Imports DotNumerics.LinearAlgebra

'#############################################################################
' Unit: Solver_Definitions
'
' This units contains most of the general components needed by the kernel
' to solve the aerodynamic, strictural and dynamic problems.
'#############################################################################
Namespace CalculationModel.Solver

    Partial Public Class Solver

#If DEBUG Then
        Private Const DebugSolver = True
#Else
        Private Const DebugSolver = False
#End If

        ''' <summary>
        ''' Initializes the class.
        ''' </summary>
        Public Sub New()
            Lattices = New List(Of BoundedLattice)
            Settings = New SimulationSettings
        End Sub

        ''' <summary>
        ''' Contains all parameters required to run a complete simulation.
        ''' </summary>
        Public Property Settings As SimulationSettings

        ''' <summary>
        ''' Contains all bounded lattices. This includes thick and slender surfaces.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Lattices As List(Of BoundedLattice)

        ''' <summary>
        ''' Stores the links between the structure and the aerodynamic lattices when there is an
        ''' aeroelastic interaction.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property StructuralLinks As List(Of StructuralLink)

        ''' <summary>
        ''' Stores the dynamic response of the model when there is rigid body motion.
        ''' </summary>
        ''' <returns></returns>
        Public Property Motion As MotionIntegrator

        ''' <summary>
        ''' Gathers all polar curves necessary for the calculation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property PolarDataBase As PolarDatabase

#Region "Private calculation variables"

        ''' <summary>
        ''' The matrix for the Dirichlet boundary conditions (slender surfaces).
        ''' </summary>
        Private MatrixDoublets As Matrix

        ''' <summary>
        ''' The matrix for the Neumman boundary conditions (thick surfaces).
        ''' </summary>
        Private MatrixSources As Matrix

        ''' <summary>
        ''' The vector containing the circulation of vortex rings.
        ''' </summary>
        Private G As Vector

        ''' <summary>
        ''' The vector containing the intensity of the sources in the thick surfaces.
        ''' </summary>
        Private S As Vector

        ''' <summary>
        '''  The right hand side of the linear system, containing the known cross flow.
        ''' </summary>
        Private RHS As Vector

        ''' <summary>
        ''' The dimension of the system
        ''' </summary>
        Private Dimension As Integer

        ''' <summary>
        ''' Indicates if source panels are present in the model. 
        ''' This is automatically detected and determines how the system is built.
        ''' </summary>
        Private WithSources As Boolean = False

        ''' <summary>
        ''' Indicates if the stream rotates.
        ''' This is automatically detected .
        ''' </summary>
        ''' <returns></returns>
        Private WithStreamRotation As Boolean = False

        ''' <summary>
        ''' Gathers the instantaneus stream properties
        ''' </summary>
        Class StreamProperties

            ''' <summary>
            ''' The uniform component of the stream velocity.
            ''' </summary>
            Public Velocity As New Vector3

            ''' <summary>
            ''' The stream rotation about the origin. This could be either imposed
            ''' or a result of the body motion.
            ''' </summary>
            Public Rotation As New Vector3

            ''' <summary>
            ''' The dynamic pressure used to compute dimensionless coefficients.
            ''' </summary>
            Public DynamicPressure As Double

            ''' <summary>
            ''' Cached value of V.V, used for internal computation of coefficients.
            ''' </summary>
            Public SquareVelocity As Double

            ''' <summary>
            ''' The stream density.
            ''' </summary>
            Public Density As Double

        End Class

#End Region

#Region "Public state variables"

        ''' <summary>
        ''' The instantaneus stream properties
        ''' </summary>
        Private Stream As New StreamProperties

        ''' <summary>
        ''' Base stream velocity [m/s]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property StreamVelocity As Vector3
            Get
                Return Stream.Velocity
            End Get
        End Property

        ''' <summary>
        ''' Stream density [kg/m³]
        ''' </summary>
        Public ReadOnly Property StreamDensity As Double
            Get
                Return Stream.Density
            End Get
        End Property

        ''' <summary>
        ''' Stream density [Pa]
        ''' </summary>
        Public ReadOnly Property StreamDynamicPressure As Double
            Get
                Return Stream.DynamicPressure
            End Get
        End Property

#End Region

#Region "Public events"

        ''' <summary>
        ''' Occurs when a progress is made.
        ''' </summary>
        ''' <param name="Title"></param>
        ''' <param name="Value"></param>
        Public Event PushProgress(ByVal Title As String, ByVal Value As Integer)

        ''' <summary>
        ''' Occurs when a progress is made.
        ''' </summary>
        ''' <param name="Title"></param>
        Public Event PushMessage(ByVal Title As String)

        ''' <summary>
        ''' Forwards a result line to the connected handler. The event is triggered when calling "ReportResults".
        ''' </summary>
        Public Event PushResultLine(ByVal Line As String)

        ''' <summary>
        ''' Occurs when the calculation finishes.
        ''' </summary>
        Public Event CalculationDone()

        ''' <summary>
        ''' Occurs when the calculation is automatically aborted.
        ''' </summary>
        Public Event CalculationAborted()

#End Region

    End Class

End Namespace
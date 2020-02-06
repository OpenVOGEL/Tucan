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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports DotNumerics.LinearAlgebra
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library

Namespace CalculationModel.Solver

    Partial Public Class Solver

        ''' <summary>
        ''' Calculates the unsteady aeroelastic transit provided a velocity profile and a structural model.
        ''' The implicit Nemark algorithm is used.
        ''' </summary>
        Public Sub AeroelasticUnsteadyTransit(ByVal DataBasePath As String)

            If WithSources Then

                ' Abort calculation: doublets are not allowed in the aeroelastic analysis

                RaiseEvent PushMessage("Cannot run aeroelastic analysis with thick bodies")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            If Settings.UseGpu Then

                ' Abort calculation: the GPU is not allowed in the aeroelastic analysis

                RaiseEvent PushMessage("Cannot run transit analysis with OpenCL")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            '/////////////////'
            ' Create database '
            '/////////////////'

            RaiseEvent PushMessage("Creating database structure")

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Aeroelastic)
            CleanDirectory(DataBaseSection.Aeroelastic)
            CreateSubFolder(DataBaseSection.Structural)
            CleanDirectory(DataBaseSection.Structural)

            RaiseEvent PushMessage("Generating stream velocity histogram")

            Settings.GenerateVelocityHistogram()

            If IsNothing(Settings.AeroelasticHistogram) Then
                RaiseEvent PushMessage("Unable to generate aeroelastic histogram!")
                Return
            End If

            '//////////////////////////////'
            ' Initialize aerodynamic model '
            '//////////////////////////////'

            RaiseEvent PushMessage("Building matrix")

            ' Build starting matrix and RHS:

            Dim WithStreamOmega As Boolean = Stream.Omega.EuclideanNorm > 0.00001

            BuildMatrixForDoublets(True)
            BuildRightHandSide1()
            InitializeWakes()

            '/////////////////////////////'
            ' Initialize structural model '
            '/////////////////////////////'

            Dim L As Integer = 0

            Dim StructuralDt As Double = Double.MaxValue

            For Each StructuralLink As StructuralLink In StructuralLinks

                ' NOTE: this could be done in parallel

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                L += 1

                RaiseEvent PushMessage(String.Format("Creating structural link {0}", L))

                RaiseEvent PushMessage("Creating mass and stiffness matrices...")
                StructuralLink.StructuralCore.CreateMatrices(Structure_Path, True)

                RaiseEvent PushMessage("Finding modes...")
                StructuralLink.StructuralCore.FindModes(Structure_Path, L)

                ' Update the minimum modal period:

                For Each Mode In StructuralLink.StructuralCore.Modes

                    Mode.C = Settings.AeroelasticHistogram.State(0).Damping * Mode.Cc
                    StructuralDt = Math.Min(StructuralDt, 2 * Math.PI / Mode.W)

                Next

            Next

            ' Calculate the structural step interval:

            Dim StructuralSteps As Integer = Math.Max(1, Math.Round(Settings.Interval / StructuralDt) * 10)

            StructuralDt = Settings.Interval / StructuralSteps

            RaiseEvent PushMessage(String.Format("Structural step: {0:F4}s (x{1})", StructuralDt, StructuralSteps))

            Settings.StructuralSettings.SubSteps = StructuralSteps

            ' Initialize the links with the selected step interval:

            For Each StructuralLink As StructuralLink In StructuralLinks

                RaiseEvent PushMessage("Initializing links...")
                StructuralLink.Initialize(StructuralDt)

            Next

            RaiseEvent PushMessage("All links have been succesfully created")

            '///////////////////'
            ' Start integration '
            '///////////////////'

            Dim Equations As New LinearEquations

            For TimeStep = 1 To Settings.SimulationSteps

                RaiseEvent PushProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                '//////////////////////////'
                ' Update stream parameters '
                '//////////////////////////'

                Stream.Velocity.Assign(Settings.AeroelasticHistogram.State(TimeStep).Velocity)
                Stream.SquareVelocity = Stream.Velocity.SquareEuclideanNorm
                Stream.Density = Settings.Density
                Stream.DynamicPressure = 0.5 * Stream.Density * Stream.SquareVelocity

                '///////////////////////////////////////////'
                ' Update modal damping and integrators data '
                '///////////////////////////////////////////'

                For Each StructuralLink As StructuralLink In StructuralLinks

                    Dim DampingChanged As Boolean = False

                    For Each Mode In StructuralLink.StructuralCore.Modes

                        Mode.C = Settings.AeroelasticHistogram.State(TimeStep).Damping * Mode.Cc

                        DampingChanged = DampingChanged Or (Settings.AeroelasticHistogram.State(TimeStep - 1).Damping <> Settings.AeroelasticHistogram.State(TimeStep).Damping)

                    Next

                    If DampingChanged Then
                        StructuralLink.UpdateIntegrators()
                    End If

                Next

                '//////////////////////////'
                ' Perform the Newmark loop '
                '//////////////////////////'

                RaiseEvent PushMessage("Performing Newmark loop")

                Dim Converged As Boolean = False

                Dim K As Integer = 0

                Dim Level As Double = 0#

                While Not Converged And K <= 10

                    Level = 0#

                    ' Rebuild RHS:

                    CalculateVelocityInducedByTheWakesOnBoundedLattices()

                    BuildRightHandSide2()

                    ' Rebuild matrix (from 2nd time step on)

                    BuildMatrixForDoublets(False)

                    ' Calculate new circulation and its time derivative

                    G = Equations.Solve(MatrixDoublets, RHS)

                    AssignDoublets()

                    ' Start the link only after a certain number of steps

                    If TimeStep > Settings.StructuralSettings.StructuralLinkingStep Then

                        RaiseEvent PushMessage("Calculating airloads")

                        ' Calculate pressure on latices

                        CalculateTotalVelocityOnBoundedLattices()

                        For Each Lattice In Lattices

                            Lattice.CalculatePressure(Stream.SquareVelocity)

                        Next

                        ' Update structural displacement with the new loads

                        Converged = True

                        For P = 0 To StructuralSteps

                            For Each StructuralLink As StructuralLink In StructuralLinks

                                ' NOTE: this could be done in parallel

                                ' Compute one step in the fixed point iteration

                                Converged = Converged And StructuralLink.ImplicitIntegration(Stream.Velocity, Stream.Density, Level, K, 0.005)

                            Next

                        Next

                        ' Force at least two iteration steps

                        If K = 0 Then Converged = False

                        ' Recomposes the wake on the primitive nodes

                        For Each Lattice In Lattices

                            ' NOTE: This could be done in parallel

                            Lattice.ReEstablishWakes()

                        Next

                        K += 1

                    Else

                        Converged = True

                    End If

                End While

                '/////////////'
                ' End of loop '
                '/////////////'

                If Converged Then

                    ' The system is in dynamic equilibrium

                    RaiseEvent PushMessage(String.Format("Convergence reached {0:P3}", Level))

                    '//////////////'
                    ' Convect wake '
                    '//////////////'

                    CalculateVelocityOnWakes()

                    For Each Lattice In Lattices

                        Lattice.PopulateWakeVortices(Settings.Interval, TimeStep, False, Nothing)

                    Next

                Else

                    '////////////////////////'
                    ' The calculation failed '
                    '////////////////////////'

                    RaiseEvent PushMessage("Error: the aeroelastic coupling didn't converge.")
                    Exit For

                End If

            Next

            '/////////////////////'
            ' Save last time step '
            '/////////////////////'

            RaiseEvent PushMessage("Writing binaries")

            WriteToXML(String.Format("{0}\Aeroelastic.res", Aeroelastic_Path, Settings.SimulationSteps))

            RaiseEvent CalculationDone()

        End Sub

    End Class

End Namespace

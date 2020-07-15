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

Imports DotNumerics.LinearAlgebra
Imports OpenVOGEL.MathTools.Integration

Namespace CalculationModel.Solver

    Partial Public Class Solver

        ''' <summary>
        ''' This method simulates an experiment where the model is only rigidly 
        ''' attached to the inertial reference frame for a certain number of steps
        ''' and then released without any constrain.
        ''' If the number of steps is sufficiently high, the method will simulate
        ''' free flight.
        ''' Be careful when using this method to select an appropriate initial state,
        ''' or the model might react violently when being released.
        ''' It is reccomended to use the Console to run a static equilibrium analysis
        ''' before any free flight simulation.
        ''' </summary>
        Public Sub FreeFlight(ByVal DataBasePath As String)

            '#############################################
            ' Prechecks
            '#############################################

            CheckForSources()

            If WithSources Then

                RaiseEvent PushMessage("Cannot run free flight analysis with thick bodies")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            If Settings.UseGpu Then

                RaiseEvent PushMessage("Cannot run free flight analysis with OpenCL")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.FreeFlight)
            CleanDirectory(DataBaseSection.FreeFlight)

            '#############################################
            ' Transform the model to the main inertia axes
            '#############################################

            RaiseEvent PushProgress("Transforming model", 0)

            ' Lattices
            '----------------------------
            For Each Lattice In Lattices

                For Each Node In Lattice.Nodes

                    Node.Position.Substract(Settings.CenterOfGravity)
                    Node.Position.Transform(Settings.MainInertialAxes)

                Next

                Lattice.CalculateLatticeParameters()

            Next

            ' Stream
            '----------------------------

            Settings.StreamVelocity.Transform(Settings.MainInertialAxes)
            Settings.StreamOmega.SetToCero()

            ' Gravity
            '----------------------------

            Settings.Gravity.Transform(Settings.MainInertialAxes)

            '#############################################
            ' Build integrator
            '#############################################

            RaiseEvent PushMessage("Building integrator")
            Dim MotionSteps As Integer = Settings.SimulationSteps - Settings.FreeFlightStartStep + 1
            Dim MotionIntegrator As New HammingIntegrator(MotionSteps,
                                                          Settings.Interval,
                                                         -Settings.StreamVelocity,
                                                          Settings.StreamOmega,
                                                          Settings.Gravity)
            MotionIntegrator.Mass = Settings.Mass
            MotionIntegrator.Ixx = Settings.Ixx
            MotionIntegrator.Iyy = Settings.Iyy
            MotionIntegrator.Izz = Settings.Izz

            '#############################################
            ' Setup initial stream
            '#############################################

            Stream.Density = Settings.Density
            Stream.Velocity.Assign(Settings.StreamVelocity)
            Stream.SquareVelocity = Stream.Velocity.SquareEuclideanNorm
            Stream.DynamicPressure = 0.5 * Stream.Density * Stream.SquareVelocity

            '#############################################
            ' Build aerodynamic influence matrix
            '#############################################

            RaiseEvent PushProgress("Building aerodynamic matrices", 0)

            WithStreamRotation = True
            BuildMatrixForDoublets()
            BuildRightHandSide1()
            InitializeWakes()

            Dim AerodynamicEquations As New LinearEquations
            AerodynamicEquations.ComputeLU(MatrixDoublets)
            G = New Vector(Dimension)

            For TimeStep = 1 To Settings.SimulationSteps

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                RaiseEvent PushProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                If TimeStep >= Settings.FreeFlightStartStep Then

                    '##################################'
                    ' Free motion step
                    '##################################'

                    If TimeStep = Settings.FreeFlightStartStep Then

                        '//////////////////////////////////'
                        ' Release the model
                        '//////////////////////////////////'

                        RaiseEvent PushMessage(" > Releasing the model")

                        ' Calculating initial airloads
                        '----------------------------------------

                        CalculateTotalVelocityOnBoundedLattices()

                        For Each Lattice In Lattices
                            Lattice.CalculatePressure(Stream.SquareVelocity)
                        Next

                        CalculateAirloads()

                        MotionIntegrator.SetInitialForces(GlobalAirloads.Force, GlobalAirloads.Moment)

                    End If

                    Dim Converged As Boolean = False
                    Dim Finalized As Boolean = False

                    For IterStep = 0 To Settings.CorrectionSteps ' (replace with maximum number of iterations)

                        If IterStep = 0 Then

                            '/////////////////////////////////////////'
                            ' Predic motion using previous derivatives
                            '/////////////////////////////////////////'

                            ' Cache the initial position of the wakes
                            '----------------------------------------

                            For Each Lattice In Lattices
                                Lattice.CacheWakePosition()
                            Next

                            ' Predict
                            '----------------------------------------

                            RaiseEvent PushMessage("  -> Predicting motion")

                            MotionIntegrator.Predict()

                            ' NOTE: as seen from the aircraft, the stream moves in the oposite direction

                            Stream.Velocity.Assign(MotionIntegrator.Velocity, -1.0#)
                            Stream.Rotation.Assign(MotionIntegrator.Rotation, -1.0#)
                            Stream.SquareVelocity = Stream.Velocity.SquareEuclideanNorm
                            Stream.DynamicPressure = 0.5 * Stream.Density * Stream.SquareVelocity

                            ' Convect wakes for this time step
                            '----------------------------------------

                            CalculateVelocityOnWakes()

                            For Each Lattice In Lattices
                                Lattice.PopulateWakeVortices(Settings.Interval, TimeStep, False, Nothing)
                            Next

                        Else

                            '//////////////////////////////////////'
                            ' Find new circulation
                            '//////////////////////////////////////'

                            If Not Converged Then
                                RaiseEvent PushMessage("  -> Correcting motion")
                            End If

                            CalculateVelocityInducedByTheWakesOnBoundedLattices()

                            BuildRightHandSide2()

                            AerodynamicEquations.SolveLU(RHS, G)

                            AssignDoublets()

                            '//////////////////////////////////'
                            ' Calculate new airloads
                            '//////////////////////////////////'

                            CalculateTotalVelocityOnBoundedLattices()

                            For Each Lattice In Lattices
                                Lattice.CalculatePressure(Stream.SquareVelocity)
                            Next

                            CalculateAirloads()

                            '//////////////////////////////////'
                            ' Exit here if already converged
                            '//////////////////////////////////'

                            If Converged Then
                                RaiseEvent PushMessage(" > Convergence reached")
                                Finalized = True
                                Exit For
                            End If

                            '//////////////////////////////////'
                            ' Calculate new kinematic state
                            '//////////////////////////////////'

                            Converged = MotionIntegrator.Correct(GlobalAirloads.Force, GlobalAirloads.Moment)

                            Stream.Velocity.Assign(MotionIntegrator.Velocity, -1.0#)
                            Stream.Rotation.Assign(MotionIntegrator.Rotation, -1.0#)
                            Stream.SquareVelocity = Stream.Velocity.SquareEuclideanNorm
                            Stream.DynamicPressure = 0.5 * Stream.Density * Stream.SquareVelocity

                            CalculateVelocityOnWakes()

                            For Each Lattice In Lattices
                                Lattice.ReconvectWakes(Settings.Interval)
                            Next

                        End If

                    Next

                    If Not Finalized Then
                        RaiseEvent PushMessage("Could not reach convergence")
                        If Converged Then
                            RaiseEvent PushMessage("(consider one more correction step)")
                        End If
                        RaiseEvent PushMessage("Calculation aborted")
                        RaiseEvent CalculationDone()
                        Exit Sub
                    End If

                    '//////////////////////////////////'
                    ' Save current step
                    '//////////////////////////////////'

                    RaiseEvent PushMessage(" > Writing binaries")

                    WriteToXML(FreeFlightResFile(TimeStep))

                Else

                    '##################################'
                    ' Constrained flight step
                    '##################################'

                    '//////////////////////////////////'
                    ' Solve the equations for G        '
                    '//////////////////////////////////'

                    AerodynamicEquations.SolveLU(RHS, G)

                    AssignDoublets()

                    '//////////////////////////////////'
                    ' Convect wakes                    '
                    '//////////////////////////////////'

                    RaiseEvent PushMessage(" > Convecting wakes")

                    CalculateVelocityOnWakes()

                    For Each Lattice In Lattices
                        Lattice.PopulateWakeVortices(Settings.Interval, TimeStep, False, Nothing)
                    Next

                    '//////////////////////////////////'
                    ' Rebuild RHS                      '
                    '//////////////////////////////////'

                    CalculateVelocityInducedByTheWakesOnBoundedLattices()

                    BuildRightHandSide2()

                End If

            Next

            RaiseEvent PushProgress("Calculation finished", 100)

            '//////////////////////////////////'
            ' Output time response             '
            '//////////////////////////////////'

            RaiseEvent PushProgress("Writting motion to file...", 100)

            Dim FileId As Integer = FreeFile()
            FileOpen(FileId, System.IO.Path.Combine(FreeFlightPath, "response.txt"), OpenMode.Output)
            PrintLine(FileId, String.Format("{0:11} | {1:11} | {2:11} | {3:11} | {4:11} | {5:11} | {6:11} | {7:11} | {8:11} | {9:11}", "Time", "Px", "Py", "Pz", "Vx", "Vy", "Vz", "Ox", "Oy", "Oz"))
            Dim Time As Double = 0.0#

            For I = 0 To MotionSteps
                Dim State As Variable = MotionIntegrator.State(I)
                PrintLine(FileId, String.Format("{0,11:F6} | {1,11:F6} | {2,11:F6} | {3,11:F6} | {4,11:F6} | {5,11:F6} | {6,11:F6} | {7,11:F6} | {8,11:F6} | {9,11:F6}", Time, State.Px, State.Py, State.Pz, State.Vx, State.Vy, State.Vz, State.Ox, State.Oy, State.Oz))
                Time += Settings.Interval
            Next

            FileClose(FileId)

            RaiseEvent PushMessage("Finished")
            RaiseEvent CalculationDone()

        End Sub

    End Class

End Namespace


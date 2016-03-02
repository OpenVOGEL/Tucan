'Copyright (C) 2016 Guillermo Hazebrouck

Imports DotNumerics.LinearAlgebra

Namespace UVLM.Solver

    Partial Public Class UVLMSolver

        ''' <summary>
        ''' Calculates the unsteady transit provided a velocity profile.
        ''' </summary>
        Public Sub UnsteadyTransit(ByVal DataBasePath As String)

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Unsteady)
            CleanDirectory(DataBaseSection.Unsteady)

            RaiseEvent PushMessage("Generating stream velocity")

            Settings.GenerateVelocityProfile()

            RaiseEvent PushProgress("Building matrix", 0)

            BuildMatrixForDoublets()
            BuildRHS_I()
            InitializeWakes()

            Dim LE As New LinearEquations
            LE.ComputeLU(MatrixDoublets)
            G = New Vector(Dimension)

            For TimeStep = 1 To Settings.SimulationSteps

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                _StreamVelocity.Assign(Settings.UnsteadyVelocity.Velocity(TimeStep))
                _StreamDensity = Settings.Density
                Dim SquareVelocity As Double = _StreamVelocity.SquareEuclideanNorm
                _StreamDynamicPressure = 0.5 * _StreamDensity * SquareVelocity

                RaiseEvent PushProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                RaiseEvent PushMessage(" > Convecting wakes")

                LE.SolveLU(RHS, G)

                AssignDoublets()

                ' Calculate induced velocity on wake NP:

                CalculateVelocityOnWakes()

                ' Convect wake:

                For Each Lattice In Lattices

                    Lattice.PopulateWakeVortices(Settings.Interval, TimeStep, False)

                Next

                CalculateVelocityInducedByTheWakesOnBoundedLattices()

                RaiseEvent PushMessage(" > Calculating airloads")

                CalculateTotalVelocityOnBoundedLattices()

                For Each Lattice In Lattices

                    Lattice.CalculatePressure(_StreamVelocity.SquareEuclideanNorm)

                Next

                ' Save current step

                RaiseEvent PushMessage(" > Writing binaries")

                Me.WriteToXML(String.Format("{0}\Unsteady.T{1}.res", Transit_Path, TimeStep))

                BuildRHS_II()

                'Next time step

            Next

            RaiseEvent PushProgress("Calculation finished", 100)
            RaiseEvent PushMessage("Finished")
            RaiseEvent CalculationDone()

        End Sub

    End Class

End Namespace


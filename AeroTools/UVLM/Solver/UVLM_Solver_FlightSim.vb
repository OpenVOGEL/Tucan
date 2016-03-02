'Copyright (C) 2016 Guillermo Hazebrouck

Imports DotNumerics.LinearAlgebra

Namespace UVLM.Solver

    Partial Public Class UVLMSolver

        ' Resolution algorithms:

        ''' <summary>
        ''' Convect wakes and calculates loads at the last time step.
        ''' </summary>
        Public Sub FlightSim(ByRef f As FormProgress, ByVal DataBasePath As String)

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Steady)
            CleanDirectory(DataBaseSection.Steady)

            _StreamVelocity.Assign(Settings.StreamVelocity)
            _StreamOmega.Assign(Settings.Omega)
            _StreamDensity = Settings.Density
            Dim SquareVelocity As Double = _StreamVelocity.SquareEuclideanNorm
            _StreamDynamicPressure = 0.5 * _StreamDensity * SquareVelocity
            Dim WithStreamOmega As Boolean = _StreamOmega.EuclideanNorm > 0.00001

            f.PushMessageWithProgress("Building matrix", 0)

            BuildMatrixForDoublets()
            BuildRHS_I()
            InitializeWakes()

            Dim LE As New LinearEquations
            LE.ComputeLU(MatrixDoublets)
            G = New Vector(Dimension)

            For TimeStep = 1 To Settings.SimulationSteps

                f.PushMessageWithProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                LE.Solve(RHS, G)

                AssignDoublets()

                ' Calculate induced velocity on wake NP:

                CalculateVelocityOnWakes(WithStreamOmega)

                ' Convect wake:

                For Each Lattice In Lattices

                    Lattice.PopulateWakeRings(Settings.Interval, TimeStep)

                Next

                CalculateVelocityInducedByTheWakesOnBoundedLattices(WithStreamOmega)

                BuildRHS_II()

                'Next time step

                f.PushMessageWithProgress("Calculating airloads", 0)

                CalculateTotalVelocityOnBoundedLattices()

                ' Calculate vortex rings Cp or DCp:

                For Each Lattice In Lattices

                    Lattice.CalculatePressure(SquareVelocity)

                Next

                ' Calculate the total airload:

                CalculateAirloads()

            Next


            ' Ready

            f.PushMessageWithProgress("Writing to database", 100)

            Me.WriteToXML(String.Format("{0}\Steady.res", Steady_Path), True)

            f.PushState("Calculation finished")

        End Sub

    End Class

End Namespace
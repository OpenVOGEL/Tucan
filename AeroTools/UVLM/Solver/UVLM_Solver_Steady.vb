'Copyright (C) 2016 Guillermo Hazebrouck

Imports DotNumerics.LinearAlgebra

Namespace UVLM.Solver

    Partial Public Class UVLMSolver

        Private _WithSources As Boolean = False

        Public ReadOnly Property WithSources As Boolean
            Get
                Return _WithSources
            End Get
        End Property

        ''' <summary>
        ''' Convect wakes and calculates loads at the last time step.
        ''' </summary>
        Public Sub SteadyState(ByVal DataBasePath As String)

            Dim StartingTime As Date = Now

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Steady)
            CleanDirectory(DataBaseSection.Steady)

            _StreamVelocity.Assign(Settings.StreamVelocity)

            _StreamOmega.Assign(Settings.Omega)

            _StreamDensity = Settings.Density

            Dim SquareVelocity As Double = _StreamVelocity.SquareEuclideanNorm

            _StreamDynamicPressure = 0.5 * _StreamDensity * SquareVelocity

            Dim WithStreamOmega As Boolean = _StreamOmega.EuclideanNorm > 0.00001

            RaiseEvent PushMessage("Building doublets matrix")
            BuildMatrixForDoublets()

            If WithSources Then

                RaiseEvent PushMessage("Building sources matrix")
                BuildMatrixForSources()

                RaiseEvent PushMessage("Assigning sources")
                AssignSources()

            End If

            RaiseEvent PushMessage("Building RHS")
            BuildRHS_I()

            RaiseEvent PushMessage("Initializing wakes")
            InitializeWakes()

            RaiseEvent PushMessage(String.Format("Generating LU decomposition ({0})", Dimension))
            Dim LE As New LinearEquations
            LE.ComputeLU(MatrixDoublets)
            G = New Vector(Dimension)

            For TimeStep = 1 To Settings.SimulationSteps

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                RaiseEvent PushProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                LE.SolveLU(RHS, G)

                AssignDoublets()

                ' Calculate induced velocity on wake NP:

                CalculateVelocityOnWakes(WithStreamOmega)

                ' Convect wake:

                For Each Lattice In Lattices

                    If WithSources And Settings.StrongWakeInfluence Then

                        Lattice.PopulateWakeRingsAndVortices(Settings.Interval, TimeStep)

                    Else

                        ' We tame the root vortex when there are fuselages.

                        Lattice.PopulateWakeVortices(Settings.Interval, TimeStep, WithSources)

                    End If

                Next

                CalculateVelocityInducedByTheWakesOnBoundedLattices(True)

                If WithSources And Settings.StrongWakeInfluence Then

                    CalculatePotentialInducedByTheWakeOnThickBoundedLattices()

                End If

                BuildRHS_II(WithStreamOmega)

                'Next time step

            Next

            RaiseEvent PushMessage("Calculating airloads")

            ' Complete the last step

            LE.SolveLU(RHS, G)

            AssignDoublets()

            CalculateVelocityInducedByTheWakesOnBoundedLattices(False)

            ' Calculate total velocity:

            CalculateTotalVelocityOnBoundedLattices(WithStreamOmega)

            ' Calculate vortex rings Cp or DCp:

            For Each Lattice In Lattices

                Lattice.CalculatePressure(SquareVelocity)

            Next

            ' Calculate the total airload:

            CalculateAirloads()

            ' Ready

            RaiseEvent PushMessage("Writing to database")

            Me.WriteToXML(String.Format("{0}\Steady.res", Steady_Path), True)

            Dim Interval As TimeSpan = Now - StartingTime
            Dim Message As String = String.Format("Calculation finished. Elapsed time: {0}m {1}.{2}s", Interval.Minutes, Interval.Seconds, Interval.Milliseconds)
            RaiseEvent PushMessage(Message)

            RaiseEvent CalculationDone()

        End Sub

    End Class

End Namespace


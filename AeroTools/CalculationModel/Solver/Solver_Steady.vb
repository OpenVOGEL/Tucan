'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

Namespace CalculationModel.Solver

    Partial Public Class Solver

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

            Dim WithStreamOmega As Boolean = _StreamOmega.EuclideanNorm > 0

            RaiseEvent PushMessage("Building doublets matrix")
            BuildMatrixForDoublets()

            If WithSources Then

                RaiseEvent PushMessage("Building sources matrix")
                BuildMatrixForSources()

                RaiseEvent PushMessage("Assigning sources")
                AssignSources(WithStreamOmega)

            End If

            RaiseEvent PushMessage("Building RHS")
            BuildRHS_I(WithStreamOmega)

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

                        Lattice.PopulateWakeVortices(Settings.Interval, TimeStep)

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


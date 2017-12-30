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

        ''' <summary>
        ''' Calculates the unsteady transit provided a velocity profile.
        ''' </summary>
        Public Sub UnsteadyTransit(ByVal DataBasePath As String)

            If _WithSources Then

                RaiseEvent PushMessage("Cannot run transit analysis with thick bodies")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Unsteady)
            CleanDirectory(DataBaseSection.Unsteady)

            RaiseEvent PushMessage("Generating stream velocity")

            Settings.GenerateVelocityProfile()

            RaiseEvent PushProgress("Building matrix", 0)

            Dim WithStreamOmega As Boolean = _StreamOmega.EuclideanNorm > 0.00001

            BuildMatrixForDoublets()
            BuildRHS_I(WithStreamOmega)
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

                CalculateVelocityOnWakes(WithStreamOmega)

                ' Convect wake:

                For Each Lattice In Lattices

                    Lattice.PopulateWakeVortices(Settings.Interval, TimeStep, False)

                Next

                CalculateVelocityInducedByTheWakesOnBoundedLattices()

                RaiseEvent PushMessage(" > Calculating airloads")

                CalculateTotalVelocityOnBoundedLattices(WithStreamOmega)

                For Each Lattice In Lattices

                    Lattice.CalculatePressure(_StreamVelocity.SquareEuclideanNorm)

                Next

                ' Save current step

                RaiseEvent PushMessage(" > Writing binaries")

                Me.WriteToXML(String.Format("{0}\Unsteady.T{1}.res", Transit_Path, TimeStep))

                BuildRHS_II(WithStreamOmega)

                'Next time step

            Next

            RaiseEvent PushProgress("Calculation finished", 100)
            RaiseEvent PushMessage("Finished")
            RaiseEvent CalculationDone()

        End Sub

    End Class

End Namespace


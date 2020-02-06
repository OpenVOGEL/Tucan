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

Namespace CalculationModel.Solver

    Partial Public Class Solver

        ''' <summary>
        ''' Calculates the unsteady transit provided a velocity profile.
        ''' </summary>
        Public Sub UnsteadyTransit(ByVal DataBasePath As String)

            If WithSources Then

                RaiseEvent PushMessage("Cannot run transit analysis with thick bodies")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            If Settings.UseGpu Then

                RaiseEvent PushMessage("Cannot run transit analysis with OpenCL")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Unsteady)
            CleanDirectory(DataBaseSection.Unsteady)

            RaiseEvent PushMessage("Generating stream velocity")

            Settings.GenerateVelocityHistogram()

            RaiseEvent PushProgress("Building matrix", 0)

            WithStreamOmega = Stream.Omega.EuclideanNorm > 0.00001

            BuildMatrixForDoublets()
            BuildRightHandSide1()
            InitializeWakes()

            Dim LE As New LinearEquations
            LE.ComputeLU(MatrixDoublets)
            G = New Vector(Dimension)

            For TimeStep = 1 To Settings.SimulationSteps

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                Stream.Velocity.Assign(Settings.UnsteadyVelocity.Velocity(TimeStep))
                Stream.Density = Settings.Density
                Stream.SquareVelocity = Stream.Velocity.SquareEuclideanNorm
                Stream.DynamicPressure = 0.5 * Stream.Density * Stream.SquareVelocity

                RaiseEvent PushProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                RaiseEvent PushMessage(" > Convecting wakes")

                LE.SolveLU(RHS, G)

                AssignDoublets()

                ' Calculate induced velocity on wake NP:

                CalculateVelocityOnWakes()

                ' Convect wake:

                For Each Lattice In Lattices

                    Lattice.PopulateWakeVortices(Settings.Interval, TimeStep, False, Nothing)

                Next

                CalculateVelocityInducedByTheWakesOnBoundedLattices()

                RaiseEvent PushMessage(" > Calculating airloads")

                CalculateTotalVelocityOnBoundedLattices()

                For Each Lattice In Lattices

                    Lattice.CalculatePressure(Stream.SquareVelocity)

                Next

                ' Save current step

                RaiseEvent PushMessage(" > Writing binaries")

                Me.WriteToXML(String.Format("{0}\Unsteady.T{1}.res", Transit_Path, TimeStep))

                BuildRightHandSide2()

                'Next time step

            Next

            RaiseEvent PushProgress("Calculation finished", 100)
            RaiseEvent PushMessage("Finished")
            RaiseEvent CalculationDone()

        End Sub

    End Class

End Namespace


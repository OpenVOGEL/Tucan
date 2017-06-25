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
            BuildRHS_I(WithStreamOmega)
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

                BuildRHS_II(WithStreamOmega)

                'Next time step

                f.PushMessageWithProgress("Calculating airloads", 0)

                CalculateTotalVelocityOnBoundedLattices(WithStreamOmega)

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
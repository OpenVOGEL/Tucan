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

        ' Resolution algorithms:

        ''' <summary>
        ''' Convect wakes and calculates loads at the last time step.
        ''' </summary>
        Public Sub FreeFlight(ByVal DataBasePath As String)

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Steady)
            CleanDirectory(DataBaseSection.Steady)

            Stream.Velocity.Assign(Settings.StreamVelocity)
            Stream.Omega.Assign(Settings.Omega)
            Stream.Density = Settings.Density
            Stream.SquareVelocity = Stream.Velocity.SquareEuclideanNorm
            Stream.DynamicPressure = 0.5 * StreamDensity * Stream.SquareVelocity
            WithStreamOmega = True

            'f.PushMessageWithProgress("Building matrix", 0)

            BuildMatrixForDoublets()
            BuildRightHandSide1()
            InitializeWakes()

            Dim LE As New LinearEquations
            LE.ComputeLU(MatrixDoublets)
            G = New Vector(Dimension)

            For TimeStep = 1 To Settings.SimulationSteps

                'f.PushMessageWithProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                LE.Solve(RHS, G)

                AssignDoublets()

                ' Calculate induced velocity on wake NP:

                CalculateVelocityOnWakes()

                ' Convect wake:

                For Each Lattice In Lattices

                    Lattice.PopulateWakeRings(Settings.Interval, TimeStep, False, Nothing)

                Next

                CalculateVelocityInducedByTheWakesOnBoundedLattices()

                BuildRightHandSide2()

                'Next time step

                'PushMessageWithProgress("Calculating airloads", 0)

                CalculateTotalVelocityOnBoundedLattices()

                ' Calculate vortex rings Cp or DCp:

                For Each Lattice In Lattices

                    Lattice.CalculatePressure(Stream.SquareVelocity)

                Next

                ' Calculate the total airload:

                CalculateAirloads()

            Next


            ' Ready

            'f.PushMessageWithProgress("Writing to database", 100)

            Me.WriteToXML(String.Format("{0}\Steady.res", Steady_Path), True)

            'f.PushState("Calculation finished")

        End Sub

    End Class

End Namespace
'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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
        ''' The explicit Newmark algorithm is used.
        ''' </summary>
        Public Sub AeroelasticUnsteadyTransit_(ByVal DataBasePath As String)

            If _WithSources Then

                RaiseEvent PushMessage("Cannot run aeroelastic analysis with thick bodies. Calculation stopped.")
                RaiseEvent CalculationDone()
                Exit Sub

            End If

            ' Create database:

            RaiseEvent PushMessage("Creating database structure")

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Aeroelastic)
            CleanDirectory(DataBaseSection.Aeroelastic)
            CreateSubFolder(DataBaseSection.Structural)
            CleanDirectory(DataBaseSection.Structural)

            RaiseEvent PushMessage("Generating stream velocity")

            Settings.GenerateVelocityProfile()

            If IsNothing(Settings.AeroelasticHistogram) Then
                RaiseEvent PushMessage("Unable to generate aeroelastic histogram!")
                Return
            End If

            ' Initialize aerodynamic model

            RaiseEvent PushMessage("Building matrix")

            ' Build starting matrix and RHS:

            Dim WithStreamOmega As Boolean = _StreamOmega.EuclideanNorm > 0.00001

            BuildMatrixForDoublets(True)
            BuildRHS_I(WithStreamOmega)
            InitializeWakes()

            ' Initialize structural link:

            Dim l As Integer = 0

            Dim sDt As Double = Double.MaxValue

            For Each StructuralLink As StructuralLink In StructuralLinks ' This should be in parallel

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                l += 1

                RaiseEvent PushMessage(String.Format("Creating structural link {0}", l))

                RaiseEvent PushMessage("Creating mass and stiffness matrices...")
                StructuralLink.StructuralCore.CreateMatrices(Structure_Path, True)

                RaiseEvent PushMessage("Finding modes...")
                StructuralLink.StructuralCore.FindModes(Structure_Path, l)

                ' Update the minimum modal period:

                For Each Mode In StructuralLink.StructuralCore.Modes

                    Mode.C = Settings.AeroelasticHistogram.State(0).Damping * Mode.Cc
                    sDt = Math.Min(sDt, 2 * Math.PI / Mode.w)

                Next

            Next

            ' Calculate the structural step interval:

            Dim StructuralSteps As Integer = Math.Max(1, Math.Round(Settings.Interval / sDt) * 10)

            sDt = Settings.Interval / StructuralSteps

            RaiseEvent PushMessage(String.Format("Structural step: {0:F4}s (x{1})", sDt, StructuralSteps))

            Settings.StructuralSettings.SubSteps = StructuralSteps

            ' Initialize the links with the selected step interval:

            For Each StructuralLink As StructuralLink In StructuralLinks

                RaiseEvent PushMessage("Initializing links...")
                StructuralLink.Initialize(sDt)

            Next

            RaiseEvent PushMessage("All links have been succesfully created")

            ' Start inegration:

            Dim LE As New LinearEquations

            For TimeStep = 1 To Settings.SimulationSteps

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                ' Update stream parameters:

                _StreamVelocity.Assign(Settings.AeroelasticHistogram.State(TimeStep).Velocity)

                _StreamDensity = Settings.Density

                Dim SquareVelocity As Double = _StreamVelocity.SquareEuclideanNorm

                _StreamDynamicPressure = 0.5 * _StreamDensity * SquareVelocity

                ' Update modal damping and integrators data:

                For Each StructuralLink As StructuralLink In StructuralLinks

                    Dim dampingChanged As Boolean = False

                    For Each Mode In StructuralLink.StructuralCore.Modes

                        Mode.C = Settings.AeroelasticHistogram.State(TimeStep).Damping * Mode.Cc

                        dampingChanged = dampingChanged Or (Settings.AeroelasticHistogram.State(TimeStep - 1).Damping <> Settings.AeroelasticHistogram.State(TimeStep).Damping)

                    Next

                    If dampingChanged Then
                        StructuralLink.UpdateIntegrators()
                    End If

                Next

                ' Perform one step:

                RaiseEvent PushProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                ' Rebuild matrix (from 2nd time step)

                If TimeStep > 1 Then BuildMatrixForDoublets(False)

                '  Calculate new circulation and its time derivative:

                G = LE.Solve(MatrixDoublets, RHS)

                AssignDoublets()

                ' Calculate induced velocity on wake NP:

                CalculateVelocityOnWakes(WithStreamOmega)

                ' Convect wake:

                For Each Lattice In Lattices

                    Lattice.PopulateWakeVortices(Settings.Interval, TimeStep)

                Next

                ' Calculate velocityI at rings NPs for RHS and airloads:

                CalculateVelocityInducedByTheWakesOnBoundedLattices()

                RaiseEvent PushMessage("Calculating airloads")

                ' Calculate velocityII (total velocity) at rings NPs (for airloads):

                CalculateTotalVelocityOnBoundedLattices(WithStreamOmega)

                For Each Lattice In Lattices

                    Lattice.CalculatePressure(SquareVelocity)

                Next

                If TimeStep > Settings.StructuralSettings.StructuralLinkingStep Then ' Leave wake be convected before starting structural interaction

                    ' Update structure displacement:

                    For k = 0 To StructuralSteps

                        For Each StructuralLink As StructuralLink In StructuralLinks ' This should be in parallel

                            StructuralLink.Integrate(_StreamVelocity, _StreamDensity)

                        Next

                    Next

                    ' Recomposes the wake on the primitive nodes

                    For Each Lattice In Lattices ' This should be in parallel
                        Lattice.ReEstablishWakes()
                    Next

                    ' Calculate new right hand side at new deformed position with new surface velocity:
                    ' (not required when the link has not been performed)

                    CalculateVelocityInducedByTheWakesOnBoundedLattices()

                End If

                BuildRHS_II(WithStreamOmega)

            Next

            ' Save last time step

            RaiseEvent PushMessage("Writing binaries")

            WriteToXML(String.Format("{0}\Aeroelastic.res", Aeroelastic_Path, Settings.SimulationSteps))

            RaiseEvent CalculationDone()

        End Sub

        ''' <summary>
        ''' Calculates the unsteady aeroelastic transit provided a velocity profile and a structural model.
        ''' The implicit Nemark algorithm is used.
        ''' </summary>
        Public Sub AeroelasticUnsteadyTransit(ByVal DataBasePath As String)

            If _WithSources Then

                RaiseEvent PushMessage("Cannot run aeroelastic analysis with thick bodies")
                RaiseEvent CalculationAborted()
                Exit Sub

            End If

            ' Create database:

            RaiseEvent PushMessage("Creating database structure")

            CreateSubFoldersNames(DataBasePath)
            CreateSubFolder(DataBaseSection.Aeroelastic)
            CleanDirectory(DataBaseSection.Aeroelastic)
            CreateSubFolder(DataBaseSection.Structural)
            CleanDirectory(DataBaseSection.Structural)

            RaiseEvent PushMessage("Generating stream velocity")

            Settings.GenerateVelocityProfile()

            If IsNothing(Settings.AeroelasticHistogram) Then
                RaiseEvent PushMessage("Unable to generate aeroelastic histogram!")
                Return
            End If

            ' Initialize aerodynamic model

            RaiseEvent PushMessage("Building matrix")

            ' Build starting matrix and RHS:

            Dim WithStreamOmega As Boolean = _StreamOmega.EuclideanNorm > 0.00001

            BuildMatrixForDoublets(True)
            BuildRHS_I(WithStreamOmega)
            InitializeWakes()

            ' Initialize structural link:

            Dim l As Integer = 0

            Dim sDt As Double = Double.MaxValue

            For Each StructuralLink As StructuralLink In StructuralLinks ' This should be in parallel

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                l += 1

                RaiseEvent PushMessage(String.Format("Creating structural link {0}", l))

                RaiseEvent PushMessage("Creating mass and stiffness matrices...")
                StructuralLink.StructuralCore.CreateMatrices(Structure_Path, True)

                RaiseEvent PushMessage("Finding modes...")
                StructuralLink.StructuralCore.FindModes(Structure_Path, l)

                ' Update the minimum modal period:

                For Each Mode In StructuralLink.StructuralCore.Modes

                    Mode.C = Settings.AeroelasticHistogram.State(0).Damping * Mode.Cc
                    sDt = Math.Min(sDt, 2 * Math.PI / Mode.w)

                Next

            Next

            ' Calculate the structural step interval:

            Dim StructuralSteps As Integer = Math.Max(1, Math.Round(Settings.Interval / sDt) * 10)

            sDt = Settings.Interval / StructuralSteps

            RaiseEvent PushMessage(String.Format("Structural step: {0:F4}s (x{1})", sDt, StructuralSteps))

            Settings.StructuralSettings.SubSteps = StructuralSteps

            ' Initialize the links with the selected step interval:

            For Each StructuralLink As StructuralLink In StructuralLinks

                RaiseEvent PushMessage("Initializing links...")
                StructuralLink.Initialize(sDt)

            Next

            RaiseEvent PushMessage("All links have been succesfully created")

            ' Start inegration:

            Dim LE As New LinearEquations

            For TimeStep = 1 To Settings.SimulationSteps

                If CancellationPending Then
                    CancelProcess()
                    Return
                End If

                ' Update stream parameters:

                _StreamVelocity.Assign(Settings.AeroelasticHistogram.State(TimeStep).Velocity)

                _StreamDensity = Settings.Density

                Dim SquareVelocity As Double = _StreamVelocity.SquareEuclideanNorm

                _StreamDynamicPressure = 0.5 * _StreamDensity * SquareVelocity

                ' Update modal damping and integrators data:

                For Each StructuralLink As StructuralLink In StructuralLinks

                    Dim dampingChanged As Boolean = False

                    For Each Mode In StructuralLink.StructuralCore.Modes

                        Mode.C = Settings.AeroelasticHistogram.State(TimeStep).Damping * Mode.Cc

                        dampingChanged = dampingChanged Or (Settings.AeroelasticHistogram.State(TimeStep - 1).Damping <> Settings.AeroelasticHistogram.State(TimeStep).Damping)

                    Next

                    If dampingChanged Then
                        StructuralLink.UpdateIntegrators()
                    End If

                Next

                RaiseEvent PushProgress(String.Format("Step {0}", TimeStep), 100 * TimeStep / Settings.SimulationSteps)

                ' Perform one step:

                ' BEGIN NEWMARK LOOP:

                Dim Converged As Boolean = False

                Dim k As Integer = 0

                While Not Converged

                    ' Calculate new right hand side at new deformed position with new surface velocity:

                    CalculateVelocityInducedByTheWakesOnBoundedLattices()

                    ' Rebuild RHS:

                    BuildRHS_II(WithStreamOmega)

                    ' Rebuild matrix (from 2nd time step on)

                    BuildMatrixForDoublets(False)

                    '  Calculate new circulation and its time derivative:

                    G = LE.Solve(MatrixDoublets, RHS)

                    AssignDoublets()

                    If TimeStep > Settings.StructuralSettings.StructuralLinkingStep Then ' Leave wake be convected before starting structural interaction

                        RaiseEvent PushMessage("Calculating airloads")

                        ' Calculate velocityII (total velocity) at rings NPs (for airloads):

                        CalculateTotalVelocityOnBoundedLattices(WithStreamOmega)

                        For Each Lattice In Lattices

                            Lattice.CalculatePressure(SquareVelocity)

                        Next

                        ' Update structural displacement with the new loads:

                        Converged = True

                        For p = 0 To StructuralSteps

                            For Each StructuralLink As StructuralLink In StructuralLinks ' This should be in parallel

                                ' Compute one step in the fixed point iteration:

                                Converged = Converged And StructuralLink.Integrate(_StreamVelocity, _StreamDensity, k, 0.005)

                            Next

                        Next

                        If k = 0 Then Converged = False

                        If k >= 10 Then Converged = True

                        ' Recomposes the wake on the primitive nodes

                        For Each Lattice In Lattices ' This should be in parallel
                            Lattice.ReEstablishWakes()
                        Next

                        k += 1

                    Else

                        Converged = True

                    End If

                End While

                ' END OF LOOP: if converged, the system is in dynamic equilibrium.

                If Converged Then

                    ' Calculate induced velocity on wake NP:

                    CalculateVelocityOnWakes(WithStreamOmega)

                    ' Convect wake:

                    For Each Lattice In Lattices

                        Lattice.PopulateWakeVortices(Settings.Interval, TimeStep)

                    Next

                Else

                    RaiseEvent PushMessage("Error: the aeroelastic coupling didn't converge.")
                    Exit For

                End If

            Next

            ' Save last time step

            RaiseEvent PushMessage("Writing binaries")

            WriteToXML(String.Format("{0}\Aeroelastic.res", Aeroelastic_Path, Settings.SimulationSteps))

            RaiseEvent CalculationDone()

        End Sub


    End Class

End Namespace

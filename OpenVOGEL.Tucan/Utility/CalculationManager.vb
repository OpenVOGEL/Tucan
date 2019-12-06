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

Imports System.ComponentModel
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools.VisualModel.Models

Namespace Tucan.Utility

    ''' <summary>
    ''' Contains all necessary functions to launch an asynchronous calculation, so that the
    ''' progress form is updated during the process.
    ''' </summary>
    Module CalculationManager

        ''' <summary>
        ''' The background procedure performing the calculation in a different thread.
        ''' </summary>
        Private CalculationWorker As BackgroundWorker

        ''' <summary>
        ''' Starts a special asynchronous calculation process for Tucan (GUI compatible).
        ''' </summary>
        ''' <param name="Type"></param>
        ''' <param name="Parent"></param>
        Public Sub StartCalculation(ByVal Type As CalculationType, ByRef Parent As Control)

            If Not IsNothing(CalculationWorker) Then
                If CalculationWorker.IsBusy Then
                    Return
                Else
                    CalculationWorker.Dispose()
                End If
            End If

            FormProgress.ClearMessages()
            If Not IsNothing(Parent) Then FormProgress.Owner = Parent
            FormProgress.ClearMessages()
            FormProgress.Show()
            FormProgress.PushMessage("Preparing calculation cell")
            ProjectRoot.SimulationSettings.AnalysisType = Type

            Try

                CalculationCore = New AeroTools.CalculationModel.Solver.Solver
                CalculationCore.GenerateFromExistingModel(Model, ProjectRoot.SimulationSettings, Type = CalculationType.ctAeroelastic)

                AddHandler CalculationCore.PushProgress, AddressOf FormProgress.PushMessageWithProgress
                AddHandler CalculationCore.PushMessage, AddressOf FormProgress.PushMessage
                AddHandler CalculationCore.CalculationDone, AddressOf CalculationFinished
                AddHandler CalculationCore.CalculationDone, AddressOf FormProgress.ChangeToCloseModus
                AddHandler CalculationCore.CalculationAborted, AddressOf CalculationAborted

                Dim StartingTime As Date = Now
                Results.SimulationSettings = CalculationCore.Settings
                FormProgress.PushMessage("Calculating with parallel solver")

                CalculationWorker = New BackgroundWorker
                CalculationWorker.WorkerSupportsCancellation = True
                CalculationWorker.WorkerReportsProgress = True
                AddHandler FormProgress.CancellationRequested, AddressOf CalculationCore.RequestCancellation

                Select Case Type

                    Case CalculationType.ctSteady

                        AddHandler CalculationWorker.DoWork, AddressOf StartWakeConvection

                    Case CalculationType.ctUnsteady

                        AddHandler CalculationWorker.DoWork, AddressOf StartUnsteadyTransit

                    Case CalculationType.ctAeroelastic

                        AddHandler CalculationWorker.DoWork, AddressOf StartAeroelsaticTransit

                End Select

                CalculationWorker.RunWorkerAsync()

            Catch ex As Exception
                CalculationWorker.CancelAsync()
                FormProgress.PushMessage(String.Format("Calculation exited with exception: ""{0}"".", ex.Message))
                FormProgress.ChangeToCloseModus()
                Return
            End Try

        End Sub

        ''' <summary>
        ''' Callback for when the calculation is done.
        ''' </summary>
        Private Sub CalculationFinished()

            FormProgress.PushMessage("Loading results")
            CalculationCore.SetCompleteModelOnResults(Results)
            FormProgress.PushMessage("Ready")
            RaiseEvent CalculationDone()

        End Sub

        ''' <summary>
        ''' Callback for when the calculation is aborted.
        ''' </summary>
        Private Sub CalculationAborted()

            FormProgress.PushMessage("Calculation aborted")
            FormProgress.ChangeToCloseModus()
            RaiseEvent CalculationDone()

        End Sub

        ''' <summary>
        ''' Occurs when the calculation finishes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Event CalculationDone()

        ''' <summary>
        ''' Callback to start the steady state analysis.
        ''' </summary>
        Private Sub StartWakeConvection(ByVal sender As Object, ByVal e As DoWorkEventArgs)

            CalculationCore.SteadyState(FilePath)

        End Sub

        ''' <summary>
        ''' Callback to start the unsteady analysis.
        ''' </summary>
        Private Sub StartUnsteadyTransit(ByVal sender As Object, ByVal e As DoWorkEventArgs)

            CalculationCore.UnsteadyTransit(FilePath)

        End Sub

        ''' <summary>
        ''' Callback to start the aeroelastic analysis.
        ''' </summary>
        Private Sub StartAeroelsaticTransit(ByVal sender As Object, ByVal e As DoWorkEventArgs)

            CalculationCore.AeroelasticUnsteadyTransit(FilePath)

        End Sub

    End Module

End Namespace

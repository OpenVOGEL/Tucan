'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools.VisualModel.Models

Namespace Tucan.Utility

    ''' <summary>
    ''' Contains all necessary functions to launch an asynchronous calculation, so that the
    ''' progress form is updated during the process.
    ''' </summary>
    Module CalculationManager

        Private CalculationCore As AeroTools.CalculationModel.Solver.Solver = Nothing

        ''' <summary>
        ''' The background procedure performing the calculation in a different thread.
        ''' </summary>
        Private CalculationWorker As BackgroundWorker

        ''' <summary>
        ''' Starts a special asynchronous calculation process for Tucan (GUI compatible).
        ''' </summary>
        ''' <param name="Type"></param>
        ''' <param name="Parent"></param>
        Public Sub StartCalculation(ByVal Type As CalculationType, OnServer As Boolean, ByRef Parent As Control)

            ProjectRoot.SimulationSettings.AnalysisType = Type

            If OnServer Then
                RequestCalculationToServer()
                Exit Sub
            End If

            If Not IsNothing(CalculationWorker) Then
                If CalculationWorker.IsBusy Then
                    Return
                Else
                    CalculationWorker.Dispose()
                End If
            End If

            FormProgress.ClearMessages()
            FormProgress.PushMessage("Saving the model")
            ProjectRoot.WriteToXML()
            If Not IsNothing(Parent) Then FormProgress.Owner = Parent
            FormProgress.ClearMessages()
            FormProgress.Show()
            FormProgress.PushMessage("Preparing calculation cell")

            Try

                CalculationCore = New AeroTools.CalculationModel.Solver.Solver
                CalculationCore.GenerateFromExistingModel(Model, ProjectRoot.SimulationSettings, Type = CalculationType.Aeroelastic)

                AddHandler CalculationCore.PushProgress, AddressOf FormProgress.PushMessageWithProgress
                AddHandler CalculationCore.PushMessage, AddressOf FormProgress.PushMessage
                AddHandler CalculationCore.CalculationDone, AddressOf CalculationFinished
                AddHandler CalculationCore.CalculationDone, AddressOf FormProgress.ChangeToCloseModus
                AddHandler CalculationCore.CalculationAborted, AddressOf CalculationAborted

                Dim StartingTime As Date = Now

                FormProgress.PushMessage("Calculating with parallel solver")

                CalculationWorker = New BackgroundWorker
                CalculationWorker.WorkerSupportsCancellation = True
                CalculationWorker.WorkerReportsProgress = True
                AddHandler FormProgress.CancellationRequested, AddressOf CalculationCore.RequestCancellation

                Select Case Type

                    Case CalculationType.SteadyState

                        AddHandler CalculationWorker.DoWork, AddressOf StartSteadyStateTransit

                    Case CalculationType.Aeroelastic

                        AddHandler CalculationWorker.DoWork, AddressOf StartAeroelsaticTransit

                    Case CalculationType.FreeFlight

                        AddHandler CalculationWorker.DoWork, AddressOf StartFreeFlightTransit

                End Select

                CalculationWorker.RunWorkerAsync()

            Catch ex As Exception
                If CalculationWorker IsNot Nothing Then
                    CalculationWorker.CancelAsync()
                End If
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
            Results.LoadFromDirectory(CalculationCore.BaseDirectoryPath)
            CalculationCore = Nothing
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
        Private Sub StartSteadyStateTransit(ByVal sender As Object, ByVal e As DoWorkEventArgs)

            CalculationCore.SteadyStateTransit(FilePath)

        End Sub

        ''' <summary>
        ''' Callback to start the unsteady analysis.
        ''' </summary>
        Private Sub StartFreeFlightTransit(ByVal sender As Object, ByVal e As DoWorkEventArgs)

            CalculationCore.FreeFlight(FilePath)

        End Sub

        ''' <summary>
        ''' Callback to start the aeroelastic analysis.
        ''' </summary>
        Private Sub StartAeroelsaticTransit(ByVal sender As Object, ByVal e As DoWorkEventArgs)

            CalculationCore.AeroelasticTransit(FilePath)

        End Sub

        ''' <summary>
        ''' Requests the calculation to the OpenVOGEL server
        ''' </summary>
        Public Sub RequestCalculationToServer()

            ' Save the model

            ProjectRoot.WriteToXML()

            ' Launch the comunication asynchronously

            If Not IsNothing(CalculationWorker) Then
                CalculationWorker.Dispose()
            End If
            CalculationWorker = New BackgroundWorker

            AddHandler CalculationWorker.DoWork, AddressOf RequestCalculationAsychronously
            AddHandler CalculationWorker.RunWorkerCompleted, AddressOf ServerCalculationFinished
            CalculationWorker.RunWorkerAsync()

        End Sub

        ''' <summary>
        ''' Callback for when the calculation is done.
        ''' </summary>
        Private Sub ServerCalculationFinished()

            FormProgress.PushMessage("Ready")
            RaiseEvent CalculationDone()

        End Sub

        ''' <summary>
        ''' Requests a steady analisis to the server
        ''' </summary>
        Private Sub RequestCalculationAsychronously()

            ' Connect to the server squekear to publish the messages and know when the calculation is over

            Dim Receiver As New UdpClient
            Receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, True)
            Receiver.Client.Bind(New IPEndPoint(IPAddress.Any, 11001))

            ' Set up the squeaker

            Dim Squeaker As New UdpClient
            Squeaker.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, True)
            Squeaker.Connect("localhost", 11000)

            ' Request the calculation and start listening

            Select Case ProjectRoot.SimulationSettings.AnalysisType
                Case CalculationType.SteadyState
                    Squeak(Squeaker, "steady;" & FilePath)
                Case CalculationType.FreeFlight
                    Squeak(Squeaker, "free_flight;" & FilePath)
                Case CalculationType.Aeroelastic
                    Squeak(Squeaker, "aeroelastic;" & FilePath)
            End Select

            Dim Done As Boolean = False

            While Not Done

                Dim ClientAddress As New IPEndPoint(IPAddress.Any, 11001)
                Dim Message As String = Encoding.ASCII.GetString(Receiver.Receive(ClientAddress))

                Dim Commands As String() = Message.Split({";"c}, StringSplitOptions.RemoveEmptyEntries)

                If Commands.Count > 0 Then

                    Select Case Commands(0)

                        Case "done"

                            Done = True
                            If Commands.Count > 1 Then
                                Dim DirectoryPath As String = Commands(1)
                                If IO.Directory.Exists(DirectoryPath) Then
                                    ProjectRoot.Results.LoadFromDirectory(DirectoryPath)
                                End If
                            End If

                    End Select

                End If

            End While

            Squeaker.Close()
            Receiver.Close()

        End Sub

        Private Sub Squeak(Squeaker As UdpClient, Message As String)
            Dim Bytes As Byte() = Text.Encoding.ASCII.GetBytes(Message)
            Squeaker.Send(Bytes, Bytes.Count)
        End Sub

    End Module

End Namespace

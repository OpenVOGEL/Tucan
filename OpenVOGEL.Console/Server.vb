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

Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports OpenVOGEL.AeroTools.CalculationModel
Imports OpenVOGEL.DesignTools

Public Module Server

    ''' <summary>
    ''' Sends back a message to the client
    ''' </summary>
    ''' <param name="Message"></param>
    ''' <param name="Address"></param>
    Private Sub Squeak(Squeaker As UdpClient, Message As String)
        Dim Bytes As Byte() = Text.Encoding.ASCII.GetBytes(Message)
        Squeaker.Send(Bytes, Bytes.Count)
    End Sub

    Public Sub RunServer()

        Dim Receiver As New UdpClient
        Receiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, True)
        Receiver.Client.Bind(New IPEndPoint(IPAddress.Any, 11000))

        System.Console.WriteLine("** OpenVOGEL server **")
        System.Console.WriteLine("Server version: 1.0")
        System.Console.WriteLine("Solver  version: " & AeroTools.CalculationModel.Solver.Solver.Version)
        System.Console.WriteLine("Listening to udp on port 11000")

        Dim Quit As Boolean = False

        While Not Quit

            Dim ClientAddress As New IPEndPoint(IPAddress.Any, 11001)
            Dim Message As String = Encoding.ASCII.GetString(Receiver.Receive(ClientAddress))
            System.Console.WriteLine(ClientAddress.ToString & ":" & Message)

            Dim Commands As String() = Message.Split({";"c}, StringSplitOptions.RemoveEmptyEntries)

            If Commands.Count > 0 Then

                Select Case Commands(0)

                    ' Stop the service
                    Case "quit"

                        Quit = True

                    ' Run steady analysis
                    Case "steady"

                        If Commands.Count > 1 Then
                            System.Console.WriteLine("Steady state requested")
                            DataStore.FilePath = Commands(1)
                            RunCalculation(Settings.CalculationType.SteadyState)
                        End If

                    ' Run free flight simulation
                    Case "free_flight"

                        If Commands.Count > 1 Then
                            System.Console.WriteLine("Free flight simulation requested")
                            DataStore.FilePath = Commands(1)
                            RunCalculation(Settings.CalculationType.FreeFlight)
                        End If

                    ' Run free flight simulation
                    Case "aeroelastic"

                        If Commands.Count > 1 Then
                            System.Console.WriteLine("Aeroelastic simulation requested")
                            DataStore.FilePath = Commands(1)
                            RunCalculation(Settings.CalculationType.Aeroelastic)
                        End If

                End Select

            End If

        End While

        Receiver.Close()

    End Sub

    Private Sub RunCalculation(Type As Settings.CalculationType)

        Dim Squeaker As New UdpClient
        Dim CalculationCore As New Solver.Solver

        Try

            Squeaker.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, True)
            Squeaker.Connect("localhost", 11001)

            ' Perform calculation

            DataStore.ProjectRoot.RestartProject()
            DataStore.ProjectRoot.ReadFromXML()
            DataStore.StartCalculation(Type, CalculationCore)

        Finally

            Squeak(Squeaker, "done;" & CalculationCore.BaseDirectoryPath)
            Squeaker.Close()

        End Try

    End Sub

    Private Sub OutputConsoleMessage(Message As String)
        System.Console.WriteLine(Message)
    End Sub

    Private Sub OutputConsoleProgress(Title As String, Value As Integer)
        System.Console.WriteLine(Title)
    End Sub

End Module

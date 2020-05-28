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

Imports System.IO
Imports OpenVOGEL.DesignTools

Module Commands

    Sub Main(Arguments As String())

        System.Console.WriteLine("** OpenVOGEL **")
        System.Console.WriteLine("Console version: 2.2")
        System.Console.WriteLine("Solver  version: " & AeroTools.CalculationModel.Solver.Solver.Version)

        MklSetup.Initialize()

        DataStore.ProjectRoot.Initialize()

        AddHandler DataStore.PushMessage, AddressOf OutputConsoleMessage
        AddHandler DataStore.PushProgress, AddressOf OutputConsoleProgress

        Dim Quit As Boolean = False

        While Not Quit

            Dim Line As String = System.Console.ReadLine()

            Dim Commands As String() = Line.Split({";"c}, StringSplitOptions.RemoveEmptyEntries)

            If Commands.Length > 0 Then

                Select Case Commands(0).ToLower

                    Case "quit"

                        Quit = True

                    Case "test"

                        TestAerodynamicSolver()

                    Case "mkl_on"

                        DotNumerics.LinearAlgebra.LinearEquations.UseIntelMathKernel = True

                    Case "mkl_off"

                        DotNumerics.LinearAlgebra.LinearEquations.UseIntelMathKernel = False

                    Case "mkl_test"

                        DotNumerics.LinearAlgebra.IntelMathKernelTest.Start()

                    Case "mkl_path"

                        If Commands.Length > 1 Then
                            MklSetup.ChangePath(Commands(1))
                        End If

                    Case "mkl_path"

                        DotNumerics.LinearAlgebra.IntelMathKernelTest.Start()

                    Case "mkl_status"

                        If DotNumerics.LinearAlgebra.LinearEquations.UseIntelMathKernel Then
                            System.Console.WriteLine("MKL is on")
                        Else
                            System.Console.WriteLine("MKL is off")
                        End If

                    Case "load"

                        System.Console.WriteLine("enter file name:")
                        DataStore.FilePath = System.Console.ReadLine
                        DataStore.ProjectRoot.RestartProject()
                        DataStore.ProjectRoot.ReadFromXML()
                        System.Console.WriteLine(String.Format("loaded {0} objects", DataStore.ProjectRoot.Model.Objects.Count))

                        Dim I As Integer = 0
                        For Each Surface In DataStore.ProjectRoot.Model.Objects
                            I += 1
                            System.Console.WriteLine(String.Format(" {0} -> {1,-20} N:{2,-5} P:{3,-5} S:{4,-5} [{5}]",
                                                                   I,
                                                                   Surface.Name,
                                                                   Surface.NumberOfNodes,
                                                                   Surface.NumberOfPanels,
                                                                   Surface.NumberOfSegments,
                                                                   Surface.GetType))
                        Next

                    Case "steady"

                        DataStore.StartCalculation(AeroTools.CalculationModel.Settings.CalculationType.ctSteady)

                    Case "alfa_scan"

                        If Commands.Length > 3 Then

                            BatchAnalysis.AlfaScan(CDbl(Commands(1)), CDbl(Commands(2)), CDbl(Commands(3)))

                        End If

                    Case "delta_scan"

                        If Commands.Length > 5 Then

                            BatchAnalysis.DeltaScan(Commands(1), CInt(Commands(2)), CDbl(Commands(3)), CDbl(Commands(4)), CDbl(Commands(5)), CDbl(Commands(6)))

                        End If

                    Case "print_report"

                        If DataStore.ProjectRoot.CalculationCore IsNot Nothing Then

                            DataStore.ProjectRoot.CalculationCore.ReportResults()

                        End If

                    Case "save_report"

                        If DataStore.ProjectRoot.CalculationCore IsNot Nothing Then

                            System.Console.WriteLine("enter destination file:")
                            Dim FileName As String = System.Console.ReadLine
                            Dim Append As Boolean = True
                            If File.Exists(FileName) Then
                                System.Console.WriteLine("the file already exists, append?")
                                If System.Console.ReadLine = "n" Then
                                    Append = False
                                End If
                            End If
                            System.Console.WriteLine("Writing results to file")
                            OutputFile = My.Computer.FileSystem.OpenTextFileWriter(FileName, Append)
                            OutputFile.WriteLine("----------------------------------------------")
                            OutputFile.WriteLine("|            OpenVOGEL results               |")
                            OutputFile.WriteLine("----------------------------------------------")
                            AddHandler DataStore.ProjectRoot.CalculationCore.PushResultLine, AddressOf WriteToFile
                            DataStore.ProjectRoot.CalculationCore.ReportResults()
                            System.Console.WriteLine("Done")

                        End If

                    Case "server"

                        Server.RunServer()

                    Case "help"

                        System.Console.WriteLine("visit www.openvogel.org for info about this console")

                    Case Else

                        System.Console.WriteLine("Unrecognized command")

                End Select

            End If

        End While

    End Sub

    Private OutputFile As StreamWriter

    Private Sub WriteToFile(Line As String)
        If OutputFile IsNot Nothing Then
            OutputFile.WriteLine(Line)
        End If
    End Sub

    Private Sub OutputConsoleMessage(Message As String)
        System.Console.WriteLine(Message)
    End Sub

    Private Sub OutputConsoleProgress(Title As String, Value As Integer)
        System.Console.WriteLine(Title)
    End Sub

End Module

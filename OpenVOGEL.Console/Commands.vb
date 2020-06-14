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
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics

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

        ' Process the user commands interactively
        '-----------------------------------------------------

        While ProcessCommand(System.Console.ReadLine(), True)

        End While

    End Sub

    ''' <summary>
    ''' Interpretes and excecutes the given command
    ''' </summary>
    ''' <param name="Command">The command</param>
    ''' <param name="Interactive">Indicates if the console might ask for data</param>
    ''' <returns></returns>
    Private Function ProcessCommand(Command As String, Interactive As Boolean) As Boolean

        Dim Commands As String() = Command.Split({";"c}, StringSplitOptions.RemoveEmptyEntries)

        If Commands.Length > 0 Then

            Select Case Commands(0).ToLower.Trim

                Case "quit"

                    Return False

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

                    If Not Interactive And Commands.Length > 1 Then
                        System.Console.WriteLine("loading file...")
                        DataStore.FilePath = Commands(1)
                    Else
                        System.Console.WriteLine("enter file name:")
                        DataStore.FilePath = System.Console.ReadLine
                    End If

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

                        BatchAnalysis.DeltaScan(CDbl(Commands(1)), Commands(2), CInt(Commands(3)), CDbl(Commands(4)), CDbl(Commands(5)), CDbl(Commands(6)))

                    End If

                Case "alfa_delta_scan"

                    If Commands.Length > 8 Then

                        BatchAnalysis.AlfaDeltaScan(CDbl(Commands(1)),
                                                        CDbl(Commands(2)),
                                                        CDbl(Commands(3)),
                                                        Commands(4),
                                                        CInt(Commands(5)),
                                                        CDbl(Commands(6)),
                                                        CDbl(Commands(7)),
                                                        CDbl(Commands(8)))

                    End If

                Case "omega_scan"

                    If Commands.Length > 5 Then

                        BatchAnalysis.OmegaScan(CDbl(Commands(1)),
                                                    CInt(Commands(2)),
                                                    CDbl(Commands(3)),
                                                    CDbl(Commands(4)),
                                                    CInt(Commands(5)))

                    End If

                Case "set_velocity"

                    If Commands.Length > 1 Then
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.X = CDbl(Commands(1))
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.Y = 0
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.Z = 0
                    End If

                    If Commands.Length > 2 Then
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.Y = CDbl(Commands(2))
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.Z = 0
                    End If

                    If Commands.Length > 3 Then
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.Z = CDbl(Commands(3))
                    End If

                Case "set_omega"

                    If Commands.Length > 1 Then
                        DataStore.ProjectRoot.SimulationSettings.Omega.X = CDbl(Commands(1))
                        DataStore.ProjectRoot.SimulationSettings.Omega.Y = 0
                        DataStore.ProjectRoot.SimulationSettings.Omega.Z = 0
                    End If

                    If Commands.Length > 2 Then
                        DataStore.ProjectRoot.SimulationSettings.Omega.Y = CDbl(Commands(2))
                        DataStore.ProjectRoot.SimulationSettings.Omega.Z = 0
                    End If

                    If Commands.Length > 3 Then
                        DataStore.ProjectRoot.SimulationSettings.Omega.Z = CDbl(Commands(3))
                    End If

                Case "set_alfa"

                    If Commands.Length > 1 Then
                        Dim Alfa As Double = CDbl(Commands(1)) / 180 * Math.PI
                        Dim V As Double = DataStore.ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.X = V * Math.Cos(Alfa)
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.Y = 0
                        DataStore.ProjectRoot.SimulationSettings.StreamVelocity.Z = V * Math.Sin(Alfa)
                    End If

                Case "set_delta"

                    If Commands.Length > 3 Then

                        Dim SurfaceName As String = Commands(1).Trim
                        Dim RegionIndex As Integer = CInt(Commands(2))
                        Dim Delta As Double = CDbl(Commands(3)) / 180 * Math.PI

                        System.Console.WriteLine("setting flap deflection for " & SurfaceName & "...")

                        ' Find the lifting surface
                        '-----------------------------------------------------------------

                        Dim LiftingSurface As LiftingSurface = Nothing

                        For Each Surface As Surface In DataStore.ProjectRoot.Model.Objects

                            If Surface.Name.ToLower = SurfaceName.ToLower Then

                                If TypeOf (Surface) Is LiftingSurface Then
                                    LiftingSurface = Surface
                                Else
                                    System.Console.WriteLine("the target surface exist in the model, but it is not a lifting surface")
                                    Exit For
                                End If

                            End If

                        Next

                        If LiftingSurface Is Nothing Then
                            System.Console.WriteLine("the target surface does not exist in the model")
                        Else

                            ' Check the region and flap
                            '-----------------------------------------------------------------

                            If RegionIndex < 1 Or RegionIndex > LiftingSurface.WingRegions.Count Then
                                System.Console.WriteLine(String.Format("invalid target region (must be between 1 and {0})", LiftingSurface.WingRegions.Count))
                            Else

                                Dim Region As WingRegion = LiftingSurface.WingRegions(RegionIndex - 1)

                                If Region.Flapped Then
                                    Region.FlapDeflection = Delta
                                    LiftingSurface.GenerateMesh()
                                Else
                                    System.Console.WriteLine("invalid target region (not flapped)")
                                End If

                            End If

                        End If

                    End If

                Case "set_density"

                    If Commands.Length > 1 Then
                        DataStore.ProjectRoot.SimulationSettings.Density = CDbl(Commands(1))
                    End If

                Case "set_viscosity"

                    If Commands.Length > 1 Then
                        DataStore.ProjectRoot.SimulationSettings.Viscocity = CDbl(Commands(1))
                    End If

                Case "print_report"

                    If DataStore.ProjectRoot.CalculationCore IsNot Nothing Then

                        DataStore.ProjectRoot.CalculationCore.ReportResults()

                    End If

                Case "save_report"

                    If DataStore.ProjectRoot.CalculationCore IsNot Nothing Then

                        Dim FileName As String = ""
                        If Interactive Then
                            System.Console.WriteLine("enter destination file:")
                            FileName = System.Console.ReadLine
                        ElseIf Commands.Length > 1
                            FileName = Commands(1).Trim
                        End If

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

                Case "load_script"

                    If Commands.Length > 1 Then

                        ProcessScript(Commands(1))

                        System.Console.WriteLine("script digested")

                    End If

                Case "help"

                    System.Console.WriteLine("visit www.openvogel.org for info about this console")

                Case Else

                    System.Console.WriteLine("Unrecognized command")

            End Select

        End If

        Return True

    End Function

    Private Sub ProcessScript(ScriptPath As String)

        If File.Exists(ScriptPath) Then

            Dim FileId = FreeFile()

            FileOpen(FileId, ScriptPath, OpenMode.Input)

            System.Console.WriteLine("digesting script")

            While Not EOF(FileId)

                ProcessCommand(LineInput(FileId), False)

            End While

            FileClose(FileId)

        Else

            System.Console.WriteLine("the provided script does not exist")

        End If

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

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

Imports System.IO
Imports OpenVOGEL.DesignTools
Imports OpenVOGEL.DesignTools.DataStore

Module MainModule

    Sub Main()

        System.Console.WriteLine("** OpenVOGEL console **")

        ProjectRoot.Initialize()

        Dim Quit As Boolean = False

        While Not Quit

            Dim Line As String = System.Console.ReadLine()

            Select Case Line

                Case "quit"

                    Quit = True

                Case "test"

                    TestAerodynamicSolver()

                Case "load"

                    System.Console.WriteLine("enter file name:")
                    DataStore.FilePath = System.Console.ReadLine
                    DataStore.ProjectRoot.ReadFromXML()

                Case "steady"

                    AddHandler DataStore.PushMessage, AddressOf OutputMessage
                    AddHandler DataStore.PushProgress, AddressOf OutputProgress
                    DataStore.StartCalculation(AeroTools.CalculationModel.Settings.CalculationType.ctSteady)

                Case "print report"

                    If DataStore.ProjectRoot.CalculationCore IsNot Nothing Then

                        AddHandler DataStore.ProjectRoot.CalculationCore.PushResultLine, AddressOf OutputMessage
                        DataStore.ProjectRoot.CalculationCore.ReportResults()

                    End If

                Case "save report"

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

            End Select

        End While


    End Sub

    Private Sub OutputMessage(Message As String)
        System.Console.WriteLine(Message)
    End Sub

    Private Sub OutputProgress(Title As String, Value As Integer)
        System.Console.WriteLine(Title)
    End Sub

    Private OutputFile As StreamWriter

    Private Sub WriteToFile(Line As String)
        If OutputFile IsNot Nothing Then
            OutputFile.WriteLine(Line)
        End If
    End Sub

End Module

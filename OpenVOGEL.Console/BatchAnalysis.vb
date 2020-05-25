Imports System.IO
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.DataStore

Module BatchAnalysis

    Public Sub AlfaScan(Alfa1 As Double, Alfa2 As Double, Delta As Double)

        If Alfa2 < Alfa1 Then
            System.Console.WriteLine("the first angle must be smaller than the second one")
            Exit Sub
        End If

        Dim FileId As Integer = FreeFile()

        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL batch analysis: alfa scan")
        PrintLine(FileId, "Alfa | CL | CDi | CDp | CMx | CMy | CMz")

        Dim N As Integer = (Alfa2 - Alfa1) / Delta

        Dim V As Double = ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm
        Dim Vy As Double = ProjectRoot.SimulationSettings.StreamVelocity.Y

        For I = 0 To N

            System.Console.WriteLine(String.Format("STEP {0} of {1}", I, N))

            Dim Alfa = Math.PI * (Alfa1 + I * Delta) / 180.0

            ProjectRoot.SimulationSettings.StreamVelocity.X = V * Math.Cos(Alfa)
            ProjectRoot.SimulationSettings.StreamVelocity.Y = Vy
            ProjectRoot.SimulationSettings.StreamVelocity.Z = V * Math.Sin(Alfa)

            ProjectRoot.StartCalculation(CalculationType.ctSteady)

            PrintLine(FileId, String.Format("{0:F8} | {1:F8} | {2:F8}", CalculationCore.GlobalAirloads.LiftCoefficient, CalculationCore.GlobalAirloads.InducedDragCoefficient, CalculationCore.GlobalAirloads.SkinDragCoefficient))

            If I = N Then

                PrintLine(FileId, String.Format("Area = {0:F8}", CalculationCore.GlobalAirloads.Area))

            End If

        Next

        FileClose(FileId)

    End Sub

End Module

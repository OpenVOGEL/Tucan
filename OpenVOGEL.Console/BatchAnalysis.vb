Imports System.IO
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.DataStore

Module BatchAnalysis

    Public Sub AlfaScan(Alfa1 As Double, Alfa2 As Double, Delta As Double)

        If Alfa2 < Alfa1 Then
            System.Console.WriteLine("the first angle must be smaller than the second one")
            Exit Sub
        End If

        Dim N As Integer = (Alfa2 - Alfa1) / Delta
        Dim Loads As New List(Of AirLoads)

        Dim V As Double = ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm
        Dim Vy As Double = ProjectRoot.SimulationSettings.StreamVelocity.Y

        For I = 0 To N

            System.Console.WriteLine(String.Format("STEP {0} of {1}", I, N))

            Dim Alfa = Math.PI * (Alfa1 + I * Delta) / 180.0

            ProjectRoot.SimulationSettings.StreamVelocity.X = V * Math.Cos(Alfa)
            ProjectRoot.SimulationSettings.StreamVelocity.Y = Vy
            ProjectRoot.SimulationSettings.StreamVelocity.Z = V * Math.Sin(Alfa)

            ProjectRoot.StartCalculation(CalculationType.ctSteady)

            Loads.Add(CalculationCore.GlobalAirloads)

        Next

        Dim FileId As Integer = FreeFile()

        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL alfa scan")
        PrintLine(FileId, "")

        PrintLine(FileId, String.Format("A = {0,12:E6}m²", CalculationCore.GlobalAirloads.Area))
        PrintLine(FileId, String.Format("L = {0,12:E6}m", CalculationCore.GlobalAirloads.Area))
        PrintLine(FileId, String.Format("q = {0,12:E6}kg/m³", CalculationCore.GlobalAirloads.DynamicPressure))

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force coefficients")
        PrintLine(FileId, String.Format("{0,-6} {1,-14} {2,-14} {3,-14}", "Alfa", "CL", "CDi", "CDp"))

        For Each Load In Loads

            PrintLine(FileId, String.Format("{0,6:F3} {1,14:E6} {2,14:E6} {3,14:E6}",
                                            Load.Alfa * 180.0 / Math.PI,
                                            Load.LiftCoefficient,
                                            Load.InducedDragCoefficient,
                                            Load.SkinDragCoefficient))

        Next

        PrintLine(FileId, "")
        PrintLine(FileId, "# Forces and moments (in [N] and  [Nm])")
        PrintLine(FileId, String.Format("{0,-6} {1,-14} {2,-14} {3,-14} {4,-14} {5,-14} {6,-14}", "Alfa", "Fx", "Fy", "Fz", "Mx", "My", "Mz"))

        For Each Load In Loads

            PrintLine(FileId, String.Format("{0,6:F3} {1,14:E6} {2,14:E6} {3,14:E6} {4,14:E6} {5,14:E6} {6,14:E6}",
                                            Load.Alfa * 180.0 / Math.PI,
                                            Load.Force.X,
                                            Load.Force.Y,
                                            Load.Force.Z,
                                            Load.Moment.X,
                                            Load.Moment.Y,
                                            Load.Moment.Z))
        Next

        FileClose(FileId)

    End Sub

End Module

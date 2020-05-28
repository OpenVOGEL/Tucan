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
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics

Module BatchAnalysis

    ''' <summary>
    ''' Performs a series of steady analysis between Alfa1 and Alfa2 using the AlfaStep in between.
    ''' </summary>
    ''' <param name="Alfa1">The initial incidence angle.</param>
    ''' <param name="Alfa2">The final incidence angle.</param>
    ''' <param name="AlfaStep">The step.</param>
    Public Sub AlfaScan(Alfa1 As Double, Alfa2 As Double, AlfaStep As Double)

        If Alfa2 < Alfa1 Then
            System.Console.WriteLine("the first angle must be smaller than the second one")
            Exit Sub
        End If

        Dim N As Integer = (Alfa2 - Alfa1) / AlfaStep
        Dim Loads As New List(Of AirLoads)

        Dim V As Double = ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm
        Dim Vy As Double = ProjectRoot.SimulationSettings.StreamVelocity.Y

        For I = 0 To N

            System.Console.WriteLine(String.Format("STEP {0} of {1}", I, N))

            Dim Alfa = Math.Min(Math.PI * (Alfa1 + I * AlfaStep) / 180.0, Alfa2)

            ProjectRoot.SimulationSettings.StreamVelocity.X = V * Math.Cos(Alfa)
            ProjectRoot.SimulationSettings.StreamVelocity.Y = Vy
            ProjectRoot.SimulationSettings.StreamVelocity.Z = V * Math.Sin(Alfa)

            ProjectRoot.StartCalculation(CalculationType.ctSteady)

            Loads.Add(CalculationCore.GlobalAirloads)

        Next

        Dim FileId As Integer = FreeFile()

        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL alfa scan")
        PrintLine(FileId, "Kernel version: " & CalculationCore.Version)
        PrintLine(FileId, "Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        PrintLine(FileId, String.Format("L = {0,12:E6}m", CalculationCore.GlobalAirloads.Area))
        PrintLine(FileId, String.Format("A = {0,12:E6}m²", CalculationCore.GlobalAirloads.Area))
        PrintLine(FileId, String.Format("q = {0,12:E6}Pa", CalculationCore.GlobalAirloads.DynamicPressure))

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
        PrintLine(FileId, "# Force and moment coefficients")
        PrintLine(FileId, String.Format("{0,-6} {1,-14} {2,-14} {3,-14} {4,-14} {5,-14} {6,-14}", "Alfa", "Fx", "Fy", "Fz", "Mx", "My", "Mz"))

        For Each Load In Loads

            Dim qS As Double = Load.DynamicPressure * Load.Area
            Dim qSL As Double = Load.DynamicPressure * Load.Area * Load.Length

            PrintLine(FileId, String.Format("{0,6:F3} {1,14:E6} {2,14:E6} {3,14:E6} {4,14:E6} {5,14:E6} {6,14:E6}",
                                            Load.Alfa * 180.0 / Math.PI,
                                            Load.Force.X / qS,
                                            Load.Force.Y / qS,
                                            Load.Force.Z / qS,
                                            Load.Moment.X / qSL,
                                            Load.Moment.Y / qSL,
                                            Load.Moment.Z / qSL))
        Next

        FileClose(FileId)

    End Sub

    ''' <summary>
    ''' Scans the airloads for a given set of flap deflections.
    ''' </summary>
    ''' <param name="Surface">The target surface name.</param>
    ''' <param name="Region">The region containing the controlled flap (1-based).</param>
    ''' <param name="Alfa">The incidence angle.</param>
    ''' <param name="Delta1">The initial flap deflection.</param>
    ''' <param name="Delta2">The final flap deflection.</param>
    ''' <param name="DeltaStep">The deflection step.</param>
    Public Sub DeltaScan(SurfaceName As String, RegionIndex As Integer, Alfa As Double, Delta1 As Double, Delta2 As Double, DeltaStep As Double)

        ' Find the lifting surface
        '-----------------------------------------------------------------

        Dim LiftingSurface As LiftingSurface = Nothing

        For Each Surface As Surface In Model.Objects

            If Surface.Name.ToLower = SurfaceName.ToLower Then

                If TypeOf (Surface) Is LiftingSurface Then

                    LiftingSurface = Surface

                Else
                    System.Console.WriteLine("the target surface exist in the model, but it is not a lifting surface")
                    Exit Sub

                End If

            End If

        Next

        If LiftingSurface Is Nothing Then
            System.Console.WriteLine("the target surface does not exist in the model")
            Exit Sub
        End If

        ' Check the region and flap
        '-----------------------------------------------------------------

        If RegionIndex < 1 Or RegionIndex > LiftingSurface.WingRegions.Count Then
            System.Console.WriteLine(String.Format("invalid target region (must be between 1 and {0})", LiftingSurface.WingRegions.Count))
            Exit Sub
        End If

        Dim Region As WingRegion = LiftingSurface.WingRegions(RegionIndex - 1)

        If Not Region.Flapped Then
            System.Console.WriteLine("invalid target region (not flapped)")
            Exit Sub
        End If

        Dim OriginalDeflection As Double = Region.FlapDeflection

        ' Check the given angles
        '-----------------------------------------------------------------

        If Delta2 < Delta1 Then
            System.Console.WriteLine("the first angle must be smaller than the second one")
            Exit Sub
        End If

        Dim N As Integer = (Delta2 - Delta1) / DeltaStep
        Dim Loads As New List(Of AirLoads)

        Dim V As Double = ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm

        ' Set the incidence angle
        '-----------------------------------------------------------------

        ProjectRoot.SimulationSettings.StreamVelocity.X = V * Math.Cos(Alfa / 180.0 * Math.PI)
        ProjectRoot.SimulationSettings.StreamVelocity.Z = V * Math.Sin(Alfa / 180.0 * Math.PI)

        ' Scan the flap deflection
        '-----------------------------------------------------------------

        For I = 0 To N

            System.Console.WriteLine(String.Format("STEP {0} of {1}", I, N))

            Region.FlapDeflection = Math.Min(Math.PI * (Delta1 + I * DeltaStep) / 180.0, Delta2)

            LiftingSurface.GenerateMesh()

            ProjectRoot.StartCalculation(CalculationType.ctSteady)

            Loads.Add(CalculationCore.GlobalAirloads)

        Next

        Region.FlapDeflection = OriginalDeflection

        ' Write results
        '-----------------------------------------------------------------

        Dim FileId As Integer = FreeFile()

        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL delta scan")
        PrintLine(FileId, "Kernel version: " & CalculationCore.Version)
        PrintLine(FileId, "Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        PrintLine(FileId, String.Format("L = {0,12:E6}m", CalculationCore.GlobalAirloads.Area))
        PrintLine(FileId, String.Format("A = {0,12:E6}m²", CalculationCore.GlobalAirloads.Area))
        PrintLine(FileId, String.Format("q = {0,12:E6}Pa", CalculationCore.GlobalAirloads.DynamicPressure))
        PrintLine(FileId, String.Format("a = {0,12:E6}°", CalculationCore.GlobalAirloads.Alfa))

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force coefficients")
        PrintLine(FileId, String.Format("{0,-6} {1,-14} {2,-14} {3,-14}", "Delta", "CL", "CDi", "CDp"))

        Dim J = 0

        For Each Load In Loads

            Dim Delta = Math.Min((Delta1 + J * DeltaStep), Delta2)

            PrintLine(FileId, String.Format("{0,6:F3} {1,14:E6} {2,14:E6} {3,14:E6}",
                                            Delta,
                                            Load.LiftCoefficient,
                                            Load.InducedDragCoefficient,
                                            Load.SkinDragCoefficient))

            J += 1
        Next

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force and moment coefficients")
        PrintLine(FileId, String.Format("{0,-6} {1,-14} {2,-14} {3,-14} {4,-14} {5,-14} {6,-14}", "Delta", "Fx", "Fy", "Fz", "Mx", "My", "Mz"))

        J = 0

        For Each Load In Loads

            Dim Delta = Math.Min((Delta1 + J * DeltaStep), Delta2)

            Dim qS As Double = Load.DynamicPressure * Load.Area
            Dim qSL As Double = Load.DynamicPressure * Load.Area * Load.Length

            PrintLine(FileId, String.Format("{0,6:F3} {1,14:E6} {2,14:E6} {3,14:E6} {4,14:E6} {5,14:E6} {6,14:E6}",
                                            Delta,
                                            Load.Force.X / qS,
                                            Load.Force.Y / qS,
                                            Load.Force.Z / qS,
                                            Load.Moment.X / qSL,
                                            Load.Moment.Y / qSL,
                                            Load.Moment.Z / qSL))

            J += 1
        Next

        FileClose(FileId)

    End Sub

End Module

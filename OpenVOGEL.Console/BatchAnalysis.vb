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
Imports OpenVOGEL.AeroTools.CalculationModel
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
    ''' <param name="AlfaS">The step.</param>
    Public Sub AlfaScan(Alfa1 As Double,
                        Alfa2 As Double,
                        AlfaS As Double)

        If Alfa2 < Alfa1 Then
            System.Console.WriteLine("the first angle must be smaller than the second one")
            Exit Sub
        End If

        Dim N As Integer = (Alfa2 - Alfa1) / AlfaS
        Dim Loads As New List(Of AirLoads)

        Dim V As Double = ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm

        For I = 0 To N

            System.Console.WriteLine(String.Format("STEP {0} of {1}", I, N))

            Dim Alfa = Math.PI * Math.Min(Alfa1 + I * AlfaS, Alfa2) / 180.0

            ProjectRoot.SimulationSettings.StreamVelocity.X = V * Math.Cos(Alfa)
            ProjectRoot.SimulationSettings.StreamVelocity.Z = V * Math.Sin(Alfa)

            Dim Kernel As New Solver.Solver

            ProjectRoot.StartCalculation(CalculationType.SteadyState, Kernel)

            Loads.Add(Kernel.GlobalAirloads)

        Next

        Dim FileId As Integer = FreeFile()

        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL alfa scan")
        PrintLine(FileId, "Kernel version: " & Solver.Solver.Version)
        PrintLine(FileId, "Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        PrintLine(FileId, String.Format("L = {0,12:E6}m", Loads(0).Length))
        PrintLine(FileId, String.Format("A = {0,12:E6}m²", Loads(0).Area))
        PrintLine(FileId, String.Format("q = {0,12:E6}Pa", Loads(0).DynamicPressure))

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
    ''' <param name="DeltaS">The deflection step.</param>
    Public Sub DeltaScan(Alfa As Double,
                         SurfaceName As String,
                         RegionIndex As Integer,
                         Delta1 As Double,
                         Delta2 As Double,
                         DeltaS As Double)

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

        Dim N As Integer = (Delta2 - Delta1) / DeltaS
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

            Region.FlapDeflection = Math.PI * Math.Min(Delta1 + I * DeltaS, Delta2) / 180.0

            LiftingSurface.GenerateMesh()

            Dim Kernel As New Solver.Solver

            ProjectRoot.StartCalculation(CalculationType.SteadyState, Kernel)

            Loads.Add(Kernel.GlobalAirloads)

        Next

        Region.FlapDeflection = OriginalDeflection

        ' Write results
        '-----------------------------------------------------------------

        Dim FileId As Integer = FreeFile()

        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL delta scan")
        PrintLine(FileId, "Kernel version: " & Solver.Solver.Version)
        PrintLine(FileId, "Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        PrintLine(FileId, String.Format("L = {0,12:E6}m", Loads(0).Length))
        PrintLine(FileId, String.Format("A = {0,12:E6}m²", Loads(0).Area))
        PrintLine(FileId, String.Format("q = {0,12:E6}Pa", Loads(0).DynamicPressure))
        PrintLine(FileId, String.Format("a = {0,12:E6}°", Loads(0).Alfa))

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force coefficients")
        PrintLine(FileId, String.Format("{0,-6} {1,-14} {2,-14} {3,-14}", "Delta", "CL", "CDi", "CDp"))

        Dim J = 0

        For Each Load In Loads

            Dim Delta = Math.Min((Delta1 + J * DeltaS), Delta2)

            PrintLine(FileId, String.Format("{0,6:F2} {1,14:E6} {2,14:E6} {3,14:E6}",
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

            Dim Delta = Math.Min((Delta1 + J * DeltaS), Delta2)

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

    ''' <summary>
    ''' Scans the airloads for a given set of flap deflections.
    ''' </summary>
    ''' <param name="Surface">The target surface name.</param>
    ''' <param name="Region">The region containing the controlled flap (1-based).</param>
    ''' <param name="Alfa">The incidence angle.</param>
    ''' <param name="Delta1">The initial flap deflection.</param>
    ''' <param name="Delta2">The final flap deflection.</param>
    ''' <param name="DeltaS">The deflection step.</param>
    Public Sub AlfaDeltaScan(Alfa1 As Double,
                             Alfa2 As Double,
                             AlfaS As Double,
                             SurfaceName As String,
                             RegionIndex As Integer,
                             Delta1 As Double,
                             Delta2 As Double,
                             DeltaS As Double)

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

        If Alfa2 < Alfa1 Then
            System.Console.WriteLine("the first incidence angle must be smaller than the second one")
            Exit Sub
        End If

        If Delta2 < Delta1 Then
            System.Console.WriteLine("the first deflection angle must be smaller than the second one")
            Exit Sub
        End If

        Dim Na As Integer = (Alfa2 - Alfa1) / AlfaS
        Dim Nd As Integer = (Delta2 - Delta1) / DeltaS
        Dim Loads As New List(Of AirLoads)

        Dim V As Double = ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm

        For I = 0 To Na

            System.Console.WriteLine(String.Format("ALFA STEP {0} of {1}", I, Na))

            ' Set the incidence angle
            '-----------------------------------------------------------------

            Dim Alfa = Math.PI * Math.Min(Alfa1 + I * AlfaS, Alfa2) / 180.0

            ProjectRoot.SimulationSettings.StreamVelocity.X = V * Math.Cos(Alfa)
            ProjectRoot.SimulationSettings.StreamVelocity.Z = V * Math.Sin(Alfa)

            ' Scan the flap deflection
            '-----------------------------------------------------------------

            For J = 0 To Nd

                System.Console.WriteLine(String.Format("DELTA STEP {0} of {1}", J, Nd))

                Region.FlapDeflection = Math.PI * Math.Min(Delta1 + J * DeltaS, Delta2) / 180.0

                LiftingSurface.GenerateMesh()

                Dim Kernel As New Solver.Solver

                ProjectRoot.StartCalculation(CalculationType.SteadyState, Kernel)

                Loads.Add(Kernel.GlobalAirloads)

            Next

            Region.FlapDeflection = OriginalDeflection

        Next

        ' Write results in dat file
        '-----------------------------------------------------------------

        Dim FileId As Integer = FreeFile()
        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL alfa delta scan")
        PrintLine(FileId, "Kernel version: " & Solver.Solver.Version)
        PrintLine(FileId, "Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        PrintLine(FileId, String.Format("L = {0,12:E6}m", Loads(0).Length))
        PrintLine(FileId, String.Format("A = {0,12:E6}m²", Loads(0).Area))
        PrintLine(FileId, String.Format("q = {0,12:E6}Pa", Loads(0).DynamicPressure))
        PrintLine(FileId, String.Format("a = {0,12:E6}°", Loads(0).Alfa))

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force coefficients")
        PrintLine(FileId, String.Format("{0,-6} {1,-6} {2,-14} {3,-14} {4,-14} {5,-14}", "Alfa", "Delta", "CL", "CDi", "CDp", "Xcg"))

        Dim L = 0
        Dim K = 0

        For Each Load In Loads

            Dim Alfa = Math.Min((Alfa1 + L * AlfaS), Alfa2)

            Dim Delta = Math.Min((Delta1 + K * DeltaS), Delta2)

            Dim qSL As Double = Load.DynamicPressure * Load.Area * Load.Length

            Dim Xcg As Double = -(Load.Moment.Y / qSL) / Load.LiftCoefficient

            PrintLine(FileId, String.Format("{0,6:F2} {1,6:F2} {2,14:E6} {3,14:E6} {4,14:E6} {5,14:E6}",
                                            Alfa,
                                            Delta,
                                            Load.LiftCoefficient,
                                            Load.InducedDragCoefficient,
                                            Load.SkinDragCoefficient,
                                            Xcg))

            If K = Nd Then
                K = 0
                L += 1
            Else
                K += 1
            End If

        Next

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force and moment coefficients")
        PrintLine(FileId, String.Format("{0,-6} {1,-6} {2,-14} {3,-14} {4,-14} {5,-14} {6,-14} {7,-14}", "Alfa", "Delta", "Fx", "Fy", "Fz", "Mx", "My", "Mz"))

        L = 0
        K = 0

        For Each Load In Loads

            Dim Alfa = Math.Min((Alfa1 + L * AlfaS), Alfa2)

            Dim Delta = Math.Min((Delta1 + K * DeltaS), Delta2)

            Dim qS As Double = Load.DynamicPressure * Load.Area
            Dim qSL As Double = Load.DynamicPressure * Load.Area * Load.Length

            PrintLine(FileId, String.Format("{0,6:F2} {1,6:F2} {2,14:E6} {3,14:E6} {4,14:E6} {5,14:E6} {6,14:E6} {7,14:E6}",
                                            Alfa,
                                            Delta,
                                            Load.Force.X / qS,
                                            Load.Force.Y / qS,
                                            Load.Force.Z / qS,
                                            Load.Moment.X / qSL,
                                            Load.Moment.Y / qSL,
                                            Load.Moment.Z / qSL))

            If K = Nd Then
                K = 0
                L += 1
            Else
                K += 1
            End If

        Next

        FileClose(FileId)

        ' Write results in Scilab script file to plot the equilibrium states
        '-------------------------------------------------------------------

        FileId = FreeFile()
        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_script.sce", OpenMode.Output)
        PrintLine(FileId, "// OpenVOGEL automatic script for alfa-delta scan")
        PrintLine(FileId, "// Kernel version: " & Solver.Solver.Version)
        PrintLine(FileId, "// Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        Dim Line As String = ""

        ' Alfa and delta vectors
        '----------------------------------------------------------------

        For I = 0 To Na

            Dim Alfa = Math.Min((Alfa1 + I * AlfaS), Alfa2)

            Line = Line & String.Format("{0,8:F2}", Alfa)

        Next

        PrintLine(FileId, String.Format("X = [{0}]", Line))

        Line = ""

        For I = 0 To Nd

            Dim Delta = Math.Min((Delta1 + I * DeltaS), Delta2)

            Line = Line & String.Format("{0,8:F2}", Delta)

        Next

        PrintLine(FileId, String.Format("Y = [{0}]", Line))

        ' Lift coefficient
        '----------------------------------------------------------------

        PrintLine(FileId, "CL = [")

        K = 0
        Line = ""

        For Each Load In Loads

            Line = Line & String.Format(" {0,14:E6}", Load.LiftCoefficient)

            If K = Nd Then
                PrintLine(FileId, Line)
                Line = ""
                K = 0
            Else
                K += 1
            End If

        Next

        PrintLine(FileId, "]")

        ' Vertical force coefficient
        '----------------------------------------------------------------

        PrintLine(FileId, "CFz = [")

        K = 0
        Line = ""

        For Each Load In Loads

            Dim qS As Double = Load.DynamicPressure * Load.Area

            Line = Line & String.Format(" {0,14:E6}", Load.Force.Z / qS)

            If K = Nd Then
                PrintLine(FileId, Line)
                Line = ""
                K = 0
            Else
                K += 1
            End If

        Next

        PrintLine(FileId, "]")

        ' Vertical force coefficient
        '----------------------------------------------------------------

        PrintLine(FileId, "CMy = [")

        K = 0
        Line = ""

        For Each Load In Loads

            Dim qSL As Double = Load.DynamicPressure * Load.Area * Load.Length

            Line = Line & String.Format(" {0,14:E6}", Load.Moment.Y / qSL)

            If K = Nd Then
                PrintLine(FileId, Line)
                Line = ""
                K = 0
            Else
                K += 1
            End If

        Next

        PrintLine(FileId, "]")

        PrintLine(FileId, "clf")
        PrintLine(FileId, "title(""Stability plot"", ""fontsize"", 4)")
        PrintLine(FileId, "xlabel(""alpha [degrees]"", ""fontsize"", 3)")
        PrintLine(FileId, "ylabel(""delta [degrees]"", ""fontsize"", 3)")
        PrintLine(FileId, "xgrid(3)")
        PrintLine(FileId, "legends([""iso-CL"", ""iso-Xcg""], [2, 5], ""lr"")")

        ' Plot lift contourn lines
        '----------------------------------------------------------------

        PrintLine(FileId, "// CL countour lines")
        PrintLine(FileId, "N_CL = 30")
        PrintLine(FileId, "Stl_CL = 2 * ones(1, N_CL)")
        PrintLine(FileId, "contour(X, Y, CL, N_CL, Stl_CL)")

        ' Plot the contour lines for the X coordinate of the 
        ' gravity center (Xcg)
        '----------------------------------------------------------------

        PrintLine(FileId, "// Expand CL And CM To refine Xcg")

        ' Build spline interpolation for CFz and FMy

        PrintLine(FileId, "CFz_Spline = splin2d(X, Y, CFz)")
        PrintLine(FileId, "CMy_Spline = splin2d(X, Y, CMy)")

        ' Expand the data in a finer grid

        PrintLine(FileId, String.Format("X_Int = linspace({0,6:F2}, {1,6:F2}, 100)", Alfa1, Alfa2))
        PrintLine(FileId, String.Format("Y_Int = linspace({0,6:F2}, {1,6:F2}, 100)", Delta1, Delta2))
        PrintLine(FileId, "[X_Grid,Y_Grid] = ndgrid(X_Int, Y_Int)")
        PrintLine(FileId, "CFz_Int = interp2d(X_Grid, Y_Grid, X, Y, CFz_Spline)")
        PrintLine(FileId, "CMy_Int = interp2d(X_Grid, Y_Grid, X, Y, CMy_Spline)")

        ' Compute center of gravity for the refined grid and plot the iso-curves

        PrintLine(FileId, "Xcg_Int = - CMy_Int ./ CFz_Int")

        PrintLine(FileId, "N_Xcg = 45")
        PrintLine(FileId, "Stl_Xcg = 5 * ones(1, N_Xcg)")
        PrintLine(FileId, "contour(X_Int, Y_Int, Xcg_Int, 45, Stl_Xcg)")

        FileClose(FileId)

    End Sub

    ''' <summary>
    ''' Performs a series of steady analysis between Omega1 and Omega2 using the OmegaS in between.
    ''' </summary>
    ''' <param name="Alfa1">The initial angular velocity.</param>
    ''' <param name="Alfa2">The final angular velocity.</param>
    ''' <param name="AlfaS">The step.</param>
    Public Sub OmegaScan(OmegaMax As Double, No As Integer, Mass1 As Double, Mass2 As Double, Nm As Integer)

        Dim Loads As New List(Of AirLoads)

        ProjectRoot.SimulationSettings.ExtendWakes = False

        For I = 0 To No

            System.Console.WriteLine(String.Format("STEP {0} of {1}", I, No))

            ProjectRoot.SimulationSettings.StreamRotation.Y = -OmegaMax * I / No

            Dim Kernel As New Solver.Solver

            ProjectRoot.StartCalculation(CalculationType.SteadyState, Kernel)

            Loads.Add(Kernel.GlobalAirloads)

        Next

        Dim FileId As Integer = FreeFile()

        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_batch.dat", OpenMode.Output)

        PrintLine(FileId, "OpenVOGEL omega scan")
        PrintLine(FileId, "Kernel version: " & Solver.Solver.Version)
        PrintLine(FileId, "Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        PrintLine(FileId, String.Format("L = {0,12:E6}m", Loads(0).Length))
        PrintLine(FileId, String.Format("A = {0,12:E6}m²", Loads(0).Area))
        PrintLine(FileId, String.Format("q = {0,12:E6}Pa", Loads(0).DynamicPressure))
        PrintLine(FileId, String.Format("a = {0,12:E6}deg", Loads(0).Alfa * 180 / Math.PI))

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force coefficients")
        PrintLine(FileId, String.Format("{0,-8} {1,-14} {2,-14} {3,-14}", "Omega", "CL", "CDi", "CDp"))

        Dim J As Integer = 0

        For Each Load In Loads

            Dim Omega = -OmegaMax * J / No

            PrintLine(FileId, String.Format("{0,8:F4} {1,14:E6} {2,14:E6} {3,14:E6}",
                                            Omega,
                                            Load.LiftCoefficient,
                                            Load.InducedDragCoefficient,
                                            Load.SkinDragCoefficient))

            J += 1

        Next

        PrintLine(FileId, "")
        PrintLine(FileId, "# Force and moment coefficients")
        PrintLine(FileId, String.Format("{0,-8} {1,-14} {2,-14} {3,-14} {4,-14} {5,-14} {6,-14}", "Alfa", "Fx", "Fy", "Fz", "Mx", "My", "Mz"))

        J = 0

        For Each Load In Loads

            Dim Omega = -OmegaMax * J / No

            Dim qS As Double = Load.DynamicPressure * Load.Area
            Dim qSL As Double = Load.DynamicPressure * Load.Area * Load.Length

            PrintLine(FileId, String.Format("{0,8:F4} {1,14:E6} {2,14:E6} {3,14:E6} {4,14:E6} {5,14:E6} {6,14:E6}",
                                            Omega,
                                            Load.Force.X / qS,
                                            Load.Force.Y / qS,
                                            Load.Force.Z / qS,
                                            Load.Moment.X / qSL,
                                            Load.Moment.Y / qSL,
                                            Load.Moment.Z / qSL))

            J += 1

        Next

        FileClose(FileId)

        ' Write results in Scilab script file to plot the equilibrium states
        '-------------------------------------------------------------------

        FileId = FreeFile()
        FileOpen(FileId, Path.Combine(Path.GetDirectoryName(FilePath), Path.GetFileNameWithoutExtension(FilePath)) & "_script.sce", OpenMode.Output)
        PrintLine(FileId, "// OpenVOGEL automatic script for omega scan")
        PrintLine(FileId, "// Kernel version: " & Solver.Solver.Version)
        PrintLine(FileId, "// Original model: " & ProjectRoot.FilePath)
        PrintLine(FileId, "")

        Dim Velocity As Double = ProjectRoot.SimulationSettings.StreamVelocity.EuclideanNorm
        Dim Density As Double = ProjectRoot.SimulationSettings.Density
        Dim Area As Double = Loads(0).Area

        ' M vector (mass, limited to Mcrit)
        '----------------------------------------------------------------

        J = 0

        For Each Load In Loads

            Dim Kappa = (OmegaMax * J / No) / Velocity
            Dim Mcrit = Mass2

            If Kappa > 0 Then
                Mcrit = 0.5 * Load.Area * Density * Load.LiftCoefficient / Kappa
                If Mass2 > Mcrit Then
                    PrintLine(FileId, String.Format("// WARNING: the upper mass limit is constrained to {0,14:E6}kg for a curvature of {1,14:E6}!", Mcrit, Kappa))
                    Mass2 = 0.9 * Mcrit
                    PrintLine(FileId, String.Format("// The upper mass limit has been reduced to {0,14:E6}kg", Mass2))
                    If Mass1 > Mass2 Then
                        Mass1 = 0.5 * Mass2
                        PrintLine(FileId, String.Format("// The lower mass limit has also been reduced to {0,14:E6}kg", Mass1))
                    End If
                    PrintLine(FileId, "// Either reduce the mass or the maximum path curvature")
                End If
            End If

            J += 1

        Next

        PrintLine(FileId, String.Format("M = linspace({0,14:E6}, {1,14:E6}, {2})", Mass1, Mass2, Nm))

        ' C vector (curvature of the trajectory)
        '----------------------------------------------------------------

        Dim Line As String = ""

        For I = 0 To No

            Dim Omega = OmegaMax * I / No

            Line = Line & String.Format("{0,14:E6}", Omega / Velocity)

        Next

        PrintLine(FileId, String.Format("C  = [{0}]", Line))

        ' X vector (Xcg)
        '----------------------------------------------------------------

        Line = ""

        For Each Load In Loads

            Line = Line & String.Format(" {0,14:E6}", -Load.Moment.Y / (Load.Force.Z * Load.Length))

        Next

        PrintLine(FileId, String.Format("X  = [{0}]", Line))

        ' CL vector
        '----------------------------------------------------------------

        Line = ""

        For Each Load In Loads

            Line = Line & String.Format(" {0,14:E6}", Load.LiftCoefficient)

        Next

        PrintLine(FileId, String.Format("CL = [{0}]", Line))

        ' Other data
        '----------------------------------------------------------------

        PrintLine(FileId, String.Format("r = {0,14:E6}", Density))
        PrintLine(FileId, String.Format("S = {0,14:E6}", Area))
        PrintLine(FileId, "g = 9.8")

        ' Compute velocity and load factor for each C and M
        '----------------------------------------------------------------

        PrintLine(FileId, "V = zeros(length(M), length(C))")
        PrintLine(FileId, "n = zeros(length(M), length(C))")
        PrintLine(FileId, "for I = 1: length(M)")
        PrintLine(FileId, "    for J = 1: length(C)")
        PrintLine(FileId, "        V(I, J) = sqrt(2 * M(I) * 9.8 / (r * S * CL(J) - 2 * C(J) * M(I)))")
        PrintLine(FileId, "        n(I, J) = 1 + C(J) * V(I, J) ^ 2 / 9.8")
        PrintLine(FileId, "    end")
        PrintLine(FileId, "end")
        PrintLine(FileId, "scf(2)")
        PrintLine(FileId, "clf")

        ' Make the diagram
        '----------------------------------------------------------------

        PrintLine(FileId, String.Format("title(""Recovery performance (alpha={0:N0}°)"", ""fontsize"", 4)", Loads(0).Alfa * 180 / Math.PI))
        PrintLine(FileId, "xlabel(""mass [kg]"", ""fontsize"", 3)")
        PrintLine(FileId, "ylabel(""Xcg [x/L]"", ""fontsize"", 3)")
        PrintLine(FileId, "xgrid(3)")
        PrintLine(FileId, "legends([""iso-n"", ""iso-V""], [2, 5], ""lr"")")
        PrintLine(FileId, "Stl_n = 2 * ones(1, 10)")
        PrintLine(FileId, "Stl_V = 5 * ones(1, 10)")
        PrintLine(FileId, "contour2d(M, X, n, 10, Stl_n)")
        PrintLine(FileId, "contour2d(M, X, V, 10, Stl_V)")

        FileClose(FileId)

    End Sub

End Module

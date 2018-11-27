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

Imports OpenVOGEL.AeroTools
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports System.Windows.Forms

Public Class FormReport

    'Private Result As String

    Private CalculationCore As CalculationModel.Solver.Solver
    Private ForcesPanel As ForcesPanel
    Private TotalForcePanel As TotalForcePanel
    Private RawResults As New Text.StringBuilder

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ForcesPanel = New ForcesPanel
        ForcesPanel.Parent = tbLoads
        ForcesPanel.Dock = DockStyle.Fill
        ForcesPanel.BorderStyle = BorderStyle.Fixed3D

        TotalForcePanel = New TotalForcePanel
        TotalForcePanel.Parent = tbTotalLoads
        TotalForcePanel.Dock = DockStyle.Fill
        TotalForcePanel.BorderStyle = BorderStyle.Fixed3D

    End Sub

    Public Sub ReportResults(ByRef CalculationCore As CalculationModel.Solver.Solver)

        Me.CalculationCore = CalculationCore

        ' Load the total forces panels

        TotalForcePanel.LoadResults(CalculationCore)

        ' Load the forces by component

        ForcesPanel.LoadResults(CalculationCore)

        ' Write the raw results text block

        RawResults.Clear()

        If Not IsNothing(CalculationCore) Then

            RawResults.AppendLine("RESULS OF THE AERODYNAMIC ANALYSIS:")
            RawResults.AppendLine("")

            RawResults.AppendLine("Reference velocity:")
            RawResults.AppendLine(String.Format("V = {0:F6}m/s", CalculationCore.StreamVelocity.EuclideanNorm))
            RawResults.AppendLine(String.Format("Vx = {0:F6}m/s, Vy = {1:F6}m/s, Vz = {2:F6}m/s",
                                            CalculationCore.StreamVelocity.X,
                                            CalculationCore.StreamVelocity.Y,
                                            CalculationCore.StreamVelocity.Z))
            RawResults.AppendLine(String.Format("Rho = {0:F6}kg/m³", CalculationCore.StreamDensity))
            RawResults.AppendLine(String.Format("q = {0:F6}Pa", CalculationCore.StreamDynamicPressure))

            Dim i As Integer = 0

            For Each Lattice In CalculationCore.Lattices

                i += 1

                RawResults.AppendLine("")
                RawResults.AppendLine(String.Format("LATTICE {0}", i))
                RawResults.AppendLine("")

                RawResults.AppendLine("Total area: (ΣSi)")
                RawResults.AppendLine(String.Format("S = {0:F6}m²", Lattice.AirLoads.Area))
                RawResults.AppendLine("")

                RawResults.AppendLine(String.Format("CL = {0:F6}", Lattice.AirLoads.CL))
                RawResults.AppendLine(String.Format("CDi = {0:F6}", Lattice.AirLoads.CDi))
                RawResults.AppendLine(String.Format("CDp = {0:F6}", Lattice.AirLoads.CDp))
                RawResults.AppendLine("")

                RawResults.AppendLine("Force due to local lift")

                RawResults.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.SlenderForce.X))
                RawResults.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.SlenderForce.Y))
                RawResults.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.SlenderForce.Z))
                RawResults.AppendLine("")

                RawResults.AppendLine("Moment due to local lift")

                RawResults.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.X))
                RawResults.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.Y))
                RawResults.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.Z))
                RawResults.AppendLine("")

                RawResults.AppendLine("Force due to local induced drag")

                RawResults.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.InducedDrag.X))
                RawResults.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.InducedDrag.Y))
                RawResults.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.InducedDrag.Z))
                RawResults.AppendLine("")

                RawResults.AppendLine("Moment due to local induced drag")

                RawResults.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.InducedMoment.X))
                RawResults.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.InducedMoment.Y))
                RawResults.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.InducedMoment.Z))
                RawResults.AppendLine("")

                RawResults.AppendLine("Force due to local skin drag")

                RawResults.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.SkinDrag.X))
                RawResults.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.SkinDrag.Y))
                RawResults.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.SkinDrag.Z))
                RawResults.AppendLine("")

                RawResults.AppendLine("Moment due to local skin drag")

                RawResults.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.SkinMoment.X))
                RawResults.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.SkinMoment.Y))
                RawResults.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.SkinMoment.Z))
                RawResults.AppendLine("")
                RawResults.AppendLine("Force on body")

                RawResults.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.BodyForce.X))
                RawResults.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.BodyForce.Y))
                RawResults.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.BodyForce.Z))
                RawResults.AppendLine("")

                RawResults.AppendLine("Moment on body")

                RawResults.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.BodyMoment.X))
                RawResults.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.BodyMoment.Y))
                RawResults.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.BodyMoment.Z))
                RawResults.AppendLine("")

                RawResults.AppendLine("Chordwise stations:")
                RawResults.AppendLine("CD, CDi, CDp")

                For Each cl In Lattice.ChordWiseStripes
                    RawResults.AppendLine(String.Format("{0:F8}, {1:F8}, {2:F8}", cl.CL, cl.CDi, cl.CDp))
                Next

                RawResults.AppendLine("")
                RawResults.AppendLine("Local vortex ring properties:")
                RawResults.AppendLine("Index, Cp, Area, G, S, Vx, Vy, Vz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    RawResults.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}, {4,12:F8}, {5,12:F8}, {6,12:F8}, {7,12:F8}", Ring.IndexL, Ring.Cp, Ring.Area, Ring.G, Ring.S, Ring.VelocityT.X, Ring.VelocityT.Y, Ring.VelocityT.Z))
                Next

                RawResults.AppendLine("")
                RawResults.AppendLine("Control point")
                RawResults.AppendLine("Index, CPx, CPy, CPz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    RawResults.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.ControlPoint.X, Ring.ControlPoint.Y, Ring.ControlPoint.Z))
                Next

                RawResults.AppendLine("")
                RawResults.AppendLine("Outer control point")
                RawResults.AppendLine("Index, CPx, CPy, CPz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    If Ring.OuterControlPoint IsNot Nothing Then
                        RawResults.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.OuterControlPoint.X, Ring.OuterControlPoint.Y, Ring.OuterControlPoint.Z))
                    End If
                Next

                RawResults.AppendLine("")
                RawResults.AppendLine("Index, Nx, Ny, Nz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    RawResults.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.Normal.X, Ring.Normal.Y, Ring.Normal.Z))
                Next

            Next

        Else

            RawResults.AppendLine("« Calculation core is not available »")

        End If

        tbRawData.Text = RawResults.ToString

        If Not IsNothing(CalculationCore.StructuralLinks) Then

            cbLink.Items.Clear()
            cbModes.Items.Clear()

            Dim slCount As Integer = 0

            For Each sl As StructuralLink In CalculationCore.StructuralLinks

                cbLink.Items.Add(String.Format("Structural link {0}", slCount))
                slCount += 1

            Next

            AddHandler cbLink.SelectedIndexChanged, AddressOf LoadLink
            AddHandler cbModes.SelectedIndexChanged, AddressOf LoadMode

            cbLink.SelectedIndex = 0

        Else

            tbModalResponse.Enabled = False

        End If

    End Sub

    Public Sub Clear()
        CalculationCore = Nothing
    End Sub

    Private Sub LoadLink()

        If (Not IsNothing(CalculationCore)) And cbLink.SelectedIndex >= 0 And cbLink.SelectedIndex < CalculationCore.StructuralLinks.Count Then

            Dim sl As StructuralLink = CalculationCore.StructuralLinks(cbLink.SelectedIndex)
            cbModes.Items.Clear()

            For Mode = 0 To sl.StructuralCore.Modes.Count - 1
                Dim f As Double = sl.StructuralCore.Modes(Mode).w / 2 / Math.PI
                cbModes.Items.Add(String.Format("Mode {0} - {1,10:F4}Hz", Mode, f))
            Next

            If sl.StructuralCore.Modes.Count > 0 Then cbModes.SelectedIndex = 0

        End If

    End Sub

    Private Sub LoadMode(obj As Object, e As EventArgs)

        If (Not IsNothing(CalculationCore)) And cbLink.SelectedIndex >= 0 And cbLink.SelectedIndex < CalculationCore.StructuralLinks.Count Then

            Dim sl As StructuralLink = CalculationCore.StructuralLinks(cbLink.SelectedIndex)

            If cbModes.SelectedIndex >= 0 And cbModes.SelectedIndex < sl.StructuralCore.Modes.Count Then

                Dim Mode As Integer = cbModes.SelectedIndex

                Dim f As Double = sl.StructuralCore.Modes(Mode).w / 2 / Math.PI

                cModalResponse.Series.Clear()

                cModalResponse.Series.Add(String.Format("Mode {0} - P - @ {1,10:F4}Hz", Mode, f))
                cModalResponse.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Spline

                cModalResponse.Series.Add(String.Format("Mode {0} - V - @ {1,10:F4}Hz", Mode, f))
                cModalResponse.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Line

                For s = 0 To sl.ModalResponse.Count - 1

                    cModalResponse.Series(0).Points.AddXY(s, sl.ModalResponse(s).Item(Mode).p) ' position
                    cModalResponse.Series(1).Points.AddXY(s, sl.ModalResponse(s).Item(Mode).v) ' velocity

                Next

            End If

        End If

    End Sub

End Class
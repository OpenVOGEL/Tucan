'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

Imports AeroTools.CalculationModel.Models.Aero.Components
Imports System.Windows.Forms
Imports AeroTools.CalculationModel.Models.Structural

Public Class FormReport

    'Private Result As String

    Private _CalculationCore As CalculationModel.Solver.UVLMSolver
    Private _ForcesPanel As ForcesPanel
    Private _TotalForcePanel As TotalForcePanel

    Public Sub ReportResults(ByRef CalculationCore As CalculationModel.Solver.UVLMSolver)

        _CalculationCore = CalculationCore

        _ForcesPanel = New ForcesPanel(CalculationCore)
        _ForcesPanel.Parent = tbLoads
        _ForcesPanel.Dock = DockStyle.Fill
        _ForcesPanel.BorderStyle = BorderStyle.Fixed3D

        _TotalForcePanel = New TotalForcePanel(CalculationCore)
        _TotalForcePanel.Parent = tbTotalLoads
        _TotalForcePanel.Dock = DockStyle.Fill
        _TotalForcePanel.BorderStyle = BorderStyle.Fixed3D

        Dim Result As New System.Text.StringBuilder

        'Dont write a string! Use text class instead!

        If Not IsNothing(CalculationCore) Then

            Result.AppendLine("RESULS OF THE AERODYNAMIC ANALYSIS:")
            Result.AppendLine("")

            Result.AppendLine("Reference velocity:")
            Result.AppendLine(String.Format("V = {0:F6}m/s", CalculationCore.StreamVelocity.EuclideanNorm))
            Result.AppendLine(String.Format("Vx = {0:F6}m/s, Vy = {1:F6}m/s, Vz = {2:F6}m/s", _
                                            CalculationCore.StreamVelocity.X, _
                                            CalculationCore.StreamVelocity.Y,
                                            CalculationCore.StreamVelocity.Z))
            Result.AppendLine(String.Format("Rho = {0:F6}kg/m³", CalculationCore.StreamDensity))
            Result.AppendLine(String.Format("q = {0:F6}Pa", CalculationCore.StreamDynamicPressure))

            Dim i As Integer = 0

            For Each Lattice In CalculationCore.Lattices

                i += 1

                Result.AppendLine("")
                Result.AppendLine(String.Format("LATTICE {0}", i))
                Result.AppendLine("")

                Result.AppendLine("Total area: (ΣSi)")
                Result.AppendLine(String.Format("S = {0:F6}m²", Lattice.AirLoads.Area))
                Result.AppendLine("")

                Result.AppendLine(String.Format("CL = {0:F6}", Lattice.AirLoads.CL))
                Result.AppendLine(String.Format("CDi = {0:F6}", Lattice.AirLoads.CDi))
                Result.AppendLine(String.Format("CDp = {0:F6}", Lattice.AirLoads.CDp))
                Result.AppendLine("")

                Result.AppendLine("Force due to local lift")

                Result.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.SlenderForce.X))
                Result.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.SlenderForce.Y))
                Result.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.SlenderForce.Z))
                Result.AppendLine("")

                Result.AppendLine("Moment due to local lift")

                Result.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.X))
                Result.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.Y))
                Result.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.Z))
                Result.AppendLine("")

                Result.AppendLine("Force due to local induced drag")

                Result.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.InducedDrag.X))
                Result.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.InducedDrag.Y))
                Result.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.InducedDrag.Z))
                Result.AppendLine("")

                Result.AppendLine("Moment due to local induced drag")

                Result.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.InducedMoment.X))
                Result.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.InducedMoment.Y))
                Result.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.InducedMoment.Z))
                Result.AppendLine("")

                Result.AppendLine("Force due to local skin drag")

                Result.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.SkinDrag.X))
                Result.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.SkinDrag.Y))
                Result.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.SkinDrag.Z))
                Result.AppendLine("")

                Result.AppendLine("Moment due to local skin drag")

                Result.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.SkinMoment.X))
                Result.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.SkinMoment.Y))
                Result.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.SkinMoment.Z))
                Result.AppendLine("")
                Result.AppendLine("Force on body")

                Result.AppendLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.BodyForce.X))
                Result.AppendLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.BodyForce.Y))
                Result.AppendLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.BodyForce.Z))
                Result.AppendLine("")

                Result.AppendLine("Moment on body")

                Result.AppendLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.BodyMoment.X))
                Result.AppendLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.BodyMoment.Y))
                Result.AppendLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.BodyMoment.Z))
                Result.AppendLine("")

                Result.AppendLine("Chordwise stations:")
                Result.AppendLine("CD, CDi, CDp")

                For Each cl In Lattice.ChordWiseStripes
                    Result.AppendLine(String.Format("{0:F8}, {1:F8}, {2:F8}", cl.CL, cl.CDi, cl.CDp))
                Next

                Result.AppendLine("")
                Result.AppendLine("Local vortex ring properties:")
                Result.AppendLine("Index, Cp, Area, G, S, Vx, Vy, Vz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    Result.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}, {4,12:F8}, {5,12:F8}, {6,12:F8}, {7,12:F8}", Ring.IndexL, Ring.Cp, Ring.Area, Ring.G, Ring.S, Ring.VelocityT.X, Ring.VelocityT.Y, Ring.VelocityT.Z))
                Next

                Result.AppendLine("")
                Result.AppendLine("Control point")
                Result.AppendLine("Index, CPx, CPy, CPz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    Result.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.ControlPoint.X, Ring.ControlPoint.Y, Ring.ControlPoint.Z))
                Next

                Result.AppendLine("")
                Result.AppendLine("Outer control point")
                Result.AppendLine("Index, CPx, CPy, CPz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    If Ring.OuterControlPoint IsNot Nothing Then
                        Result.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.OuterControlPoint.X, Ring.OuterControlPoint.Y, Ring.OuterControlPoint.Z))
                    End If
                Next

                Result.AppendLine("")
                Result.AppendLine("Index, Nx, Ny, Nz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    Result.AppendLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.Normal.X, Ring.Normal.Y, Ring.Normal.Z))
                Next

            Next

        Else

            Result.AppendLine("« Calculation core is not available »")

        End If

        Me.tbRawData.Text = Result.ToString

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

    Private Sub LoadLink()

        If (Not IsNothing(_CalculationCore)) And cbLink.SelectedIndex >= 0 And cbLink.SelectedIndex < _CalculationCore.StructuralLinks.Count Then

            Dim sl As StructuralLink = _CalculationCore.StructuralLinks(cbLink.SelectedIndex)
            cbModes.Items.Clear()

            For Mode = 0 To sl.StructuralCore.Modes.Count - 1
                Dim f As Double = sl.StructuralCore.Modes(Mode).w / 2 / Math.PI
                cbModes.Items.Add(String.Format("Mode {0} - {1,10:F4}Hz", Mode, f))
            Next

            If sl.StructuralCore.Modes.Count > 0 Then cbModes.SelectedIndex = 0

        End If

    End Sub

    Private Sub LoadMode(obj As Object, e As EventArgs)

        If (Not IsNothing(_CalculationCore)) And cbLink.SelectedIndex >= 0 And cbLink.SelectedIndex < _CalculationCore.StructuralLinks.Count Then

            Dim sl As StructuralLink = _CalculationCore.StructuralLinks(cbLink.SelectedIndex)

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
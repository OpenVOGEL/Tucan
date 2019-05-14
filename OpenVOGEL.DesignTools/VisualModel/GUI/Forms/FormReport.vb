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

            AddHandler CalculationCore.PushResultLine, AddressOf RawResults.AppendLine
            CalculationCore.ReportResutls()

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
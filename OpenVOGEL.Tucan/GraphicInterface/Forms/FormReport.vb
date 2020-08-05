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

Imports OpenVOGEL.AeroTools
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library
Imports OpenVOGEL.DesignTools.DataStore

Public Class FormReport

    'Private Result As String

    Private AirloadsPanel As AirloadsPanel
    Private TotalForcePanel As TotalForcePanel
    Private RawResults As New Text.StringBuilder

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        AirloadsPanel = New AirloadsPanel
        AirloadsPanel.Parent = tbLoads
        AirloadsPanel.Dock = DockStyle.Fill
        AirloadsPanel.BorderStyle = BorderStyle.Fixed3D

        TotalForcePanel = New TotalForcePanel
        TotalForcePanel.Parent = tbTotalLoads
        TotalForcePanel.Dock = DockStyle.Fill
        TotalForcePanel.BorderStyle = BorderStyle.Fixed3D

        Dim Title As New DataVisualization.Charting.Title
        Title.Text = "Modal response"
        cModalResponse.Titles.Add(Title)

    End Sub

    Private Sub AppendLine(Line As String)
        RawResults.AppendLine(Line)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub UpdateLoads()

        ' Load the total forces panels

        TotalForcePanel.LoadResults()

        ' Load the forces by component

        AirloadsPanel.LoadResults()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub ReportResults()

        ' Load the total forces panels

        TotalForcePanel.LoadResults()

        ' Load the forces by component

        AirloadsPanel.LoadResults()

        ' Write the raw results text block

        RawResults.Clear()

        RawResults.AppendLine("« Calculation core data not available »")

        tbRawData.Text = RawResults.ToString

        If Not IsNothing(Results.AeroelasticResult) Then

            cbLink.Items.Clear()
            cbModes.Items.Clear()

            Dim slCount As Integer = 0

            For Each Link In Results.AeroelasticResult.Links

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

    ''' <summary>
    ''' Loads the active link
    ''' </summary>
    Private Sub LoadLink()

        If Results.AeroelasticResult IsNot Nothing And cbLink.SelectedIndex >= 0 And cbLink.SelectedIndex < Results.AeroelasticResult.Links.Count Then

            Dim LinkResult = Results.AeroelasticResult.Links(cbLink.SelectedIndex)
            cbModes.Items.Clear()

            Dim Index As Integer = 1
            For Each Mode In LinkResult.Modes
                Dim Frequency As Double = Mode.W / 2 / Math.PI
                cbModes.Items.Add(String.Format("Mode {0} - {1,10:F4}Hz", Index, Frequency))
                Index += 1
            Next

            If LinkResult.Modes.Count > 0 Then cbModes.SelectedIndex = 0

        End If

    End Sub

    ''' <summary>
    ''' Loads the active mode
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <param name="e"></param>
    Private Sub LoadMode(obj As Object, e As EventArgs)

        If Results.AeroelasticResult IsNot Nothing And cbLink.SelectedIndex >= 0 And cbLink.SelectedIndex < Results.AeroelasticResult.Links.Count Then

            Dim LinkResult = Results.AeroelasticResult.Links(cbLink.SelectedIndex)

            If cbModes.SelectedIndex >= 0 And cbModes.SelectedIndex < LinkResult.Modes.Count Then

                Dim ModeIndex As Integer = cbModes.SelectedIndex

                Dim Frequency As Double = LinkResult.Modes(ModeIndex).W / 2 / Math.PI

                cModalResponse.Series.Clear()

                cModalResponse.Series.Add(String.Format("Mode {0} - P - @ {1,10:F4}Hz", ModeIndex, Frequency))
                cModalResponse.Series(0).ChartType = DataVisualization.Charting.SeriesChartType.Line
                cModalResponse.Series(0).Color = Color.Blue
                cModalResponse.Series(0).MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
                cModalResponse.Series(0).MarkerSize = 3
                cModalResponse.Series(0).MarkerColor = Color.Blue

                cModalResponse.Series.Add(String.Format("Mode {0} - V - @ {1,10:F4}Hz", ModeIndex, Frequency))
                cModalResponse.Series(1).ChartType = DataVisualization.Charting.SeriesChartType.Line
                cModalResponse.Series(1).Color = Color.Green
                cModalResponse.Series(1).MarkerStyle = DataVisualization.Charting.MarkerStyle.Circle
                cModalResponse.Series(1).MarkerSize = 3
                cModalResponse.Series(1).MarkerColor = Color.Green

                cModalResponse.ChartAreas(0).AxisX.Title = "Time [s]"
                Dim Space As Double = 10.0# * Results.Settings.Interval
                Dim Decimals As Double = Math.Round(Math.Log10(Space), MidpointRounding.AwayFromZero)
                cModalResponse.ChartAreas(0).AxisX.MajorGrid.Interval = Math.Pow(10, Decimals) * Math.Round(Space * Math.Pow(10, -Decimals))
                cModalResponse.ChartAreas(0).AxisX.MinorGrid.Interval = cModalResponse.ChartAreas(0).AxisX.MajorGrid.Interval / 10.0
                cModalResponse.ChartAreas(0).AxisX.LineWidth = 2

                cModalResponse.ChartAreas(0).AxisY.Title = "Displacement [-]"
                cModalResponse.ChartAreas(0).AxisY.LineWidth = 2

                cModalResponse.Legends(0).BackColor = Color.DimGray
                cModalResponse.Legends(0).Position.X = 40.0
                cModalResponse.Legends(0).Position.Y = 20.0

                Dim TimeIndex As Double = 0.0#
                For Each Coordinate In LinkResult.Modes(ModeIndex).Response

                    cModalResponse.Series(0).Points.AddXY(TimeIndex, Coordinate.P)
                    cModalResponse.Series(1).Points.AddXY(TimeIndex, Coordinate.V)
                    TimeIndex += Results.Settings.Interval

                Next

            End If

        End If

    End Sub

    Private Sub FormReport_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Hide()
        End If

    End Sub

End Class
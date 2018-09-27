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

Imports System.Windows.Forms.DataVisualization.Charting
Imports OpenVOGEL.AeroTools.CalculationModel.Perturbations
Imports OpenVOGEL.AeroTools.CalculationModel.Settings

Public Class FormUnsteadyVelocity

    Private CambiandoInicio As Boolean = True
    Private CargandoElForm As Boolean = True

    Private _Profile As New UnsteadyVelocity
    Private _OuterProfile As UnsteadyVelocity
    Private _Settings As SimulationSettings

    Private ReadOnly Property ProfileLoaded As Boolean
        Get
            Return Not Me._Profile Is Nothing
        End Get
    End Property

    Private ReadOnly Property SelectedAxis As Axes
        Get
            If EjeX.Checked Then Return Axes.X
            If EjeY.Checked Then Return Axes.Y
            If EjeZ.Checked Then Return Axes.Z
            Return Axes.X
        End Get
    End Property

    Private ReadOnly Property SelectedType As ProfileType
        Get
            If rbtImpulsive.Checked Then Return ProfileType.Impulsive
            If rbtPerturbation.Checked Then Return ProfileType.Perturbation
            Return ProfileType.Impulsive
        End Get
    End Property

    Public Sub ShowProfile(ByRef Settings As SimulationSettings, ByVal UnLocked As Boolean)

        ' Set up objects:

        _OuterProfile = Settings.UnsteadyVelocity
        _Profile.Assign(_OuterProfile)
        _Settings = Settings

        ' Set up data:

        tbxVelocity.Text = Format(_Settings.StreamVelocity.EuclideanNorm, "##0.## m/s")
        tbxInterval.Text = Format(_Settings.SimulationSteps * _Settings.Interval, "##0.###### s" & " (" & _Settings.SimulationSteps & " steps)")
        EjeZ.Checked = True

        ' Show data

        ShowData()

        SetUnlocked(UnLocked)

        If UnLocked Then

            AddHandler nudFinalValue.ValueChanged, AddressOf GetData
            AddHandler nudIntensity.ValueChanged, AddressOf GetData
            AddHandler nudInterval.ValueChanged, AddressOf GetData
            AddHandler nudPeak.ValueChanged, AddressOf GetData
            AddHandler nudStart.ValueChanged, AddressOf GetData
            AddHandler rbtImpulsive.CheckedChanged, AddressOf ChangeType
            AddHandler rbtPerturbation.CheckedChanged, AddressOf ChangeType
            AddHandler EjeX.CheckedChanged, AddressOf ShowData
            AddHandler EjeY.CheckedChanged, AddressOf ShowData
            AddHandler EjeZ.CheckedChanged, AddressOf ShowData

        End If

        ' Display form:

        If ProfileLoaded Then
            Dim Result As System.Windows.Forms.DialogResult = ShowDialog()
            Select Case Result
                Case System.Windows.Forms.DialogResult.OK
                    If UnLocked Then _OuterProfile.Assign(_Profile)
            End Select
        End If

    End Sub

    Private _AllowGetData As Boolean = False

    Public Sub PlotOnChart(Perturbation As UnsteadyVelocity, ByRef Graph As Chart)

        Graph.Titles.Item(0).Text = "Unsteady velocity"
        Graph.ChartAreas.Item(0).AxisY.Title = "V(t)/Vo"
        Graph.ChartAreas.Item(0).AxisX.Title = "t/Δt"

        If Graph.Series("Vx").Points.Count >= 0 Then
            Graph.Series("Vx").Points.Clear()
        End If

        If Graph.Series("Vy").Points.Count >= 0 Then
            Graph.Series("Vy").Points.Clear()
        End If

        If Graph.Series("Vz").Points.Count >= 0 Then
            Graph.Series("Vz").Points.Clear()
        End If

        For i = 0 To Perturbation.nSteps - 1

            Graph.Series("Vx").Points.AddXY(i + 1, Perturbation.Intensity(i).X)
            Graph.Series("Vy").Points.AddXY(i + 1, Perturbation.Intensity(i).Y)
            Graph.Series("Vz").Points.AddXY(i + 1, Perturbation.Intensity(i).Z)

        Next

    End Sub

    Private Sub ShowData()

        _AllowGetData = False

        Dim ShowPerturbation As Boolean = _Profile.Type = ProfileType.Perturbation
        rbtPerturbation.Checked = ShowPerturbation
        rbtImpulsive.Checked = Not ShowPerturbation
        nudStart.Enabled = ShowPerturbation
        nudPeak.Enabled = ShowPerturbation
        nudIntensity.Enabled = ShowPerturbation
        nudFinalValue.Enabled = ShowPerturbation
        nudInterval.Enabled = ShowPerturbation

        Dim Axis As Axes = SelectedAxis
        nudStart.Value = _Profile.Perturbation(Axis).Start
        nudPeak.Value = _Profile.Perturbation(Axis).PeakInstant
        nudIntensity.Value = _Profile.Perturbation(Axis).Intensity
        nudFinalValue.Value = _Profile.Perturbation(Axis).FinalDrop
        nudInterval.Value = _Profile.Perturbation(Axis).ElapsedTime

        _Profile.GeneratePerturbation(_Settings.SimulationSteps)

        PlotOnChart(_Profile, gVelocity)

        _AllowGetData = True

    End Sub

    Private Sub GetData()

        If _AllowGetData Then

            Dim Axis As Axes = SelectedAxis
            _Profile.Perturbation(Axis).Start = nudStart.Value
            _Profile.Perturbation(Axis).PeakInstant = nudPeak.Value
            _Profile.Perturbation(Axis).Intensity = nudIntensity.Value
            _Profile.Perturbation(Axis).FinalDrop = nudFinalValue.Value
            _Profile.Perturbation(Axis).ElapsedTime = nudInterval.Value
            _Profile.GeneratePerturbation(_Settings.SimulationSteps)

            PlotOnChart(_Profile, gVelocity)

        End If

    End Sub

    Private Sub ChangeType()

        _Profile.Type = SelectedType
        ShowData()

    End Sub

    Private Sub SetUnlocked(ByVal value As Boolean)

        EjeX.Enabled = value
        EjeY.Enabled = value
        EjeZ.Enabled = value
        nudFinalValue.Enabled = value
        nudIntensity.Enabled = value
        nudInterval.Enabled = value
        nudPeak.Enabled = value
        nudStart.Enabled = value
        rbtImpulsive.Enabled = value
        rbtPerturbation.Enabled = value

    End Sub

End Class
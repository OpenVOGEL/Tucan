'## Open VOGEL ##
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Imports AeroTools.UVLM.SimulationTools
Imports AeroTools.UVLM.Settings

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
            If rbtImpulsive.Checked Then Return ProfileType.Impulsivo
            If rbtPerturbation.Checked Then Return ProfileType.Perturbado
            Return ProfileType.Impulsivo
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
            Dim Result As Windows.Forms.DialogResult = ShowDialog()
            Select Case Result
                Case Windows.Forms.DialogResult.OK
                    If UnLocked Then _OuterProfile.Assign(_Profile)
            End Select
        End If

    End Sub

    Private _AllowGetData As Boolean = False

    Private Sub ShowData()

        _AllowGetData = False

        Dim ShowPerturbation As Boolean = _Profile.Type = ProfileType.Perturbado
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
        _Profile.PlotOnChart(gVelocity)

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
            _Profile.PlotOnChart(gVelocity)

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
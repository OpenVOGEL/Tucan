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

Imports AeroTools.UVLM.Settings
Imports AeroTools.UVLM.SimulationTools

Public Class FormHistogram

    Private _Settings As SimulationSettings
    Private _LocalHistogram As IAeroelasticHistogram
    Private _FlutterTestControl As FlutterTestControl

    Public Sub New(Settings As SimulationSettings)

        InitializeComponent()

        DoubleBuffered = True

        _Settings = Settings

        If (Not IsNothing(_Settings)) AndAlso (Not IsNothing(_Settings.AeroelasticHistogram)) Then
            _LocalHistogram = _Settings.AeroelasticHistogram.Clone
        End If

        cbHistogramType.Items.Add("Flutter test")
        cbHistogramType.SelectedIndex = -1

        AddHandler cbHistogramType.SelectedIndexChanged, AddressOf SelectNewHistogram

        GenerateControl()

    End Sub

    Private Sub GenerateControl()

        If (Not IsNothing(_Settings)) AndAlso (Not IsNothing(_LocalHistogram)) Then

            Select Case _LocalHistogram.Type

                Case HistogramType.FlutterTest

                    DestroyAllControls()
                    _FlutterTestControl = New FlutterTestControl(_LocalHistogram, _Settings.SimulationSteps, _Settings.Interval, _Settings.StreamVelocity)
                    _FlutterTestControl.Parent = Me
                    _FlutterTestControl.Top = cbHistogramType.Bottom + 5
                    _FlutterTestControl.Height = btnOK.Top - cbHistogramType.Bottom - 10
                    _FlutterTestControl.Left = 8
                    _FlutterTestControl.Width = Width - 16
                    _FlutterTestControl.Refresh()
                    cbHistogramType.SelectedIndex = _LocalHistogram.Type

            End Select

        End If

    End Sub

    Private Sub SelectNewHistogram()

        If _LocalHistogram IsNot Nothing AndAlso cbHistogramType.SelectedItem <> CInt(_LocalHistogram.Type) Then
            Return
        End If

        Select Case cbHistogramType.SelectedIndex

            Case HistogramType.FlutterTest
                _LocalHistogram = New FlutterTestHistogram()

        End Select

        GenerateControl()

    End Sub

    Private Sub DestroyAllControls()

        If Not IsNothing(_FlutterTestControl) Then
            _FlutterTestControl.Dispose()
        End If

    End Sub

    Public Sub CopyHistogram()

        If Not IsNothing(_Settings) Then _Settings.AeroelasticHistogram = _LocalHistogram.Clone()

    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        CopyHistogram()

    End Sub

End Class
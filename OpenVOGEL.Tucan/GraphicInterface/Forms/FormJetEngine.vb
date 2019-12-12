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

Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

Public Class FormJetEngine

    Private _JetEngine As JetEngine

    Public Event UpdateModel()

    Public Sub New(ByRef JetEngine As JetEngine)

        InitializeComponent()

        _JetEngine = JetEngine

        SetUpControls()

        AddHandler nudX.ValueChanged, AddressOf RequestUpdate
        AddHandler nudY.ValueChanged, AddressOf RequestUpdate
        AddHandler nudZ.ValueChanged, AddressOf RequestUpdate
        AddHandler nudFrontD.ValueChanged, AddressOf RequestUpdate
        AddHandler nudRearD.ValueChanged, AddressOf RequestUpdate
        AddHandler nudFrontL.ValueChanged, AddressOf RequestUpdate
        AddHandler nudRearL.ValueChanged, AddressOf RequestUpdate
        AddHandler nudTotalL.ValueChanged, AddressOf RequestUpdate
        AddHandler nudPsi.ValueChanged, AddressOf RequestUpdate
        AddHandler nudTita.ValueChanged, AddressOf RequestUpdate
        AddHandler nudPhi.ValueChanged, AddressOf RequestUpdate
        AddHandler tbxName.TextChanged, AddressOf RequestUpdate
        AddHandler nudResolution.ValueChanged, AddressOf RequestUpdate
        AddHandler nudCuttingStep.ValueChanged, AddressOf RequestUpdate

    End Sub

    Private Sub SetUpControls()

        nudX.DecimalPlaces = 3
        nudX.Minimum = -10000000
        nudX.Maximum = 10000000
        nudX.Value = _JetEngine.Position.X

        nudY.DecimalPlaces = 3
        nudY.Minimum = -10000000
        nudY.Maximum = 10000000
        nudY.Value = _JetEngine.Position.Y

        nudZ.DecimalPlaces = 3
        nudZ.Minimum = -10000000
        nudZ.Maximum = 10000000
        nudZ.Value = _JetEngine.Position.Z

        nudPsi.DecimalPlaces = 3
        nudPsi.Minimum = -180
        nudPsi.Maximum = 180
        nudPsi.Value = _JetEngine.Orientation.Psi

        nudTita.DecimalPlaces = 3
        nudTita.Minimum = -180
        nudTita.Maximum = 180
        nudTita.Value = _JetEngine.Orientation.Tita

        nudPhi.DecimalPlaces = 3
        nudPhi.Minimum = -180
        nudPhi.Maximum = 180
        nudPhi.Value = _JetEngine.Orientation.Fi

        nudFrontD.DecimalPlaces = 3
        nudFrontD.Value = _JetEngine.FrontDiameter

        nudFrontL.DecimalPlaces = 3
        nudFrontL.Value = _JetEngine.FrontLength

        nudRearD.DecimalPlaces = 3
        nudRearD.Value = _JetEngine.BackDiameter

        nudRearL.DecimalPlaces = 3
        nudRearL.Value = _JetEngine.BackLength

        nudTotalL.DecimalPlaces = 3
        nudTotalL.Value = _JetEngine.Length

        tbxName.Text = _JetEngine.Name

        nudResolution.Value = _JetEngine.Resolution

        nudCuttingStep.Value = _JetEngine.CuttingStep

    End Sub

    Private Sub RequestUpdate()

        If _JetEngine IsNot Nothing Then

            _JetEngine.Position.X = nudX.Value
            _JetEngine.Position.Y = nudY.Value
            _JetEngine.Position.Z = nudZ.Value
            _JetEngine.FrontDiameter = nudFrontD.Value
            _JetEngine.BackDiameter = nudRearD.Value
            _JetEngine.FrontLength = nudFrontL.Value
            _JetEngine.BackLength = nudRearL.Value
            _JetEngine.Length = nudTotalL.Value
            _JetEngine.Orientation.Psi = nudPsi.Value ' * Math.PI / 180
            _JetEngine.Orientation.Tita = nudTita.Value ' * Math.PI / 180
            _JetEngine.Orientation.Fi = nudPhi.Value ' * Math.PI / 180
            _JetEngine.Name = tbxName.Text
            _JetEngine.Resolution = nudResolution.Value
            _JetEngine.CuttingStep = nudCuttingStep.Value

            _JetEngine.GenerateMesh()
            RaiseEvent UpdateModel()

        End If

    End Sub

End Class
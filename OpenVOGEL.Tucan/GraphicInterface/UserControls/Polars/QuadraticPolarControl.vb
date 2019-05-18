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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components

Public Class QuadraticPolarControl

    Private _Polar As QuadraticPolar

    Public Event OnCurveChanged()
    Public Event OnNameChanged()
    Public Event OnReynoldsChanged()

    Private AllowEvents As Boolean = True

    Public Sub New()

        InitializeComponent()

        AddHandler nudCD0.ValueChanged, AddressOf GetPolarData
        AddHandler nudA.ValueChanged, AddressOf GetPolarData
        AddHandler nudB.ValueChanged, AddressOf GetPolarData
        AddHandler tbPolarName.TextChanged, AddressOf GetPolarName
        AddHandler nudReynolds.ValueChanged, AddressOf GetReynolds

    End Sub

    Private Sub RefreshPolarData()

        If Polar IsNot Nothing Then
            AllowEvents = False
            nudCD0.Value = Polar.Cd0
            nudA.Value = Polar.Cd1
            nudB.Value = Polar.Cd2
            nudReynolds.Value = Polar.Reynolds
            tbPolarName.Text = Polar.Name
            AllowEvents = True
        End If

    End Sub

    Public Property Polar As QuadraticPolar
        Set(value As QuadraticPolar)
            _Polar = value
            RefreshPolarData()
        End Set
        Get
            Return _Polar
        End Get
    End Property

    Private Sub GetPolarData()

        If Polar IsNot Nothing And AllowEvents Then

            Polar.Cd0 = nudCD0.Value
            Polar.Cd1 = nudA.Value
            Polar.Cd2 = nudB.Value

            RaiseEvent OnCurveChanged()

        End If

    End Sub

    Private Sub GetPolarName()

        If Polar IsNot Nothing And AllowEvents Then

            Polar.Name = tbPolarName.Text

            RaiseEvent OnNameChanged()

        End If

    End Sub

    Private Sub GetReynolds()

        If _Polar IsNot Nothing And AllowEvents Then

            _Polar.Reynolds = nudReynolds.Value

            RaiseEvent OnReynoldsChanged()

        End If

    End Sub

End Class

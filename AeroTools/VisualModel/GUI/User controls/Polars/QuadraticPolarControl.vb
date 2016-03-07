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

Imports AeroTools.UVLM.Models.Aero.Components

Public Class QuadraticPolarControl

    Private _Polar As QuadraticPolar

    Private Sub SetBindings()

        nudCD0.DataBindings.Clear()
        nudCD0.DataBindings.Add("Value", _Polar, "Cd0", True, Windows.Forms.DataSourceUpdateMode.OnPropertyChanged)

        nudA.DataBindings.Clear()
        nudA.DataBindings.Add("Value", _Polar, "Cd1", True, Windows.Forms.DataSourceUpdateMode.OnPropertyChanged)

        nudB.DataBindings.Clear()
        nudB.DataBindings.Add("Value", _Polar, "Cd2", True, Windows.Forms.DataSourceUpdateMode.OnPropertyChanged)

        nudReynolds.DataBindings.Clear()
        nudReynolds.DataBindings.Add("Value", _Polar, "Reynolds", True, Windows.Forms.DataSourceUpdateMode.OnPropertyChanged)

        tbPolarName.DataBindings.Clear()
        tbPolarName.DataBindings.Add("Text", _Polar, "Name", True, Windows.Forms.DataSourceUpdateMode.OnPropertyChanged)

    End Sub

    Public Property Polar As QuadraticPolar
        Set(value As QuadraticPolar)
            _Polar = value
            SetBindings()
        End Set
        Get
            Return _Polar
        End Get
    End Property

End Class

'Copyright (C) 2016 Guillermo Hazebrouck

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

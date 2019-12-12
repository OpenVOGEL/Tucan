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
Imports OpenVOGEL.MathTools.Magnitudes

Public Class FormResults

    ' Components:

    Private rbVelocity As New ResultBox(Magnitudes.Velocity)
    Private rbDensity As New ResultBox(Magnitudes.Density)

    Private rbAlpha As New ResultBox(Magnitudes.Angular)
    Private rbBeta As New ResultBox(Magnitudes.Angular)

    Private rbArea As New ResultBox(Magnitudes.Area)

    Private rbCL As New ResultBox(Magnitudes.Dimensionless)
    Private rbCD As New ResultBox(Magnitudes.Dimensionless)
    Private rbCDi As New ResultBox(Magnitudes.Dimensionless)
    Private rbCDp As New ResultBox(Magnitudes.Dimensionless)

    Private rbCFx As New ResultBox(Magnitudes.Dimensionless)
    Private rbCFy As New ResultBox(Magnitudes.Dimensionless)
    Private rbCFz As New ResultBox(Magnitudes.Dimensionless)

    Private rbCMx As New ResultBox(Magnitudes.Dimensionless)
    Private rbCMy As New ResultBox(Magnitudes.Dimensionless)
    Private rbCMz As New ResultBox(Magnitudes.Dimensionless)

    Private Sub SetUpComponents()

        rbVelocity.Name = "V"
        rbVelocity.Top = 10
        rbVelocity.Left = 10
        rbVelocity.Parent = Me

        rbDensity.Name = "r"
        rbDensity.GreekLetter = True
        rbDensity.Top = rbVelocity.Top
        rbDensity.Left = rbVelocity.Right + 10
        rbDensity.Parent = Me

        rbAlpha.GreekLetter = True
        rbAlpha.Name = "a"
        rbAlpha.Top = rbDensity.Top
        rbAlpha.Left = rbDensity.Right + 10
        rbAlpha.Parent = Me
        Dim ma As AngularMagnitude = rbAlpha.Magnitude
        ma.Unit = AngularMagnitude.Units.Degrees

        rbBeta.GreekLetter = True
        rbBeta.Name = "b"
        rbBeta.Top = rbAlpha.Bottom + 10
        rbBeta.Left = rbAlpha.Left
        rbBeta.Parent = Me
        Dim mb As AngularMagnitude = rbBeta.Magnitude
        mb.Unit = AngularMagnitude.Units.Degrees

        cbLattices.Top = rbVelocity.Bottom + 10
        cbLattices.Left = rbVelocity.Left

        rbArea.Name = "A"
        rbArea.Top = cbLattices.Bottom + 10
        rbArea.Left = cbLattices.Left
        rbArea.Parent = Me

        rbCL.Name = "CL"
        rbCL.Top = rbArea.Bottom + 10
        rbCL.Left = rbArea.Left
        rbCL.Parent = Me

        rbCDi.Name = "CDi"
        rbCDi.Top = rbCL.Bottom + 10
        rbCDi.Left = rbCL.Left
        rbCDi.Parent = Me

        rbCDp.Name = "CDp"
        rbCDp.Top = rbCDi.Bottom + 10
        rbCDp.Left = rbCDi.Left
        rbCDp.Parent = Me

        rbCFx.Name = "CFx"
        rbCFx.Top = rbCL.Top
        rbCFx.Left = rbCL.Right + 10
        rbCFx.Parent = Me

        rbCFy.Name = "CFy"
        rbCFy.Top = rbCFx.Bottom + 10
        rbCFy.Left = rbCFx.Left
        rbCFy.Parent = Me

        rbCFz.Name = "CFz"
        rbCFz.Top = rbCFy.Bottom + 10
        rbCFz.Left = rbCFy.Left
        rbCFz.Parent = Me

        rbCMx.Name = "CMx"
        rbCMx.Top = rbCFx.Top
        rbCMx.Left = rbCFx.Right + 10
        rbCMx.Parent = Me

        rbCMy.Name = "CMy"
        rbCMy.Top = rbCMx.Bottom + 10
        rbCMy.Left = rbCMx.Left
        rbCMy.Parent = Me

        rbCMz.Name = "CMz"
        rbCMz.Top = rbCMy.Bottom + 10
        rbCMz.Left = rbCMy.Left
        rbCMz.Parent = Me

        AddHandler cbLattices.SelectedIndexChanged, AddressOf LoadLattice

    End Sub

    ' Results object:

    Private _CalculationCore As CalculationModel.Solver.Solver

    Private Sub LoadResultsData()

        If _CalculationCore IsNot Nothing Then

            rbVelocity.Value = _CalculationCore.StreamVelocity.EuclideanNorm
            rbDensity.Value = _CalculationCore.StreamDensity
            rbAlpha.Value = Math.Asin(_CalculationCore.StreamVelocity.Z / _CalculationCore.StreamVelocity.EuclideanNorm)
            rbBeta.Value = Math.Asin(_CalculationCore.StreamVelocity.Y / _CalculationCore.StreamVelocity.EuclideanNorm)

            _CalculationCore.CalculateAirloads()

            For i = 0 To _CalculationCore.Lattices.Count - 1

                cbLattices.Items.Add(String.Format("Lattice {0}", i))

            Next

            cbLattices.SelectedIndex = 0

        End If

    End Sub

    Private Sub LoadLattice()

        Dim Index As Integer = cbLattices.SelectedIndex

        If Index >= 0 And Index < _CalculationCore.Lattices.Count Then

            rbArea.Value = _CalculationCore.Lattices(Index).AirLoads.Area

            rbCL.Value = _CalculationCore.Lattices(Index).AirLoads.CL
            rbCDp.Value = _CalculationCore.Lattices(Index).AirLoads.CDp
            rbCDi.Value = _CalculationCore.Lattices(Index).AirLoads.CDi

            rbCFx.Value = _CalculationCore.Lattices(Index).AirLoads.Force.X
            rbCFy.Value = _CalculationCore.Lattices(Index).AirLoads.Force.Y
            rbCFz.Value = _CalculationCore.Lattices(Index).AirLoads.Force.Z

            rbCMx.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.X
            rbCMy.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.Y
            rbCMz.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.Z

        End If

    End Sub

    Public Sub New()

        InitializeComponent()

        SetUpComponents()

    End Sub

    Public Sub New(CalculationCore As CalculationModel.Solver.Solver)

        InitializeComponent()

        SetUpComponents()

        _CalculationCore = CalculationCore

        LoadResultsData()

    End Sub

End Class
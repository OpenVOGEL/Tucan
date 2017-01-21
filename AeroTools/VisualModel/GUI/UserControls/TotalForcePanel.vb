'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2017 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

Imports AeroTools.DataStacks
Imports MathTools.Magnitudes
Imports AeroTools.CalculationModel.Models.Aero
Imports MathTools.Algebra.EuclideanSpace

Public Class TotalForcePanel

    Private _CalculationCore As CalculationModel.Solver.UVLMSolver

    ' Components:

    Private rbVelocity As New ResultBox(UserMagnitudes(Magnitudes.Velocity))
    Private rbDensity As New ResultBox(UserMagnitudes(Magnitudes.Density))

    Private rbAlpha As New ResultBox(UserMagnitudes(Magnitudes.Angular))
    Private rbBeta As New ResultBox(UserMagnitudes(Magnitudes.Angular))

    Private rbFx As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbFy As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbFz As New ResultBox(UserMagnitudes(Magnitudes.Force))

    Private rbMx As New ResultBox(UserMagnitudes(Magnitudes.Moment))
    Private rbMy As New ResultBox(UserMagnitudes(Magnitudes.Moment))
    Private rbMz As New ResultBox(UserMagnitudes(Magnitudes.Moment))

    Private TotalAirloads As New TotalAirLoads
    Private TotalForce As New EVector3
    Private TotalMoment As New EVector3

    Public Sub New(CalculationCore As CalculationModel.Solver.UVLMSolver)

        InitializeComponent()

        SetUpComponents()

        _CalculationCore = CalculationCore

        _CalculationCore.CalculateAirloads()

        LoadResultsData()

    End Sub

    Private Sub SetUpComponents()

        rbVelocity.Name = "V"
        rbVelocity.Top = gbReferencePoint.Bottom + 10
        rbVelocity.Left = 10
        rbVelocity.Parent = Me
        rbVelocity.Decimals = GlobalDecimals(Magnitudes.Velocity)

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

        rbFx.Name = "Fx"
        rbFx.Top = rbBeta.Bottom + 10
        rbFx.Left = rbVelocity.Left
        rbFx.Parent = Me

        rbFy.Name = "Fy"
        rbFy.Top = rbFx.Bottom + 10
        rbFy.Left = rbVelocity.Left
        rbFy.Parent = Me

        rbFz.Name = "Fz"
        rbFz.Top = rbFy.Bottom + 10
        rbFz.Left = rbVelocity.Left
        rbFz.Parent = Me

        rbMx.Name = "Mx"
        rbMx.Top = rbBeta.Bottom + 10
        rbMx.Left = rbFx.Right + 10
        rbMx.Parent = Me

        rbMy.Name = "My"
        rbMy.Top = rbMx.Bottom + 10
        rbMy.Left = rbMx.Left
        rbMy.Parent = Me

        rbMz.Name = "Mz"
        rbMz.Top = rbMy.Bottom + 10
        rbMz.Left = rbMx.Left
        rbMz.Parent = Me

    End Sub

    Private Sub LoadResultsData()

        If _CalculationCore IsNot Nothing Then

            rbVelocity.Value = _CalculationCore.StreamVelocity.EuclideanNorm
            rbDensity.Value = _CalculationCore.StreamDensity
            rbAlpha.Value = Math.Asin(_CalculationCore.StreamVelocity.Z / _CalculationCore.StreamVelocity.EuclideanNorm)
            rbBeta.Value = Math.Asin(_CalculationCore.StreamVelocity.Y / _CalculationCore.StreamVelocity.EuclideanNorm)

        End If

        TotalAirloads.Clear()

        For i = 0 To _CalculationCore.Lattices.Count - 1

            TotalAirloads.Add(_CalculationCore.Lattices(i).AirLoads)

        Next

        RecalculateLoads()

    End Sub

    Private Sub RecalculateLoads()

        TotalForce.SetToCero()
        TotalForce.Add(TotalAirloads.SlenderForce)
        TotalForce.Add(TotalAirloads.InducedDrag)
        TotalForce.Add(TotalAirloads.SkinDrag)
        TotalForce.Add(TotalAirloads.BodyForce)
        TotalForce.Scale(_CalculationCore.StreamDynamicPressure)

        TotalMoment.SetToCero()
        TotalMoment.Add(TotalAirloads.SlenderMoment)
        TotalMoment.Add(TotalAirloads.InducedMoment)
        TotalMoment.Add(TotalAirloads.SkinMoment)
        TotalMoment.Add(TotalAirloads.BodyMoment)
        TotalMoment.Scale(_CalculationCore.StreamDynamicPressure)

        Dim R As New EVector3
        R.X = nudRx.Value
        R.Y = nudRy.Value
        R.Z = nudRz.Value

        TotalMoment.AddCrossProduct(TotalForce, R)

        rbFx.Value = TotalForce.X
        rbFy.Value = TotalForce.Y
        rbFz.Value = TotalForce.Z

        rbMx.Value = TotalMoment.X
        rbMy.Value = TotalMoment.Y
        rbMz.Value = TotalMoment.Z

    End Sub

    Private Sub nudRx_ValueChanged(sender As Object, e As EventArgs) Handles nudRx.ValueChanged

        RecalculateLoads()

    End Sub

    Private Sub nudRy_ValueChanged(sender As Object, e As EventArgs) Handles nudRy.ValueChanged

        RecalculateLoads()

    End Sub

    Private Sub nudRz_ValueChanged(sender As Object, e As EventArgs) Handles nudRz.ValueChanged

        RecalculateLoads()

    End Sub

End Class

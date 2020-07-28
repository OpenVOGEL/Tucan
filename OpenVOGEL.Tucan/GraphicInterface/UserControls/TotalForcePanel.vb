'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2017 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.MathTools.Magnitudes
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Public Class TotalForcePanel

    ' Components:

    Private rbVelocity As New ResultBox(UserMagnitudes(Magnitudes.Velocity))
    Private rbDensity As New ResultBox(UserMagnitudes(Magnitudes.Density))
    Private rbq As New ResultBox(UserMagnitudes(Magnitudes.Pressure))

    Private rbAlpha As New ResultBox(UserMagnitudes(Magnitudes.Angular))
    Private rbBeta As New ResultBox(UserMagnitudes(Magnitudes.Angular))

    Private rbFx As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbFy As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbFz As New ResultBox(UserMagnitudes(Magnitudes.Force))

    Private rbMx As New ResultBox(UserMagnitudes(Magnitudes.Moment))
    Private rbMy As New ResultBox(UserMagnitudes(Magnitudes.Moment))
    Private rbMz As New ResultBox(UserMagnitudes(Magnitudes.Moment))

    Private rbCFx As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCFy As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCFz As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))

    Private rbCMx As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCMy As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCMz As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))

    Private rbL As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbD As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbY As New ResultBox(UserMagnitudes(Magnitudes.Force))

    Private rbCL As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCD As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCY As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))

    Private TotalForce As New Vector3
    Private TotalMoment As New Vector3

    Public Sub New()

        InitializeComponent()

        SetUpComponents()

    End Sub

    Public Sub LoadResults()

        LoadResultsData()

    End Sub

    Private Sub SetUpComponents()

        rbVelocity.Name = "V"
        rbVelocity.Top = gbReferencePoint.Bottom + 10
        rbVelocity.Left = 10
        rbVelocity.Parent = Me
        rbVelocity.Decimals = GlobalDecimals(Magnitudes.Velocity)

        rbq.Name = "q"
        rbq.Top = rbVelocity.Bottom + 10
        rbq.Left = 10
        rbq.Parent = Me
        rbq.Decimals = GlobalDecimals(Magnitudes.Pressure)

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

        ' Forces and moments

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

        ' Dimensionless forces and moments

        rbCFx.Name = "CFx"
        rbCFx.Top = rbBeta.Bottom + 10
        rbCFx.Left = rbVelocity.Left
        rbCFx.Parent = Me
        rbCFx.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCFy.Name = "CFy"
        rbCFy.Top = rbFx.Bottom + 10
        rbCFy.Left = rbVelocity.Left
        rbCFy.Parent = Me
        rbCFy.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCFz.Name = "CFz"
        rbCFz.Top = rbFy.Bottom + 10
        rbCFz.Left = rbVelocity.Left
        rbCFz.Parent = Me
        rbCFz.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCMx.Name = "CMx"
        rbCMx.Top = rbBeta.Bottom + 10
        rbCMx.Left = rbFx.Right + 10
        rbCMx.Parent = Me
        rbCMx.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCMy.Name = "CMy"
        rbCMy.Top = rbMx.Bottom + 10
        rbCMy.Left = rbMx.Left
        rbCMy.Parent = Me
        rbCMy.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCMz.Name = "CMz"
        rbCMz.Top = rbMy.Bottom + 10
        rbCMz.Left = rbMx.Left
        rbCMz.Parent = Me
        rbCMz.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        ' Aerodynamic coordinates

        rbL.Name = "L"
        rbL.Top = rbBeta.Bottom + 10
        rbL.Left = rbVelocity.Left
        rbL.Parent = Me

        rbD.Name = "D"
        rbD.Top = rbFx.Bottom + 10
        rbD.Left = rbVelocity.Left
        rbD.Parent = Me

        rbY.Name = "Y"
        rbY.Top = rbFy.Bottom + 10
        rbY.Left = rbVelocity.Left
        rbY.Parent = Me


        rbCL.Name = "CL"
        rbCL.Top = rbBeta.Bottom + 10
        rbCL.Left = rbVelocity.Left
        rbCL.Parent = Me
        rbCL.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCD.Name = "CD"
        rbCD.Top = rbFx.Bottom + 10
        rbCD.Left = rbVelocity.Left
        rbCD.Parent = Me
        rbCD.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCY.Name = "CY"
        rbCY.Top = rbFy.Bottom + 10
        rbCY.Left = rbVelocity.Left
        rbCY.Parent = Me
        rbCY.Decimals = GlobalDecimals(Magnitudes.Dimensionless)


        ' Labels

        lblSurfaceUnit.Text = GlobalMagnitudes.UserMagnitudes(Magnitudes.Area).Label

        lblLengthUnit.Text = GlobalMagnitudes.UserMagnitudes(Magnitudes.Length).Label

        lblRxUnit.Text = GlobalMagnitudes.UserMagnitudes(Magnitudes.Length).Label

        lblRyUnit.Text = GlobalMagnitudes.UserMagnitudes(Magnitudes.Length).Label

        lblRzUnit.Text = GlobalMagnitudes.UserMagnitudes(Magnitudes.Length).Label

        UpdateLayout()

    End Sub

    Private Sub LoadResultsData()

        If ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Frame = ProjectRoot.Results.ActiveFrame
            rbVelocity.Value = Frame.StreamVelocity.EuclideanNorm
            rbDensity.Value = ProjectRoot.Results.Settings.Density
            rbAlpha.Value = Frame.TotalAirLoads.Alfa
            rbBeta.Value = Frame.TotalAirLoads.Beta
            nudSurface.Value = Frame.TotalAirLoads.Area

            RecalculateLoads()

        End If

    End Sub

    Private Sub RecalculateLoads()

        If ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim TotalAirloads As AirLoads = ProjectRoot.Results.ActiveFrame.TotalAirLoads

            rbq.Value = TotalAirloads.DynamicPressure

            TotalForce.SetToCero()

            If cbSlenderForces.Checked Then TotalForce.Add(TotalAirloads.LiftForce)
            If cbInducedForces.Checked Then TotalForce.Add(TotalAirloads.InducedDragForce)
            If cbSkinDrag.Checked Then TotalForce.Add(TotalAirloads.SkinDragForce)
            If cbBodyForces.Checked Then TotalForce.Add(TotalAirloads.BodyForce)

            TotalMoment.SetToCero()
            If cbSlenderForces.Checked Then TotalMoment.Add(TotalAirloads.LiftMoment)
            If cbInducedForces.Checked Then TotalMoment.Add(TotalAirloads.InducedDragMoment)
            If cbSkinDrag.Checked Then TotalMoment.Add(TotalAirloads.SkinDragMoment)
            If cbBodyForces.Checked Then TotalMoment.Add(TotalAirloads.BodyMoment)

            Dim R As New Vector3
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

            ' Dimensionless (the user can adapt the reference area and length)

            Dim S As Double = Math.Max(0.001, nudSurface.Value)
            Dim c As Double = Math.Max(0.001, nudLength.Value)

            Dim qS As Double = TotalAirloads.DynamicPressure * S
            Dim qSc As Double = TotalAirloads.DynamicPressure * S * c

            rbCFx.Value = TotalForce.X / qS
            rbCFy.Value = TotalForce.Y / qS
            rbCFz.Value = TotalForce.Z / qS

            rbCMx.Value = TotalMoment.X / qSc
            rbCMy.Value = TotalMoment.Y / qSc
            rbCMz.Value = TotalMoment.Z / qSc

            ' Components in aerodynamic coordinates

            Dim Basis As New Base3

            Basis.U.X = ProjectRoot.Results.ActiveFrame.StreamVelocity.X
            Basis.U.Y = ProjectRoot.Results.ActiveFrame.StreamVelocity.Y
            Basis.U.Z = ProjectRoot.Results.ActiveFrame.StreamVelocity.Z
            Basis.U.Normalize()

            Basis.W.X = Basis.U.X
            Basis.W.Z = Basis.U.Z
            Dim Ux As Double = Basis.W.X
            Dim Uz As Double = Basis.W.Z
            Basis.W.Z = Ux
            Basis.W.X = -Uz
            Basis.W.Normalize()

            Basis.V.FromVectorProduct(Basis.W, Basis.U)

            Dim L As Double = TotalForce.InnerProduct(Basis.W)
            Dim D As Double = TotalForce.InnerProduct(Basis.U)
            Dim Y As Double = TotalForce.InnerProduct(Basis.V)

            rbL.Value = L
            rbD.Value = D
            rbY.Value = Y

            rbCL.Value = L / qS
            rbCD.Value = D / qS
            rbCY.Value = Y / qS

        End If

    End Sub

    Private Sub UpdateLayout()

        nudSurface.Enabled = cbDimensionless.Checked

        nudLength.Enabled = cbDimensionless.Checked

        rbFx.Visible = Not cbDimensionless.Checked And rbBodyCoordinates.Checked
        rbFy.Visible = Not cbDimensionless.Checked And rbBodyCoordinates.Checked
        rbFz.Visible = Not cbDimensionless.Checked And rbBodyCoordinates.Checked

        rbCFx.Visible = cbDimensionless.Checked And rbBodyCoordinates.Checked
        rbCFy.Visible = cbDimensionless.Checked And rbBodyCoordinates.Checked
        rbCFz.Visible = cbDimensionless.Checked And rbBodyCoordinates.Checked

        rbL.Visible = Not cbDimensionless.Checked And rbAeroCoordinates.Checked
        rbD.Visible = Not cbDimensionless.Checked And rbAeroCoordinates.Checked
        rbY.Visible = Not cbDimensionless.Checked And rbAeroCoordinates.Checked

        rbCL.Visible = cbDimensionless.Checked And rbAeroCoordinates.Checked
        rbCD.Visible = cbDimensionless.Checked And rbAeroCoordinates.Checked
        rbCY.Visible = cbDimensionless.Checked And rbAeroCoordinates.Checked

        rbMx.Visible = Not cbDimensionless.Checked
        rbMy.Visible = Not cbDimensionless.Checked
        rbMz.Visible = Not cbDimensionless.Checked

        rbCMx.Visible = cbDimensionless.Checked
        rbCMy.Visible = cbDimensionless.Checked
        rbCMz.Visible = cbDimensionless.Checked

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

    Private Sub cbDimensionless_CheckedChanged(sender As Object, e As EventArgs) Handles cbDimensionless.CheckedChanged

        UpdateLayout()

    End Sub

    Private Sub cbSlenderForces_CheckedChanged(sender As Object, e As EventArgs) Handles cbSlenderForces.CheckedChanged

        RecalculateLoads()

    End Sub

    Private Sub cbInducedForces_CheckedChanged(sender As Object, e As EventArgs) Handles cbInducedForces.CheckedChanged

        RecalculateLoads()

    End Sub

    Private Sub cbSkinDrag_CheckedChanged(sender As Object, e As EventArgs) Handles cbSkinDrag.CheckedChanged

        RecalculateLoads()

    End Sub

    Private Sub cbBodyForces_CheckedChanged(sender As Object, e As EventArgs) Handles cbBodyForces.CheckedChanged

        RecalculateLoads()

    End Sub

    Private Sub nudSurface_ValueChanged(sender As Object, e As EventArgs) Handles nudSurface.ValueChanged

        RecalculateLoads()

    End Sub

    Private Sub nudLengthUnit_ValueChanged(sender As Object, e As EventArgs) Handles nudLength.ValueChanged

        RecalculateLoads()

    End Sub

    Private Sub rbBodyCoordinates_CheckedChanged(sender As Object, e As EventArgs) Handles rbBodyCoordinates.CheckedChanged

        UpdateLayout()

    End Sub

    Private Sub rbAeroCoordinates_CheckedChanged(sender As Object, e As EventArgs) Handles rbAeroCoordinates.CheckedChanged

        UpdateLayout()

    End Sub

End Class

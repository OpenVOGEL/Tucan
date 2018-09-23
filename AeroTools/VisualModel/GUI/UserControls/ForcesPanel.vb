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

Imports System.Drawing
Imports System.Windows.Forms
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.AeroTools.DataStore
Imports OpenVOGEL.MathTools.Magnitudes

Public Class ForcesPanel

    ' Components:

    Private rbVelocity As New ResultBox(UserMagnitudes(Magnitudes.Velocity))
    Private rbDensity As New ResultBox(UserMagnitudes(Magnitudes.Density))

    Private rbAlpha As New ResultBox(UserMagnitudes(Magnitudes.Angular))
    Private rbBeta As New ResultBox(UserMagnitudes(Magnitudes.Angular))

    Private rbArea As New ResultBox(UserMagnitudes(Magnitudes.Area))

    Private rbCL As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCD As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCDi As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCDp As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))

    Private rbCFx As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCFy As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCFz As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))

    Private rbCMx As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCMy As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))
    Private rbCMz As New ResultBox(UserMagnitudes(Magnitudes.Dimensionless))

    Private rbL As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbD As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbDi As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbDp As New ResultBox(UserMagnitudes(Magnitudes.Force))

    Private rbFx As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbFy As New ResultBox(UserMagnitudes(Magnitudes.Force))
    Private rbFz As New ResultBox(UserMagnitudes(Magnitudes.Force))

    Private rbMx As New ResultBox(UserMagnitudes(Magnitudes.Moment))
    Private rbMy As New ResultBox(UserMagnitudes(Magnitudes.Moment))
    Private rbMz As New ResultBox(UserMagnitudes(Magnitudes.Moment))

    Private rbDimensionless As New CheckBox()

    Private pbLoadingGraph As New PictureBox

    Private Const strCL As String = "CL"
    Private Const strCDp As String = "CDp"
    Private Const strCDi As String = "CDi"

    Private Sub SetUpComponents()

        rbVelocity.Name = "V"
        rbVelocity.Top = 10
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

        cbLattices.Top = rbVelocity.Bottom + 10
        cbLattices.Left = rbVelocity.Left

        rbArea.Name = "S"
        rbArea.Top = cbLattices.Bottom + 10
        rbArea.Left = cbLattices.Left
        rbArea.Parent = Me

        ' Coefficients:

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

        ' Net forces:

        rbL.Name = "L"
        rbL.Top = rbArea.Bottom + 10
        rbL.Left = rbArea.Left
        rbL.Parent = Me
        rbL.Decimals = GlobalDecimals(Magnitudes.Force)

        rbDi.Name = "Di"
        rbDi.Top = rbCL.Bottom + 10
        rbDi.Left = rbCL.Left
        rbDi.Parent = Me
        rbDi.Decimals = GlobalDecimals(Magnitudes.Force)

        rbDp.Name = "Dp"
        rbDp.Top = rbCDi.Bottom + 10
        rbDp.Left = rbCDi.Left
        rbDp.Parent = Me
        rbDp.Decimals = GlobalDecimals(Magnitudes.Force)

        rbFx.Name = "Fx"
        rbFx.Top = rbCL.Top
        rbFx.Left = rbCL.Right + 10
        rbFx.Parent = Me
        rbFx.Decimals = GlobalDecimals(Magnitudes.Force)

        rbFy.Name = "Fy"
        rbFy.Top = rbCFx.Bottom + 10
        rbFy.Left = rbCFx.Left
        rbFy.Parent = Me
        rbFy.Decimals = GlobalDecimals(Magnitudes.Force)

        rbFz.Name = "Fz"
        rbFz.Top = rbCFy.Bottom + 10
        rbFz.Left = rbCFy.Left
        rbFz.Parent = Me
        rbFz.Decimals = GlobalDecimals(Magnitudes.Force)

        rbMx.Name = "Mx"
        rbMx.Top = rbCFx.Top
        rbMx.Left = rbCFx.Right + 10
        rbMx.Parent = Me
        rbMx.Decimals = GlobalDecimals(Magnitudes.Moment)

        rbMy.Name = "My"
        rbMy.Top = rbCMx.Bottom + 10
        rbMy.Left = rbCMx.Left
        rbMy.Parent = Me
        rbMy.Decimals = GlobalDecimals(Magnitudes.Moment)

        rbMz.Name = "Mz"
        rbMz.Top = rbCMy.Bottom + 10
        rbMz.Left = rbCMy.Left
        rbMz.Parent = Me
        rbMz.Decimals = GlobalDecimals(Magnitudes.Moment)

        cbResultType.Items.Clear()
        cbResultType.Items.Add(strCL)
        cbResultType.Items.Add(strCDp)
        cbResultType.Items.Add(strCDi)
        cbResultType.Left = rbVelocity.Left
        cbResultType.Top = rbCMz.Bottom + 10

        pbLoadingGraph.Parent = Me
        pbLoadingGraph.Top = cbResultType.Bottom + 10
        pbLoadingGraph.Left = rbVelocity.Left
        pbLoadingGraph.Width = Width - 20
        pbLoadingGraph.Height = Height - pbLoadingGraph.Top - 10
        pbLoadingGraph.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        pbLoadingGraph.BackColor = Color.White
        pbLoadingGraph.BorderStyle = BorderStyle.None
        AddHandler pbLoadingGraph.MouseMove, AddressOf pgLoadingGraph_MouseMove
        AddHandler pbLoadingGraph.Paint, AddressOf DrawLoad
        AddHandler cbResultType.SelectedIndexChanged, AddressOf pbLoadingGraph.Refresh

        AddHandler cbLattices.SelectedIndexChanged, AddressOf LoadLattice

        rbDimensionless.Text = "Dimensionless"
        rbDimensionless.Top = rbAlpha.Top
        rbDimensionless.Left = rbAlpha.Right + 10
        rbDimensionless.Parent = Me
        AddHandler rbDimensionless.CheckedChanged, AddressOf SwitchDimensionless
        rbDimensionless.Checked = True

    End Sub

    Private Sub SwitchDimensionless()

        rbCL.Visible = rbDimensionless.Checked
        rbCDp.Visible = rbDimensionless.Checked
        rbCDi.Visible = rbDimensionless.Checked

        rbCFx.Visible = rbDimensionless.Checked
        rbCFy.Visible = rbDimensionless.Checked
        rbCFz.Visible = rbDimensionless.Checked

        rbCMx.Visible = rbDimensionless.Checked
        rbCMy.Visible = rbDimensionless.Checked
        rbCMz.Visible = rbDimensionless.Checked

        rbL.Visible = Not rbDimensionless.Checked
        rbDp.Visible = Not rbDimensionless.Checked
        rbDi.Visible = Not rbDimensionless.Checked

        rbFx.Visible = Not rbDimensionless.Checked
        rbFy.Visible = Not rbDimensionless.Checked
        rbFz.Visible = Not rbDimensionless.Checked

        rbMx.Visible = Not rbDimensionless.Checked
        rbMy.Visible = Not rbDimensionless.Checked
        rbMz.Visible = Not rbDimensionless.Checked

    End Sub

    ' Results object:

    Private _CalculationCore As CalculationModel.Solver.Solver

    Private Sub LoadResultsData()

        If _CalculationCore IsNot Nothing Then

            rbVelocity.Value = _CalculationCore.StreamVelocity.EuclideanNorm
            rbDensity.Value = _CalculationCore.StreamDensity
            rbAlpha.Value = Math.Asin(_CalculationCore.StreamVelocity.Z / _CalculationCore.StreamVelocity.EuclideanNorm)
            rbBeta.Value = Math.Asin(_CalculationCore.StreamVelocity.Y / _CalculationCore.StreamVelocity.EuclideanNorm)

            For i = 0 To _CalculationCore.Lattices.Count - 1

                cbLattices.Items.Add(String.Format("Lattice {0}", i))

            Next

            cbLattices.SelectedIndex = 0
            cbResultType.SelectedIndex = 0

        End If

    End Sub

    Private Sub LoadLattice()

        Dim Index As Integer = cbLattices.SelectedIndex

        If Index >= 0 And Index < _CalculationCore.Lattices.Count Then

            Dim s As Double = _CalculationCore.Lattices(Index).AirLoads.Area
            rbArea.Value = s

            rbCL.Value = _CalculationCore.Lattices(Index).AirLoads.CL
            rbCDp.Value = _CalculationCore.Lattices(Index).AirLoads.CDp
            rbCDi.Value = _CalculationCore.Lattices(Index).AirLoads.CDi

            rbCFx.Value = _CalculationCore.Lattices(Index).AirLoads.Force.X
            rbCFy.Value = _CalculationCore.Lattices(Index).AirLoads.Force.Y
            rbCFz.Value = _CalculationCore.Lattices(Index).AirLoads.Force.Z

            rbCMx.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.X
            rbCMy.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.Y
            rbCMz.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.Z

            ' Net forces:

            Dim q As Double = 0.5 * _CalculationCore.StreamVelocity.SquareEuclideanNorm * _CalculationCore.StreamDensity

            rbL.Value = _CalculationCore.Lattices(Index).AirLoads.CL * q * s
            rbDp.Value = _CalculationCore.Lattices(Index).AirLoads.CDp * q * s
            rbDi.Value = _CalculationCore.Lattices(Index).AirLoads.CDi * q * s

            rbFx.Value = _CalculationCore.Lattices(Index).AirLoads.Force.X * q * s
            rbFy.Value = _CalculationCore.Lattices(Index).AirLoads.Force.Y * q * s
            rbFz.Value = _CalculationCore.Lattices(Index).AirLoads.Force.Z * q * s

            rbMx.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.X * q * s
            rbMy.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.Y * q * s
            rbMz.Value = _CalculationCore.Lattices(Index).AirLoads.Moment.Z * q * s

            pbLoadingGraph.Refresh()

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

    Private GridPen As New Pen(Color.Gainsboro, 1)
    Private MarkerPen As New Pen(Color.Black, 1)
    Private FontLabel As New Font("Segoe UI", 6.5)
    Private SelectedPointIndex As Integer
    Private MousePoint As New PointF(Single.MinValue, Single.MinValue)

    Private Sub DrawLoad(sender As Object, e As PaintEventArgs)

        If _CalculationCore IsNot Nothing Then

            Dim Index As Integer = cbLattices.SelectedIndex

            If Index >= 0 And Index < _CalculationCore.Lattices.Count Then

                If _CalculationCore.Lattices(Index).ChordWiseStripes.Count > 0 Then

                    Dim Stripes As List(Of ChorwiseStripe) = _CalculationCore.Lattices(Index).ChordWiseStripes

                    Dim ymax As Single

                    Dim Data(Stripes.Count - 1) As PointF

                    For i = 0 To Stripes.Count - 1

                        Select Case cbResultType.SelectedIndex

                            Case 0
                                Data(i).Y = Stripes(i).CL

                            Case 1
                                Data(i).Y = Stripes(i).CDp

                            Case 2
                                Data(i).Y = Stripes(i).CDi

                        End Select

                        If i = 0 Then
                            ymax = Data(i).Y
                        Else
                            ymax = Math.Max(ymax, Data(i).Y)
                        End If

                        If i > 0 Then Data(i).X = Data(i - 1).X + Stripes(i - 1).CenterPoint.DistanceTo(Stripes(i).CenterPoint)

                    Next

                    Dim xmax As Single = Data(Stripes.Count - 1).X

                    If xmax > 0 And ymax > 0 Then

                        Dim g As Graphics = e.Graphics

                        g.DrawRectangle(Pens.DarkGray, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)

                        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

                        Dim gO As New PointF(10, 10)
                        Dim gW As Single = pbLoadingGraph.Width - 2 * gO.X
                        Dim gH As Single = pbLoadingGraph.Height - 2 * gO.Y

                        ' Gridlines:

                        Dim ny As Integer = 10

                        For i = 0 To ny

                            Dim y As Single = gO.Y + i * gH / ny
                            g.DrawLine(GridPen, gO.X, y, gO.X + gW, y)

                        Next

                        ' draw grid:

                        Dim nx As Integer = 10

                        Dim x As Single

                        For i = 0 To nx

                            x = gO.X + (i / nx) * gW
                            g.DrawLine(GridPen, x, gO.Y, x, gO.Y + gH)

                        Next

                        Dim pnts(Stripes.Count) As Point

                        ' convert to screen coordinates

                        For i = 0 To Stripes.Count - 1

                            pnts(i).X = gO.X + gW * (Data(i).X / xmax)
                            pnts(i).Y = gO.Y + gH * (1 - Data(i).Y / ymax)

                        Next

                        ' draw lines:

                        For i = 1 To Stripes.Count - 1

                            g.DrawLine(Pens.Blue, pnts(i - 1), pnts(i))

                        Next

                        ' draw points:

                        SelectedPointIndex = -1

                        Dim pnt As Point

                        For i = 0 To Stripes.Count - 1

                            pnt = pnts(i)

                            Dim dmx As Single = pnt.X - MousePoint.X
                            Dim dmy As Single = pnt.Y - MousePoint.Y

                            ' draw marker:

                            If dmx * dmx + dmy * dmy < 9 Then

                                SelectedPointIndex = i

                                g.FillEllipse(Brushes.Orange, pnt.X - 3, pnt.Y - 3, 6, 6)
                                g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                            Else

                                g.FillEllipse(Brushes.White, pnt.X - 3, pnt.Y - 3, 6, 6)
                                g.DrawEllipse(MarkerPen, pnt.X - 3, pnt.Y - 3, 6, 6)

                            End If

                        Next

                        ' draw label on selected point:

                        If SelectedPointIndex >= 0 Then

                            pnt = pnts(SelectedPointIndex)

                            Select Case cbResultType.SelectedIndex

                                Case 0

                                    Dim lblPoint As String = String.Format("d = {0:F3}m / CL = {1:F4}", Data(SelectedPointIndex).X, Data(SelectedPointIndex).Y)
                                    DrawLabel(g, lblPoint, pnt, FontLabel)

                                Case 1

                                    Dim lblPoint As String = String.Format("d = {0:F3}m / CDp = {1:F4}", Data(SelectedPointIndex).X, Data(SelectedPointIndex).Y)
                                    DrawLabel(g, lblPoint, pnt, FontLabel)

                                Case 2

                                    Dim lblPoint As String = String.Format("d = {0:F3}m / CDi = {1:F4}", Data(SelectedPointIndex).X, Data(SelectedPointIndex).Y)
                                    DrawLabel(g, lblPoint, pnt, FontLabel)

                            End Select

                        End If

                    End If

                End If

            End If

        End If

    End Sub

    Private Sub DrawLabel(g As Graphics, Content As String, Point As Point, Font As Font, Optional Leg As Integer = 10, Optional Mrg As Integer = 2)

        Dim lblSize As SizeF = g.MeasureString(Content, Font)

        If Point.X + Leg + Mrg + lblSize.Width < Width Then

            If Point.Y + Leg + lblSize.Height + Mrg < Height Then

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y + Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y + Leg - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y + Leg)

            Else

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y - Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y - Leg - lblSize.Height - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y - Leg - lblSize.Height)

            End If

        Else

            If Point.Y + Leg + lblSize.Height + Mrg < Height Then

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X - Leg, Point.Y + Leg)
                g.FillRectangle(Brushes.Orange, Point.X - Leg - Mrg - lblSize.Width, Point.Y + Leg - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X - Leg - lblSize.Width, Point.Y + Leg)

            Else

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X - Leg, Point.Y - Leg)
                g.FillRectangle(Brushes.Orange, Point.X - Leg - Mrg - lblSize.Width, Point.Y - Leg - lblSize.Height - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X - Leg - lblSize.Width, Point.Y - Leg - lblSize.Height)

            End If

        End If

    End Sub

    Private Sub pgLoadingGraph_MouseMove(sender As Object, e As Windows.Forms.MouseEventArgs)

        MousePoint.X = e.X
        MousePoint.Y = e.Y

        Refresh()

    End Sub


End Class

'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools.VisualModel.Models
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.MathTools.Magnitudes

Public Class AirloadsPanel

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

    Public Sub New()

        InitializeComponent()

        SetUpComponents()

    End Sub

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
        rbCL.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCDi.Name = "CDi"
        rbCDi.Top = rbCL.Bottom + 10
        rbCDi.Left = rbCL.Left
        rbCDi.Parent = Me
        rbCDi.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCDp.Name = "CDp"
        rbCDp.Top = rbCDi.Bottom + 10
        rbCDp.Left = rbCDi.Left
        rbCDp.Parent = Me
        rbCDp.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCFx.Name = "CFx"
        rbCFx.Top = rbCL.Top
        rbCFx.Left = rbCL.Right + 10
        rbCFx.Parent = Me
        rbCFx.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCFy.Name = "CFy"
        rbCFy.Top = rbCFx.Bottom + 10
        rbCFy.Left = rbCFx.Left
        rbCFy.Parent = Me
        rbCFy.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCFz.Name = "CFz"
        rbCFz.Top = rbCFy.Bottom + 10
        rbCFz.Left = rbCFy.Left
        rbCFz.Parent = Me
        rbCFz.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCMx.Name = "CMx"
        rbCMx.Top = rbCFx.Top
        rbCMx.Left = rbCFx.Right + 10
        rbCMx.Parent = Me
        rbCMx.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCMy.Name = "CMy"
        rbCMy.Top = rbCMx.Bottom + 10
        rbCMy.Left = rbCMx.Left
        rbCMy.Parent = Me
        rbCMy.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

        rbCMz.Name = "CMz"
        rbCMz.Top = rbCMy.Bottom + 10
        rbCMz.Left = rbCMy.Left
        rbCMz.Parent = Me
        rbCMz.Decimals = GlobalDecimals(Magnitudes.Dimensionless)

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

    Private Sub LoadResultsData()

        If ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Frame As ResultFrame = ProjectRoot.Results.ActiveFrame

            rbVelocity.Value = Frame.StreamVelocity.EuclideanNorm
            rbDensity.Value = ProjectRoot.Results.Settings.Density
            rbAlpha.Value = Frame.TotalAirLoads.Alfa * 180 / Math.PI
            rbBeta.Value = Frame.TotalAirLoads.Beta * 180 / Math.PI
            cbLattices.Items.Clear()

            If Frame.PartialAirLoads.Count > 0 Then

                cbLattices.Enabled = True
                cbResultType.Enabled = True

                For i = 0 To Frame.PartialAirLoads.Count - 1

                    cbLattices.Items.Add(String.Format("Lattice {0}", i))

                Next

                cbLattices.SelectedIndex = 0
                cbResultType.SelectedIndex = 0

            Else
                cbLattices.Enabled = False
                cbResultType.Enabled = False
            End If

        End If

        pbLoadingGraph.Refresh()

    End Sub

    Private Sub ClearData()

        rbArea.Value = 0.0#

        rbCL.Value = 0.0#
        rbCDp.Value = 0.0#
        rbCDi.Value = 0.0#

        rbCFx.Value = 0.0#
        rbCFy.Value = 0.0#
        rbCFz.Value = 0.0#

        rbCMx.Value = 0.0#
        rbCMy.Value = 0.0#
        rbCMz.Value = 0.0#

        rbL.Value = 0.0#
        rbDp.Value = 0.0#
        rbDi.Value = 0.0#

        rbFx.Value = 0.0#
        rbFy.Value = 0.0#
        rbFz.Value = 0.0#

        rbMx.Value = 0.0#
        rbMy.Value = 0.0#
        rbMz.Value = 0.0#

    End Sub

    Private Sub LoadLattice()

        If ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Frame As ResultFrame = ProjectRoot.Results.ActiveFrame

            Dim Index As Integer = cbLattices.SelectedIndex

            If Index >= 0 And Index < Frame.PartialAirLoads.Count Then

                Dim AirLoad As AirLoads = Frame.PartialAirLoads(Index).AirLoads
                Dim q As Double = AirLoad.DynamicPressure
                Dim S As Double = AirLoad.Area
                Dim qS As Double = q * S
                Dim qSc As Double = qS * AirLoad.Length

                rbArea.Value = S

                rbCL.Value = AirLoad.LiftCoefficient
                rbCDp.Value = AirLoad.SkinDragCoefficient
                rbCDi.Value = AirLoad.InducedDragCoefficient

                rbCFx.Value = AirLoad.Force.X / qS
                rbCFy.Value = AirLoad.Force.Y / qS
                rbCFz.Value = AirLoad.Force.Z / qS

                rbCMx.Value = AirLoad.Moment.X / qSc
                rbCMy.Value = AirLoad.Moment.Y / qSc
                rbCMz.Value = AirLoad.Moment.Z / qSc

                ' Net forces:

                rbL.Value = AirLoad.LiftCoefficient * qS
                rbDp.Value = AirLoad.SkinDragCoefficient * qS
                rbDi.Value = AirLoad.InducedDragCoefficient * qS

                rbFx.Value = AirLoad.Force.X
                rbFy.Value = AirLoad.Force.Y
                rbFz.Value = AirLoad.Force.Z

                rbMx.Value = AirLoad.Moment.X
                rbMy.Value = AirLoad.Moment.Y
                rbMz.Value = AirLoad.Moment.Z

            Else
                ClearData()

            End If

        Else
            ClearData()

        End If

        pbLoadingGraph.Refresh()

    End Sub

    Public Sub LoadResults()

        LoadResultsData()

    End Sub

    Private GridPen As New Pen(Color.Gainsboro, 1)
    Private MarkerPen As New Pen(Color.Black, 1)
    Private FontLabel As New Font("Segoe UI", 6.5)
    Private SelectedPointIndex As Integer
    Private MousePoint As New PointF(Single.MinValue, Single.MinValue)

    Private Sub DrawLoad(sender As Object, e As PaintEventArgs)

        e.Graphics.DrawRectangle(Pens.DarkGray, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)

        If ProjectRoot.Results.ActiveFrame IsNot Nothing Then

            Dim Frame As ResultFrame = ProjectRoot.Results.ActiveFrame

            Dim Index As Integer = cbLattices.SelectedIndex

            If Index >= 0 And Index < Frame.PartialAirLoads.Count Then

                Dim Loads As PartialAirLoads = Frame.PartialAirLoads(Index)

                Dim Vectors As List(Of FixedVector) = Nothing

                Select Case cbResultType.SelectedIndex

                    Case 0
                        Vectors = Loads.LiftVectors

                    Case 1
                        Vectors = Loads.SkinDragVectors

                    Case 2
                        Vectors = Loads.InducedDragVectors

                End Select

                If Vectors.Count > 0 Then

                    Dim ymax As Single

                    Dim Data(Vectors.Count - 1) As PointF

                    For I = 0 To Vectors.Count - 1

                        Data(I).Y = Vectors(I).Vector.EuclideanNorm

                        If I = 0 Then
                            ymax = Data(I).Y
                        Else
                            ymax = Math.Max(ymax, Data(I).Y)
                        End If

                        If I > 0 Then Data(I).X = Data(I - 1).X + Vectors(I - 1).Point.DistanceTo(Vectors(I).Point)

                    Next

                    Dim xmax As Single = Data(Vectors.Count - 1).X

                    If xmax > 0 And ymax > 0 Then

                        Dim g As Graphics = e.Graphics

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

                        Dim pnts(Vectors.Count) As Point

                        ' convert to screen coordinates

                        For i = 0 To Vectors.Count - 1

                            pnts(i).X = gO.X + gW * (Data(i).X / xmax)
                            pnts(i).Y = gO.Y + gH * (1 - Data(i).Y / ymax)

                        Next

                        ' draw lines:

                        For i = 1 To Vectors.Count - 1

                            g.DrawLine(Pens.Blue, pnts(i - 1), pnts(i))

                        Next

                        ' draw points:

                        SelectedPointIndex = -1

                        Dim pnt As Point

                        For i = 0 To Vectors.Count - 1

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

    Private Sub pgLoadingGraph_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs)

        MousePoint.X = e.X
        MousePoint.Y = e.Y

        Refresh()

    End Sub


End Class

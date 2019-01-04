Imports System.Drawing
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Public Class FormCamberLine

    Public Sub New(selectedCamberID As Guid)

        InitializeComponent()

        _SelectedCamberID = selectedCamberID

        RefreshList()

        pbPlot.Refresh()

    End Sub

    Private _SelectedCamberID As Guid = Guid.Empty

    Public ReadOnly Property SelectedCamberID As Guid
        Get
            Return _SelectedCamberID
        End Get
    End Property

    Private Sub RefreshList()

        lbLines.Items.Clear()

        Dim selectedIndex As Integer = -1

        For i = 0 To CamberLinesDatabase.CamberLines.Count - 1

            lbLines.Items.Add(CamberLinesDatabase.CamberLines(i).Name)

            If (CamberLinesDatabase.CamberLines(i).ID.Equals(_SelectedCamberID)) Then selectedIndex = i

        Next

        If (selectedIndex >= 0) Then lbLines.SelectedIndex = selectedIndex

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        Dim newLine As New CamberLine()

        newLine.GenerateNaca(0.02, 0.4, 10)

        newLine.Name = "NACA 2400"

        newLine.ID = Guid.NewGuid()

        CamberLinesDatabase.CamberLines.Add(newLine)

        RefreshList()

    End Sub

    Private Sub lbLines_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lbLines.SelectedIndexChanged

        If lbLines.SelectedIndex >= 0 AndAlso lbLines.SelectedIndex < CamberLinesDatabase.CamberLines.Count Then

            _SelectedCamberID = CamberLinesDatabase.CamberLines(lbLines.SelectedIndex).ID

            tbxCamberName.Text = CamberLinesDatabase.CamberLines(lbLines.SelectedIndex).Name

            SelectedPointIndex = -1

            pbPlot.Refresh()

            Dim allowEdit As Boolean = Not CamberLinesDatabase.CamberLines(lbLines.SelectedIndex).ID.Equals(Guid.Empty)

            btnRemove.Enabled = allowEdit

            tbxCamberName.Enabled = allowEdit

            btnAddNode.Enabled = allowEdit

            btnDelNode.Enabled = allowEdit

            gbxNacaGenerate.Enabled = allowEdit

            gbxNode.Enabled = allowEdit

        End If

    End Sub

    Private Sub tbxCamberName_TextChanged(sender As Object, e As EventArgs) Handles tbxCamberName.TextChanged

        Dim selectedCamber As CamberLine = Nothing

        For Each camber In CamberLinesDatabase.CamberLines

            If (camber.ID.Equals(_SelectedCamberID)) Then

                selectedCamber = camber

                Exit For

            End If

        Next

        If (selectedCamber IsNot Nothing) Then

            selectedCamber.Name = tbxCamberName.Text

        End If

        If lbLines.SelectedIndex > 0 AndAlso lbLines.SelectedIndex < CamberLinesDatabase.CamberLines.Count Then

            lbLines.Items(lbLines.SelectedIndex) = CamberLinesDatabase.CamberLines(lbLines.SelectedIndex).Name

        End If

    End Sub

#Region " Graphic part "

    Private AxisPen As New Pen(Color.DarkGray, 2)

    Private GridPen As New Pen(Color.Gainsboro, 1)

    Private CurvePen As New Pen(Color.Blue, 2)

    Private MarkerPen As New Pen(Color.Black, 1)

    Private FontLabel As New Font("Segoe UI", 6.5)

    Private mX As Integer = 10

    Private mY As Integer = 10

    Private FocusedPointIndex As Integer

    Private DragPoint As Boolean = False

    Private Sub DrawCurve(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles pbPlot.Paint

        Dim selectedCamber As CamberLine = Nothing

        For Each camber In CamberLinesDatabase.CamberLines

            If (camber.ID.Equals(_SelectedCamberID)) Then

                selectedCamber = camber

                Exit For

            End If

        Next

        If selectedCamber IsNot Nothing Then

            Dim g As Graphics = e.Graphics

            g.DrawRectangle(Pens.DarkGray, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            Dim gW As Integer = pbPlot.Width - 2 * mX

            Dim gH As Integer = pbPlot.Height - 2 * mY

            ' Gridlines:

            Dim ny As Integer = 10

            For i = 1 To ny - 1

                Dim y As Single = mY + i * gH / ny

                g.DrawLine(GridPen, mX, y, mX + gW, y)

            Next

            g.DrawLine(AxisPen, mX, mY + gH, mX + gW, mY + gH)

            g.DrawLine(AxisPen, mX, mY, mX, mY + gH)

            g.DrawLine(AxisPen, mX + gW, mY, mX + gW, mY + gH)

            ' Function:

            Dim np As Integer = 30

            Dim ymax As Double = 1

            Dim ymin As Double = 0

            Dim xmin As Double = -1

            Dim xmax As Double = 1

            If selectedCamber.Nodes.Count > 0 Then

                ymax = selectedCamber.Nodes(0).Y

                ymin = selectedCamber.Nodes(0).Y

                xmin = selectedCamber.Nodes(0).X

                xmax = selectedCamber.Nodes(0).X

                For i = 0 To selectedCamber.Nodes.Count - 1

                    ymin = Math.Min(ymin, selectedCamber.Nodes(i).Y)

                    ymax = Math.Max(ymax, selectedCamber.Nodes(i).Y)

                    xmin = Math.Min(xmin, selectedCamber.Nodes(i).X)

                    xmax = Math.Max(xmax, selectedCamber.Nodes(i).X)

                Next

                Dim dx As Double = xmax - xmin

                Dim dy As Double = ymax - ymin

                If dx > 0 Then

                    Dim scale As Single = gW / dx

                    If dy > 0 Then

                        scale = Math.Min(gW / dx, gH / dy)

                    End If

                    ' update position of selected point:

                    If DragPoint AndAlso FocusedPointIndex >= 0 AndAlso FocusedPointIndex < selectedCamber.Nodes.Count Then

                        selectedCamber.Nodes(FocusedPointIndex).X = Math.Max(0, Math.Min(1, (MousePoint.X - mX) / scale + xmin))
                        selectedCamber.Nodes(FocusedPointIndex).Y = ((mY + gH) - MousePoint.Y) / scale + ymin

                        LockEvents = True

                        nudX.Value = selectedCamber.Nodes(FocusedPointIndex).X

                        nudY.Value = selectedCamber.Nodes(FocusedPointIndex).Y

                        LockEvents = False

                    End If

                    ' draw grid:

                    Dim nx As Integer = 10
                    Dim x As Single

                    For i = 1 To nx - 1

                        x = mX + ((gW + Math.Round(xmin * nx * scale)) * i / nx - xmin * scale)
                        g.DrawLine(GridPen, x, mY, x, mY + gH)

                    Next

                    x = CSng(mX + Math.Abs(xmin) * scale)
                    g.DrawLine(AxisPen, x, mY, x, mY + gH)
                    g.DrawLine(AxisPen, mX, mY, mX + gW, mY)

                    Dim pnts(selectedCamber.Nodes.Count) As Point

                    ' convert to screen coordinates

                    For i = 0 To selectedCamber.Nodes.Count - 1

                        pnts(i).X = mX + (selectedCamber.Nodes(i).X - xmin) * scale
                        pnts(i).Y = mY + (gH - (selectedCamber.Nodes(i).Y - ymin) * scale)

                    Next

                    ' draw lines:

                    For i = 1 To selectedCamber.Nodes.Count - 1

                        g.DrawLine(CurvePen, pnts(i - 1), pnts(i))

                    Next

                    ' draw points:

                    FocusedPointIndex = -1
                    Dim pnt As Point

                    For i = 0 To selectedCamber.Nodes.Count - 1

                        pnt = pnts(i)

                        Dim dmx As Single = pnt.X - MousePoint.X
                        Dim dmy As Single = pnt.Y - MousePoint.Y

                        ' draw marker:

                        If dmx * dmx + dmy * dmy < 9 Then

                            FocusedPointIndex = i

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

                        Dim lblPoint As String = String.Format("({0:F2}; {1:F2})", selectedCamber.Nodes(SelectedPointIndex).X, selectedCamber.Nodes(SelectedPointIndex).Y)

                        DrawLabel(g, lblPoint, pnt, FontLabel)

                    End If

                End If

            End If

        End If

    End Sub

    Private Sub DrawLabel(g As Graphics, Content As String, Point As Point, Font As Font, Optional Leg As Integer = 10, Optional Mrg As Integer = 2)

        Dim lblSize As SizeF = g.MeasureString(Content, Font)

        If Point.X + Leg + Mrg + lblSize.Width < pbPlot.Width Then

            If Point.Y + Leg + lblSize.Height + Mrg < pbPlot.Height Then

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y + Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y + Leg - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y + Leg)

            Else

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y - Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y - Leg - lblSize.Height - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y - Leg - lblSize.Height)

            End If

        Else

            If Point.Y + Leg + lblSize.Height + Mrg < pbPlot.Height Then

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

    Private MousePoint As New PointF(Single.MinValue, Single.MinValue)

    Private SelectedPointIndex As Integer = -1

    Private Sub PolarPlotter_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pbPlot.MouseDown

        DragPoint = True

        MousePoint.X = e.X
        MousePoint.Y = e.Y

        pbPlot.Refresh()

        SelectedPointIndex = FocusedPointIndex

    End Sub

    Private Sub PolarPlotter_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pbPlot.MouseMove

        MousePoint.X = e.X
        MousePoint.Y = e.Y

        pbPlot.Refresh()

    End Sub

    Private Sub PolarPlotter_MouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles pbPlot.MouseUp

        DragPoint = False

        pbPlot.Refresh()

    End Sub

    Private Sub btnAddNode_Click(sender As Object, e As EventArgs) Handles btnAddNode.Click

        Dim selectedCamber As CamberLine = Nothing

        For Each camber In CamberLinesDatabase.CamberLines

            If (camber.ID.Equals(_SelectedCamberID)) Then
                selectedCamber = camber
                Exit For
            End If

        Next

        If (selectedCamber IsNot Nothing) Then

            If SelectedPointIndex > 0 AndAlso SelectedPointIndex < selectedCamber.Nodes.Count - 1 Then

                Dim x As Double = 0.5 * (selectedCamber.Nodes(SelectedPointIndex).X + selectedCamber.Nodes(SelectedPointIndex + 1).X)
                Dim y As Double = 0.5 * (selectedCamber.Nodes(SelectedPointIndex).Y + selectedCamber.Nodes(SelectedPointIndex + 1).Y)

                Dim newNode As New EVector2(x, y)

                selectedCamber.Nodes.Insert(SelectedPointIndex, newNode)

                SelectedPointIndex = -1

                pbPlot.Refresh()

            End If

        End If

    End Sub

    Private Sub btnDelNode_Click(sender As Object, e As EventArgs) Handles btnDelNode.Click

        Dim selectedCamber As CamberLine = Nothing

        For Each camber In CamberLinesDatabase.CamberLines

            If (camber.ID.Equals(_SelectedCamberID)) Then
                selectedCamber = camber
                Exit For
            End If

        Next

        If (selectedCamber IsNot Nothing) Then

            If SelectedPointIndex > 0 AndAlso SelectedPointIndex < selectedCamber.Nodes.Count - 1 Then

                selectedCamber.Nodes.RemoveAt(SelectedPointIndex)

                SelectedPointIndex = -1

                pbPlot.Refresh()

            End If

        End If

    End Sub

    Private Sub btnGenerateNaca_Click(sender As Object, e As EventArgs) Handles btnGenerateNaca.Click

        SelectedPointIndex = -1

        Dim selectedCamber As CamberLine = Nothing

        For Each camber In CamberLinesDatabase.CamberLines

            If (camber.ID.Equals(_SelectedCamberID)) Then
                selectedCamber = camber
                Exit For
            End If

        Next

        If (selectedCamber IsNot Nothing) Then

            selectedCamber.GenerateNaca(nudMaxCamber.Value, nudXmax.Value)

            pbPlot.Refresh()

        End If

    End Sub

    Private LockEvents As Boolean = True

    Private Sub nudX_ValueChanged(sender As Object, e As EventArgs) Handles nudX.ValueChanged

        UpdateCoordinates()

    End Sub

    Private Sub nudY_ValueChanged(sender As Object, e As EventArgs) Handles nudY.ValueChanged

        UpdateCoordinates()

    End Sub

    Private Sub UpdateCoordinates()

        If Not LockEvents Then

            Dim selectedCamber As CamberLine = Nothing

            For Each camber In CamberLinesDatabase.CamberLines

                If (camber.ID.Equals(_SelectedCamberID)) Then
                    selectedCamber = camber
                    Exit For
                End If

            Next

            If (selectedCamber IsNot Nothing) Then

                If SelectedPointIndex > 0 AndAlso SelectedPointIndex < selectedCamber.Nodes.Count - 1 Then

                    selectedCamber.Nodes(SelectedPointIndex).X = nudX.Value

                    selectedCamber.Nodes(SelectedPointIndex).Y = nudY.Value

                    pbPlot.Refresh()

                End If

            End If

        End If

    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        CamberLinesDatabase.RemoveCamberLine(_SelectedCamberID)

        RefreshList()

    End Sub

#End Region

End Class
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
Imports System.Drawing
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports System.Windows.Forms
Imports OpenVOGEL.MathTools

Public Class FormFuselageEditor

    Private _Fuselage As Fuselage
    Private _LiftingSurfaces As List(Of LiftingSurface)

    Public Sub New(ByRef Fuselage As Fuselage, ByRef LiftingSurfaces As List(Of LiftingSurface))

        DoubleBuffered = True
        InitializeComponent()
        SetUpControls()

        _Fuselage = Fuselage
        _LiftingSurfaces = LiftingSurfaces

        LoadSections()
        CurrentSectionIndex = 0
        ObtainGlobalExtremeCoordinates()
        LoadLiftingSurfaces()
        UpdateGeometricParameters()

        nudNPS.Value = _Fuselage.CrossRefinement
        nudNPZ.Value = _Fuselage.LongitudinalRefinement

        tbName.DataBindings.Add("Text", Fuselage, "Name")

    End Sub

    Private Sub SetUpControls()

        nudPosition.DecimalPlaces = 3
        nudPosition.Minimum = -10000000
        nudPosition.Maximum = 10000000

    End Sub

    Private Sub LoadSections()

        lbSections.Items.Clear()

        Dim index As Integer = 0
        For Each Section In _Fuselage.CrossSections
            lbSections.Items.Add(String.Format("Section {0}", index))
            index += 1
        Next

    End Sub

    Private Property CurrentSection As CrossSection
        Get
            If lbSections.SelectedIndex >= 0 And lbSections.SelectedIndex < _Fuselage.CrossSections.Count Then
                Return _Fuselage.CrossSections(lbSections.SelectedIndex)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As CrossSection)
            If lbSections.SelectedIndex >= 0 And lbSections.SelectedIndex < _Fuselage.CrossSections.Count Then
                _Fuselage.CrossSections(lbSections.SelectedIndex) = value
            End If
        End Set
    End Property

    Private Property CurrentSectionIndex As Integer
        Get
            Return lbSections.SelectedIndex
        End Get
        Set(ByVal value As Integer)
            If value >= 0 And value < lbSections.Items.Count Then lbSections.SelectedIndex = value
        End Set
    End Property

    Private Sub SelectSection() Handles lbSections.SelectedIndexChanged
        _SelectedVertexIndex = -1
        CoordinateControlsEnabled = False
        AllowSectionEvents = False
        If CurrentSection IsNot Nothing Then
            nudPosition.Value = CurrentSection.Z
            cbxBrokenEdge.Checked = CurrentSection.BrokenEdge
        End If
        AllowSectionEvents = True
        Refresh()
    End Sub

    Private Enum CrossSectionStyle As Byte
        Active = 0
        Inactive = 1
    End Enum

    Private _scale As Single
    Private _mcenter As New Vector2
    Private _gcenter As PointF

    Private Sub UpdateGeometricParameters()

        Dim mW As Single = Math.Max(0.1, _URg.X - _BLg.X)
        Dim mH As Single = Math.Max(0.1, _URg.Y - _BLg.Y)

        _scale = 0.75 * Math.Min(pbSections.Height / mH, pbSections.Width / mW)
        _mcenter.SetCoordinates(_BLg.X, 0.5 * (_BLg.Y + _URg.Y))
        _gcenter.X = 0.5 * pbSections.Width
        _gcenter.Y = 0.5 * pbSections.Height
        panningDisplacement.X = 0
        panningDisplacement.Y = 0

    End Sub

    Private Function GetPoint(ByVal x As Double, ByVal y As Double) As PointF
        Dim p As PointF
        p.X = _scale * (x - _mcenter.X) + _gcenter.X + panningDisplacement.X
        p.Y = -_scale * (y - _mcenter.Y) + _gcenter.Y + panningDisplacement.Y
        Return p
    End Function

    Private Function GetPoint(ByVal v As Vector2) As PointF
        Dim p As PointF
        p.X = _scale * (v.X - _mcenter.X) + _gcenter.X + panningDisplacement.X
        p.Y = -_scale * (v.Y - _mcenter.Y) + _gcenter.Y + panningDisplacement.Y
        Return p
    End Function

    Private _URg As New Vector2
    Private _BLg As New Vector2
    Private _Zlimits As New LimitValues

    Public Sub ObtainGlobalExtremeCoordinates()

        Dim firstSection As Boolean = True
        Dim firstVertex As Boolean = True

        _URg.X = 0
        _URg.Y = 0
        _BLg.X = 0
        _BLg.Y = 0
        _Zlimits.Minimum = 0
        _Zlimits.Maximum = 0

        For Each Section In _Fuselage.CrossSections

            If firstSection Then
                _Zlimits.Minimum = Section.Z
                _Zlimits.Maximum = Section.Z
                firstSection = False
            Else
                _Zlimits.Minimum = Math.Min(_Zlimits.Minimum, Section.Z)
                _Zlimits.Maximum = Math.Max(_Zlimits.Maximum, Section.Z)
            End If

            For Each Vertex In Section.Vertices
                If firstVertex Then
                    _URg.X = Vertex.X
                    _URg.Y = Vertex.Y
                    _BLg.X = Vertex.X
                    _BLg.Y = Vertex.Y
                    firstVertex = False
                Else
                    _URg.X = Math.Max(_URg.X, Vertex.X)
                    _URg.Y = Math.Max(_URg.Y, Vertex.Y)
                    _BLg.X = Math.Min(_BLg.X, Vertex.X)
                    _BLg.Y = Math.Min(_BLg.Y, Vertex.Y)
                End If
            Next

        Next

    End Sub

    Private _URl As New Vector2
    Private _BLl As New Vector2

    Public Sub ObtainExtremeCoordinates(ByVal Section As CrossSection)

        Dim firstone As Boolean = True

        _URl.X = 0
        _URl.Y = 0
        _BLl.X = 0
        _BLl.Y = 0

        If Not IsNothing(Section) Then

            For Each Vertex In Section.Vertices
                If firstone Then
                    _URl.X = Vertex.X
                    _URl.Y = Vertex.Y
                    _BLl.X = Vertex.X
                    _BLl.Y = Vertex.Y
                    firstone = False
                Else
                    _URl.X = Math.Max(_URl.X, Vertex.X)
                    _URl.Y = Math.Max(_URl.Y, Vertex.Y)
                    _BLl.X = Math.Min(_BLl.X, Vertex.X)
                    _BLl.Y = Math.Min(_BLl.Y, Vertex.Y)
                End If
            Next

        End If

    End Sub

    Private Sub pbSections_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pbSections.Paint

        Dim g As Graphics = e.Graphics

        g.FillRectangle(DrawingBackground, e.ClipRectangle)
        g.DrawRectangle(Pens.DarkGray, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        ' Draw axes:

        Dim ori As PointF = GetPoint(0, 0)

        XaxisPen.EndCap = Drawing2D.LineCap.ArrowAnchor
        YaxisPen.EndCap = Drawing2D.LineCap.ArrowAnchor

        g.DrawLine(XaxisPen, ori.X, ori.Y, ori.X + AXIS_LENGHT, ori.Y)
        g.DrawLine(YaxisPen, ori.X, ori.Y, ori.X, ori.Y - AXIS_LENGHT)

        g.FillEllipse(Brushes.White, ori.X - 2, ori.Y - 2, 4, 4)
        g.DrawEllipse(MarkerPen, ori.X - 2, ori.Y - 2, 4, 4)

        ' Draw sections:

        Dim style As CrossSectionStyle = CrossSectionStyle.Inactive

        For i = 0 To _Fuselage.CrossSections.Count - 1
            If i <> CurrentSectionIndex Then
                DrawSection(CrossSectionStyle.Inactive, _Fuselage.CrossSections(i), g)
            End If
        Next

        DrawSection(CrossSectionStyle.Active, CurrentSection, g)

    End Sub

    Private MarkerPen As New Pen(Color.Black, 1)
    Private FontLabel As New Font("Consolas", 8.0)

    Private Sub DrawSection(ByVal Style As CrossSectionStyle, ByVal Section As CrossSection, ByVal g As Graphics)

        ObtainExtremeCoordinates(Section)

        If Not IsNothing(CurrentSection) Then

            Dim po As PointF
            Dim pf As PointF

            Dim pen1 As Pen = Nothing
            Dim pen2 As Pen = Nothing
            Dim brush1 As Brush = Nothing
            Dim brush2 As Brush = Nothing

            Select Case Style
                Case CrossSectionStyle.Active
                    pen1 = New Pen(Brushes.Black, 3)
                    pen2 = New Pen(Brushes.Gray, 3)
                    brush1 = Brushes.Red
                    brush2 = Brushes.Orange
                Case CrossSectionStyle.Inactive
                    pen1 = New Pen(Brushes.LightGray, 2)
                    pen2 = New Pen(Brushes.LightGray, 2)
                    brush1 = Brushes.LightGray
                    brush2 = Brushes.LightGray
                Case Else
                    pen1 = New Pen(Brushes.LightGray, 2)
                    pen2 = New Pen(Brushes.LightGray, 2)
                    brush1 = Brushes.LightGray
                    brush2 = Brushes.LightGray
            End Select

            pen1.StartCap = Drawing2D.LineCap.Round
            pen1.EndCap = Drawing2D.LineCap.Round
            pen2.StartCap = Drawing2D.LineCap.Round
            pen2.EndCap = Drawing2D.LineCap.Round

            For i = 1 To Section.Vertices.Count - 1

                pf = GetPoint(Section.Vertices(i))
                po = GetPoint(Section.Vertices(i - 1))
                g.DrawLine(pen1, po, pf)

                pf = GetPoint(-Section.Vertices(i).X, Section.Vertices(i).Y)
                po = GetPoint(-Section.Vertices(i - 1).X, Section.Vertices(i - 1).Y)
                g.DrawLine(pen2, po, pf)

            Next

            If Style = CrossSectionStyle.Active Then

                Dim ps As PointF
                Dim content As String = ""

                For i = 0 To Section.Vertices.Count - 1

                    po = GetPoint(Section.Vertices(i))

                    If i = _SelectedVertexIndex Then

                        g.FillEllipse(Brushes.Orange, po.X - 3, po.Y - 3, 6, 6)
                        g.DrawEllipse(MarkerPen, po.X - 3, po.Y - 3, 6, 6)

                        content = String.Format("{0:D}: [{1:F3}; {2:F3}]", i + 1, Section.Vertices(i).X, Section.Vertices(i).Y)
                        ps = po

                    Else

                        g.FillEllipse(Brushes.White, po.X - 3, po.Y - 3, 6, 6)
                        g.DrawEllipse(MarkerPen, po.X - 3, po.Y - 3, 6, 6)

                    End If

                Next

                If content <> "" Then

                    DrawLabel(g, content, ps, FontLabel)

                End If

            End If

        End If

    End Sub

    Private Sub DrawLabel(g As Graphics, Content As String, Point As PointF, Font As Font, Optional Leg As Integer = 10, Optional Mrg As Integer = 2)

        Dim lblSize As SizeF = g.MeasureString(Content, Font)

        If Point.X + Leg + Mrg + lblSize.Width < pbSections.Width Then

            If Point.Y + Leg + lblSize.Height + Mrg < pbSections.Height Then

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y + Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y + Leg - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y + Leg)

            Else

                g.DrawLine(Pens.Orange, Point.X, Point.Y, Point.X + Leg, Point.Y - Leg)
                g.FillRectangle(Brushes.Orange, Point.X + Leg - Mrg, Point.Y - Leg - lblSize.Height - Mrg, lblSize.Width + 2 * Mrg, lblSize.Height + 2 * Mrg)
                g.DrawString(Content, FontLabel, Brushes.Maroon, Point.X + Leg, Point.Y - Leg - lblSize.Height)

            End If

        Else

            If Point.Y + Leg + lblSize.Height + Mrg < pbSections.Height Then

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

    Private panning As Boolean = False
    Private panningAnkor As PointF
    Private panningDisplacement As PointF

    Private Sub pbSections_MouseMove(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseMove
        If panning Then
            panningDisplacement.X = e.X - panningAnkor.X
            panningDisplacement.Y = e.Y - panningAnkor.Y
            Refresh()
        End If
    End Sub

    Private Sub pbSections_MouseDown(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseDown
        If e.Button = MouseButtons.Middle Then
            panning = True
            panningAnkor.X = e.X - panningDisplacement.X
            panningAnkor.Y = e.Y - panningDisplacement.Y
        End If
        Refresh()
    End Sub

    Public Sub pbSections_MouseUp(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseUp

        panning = False

        ObtainGlobalExtremeCoordinates()

        Refresh()

    End Sub

    Private Sub pbSections_MouseHover(ByVal s As Object, ByVal e As EventArgs) Handles pbSections.MouseHover
        pbSections.Focus()
    End Sub

    Private Sub pbSections_MouseWheel(ByVal s As Object, ByVal e As MouseEventArgs) Handles Me.MouseWheel
        If pbSections.Focused Then
            _scale = Math.Max(0, _scale + e.Delta)
            Refresh()
        End If
    End Sub

    Private _SelectedVertexIndex As Integer = -1

    Private Sub SelectVertex(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseDown

        If e.Button = MouseButtons.Left Then

            If CurrentSection IsNot Nothing Then

                _SelectedVertexIndex = -1

                ObtainExtremeCoordinates(CurrentSection)

                For i = 0 To CurrentSection.Vertices.Count - 1
                    Dim p As PointF = GetPoint(CurrentSection.Vertices(i))
                    Dim dx As Single = p.X - e.X
                    Dim dy As Single = p.Y - e.Y
                    If dx * dx + dy * dy < 25 Then
                        _SelectedVertexIndex = i
                        movingVertex = True
                        vertexDrawingAnkor.X = e.X
                        vertexDrawingAnkor.Y = e.Y
                        vertexModelAnkor.X = CurrentSection.Vertices(i).X
                        vertexModelAnkor.Y = CurrentSection.Vertices(i).Y

                        ' Enable controls:

                        CoordinateControlsEnabled = True

                        UpdateControlValues()

                        Exit For
                    End If
                Next

                If _SelectedVertexIndex < 0 Then

                    ' Disable controls:

                    CoordinateControlsEnabled = False

                End If

                Refresh()

            End If

        End If

    End Sub

    Private movingVertex As Boolean = False
    Private vertexDrawingAnkor As PointF
    Private vertexDisplacement As PointF
    Private vertexModelAnkor As New Vector2

    Private Sub MoveSelectedVertex(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseMove

        If movingVertex Then

            vertexDisplacement.X = e.X - vertexDrawingAnkor.X
            vertexDisplacement.Y = vertexDrawingAnkor.Y - e.Y

            If _SelectedVertexIndex >= 0 And _SelectedVertexIndex < CurrentSection.Vertices.Count Then

                ' Update vertex position:

                CurrentSection.Vertices(_SelectedVertexIndex).X = Math.Max(0, vertexModelAnkor.X + vertexDisplacement.X / _scale)
                CurrentSection.Vertices(_SelectedVertexIndex).Y = vertexModelAnkor.Y + vertexDisplacement.Y / _scale
                CurrentSection.CalculatePerimeter()

                ' Update control values:

                UpdateControlValues()

            End If

            Refresh()

        End If

    End Sub

    Public Sub StopMovingVertex(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseUp

        movingVertex = False

    End Sub

    Private Sub AddPoint(ByVal s As Object, ByVal e As EventArgs) Handles btnAddPoint.Click

        If _SelectedVertexIndex >= 0 And _SelectedVertexIndex < CurrentSection.Vertices.Count - 1 Then

            Dim newVertex As New Vector2
            newVertex.X = 0.5 * (CurrentSection.Vertices(_SelectedVertexIndex).X + CurrentSection.Vertices(_SelectedVertexIndex + 1).X)
            newVertex.Y = 0.5 * (CurrentSection.Vertices(_SelectedVertexIndex).Y + CurrentSection.Vertices(_SelectedVertexIndex + 1).Y)

            CurrentSection.Vertices.Insert(_SelectedVertexIndex + 1, newVertex)

            Refresh()

        Else

            Dim newVertex As New Vector2

            If CurrentSection.Vertices.Count > 0 Then
                newVertex.Y = 0.1
            End If

            CurrentSection.Vertices.Add(newVertex)

            _SelectedVertexIndex = 0

            ObtainGlobalExtremeCoordinates()

            UpdateGeometricParameters()

            Refresh()

        End If

    End Sub

    Private Sub RemovePoint(ByVal s As Object, ByVal e As EventArgs) Handles btnRemovePoint.Click

        If _SelectedVertexIndex > 0 And _SelectedVertexIndex < CurrentSection.Vertices.Count Then

            CoordinateControlsEnabled = False

            CurrentSection.Vertices.RemoveAt(_SelectedVertexIndex)

            Refresh()

        End If

    End Sub

    Private Sub RemoveSection(ByVal s As Object, ByVal e As EventArgs) Handles btnRemoveSection.Click

        If (CurrentSectionIndex > -1 And CurrentSectionIndex < _Fuselage.CrossSections.Count) Then

            CoordinateControlsEnabled = False

            _Fuselage.CrossSections.RemoveAt(CurrentSectionIndex)
            LoadSections()
            ObtainGlobalExtremeCoordinates()

        End If

    End Sub

    Private Sub AddSection(ByVal s As Object, ByVal e As EventArgs) Handles btnAddSection.Click

        Dim SectionA As CrossSection = Nothing
        Dim SectionB As CrossSection = Nothing

        If CurrentSectionIndex >= 0 And CurrentSectionIndex < _Fuselage.CrossSections.Count Then
            SectionA = CurrentSection
        End If
        If CurrentSectionIndex < _Fuselage.CrossSections.Count - 1 Then
            SectionB = _Fuselage.CrossSections(CurrentSectionIndex + 1)
        End If

        Dim Section As New CrossSection

        If (Not IsNothing(SectionA)) And (Not IsNothing(SectionB)) Then
            Section.Z = 0.5 * (SectionA.Z + SectionB.Z)
        ElseIf Not IsNothing(SectionA) Then
            Section.Z = SectionA.Z + 1
        ElseIf Not IsNothing(SectionB) Then
            Section.Z = SectionB.Z + 1
        End If

        For i = 0 To 10

            Dim c As Double = i / 10

            Dim p1 As Vector2 = Nothing
            If Not IsNothing(SectionA) Then p1 = SectionA.GetPoint(c)

            Dim p2 As Vector2 = Nothing
            If Not IsNothing(SectionB) Then p2 = SectionB.GetPoint(c)

            If (Not IsNothing(p1)) And (Not IsNothing(p2)) Then
                Dim p As New Vector2
                p.X = 0.5 * (p1.X + p2.X)
                p.Y = 0.5 * (p1.Y + p2.Y)
                Section.Vertices.Add(p)
            ElseIf Not IsNothing(p1) Then
                Section.Vertices.Add(p1)
            ElseIf Not IsNothing(p2) Then
                Section.Vertices.Add(p2)
            End If

        Next

        Section.CalculatePerimeter()

        _Fuselage.CrossSections.Insert(CurrentSectionIndex + 1, Section)

        LoadSections()

        ObtainGlobalExtremeCoordinates()

    End Sub

    Private WriteOnly Property CoordinateControlsEnabled
        Set(ByVal value)
            If value = False Then
                nudX.Value = 0
                nudY.Value = 0
            End If
            nudX.Enabled = value
            nudY.Enabled = value
        End Set
    End Property

    Private Sub UpdateXCoordinate(ByVal s As Object, ByVal e As EventArgs) Handles nudX.ValueChanged

        If Not movingVertex Then

            If _SelectedVertexIndex >= 0 And _SelectedVertexIndex < CurrentSection.Vertices.Count Then

                ' Update vertex position:

                CurrentSection.Vertices(_SelectedVertexIndex).X = nudX.Value
                CurrentSection.CalculatePerimeter()

            End If

            Refresh()

        End If

    End Sub

    Private Sub UpdateYCoordinate(ByVal s As Object, ByVal e As EventArgs) Handles nudY.ValueChanged

        If Not movingVertex Then

            If _SelectedVertexIndex >= 0 And _SelectedVertexIndex < CurrentSection.Vertices.Count Then

                ' Update vertex position:

                CurrentSection.Vertices(_SelectedVertexIndex).Y = nudY.Value
                CurrentSection.CalculatePerimeter()

            End If

            Refresh()

        End If

    End Sub

    Private Sub UpdateControlValues()

        nudX.Value = CurrentSection.Vertices(_SelectedVertexIndex).X
        nudY.Value = CurrentSection.Vertices(_SelectedVertexIndex).Y

    End Sub

#Region " Anchors to lifting surfaces "

    Private Sub LoadLiftingSurfaces()

        clbWingsToAnchor.Items.Clear()

        For i = 0 To _LiftingSurfaces.Count - 1

            Dim Name As String = _LiftingSurfaces(i).Name
            If IsNothing(Name) OrElse Name = "" Then Name = String.Format("Lifting surface {0}", i)
            clbWingsToAnchor.Items.Add(Name)

            clbWingsToAnchor.SetItemChecked(i, False)

            For Each AnchLine In _Fuselage.AnchorLines

                If Not IsNothing(AnchLine.WingAnchorInfo) Then
                    If AnchLine.WingAnchorInfo.ParentID = _LiftingSurfaces(i).ID Then
                        clbWingsToAnchor.SetItemChecked(i, True)
                        Exit For
                    End If
                End If

            Next

        Next

    End Sub

    Private Sub LoadWingAnchorsToBody()

        _Fuselage.AnchorLines.Clear()

        For i = 0 To _LiftingSurfaces.Count - 1

            If clbWingsToAnchor.GetItemChecked(i) Then

                Dim AnchorLine As New AnchorLine

                Dim n As Integer = _LiftingSurfaces(i).NumberOfChordPanels

                For j = 0 To n

                    Dim Line As New Line3()

                    Line.Point.Z = _LiftingSurfaces(i).Mesh.Nodes(j).Position.X
                    Line.Point.Y = _LiftingSurfaces(i).Mesh.Nodes(j).Position.Z
                    Line.Point.X = _LiftingSurfaces(i).Mesh.Nodes(j).Position.Y

                    Dim pa As Vector3 = _LiftingSurfaces(i).Mesh.Nodes(j).Position
                    Dim pb As Vector3 = _LiftingSurfaces(i).Mesh.Nodes(j + n + 1).Position

                    Line.Direction.X = pa.Y - pb.Y
                    Line.Direction.Y = pa.Z - pb.Z
                    Line.Direction.Z = pa.X - pb.X
                    Line.Direction.Normalize()

                    AnchorLine.Lines.Add(Line)

                Next

                Dim Info As New WingAnchorInfo

                Info.ParentID = _LiftingSurfaces(i).ID

                AnchorLine.WingAnchorInfo = Info

                _Fuselage.AnchorLines.Add(AnchorLine)

            End If

        Next

    End Sub

#End Region
    Private Sub nudNPS_ValueChanged(sender As Object, e As EventArgs) Handles nudNPS.ValueChanged
        _Fuselage.CrossRefinement = nudNPS.Value
    End Sub

    Private Sub nudNPZ_ValueChanged(sender As Object, e As EventArgs) Handles nudNPZ.ValueChanged
        _Fuselage.LongitudinalRefinement = nudNPZ.Value
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        _Fuselage.CrossSections.Sort()

        LoadWingAnchorsToBody()

    End Sub

    Private DashedPen As New Pen(Color.Gray, 1)
    Private XaxisPen As New Pen(Color.Green, 2)
    Private YaxisPen As New Pen(Color.Red, 2)
    Private ZaxisPen As New Pen(Color.Magenta, 2)
    Private Const AXIS_LENGHT As Single = 25
    Private SelectedLinePen As New Pen(Color.Black, 2)
    Private DrawingBackground As New SolidBrush(Color.FromArgb(240, 240, 255))

    Private Sub pbSideView_Paint(sender As Object, e As PaintEventArgs) Handles pbSideView.Paint

        DashedPen.DashPattern = {5, 2}

        ' Draw here a simple sketch of the side view of the sections

        Dim g As Graphics = e.Graphics

        g.FillRectangle(DrawingBackground, e.ClipRectangle)
        g.DrawRectangle(Pens.DarkGray, 0, 0, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)

        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality

        ' margin:

        Dim mx As Single = 15
        Dim my As Single = 15

        ' origin:

        Dim ox As Single = 0.5 * pbSideView.Width
        Dim oy As Single = 0.5 * pbSideView.Height

        ' center coordinates of the model:

        Dim xmean As Single = 0.5 * (_Zlimits.Minimum + _Zlimits.Maximum)
        Dim ymean As Single = 0.5 * (_URg.Y + _BLg.Y)

        ' lenght and height of the model:

        Dim dx As Single = _Zlimits.Maximum - _Zlimits.Minimum
        Dim dy As Single = _URg.Y - _BLl.Y

        ' width and height of the graph:

        Dim mW As Single = pbSideView.Width - 2 * mx
        Dim mH As Single = pbSideView.Height - 2 * my

        ' scale of the graph:

        If dx > 0 And dy > 0 Then

            Dim scale As Single = Math.Min(mW / dx, mH / dy)

            ' Draw axes:

            Dim z As Single = ox - xmean * scale
            Dim y As Single = oy + ymean * scale

            ZaxisPen.EndCap = Drawing2D.LineCap.ArrowAnchor
            YaxisPen.EndCap = Drawing2D.LineCap.ArrowAnchor

            g.DrawLine(ZaxisPen, z, y, z + AXIS_LENGHT, y)
            g.DrawLine(YaxisPen, z, y, z, y - AXIS_LENGHT)

            g.FillEllipse(Brushes.White, z - 2, y - 2, 4, 4)
            g.DrawEllipse(MarkerPen, z - 2, y - 2, 4, 4)

            ' backward values

            Dim y0_m1 As Single = 0
            Dim y1_m1 As Single = 0
            Dim z_m1 As Single = 0

            Dim first As Boolean = True
            Dim i As Integer = 0

            'Dim UpperPoints As New List(Of Vector3)
            'Dim LowerPoints As New List(Of Vector3)
            'Dim Z0 As Double = _Fuselage.CrossSections.First.Z
            'Dim Z1 As Double = _Fuselage.CrossSections.Last.Z

            'For i = 0 To 100
            'UpperPoints.Add(_Fuselage.GetPoint(Z0 + (Z1 - Z0) * i / 100, 1))
            'LowerPoints.Add(_Fuselage.GetPoint(Z0 + (Z1 - Z0) * i / 100, 0))
            'Next

            For Each Section In _Fuselage.CrossSections

                z = ox + (Section.Z - xmean) * scale

                If Section.Vertices.Count > 0 Then

                    Dim ymax_v As Single = Section.Vertices(0).Y
                    Dim ymin_v As Single = Section.Vertices(0).Y

                    For Each Vertex In Section.Vertices

                        ymax_v = Math.Max(ymax_v, Vertex.Y)
                        ymin_v = Math.Min(ymin_v, Vertex.Y)

                    Next

                    Dim y0 As Single = oy + (ymean - ymax_v) * scale
                    Dim y1 As Single = oy + (ymean - ymin_v) * scale

                    If i = lbSections.SelectedIndex Then

                        ' draw the Z coordinate

                        g.DrawLine(Pens.Orange, z, 0, z, pbSideView.Height)

                        Dim Points(2) As Point

                        Points(0).X = z
                        Points(0).Y = 10.0F
                        Points(1).X = z - 3.0F
                        Points(1).Y = 0F
                        Points(2).X = z + 3.0F
                        Points(2).Y = 0

                        g.FillPolygon(Brushes.Gray, Points)

                        Points(0).Y = pbSideView.Height - 10.0F
                        Points(1).Y = pbSideView.Height
                        Points(2).Y = pbSideView.Height

                        g.FillPolygon(Brushes.Gray, Points)

                        Dim lblZ As String = String.Format("{0:D}: {1:F3}", i + 1, Section.Z)
                        Dim lblZsize = g.MeasureString(lblZ, FontLabel)
                        g.FillRectangle(Brushes.Orange, z - 0.5F * lblZsize.Width - 2.0F, 10.0F, lblZsize.Width + 4.0F, lblZsize.Height + 4.0F)
                        g.DrawString(lblZ, FontLabel, Brushes.Brown, z - 0.5F * lblZsize.Width, 12.0F)

                        ' Draw section segment:

                        g.DrawLine(SelectedLinePen, z, y0, z, y1)

                    Else

                        g.DrawLine(MarkerPen, z, y0, z, y1)

                    End If

                    If Not first Then

                        g.DrawLine(DashedPen, z_m1, y0_m1, z, y0)
                        g.DrawLine(DashedPen, z_m1, y1_m1, z, y1)

                    End If

                    first = False

                    g.FillEllipse(Brushes.White, z - 2, y0 - 2, 4, 4)
                    g.DrawEllipse(MarkerPen, z - 2, y0 - 2, 4, 4)

                    g.FillEllipse(Brushes.White, z - 2, y1 - 2, 4, 4)
                    g.DrawEllipse(MarkerPen, z - 2, y1 - 2, 4, 4)

                    y0_m1 = y0
                    y1_m1 = y1
                    z_m1 = z

                End If

                i += 1

            Next

        End If

    End Sub

    Private Sub FormFuselageEditor_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize

        pbSections.Refresh()
        pbSideView.Refresh()

    End Sub

    Private AllowSectionEvents As Boolean = False

    Private Sub nudPosition_ValueChanged(sender As Object, e As EventArgs) Handles nudPosition.ValueChanged

        If AllowSectionEvents And CurrentSection IsNot Nothing Then
            CurrentSection.Z = nudPosition.Value
            pbSideView.Refresh()
        End If

    End Sub

    Private Sub btnCenter_Click(sender As Object, e As EventArgs) Handles btnCenter.Click
        UpdateGeometricParameters()
        Refresh()
    End Sub

    Private Sub cbxBrokenEdge_CheckedChanged(sender As Object, e As EventArgs) Handles cbxBrokenEdge.CheckedChanged

        If AllowSectionEvents And CurrentSection IsNot Nothing Then
            CurrentSection.BrokenEdge = cbxBrokenEdge.Checked
            pbSideView.Refresh()
        End If

    End Sub

    Private Sub btnInertia_Click(sender As Object, e As EventArgs) Handles btnInertia.Click

        If _Fuselage IsNot Nothing Then
            FormInertia.SetInertia(_Fuselage.Inertia)
            If FormInertia.ShowDialog() = DialogResult.OK Then
                _Fuselage.Inertia = FormInertia.GetInertia
            End If
        End If

    End Sub

    Private Sub FormFuselageEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing

        If e.CloseReason = CloseReason.UserClosing Then
            e.Cancel = True
            Hide()
        End If

    End Sub

End Class
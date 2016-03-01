Imports AeroTools.VisualModel.Models.Components
Imports System.Drawing
Imports MathTools.Algebra.EuclideanSpace
Imports System.Windows.Forms
Imports System.Collections.ObjectModel

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
        ObtainGlobalExtremeCoordinates()
        LoadLiftingSurfaces()

        btnUp.Enabled = False
        btnDown.Enabled = False

        If _Fuselage.MeshType = MeshTypes.StructuredQuadrilaterals Then
            rbQuads.Checked = True
        Else
            rbTriangles.Checked = True
        End If

        rbTriangles.Enabled = False

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

        CurrentSectionIndex = 0

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
            lbSections.SelectedIndex = value
        End Set
    End Property

    Private Sub SelectSection() Handles lbSections.SelectedIndexChanged
        _SelectedVertexIndex = -1
        CoordinateControlsEnabled = False
        Refresh()
        nudPosition.DataBindings.Clear()
        nudPosition.DataBindings.Add("Value", CurrentSection, "Z")
    End Sub

    Private Enum CrossSectionStyle As Byte
        Active = 0
        Inactive = 1
    End Enum

    Private _scale As Single
    Private _mcenter As EVector2
    Private _gcenter As PointF

    Private Sub UpdateGeometricParameters()

        Dim mw As Single = _URg.X - _BLg.X
        Dim mh As Single = _URg.Y - _BLg.Y
        _scale = 0.75 * Math.Min(pbSections.Height / mh, pbSections.Width / mw)
        _mcenter = New EVector2(_BLg.X, 0.5 * (_BLg.Y + _URg.Y))
        _gcenter = New PointF(0.5 * pbSections.Width, 0.5 * pbSections.Height)

    End Sub

    Private Function GetPoint(ByVal x As Double, ByVal y As Double) As PointF
        Dim p As PointF
        p.X = _scale * (x - _mcenter.X) + _gcenter.X + panningDisplacement.X
        p.Y = -_scale * (y - _mcenter.Y) + _gcenter.Y + panningDisplacement.Y
        Return p
    End Function

    Private Function GetPoint(ByVal v As EVector2) As PointF
        Dim p As PointF
        p.X = _scale * (v.X - _mcenter.X) + _gcenter.X + panningDisplacement.X
        p.Y = -_scale * (v.Y - _mcenter.Y) + _gcenter.Y + panningDisplacement.Y
        Return p
    End Function

    Private Sub EvaluateScreenPoint(ByVal drawingPoint As PointF, ByRef modelPoint As EVector3)

    End Sub

    Private MarkerPen As New Pen(Color.Black, 1)

    Private Sub DrawSection(ByVal Style As CrossSectionStyle, ByVal Section As CrossSection, ByVal g As Graphics)

        ObtainExtremeCoordinates(Section)

        If Not IsNothing(CurrentSection) Then

            g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            UpdateGeometricParameters()

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

                        content = String.Format("X:{0:F3}; Y:{1:F3}", Section.Vertices(i).X, Section.Vertices(i).Y)
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

    Private _URg As New EVector2
    Private _BLg As New EVector2

    Public Sub ObtainGlobalExtremeCoordinates()

        Dim firstone As Boolean = True

        _URg.X = 0
        _URg.Y = 0
        _BLg.X = 0
        _BLg.Y = 0

        For Each Section In _Fuselage.CrossSections

            For Each Vertex In Section.Vertices
                If firstone Then
                    _URg.X = Vertex.X
                    _URg.Y = Vertex.Y
                    _BLg.X = Vertex.X
                    _BLg.Y = Vertex.Y
                    firstone = False
                Else
                    _URg.X = Math.Max(_URg.X, Vertex.X)
                    _URg.Y = Math.Max(_URg.Y, Vertex.Y)
                    _BLg.X = Math.Min(_BLg.X, Vertex.X)
                    _BLg.Y = Math.Min(_BLg.Y, Vertex.Y)
                End If
            Next

        Next

    End Sub

    Private _URl As New EVector2
    Private _BLl As New EVector2

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

    Private Sub pbDrawing_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pbSections.Paint

        Dim style As CrossSectionStyle = CrossSectionStyle.Inactive

        For i = 0 To _Fuselage.CrossSections.Count - 1
            If i <> CurrentSectionIndex Then
                DrawSection(CrossSectionStyle.Inactive, _Fuselage.CrossSections(i), e.Graphics)
            End If
        Next

        DrawSection(CrossSectionStyle.Active, CurrentSection, e.Graphics)

    End Sub

    Private FontLabel As New Font("Segoe UI", 6.5)

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

    Private Sub MoveSections(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseMove
        If panning Then
            panningDisplacement.X = e.X - panningAnkor.X
            panningDisplacement.Y = e.Y - panningAnkor.Y
            Refresh()
        End If
    End Sub

    Private Sub SetAnkor(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Middle Then
            panning = True
            panningAnkor.X = e.X - panningDisplacement.X
            panningAnkor.Y = e.Y - panningDisplacement.Y
        End If
        Refresh()
    End Sub

    Public Sub RemoveAnkor(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseUp
        panning = False
    End Sub

    Private _SelectedVertexIndex As Integer = -1

    Private Sub SelectVertex(ByVal s As Object, ByVal e As MouseEventArgs) Handles pbSections.MouseDown

        If e.Button = Windows.Forms.MouseButtons.Left Then

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

        End If
        Refresh()
    End Sub

    Private movingVertex As Boolean = False
    Private vertexDrawingAnkor As PointF
    Private vertexDisplacement As PointF
    Private vertexModelAnkor As New EVector2

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

            Dim newVertex As New EVector2
            newVertex.X = 0.5 * (CurrentSection.Vertices(_SelectedVertexIndex).X + CurrentSection.Vertices(_SelectedVertexIndex + 1).X)
            newVertex.Y = 0.5 * (CurrentSection.Vertices(_SelectedVertexIndex).Y + CurrentSection.Vertices(_SelectedVertexIndex + 1).Y)

            CurrentSection.Vertices.Insert(_SelectedVertexIndex + 1, newVertex)

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
            Section.Z = SectionA.Z - 1
        ElseIf Not IsNothing(SectionB) Then
            Section.Z = SectionB.Z + 1
        End If

        For i = 0 To 10

            Dim c As Double = i / 10

            Dim p1 As EVector2 = Nothing
            If Not IsNothing(SectionA) Then p1 = SectionA.GetPoint(c)

            Dim p2 As EVector2 = Nothing
            If Not IsNothing(SectionB) Then p2 = SectionB.GetPoint(c)

            If (Not IsNothing(p1)) And (Not IsNothing(p2)) Then
                Dim p As New EVector2
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
                    If AnchLine.WingAnchorInfo.ParentSerialNumber = _LiftingSurfaces(i).SerialNumber Then
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

                Dim n As Integer = _LiftingSurfaces(i).nChordPanels

                For j = 0 To n

                    Dim Line As New ELine3()

                    Line.Point.Z = _LiftingSurfaces(i).Mesh.NodalPoints(j).Position.X
                    Line.Point.Y = _LiftingSurfaces(i).Mesh.NodalPoints(j).Position.Z
                    Line.Point.X = _LiftingSurfaces(i).Mesh.NodalPoints(j).Position.Y

                    Dim pa As EVector3 = _LiftingSurfaces(i).Mesh.NodalPoints(j).Position
                    Dim pb As EVector3 = _LiftingSurfaces(i).Mesh.NodalPoints(j + n + 1).Position

                    Line.Direction.X = pa.Y - pb.Y
                    Line.Direction.Y = pa.Z - pb.Z
                    Line.Direction.Z = pa.X - pb.X
                    Line.Direction.Normalize()

                    AnchorLine.Lines.Add(Line)

                Next

                Dim Info As New WingAnchorInfo

                Info.ParentSerialNumber = _LiftingSurfaces(i).SerialNumber

                AnchorLine.WingAnchorInfo = Info

                _Fuselage.AnchorLines.Add(AnchorLine)

            End If

        Next

    End Sub

#End Region

    Private Sub SelectQuads(sender As Object, e As EventArgs) Handles rbQuads.Click
        _Fuselage.MeshType = MeshTypes.StructuredQuadrilaterals
    End Sub

    Private Sub SelectTriangles(sender As Object, e As EventArgs) Handles rbTriangles.Click
        _Fuselage.MeshType = MeshTypes.UnstructuredTriangles
    End Sub

    Private Sub rbQuads_CheckedChanged(sender As Object, e As EventArgs) Handles rbQuads.CheckedChanged
        nudNPS.Enabled = rbQuads.Checked
        lblNPS.Enabled = rbQuads.Checked
        nudNPZ.Enabled = rbQuads.Checked
        lblNPZ.Enabled = rbQuads.Checked
    End Sub

    Private Sub rbTriangles_CheckedChanged(sender As Object, e As EventArgs) Handles rbTriangles.CheckedChanged
        nudNPS.Enabled = rbQuads.Checked
        lblNPS.Enabled = rbQuads.Checked
        nudNPZ.Enabled = rbQuads.Checked
        lblNPZ.Enabled = rbQuads.Checked
    End Sub

    Private Sub nudNPS_ValueChanged(sender As Object, e As EventArgs) Handles nudNPS.ValueChanged
        _Fuselage.CrossRefinement = nudNPS.Value
    End Sub

    Private Sub nudNPZ_ValueChanged(sender As Object, e As EventArgs) Handles nudNPZ.ValueChanged
        _Fuselage.LongitudinalRefinement = nudNPZ.Value
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        LoadWingAnchorsToBody()

    End Sub

End Class
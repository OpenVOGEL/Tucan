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

Imports System.Drawing
Imports System.Windows.Forms
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools

Public Class WingControl

    Public Event RefreshGL()

    Private Surface As LiftingSurface
    Private PolarDataBase As PolarDatabase

    Private ModifyingSegment1 As Boolean = False
    Private ModifyingSegment2 As Boolean = False

    Private WithChanges As Boolean = False
    Private Ready As Boolean = False

    Public ReadOnly Property Assigned As Boolean
        Get
            Return Not IsNothing(Surface)
        End Get
    End Property

    Public Sub InitializeControl(ByRef SuperficieDeReferencia As LiftingSurface, Optional ByRef PolarDataBase As PolarDatabase = Nothing)

        Ready = False

        Me.PolarDataBase = PolarDataBase

        Surface = SuperficieDeReferencia
        ShowSurfaceState()

        LoadSurfaceToForm()
        LoadRegionToForm()

    End Sub

    Private Sub LoadControl(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Assigned Then Exit Sub

        Ready = False

        ShowSurfaceState()
        LoadSurfaceToForm()
        LoadRegionToForm()

    End Sub

    Private Sub ShowControl(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged

        If Not Assigned Then Exit Sub

        Ready = False

        Me.ShowSurfaceState()
        Me.LoadSurfaceToForm()
        Me.LoadRegionToForm()

    End Sub

    Private Sub PointToCurrentRegion()

        Surface.CurrentRegionID = Me.nudSelectRegion.Value

    End Sub

    Private Sub CollectSurfaceData(Optional ByVal Replot As Boolean = True)

        If Not Assigned Then Exit Sub

        If Not Ready Then Exit Sub

        PointToCurrentRegion()

        Surface.Name = tbSurfaceName.Text
        Surface.NumberOfChordPanels = nudChordwisePanels.Value

        Surface.CurrentRegion.SpanPanelsCount = nudSpanwiseRings.Value
        Surface.CurrentRegion.TipChord = nudTipChord.Value
        Surface.CurrentRegion.Length = nudLength.Value
        Surface.CurrentRegion.Sweepback = nudSweepback.Value
        Surface.CurrentRegion.Dihedral = nudDihedral.Value
        Surface.CurrentRegion.Twist = nudTwist.Value
        Surface.CurrentRegion.TwistAxis = nudTwistingAxis.Value

        If rbConstantSpacement.Checked Then
            Surface.CurrentRegion.SpacementType = WingRegion.Spacements.Constant
        ElseIf rbLinearSpacement.Checked Then
            Surface.CurrentRegion.SpacementType = WingRegion.Spacements.Linear
        Else
            Surface.CurrentRegion.SpacementType = WingRegion.Spacements.Constant
        End If

        Surface.RootFlap = nudRootFlap.Value
        Surface.FlapPanels = nudFlapPanels.Value
        Surface.CurrentRegion.FlapChord = nudFlapChord.Value
        Surface.CurrentRegion.FlapDeflection = nudFlapDeflection.Value * Math.PI / 180
        Surface.CurrentRegion.Flapped = cbFlapped.Checked

        If rbTipSection.Checked Then

            Surface.CurrentRegion.TipSection.AE = nudArea.Value * 1000 ' kNm to Nm
            Surface.CurrentRegion.TipSection.GJ = nudIu.Value
            Surface.CurrentRegion.TipSection.EIy = nudIv.Value
            Surface.CurrentRegion.TipSection.EIz = nudIw.Value
            Surface.CurrentRegion.TipSection.rIp = nudJ.Value
            Surface.CurrentRegion.TipSection.m = nudM.Value
            Surface.CurrentRegion.TipSection.CMy = nudCMy.Value
            Surface.CurrentRegion.TipSection.CMz = nudCMz.Value

        Else

            If nudSelectRegion.Value = 1 Then

                Surface.RootSection.AE = nudArea.Value * 1000 ' kNm to Nm
                Surface.RootSection.GJ = nudIu.Value
                Surface.RootSection.EIy = nudIv.Value
                Surface.RootSection.EIz = nudIw.Value
                Surface.RootSection.rIp = nudJ.Value
                Surface.RootSection.m = nudM.Value
                Surface.RootSection.CMy = nudCMy.Value
                Surface.RootSection.CMz = nudCMz.Value

            End If

        End If

        Surface.CurrentRegion.CenterOfShear = nudCS.Value

        If nudSelectRegion.Value = 1 Then
            Surface.RootChord = nudRootChord.Value
        End If

        Surface.ConvectWake = cbConvectWake.Checked
        Surface.TrailingEdgeConvection = cbTrailingEdge.Checked
        Surface.VisualProperties.ShowPrimitives = cbShowPriitives.Checked

        Surface.Symmetric = cbSymetricWing.Checked

        Surface.CuttingStep = nudCuttingStep.Value

        Surface.GenerateMesh()

        WithChanges = True

        If Replot Then RaiseEvent RefreshGL()

    End Sub

    Private Sub LoadSurfaceToForm()

        If Not Assigned Then Exit Sub

        Ready = False

        tbSurfaceName.Text = Surface.Name
        nudSelectRegion.Maximum = Surface.WingRegions.Count
        nudSelectRegion.Minimum = 1
        nudSelectRegion.Value = Surface.CurrentRegionID
        nudRootChord.Value = Surface.RootChord
        nudRootFlap.Value = Surface.RootFlap
        nudFlapPanels.Value = Surface.FlapPanels
        nudChordwisePanels.Value = Surface.NumberOfChordPanels
        cbSymetricWing.Checked = Surface.Symmetric
        SetPrimitiveBounds()
        nudLastPrimitive.Value = Surface.LastPrimitiveSegment
        nudFirstPrimitive.Value = Surface.FirstPrimitiveSegment
        cbConvectWake.Checked = Surface.ConvectWake
        cbTrailingEdge.Checked = Surface.TrailingEdgeConvection
        cbShowPriitives.Checked = Surface.VisualProperties.ShowPrimitives
        nudCuttingStep.Value = Surface.CuttingStep

        LoadRegionToForm()

    End Sub

    Private Sub SwitchSectionEdition(Enabled As Boolean)
        nudArea.Enabled = Enabled
        nudIu.Enabled = Enabled
        nudIv.Enabled = Enabled
        nudIw.Enabled = Enabled
        nudJ.Enabled = Enabled
        nudM.Enabled = Enabled
        nudCMy.Enabled = Enabled
        nudCMz.Enabled = Enabled
    End Sub

    Function ThereIsPreviousRegion() As Boolean

        Return nudSelectRegion.Value > 1 And nudSelectRegion.Value <= Surface.WingRegions.Count

    End Function

    Function GetPreviousRegion() As WingRegion

        If ThereIsPreviousRegion() Then
            Return Surface.WingRegions(nudSelectRegion.Value - 2)
        Else
            Return Nothing
        End If

    End Function

    Private Sub LoadRegionToForm()

        If Not Assigned Then Exit Sub

        ' Asigna todas las propiedades del tramo activo para la superficie activa

        Ready = False

        nudSpanwiseRings.Value = Surface.CurrentRegion.SpanPanelsCount
        nudTipChord.Value = Surface.CurrentRegion.TipChord
        nudLength.Value = Surface.CurrentRegion.Length
        nudSweepback.Value = Surface.CurrentRegion.Sweepback
        nudDihedral.Value = Surface.CurrentRegion.Dihedral
        nudTwist.Value = Surface.CurrentRegion.Twist
        nudTwistingAxis.Value = Surface.CurrentRegion.TwistAxis

        Select Case Surface.CurrentRegion.SpacementType
            Case WingRegion.Spacements.Constant
                rbConstantSpacement.Checked = True
            Case WingRegion.Spacements.Linear
                rbLinearSpacement.Checked = True
        End Select

        cbFlapped.Checked = Surface.CurrentRegion.Flapped
        nudFlapChord.Value = Surface.CurrentRegion.FlapChord
        nudFlapDeflection.Value = Math.Max(-90, Math.Min(Surface.CurrentRegion.FlapDeflection / Math.PI * 180, 90))

        nudRootChord.Enabled = nudSelectRegion.Value = 1
        If ThereIsPreviousRegion() Then
            Dim PreviousRegion As WingRegion = GetPreviousRegion()
            nudRootChord.Value = PreviousRegion.TipChord
        Else
            nudRootChord.Value = Surface.RootChord
        End If

        If rbRootSection.Checked Then

            If nudSelectRegion.Value = 1 Then

                SwitchSectionEdition(True)
                nudArea.Value = Surface.RootSection.AE / 1000 ' Nm to kNm
                nudIu.Value = Surface.RootSection.GJ
                nudIv.Value = Surface.RootSection.EIy
                nudIw.Value = Surface.RootSection.EIz
                nudJ.Value = Surface.RootSection.rIp
                nudM.Value = Surface.RootSection.m

            Else

                SwitchSectionEdition(False)
                If ThereIsPreviousRegion() Then
                    Dim PreviousRegion As WingRegion = GetPreviousRegion()
                    nudArea.Value = PreviousRegion.TipSection.AE / 1000 ' Nm to kNm
                    nudIu.Value = PreviousRegion.TipSection.GJ
                    nudIv.Value = PreviousRegion.TipSection.EIy
                    nudIw.Value = PreviousRegion.TipSection.EIz
                    nudJ.Value = PreviousRegion.TipSection.rIp
                    nudM.Value = PreviousRegion.TipSection.m
                End If

            End If

        Else

            SwitchSectionEdition(True)
            nudArea.Value = Surface.CurrentRegion.TipSection.AE / 1000 ' Nm to kNm
            nudIu.Value = Surface.CurrentRegion.TipSection.GJ
            nudIv.Value = Surface.CurrentRegion.TipSection.EIy
            nudIw.Value = Surface.CurrentRegion.TipSection.EIz
            nudJ.Value = Surface.CurrentRegion.TipSection.rIp
            nudM.Value = Surface.CurrentRegion.TipSection.m

        End If

        nudCS.Value = Surface.CurrentRegion.CenterOfShear
        nudCMy.Value = Surface.CurrentRegion.TipSection.CMy
        nudCMz.Value = Surface.CurrentRegion.TipSection.CMz

        Dim camber As CamberLine = CamberLinesDatabase.GetCamberLineFromId(Surface.CurrentRegion.CamberLineId)

        If camber IsNot Nothing Then
            lblCamberLineName.Text = camber.Name
        End If

        Dim polar As PolarFamily = ProjectRoot.Model.PolarDataBase.GetFamilyFromID(Surface.CurrentRegion.PolarID)

        If polar IsNot Nothing Then
            lblPolarName.Text = polar.Name
        End If

        pbProfileSketch.Refresh()

        Ready = True

    End Sub

#Region " Macro paneles "

    Private Sub nudSelectRegion_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSelectRegion.ValueChanged

        If Not Ready Then Exit Sub

        PointToCurrentRegion()

        LoadRegionToForm()

    End Sub

    Private Sub btnAddPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPanel.Click

        If Not Ready Then Exit Sub

        PointToCurrentRegion()

        Surface.AddRegion()

        Ready = False
        nudSelectRegion.Maximum = Surface.WingRegions.Count
        nudSelectRegion.Minimum = 1
        nudSelectRegion.Value = Surface.CurrentRegionID
        Ready = True

        LoadRegionToForm()

        Surface.GenerateMesh()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub btnInsertPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertPanel.Click

        If Not Ready Then Exit Sub

        PointToCurrentRegion()

        Surface.InsertRegion()

        Ready = False
        Me.nudSelectRegion.Maximum = Surface.WingRegions.Count
        Me.nudSelectRegion.Minimum = 1
        Me.nudSelectRegion.Value = Surface.CurrentRegionID
        Ready = True

        LoadRegionToForm()

        Surface.GenerateMesh()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub EliminarMacroPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeletePanel.Click
        If Not Ready Then Exit Sub

        PointToCurrentRegion()

        Surface.RemoveCurrentRegion()
        LoadSurfaceToForm()
        Surface.GenerateMesh()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub SetPrimitiveBounds()

        If Surface.FirstPrimitiveSegment < 1 Then
            Surface.FirstPrimitiveSegment = 1
        End If
        If Surface.LastPrimitiveSegment < 1 Then
            Surface.LastPrimitiveSegment = 1
        End If

        nudLastPrimitive.Minimum = Surface.FirstPrimitiveSegment
        nudLastPrimitive.Maximum = Surface.nBoundarySegments
        nudFirstPrimitive.Minimum = 1
        nudFirstPrimitive.Maximum = Surface.LastPrimitiveSegment

    End Sub

#End Region

#Region " Draw profile "

    Public Sub DrawCamberLine(Line As CamberLine, ByVal n As Integer,
                             ByVal wid As Double, ByVal hgt As Double, ByRef g As Graphics)

        Dim Node1 As PointF
        Dim Node2 As PointF
        Dim k As Integer

        Dim myPen As New Pen(Brushes.Black)

        myPen.SetLineCap(Drawing2D.LineCap.RoundAnchor, Drawing2D.LineCap.RoundAnchor, Drawing2D.DashCap.Round)
        myPen.DashPattern = New Single() {4.0F, 2.0F, 1.0F, 3.0F}
        myPen.Width = 1.0F
        g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
        g.Clear(Color.White)

        ' Cord

        Node1.X = 10.0
        Node1.Y = hgt / 2
        Node2.X = wid - 10.0
        Node2.Y = hgt / 2

        g.DrawLine(myPen, Node1, Node2)

        ' Axes

        DrawCoordinates("x", 1, "z", 1, Node1, g)

        myPen.DashPattern = New Single() {200.0F}
        myPen.SetLineCap(Drawing2D.LineCap.RoundAnchor, Drawing2D.LineCap.RoundAnchor, Drawing2D.DashCap.Round)
        myPen.Brush = Brushes.Black
        myPen.Width = 2.0F

        ' Linea media y nodos

        For k = 1 To n

            Node2.X = k / n
            If Line IsNot Nothing Then
                Node2.Y = Line.Y(Node2.X)
            Else
                Node2.Y = 0
            End If

            Node2.X = (wid - 20.0) * Node2.X + 10.0
            Node2.Y = hgt / 2 - (wid - 20.0) * Node2.Y

            g.DrawLine(myPen, Node1, Node2)

            Node1 = Node2

        Next

    End Sub

    Public Sub DrawCoordinates(ByVal x As String, ByVal dirX As Integer, ByVal y As String, ByVal dirY As Integer, ByVal Origin As PointF, ByVal Graph As Graphics)

        Dim Letra As New Font("Microsoft Sans Serif", 9)
        Dim Lapiz As New Pen(Brushes.Black)
        Dim Node1 As PointF
        Dim Node2 As PointF
        Dim ellipse As System.Drawing.Rectangle

        Lapiz.SetLineCap(Drawing2D.LineCap.Round, Drawing2D.LineCap.ArrowAnchor, Drawing2D.DashCap.Round)
        Lapiz.DashPattern = New Single() {200.0F}

        Select Case x
            Case "x"
                Lapiz.Brush = Brushes.Green
            Case "y"
                Lapiz.Brush = Brushes.Red
            Case "z"
                Lapiz.Brush = Brushes.Purple
        End Select

        Lapiz.Width = 3.0F
        Node1 = Origin
        Node2.X = Node1.X + dirX * 30
        Node2.Y = Node1.Y
        Graph.DrawLine(Lapiz, Node1, Node2)
        'Graph.DrawString(x, Letra, Brushes.Black, Node2.X - dirX * 10, Node2.Y + 2)

        Select Case y
            Case "x"
                Lapiz.Brush = Brushes.Green
            Case "y"
                Lapiz.Brush = Brushes.Red
            Case "z"
                Lapiz.Brush = Brushes.Purple
        End Select

        Lapiz.Width = 3.0F
        Node2.X = Node1.X
        Node2.Y = Node1.Y - dirY * 30
        Graph.DrawLine(Lapiz, Node1, Node2)
        'Graph.DrawString(y, Letra, Brushes.Black, Origin.X - dirX * 10, Origin.Y - dirY * 30)

        ellipse.X = Origin.X - 4
        ellipse.Y = Origin.Y - 4
        ellipse.Height = 8
        ellipse.Width = 8

        Graph.FillEllipse(Brushes.White, ellipse)

        Select Case x
            Case "x"
                Select Case y
                    Case "y"
                        Graph.DrawEllipse(Pens.Purple, ellipse)
                    Case "z"
                        Graph.DrawEllipse(Pens.Red, ellipse)
                End Select
            Case "y"
                Select Case y
                    Case "x"
                        Graph.DrawEllipse(Pens.Purple, ellipse)
                    Case "z"
                        Graph.DrawEllipse(Pens.Green, ellipse)
                End Select
            Case "z"
                Select Case y
                    Case "x"
                        Graph.DrawEllipse(Pens.Red, ellipse)
                    Case "y"
                        Graph.DrawEllipse(Pens.Green, ellipse)
                End Select
        End Select

    End Sub

#End Region

#Region " GUI Events "

    Private Sub SurfaceNameText_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSurfaceName.TextChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarPanelesEnLaCuerda(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudChordwisePanels.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarCuerdaRaiz(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRootChord.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarPanelesEnLaEnvergadura(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSpanwiseRings.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarCuerdaExterna(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTipChord.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarLongitud(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLength.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarSweepBack(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudSweepback.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarDihedro(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDihedral.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarTorsionTotal(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTwist.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarEjeDeTorsion(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudTwistingAxis.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarSimetria(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSymetricWing.CheckedChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarEspaciamientoConstante(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbConstantSpacement.CheckedChanged
        CollectSurfaceData()
    End Sub

    Private Sub CambiarEspaciamientoCuadratico(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLinearSpacement.CheckedChanged
        CollectSurfaceData()
    End Sub

    Private Sub CambiarCombaduraMaxima(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CollectSurfaceData()
        pbProfileSketch.Refresh()
    End Sub

    Private Sub CambiarPosicionDeCombaduraMaxima(ByVal sender As System.Object, ByVal e As System.EventArgs)
        CollectSurfaceData()
        pbProfileSketch.Refresh()
    End Sub

    Private Sub nudCuttingStep_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCuttingStep.ValueChanged
        CollectSurfaceData()
    End Sub

    Private Sub CambiarSegmentoPrimitivo1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudFirstPrimitive.ValueChanged

        ModifyingSegment1 = True

        If Not ModifyingSegment2 And Ready Then

            Surface.FirstPrimitiveSegment = nudFirstPrimitive.Value

            nudLastPrimitive.Minimum = Surface.FirstPrimitiveSegment
            nudLastPrimitive.Maximum = Surface.nBoundarySegments

        End If

        ModifyingSegment1 = False

        Me.CollectSurfaceData()

    End Sub

    Private Sub CambiarSegmentoPrimitivo2(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLastPrimitive.ValueChanged
        If Not ModifyingSegment1 And Ready Then

            Surface.LastPrimitiveSegment = nudLastPrimitive.Value

            nudFirstPrimitive.Minimum = 1
            nudFirstPrimitive.Maximum = Surface.LastPrimitiveSegment

        End If

        ModifyingSegment2 = False

        Me.CollectSurfaceData()

    End Sub

    Private Sub CambiarConvectarEstela(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbConvectWake.CheckedChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub CambiarMostrarPrimitivas(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowPriitives.CheckedChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarOrigenX(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarOrigenY(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarOrigenZ(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarCentroX(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarCentroY(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarCentroZ(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarPsi(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarTita(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub CambiarPhi(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub MostrarSuperficie(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHideSurface.Click
        Surface.VisualProperties.ShowSurface = Not Surface.VisualProperties.ShowSurface
        Me.ShowSurfaceState()
    End Sub

    Private Sub BloquearContenido(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLockSurface.Click
        Surface.Lock = Not Surface.Lock
        Me.ShowSurfaceState()
    End Sub

    Private Sub nudArea_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudArea.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudIu_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIu.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudIv_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIv.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudIw_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudIw.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudE_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData()
    End Sub

    Private Sub nudNu_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudRho_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudRootArea_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudRootIu_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudRootIv_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudRootIw_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub btSurfaceData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btSurfaceData.Click
        tbSurfaceData.Text = Surface.RetriveStringData()
    End Sub

    Private Sub nud_Root_J_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nud_Root_m_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nud_J_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudJ.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nud_m_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudM.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub cbFlapped_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFlapped.CheckedChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub nudFlapChord_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudFlapChord.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub nudFlapDeflection_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudFlapDeflection.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub nudRootFlap_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRootFlap.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub nudFlapPanels_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudFlapPanels.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub nudExcentricity_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCS.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    Private Sub nudCMy_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCMy.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudCMz_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCMz.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudCMyRoot_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub nudCMzRoot_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.CollectSurfaceData(False)
    End Sub

    Private Sub cbSecuence_SelectedIndexChanged(sender As Object, e As EventArgs)
        CollectSurfaceData()
    End Sub

#End Region

#Region " Others "

    Private Sub Espaciamiento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim Posicion As System.Drawing.Point
        Posicion.X = 22
        Posicion.Y = 40
    End Sub

    Private Sub ShowSurfaceState()

        If Not Assigned Then Exit Sub

        If Surface.Lock Then
            PanelDeEdicion.Enabled = False
            btnLockSurface.BackColor = Color.DeepSkyBlue
        Else
            Me.PanelDeEdicion.Enabled = True
            btnLockSurface.BackColor = Color.White
        End If

        If Surface.VisualProperties.ShowSurface Then
            btnHideSurface.BackColor = Color.White
        Else
            btnHideSurface.BackColor = Color.DeepSkyBlue
        End If

    End Sub

    Private Sub pbProfileSketch_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pbProfileSketch.Paint

        Dim g As Graphics = e.Graphics

        DrawCamberLine(GetCamberLineFromId(Surface.CurrentRegion.CamberLineId), nudChordwisePanels.Value, pbProfileSketch.Width, pbProfileSketch.Height, g)

    End Sub

#End Region

    Private Sub btnPolarCurves_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPolarCurves.Click

        If Not IsNothing(PolarDataBase) Then
            Dim ID As Guid = Guid.Empty
            If Not IsNothing(Surface.CurrentRegion.PolarFamiliy) Then
                ID = Surface.CurrentRegion.PolarFamiliy.ID
            End If
            Dim form As New FormPolarCurve(PolarDataBase, ID)
            If form.ShowDialog() = vbOK Then
                If Not form.SelectedFamilyID.Equals(Guid.Empty) Then
                    Surface.CurrentRegion.PolarFamiliy = PolarDataBase.GetFamilyFromID(form.SelectedFamilyID)
                    Surface.CurrentRegion.PolarID = form.SelectedFamilyID
                End If
            End If
            LoadRegionToForm()
        End If

    End Sub

    Private Sub tcMacroPanelProperties_Selected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles tcMacroPanelProperties.Selected
        Select Case tcMacroPanelProperties.SelectedIndex
            Case 0, 1, 2, 3
                Surface.VisualProperties.VisualizationMode = VisualModel.Interface.VisualizationMode.Lattice
            Case 4
                Surface.VisualProperties.VisualizationMode = VisualModel.Interface.VisualizationMode.Structural
        End Select

        RaiseEvent RefreshGL()

    End Sub

    Public Event OnClose()

    Private Sub CloseForm(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        RaiseEvent OnClose()
        Me.Visible = False
    End Sub

    Private Sub cbTrailingEdge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTrailingEdge.CheckedChanged
        CollectSurfaceData()
        nudFirstPrimitive.Enabled = Not cbTrailingEdge.Checked
        nudLastPrimitive.Enabled = Not cbTrailingEdge.Checked
    End Sub

    Private Sub btnCamberLines_Click(sender As Object, e As EventArgs) Handles btnCamberLines.Click

        If (Surface.CurrentRegion IsNot Nothing) Then

            Dim form As New FormCamberLine(Surface.CurrentRegion.CamberLineId)

            Select Case form.ShowDialog()
                Case DialogResult.OK
                    Surface.CurrentRegion.CamberLineId = form.SelectedCamberID
                    Surface.GenerateMesh()
                    RaiseEvent RefreshGL()
            End Select

            LoadRegionToForm()

        End If

    End Sub

    Private Sub rbTipSection_CheckedChanged(sender As Object, e As EventArgs) Handles rbTipSection.CheckedChanged
        LoadRegionToForm()
    End Sub

    Private Sub rbRootSection_CheckedChanged(sender As Object, e As EventArgs) Handles rbRootSection.CheckedChanged
        LoadRegionToForm()
    End Sub
End Class

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
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools

Public Class WingControl

    ''' <summary>
    ''' Indicates that a refresh in the 3D model is required
    ''' </summary>
    Public Event RefreshModelView()

    ''' <summary>
    ''' A reference to the lifting surface beeing edited
    ''' </summary>
    Private Wing As LiftingSurface

    ''' <summary>
    ''' Blocks the modification of the first primitive
    ''' during the moodification of the second primitive
    ''' </summary>
    Private LockFirstPrimitive As Boolean = False

    ''' <summary>
    ''' Blocks the modification of the second primitive
    ''' during the moodification of the first primitive
    ''' </summary>
    Private LockLastPrimitive As Boolean = False
    Private WithChanges As Boolean = False
    Private Loaded As Boolean = False

    ''' <summary>
    ''' Indicates if the control has a valid reference to a surface
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Assigned As Boolean
        Get
            Return Not IsNothing(Wing)
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub InitializeControl(ByRef ReferenceWing As LiftingSurface)

        Loaded = False

        If ReferenceWing IsNot Wing Then

            Wing = ReferenceWing
            ShowSurfaceState()
            LoadSurfaceToForm()
            LoadRegionToForm()

        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadControl(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not Assigned Then Exit Sub

        Loaded = False

        ShowSurfaceState()
        LoadSurfaceToForm()
        LoadRegionToForm()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ShowControl(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged

        If Not Assigned Then Exit Sub

        Loaded = False

        Me.ShowSurfaceState()
        Me.LoadSurfaceToForm()
        Me.LoadRegionToForm()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PointToCurrentRegion()

        Wing.CurrentRegionId = Me.NudSelectedRegion.Value

        RaiseEvent RefreshModelView()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CollectSurfaceData(Optional ByVal Replot As Boolean = True)

        If Not Assigned Then Exit Sub

        If Not Loaded Then Exit Sub

        PointToCurrentRegion()

        Wing.Name = TabSurfaceName.Text
        Wing.NumberOfChordPanels = NudChordwisePanels.Value

        Wing.CurrentRegion.SpanPanelsCount = NudSpanwiseRings.Value
        Wing.CurrentRegion.TipChord = NudTipChord.Value
        Wing.CurrentRegion.Length = NudLength.Value
        Wing.CurrentRegion.Sweepback = NudSweepback.Value
        Wing.CurrentRegion.Dihedral = NudDihedral.Value
        Wing.CurrentRegion.Twist = NudTwist.Value
        Wing.CurrentRegion.TwistAxis = NudTwistingAxis.Value

        If RbConstantSpacement.Checked Then
            Wing.CurrentRegion.SpacementType = WingRegion.Spacements.Constant
        ElseIf RbLinearSpacement.Checked Then
            Wing.CurrentRegion.SpacementType = WingRegion.Spacements.Linear
        Else
            Wing.CurrentRegion.SpacementType = WingRegion.Spacements.Constant
        End If

        Wing.RootFlap = NudRootFlap.Value
        Wing.FlapPanels = NudFlapPanels.Value
        Wing.CurrentRegion.FlapChord = NudFlapChord.Value
        Wing.CurrentRegion.FlapDeflection = NudFlapDeflection.Value * Math.PI / 180
        Wing.CurrentRegion.Flapped = CbFlapped.Checked

        If rbTipSection.Checked Then

            Wing.CurrentRegion.TipSection.AE = NudArea.Value
            Wing.CurrentRegion.TipSection.GJ = NudIu.Value
            Wing.CurrentRegion.TipSection.EIy = NudIv.Value
            Wing.CurrentRegion.TipSection.EIz = NudIw.Value
            Wing.CurrentRegion.TipSection.Ip = NudJ.Value
            Wing.CurrentRegion.TipSection.M = NudM.Value
            Wing.CurrentRegion.TipSection.Cmy = NudCmy.Value
            Wing.CurrentRegion.TipSection.Cmz = NudCmz.Value

        Else

            If NudSelectedRegion.Value = 1 Then

                Wing.RootSection.AE = NudArea.Value
                Wing.RootSection.GJ = NudIu.Value
                Wing.RootSection.EIy = NudIv.Value
                Wing.RootSection.EIz = NudIw.Value
                Wing.RootSection.Ip = NudJ.Value
                Wing.RootSection.M = NudM.Value
                Wing.RootSection.Cmy = NudCmy.Value
                Wing.RootSection.Cmz = NudCmz.Value

            End If

        End If

        Wing.CurrentRegion.CenterOfShear = NudCS.Value

        If NudSelectedRegion.Value = 1 Then
            Wing.RootChord = NudRootChord.Value
        End If

        Wing.ConvectWake = CbConvectWake.Checked
        Wing.TrailingEdgeConvection = cbTrailingEdge.Checked
        Wing.VisualProperties.ShowPrimitives = CbShowPriitives.Checked

        Wing.Symmetric = CbSymetricWing.Checked

        Wing.CuttingStep = NudCuttingStep.Value

        Wing.GenerateMesh()

        WithChanges = True

        If Replot Then RaiseEvent RefreshModelView()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadSurfaceToForm()

        If Not Assigned Then Exit Sub

        Loaded = False

        TabSurfaceName.Text = Wing.Name
        NudSelectedRegion.Maximum = Wing.WingRegions.Count
        NudSelectedRegion.Minimum = 1
        NudSelectedRegion.Value = Wing.CurrentRegionId
        NudRootChord.Value = Wing.RootChord
        NudRootFlap.Value = Wing.RootFlap
        NudFlapPanels.Value = Wing.FlapPanels
        NudChordwisePanels.Value = Wing.NumberOfChordPanels
        CbSymetricWing.Checked = Wing.Symmetric
        SetPrimitiveBounds()
        NudLastPrimitive.Value = Wing.LastPrimitiveSegment
        NudFirstPrimitive.Value = Wing.FirstPrimitiveSegment
        CbConvectWake.Checked = Wing.ConvectWake
        cbTrailingEdge.Checked = Wing.TrailingEdgeConvection
        CbShowPriitives.Checked = Wing.VisualProperties.ShowPrimitives
        NudCuttingStep.Value = Wing.CuttingStep

        LoadRegionToForm()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub SwitchSectionEdition(Enabled As Boolean)

        NudArea.Enabled = Enabled
        NudIu.Enabled = Enabled
        NudIv.Enabled = Enabled
        NudIw.Enabled = Enabled
        NudJ.Enabled = Enabled
        NudM.Enabled = Enabled
        NudCmy.Enabled = Enabled
        NudCmz.Enabled = Enabled

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Function ThereIsPreviousRegion() As Boolean

        Return NudSelectedRegion.Value > 1 And NudSelectedRegion.Value <= Wing.WingRegions.Count

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Function GetPreviousRegion() As WingRegion

        If ThereIsPreviousRegion() Then
            Return Wing.WingRegions(NudSelectedRegion.Value - 2)
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadRegionToForm()

        If Not Assigned Then Exit Sub

        ' Asigna todas las propiedades del tramo activo para la superficie activa

        Loaded = False

        NudSpanwiseRings.Value = Wing.CurrentRegion.SpanPanelsCount
        NudTipChord.Value = Wing.CurrentRegion.TipChord
        NudLength.Value = Wing.CurrentRegion.Length
        NudSweepback.Value = Wing.CurrentRegion.Sweepback
        NudDihedral.Value = Wing.CurrentRegion.Dihedral
        NudTwist.Value = Wing.CurrentRegion.Twist
        NudTwistingAxis.Value = Wing.CurrentRegion.TwistAxis

        Select Case Wing.CurrentRegion.SpacementType
            Case WingRegion.Spacements.Constant
                RbConstantSpacement.Checked = True
            Case WingRegion.Spacements.Linear
                RbLinearSpacement.Checked = True
        End Select

        CbFlapped.Checked = Wing.CurrentRegion.Flapped
        NudFlapChord.Value = Wing.CurrentRegion.FlapChord
        NudFlapDeflection.Value = Math.Max(-90, Math.Min(Wing.CurrentRegion.FlapDeflection / Math.PI * 180, 90))

        NudRootChord.Enabled = NudSelectedRegion.Value = 1
        If ThereIsPreviousRegion() Then
            Dim PreviousRegion As WingRegion = GetPreviousRegion()
            NudRootChord.Value = PreviousRegion.TipChord
        Else
            NudRootChord.Value = Wing.RootChord
        End If

        If rbRootSection.Checked Then

            If NudSelectedRegion.Value = 1 Then

                SwitchSectionEdition(True)

                NudArea.Value = Wing.RootSection.AE
                NudIu.Value = Wing.RootSection.GJ
                NudIv.Value = Wing.RootSection.EIy
                NudIw.Value = Wing.RootSection.EIz
                NudJ.Value = Wing.RootSection.Ip
                NudM.Value = Wing.RootSection.M
                NudCmy.Value = Wing.RootSection.Cmy
                NudCmz.Value = Wing.RootSection.Cmz

            Else

                SwitchSectionEdition(False)

                If ThereIsPreviousRegion() Then

                    Dim PreviousRegion As WingRegion = GetPreviousRegion()
                    NudArea.Value = PreviousRegion.TipSection.AE
                    NudIu.Value = PreviousRegion.TipSection.GJ
                    NudIv.Value = PreviousRegion.TipSection.EIy
                    NudIw.Value = PreviousRegion.TipSection.EIz
                    NudJ.Value = PreviousRegion.TipSection.Ip
                    NudM.Value = PreviousRegion.TipSection.M
                    NudCmy.Value = PreviousRegion.TipSection.Cmy
                    NudCmz.Value = PreviousRegion.TipSection.Cmz

                End If

            End If

        Else

            SwitchSectionEdition(True)

            NudArea.Value = Wing.CurrentRegion.TipSection.AE
            NudIu.Value = Wing.CurrentRegion.TipSection.GJ
            NudIv.Value = Wing.CurrentRegion.TipSection.EIy
            NudIw.Value = Wing.CurrentRegion.TipSection.EIz
            NudJ.Value = Wing.CurrentRegion.TipSection.Ip
            NudM.Value = Wing.CurrentRegion.TipSection.M
            NudCmy.Value = Wing.CurrentRegion.TipSection.Cmy
            NudCmz.Value = Wing.CurrentRegion.TipSection.Cmz

        End If

        NudCS.Value = Wing.CurrentRegion.CenterOfShear

        Dim camber As CamberLine = CamberLinesDatabase.GetCamberLineFromId(Wing.CurrentRegion.CamberLineId)

        If camber IsNot Nothing Then
            lblCamberLineName.Text = camber.Name
        End If

        Dim polar As PolarFamily = ProjectRoot.Model.PolarDataBase.GetFamilyFromID(Wing.CurrentRegion.PolarID)

        If polar IsNot Nothing Then
            lblPolarName.Text = polar.Name
        End If

        pbProfileSketch.Refresh()

        Loaded = True

    End Sub

#Region " Macro paneles "

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudSelectRegion_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudSelectedRegion.ValueChanged

        If Not Loaded Then Exit Sub

        PointToCurrentRegion()

        LoadRegionToForm()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BtnAddPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddPanel.Click

        If Not Loaded Then Exit Sub

        PointToCurrentRegion()

        Wing.AddRegion()

        Loaded = False
        NudSelectedRegion.Maximum = Wing.WingRegions.Count
        NudSelectedRegion.Minimum = 1
        NudSelectedRegion.Value = Wing.CurrentRegionId
        Loaded = True

        LoadRegionToForm()

        Wing.GenerateMesh()

        RaiseEvent RefreshModelView()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BtnInsertPanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnInsertPanel.Click

        If Not Loaded Then Exit Sub

        PointToCurrentRegion()

        Wing.InsertRegion()

        Loaded = False
        Me.NudSelectedRegion.Maximum = Wing.WingRegions.Count
        Me.NudSelectedRegion.Minimum = 1
        Me.NudSelectedRegion.Value = Wing.CurrentRegionId
        Loaded = True

        LoadRegionToForm()

        Wing.GenerateMesh()

        RaiseEvent RefreshModelView()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BtnDeletePanel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDeletePanel.Click
        If Not Loaded Then Exit Sub

        PointToCurrentRegion()

        Wing.RemoveCurrentRegion()
        LoadSurfaceToForm()
        Wing.GenerateMesh()

        RaiseEvent RefreshModelView()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub SetPrimitiveBounds()

        If Wing.FirstPrimitiveSegment < 1 Then
            Wing.FirstPrimitiveSegment = 1
        End If
        If Wing.LastPrimitiveSegment < 1 Then
            Wing.LastPrimitiveSegment = 1
        End If

        NudLastPrimitive.Minimum = Wing.FirstPrimitiveSegment
        NudLastPrimitive.Maximum = Wing.NumBoundarySegments
        NudFirstPrimitive.Minimum = 1
        NudFirstPrimitive.Maximum = Wing.LastPrimitiveSegment

    End Sub

#End Region

#Region " Draw profile "

    ''' <summary>
    ''' 
    ''' </summary>
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

    ''' <summary>
    ''' 
    ''' </summary>
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

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub TabSurfaceName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabSurfaceName.TextChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudChordwisePanels_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudChordwisePanels.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudRootChord_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudRootChord.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudSpanwiseRings_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudSpanwiseRings.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudTipChord_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudTipChord.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudLength_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudLength.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudSweepback_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudSweepback.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudDihedral_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudDihedral.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudTwist_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudTwist.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudTwistingAxis_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudTwistingAxis.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CbSymetricWing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbSymetricWing.CheckedChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub RbConstantSpacement_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RbConstantSpacement.CheckedChanged
        CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub RbLinearSpacement_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RbLinearSpacement.CheckedChanged
        CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudCuttingStep_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudCuttingStep.ValueChanged
        CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudFirstPrimitive_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudFirstPrimitive.ValueChanged

        LockFirstPrimitive = True

        If Not LockLastPrimitive And Loaded Then

            Wing.FirstPrimitiveSegment = NudFirstPrimitive.Value

            NudLastPrimitive.Minimum = Wing.FirstPrimitiveSegment
            NudLastPrimitive.Maximum = Wing.NumBoundarySegments

        End If

        LockFirstPrimitive = False

        Me.CollectSurfaceData()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudLastPrimitive_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudLastPrimitive.ValueChanged

        LockLastPrimitive = True

        If Not LockFirstPrimitive And Loaded Then

            Wing.LastPrimitiveSegment = NudLastPrimitive.Value

            NudFirstPrimitive.Minimum = 1
            NudFirstPrimitive.Maximum = Wing.LastPrimitiveSegment

        End If

        LockLastPrimitive = False

        Me.CollectSurfaceData()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CbConvectWake_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbConvectWake.CheckedChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CbShowPriitives_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbShowPriitives.CheckedChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BtnHideSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnHideSurface.Click
        Wing.VisualProperties.ShowSurface = Not Wing.VisualProperties.ShowSurface
        Me.ShowSurfaceState()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BtnLockSurface_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnLockSurface.Click
        Wing.Lock = Not Wing.Lock
        Me.ShowSurfaceState()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudArea_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudArea.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudIu_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudIu.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudIv_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudIv.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudIw_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudIw.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BtnSurfaceData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSurfaceData.Click
        tbSurfaceData.Text = Wing.RetriveStringData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudJ_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudJ.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub Nud_m_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudM.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CbFlapped_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CbFlapped.CheckedChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudFlapChord_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudFlapChord.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudFlapDeflection_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudFlapDeflection.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudRootFlap_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudRootFlap.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudFlapPanels_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudFlapPanels.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudExcentricity_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudCS.ValueChanged
        Me.CollectSurfaceData()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudCMy_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudCmy.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub NudCMz_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NudCmz.ValueChanged
        Me.CollectSurfaceData(False)
    End Sub

#End Region

#Region " Others "

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ShowSurfaceState()

        If Not Assigned Then Exit Sub

        If Wing.Lock Then
            PanelDeEdicion.Enabled = False
            BtnLockSurface.BackColor = Color.DeepSkyBlue
        Else
            Me.PanelDeEdicion.Enabled = True
            BtnLockSurface.BackColor = Color.White
        End If

        If Wing.VisualProperties.ShowSurface Then
            BtnHideSurface.BackColor = Color.White
        Else
            BtnHideSurface.BackColor = Color.DeepSkyBlue
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub pbProfileSketch_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles pbProfileSketch.Paint

        Dim g As Graphics = e.Graphics

        DrawCamberLine(GetCamberLineFromId(Wing.CurrentRegion.CamberLineId), NudChordwisePanels.Value, pbProfileSketch.Width, pbProfileSketch.Height, g)

    End Sub

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub btnPolarCurves_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPolarCurves.Click

        If Not IsNothing(DataStore.Model.PolarDataBase) Then
            Dim ID As Guid = Guid.Empty
            If Not IsNothing(Wing.CurrentRegion.PolarFamiliy) Then
                ID = Wing.CurrentRegion.PolarFamiliy.ID
            End If
            Dim form As New FormPolarCurve(DataStore.Model.PolarDataBase, ID)
            If form.ShowDialog() = vbOK Then
                If Not form.SelectedFamilyId.Equals(Guid.Empty) Then
                    Wing.CurrentRegion.PolarFamiliy = DataStore.Model.PolarDataBase.GetFamilyFromID(form.SelectedFamilyId)
                    Wing.CurrentRegion.PolarID = form.SelectedFamilyId
                End If
            End If
            LoadRegionToForm()
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub tcMacroPanelProperties_Selected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TabControlEventArgs) Handles tcMacroPanelProperties.Selected
        Select Case tcMacroPanelProperties.SelectedIndex
            Case 0, 1, 2, 3
                Wing.VisualProperties.VisualizationMode = VisualModel.Interface.VisualizationMode.Lattice
            Case 4
                Wing.VisualProperties.VisualizationMode = VisualModel.Interface.VisualizationMode.Structural
        End Select

        RaiseEvent RefreshModelView()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Event OnClose()

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CloseForm(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        RaiseEvent OnClose()
        Me.Visible = False
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub cbTrailingEdge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTrailingEdge.CheckedChanged
        CollectSurfaceData()
        NudFirstPrimitive.Enabled = Not cbTrailingEdge.Checked
        NudLastPrimitive.Enabled = Not cbTrailingEdge.Checked
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub btnCamberLines_Click(sender As Object, e As EventArgs) Handles btnCamberLines.Click

        If (Wing.CurrentRegion IsNot Nothing) Then

            Dim form As New FormCamberLine(Wing.CurrentRegion.CamberLineId)

            Select Case form.ShowDialog()
                Case DialogResult.OK
                    Wing.CurrentRegion.CamberLineId = form.SelectedCamberID
                    Wing.GenerateMesh()
                    RaiseEvent RefreshModelView()
            End Select

            LoadRegionToForm()

        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub rbTipSection_CheckedChanged(sender As Object, e As EventArgs) Handles rbTipSection.CheckedChanged
        LoadRegionToForm()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub rbRootSection_CheckedChanged(sender As Object, e As EventArgs) Handles rbRootSection.CheckedChanged
        LoadRegionToForm()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub btnInertia_Click(sender As Object, e As EventArgs) Handles btnInertia.Click

        If Wing IsNot Nothing Then
            FormInertia.SetInertia(Wing.Inertia)
            If FormInertia.ShowDialog() = DialogResult.OK Then
                Wing.Inertia = FormInertia.GetInertia
            End If
        End If

    End Sub

End Class

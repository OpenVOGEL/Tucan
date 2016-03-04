'Copyright (C) 2016 Guillermo Hazebrouck

Imports System.IO
Imports MathTools.Algebra.EuclideanSpace
Imports AeroTools.UVLM.Models.Aero
Imports AeroTools.UVLM.Models.Aero.Components
Imports AeroTools.UVLM.Models.Structural
Imports System.Xml
Imports AeroTools.VisualModel.Interface.Results
Imports AeroTools.UVLM.Models.Structural.Library
Imports AeroTools.VisualModel.Models
Imports AeroTools.VisualModel.Models.Components
Imports AeroTools.VisualModel.IO

Namespace UVLM.Solver

    Partial Public Class UVLMSolver

        ''' <summary>
        ''' Transfers a geometric model to the calculation cell
        ''' </summary>
        ''' <param name="Model">Model to be transferred</param>
        ''' <param name="GenerateStructure">Indicates if a structural link should be created</param>
        ''' <remarks></remarks>
        Public Sub GenerateFromExistingModel(ByVal Model As CalculationModel, Optional ByVal GenerateStructure As Boolean = False)

            ' Import polar database:

            If Not IsNothing(Model.PolarDataBase) Then
                PolarDataBase = Model.PolarDataBase.Clone()
            End If

            ' Add lifting surfaces:

            If GenerateStructure Then StructuralLinks = New List(Of StructuralLink)

            Dim count As Integer = 0

            For i = 1 To Model.LiftingSurfaces.Count

                Model.CurrentLiftingSurfaceID = i

                If Model.CurrentLiftingSurface.IncludeInCalculation Then

                    count += 1

                    AddLiftingSurface(Model.CurrentLiftingSurface, False, GenerateStructure)

                    If Model.CurrentLiftingSurface.Symmetric Then
                        AddLiftingSurface(Model.CurrentLiftingSurface, True, GenerateStructure)
                    End If

                End If

            Next

            Dim n As Integer = Model.Fuselages.Count

            For i = 1 To n

                Model.CurrentBodyID = i

                If Model.BodySelected AndAlso Model.CurrentBody.IncludeInCalculation Then

                    Dim Lattice As New BoundedLattice
                    Me.Lattices.Add(Lattice)

                    For j = 1 To Model.CurrentBody.NumberOfNodes

                        Lattice.AddNode(Model.CurrentBody.NodalPosition(j))

                    Next

                    For j = 1 To Model.CurrentBody.NumberOfPanels

                        Dim Node1 As Integer = Model.CurrentBody.Panel(j).N1 - 1
                        Dim Node2 As Integer = Model.CurrentBody.Panel(j).N2 - 1
                        Dim Node3 As Integer = Model.CurrentBody.Panel(j).N3 - 1
                        Dim Node4 As Integer = Model.CurrentBody.Panel(j).N4 - 1
                        Dim Reversed As Boolean = Model.CurrentBody.Panel(j).Reversed
                        Dim Slender As Boolean = Model.CurrentBody.Panel(j).IsSlender

                        If Model.CurrentBody.Panel(j).IsTriangular Then
                            Lattice.AddVortexRing3(Node1, Node2, Node3, Reversed, Slender)
                        Else
                            Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, Reversed, Slender)
                        End If

                        Lattice.VortexRings(j - 1).IsPrimitive = Model.CurrentBody.Panel(j).IsPrimitive

                    Next

                    If Not Settings.GlobalPanelSurvey Then Lattice.FindSurroundingRings()

                End If

            Next

            For i = 1 To Model.JetEngines.Count

                Model.CurrentJetEngineID = i

                If Model.JetEngineSelected AndAlso Model.CurrentJetEngine.IncludeInCalculation Then

                    Dim Lattice As New BoundedLattice

                    Lattices.Add(Lattice)

                    For j = 1 To Model.CurrentJetEngine.NumberOfNodes

                        Lattice.AddNode(Model.CurrentJetEngine.NodalPosition(j))

                    Next

                    For j = 1 To Model.CurrentJetEngine.NumberOfPanels

                        Dim Node1 As Integer = Model.CurrentJetEngine.Panel(j).N1 - 1
                        Dim Node2 As Integer = Model.CurrentJetEngine.Panel(j).N2 - 1
                        Dim Node3 As Integer = Model.CurrentJetEngine.Panel(j).N3 - 1
                        Dim Node4 As Integer = Model.CurrentJetEngine.Panel(j).N4 - 1
                        Dim Reversed As Boolean = False
                        Dim Slender As Boolean = True

                        Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, Reversed, Slender)

                    Next

                End If

            Next

            If Lattices.Count = 0 Then
                Throw New Exception("There are no lattices in the calculation model")
            End If

            ' Set global indices in the elements (to access circulation from matrices)

            IndexateLattices()

            ' Find surrounding rings

            If Settings.GlobalPanelSurvey Then

                FindSurroundingRingsGlobally()

            End If

            _WithSources = CheckIfThereAreSources()

            For Each Lattice In Lattices

                Lattice.PopulateVortices()

            Next

        End Sub

        ''' <summary>
        ''' Adds a bounded lattice with wakes from a lifting surface.
        ''' </summary>
        ''' <param name="Surface"></param>
        ''' <param name="Symmetric"></param>
        ''' <param name="GenerateStructure"></param>
        ''' <remarks></remarks>
        Private Sub AddLiftingSurface(ByRef Surface As LiftingSurface, Optional ByVal Symmetric As Boolean = False, Optional ByVal GenerateStructure As Boolean = False)

            ' Add nodal points:

            Dim Lattice As New BoundedLattice

            Lattices.Add(Lattice)

            For j = 1 To Surface.nNodes

                Lattice.AddNode(Surface.GetNodalPoint(j))

                If Symmetric Then Lattice.Nodes(Lattice.Nodes.Count - 1).Position.Y *= -1

            Next

            ' Add rings:

            For j = 1 To Surface.nPanels

                Dim Node1 As Integer = Surface.GetQuadPanel(j).N1 - 1
                Dim Node2 As Integer = Surface.GetQuadPanel(j).N2 - 1
                Dim Node3 As Integer = Surface.GetQuadPanel(j).N3 - 1
                Dim Node4 As Integer = Surface.GetQuadPanel(j).N4 - 1

                Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, False, True)

            Next

            ' Add wakes:

            If Surface.ConvectWake Then

                Dim Wake As New Wake

                For k = Surface.FirstPrimitiveNode To Surface.LastPrimitiveNode
                    Wake.Primitive.Nodes.Add(Surface.GetPrimitiveNodeIndex(k) - 1)
                Next

                For k = Surface.FirstPrimitiveSegment To Surface.LastPrimitiveSegment
                    Wake.Primitive.Rings.Add(Surface.GetPrimitivePanelIndex(k) - 1)
                Next

                Wake.CuttingStep = Surface.CuttingStep

                Lattice.Wakes.Add(Wake)

            End If

            ' Find surrounding rings (for loads calculation), only if global survey options has not been turned on.

            If Not Settings.GlobalPanelSurvey Then Lattice.FindSurroundingRings()

            ' Generate structural link:

            Dim kLink As KinematicLink
            Dim mLink As MechanicLink

            Dim snCount As Integer = -1
            Dim seCount As Integer = -1

            If Surface.IncludeStructure And GenerateStructure Then

                ' Add root node (this node is being clamped, and it is the only one with contrains at the moment):

                Dim StructuralLink As New StructuralLink

                snCount = 0
                seCount = -1
                StructuralLink.StructuralCore.StructuralSettings = Settings.StructuralSettings
                StructuralLink.StructuralCore.Nodes.Add(New Library.StructuralNode(snCount))
                StructuralLink.StructuralCore.Nodes(snCount).Position.Assign(Surface.StructuralPartition(0).p)
                If (Symmetric) Then StructuralLink.StructuralCore.Nodes(snCount).Position.Y *= -1
                StructuralLink.StructuralCore.Nodes(snCount).Contrains.Clamped()

                ' Add kinematic link

                Dim lv As Integer = -1 ' > linked vortex ring
                Dim ln As Integer = -1 ' > linked node

                kLink = New KinematicLink(StructuralLink.StructuralCore.Nodes(snCount))
                For n = 0 To Surface.nChordPanels
                    ln += 1
                    kLink.Link(Lattice.Nodes(ln))
                Next
                StructuralLink.KinematicLinks.Add(kLink)

                ' Add rest of the nodes and elements:

                For pn = 1 To Surface.StructuralPartition.Count - 1

                    ' Add nodes:

                    snCount += 1
                    StructuralLink.StructuralCore.Nodes.Add(New Library.StructuralNode(snCount))
                    StructuralLink.StructuralCore.Nodes(snCount).Position.Assign(Surface.StructuralPartition(pn).p)
                    If (Symmetric) Then StructuralLink.StructuralCore.Nodes(snCount).Position.Y *= -1

                    ' Add element:

                    seCount += 1
                    Dim element As New Library.GeneralBeamElement(seCount)
                    element.NodeA = StructuralLink.StructuralCore.Nodes(snCount - 1)
                    element.NodeB = StructuralLink.StructuralCore.Nodes(snCount)
                    element.Section.Assign(Surface.StructuralPartition(seCount).LocalSection)
                    StructuralLink.StructuralCore.Elements.Add(element)

                    ' Add kinematic link:

                    Dim len As Integer = ln + 1 '(leading edge lattice node index)

                    kLink = New KinematicLink(StructuralLink.StructuralCore.Nodes(snCount))
                    For n = 0 To Surface.nChordPanels
                        ln += 1
                        kLink.Link(Lattice.Nodes(ln))
                    Next
                    StructuralLink.KinematicLinks.Add(kLink)

                    Dim ten As Integer = ln '(trailing edge lattice node index)

                    ' Add mechanic link:

                    mLink = New MechanicLink(element)
                    For r = 0 To Surface.nChordPanels - 1
                        lv += 1
                        mLink.Link(Lattice.VortexRings(lv))
                    Next
                    StructuralLink.MechanicLinks.Add(mLink)

                    ' Build the element basis:

                    ' U has the direction of the element
                    element.Basis.U.X = element.NodeB.Position.X - element.NodeA.Position.X
                    element.Basis.U.Y = element.NodeB.Position.Y - element.NodeA.Position.Y
                    element.Basis.U.Z = element.NodeB.Position.Z - element.NodeA.Position.Z
                    element.Basis.U.Normalize()

                    Dim c As New EVector3 ' chordwise vector
                    c.X = Lattice.Nodes(ten).Position.X - Lattice.Nodes(len).Position.X
                    c.Y = Lattice.Nodes(ten).Position.Y - Lattice.Nodes(len).Position.Y
                    c.Z = Lattice.Nodes(ten).Position.Z - Lattice.Nodes(len).Position.Z

                    ' W is normal to the surface:
                    element.Basis.W.FromVectorProduct(c, element.Basis.U)
                    element.Basis.W.Normalize()

                    ' V is normal to W and U, and points to the trailing edge
                    element.Basis.V.FromVectorProduct(element.Basis.W, element.Basis.U)

                Next

                ' Add this structural link to the list:

                StructuralLinks.Add(StructuralLink)

            End If

            ' Load chordwise stripes (for skin drag computation)

            Dim count As Integer = 0

            For j = 1 To Surface.nWingRegions

                Surface.CurrentRegionID = j

                'If (Not IsNothing(PolarDataBase)) And PolarDataBase.Polars.Count > 0 Then

                For k = 1 To Surface.CurrentRegion.nSpanPanels

                    Dim Stripe As New ChorwiseStripe()

                    Stripe.Polars = Surface.CurrentRegion.PolarFamiliy

                    For p = 1 To Surface.nChordPanels
                        Stripe.Rings.Add(Lattice.VortexRings(count))
                        count += 1
                    Next

                    Lattice.ChordWiseStripes.Add(Stripe)

                Next

                'End If

            Next

        End Sub

        ''' <summary>
        ''' Assign surrounding rings by searching on all lattices
        ''' </summary>
        Public Sub FindSurroundingRingsGlobally()

            Dim STOL As Double = Settings.SurveyTolerance

            Dim Order4(4, 2) As Integer

            Order4(0, 0) = 1
            Order4(0, 1) = 2

            Order4(1, 0) = 2
            Order4(1, 1) = 3

            Order4(2, 0) = 3
            Order4(2, 1) = 4

            Order4(3, 0) = 4
            Order4(3, 1) = 1

            Dim Order3(3, 2) As Integer

            Order3(0, 0) = 1
            Order3(0, 1) = 2

            Order3(1, 0) = 2
            Order3(1, 1) = 3

            Order3(2, 0) = 3
            Order3(2, 1) = 1

            For Each Lattice In Lattices

                Dim mi As EVector3
                Dim mj As EVector3

                Dim ni As EVector3
                Dim nj As EVector3

                For Each Ring In Lattice.VortexRings

                    If Not Ring.IsSlender Then Continue For ' Do not find adjacent panels of non-slender rings (not required)

                    Ring.InitializeSurroundingRings()

                    If Ring.Type = VortexRingType.VR4 Then

                        For m = 0 To 3 ' For each local segment:

                            mi = Ring.Node(Order4(m, 0)).Position
                            mj = Ring.Node(Order4(m, 1)).Position

                            For Each OtherLattice In Lattices

                                For Each OtherRing In OtherLattice.VortexRings

                                    If Ring.IndexG <> OtherRing.IndexG Then

                                        If OtherRing.Type = VortexRingType.VR4 Then

                                            For n = 0 To 3 ' For each other segment:

                                                ni = OtherRing.Node(Order4(n, 0)).Position
                                                nj = OtherRing.Node(Order4(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        ElseIf OtherRing.Type = VortexRingType.VR3 Then

                                            For n = 0 To 2 ' For each other segment:

                                                ni = OtherRing.Node(Order3(n, 0)).Position
                                                nj = OtherRing.Node(Order3(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        End If

                                    End If

                                Next

                            Next

                        Next

                    ElseIf Ring.Type = VortexRingType.VR3 Then

                        For m = 0 To 2 ' For each local segment:

                            mi = Ring.Node(Order3(m, 0)).Position
                            mj = Ring.Node(Order3(m, 1)).Position

                            For Each OtherLattice In Lattices

                                For Each OtherRing In OtherLattice.VortexRings

                                    If Ring.IndexG <> OtherRing.IndexG Then

                                        If OtherRing.Type = VortexRingType.VR4 Then

                                            For n = 0 To 3 ' For each other segment:

                                                ni = OtherRing.Node(Order4(n, 0)).Position
                                                nj = OtherRing.Node(Order4(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        ElseIf OtherRing.Type = VortexRingType.VR3 Then

                                            For n = 0 To 2 ' For each other segment:

                                                ni = OtherRing.Node(Order3(n, 0)).Position
                                                nj = OtherRing.Node(Order3(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        End If

                                    End If

                                Next

                            Next

                        Next

                    End If

                Next

                ' Set primitive flag on primitive panels:

                For Each Wake In Lattice.Wakes

                    For Each Primitive In Wake.Primitive.Rings
                        Lattice.VortexRings(Primitive).IsPrimitive = True
                    Next

                Next

            Next

        End Sub

        ''' <summary>
        ''' Sets the lattices on the result object
        ''' </summary>
        ''' <param name="Results"></param>
        ''' <remarks></remarks>
        Public Sub SetCompleteModelOnResults(ByRef Results As ResultModel)

            Results.Loaded = False
            Results.Model.Name = "Results"
            Results.Model.Clear()
            Results.DynamicModes.Clear()
            Results.SimulationSettings.Assign(Settings)

            Dim CantidadDeSuperficies As Integer = 0

            Dim GlobalIndexNodes As Integer = 0
            Dim GlobalIndexRings As Integer = 0

            For Each Lattice In Lattices

                For Each NodalPoint In Lattice.Nodes
                    GlobalIndexNodes += 1
                    NodalPoint.IndexG = GlobalIndexNodes
                    Results.Model.AddNodalPoint(NodalPoint.Position)
                Next

                For Each VortexRing In Lattice.VortexRings

                    GlobalIndexRings += 1

                    If VortexRing.Type = VortexRingType.VR4 Then

                        Results.Model.AddPanel(VortexRing.Node(1).IndexG,
                                                      VortexRing.Node(2).IndexG,
                                                      VortexRing.Node(3).IndexG,
                                                      VortexRing.Node(4).IndexG)

                    Else

                        Results.Model.AddPanel(VortexRing.Node(1).IndexG,
                                                      VortexRing.Node(2).IndexG,
                                                      VortexRing.Node(3).IndexG,
                                                      VortexRing.Node(1).IndexG)

                    End If

                    Results.Model.Panel(GlobalIndexRings).Circulation = VortexRing.G
                    Results.Model.Panel(GlobalIndexRings).Cp = VortexRing.Cp
                    Results.Model.Panel(GlobalIndexRings).Area = VortexRing.Area
                    Results.Model.Panel(GlobalIndexRings).NormalVector.Assign(VortexRing.Normal)
                    Results.Model.Panel(GlobalIndexRings).LocalVelocity.Assign(VortexRing.VelocityT)
                    Results.Model.Panel(GlobalIndexRings).ControlPoint.Assign(VortexRing.ControlPoint)
                    Results.Model.Panel(GlobalIndexRings).IsSlender = VortexRing.IsSlender
                Next

            Next

            Results.Model.FindPressureRange()
            Results.Model.DistributePressureOnNodes()
            Results.Model.UpdateColormapWithPressure()
            Results.Model.VisualProps.ShowColormap = True

            Results.Model.Mesh.GenerateLattice()

            GlobalIndexNodes = 0
            GlobalIndexRings = 0
            Results.Wakes.Clear()

            For Each Lattice In Lattices

                For Each Wake In Lattice.Wakes

                    For Each NodalPoint In Wake.Nodes
                        GlobalIndexNodes += 1
                        NodalPoint.IndexG = GlobalIndexNodes
                        Results.Wakes.AddNodalPoint(NodalPoint.Position)
                    Next

                    For Each VortexRing In Wake.VortexRings
                        GlobalIndexRings += 1
                        Results.Wakes.AddPanel(VortexRing.Node(1).IndexG,
                                                               VortexRing.Node(2).IndexG,
                                                               VortexRing.Node(3).IndexG,
                                                               VortexRing.Node(4).IndexG)
                        Results.Wakes.Panel(GlobalIndexRings).Circulation = VortexRing.G

                    Next

                    For Each Vortex In Wake.Vortices
                        Dim Segment As New Basics.LatticeSegment
                        Segment.N1 = Vortex.Node1.IndexG
                        Segment.N2 = Vortex.Node2.IndexG
                        Results.Wakes.Mesh.Lattice.Add(Segment)
                    Next

                Next

            Next

            Results.Wakes.VisualProps.ShowMesh = True
            'Resultados.Wakes.GenerateLattice()

            If Not IsNothing(StructuralLinks) Then

                For Each sl As StructuralLink In StructuralLinks

                    For Each Mode As Mode In sl.StructuralCore.Modes

                        Dim ModeShapeModel As New GeneralSurface()

                        ModeShapeModel.Name = String.Format("Mode {0} - {1:F3}Hz", Mode.Index, Mode.w / (2 * Math.PI))
                        ModeShapeModel.VisualProps.ColorMesh = System.Drawing.Color.Maroon
                        ModeShapeModel.VisualProps.ColorSurface = System.Drawing.Color.Orange
                        ModeShapeModel.VisualProps.Transparency = 1.0
                        ModeShapeModel.VisualProps.ShowSurface = True
                        ModeShapeModel.VisualProps.ShowMesh = True
                        ModeShapeModel.VisualProps.ShowNodes = False
                        ModeShapeModel.VisualProps.ThicknessMesh = 0.8
                        ModeShapeModel.VisualProps.ShowNodes = False
                        ModeShapeModel.VisualProps.ShowLoadVectors = False
                        ModeShapeModel.VisualProps.ShowVelocityVectors = False
                        ModeShapeModel.VisualProps.ShowColormap = True

                        ' Reset all displacements:

                        For Each Othersl As StructuralLink In StructuralLinks
                            Othersl.StructuralCore.ResetDisplacements()
                            For Each kl As KinematicLink In Othersl.KinematicLinks
                                kl.TransferMotion()
                            Next
                        Next

                        ' Load the displacement associated with the current mode:

                        sl.StructuralCore.TransferModeShapeToNodes(Mode.Index, 1.0)

                        For Each kl As KinematicLink In sl.KinematicLinks

                            kl.TransferMotion()

                        Next

                        ' Make a lattice based on the current modal displacement:

                        GlobalIndexNodes = 0
                        GlobalIndexRings = 0

                        For Each Lattice In Lattices

                            For Each NodalPoint In Lattice.Nodes
                                NodalPoint.IndexG = GlobalIndexNodes
                                GlobalIndexNodes += 1
                                ModeShapeModel.AddNodalPoint(NodalPoint.OriginalPosition, NodalPoint.Displacement)
                            Next

                            ModeShapeModel.UpdateDisplacement()

                            For Each VortexRing In Lattice.VortexRings
                                GlobalIndexRings += 1
                                ModeShapeModel.AddPanel(VortexRing.Node(1).IndexG + 1,
                                                            VortexRing.Node(2).IndexG + 1,
                                                            VortexRing.Node(3).IndexG + 1,
                                                            VortexRing.Node(4).IndexG + 1)
                                ModeShapeModel.Panel(GlobalIndexRings).Circulation = 0.0
                                ModeShapeModel.Panel(GlobalIndexRings).Cp = 0.0
                                ModeShapeModel.Panel(GlobalIndexRings).IsSlender = True
                            Next

                        Next

                        ModeShapeModel.Mesh.GenerateLattice()
                        ModeShapeModel.FindDisplacementsRange()
                        ModeShapeModel.UpdateColormapWithDisplacements()
                        Results.DynamicModes.Add(ModeShapeModel)

                    Next

                Next

                ' Load transit:

                Results.TransitLattices.Clear()

                Dim NodalDisplacement As New EVector3
                Dim nTimeSteps = 0

                If Settings.StructuralSettings.SubSteps < 1 Then Settings.StructuralSettings.SubSteps = 1

                If StructuralLinks.Count > 0 Then
                    nTimeSteps = StructuralLinks(0).ModalResponse.Count '/ Settings.StructuralSettings.SubSteps
                End If

                Dim TimeStep As Integer = 0

                While TimeStep < nTimeSteps - 1

                    ' Make a lattice based on the current modal displacement:

                    Dim _TransitLattice As New GeneralSurface()

                    GlobalIndexNodes = 0
                    GlobalIndexRings = 0

                    For Each Lattice In Lattices

                        For Each NodalPoint In Lattice.Nodes

                            GlobalIndexNodes += 1

                            NodalDisplacement.SetToCero()

                            Dim FirstModeIndex As Integer = 0

                            For Each Link In StructuralLinks

                                ' Make a lattice based on the current modal displacement:

                                Dim ModalResponse As ModalCoordinates = Link.ModalResponse(TimeStep)

                                For ModeIndex = 0 To Link.StructuralCore.Modes.Count - 1

                                    NodalDisplacement.X += Results.DynamicModes(ModeIndex + FirstModeIndex).NodalPoint(GlobalIndexNodes).Displacement.X * ModalResponse(ModeIndex).p
                                    NodalDisplacement.Y += Results.DynamicModes(ModeIndex + FirstModeIndex).NodalPoint(GlobalIndexNodes).Displacement.Y * ModalResponse(ModeIndex).p
                                    NodalDisplacement.Z += Results.DynamicModes(ModeIndex + FirstModeIndex).NodalPoint(GlobalIndexNodes).Displacement.Z * ModalResponse(ModeIndex).p

                                Next

                                FirstModeIndex += Link.StructuralCore.Modes.Count

                            Next

                            _TransitLattice.AddNodalPoint(NodalPoint.OriginalPosition, NodalDisplacement)

                        Next

                        For Each VortexRing In Lattice.VortexRings
                            GlobalIndexRings += 1
                            _TransitLattice.AddPanel(VortexRing.Node(1).IndexG + 1,
                                                         VortexRing.Node(2).IndexG + 1,
                                                         VortexRing.Node(3).IndexG + 1,
                                                         VortexRing.Node(4).IndexG + 1)
                            _TransitLattice.Panel(GlobalIndexRings).Circulation = 0.0
                            _TransitLattice.Panel(GlobalIndexRings).Cp = 0.0
                            _TransitLattice.Panel(GlobalIndexRings).IsSlender = True
                        Next

                    Next

                    _TransitLattice.Mesh.GenerateLattice()
                    Results.TransitLattices.Add(_TransitLattice)

                    TimeStep += 1 'Settings.StructuralSettings.SubSteps

                End While

                Results.TransitLoaded = True

            End If

            Results.Loaded = True

        End Sub

        ''' <summary>
        ''' Read a written step
        ''' </summary>
        ''' <param name="FilePath"></param>
        ''' <remarks></remarks>
        Public Sub ReadFromXML(ByVal FilePath As String)

            If Not File.Exists(FilePath) Then
                MsgBox("Results file not found!")
                Exit Sub
            End If

            Dim reader As XmlReader = XmlReader.Create(FilePath)

            If reader.ReadToFollowing("Solver", "UVLMSolver") Then

                Dim nBLattices As Integer = reader.GetAttribute("BLattices")
                Dim nLinks As Integer = reader.GetAttribute("Links")

                _StreamVelocity.X = IOXML.ReadDouble(reader, "VX", 1.0)
                _StreamVelocity.Y = IOXML.ReadDouble(reader, "VY", 0.0)
                _StreamVelocity.Z = IOXML.ReadDouble(reader, "VZ", 0.0)
                _StreamOmega.X = IOXML.ReadDouble(reader, "OX", 0.0)
                _StreamOmega.Y = IOXML.ReadDouble(reader, "OY", 0.0)
                _StreamOmega.Z = IOXML.ReadDouble(reader, "OZ", 0.0)
                _StreamDensity = IOXML.ReadDouble(reader, "Rho", 1.225)
                If _StreamDensity = 0 Then _StreamDensity = 1.225
                _StreamDynamicPressure = 0.5 * _StreamVelocity.SquareEuclideanNorm * _StreamDensity

                Try
                    If File.Exists(FilePath & ".Polars.bin") Then
                        If IsNothing(PolarDataBase) Then
                            PolarDataBase = New PolarDatabase
                        End If
                        PolarDataBase.ReadBinary(FilePath & ".Polars.bin")
                    End If
                Catch ex As Exception
                    PolarDataBase = Nothing
                End Try

                For i = 1 To nBLattices
                    Lattices.Add(New BoundedLattice())
                    Lattices(i - 1).ReadBinary(FilePath & String.Format(".lat_{0}.bin", i), PolarDataBase)
                Next

                If (nLinks > 0) Then

                    Dim NodalStak As New List(Of Node)
                    Dim RingStak As New List(Of VortexRing)

                    Dim nIndex As Integer = 0
                    Dim eIndex As Integer = 0

                    For Each Lattice In Lattices
                        For Each Node In Lattice.Nodes
                            Node.IndexG = nIndex
                            NodalStak.Add(Node)
                            nIndex += 1
                        Next
                        For Each Ring In Lattice.VortexRings
                            Ring.IndexG = eIndex
                            RingStak.Add(Ring)
                            eIndex += 1
                        Next
                    Next

                    StructuralLinks = New List(Of StructuralLink)

                    For i = 1 To nLinks
                        StructuralLinks.Add(New StructuralLink())
                        StructuralLinks(i - 1).ReadBinary(FilePath & String.Format(".link_{0}.bin", i), NodalStak, RingStak)
                    Next

                End If

                If reader.ReadToFollowing("Simulacion", "TSimulacion") Then
                    Settings.ReadFromXML(reader.ReadSubtree)
                Else
                    MsgBox("Warning! Unable to read simulation parameters.")
                End If

            End If

            reader.Close()

            Settings.GenerateVelocityProfile()

        End Sub

        ''' <summary>
        ''' Writes the current step
        ''' </summary>
        ''' <param name="FilePath"></param>
        ''' <remarks></remarks>
        Public Sub WriteToXML(ByVal FilePath As String, Optional ByVal WakesNodalVelocity As Boolean = False)

            Dim writer As XmlWriter = XmlWriter.Create(FilePath)

            writer.WriteStartElement("Solver", "UVLMSolver")

            writer.WriteAttributeString("VX", _StreamVelocity.X)
            writer.WriteAttributeString("VY", _StreamVelocity.Y)
            writer.WriteAttributeString("VZ", _StreamVelocity.Z)
            writer.WriteAttributeString("OX", _StreamOmega.X)
            writer.WriteAttributeString("OY", _StreamOmega.Y)
            writer.WriteAttributeString("OZ", _StreamOmega.Z)
            writer.WriteAttributeString("Rho", _StreamDensity)

            writer.WriteAttributeString("BLattices", Lattices.Count)

            For i = 1 To Lattices.Count
                Lattices(i - 1).WriteBinary(FilePath & String.Format(".lat_{0}.bin", i), WakesNodalVelocity)
            Next

            If IsNothing(StructuralLinks) Then
                writer.WriteAttributeString("Links", 0)
            Else
                writer.WriteAttributeString("Links", StructuralLinks.Count)
                For i = 1 To StructuralLinks.Count
                    StructuralLinks(i - 1).WriteBinary(FilePath & String.Format(".link_{0}.bin", i))
                Next
            End If

            writer.WriteStartElement("Simulacion", "TSimulacion")
            Settings.SaveToXML(writer)
            writer.WriteEndElement()

            PolarDataBase.WriteBinary(FilePath & ".Polars.bin")

            writer.WriteEndElement()

            writer.Close()

        End Sub

#Region "Sub folders"

        Private Enum DataBaseSection As Byte

            Base = 0
            Structural = 1
            Aeroelastic = 2
            Steady = 3
            Unsteady = 4

        End Enum

        Const DB_File_Structure = "_Structural"
        Const DB_File_Aeroelastic = "_Aeroelastic"
        Const DB_File_Steady = "_Steady"
        Const DB_File_Transit = "_Unsteady"

        Private Base_Path As String
        Private Structure_Path As String
        Private Aeroelastic_Path As String
        Private Steady_Path As String
        Private Transit_Path As String

        ''' <summary>
        ''' Creates the subfolders where results are stored
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateSubFoldersNames(ByVal DataBasePath As String)

            If DataBasePath IsNot Nothing AndAlso DataBasePath <> "" Then

                Dim Base_Path As String = Path.GetDirectoryName(DataBasePath) + "\" + Path.GetFileNameWithoutExtension(DataBasePath)

                Structure_Path = Base_Path + DB_File_Structure
                Aeroelastic_Path = Base_Path + DB_File_Aeroelastic
                Steady_Path = Base_Path + DB_File_Steady
                Transit_Path = Base_Path + Transit_Path

            Else

                RaiseEvent PushMessage("Database path not defined. Cancellation requested.")
                RequestCancellation()

            End If

        End Sub

        Private Sub CreateSubFolder(ByVal DataBaseSection As DataBaseSection)

            Try

                Select Case DataBaseSection

                    Case UVLMSolver.DataBaseSection.Aeroelastic
                        System.IO.Directory.CreateDirectory(Aeroelastic_Path)

                    Case UVLMSolver.DataBaseSection.Steady
                        System.IO.Directory.CreateDirectory(Steady_Path)

                    Case UVLMSolver.DataBaseSection.Structural
                        System.IO.Directory.CreateDirectory(Structure_Path)

                    Case UVLMSolver.DataBaseSection.Unsteady
                        System.IO.Directory.CreateDirectory(Transit_Path)

                End Select

            Catch e As Exception

                RaiseEvent PushMessage("Cannot create subfoders. Cancellation requested.")
                RequestCancellation()

            End Try

        End Sub

        ''' <summary>
        ''' Removes all calculation files from the selected path
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CleanDirectory(ByVal DataBaseSection As DataBaseSection)

            Dim path As String = ""

            Select Case DataBaseSection

                Case UVLMSolver.DataBaseSection.Aeroelastic
                    path = Aeroelastic_Path

                Case UVLMSolver.DataBaseSection.Steady
                    path = Steady_Path

                Case UVLMSolver.DataBaseSection.Structural
                    path = Structure_Path

                Case UVLMSolver.DataBaseSection.Unsteady
                    path = Transit_Path

            End Select

            Try

                Dim Files As String() = System.IO.Directory.GetFiles(path)

                For Each FileName In Files

                    File.Delete(FileName)

                Next

            Catch ex As Exception

            End Try


        End Sub

#End Region

    End Class

End Namespace


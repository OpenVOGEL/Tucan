'#############################################################################
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

'' Standard .NET frameworks dependencies
'-----------------------------------------------------------------------------
Imports System.Runtime.CompilerServices

'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports OpenVOGEL.AeroTools.CalculationModel
Imports OpenVOGEL.AeroTools.CalculationModel.Solver
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Elements
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.DesignTools.VisualModel.Models
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

'#############################################################################
' Unit: Converter
'
' This units does the automatic conversion from the design model into the 
' calculation model. Each component is here translated into a bounded lattice.
' For lifting surfaces:
'   > the wake is loaded at the shedding edge
'   > the chorwise strips are generated using the polars
'   > the structural model is generated based on the nodal partition and 
'     section properties, taking into account the symmetry of the model.
' For fuselages:
'   > the anchors are loaded as slender panels
'   > the kutta condition is imposed at the sheding edge of the last anchor panel
'     by the introduction of a single vortex
' For engine nacelles:
'   > the wake is loaded at the shedding edge (trailing edge)
' Propellers:
'   > the chorwise strips are generated using the polars
'   > the wakes are loaded at the shedding edge (trailing edge) of each blade
'#############################################################################
Namespace VisualModel.Models

    ''' <summary>
    ''' The converter module provides a group of extesions to the Solver to automatically
    ''' load the different kinds of standard surfaces from the design model. 
    ''' </summary>
    Public Module Converter

        ''' <summary>
        ''' Transfers a geometric model to the calculation cell
        ''' </summary>
        ''' <param name="Model">Model to be transferred</param>
        ''' <param name="GenerateStructure">Indicates if a structural link that should be created</param>
        ''' <remarks></remarks>
        <Extension()>
        Public Sub GenerateFromExistingModel(This As Solver,
                                             ByVal Model As DesignModel,
                                             Settings As SimulationSettings,
                                             Optional ByVal GenerateStructure As Boolean = False)

            This.Settings = New SimulationSettings
            This.Settings.Assign(Settings)

            ' Import polar database
            '---------------------------------------------------

            If Not IsNothing(Model.PolarDataBase) Then

                This.PolarDataBase = Model.PolarDataBase.Clone()

            End If

            ' Add lifting surfaces
            '---------------------------------------------------

            If GenerateStructure Then This.StructuralLinks = New List(Of StructuralLink)

            For ObjectIndex = 0 To Model.Objects.Count - 1

                If TypeOf Model.Objects(ObjectIndex) Is LiftingSurface AndAlso Model.Objects(ObjectIndex).IncludeInCalculation Then

                    Dim Wing As LiftingSurface = Model.Objects(ObjectIndex)

                    This.AddLiftingSurface(Wing, False, GenerateStructure, Wing.Symmetric)

                    If Wing.Symmetric Then
                        This.AddLiftingSurface(Wing, True, GenerateStructure, Wing.Symmetric)
                    End If

                End If

            Next

            ' Add fuselages
            '---------------------------------------------------

            For ObjectIndex = 0 To Model.Objects.Count - 1

                If TypeOf Model.Objects(ObjectIndex) Is Fuselage AndAlso Model.Objects(ObjectIndex).IncludeInCalculation Then

                    Dim Body As Fuselage = Model.Objects(ObjectIndex)

                    Dim Lattice As New BoundedLattice

                    This.Lattices.Add(Lattice)

                    For NodeIndex = 0 To Body.NumberOfNodes - 1

                        Lattice.AddNode(Body.Mesh.Nodes(NodeIndex).Position)

                    Next

                    For PanelIndex = 0 To Body.NumberOfPanels - 1

                        Dim Node1 As Integer = Body.Mesh.Panels(PanelIndex).N1
                        Dim Node2 As Integer = Body.Mesh.Panels(PanelIndex).N2
                        Dim Node3 As Integer = Body.Mesh.Panels(PanelIndex).N3
                        Dim Node4 As Integer = Body.Mesh.Panels(PanelIndex).N4
                        Dim Reversed As Boolean = Body.Mesh.Panels(PanelIndex).IsReversed
                        Dim Slender As Boolean = Body.Mesh.Panels(PanelIndex).IsSlender

                        If Body.Mesh.Panels(PanelIndex).IsTriangular Then

                            Lattice.AddVortexRing3(Node1, Node2, Node3, Reversed, Slender)

                        Else

                            Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, Reversed, Slender)

                        End If

                        Lattice.VortexRings(PanelIndex).IsPrimitive = Body.Mesh.Panels(PanelIndex).IsPrimitive

                        ' Add the Kutta vortex at the trailing edge of the anchor
                        ' NOTE: no convection recommended from here due to high chance of spurious velocities

                        If Lattice.VortexRings(PanelIndex).IsPrimitive Then
                            Dim KuttaVortex As New Wake
                            KuttaVortex.Primitive.Nodes.Add(Node2)
                            KuttaVortex.Primitive.Nodes.Add(Node3)
                            KuttaVortex.Primitive.Rings.Add(Lattice.VortexRings.Count - 1)
                            KuttaVortex.CuttingStep = 0
                            Lattice.Wakes.Add(KuttaVortex)
                        End If

                    Next

                End If

            Next

            ' Add jet engine nacelles
            '---------------------------------------------------

            For ObjectIndex = 0 To Model.Objects.Count - 1

                If TypeOf Model.Objects(ObjectIndex) Is JetEngine AndAlso Model.Objects(ObjectIndex).IncludeInCalculation Then

                    Dim Nacelle As JetEngine = Model.Objects(ObjectIndex)

                    This.AddJetEngine(Nacelle)

                End If

            Next

            ' Add propeller blades
            '---------------------------------------------------

            For ObjectIndex = 0 To Model.Objects.Count - 1

                If TypeOf Model.Objects(ObjectIndex) Is Propeller AndAlso Model.Objects(ObjectIndex).IncludeInCalculation Then

                    Dim Prop As Propeller = Model.Objects(ObjectIndex)

                    This.AddPropeller(Prop)

                End If

            Next

            ' Add imported surfaces
            '---------------------------------------------------

            For ObjectIndex = 0 To Model.Objects.Count - 1

                If TypeOf Model.Objects(ObjectIndex) Is ImportedSurface AndAlso Model.Objects(ObjectIndex).IncludeInCalculation Then

                    Dim Body As ImportedSurface = Model.Objects(ObjectIndex)

                    Dim Lattice As New BoundedLattice

                    This.Lattices.Add(Lattice)

                    For NodeIndex = 0 To Body.NumberOfNodes - 1

                        Lattice.AddNode(Body.Mesh.Nodes(NodeIndex).Position)

                    Next

                    For PanelIndex = 0 To Body.NumberOfPanels - 1

                        Dim Node1 As Integer = Body.Mesh.Panels(PanelIndex).N1
                        Dim Node2 As Integer = Body.Mesh.Panels(PanelIndex).N2
                        Dim Node3 As Integer = Body.Mesh.Panels(PanelIndex).N3
                        Dim Node4 As Integer = Body.Mesh.Panels(PanelIndex).N4
                        Dim Reversed As Boolean = Body.Mesh.Panels(PanelIndex).IsReversed
                        Dim Slender As Boolean = Body.Mesh.Panels(PanelIndex).IsSlender

                        If Body.Mesh.Panels(PanelIndex).IsTriangular Then

                            Lattice.AddVortexRing3(Node1, Node2, Node3, Reversed, Slender)

                        Else

                            Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, Reversed, Slender)

                        End If

                        Lattice.VortexRings(PanelIndex).IsPrimitive = Body.Mesh.Panels(PanelIndex).IsPrimitive

                    Next

                End If

            Next

            If This.Lattices.Count = 0 Then

                Throw New Exception("There are no lattices in the calculation model")

            End If

            ' Set global indices in the elements (to access circulation from matrices)
            '-------------------------------------------------------------------------

            This.IndexateLattices()

            ' Find surrounding rings
            '---------------------------------------------------

            This.FindSurroundingRingsGlobally()

            ' Populate wakes with vortices
            '---------------------------------------------------

            For Each Lattice In This.Lattices

                Lattice.PopulateVortices()

            Next

            ' Global inertial properties
            '---------------------------------------------------

            If Settings.AnalysisType = CalculationType.FreeFlight Then

                Dim Inertia As InertialProperties = Model.GetGlobalInertia

                This.Settings.Mass = Inertia.Mass

                This.Settings.CenterOfGravity.X = Inertia.Xcg
                This.Settings.CenterOfGravity.Y = Inertia.Ycg
                This.Settings.CenterOfGravity.Z = Inertia.Zcg

                Inertia.ToMainInertia(This.Settings.InertialBasis,
                                      This.Settings.Ixx,
                                      This.Settings.Iyy,
                                      This.Settings.Izz)

            Else

                This.Settings.InertialBasis.CanonicalBase()

            End If

        End Sub

        ''' <summary>
        ''' Adds a bounded lattice with wakes from a lifting surface.
        ''' </summary>
        ''' <param name="Surface"></param>
        ''' <param name="Mirror"></param>
        ''' <param name="GenerateStructure"></param>
        ''' <remarks></remarks>
        <Extension()>
        Private Sub AddLiftingSurface(This As Solver,
                                      ByRef Surface As LiftingSurface,
                                      Optional ByVal Mirror As Boolean = False,
                                      Optional ByVal GenerateStructure As Boolean = False,
                                      Optional IsSymetric As Boolean = True)

            ' Add nodal points
            '-----------------------------------------

            Dim Lattice As New BoundedLattice

            This.Lattices.Add(Lattice)

            For NodeIndex = 0 To Surface.NumberOfNodes - 1

                Lattice.AddNode(Surface.Mesh.Nodes(NodeIndex).Position)

                If Mirror Then Lattice.Nodes(Lattice.Nodes.Count - 1).Position.Y *= -1

            Next

            ' Add rings
            '-----------------------------------------

            For PanelIndex = 0 To Surface.NumberOfPanels - 1

                Dim Node1 As Integer = Surface.Mesh.Panels(PanelIndex).N1
                Dim Node2 As Integer = Surface.Mesh.Panels(PanelIndex).N2
                Dim Node3 As Integer = Surface.Mesh.Panels(PanelIndex).N3
                Dim Node4 As Integer = Surface.Mesh.Panels(PanelIndex).N4

                Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, False, True)

            Next

            ' Add wakes
            '-----------------------------------------

            If Surface.ConvectWake Then

                Dim Wake As New Wake

                For PrimitiveIndex = Surface.FirstPrimitiveNode To Surface.LastPrimitiveNode
                    Wake.Primitive.Nodes.Add(Surface.GetPrimitiveNodeIndex(PrimitiveIndex) - 1)
                Next

                For PrimitiveIndex = Surface.FirstPrimitiveSegment To Surface.LastPrimitiveSegment
                    Wake.Primitive.Rings.Add(Surface.GetPrimitivePanelIndex(PrimitiveIndex) - 1)
                Next

                Wake.CuttingStep = Surface.CuttingStep

                Wake.SupressInnerCircuation = IsSymetric

                Lattice.Wakes.Add(Wake)

            End If

            ' Generate structural link
            '-----------------------------------------

            If Surface.IncludeStructure And GenerateStructure Then

                Dim KineLink As KinematicLink
                Dim MechaLink As MechanicLink
                Dim NodeCount As Integer = 0
                Dim ElementCount As Integer = -1

                ' Add root node (this node is being clamped, and it is the only one with contrains at the moment):

                Dim StructuralLink As New StructuralLink

                StructuralLink.StructuralCore.StructuralSettings.Assign(This.Settings.StructuralSettings)
                StructuralLink.StructuralCore.Nodes.Add(New StructuralNode(NodeCount))
                StructuralLink.StructuralCore.Nodes(NodeCount).Position.Assign(Surface.StructuralPartition(0).P)
                If (Mirror) Then StructuralLink.StructuralCore.Nodes(NodeCount).Position.Y *= -1
                StructuralLink.StructuralCore.Nodes(NodeCount).Contrains.Clamped()

                ' Add kinematic link

                Dim LinkedVortexIndex As Integer = -1 ' > linked vortex ring
                Dim LinkedNodeIndex As Integer = -1   ' > linked node

                KineLink = New KinematicLink(StructuralLink.StructuralCore.Nodes(NodeCount))
                For n = 0 To Surface.NumberOfChordPanels
                    LinkedNodeIndex += 1
                    KineLink.Link(Lattice.Nodes(LinkedNodeIndex))
                Next
                StructuralLink.KinematicLinks.Add(KineLink)

                ' Add rest of the nodes and elements:

                For PartitionNodeIndex = 1 To Surface.StructuralPartition.Count - 1

                    ' Add node:

                    NodeCount += 1

                    StructuralLink.StructuralCore.Nodes.Add(New StructuralNode(NodeCount))
                    StructuralLink.StructuralCore.Nodes(NodeCount).Position.Assign(Surface.StructuralPartition(PartitionNodeIndex).P)
                    If (Mirror) Then StructuralLink.StructuralCore.Nodes(NodeCount).Position.Y *= -1

                    ' Add element:

                    ElementCount += 1

                    Dim Element As New ConstantBeamElement(ElementCount)

                    Element.NodeA = StructuralLink.StructuralCore.Nodes(NodeCount - 1)
                    Element.NodeB = StructuralLink.StructuralCore.Nodes(NodeCount)

                    Dim SectionA As Section = Surface.StructuralPartition(PartitionNodeIndex - 1).LocalSection
                    Dim SectionB As Section = Surface.StructuralPartition(PartitionNodeIndex).LocalSection
                    Element.Section.Combine(SectionA, SectionB)
                    If (Mirror) Then Element.Section.Cmz *= -1.0

                    StructuralLink.StructuralCore.Elements.Add(Element)

                    ' Add kinematic link:

                    Dim LeadingEdgeNodeIndex As Integer = LinkedNodeIndex + 1 '(leading edge lattice node index)

                    KineLink = New KinematicLink(StructuralLink.StructuralCore.Nodes(NodeCount))

                    For NodeCounter = 0 To Surface.NumberOfChordPanels
                        LinkedNodeIndex += 1
                        KineLink.Link(Lattice.Nodes(LinkedNodeIndex))
                    Next

                    StructuralLink.KinematicLinks.Add(KineLink)

                    Dim TrailingEdgeNodeIndex As Integer = LinkedNodeIndex '(trailing edge lattice node index)

                    ' Add mechanic link:

                    MechaLink = New MechanicLink(Element)

                    For PanelCounter = 0 To Surface.NumberOfChordPanels - 1
                        LinkedVortexIndex += 1
                        MechaLink.Link(Lattice.VortexRings(LinkedVortexIndex))
                    Next

                    StructuralLink.MechanicLinks.Add(MechaLink)

                    ' Find chordwise vector

                    Dim ChordVector As New Vector3
                    ChordVector.X = Lattice.Nodes(TrailingEdgeNodeIndex).Position.X - Lattice.Nodes(LeadingEdgeNodeIndex).Position.X
                    ChordVector.Y = Lattice.Nodes(TrailingEdgeNodeIndex).Position.Y - Lattice.Nodes(LeadingEdgeNodeIndex).Position.Y
                    ChordVector.Z = Lattice.Nodes(TrailingEdgeNodeIndex).Position.Z - Lattice.Nodes(LeadingEdgeNodeIndex).Position.Z

                    ' Build the element orthonormal basis:

                    ' NOTE: U has the direction of the element

                    Element.Basis.U.X = Element.NodeB.Position.X - Element.NodeA.Position.X
                    Element.Basis.U.Y = Element.NodeB.Position.Y - Element.NodeA.Position.Y
                    Element.Basis.U.Z = Element.NodeB.Position.Z - Element.NodeA.Position.Z
                    Element.Basis.U.Normalize()

                    ' NOTE: W is normal to the surface

                    Element.Basis.W.FromVectorProduct(ChordVector, Element.Basis.U)
                    Element.Basis.W.Normalize()

                    ' NOTE: V is normal to W and U, and points to the trailing edge for the 
                    ' original part and to the leading edge on the mirror.
                    ' That is why CMy is opposed for the mirror.

                    Element.Basis.V.FromVectorProduct(Element.Basis.W, Element.Basis.U)

                Next

                ' Add this structural link to the list:

                This.StructuralLinks.Add(StructuralLink)

            End If

            ' Load chordwise stripes (for skin drag computation)

            Dim VortexRingIndex As Integer = 0

            For RegionIndex = 0 To Surface.WingRegions.Count - 1

                Dim Region As WingRegion = Surface.WingRegions(RegionIndex)

                'If (Not IsNothing(PolarDataBase)) And PolarDataBase.Polars.Count > 0 Then

                For SpanPanelIndex = 1 To Region.SpanPanelsCount

                    Dim Stripe As New ChorwiseStripe()

                    Stripe.Polars = Region.PolarFamiliy

                    For PanelCount = 1 To Surface.NumberOfChordPanels
                        Stripe.Rings.Add(Lattice.VortexRings(VortexRingIndex))
                        VortexRingIndex += 1
                    Next

                    Lattice.ChordWiseStripes.Add(Stripe)

                Next

                'End If

            Next

        End Sub

        ''' <summary>
        ''' Adds a bounded lattice with wakes from a propeller.
        ''' </summary>
        <Extension()>
        Private Sub AddPropeller(This As Solver,
                                 ByRef Surface As Propeller)

            Dim NumberOfChordNodes As Integer = Surface.NumberOfChordPanels + 1
            Dim NumberOfSpanNodes As Integer = Surface.NumberOfSpanPanels + 1
            Dim NumberOfBladeNodes As Integer = NumberOfChordNodes * NumberOfSpanNodes
            Dim NumberOfBladePanels As Integer = Surface.NumberOfChordPanels * Surface.NumberOfSpanPanels

            Dim NodeIndex As Integer = 0
            Dim PanelIndex As Integer = 0
            Dim NodeOffset As Integer = 0

            For K = 1 To Surface.NumberOfBlades

                Dim Lattice As New BoundedLattice

                This.Lattices.Add(Lattice)

                Dim Wake As New Wake

                Lattice.Wakes.Add(Wake)

                Wake.CuttingStep = Surface.CuttingStep

                Wake.SupressInnerCircuation = True

                ' Add nodal points
                '-----------------------------------------

                Dim N As Integer = 0

                For I = 1 To NumberOfSpanNodes

                    For J = 1 To NumberOfChordNodes

                        Lattice.AddNode(Surface.Mesh.Nodes(NodeIndex).Position)

                        If J = NumberOfChordNodes Then

                            Wake.Primitive.Nodes.Add(N)

                        End If

                        N += 1
                        NodeIndex += 1

                    Next

                Next

                ' Add rings
                '-----------------------------------------

                Dim P As Integer = 0

                For I = 1 To Surface.NumberOfSpanPanels

                    For J = 1 To Surface.NumberOfChordPanels

                        Dim Node1 As Integer = Surface.Mesh.Panels(PanelIndex).N1 - NodeOffset
                        Dim Node2 As Integer = Surface.Mesh.Panels(PanelIndex).N2 - NodeOffset
                        Dim Node3 As Integer = Surface.Mesh.Panels(PanelIndex).N3 - NodeOffset
                        Dim Node4 As Integer = Surface.Mesh.Panels(PanelIndex).N4 - NodeOffset

                        Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, Surface.Mesh.Panels(PanelIndex).IsReversed, True)

                        If J = Surface.NumberOfChordPanels Then

                            Wake.Primitive.Rings.Add(P)

                        End If

                        P += 1
                        PanelIndex += 1

                    Next

                Next

                ' Load chordwise stripes (for skin drag computation)
                '-----------------------------------------

                Dim VortexRingIndex As Integer = 0

                For I = 1 To Surface.NumberOfSpanPanels

                    Dim Stripe As New ChorwiseStripe()

                    Stripe.Polars = Surface.PolarFamiliy

                    For J = 1 To Surface.NumberOfChordPanels

                        Stripe.Rings.Add(Lattice.VortexRings(VortexRingIndex))
                        VortexRingIndex += 1

                    Next

                    Lattice.ChordWiseStripes.Add(Stripe)

                Next

                NodeOffset += NumberOfBladeNodes

            Next

        End Sub

        ''' <summary>
        ''' Includes the model of a jet engine in the calculation core
        ''' </summary>
        <Extension()>
        Private Sub AddJetEngine(This As Solver, ByRef Nacelle As JetEngine)

            Dim Lattice As New BoundedLattice

            This.Lattices.Add(Lattice)

            For j = 0 To Nacelle.NumberOfNodes - 1

                Lattice.AddNode(Nacelle.Mesh.Nodes(j).Position)

            Next

            For j = 0 To Nacelle.NumberOfPanels - 1

                Dim Node1 As Integer = Nacelle.Mesh.Panels(j).N1
                Dim Node2 As Integer = Nacelle.Mesh.Panels(j).N2
                Dim Node3 As Integer = Nacelle.Mesh.Panels(j).N3
                Dim Node4 As Integer = Nacelle.Mesh.Panels(j).N4

                Lattice.AddVortexRing4(Node1, Node2, Node3, Node4, False, True)

            Next

            ' Add wakes:

            If Nacelle.ConvectWake And Nacelle.CuttingStep > 0 Then

                Dim Wake As New AeroTools.CalculationModel.Models.Aero.Wake

                Wake.SupressInnerCircuation = False

                For k = 0 To Nacelle.Resolution
                    Wake.Primitive.Nodes.Add(Nacelle.NumberOfNodes + k - Nacelle.Resolution - 1)
                Next

                Wake.Primitive.Nodes.Add(Nacelle.NumberOfNodes - Nacelle.Resolution - 1)

                For k = 0 To Nacelle.Resolution
                    Wake.Primitive.Rings.Add(Nacelle.NumberOfPanels + k - Nacelle.Resolution - 1)
                Next

                Wake.CuttingStep = Nacelle.CuttingStep

                Lattice.Wakes.Add(Wake)

            End If

        End Sub

    End Module

End Namespace


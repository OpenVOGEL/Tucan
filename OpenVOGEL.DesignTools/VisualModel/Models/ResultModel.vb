'Open VOGEL (openvogel.org)
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

Imports OpenVOGEL.AeroTools.CalculationModel
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.MathTools.Integration

Namespace VisualModel.Models

    Public Enum ResultFrameKinds

        Transit = 1
        EndState = 2
        DynamicMode = 3

    End Enum

    ''' <summary>
    ''' Represents the latties at a single transit state
    ''' </summary>
    Public Class ResultFrame

        ''' <summary>
        ''' The model
        ''' </summary>
        Public ReadOnly Property Model As ResultContainer

        ''' <summary>
        ''' The wakes shed from the model
        ''' </summary>
        Public ReadOnly Property Wakes As ResultContainer

        ''' <summary>
        ''' The stream velocity for this frame
        ''' </summary>
        Public ReadOnly Property StreamVelocity As New Vector3

        ''' <summary>
        ''' The stream velocity for this frame
        ''' </summary>
        Public ReadOnly Property StreamRotation As New Vector3

        ''' <summary>
        ''' Contains the airloads of each component
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TotalAirLoads As New AirLoads

        ''' <summary>
        ''' The airloads for each component of the model
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PartialAirLoads As New List(Of PartialAirLoads)

        ''' <summary>
        ''' The orientation of the model in the inertial reference frame.
        ''' This property is assigned in free flight.
        ''' </summary>
        Public ReadOnly Property Orientation As New OrientationAngles

        ''' <summary>
        ''' The position the model respect to the original position in the inertial reference frame.
        ''' This property is assigned in free flight.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Position As New Vector3

        ''' <summary>
        ''' The kind of frame
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FrameKind As ResultFrameKinds

        Public Sub New(Kind As ResultFrameKinds,
                       Velocity As Vector3, Rotation As Vector3,
                       ModelVisuals As VisualProperties,
                       WakesVisuals As VisualProperties)

            _FrameKind = Kind

            _Model = New ResultContainer(ModelVisuals)
            _Wakes = New ResultContainer(WakesVisuals)

            _Model.Name = "Full_Model"
            _Model.VisualProperties.ColorMesh = System.Drawing.Color.DimGray
            _Model.VisualProperties.ColorSurface = System.Drawing.Color.Orange
            _Model.VisualProperties.Transparency = 1.0
            _Model.VisualProperties.ShowSurface = True
            _Model.VisualProperties.ShowMesh = True
            _Model.VisualProperties.ShowNodes = False
            _Model.VisualProperties.ThicknessMesh = 0.8
            _Model.VisualProperties.ShowNodes = False
            _Model.ActiveResult = ResultContainer.ResultKinds.PanelPressure

            _Wakes.Name = "All_Wakes"
            _Wakes.VisualProperties.ColorMesh = System.Drawing.Color.Silver
            _Wakes.VisualProperties.ColorSurface = System.Drawing.Color.LightBlue
            _Wakes.VisualProperties.ColorNodes = Drawing.Color.Black
            _Wakes.VisualProperties.Transparency = 1.0
            _Wakes.VisualProperties.ShowSurface = False
            _Wakes.VisualProperties.ShowMesh = False
            _Wakes.VisualProperties.ThicknessMesh = 0.8
            _Wakes.VisualProperties.SizeNodes = 3.0#
            _Wakes.VisualProperties.ShowNodes = True
            _Wakes.ActiveResult = ResultContainer.ResultKinds.None

            _Model.Clear()
            _Wakes.Clear()

            If Velocity IsNot Nothing Then
                _StreamVelocity.Assign(Velocity)
            End If

            If Rotation IsNot Nothing Then
                _StreamRotation.Assign(Rotation)
            End If

        End Sub

    End Class

    ''' <summary>
    ''' Stores results for a given time step.
    ''' </summary>
    Public Class ResultModel

        ''' <summary>
        ''' Creates a new empty result model
        ''' </summary>
        Public Sub New()
            _Frames = New List(Of ResultFrame)
            _Settings = New SimulationSettings
        End Sub

        ''' <summary>
        ''' Name of the model
        ''' </summary>
        Public Name As String

        ''' <summary>
        ''' The active transit
        ''' </summary>
        Public Property ActiveFrame As ResultFrame

        ''' <summary>
        ''' Contains all the transit states
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Frames As List(Of ResultFrame)

        ''' <summary>
        ''' The visual properties for the model
        ''' </summary>
        Private ModelVisuals As New VisualProperties(ComponentTypes.etResultContainer)

        ''' <summary>
        ''' ''' The visual properties for the wakes
        ''' </summary>
        Private WakesVisuals As New VisualProperties(ComponentTypes.etResultContainer)

        ''' <summary>
        ''' The simulation settings that where used for this results.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Settings As SimulationSettings

        ''' <summary>
        ''' The collection of polars that where used for the analysis
        ''' </summary>
        Public ReadOnly Property PolarDatabase As PolarDatabase

        ''' <summary>
        ''' The simulated motion of the aircraft (if any)
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FreeMotion As FreeMotionResults

        ''' <summary>
        ''' The modal response of the wings (if any)
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AeroelasticResult As AeroelasticResults

        Public Sub Clear()

            _Frames.Clear()
            _Settings = Nothing
            _PolarDatabase = Nothing
            _FreeMotion = Nothing
            _AeroelasticResult = Nothing
            _ActiveFrame = Nothing

        End Sub

        ''' <summary>
        ''' Sets the lattices on the result object
        ''' </summary>
        Public Sub LoadFromDirectory(DirectoryPath As String)

            Frames.Clear()

            ' Read polars database
            '----------------------------------------

            _PolarDatabase = New PolarDatabase
            PolarDatabase.ReadBinary(IO.Path.Combine(DirectoryPath, "Polars.bin"))

            ' Read settings
            '----------------------------------------

            _Settings = New SimulationSettings
            Settings.ReadFromXML(IO.Path.Combine(DirectoryPath, "Settings.xml"))

            If Settings.AnalysisType = CalculationType.Aeroelastic Then
                Settings.AeroelasticHistogram.Generate(Settings.StreamVelocity,
                                                       Settings.Interval,
                                                       Settings.SimulationSteps)
            End If

            ' Read motion
            '----------------------------------------

            Dim MotionData As MotionIntegrator = Nothing

            Dim MotionFilePath As String = IO.Path.Combine(DirectoryPath, "Motion.bin")
            If System.IO.File.Exists(MotionFilePath) Then
                MotionData = New MotionIntegrator(MotionFilePath)
            End If

            ' Read frames and lattices 
            '----------------------------------------

            Dim FrameIndex As Integer = 0
            Dim KeepReadingFrames As Boolean = True

            While KeepReadingFrames

                ' Try loading the lattices for this frame
                '-----------------------------------------------

                Dim Lattices As List(Of BoundedLattice) = Nothing

                Solver.Solver.ReadLattices(DirectoryPath, FrameIndex, Lattices, PolarDatabase)

                If Lattices Is Nothing Then

                    KeepReadingFrames = False
                    Exit While

                End If

                ' Try loading the modes and the modal response
                '-----------------------------------------------

                If FrameIndex = 0 Then

                    Dim StructuralLinks As List(Of StructuralLink) = Nothing

                    Solver.Solver.ReadStructuralLinks(DirectoryPath, Lattices, StructuralLinks)

                    If StructuralLinks IsNot Nothing Then

                        LoadDynamicModes(Lattices, StructuralLinks)

                        ' Extract the modal response
                        '-----------------------------------------------

                        _AeroelasticResult = New AeroelasticResults

                        For Each Link In StructuralLinks

                            Dim NumberOfModes As Integer = Link.StructuralCore.Modes.Count
                            Dim LinkResult As New AeroelasticLinkResults

                            For I = 0 To NumberOfModes - 1
                                Dim ModalResult As New ModalResults
                                ModalResult.K = Link.StructuralCore.Modes(I).K
                                ModalResult.M = Link.StructuralCore.Modes(I).M
                                ModalResult.W = Link.StructuralCore.Modes(I).W
                                ModalResult.C = Link.StructuralCore.Modes(I).C
                                For J = 0 To Link.ModalResponse.Count - 1
                                    Dim ModalReponse As New ModalResponse
                                    ModalReponse.P = Link.ModalResponse(J).Item(I).P
                                    ModalReponse.V = Link.ModalResponse(J).Item(I).V
                                    ModalResult.Response.Add(ModalReponse)
                                Next
                                LinkResult.Modes.Add(ModalResult)
                            Next

                            AeroelasticResult.Links.Add(LinkResult)

                        Next

                    End If

                End If

                ' Load the frame
                '-----------------------------------------------

                Dim FrameKind As ResultFrameKinds = ResultFrameKinds.EndState

                If Settings.AnalysisType = CalculationType.Aeroelastic Or
                   Settings.AnalysisType = CalculationType.FreeFlight Then
                    FrameKind = ResultFrameKinds.Transit
                End If

                Dim Frame As New ResultFrame(FrameKind,
                                             Settings.StreamVelocity,
                                             Settings.StreamRotation,
                                             ModelVisuals, WakesVisuals)

                Frame.Model.Name = "Frame " & FrameIndex

                Frames.Add(Frame)

                ' Load lattices to frame
                '-----------------------------------------------

                LoadLatticesToFrame(Frame, Lattices)

                ' Complete the frame info
                '-----------------------------------------------

                Select Case Settings.AnalysisType

                    Case CalculationType.SteadyState

                        Frame.StreamVelocity.Assign(Settings.StreamVelocity)
                        Frame.StreamRotation.Assign(Settings.StreamRotation)

                    Case CalculationType.FreeFlight

                        If MotionData IsNot Nothing Then

                            Dim State As Variable = MotionData.State(FrameIndex)
                            Frame.StreamVelocity.X = -State.Vx
                            Frame.StreamVelocity.Y = -State.Vy
                            Frame.StreamVelocity.Z = -State.Vz
                            Frame.StreamVelocity.AntiTransform(Settings.InertialBasis)
                            Frame.StreamRotation.X = -State.Ox
                            Frame.StreamRotation.Y = -State.Oy
                            Frame.StreamRotation.Z = -State.Oz
                            Frame.StreamRotation.AntiTransform(Settings.InertialBasis)

                            ' Assign the orientation in Tait-Bryan angles
                            ' (yaw, pitch, roll)
                            '------------------------------------------------

                            Dim I As New Vector3
                            I.X = State.Ix
                            I.Y = State.Iy
                            I.Z = State.Iz
                            I.AntiTransform(Settings.InertialBasis)

                            Dim J As New Vector3
                            J.X = State.Jx
                            J.Y = State.Jy
                            J.Z = State.Jz
                            J.AntiTransform(Settings.InertialBasis)

                            Frame.Orientation.Sequence = RotationSequence.XYZ
                            Frame.Orientation.R1 = Math.Asin(I.Y / Math.Sqrt(1 - I.Z ^ 2))
                            Frame.Orientation.R2 = Math.Asin(-I.Z)
                            Frame.Orientation.R3 = Math.Asin(J.Z / Math.Sqrt(1 - I.Z ^ 2))
                            Frame.Orientation.ToDegrees()

                            ' Load position
                            '--------------------------------------------------

                            Frame.Position.X = State.Px
                            Frame.Position.Y = State.Py
                            Frame.Position.Z = State.Pz

                            Frame.Position.AntiTransform(Settings.InertialBasis)

                        End If

                    Case CalculationType.Aeroelastic

                        Dim Offset As Integer = FrameIndex + Settings.StructuralSettings.StructuralLinkingStep
                        Frame.StreamVelocity.Assign(Settings.AeroelasticHistogram.State(Offset).Velocity)
                        Frame.StreamRotation.Assign(Settings.StreamRotation)

                End Select

                Frame.TotalAirLoads.Area = 1.0#
                Frame.TotalAirLoads.Length = 1.0#
                Frame.TotalAirLoads.DynamicPressure = 0.5 * Settings.Density * Frame.StreamVelocity.SquareEuclideanNorm
                Frame.TotalAirLoads.Alfa = Math.Atan2(Frame.StreamVelocity.Z, Frame.StreamVelocity.X)
                Frame.TotalAirLoads.Beta = Math.Atan2(Frame.StreamVelocity.Y, Frame.StreamVelocity.X)

                FrameIndex += 1

            End While

            ' Transform the position to the inertial reference frame
            ' Todo: check this
            '-------------------------------------------------------

            If MotionData IsNot Nothing Then

                Dim State As Variable = MotionData.State(FrameIndex - 1)

                Dim Basis As New Base3

                Basis.U.X = State.Ix
                Basis.U.Y = State.Iy
                Basis.U.Z = State.Iz
                Basis.U.AntiTransform(Settings.InertialBasis)

                Basis.V.X = State.Jx
                Basis.V.Y = State.Jy
                Basis.V.Z = State.Jz
                Basis.V.AntiTransform(Settings.InertialBasis)

                Basis.W.FromVectorProduct(Basis.U, Basis.V)

                For Each Frame In Frames
                    Frame.Position.Transform(Basis)
                Next

            End If

            If Frames.Count > 0 Then
                ActiveFrame = Frames.Last
            Else
                ActiveFrame = Nothing
            End If

        End Sub

        ''' <summary>
        ''' Loads the dynamic modes using the given lattices and structural links
        ''' </summary>
        ''' <param name="Lattices"></param>
        ''' <param name="StructuralLinks"></param>
        Private Sub LoadDynamicModes(ByRef Lattices As List(Of BoundedLattice),
                                    ByRef StructuralLinks As List(Of StructuralLink))

            Dim GlobalIndexNodes As Integer = -1
            Dim GlobalIndexRings As Integer = -1

            If StructuralLinks IsNot Nothing Then

                Dim Modes As New List(Of ResultContainer)

                For Each Link As StructuralLink In StructuralLinks

                    If Link.StructuralCore.Modes IsNot Nothing Then

                        For Each Mode As Mode In Link.StructuralCore.Modes

                            Dim ModelShapeFrame As New ResultFrame(ResultFrameKinds.DynamicMode,
                                                                   Nothing, Nothing,
                                                                   ModelVisuals, WakesVisuals)
                            Frames.Add(ModelShapeFrame)

                            Dim ModalShapeModel As ResultContainer = ModelShapeFrame.Model
                            Modes.Add(ModalShapeModel)

                            ModalShapeModel.Name = String.Format("Mode {0} - {1:F3}Hz", Mode.Index, Mode.W / (2 * Math.PI))
                            ModalShapeModel.VisualProperties.ColorMesh = Drawing.Color.Maroon
                            ModalShapeModel.VisualProperties.ColorSurface = Drawing.Color.Orange
                            ModalShapeModel.VisualProperties.Transparency = 1.0
                            ModalShapeModel.VisualProperties.ShowSurface = True
                            ModalShapeModel.VisualProperties.ShowMesh = True
                            ModalShapeModel.VisualProperties.ShowNodes = False
                            ModalShapeModel.VisualProperties.ThicknessMesh = 0.8
                            ModalShapeModel.VisualProperties.ShowNodes = False
                            ModalShapeModel.VisualProperties.ShowLoadVectors = False
                            ModalShapeModel.VisualProperties.ShowVelocityVectors = False
                            ModalShapeModel.VisualProperties.ShowColormap = True

                            ' Reset all displacements
                            '-----------------------------------------------------------

                            For Each OtherLink As StructuralLink In StructuralLinks
                                OtherLink.StructuralCore.ResetDisplacements()
                                For Each kl As KinematicLink In OtherLink.KinematicLinks
                                    kl.TransferMotion()
                                Next
                            Next

                            ' Load the displacement associated with the current mode
                            '-----------------------------------------------------------

                            Link.StructuralCore.TransferModeShapeToNodes(Mode.Index, 1.0)

                            For Each kl As KinematicLink In Link.KinematicLinks

                                kl.TransferMotion()

                            Next

                            ' Make a lattice based on the current modal displacement
                            '-----------------------------------------------------------

                            GlobalIndexNodes = -1
                            GlobalIndexRings = -1

                            For Each Lattice In Lattices

                                For Each NodalPoint In Lattice.Nodes

                                    NodalPoint.IndexG = GlobalIndexNodes
                                    GlobalIndexNodes += 1
                                    ModalShapeModel.AddNodalPoint(NodalPoint.OriginalPosition, NodalPoint.Displacement)

                                Next

                                ModalShapeModel.UpdateDisplacement()

                                For Each VortexRing In Lattice.VortexRings

                                    GlobalIndexRings += 1

                                    ModalShapeModel.AddPanel(VortexRing.Node(1).IndexG + 1,
                                                             VortexRing.Node(2).IndexG + 1,
                                                             VortexRing.Node(3).IndexG + 1,
                                                             VortexRing.Node(4).IndexG + 1)

                                    ModalShapeModel.Mesh.Panels(GlobalIndexRings).Circulation = 0.0
                                    ModalShapeModel.Mesh.Panels(GlobalIndexRings).Cp = 0.0
                                    ModalShapeModel.Mesh.Panels(GlobalIndexRings).IsSlender = True

                                Next

                            Next

                            ModalShapeModel.ActiveResult = ResultContainer.ResultKinds.NodalDisplacement
                            ModalShapeModel.Mesh.GenerateLattice()
                            ModalShapeModel.FindDisplacementsRange()
                            ModalShapeModel.UpdateColormapWithDisplacements()

                        Next

                    End If

                Next

            End If

        End Sub

        ''' <summary>
        ''' Loads all the lattices in a new frame
        ''' </summary>
        ''' <param name="Lattices"></param>
        Private Sub LoadLatticesToFrame(ByRef Frame As ResultFrame,
                                        ByRef Lattices As List(Of BoundedLattice))

            Dim GlobalIndexNodes As Integer = -1
            Dim GlobalIndexRings As Integer = -1

            For Each Lattice In Lattices

                '-----------------------'
                ' Load the nodal points '
                '-----------------------'

                For Each NodalPoint In Lattice.Nodes

                    GlobalIndexNodes += 1
                    NodalPoint.IndexG = GlobalIndexNodes
                    NodalPoint.Position.AntiTransform(Settings.InertialBasis)
                    Frame.Model.AddNodalPoint(NodalPoint.Position)

                Next

                '-----------------------'
                ' Load the vortex rings '
                '-----------------------'

                For Each VortexRing In Lattice.VortexRings

                    GlobalIndexRings += 1

                    If VortexRing.Type = VortexRingType.VR4 Then

                        Frame.Model.AddPanel(VortexRing.Node(1).IndexG,
                                             VortexRing.Node(2).IndexG,
                                             VortexRing.Node(3).IndexG,
                                             VortexRing.Node(4).IndexG)

                    Else

                        Frame.Model.AddPanel(VortexRing.Node(1).IndexG,
                                             VortexRing.Node(2).IndexG,
                                             VortexRing.Node(3).IndexG,
                                             VortexRing.Node(1).IndexG)

                    End If

                    VortexRing.Normal.AntiTransform(Settings.InertialBasis)
                    VortexRing.VelocityT.AntiTransform(Settings.InertialBasis)
                    VortexRing.ControlPoint.AntiTransform(Settings.InertialBasis)

                    Frame.Model.Mesh.Panels(GlobalIndexRings).Circulation = VortexRing.G
                    Frame.Model.Mesh.Panels(GlobalIndexRings).SourceStrength = VortexRing.S
                    Frame.Model.Mesh.Panels(GlobalIndexRings).Cp = VortexRing.Cp
                    Frame.Model.Mesh.Panels(GlobalIndexRings).Area = VortexRing.Area
                    Frame.Model.Mesh.Panels(GlobalIndexRings).NormalVector.Assign(VortexRing.Normal)
                    Frame.Model.Mesh.Panels(GlobalIndexRings).LocalVelocity.Assign(VortexRing.VelocityT)
                    Frame.Model.Mesh.Panels(GlobalIndexRings).ControlPoint.Assign(VortexRing.ControlPoint)
                    Frame.Model.Mesh.Panels(GlobalIndexRings).IsSlender = VortexRing.IsSlender

                Next

                '------------------------'
                ' Load the airloads      '
                '------------------------'

                Dim Loads As New PartialAirLoads
                Frame.PartialAirLoads.Add(Loads)
                Loads.Name = Lattice.Name

                Lattice.AirLoads.AntiTransform(Settings.InertialBasis)
                Loads.AirLoads = Lattice.AirLoads
                Frame.TotalAirLoads.Add(Lattice.AirLoads)

                '------------------------'
                ' Load the fixed vectors '
                '------------------------'

                For Each Stripe In Lattice.ChordWiseStripes

                    Stripe.CenterPoint.AntiTransform(Settings.InertialBasis)
                    Stripe.ChordWiseVector.AntiTransform(Settings.InertialBasis)

                    Stripe.Lift.AntiTransform(Settings.InertialBasis)
                    Stripe.InducedDrag.AntiTransform(Settings.InertialBasis)
                    Stripe.SkinDrag.AntiTransform(Settings.InertialBasis)

                    Stripe.LiftMoment.AntiTransform(Settings.InertialBasis)
                    Stripe.InducedDragMoment.AntiTransform(Settings.InertialBasis)
                    Stripe.SkinDragMoment.AntiTransform(Settings.InertialBasis)

                    ' Lift vectors
                    '-----------------------------------
                    If Stripe.Lift.EuclideanNorm > 0.0 Then
                        Dim LiftVector As New FixedVector
                        LiftVector.Vector.Assign(Stripe.Lift)
                        LiftVector.Vector.Normalize()
                        LiftVector.Vector.Scale(Stripe.LiftCoefficient)
                        LiftVector.Point.Assign(Stripe.CenterPoint)
                        Loads.LiftVectors.Add(LiftVector)
                        Loads.MaximumLift = Math.Max(Loads.MaximumLift, Stripe.LiftCoefficient)
                    End If

                    ' Induced drag vectors
                    '-----------------------------------
                    If Stripe.InducedDrag.EuclideanNorm > 0.0 Then
                        Dim DragVector As New FixedVector
                        DragVector.Vector.Assign(Stripe.InducedDrag)
                        DragVector.Vector.Normalize()
                        DragVector.Vector.Scale(Stripe.InducedDragCoefficient)
                        DragVector.Point.Assign(Stripe.CenterPoint)
                        Loads.InducedDragVectors.Add(DragVector)
                        Loads.MaximumInducedDrag = Math.Max(Loads.MaximumInducedDrag, Stripe.InducedDragCoefficient)
                    End If

                    ' Skin drag  vectors
                    '-----------------------------------
                    If Stripe.SkinDrag.EuclideanNorm > 0.0 Then
                        Dim DragVector As New FixedVector
                        DragVector.Vector.Normalize()
                        DragVector.Vector.Scale(Stripe.SkinDragCoefficient)
                        DragVector.Vector.Assign(Stripe.SkinDrag)
                        DragVector.Point.Assign(Stripe.CenterPoint)
                        Loads.SkinDragVectors.Add(DragVector)
                        Loads.MaximumSkinDrag = Math.Max(Loads.MaximumSkinDrag, Stripe.SkinDragCoefficient)
                    End If

                Next

            Next

            Frame.Model.FindPressureRange()
            Frame.Model.UpdatePressureColormap()
            Frame.Model.VisualProperties.ShowSurface = True
            Frame.Model.VisualProperties.ShowColormap = True
            Frame.Model.VisualProperties.ShowVelocityVectors = True
            Frame.Model.VisualProperties.ShowMesh = True
            Frame.Model.FindBestVelocityScale()

            Frame.Model.Mesh.GenerateLattice()

            '------------------------'
            ' Load the wakes         '
            '------------------------'

            GlobalIndexNodes = -1
            GlobalIndexRings = -1
            Frame.Wakes.Clear()

            For Each Lattice In Lattices

                For Each Wake In Lattice.Wakes

                    For Each NodalPoint In Wake.Nodes

                        GlobalIndexNodes += 1
                        NodalPoint.IndexG = GlobalIndexNodes
                        NodalPoint.Position.AntiTransform(Settings.InertialBasis)
                        Frame.Wakes.AddNodalPoint(NodalPoint.Position)

                    Next

                    For Each VortexRing In Wake.VortexRings

                        GlobalIndexRings += 1
                        Frame.Wakes.AddPanel(VortexRing.Node(1).IndexG,
                                               VortexRing.Node(2).IndexG,
                                               VortexRing.Node(3).IndexG,
                                               VortexRing.Node(4).IndexG)
                        Frame.Wakes.Mesh.Panels(GlobalIndexRings).Circulation = VortexRing.G
                        Frame.Wakes.Mesh.Panels(GlobalIndexRings).SourceStrength = VortexRing.S
                        Frame.Wakes.Mesh.Panels(GlobalIndexRings).IsSlender = VortexRing.IsSlender

                    Next

                    For Each Vortex In Wake.Vortices
                        Dim Segment As New Basics.LatticeSegment
                        Segment.N1 = Vortex.Node1.IndexG
                        Segment.N2 = Vortex.Node2.IndexG
                        Frame.Wakes.Mesh.Lattice.Add(Segment)
                    Next

                Next

            Next

            Frame.Wakes.VisualProperties.ShowSurface = True
            Frame.Wakes.VisualProperties.ShowMesh = False
            Frame.Wakes.VisualProperties.ShowNodes = True

        End Sub

    End Class

#Region "Modal response results"

    Public Class ModalResponse

        Public Property P As Double

        Public Property V As Double

    End Class

    Public Class ModalResults

        Public Property M As Double

        Public Property K As Double

        Public Property C As Double

        Public Property W As Double

        Public Property Response As New List(Of ModalResponse)

    End Class

    Public Class AeroelasticLinkResults

        Public Modes As New List(Of ModalResults)

    End Class

    Public Class AeroelasticResults

        Public Links As New List(Of AeroelasticLinkResults)

    End Class

#End Region

#Region "Partial airloads"

    ''' <summary>
    ''' Describes the loads of a single component of the model
    ''' </summary>
    Public Class PartialAirLoads

        ''' <summary>
        ''' The name of the component
        ''' </summary>
        Public Name As String = "Lattice"

        ''' <summary>
        ''' Contains the airloads for each component
        ''' </summary>
        ''' <returns></returns>
        Public Property AirLoads As New AirLoads

        ''' <summary>
        ''' All lift vectors
        ''' </summary>
        Public Property LiftVectors As New List(Of FixedVector)

        ''' <summary>
        ''' The maximum lift
        ''' </summary>
        ''' <returns></returns>
        Public Property MaximumLift As Double = 0

        ''' <summary>
        ''' All induced drag vectors
        ''' </summary>
        Public Property InducedDragVectors As New List(Of FixedVector)

        ''' <summary>
        ''' The maximum induced drag
        ''' </summary>
        ''' <returns></returns>
        Public Property MaximumInducedDrag As Double = 0

        ''' <summary>
        ''' All skin drag vectors
        ''' </summary>
        Public Property SkinDragVectors As New List(Of FixedVector)

        ''' <summary>
        ''' The maximum skin drag
        ''' </summary>
        ''' <returns></returns>
        Public Property MaximumSkinDrag As Double = 0

    End Class

#End Region

#Region "Motion results"

    Public Class FreeMotionResults

        Public Sub New(FilePath As String)

        End Sub

    End Class

#End Region

End Namespace
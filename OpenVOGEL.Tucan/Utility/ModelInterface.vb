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

Imports SharpGL
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports System.Xml
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.DataStore

Namespace Tucan.Utility

    Public Enum InterfaceModes As Integer
        Design = 1
        Postprocess = 2
    End Enum

    ''' <summary>
    ''' Provides all the methos necessary to work with the 3D model
    ''' </summary>
    Module ModelInterface

        Private _Initialized As Boolean = False

        ReadOnly Property Initialized As Boolean
            Get
                Return _Initialized
            End Get
        End Property

        Public Sub Initialize()

            ControlGL = New OpenGL

            ProjectRoot.Initialize()

            Visualization.Initialize()

            Visualization.Proximity = 100

            AddHandler Model.OperationsTool.OnTaskReady, AddressOf RefreshOnGL

            RefreshOnGL()

            AddHandler ProjectRoot.Model.OperationsTool.OnTaskReady, AddressOf RefreshOnGL
            'AddHandler ProjectRoot.ProjectRestared, AddressOf RefreshOnGL

            _Initialized = True

        End Sub

        Public Sub RestartProject()

            ProjectRoot.RestartProject()
            Visualization.Initialize()
            RefreshOnGL()

        End Sub

        Private _InterfaceMode As InterfaceModes = InterfaceModes.Design

        Public ReadOnly Property InterfaceMode As InterfaceModes
            Get
                Return _InterfaceMode
            End Get
        End Property

        Public ReadOnly Property CurrentModeName As String
            Get
                Select Case _InterfaceMode
                    Case InterfaceModes.Design
                        Return "Design"
                    Case InterfaceModes.Postprocess
                        Return "Postprocess"
                    Case Else
                        Return "No mode"
                End Select
            End Get
        End Property

        Public Sub DesignMode()

            _InterfaceMode = InterfaceModes.Design
            RefreshOnGL()

        End Sub

        Public Sub PostprocessMode()

            If Results.Loaded Then
                _InterfaceMode = InterfaceModes.Postprocess
                RefreshOnGL()
            End If

        End Sub

        Public Sub ShowModeWarning()

            If _InterfaceMode = InterfaceModes.Postprocess Then
                MsgBox("This function is only available in design mode", MsgBoxStyle.Information, "VOGEL")
            ElseIf _InterfaceMode = InterfaceModes.Design Then
                MsgBox("This function is only available in post process mode", MsgBoxStyle.Information, "VOGEL")
            End If

        End Sub

        ''' <summary>
        ''' Indicates if the simulation is active
        ''' </summary>
        Public Simulating As Boolean = False

        ''' <summary>
        ''' Visuazation properties
        ''' </summary>
        ''' <returns></returns>
        Public Property Visualization As New VisualizationParameters

        ''' <summary>
        ''' Selection tool.
        ''' </summary>
        ''' <returns></returns>
        Public Property Selection As New Selection

        ''' <summary>
        ''' The OpenGL control provided by SharpGL
        ''' </summary>
        Private ControlGL As OpenGL

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property ControlGLWidth As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property ControlGLHeight As Integer

        ''' <summary>
        ''' SharpGL control used to draw this project.
        ''' </summary>
        ''' <remarks></remarks>
        Private Structure GLElement

            Public Name As UInteger
            Public ShowOnPan As Boolean
            Public ShowOnRotate As Boolean

        End Structure

        ''' <summary>
        ''' Stablishes a reference to an SharpGL control.
        ''' </summary>
        ''' <param name="Control"></param>
        ''' <remarks></remarks>
        Public Sub SetControlGL(ByRef Control As OpenGL)
            ControlGL = Control
        End Sub

        ''' <summary>
        ''' Represents the current lists on OpenGL.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RepresentOnGL()

            ' Don't do it if there is a simulation on course:

            If Simulating Then Return

            'FastRefreshOnGL()

            ControlGL.ClearColor(Visualization.ScreenColor.R / 255, Visualization.ScreenColor.G / 255, Visualization.ScreenColor.B / 255, Visualization.ScreenColor.A / 255)

            ControlGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT)
            ControlGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT)

            If Visualization.AllowAlphaBlending Then
                ControlGL.Enable(OpenGL.GL_BLEND)
                ControlGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA)
            Else
                ControlGL.Disable(OpenGL.GL_BLEND)
            End If

            If Visualization.AllowLineSmoothing Then ControlGL.Enable(OpenGL.GL_LINE_SMOOTH) Else ControlGL.Disable(OpenGL.GL_LINE_SMOOTH)

            Dim Origen As New Vector3
            Dim Punto As Vector3 = Visualization.CameraPosition
            Dim Orientacion As EulerAngles = Visualization.CameraOrientation

            ControlGL.RenderMode(OpenGL.GL_RENDER)
            ControlGL.MatrixMode(OpenGL.GL_PROJECTION)

            ControlGL.LoadIdentity()

            ControlGL.Ortho(-0.5 * ControlGLWidth, 0.5 * ControlGLWidth, -0.5 * ControlGLHeight, 0.5 * ControlGLHeight, -100000, 100000)

            ControlGL.Translate(Visualization.CameraPosition.X, Visualization.CameraPosition.Y, 0)
            ControlGL.Rotate(Orientacion.Fi, Orientacion.Tita, Orientacion.Psi)
            ControlGL.Scale(Visualization.Proximity, Visualization.Proximity, Visualization.Proximity)

            For Each List In ListOfSurfacesToDraw
                If Not Visualization.Panning And Not Visualization.Rotating Or
               (Visualization.Panning And List.ShowOnPan) Or
               (Visualization.Rotating And List.ShowOnRotate) Then
                    ControlGL.CallList(List.Name)
                End If
            Next

            RepresentTaskOnGL()

            ControlGL.Flush()

        End Sub

        ''' <summary>
        ''' Remakes OpenGL lists and redraws.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RefreshOnGL()

            If IsNothing(ControlGL) Or Simulating Then Return
            MakeDrawingLists()
            RepresentOnGL()

        End Sub

        ''' <summary>
        ''' Redraws by using drawing subs 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FastRefreshOnGL(Width As Integer, Height As Integer)

            ControlGL.ClearColor(Visualization.ScreenColor.R / 255, Visualization.ScreenColor.G / 255, Visualization.ScreenColor.B / 255, Visualization.ScreenColor.A / 255)

            ControlGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT)
            ControlGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT)

            If Visualization.AllowAlphaBlending Then
                ControlGL.Enable(OpenGL.GL_BLEND)
                ControlGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA)
            Else
                ControlGL.Disable(OpenGL.GL_BLEND)
            End If

            If Visualization.AllowLineSmoothing Then ControlGL.Enable(OpenGL.GL_LINE_SMOOTH) Else ControlGL.Disable(OpenGL.GL_LINE_SMOOTH)

            Dim Origen As New Vector3
            Dim Punto As Vector3 = Visualization.CameraPosition
            Dim Orientacion As EulerAngles = Visualization.CameraOrientation

            ControlGL.RenderMode(OpenGL.GL_RENDER)
            ControlGL.MatrixMode(OpenGL.GL_PROJECTION)

            ControlGL.LoadIdentity()

            ControlGL.Ortho(-0.5 * Width, 0.5 * Width, -0.5 * Height, 0.5 * Height, -100000, 100000)

            ControlGL.Translate(Visualization.CameraPosition.X, Visualization.CameraPosition.Y, 0)
            ControlGL.Rotate(Orientacion.Fi, Orientacion.Tita, Orientacion.Psi)
            ControlGL.Scale(Visualization.Proximity, Visualization.Proximity, Visualization.Proximity)

            Visualization.Axes.Extension = 1.0
            Visualization.Axes.GenerateWireFrame(ControlGL)
            Visualization.ReferenceFrame.GenerateWireFrame(ControlGL)

            ' Model:
            Select Case InterfaceMode

                Case InterfaceModes.Design

                    For i = 0 To Model.Objects.Count - 1

                        If Model.Objects(i).VisualProperties.ShowSurface Then

                            Model.Objects(i).Refresh3DModel(ControlGL)

                        End If

                    Next

                Case InterfaceModes.Postprocess

                    ' Results:

                    If Results.VisualizeModes Then

                        Results.DynamicModes(Results.SelectedModeIndex).Refresh3DModel(ControlGL)

                    Else

                        If Results.Model.VisualProperties.ShowSurface Then

                            Results.Model.Refresh3DModel(ControlGL)

                        End If

                        If Not IsNothing(Results.Wakes) Then

                            Results.Wakes.Refresh3DModel(ControlGL)

                        End If

                        ProjectRoot.VelocityPlane.Updte3DModel(ControlGL)

                    End If

            End Select

            RepresentTaskOnGL()

            ControlGL.Flush()

        End Sub

        ''' <summary>
        ''' OpenGL lists to be drawn.
        ''' </summary>
        ''' <remarks></remarks>
        Private ListOfSurfacesToDraw As New List(Of GLElement)

        ''' <summary>
        ''' Constructs OpenGL lists.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub MakeDrawingLists()

            For Each List In ListOfSurfacesToDraw
                ControlGL.DeleteLists(List.Name, 1)
            Next

            ListOfSurfacesToDraw.Clear()

            Visualization.Axes.Extension = 1.0

            Dim Coordinates As GLElement
            Coordinates.Name = ControlGL.GenLists(1)
            Coordinates.ShowOnPan = True
            Coordinates.ShowOnRotate = True
            ListOfSurfacesToDraw.Add(Coordinates)
            ControlGL.NewList(Coordinates.Name, OpenGL.GL_COMPILE)
            Visualization.Axes.GenerateWireFrame(ControlGL)
            ControlGL.EndList()

            Dim Reference As GLElement
            Reference.Name = ControlGL.GenLists(1)
            Reference.ShowOnPan = True
            Reference.ShowOnRotate = True
            ListOfSurfacesToDraw.Add(Reference)
            ControlGL.NewList(Reference.Name, OpenGL.GL_COMPILE)
            Visualization.ReferenceFrame.GenerateWireFrame(ControlGL)
            ControlGL.EndList()

            'Simulacion.RepresentarVectorDeVelocidad(GL, New EVector3)

            ' Model:
            Select Case InterfaceMode

                Case InterfaceModes.Design

                    For Each Surface In Model.Objects

                        If Surface.VisualProperties.ShowSurface Then

                            Dim List As GLElement
                            List.Name = ControlGL.GenLists(1)
                            List.ShowOnPan = True
                            List.ShowOnRotate = True
                            ListOfSurfacesToDraw.Add(List)
                            ControlGL.NewList(List.Name, OpenGL.GL_COMPILE)
                            Surface.Refresh3DModel(ControlGL)
                            ControlGL.EndList()

                        End If

                    Next

                Case InterfaceModes.Postprocess

                    If Results.VisualizeModes And Not IsNothing(Results.SelectedMode) Then
                        Dim List As GLElement
                        List.Name = ControlGL.GenLists(1)
                        List.ShowOnPan = True
                        List.ShowOnRotate = True
                        ListOfSurfacesToDraw.Add(List)
                        ControlGL.NewList(List.Name, OpenGL.GL_COMPILE)
                        Results.SelectedMode.Refresh3DModel(ControlGL)
                        ControlGL.EndList()
                    Else

                        ' Results:

                        If Results.Model.VisualProperties.ShowSurface Then
                            Dim List As GLElement
                            List.Name = ControlGL.GenLists(1)
                            List.ShowOnPan = True
                            List.ShowOnRotate = True
                            ListOfSurfacesToDraw.Add(List)
                            ControlGL.NewList(List.Name, OpenGL.GL_COMPILE)
                            Results.Model.Refresh3DModel(ControlGL)
                            ControlGL.EndList()
                        End If

                        If Not IsNothing(Results.Wakes) Then
                            Dim List As GLElement
                            List.Name = ControlGL.GenLists(1)
                            List.ShowOnPan = False
                            List.ShowOnRotate = False
                            ListOfSurfacesToDraw.Add(List)
                            ControlGL.NewList(List.Name, OpenGL.GL_COMPILE)
                            Results.Wakes.Refresh3DModel(ControlGL)
                            ControlGL.EndList()
                        End If

                        Dim Plane As GLElement
                        Plane.Name = ControlGL.GenLists(1)
                        Plane.ShowOnPan = False
                        Plane.ShowOnRotate = False
                        ListOfSurfacesToDraw.Add(Plane)
                        ControlGL.NewList(Plane.Name, OpenGL.GL_COMPILE)
                        ProjectRoot.VelocityPlane.Updte3DModel(ControlGL)
                        ControlGL.EndList()

                    End If

            End Select

        End Sub

        ''' <summary>
        ''' Represents the aeroelastic transit
        ''' </summary>
        ''' <param name="TimeStep"></param>
        Public Sub RepresentResultsTransitWithOpenGL(ByVal TimeStep As Integer)

            ControlGL.ClearColor(Visualization.ScreenColor.R / 255, Visualization.ScreenColor.G / 255, Visualization.ScreenColor.B / 255, Visualization.ScreenColor.A / 255)

            ControlGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT)
            ControlGL.Clear(OpenGL.GL_DEPTH_BUFFER_BIT)

            If Visualization.AllowAlphaBlending Then
                ControlGL.Enable(OpenGL.GL_BLEND)
                ControlGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA)
            Else
                ControlGL.Disable(OpenGL.GL_BLEND)
            End If

            If Visualization.AllowLineSmoothing Then ControlGL.Enable(OpenGL.GL_LINE_SMOOTH) Else ControlGL.Disable(OpenGL.GL_LINE_SMOOTH)

            Dim Origin As New Vector3
            Dim Punto As Vector3 = Visualization.CameraPosition
            Dim Orientacion As EulerAngles = Visualization.CameraOrientation

            ControlGL.RenderMode(OpenGL.GL_RENDER)
            ControlGL.MatrixMode(OpenGL.GL_PROJECTION)

            ControlGL.LoadIdentity()

            ControlGL.Ortho(-0.5 * ControlGLWidth, 0.5 * ControlGLWidth, -0.5 * ControlGLHeight, 0.5 * ControlGLHeight, -100000, 100000)

            ControlGL.Translate(Visualization.CameraPosition.X, Visualization.CameraPosition.Y, 0)
            ControlGL.Rotate(Orientacion.Fi, Orientacion.Tita, Orientacion.Psi)
            ControlGL.Scale(Visualization.Proximity, Visualization.Proximity, Visualization.Proximity)

            Visualization.Axes.Extension = 1.0
            Visualization.Axes.GenerateWireFrame(ControlGL)
            Visualization.ReferenceFrame.GenerateWireFrame(ControlGL)

            ProjectRoot.VelocityPlane.RepresentVelocityVector(ControlGL, ProjectRoot.SimulationSettings.StreamVelocity, Origin)

            Results.RepresentTransitState(ControlGL, TimeStep)

            ControlGL.Flush()

        End Sub

        ''' <summary>
        ''' Makes a selection on the 3D model
        ''' </summary>
        ''' <param name="X"></param>
        ''' <param name="Y"></param>
        ''' <param name="Width"></param>
        ''' <param name="Height"></param>
        Public Sub SelectOnGL(ByVal X As Double, ByVal Y As Double, Width As Integer, Height As Integer)

            ControlGL.ClearColor(Visualization.ScreenColor.R / 255, Visualization.ScreenColor.G / 255, Visualization.ScreenColor.B / 255, Visualization.ScreenColor.A / 255)

            Dim Buffer(OpenGL.GL_SELECTION_BUFFER_SIZE) As UInteger
            ControlGL.SelectBuffer(512, Buffer)

            ControlGL.RenderMode(OpenGL.GL_SELECT)
            ControlGL.InitNames()

            ControlGL.LoadIdentity()

            Dim Viewport(3) As Integer
            ControlGL.GetInteger(OpenGL.GL_VIEWPORT, Viewport)
            ControlGL.PickMatrix(X, Viewport(3) - Y, 5, 5, Viewport)

            ControlGL.Ortho(-0.5 * ControlGLWidth, 0.5 * ControlGLWidth, -0.5 * ControlGLHeight, 0.5 * ControlGLHeight, -100000, 100000)

            Dim Origin As New Vector3
            Dim Point As Vector3 = Visualization.CameraPosition
            Dim Orientation As EulerAngles = Visualization.CameraOrientation

            ControlGL.Translate(Visualization.CameraPosition.X, Visualization.CameraPosition.Y, 0)
            ControlGL.Rotate(Orientation.Fi, Orientation.Tita, Orientation.Psi)
            ControlGL.Scale(Visualization.Proximity, Visualization.Proximity, Visualization.Proximity)

            Select Case InterfaceMode

                Case InterfaceModes.Design

                    If Not Selection.MultipleSelection Then Selection.SelectionList.Clear()

                    For i = 0 To Model.Objects.Count - 1

                        If Model.Objects(i) IsNot Nothing Then

                            Model.Objects(i).Active = False

                            If Not Selection.MultipleSelection Then Model.Objects(i).UnselectAll()

                            If Model.Objects(i).VisualProperties.ShowSurface Then

                                Model.Objects(i).Refresh3DModel(ControlGL, True, i)

                            End If

                        End If

                    Next

                Case InterfaceModes.Postprocess

                    If Results.Model IsNot Nothing Then

                        Results.Model.Active = False

                        If Not Selection.MultipleSelection Then
                            Results.Model.UnselectAll()
                            Selection.SelectionList.Clear()
                        End If

                        Results.Model.Refresh3DModel(ControlGL, True, 0)

                    End If

            End Select

            ControlGL.Flush()

            ' Read the buffer and store selected elements on selection list:

            Dim Hits As Integer = ControlGL.RenderMode(OpenGL.GL_RENDER)

            If Hits > 0 Then

                Dim InitialPos As Integer = 0
                Dim PreviousHits As Integer = 0
                Dim SelectedItem As SelectionRecord
                Dim LocalList As New List(Of SelectionRecord)
                Dim Mesh As Basics.Mesh
                Dim R1 As Vector3
                Dim R2 As Vector3
                Dim R3 As Vector3
                Dim R4 As Vector3
                Dim N1 As Integer
                Dim N2 As Integer
                Dim N3 As Integer
                Dim N4 As Integer
                Dim EyeVector As New Vector3

                EyeVector.Z = 1.0
                EyeVector.Rotate(-Orientation.Psi, -Orientation.Tita, -Orientation.Fi)

                For i = 1 To Hits

                    PreviousHits = Buffer(InitialPos)

                    If PreviousHits > 0 Then

                        SelectedItem.ID = Buffer(InitialPos + 2 + PreviousHits)

                        ' Only take the entities of the active selection type

                        If SelectedItem.EntityType = Selection.EntityToSelect Then

                            ' Calculate the eye depth

                            Select Case InterfaceMode

                                Case InterfaceModes.Design

                                    Mesh = Model.Objects(SelectedItem.ComponentIndex).Mesh

                                    Select Case SelectedItem.EntityType

                                        Case EntityTypes.etNode

                                            R1 = Mesh.Nodes(SelectedItem.EntityIndex).Position
                                            SelectedItem.EyeDepth =
                                                            EyeVector.X * R1.X +
                                                            EyeVector.Y * R1.Y +
                                                            EyeVector.Z * R1.Z

                                            LocalList.Add(SelectedItem)

                                        Case EntityTypes.etSegment

                                            N1 = Mesh.Lattice(SelectedItem.EntityIndex).N1
                                            N2 = Mesh.Lattice(SelectedItem.EntityIndex).N2
                                            R1 = Mesh.Nodes(N1).Position
                                            R2 = Mesh.Nodes(N2).Position
                                            SelectedItem.EyeDepth =
                                                            EyeVector.X * (0.5 * (R1.X + R2.X)) +
                                                            EyeVector.Y * (0.5 * (R1.Y + R2.Y)) +
                                                            EyeVector.Z * (0.5 * (R1.Z + R2.Z))

                                            LocalList.Add(SelectedItem)

                                        Case EntityTypes.etPanel

                                            N1 = Mesh.Panels(SelectedItem.EntityIndex).N1
                                            N2 = Mesh.Panels(SelectedItem.EntityIndex).N2
                                            N3 = Mesh.Panels(SelectedItem.EntityIndex).N3
                                            N4 = Mesh.Panels(SelectedItem.EntityIndex).N4
                                            R1 = Mesh.Nodes(N1).Position
                                            R2 = Mesh.Nodes(N2).Position
                                            R3 = Mesh.Nodes(N3).Position
                                            R4 = Mesh.Nodes(N4).Position
                                            SelectedItem.EyeDepth =
                                                            EyeVector.X * (0.25 * (R1.X + R2.X + R3.X + R4.X)) +
                                                            EyeVector.Y * (0.25 * (R1.Y + R2.Y + R3.Y + R4.Y)) +
                                                            EyeVector.Z * (0.25 * (R1.Z + R2.Z + R3.Z + R4.Z))

                                            LocalList.Add(SelectedItem)

                                        Case EntityTypes.etStructuralElement

                                            LocalList.Add(SelectedItem)

                                        Case EntityTypes.etStructuralNode

                                            LocalList.Add(SelectedItem)

                                    End Select

                                Case InterfaceModes.Postprocess

                                    If SelectedItem.ComponentType = ComponentTypes.etResultContainer Then

                                        Mesh = Results.Model.Mesh

                                        Select Case SelectedItem.EntityType

                                            Case EntityTypes.etPanel

                                                N1 = Mesh.Panels(SelectedItem.EntityIndex).N1
                                                N2 = Mesh.Panels(SelectedItem.EntityIndex).N2
                                                N3 = Mesh.Panels(SelectedItem.EntityIndex).N3
                                                N4 = Mesh.Panels(SelectedItem.EntityIndex).N4
                                                R1 = Mesh.Nodes(N1).Position
                                                R2 = Mesh.Nodes(N2).Position
                                                R3 = Mesh.Nodes(N3).Position
                                                R4 = Mesh.Nodes(N4).Position
                                                SelectedItem.EyeDepth =
                                                                EyeVector.X * (0.25 * (R1.X + R2.X + R3.X + R4.X)) +
                                                                EyeVector.Y * (0.25 * (R1.Y + R2.Y + R3.Y + R4.Y)) +
                                                                EyeVector.Z * (0.25 * (R1.Z + R2.Z + R3.Z + R4.Z))

                                                LocalList.Add(SelectedItem)

                                        End Select

                                    End If

                            End Select

                        End If

                    End If

                    InitialPos = InitialPos + 3 + PreviousHits

                Next

                ' Only add the item closest to the eye

                If LocalList.Count > 0 Then

                    Dim ClosestItem As SelectionRecord
                    Dim First As Boolean = True

                    For Each Record In LocalList
                        If First Or Record.EyeDepth > ClosestItem.EyeDepth Then
                            ClosestItem = Record
                            First = False
                        End If
                    Next

                    If ClosestItem.EntityType <> EntityTypes.etNothing Then

                        Selection.SelectionList.Add(ClosestItem)

                        Select Case InterfaceMode

                            Case InterfaceModes.Design

                                Mesh = Model.Objects(ClosestItem.ComponentIndex).Mesh

                                Select Case ClosestItem.EntityType

                                    Case EntityTypes.etNode
                                        Mesh.Nodes(ClosestItem.EntityIndex).Active = True

                                    Case EntityTypes.etSegment
                                        'TODO: mark as active

                                    Case EntityTypes.etPanel
                                        Model.Objects(ClosestItem.ComponentIndex).Active = True
                                        Mesh.Panels(ClosestItem.EntityIndex).Active = True

                                    Case EntityTypes.etStructuralElement
                                        'TODO: mark as active

                                    Case EntityTypes.etStructuralNode
                                        'TODO: mark as active

                                End Select

                            Case InterfaceModes.Postprocess

                                If ClosestItem.ComponentType = ComponentTypes.etResultContainer Then

                                    Mesh = Results.Model.Mesh

                                    Select Case ClosestItem.EntityType

                                        Case EntityTypes.etNode
                                            Mesh.Nodes(ClosestItem.EntityIndex).Active = True

                                        Case EntityTypes.etPanel
                                            Mesh.Panels(ClosestItem.EntityIndex).Active = True

                                    End Select

                                End If

                        End Select

                    End If

                End If

                RefreshOnGL()

            End If

        End Sub

        ''' <summary>
        ''' Represents a task in the 3D model
        ''' </summary>
        Public Sub RepresentTaskOnGL()

            Model.OperationsTool.RepresentTaskOnGL(ControlGL)

        End Sub

#Region "Input/Output"

        ''' <summary>
        ''' Occurs when the project is saved
        ''' </summary>
        ''' <param name="Title"></param>
        Public Event InputOutputDone(Title As String)

        ''' <summary>
        ''' Reads a full project from XML
        ''' </summary>
        Public Sub ReadFromXML()

            If Not ExistsOnDatabase Then Exit Sub

            Dim Reader As XmlReader = XmlReader.Create(FilePath)

            While Reader.Read

                Select Case Reader.Name

                    Case "Project"
                        ProjectRoot.ReadFromXML(Reader.ReadSubtree)

                    Case "Visualization"
                        Visualization.ReadFromXML(Reader.ReadSubtree)

                End Select

            End While

            Reader.Close()

            RaiseEvent InputOutputDone(FilePath)

            RepresentOnGL()

        End Sub

        ''' <summary>
        ''' Writes the full project to an XML
        ''' </summary>
        Public Sub WriteToXML()

            Dim Writer As XmlWriter = XmlWriter.Create(FilePath)

            Try

                Writer.WriteStartElement("OpenVOGEL")

                Writer.WriteStartElement("Project")
                ProjectRoot.WriteToXML(Writer)
                Writer.WriteEndElement()

                Writer.WriteStartElement("Visualization")
                Visualization.SaveToXML(Writer)
                Writer.WriteEndElement()

                Writer.WriteEndElement()

                Writer.Close()

                RaiseEvent InputOutputDone(FilePath)

            Catch

                Writer.Close()

                Throw New Exception("Error while reading the project")

            End Try

        End Sub

#End Region

    End Module

End Namespace

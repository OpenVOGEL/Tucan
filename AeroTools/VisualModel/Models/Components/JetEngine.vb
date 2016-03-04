'Copyright (C) 2016 Guillermo Hazebrouck

Imports MathTools.Algebra.EuclideanSpace
Imports System.Xml
Imports AeroTools.VisualModel.Models.Basics
Imports SharpGL
Imports AeroTools.VisualModel.Interface
Imports AeroTools.VisualModel.IO

Namespace VisualModel.Models.Components

    Public Class JetEngine

        Inherits GeneralSurface

        Public Sub New()

            Length = 2
            FrontDiameter = 1
            BackDiameter = 0.6
            FrontLength = 0.6
            BackLength = 0.4
            Resolution = 10

            GenerateMesh()

            VisualProps.ShowSurface = True
            VisualProps.ShowMesh = True
            IncludeInCalculation = True

        End Sub

        Public Property Length As Double

        Public Property FrontDiameter As Double

        Public Property BackDiameter As Double

        Public Property FrontLength As Double

        Public Property BackLength As Double

        Public Property Resolution As Integer

        ''' <summary>
        ''' Generates a triangular or quadrilateral mesh.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub GenerateMesh()

            Mesh.NodalPoints.Clear()
            Mesh.Panels.Clear()

            For i = 0 To 3

                Dim x As Double
                Dim r As Double

                Select Case i

                    Case 0
                        x = 0.0#
                        r = 0.5 * FrontDiameter

                    Case 1
                        x = FrontLength
                        r = 0.5 * FrontDiameter

                    Case 2
                        x = Length - BackLength
                        r = 0.5 * BackDiameter

                    Case 3
                        x = Length
                        r = 0.5 * BackDiameter

                End Select

                For j = 0 To Resolution

                    Dim angle As Double = 2 * Math.PI * j / (Resolution + 1)

                    Dim p As New EVector3(x, r * Math.Cos(angle), r * Math.Sin(angle))

                    Mesh.NodalPoints.Add(New NodalPoint(p))

                    If i > 0 Then

                        Dim N1 As Integer
                        Dim N2 As Integer
                        Dim N3 As Integer
                        Dim N4 As Integer

                        If j < Resolution Then

                            N1 = (i - 1) * (Resolution + 1) + j + 1
                            N2 = (i - 1) * (Resolution + 1) + j + 2
                            N3 = i * (Resolution + 1) + j + 2
                            N4 = i * (Resolution + 1) + j + 1

                            Dim q As New Panel(N1, N2, N3, N4)

                            Mesh.Panels.Add(q)

                        Else

                            N1 = (i - 1) * (Resolution + 1) + j + 1
                            N2 = (i - 1) * (Resolution + 1) + 1
                            N3 = i * (Resolution + 1) + 1
                            N4 = i * (Resolution + 1) + j + 1

                            Dim q As New Panel(N1, N2, N3, N4)

                            Mesh.Panels.Add(q)

                        End If

                    End If

                Next

            Next

            Mesh.Rotate(New EVector3, Orientation)

            Mesh.Translate(Position)

            Mesh.GenerateLattice()

        End Sub

#Region " 3D Functions "

        Public Overrides Sub Refresh3DModel(ByRef gl As OpenGL, Optional ByVal SelectionMode As SelectionModes = SelectionModes.smNoSelection, Optional ByVal ElementIndex As Integer = 0)

            'Version para OpenGL

            Dim i As Integer

            Dim Nodo As NodalPoint

            If Me.VisualProps.ShowSurface Then

                ' load homogeneous color:
                Dim SColor As New EVector3
                If Not Selected Then
                    SColor.X = Me.VisualProps.ColorSurface.R / 255
                    SColor.Y = Me.VisualProps.ColorSurface.G / 255
                    SColor.Z = Me.VisualProps.ColorSurface.B / 255
                Else
                    ' default selected color is {255, 194, 14} (orange)
                    SColor.X = 1
                    SColor.Y = 0.76078
                    SColor.Z = 0.0549
                End If
                gl.Color(SColor.X, SColor.Y, SColor.Z, Me.VisualProps.Transparency)

                gl.InitNames()
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etQuadPanel, 0)

                For i = 1 To NumberOfPanels

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_TRIANGLES)

                    Nodo = NodalPoint((Panel(i).N1))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N2))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N3))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N3))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N4))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    Nodo = NodalPoint((Panel(i).N1))
                    If VisualProps.ShowColormap Then gl.Color(Nodo.Color.R, Nodo.Color.G, Nodo.Color.B)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)

                    gl.End()
                    gl.PopName()

                    'Dim p As EVector3 = Me.QuadPanel(i).ControlPoint
                    'Dim v As EVector3 = Me.QuadPanel(i).NormalVector

                    'gl.Begin(OpenGL.GL_LINES)
                    'gl.Color(1, 0.7, 0.05)
                    'gl.Vertex(p.X, p.Y, p.Z)
                    'gl.Vertex(p.X + v.X, p.Y + v.Y, p.Z + v.Z)
                    'gl.End()

                Next

            End If

            If SelectionMode = SelectionModes.smNodePicking Then

                gl.InitNames()
                Dim Code As Integer = Selection.GetSelectionCode(ComponentTypes.etJetEngine, ElementIndex, EntityTypes.etNode, 0)

                gl.PointSize(VisualProps.SizeNodes)
                gl.Color(Me.VisualProps.ColorNodes.R / 255, Me.VisualProps.ColorNodes.G / 255, Me.VisualProps.ColorNodes.B / 255)

                For i = 1 To NumberOfNodes

                    gl.PushName(Code + i)
                    gl.Begin(OpenGL.GL_POINTS)
                    Nodo = Me.NodalPoint(i)
                    gl.Vertex(Nodo.Position.X, Nodo.Position.Y, Nodo.Position.Z)
                    gl.End()
                    gl.PopName()

                Next

            End If

            ' Genera el mallado:

            If Me.VisualProps.ShowMesh Then

                gl.LineWidth(VisualProps.ThicknessMesh)
                gl.Begin(OpenGL.GL_LINES)

                Dim Nodo1 As EVector3
                Dim Nodo2 As EVector3
                Dim Vector As EVector3
                Dim Carga As New EVector3

                gl.Color(Me.VisualProps.ColorMesh.R / 255, Me.VisualProps.ColorMesh.G / 255, Me.VisualProps.ColorMesh.B / 255)

                For i = 1 To NumberOfSegments

                    Nodo1 = Me.NodalPosition(Me.Segment(i).N1)
                    Nodo2 = Me.NodalPosition(Me.Segment(i).N2)

                    gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                    gl.Vertex(Nodo2.X, Nodo2.Y, Nodo2.Z)

                Next

                gl.Color(Me.VisualProps.ColorVelocity.R / 255, Me.VisualProps.ColorVelocity.G / 255, Me.VisualProps.ColorVelocity.B / 255)

                If VisualProps.ShowVelocityVectors Then

                    For i = 1 To NumberOfPanels

                        Nodo1 = Me.Panel(i).ControlPoint
                        Vector = Me.Panel(i).LocalVelocity

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + VisualProps.ScaleVelocity * Vector.X, Nodo1.Y + VisualProps.ScaleVelocity * Vector.Y, Nodo1.Z + VisualProps.ScaleVelocity * Vector.Z)

                    Next

                End If

                gl.Color(Me.VisualProps.ColorLoads.R / 255, Me.VisualProps.ColorLoads.G / 255, Me.VisualProps.ColorLoads.B / 255)

                If VisualProps.ShowLoadVectors Then

                    For i = 1 To NumberOfPanels

                        Nodo1 = Panel(i).ControlPoint
                        Carga.Assign(Panel(i).NormalVector)
                        Carga.Scale(VisualProps.ScalePressure * Panel(i).Cp * Panel(i).Area)

                        gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                        gl.Vertex(Nodo1.X + Carga.X, Nodo1.Y + Carga.Y, Nodo1.Z + Carga.Z)

                    Next

                End If

                gl.End()

            End If

            'If _SelectedControlPoint >= 1 And _SelectedControlPoint <= Me.NumberOfPanels Then

            '    gl.PointSize(2 * Me.VisualizationProperties.SizeNodes)
            '    gl.Color(VisualizationProperties.ColorNodes.R / 255, VisualizationProperties.ColorNodes.G / 255, VisualizationProperties.ColorNodes.B / 255)
            '    gl.Begin(OpenGL.GL_POINTS)

            '    gl.Vertex(Me.QuadPanel(_SelectedControlPoint).ControlPoint.X, Me.QuadPanel(_SelectedControlPoint).ControlPoint.Y, Me.QuadPanel(_SelectedControlPoint).ControlPoint.Z)

            '    gl.End()

            'End If

        End Sub

#End Region

#Region " IO "

        ''' <summary>
        ''' Reads the wing from an XML file.
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <remarks></remarks>
        Public Overrides Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                Select Case reader.Name

                    Case "Identity"

                        Name = reader.GetAttribute("Name")
                        ID = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))
                        Resolution = IOXML.ReadInteger(reader, "RE", 10)
                        FrontDiameter = IOXML.ReadDouble(reader, "FD", 1)
                        BackDiameter = IOXML.ReadDouble(reader, "BD", 0.5)
                        FrontLength = IOXML.ReadDouble(reader, "FL", 1)
                        BackLength = IOXML.ReadDouble(reader, "BL", 0.5)
                        Length = IOXML.ReadDouble(reader, "TL", 0.5)

                        Position.X = IOXML.ReadDouble(reader, "X", 0.0)
                        Position.Y = IOXML.ReadDouble(reader, "Y", 0.0)
                        Position.Z = IOXML.ReadDouble(reader, "Z", 0.0)

                        Orientation.Psi = IOXML.ReadDouble(reader, "Psi", 0)
                        Orientation.Tita = IOXML.ReadDouble(reader, "Theta", 0)
                        Orientation.Fi = IOXML.ReadDouble(reader, "Phi", 0)

                    Case "VisualProperties"

                        VisualProps.ReadFromXML(reader.ReadSubtree)

                End Select

            End While

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Writes the wing to an XML file.
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Public Overrides Sub WriteToXML(ByRef writer As XmlWriter)

            ' Identity:

            writer.WriteStartElement("Identity")
            writer.WriteAttributeString("Name", Name)
            writer.WriteAttributeString("ID", ID.ToString)
            writer.WriteAttributeString("FD", CDbl(FrontDiameter))
            writer.WriteAttributeString("BD", CDbl(BackDiameter))
            writer.WriteAttributeString("FL", CDbl(FrontLength))
            writer.WriteAttributeString("BL", CDbl(BackLength))
            writer.WriteAttributeString("TL", CDbl(Length))

            writer.WriteAttributeString("X", CDbl(Position.X))
            writer.WriteAttributeString("Y", CDbl(Position.Y))
            writer.WriteAttributeString("Z", CDbl(Position.Z))

            writer.WriteAttributeString("Psi", CDbl(Orientation.Psi))
            writer.WriteAttributeString("Theta", CDbl(Orientation.Tita))
            writer.WriteAttributeString("Phi", CDbl(Orientation.Fi))

            writer.WriteAttributeString("RE", CInt(Resolution))
            writer.WriteEndElement()

            ' Visual properties:

            writer.WriteStartElement("VisualProperties")
            VisualProps.WriteToXML(writer)
            writer.WriteEndElement()

        End Sub

        Public Sub CopyFrom(Engine As JetEngine)

            Length = Engine.Length
            FrontDiameter = Engine.FrontDiameter
            BackDiameter = Engine.BackDiameter
            FrontLength = Engine.FrontLength
            BackLength = Engine.BackLength
            Resolution = Engine.Resolution

            Position.X = Engine.Position.X - Engine.Length
            Position.Y = Engine.Position.Y
            Position.Z = Engine.Position.Z

            Orientation.Psi = Engine.Orientation.Psi
            Orientation.Tita = Engine.Orientation.Tita
            Orientation.Fi = Engine.Orientation.Fi

            GenerateMesh()

            VisualProps.ShowSurface = True
            VisualProps.ShowMesh = True
            IncludeInCalculation = True

        End Sub

#End Region

    End Class

End Namespace
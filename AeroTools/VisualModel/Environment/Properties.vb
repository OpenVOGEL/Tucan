Imports MathTools.Algebra.EuclideanSpace
Imports System.Drawing
Imports System.Xml
Imports AeroTools.VisualModel.Environment.Frames
Imports AeroTools.VisualModel.IO

Namespace VisualModel.Environment.Properties

    Public Class VisualizationParameters

        Public CameraOrientation As OrientationCoordinates
        Public CameraPosition As EVector3
        Public Proximity As Double = 1
        Public OrthoProjection As Boolean
        Public ReferenceFrame As New ReferenceFrame
        Public Axes As New CoordinateAxes
        Public ScreenColor As Color = Color.White ' Drawing.Color.FromArgb(49, 49, 49)
        Public AllowLineSmoothing As Boolean = True
        Public AllowAlphaBlending As Boolean = True
        Public MouseStartPosition As EVector2
        Public Panning As Boolean = False
        Public Rotating As Boolean = False

        Public Sub IniciarParametros()

            CameraOrientation = New OrientationCoordinates
            CameraOrientation.Psi = 0.0
            CameraOrientation.Tita = 0.0
            CameraOrientation.Fi = 0.0

            CameraPosition = New EVector3
            CameraPosition.X = 0
            CameraPosition.Y = 0
            CameraPosition.Z = 0

            Proximity = 20.0
            OrthoProjection = False

        End Sub

        Public Sub AdquirirPosicionDeCámara(ByVal PosicionDelMouseX As Integer, ByVal PosicionDelMouseY As Integer, ByVal Escala As Double)

            CameraPosition.X = CameraPosition.X - (MouseStartPosition.X - PosicionDelMouseX) * Escala
            CameraPosition.Y = CameraPosition.Y + (MouseStartPosition.Y - PosicionDelMouseY) * Escala

        End Sub

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Camera")
            writer.WriteAttributeString("Psi", String.Format("{0}", CameraOrientation.Psi))
            writer.WriteAttributeString("Tita", String.Format("{0}", CameraOrientation.Tita))
            writer.WriteAttributeString("Fi", String.Format("{0}", CameraOrientation.Fi))
            writer.WriteAttributeString("X", String.Format("{0}", CameraPosition.X))
            writer.WriteAttributeString("Y", String.Format("{0}", CameraPosition.Y))
            writer.WriteAttributeString("Z", String.Format("{0}", CameraPosition.Z))
            writer.WriteAttributeString("Proximity", String.Format("{0}", Proximity))
            writer.WriteEndElement()

            writer.WriteStartElement("Screen")
            writer.WriteAttributeString("ScreenColorR", String.Format("{0}", ScreenColor.R))
            writer.WriteAttributeString("ScreenColorG", String.Format("{0}", ScreenColor.G))
            writer.WriteAttributeString("ScreenColorB", String.Format("{0}", ScreenColor.B))
            writer.WriteEndElement()

            writer.WriteStartElement("Representation")
            writer.WriteAttributeString("Orthographic", String.Format("{0}", CInt(OrthoProjection)))
            writer.WriteAttributeString("LineSmoothing", String.Format("{0}", CInt(AllowLineSmoothing)))
            writer.WriteAttributeString("AlphaBlending", String.Format("{0}", CInt(AllowAlphaBlending)))
            writer.WriteEndElement()

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "Camera"

                            CameraOrientation.Psi = IOXML.ReadDouble(reader, "Psi", 0)
                            CameraOrientation.Tita = IOXML.ReadDouble(reader, "Tita", 0)
                            CameraOrientation.Fi = IOXML.ReadDouble(reader, "Fi", 0)
                            CameraPosition.X = IOXML.ReadDouble(reader, "X", 0)
                            CameraPosition.Y = IOXML.ReadDouble(reader, "Y", 0)
                            CameraPosition.Z = IOXML.ReadDouble(reader, "Z", 0)
                            Proximity = IOXML.ReadDouble(reader, "Proximity", 20)

                        Case "Screen"

                            Dim R As Integer = IOXML.ReadInteger(reader, "ScreenColorR", 0)
                            Dim G As Integer = IOXML.ReadInteger(reader, "ScreenColorG", 0)
                            Dim B As Integer = IOXML.ReadInteger(reader, "ScreenColorB", 0)

                            ScreenColor = Color.FromArgb(R, G, B)

                        Case "Representation"

                            OrthoProjection = IOXML.ReadBoolean(reader, "Orthographic", False)
                            AllowLineSmoothing = IOXML.ReadBoolean(reader, "LineSmoothing", True)
                            AllowAlphaBlending = IOXML.ReadBoolean(reader, "AlphaBlending", True)

                    End Select

                End If

            End While

        End Sub

    End Class

End Namespace

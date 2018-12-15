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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.DesignTools.VisualModel.Tools.Frames
Imports OpenVOGEL.AeroTools.IoHelper
Imports System.Drawing
Imports System.Xml

Namespace VisualModel.Tools.Properties

    Public Class VisualizationParameters

        Public Property CameraOrientation As EulerAngles
        Public Property CameraPosition As EVector3
        Public Property Proximity As Double = 1
        Public Property OrthoProjection As Boolean
        Public Property ReferenceFrame As New ReferenceFrame
        Public Property Axes As New CoordinateAxes
        Public Property ScreenColor As Color = Color.FromArgb(50, 50, 50)
        Public Property AllowLineSmoothing As Boolean = True
        Public Property AllowAlphaBlending As Boolean = True
        Public Property MouseStartPosition As EVector2
        Public Property Panning As Boolean = False
        Public Property Rotating As Boolean = False

        Public Sub Initialize()

            CameraOrientation = New EulerAngles
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

        Public Sub AcquireCameraPosition(ByVal PosicionDelMouseX As Integer, ByVal PosicionDelMouseY As Integer, ByVal Escala As Double)

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

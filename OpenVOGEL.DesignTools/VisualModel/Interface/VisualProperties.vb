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

Imports System.Xml
Imports System.Drawing
Imports System.ComponentModel
Imports OpenVOGEL.AeroTools.IoHelper

Namespace VisualModel.Interface

    ''' <summary>
    ''' The different kind of visualization modes.
    ''' This is used to know what kind of interactions are allowed. 
    ''' </summary>
    Public Enum VisualizationMode As Integer
        Physic
        Lattice
        Airfoils
        Structural
    End Enum

    ''' <summary>
    ''' Provides information about the visual aspect of objects.
    ''' </summary>
    Public Class VisualProperties

        Implements INotifyPropertyChanged

        Private _ColorSurface As Color

        ''' <summary>
        ''' Color of the surface.
        ''' </summary>
        Public Property ColorSurface As Color
            Set(value As Color)
                If (value <> _ColorSurface) Then
                    _ColorSurface = value
                    RaisePropertyChanged("ColorSurface")
                End If
            End Set
            Get
                Return _ColorSurface
            End Get
        End Property

        Private _ColorMesh As Color

        ''' <summary>
        ''' Color of the mesh.
        ''' </summary>
        Public Property ColorMesh As Color
            Set(value As Color)
                If (value <> _ColorMesh) Then
                    _ColorMesh = value
                    RaisePropertyChanged("ColorMesh")
                End If
            End Set
            Get
                Return _ColorMesh
            End Get
        End Property

        Private _ThicknessMesh As Double

        ''' <summary>
        ''' Thickness of the mesh lattice.
        ''' </summary>
        Public Property ThicknessMesh As Double
            Set(value As Double)
                If (value <> _ThicknessMesh) Then
                    _ThicknessMesh = value
                End If
            End Set
            Get
                Return _ThicknessMesh
            End Get
        End Property

        Private _Transparency As Double

        ''' <summary>
        ''' Transparency of the model.
        ''' </summary>
        Public Property Transparency As Double
            Set(value As Double)
                If value <> _Transparency Then
                    _Transparency = value
                    RaisePropertyChanged("Transparency")
                End If
            End Set
            Get
                Return _Transparency
            End Get
        End Property

        ''' <summary>
        ''' The color of the shedding edge primitives.
        ''' </summary>
        Public Property ColorPrimitives As Color

        ''' <summary>
        ''' The color of the surface when selected.
        ''' </summary>
        Public Property ColorSelection As Color

        Private _ShowMesh As Boolean

        ''' <summary>
        ''' Indicates if the mesh should be displayed.
        ''' </summary>
        Public Property ShowMesh As Boolean
            Set(value As Boolean)
                If value <> _ShowMesh Then
                    _ShowMesh = value
                    RaisePropertyChanged("ShowMesh")
                End If
            End Set
            Get
                Return _ShowMesh
            End Get
        End Property

        Private _ShowSurface As Boolean

        ''' <summary>
        ''' Indicates if the surface should be displayed.
        ''' </summary>
        Public Property ShowSurface As Boolean
            Set(value As Boolean)
                If value <> _ShowSurface Then
                    _ShowSurface = value
                    RaisePropertyChanged("ShowSurface")
                End If
            End Set
            Get
                Return _ShowSurface
            End Get
        End Property

        Private _ShowPrimitives As Boolean

        ''' <summary>
        ''' Indicates if the primitive shedding edges should be displayed.
        ''' </summary>
        Public Property ShowPrimitives As Boolean
            Set(value As Boolean)
                If value <> _ShowPrimitives Then
                    _ShowPrimitives = value
                    RaisePropertyChanged("ShowPrimitives")
                End If
            End Set
            Get
                Return _ShowPrimitives
            End Get
        End Property

        ''' <summary>
        ''' The color of the mesh nodes.
        ''' </summary>
        Public Property ColorNodes As Color

        ''' <summary>
        ''' The color of the velocity vectors.
        ''' </summary>
        Public Property ColorVelocity As Color

        ''' <summary>
        ''' The color of a positive load.
        ''' For thick bodies, a positive load is when it is higher than the static pressure.
        ''' </summary>
        Public Property ColorPositiveLoad As Color

        ''' <summary>
        ''' The color of a negative load.
        ''' For thick bodies, a negative load is when it is lower than the static pressure.
        ''' </summary>
        Public Property ColorNegativeLoad As Color

        ''' <summary>
        ''' The size of the mesh nodes.
        ''' </summary>
        Public Property SizeNodes As Double

        ''' <summary>
        ''' The scale of the velocity vectors.
        ''' </summary>
        Public Property ScaleVelocityVectors As Double

        ''' <summary>
        ''' The scale of the pressure load vectors.
        ''' </summary>
        Public Property ScaleLoadVectors As Double

        ''' <summary>
        ''' Indicqtes if the nodes should be displayed.
        ''' </summary>
        Public Property ShowNodes As Boolean

        ''' <summary>
        ''' Indicates if the velocity vectors should be displayed.
        ''' </summary>
        Public Property ShowVelocityVectors As Boolean

        ''' <summary>
        ''' Indicates if the pressure load vectors should be displayed.
        ''' </summary>
        ''' <returns></returns>
        Public Property ShowLoadVectors As Boolean

        ''' <summary>
        ''' Indicates if the color of the surface should be overriden by a pressure colormap.
        ''' </summary>
        Public Property ShowColormap As Boolean

        ''' <summary>
        ''' Indicates if the local coordinates system should be displayed.
        ''' </summary>
        Public Property ShowLocalCoordinates As Boolean = True

        ''' <summary>
        ''' Indicates if the normal vectors should be displayed.
        ''' </summary>
        Public Property ShowNormalVectors As Boolean

        Private _VisualizationMode As VisualizationMode = [Interface].VisualizationMode.Lattice

        ''' <summary>
        ''' Indicates how the model should be represented.
        ''' </summary>
        ''' <returns></returns>
        Public Property VisualizationMode As VisualizationMode
            Set(ByVal value As VisualizationMode)
                _VisualizationMode = value
                Select Case VisualizationMode
                    Case VisualizationMode.Lattice
                        ShowMesh = True
                        ShowPrimitives = True
                End Select
            End Set
            Get
                Return _VisualizationMode
            End Get
        End Property

        ''' <summary>
        ''' Creates a visualization property object with default values for the given component type.
        ''' </summary>
        ''' <param name="ElementType"></param>
        Public Sub New(ByVal ElementType As ComponentTypes)

            Select Case ElementType

                Case ComponentTypes.etLiftingSurface

                    ColorSurface = Color.Gray
                    ColorMesh = Color.Black
                    ColorPrimitives = Color.SkyBlue
                    ColorSelection = Color.Beige
                    ColorNodes = Color.Black
                    ColorVelocity = Color.BlueViolet
                    ShowMesh = True
                    ShowSurface = True
                    ShowPrimitives = True
                    ShowNodes = False
                    Transparency = 1.0#
                    ThicknessMesh = 1.0#
                    SizeNodes = 8.0#
                    ScaleVelocityVectors = 0.0#
                    ScaleLoadVectors = 0.0#

                Case ComponentTypes.etFuselage

                    ColorSurface = Color.LightGray
                    ColorMesh = Color.Black
                    ColorNodes = Color.Black
                    ColorVelocity = Color.BlueViolet
                    Transparency = 1.0#
                    ThicknessMesh = 1.0#
                    SizeNodes = 8.0#
                    ScaleVelocityVectors = 0.0#
                    ScaleLoadVectors = 0.0#
                    ShowMesh = True
                    ShowSurface = True
                    ShowNodes = False

                Case ComponentTypes.etJetEngine

                    ColorSurface = Color.SteelBlue
                    ColorMesh = Color.Black
                    ColorNodes = Color.Black
                    ColorVelocity = Color.BlueViolet
                    Transparency = 1.0#
                    ThicknessMesh = 1.0#
                    SizeNodes = 8.0#
                    ScaleVelocityVectors = 0.0#
                    ScaleLoadVectors = 0.0#
                    ShowMesh = True
                    ShowSurface = True
                    ShowNodes = False

                Case ComponentTypes.etResultContainer

                    ColorSurface = Color.LightGray
                    ColorMesh = Color.Black
                    ColorNodes = Color.Black
                    ColorVelocity = Color.BlueViolet
                    ColorPositiveLoad = Color.Red
                    ColorNegativeLoad = Color.Blue
                    Transparency = 1.0#
                    ThicknessMesh = 1.0#
                    SizeNodes = 8.0#
                    ScaleVelocityVectors = 0.01#
                    ScaleLoadVectors = 1.0#
                    ShowMesh = True
                    ShowSurface = True
                    ShowNodes = False

                Case ComponentTypes.etWake

                    ColorSurface = Color.LightGray
                    ColorMesh = Color.Black
                    ColorNodes = Color.Black
                    ColorVelocity = Color.BlueViolet
                    Transparency = 1.0#
                    ThicknessMesh = 1.0#
                    SizeNodes = 2.0#
                    ScaleVelocityVectors = 0.0#
                    ScaleLoadVectors = 0.0#
                    ShowMesh = True
                    ShowSurface = False
                    ShowNodes = False

            End Select
        End Sub

        ''' <summary>
        ''' Reads the data from an XML node.
        ''' </summary>
        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "Visibility"
                            ShowSurface = IOXML.ReadBoolean(reader, "ShowSurface", True)
                            ShowMesh = IOXML.ReadBoolean(reader, "ShowLattice", True)
                            ShowPrimitives = IOXML.ReadBoolean(reader, "ShowPrimitives", True)
                            ShowNodes = IOXML.ReadBoolean(reader, "ShowNodes", False)
                            ShowLoadVectors = IOXML.ReadBoolean(reader, "ShowLoads", False)
                            ShowLocalCoordinates = IOXML.ReadBoolean(reader, "ShowCoordinates", False)
                            ShowVelocityVectors = IOXML.ReadBoolean(reader, "ShowVelocity", False)
                            ShowColormap = IOXML.ReadBoolean(reader, "ShowColormap", False)
                            Transparency = IOXML.ReadDouble(reader, "Transparency", 1.0)

                        Case "Size"
                            SizeNodes = IOXML.ReadDouble(reader, "NodeSize", 1.0)
                            SizeNodes = IOXML.ReadDouble(reader, "LatticeThickness", 2.0)
                            ScaleVelocityVectors = IOXML.ReadDouble(reader, "ScaleVelocity", 1.0)
                            ScaleLoadVectors = IOXML.ReadDouble(reader, "ScalePressure", 1)

                        Case "Colors"

                            Dim R As Integer
                            Dim G As Integer
                            Dim B As Integer

                            R = IOXML.ReadInteger(reader, "CSR", 255)
                            G = IOXML.ReadInteger(reader, "CSG", 255)
                            B = IOXML.ReadInteger(reader, "CSB", 255)

                            ColorSurface = Color.FromArgb(R, G, B)

                            R = IOXML.ReadInteger(reader, "CMR", 255)
                            G = IOXML.ReadInteger(reader, "CMG", 255)
                            B = IOXML.ReadInteger(reader, "CMB", 255)

                            ColorMesh = Color.FromArgb(R, G, B)

                    End Select

                End If

            End While

        End Sub

        ''' <summary>
        ''' Writes the data to an XML node.
        ''' </summary>
        ''' <param name="writer"></param>
        Public Sub WriteToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Visibility")
            writer.WriteAttributeString("ShowSurface", String.Format("{0}", Me.ShowSurface))
            writer.WriteAttributeString("ShowLattice", String.Format("{0}", Me.ShowMesh))
            writer.WriteAttributeString("ShowPrimitives", String.Format("{0}", Me.ShowPrimitives))
            writer.WriteAttributeString("ShowNodes", String.Format("{0}", Me.ShowNodes))
            writer.WriteAttributeString("ShowCoordinates", String.Format("{0}", Me.ShowLocalCoordinates))
            writer.WriteAttributeString("ShowVelocity", String.Format("{0}", Me.ShowVelocityVectors))
            writer.WriteAttributeString("ShowLoads", String.Format("{0}", Me.ShowLoadVectors))
            writer.WriteAttributeString("ShowColormap", String.Format("{0}", Me.ShowColormap))
            writer.WriteAttributeString("Transparency", String.Format("{0}", Me.Transparency))
            writer.WriteEndElement()

            writer.WriteStartElement("Size")
            writer.WriteAttributeString("NodeSize", String.Format("{0}", Me.SizeNodes))
            writer.WriteAttributeString("LatticeThickness", String.Format("{0}", Me.ThicknessMesh))
            writer.WriteAttributeString("ScaleVelocity", String.Format("{0}", Me.ScaleVelocityVectors))
            writer.WriteAttributeString("ScalePressure", String.Format("{0}", Me.ScaleLoadVectors))
            writer.WriteEndElement()

            writer.WriteStartElement("Colors")
            ' Surface:
            writer.WriteAttributeString("CSR", String.Format("{0}", Me.ColorSurface.R))
            writer.WriteAttributeString("CSG", String.Format("{0}", Me.ColorSurface.G))
            writer.WriteAttributeString("CSB", String.Format("{0}", Me.ColorSurface.B))
            ' Lattice:
            writer.WriteAttributeString("CMR", String.Format("{0}", Me.ColorMesh.R))
            writer.WriteAttributeString("CMG", String.Format("{0}", Me.ColorMesh.G))
            writer.WriteAttributeString("CMB", String.Format("{0}", Me.ColorMesh.B))
            writer.WriteEndElement()

        End Sub

#Region "Property Changed"

        ''' <summary>
        ''' Indicates that a property has changed.
        ''' Note: not all properties raise this event.
        ''' </summary>
        Public Event PropertyChanged(Sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

        Private Sub RaisePropertyChanged(PropertyName As String)

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))

        End Sub

#End Region

    End Class

End Namespace
Imports System.Xml
Imports System.Drawing
Imports System.ComponentModel
Imports AeroTools.VisualModel.IO

Namespace VisualModel.Interface

    Public Enum VisualizationMode As Integer

        Phisic
        Lattice
        Airfoils
        Structural

    End Enum

    Public Class VisualizationProperties

        Implements INotifyPropertyChanged

        Private _ColorSurface As Color

        ''' <summary>
        ''' Color of the surface
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
        ''' Color of the mesh
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
        ''' Thickness of the mesh
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
        ''' Transparency of the model
        ''' </summary>
        ''' <returns></returns>
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

        Public Property ColorPrimitives As Color
        Public Property ColorSelection As Color

        Private _ShowMesh As Boolean

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

        Public Property ColorNodes As System.Drawing.Color
        Public Property ColorVelocity As System.Drawing.Color
        Public Property ColorLoads As System.Drawing.Color
        Public Property SizeNodes As Double
        Public Property ScaleVelocity As Double
        Public Property ScalePressure As Double

        Public Property ShowNodes As Boolean
        Public Property ShowVelocityVectors As Boolean
        Public Property ShowLoadVectors As Boolean
        Public Property ShowColormap As Boolean
        Public Property ShowLocalCoordinates As Boolean = True

        Private _VisualizationMode As VisualizationMode = [Interface].VisualizationMode.Lattice

        Public Property VisualizationMode As VisualizationMode
            Set(ByVal value As VisualizationMode)
                _VisualizationMode = value
                Select Case VisualizationMode
                    Case [Interface].VisualizationMode.Lattice
                        ShowMesh = True
                        ShowPrimitives = True
                End Select
            End Set
            Get
                Return _VisualizationMode
            End Get
        End Property

        Public Sub New(ByVal ElementType As ComponentTypes)
            Select Case ElementType

                Case ComponentTypes.etLiftingSurface

                    ColorSurface = System.Drawing.Color.Gray
                    ColorMesh = System.Drawing.Color.Black
                    ColorPrimitives = System.Drawing.Color.SkyBlue
                    ColorSelection = System.Drawing.Color.Beige
                    ColorNodes = System.Drawing.Color.Black
                    ColorVelocity = System.Drawing.Color.BlueViolet
                    ColorLoads = System.Drawing.Color.Red

                    ShowMesh = True
                    ShowSurface = True
                    ShowPrimitives = True
                    ShowNodes = False
                    ShowVelocityVectors = True
                    ShowLoadVectors = True
                    ShowColormap = True

                    Transparency = 1.0#
                    ThicknessMesh = 1.0#
                    SizeNodes = 4.0#
                    ScaleVelocity = 0.01#
                    ScalePressure = 1.0#

                Case ComponentTypes.etBody

                    ColorSurface = System.Drawing.Color.LightGray
                    ColorMesh = System.Drawing.Color.Black
                    ColorNodes = System.Drawing.Color.Black
                    ColorVelocity = System.Drawing.Color.BlueViolet
                    ColorLoads = System.Drawing.Color.Red
                    Transparency = 1.0#
                    ThicknessMesh = 0.1#
                    SizeNodes = 4.0#
                    ScaleVelocity = 0.01#
                    ScalePressure = 1.0#
                    ShowNodes = False
                    ShowVelocityVectors = True
                    ShowLoadVectors = True
                    ShowColormap = False

            End Select
        End Sub

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
                            ScaleVelocity = IOXML.ReadDouble(reader, "ScaleVelocity", 1.0)
                            ScalePressure = IOXML.ReadDouble(reader, "ScalePressure", 1)

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
            writer.WriteAttributeString("ScaleVelocity", String.Format("{0}", Me.ScaleVelocity))
            writer.WriteAttributeString("ScalePressure", String.Format("{0}", Me.ScalePressure))
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

        Public Event PropertyChanged(Sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

        Private Sub RaisePropertyChanged(PropertyName As String)

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(PropertyName))

        End Sub

#End Region


    End Class

End Namespace
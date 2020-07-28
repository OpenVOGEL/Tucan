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
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.DataStore
Imports System.Xml
Imports OpenVOGEL.AeroTools.CalculationModel.Settings

Namespace VisualModel.Models

    Public Class DesignModel

        ''' <summary>
        ''' Project name.
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String

        ''' <summary>
        ''' List of model objects.
        ''' </summary>
        ''' <returns></returns>
        Public Property Objects As New List(Of Surface)

        ''' <summary>
        ''' Tool that provides back up info while operating (moving or aligning objects).
        ''' </summary>
        ''' <returns></returns>
        Public Property OperationsTool As New OperationsTool

        ''' <summary>
        ''' Polars.
        ''' </summary>
        ''' <returns></returns>
        Public Property PolarDataBase As New PolarDatabase

#Region " Add and clone objects "

        ''' <summary>
        ''' Adds a lifting surface and sets it as current.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddLiftingSurface()

            Dim NewLiftingSurface = New LiftingSurface
            NewLiftingSurface.Name = String.Format("Wing - {0}", Objects.Count)
            Objects.Add(NewLiftingSurface)

        End Sub

        ''' <summary>
        ''' Adds an extruded body
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddExtrudedBody()

            Dim NewFuselage As New Fuselage
            NewFuselage.Name = String.Format("Fuselage - {0}", Objects.Count)
            NewFuselage.IncludeInCalculation = True
            NewFuselage.Position.SetToCero()
            NewFuselage.CenterOfRotation.SetToCero()
            NewFuselage.Orientation.SetToCero()
            NewFuselage.VisualProperties.ShowColormap = False
            NewFuselage.VisualProperties.ShowLoadVectors = False
            NewFuselage.VisualProperties.ShowNodes = False
            NewFuselage.VisualProperties.ShowVelocityVectors = False
            NewFuselage.VisualProperties.ShowPrimitives = False
            NewFuselage.VisualProperties.ShowSurface = True
            NewFuselage.VisualProperties.ShowMesh = True

            Objects.Add(NewFuselage)

        End Sub

        ''' <summary>
        ''' Adds a jet engine
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddJetEngine()

            Dim Engine As New JetEngine
            Engine.Name = String.Format("Jet engine - {0}", Objects.Count)
            Engine.GenerateMesh()
            Engine.IncludeInCalculation = True
            Engine.VisualProperties.ShowColormap = False
            Engine.VisualProperties.ShowLoadVectors = False
            Engine.VisualProperties.ShowNodes = False
            Engine.VisualProperties.ShowVelocityVectors = False
            Engine.VisualProperties.ShowPrimitives = False
            Engine.VisualProperties.ShowSurface = True
            Engine.VisualProperties.ShowMesh = True

            Objects.Add(Engine)

        End Sub

        ''' <summary>
        ''' Adds an imported surface and sets it as current.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddImportedSurface()

            Dim NewImportedSurface = New ImportedSurface
            NewImportedSurface.Name = "Imported mesh"
            Objects.Add(NewImportedSurface)

        End Sub

        ''' <summary>
        ''' Generates a copy of the current lifting surface with the option symmetric in true
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloneObject(ByVal Index As Integer)

            If Index > 0 And Index <= Objects.Count Then
                Dim newSurface As Surface = Objects(Index).Clone
                If newSurface IsNot Nothing Then
                    Objects.Add(newSurface)
                End If
            End If

        End Sub

#End Region

#Region " IO "

        Public Sub ReadFromXML(ByRef Reader As XmlReader)

            Reader.Read()

            Name = Reader("Name")

            While Reader.Read()

                Select Case Reader.Name

                    Case "PolarDataBase"

                        PolarDataBase.ReadFromXML(Reader.ReadSubtree())

                    Case "CamberLinesDatabase"

                        CamberLinesDatabase.ReadFromXML(Reader.ReadSubtree())

                    Case "ModelProperties"

                        Dim SubReader As XmlReader = Reader.ReadSubtree()

                        SubReader.Read()

                        While SubReader.Read

                            If SubReader.NodeType = XmlNodeType.Element Then

                                If SubReader.Name = "LiftingSurface" Then

                                    Dim LiftingSurface As New LiftingSurface()

                                    LiftingSurface.ReadFromXML(SubReader.ReadSubtree)

                                    For i = 0 To LiftingSurface.WingRegions.Count - 1

                                        Dim Region As WingRegion = LiftingSurface.WingRegions(i)

                                        If Not Region.PolarID.Equals(Guid.Empty) Then

                                            Region.PolarFamiliy = PolarDataBase.GetFamilyFromID(Region.PolarID)

                                        End If

                                    Next

                                    Objects.Add(LiftingSurface)

                                ElseIf SubReader.Name = "ExtrudedSurface" Then

                                    Dim ExtrudedSurface As New Fuselage

                                    ExtrudedSurface.ReadFromXML(Reader.ReadSubtree)

                                    Objects.Add(ExtrudedSurface)

                                ElseIf SubReader.Name = "JetEngine" Then

                                    Dim JetEngine As New JetEngine

                                    JetEngine.ReadFromXML(Reader.ReadSubtree)

                                    Objects.Add(JetEngine)

                                ElseIf SubReader.Name = "ImportedSurface" Then

                                    Dim Import As New ImportedSurface

                                    Import.ReadFromXML(Reader.ReadSubtree)

                                    Objects.Add(Import)

                                End If

                            End If


                        End While

                        SubReader.Close()

                End Select

            End While

            Reader.Close()

        End Sub

        Public Sub WriteToXML(ByRef Writer As XmlWriter)

            Writer.WriteAttributeString("Name", Name)

            Writer.WriteStartElement("PolarDataBase")
            PolarDataBase.WriteToXML(Writer)
            Writer.WriteEndElement()

            Writer.WriteStartElement("CamberLinesDatabase")
            CamberLinesDatabase.WriteToXML(Writer)
            Writer.WriteEndElement()

            Writer.WriteStartElement("ModelProperties")

            For i = 0 To Objects.Count - 1

                If TypeOf Objects(i) Is LiftingSurface Then

                    Writer.WriteStartElement("LiftingSurface")

                    Objects(i).WriteToXML(Writer)

                    Writer.WriteEndElement()

                End If

                If TypeOf Objects(i) Is Fuselage Then

                    Writer.WriteStartElement("ExtrudedSurface")

                    Objects(i).WriteToXML(Writer)

                    Writer.WriteEndElement()

                End If

                If TypeOf Objects(i) Is JetEngine Then

                    Writer.WriteStartElement("JetEngine")

                    Objects(i).WriteToXML(Writer)

                    Writer.WriteEndElement()

                End If

                If TypeOf Objects(i) Is ImportedSurface Then

                    Writer.WriteStartElement("ImportedSurface")

                    Objects(i).WriteToXML(Writer)

                    Writer.WriteEndElement()

                End If

            Next

            Writer.WriteEndElement()

        End Sub

        ''' <summary>
        ''' Returns the global inertia of the model
        ''' </summary>
        ''' <returns></returns>
        Public Function GetGlobalInertia() As InertialProperties

            Dim Inertia As InertialProperties

            Inertia.SetToZero()

            For Each Surface In Objects

                Dim LocalInertia As InertialProperties = Surface.Inertia

                ' Move CG to global coordinates
                '-------------------------------------

                LocalInertia.Xcg += Surface.Position.X
                LocalInertia.Ycg += Surface.Position.Y
                LocalInertia.Zcg += Surface.Position.Z

                ' Take into account complementing parts
                '-------------------------------------

                If TypeOf Surface Is LiftingSurface Then

                    Dim Wing As LiftingSurface = Surface

                    If Wing.Symmetric Then

                        Dim ComplementInertia As InertialProperties = LocalInertia

                        ComplementInertia.Ixy *= -1.0
                        ComplementInertia.Iyz *= -1.0
                        ComplementInertia.Ycg *= -1.0

                        LocalInertia += ComplementInertia

                    End If

                End If

                ' Add to the total
                '-------------------------------------

                Inertia += LocalInertia

            Next

            Return Inertia

        End Function

#End Region

    End Class

End Namespace
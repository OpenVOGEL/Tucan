'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

Imports AeroTools.VisualModel.Interface
Imports AeroTools.VisualModel.Models.Components
Imports System.Xml
Imports AeroTools.VisualModel.Models.Basics
Imports AeroTools.UVLM.Models.Aero.Components

Namespace VisualModel.Models

    Public Class CalculationModel

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
        ''' Selection tool.
        ''' </summary>
        ''' <returns></returns>
        Public Property Selection As New Selection

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
        ''' Adds a nacelle and sets it as current.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddNacelle()

            Dim NewNacelle As New FullNacelle ' GeneralSurface
            NewNacelle.Name = "Nacelle"
            NewNacelle.GenerateLattice()
            NewNacelle.IncludeInCalculation = True
            NewNacelle.VisualProperties.ShowColormap = False
            NewNacelle.VisualProperties.ShowLoadVectors = False
            NewNacelle.VisualProperties.ShowNodes = False
            NewNacelle.VisualProperties.ShowVelocityVectors = False
            NewNacelle.VisualProperties.ShowPrimitives = False
            NewNacelle.VisualProperties.ShowSurface = True
            NewNacelle.VisualProperties.ShowMesh = True

            Objects.Add(NewNacelle)

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

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            reader.Read()

            Name = reader("Name")

            While reader.Read()

                Select Case reader.Name

                    Case "PolarDataBase"

                        PolarDataBase.ReadFromXML(reader.ReadSubtree())

                    Case "ModelProperties"

                        Dim SubReader As XmlReader = reader.ReadSubtree()

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

                                    ExtrudedSurface.ReadFromXML(reader.ReadSubtree)

                                    Objects.Add(ExtrudedSurface)

                                ElseIf SubReader.Name = "JetEngine" Then

                                    Dim JetEngine As New JetEngine

                                    JetEngine.ReadFromXML(reader.ReadSubtree)

                                    Objects.Add(JetEngine)

                                End If

                            End If


                        End While

                        SubReader.Close()

                End Select

            End While

            reader.Close()

        End Sub

        Public Sub WriteToXML(ByRef writer As XmlWriter)

            writer.WriteAttributeString("Name", Name)

            writer.WriteStartElement("PolarDataBase")
            PolarDataBase.WriteToXML(writer)
            writer.WriteEndElement()

            writer.WriteStartElement("ModelProperties")

            For i = 0 To Objects.Count - 1

                If TypeOf Objects(i) Is LiftingSurface Then

                    writer.WriteStartElement("LiftingSurface")

                    Objects(i).WriteToXML(writer)

                    writer.WriteEndElement()

                End If

                If TypeOf Objects(i) Is Fuselage Then

                    writer.WriteStartElement("ExtrudedSurface")

                    Objects(i).WriteToXML(writer)

                    writer.WriteEndElement()

                End If

                If TypeOf Objects(i) Is JetEngine Then

                    writer.WriteStartElement("JetEngine")

                    Objects(i).WriteToXML(writer)

                    writer.WriteEndElement()

                End If

            Next

            writer.WriteEndElement()

        End Sub

#End Region

    End Class

End Namespace
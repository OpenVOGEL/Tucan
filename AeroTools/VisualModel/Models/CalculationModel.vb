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

Namespace VisualModel.Models

    Public Class CalculationModel

        ''' <summary>
        ''' Project name
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String

        Public Property LiftingSurfaces As New List(Of LiftingSurface)
        Public Property Fuselages As New List(Of GeneralSurface)
        Public Property JetEngines As New List(Of JetEngine)

        Private _CurrentLiftingSurfaceIndex As Integer = 0
        Private _CurrentBodyIndex As Integer = 0
        Private _CurrentJetIndex As Integer = 0

        Public Property Selection As New Selection
        Public Property OperationsTool As New OperationsTool
        Public Property PolarDataBase As New UVLM.Models.Aero.Components.PolarDatabase

#Region " Iniciar "

        Public Sub New()

        End Sub

        Public Sub InitializeLiftingSurfaces()

            Dim StartingSurface As New LiftingSurface
            StartingSurface.Name = String.Format("Lifting surface {0}", LiftingSurfaces.Count)
            LiftingSurfaces.Add(StartingSurface)

        End Sub

        Public Sub InitializeBodies()

        End Sub

#End Region

#Region " Add or remove objects "

        ''' <summary>
        ''' Adds a lifting surface and sets it as current.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddLiftingSurface()

            Dim NewSurface = New LiftingSurface
            NewSurface.Name = String.Format("Lifting surface {0}", LiftingSurfaces.Count)
            NewSurface.LocalOrigin.X = LiftingSurfaces.Count
            LiftingSurfaces.Add(NewSurface)
            CurrentLiftingSurfaceID = LiftingSurfaces.Count

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
            NewNacelle.Position.SetToCero()
            NewNacelle.CenterOfRotation.SetToCero()
            NewNacelle.Orientation.SetToCero()
            NewNacelle.VisualProps.ShowColormap = False
            NewNacelle.VisualProps.ShowLoadVectors = False
            NewNacelle.VisualProps.ShowNodes = False
            NewNacelle.VisualProps.ShowVelocityVectors = False
            NewNacelle.VisualProps.ShowPrimitives = False
            NewNacelle.VisualProps.ShowSurface = True
            NewNacelle.VisualProps.ShowMesh = True

            Me.Fuselages.Add(NewNacelle)
            Me.CurrentBodyID = Me.Fuselages.Count

        End Sub

        ''' <summary>
        ''' Adds an extruded body
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddExtrudedBody()

            Dim NewFuselage As New Fuselage
            NewFuselage.Name = String.Format("Fuselage {0}", Fuselages.Count)
            'NewFuselage.GenerateExample()
            NewFuselage.IncludeInCalculation = True
            NewFuselage.Position.SetToCero()
            NewFuselage.CenterOfRotation.SetToCero()
            NewFuselage.Orientation.SetToCero()
            NewFuselage.VisualProps.ShowColormap = False
            NewFuselage.VisualProps.ShowLoadVectors = False
            NewFuselage.VisualProps.ShowNodes = False
            NewFuselage.VisualProps.ShowVelocityVectors = False
            NewFuselage.VisualProps.ShowPrimitives = False
            NewFuselage.VisualProps.ShowSurface = True
            NewFuselage.VisualProps.ShowMesh = True

            Me.Fuselages.Add(NewFuselage)
            Me.CurrentBodyID = Me.Fuselages.Count

        End Sub

        ''' <summary>
        ''' Adds a jet engine
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddJetEngine()

            Dim Engine As New JetEngine
            Engine.Name = String.Format("Jet engine {0}", JetEngines.Count)
            Engine.GenerateMesh()
            Engine.IncludeInCalculation = True
            Engine.Position.SetToCero()
            Engine.CenterOfRotation.SetToCero()
            Engine.Orientation.SetToCero()
            Engine.VisualProps.ShowColormap = False
            Engine.VisualProps.ShowLoadVectors = False
            Engine.VisualProps.ShowNodes = False
            Engine.VisualProps.ShowVelocityVectors = False
            Engine.VisualProps.ShowPrimitives = False
            Engine.VisualProps.ShowSurface = True
            Engine.VisualProps.ShowMesh = True

            JetEngines.Add(Engine)
            CurrentJetEngineID = JetEngines.Count

        End Sub

        ''' <summary>
        ''' Removes the selected surface if it is not the only one in the list.
        ''' </summary>
        ''' <param name="Superficie"></param>
        ''' <remarks></remarks>
        Public Sub RemoveLiftingSurface(ByVal Superficie As Integer)

            If Superficie >= 1 And Superficie <= Me.LiftingSurfaces.Count Then
                Me.LiftingSurfaces.RemoveAt(Superficie - 1)
            End If

        End Sub

        ''' <summary>
        ''' Removes the selected body if it is not the only one on the list.
        ''' </summary>
        ''' <param name="Index"></param>
        ''' <remarks></remarks>
        Public Sub RemoveBody(ByVal Index As Integer)

            If Index >= 1 And Index <= Fuselages.Count Then
                Fuselages.RemoveAt(Index - 1)
            End If

        End Sub

        ''' <summary>
        ''' Removes the selected jet engine if it is not the only one on the list.
        ''' </summary>
        ''' <param name="Index"></param>
        ''' <remarks></remarks>
        Public Sub RemoveJetEngine(ByVal Index As Integer)

            If Index >= 1 And Index <= JetEngines.Count Then
                JetEngines.RemoveAt(Index - 1)
            End If

        End Sub


        ''' <summary>
        ''' Generates a copy of the current lifting surface with the option symmetric in true
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CloneLiftingSurface(ByVal Index As Integer)

            If Index > 0 And Index <= LiftingSurfaces.Count Then
                Me.LiftingSurfaces.Add(Me.LiftingSurfaces(Index - 1).Clone)
                Me.CurrentLiftingSurfaceID = LiftingSurfaces.Count
                CurrentLiftingSurface.Symmetric = True
            End If

        End Sub

#End Region

#Region " Objects "

        ''' <summary>
        ''' Currently active lifting surface
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentLiftingSurface As LiftingSurface
            Get
                If 0 <= _CurrentLiftingSurfaceIndex AndAlso _CurrentLiftingSurfaceIndex < LiftingSurfaces.Count Then
                    Return LiftingSurfaces(_CurrentLiftingSurfaceIndex)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Currently active body
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentBody As GeneralSurface
            Get
                If 0 <= _CurrentBodyIndex AndAlso _CurrentBodyIndex < Fuselages.Count Then
                    Return Fuselages(_CurrentBodyIndex)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        ''' <summary>
        ''' Currently active jet engine
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CurrentJetEngine As JetEngine
            Get
                If 0 <= _CurrentJetIndex AndAlso _CurrentJetIndex < JetEngines.Count Then
                    Return JetEngines(_CurrentJetIndex)
                Else
                    Return Nothing
                End If
            End Get
        End Property

#End Region

#Region " Objects marking "

        Public Property CurrentLiftingSurfaceID As Integer
            Get
                Return Me._CurrentLiftingSurfaceIndex + 1
            End Get
            Set(ByVal value As Integer)
                If value >= 1 And value <= LiftingSurfaces.Count Then
                    _CurrentLiftingSurfaceIndex = value - 1
                Else
                    _CurrentLiftingSurfaceIndex = 0
                End If
            End Set
        End Property

        Public Property CurrentBodyID As Integer
            Get
                Return Me._CurrentBodyIndex + 1
            End Get
            Set(ByVal value As Integer)
                If value >= 1 And value <= Me.Fuselages.Count Then
                    _CurrentBodyIndex = value - 1
                Else
                    _CurrentBodyIndex = 0
                End If
            End Set
        End Property

        Public Property CurrentJetEngineID As Integer
            Get
                Return _CurrentJetIndex + 1
            End Get
            Set(ByVal value As Integer)
                If value >= 1 And value <= JetEngines.Count Then
                    _CurrentJetIndex = value - 1
                Else
                    _CurrentJetIndex = 0
                End If
            End Set
        End Property

        ''' <summary>
        ''' Indicates if the current lifting surface exists.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property LiftingSurfaceSelected As Boolean
            Get
                Return Not IsNothing(CurrentLiftingSurface)
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the current body exists.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property BodySelected As Boolean
            Get
                Return Not IsNothing(CurrentBody)
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the current jet engine exists.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property JetEngineSelected As Boolean
            Get
                Return Not IsNothing(CurrentJetEngine)
            End Get
        End Property

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

                        Dim Index As Integer = 1

                        While SubReader.Read

                            If SubReader.NodeType = XmlNodeType.Element Then

                                If SubReader.Name = String.Format("LiftingSurface{0}", Index) Or SubReader.Name = "LiftingSurface" Then

                                    AddLiftingSurface()

                                    CurrentLiftingSurfaceID = Index

                                    CurrentLiftingSurface.ReadFromXML(SubReader.ReadSubtree)
                                    For i = 1 To CurrentLiftingSurface.nWingRegions
                                        CurrentLiftingSurface.CurrentRegionID = i
                                        If Not CurrentLiftingSurface.CurrentRegion.PolarID.Equals(Guid.Empty) Then
                                            CurrentLiftingSurface.CurrentRegion.PolarFamiliy = PolarDataBase.GetFamilyFromID(CurrentLiftingSurface.CurrentRegion.PolarID)
                                        End If
                                    Next

                                    Index += 1

                                ElseIf SubReader.Name = "ExtrudedSurface" Then

                                    Dim ExtrudedSurface As New Fuselage
                                    ExtrudedSurface.ReadFromXML(reader.ReadSubtree)

                                    Fuselages.Add(ExtrudedSurface)

                                ElseIf SubReader.Name = "JetEngine" Then

                                    Dim JetEngine As New JetEngine
                                    JetEngine.ReadFromXML(reader.ReadSubtree)

                                    JetEngines.Add(JetEngine)

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

            writer.WriteAttributeString("NumberOfLiftingSurfaces", String.Format("{0}", LiftingSurfaces.Count))

            For i = 1 To LiftingSurfaces.Count

                CurrentLiftingSurfaceID = i

                writer.WriteStartElement("LiftingSurface")

                CurrentLiftingSurface.WriteToXML(writer)

                writer.WriteEndElement()

            Next

            For i = 1 To Fuselages.Count

                CurrentBodyID = i

                If TypeOf (CurrentBody) Is Fuselage Then

                    writer.WriteStartElement("ExtrudedSurface")

                    CurrentBody.WriteToXML(writer)

                    writer.WriteEndElement()

                End If

            Next

            For i = 1 To JetEngines.Count

                CurrentJetEngineID = i

                writer.WriteStartElement("JetEngine")

                CurrentJetEngine.WriteToXML(writer)

                writer.WriteEndElement()

            Next

            writer.WriteEndElement()

        End Sub

#End Region

    End Class

End Namespace
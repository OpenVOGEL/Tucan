﻿'OpenVOGEL (openvogel.org)
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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.AeroTools.IoHelper
Imports System.Xml
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.MathTools.Algebra.CustomMatrices

Namespace VisualModel.Models.Components

    Public Class JetEngine

        Inherits Surface

        Public Sub New()

            VisualProperties = New VisualProperties(ComponentTypes.etJetEngine)
            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True

            Length = 2
            FrontDiameter = 1
            BackDiameter = 0.6
            FrontLength = 0.6
            BackLength = 0.4
            Resolution = 15

            Mesh = New Mesh()

            GenerateMesh()

        End Sub

        Public Property Length As Double

        Public Property FrontDiameter As Double

        Public Property BackDiameter As Double

        Public Property FrontLength As Double

        Public Property BackLength As Double

        Public Property ConvectWake As Boolean = True

        Public Property CuttingStep As Integer = 40

        ''' <summary>
        ''' The number of panels in radial direction
        ''' </summary>
        ''' <returns></returns>
        Public Property Resolution As Integer

        ''' <summary>
        ''' Generates a triangular or quadrilateral mesh.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub GenerateMesh()

            Mesh.Nodes.Clear()
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

                    Dim p As New Vector3(x, r * Math.Cos(angle), r * Math.Sin(angle))

                    Mesh.Nodes.Add(New NodalPoint(p))

                    If i > 0 Then

                        Dim N1 As Integer
                        Dim N2 As Integer
                        Dim N3 As Integer
                        Dim N4 As Integer

                        If j < Resolution Then

                            N1 = (i - 1) * (Resolution + 1) + j + 0
                            N2 = (i - 1) * (Resolution + 1) + j + 1
                            N3 = i * (Resolution + 1) + j + 1
                            N4 = i * (Resolution + 1) + j + 0

                            Dim q As New Panel(N1, N4, N3, N2)

                            Mesh.Panels.Add(q)

                        Else

                            N1 = (i - 1) * (Resolution + 1) + j + 0
                            N2 = (i - 1) * (Resolution + 1) + 0
                            N3 = i * (Resolution + 1) + 0
                            N4 = i * (Resolution + 1) + j + 0

                            Dim q As New Panel(N1, N4, N3, N2)

                            Mesh.Panels.Add(q)

                        End If

                    End If

                Next

            Next

            Mesh.Rotate(CenterOfRotation, Orientation.InRadians)

            Mesh.Translate(Position)

            Mesh.GenerateLattice()

            ' Local base:

            Dim LocalRotationMatrix As New RotationMatrix

            LocalRotationMatrix.Generate(Orientation.InRadians)

            LocalDirections.U.X = 1.0
            LocalDirections.U.Y = 0.0
            LocalDirections.U.Z = 0.0
            LocalDirections.U.Rotate(LocalRotationMatrix)

            LocalDirections.V.X = 0.0
            LocalDirections.V.Y = 1.0
            LocalDirections.V.Z = 0.0
            LocalDirections.V.Rotate(LocalRotationMatrix)

            LocalDirections.W.X = 0.0
            LocalDirections.W.Y = 0.0
            LocalDirections.W.Z = 1.0
            LocalDirections.W.Rotate(LocalRotationMatrix)

            ' Launch base sub to raise update event.

            MyBase.GenerateMesh()

        End Sub

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
                        Id = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))
                        IncludeInCalculation = IOXML.ReadBoolean(reader, "Include", True)

                        Resolution = IOXML.ReadInteger(reader, "RE", 10)
                        FrontDiameter = IOXML.ReadDouble(reader, "FD", 1)
                        BackDiameter = IOXML.ReadDouble(reader, "BD", 0.5)
                        FrontLength = IOXML.ReadDouble(reader, "FL", 1)
                        BackLength = IOXML.ReadDouble(reader, "BL", 0.5)
                        Length = IOXML.ReadDouble(reader, "TL", 0.5)
                        Resolution = Math.Max(11, IOXML.ReadInteger(reader, "RS", 15))
                        CuttingStep = IOXML.ReadInteger(reader, "CS", 20)

                        Position.X = IOXML.ReadDouble(reader, "X", 0.0)
                        Position.Y = IOXML.ReadDouble(reader, "Y", 0.0)
                        Position.Z = IOXML.ReadDouble(reader, "Z", 0.0)

                        Orientation.R1 = IOXML.ReadDouble(reader, "Psi", 0)
                        Orientation.R2 = IOXML.ReadDouble(reader, "Theta", 0)
                        Orientation.R3 = IOXML.ReadDouble(reader, "Phi", 0)
                        Orientation.Sequence = IOXML.ReadInteger(reader, "Sequence", CInt(RotationSequence.ZYX))

                        CenterOfRotation.X = IOXML.ReadDouble(reader, "Xcr", 0.0)
                        CenterOfRotation.Y = IOXML.ReadDouble(reader, "Ycr", 0.0)
                        CenterOfRotation.Z = IOXML.ReadDouble(reader, "Zcr", 0.0)

                    Case "VisualProperties"

                        VisualProperties.ReadFromXML(reader.ReadSubtree)

                    Case "Inertia"

                        Dim I As InertialProperties

                        I.Mass = IOXML.ReadDouble(reader, "Mass", 0.0)

                        I.Xcg = IOXML.ReadDouble(reader, "Xcg", 0.0)
                        I.Ycg = IOXML.ReadDouble(reader, "Ycg", 0.0)
                        I.Zcg = IOXML.ReadDouble(reader, "Zcg", 0.0)

                        I.Ixx = IOXML.ReadDouble(reader, "Ixx", 0.0)
                        I.Iyy = IOXML.ReadDouble(reader, "Iyy", 0.0)
                        I.Izz = IOXML.ReadDouble(reader, "Izz", 0.0)

                        I.Ixy = IOXML.ReadDouble(reader, "Ixy", 0.0)
                        I.Ixz = IOXML.ReadDouble(reader, "Ixz", 0.0)
                        I.Iyz = IOXML.ReadDouble(reader, "Iyz", 0.0)

                        Inertia = I

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

            ' Identity
            '-----------------------------------------------------

            writer.WriteStartElement("Identity")
            writer.WriteAttributeString("Name", Name)
            writer.WriteAttributeString("ID", Id.ToString)
            writer.WriteAttributeString("Include", String.Format("{0}", IncludeInCalculation))

            writer.WriteAttributeString("FD", CDbl(FrontDiameter))
            writer.WriteAttributeString("BD", CDbl(BackDiameter))
            writer.WriteAttributeString("FL", CDbl(FrontLength))
            writer.WriteAttributeString("BL", CDbl(BackLength))
            writer.WriteAttributeString("TL", CDbl(Length))
            writer.WriteAttributeString("RS", CInt(Resolution))
            writer.WriteAttributeString("CS", CInt(CuttingStep))

            writer.WriteAttributeString("X", CDbl(Position.X))
            writer.WriteAttributeString("Y", CDbl(Position.Y))
            writer.WriteAttributeString("Z", CDbl(Position.Z))

            writer.WriteAttributeString("Psi", CDbl(Orientation.R1))
            writer.WriteAttributeString("Theta", CDbl(Orientation.R2))
            writer.WriteAttributeString("Phi", CDbl(Orientation.R3))
            writer.WriteAttributeString("Sequence", String.Format("{0}", CInt(Orientation.Sequence)))

            writer.WriteAttributeString("Xcr", String.Format("{0}", Position.X))
            writer.WriteAttributeString("Ycr", String.Format("{0}", Position.Y))
            writer.WriteAttributeString("Zcr", String.Format("{0}", Position.Z))

            writer.WriteAttributeString("RE", CInt(Resolution))
            writer.WriteEndElement()

            ' Visual properties
            '-----------------------------------------------------

            writer.WriteStartElement("VisualProperties")
            VisualProperties.WriteToXML(writer)
            writer.WriteEndElement()

            ' Inertia
            '-----------------------------------------------------

            writer.WriteStartElement("Inertia")

            writer.WriteAttributeString("Mass", String.Format("{0,14:E6}", Inertia.Mass))

            writer.WriteAttributeString("Xcg", String.Format("{0,14:E6}", Inertia.Xcg))
            writer.WriteAttributeString("Ycg", String.Format("{0,14:E6}", Inertia.Ycg))
            writer.WriteAttributeString("Zcg", String.Format("{0,14:E6}", Inertia.Zcg))

            writer.WriteAttributeString("Ixx", String.Format("{0,14:E6}", Inertia.Ixx))
            writer.WriteAttributeString("Iyy", String.Format("{0,14:E6}", Inertia.Iyy))
            writer.WriteAttributeString("Izz", String.Format("{0,14:E6}", Inertia.Izz))

            writer.WriteAttributeString("Ixy", String.Format("{0,14:E6}", Inertia.Ixy))
            writer.WriteAttributeString("Ixz", String.Format("{0,14:E6}", Inertia.Ixz))
            writer.WriteAttributeString("Iyz", String.Format("{0,14:E6}", Inertia.Iyz))

            writer.WriteEndElement()

        End Sub

        Public Sub CopyFrom(Engine As JetEngine)

            Name = Engine.Name + " - Copy"
            Length = Engine.Length
            FrontDiameter = Engine.FrontDiameter
            BackDiameter = Engine.BackDiameter
            FrontLength = Engine.FrontLength
            BackLength = Engine.BackLength
            Resolution = Engine.Resolution

            Position.X = Engine.Position.X - Engine.Length
            Position.Y = Engine.Position.Y
            Position.Z = Engine.Position.Z

            Orientation.R1 = Engine.Orientation.R1
            Orientation.R2 = Engine.Orientation.R2
            Orientation.R3 = Engine.Orientation.R3

            GenerateMesh()

            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True

        End Sub

        Public Overrides Function Clone() As Surface

            Dim ClonedEngine As New JetEngine
            ClonedEngine.CopyFrom(Me)
            ClonedEngine.Position.Y *= -1
            Return ClonedEngine

        End Function

#End Region

    End Class

End Namespace
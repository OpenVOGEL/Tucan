'OpenVOGEL (openvogel.org)
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
        Public Property Resolution As Integer

        ''' <summary>
        ''' Generates a triangular or quadrilateral mesh
        ''' </summary>
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
        ''' <param name="Reader"></param>
        ''' <remarks></remarks>
        Public Overrides Sub ReadFromXML(ByRef Reader As XmlReader)

            While Reader.Read

                Select Case Reader.Name

                    Case "Identity"

                        Name = Reader.GetAttribute("Name")
                        Id = New Guid(IOXML.ReadString(Reader, "ID", Guid.NewGuid.ToString))
                        IncludeInCalculation = IOXML.ReadBoolean(Reader, "Include", True)

                        Resolution = IOXML.ReadInteger(Reader, "RE", 10)
                        FrontDiameter = IOXML.ReadDouble(Reader, "FD", 1)
                        BackDiameter = IOXML.ReadDouble(Reader, "BD", 0.5)
                        FrontLength = IOXML.ReadDouble(Reader, "FL", 1)
                        BackLength = IOXML.ReadDouble(Reader, "BL", 0.5)
                        Length = IOXML.ReadDouble(Reader, "TL", 0.5)
                        Resolution = Math.Max(11, IOXML.ReadInteger(Reader, "RS", 15))
                        CuttingStep = IOXML.ReadInteger(Reader, "CS", 20)

                        Position.X = IOXML.ReadDouble(Reader, "X", 0.0)
                        Position.Y = IOXML.ReadDouble(Reader, "Y", 0.0)
                        Position.Z = IOXML.ReadDouble(Reader, "Z", 0.0)

                        Orientation.Angle1 = IOXML.ReadDouble(Reader, "Psi", 0)
                        Orientation.Angle2 = IOXML.ReadDouble(Reader, "Theta", 0)
                        Orientation.Angle3 = IOXML.ReadDouble(Reader, "Phi", 0)
                        Orientation.Sequence = IOXML.ReadInteger(Reader, "Sequence", CInt(RotationSequence.ZYX))

                        CenterOfRotation.X = IOXML.ReadDouble(Reader, "Xcr", 0.0)
                        CenterOfRotation.Y = IOXML.ReadDouble(Reader, "Ycr", 0.0)
                        CenterOfRotation.Z = IOXML.ReadDouble(Reader, "Zcr", 0.0)

                    Case "VisualProperties"

                        VisualProperties.ReadFromXML(Reader.ReadSubtree)

                    Case "Inertia"

                        Dim I As InertialProperties

                        I.Mass = IOXML.ReadDouble(Reader, "Mass", 0.0)

                        I.Xcg = IOXML.ReadDouble(Reader, "Xcg", 0.0)
                        I.Ycg = IOXML.ReadDouble(Reader, "Ycg", 0.0)
                        I.Zcg = IOXML.ReadDouble(Reader, "Zcg", 0.0)

                        I.Ixx = IOXML.ReadDouble(Reader, "Ixx", 0.0)
                        I.Iyy = IOXML.ReadDouble(Reader, "Iyy", 0.0)
                        I.Izz = IOXML.ReadDouble(Reader, "Izz", 0.0)

                        I.Ixy = IOXML.ReadDouble(Reader, "Ixy", 0.0)
                        I.Ixz = IOXML.ReadDouble(Reader, "Ixz", 0.0)
                        I.Iyz = IOXML.ReadDouble(Reader, "Iyz", 0.0)

                        Inertia = I

                End Select

            End While

            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Writes the wing to an XML file.
        ''' </summary>
        ''' <param name="Writer"></param>
        ''' <remarks></remarks>
        Public Overrides Sub WriteToXML(ByRef Writer As XmlWriter)

            ' Identity
            '-----------------------------------------------------

            Writer.WriteStartElement("Identity")
            Writer.WriteAttributeString("Name", Name)
            Writer.WriteAttributeString("ID", Id.ToString)
            Writer.WriteAttributeString("Include", String.Format("{0}", IncludeInCalculation))

            Writer.WriteAttributeString("FD", CDbl(FrontDiameter))
            Writer.WriteAttributeString("BD", CDbl(BackDiameter))
            Writer.WriteAttributeString("FL", CDbl(FrontLength))
            Writer.WriteAttributeString("BL", CDbl(BackLength))
            Writer.WriteAttributeString("TL", CDbl(Length))
            Writer.WriteAttributeString("RS", CInt(Resolution))
            Writer.WriteAttributeString("CS", CInt(CuttingStep))

            Writer.WriteAttributeString("X", CDbl(Position.X))
            Writer.WriteAttributeString("Y", CDbl(Position.Y))
            Writer.WriteAttributeString("Z", CDbl(Position.Z))

            Writer.WriteAttributeString("Psi", CDbl(Orientation.Angle1))
            Writer.WriteAttributeString("Theta", CDbl(Orientation.Angle2))
            Writer.WriteAttributeString("Phi", CDbl(Orientation.Angle3))
            Writer.WriteAttributeString("Sequence", String.Format("{0}", CInt(Orientation.Sequence)))

            Writer.WriteAttributeString("Xcr", String.Format("{0}", Position.X))
            Writer.WriteAttributeString("Ycr", String.Format("{0}", Position.Y))
            Writer.WriteAttributeString("Zcr", String.Format("{0}", Position.Z))

            Writer.WriteAttributeString("RE", CInt(Resolution))
            Writer.WriteEndElement()

            ' Visual properties
            '-----------------------------------------------------

            Writer.WriteStartElement("VisualProperties")
            VisualProperties.WriteToXML(Writer)
            Writer.WriteEndElement()

            ' Inertia
            '-----------------------------------------------------

            Writer.WriteStartElement("Inertia")

            Writer.WriteAttributeString("Mass", String.Format("{0,14:E6}", Inertia.Mass))

            Writer.WriteAttributeString("Xcg", String.Format("{0,14:E6}", Inertia.Xcg))
            Writer.WriteAttributeString("Ycg", String.Format("{0,14:E6}", Inertia.Ycg))
            Writer.WriteAttributeString("Zcg", String.Format("{0,14:E6}", Inertia.Zcg))

            Writer.WriteAttributeString("Ixx", String.Format("{0,14:E6}", Inertia.Ixx))
            Writer.WriteAttributeString("Iyy", String.Format("{0,14:E6}", Inertia.Iyy))
            Writer.WriteAttributeString("Izz", String.Format("{0,14:E6}", Inertia.Izz))

            Writer.WriteAttributeString("Ixy", String.Format("{0,14:E6}", Inertia.Ixy))
            Writer.WriteAttributeString("Ixz", String.Format("{0,14:E6}", Inertia.Ixz))
            Writer.WriteAttributeString("Iyz", String.Format("{0,14:E6}", Inertia.Iyz))

            Writer.WriteEndElement()

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

            Orientation.Angle1 = Engine.Orientation.Angle1
            Orientation.Angle2 = Engine.Orientation.Angle2
            Orientation.Angle3 = Engine.Orientation.Angle3

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
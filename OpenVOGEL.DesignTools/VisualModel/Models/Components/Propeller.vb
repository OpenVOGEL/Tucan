'#############################################################################
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

'' Standard .NET frameworks dependencies
'-----------------------------------------------------------------------------
Imports System.Xml

'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.MathTools.Algebra.CustomMatrices
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Elements
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.AeroTools.IoHelper
Imports OpenVOGEL.AeroTools.CalculationModel.Settings

'#############################################################################
' Unit: Propeller
'
' This unit provides a propeller model
'#############################################################################
Namespace VisualModel.Models.Components

    Public Class Propeller

        Inherits Surface

        ''' <summary>
        ''' Initializes the propeller using default values
        ''' </summary>
        Public Sub New()

            VisualProperties = New VisualProperties(ComponentTypes.etJetEngine)
            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True

            Mesh = New Mesh
            ExternalDiameter = 2.0
            CentralDiameter = 0.1
            TwistNodes.Add(New Vector2(0.0, 45.0))
            TwistNodes.Add(New Vector2(1.0, 15.0))
            ChordNodes.Add(New Vector2(0.0, 0.25))
            ChordNodes.Add(New Vector2(1.0, 0.1))
            NumberOfBlades = 2
            NumberOfChordPanels = 5
            NumberOfSpanPanels = 20
            GenerateMesh()

        End Sub

        ''' <summary>
        ''' Position of the local origin in global coordinates.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property LocalOrigin As New Vector3

        ''' <summary>
        ''' Cached points used to represent the directions of the local axes.
        ''' </summary>
        Public Property MainDirections As New Base3

        ''' <summary>
        ''' Index of polar curve to be loaded.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PolarId As Guid

        ''' <summary>
        ''' Local polar curve.
        ''' </summary>
        Public Property PolarFamiliy As PolarFamily

#Region " Parametric geometry "

        ''' <summary>
        ''' The diameter of the propeller
        ''' </summary>
        Public Property ExternalDiameter As Double

        ''' <summary>
        ''' The diameter of the inner part (not covered by the blades)
        ''' </summary>
        Public Property CentralDiameter As Double

        ''' <summary>
        ''' Index of polar curve to be loaded.
        ''' </summary>
        Public Property CamberLineId As Guid

        ''' <summary>
        ''' The number of blades
        ''' </summary>
        Public Property NumberOfBlades As Integer

        ''' <summary>
        '''  The number of panels along the span
        ''' </summary>
        Public Property NumberOfSpanPanels As Integer

        ''' <summary>
        ''' The number of panels along the chord
        ''' </summary>
        Public Property NumberOfChordPanels As Integer

        ''' <summary>
        ''' The nodes describing the twist of the blade along the normalized coordinate
        ''' </summary>
        Public Property TwistNodes As New List(Of Vector2)

        ''' <summary>
        ''' Returns the twist at an specific normalized coordinate (rande from 0 to 1)
        ''' </summary>
        Function Twist(X As Double) As Double

            For i = 0 To TwistNodes.Count - 2

                If X > TwistNodes(i).X And X <= TwistNodes(i + 1).X Then

                    Return TwistNodes(i).Y + (TwistNodes(i + 1).Y - TwistNodes(i).Y) * (X - TwistNodes(i).X) / (TwistNodes(i + 1).X - TwistNodes(i).X)

                End If

            Next

            Return 0.0#

        End Function

        ''' <summary>
        ''' The nodes describing the chord of the blade along the normalized coordinate
        ''' </summary>
        Public Property ChordNodes As New List(Of Vector2)

        ''' <summary>
        ''' Returns the twist at an specific normalized coordinate (rande from 0 to 1)
        ''' </summary>
        Function Chord(X As Double) As Double

            For i = 0 To ChordNodes.Count - 2

                If X > ChordNodes(i).X And X <= ChordNodes(i + 1).X Then

                    Return ChordNodes(i).Y + (ChordNodes(i + 1).Y - ChordNodes(i).Y) * (X - ChordNodes(i).X) / (ChordNodes(i + 1).X - ChordNodes(i).X)

                End If

            Next

            Return 0.0#

        End Function

#End Region

#Region " 3D model and vortices generation "

        ''' <summary>
        ''' Generates the lifting surface mesh based on the geometrical parameters.
        ''' </summary>
        Public Overrides Sub GenerateMesh()

            Mesh.Nodes.Clear()
            Mesh.Panels.Clear()
            Mesh.Lattice.Clear()

            ' Load quad panels (base on indices only)
            '---------------------------------------------------------------------

            Dim NumberOfChordNodes As Integer = NumberOfChordPanels + 1

            For p = 1 To NumberOfSpanPanels

                For q = 0 To NumberOfChordPanels - 1

                    Dim Panel As New Panel

                    Panel.N1 = (p - 1) * NumberOfChordNodes + q
                    Panel.N2 = (p - 1) * NumberOfChordNodes + q + 1
                    Panel.N3 = p * NumberOfChordNodes + q + 1
                    Panel.N4 = p * NumberOfChordNodes + q

                    Mesh.Panels.Add(Panel)

                Next

            Next

            ' Load nodes (base on parameters)
            '---------------------------------------------------------------------

            Dim Camber As CamberLine = GetCamberLineFromId(CamberLineId)

            For I = 0 To NumberOfSpanPanels

                Dim K As Double = CDbl(I) / CDbl(NumberOfSpanPanels)
                Dim R As Double = 0.5 * (CentralDiameter + K * (ExternalDiameter - CentralDiameter))
                Dim C As Double = Chord(R)
                Dim T As Double = Twist(R) * Math.PI / 180.0#

                For J = 0 To NumberOfChordPanels

                    Dim Q As Double = 1.0 - CDbl(J) / CDbl(NumberOfChordPanels)
                    Dim B As Double = 0.0#
                    If Camber IsNot Nothing Then
                        B = Camber.Y(Q)
                    End If
                    Dim Node As New NodalPoint
                    Mesh.Nodes.Add(Node)

                    ' Apply profile

                    Node.Position.X = -C * B
                    Node.Position.Y = 0
                    Node.Position.Z = C * Q

                    ' Apply twist (for now only around C/2)

                    Node.Position.Z -= C * 0.5

                    Dim M As New RotationMatrix()
                    Dim A As New OrientationAngles
                    A.R1 = 0
                    A.R2 = T
                    A.R3 = 0
                    A.Sequence = RotationSequence.XYZ
                    M.Generate(A)

                    Node.Position.Rotate(M)

                    ' Apply local position

                    Node.Position.Y += R

                Next

            Next

            Mesh.GenerateLattice()

        End Sub

#End Region

        ''' <summary>
        ''' Generates an identical propeller
        ''' </summary>
        Public Overrides Function Clone() As Surface

            Throw New NotImplementedException()

        End Function

        ''' <summary>
        ''' Writes the data to an XML node
        ''' </summary>
        Public Overrides Sub WriteToXML(ByRef writes As XmlWriter)

            Throw New NotImplementedException()

        End Sub

        ''' <summary>
        ''' Reads the data from an XML node
        ''' </summary>
        Public Overrides Sub ReadFromXML(ByRef reader As XmlReader)

            Throw New NotImplementedException()

        End Sub
    End Class

End Namespace

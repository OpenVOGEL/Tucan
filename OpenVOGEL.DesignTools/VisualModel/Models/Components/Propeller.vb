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
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.AeroTools.IoHelper
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports System.IO

'#############################################################################
' Unit: Propeller
'
' This unit provides a propeller model for any number of blades. The geometry  
' of the blades is generated using the functions that describe the distribution
' of twist and chord along the span (the usual geometry representation available
' on databases). The position coordinate given to the functions is considered 
' as normalized with respect to the radius of the propeller.
' The shape functions are a linear interpolation of provided nodes.
' Take into acconunt that the range of the functions must be from 0 to 1 or 
' there will be troubles while meshing. This range is not checked by the
' mesher.
' The shape of the camber line of the airfoil is constant along the span and
' there is only one polar family (a set of Reynolds dependent polars).
' The collective pitch can be adjusted as independent variable.
'#############################################################################
Namespace VisualModel.Models.Components

    Public Class Propeller

        Inherits Surface

        ''' <summary>
        ''' Initializes the propeller using default values
        ''' </summary>
        Public Sub New()

            VisualProperties = New VisualProperties(ComponentTypes.etPropeller)
            VisualProperties.ShowSurface = True
            VisualProperties.ShowMesh = True
            IncludeInCalculation = True

            Mesh = New Mesh
            NumberOfBlades = 2
            NumberOfChordPanels = 5
            NumberOfSpanPanels = 20
            Diameter = 2.0#
            CentralRing = 10.0#
            CollectivePitch = 0.0#
            TwistFunction.Add(New Vector2(0.0, 45.0))
            TwistFunction.Add(New Vector2(1.0, 15.0))
            ChordFunction.Add(New Vector2(0.0, 0.25))
            ChordFunction.Add(New Vector2(1.0, 0.1))

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
        Public Property Diameter As Double

        ''' <summary>
        ''' The diameter of the inner part relative to the outer diameter (%)
        ''' </summary>
        Public Property CentralRing As Double

        ''' <summary>
        ''' The collective pitch angle
        ''' </summary>
        Public Property CollectivePitch As Double

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
        Public Property TwistFunction As New List(Of Vector2)

        ''' <summary>
        ''' Returns the twist at an specific normalized coordinate (rande from 0 to 1)
        ''' </summary>
        Function Twist(X As Double) As Double

            For i = 0 To TwistFunction.Count - 2

                If X >= TwistFunction(i).X And X <= TwistFunction(i + 1).X Then

                    Return TwistFunction(i).Y + (TwistFunction(i + 1).Y - TwistFunction(i).Y) * (X - TwistFunction(i).X) / (TwistFunction(i + 1).X - TwistFunction(i).X)

                End If

            Next

            Return 0.0#

        End Function

        ''' <summary>
        ''' The nodes describing the chord of the blade along the normalized coordinate
        ''' </summary>
        Public Property ChordFunction As New List(Of Vector2)

        ''' <summary>
        ''' Returns the twist at an specific normalized coordinate (rande from 0 to 1)
        ''' </summary>
        Function Chord(X As Double) As Double

            For i = 0 To ChordFunction.Count - 2

                If X >= ChordFunction(i).X And X <= ChordFunction(i + 1).X Then

                    Return ChordFunction(i).Y + (ChordFunction(i + 1).Y - ChordFunction(i).Y) * (X - ChordFunction(i).X) / (ChordFunction(i + 1).X - ChordFunction(i).X)

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
            Dim NumberOfSpanNodes As Integer = NumberOfSpanPanels + 1
            Dim NumberOfPanels As Integer = NumberOfChordPanels * NumberOfSpanPanels
            Dim NumberOfNodes As Integer = NumberOfChordNodes * NumberOfSpanNodes

            For K = 1 To NumberOfBlades

                Dim LastCount As Integer = (K - 1) * NumberOfNodes

                For I = 1 To NumberOfSpanPanels

                    For J = 0 To NumberOfChordPanels - 1

                        Dim Panel As New Panel

                        Panel.N1 = (I - 1) * NumberOfChordNodes + J + LastCount
                        Panel.N2 = (I - 1) * NumberOfChordNodes + J + 1 + LastCount
                        Panel.N3 = I * NumberOfChordNodes + J + 1 + LastCount
                        Panel.N4 = I * NumberOfChordNodes + J + LastCount

                        Panel.IsSlender = True
                        Panel.IsReversed = False
                        Panel.IsPrimitive = (J = NumberOfChordPanels - 1)

                        Mesh.Panels.Add(Panel)

                    Next

                Next

            Next

            ' Load nodes (base on parameters)
            '---------------------------------------------------------------------

            Dim Camber As CamberLine = GetCamberLineFromId(CamberLineId)
            Dim CentralDiameter As Double = CentralRing * Diameter / 100.0#

            For K = 1 To NumberOfBlades

                Dim F As Double = (K - 1) / NumberOfBlades * 2.0# * Math.PI

                For I = 0 To NumberOfSpanPanels

                    Dim S As Double = CDbl(I) / CDbl(NumberOfSpanPanels)
                    Dim R As Double = 0.5# * (CentralDiameter + S * (Diameter - CentralDiameter))
                    Dim C As Double = 0.5# * Diameter * Chord(S)
                    Dim T As Double = (CollectivePitch + Twist(S)) * Math.PI / 180.0#

                    For J = 0 To NumberOfChordPanels

                        Dim Q As Double = 1.0# - J / NumberOfChordPanels
                        Dim B As Double = 0.0#
                        If Camber IsNot Nothing Then
                            B = Camber.Y(Q)
                        End If
                        Dim Node As New NodalPoint
                        Mesh.Nodes.Add(Node)

                        ' Apply profile

                        Node.Position.X = -C * B
                        Node.Position.Y = 0.0#
                        Node.Position.Z = C * Q

                        ' Apply twist (for now only around C/2)

                        Node.Position.Z -= C * 0.5#

                        Dim M As New RotationMatrix()
                        Dim A As New OrientationAngles
                        A.R1 = 0.0#
                        A.R2 = T
                        A.R3 = 0.0#
                        A.Sequence = RotationSequence.XYZ
                        M.Generate(A)

                        Node.Position.Rotate(M)

                        ' Apply local position

                        Node.Position.Y += R

                        ' Apply blade rotation around proppeller axis (X)

                        A.R1 = F
                        A.R2 = 0.0#
                        A.R3 = 0.0#
                        A.Sequence = RotationSequence.XYZ
                        M.Generate(A)

                        Node.Position.Rotate(M)

                    Next

                Next

            Next

            Mesh.GenerateLattice()

        End Sub

#End Region

        ''' <summary>
        ''' Loads the geometry of the propeller from a text file
        ''' The format of each line must be: r/R c/R beta
        ''' </summary>
        Public Sub LoadFromFile(FilePath As String)

            If File.Exists(FilePath) Then

                Dim FileId = FreeFile()

                FileOpen(FileId, FilePath, OpenMode.Input)

                TwistFunction.Clear()
                ChordFunction.Clear()

                While Not EOF(FileId)

                    Dim Line As String = LineInput(FileId)

                    If Line.Length > 0 AndAlso Line(0) <> "#" Then

                        Dim Values As String() = Line.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

                        If Values.Length = 3 Then

                            Dim R As Double = CDbl(Values(0))

                            ChordFunction.Add(New Vector2(R, CDbl(Values(1))))

                            TwistFunction.Add(New Vector2(R, CDbl(Values(2))))

                        End If

                    End If

                End While

                FileClose(FileId)

                GenerateMesh()

            End If

        End Sub

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

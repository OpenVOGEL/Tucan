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

Imports OpenVOGEL.DesignTools.VisualModel.Interface
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components.Basics
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports System.IO
Imports System.Xml
Imports OpenVOGEL.DesignTools.VisualModel.Tools.Colormaping
Imports OpenVOGEL.MathTools

Namespace VisualModel.Models.Components

    ''' <summary>
    ''' A vector that is attached to a point
    ''' </summary>
    Public Class FixedVector

        ''' <summary>
        ''' The associated point
        ''' </summary>
        Public Point As New Vector3

        ''' <summary>
        ''' The associated vector
        ''' </summary>
        Public Vector As New Vector3

    End Class

    ''' <summary>
    ''' Represents a multi-purpose surface for post-processing.
    ''' </summary>
    Public Class ResultContainer

        Inherits Surface

        ''' <summary>
        ''' All lift vectors
        ''' </summary>
        Public Property LiftVectors As New List(Of FixedVector)

        ''' <summary>
        ''' The maximum lift
        ''' </summary>
        ''' <returns></returns>
        Public Property MaximumLift As Double = 0

        ''' <summary>
        ''' All induced drag vectors
        ''' </summary>
        Public Property InducedDragVectors As New List(Of FixedVector)

        ''' <summary>
        ''' The maximum induced drag
        ''' </summary>
        ''' <returns></returns>
        Public Property MaximumInducedDrag As Double = 0

        ''' <summary>
        ''' All skin drag vectors
        ''' </summary>
        Public Property SkinDragVectors As New List(Of FixedVector)

        ''' <summary>
        ''' The maximum skin drag
        ''' </summary>
        ''' <returns></returns>
        Public Property MaximumSkinDrag As Double = 0

        ''' <summary>
        ''' The different kind of results
        ''' </summary>
        Public Enum ResultKinds

            ''' <summary>
            ''' No results
            ''' </summary>
            None

            ''' <summary>
            ''' The mean pressure over a panel
            ''' </summary>
            PanelPressure

            ''' <summary>
            ''' The velocity computed at the panel control point
            ''' </summary>
            PanelVelocity

            ''' <summary>
            ''' The nodal displacements (for dynamic modes and aeroelasticity)
            ''' </summary>
            NodalDisplacement

        End Enum

        ''' <summary>
        ''' The active result
        ''' </summary>
        ''' <returns></returns>
        Public Property ActiveResult As ResultKinds

        Public Sub New()

            ActiveResult = ResultKinds.None
            Mesh = New Mesh()
            VisualProperties = New VisualProperties(ComponentTypes.etResultContainer)

        End Sub

        ''' <summary>
        ''' Clears the mesh.
        ''' </summary>
        Public Sub Clear()

            Mesh.Nodes.Clear()
            Mesh.Panels.Clear()
            Mesh.Lattice.Clear()

            _GeometryLoaded = False

        End Sub

        ''' <summary>
        ''' Location of this lattice in the database.
        ''' </summary>
        ''' <returns></returns>
        Public Property AccessPath As String

        ''' <summary>
        ''' Extreme values of the local pressure.
        ''' </summary>
        ''' <returns></returns>
        Public Property PressureDeltaRange As New LimitValues

        ''' <summary>
        ''' Extreme values of the local pressure.
        ''' </summary>
        ''' <returns></returns>
        Public Property PressureRange As New LimitValues

        ''' <summary>
        ''' Maximum and minimum displacements.
        ''' </summary>
        ''' <returns></returns>
        Public Property DisplacementRange As New LimitValues

        Private _GeometryLoaded As Boolean = False

        Public ReadOnly Property GeometryLoaded As Boolean
            Get
                If _GeometryLoaded And Not IsNothing(Mesh.Nodes) And Not IsNothing(Mesh.Panels) Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

#Region " Add elements "

        Public Overloads Sub AddNodalPoint(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)

            Dim Posicion As Integer = Mesh.Nodes.Count - 1

            Dim NodalPoint As New NodalPoint

            NodalPoint.ReferencePosition = New Vector3(X, Y, Z)
            NodalPoint.Position.X = X
            NodalPoint.Position.Y = Y
            NodalPoint.Position.Z = Z

            Me.Mesh.Nodes.Add(NodalPoint)

        End Sub

        Public Overloads Sub AddNodalPoint(ByVal Punto As Vector3, Optional ByVal Displacement As Vector3 = Nothing)

            Dim NodalPoint As New NodalPoint
            NodalPoint.ReferencePosition = New Vector3(Punto.X, Punto.Y, Punto.Z)
            NodalPoint.Position.Assign(Punto)
            If Not IsNothing(Displacement) Then
                NodalPoint.Displacement = New Vector3(Displacement)
                NodalPoint.Position.Add(Displacement)
            End If

            Me.Mesh.Nodes.Add(NodalPoint)

        End Sub

        Public Function AddPanel(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal N4 As Integer) As Integer

            Dim Panel As New Panel
            Panel.N1 = N1
            Panel.N2 = N2
            Panel.N3 = N3
            Panel.N4 = N4

            Mesh.Panels.Add(Panel)

            Return Mesh.Panels.Count

        End Function

        Public Function AddPanel(ByVal Panel As Panel) As Integer

            Mesh.Panels.Add(Panel)
            Return Mesh.Panels.Count

        End Function

        ''' <summary>
        ''' Loads the geometry from a calculation model lattice.
        ''' </summary>
        ''' <param name="Lattice"></param>
        Public Sub LoadFromLattice(ByVal Lattice As Lattice)

            For i = 0 To Lattice.Nodes.Count - 1
                AddNodalPoint(Lattice.Nodes(i).Position.X, Lattice.Nodes(i).Position.Y, Lattice.Nodes(i).Position.Z)
            Next

            For i = 0 To Lattice.VortexRings.Count - 1
                AddPanel(Lattice.VortexRings(i).Node(1).IndexL + 1, Lattice.VortexRings(i).Node(2).IndexL + 1, Lattice.VortexRings(i).Node(3).IndexL + 1, Lattice.VortexRings(i).Node(4).IndexL + 1)
            Next

            _GeometryLoaded = True

        End Sub

#End Region

#Region " 3D Functions "

        ''' <summary>
        ''' Updates the position based on the reference nodal position and displacement.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpdateDisplacement(Optional ByVal Scale As Double = 1.0)

            If NumberOfNodes > 0 Then

                For i = 0 To NumberOfNodes - 1

                    If (Not IsNothing(Mesh.Nodes(i).ReferencePosition)) And (Not IsNothing(Mesh.Nodes(i).Displacement)) Then

                        Mesh.Nodes(i).Position.X = Mesh.Nodes(i).ReferencePosition.X + Scale * Mesh.Nodes(i).Displacement.X
                        Mesh.Nodes(i).Position.Y = Mesh.Nodes(i).ReferencePosition.Y + Scale * Mesh.Nodes(i).Displacement.Y
                        Mesh.Nodes(i).Position.Z = Mesh.Nodes(i).ReferencePosition.Z + Scale * Mesh.Nodes(i).Displacement.Z

                    End If
                Next

            End If

        End Sub

        Public Overrides Sub GenerateMesh()

            Throw New Exception("Cannot generate the mesh of a general surface.")

        End Sub

        ''' <summary>
        ''' Generates control points and normal vectors for each panel.
        ''' </summary>
        Public Sub GenerateControlPointsAndNormalVectors()

            Dim Nodo1 As Vector3
            Dim Nodo2 As Vector3
            Dim Nodo3 As Vector3
            Dim Nodo4 As Vector3

            Dim Vector1 As Vector3
            Dim Vector2 As Vector3
            Dim Vector3 As Vector3
            Dim Vector4 As Vector3

            Dim Diagonal1 As New Vector3
            Dim Diagonal2 As New Vector3

            For i = 0 To NumberOfPanels - 1

                If Mesh.Panels(i).IsTriangular Then

                    Nodo1 = Mesh.Nodes(Mesh.Panels(i).N1).Position
                    Nodo2 = Mesh.Nodes(Mesh.Panels(i).N2).Position
                    Nodo3 = Mesh.Nodes(Mesh.Panels(i).N3).Position

                    Vector1 = Nodo1.GetVectorToPoint(Nodo2)
                    Vector2 = Nodo2.GetVectorToPoint(Nodo3)
                    Vector3 = Nodo3.GetVectorToPoint(Nodo1)

                    Mesh.Panels(i).ControlPoint.X = (Nodo1.X + Nodo2.X + Nodo3.X) / 3
                    Mesh.Panels(i).ControlPoint.Y = (Nodo1.Y + Nodo2.Y + Nodo3.Y) / 3
                    Mesh.Panels(i).ControlPoint.Z = (Nodo1.Z + Nodo2.Z + Nodo3.Z) / 3

                    Diagonal1.X = Nodo2.X - Nodo1.X
                    Diagonal1.Y = Nodo2.Y - Nodo1.Y
                    Diagonal1.Z = Nodo2.Z - Nodo1.Z

                    Diagonal2.X = Nodo3.X - Nodo1.X
                    Diagonal2.Y = Nodo3.Y - Nodo1.Y
                    Diagonal2.Z = Nodo3.Z - Nodo1.Z

                    Mesh.Panels(i).NormalVector = Algebra.VectorProduct(Diagonal1, Diagonal2).NormalizedDirection
                    Mesh.Panels(i).Area = 0.5 * Algebra.VectorProduct(Vector1, Vector2).EuclideanNorm

                Else

                    Nodo1 = Mesh.Nodes(Mesh.Panels(i).N1).Position
                    Nodo2 = Mesh.Nodes(Mesh.Panels(i).N2).Position
                    Nodo3 = Mesh.Nodes(Mesh.Panels(i).N3).Position
                    Nodo4 = Mesh.Nodes(Mesh.Panels(i).N4).Position

                    Vector1 = Nodo1.GetVectorToPoint(Nodo2)
                    Vector2 = Nodo2.GetVectorToPoint(Nodo3)
                    Vector3 = Nodo3.GetVectorToPoint(Nodo4)
                    Vector4 = Nodo4.GetVectorToPoint(Nodo1)

                    Mesh.Panels(i).ControlPoint.X = 0.25 * (Nodo1.X + Nodo2.X + Nodo3.X + Nodo4.X)
                    Mesh.Panels(i).ControlPoint.Y = 0.25 * (Nodo1.Y + Nodo2.Y + Nodo3.Y + Nodo4.Y)
                    Mesh.Panels(i).ControlPoint.Z = 0.25 * (Nodo1.Z + Nodo2.Z + Nodo3.Z + Nodo4.Z)

                    Diagonal1.X = Nodo2.X - Nodo4.X
                    Diagonal1.Y = Nodo2.Y - Nodo4.Y
                    Diagonal1.Z = Nodo2.Z - Nodo4.Z

                    Diagonal2.X = Nodo3.X - Nodo1.X
                    Diagonal2.Y = Nodo3.Y - Nodo1.Y
                    Diagonal2.Z = Nodo3.Z - Nodo1.Z

                    Mesh.Panels(i).NormalVector = Algebra.VectorProduct(Diagonal1, Diagonal2).NormalizedDirection
                    Mesh.Panels(i).Area = 0.5 * Algebra.VectorProduct(Vector1, Vector2).EuclideanNorm + 0.5 * Algebra.VectorProduct(Vector3, Vector4).EuclideanNorm

                End If

                If Mesh.Panels(i).IsReversed Then

                    Mesh.Panels(i).NormalVector.Scale(-1.0#)

                End If

            Next

        End Sub

#End Region

#Region " Posprocess UVLM "

        ''' <summary>
        ''' Finds the ragne of pressure over the surface.
        ''' </summary>
        ''' <param name="AbsoluteValue"></param>
        Public Sub FindPressureRange(Optional ByVal AbsoluteValue As Boolean = True)

            Dim FirstWing As Boolean = True
            Dim FirstBody As Boolean = True

            If NumberOfPanels >= 0 And AbsoluteValue Then

                Dim Cp As Double

                For i = 0 To NumberOfPanels - 1

                    Cp = Mesh.Panels(i).Cp

                    If Mesh.Panels(i).IsSlender Then

                        Cp = Math.Abs(Cp)

                        If FirstWing Then
                            PressureDeltaRange.Maximum = Cp
                            PressureDeltaRange.Minimum = Cp
                            FirstWing = False
                        Else
                            If Cp > PressureDeltaRange.Maximum Then PressureDeltaRange.Maximum = Cp
                            If Cp < PressureDeltaRange.Minimum Then PressureDeltaRange.Minimum = Cp
                        End If

                    Else

                        If FirstBody Then
                            PressureRange.Maximum = Cp
                            PressureRange.Minimum = Cp
                            FirstBody = False
                        Else
                            If Cp > PressureRange.Maximum Then PressureRange.Maximum = Cp
                            If Cp < PressureRange.Minimum Then PressureRange.Minimum = Cp
                        End If

                    End If

                Next

            End If

        End Sub

        ''' <summary>
        ''' Updates the map with pressure.
        ''' </summary>
        Public Sub UpdatePressureColormap()

            For Each Panel In Mesh.Panels

                If Panel.IsSlender Then
                    Panel.CpColor = Colormap.ScalarToColor(Math.Abs(Panel.Cp),
                                                           PressureDeltaRange.Maximum,
                                                           PressureDeltaRange.Minimum)
                Else
                    Panel.CpColor = Colormap.ScalarToColor(Panel.Cp,
                                                           PressureRange.Maximum,
                                                           PressureRange.Minimum)
                End If
            Next

        End Sub

        Public Sub FindDisplacementsRange()

            If NumberOfNodes >= 1 Then

                DisplacementRange.Maximum = Mesh.Nodes(0).Displacement.EuclideanNorm
                DisplacementRange.Minimum = DisplacementRange.Maximum

                Dim d As Double

                For i = 0 To NumberOfNodes - 1

                    d = Mesh.Nodes(i).Displacement.EuclideanNorm
                    If d > DisplacementRange.Maximum Then DisplacementRange.Maximum = d
                    If d < DisplacementRange.Minimum Then DisplacementRange.Minimum = d

                Next

            End If

        End Sub

        Public Sub UpdateColormapWithDisplacements()

            For Each Node In Mesh.Nodes

                Node.DisplacementColor = Colormap.ScalarToColor(Node.Displacement.EuclideanNorm, DisplacementRange.Maximum, DisplacementRange.Minimum)

            Next

        End Sub

        Public Sub FindBestVelocityScale()

            ' Find max diagonal / velocity ratio

            VisualProperties.ScaleVelocityVectors = 0.0#

            For Each Panel In Mesh.Panels

                Dim Diagonal = Mesh.Nodes(Panel.N1).Position.DistanceTo(Mesh.Nodes(Panel.N3).Position)
                Diagonal = Math.Min(Diagonal, Mesh.Nodes(Panel.N2).Position.DistanceTo(Mesh.Nodes(Panel.N4).Position))
                Dim Velocity As Double = Panel.LocalVelocity.EuclideanNorm
                If Velocity > 0.0# Then
                    VisualProperties.ScaleVelocityVectors = Math.Max(VisualProperties.ScaleVelocityVectors, 0.2 * Diagonal / Velocity)
                End If

            Next

        End Sub

#End Region

#Region " IO "

        Public Overrides Sub WriteToXML(ByRef writes As XmlWriter)

        End Sub

        Public Overrides Sub ReadFromXML(ByRef reader As XmlReader)

        End Sub

        Public Sub WriteToBinary()

            Dim bw As BinaryWriter = New BinaryWriter(New FileStream(AccessPath, FileMode.Create))

            bw.Write(NumberOfNodes)
            For Each Node In Me.Mesh.Nodes
                bw.Write(Node.Position.X)
                bw.Write(Node.Position.Y)
                bw.Write(Node.Position.Z)
            Next

            bw.Write(NumberOfPanels)
            For Each p In Mesh.Panels

                bw.Write(p.N1)
                bw.Write(p.N2)
                bw.Write(p.N3)
                bw.Write(p.N4)
                bw.Write(p.Circulation)
                bw.Write(p.Cp)
                bw.Write(p.LocalVelocity.X)
                bw.Write(p.LocalVelocity.Y)
                bw.Write(p.LocalVelocity.Z)

            Next

            bw.Close()

        End Sub

        Public Sub ReadFromBinary()

            If Not File.Exists(AccessPath) Then Throw New Exception("Result file could not have been found.")

            Dim br As BinaryReader = New BinaryReader(New FileStream(AccessPath, FileMode.Open))

            Dim n As Integer = br.ReadInt32()

            For i = 1 To n

                AddNodalPoint(br.ReadDouble, br.ReadDouble, br.ReadDouble)

            Next

            Mesh.Panels.Clear()

            n = br.ReadInt32()

            For i = 1 To n

                AddPanel(br.ReadInt32, br.ReadInt32, br.ReadInt32, br.ReadInt32)
                Mesh.Panels(i - 1).Circulation = br.ReadDouble
                Mesh.Panels(i - 1).Cp = br.ReadDouble
                Mesh.Panels(i - 1).LocalVelocity.X = br.ReadDouble
                Mesh.Panels(i - 1).LocalVelocity.Y = br.ReadDouble
                Mesh.Panels(i - 1).LocalVelocity.Z = br.ReadDouble

            Next

            br.Close()

            Mesh.GenerateLattice()

        End Sub

        ''' <summary>
        ''' Loads the data from a text file (using the access path).
        ''' </summary>
        ''' <returns></returns>
        Public Function LoadFromTextFile() As Boolean

            Try

                Dim Line As String

                FileOpen(25, AccessPath, OpenMode.Input, OpenAccess.Read)

                Line = LineInput(25)
                Me.Name = Line

                Line = LineInput(25)

                Line = LineInput(25)
                Dim NumeroDeNodos As Integer = CInt(Right(Line, 5))

                Line = LineInput(25)
                Dim NumeroDePaneles As Integer = CInt(Right(Line, 5))

                Do Until Trim(Line) = "## MATRICES"
                    Line = LineInput(25)
                Loop

                Line = LineInput(25) ' Lee el espacio

                Mesh.Nodes.Clear()
                Mesh.Panels.Clear()

                For i = 1 To NumeroDeNodos ' Comienza a leer la matriz de coordenadas

                    Line = LineInput(25)
                    AddNodalPoint(CDbl(Left(Line, 13)), CDbl(Mid(Line, 14, 12)), CDbl(Right(Line, 13)))

                Next

                For i = 1 To NumeroDePaneles ' Comienza a leer la matriz de conectividad

                    Line = LineInput(25)
                    AddPanel(CInt(Left(Line, 5)),
                                                                              CInt(Mid(Line, 6, 5)),
                                                                              CInt(Mid(Line, 11, 5)),
                                                                              CInt(Right(Line, 4)))
                Next

                FileClose(25)

                Me._GeometryLoaded = True

                Dim ErrorDeTamaño As Boolean = Not ((NumeroDeNodos = Mesh.Nodes.Count) And (NumeroDePaneles = Mesh.Panels.Count) Or Mesh.Nodes.Count >= 0 Or Mesh.Panels.Count >= 0)

                If ErrorDeTamaño Then

                    Clear()

                    Return False

                End If

                Mesh.GenerateLattice()

                MsgBox("Geometria cargada correctamente.", MsgBoxStyle.Information)

                Return True

            Catch ex1 As Exception

                _GeometryLoaded = False

                Clear()

                Try
                    FileClose(25)
                Catch

                End Try

                Return False

            End Try

        End Function

        Public Overrides Function Clone() As Surface

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
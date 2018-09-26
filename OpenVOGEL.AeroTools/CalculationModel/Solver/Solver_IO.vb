'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Imports System.IO
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports System.Xml
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library
Imports OpenVOGEL.AeroTools.VisualModel.Models
Imports OpenVOGEL.AeroTools.VisualModel.Models.Components
Imports OpenVOGEL.AeroTools.VisualModel.IO
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Nodes
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Elements
Imports OpenVOGEL.AeroTools.DataStore
Imports OpenVOGEL.AeroTools.IoHelper

Namespace CalculationModel.Solver

    Partial Public Class Solver

        ''' <summary>
        ''' Assign surrounding rings by searching on all lattices
        ''' </summary>
        Public Sub FindSurroundingRingsGlobally()

            Dim STOL As Double = Settings.SurveyTolerance

            Dim Order4(4, 2) As Integer

            Order4(0, 0) = 1
            Order4(0, 1) = 2

            Order4(1, 0) = 2
            Order4(1, 1) = 3

            Order4(2, 0) = 3
            Order4(2, 1) = 4

            Order4(3, 0) = 4
            Order4(3, 1) = 1

            Dim Order3(3, 2) As Integer

            Order3(0, 0) = 1
            Order3(0, 1) = 2

            Order3(1, 0) = 2
            Order3(1, 1) = 3

            Order3(2, 0) = 3
            Order3(2, 1) = 1

            For Each Lattice In Lattices

                Dim mi As EVector3
                Dim mj As EVector3

                Dim ni As EVector3
                Dim nj As EVector3

                For Each Ring In Lattice.VortexRings

                    If Not Ring.IsSlender Then Continue For ' Do not find adjacent panels of non-slender rings (not required)

                    Ring.InitializeSurroundingRings()

                    If Ring.Type = VortexRingType.VR4 Then

                        For m = 0 To 3 ' For each local segment:

                            mi = Ring.Node(Order4(m, 0)).Position
                            mj = Ring.Node(Order4(m, 1)).Position

                            For Each OtherLattice In Lattices

                                For Each OtherRing In OtherLattice.VortexRings

                                    If Ring.IndexG <> OtherRing.IndexG Then

                                        If OtherRing.Type = VortexRingType.VR4 Then

                                            For n = 0 To 3 ' For each other segment:

                                                ni = OtherRing.Node(Order4(n, 0)).Position
                                                nj = OtherRing.Node(Order4(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        ElseIf OtherRing.Type = VortexRingType.VR3 Then

                                            For n = 0 To 2 ' For each other segment:

                                                ni = OtherRing.Node(Order3(n, 0)).Position
                                                nj = OtherRing.Node(Order3(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        End If

                                    End If

                                Next

                            Next

                        Next

                    ElseIf Ring.Type = VortexRingType.VR3 Then

                        For m = 0 To 2 ' For each local segment:

                            mi = Ring.Node(Order3(m, 0)).Position
                            mj = Ring.Node(Order3(m, 1)).Position

                            For Each OtherLattice In Lattices

                                For Each OtherRing In OtherLattice.VortexRings

                                    If Ring.IndexG <> OtherRing.IndexG Then

                                        If OtherRing.Type = VortexRingType.VR4 Then

                                            For n = 0 To 3 ' For each other segment:

                                                ni = OtherRing.Node(Order4(n, 0)).Position
                                                nj = OtherRing.Node(Order4(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        ElseIf OtherRing.Type = VortexRingType.VR3 Then

                                            For n = 0 To 2 ' For each other segment:

                                                ni = OtherRing.Node(Order3(n, 0)).Position
                                                nj = OtherRing.Node(Order3(n, 1)).Position

                                                If (mi.DistanceTo(ni) < STOL And mj.DistanceTo(nj) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, -1)
                                                ElseIf (mi.DistanceTo(nj) < STOL And mj.DistanceTo(ni) < STOL) Then
                                                    Ring.AttachNeighbourAtSide(m, OtherRing, 1)
                                                End If

                                            Next

                                        End If

                                    End If

                                Next

                            Next

                        Next

                    End If

                Next

                ' Set primitive flag on primitive panels:

                For Each Wake In Lattice.Wakes

                    For Each Primitive In Wake.Primitive.Rings
                        Lattice.VortexRings(Primitive).IsPrimitive = True
                    Next

                Next

            Next

        End Sub

        ''' <summary>
        ''' Read a written step
        ''' </summary>
        ''' <param name="FilePath"></param>
        ''' <remarks></remarks>
        Public Sub ReadFromXML(ByVal FilePath As String)

            If Not File.Exists(FilePath) Then
                MsgBox("Results file not found!")
                Exit Sub
            End If

            Dim reader As XmlReader = XmlReader.Create(FilePath)

            If reader.ReadToFollowing("Solver", "UVLMSolver") Then

                Dim nBLattices As Integer = reader.GetAttribute("BLattices")
                Dim nLinks As Integer = reader.GetAttribute("Links")

                _StreamVelocity.X = IOXML.ReadDouble(reader, "VX", 1.0)
                _StreamVelocity.Y = IOXML.ReadDouble(reader, "VY", 0.0)
                _StreamVelocity.Z = IOXML.ReadDouble(reader, "VZ", 0.0)
                _StreamOmega.X = IOXML.ReadDouble(reader, "OX", 0.0)
                _StreamOmega.Y = IOXML.ReadDouble(reader, "OY", 0.0)
                _StreamOmega.Z = IOXML.ReadDouble(reader, "OZ", 0.0)
                _StreamDensity = IOXML.ReadDouble(reader, "Rho", 1.225)

                If _StreamDensity = 0 Then _StreamDensity = 1.225

                _StreamDynamicPressure = 0.5 * _StreamVelocity.SquareEuclideanNorm * _StreamDensity

                Try
                    If File.Exists(FilePath & ".Polars.bin") Then
                        If IsNothing(PolarDataBase) Then
                            PolarDataBase = New PolarDatabase
                        End If
                        PolarDataBase.ReadBinary(FilePath & ".Polars.bin")
                    End If
                Catch ex As Exception
                    PolarDataBase = Nothing
                End Try

                For i = 1 To nBLattices
                    Lattices.Add(New BoundedLattice())
                    Lattices(i - 1).ReadBinary(FilePath & String.Format(".lat_{0}.bin", i), PolarDataBase)
                Next

                If (nLinks > 0) Then

                    Dim NodalStak As New List(Of Node)
                    Dim RingStak As New List(Of VortexRing)

                    Dim nIndex As Integer = 0
                    Dim eIndex As Integer = 0

                    For Each Lattice In Lattices
                        For Each Node In Lattice.Nodes
                            Node.IndexG = nIndex
                            NodalStak.Add(Node)
                            nIndex += 1
                        Next
                        For Each Ring In Lattice.VortexRings
                            Ring.IndexG = eIndex
                            RingStak.Add(Ring)
                            eIndex += 1
                        Next
                    Next

                    StructuralLinks = New List(Of StructuralLink)

                    For i = 1 To nLinks
                        StructuralLinks.Add(New StructuralLink())
                        StructuralLinks(i - 1).ReadBinary(FilePath & String.Format(".link_{0}.bin", i), NodalStak, RingStak)
                    Next

                End If

                If reader.ReadToFollowing("Simulacion", "TSimulacion") Then
                    Settings.ReadFromXML(reader.ReadSubtree)
                Else
                    MsgBox("Warning! Unable to read simulation parameters.")
                End If

            End If

            reader.Close()

            Settings.GenerateVelocityProfile()

            CalculateAirloads()

        End Sub

        ''' <summary>
        ''' Writes the current step
        ''' </summary>
        ''' <param name="FilePath"></param>
        ''' <remarks></remarks>
        Public Sub WriteToXML(ByVal FilePath As String, Optional ByVal WakesNodalVelocity As Boolean = False)

            Dim writer As XmlWriter = XmlWriter.Create(FilePath)

            writer.WriteStartElement("Solver", "UVLMSolver")

            writer.WriteAttributeString("VX", _StreamVelocity.X)
            writer.WriteAttributeString("VY", _StreamVelocity.Y)
            writer.WriteAttributeString("VZ", _StreamVelocity.Z)
            writer.WriteAttributeString("OX", _StreamOmega.X)
            writer.WriteAttributeString("OY", _StreamOmega.Y)
            writer.WriteAttributeString("OZ", _StreamOmega.Z)
            writer.WriteAttributeString("Rho", _StreamDensity)

            writer.WriteAttributeString("BLattices", Lattices.Count)

            For i = 1 To Lattices.Count
                Lattices(i - 1).WriteBinary(FilePath & String.Format(".lat_{0}.bin", i), WakesNodalVelocity)
            Next

            If IsNothing(StructuralLinks) Then
                writer.WriteAttributeString("Links", 0)
            Else
                writer.WriteAttributeString("Links", StructuralLinks.Count)
                For i = 1 To StructuralLinks.Count
                    StructuralLinks(i - 1).WriteBinary(FilePath & String.Format(".link_{0}.bin", i))
                Next
            End If

            writer.WriteStartElement("Simulacion", "TSimulacion")
            Settings.SaveToXML(writer)
            writer.WriteEndElement()

            PolarDataBase.WriteBinary(FilePath & ".Polars.bin")

            writer.WriteEndElement()

            writer.Close()

        End Sub

#Region "Sub folders"

        Private Enum DataBaseSection As Byte

            Base = 0
            Structural = 1
            Aeroelastic = 2
            Steady = 3
            Unsteady = 4

        End Enum

        Const DB_File_Structure = "_Structural"
        Const DB_File_Aeroelastic = "_Aeroelastic"
        Const DB_File_Steady = "_Steady"
        Const DB_File_Transit = "_Unsteady"

        Private Base_Path As String
        Private Structure_Path As String
        Private Aeroelastic_Path As String
        Private Steady_Path As String
        Private Transit_Path As String

        ''' <summary>
        ''' Creates the subfolders where results are stored
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateSubFoldersNames(ByVal DataBasePath As String)

            If DataBasePath IsNot Nothing AndAlso DataBasePath <> "" Then

                Dim Base_Path As String = Path.GetDirectoryName(DataBasePath) + "\" + Path.GetFileNameWithoutExtension(DataBasePath)

                Structure_Path = Base_Path + DB_File_Structure
                Aeroelastic_Path = Base_Path + DB_File_Aeroelastic
                Steady_Path = Base_Path + DB_File_Steady
                Transit_Path = Base_Path + Transit_Path

            Else

                RaiseEvent PushMessage("Database path not defined. Cancellation requested.")
                RequestCancellation()

            End If

        End Sub

        Private Sub CreateSubFolder(ByVal DataBaseSection As DataBaseSection)

            Try

                Select Case DataBaseSection

                    Case Solver.DataBaseSection.Aeroelastic
                        System.IO.Directory.CreateDirectory(Aeroelastic_Path)

                    Case Solver.DataBaseSection.Steady
                        System.IO.Directory.CreateDirectory(Steady_Path)

                    Case Solver.DataBaseSection.Structural
                        System.IO.Directory.CreateDirectory(Structure_Path)

                    Case Solver.DataBaseSection.Unsteady
                        System.IO.Directory.CreateDirectory(Transit_Path)

                End Select

            Catch e As Exception

                RaiseEvent PushMessage("Cannot create subfoders. Cancellation requested.")
                RequestCancellation()

            End Try

        End Sub

        ''' <summary>
        ''' Removes all calculation files from the selected path
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CleanDirectory(ByVal DataBaseSection As DataBaseSection)

            Dim path As String = ""

            Select Case DataBaseSection

                Case Solver.DataBaseSection.Aeroelastic
                    path = Aeroelastic_Path

                Case Solver.DataBaseSection.Steady
                    path = Steady_Path

                Case Solver.DataBaseSection.Structural
                    path = Structure_Path

                Case Solver.DataBaseSection.Unsteady
                    path = Transit_Path

            End Select

            Try

                Dim Files As String() = System.IO.Directory.GetFiles(path)

                For Each FileName In Files

                    File.Delete(FileName)

                Next

            Catch ex As Exception

            End Try


        End Sub

#End Region

    End Class

End Namespace


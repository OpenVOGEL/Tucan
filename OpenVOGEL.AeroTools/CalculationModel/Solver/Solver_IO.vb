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

Imports System.IO
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports System.Xml
Imports OpenVOGEL.AeroTools.IoHelper

Namespace CalculationModel.Solver

    Partial Public Class Solver

        ''' <summary>
        ''' The version of the solver
        ''' </summary>
        Public Const Version As String = "1.2-2019.06"

        ''' <summary>
        ''' Read a written step
        ''' </summary>
        ''' <param name="FilePath"></param>
        ''' <remarks></remarks>
        Public Sub ReadFromXML(ByVal FilePath As String)

            If Not File.Exists(FilePath) Then
                RaiseEvent PushMessage("Results file not found!")
                Exit Sub
            End If

            Dim reader As XmlReader = XmlReader.Create(FilePath)

            If reader.ReadToFollowing("Solver") Then

                Dim nLattices As Integer = reader.GetAttribute("Lattices")
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

                For i = 1 To nLattices
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

                If reader.ReadToFollowing("Settings") Then
                    Settings.ReadFromXML(reader.ReadSubtree)
                Else
                    RaiseEvent PushMessage("Warning: unable to read settings")
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

            writer.WriteStartElement("Solver")

            writer.WriteAttributeString("Version", Version)

            writer.WriteAttributeString("VX", _StreamVelocity.X)
            writer.WriteAttributeString("VY", _StreamVelocity.Y)
            writer.WriteAttributeString("VZ", _StreamVelocity.Z)
            writer.WriteAttributeString("OX", _StreamOmega.X)
            writer.WriteAttributeString("OY", _StreamOmega.Y)
            writer.WriteAttributeString("OZ", _StreamOmega.Z)
            writer.WriteAttributeString("Rho", _StreamDensity)

            writer.WriteAttributeString("Lattices", Lattices.Count)

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

            writer.WriteStartElement("Settings")
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

                Dim Base_Path As String = Path.Combine(Path.GetDirectoryName(DataBasePath), Path.GetFileNameWithoutExtension(DataBasePath))

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

        Public Sub ReportResults()

            RaiseEvent PushResultLine("RESULS OF THE AERODYNAMIC ANALYSIS:")
            RaiseEvent PushResultLine("")

            RaiseEvent PushResultLine("Reference velocity:")
            RaiseEvent PushResultLine(String.Format("V = {0:F6}m/s", StreamVelocity.EuclideanNorm))
            RaiseEvent PushResultLine(String.Format("Vx = {0:F6}m/s, Vy = {1:F6}m/s, Vz = {2:F6}m/s",
                                            StreamVelocity.X,
                                            StreamVelocity.Y,
                                            StreamVelocity.Z))
            RaiseEvent PushResultLine(String.Format("Rho = {0:F6}kg/m³", StreamDensity))
            RaiseEvent PushResultLine(String.Format("q = {0:F6}Pa", StreamDynamicPressure))

            Dim i As Integer = 0

            For Each Lattice In Lattices

                i += 1

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine(String.Format("LATTICE {0}", i))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Total area: (ΣSi)")
                RaiseEvent PushResultLine(String.Format("S = {0:F6}m²", Lattice.AirLoads.Area))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine(String.Format("CL = {0:F6}", Lattice.AirLoads.CL))
                RaiseEvent PushResultLine(String.Format("CDi = {0:F6}", Lattice.AirLoads.CDi))
                RaiseEvent PushResultLine(String.Format("CDp = {0:F6}", Lattice.AirLoads.CDp))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Force due to local lift")

                RaiseEvent PushResultLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.SlenderForce.X))
                RaiseEvent PushResultLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.SlenderForce.Y))
                RaiseEvent PushResultLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.SlenderForce.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Moment due to local lift")

                RaiseEvent PushResultLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.X))
                RaiseEvent PushResultLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.SlenderMoment.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Force due to local induced drag")

                RaiseEvent PushResultLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.InducedDrag.X))
                RaiseEvent PushResultLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.InducedDrag.Y))
                RaiseEvent PushResultLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.InducedDrag.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Moment due to local induced drag")

                RaiseEvent PushResultLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.InducedMoment.X))
                RaiseEvent PushResultLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.InducedMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.InducedMoment.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Force due to local skin drag")

                RaiseEvent PushResultLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.SkinDrag.X))
                RaiseEvent PushResultLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.SkinDrag.Y))
                RaiseEvent PushResultLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.SkinDrag.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Moment due to local skin drag")

                RaiseEvent PushResultLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.SkinMoment.X))
                RaiseEvent PushResultLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.SkinMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.SkinMoment.Z))
                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("Force on body")

                RaiseEvent PushResultLine(String.Format("Fx/qS = {0:F8}", Lattice.AirLoads.BodyForce.X))
                RaiseEvent PushResultLine(String.Format("Fy/qS = {0:F8}", Lattice.AirLoads.BodyForce.Y))
                RaiseEvent PushResultLine(String.Format("Fz/qS = {0:F8}", Lattice.AirLoads.BodyForce.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Moment on body")

                RaiseEvent PushResultLine(String.Format("Mx/qS = {0:F8}", Lattice.AirLoads.BodyMoment.X))
                RaiseEvent PushResultLine(String.Format("My/qS = {0:F8}", Lattice.AirLoads.BodyMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz/qS = {0:F8}", Lattice.AirLoads.BodyMoment.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("Chordwise stations:")
                RaiseEvent PushResultLine("CD, CDi, CDp")

                For Each cl In Lattice.ChordWiseStripes
                    RaiseEvent PushResultLine(String.Format("{0:F8}, {1:F8}, {2:F8}", cl.CL, cl.CDi, cl.CDp))
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("Local vortex ring properties:")
                RaiseEvent PushResultLine("Index, Cp, Area, G, S, Vx, Vy, Vz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    RaiseEvent PushResultLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}, {4,12:F8}, {5,12:F8}, {6,12:F8}, {7,12:F8}", Ring.IndexL, Ring.Cp, Ring.Area, Ring.G, Ring.S, Ring.VelocityT.X, Ring.VelocityT.Y, Ring.VelocityT.Z))
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("Control point")
                RaiseEvent PushResultLine("Index, CPx, CPy, CPz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    RaiseEvent PushResultLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.ControlPoint.X, Ring.ControlPoint.Y, Ring.ControlPoint.Z))
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("Outer control point")
                RaiseEvent PushResultLine("Index, CPx, CPy, CPz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    If Ring.OuterControlPoint IsNot Nothing Then
                        RaiseEvent PushResultLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.OuterControlPoint.X, Ring.OuterControlPoint.Y, Ring.OuterControlPoint.Z))
                    End If
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("Index, Nx, Ny, Nz")
                For Each Ring As VortexRing In Lattice.VortexRings
                    RaiseEvent PushResultLine(String.Format("{0,4:D}: {1,12:F8}, {2,12:F8}, {3,12:F8}", Ring.IndexL, Ring.Normal.X, Ring.Normal.Y, Ring.Normal.Z))
                Next

            Next

        End Sub

    End Class

End Namespace


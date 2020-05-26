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
        Public Const Version As String = "2.1-2020.05"

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

                Stream.Velocity.X = IOXML.ReadDouble(reader, "VX", 1.0)
                Stream.Velocity.Y = IOXML.ReadDouble(reader, "VY", 0.0)
                Stream.Velocity.Z = IOXML.ReadDouble(reader, "VZ", 0.0)
                Stream.Omega.X = IOXML.ReadDouble(reader, "OX", 0.0)
                Stream.Omega.Y = IOXML.ReadDouble(reader, "OY", 0.0)
                Stream.Omega.Z = IOXML.ReadDouble(reader, "OZ", 0.0)
                Stream.Density = IOXML.ReadDouble(reader, "Rho", 1.225)

                If Stream.Density = 0 Then Stream.Density = 1.225

                Stream.DynamicPressure = 0.5 * Stream.Velocity.SquareEuclideanNorm * Stream.Density

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

            Settings.GenerateVelocityHistogram()

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

            writer.WriteAttributeString("VX", Stream.Velocity.X)
            writer.WriteAttributeString("VY", Stream.Velocity.Y)
            writer.WriteAttributeString("VZ", Stream.Velocity.Z)
            writer.WriteAttributeString("OX", Stream.Omega.X)
            writer.WriteAttributeString("OY", Stream.Omega.Y)
            writer.WriteAttributeString("OZ", Stream.Omega.Z)
            writer.WriteAttributeString("Rho", Stream.Density)

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

        Const DatabaseFileStructure = "_Structural"
        Const DatabaseFileAeroelastic = "_Aeroelastic"
        Const DatabaseFileSteady = "_Steady"
        Const DatabaseFileTransit = "_Unsteady"

        Public BasePath As String
        Public StructurePath As String
        Public AeroelasticPath As String
        Public SteadyPath As String
        Public TransitPath As String

        ''' <summary>
        ''' Creates the subfolders where results are stored
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CreateSubFoldersNames(ByVal DataBasePath As String)

            If DataBasePath IsNot Nothing AndAlso DataBasePath <> "" Then

                BasePath = Path.Combine(Path.GetDirectoryName(DataBasePath), Path.GetFileNameWithoutExtension(DataBasePath))
                StructurePath = BasePath + DatabaseFileStructure
                AeroelasticPath = BasePath + DatabaseFileAeroelastic
                SteadyPath = BasePath + DatabaseFileSteady
                TransitPath = BasePath + TransitPath

            Else

                RaiseEvent PushMessage("Database path not defined. Cancellation requested.")
                RequestCancellation()

            End If

        End Sub

        Private Sub CreateSubFolder(ByVal DataBaseSection As DataBaseSection)

            Try

                Select Case DataBaseSection

                    Case Solver.DataBaseSection.Aeroelastic
                        System.IO.Directory.CreateDirectory(AeroelasticPath)

                    Case Solver.DataBaseSection.Steady
                        System.IO.Directory.CreateDirectory(SteadyPath)

                    Case Solver.DataBaseSection.Structural
                        System.IO.Directory.CreateDirectory(StructurePath)

                    Case Solver.DataBaseSection.Unsteady
                        System.IO.Directory.CreateDirectory(TransitPath)

                End Select

            Catch e As Exception

                RaiseEvent PushMessage("Cannot create subfoders. Cancellation requested.")
                RequestCancellation()

            End Try

        End Sub

        ''' <summary>
        ''' The file where the steady state results are written
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property SteadyResFile As String
            Get
                Return System.IO.Path.Combine(SteadyPath, "Steady.res")
            End Get
        End Property

        ''' <summary>
        ''' The file where the aeroelastic results are written
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AeroelasticResFile(TimeStep As Integer) As String
            Get
                Return System.IO.Path.Combine(AeroelasticPath, String.Format("Aeroelastic_{0}.res", TimeStep))
            End Get
        End Property

        ''' <summary>
        ''' The file where the transit results are written
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property TransitResFile(TimeStep As Integer) As String
            Get
                Return System.IO.Path.Combine(AeroelasticPath, String.Format("Transit_{0}.res", TimeStep))
            End Get
        End Property

        ''' <summary>
        ''' Removes all calculation files from the selected path
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CleanDirectory(ByVal DataBaseSection As DataBaseSection)

            Dim path As String = ""

            Select Case DataBaseSection

                Case Solver.DataBaseSection.Aeroelastic
                    path = AeroelasticPath

                Case Solver.DataBaseSection.Steady
                    path = SteadyPath

                Case Solver.DataBaseSection.Structural
                    path = StructurePath

                Case Solver.DataBaseSection.Unsteady
                    path = TransitPath

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

            RaiseEvent PushResultLine("RESULS OF THE AERODYNAMIC ANALYSIS")
            RaiseEvent PushResultLine(String.Format("OpenVOGEL kernel: {0}", Version))
            RaiseEvent PushResultLine("")

            RaiseEvent PushResultLine("# Reference velocity [m/s]")
            RaiseEvent PushResultLine(String.Format("V  = {0,14:E6}", StreamVelocity.EuclideanNorm))
            RaiseEvent PushResultLine(String.Format("Vx = {0,14:E6}", StreamVelocity.X))
            RaiseEvent PushResultLine(String.Format("Vy = {0,14:E6}", StreamVelocity.Y))
            RaiseEvent PushResultLine(String.Format("Vz = {0,14:E6}", StreamVelocity.Z))
            RaiseEvent PushResultLine("")

            RaiseEvent PushResultLine("# Dynamic pressure")
            RaiseEvent PushResultLine(String.Format("Rho = {0,14:E6} kg/m³", StreamDensity))
            RaiseEvent PushResultLine(String.Format("q   = {0,14:E6} Pa", StreamDynamicPressure))

            Dim i As Integer = 0

            For Each Lattice In Lattices

                i += 1

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine(String.Format("LATTICE {0}", i))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Total area (ΣSi) [m²]")
                RaiseEvent PushResultLine(String.Format("S = {0,14:E6}", Lattice.AirLoads.Area))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Classic dimensionless force coefficients")
                RaiseEvent PushResultLine(String.Format("CL  = {0,14:E6}", Lattice.AirLoads.LiftCoefficient))
                RaiseEvent PushResultLine(String.Format("CDi = {0,14:E6}", Lattice.AirLoads.InducedDragCoefficient))
                RaiseEvent PushResultLine(String.Format("CDp = {0,14:E6}", Lattice.AirLoads.SkinDragCoefficient))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Force due to local lift [N]")

                RaiseEvent PushResultLine(String.Format("Fx = {0,14:E6}", Lattice.AirLoads.LiftForce.X))
                RaiseEvent PushResultLine(String.Format("Fy = {0,14:E6}", Lattice.AirLoads.LiftForce.Y))
                RaiseEvent PushResultLine(String.Format("Fz = {0,14:E6}", Lattice.AirLoads.LiftForce.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Moment due to local lift [Nm]")

                RaiseEvent PushResultLine(String.Format("Mx = {0,14:E6}", Lattice.AirLoads.LiftMoment.X))
                RaiseEvent PushResultLine(String.Format("My = {0,14:E6}", Lattice.AirLoads.LiftMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz = {0,14:E6}", Lattice.AirLoads.LiftMoment.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Force due to local induced drag [N]")

                RaiseEvent PushResultLine(String.Format("Fx = {0,14:E6}", Lattice.AirLoads.InducedDragForce.X))
                RaiseEvent PushResultLine(String.Format("Fy = {0,14:E6}", Lattice.AirLoads.InducedDragForce.Y))
                RaiseEvent PushResultLine(String.Format("Fz = {0,14:E6}", Lattice.AirLoads.InducedDragForce.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Moment due to local induced drag [Nm]")

                RaiseEvent PushResultLine(String.Format("Mx = {0,14:E6}", Lattice.AirLoads.InducedDragMoment.X))
                RaiseEvent PushResultLine(String.Format("My = {0,14:E6}", Lattice.AirLoads.InducedDragMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz = {0,14:E6}", Lattice.AirLoads.InducedDragMoment.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Force due to local skin drag [N]")

                RaiseEvent PushResultLine(String.Format("Fx = {0,14:E6}", Lattice.AirLoads.SkinDragForce.X))
                RaiseEvent PushResultLine(String.Format("Fy = {0,14:E6}", Lattice.AirLoads.SkinDragForce.Y))
                RaiseEvent PushResultLine(String.Format("Fz = {0,14:E6}", Lattice.AirLoads.SkinDragForce.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Moment due to local skin drag [Nm]")

                RaiseEvent PushResultLine(String.Format("Mx = {0,14:E6}", Lattice.AirLoads.SkinDragMoment.X))
                RaiseEvent PushResultLine(String.Format("My = {0,14:E6}", Lattice.AirLoads.SkinDragMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz = {0,14:E6}", Lattice.AirLoads.SkinDragMoment.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Force on body [N]")

                RaiseEvent PushResultLine(String.Format("Fx = {0,14:E6}", Lattice.AirLoads.BodyForce.X))
                RaiseEvent PushResultLine(String.Format("Fy = {0,14:E6}", Lattice.AirLoads.BodyForce.Y))
                RaiseEvent PushResultLine(String.Format("Fz = {0,14:E6}", Lattice.AirLoads.BodyForce.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Moment on body [Nm]")

                RaiseEvent PushResultLine(String.Format("Mx = {0,14:E6}", Lattice.AirLoads.BodyMoment.X))
                RaiseEvent PushResultLine(String.Format("My = {0,14:E6}", Lattice.AirLoads.BodyMoment.Y))
                RaiseEvent PushResultLine(String.Format("Mz = {0,14:E6}", Lattice.AirLoads.BodyMoment.Z))
                RaiseEvent PushResultLine("")

                RaiseEvent PushResultLine("# Spanwise load distribution (dimensionless coefficients)")
                RaiseEvent PushResultLine(String.Format("{0,-14} {1,-14} {2,-14}", "CD", "CDi", "CDp"))

                For Each Stripe In Lattice.ChordWiseStripes
                    RaiseEvent PushResultLine(String.Format("{0,14:E6} {1,14:E6} {2,14:E6}", Stripe.LiftCoefficient, Stripe.InducedDragCoefficient, Stripe.SkinDragCoefficient))
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("# Vortex rings (velocity in [m/s])")
                RaiseEvent PushResultLine(String.Format("{0,-4} {1,-14} {2,-14} {3,-14} {4,-14} {5,-14} {6,-14} {7,-14}", "Index", "Cp", "Area", "G", "S", "Vx", "Vy", "Vz"))
                For Each Ring As VortexRing In Lattice.VortexRings
                    RaiseEvent PushResultLine(String.Format("{0,4:D}: {1,14:E6} {2,14:E6} {3,14:E6} {4,14:E6} {5,14:E6} {6,14:E6} {7,14:E6}", Ring.IndexL, Ring.Cp, Ring.Area, Ring.G, Ring.S, Ring.VelocityT.X, Ring.VelocityT.Y, Ring.VelocityT.Z))
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("# Control points [m]")
                RaiseEvent PushResultLine(String.Format("{0,-4} {1,-14} {2,-14} {3,-14}", "Index", "X", "Y", "Z"))
                For Each Ring As VortexRing In Lattice.VortexRings
                    RaiseEvent PushResultLine(String.Format("{0,4:D} {1,14:E6} {2,14:E6} {3,14:E6}", Ring.IndexL, Ring.ControlPoint.X, Ring.ControlPoint.Y, Ring.ControlPoint.Z))
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("# Outer control points [m]")
                RaiseEvent PushResultLine(String.Format("{0,-4} {1,-14} {2,-14} {3,-14}", "Index", "X", "Y", "Z"))
                For Each Ring As VortexRing In Lattice.VortexRings
                    If Ring.OuterControlPoint IsNot Nothing Then
                        RaiseEvent PushResultLine(String.Format("{0,4:D}: {1,14:E6}, {2,14:E6}, {3,14:E6}", Ring.IndexL, Ring.OuterControlPoint.X, Ring.OuterControlPoint.Y, Ring.OuterControlPoint.Z))
                    End If
                Next

                RaiseEvent PushResultLine("")
                RaiseEvent PushResultLine("# Normal vectors")
                RaiseEvent PushResultLine(String.Format("{0,-4} {1,-14} {2,-14} {3,-14}", "Index", "X", "Y", "Z"))
                For Each Ring As VortexRing In Lattice.VortexRings
                    RaiseEvent PushResultLine(String.Format("{0,4:D}: {1,14:E6}, {2,14:E6}, {3,14:E6}", Ring.IndexL, Ring.Normal.X, Ring.Normal.Y, Ring.Normal.Z))
                Next

            Next

        End Sub

    End Class

End Namespace


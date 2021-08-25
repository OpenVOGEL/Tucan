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

'' Standard .NET dependencies
'-----------------------------------------------------------------------------
Imports System.IO
Imports System.Xml

'' OpenVOGEL dependencies
'-----------------------------------------------------------------------------
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural
Imports OpenVOGEL.AeroTools.IoHelper
Imports OpenVOGEL.MathTools.Integration
Imports OpenVOGEL.AeroTools.CalculationModel.Settings

'#############################################################################
' Unit: Solver_IO
'
' This units provides methods to read and write the model in binary and XML
' formats.
''#############################################################################
Namespace CalculationModel.Solver

    Partial Public Class Solver

        ''' <summary>
        ''' The version of the solver.
        ''' The first number is increased when functionality changes are introduced.
        ''' The second number is increased when there are fixes without touching the basic features.
        ''' </summary>
        Public Shared ReadOnly Property Version As String = "5.0-2021.08"

        ''' <summary>
        ''' Read a written snapshot
        ''' </summary>
        ''' <param name="FilePath"></param>
        ''' <remarks></remarks>
        Public Sub ReadFromXML(ByVal FilePath As String)

            If Not File.Exists(FilePath) Then
                RaiseEvent PushMessage("Results file not found!")
                Exit Sub
            End If

            Dim Reader As XmlReader = XmlReader.Create(FilePath)

            If Reader.ReadToFollowing("Solver") Then

                Dim nLattices As Integer = Reader.GetAttribute("Lattices")
                Dim nLinks As Integer = Reader.GetAttribute("Links")

                Stream.Velocity.X = IOXML.ReadDouble(Reader, "Vx", 1.0)
                Stream.Velocity.Y = IOXML.ReadDouble(Reader, "Vy", 0.0)
                Stream.Velocity.Z = IOXML.ReadDouble(Reader, "Vz", 0.0)
                Stream.Rotation.X = IOXML.ReadDouble(Reader, "Ox", 0.0)
                Stream.Rotation.Y = IOXML.ReadDouble(Reader, "Oy", 0.0)
                Stream.Rotation.Z = IOXML.ReadDouble(Reader, "Oz", 0.0)
                Stream.Density = IOXML.ReadDouble(Reader, "Rho", 1.225)

                If Stream.Density = 0 Then Stream.Density = 1.225

                Stream.DynamicPressure = 0.5 * Stream.Velocity.SquareEuclideanNorm * Stream.Density

                Try
                    If File.Exists(FilePath & ".Polars.bin") Then
                        If IsNothing(PolarDataBase) Then
                            PolarDataBase = New PolarDatabase
                        End If
                        PolarDataBase.ReadBinary(FilePath & ".Polars.bin")
                    End If
                Catch
                    PolarDataBase = Nothing
                End Try

                For i = 1 To nLattices
                    Lattices.Add(New BoundedLattice())
                    Lattices(i - 1).ReadBinary(FilePath & String.Format(".Lattice_{0}.bin", i), PolarDataBase)
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

                    For i = 0 To nLinks - 1
                        StructuralLinks.Add(New StructuralLink())
                        StructuralLinks(i).ReadBinary(FilePath & String.Format(".Link_{0}.bin", i), NodalStak, RingStak)
                    Next

                End If

                Try
                    If File.Exists(FilePath & ".Response.bin") Then
                        If Motion Is Nothing Then
                            Motion = New MotionIntegrator(FilePath & ".Response.bin")
                        End If
                    End If
                Catch
                    Motion = Nothing
                End Try

                If Reader.ReadToFollowing("Settings") Then
                    Settings.ReadFromXML(Reader.ReadSubtree)
                Else
                    RaiseEvent PushMessage("Warning: unable to read settings")
                End If

            End If

            Reader.Close()

            Settings.GenerateVelocityHistogram()

            CalculateAirloads()

        End Sub

        ''' <summary>
        ''' Writes a snapshot of the current solver
        ''' </summary>
        ''' <param name="FilePath"></param>
        ''' <remarks></remarks>
        Public Sub WriteToXML(ByVal FilePath As String, Optional ByVal WakesNodalVelocity As Boolean = False)

            Dim Writer As XmlWriter = XmlWriter.Create(FilePath)

            Writer.WriteStartElement("Solver")

            Writer.WriteAttributeString("Version", Version)

            Writer.WriteAttributeString("Vx", Stream.Velocity.X)
            Writer.WriteAttributeString("Vy", Stream.Velocity.Y)
            Writer.WriteAttributeString("Vz", Stream.Velocity.Z)
            Writer.WriteAttributeString("Ox", Stream.Rotation.X)
            Writer.WriteAttributeString("Oy", Stream.Rotation.Y)
            Writer.WriteAttributeString("Oz", Stream.Rotation.Z)
            Writer.WriteAttributeString("Rho", Stream.Density)

            ' Lattices
            '------------------------------------------
            Writer.WriteAttributeString("Lattices", Lattices.Count)

            For i = 0 To Lattices.Count - 1
                Lattices(i).WriteBinary(FilePath & String.Format(".Lattice_{0}.bin", i), WakesNodalVelocity)
            Next

            ' Dynamic links
            '------------------------------------------
            If StructuralLinks Is Nothing Then
                Writer.WriteAttributeString("Links", 0)
            Else
                Writer.WriteAttributeString("Links", StructuralLinks.Count)
                For i = 0 To StructuralLinks.Count - 1
                    StructuralLinks(i).WriteBinary(FilePath & String.Format(".Link_{0}.bin", i))
                Next
            End If

            ' Settings
            '------------------------------------------
            Writer.WriteStartElement("Settings")
            Settings.SaveToXML(Writer)
            Writer.WriteEndElement()

            ' Polars
            '------------------------------------------
            PolarDataBase.WriteBinary(FilePath & ".Polars.bin")

            Writer.WriteEndElement()
            Writer.Close()

        End Sub

        ''' <summary>
        ''' Writes all lattices to the given directory using the FrameIndex for the name
        ''' </summary>
        ''' <param name="DirectoryPath"></param>
        ''' <param name="FrameIndex"></param>
        Public Sub WriteLattices(ByVal DirectoryPath As String, FrameIndex As Integer)

            For I = 0 To Lattices.Count - 1
                Lattices(I).WriteBinary(System.IO.Path.Combine(DirectoryPath, String.Format("Lattice_{0}_{1}.bin", FrameIndex, I)), False)
            Next

        End Sub

        ''' <summary>
        ''' Reads all the lattices named for the given FrameIndex in the given directory.
        ''' </summary>
        ''' <param name="DirectoryPath"></param>
        ''' <param name="FrameIndex"></param>
        Public Shared Sub ReadLattices(ByVal DirectoryPath As String,
                                       ByVal FrameIndex As Integer,
                                       ByRef Lattices As List(Of BoundedLattice),
                                       ByRef PolarDatabase As PolarDatabase)

            Dim LatticeIndex = 0
            Dim KeepReading As Boolean = True

            If Lattices IsNot Nothing Then
                Lattices.Clear()
            End If

            While KeepReading
                Dim File As String = System.IO.Path.Combine(DirectoryPath, String.Format("Lattice_{0}_{1}.bin", FrameIndex, LatticeIndex))
                If System.IO.File.Exists(File) Then

                    If Lattices Is Nothing Then
                        Lattices = New List(Of BoundedLattice)
                    End If

                    Dim Lattice As New BoundedLattice
                    Lattice.ReadBinary(File, PolarDatabase, True)
                    Lattices.Add(Lattice)
                    LatticeIndex += 1

                Else
                    KeepReading = False
                End If
            End While

        End Sub

        ''' <summary>
        ''' Loads all the lattices named for the given FrameIndex in the given directory.
        ''' </summary>
        ''' <param name="DirectoryPath"></param>
        ''' <param name="FrameIndex"></param>
        Public Sub LoadLattices(ByVal DirectoryPath As String, FrameIndex As Integer)

            ReadLattices(DirectoryPath,
                         FrameIndex,
                         Lattices,
                         PolarDataBase)
        End Sub

        ''' <summary>
        ''' Writes the structural links to the given directory
        ''' </summary>
        ''' <param name="DirectoryPath"></param>
        Public Sub WriteStructuralLinks(ByVal DirectoryPath As String)

            For i = 0 To StructuralLinks.Count - 1

                StructuralLinks(i).WriteBinary(Path.Combine(DirectoryPath, String.Format("Link_{0}.bin", i)))

            Next

        End Sub

        ''' <summary>
        ''' Reads the structural links from the given directory
        ''' </summary>
        ''' <param name="DirectoryPath"></param>
        Public Shared Sub ReadStructuralLinks(ByVal DirectoryPath As String,
                                              ByRef Lattices As List(Of BoundedLattice),
                                              ByRef StructuralLinks As List(Of StructuralLink))

            Dim NodalStack As New List(Of Node)
            Dim RingStack As New List(Of VortexRing)

            Dim First As Boolean = True
            Dim LinkIndex = 0
            Dim KeepReading As Boolean = True

            While KeepReading

                Dim FilePath As String = Path.Combine(DirectoryPath, String.Format("Link_{0}.bin", LinkIndex))

                If File.Exists(FilePath) Then

                    If First Then

                        ' Initialize the stacks
                        '--------------------------------------------

                        StructuralLinks = New List(Of StructuralLink)
                        NodalStack = New List(Of Node)
                        RingStack = New List(Of VortexRing)

                        Dim nIndex As Integer = 0
                        Dim eIndex As Integer = 0

                        For Each Lattice In Lattices
                            For Each Node In Lattice.Nodes
                                Node.IndexG = nIndex
                                NodalStack.Add(Node)
                                nIndex += 1
                            Next
                            For Each Ring In Lattice.VortexRings
                                Ring.IndexG = eIndex
                                RingStack.Add(Ring)
                                eIndex += 1
                            Next
                        Next

                        First = False

                    End If

                    StructuralLinks.Add(New StructuralLink())

                    StructuralLinks(LinkIndex).ReadBinary(FilePath, NodalStack, RingStack)

                    LinkIndex += 1

                Else
                    KeepReading = False

                End If

            End While

        End Sub

        ''' <summary>
        ''' Loads the structural links from the given directory
        ''' </summary>
        ''' <param name="DirectoryPath"></param>
        Public Sub LoadStructuralLinks(ByVal DirectoryPath As String)

            ReadStructuralLinks(DirectoryPath,
                                Lattices,
                                StructuralLinks)

        End Sub

        ''' <summary>
        ''' Writes a file for the information of the data
        ''' </summary>
        ''' <param name="DirectoryPath"></param>
        Private Sub WriteInfoFile(DirectoryPath As String,
                                  CalculationMode As CalculationType)

            Dim FileId As Integer = FreeFile()
            FileOpen(FileId, IO.Path.Combine(DirectoryPath, "Info.vri"), OpenMode.Output)

            PrintLine(FileId, "OpenVOGEL database directory")
            PrintLine(FileId, String.Format("Kernel version:     {0}", Version))
            PrintLine(FileId, String.Format("Calculation date:   {0}", DateAndTime.Now.ToShortDateString))
            PrintLine(FileId, String.Format("Calculation kind:   {0}", CalculationMode.ToString))
            PrintLine(FileId, String.Format("Number of lattices: {0}", Lattices.Count))

            FileClose(FileId)

        End Sub

#Region "Sub folders"

        Const DirectoryNameSteadyState = "_SteadyState"
        Const DirectoryNameAeroelastic = "_Aeroelastic"
        Const DirectoryNameFreeFlight = "_FreeFlight"

        ''' <summary>
        ''' The working directory
        ''' </summary>
        Public Property BaseDirectoryPath As String

        ''' <summary>
        ''' Creates the subfolder for the results
        ''' </summary>
        ''' <param name="DataBaseSection"></param>
        ''' <param name="ReferenceFilePath"></param>
        Private Sub CreateSubFolder(ByVal DataBaseSection As CalculationType, ByVal ReferenceFilePath As String)

            Try

                BaseDirectoryPath = Path.Combine(Path.GetDirectoryName(ReferenceFilePath), Path.GetFileNameWithoutExtension(ReferenceFilePath))

                Select Case DataBaseSection

                    Case CalculationType.SteadyState
                        BaseDirectoryPath += DirectoryNameSteadyState
                        System.IO.Directory.CreateDirectory(BaseDirectoryPath)

                    Case CalculationType.Aeroelastic
                        BaseDirectoryPath += DirectoryNameAeroelastic
                        System.IO.Directory.CreateDirectory(BaseDirectoryPath)

                    Case CalculationType.FreeFlight
                        BaseDirectoryPath += DirectoryNameFreeFlight
                        System.IO.Directory.CreateDirectory(BaseDirectoryPath)

                End Select

                CleanDirectory()

            Catch e As Exception

                RaiseEvent PushMessage("Cannot create subfoders. Cancellation requested.")
                RequestCancellation()

            End Try

        End Sub

        ''' <summary>
        ''' Removes all calculation files from the selected path
        ''' </summary>
        Private Sub CleanDirectory()

            Try

                Dim Files As String() = System.IO.Directory.GetFiles(BaseDirectoryPath)

                For Each FileName In Files

                    File.Delete(FileName)

                Next

            Catch

            End Try


        End Sub

#End Region

        ''' <summary>
        ''' Generates an standard output providing data on the current solver state.
        ''' To get the data connect a handler to the "PushResultLine" event.
        ''' </summary>
        Public Sub ReportResults()

            RaiseEvent PushResultLine("RESULS OF THE AERODYNAMIC ANALYSIS")
            RaiseEvent PushResultLine(String.Format("OpenVOGEL kernel:  {0}", Version))
            RaiseEvent PushResultLine("")

            RaiseEvent PushResultLine("# Reference velocity [m/s]")
            RaiseEvent PushResultLine(String.Format("V  = {0,14:E6}", StreamVelocity.Norm2))
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


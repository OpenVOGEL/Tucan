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

Imports OpenVOGEL.DesignTools.VisualModel.Models
Imports OpenVOGEL.AeroTools.CalculationModel.Solver
Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports System.Xml
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

Namespace DataStore

    Public Module ProjectRoot

        ''' <summary>
        ''' The name of the project
        ''' </summary>
        ''' <returns></returns>
        Public Property Name As String = "New aircraft"

        ''' <summary>
        ''' The file path where the project is hosted
        ''' </summary>
        ''' <returns></returns>
        Public Property FilePath As String = ""

        ''' <summary>
        ''' The settings used to run the simulation
        ''' </summary>
        ''' <returns></returns>
        Public Property SimulationSettings As New SimulationSettings

        ''' <summary>
        ''' The complete design model made of high level components
        ''' </summary>
        ''' <returns></returns>
        Public Property Model As DesignModel = New DesignModel

        ''' <summary>
        ''' The result model
        ''' </summary>
        ''' <returns></returns>
        Public Property Results As New ResultModel

        ''' <summary>
        ''' The calculation core
        ''' </summary>
        ''' <returns></returns>
        Public Property CalculationCore As Solver

        ''' <summary>
        ''' A set of coplanar points where the velocity can be evaluated using the calculation core
        ''' </summary>
        ''' <returns></returns>
        Public Property VelocityPlane As New VelocityPlane

        Private _Initialized As Boolean = False

        ''' <summary>
        ''' Indicates if the project has been initialized
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property Initialized As Boolean
            Get
                Return _Initialized
            End Get
        End Property

        ''' <summary>
        ''' Initializes the module
        ''' </summary>
        Public Sub Initialize()

            SimulationSettings.InitializaParameters()
            Results.InitializeResults()
            _Initialized = True

        End Sub

        ''' <summary>
        ''' Restarts the project by reseting the model an clearing the results
        ''' </summary>
        Public Sub RestartProject()

            Name = "New aircraft"
            Model.Objects.Clear()
            SimulationSettings.InitializaParameters()
            Results.InitializeResults()
            CalculationCore = Nothing

        End Sub

        ''' <summary>
        ''' Indicates if the current file path exist
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ExistsOnDatabase As Boolean
            Get
                Return System.IO.File.Exists(FilePath)
            End Get
        End Property

#Region " Basic calculation launcher "

        ''' <summary>
        ''' Starts a synchronous calculation process.
        ''' This call is used for the console, but is less sutable for an animated GUI.
        ''' </summary>
        ''' <param name="Type"></param>
        Public Sub StartCalculation(ByVal Type As CalculationType)

            SimulationSettings.AnalysisType = Type

            Try

                RaiseEvent PushMessage("Preparing calculation cell")
                Dim BuildStructure As Boolean = Type = CalculationType.ctAeroelastic
                CalculationCore = New Solver
                CalculationCore.GenerateFromExistingModel(Model, SimulationSettings, BuildStructure)

                AddHandler CalculationCore.PushProgress, AddressOf HandleProgress
                AddHandler CalculationCore.PushMessage, AddressOf HandleMessage
                AddHandler CalculationCore.CalculationDone, AddressOf CalculationFinished
                AddHandler CalculationCore.CalculationAborted, AddressOf CalculationAborted

                Dim StartingTime As Date = Now
                Results.SimulationSettings = CalculationCore.Settings
                RaiseEvent PushMessage("Calculating with parallel solver")

                Select Case Type

                    Case CalculationType.ctSteady

                        CalculationCore.SteadyState(FilePath)

                    Case CalculationType.ctUnsteady

                        CalculationCore.UnsteadyTransit(FilePath)

                    Case CalculationType.ctAeroelastic
                        CalculationCore.AeroelasticUnsteadyTransit(FilePath)

                End Select

            Catch ex As Exception
                RaiseEvent PushMessage(String.Format("Calculation exited with exception: ""{0}"".", ex.Message))
                RaiseEvent PushMessage("Exited with exception!")
                RaiseEvent CalculationDone()
                Return
            End Try

        End Sub

        ''' <summary>
        ''' Handles the calculation finished of the solver
        ''' </summary>
        Private Sub CalculationFinished()

            If CalculationCore IsNot Nothing Then
                RaiseEvent PushMessage("Ready")
                RaiseEvent PushMessage("Calculation done")
                RaiseEvent CalculationDone()
            End If

        End Sub

        ''' <summary>
        ''' Handles the calculation aborted of the solver
        ''' </summary>
        Private Sub CalculationAborted()

            RaiseEvent PushMessage("Calculation aborted")
            RaiseEvent CalculationDone()

        End Sub

        ''' <summary>
        ''' Occurs when the calculation finishes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Event CalculationDone()

        ''' <summary>
        ''' Occurs when the calculation finishes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Event PushProgress(Title As String, Value As Integer)

        ''' <summary>
        ''' Occurs when the calculation finishes.
        ''' </summary>
        ''' <remarks></remarks>
        Public Event PushMessage(Message As String)

        ''' <summary>
        ''' Handles the progress of the solver
        ''' </summary>
        ''' <param name="Progress"></param>
        ''' <param name="Message"></param>
        Public Sub HandleProgress(Title As String, Value As Integer)

            RaiseEvent PushProgress(Title, Value)

        End Sub

        ''' <summary>
        ''' Handles the messages of the solver
        ''' </summary>
        ''' <param name="Message"></param>
        Public Sub HandleMessage(Message As String)

            RaiseEvent PushMessage(Message)

        End Sub

#End Region

#Region " Input/Output "

        ''' <summary>
        ''' Reads a full project from XML file
        ''' </summary>
        Public Sub ReadFromXML()

            If Not ExistsOnDatabase Then Exit Sub

            Dim Reader As XmlReader = XmlReader.Create(FilePath)

            If Reader.ReadToDescendant("Project") Then

                ReadFromXML(Reader)

            End If

            Reader.Close()

        End Sub

        ''' <summary>
        ''' Reads a full project from XML subtree
        ''' </summary>
        Public Sub ReadFromXML(Reader As XmlReader)

            Name = Reader.GetAttribute("Name")

            While Reader.Read

                Select Case Reader.Name

                    Case "Model"
                        Model.ReadFromXML(Reader.ReadSubtree)

                    Case "Simulacion"
                        SimulationSettings.ReadFromXML(Reader.ReadSubtree)

                    Case "VelocityPlane"
                        VelocityPlane.ReadFromXML(Reader.ReadSubtree)

                End Select

            End While

        End Sub

        ''' <summary>
        ''' Writes the full project to an XML file
        ''' </summary>
        Public Sub WriteToXML()

            Dim Writer As XmlWriter = XmlWriter.Create(FilePath)

            Writer.WriteStartElement("OpenVOGEL")

            Writer.WriteStartElement("Project")
            WriteToXML(Writer)
            Writer.WriteEndElement()

            Writer.WriteEndElement()

            Writer.Close()

        End Sub

        ''' <summary>
        ''' Writes the full project to an XML subtree
        ''' </summary>
        Public Sub WriteToXML(Writer As XmlWriter)

            Writer.WriteAttributeString("Name", Name)

            Writer.WriteStartElement("Model")
            Model.WriteToXML(Writer)
            Writer.WriteEndElement()

            Writer.WriteStartElement("Simulacion")
            SimulationSettings.SaveToXML(Writer)
            Writer.WriteEndElement()

            Writer.WriteStartElement("VelocityPlane")
            VelocityPlane.SaveToXML(Writer)
            Writer.WriteEndElement()

        End Sub

        ''' <summary>
        ''' Loads the results from an XML results file
        ''' </summary>
        ''' <param name="FilePath"></param>
        Public Sub ReadResults(ByVal FilePath As String)

            CalculationCore = New Solver()
            CalculationCore.ReadFromXML(FilePath)
            CalculationCore.SetCompleteModelOnResults(Results)

        End Sub

        Public Enum ExportTypes As Integer
            ExportStl
            ExportCalculix
        End Enum

        ''' <summary>
        ''' Exports the model geometry in the given format.
        ''' </summary>
        ''' <param name="Type"></param>
        ''' <param name="OneFile"></param>
        Public Sub ExportDesignModel(Type As ExportTypes, OneFile As Boolean)

            Dim FileName As String = Strings.Left(FilePath, FilePath.Length - 4)

            Select Case Type

                Case ExportTypes.ExportStl

                    If OneFile Then

                        ' Write all solids in a single file

                        FileName = FileName & ".stl"

                        Dim FileId As Integer = FreeFile()
                        FileOpen(FileId, FileName, OpenMode.Output)
                        PrintLine(FileId, "solid Model")
                        FileClose(FileId)

                        For Each Surface In Model.Objects
                            Surface.ExportStlFile(FileName, True)
                        Next

                        FileId = FreeFile()
                        FileOpen(FileId, FileName, OpenMode.Append)
                        PrintLine(FileId, "endsolid Model")
                        FileClose(FileId)

                    Else

                        ' Write each model surface in a different file

                        Dim I As Integer = 0

                        For Each Surface In Model.Objects

                            Dim Name As String = Surface.Name

                            If Name = "" Then
                                Name = "Surface_" & I
                            End If

                            Surface.ExportStlFile(FileName & "_" & Name & ".stl", False)

                        Next

                    End If

                Case ExportTypes.ExportCalculix

                    If OneFile Then

                        ' Write all solids in a single file

                        FileName = FileName & ".dat"

                        For Each Surface In Model.Objects
                            Surface.ExportNativeFile(FileName, True)
                        Next

                    Else

                        ' Write each model surface in a different file

                        Dim I As Integer = 0

                        For Each Surface In Model.Objects

                            Dim Name As String = Surface.Name

                            If Name = "" Then
                                Name = "Surface_" & I
                            End If

                            Surface.ExportNativeFile(FileName & "_" & Name & ".dat", False)

                        Next

                    End If

            End Select

        End Sub

#End Region

    End Module

End Namespace







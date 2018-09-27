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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports System.Xml
Imports OpenVOGEL.AeroTools.CalculationModel.Perturbations
Imports OpenVOGEL.AeroTools.IoHelper

Namespace CalculationModel.Settings

    Public Enum Sence As Integer
        Positive = 1
        Negative = -1
    End Enum

    Public Enum MatrixSolverType As Byte
        LU = 0
        QR = 1
    End Enum

    Public Enum AdjacentRing As Byte
        Panel1 = 0
        Panel2 = 1
        Panel3 = 2
        Panel4 = 3
    End Enum

    Public Enum CalculationType As Byte
        ctSteady = 0
        ctUnsteady = 1
        ctAeroelastic = 2
    End Enum

    Public Class SimulationSettings

        ''' <summary>
        ''' Free stream velocity amplitude vector (components are scaled by an amplitude factor in unsteady problems).
        ''' </summary>
        ''' <remarks></remarks>
        Public Property StreamVelocity As New EVector3

        ''' <summary>
        ''' Angular velocity of aircraft reference frame (not implemented yet).
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Omega As New EVector3

        ''' <summary>
        ''' Free stream density in kg/m³
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Density As Double = 1.225#

        ''' <summary>
        ''' Free stream static pressure in Pa.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property StaticPressure As Double = 101300.0#

        ''' <summary>
        ''' Free stream viscosity
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Viscocity As Double = 0.0000178#

        Private _ClippingStep As Integer = 1

        ''' <summary>
        ''' Specifies the global cutting step
        ''' </summary>
        Public Property ClippingStep As Integer
            Set(ByVal value As Integer)
                If value > 0 Then _ClippingStep = value
            End Set
            Get
                Return _ClippingStep
            End Get
        End Property

        Private _SimulationSteps As Integer = 1

        ''' <summary>
        ''' Specifies the number of integration steps
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SimulationSteps As Integer
            Set(ByVal value As Integer)
                If value > 0 Then _SimulationSteps = value
            End Set
            Get
                Return _SimulationSteps
            End Get
        End Property

        Private _Interval As Double = 0.1

        ''' <summary>
        ''' Specifies the size of the instegration step
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Interval As Double
            Set(ByVal value As Double)
                If value > 0 Then _Interval = value
            End Set
            Get
                Return _Interval
            End Get
        End Property

        Private _Cutoff As Double = 0.0001
        ''' <summary>
        ''' Specifies the radius of the region around vortices where the velocity is null
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Cutoff As Double
            Set(ByVal value As Double)
                If value > 0 Then _Cutoff = value
            End Set
            Get
                Return _Cutoff
            End Get
        End Property

        ''' <summary>
        ''' Specifies whether the cutoff has to be automatically estimated
        ''' </summary>
        ''' <remarks></remarks>
        Public Property CalculateCutoff As Boolean

        ''' <summary>
        ''' 'Contains structural settings
        ''' </summary>
        ''' <remarks></remarks>
        Public Property StructuralSettings As New Models.Structural.Library.StructuralSettings

        ''' <summary>
        ''' Amplitude of free stream velocity components in time.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property UnsteadyVelocity As New UnsteadyVelocity

        ''' <summary>
        ''' Contains information about how the simulation parameters vary during an aeroelastic analysis.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property AeroelasticHistogram As IAeroelasticHistogram

        ''' <summary>
        ''' Parameter used to parametrizise force and moment coeficients.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property CharacteristicLenght As Double = 1

        'Public Property SolverType As SolverType = SolverType.stParallelNET

        Public Property AnalysisType As CalculationType = CalculationType.ctSteady

        ''' <summary>
        ''' Global model position
        ''' </summary>
        ''' <remarks></remarks>
        Public Property GlobalPosition As New EVector3

        ''' <summary>
        ''' Global center of rotation
        ''' </summary>
        ''' <remarks></remarks>
        Public Property GlobalRotationCenter As New EVector3

        ''' <summary>
        ''' Global orientation
        ''' </summary>
        ''' <remarks></remarks>
        Public Property GlobalOrientation As New EulerAngles

        ''' <summary>
        ''' Indicates if adjacent rings should be searched by proximity and not by indexation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property GlobalPanelSurvey As Boolean = True

        Private _SurveyTolerance As Double = 0.001

        ''' <summary>
        ''' Maximum distance between rings to be considered as adjacent.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SurveyTolerance As Double
            Set(ByVal value As Double)
                If value > 0 Then _SurveyTolerance = value
            End Set
            Get
                Return _SurveyTolerance
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the influence of the wake in the fuselage should be included.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StrongWakeInfluence As Boolean = False

        ''' <summary>
        ''' Sets default values.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub InitializaParameters()

            StreamVelocity = New EVector3
            StreamVelocity.X = 10.0
            StreamVelocity.Y = 0
            StreamVelocity.Z = 0

            Omega = New EVector3
            Omega.X = 0
            Omega.Y = 0
            Omega.Z = 0

            Density = 1.225
            Viscocity = 0.0000178

            SimulationSteps = 25
            ClippingStep = 25
            StructuralSettings.StructuralLinkingStep = 5
            StructuralSettings.ModalDamping = 0.05

            Interval = 0.01
            Cutoff = 0.0001
            CalculateCutoff = False

            GlobalPanelSurvey = True

        End Sub

        ''' <summary>
        ''' Copies the object content into another one.
        ''' </summary>
        ''' <param name="Simulacion"></param>
        ''' <remarks></remarks>
        Public Sub Assign(ByVal Simulacion As SimulationSettings)

            AnalysisType = Simulacion.AnalysisType
            Cutoff = Simulacion.Cutoff
            StreamVelocity.Assign(Simulacion.StreamVelocity)
            Omega.Assign(Simulacion.Omega)
            SimulationSteps = Simulacion.SimulationSteps
            ClippingStep = Simulacion.ClippingStep
            Interval = Simulacion.Interval
            Viscocity = Simulacion.Viscocity
            Density = Simulacion.Density
            StaticPressure = Simulacion.StaticPressure
            SurveyTolerance = Simulacion.SurveyTolerance
            GlobalPanelSurvey = Simulacion.GlobalPanelSurvey
            GlobalPosition.Assign(Simulacion.GlobalPosition)
            GlobalRotationCenter = Simulacion.GlobalRotationCenter
            UnsteadyVelocity.Assign(Simulacion.UnsteadyVelocity)
            StructuralSettings.Assign(Simulacion.StructuralSettings)

            If Not IsNothing(Simulacion.AeroelasticHistogram) Then
                AeroelasticHistogram = Simulacion.AeroelasticHistogram.Clone
            Else
                AeroelasticHistogram = Nothing
            End If

        End Sub

        Public ReadOnly Property DynamicPressure As Double
            Get
                Return 0.5 * Me.StreamVelocity.SquareEuclideanNorm * Me.Density
            End Get
        End Property

        Public ReadOnly Property UnitReynoldsNumber
            Get
                Return Me.StreamVelocity.EuclideanNorm * Me.Density / Me.Viscocity
            End Get
        End Property

        ''' <summary>
        ''' Generates a vector containing the velocity at each time step.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub GenerateVelocityProfile()

            Select Case AnalysisType

                Case Settings.CalculationType.ctUnsteady
                    UnsteadyVelocity.BaseVelocity.Assign(StreamVelocity)
                    UnsteadyVelocity.GeneratePerturbation(SimulationSteps)

                Case Settings.CalculationType.ctAeroelastic
                    If Not IsNothing(AeroelasticHistogram) Then
                        AeroelasticHistogram.Generate(StreamVelocity, Interval, SimulationSteps)
                    End If

            End Select

        End Sub

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("StreamVelocity")
            writer.WriteAttributeString("X", String.Format("{0}", StreamVelocity.X))
            writer.WriteAttributeString("Y", String.Format("{0}", StreamVelocity.Y))
            writer.WriteAttributeString("Z", String.Format("{0}", StreamVelocity.Z))
            writer.WriteEndElement()

            writer.WriteStartElement("StreamOmega")
            writer.WriteAttributeString("X", String.Format("{0}", Omega.X))
            writer.WriteAttributeString("Y", String.Format("{0}", Omega.Y))
            writer.WriteAttributeString("Z", String.Format("{0}", Omega.Z))
            writer.WriteEndElement()

            writer.WriteStartElement("Parameters")
            writer.WriteAttributeString("Interval", String.Format("{0}", Interval))
            writer.WriteAttributeString("Steps", String.Format("{0}", SimulationSteps))
            writer.WriteAttributeString("MaxSteps", String.Format("{0}", ClippingStep))
            writer.WriteAttributeString("Cutoff", String.Format("{0}", Cutoff))
            writer.WriteAttributeString("GlobalSurvey", String.Format("{0}", GlobalPanelSurvey))
            writer.WriteAttributeString("SurveyTolerance", String.Format("{0}", SurveyTolerance))
            writer.WriteEndElement()

            writer.WriteStartElement("Fluid")
            writer.WriteAttributeString("Density", String.Format("{0}", Density))
            writer.WriteAttributeString("Viscocity", String.Format("{0}", Viscocity))
            writer.WriteAttributeString("Po", String.Format("{0}", StaticPressure))
            writer.WriteEndElement()

            writer.WriteStartElement("Structure")
            writer.WriteAttributeString("StructureStartStep", String.Format("{0}", StructuralSettings.StructuralLinkingStep))
            writer.WriteAttributeString("Modes", String.Format("{0}", StructuralSettings.NumberOfModes))
            writer.WriteAttributeString("ModalDamping", String.Format("{0}", StructuralSettings.ModalDamping))
            writer.WriteAttributeString("SubSteps", String.Format("{0}", StructuralSettings.SubSteps))
            writer.WriteEndElement()

            If Not IsNothing(UnsteadyVelocity) Then
                writer.WriteStartElement("VelocityProfile")
                UnsteadyVelocity.SaveToXML(writer)
                writer.WriteEndElement()
            End If

            If Not IsNothing(AeroelasticHistogram) Then
                writer.WriteStartElement("AeroelasticHistogram")
                writer.WriteAttributeString("Type", String.Format("{0}", AeroelasticHistogram.Type))
                AeroelasticHistogram.SaveToXML(writer)
                writer.WriteEndElement()
            End If

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "StreamVelocity"
                            StreamVelocity.X = IOXML.ReadDouble(reader, "X", 1.0)
                            StreamVelocity.Y = IOXML.ReadDouble(reader, "Y", 0.0)
                            StreamVelocity.Z = IOXML.ReadDouble(reader, "Z", 0.0)

                        Case "StreamOmega"
                            Omega.X = IOXML.ReadDouble(reader, "X", 0.0)
                            Omega.Y = IOXML.ReadDouble(reader, "Y", 0.0)
                            Omega.Z = IOXML.ReadDouble(reader, "Z", 0.0)

                        Case "Parameters"
                            Interval = IOXML.ReadDouble(reader, "Interval", 0.1)
                            SimulationSteps = IOXML.ReadInteger(reader, "Steps", 15)
                            ClippingStep = IOXML.ReadInteger(reader, "MaxSteps", 15)
                            Cutoff = IOXML.ReadDouble(reader, "Cutoff", 0.0001)
                            GlobalPanelSurvey = IOXML.ReadBoolean(reader, "GlobalSurvey", False)
                            SurveyTolerance = IOXML.ReadDouble(reader, "SurveyTolerance", 0.001)

                        Case "Fluid"
                            Density = IOXML.ReadDouble(reader, "Density", 1.225)
                            Viscocity = IOXML.ReadDouble(reader, "Viscocity", 0.0000178)
                            StaticPressure = IOXML.ReadDouble(reader, "Po", 101300)

                        Case "Structure"
                            StructuralSettings.NumberOfModes = IOXML.ReadInteger(reader, "Modes", 6)
                            StructuralSettings.StructuralLinkingStep = IOXML.ReadInteger(reader, "StructureStartStep", 10)
                            StructuralSettings.ModalDamping = IOXML.ReadDouble(reader, "ModalDamping", 0.05)
                            StructuralSettings.SubSteps = IOXML.ReadInteger(reader, "SubSteps", 1)

                        Case "VelocityProfile"
                            UnsteadyVelocity.ReadFromXML(reader.ReadSubtree)

                        Case "AeroelasticHistogram"
                            Dim type As HistogramType = IOXML.ReadInteger(reader, "Type", HistogramType.FlutterTest)
                            AeroelasticHistogram = HistogramTools.GetInstance(type)
                            AeroelasticHistogram.ReadFromXML(reader.ReadSubtree)

                    End Select

                End If

            End While

        End Sub

    End Class

End Namespace
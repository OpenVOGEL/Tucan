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
Imports OpenVOGEL.AeroTools.IoHelper
Imports System.Xml

Namespace CalculationModel.Perturbations

    Public Class Perturbation

        Private _Start As Integer
        Private _ElapsedTime As Integer
        Private _PeakInstant As Integer

        Public Property Intensity As Double
        Public Property FinalDrop As Double

        Public Sub New()
            _Start = 1
            _PeakInstant = 2
            _ElapsedTime = 3
        End Sub

        Public Property Start As Integer
            Set(ByVal value As Integer)
                _Start = value
                If _PeakInstant <= _Start Then
                    _PeakInstant = _Start + 1
                End If
                If _Start + _ElapsedTime <= _PeakInstant Then
                    _ElapsedTime = _PeakInstant - _Start + 1
                End If
            End Set
            Get
                Return _Start
            End Get
        End Property

        Public Property ElapsedTime As Integer
            Set(ByVal value As Integer)
                _ElapsedTime = value
                If _Start + _ElapsedTime <= _PeakInstant Then
                    _ElapsedTime = _PeakInstant - _Start + 1
                End If
            End Set
            Get
                Return _ElapsedTime
            End Get
        End Property

        Public Property PeakInstant As Integer
            Set(ByVal value As Integer)
                If value > _Start Then
                    _PeakInstant = value
                Else
                    _PeakInstant = _Start + 1
                End If
                If _Start + _ElapsedTime <= _PeakInstant Then
                    _ElapsedTime = _PeakInstant - _Start + 1
                End If
            End Set
            Get
                Return _PeakInstant
            End Get
        End Property

        Public Sub Assign(ByRef Perturbacion As Perturbation)

            _Start = Perturbacion.Start
            _PeakInstant = Perturbacion.PeakInstant
            _ElapsedTime = Perturbacion.ElapsedTime
            Intensity = Perturbacion.Intensity
            FinalDrop = Perturbacion.FinalDrop

        End Sub

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Time")
            writer.WriteAttributeString("Start", String.Format("{0}", _Start))
            writer.WriteAttributeString("TimeSpan", String.Format("{0}", _ElapsedTime))
            writer.WriteAttributeString("PeakAt", String.Format("{0}", _PeakInstant))
            writer.WriteEndElement()

            writer.WriteStartElement("Amplitude")
            writer.WriteAttributeString("Intensity", String.Format("{0}", Intensity))
            writer.WriteAttributeString("FinalDrop", String.Format("{0}", FinalDrop))
            writer.WriteEndElement()

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "Time"
                            _Start = CInt(reader.GetAttribute("Start"))
                            _ElapsedTime = CInt(reader.GetAttribute("TimeSpan"))
                            _PeakInstant = CInt(reader.GetAttribute("PeakAt"))

                        Case "Amplitude"
                            Intensity = CDbl(reader.GetAttribute("Intensity"))
                            FinalDrop = CDbl(reader.GetAttribute("FinalDrop"))

                    End Select

                End If

            End While

        End Sub

    End Class

    ''' <summary>
    ''' Different axes for the perturbation
    ''' </summary>
    Public Enum Axes As Integer
        X = 0
        Y = 1
        Z = 2
    End Enum

    ''' <summary>
    ''' Different types of velocity profiles
    ''' </summary>
    Public Enum ProfileType As Integer
        Impulsive = 0
        Perturbation = 1
    End Enum

    Public Class UnsteadyVelocity

        ''' <summary>
        ''' Base velocity.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BaseVelocity As New EVector3(0, 0, 0)

        Private _Velocity As New List(Of EVector3)
        Private _Intensity As New List(Of EVector3)

        ''' <summary>
        ''' Profile type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Type As ProfileType = ProfileType.Impulsive

        Private _Perturbation(2) As Perturbation ' Representa una perturbación en cada eje de coordenadas.

        Public Sub New()
            _Perturbation(Axes.X) = New Perturbation
            _Perturbation(Axes.Y) = New Perturbation
            _Perturbation(Axes.Z) = New Perturbation
        End Sub

        ''' <summary>
        ''' Gets the velocity at a given time step
        ''' </summary>
        ''' <param name="Instant">Time stape (1-based)</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Velocity(ByVal Instant As Integer) As EVector3
            Get
                Return _Velocity(Instant - 1)
            End Get
        End Property

        ''' <summary>
        ''' Gets the intensity at a given time step
        ''' </summary>
        ''' <param name="Instant">Time stape (1-based)</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Intensity(ByVal Instant As Integer) As EVector3
            Get
                Return _Intensity(Instant - 1)
            End Get
        End Property

        Public Property Perturbation(ByVal Eje As Axes) As Perturbation
            Set(ByVal value As Perturbation)
                _Perturbation(Eje) = value
            End Set
            Get
                Return _Perturbation(Eje)
            End Get
        End Property

        ''' <summary>
        ''' Number of steps.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property nSteps As Integer
            Get
                Return _Velocity.Count
            End Get
        End Property

        Public Sub GeneratePerturbation(ByVal NumberOfSteps As Integer)

            _Velocity.Clear()
            _Intensity.Clear()

            If (Type = ProfileType.Perturbation) Then

                _Velocity.Capacity = NumberOfSteps - 1 ' El cero cuenta como primer indice
                _Intensity.Capacity = NumberOfSteps - 1

                Dim InstanteFinalX As Integer = Perturbation(Axes.X).Start + Perturbation(Axes.X).ElapsedTime
                Dim InstanteFinalY As Integer = Perturbation(Axes.Y).Start + Perturbation(Axes.Y).ElapsedTime
                Dim InstanteFinalZ As Integer = Perturbation(Axes.Z).Start + Perturbation(Axes.Z).ElapsedTime
                Dim Tita As Double
                Dim Parametro As Double
                Dim LIntensity As Double
                Dim LVelocity As Double = BaseVelocity.EuclideanNorm

                For i = 0 To NumberOfSteps - 1

                    _Velocity.Add(New EVector3)
                    _Intensity.Add(New EVector3)

                    LIntensity = 0
                    If i >= Perturbation(Axes.X).Start And i <= Perturbation(Axes.X).PeakInstant Then
                        Parametro = (i - Perturbation(Axes.X).Start) / (Perturbation(Axes.X).PeakInstant - Perturbation(Axes.X).Start)
                        Tita = Math.PI * (Parametro - 0.5)
                        LIntensity = Perturbation(Axes.X).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    ElseIf i > Perturbation(Axes.X).PeakInstant And i <= InstanteFinalX Then
                        Parametro = (i - Perturbation(Axes.X).PeakInstant) / (InstanteFinalX - Perturbation(Axes.X).PeakInstant)
                        Tita = Math.PI * (0.5 - Parametro)
                        LIntensity = Perturbation(Axes.X).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    End If

                    _Velocity(i).X = BaseVelocity.X + LVelocity * LIntensity
                    _Intensity(i).X = LIntensity

                    LIntensity = 0
                    If i >= Perturbation(Axes.Y).Start And i <= Perturbation(Axes.Y).PeakInstant Then
                        Parametro = (i - Perturbation(Axes.Y).Start) / (Perturbation(Axes.Y).PeakInstant - Perturbation(Axes.Y).Start)
                        Tita = Math.PI * (Parametro - 0.5)
                        LIntensity = Perturbation(Axes.Y).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    ElseIf i > Perturbation(Axes.Y).PeakInstant And i <= InstanteFinalY Then
                        Parametro = (i - Perturbation(Axes.Y).PeakInstant) / (InstanteFinalY - Perturbation(Axes.Y).PeakInstant)
                        Tita = Math.PI * (0.5 - Parametro)
                        LIntensity = Perturbation(Axes.Y).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    End If

                    _Velocity(i).Y = BaseVelocity.Y + LVelocity * LIntensity
                    _Intensity(i).Y = LIntensity

                    LIntensity = 0
                    If i >= Perturbation(Axes.Z).Start And i <= Perturbation(Axes.Z).PeakInstant Then
                        Parametro = (i - Perturbation(Axes.Z).Start) / (Perturbation(Axes.Z).PeakInstant - Perturbation(Axes.Z).Start)
                        Tita = Math.PI * (Parametro - 0.5)
                        LIntensity = Perturbation(Axes.Z).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    ElseIf i > Perturbation(Axes.Z).PeakInstant And i <= InstanteFinalZ Then
                        Parametro = (i - Perturbation(Axes.Z).PeakInstant) / (InstanteFinalZ - Perturbation(Axes.Z).PeakInstant)
                        Tita = Math.PI * (0.5 - Parametro)
                        LIntensity = Perturbation(Axes.Z).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    End If

                    _Velocity(i).Z = BaseVelocity.Z + LVelocity * LIntensity
                    _Intensity(i).Z = LIntensity

                Next

            End If

            If (Type = ProfileType.Impulsive) Then

                _Velocity.Capacity = NumberOfSteps - 1
                _Intensity.Capacity = NumberOfSteps - 1

                For i = 0 To NumberOfSteps - 1
                    _Velocity.Add(New EVector3(BaseVelocity))
                    _Intensity.Add(New EVector3)
                Next

            End If

        End Sub

        Public Sub Assign(ByRef Profile As UnsteadyVelocity)

            Type = Profile.Type
            BaseVelocity.Assign(Profile.BaseVelocity)
            Perturbation(Axes.X).Assign(Profile.Perturbation(Axes.X))
            Perturbation(Axes.Y).Assign(Profile.Perturbation(Axes.Y))
            Perturbation(Axes.Z).Assign(Profile.Perturbation(Axes.Z))

        End Sub

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Properties")
            writer.WriteAttributeString("Type", CInt(Type))
            writer.WriteEndElement()

            writer.WriteStartElement("X")
            Perturbation(Axes.X).SaveToXML(writer)
            writer.WriteEndElement()

            writer.WriteStartElement("Y")
            Perturbation(Axes.Y).SaveToXML(writer)
            writer.WriteEndElement()

            writer.WriteStartElement("Z")
            Perturbation(Axes.Z).SaveToXML(writer)
            writer.WriteEndElement()

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "X"
                            Perturbation(Axes.X).ReadFromXML(reader.ReadSubtree)

                        Case "Y"
                            Perturbation(Axes.Y).ReadFromXML(reader.ReadSubtree)

                        Case "Z"
                            Perturbation(Axes.Z).ReadFromXML(reader.ReadSubtree)

                        Case "Properties"
                            Type = IOXML.ReadInteger(reader, "Type", ProfileType.Impulsive)

                    End Select

                End If

            End While

        End Sub

    End Class

End Namespace
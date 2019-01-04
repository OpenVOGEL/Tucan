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
Imports System.IO
Imports System.Xml
Imports OpenVOGEL.AeroTools.IoHelper

Namespace CalculationModel.Models.Aero.Components

    Public Interface IPolarCurve

        Function SkinDrag(ByRef Cl) As Double
        Property Reynolds As Double
        ReadOnly Property Type As PolarType
        Property ID As Guid
        Property Name As String

        Sub ReadBinary(ByRef r As BinaryReader)
        Sub WriteBinary(ByRef w As BinaryWriter)
        Sub WriteToXML(ByRef writer As XmlWriter)
        Sub ReadFromXML(ByRef reader As XmlReader)

        Function Clone() As IPolarCurve

    End Interface

    Public Enum PolarType As Short
        Quadratic = 0
        Custom = 1
    End Enum

    ''' <summary>
    ''' Represents a quadratic polar curve
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QuadraticPolar

        Implements IPolarCurve

        Public Property Cd0 As Double = 0
        Public Property Cd1 As Double = 0
        Public Property Cd2 As Double = 1
        Private _ID As Guid = Guid.NewGuid
        Private _Name As String = "Polar"

        Public Function SkinDrag(ByRef Cl As Object) As Double Implements IPolarCurve.SkinDrag

            Return Cd0 + Cd1 * Cl + Cd2 * Cl * Cl

        End Function

        Private _Reynolds As Double = 0.0#

        Public ReadOnly Property Type As PolarType Implements IPolarCurve.Type
            Get
                Return PolarType.Quadratic
            End Get
        End Property

        Public Property ID As Guid Implements IPolarCurve.ID
            Get
                Return _ID
            End Get
            Set(ByVal value As Guid)
                _ID = value
            End Set
        End Property

        Public Property Name As String Implements IPolarCurve.Name
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public Property Reynolds As Double Implements IPolarCurve.Reynolds
            Set(ByVal value As Double)
                _Reynolds = value
            End Set
            Get
                Return _Reynolds
            End Get
        End Property

        Sub ReadBinary(ByRef r As BinaryReader) Implements IPolarCurve.ReadBinary
            Try
                _ID = New Guid(r.ReadString)
                _Reynolds = r.ReadDouble
                Cd0 = r.ReadDouble
                Cd1 = r.ReadDouble
                Cd2 = r.ReadDouble
            Catch ex As Exception
                Cd0 = 0
                Cd1 = 0
                Cd2 = 0
                _Reynolds = 0
            End Try
        End Sub

        Sub WriteBinary(ByRef w As BinaryWriter) Implements IPolarCurve.WriteBinary
            w.Write(Type)
            w.Write(_ID.ToString)
            w.Write(_Reynolds)
            w.Write(Cd0)
            w.Write(Cd1)
            w.Write(Cd2)
        End Sub

        Public Function Clone() As IPolarCurve Implements IPolarCurve.Clone
            Dim Polar As New QuadraticPolar
            Polar.Cd0 = Cd0
            Polar.Cd1 = Cd1
            Polar.Cd2 = Cd2
            Polar._Reynolds = _Reynolds
            Return Polar
        End Function

#Region " IO "

        Public Sub WriteToXML(ByRef writer As XmlWriter) Implements IPolarCurve.WriteToXML

            writer.WriteAttributeString("CD0", String.Format("{0}", Cd0))
            writer.WriteAttributeString("CD1", String.Format("{0}", Cd1))
            writer.WriteAttributeString("CD2", String.Format("{0}", Cd2))
            writer.WriteAttributeString("Re", String.Format("{0}", Reynolds))
            writer.WriteAttributeString("ID", String.Format("{0}", ID.ToString))
            writer.WriteAttributeString("Name", String.Format("{0}", Name))

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader) Implements IPolarCurve.ReadFromXML

            Cd0 = IOXML.ReadDouble(reader, "CD0", 0.05)
            Cd1 = IOXML.ReadDouble(reader, "CD1", 0.1)
            Cd2 = IOXML.ReadDouble(reader, "CD2", 1.0)
            Reynolds = IOXML.ReadDouble(reader, "Re", 1.0)
            ID = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))
            Name = IOXML.ReadString(reader, "Name", 0.0)

        End Sub

#End Region

    End Class

    Public Class CustomPolar

        Implements IPolarCurve

        Private _ID As Guid = Guid.NewGuid

        Public Property ID As Guid Implements IPolarCurve.ID
            Get
                Return _ID
            End Get
            Set(ByVal value As Guid)
                _ID = value
            End Set
        End Property

        Private _Name As String = "Polar"

        Public Property Name As String Implements IPolarCurve.Name
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property

        Public ReadOnly Property Type As PolarType Implements IPolarCurve.Type
            Get
                Return PolarType.Custom
            End Get
        End Property

        Private _Reynolds As Double = 0.0#

        Public Property Reynolds As Double Implements IPolarCurve.Reynolds
            Set(ByVal value As Double)
                _Reynolds = value
            End Set
            Get
                Return _Reynolds
            End Get
        End Property

        Public Property Nodes As New List(Of EVector2)

        Sub ReadBinary(ByRef r As BinaryReader) Implements IPolarCurve.ReadBinary

            Try

                _ID = New Guid(r.ReadString)
                _Reynolds = r.ReadDouble

                Nodes.Clear()
                Dim n As Integer = r.ReadInt32

                For i = 1 To n
                    Nodes.Add(New EVector2(r.ReadDouble, r.ReadDouble))
                Next

            Catch ex As Exception
                Nodes.Clear()
                _Reynolds = 0
            End Try

        End Sub

        Sub WriteBinary(ByRef w As BinaryWriter) Implements IPolarCurve.WriteBinary

            w.Write(Type)
            w.Write(_ID.ToString)
            w.Write(_Reynolds)

            w.Write(Nodes.Count)

            For Each p In Nodes
                w.Write(p.X)
                w.Write(p.Y)
            Next

        End Sub

        Public Function Clone() As IPolarCurve Implements IPolarCurve.Clone

            Dim Polar As New CustomPolar
            For Each p In Nodes
                Polar.Nodes.Add(New EVector2(p.X, p.Y))
            Next
            Polar._Reynolds = _Reynolds
            Polar.Name = _Name
            Polar.ID = _ID
            Return Polar

        End Function

#Region " IO "

        Public Sub WriteToXML(ByRef writer As XmlWriter) Implements IPolarCurve.WriteToXML

            writer.WriteAttributeString("N", String.Format("{0}", Nodes.Count))

            For i = 0 To Nodes.Count - 1
                writer.WriteAttributeString("X" + i.ToString, String.Format("{0}", Nodes(i).X))
                writer.WriteAttributeString("Y" + i.ToString, String.Format("{0}", Nodes(i).Y))
            Next

            writer.WriteAttributeString("Re", String.Format("{0}", Reynolds))
            writer.WriteAttributeString("ID", String.Format("{0}", ID.ToString))
            writer.WriteAttributeString("Name", String.Format("{0}", Name))

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader) Implements IPolarCurve.ReadFromXML

            Dim n As Integer = IOXML.ReadInteger(reader, "N", 0)

            For i = 0 To n - 1
                Nodes.Add(New EVector2)
                Nodes(i).X = IOXML.ReadDouble(reader, "X" + i.ToString, 0.0)
                Nodes(i).Y = IOXML.ReadDouble(reader, "Y" + i.ToString, 0.0)
            Next

            Reynolds = IOXML.ReadDouble(reader, "Re", 1.0)
            ID = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))
            Name = IOXML.ReadString(reader, "Name", 0.0)

        End Sub

#End Region

        Public Function SkinDrag(ByRef Cl As Object) As Double Implements IPolarCurve.SkinDrag

            Dim CDi As Double = 0.0#

            For i = 0 To Nodes.Count - 2

                If Cl > Nodes(i).X And Cl <= Nodes(i + 1).X Then

                    CDi = Nodes(i).Y + (Nodes(i + 1).Y - Nodes(i).Y) * (Cl - Nodes(i).X) / (Nodes(i + 1).X - Nodes(i).X)

                    Exit For

                End If

            Next

            Return CDi

        End Function

    End Class

    Public Class PolarFamily

        Public Property Polars As New List(Of IPolarCurve)

        Public Property Name As String = "Polar family"

        Public Property ID As Guid = Guid.NewGuid

        Public Sub SortPolars()

            Polars.Sort(Function(x As IPolarCurve, y As IPolarCurve)
                            If x Is Nothing AndAlso y Is Nothing Then
                                Return 0
                            ElseIf x Is Nothing Then
                                Return 1
                            ElseIf y Is Nothing Then
                                Return -1
                            Else
                                If x.Reynolds > y.Reynolds Then
                                    Return 1
                                Else
                                    Return -1
                                End If
                            End If
                        End Function)

        End Sub

        Public Sub WriteToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Info")
            writer.WriteAttributeString("NumberOfPolars", String.Format("{0}", Polars.Count))
            writer.WriteAttributeString("Name", Name)
            writer.WriteAttributeString("ID", ID.ToString)
            writer.WriteEndElement()

            Dim i As Integer = 0

            For Each polar As IPolarCurve In Polars

                If TypeOf polar Is QuadraticPolar Then
                    writer.WriteStartElement("QuadraticPolar")
                    polar.WriteToXML(writer)
                    writer.WriteEndElement()
                    i += 1
                End If

            Next

            For Each polar As IPolarCurve In Polars

                If TypeOf polar Is CustomPolar Then
                    writer.WriteStartElement("CustomPolar")
                    polar.WriteToXML(writer)
                    writer.WriteEndElement()
                    i += 1
                End If

            Next

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            reader.Read()

            Polars.Clear()

            While reader.Read()
                Select Case reader.Name
                    Case "Info"
                        Name = IOXML.ReadString(reader, "Name", "Polar family")
                        ID = New Guid(IOXML.ReadString(reader, "ID", Guid.NewGuid.ToString))
                    Case "QuadraticPolar"
                        Dim polar As New QuadraticPolar
                        polar.ReadFromXML(reader)
                        Polars.Add(polar)
                    Case "CustomPolar"
                        Dim polar As New CustomPolar
                        polar.ReadFromXML(reader)
                        Polars.Add(polar)
                End Select
            End While

            reader.Close()

        End Sub

        Public Overloads Sub ReadBinary(ByRef r As BinaryReader)
            Try
                ID = New Guid(r.ReadString)
                For i = 1 To r.ReadInt32
                    Dim type As Short = r.ReadInt16
                    Select Case type
                        Case PolarType.Quadratic
                            Dim Polar As New QuadraticPolar()
                            Polar.ReadBinary(r)
                            Polars.Add(Polar)
                        Case PolarType.Custom
                            Dim Polar As New CustomPolar()
                            Polar.ReadBinary(r)
                            Polars.Add(Polar)
                    End Select
                Next
            Catch ex As Exception
                Polars.Clear()
            End Try
        End Sub

        Public Overloads Sub WriteBinary(ByRef w As BinaryWriter)

            w.Write(ID.ToString)

            w.Write(Polars.Count)

            Dim i As Integer = 0
            For Each Polar In Polars
                i += 1
                Polar.WriteBinary(w)
            Next

        End Sub

        Public Function SkinDrag(Cl As Double, Re As Double)

            If Polars.Count = 0 Then
                Return 0.0#
            ElseIf Polars.Count = 1 Then
                Return Polars(0).SkinDrag(Cl)
            Else
                If Polars(0).Reynolds > Re Then
                    Return Polars(0).SkinDrag(Cl)
                ElseIf Polars(Polars.Count - 1).Reynolds < Re Then
                    Return Polars(Polars.Count - 1).SkinDrag(Cl)
                Else
                    For i = 0 To Polars.Count - 2
                        If Re > Polars(i).Reynolds Then
                            Return 0.5 * (Polars(i).SkinDrag(Cl) + Polars(i + 1).SkinDrag(Cl))
                        End If
                    Next
                    Return 0.0#
                End If
            End If

        End Function

    End Class

    Public Class PolarDatabase

        Public Property Families As New List(Of PolarFamily)

        Public Path As String = ""

        Public Overloads Sub ReadBinary(ByRef r As BinaryReader)
            Try
                Dim n As Integer = r.ReadInt32
                For i = 1 To n
                    Dim family As New PolarFamily()
                    family.ReadBinary(r)
                    Families.Add(family)
                Next
            Catch ex As Exception
                Families.Clear()
            End Try
        End Sub

        Public Overloads Sub WriteBinary(ByRef w As BinaryWriter)
            w.Write(Families.Count)
            Dim i As Integer = 0
            For Each family In Families
                i += 1
                family.WriteBinary(w)
            Next
        End Sub

        Public Overloads Sub ReadBinary(ByRef FilePath As String)

            If File.Exists(FilePath) Then
                Families.Clear()
                Path = FilePath
                Dim r As New BinaryReader(New FileStream(FilePath, FileMode.Open))
                ReadBinary(r)
                r.Close()
            End If

        End Sub

        Public Overloads Sub WriteBinary(ByRef FilePath As String)

            Try
                Dim w As New BinaryWriter(New FileStream(FilePath, FileMode.Create))
                WriteBinary(w)
                w.Close()
            Catch
                Families.Clear()
            End Try

        End Sub

        Public Overloads Sub ReadBinary()

            ReadBinary(Path)

        End Sub

        Public Overloads Sub WriteBinary()

            WriteBinary(Path)

        End Sub

        Public Function Clone() As PolarDatabase

            Dim db As New PolarDatabase

            db.Path = Path

            For Each Family In Families

                Dim newFamily As New PolarFamily
                newFamily.Name = Family.Name
                newFamily.ID = Family.ID
                db.Families.Add(newFamily)

                For Each Polar In Family.Polars
                    newFamily.Polars.Add(Polar.Clone)
                Next

            Next

            Return db

        End Function

        Public Sub WriteToXML(ByRef writer As XmlWriter)

            writer.WriteAttributeString("NumberOfFamilies", String.Format("{0}", Families.Count))

            Dim i As Integer = 0
            For Each family As PolarFamily In Families
                writer.WriteStartElement("PolarFamily")
                family.WriteToXML(writer)
                writer.WriteEndElement()
                i += 1
            Next

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            reader.Read()

            Families.Clear()

            Dim index As Integer = 0

            While reader.ReadToFollowing("PolarFamily")
                Dim family As New PolarFamily
                family.ReadFromXML(reader.ReadSubtree)
                Families.Add(family)
                index += 1
            End While

            reader.Close()

        End Sub

        Function GetFamilyFromID(FamilyID As Guid) As PolarFamily
            For Each Family In Families
                If Family.ID.Equals(FamilyID) Then
                    Return Family
                End If
            Next
            Return Nothing
        End Function
    End Class

End Namespace

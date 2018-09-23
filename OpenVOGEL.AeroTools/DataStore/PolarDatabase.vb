
Imports System.IO
Imports System.Xml
Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components

Namespace DataStore

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

        Friend Function GetFamilyFromID(FamilyID As Guid) As PolarFamily
            For Each Family In Families
                If Family.ID.Equals(FamilyID) Then
                    Return Family
                End If
            Next
            Return Nothing
        End Function
    End Class

End Namespace

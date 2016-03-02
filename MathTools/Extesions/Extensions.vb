'Copyright (C) 2016 Guillermo Hazebrouck

Imports DotNumerics.LinearAlgebra
Imports System.Runtime.CompilerServices
Imports System.IO

Namespace Extensions

    Public Module Extensions_IO

        <Extension()>
        Public Sub WriteTXT(ByVal Matrix As Matrix, ByVal Path As String)

            If File.Exists(Path) Then
                File.Delete(Path)
            End If

            Dim _file As System.IO.StreamWriter
            _file = My.Computer.FileSystem.OpenTextFileWriter(Path, False)

            For i = 0 To Matrix.RowCount - 1

                For j = 0 To Matrix.ColumnCount - 1

                    _file.Write(String.Format("{0,12:E12}{1}", Matrix(i, j), vbTab))

                Next

                _file.Write(vbNewLine)

            Next

            _file.Close()

        End Sub

    End Module

End Namespace
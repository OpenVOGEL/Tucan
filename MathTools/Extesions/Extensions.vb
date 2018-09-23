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
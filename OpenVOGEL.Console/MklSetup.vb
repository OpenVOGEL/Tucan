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

''' <summary>
''' Setup module for MKL suit
''' </summary>
Public Module MklSetup

    ''' <summary>
    ''' The original content of the path enviromental varialbe
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly OriginalPath As String = Environment.GetEnvironmentVariable("path")

    ''' <summary>
    ''' The path to the MKL suit (mkl_rt.dll)
    ''' </summary>
    ''' <returns></returns>
    Private MklSuitPath As String = ""

    'C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2020\windows\redist\intel64\mkl
    Private InitPath As String = IO.Path.Combine(IO.Directory.GetCurrentDirectory, "MklPath.dat")

    ''' <summary>
    ''' Sets the MKL library path
    ''' </summary>
    Public Sub ChangePath(Path As String)

        MklSuitPath = Path
        Environment.SetEnvironmentVariable("path", String.Format("{0};{1}", MklSuitPath, OriginalPath))

        Dim FileId As Integer = FreeFile()
        FileOpen(FileId, InitPath, OpenMode.Output)
        PrintLine(FileId, Path)
        FileClose(FileId)

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub Initialize()

        DotNumerics.LinearAlgebra.LinearEquations.UseIntelMathKernel = False

        ' Use the path previously given by the user (if the configuration exists)

        If IO.File.Exists(InitPath) Then
            Dim FileId As Integer = FreeFile()
            FileOpen(FileId, InitPath, OpenMode.Input)
            If Not EOF(FileId) Then
                MklSuitPath = LineInput(FileId)
                Environment.SetEnvironmentVariable("path", String.Format("{0};{1}", MklSuitPath, OriginalPath))
                DotNumerics.LinearAlgebra.LinearEquations.UseIntelMathKernel = True
            End If
            FileClose(FileId)
        End If

    End Sub

End Module

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
    Private MklSuitPath As String = "C:\Program Files (x86)\IntelSWTools\compilers_and_libraries_2020\windows\redist\intel64\mkl"

    ''' <summary>
    ''' Sets the MKL library path
    ''' </summary>
    Public Sub ChangePath(Path As String)

        MklSuitPath = Path
        Environment.SetEnvironmentVariable("path", String.Format("{0};{1}", MklSuitPath, OriginalPath))

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub Initialize()

        DotNumerics.LinearAlgebra.LinearEquations.UseIntelMathKernel = True

        'TODO: read user path from inifile to avoid reentering it every time

        ChangePath(MklSuitPath)

    End Sub

End Module

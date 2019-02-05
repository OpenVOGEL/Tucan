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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Structural.Library.Elements
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace VisualModel.Models.Components.Basics

    ''' <summary>
    ''' Represents an structural partition
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralPartition

        ''' <summary>
        ''' Position of this partition
        ''' </summary>
        ''' <remarks></remarks>
        Public P As Vector3

        Public Basis As EBase3

        ''' <summary>
        ''' Section associated to this partition
        ''' </summary>
        ''' <remarks></remarks>
        Public LocalSection As Section

        Public LocalChord As Double

        Public Sub New(ByVal p As Vector3, ByVal section As Section)

            Me.P = New Vector3(p.X, p.Y, p.Z)
            LocalSection = New Section
            LocalSection.Assign(section)

        End Sub

        Public Sub New()

            P = New Vector3()
            LocalSection = New Section
            Basis = New EBase3()

        End Sub

    End Class

End Namespace


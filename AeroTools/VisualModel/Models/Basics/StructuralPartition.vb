'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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

Imports MathTools.Algebra.EuclideanSpace

Namespace VisualModel.Models.Basics

    ''' <summary>
    ''' Represents an structural partition
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralPartition

        ''' <summary>
        ''' Position of this partition
        ''' </summary>
        ''' <remarks></remarks>
        Public p As EVector3

        Public Basis As EBase3

        ''' <summary>
        ''' Section associated to this partition
        ''' </summary>
        ''' <remarks></remarks>
        Public LocalSection As UVLM.Models.Structural.Library.Section

        Public LocalChord As Double

        Public Sub New(ByVal p As EVector3, ByVal section As UVLM.Models.Structural.Library.Section)

            Me.p = New EVector3(p.X, p.Y, p.Z)
            LocalSection = New UVLM.Models.Structural.Library.Section
            LocalSection.Assign(section)

        End Sub

        Public Sub New()

            p = New EVector3()
            LocalSection = New UVLM.Models.Structural.Library.Section
            Basis = New EBase3()

        End Sub

    End Class

End Namespace


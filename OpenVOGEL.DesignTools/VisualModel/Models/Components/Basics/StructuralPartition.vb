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

        ''' <summary>
        ''' The center of mass
        ''' </summary>
        ''' <remarks></remarks>
        Public M As Vector3

        ''' <summary>
        ''' The local system of coordinates
        ''' </summary>
        Public Basis As Base3

        ''' <summary>
        ''' Section associated to this partition point
        ''' </summary>
        ''' <remarks></remarks>
        Public LocalSection As Section

        ''' <summary>
        ''' Chord associated to this partition
        ''' </summary>
        Public LocalChord As Double

        ''' <summary>
        ''' Creates a new partition with the given position
        ''' </summary>
        ''' <param name="p"></param>
        ''' <param name="section"></param>
        Public Sub New(ByVal p As Vector3, ByVal section As Section)

            Me.P = New Vector3(p.X, p.Y, p.Z)
            Me.M = New Vector3(p.X, p.Y, p.Z)

            LocalSection = New Section
            LocalSection.Assign(section)

        End Sub

        ''' <summary>
        ''' Creates a new partition
        ''' </summary>
        Public Sub New()

            Me.P = New Vector3()
            Me.M = New Vector3()

            LocalSection = New Section
            Basis = New Base3()

        End Sub

    End Class

End Namespace


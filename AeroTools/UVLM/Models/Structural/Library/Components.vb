'## Open VOGEL ##
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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

Namespace UVLM.Models.Structural.Library

    ''' <summary>
    ''' Gathers global section porperties.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Section

        ''' <summary>
        ''' Longitudinal stiffness [N/m]
        ''' </summary>
        ''' <remarks></remarks>
        Public AE As Double = 1

        ''' <summary>
        ''' Torsional rigidity [Nm/rad]
        ''' </summary>
        ''' <remarks></remarks>
        Public GJ As Double = 10

        ''' <summary>
        ''' Flexional rigidity of inertia around local axis y [Nm/rad]
        ''' </summary>
        ''' <remarks></remarks>
        Public EIy As Double = 10

        ''' <summary>
        ''' Flexional rigidity of inertia around local axis z [Nm/rad]
        ''' </summary>
        ''' <remarks></remarks>
        Public EIz As Double = 10

        ''' <summary>
        ''' Torsional moment of inertia [kgm²]
        ''' </summary>
        ''' <remarks></remarks>
        Public rIp As Double = 1.0

        ''' <summary>
        ''' Mass per unit length [kg/m]
        ''' </summary>
        ''' <remarks></remarks>
        Public m As Double = 1.0

        ''' <summary>
        ''' Y coordinate of center of mass [m]
        ''' </summary>
        ''' <remarks></remarks>
        Public CMy As Double

        ''' <summary>
        ''' Z coordinate of center of mass [m]
        ''' </summary>
        ''' <remarks></remarks>
        Public CMz As Double

        Public Sub Assign(ByVal Section As Section)

            AE = Section.AE
            CMy = Section.CMy
            CMz = Section.CMz
            GJ = Section.GJ
            EIy = Section.EIy
            EIz = Section.EIz
            rIp = Section.rIp
            m = Section.m

        End Sub

    End Class

    ''' <summary>
    ''' Represents a material.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Material

        ''' <summary>
        ''' Young module [N/m²]
        ''' </summary>
        ''' <remarks></remarks>
        Public E As Double ' 210GPa

        ''' <summary>
        ''' Transverse module [N/m²]
        ''' </summary>
        ''' <remarks></remarks>
        Public G As Double ' 100GPa

        ''' <summary>
        ''' Poisson coefficient
        ''' </summary>
        ''' <remarks></remarks>
        Public v As Double = 0.3

        ''' <summary>
        ''' Material density [kg/m³]
        ''' </summary>
        ''' <remarks></remarks>
        Public Density As Double = 7800

        Public Sub New()
            E = 69000000000 ' Aluminium
            v = 0.3
            G = 0.5 * E / (1 + v)
        End Sub

        Public Sub Assign(ByVal Material As Material)

            E = Material.E
            G = Material.G
            v = Material.v
            Density = Material.Density

        End Sub

    End Class

    ''' <summary>
    ''' Represents a nodal displacement.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NodalDisplacement

        ''' <summary>
        ''' Contains the nodal displacements (Dx, Dy, Dz, Rx, Ry, Rz)
        ''' </summary>
        ''' <remarks></remarks>
        Public Values(5) As Double

        Public Property Dx As Double
            Set(ByVal value As Double)
                Values(0) = value
            End Set
            Get
                Return Values(0)
            End Get
        End Property

        Public Property Dy As Double
            Set(ByVal value As Double)
                Values(1) = value
            End Set
            Get
                Return Values(1)
            End Get
        End Property

        Public Property Dz As Double
            Set(ByVal value As Double)
                Values(2) = value
            End Set
            Get
                Return Values(2)
            End Get
        End Property

        Public Property Rx As Double
            Set(ByVal value As Double)
                Values(3) = value
            End Set
            Get
                Return Values(3)
            End Get
        End Property

        Public Property Ry As Double
            Set(ByVal value As Double)
                Values(4) = value
            End Set
            Get
                Return Values(4)
            End Get
        End Property

        Public Property Rz As Double
            Set(ByVal value As Double)
                Values(5) = value
            End Set
            Get
                Return Values(5)
            End Get
        End Property

        Public Sub Clear()
            Dx = 0.0#
            Dy = 0.0#
            Dz = 0.0#
            Rx = 0.0#
            Ry = 0.0#
            Rz = 0.0#
        End Sub

        ''' <summary>
        ''' Calculates the virtual work of this displacement with the given load
        ''' </summary>
        ''' <param name="Load">Nodal load</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualWork(ByVal Load As NodalLoad) As Double

            Dim vw As Double = 0.0#
            For i = 0 To 5
                vw += Me.Values(i) * Load.Values(i)
            Next

            Return vw

        End Function

    End Class

    ''' <summary>
    ''' Represents a load on a structural node.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NodalLoad

        ''' <summary>
        ''' Contains the nodal loads (Px, Py, Pz, Tx, Ty, Tz)
        ''' </summary>
        ''' <remarks></remarks>
        Public Values(5) As Double

        Public Property Px As Double
            Set(ByVal value As Double)
                Values(0) = value
            End Set
            Get
                Return Values(0)
            End Get
        End Property

        Public Property Py As Double
            Set(ByVal value As Double)
                Values(1) = value
            End Set
            Get
                Return Values(1)
            End Get
        End Property

        Public Property Pz As Double
            Set(ByVal value As Double)
                Values(2) = value
            End Set
            Get
                Return Values(2)
            End Get
        End Property

        Public Property Tx As Double
            Set(ByVal value As Double)
                Values(3) = value
            End Set
            Get
                Return Values(3)
            End Get
        End Property

        Public Property Ty As Double
            Set(ByVal value As Double)
                Values(4) = value
            End Set
            Get
                Return Values(4)
            End Get
        End Property

        Public Property Tz As Double
            Set(ByVal value As Double)
                Values(5) = value
            End Set
            Get
                Return Values(5)
            End Get
        End Property

        Public Sub Clear()
            Px = 0.0#
            Py = 0.0#
            Pz = 0.0#
            Tx = 0.0#
            Ty = 0.0#
            Tz = 0.0#
        End Sub

        ''' <summary>
        ''' Calculates the virtual work of this load with the given displacement.
        ''' </summary>
        ''' <param name="Displacement">Nodal displacement</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VirtualWork(ByVal Displacement As NodalDisplacement) As Double

            Dim vw As Double = 0.0#
            For i = 0 To 5
                vw += Me.Values(i) * Displacement.Values(i)
            Next

            Return vw

        End Function


    End Class

End Namespace
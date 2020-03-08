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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace CalculationModel.Models.Aero.Components

    Public Enum VortexRingType As Byte

        VR4 = 0
        VR3 = 1

    End Enum

    ''' <summary>
    ''' Represents a vortex ring
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface VortexRing

        ReadOnly Property Type As VortexRingType

        ''' <summary>
        ''' Total velocity at the control point.
        ''' </summary>
        Property VelocityT As Vector3

        ''' <summary>
        ''' This is the velocity induced by the wakes. 
        ''' </summary>
        Property VelocityW As Vector3

        ''' <summary>
        ''' Surface velocity at the control point.
        ''' </summary>
        ''' <remarks></remarks>
        Property VelocityS As Vector3

        ''' <summary>
        ''' Potential induced by wake doublets.
        ''' </summary>
        Property PotentialW As Double

        ''' <summary>
        ''' Local circulation.
        ''' </summary>
        Property G As Double

        ''' <summary>
        ''' First derivative of circulation in time.
        ''' </summary>
        Property DGdt As Double

        ''' <summary>
        ''' Local source intensity.
        ''' </summary>
        Property S As Double

        ''' <summary>
        ''' Local pressure coeficient. When parent lattice "IsSlender" field is true it represents the coeficient of local jump of pressure.
        ''' </summary>
        Property Cp As Double

        ''' <summary>
        ''' Local component of induced drag (only valid for slender rings).
        ''' </summary>
        ''' <remarks></remarks>
        Property Cdi As Double

        ''' <summary>
        ''' Local index: represents the position of this panel on the local storage.
        ''' </summary>
        Property IndexL As Integer

        ''' <summary>
        ''' Global index: represents the index of this panel on the influence matrix and G and RHS vectors.
        ''' </summary>
        Property IndexG As Integer

        ''' <summary>
        ''' Sets or gets the i-th corner node. This property is 1-based.
        ''' </summary>
        Property Node(ByVal Index As Integer) As Node

        ''' <summary>
        ''' Normal vector at the control point.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Normal As Vector3

        ''' <summary>
        ''' Control point used to impose local boundary conditions.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property ControlPoint As Vector3

        ''' <summary>
        ''' Control point used to impose local boundary conditions.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property OuterControlPoint As Vector3

        ''' <summary>
        ''' Ring area.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Area As Double

        ''' <summary>
        ''' Indicates if the sense of the ring is reversed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property Reversed As Boolean

        ''' <summary>
        ''' Calculates all geometric entities associated to the vortex ring.
        ''' </summary>
        ''' <remarks></remarks>
        Sub CalculateGeometricEntities()

        ''' <summary>
        ''' Forces the recalculation of the local coordinates.
        ''' </summary>
        ''' <remarks></remarks>
        Sub RecalculateLocalCoordinates()

        ''' <summary>
        ''' Forces the recalculation of the normal.
        ''' </summary>
        ''' <remarks></remarks>
        Sub RecalculateBasis()

        ''' <summary>
        ''' Returns the influence of the doublet distribution in the velocity.
        ''' If WidthG is true vector is scaled by G.
        ''' </summary>
        Function GetDoubletVelocityInfluence(ByVal Point As Vector3, Optional ByVal CutOff As Double = 0.0001, Optional ByVal WithG As Boolean = True) As Vector3

        ''' <summary>
        ''' Adds the influence of the doublet distribution in the velocity.
        ''' If WidthG is true vector is scaled by G.
        ''' </summary>
        Sub AddDoubletVelocityInfluence(ByRef Vector As Vector3,
                                        ByVal Point As Vector3,
                                        Optional ByVal CutOff As Double = 0.0001,
                                        Optional ByVal WithG As Boolean = True)

        ''' <summary>
        ''' Adds the influence of the source distribution in the velocity.
        ''' </summary>
        Sub AddSourceVelocityInfluence(ByRef Vector As Vector3,
                                       ByVal Point As Vector3,
                                       Optional ByVal WithS As Boolean = True)

        ''' <summary>
        ''' Returns the influence of the doublet distribution in the velocity potential.
        ''' </summary>
        Function GetDoubletPotentialInfluence(ByVal Point As Vector3, Optional ByVal WithG As Boolean = True) As Double

        ''' <summary>
        ''' Returns the influence of the source distribution in the velocity potential.
        ''' </summary>
        Function GetSourcePotentialInfluence(ByVal Point As Vector3, Optional ByVal WithS As Boolean = True) As Double

        ''' <summary>
        ''' Computes the induced velocity at a given point by counting only the streamwise vortices.
        ''' </summary>
        ''' <param name="Point">Point where influence is to be calculated</param>
        ''' <param name="N1">First streamwise segment</param>
        ''' <param name="N2">Second streamwise segment</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function StreamwiseInfluence(ByVal Point As Vector3, ByVal N1 As Integer, ByVal N2 As Integer, Optional ByVal CutOff As Double = 0.0001) As Vector3

        ''' <summary>
        ''' Approaches the local velocity using the local source strength and the circulation of the adjacent panels.
        ''' </summary>
        Sub CalculateLocalVelocity(StreamVelocity As Vector3)

        ''' <summary>
        ''' Calculates the jump of pressure through the vortex ring.
        ''' </summary>
        ''' <param name="VSqr">
        ''' Square of reference velocity Norm2.
        ''' </param>
        Sub CalculateCp(ByVal VSqr As Double)

        ''' <summary>
        ''' Indicates whether the panel is used to convect wake or not. 
        ''' This conditionates the local circulation to meet the Kutta condition.
        ''' </summary>
        Property IsPrimitive As Boolean

        ''' <summary>
        ''' Indicates whether the surface is slender or not. This will conditionate the calculation of the local pressure coeficient.
        ''' </summary>
        ReadOnly Property IsSlender As Boolean

        ''' <summary>
        ''' Provides refference to the local neighbor rings. This field is driven by "FindSurroundingRings" of parent lattice.
        ''' </summary>
        ''' <remarks>
        '''  If there is no ring at the given position "nothing" will be returned.
        ''' </remarks>
        Property SurroundingRing(ByVal side As UInt16, ByVal location As UInt16) As VortexRing

        ''' <summary>
        ''' Sence of the adjacent ring. 1 if same as this ring, -1 if oposite and 0 if there is no assigned ring on that location.
        ''' </summary>
        ''' <remarks></remarks>
        Property SurroundingRingsSence(ByVal side As UInt16, ByVal location As UInt16) As Int16

        ''' <summary>
        ''' Indicates whether this ring has a neighbor ring at the given position or not.
        ''' </summary>
        ''' <param name="side">
        ''' 1-based index indicating the local position of the boundary line.
        ''' </param>
        ReadOnly Property HasNeighborAt(ByVal side As UInt16, ByVal location As UInt16)

        ''' <summary>
        ''' Attaches a neighbour ring at the following location on the given side.
        ''' </summary>
        ''' <param name="side">0-based index indicating the local position of the boundary line.</param>
        ''' <remarks></remarks>
        Sub AttachNeighbourAtSide(ByVal side As Int16, ByRef Ring As VortexRing, ByVal Sence As Int16)

        ''' <summary>
        ''' Indicates the local circulation sence (1 or -1).
        ''' </summary>
        Property CirculationSence As Int16

        ''' <summary>
        ''' Stores the circulation of each vortex segment composing this ring. Driven by "CalculateGammas".
        ''' </summary>
        Property Gamma(ByVal index As Integer) As Double

        ''' <summary>
        ''' Sets surrounding rings to nothing.
        ''' </summary>
        ''' <remarks></remarks>
        Sub InitializeSurroundingRings()

    End Interface

End Namespace
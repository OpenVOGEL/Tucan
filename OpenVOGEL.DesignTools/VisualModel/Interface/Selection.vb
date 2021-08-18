'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Namespace VisualModel.Interface

    ''' <summary>
    ''' The different types of components that can be interacted with.
    ''' </summary>
    Public Enum ComponentTypes As Integer

        etNothing = 0
        etLiftingSurface = 1
        etFuselage = 2
        etResultContainer = 3
        etWake = 4
        etJetEngine = 5
        etPropeller = 6

    End Enum

    ''' <summary>
    ''' The different kind of entities that can be interacted with.
    ''' </summary>
    Public Enum EntityTypes As Integer

        etNothing = 0
        etNode = 1
        etSegment = 2
        etPanel = 3
        etStructuralElement = 4
        etStructuralNode = 5

    End Enum

    ''' <summary>
    ''' Provides information about a selection based in a single integer identifier.
    ''' Note that the selection mechanism is not part of this library.
    ''' </summary>
    Public Structure SelectionInfo

        Private _Id As Integer

        ''' <summary>
        ''' Reads back the code and sets identity credentials.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Id As Integer
            Set(ByVal value As Integer)
                _Id = value
                SetUp()
            End Set
            Get
                Return _Id
            End Get
        End Property

        Private _ComponentType As ComponentTypes

        ''' <summary>
        ''' The component type given as described by the ComponentTypes enum.
        ''' </summary>
        Public ReadOnly Property ComponentType As ComponentTypes
            Get
                Return _ComponentType
            End Get
        End Property

        Private _EntityType As EntityTypes

        ''' <summary>
        ''' The entity type given as described by the EntityTypes enum.
        ''' </summary>
        Public ReadOnly Property EntityType As EntityTypes
            Get
                Return _EntityType
            End Get
        End Property

        Private _ComponentIndex As Integer

        ''' <summary>
        ''' The component index in its original stack.
        ''' </summary>
        Public ReadOnly Property ComponentIndex As Integer
            Get
                Return _ComponentIndex
            End Get
        End Property

        Private _EntityIndex As Integer

        ''' <summary>
        ''' The entity index in its original stack inside the parent component.
        ''' </summary>
        Public ReadOnly Property EntityIndex As Integer
            Get
                Return _EntityIndex
            End Get
        End Property

        ''' <summary>
        ''' Decodes the identifier as a component type, component index, entity type and entity index.
        ''' </summary>
        Private Sub SetUp()

            Dim Name As String = _Id.ToString

            _ComponentType = CInt(Left(Name, 1))

            _ComponentIndex = CInt(Mid(Name, 2, 2))

            _EntityType = Mid(Name, 4, 1)

            _EntityIndex = CInt(Mid(Name, 5, 4))

        End Sub

        ''' <summary>
        ''' The depth of the entity associated to this selection info.
        ''' </summary>
        ''' <returns></returns>
        Public Property EyeDepth As Double

    End Structure

    ''' <summary>
    ''' Gathers a collection of selection infos.
    ''' </summary>
    Public Class Selection

        ''' <summary>
        ''' The entities that have be selected.
        ''' </summary>
        Public SelectionList As New List(Of SelectionInfo)

        ''' <summary>
        ''' The specific entities that can be selected.
        ''' </summary>
        Public EntityToSelect As EntityTypes = EntityTypes.etPanel

        ''' <summary>
        ''' Indicates if multiple entities can be selected at the same time.
        ''' </summary>
        Public MultipleSelection As Boolean = False

        ''' <summary>
        ''' Returns the selection Id for the given component and entity.
        ''' </summary>
        Public Shared Function GetSelectionCode(ByVal ElementType As ComponentTypes, ByVal ElementIndex As Integer, ByVal EntityType As EntityTypes, ByVal EntityIndex As Integer) As Integer
            Return ElementType * 10000000 + ElementIndex * 100000 + EntityType * 10000 + EntityIndex
        End Function

    End Class

    ''' <summary>
    ''' Interface that must be implemented by components that can be selected.
    ''' </summary>
    Public Interface ISelectable

        Property Active As Boolean
        Sub UnselectAll()

    End Interface

End Namespace

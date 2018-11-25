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

Namespace VisualModel.Interface

    Public Enum ComponentTypes As Integer

        etNothing = 0
        etLiftingSurface = 1
        etFuselage = 2
        etResultContainer = 3
        etWake = 4
        etJetEngine = 5

    End Enum

    Public Enum EntityTypes As Integer

        etNothing = 0
        etNode = 1
        etSegment = 2
        etPanel = 3
        etStructuralElement = 4
        etStructuralNode = 5

    End Enum

    Public Structure SelectionRecord

        Private FID As Integer

        ''' <summary>
        ''' Reads back the code and sets identity credentials.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ID As Integer
            Set(ByVal value As Integer)
                FID = value
                SetUp()
            End Set
            Get
                Return FID
            End Get
        End Property

        Private FComponentType As ComponentTypes

        Public ReadOnly Property ComponentType As ComponentTypes
            Get
                Return FComponentType
            End Get
        End Property

        Private FEntityType As EntityTypes

        Public ReadOnly Property EntityType As EntityTypes
            Get
                Return FEntityType
            End Get
        End Property

        Private FComponentIndex As Integer

        Public ReadOnly Property ComponentIndex As Integer
            Get
                Return FComponentIndex
            End Get
        End Property

        Private FEntityIndex As Integer

        Public ReadOnly Property EntityIndex As Integer
            Get
                Return FEntityIndex
            End Get
        End Property

        Private Sub SetUp()

            Dim Name As String = FID.ToString

            FComponentType = CInt(Left(Name, 1))

            FComponentIndex = CInt(Mid(Name, 2, 2))

            FEntityType = Mid(Name, 4, 1)

            FEntityIndex = CInt(Mid(Name, 5, 4))

        End Sub

        Public Property EyeDepth As Double

    End Structure

    Public Class Selection

        Public SelectionList As New List(Of SelectionRecord)
        Public EntityToSelect As EntityTypes = EntityTypes.etPanel
        Public MultipleSelection As Boolean = False

        Public Shared Function GetSelectionCode(ByVal ElementType As ComponentTypes, ByVal ElementIndex As Integer, ByVal EntityType As EntityTypes, ByVal EntityIndex As Integer) As Integer
            Return ElementType * 10000000 + ElementIndex * 100000 + EntityType * 10000 + EntityIndex
        End Function

    End Class

    Public Interface ISelectable

        Property Active As Boolean
        Sub UnselectAll()

    End Interface

End Namespace

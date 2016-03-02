'Copyright (C) 2016 Guillermo Hazebrouck

Namespace VisualModel.Interface

    Public Enum ComponentTypes As Integer

        etNothing = 0
        etLiftingSurface = 1
        etBody = 2
        etCompleteModel = 3
        etWake = 4
        etJetEngine = 5

    End Enum

    Public Enum EntityTypes As Integer

        etNothing = 0
        etNode = 1
        etVortex = 2
        etQuadPanel = 3
        etStructuralElement = 4
        etStructuralNode = 5

    End Enum

    Public Enum SelectionModes As Integer

        smNoSelection = 0
        smSurface = 1
        smNodePicking = 2
        smVortexPicking = 3
        smQuadPicking = 4

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

    End Structure

    Public Class Selection

        Public SelectionList As New List(Of SelectionRecord)
        Public SelectionMode As SelectionModes = SelectionModes.smSurface
        Public MultipleSelection As Boolean = False

        Public Shared Function GetSelectionCode(ByVal ElementType As ComponentTypes, ByVal ElementIndex As Integer, ByVal EntityType As EntityTypes, ByVal EntityIndex As Integer) As Integer
            Return ElementType * 10000000 + ElementIndex * 100000 + EntityType * 10000 + EntityIndex
        End Function

    End Class

    Public Interface ISelectable

        Property Selected As Boolean
        Sub UnselectAll()

    End Interface

End Namespace

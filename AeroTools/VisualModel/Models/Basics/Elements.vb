'Copyright (C) 2016 Guillermo Hazebrouck

Imports MathTools.Algebra.EuclideanSpace
Imports AeroTools.UVLM.Settings
Imports AeroTools.VisualModel.Environment.Colormaping

Namespace VisualModel.Models.Basics

    ''' <summary>
    ''' Represents a panel on the model.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Panel

        Public Property N1 As Integer
        Public Property N2 As Integer
        Public Property N3 As Integer
        Public Property N4 As Integer

        Public Property GlobalIndex As Integer = 0
        Public Property ControlPoint As New EVector3
        Public Property NormalVector As New EVector3
        Public Property Area As Double = 0.0#
        Public Property Reversed As Boolean = False

        Public Property LocalVelocity As New EVector3
        Public Property Circulation As Double = 0.0#
        Public Property Cp As Double = 0.0#

        Public IsPrimitive As Boolean = False
        Public IsSlender As Boolean = True
        Private _IsTriangular As Boolean

        Public ReadOnly Property IsTriangular As Boolean
            Get
                Return _IsTriangular
            End Get
        End Property

        Private _AdjacenPanelsSences(3) As Sence
        Private _AdjacentPanels(3) As Panel

        Private _InducedVelocity As New EVector3

        Public Sub New()

        End Sub

        ''' <summary>
        ''' Generates a quadrilateral element
        ''' </summary>
        ''' <param name="N1"></param>
        ''' <param name="N2"></param>
        ''' <param name="N3"></param>
        ''' <param name="N4"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal N4 As Integer)

            Me.N1 = N1
            Me.N2 = N2
            Me.N3 = N3
            Me.N4 = N4
            _IsTriangular = False

        End Sub

        ''' <summary>
        ''' Generates a triangular element
        ''' </summary>
        ''' <param name="N1"></param>
        ''' <param name="N2"></param>
        ''' <param name="N3"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer)

            Me.N1 = N1
            Me.N2 = N2
            Me.N3 = N3
            Me.N4 = N1
            _IsTriangular = True

        End Sub

        Public Property ObtenerPanelAdyacente(ByVal Index As AdjacentRing) As Panel
            Set(ByVal value As Panel)
                _AdjacentPanels(Index) = value
            End Set
            Get
                If Not IsNothing(_AdjacentPanels(Index)) Then
                    Return _AdjacentPanels(Index)
                Else
                    Return New Panel
                End If
            End Get
        End Property

        Public ReadOnly Property HayPanelAdyacente(ByVal Index As AdjacentRing) As Boolean
            Get
                If IsNothing(_AdjacentPanels(Index)) Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public Property ObtenerSentido(ByVal Index As AdjacentRing) As Sence
            Get
                Return _AdjacenPanelsSences(Index)
            End Get
            Set(ByVal value As Sence)
                _AdjacenPanelsSences(Index) = value
            End Set
        End Property

        Public ReadOnly Property VelocidadInducida As EVector3
            Get
                Return _InducedVelocity
            End Get
        End Property

        Public Sub ReiniciarVelocidadInducida()

            _InducedVelocity = New EVector3

        End Sub

        Public Sub Assign(ByVal Panel As Panel)

            Me.N1 = Panel.N1
            Me.N2 = Panel.N2
            Me.N3 = Panel.N3
            Me.N4 = Panel.N4

            _IsTriangular = Panel.IsTriangular

            Me.GlobalIndex = Panel.GlobalIndex
            Me.ControlPoint = Panel.ControlPoint
            Me.NormalVector = Panel.NormalVector
            Me.Area = Panel.Area
            Me.Reversed = Panel.Reversed

            Me.LocalVelocity.Assign(Panel.LocalVelocity)
            Me.Circulation = Panel.Circulation
            Me.Cp = Panel.Cp

            Me.IsPrimitive = Panel.IsPrimitive
            Me.IsSlender = Panel.IsSlender

            'Me.SentidoDePanelesAdyacentes(4) As Sentido
            'Me.PanelesAdyacentes(4) As TQuadPanel

            Me._InducedVelocity.Assign(Panel.VelocidadInducida)

        End Sub

    End Class

    Public Class NodalPoint

        Public Active As Boolean = False
        Public ReferencePosition As EVector3
        Public Position As EVector3
        Public Displacement As EVector3
        Public Color As ColorSharpGL
        Public Pressure As Double

        Public Sub New()
            Position = New EVector3
        End Sub

        Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
            Position = New EVector3(X, Y, Z)
        End Sub

        Public Sub New(ByRef p As EVector3)
            Position = p
        End Sub

    End Class

    Public Class LatticeSegment

        Public N1 As Integer
        Public N2 As Integer

        Private FPanelAdyacente1 As Panel
        Private FPanelAdyacente2 As Panel

        Public Property PanelAdyacente1 As Panel
            Set(ByVal value As Panel)
                FPanelAdyacente1 = value
            End Set
            Get
                If IsNothing(FPanelAdyacente1) Then
                    Return New Panel
                Else
                    Return FPanelAdyacente1
                End If
            End Get
        End Property

        Public Property PanelAdyacente2 As Panel
            Set(ByVal value As Panel)
                FPanelAdyacente2 = value
            End Set
            Get
                If IsNothing(FPanelAdyacente2) Then
                    Return New Panel
                Else
                    Return FPanelAdyacente2
                End If
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal N1 As Integer, ByVal N2 As Integer)
            Me.N1 = N1
            Me.N2 = N2
        End Sub

    End Class

End Namespace

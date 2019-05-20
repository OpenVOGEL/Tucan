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

Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports OpenVOGEL.AeroTools.IoHelper
Imports System.IO
Imports System.Xml
Imports OpenVOGEL.DesignTools.VisualModel.Interface

Namespace VisualModel.Models

    ''' <summary>
    ''' Stores results for a given time step.
    ''' </summary>
    Public Class ResultModel

        Public Name As String
        Public TimeStep As Integer

        Private _Model As ResultContainer
        Private _Wakes As ResultContainer
        Private _DynamicModes As List(Of ResultContainer)

        Public Property SimulationSettings As New SimulationSettings
        Public TotalArea As Double

        Public Property Loaded As Boolean = False

        Public Property VisualizeModes As Boolean

        Public Property SelectedModeIndex As Integer

        Public ReadOnly Property Model As ResultContainer
            Get
                Return _Model
            End Get
        End Property

        Public ReadOnly Property Wakes As ResultContainer
            Get
                Return _Wakes
            End Get
        End Property

        Public Property DynamicModes As List(Of ResultContainer)
            Get
                Return _DynamicModes
            End Get
            Set(ByVal value As List(Of ResultContainer))
                _DynamicModes = value
            End Set
        End Property

        Public ReadOnly Property SelectedMode As ResultContainer
            Get
                If SelectedModeIndex > -1 And SelectedModeIndex < DynamicModes.Count Then
                    Return _DynamicModes(SelectedModeIndex)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Sub InitializeResults()

            _Model = New ResultContainer
            _Wakes = New ResultContainer
            _DynamicModes = New List(Of ResultContainer)

            _Model.Name = "Full_Model"
            _Model.VisualProperties.ColorMesh = System.Drawing.Color.Maroon
            _Model.VisualProperties.ColorSurface = System.Drawing.Color.Orange
            _Model.VisualProperties.Transparency = 1.0
            _Model.VisualProperties.ShowSurface = True
            _Model.VisualProperties.ShowMesh = False
            _Model.VisualProperties.ShowNodes = False
            _Model.VisualProperties.ThicknessMesh = 0.8
            _Model.VisualProperties.ShowNodes = False
            _Model.ActiveResult = ResultContainer.ResultKinds.PanelPressure

            _Wakes.Name = "All_Wakes"
            _Wakes.VisualProperties.ColorMesh = System.Drawing.Color.Silver
            _Wakes.VisualProperties.ColorSurface = System.Drawing.Color.LightBlue
            _Wakes.VisualProperties.ColorNodes = Drawing.Color.Black
            _Wakes.VisualProperties.Transparency = 1.0
            _Wakes.VisualProperties.ShowSurface = False
            _Wakes.VisualProperties.ShowMesh = False
            _Wakes.VisualProperties.ThicknessMesh = 0.8
            _Wakes.VisualProperties.SizeNodes = 3.0#
            _Wakes.VisualProperties.ShowNodes = True
            _Wakes.ActiveResult = ResultContainer.ResultKinds.None

            _Model.Clear()
            _Wakes.Clear()

        End Sub

        Public Sub Clear()

            _Model.Clear()
            _Wakes.Clear()
            _DynamicModes.Clear()

        End Sub

#Region " Transit simulation "

        ''' <summary>
        ''' Gathers all transit lattices.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TransitLattices As New List(Of ResultContainer)

        Private _TransitStages As Integer = 0

        ''' <summary>
        ''' Indicates the number of loaded stages
        ''' </summary>
        Public ReadOnly Property TransitStages As Integer
            Get
                Return TransitLattices.Count
            End Get
        End Property

        ''' <summary>
        ''' Indicates if the simulation frames have been loaded
        ''' </summary>
        ''' <remarks></remarks>
        Public Property TransitLoaded As Boolean

        Public Sub ClearTransit()
            TransitLattices.Clear()
            _TransitLoaded = False
        End Sub

#End Region

    End Class

End Namespace
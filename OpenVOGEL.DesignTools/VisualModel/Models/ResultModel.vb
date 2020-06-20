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

Imports OpenVOGEL.AeroTools.CalculationModel.Settings
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

Namespace VisualModel.Models

    Public Enum ResultFrameKinds

        Transit = 1
        EndState = 2
        DynamicMode = 3

    End Enum

    ''' <summary>
    ''' Represents the latties at a single transit state
    ''' </summary>
    Public Class ResultFrame

        ''' <summary>
        ''' The model
        ''' </summary>
        Public ReadOnly Property Model As ResultContainer

        ''' <summary>
        ''' The wakes shed from the model
        ''' </summary>
        Public ReadOnly Property Wakes As ResultContainer

        ''' <summary>
        ''' The kind of frame
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FrameKind As ResultFrameKinds

        Public Sub New(Kind As ResultFrameKinds)

            _FrameKind = Kind

            _Model = New ResultContainer
            _Wakes = New ResultContainer

            _Model.Name = "Full_Model"
            _Model.VisualProperties.ColorMesh = System.Drawing.Color.DimGray
            _Model.VisualProperties.ColorSurface = System.Drawing.Color.Orange
            _Model.VisualProperties.Transparency = 1.0
            _Model.VisualProperties.ShowSurface = True
            _Model.VisualProperties.ShowMesh = True
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

    End Class

    ''' <summary>
    ''' Stores results for a given time step.
    ''' </summary>
    Public Class ResultModel

        ''' <summary>
        ''' Name of the model
        ''' </summary>
        Public Name As String

        ''' <summary>
        ''' The active transit
        ''' </summary>
        Public Property ActiveFrame As ResultFrame

        ''' <summary>
        ''' Contains all the transit states
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Frames As List(Of ResultFrame)

        ''' <summary>
        ''' Creates a new empty result model
        ''' </summary>
        Public Sub New()
            _Frames = New List(Of ResultFrame)
        End Sub

        ''' <summary>
        ''' The simulation settings that where used for this results.
        ''' </summary>
        ''' <returns></returns>
        Public Property SimulationSettings As New SimulationSettings

        Public Sub Clear()
            ActiveFrame = Nothing
            _Frames.Clear()
        End Sub

    End Class

End Namespace
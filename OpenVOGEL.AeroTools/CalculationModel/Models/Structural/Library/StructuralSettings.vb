'Open VOGEL (openvogel.org)
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

Namespace CalculationModel.Models.Structural.Library

    Public Class StructuralSettings

        Private _ModalDamping As Double = 0.05
        ''' <summary>
        ''' Damping associated to the structural modes
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ModalDamping As Double
            Set(ByVal value As Double)
                If value > 0 Then _ModalDamping = value
            End Set
            Get
                Return _ModalDamping
            End Get
        End Property

        Private _NumberOfModes As Integer = 6
        ''' <summary>
        ''' Specifies the number of dynamic modes to be used
        ''' </summary>
        ''' <remarks></remarks>
        Public Property NumberOfModes As Integer
            Set(ByVal value As Integer)
                If value > 0 Then _NumberOfModes = value
            End Set
            Get
                Return _NumberOfModes
            End Get
        End Property

        Private _StructuralLinkingStep As Integer = 20
        ''' <summary>
        ''' Specifies when at which instant the structure is coupled to the model.
        ''' (use it when a steady state is required as start condition).
        ''' </summary>
        ''' <remarks></remarks>
        Public Property StructuralLinkingStep As Integer
            Set(ByVal value As Integer)
                If value > 0 Then _StructuralLinkingStep = value
            End Set
            Get
                Return _StructuralLinkingStep
            End Get
        End Property

        ''' <summary>
        ''' Sub steps used to integrate the equations of motion in one aerodynamic step.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SubSteps As Integer = 1

        Public Sub Assign(ByVal ss As StructuralSettings)
            ModalDamping = ss.ModalDamping
            SubSteps = ss.SubSteps
            _NumberOfModes = ss._NumberOfModes
            _StructuralLinkingStep = ss._StructuralLinkingStep
        End Sub

    End Class

End Namespace

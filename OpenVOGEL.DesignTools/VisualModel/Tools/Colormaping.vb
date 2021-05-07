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

Namespace VisualModel.Tools.Colormaping

    ''' <summary>
    ''' Represents a color to be used on SharpGL
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure ColorSharpGL

        Private CR As Double
        Private CG As Double
        Private CB As Double

        Public ReadOnly Property R As Double
            Get
                Return Me.CR
            End Get
        End Property

        Public ReadOnly Property G As Double
            Get
                Return Me.CG
            End Get
        End Property

        Public ReadOnly Property B As Double
            Get
                Return Me.CB
            End Get
        End Property

        Public Sub FromARGB(ByVal Red As Double, ByVal Green As Double, ByVal Blue As Double)
            Me.CR = Red
            Me.CG = Green
            Me.CB = Blue
        End Sub

    End Structure

    ''' <summary>
    ''' Provides methods to draw scalar in colors
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Colormap

        Public Shared Function ScalarToColor2(ByVal Escalar As Double, ByVal Maximo As Double, ByVal Minimo As Double) As ColorSharpGL

            Dim ColorDeEscala As ColorSharpGL
            Dim EscalaNormalizada As Double = Math.Abs(Escalar - Minimo) / Math.Abs(Maximo - Minimo)

            If EscalaNormalizada <= 0.5 Then ColorDeEscala.FromARGB(0, EscalaNormalizada / 0.5, 1 - EscalaNormalizada / 0.5)
            If EscalaNormalizada > 0.5 Then ColorDeEscala.FromARGB(EscalaNormalizada / 0.5 - 1, (1 - EscalaNormalizada) / 0.5, 0)

            Return ColorDeEscala

        End Function

        ''' <summary>
        ''' Converts a scalar into a color provided the red and blue extremes
        ''' </summary>
        ''' <param name="Escalar">Value to convert</param>
        ''' <param name="Maximum">Value to be displayed as red</param>
        ''' <param name="Minimum">Value to be displayed as blue</param>
        ''' <returns>A color representing the given scalar</returns>
        ''' <remarks></remarks>
        Public Shared Function ScalarToColor(ByVal Escalar As Double, ByVal Maximum As Double, ByVal Minimum As Double) As ColorSharpGL

            Dim Color As ColorSharpGL
            Dim L As Double = 120 / 255
            Dim S As Double = 240 / 255
            Dim H As Double = 0
            If Escalar < Minimum Then
                H = 0
            ElseIf Escalar > Maximum Then
                H = 0.6274
            Else
                H = (1 - Math.Abs(Escalar - Minimum) / Math.Abs(Maximum - Minimum)) * 0.6274 ' Between 0 and 160 / 255 to go from red to blue passing through yellow
            End If

            Dim temp2 As Double = GetTemp2(H, S, L)
            Dim temp1 As Double = 2.0 * L - temp2

            Dim R As Double = GetColorComponent(temp1, temp2, H + 1.0 / 3.0)
            Dim G As Double = GetColorComponent(temp1, temp2, H)
            Dim B As Double = GetColorComponent(temp1, temp2, H - 1.0 / 3.0)

            Color.FromARGB(R, G, B)

            Return Color

        End Function

        Private Shared Function GetTemp2(ByVal H As Double, ByVal S As Double, ByVal L As Double) As Double

            Dim temp2 As Double
            If (L < 0.5) Then
                temp2 = L * (1.0 + S)
            Else
                temp2 = L + S - (L * S)
            End If
            Return temp2

        End Function

        Private Shared Function GetColorComponent(ByVal temp1 As Double, ByVal temp2 As Double, ByVal temp3 As Double) As Double

            temp3 = MoveIntoRange(temp3)

            If (temp3 < 1.0 / 6.0) Then
                Return temp1 + (temp2 - temp1) * 6.0 * temp3
            ElseIf (temp3 < 0.5) Then
                Return temp2
            ElseIf (temp3 < 2.0 / 3.0) Then
                Return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0)
            Else
                Return temp1
            End If

        End Function

        Private Shared Function MoveIntoRange(ByVal temp3 As Double) As Double
            If (temp3 < 0.0) Then
                temp3 += 1.0
            ElseIf (temp3 > 1.0) Then
                temp3 -= 1.0
            End If
            Return temp3
        End Function

    End Class

End Namespace
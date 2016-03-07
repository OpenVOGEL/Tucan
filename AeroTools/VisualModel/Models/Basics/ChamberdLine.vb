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

Imports MathTools.Algebra.EuclideanSpace

Namespace VisualModel.Models.Basics

    Public Enum ChamberType
        NACA4 = 1
        NACA5 = 2
        NACA6 = 3
    End Enum

    Public Enum ChamberDim As Byte
        MaxChamber = 0
        PosMaxChamber = 1
    End Enum

    Public Class ChamberedLine

        Private _Dimensions(10) As Double

        ''' <summary>
        ''' Returns a NACA chamber point provided the position as fraction of the chord.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal Type As ChamberType)
            ChamberType = Type
        End Sub

        Public Sub New(ByVal cl As ChamberedLine)
            Me.ChamberType = cl.ChamberType
            Me.FlapChord = cl.FlapChord
            Me.FlapDeflection = cl.FlapDeflection
            Me.Flapped = cl.Flapped
            For i = 0 To _Dimensions.Length - 1
                Me._Dimensions(i) = cl.Dimension(i)
            Next
        End Sub

        Public Property Dimension(ByVal Name As ChamberDim) As Double
            Get
                Return _Dimensions(Name)
            End Get
            Set(ByVal value As Double)
                _Dimensions(Name) = value
            End Set
        End Property

        ''' <summary>
        ''' Returns the Y coordinate of the basic chamber function given a chordwise coordinate.
        ''' </summary>
        ''' <param name="p">Chordwise coordinate</param>
        Public Function BasicLine(ByVal p As Double) As Double
            Select Case ChamberType
                Case Basics.ChamberType.NACA4
                    Return BasicLine_NACA4(p)
                Case Else
                    Return 0
            End Select
        End Function

        Private Function BasicLine_NACA4(ByVal p As Double) As Double

            Dim _y As Single = 0

            If p <= Dimension(ChamberDim.PosMaxChamber) Then
                _y = Dimension(ChamberDim.MaxChamber) * p / Dimension(ChamberDim.PosMaxChamber) ^ 2 * (2 * Dimension(ChamberDim.PosMaxChamber) - p)
            ElseIf p > Dimension(ChamberDim.PosMaxChamber) Then
                _y = Dimension(ChamberDim.MaxChamber) * (1.0 - p) / (1.0 - Dimension(ChamberDim.PosMaxChamber)) ^ 2 * (1 + p - 2 * Dimension(ChamberDim.PosMaxChamber))
            Else
                _y = 0
            End If

            Return _y

        End Function

        Private _ChamberType As ChamberType

        ''' <summary>
        ''' Sets or gets the type of chamber.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChamberType As ChamberType
            Set(ByVal value As ChamberType)
                _ChamberType = value
            End Set
            Get
                Return _ChamberType
            End Get
        End Property

        Private _Flapped As Boolean = False

        Public Property Flapped As Boolean
            Set(ByVal value As Boolean)
                _Flapped = value
            End Set
            Get
                Return _Flapped
            End Get
        End Property

        Private _FlapChord As Single = 0.2

        Public Property FlapChord As Single
            Set(ByVal value As Single)
                _FlapChord = value
            End Set
            Get
                Return _FlapChord
            End Get
        End Property

        Private _FlapDeflection As Single = 0

        Public Property FlapDeflection As Single
            Set(ByVal value As Single)
                _FlapDeflection = value
            End Set
            Get
                Return _FlapDeflection
            End Get
        End Property

        Public Sub EvaluatePoint(ByRef point As EVector2, ByVal p As Double)

            point.X = p
            point.Y = BasicLine(p)

            If Flapped And 1 - p <= FlapChord + 0.01 Then

                Dim y As Double = BasicLine(1 - FlapChord)
                point.Rotate(FlapDeflection, 1 - FlapChord, y)

            End If

        End Sub

        Public Sub Assign(ByVal Chamber As ChamberedLine)
            ChamberType = Chamber.ChamberType
            For i = 0 To Chamber._Dimensions.Length - 1
                _Dimensions(i) = Chamber.Dimension(i)
            Next
            Flapped = Chamber.Flapped
            FlapDeflection = Chamber.FlapDeflection
            FlapChord = Chamber.FlapChord
        End Sub

    End Class

End Namespace


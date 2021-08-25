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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Namespace Algebra.CustomMatrices

    ''' <summary>
    ''' A generic n x m matrix
    ''' </summary>
    Public Structure Matrix

        Public Sub New(ByVal n As Integer, ByVal m As Integer)
            ReDim _Elements(n - 1, m - 1)
            Me._Rows = n
            Me._Columns = m
            _Transponse = False
        End Sub

        Private _Elements(,) As Double

        Public Property Element(ByVal i As Integer, ByVal j As Integer) As Double
            Get
                If Not _Transponse Then
                    Return _Elements(i - 1, j - 1)
                Else
                    Return _Elements(j - 1, i - 1)
                End If
            End Get
            Set(ByVal value As Double)
                If Not _Transponse Then
                    _Elements(i - 1, j - 1) = value
                Else
                    _Elements(j - 1, i - 1) = value
                End If
            End Set
        End Property

        Private _Rows As Integer

        Public ReadOnly Property Rows As Integer
            Get
                If Not _Transponse Then
                    Return _Rows
                Else
                    Return _Columns
                End If
            End Get
        End Property

        Private _Columns As Integer

        Public ReadOnly Property Columns As Integer
            Get
                If Not _Transponse Then
                    Return _Columns
                Else
                    Return _Rows
                End If
            End Get
        End Property

        Private _Transponse As Boolean

        Public Sub Transponse()
            _Transponse = Not _Transponse
        End Sub

        Public Sub Clear()
            For i = 1 To Me.Rows
                For j = 1 To Me.Columns
                    Element(i, j) = 0
                Next
            Next
        End Sub

        Public Sub Assign(ByVal Matrix As Matrix)
            If Matrix.Rows = Me.Rows And Matrix.Columns = Me.Columns Then
                For i = 1 To Me.Rows
                    For j = 1 To Me.Columns
                        Element(i, j) = Matrix.Element(i, j)
                    Next
                Next
            End If
        End Sub

#Region " Matrix operators "

        'Sum:
        Public Shared Operator +(ByVal M1 As Matrix, ByVal M2 As Matrix) As Matrix

            If M1.Rows = M2.Rows And M1.Columns = M2.Columns Then
                Dim Sum As New Matrix(M1.Rows, M1.Columns)

                For i = 1 To Sum.Rows
                    For j = 1 To Sum.Columns

                        Sum.Element(i, j) = M1.Element(i, j) + M2.Element(i, j)

                    Next
                Next

                Return Sum

            Else
                Return New Matrix(0, 0)
            End If

        End Operator

        'Scalar multiplication:
        Public Shared Operator *(ByVal Escalar As Double, ByVal M As Matrix) As Matrix
            Dim Product As New Matrix(M.Rows, M.Columns)

            For i = 1 To Product.Rows
                For j = 1 To Product.Columns

                    M.Element(i, j) = Escalar * M.Element(i, j)

                Next
            Next

            Return Product

        End Operator

        'Matrix multiplication:
        Public Shared Operator ^(ByVal M1 As Matrix, ByVal M2 As Matrix) As Matrix

            If M1.Columns = M2.Rows Then
                Dim Product As New Matrix(M1.Rows, M2.Columns)
                For i = 1 To M1.Rows
                    For j = 1 To M2.Columns

                        For m = 1 To M1.Columns
                            For n = 1 To M2.Rows
                                Product.Element(i, j) = Product.Element(i, j) + M1.Element(i, m) * M2.Element(n, j)
                            Next
                        Next

                    Next
                Next
                Return Product
            Else
                Return New Matrix(0, 0)
            End If

        End Operator

#End Region

    End Structure

    Public Class Matrix3x3

        Private Elements(2, 2) As Double

        ''' <summary>
        ''' Sets and gets the value of an item. Indices are 1-based (from 1 to 3)
        ''' </summary>
        ''' <param name="i">1-based row index</param>
        ''' <param name="j">1-based column index</param>
        ''' <returns></returns>
        Public Property Item(ByVal i As Integer, ByVal j As Integer) As Double
            Get
                If 1 < i < 3 And 1 < i < 3 Then
                    Return Elements(i - 1, j - 1)
                Else
                    Return 0
                End If
            End Get
            Set(ByVal value As Double)
                If 1 < i < 3 And 1 < i < 3 Then
                    Elements(i - 1, j - 1) = value
                End If
            End Set
        End Property

        Public ReadOnly Property Determinant As Double
            Get
                Return Item(1, 1) * (Item(2, 2) * Item(3, 3) - Item(3, 2) * Item(2, 3)) -
                Item(1, 2) * (Item(2, 1) * Item(3, 3) - Item(3, 1) * Item(3, 2)) +
                Item(1, 3) * (Item(2, 1) * Item(3, 2) - Item(3, 1) * Item(2, 2))
            End Get
        End Property

        Public Sub Transpose()
            Dim Matriz As New Matrix3x3
            Matriz.Elements = Me.Elements
            For i = 1 To 3
                For j = 1 To 3
                    Me.Item(i, j) = Matriz.Item(j, i)
                Next
            Next
        End Sub

        Public Sub Invert()

            Dim m As New DotNumerics.LinearAlgebra.Matrix(3)

            m.Item(0, 0) = Item(0, 0)
            m.Item(0, 1) = Item(0, 1)
            m.Item(0, 2) = Item(0, 2)

            m.Item(1, 0) = Item(1, 0)
            m.Item(1, 1) = Item(1, 1)
            m.Item(1, 2) = Item(1, 2)

            m.Item(2, 0) = Item(2, 0)
            m.Item(2, 1) = Item(2, 1)
            m.Item(2, 2) = Item(2, 2)

            Dim i As DotNumerics.LinearAlgebra.Matrix = m.Inverse

            Item(0, 0) = m.Item(0, 0)
            Item(0, 1) = m.Item(0, 1)
            Item(0, 2) = m.Item(0, 2)

            Item(1, 0) = m.Item(1, 0)
            Item(1, 1) = m.Item(1, 1)
            Item(1, 2) = m.Item(1, 2)

            Item(2, 0) = m.Item(2, 0)
            Item(2, 1) = m.Item(2, 1)
            Item(2, 2) = m.Item(2, 2)

        End Sub

    End Class

    Public Class RotationMatrix

        Inherits Matrix3x3

        ''' <summary>
        ''' Generates the rotation matrix from a sequence of rotations
        ''' </summary>
        ''' <param name="Orientation"></param>
        Public Sub Generate(ByVal Angle1 As Double, ByVal Angle2 As Double, ByVal Angle3 As Double)

            Item(1, 1) = Math.Cos(Angle1) * Math.Cos(Angle2)
            Item(1, 2) = Math.Cos(Angle1) * Math.Sin(Angle2) * Math.Sin(Angle3) - Math.Sin(Angle1) * Math.Cos(Angle3)
            Item(1, 3) = Math.Sin(Angle1) * Math.Sin(Angle3) + Math.Cos(Angle1) * Math.Sin(Angle2) * Math.Cos(Angle3)
            Item(2, 1) = Math.Sin(Angle1) * Math.Cos(Angle2)
            Item(2, 2) = Math.Sin(Angle1) * Math.Sin(Angle2) * Math.Sin(Angle3) + Math.Cos(Angle1) * Math.Cos(Angle3)
            Item(2, 3) = Math.Sin(Angle1) * Math.Sin(Angle2) * Math.Cos(Angle3) - Math.Cos(Angle1) * Math.Sin(Angle3)
            Item(3, 1) = -Math.Sin(Angle2)
            Item(3, 2) = Math.Cos(Angle2) * Math.Sin(Angle3)
            Item(3, 3) = Math.Cos(Angle2) * Math.Cos(Angle3)

        End Sub

        ''' <summary>
        ''' Generates the rotation matrix from a sequence of rotations
        ''' </summary>
        ''' <param name="Orientation"></param>
        Public Sub Generate(ByVal Orientation As OrientationAngles)

            Dim c1 As Double = Math.Cos(Orientation.Angle1)
            Dim s1 As Double = Math.Sin(Orientation.Angle1)

            Dim c2 As Double = Math.Cos(Orientation.Angle2)
            Dim s2 As Double = Math.Sin(Orientation.Angle2)

            Dim c3 As Double = Math.Cos(Orientation.Angle3)
            Dim s3 As Double = Math.Sin(Orientation.Angle3)

            Select Case Orientation.Sequence

                Case RotationSequence.ZYX

                    Item(1, 1) = c1 * c2
                    Item(1, 2) = c1 * s2 * s3 - s1 * c3
                    Item(1, 3) = s1 * s3 + c1 * s2 * c3
                    Item(2, 1) = s1 * c2
                    Item(2, 2) = s1 * s2 * s3 + c1 * c3
                    Item(2, 3) = s1 * s2 * c3 - c1 * s3
                    Item(3, 1) = -s2
                    Item(3, 2) = c2 * s3
                    Item(3, 3) = c2 * c3

                Case RotationSequence.XYZ

                    Item(1, 1) = c2 * c3
                    Item(1, 2) = -c2 * s3
                    Item(1, 3) = s2
                    Item(2, 1) = c1 * s3 + c3 * s1 * s2
                    Item(2, 2) = c1 * c3 - s1 * s2 * s3
                    Item(2, 3) = -c2 * s1
                    Item(3, 1) = s1 * s3 - c1 * c3 * s2
                    Item(3, 2) = c3 * s1 + c1 * s2 * s3
                    Item(3, 3) = c1 * c2

            End Select

        End Sub

        ''' <summary>
        ''' Generates the rotation matrix from a quaternion
        ''' </summary>
        ''' <param name="Orientation"></param>
        Public Sub Generate(ByVal Qr As Double, ByVal Qi As Double, ByVal Qj As Double, ByVal Qk As Double)

            Dim S As Double = 1 / (Qr * Qr + Qi * Qi + Qj * Qj + Qk * Qk)

            Item(1, 1) = 1.0# - 2.0# * S * (Qj * Qj + Qk * Qk)
            Item(1, 2) = 2.0# * S * (Qi * Qj - Qk * Qr)
            Item(1, 3) = 2.0# * S * (Qi * Qk + Qj * Qr)
            Item(2, 1) = 2.0# * S * (Qi * Qj + Qk * Qr)
            Item(2, 2) = 1.0# - 2.0# * S * (Qi * Qi + Qk * Qk)
            Item(2, 3) = 2.0# * S * (Qj * Qk - Qi * Qr)
            Item(3, 1) = 2.0# * S * (Qi * Qk - Qj * Qr)
            Item(3, 2) = 2.0# * S * (Qj * Qk + Qi * Qr)
            Item(3, 3) = 1.0# - 2.0# * S * (Qi * Qi + Qj * Qj)

        End Sub

    End Class

End Namespace

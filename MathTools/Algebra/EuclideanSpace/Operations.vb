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

Namespace Algebra.EuclideanSpace

    ''' <summary>
    ''' Provides static methods to operate with vectors and matrices.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Algebra

        Public Overloads Shared Function VectorProduct(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As EVector3
            Dim Producto As New EVector3

            Producto.X = Vector1.Y * Vector2.Z - Vector1.Z * Vector2.Y
            Producto.Y = -Vector1.X * Vector2.Z + Vector2.X * Vector1.Z
            Producto.Z = Vector1.X * Vector2.Y - Vector1.Y * Vector2.X

            Return Producto

        End Function

        Public Overloads Shared Function InnerProduct(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As Double

            Dim Producto As Double

            Producto = Vector1.X * Vector2.X + Vector1.Y * Vector2.Y + Vector1.Z * Vector2.Z

            Return Producto

        End Function

        Public Overloads Shared Function SubstractVectors(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As EVector3

            Dim Resta As New EVector3

            Resta.X = Vector1.X - Vector2.X
            Resta.Y = Vector1.Y - Vector2.Y
            Resta.Z = Vector1.Z - Vector2.Z

            Return Resta

        End Function

        Public Overloads Shared Function AddVectors(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As EVector3

            Dim Resta As New EVector3

            Resta.X = Vector1.X + Vector2.X
            Resta.Y = Vector1.Y + Vector2.Y
            Resta.Z = Vector1.Z + Vector2.Z

            Return Resta

        End Function

        Public Overloads Shared Function ScalarProduct(ByVal Escalar As Double, ByVal Vector As EVector3) As EVector3

            Dim Resultado As New EVector3

            Resultado.X = Escalar * Vector.X
            Resultado.Y = Escalar * Vector.Y
            Resultado.Z = Escalar * Vector.Z

            Return Resultado

        End Function

    End Class

End Namespace


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

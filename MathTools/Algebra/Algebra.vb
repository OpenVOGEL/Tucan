Imports MathTools.Algebra.CustomMatrices
Imports MathTools.Algebra.EuclideanSpace

Namespace Algebra.EuclideanSpace

    Public Class EVector2

        Public Property X As Double
        Public Property Y As Double

        Public Sub New()

        End Sub

        Public Sub New(ByVal x As Double, ByVal y As Double)
            Me.X = x
            Me.Y = y
        End Sub

#Region " Operaciones aritméticos "

        Public Shared Operator +(ByVal V1 As EVector2, ByVal V2 As EVector2) As EVector2
            Dim Suma As New EVector2
            Suma.X = V1.X + V2.X
            Suma.Y = V1.Y + V2.Y
            Return Suma
        End Operator

        Public Shared Operator -(ByVal V1 As EVector2, ByVal V2 As EVector2) As EVector2
            Dim Resta As New EVector2
            Resta.X = V1.X - V2.X
            Resta.Y = V1.Y - V2.Y
            Return Resta
        End Operator

        Public Shared Operator -(ByVal V As EVector2) As EVector2
            Dim Opuesto As New EVector2
            Opuesto.X = -V.X
            Opuesto.Y = -V.Y
            Return Opuesto
        End Operator

        Public Overloads Shared Operator *(ByVal V1 As EVector2, ByVal V2 As EVector2) As Double
            Return V1.X * V2.X + V1.Y * V2.Y
        End Operator

        Public Overloads Shared Operator *(ByVal Escalar As Double, ByVal V As EVector2) As EVector2
            Dim Producto As New EVector2

            Producto.X = Escalar * V.X
            Producto.Y = Escalar * V.Y

            Return Producto
        End Operator

        Public Shared Operator ^(ByVal V1 As EVector2, ByVal V2 As EVector2) As Double

            Return V1.X * V2.Y - V1.Y * V2.X

        End Operator

        Public Sub Oppose()
            Me.X = -Me.X
            Me.Y = -Me.Y
        End Sub

        Public Sub Scale(ByVal Scalar As Double)
            Me.X = Scalar * Me.X
            Me.Y = Scalar * Me.Y
        End Sub

        Public Function PinCrossProduct(ByVal V1 As EVector2, ByVal V2 As EVector2) As Double

            Dim x1 As Double = V1.X - X
            Dim y1 As Double = V1.Y - Y

            Dim x2 As Double = V2.X - X
            Dim y2 As Double = V2.Y - Y

            Return x1 * y2 - y1 * x2

        End Function

#End Region

#Region " Metric operations "

        Public ReadOnly Property Norm2 As Double
            Get
                Return Math.Sqrt(Me * Me)
            End Get
        End Property

        Public ReadOnly Property SqrNorm2 As Double
            Get
                Return X * X + Y * Y
            End Get
        End Property

        Public Function RelativePosition(ByVal Punto As EVector2) As EVector2
            Return New EVector2(Punto.X - X, Punto.Y - Y)
        End Function

        Public Function DistanceTo(ByVal Punto As EVector2) As Double
            Dim dx As Double = X - Punto.X
            Dim dy As Double = Y - Punto.Y
            Return Math.Sqrt(dx * dx + dy * dy)
        End Function

        Public ReadOnly Property ProjectionVector(ByVal Vector As EVector2) As EVector2
            Get
                Return (Me * Vector) / (Vector * Vector) * Vector
            End Get
        End Property

        Public ReadOnly Property OrthogonalVector(ByVal Vector As EVector2) As EVector2
            Get
                Return Me - Me.ProjectionVector(Vector)
            End Get
        End Property

        Public Sub Ortogonalize()

            Dim Xo As Double = Me.X

            Me.X = -Me.Y
            Me.Y = Xo

        End Sub

        Public Sub Normalize()
            Dim Norm As Double = Me.Norm2
            Me.X = Me.X / Norm
            Me.Y = Me.Y / Norm
        End Sub

        Public Function DotProduct(ByVal v As EVector2) As Double
            Return X * v.X + Y * v.Y
        End Function

#End Region

#Region " Otras operaciones "

        Public Sub SetToCero()
            Me.X = 0
            Me.Y = 0
        End Sub

        Public Sub ReadFromString(ByVal Line As String, Optional ByVal Margen As Integer = 1)
            Me.X = CDbl(Mid(Line, Margen, 25))
            Me.Y = CDbl(Mid(Line, Margen + 25, 25))
        End Sub

        Public Sub SetCoordinates(ByVal X As Double, ByVal Y As Double)
            Me.X = X
            Me.Y = Y
        End Sub

        ''' <summary>
        ''' Rotates the vector an angle a [rad]
        ''' </summary>
        ''' <param name="a"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Rotate(ByVal a As Double)

            Dim rotX = X * Math.Cos(a) - Y * Math.Sin(a)
            Dim rotY = X * Math.Sin(a) + Y * Math.Cos(a)
            Me.X = rotX
            Me.Y = rotY

        End Sub

        ''' <summary>
        ''' Rotates the vector an angle a [rad]
        ''' </summary>
        ''' <param name="a"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Rotate(ByVal a As Double, ByVal x As Double, ByVal y As Double)
            Me.X -= x
            Me.Y -= y
            Dim rotX As Double = Me.X * Math.Cos(a) - Me.Y * Math.Sin(a)
            Dim rotY As Double = Me.X * Math.Sin(a) + Me.Y * Math.Cos(a)
            Me.X = rotX + x
            Me.Y = rotY + y
        End Sub

        Public Shared Function ParametricIntersection(ByVal p1 As EVector2, ByVal v1 As EVector2, ByVal p2 As EVector2, ByVal v2 As EVector2) As EVector2

            Dim det As Double = v2.X * v1.Y - v1.X * v2.Y

            If det <> 0 Then

                Dim intersection As New EVector2
                Dim bx As Double = p2.X - p1.X
                Dim by As Double = p2.Y - p1.Y

                intersection.X = (-v2.Y * bx + v2.X * by) / det
                intersection.Y = (-v1.Y * bx + v1.X * by) / det

                Return intersection

            Else

                Return Nothing

            End If

        End Function

        ''' <summary>
        ''' Calculates the intersection between line {p, v} with segment {a, b} as a coordinate relative to point a.
        ''' </summary>
        ''' <param name="p"></param>
        ''' <param name="v"></param>
        ''' <param name="a"></param>
        ''' <param name="b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function IntersectionCoordinate(ByVal p As EVector2, ByVal v As EVector2, ByVal a As EVector2, ByVal b As EVector2) As Double

            Dim u As New EVector2(b.X - a.X, b.Y - a.Y)
            Dim coordinates As EVector2 = ParametricIntersection(p, v, a, u)

            If coordinates.Y >= 0 And coordinates.Y <= 1 Then
                Return coordinates.Y
            Else
                Return Double.NaN
            End If

        End Function

#End Region

    End Class

    Public Class EVector3

        Public X As Double
        Public Y As Double
        Public Z As Double
        Public Active As Boolean = False

        Public Sub New()

        End Sub

        Public Sub New(ByVal Vector As EVector3)
            Me.X = Vector.X
            Me.Y = Vector.Y
            Me.Z = Vector.Z
        End Sub

        Public Sub New(ByVal Vector As EVector3, ByVal Factor As Double)
            Me.X = Factor * Vector.X
            Me.Y = Factor * Vector.Y
            Me.Z = Factor * Vector.Z
        End Sub

        Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
            Me.X = X
            Me.Y = Y
            Me.Z = Z
        End Sub

#Region " Operaciones aritméticas "

        ''' <summary>
        ''' Adds the given vector
        ''' </summary>
        ''' <param name="Vector"></param>
        ''' <remarks></remarks>
        Public Sub Add(ByVal Vector As EVector3)
            Me.X += Vector.X
            Me.Y += Vector.Y
            Me.Z += Vector.Z
        End Sub

        ''' <summary>
        ''' Adds the given vector premltiplied by the given scalar
        ''' </summary>
        ''' <param name="Vector"></param>
        ''' <param name="Scalar"></param>
        ''' <remarks></remarks>
        Public Sub Add(ByVal Vector As EVector3, ByVal Scalar As Double)

            Me.X += Vector.X * Scalar
            Me.Y += Vector.Y * Scalar
            Me.Z += Vector.Z * Scalar

        End Sub

        ''' <summary>
        ''' Adds the given vector coordinates
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Add(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
            Me.X += X
            Me.Y += Y
            Me.Z += Z
        End Sub

        ''' <summary>
        ''' Substracts the given vector
        ''' </summary>
        ''' <param name="Vector"></param>
        ''' <remarks></remarks>
        Public Sub Substract(ByVal Vector As EVector3)

            Me.X -= Vector.X
            Me.Y -= Vector.Y
            Me.Z -= Vector.Z

        End Sub

        ''' <summary>
        ''' Adds the cross product between two vectors
        ''' </summary>
        ''' <param name="V1"></param>
        ''' <param name="V2"></param>
        ''' <remarks></remarks>
        Public Sub AddCrossProduct(ByVal V1 As EVector3, ByVal V2 As EVector3)
            X += V1.Y * V2.Z - V1.Z * V2.Y
            Y += V1.Z * V2.X - V1.X * V2.Z
            Z += V1.X * V2.Y - V1.Y * V2.X
        End Sub

        Public Shared Operator +(ByVal V1 As EVector3, ByVal V2 As EVector3) As EVector3
            Dim Suma As New EVector3
            Suma.X = V1.X + V2.X
            Suma.Y = V1.Y + V2.Y
            Suma.Z = V1.Z + V2.Z
            Return Suma
        End Operator

        Public Shared Operator -(ByVal V1 As EVector3, ByVal V2 As EVector3) As EVector3
            Dim Resta As New EVector3
            Resta.X = V1.X - V2.X
            Resta.Y = V1.Y - V2.Y
            Resta.Z = V1.Z - V2.Z
            Return Resta
        End Operator

        Public Shared Operator -(ByVal V As EVector3) As EVector3
            Dim Opuesto As New EVector3
            Opuesto.X = -V.X
            Opuesto.Y = -V.Y
            Opuesto.Z = -V.Z
            Return Opuesto
        End Operator

        Public Overloads Shared Operator *(ByVal V1 As EVector3, ByVal V2 As EVector3) As Double
            Return V1.X * V2.X + V1.Y * V2.Y + V1.Z * V2.Z
        End Operator

        Public Overloads Shared Operator *(ByVal Escalar As Double, ByVal V As EVector3) As EVector3
            Dim Producto As New EVector3

            Producto.X = Escalar * V.X
            Producto.Y = Escalar * V.Y
            Producto.Z = Escalar * V.Z

            Return Producto
        End Operator

        Public Shared Operator ^(ByVal V1 As EVector3, ByVal V2 As EVector3) As EVector3
            Dim Producto As New EVector3

            Producto.X = V1.Y * V2.Z - V1.Z * V2.Y
            Producto.Y = V1.Z * V2.X - V1.X * V2.Z
            Producto.Z = V1.X * V2.Y - V1.Y * V2.X

            Return Producto
        End Operator

        Public Sub Oppose()
            Me.X = -Me.X
            Me.Y = -Me.Y
            Me.Z = -Me.Z
        End Sub

#End Region

#Region " Operaciones métricas "

        Public Function InnerProduct(ByVal Punto As EVector3) As Double
            Return X * Punto.X + Y * Punto.Y + Z * Punto.Z
        End Function

        Public ReadOnly Property EuclideanNorm As Double
            Get
                ' In the .NET frameworks X * X is much more effective than X ^ 2.
                Dim Value As Double = Math.Sqrt(X * X + Y * Y + Z * Z)
                Return Value
            End Get
        End Property

        Public ReadOnly Property SquareEuclideanNorm As Double
            Get
                Dim Value As Double = X * X + Y * Y + Z * Z
                Return Value
            End Get
        End Property

        ' Review these properties. They might work much slower than they should!!

        Public Function RelativePosition(ByVal Punto As EVector3) As EVector3
            Dim Posicion As New EVector3(Punto)
            Posicion.Substract(Me)
            Return Posicion
        End Function

        Public Function DistanceTo(ByVal Punto As EVector3) As Double
            Return Me.RelativePosition(Punto).EuclideanNorm
        End Function

        Public ReadOnly Property ProjectedVector(ByVal Vector As EVector3) As EVector3
            Get
                Dim Projection As Double = Algebra.InnerProduct(Me, Vector)
                Dim SqrNorm As Double = Vector.SquareEuclideanNorm
                Return Algebra.ScalarProduct(Projection / SqrNorm, Vector)
            End Get
        End Property

        Public ReadOnly Property OrthogonalVector(ByVal Vector As EVector3) As EVector3
            Get
                Dim Projection As EVector3 = ProjectedVector(Vector)
                Return Algebra.SubstractVectors(Me, Projection)
            End Get
        End Property

        Public ReadOnly Property NormalizedDirection As EVector3
            Get
                Dim Norma As Double = Me.EuclideanNorm
                Dim VectorEscalado As New EVector3(Me, 1 / Norma)
                'VectorEscalado.Escalar(1 / Norma)
                Return VectorEscalado
                'Dim Nm1 As Double = 1 / Me.NormaEuclidea
                'Return New EVector3(Nm1 * X, Nm1 * Y, Nm1 * Z)
            End Get
        End Property

        Public Sub Normalize()
            Dim Nm1 As Double = 1 / Math.Sqrt(X * X + Y * Y + Z * Z)
            Scale(Nm1)
        End Sub

#End Region

#Region " Operaciones lineales "

        ''' <summary>
        ''' Multiplies the vector by a given scalar factor.
        ''' </summary>
        ''' <param name="Factor"></param>
        ''' <remarks></remarks>
        Public Sub Scale(ByVal Factor As Double)
            X = X * Factor
            Y = Y * Factor
            Z = Z * Factor
        End Sub

        ''' <summary>
        ''' Rotatets the vector in euler angles.
        ''' </summary>
        ''' <param name="Psi"></param>
        ''' <param name="Tita"></param>
        ''' <param name="Fi"></param>
        ''' <remarks></remarks>
        Public Sub Rotate(ByVal Psi As Double, ByVal Tita As Double, ByVal Fi As Double)

            Dim RotM As New RotationMatrix
            Dim Px As Double
            Dim Py As Double
            Dim Pz As Double

            Px = X
            Py = Y
            Pz = Z

            RotM.Generate(Psi * Math.PI / 180, Tita * Math.PI / 180, Fi * Math.PI / 180)

            Me.X = Px * RotM.Item(1, 1) + Py * RotM.Item(1, 2) + Pz * RotM.Item(1, 3)
            Me.Y = Px * RotM.Item(2, 1) + Py * RotM.Item(2, 2) + Pz * RotM.Item(2, 3)
            Me.Z = Px * RotM.Item(3, 1) + Py * RotM.Item(3, 2) + Pz * RotM.Item(3, 3)

        End Sub

        ''' <summary>
        ''' Rotatets the vector with a given rotation matrix.
        ''' </summary>
        ''' <param name="RotM"></param>
        ''' <remarks></remarks>
        Public Sub Rotate(ByVal RotM As RotationMatrix)

            Dim Px As Double
            Dim Py As Double
            Dim Pz As Double

            Px = X
            Py = Y
            Pz = Z

            X = Px * RotM.Item(1, 1) + Py * RotM.Item(1, 2) + Pz * RotM.Item(1, 3)
            Y = Px * RotM.Item(2, 1) + Py * RotM.Item(2, 2) + Pz * RotM.Item(2, 3)
            Z = Px * RotM.Item(3, 1) + Py * RotM.Item(3, 2) + Pz * RotM.Item(3, 3)

            RotM = Nothing

        End Sub

        Public Sub Rotate(ByVal ReferencePoint As EVector3, ByVal RotM As RotationMatrix)
            Substract(ReferencePoint)
            Rotate(RotM)
            Add(ReferencePoint)
        End Sub

        ''' <summary>
        ''' Projects the vector on a given direction.
        ''' </summary>
        ''' <param name="Direction"></param>
        ''' <remarks></remarks>
        Public Sub ProjectOnVector(ByVal Direction As EVector3)
            Dim Proyección As Double = InnerProduct(Direction)
            Me.Assign(Direction, Proyección)
        End Sub

        ''' <summary>
        ''' Projects the vector on a given plane.
        ''' </summary>
        ''' <param name="NormalDirection"></param>
        ''' <remarks></remarks>
        Public Sub ProjectOnPlane(ByVal NormalDirection As EVector3)
            Dim Proyección As New EVector3
            Dim dp As Double = Me.X * NormalDirection.X + Me.Y * NormalDirection.Y + Me.Z * NormalDirection.Z
            Proyección.X = dp * NormalDirection.X
            Proyección.Y = dp * NormalDirection.Y
            Proyección.Z = dp * NormalDirection.Z
            Me.X -= Proyección.X
            Me.Y -= Proyección.Y
            Me.Z -= Proyección.Z
        End Sub

        ''' <summary>
        ''' Returns the projection of the vector on a given direction.
        ''' </summary>
        ''' <param name="Direction"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetProjection(ByVal Direction As EVector3) As EVector3
            Dim NuevoVector As New EVector3
            Direction.Normalize()
            NuevoVector.X = Direction.X * InnerProduct(Direction)
            NuevoVector.Y = Direction.Y * InnerProduct(Direction)
            NuevoVector.Y = Direction.Y * InnerProduct(Direction)
            Return NuevoVector

        End Function

#End Region

#Region " Operaciones vectoriales "

        Public Function VectorProduct(ByVal Vector As EVector3) As EVector3
            Dim Producto As New EVector3
            Producto.X = Y * Vector.Z - Z * Vector.Y
            Producto.Y = Z * Vector.X - X * Vector.Z
            Producto.Z = X * Vector.Y - Y * Vector.X
            Return Producto
        End Function

        Public Sub VectorMultiplication(ByVal Vector As EVector3)
            Dim Producto As New EVector3(Me)
            Dim Xo As Double = X
            Dim Yo As Double = Y
            Dim Zo As Double = Z
            X = Yo * Vector.Z - Zo * Vector.Y
            Y = Zo * Vector.X - Xo * Vector.Z
            Z = Xo * Vector.Y - Yo * Vector.X
        End Sub

        Public Sub FromVectorProduct(ByVal V1 As EVector3, ByVal V2 As EVector3)
            X = V1.Y * V2.Z - V1.Z * V2.Y
            Y = V1.Z * V2.X - V1.X * V2.Z
            Z = V1.X * V2.Y - V1.Y * V2.X
        End Sub

        Public Sub FromSubstraction(ByVal V1 As EVector3, ByVal V2 As EVector3)
            X = V1.X - V2.X
            Y = V1.Y - V2.Y
            Z = V1.Z - V2.Z
        End Sub

        Public Function GetVectorToPoint(ByVal Punto As EVector3) As EVector3
            Dim Diferencia As New EVector3
            Diferencia.X = Punto.X - Me.X
            Diferencia.Y = Punto.Y - Me.Y
            Diferencia.Z = Punto.Z - Me.Z
            Return Diferencia
        End Function

#End Region

#Region " Otras operaciones "

        Public Sub SetToCero()
            X = 0
            Y = 0
            Z = 0
        End Sub

        Public Overloads Sub Assign(ByVal Vector As EVector3)
            X = Vector.X
            Y = Vector.Y
            Z = Vector.Z
        End Sub

        Public Overloads Sub Assign(ByVal Vector As EVector3, ByVal Factor As Double)
            X = Factor * Vector.X
            Y = Factor * Vector.Y
            Z = Factor * Vector.Z
        End Sub

        Public Sub ReadFromString(ByVal Line As String, Optional ByVal Margen As Integer = 1)
            X = CDbl(Mid(Line, Margen, 25))
            Y = CDbl(Mid(Line, Margen + 25, 25))
            Z = CDbl(Right(Line, 25))
        End Sub

        Public Function Clone() As EVector3
            Return New EVector3(Me)
        End Function

#End Region

    End Class

    'Public Class EPoint3

    '    Inherits EVector3

    '    Public Sub New()

    '    End Sub

    '    Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
    '        Me.X = X
    '        Me.Y = Y
    '        Me.Z = Z
    '    End Sub

    '    Public Sub Translate(ByVal DX As Double, ByVal DY As Double, ByVal DZ As Double)
    '        Me.X += DX
    '        Me.Y += DY
    '        Me.Z += DZ
    '    End Sub

    '    Public Sub Translate(ByVal Vector As EVector3)
    '        Me.X += Vector.X
    '        Me.Y += Vector.Y
    '        Me.Z += Vector.Z
    '    End Sub

    '    Public Function GetDirectionToOrigin() As EVector3

    '        Dim UnitVector As New EVector3
    '        If Me.EuclideanNorm > 0 Then
    '            UnitVector.X = Me.X / Me.EuclideanNorm
    '            UnitVector.Y = Me.Y / Me.EuclideanNorm
    '            UnitVector.Z = Me.Z / Me.EuclideanNorm
    '        Else
    '            UnitVector.X = 0
    '            UnitVector.Y = 0
    '            UnitVector.Z = 0
    '        End If

    '        Return UnitVector

    '    End Function

    '    Public Function GetVectorToPoint(ByVal Punto As EPoint3) As EVector3
    '        Dim Diferencia As New EVector3
    '        Diferencia.X = Punto.X - Me.X
    '        Diferencia.Y = Punto.Y - Me.Y
    '        Diferencia.Z = Punto.Z - Me.Z
    '        Return Diferencia
    '    End Function

    '    Public Function GetDirectionToPoint(ByVal Punto As EPoint3) As EVector3
    '        Dim UnitVector As New EVector3
    '        Dim Distancia As Double = 1 / GetDistanceToPoint(Punto)
    '        UnitVector.X = (X - Punto.X) * Distancia
    '        UnitVector.Y = (Y - Punto.Y) * Distancia
    '        UnitVector.Z = (Z - Punto.Z) * Distancia
    '        Return UnitVector
    '    End Function

    '    Public Function GetDistanceToPoint(ByVal Punto As EPoint3) As Double
    '        Dim Diferencia As New EPoint3
    '        Diferencia.X = X - Punto.X
    '        Diferencia.Y = Y - Punto.Y
    '        Diferencia.Z = Z - Punto.Z
    '        Return Diferencia.EuclideanNorm
    '    End Function

    'End Class

    ''' <summary>
    ''' Represents a basis of orthonormal vectors on the euclidean space.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class EBase3

        Public U As New EVector3
        Public V As New EVector3
        Public W As New EVector3

    End Class

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

    ''' <summary>
    ''' Represents an orientation in Euler angles
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OrientationCoordinates

        Public Psi As Double
        Public Tita As Double
        Public Fi As Double

        Public Sub SetToCero()
            Me.Psi = 0
            Me.Tita = 0
            Me.Fi = 0
        End Sub

        Public Function ToRadians() As OrientationCoordinates

            Dim NuevaOrientacion As New OrientationCoordinates
            Dim Conversion As Double = Math.PI / 180
            NuevaOrientacion.Psi = Me.Psi * Conversion
            NuevaOrientacion.Tita = Me.Tita * Conversion
            NuevaOrientacion.Fi = Me.Fi * Conversion
            Return NuevaOrientacion

        End Function

        Public Sub Assign(ByVal Ori As OrientationCoordinates)

            Me.Fi = Ori.Fi
            Me.Psi = Ori.Psi
            Me.Tita = Ori.Tita

        End Sub

    End Class

End Namespace

Namespace Algebra.CustomMatrices

    Public Structure Matrix

        Public Sub New(ByVal i As Integer, ByVal j As Integer)
            ReDim FElements(i - 1, j - 1)
            Me.FRows = i
            Me.FColumns = j
            FTransponse = False
        End Sub

        Private FElements(,) As Double
        Public Property Element(ByVal i As Integer, ByVal j As Integer) As Double
            Get
                If Not FTransponse Then
                    Return FElements(i - 1, j - 1)
                Else
                    Return FElements(j - 1, i - 1)
                End If
            End Get
            Set(ByVal value As Double)
                If Not FTransponse Then
                    FElements(i - 1, j - 1) = value
                Else
                    FElements(j - 1, i - 1) = value
                End If
            End Set
        End Property

        Private FRows As Integer
        Public ReadOnly Property Rows As Integer
            Get
                If Not FTransponse Then
                    Return FRows
                Else
                    Return FColumns
                End If
            End Get
        End Property

        Private FColumns As Integer
        Public ReadOnly Property Columns As Integer
            Get
                If Not FTransponse Then
                    Return FColumns
                Else
                    Return FRows
                End If
            End Get
        End Property

        Private FTransponse As Boolean
        Public Sub Transponse()
            FTransponse = Not FTransponse
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

        Private RM(2, 2) As Double

        Public Property Item(ByVal i As Integer, ByVal j As Integer) As Double
            Get
                If 1 < i < 3 And 1 < i < 3 Then
                    Return RM(i - 1, j - 1)
                Else
                    Return 0
                End If
            End Get
            Set(ByVal value As Double)
                If 1 < i < 3 And 1 < i < 3 Then
                    RM(i - 1, j - 1) = value
                End If
            End Set
        End Property

        Public ReadOnly Property Determinante As Double
            Get
                Return Item(1, 1) * (Item(2, 2) * Item(3, 3) - Item(3, 2) * Item(2, 3)) - _
                Item(1, 2) * (Item(2, 1) * Item(3, 3) - Item(3, 1) * Item(3, 2)) + _
                Item(1, 3) * (Item(2, 1) * Item(3, 2) - Item(3, 1) * Item(2, 2))
            End Get
        End Property

        Public Sub Transponer()
            Dim Matriz As New Matrix3x3
            Matriz.RM = Me.RM
            For i = 1 To 3
                For j = 1 To 3
                    Me.Item(i, j) = Matriz.Item(j, i)
                Next
            Next
        End Sub

        Public Sub Invertir()

        End Sub

    End Class

    Public Class RotationMatrix

        Inherits Matrix3x3

        Public Sub Generate(ByVal t1 As Double, ByVal t2 As Double, ByVal t3 As Double)

            Item(1, 1) = Math.Cos(t1) * Math.Cos(t2)
            Item(1, 2) = Math.Cos(t1) * Math.Sin(t2) * Math.Sin(t3) - Math.Sin(t1) * Math.Cos(t3)
            Item(1, 3) = Math.Sin(t1) * Math.Sin(t3) + Math.Cos(t1) * Math.Sin(t2) * Math.Cos(t3)
            Item(2, 1) = Math.Sin(t1) * Math.Cos(t2)
            Item(2, 2) = Math.Sin(t1) * Math.Sin(t2) * Math.Sin(t3) + Math.Cos(t1) * Math.Cos(t3)
            Item(2, 3) = Math.Sin(t1) * Math.Sin(t2) * Math.Cos(t3) - Math.Cos(t1) * Math.Sin(t3)
            Item(3, 1) = -Math.Sin(t2)
            Item(3, 2) = Math.Cos(t2) * Math.Sin(t3)
            Item(3, 3) = Math.Cos(t2) * Math.Cos(t3)

        End Sub

        Public Sub Generate(ByVal Orientation As OrientationCoordinates)

            Item(1, 1) = Math.Cos(Orientation.Psi) * Math.Cos(Orientation.Tita)
            Item(1, 2) = Math.Cos(Orientation.Psi) * Math.Sin(Orientation.Tita) * Math.Sin(Orientation.Fi) - Math.Sin(Orientation.Psi) * Math.Cos(Orientation.Fi)
            Item(1, 3) = Math.Sin(Orientation.Psi) * Math.Sin(Orientation.Fi) + Math.Cos(Orientation.Psi) * Math.Sin(Orientation.Tita) * Math.Cos(Orientation.Fi)
            Item(2, 1) = Math.Sin(Orientation.Psi) * Math.Cos(Orientation.Tita)
            Item(2, 2) = Math.Sin(Orientation.Psi) * Math.Sin(Orientation.Tita) * Math.Sin(Orientation.Fi) + Math.Cos(Orientation.Psi) * Math.Cos(Orientation.Fi)
            Item(2, 3) = Math.Sin(Orientation.Psi) * Math.Sin(Orientation.Tita) * Math.Cos(Orientation.Fi) - Math.Cos(Orientation.Psi) * Math.Sin(Orientation.Fi)
            Item(3, 1) = -Math.Sin(Orientation.Tita)
            Item(3, 2) = Math.Cos(Orientation.Tita) * Math.Sin(Orientation.Fi)
            Item(3, 3) = Math.Cos(Orientation.Tita) * Math.Cos(Orientation.Fi)

        End Sub

    End Class

End Namespace

Public Structure LimitValues
    Public Maximum As Double
    Public Minimum As Double
End Structure
Imports UVLM_General.MathLibrary.EspacioEuclideo
Imports UVLM_General.MathLibrary.CustomMatrices

Namespace MathLibrary.EspacioEuclideo

    Public Class Base

        Public Shared U As New EVector3
        Public Shared V As New EVector3
        Public Shared W As New EVector3

        Public Shared Sub BaseCanónica()

            U.X = 1.0
            U.Y = 0.0
            U.Z = 0.0

            V.X = 0.0
            V.Y = 1.0
            V.Z = 0.0

            W.X = 0.0
            W.Y = 0.0
            W.Z = 1.0

        End Sub

    End Class

    Public Structure EVector2

        Public X As Double
        Public Y As Double

#Region " Operaciones aritméticos "

        Public Shared Operator +(ByVal V1 As EVector2, ByVal V2 As EVector2) As EVector2
            Dim Suma As EVector2
            Suma.X = V1.X + V2.X
            Suma.Y = V1.Y + V2.Y
            Return Suma
        End Operator

        Public Shared Operator -(ByVal V1 As EVector2, ByVal V2 As EVector2) As EVector2
            Dim Resta As EVector2
            Resta.X = V1.X - V2.X
            Resta.Y = V1.Y - V2.Y
            Return Resta
        End Operator

        Public Shared Operator -(ByVal V As EVector2) As EVector2
            Dim Opuesto As EVector2
            Opuesto.X = -V.X
            Opuesto.Y = -V.Y
            Return Opuesto
        End Operator

        Public Overloads Shared Operator *(ByVal V1 As EVector2, ByVal V2 As EVector2) As Double
            Return V1.X * V2.X + V1.Y * V2.Y
        End Operator

        Public Overloads Shared Operator *(ByVal Escalar As Double, ByVal V As EVector2) As EVector2
            Dim Producto As EVector2

            Producto.X = Escalar * V.X
            Producto.Y = Escalar * V.Y

            Return Producto
        End Operator

        Public Shared Operator ^(ByVal V1 As EVector2, ByVal V2 As EVector2) As Double

            Return V1.X * V2.Y - V1.Y * V2.X

        End Operator

        Public Sub Oponer()
            Me.X = -Me.X
            Me.Y = -Me.Y
        End Sub

        Public Sub Scale(ByVal Scalar As Double)
            Me.X = Scalar * Me.X
            Me.Y = Scalar * Me.Y
        End Sub

#End Region

#Region " Operaciones métricas "

        Public ReadOnly Property NormaEuclidea
            Get
                Return Math.Sqrt(Me * Me)
            End Get
        End Property

        Public ReadOnly Property SqrtNormaEuclidea As Double
            Get
                Return Me * Me
            End Get
        End Property

        Public Function PosiciónRelativa(ByVal Punto As EVector2) As EVector2
            Return Punto - Me
        End Function

        Public Function Distancia(ByVal Punto As EVector2) As Double
            Return Me.PosiciónRelativa(Punto).NormaEuclidea
        End Function

        Public ReadOnly Property VectorProyección(ByVal Vector As EVector2) As EVector2
            Get
                Return (Me * Vector) / (Vector * Vector) * Vector
            End Get
        End Property

        Public ReadOnly Property VectorOrtogonal(ByVal Vector As EVector2) As EVector2
            Get
                Return Me - Me.VectorProyección(Vector)
            End Get
        End Property

        Public Sub Ortogonalizar()

            Dim Xo As Double = Me.X

            Me.X = -Me.Y
            Me.Y = Xo

        End Sub

        Public Sub Normalizar()
            Dim Norm As Double = Me.NormaEuclidea
            Me.X = Me.X / Norm
            Me.Y = Me.Y / Norm
        End Sub

#End Region

#Region " Otras operaciones "

        Public Sub SetToCero()
            Me.X = 0
            Me.Y = 0
        End Sub

        Public Sub LeerDesdeString(ByVal Line As String, Optional ByVal Margen As Integer = 1)
            Me.X = CDbl(Mid(Line, Margen, 25))
            Me.Y = CDbl(Mid(Line, Margen + 25, 25))
        End Sub

        Public Sub SetCoordinates(ByVal X As Double, ByVal Y As Double)
            Me.X = X
            Me.Y = Y
        End Sub

#End Region

    End Structure

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

        Public Sub Add(ByVal Vector As EVector3)
            Me.X += Vector.X
            Me.Y += Vector.Y
            Me.Z += Vector.Z
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

        Public Sub Oponer()
            Me.X = -Me.X
            Me.Y = -Me.Y
            Me.Z = -Me.Z
        End Sub

        Public Sub Adicionar(ByVal Vector As EVector3)

            Me.X += Vector.X
            Me.Y += Vector.Y
            Me.Z += Vector.Z

        End Sub

        Public Sub Adicionar(ByVal Vector As EVector3, ByVal Escalar As Double)

            Me.X += Vector.X * Escalar
            Me.Y += Vector.Y * Escalar
            Me.Z += Vector.Z * Escalar

        End Sub

        Public Sub Substraer(ByVal Vector As EVector3)

            Me.X -= Vector.X
            Me.Y -= Vector.Y
            Me.Z -= Vector.Z

        End Sub

#End Region

#Region " Operaciones métricas "

        Public Function ProductoInterno(ByVal Punto As EVector3) As Double
            Return X * Punto.X + Y * Punto.Y + Z * Punto.Z
        End Function

        Public ReadOnly Property NormaEuclidea As Double
            Get
                ' In the .NET frameworks X * X is much more effective than X ^ 2.
                Dim Value As Double = Math.Sqrt(X * X + Y * Y + Z * Z)
                Return Value
            End Get
        End Property

        Public ReadOnly Property SqrNormaEuclidea As Double
            Get
                Dim Value As Double = X * X + Y * Y + Z * Z
                Return Value
            End Get
        End Property

        ' Review these properties. They might work much slower than they should!!

        Public Function PosiciónRelativa(ByVal Punto As EVector3) As EVector3
            Dim Posicion As New EVector3(Punto)
            Posicion.Substraer(Me)
            Return Posicion
        End Function

        Public Function Distancia(ByVal Punto As EVector3) As Double
            Return Me.PosiciónRelativa(Punto).NormaEuclidea
        End Function

        Public ReadOnly Property VectorProyección(ByVal Vector As EVector3) As EVector3
            Get
                Dim Projection As Double = Algebra.ProductoInterno(Me, Vector)
                Dim SqrNorm As Double = Vector.SqrNormaEuclidea
                Return Algebra.ProductoAbierto(Projection / SqrNorm, Vector)
            End Get
        End Property

        Public ReadOnly Property VectorOrtogonal(ByVal Vector As EVector3) As EVector3
            Get
                Dim Projection As EVector3 = VectorProyección(Vector)
                Return Algebra.RestarVectores(Me, Projection)
            End Get
        End Property

        Public ReadOnly Property VectorNormalizado As EVector3
            Get
                Dim Norma As Double = Me.NormaEuclidea
                Dim VectorEscalado As New EVector3(Me, 1 / Norma)
                'VectorEscalado.Escalar(1 / Norma)
                Return VectorEscalado
                'Dim Nm1 As Double = 1 / Me.NormaEuclidea
                'Return New EVector3(Nm1 * X, Nm1 * Y, Nm1 * Z)
            End Get
        End Property

        Public Sub Normalizar()
            Dim Nm1 As Double = 1 / Math.Sqrt(X * X + Y * Y + Z * Z)
            Escalar(Nm1)
        End Sub

#End Region

#Region " Operaciones lineales "

        Public Sub Escalar(ByVal Escala As Double)
            X = X * Escala
            Y = Y * Escala
            Z = Z * Escala
        End Sub

        Public Sub Rotar(ByVal Psi As Double, ByVal Tita As Double, ByVal Fi As Double)

            Dim RotM As New TMatrizDeRotación
            Dim Px As Double
            Dim Py As Double
            Dim Pz As Double

            Px = X
            Py = Y
            Pz = Z

            RotM.Generar(Psi * Math.PI / 180, Tita * Math.PI / 180, Fi * Math.PI / 180)

            Me.X = Px * RotM.Item(1, 1) + Py * RotM.Item(1, 2) + Pz * RotM.Item(1, 3)
            Me.Y = Px * RotM.Item(2, 1) + Py * RotM.Item(2, 2) + Pz * RotM.Item(2, 3)
            Me.Z = Px * RotM.Item(3, 1) + Py * RotM.Item(3, 2) + Pz * RotM.Item(3, 3)

        End Sub

        Public Sub Rotar(ByVal RotM As TMatrizDeRotación)

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

        Public Sub MultiplicarPorEscalar(ByVal Escalar As Double)
            X = X * Escalar
            Y = Y * Escalar
            Z = Z * Escalar
        End Sub

        Public Sub ProyectarSobreVector(ByVal Dirección As EVector3)
            Dim Proyección As Double = ProductoInterno(Dirección)
            Me.Asignar(Dirección, Proyección)
        End Sub

        Public Sub ProyectarSobrePlano(ByVal Dirección As EVector3)
            Dim Proyección As New EVector3
            Dim dp As Double = Me.X * Dirección.X + Me.Y * Dirección.Y + Me.Z * Dirección.Z
            Proyección.X = dp * Dirección.X
            Proyección.Y = dp * Dirección.Y
            Proyección.Z = dp * Dirección.Z
            Me.X -= Proyección.X
            Me.Y -= Proyección.Y
            Me.Z -= Proyección.Z
        End Sub

        Public Function ObtenerProyección(ByVal Dirección As EVector3) As EVector3
            Dim NuevoVector As New EVector3
            Dirección.Normalizar()
            NuevoVector.X = Dirección.X * ProductoInterno(Dirección)
            NuevoVector.Y = Dirección.Y * ProductoInterno(Dirección)
            NuevoVector.Y = Dirección.Y * ProductoInterno(Dirección)
            Return NuevoVector

        End Function

#End Region

#Region " Operaciones vectoriales "

        Public Function ProductoVectorial(ByVal Vector As EVector3) As EVector3
            Dim Producto As New EVector3
            Producto.X = Y * Vector.Z - Z * Vector.Y
            Producto.Y = Z * Vector.X - X * Vector.Z
            Producto.Z = X * Vector.Y - Y * Vector.X
            Return Producto
        End Function

        Public Sub MultiplicarVectorialmente(ByVal Vector As EVector3)
            Dim Producto As New EVector3(Me)
            Dim Xo As Double = X
            Dim Yo As Double = Y
            Dim Zo As Double = Z
            X = Yo * Vector.Z - Zo * Vector.Y
            Y = Zo * Vector.X - Xo * Vector.Z
            Z = Xo * Vector.Y - Yo * Vector.X
        End Sub

        Public Sub DeProductoVectorial(ByVal V1 As EVector3, ByVal V2 As EVector3)
            X = V1.Y * V2.Z - V1.Z * V2.Y
            Y = V1.Z * V2.X - V1.X * V2.Z
            Z = V1.X * V2.Y - V1.Y * V2.X
        End Sub

        Public Sub DeResta(ByVal V1 As EVector3, ByVal V2 As EVector3)
            X = V1.X - V2.X
            Y = V1.Y - V2.Y
            Z = V1.Z - V2.Z
        End Sub

#End Region

#Region " Otras operaciones "

        Public Sub SetToCero()
            X = 0
            Y = 0
            Z = 0
        End Sub

        Public Overloads Sub Asignar(ByVal Vector As EVector3)
            X = Vector.X
            Y = Vector.Y
            Z = Vector.Z
        End Sub

        Public Overloads Sub Asignar(ByVal Vector As EVector3, ByVal Factor As Double)
            X = Factor * Vector.X
            Y = Factor * Vector.Y
            Z = Factor * Vector.Z
        End Sub

        Public Sub LeerDesdeString(ByVal Line As String, Optional ByVal Margen As Integer = 1)
            X = CDbl(Mid(Line, Margen, 25))
            Y = CDbl(Mid(Line, Margen + 25, 25))
            Z = CDbl(Right(Line, 25))
        End Sub

        Public Function Duplicar() As EVector3
            Return New EVector3(Me)
        End Function

#End Region

    End Class

    Public Class EPoint3

        Inherits EVector3

        Public Sub New()

        End Sub

        Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
            Me.X = X
            Me.Y = Y
            Me.Z = Z
        End Sub

        Public Sub Transladar(ByVal DX As Double, ByVal DY As Double, ByVal DZ As Double)
            Me.X = Me.X + DX
            Me.Y = Me.Y + DY
            Me.Z = Me.Z + DZ
        End Sub

        Public Sub Transladar(ByVal Vector As EVector3)
            Me.X = Me.X + Vector.X
            Me.Y = Me.Y + Vector.Y
            Me.Z = Me.Z + Vector.Z
        End Sub

        Public Sub Simetrizar()
            Me.Y = -Me.Y
        End Sub

        Public Function ObtenerDirecciónAlOrigen() As EVector3

            Dim UnitVector As New EVector3
            If Me.NormaEuclidea > 0 Then
                UnitVector.X = Me.X / Me.NormaEuclidea
                UnitVector.Y = Me.Y / Me.NormaEuclidea
                UnitVector.Z = Me.Z / Me.NormaEuclidea
            Else
                UnitVector.X = 0
                UnitVector.Y = 0
                UnitVector.Z = 0
            End If

            Return UnitVector

        End Function

        Public Function ObtenerVectorAlPunto(ByVal Punto As EPoint3) As EVector3
            Dim Diferencia As New EVector3
            Diferencia.X = Punto.X - Me.X
            Diferencia.Y = Punto.Y - Me.Y
            Diferencia.Z = Punto.Z - Me.Z
            Return Diferencia
        End Function

        Public Function ObtenerDirecciónAlPunto(ByVal Punto As EPoint3) As EVector3
            Dim UnitVector As New EVector3
            Dim Distancia As Double = 1 / ObtenerDistanciaAlPunto(Punto)
            UnitVector.X = (X - Punto.X) * Distancia
            UnitVector.Y = (Y - Punto.Y) * Distancia
            UnitVector.Z = (Z - Punto.Z) * Distancia
            Return UnitVector
        End Function

        Public Function ObtenerDistanciaAlPunto(ByVal Punto As EPoint3) As Double
            Dim Diferencia As New EPoint3
            Diferencia.X = X - Punto.X
            Diferencia.Y = Y - Punto.Y
            Diferencia.Z = Z - Punto.Z
            Return Diferencia.NormaEuclidea
        End Function

    End Class

    Public Class EBase3

        Public U As New EVector3
        Public V As New EVector3
        Public W As New EVector3

    End Class

    Public Class Algebra

        Public Overloads Shared Function ProductoVectorial(ByVal Vector1 As TEuclideanVector, ByVal Vector2 As TEuclideanVector) As TEuclideanVector
            Dim Producto As New TEuclideanVector

            Producto.X = Vector1.Y * Vector2.Z - Vector1.Z * Vector2.Y
            Producto.Y = -Vector1.X * Vector2.Z + Vector2.X * Vector1.Z
            Producto.Z = Vector1.X * Vector2.Y - Vector1.Y * Vector2.X

            Return Producto

        End Function

        Public Overloads Shared Function ProductoVectorial(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As EVector3
            Dim Producto As New EVector3

            Producto.X = Vector1.Y * Vector2.Z - Vector1.Z * Vector2.Y
            Producto.Y = -Vector1.X * Vector2.Z + Vector2.X * Vector1.Z
            Producto.Z = Vector1.X * Vector2.Y - Vector1.Y * Vector2.X

            Return Producto

        End Function

        Public Overloads Shared Function ProductoInterno(ByVal Vector1 As TEuclideanVector, ByVal Vector2 As TEuclideanVector) As Double

            Dim Producto As Double

            Producto = Vector1.X * Vector2.X + Vector1.Y * Vector2.Y + Vector1.Z * Vector2.Z

            Return Producto

        End Function

        Public Overloads Shared Function ProductoInterno(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As Double

            Dim Producto As Double

            Producto = Vector1.X * Vector2.X + Vector1.Y * Vector2.Y + Vector1.Z * Vector2.Z

            Return Producto

        End Function

        Public Overloads Shared Function RestarVectores(ByVal Vector1 As TEuclideanVector, ByVal Vector2 As TEuclideanVector) As TEuclideanVector

            Dim Resta As New TEuclideanVector

            Resta.X = Vector1.X - Vector2.X
            Resta.Y = Vector1.Y - Vector2.Y
            Resta.Z = Vector1.Z - Vector2.Z

            Return Resta

        End Function

        Public Overloads Shared Function RestarVectores(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As EVector3

            Dim Resta As New EVector3

            Resta.X = Vector1.X - Vector2.X
            Resta.Y = Vector1.Y - Vector2.Y
            Resta.Z = Vector1.Z - Vector2.Z

            Return Resta

        End Function

        Public Overloads Shared Function SumarVectores(ByVal Vector1 As TEuclideanVector, ByVal Vector2 As TEuclideanVector) As TEuclideanVector

            Dim Resta As New TEuclideanVector

            Resta.X = Vector1.X + Vector2.X
            Resta.Y = Vector1.Y + Vector2.Y
            Resta.Z = Vector1.Z + Vector2.Z

            Return Resta

        End Function

        Public Overloads Shared Function SumarVectores(ByVal Vector1 As EVector3, ByVal Vector2 As EVector3) As EVector3

            Dim Resta As New EVector3

            Resta.X = Vector1.X + Vector2.X
            Resta.Y = Vector1.Y + Vector2.Y
            Resta.Z = Vector1.Z + Vector2.Z

            Return Resta

        End Function

        Public Overloads Shared Function ProductoAbierto(ByVal Escalar As Double, ByVal Vector As TEuclideanVector) As TEuclideanVector

            Dim Resultado As New TEuclideanVector

            Resultado.X = Escalar * Vector.X
            Resultado.Y = Escalar * Vector.Y
            Resultado.Z = Escalar * Vector.Z

            Return Resultado

        End Function

        Public Overloads Shared Function ProductoAbierto(ByVal Escalar As Double, ByVal Vector As EVector3) As EVector3

            Dim Resultado As New EVector3

            Resultado.X = Escalar * Vector.X
            Resultado.Y = Escalar * Vector.Y
            Resultado.Z = Escalar * Vector.Z

            Return Resultado

        End Function

    End Class

    Public Class TOrientacion

        Public Psi As Double
        Public Tita As Double
        Public Fi As Double

        Public Sub SetToCero()
            Me.Psi = 0
            Me.Tita = 0
            Me.Fi = 0
        End Sub

        Public Function ToRadians() As TOrientacion

            Dim NuevaOrientacion As New TOrientacion
            Dim Conversion As Double = Math.PI / 180
            NuevaOrientacion.Psi = Me.Psi * Conversion
            NuevaOrientacion.Tita = Me.Tita * Conversion
            NuevaOrientacion.Tita = Me.Tita * Conversion
            Return NuevaOrientacion

        End Function

    End Class

End Namespace

Namespace MathLibrary.CustomMatrices

    Public Structure TMatrix

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

        Public Sub Assign(ByVal Matrix As TMatrix)
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
        Public Shared Operator +(ByVal M1 As TMatrix, ByVal M2 As TMatrix) As TMatrix

            If M1.Rows = M2.Rows And M1.Columns = M2.Columns Then
                Dim Sum As New TMatrix(M1.Rows, M1.Columns)

                For i = 1 To Sum.Rows
                    For j = 1 To Sum.Columns

                        Sum.Element(i, j) = M1.Element(i, j) + M2.Element(i, j)

                    Next
                Next

                Return Sum

            Else
                Return New TMatrix(0, 0)
            End If

        End Operator

        'Scalar multiplication:
        Public Shared Operator *(ByVal Escalar As Double, ByVal M As TMatrix) As TMatrix
            Dim Product As New TMatrix(M.Rows, M.Columns)

            For i = 1 To Product.Rows
                For j = 1 To Product.Columns

                    M.Element(i, j) = Escalar * M.Element(i, j)

                Next
            Next

            Return Product

        End Operator

        'Matrix multiplication:
        Public Shared Operator ^(ByVal M1 As TMatrix, ByVal M2 As TMatrix) As TMatrix

            If M1.Columns = M2.Rows Then
                Dim Product As New TMatrix(M1.Rows, M2.Columns)
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
                Return New TMatrix(0, 0)
            End If

        End Operator

#End Region

    End Structure

    Public Class Matriz3x3

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
            Dim Matriz As New Matriz3x3
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

    Public Class TMatrizDeRotación

        Inherits Matriz3x3

        Public Sub Generar(ByVal t1 As Double, ByVal t2 As Double, ByVal t3 As Double)

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

        Public Sub Generar(ByVal Orientacion As TOrientacion)

            Item(1, 1) = Math.Cos(Orientacion.Psi) * Math.Cos(Orientacion.Tita)
            Item(1, 2) = Math.Cos(Orientacion.Psi) * Math.Sin(Orientacion.Tita) * Math.Sin(Orientacion.Fi) - Math.Sin(Orientacion.Psi) * Math.Cos(Orientacion.Fi)
            Item(1, 3) = Math.Sin(Orientacion.Psi) * Math.Sin(Orientacion.Fi) + Math.Cos(Orientacion.Psi) * Math.Sin(Orientacion.Tita) * Math.Cos(Orientacion.Fi)
            Item(2, 1) = Math.Sin(Orientacion.Psi) * Math.Cos(Orientacion.Tita)
            Item(2, 2) = Math.Sin(Orientacion.Psi) * Math.Sin(Orientacion.Tita) * Math.Sin(Orientacion.Fi) + Math.Cos(Orientacion.Psi) * Math.Cos(Orientacion.Fi)
            Item(2, 3) = Math.Sin(Orientacion.Psi) * Math.Sin(Orientacion.Tita) * Math.Cos(Orientacion.Fi) - Math.Cos(Orientacion.Psi) * Math.Sin(Orientacion.Fi)
            Item(3, 1) = -Math.Sin(Orientacion.Tita)
            Item(3, 2) = Math.Cos(Orientacion.Tita) * Math.Sin(Orientacion.Fi)
            Item(3, 3) = Math.Cos(Orientacion.Tita) * Math.Cos(Orientacion.Fi)

        End Sub

    End Class

End Namespace

Public Class TEuclideanVector

    Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
        Me.X = X
        Me.Y = Y
        Me.Z = Z
    End Sub

    Public Active As Boolean = False

    Private FX As Double = 0.0
    Public Property X As Double
        Get
            Return FX
        End Get
        Set(ByVal value As Double)
            FX = value
        End Set
    End Property

    Private FY As Double = 0.0
    Public Property Y As Double
        Get
            Return FY
        End Get
        Set(ByVal value As Double)
            FY = value
        End Set
    End Property

    Private FZ As Double = 0.0
    Public Property Z As Double
        Get
            Return FZ
        End Get
        Set(ByVal value As Double)
            FZ = value
        End Set
    End Property

#Region " I/O "

    Public Sub New()
        X = 0.0#
        Y = 0.0#
        Z = 0.0#
    End Sub

    Public Sub SetToCero()
        Me.X = 0
        Me.Y = 0
        Me.Z = 0
    End Sub

    Public Sub Clear()
        Me.X = 0.0#
        Me.Y = 0.0#
        Me.Z = 0.0#
    End Sub

    Public Sub AsignarValor(ByVal Vector As TEuclideanVector)
        Me.X = Vector.X
        Me.Y = Vector.Y
        Me.Z = Vector.Z
    End Sub

    Public Sub AsignarValor(ByVal Vector As TEuclideanVector, ByVal Escalar As Double)
        Me.X = Vector.X * Escalar
        Me.Y = Vector.Y * Escalar
        Me.Z = Vector.Z * Escalar
    End Sub

    Public Sub LeerDesdeString(ByVal Line As String, Optional ByVal Margen As Integer = 1)
        Me.X = CDbl(Mid(Line, Margen, 25))
        Me.Y = CDbl(Mid(Line, Margen + 25, 25))
        Me.Z = CDbl(Right(Line, 25))
    End Sub

    Public Function ToTEuclideanVector() As TEuclideanVector
        Dim Vec3 As New TEuclideanVector
        Vec3.X = Me.X
        Vec3.Y = Me.Y
        Vec3.Z = Me.Z
        Return Vec3
    End Function

    Public Sub FromTEuclideanVector(ByVal Vector As TEuclideanVector)
        Me.X = Vector.X
        Me.Y = Vector.Y
        Me.Z = Vector.Z
    End Sub

#End Region

#Region " Propiedades geométricas "


#End Region

#Region " Operadores aritméticos "

    Public Shared Operator +(ByVal EV1 As TEuclideanVector, ByVal EV2 As TEuclideanVector) As TEuclideanVector
        Dim Suma As New TEuclideanVector
        Suma.FX = EV1.FX + EV2.FX
        Suma.FY = EV1.FY + EV2.FY
        Return Suma
    End Operator

    Public Shared Operator -(ByVal EV1 As TEuclideanVector, ByVal EV2 As TEuclideanVector) As TEuclideanVector
        Dim Resta As New TEuclideanVector
        Resta.FX = EV1.FX - EV2.FX
        Resta.FY = EV1.FY - EV2.FY
        Return Resta
    End Operator

    Public Shared Operator -(ByVal EV As TEuclideanVector) As TEuclideanVector
        Dim Opuesto As New TEuclideanVector
        Opuesto.FX = -EV.FX
        Opuesto.FY = -EV.FY
        Return Opuesto
    End Operator

    Public Shared Operator *(ByVal EV1 As TEuclideanVector, ByVal EV2 As TEuclideanVector) As Double
        Return EV1.FX * EV2.FX + EV1.FY * EV2.FY
    End Operator

#End Region

#Region " Operaciones lineales "

    Public Sub Escalar(ByVal Escala As Double)
        FX = FX * Escala
        FY = FY * Escala
        FZ = FZ * Escala
    End Sub

    Public Sub Rotar(ByVal Psi As Double, ByVal Tita As Double, ByVal Fi As Double)

        Dim RotM As New TMatrizDeRotación
        Dim Px As Double
        Dim Py As Double
        Dim Pz As Double

        Px = FX
        Py = FY
        Pz = FZ

        RotM.Generar(Psi * Math.PI / 180, Tita * Math.PI / 180, Fi * Math.PI / 180)

        Me.FX = Px * RotM.Item(1, 1) + Py * RotM.Item(1, 2) + Pz * RotM.Item(1, 3)
        Me.FY = Px * RotM.Item(2, 1) + Py * RotM.Item(2, 2) + Pz * RotM.Item(2, 3)
        Me.FZ = Px * RotM.Item(3, 1) + Py * RotM.Item(3, 2) + Pz * RotM.Item(3, 3)

    End Sub

    Public Sub Rotar(ByVal RotM As TMatrizDeRotación)

        Dim Px As Double
        Dim Py As Double
        Dim Pz As Double

        Px = FX
        Py = FY
        Pz = FZ

        FX = Px * RotM.Item(1, 1) + Py * RotM.Item(1, 2) + Pz * RotM.Item(1, 3)
        FY = Px * RotM.Item(2, 1) + Py * RotM.Item(2, 2) + Pz * RotM.Item(2, 3)
        FZ = Px * RotM.Item(3, 1) + Py * RotM.Item(3, 2) + Pz * RotM.Item(3, 3)

        RotM = Nothing

    End Sub

    Public Sub Adicionar(ByVal Vector As TEuclideanVector)

        FX = FX + Vector.FX
        FY = FY + Vector.FY
        FZ = FZ + Vector.FZ

    End Sub

    Public Sub Adicionar(ByVal Vector As TEuclideanVector, ByVal Escalar As Double)

        FX = FX + Vector.FX * Escalar
        FY = FY + Vector.FY * Escalar
        FZ = FZ + Vector.FZ * Escalar

    End Sub

    Public Sub Substraer(ByVal Vector As TEuclideanVector)

        FX = FX - Vector.FX
        FY = FY - Vector.FY
        FZ = FZ - Vector.FZ

    End Sub

    Public Sub MultiplicarPorEscalar(ByVal Escalar As Double)
        Me.FX = Me.FX * Escalar
        Me.FY = Me.FY * Escalar
        Me.FZ = Me.FZ * Escalar
    End Sub

    Public Sub ProyectarSobreVector(ByVal Dirección As TEuclideanVector)
        Dim NuevoVector As New TEuclideanVector
        NuevoVector.FX = Dirección.FX * ProductoInterno(Dirección)
        NuevoVector.FY = Dirección.FY * ProductoInterno(Dirección)
        NuevoVector.FY = Dirección.FY * ProductoInterno(Dirección)
        FX = NuevoVector.FX
        FY = NuevoVector.FY
        FZ = NuevoVector.FZ
        NuevoVector = Nothing
    End Sub

    Public Sub ProyectarSobrePlano(ByVal Dirección As TEuclideanVector)
        Dim Proyección As New TEuclideanVector
        Proyección = Me.ObtenerProyección(Dirección)
        Me.Substraer(Proyección)
        Proyección = Nothing
    End Sub

    Public Function ObtenerProyección(ByVal Dirección As TEuclideanVector) As TEuclideanVector
        Dim NuevoVector As New TEuclideanVector
        Dirección.Normalizar()
        NuevoVector.FX = Dirección.FX * Me.ProductoInterno(Dirección)
        NuevoVector.FY = Dirección.FY * Me.ProductoInterno(Dirección)
        NuevoVector.FY = Dirección.FY * Me.ProductoInterno(Dirección)
        Return NuevoVector
        NuevoVector = Nothing
    End Function

#End Region

#Region " Operaciones métricas (euclideas) "

    Public Sub Normalizar()
        Dim Norma As Double = Me.NormaEuclidea
        X = X / Norma
        Y = Y / Norma
        Z = Z / Norma
    End Sub

    Public Function VectorNormalizado() As TEuclideanVector

        Dim Norma As Double = Me.NormaEuclidea
        Dim VectorEscalado As New TEuclideanVector
        If Norma > 0 Then
            VectorEscalado = Algebra.ProductoAbierto(1 / Norma, Me)
        End If
        Return VectorEscalado

    End Function

    Public Function NormaEuclidea() As Double
        Return Math.Sqrt(X ^ 2 + Y ^ 2 + Z ^ 2)
    End Function

    Public Function SqrNormaEuclidea() As Double
        Return X ^ 2 + Y ^ 2 + Z ^ 2
    End Function

    Public Function ProductoInterno(ByVal Punto As TEuclideanVector) As Double
        Return X * Punto.X + Y * Punto.Y + Z * Punto.Z
    End Function

#End Region

#Region " Operaciones vectoriales "

    Public Function ProductoVectorial(ByVal Vector As TEuclideanVector) As TEuclideanVector
        Dim Producto As New TEuclideanVector
        Producto.X = Me.Y * Vector.Z - Me.Z * Vector.Y
        Producto.Y = Me.Z * Vector.X - Me.X * Vector.Z
        Producto.Z = Me.X * Vector.Y - Me.Y * Vector.X
        Return Producto
    End Function

    Public Sub MultiplicarVectorialmente(ByVal Vector As TEuclideanVector)
        Dim Producto As New TEuclideanVector
        Dim Xo As Double = Me.X
        Dim Yo As Double = Me.Y
        Dim Zo As Double = Me.Z
        Me.X = Yo * Vector.Z - Zo * Vector.Y
        Me.Y = Zo * Vector.X - Xo * Vector.Z
        Me.Z = Xo * Vector.Y - Yo * Vector.X
    End Sub


#End Region

End Class

Public Class TEuclideanPoint

    Inherits TEuclideanVector

    Public Sub New()

    End Sub

    Public Sub New(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)
        Me.X = X
        Me.Y = Y
        Me.Z = Z
    End Sub

    Public Sub Transladar(ByVal DX As Double, ByVal DY As Double, ByVal DZ As Double)
        Me.X = Me.X + DX
        Me.Y = Me.Y + DY
        Me.Z = Me.Z + DZ
    End Sub

    Public Sub Transladar(ByVal Vector As TEuclideanVector)
        Me.X = Me.X + Vector.X
        Me.Y = Me.Y + Vector.Y
        Me.Z = Me.Z + Vector.Z
    End Sub

    Public Sub Simetrizar()
        Me.Y = -Me.Y
    End Sub

    Public Function ObtenerDirecciónAlOrigen() As TEuclideanVector

        Dim UnitVector As New TEuclideanVector
        If Me.NormaEuclidea > 0 Then
            UnitVector.X = Me.X / Me.NormaEuclidea
            UnitVector.Y = Me.Y / Me.NormaEuclidea
            UnitVector.Z = Me.Z / Me.NormaEuclidea
        Else
            UnitVector.X = 0
            UnitVector.Y = 0
            UnitVector.Z = 0
        End If

        Return UnitVector

    End Function

    Public Function ObtenerVectorAlPunto(ByVal Punto As TEuclideanPoint) As TEuclideanVector
        Dim Diferencia As New TEuclideanVector
        Diferencia.X = Punto.X - Me.X
        Diferencia.Y = Punto.Y - Me.Y
        Diferencia.Z = Punto.Z - Me.Z
        Return Diferencia
    End Function

    Public Function ObtenerDirecciónAlPunto(ByVal Punto As TEuclideanPoint) As TEuclideanVector
        Dim UnitVector As New TEuclideanVector
        Dim Distancia As Double
        Distancia = ObtenerDistanciaAlPunto(Punto)
        UnitVector.X = (X - Punto.X) / Distancia
        UnitVector.Y = (Y - Punto.Y) / Distancia
        UnitVector.Z = (Z - Punto.Z) / Distancia
        Return UnitVector
    End Function

    Public Function ObtenerDistanciaAlPunto(ByVal Punto As TEuclideanPoint) As Double
        Dim Diferencia As New TEuclideanPoint
        Diferencia.X = X - Punto.X
        Diferencia.Y = Y - Punto.Y
        Diferencia.Z = Z - Punto.Z
        Return Diferencia.NormaEuclidea
    End Function

End Class

Public Structure TValoresLímite
    Public Maximo As Double
    Public Minimo As Double
End Structure
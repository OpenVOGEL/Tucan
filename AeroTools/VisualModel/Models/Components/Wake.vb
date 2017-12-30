'Open VOGEL (www.openvogel.com)
'Open source software for aerodynamics
'Copyright (C) 2016 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.com)

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
Imports SharpGL
Imports AeroTools.CalculationModel.Models.Aero
Imports AeroTools.VisualModel.Models.Components.Basics

Namespace VisualModel.Models.Components

    Public Class TWake

        Public Nombre As String
        Public RutaDeAcceso As String

        Private NodalPoints As New List(Of EVector3)
        Private QuadPanels As New List(Of Panel)
        Private Vortices As New List(Of LatticeSegment) ' Matriz de conexion de vórcies

        Public PasoDeCorte As Integer = 100

        Public ColorDeSuperficie As System.Drawing.Color = System.Drawing.Color.SteelBlue
        Public ColorDeMallado As System.Drawing.Color = System.Drawing.Color.Navy
        Public Transparencia As Double = 1.0#
        Public EspesorDeMallado As Double = 1

        Public MostrarMallado As Boolean = True
        Public MostrarSuperficie As Boolean = False

        Public IncluirEnCalculo As Boolean = False
        Public BloquearContendio As Boolean = True

        Public Escala As Double = 1.0

        Private FGeometriaCargada As Boolean = False

        Private FXYZVI As New List(Of EVector3) ' Velocidad inducida en los puntos nodales

#Region " Propiedades geométricas: "

        Public ReadOnly Property NV As Integer
            Get
                Return Vortices.Count
            End Get
        End Property

        Public ReadOnly Property NN As Integer
            Get
                If Not IsNothing(NodalPoints) Then
                    Return NodalPoints.Count
                Else
                    Return 0
                End If
            End Get
        End Property

        Public ReadOnly Property NP As Integer
            Get
                If Not IsNothing(QuadPanels) Then
                    Return QuadPanels.Count
                Else
                    Return 0
                End If
            End Get
        End Property

#End Region

#Region " Punto nodal: "

        Public ReadOnly Property EvaluarPuntoNodal(ByVal Node As Integer) As EVector3
            'Esta propiedad solo pasa el valor
            Get
                Dim Punto As New EVector3
                If Node <= NN And Node > 0 Then
                    Punto.X = Me.NodalPoints.Item(Node - 1).X
                    Punto.Y = Me.NodalPoints.Item(Node - 1).Y
                    Punto.Z = Me.NodalPoints.Item(Node - 1).Z 'Recordar que el nodo 1 corresponde al indice 0 en la matriz
                End If
                Return Punto
            End Get
        End Property

        Public ReadOnly Property EvaluarVelocidadEnPuntoNodal(ByVal Node As Integer) As EVector3
            'Esta propiedad solo pasa el valor
            Get
                Dim Vector As New EVector3
                If Node <= NN And Node > 0 Then
                    Vector.X = Me.FXYZVI.Item(Node - 1).X
                    Vector.Y = Me.FXYZVI.Item(Node - 1).Y
                    Vector.Z = Me.FXYZVI.Item(Node - 1).Z 'Recordar que el nodo 1 corresponde al indice 0 en la matriz
                End If
                Return Vector
            End Get
        End Property

        Public Property ObtenerPuntoNodal(ByVal Node As Integer) As EVector3
            'Esta propiedad hace referencia completa al macro panel
            Get
                If Node <= NN And Node > 0 Then
                    Return Me.NodalPoints.Item(Node - 1) 'Recordar que el nodo 1 corresponde al indice 0 en la matriz
                Else
                    Return New EVector3
                End If
            End Get
            Set(ByVal value As EVector3)
                If Node <= NN Then
                    Me.NodalPoints.Item(Node - 1) = value
                End If
            End Set
        End Property

        Public Property ObtenerVelocidadEnPuntoNodal(ByVal Node As Integer) As EVector3
            'Esta propiedad hace referencia completa a la velocidad en el punto nodal.
            Get
                If Node <= NN And Node > 0 Then
                    Return Me.FXYZVI.Item(Node - 1)
                Else
                    Return New EVector3
                End If
            End Get
            Set(ByVal Value As EVector3)
                If Node <= NN And Node > 0 Then
                    Me.FXYZVI.Item(Node - 1).Assign(Value)
                End If
            End Set
        End Property

        Public Overloads Sub AgregarPuntoNodal(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)

            Dim PuntoNodal As New EVector3(X, Y, Z)

            Me.NodalPoints.Add(PuntoNodal)
            Me.FXYZVI.Add(New EVector3)

        End Sub

        Public Overloads Sub AgregarPuntoNodal(ByVal Punto As EVector3)

            Me.NodalPoints.Add(Punto)
            Me.FXYZVI.Add(New EVector3)

        End Sub

#End Region

#Region " Quad panels: "

        Public ReadOnly Property EvaluarQuadPanel(ByVal Node As Integer) As Panel
            'Esta propiedad pasa solo el valor del macro panel
            Get
                Dim QuadPanel As New Panel
                If Node <= Me.NP And Node > 0 Then

                    QuadPanel.Assign(QuadPanels.Item(Node - 1))

                End If
                Return QuadPanel
            End Get

        End Property

        Public Property ObtenerQuadPanel(ByVal Node As Integer) As Panel
            'Esta propiedad hace referencia completa al macro panel
            Get
                If Node <= Me.NP And Node > 0 Then
                    Return QuadPanels.Item(Node - 1)
                Else
                    Return New Panel
                End If
            End Get
            Set(ByVal value As Panel)
                If Node <= Me.NP And Node > 0 Then
                    QuadPanels.Item(Node - 1) = value
                End If
            End Set
        End Property

        Public Function AgregarPanel(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal N4 As Integer) As Integer

            Dim Panel As New Panel(N1, N2, N3, N4)
            Me.QuadPanels.Add(Panel)

            Return NP

        End Function

        Public Function AgregarPanel(ByVal Panel As Panel) As Integer

            Me.QuadPanels.Add(Panel)
            Return NP

        End Function

#End Region

#Region " Modelo 3D y vórtices: "

        Public Function GenerarMallado() As Boolean

            Try

                Vortices.Clear()

                ' Arma la matriz de conexiones de vortices:

                Dim N1 As Integer
                Dim N2 As Integer
                Dim m As Integer
                Dim Esta As Boolean
                Dim p As Integer = 1

                Dim Vortex As New LatticeSegment

                Vortex.N1 = QuadPanels(0).N1
                Vortex.N2 = QuadPanels(0).N2

                Vortices.Add(Vortex)

                For i = 1 To NP

                    For k = 1 To 4

                        Select Case k
                            Case 1
                                N1 = QuadPanels(i - 1).N1
                                N2 = QuadPanels(i - 1).N2
                            Case 2
                                N1 = QuadPanels(i - 1).N2
                                N2 = QuadPanels(i - 1).N3
                            Case 3
                                N1 = QuadPanels(i - 1).N3
                                N2 = QuadPanels(i - 1).N4
                            Case 4
                                N1 = QuadPanels(i - 1).N4
                                N2 = QuadPanels(i - 1).N1
                        End Select

                        Esta = False

                        For m = 1 To p

                            If Vortices.Item(m - 1).N1 = N1 And Vortices.Item(m - 1).N2 = N2 Then

                                Esta = True

                            ElseIf Vortices.Item(m - 1).N1 = N2 And Vortices.Item(m - 1).N2 = N1 Then

                                Esta = True

                            End If

                        Next

                        If Esta = False Then

                            p = p + 1

                            Vortices.Add(New LatticeSegment(N1, N2))

                        End If

                    Next

                Next

                Return True

            Catch ex As Exception

                Me.FGeometriaCargada = False

                Me.ReDimensionarEnCero()

                MsgBox("Imposible de generar el indexado de vortices de estela.")

                Return False

            End Try

        End Function

        Public Function ObtenerVortice(ByVal VortexNumber As Integer) As LatticeSegment
            Return Vortices.Item(VortexNumber - 1)
        End Function

        Public Sub ActualizarModelo3D(ByRef gl As OpenGL)

            'Version para OpenGL

            Dim i As Integer

            ' Agrega los triangulos:

            Dim Nodo As EVector3

            If Me.MostrarSuperficie Then

                gl.Begin(OpenGL.GL_TRIANGLES)

                For i = 1 To NP

                    ' Primer triángulo:

                    Nodo = Me.NodalPoints.Item(Me.ObtenerQuadPanel(i).N1 - 1)
                    gl.Color(1.0 * Me.ColorDeSuperficie.R, 1.0 * Me.ColorDeSuperficie.G, 1.0 * Me.ColorDeSuperficie.B)
                    gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                    Nodo = Me.NodalPoints.Item(Me.ObtenerQuadPanel(i).N2 - 1)
                    gl.Color(1.0 * Me.ColorDeSuperficie.R, 1.0 * Me.ColorDeSuperficie.G, 1.0 * Me.ColorDeSuperficie.B)
                    gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                    Nodo = Me.NodalPoints.Item(Me.ObtenerQuadPanel(i).N3 - 1)
                    gl.Color(1.0 * Me.ColorDeSuperficie.R, 1.0 * Me.ColorDeSuperficie.G, 1.0 * Me.ColorDeSuperficie.B)
                    gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                    Nodo = Me.NodalPoints.Item(Me.ObtenerQuadPanel(i).N3 - 1)
                    gl.Color(1.0 * Me.ColorDeSuperficie.R, 1.0 * Me.ColorDeSuperficie.G, 1.0 * Me.ColorDeSuperficie.B)
                    gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                    Nodo = Me.NodalPoints.Item(Me.ObtenerQuadPanel(i).N4 - 1)
                    gl.Color(1.0 * Me.ColorDeSuperficie.R, 1.0 * Me.ColorDeSuperficie.G, 1.0 * Me.ColorDeSuperficie.B)
                    gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                    Nodo = Me.NodalPoints.Item(Me.ObtenerQuadPanel(i).N1 - 1)
                    gl.Color(1.0 * Me.ColorDeSuperficie.R, 1.0 * Me.ColorDeSuperficie.G, 1.0 * Me.ColorDeSuperficie.B)
                    gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                Next

                gl.End()

            End If

            If False Then

                gl.Begin(OpenGL.GL_POINTS)
                gl.PointSize(2.0F)
                gl.Color(0.0F, 0.0F, 0.0F)

                For i = 1 To NN

                    Nodo = Me.ObtenerPuntoNodal(i)
                    gl.Vertex(Nodo.X, Nodo.Y, Nodo.Z)

                Next

                gl.End()

            End If

            ' Genera el mallado:

            If Me.MostrarMallado Then

                gl.LineWidth(0.2F)
                gl.Begin(OpenGL.GL_LINES)

                Dim Nodo1 As New EVector3
                Dim Nodo2 As New EVector3

                gl.Color(Me.ColorDeMallado.R / 255, 1.0 * Me.ColorDeMallado.G / 255, 1.0 * Me.ColorDeMallado.B / 255)

                For i = 1 To NV

                    Nodo1 = Me.ObtenerPuntoNodal(Me.ObtenerVortice(i).N1)
                    Nodo2 = Me.ObtenerPuntoNodal(Me.ObtenerVortice(i).N2)

                    gl.Vertex(Nodo1.X, Nodo1.Y, Nodo1.Z)
                    gl.Vertex(Nodo2.X, Nodo2.Y, Nodo2.Z)

                Next

                gl.End()

            End If

        End Sub

#End Region

#Region " I/O: "

        Public Function CargarDesdeArchivo() As Boolean

            Dim PistaDelError As String = ""

            Try

                Dim Line As String

                FileOpen(25, Me.RutaDeAcceso, OpenMode.Input, OpenAccess.Read)

                Line = LineInput(25)
                Me.Nombre = Line

                Line = LineInput(25)

                Line = LineInput(25)
                Dim NumeroDeNodos As Integer = CInt(Microsoft.VisualBasic.Right(Line, 5))

                Line = LineInput(25)
                Dim NumeroDePaneles As Integer = CInt(Microsoft.VisualBasic.Right(Line, 5))

                PistaDelError = "Puede que el formato sea incorrecto."

                Do Until Trim(Line) = "## MATRICES"
                    Line = LineInput(25)
                Loop

                Me.NodalPoints.Clear()
                Me.QuadPanels.Clear()

                For i = 1 To NumeroDeNodos ' Comienza a leer la matriz de coordenadas

                    PistaDelError = "Puede que exista inconcistencia en la matriz de nodos."

                    Line = LineInput(25)
                    Me.AgregarPuntoNodal(CDbl(Left(Line, 13)), CDbl(Mid(Line, 14, 12)), CDbl(Right(Line, 13)))

                Next

                For i = 1 To NumeroDePaneles ' Comienza a leer la matriz de conectividad

                    PistaDelError = "Puede que exista inconcistencia en la matriz de conectividades."

                    Line = LineInput(25)
                    Me.AgregarPanel(CInt(Microsoft.VisualBasic.Left(Line, 5)), _
                                                                              CInt(Microsoft.VisualBasic.Mid(Line, 6, 5)), _
                                                                              CInt(Microsoft.VisualBasic.Mid(Line, 11, 5)), _
                                                                              CInt(Microsoft.VisualBasic.Right(Line, 4)))
                Next

                FileClose(25)

                Me.FGeometriaCargada = True

                Dim ErrorDeTamaño As Boolean = Not ((NumeroDeNodos = NN) And (NumeroDePaneles = NP) Or NN >= 0 Or NP >= 0)

                If ErrorDeTamaño Then

                    MsgBox("Error en el formato del archivo. " & PistaDelError)

                    ReDimensionarEnCero()

                    Return False

                End If

                Me.GenerarMallado()

                MsgBox("Geometria cargada correctamente.", MsgBoxStyle.Information)

                Return True

            Catch ex1 As Exception

                Me.FGeometriaCargada = False

                MsgBox("Error en el formato del archivo. " & PistaDelError)

                Me.ReDimensionarEnCero()

                Try
                    FileClose(25)
                Catch

                End Try

                Return False

            End Try

        End Function

        Public ReadOnly Property GeometriaCargada As Boolean
            Get
                If FGeometriaCargada And Not IsNothing(NodalPoints) And Not IsNothing(QuadPanels) Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Sub ReDimensionarEnCero()

            NodalPoints.Clear()
            FXYZVI.Clear()
            QuadPanels.Clear()
            Vortices.Clear()

        End Sub

        Public Sub LoadFromTLattice(ByVal Lattice As Lattice)

            NodalPoints.Clear()
            QuadPanels.Clear()
            Vortices.Clear()

            For i = 0 To Lattice.Nodes.Count - 1
                AgregarPuntoNodal(Lattice.Nodes(i).Position.X, Lattice.Nodes(i).Position.Y, Lattice.Nodes(i).Position.Z)
            Next

            For i = 0 To Lattice.VortexRings.Count - 1
                AgregarPanel(Lattice.VortexRings(i).Node(1).IndexL + 1, Lattice.VortexRings(i).Node(2).IndexL + 1, Lattice.VortexRings(i).Node(3).IndexL + 1, Lattice.VortexRings(i).Node(4).IndexL + 1)
            Next

        End Sub

#End Region

#Region " UVLM: "

        Public Sub CalcularPuntosDeControlYVectoresNormales()

            Dim Nodo1 As New EVector3
            Dim Nodo2 As New EVector3
            Dim Nodo3 As New EVector3
            Dim Nodo4 As New EVector3

            Dim Diagonal1 As New EVector3
            Dim Diagonal2 As New EVector3

            For i = 1 To Me.NP

                Nodo1 = Me.ObtenerPuntoNodal(Me.ObtenerQuadPanel(i).N1)
                Nodo2 = Me.ObtenerPuntoNodal(Me.ObtenerQuadPanel(i).N2)
                Nodo3 = Me.ObtenerPuntoNodal(Me.ObtenerQuadPanel(i).N3)
                Nodo4 = Me.ObtenerPuntoNodal(Me.ObtenerQuadPanel(i).N4)

                Me.ObtenerQuadPanel(i).ControlPoint.X = 0.25 * (Nodo1.X + Nodo2.X + Nodo3.X + Nodo4.X)
                Me.ObtenerQuadPanel(i).ControlPoint.Y = 0.25 * (Nodo1.Y + Nodo2.Y + Nodo3.Y + Nodo4.Y)
                Me.ObtenerQuadPanel(i).ControlPoint.Z = 0.25 * (Nodo1.Z + Nodo2.Z + Nodo3.Z + Nodo4.Z)

                Diagonal1.X = Nodo2.X - Nodo4.X
                Diagonal1.Y = Nodo2.Y - Nodo4.Y
                Diagonal1.Z = Nodo2.Z - Nodo4.Z

                Diagonal2.X = Nodo3.X - Nodo1.X
                Diagonal2.Y = Nodo3.Y - Nodo1.Y
                Diagonal2.Z = Nodo3.Z - Nodo1.Z

                Me.ObtenerQuadPanel(i).NormalVector = Algebra.VectorProduct(Diagonal1, Diagonal2).NormalizedDirection

            Next

        End Sub

#End Region

    End Class

End Namespace
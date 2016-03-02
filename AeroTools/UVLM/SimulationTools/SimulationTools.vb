'Copyright (C) 2016 Guillermo Hazebrouck

Imports SharpGL
Imports MathTools.Algebra.EuclideanSpace
Imports System.Xml
Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting
Imports AeroTools.VisualModel.IO

Namespace UVLM.SimulationTools

    Public Class VelocityPlane

        Private XYZVP(0) As EVector3
        Private XYZVI(0) As EVector3

        Public Origin As New EVector3

        Private Direccion_1 As New EVector3
        Private Direccion_2 As New EVector3

        Private Esquina1 As New EVector3
        Private Esquina2 As New EVector3
        Private Esquina3 As New EVector3
        Private Esquina4 As New EVector3

        Public VectorNormal As New EVector3

        Public Extension_1 As Double = 1.0F
        Public Extension_2 As Double = 1.0F

        Public _Psi As Double = 0.5 * Math.PI
        Public _Tita As Double = 0.0#

        Private _NodesInDirection_1 As Integer = 10
        Private _NodesInDirection_2 As Integer = 10

        Public ColorDeNodos As System.Drawing.Color = Drawing.Color.DarkGray
        Public ColorDeVectores As System.Drawing.Color = Drawing.Color.LightBlue
        Public ColorDePlano As System.Drawing.Color = Color.LightGray

        Public Scale As Double = 0.1
        Public VectorThickness As Double = 1.0
        Public NodeSize As Double = 3

        Public Visible As Boolean = True
        Public InducedVelocity As Boolean = False ' Defines whether the total or induced velocity is represented.

        Public TreftSegments As New List(Of UVLM.Solver.UVLMSolver.TrefftzSegment)

        Public Property Psi As Double
            Set(ByVal value As Double)
                _Psi = value
                VectorNormal = Direccion_1.VectorProduct(Direccion_2)
            End Set
            Get
                Return _Psi
            End Get
        End Property

        Public Property Tita As Double
            Set(ByVal value As Double)
                _Tita = value
                VectorNormal = Direccion_1.VectorProduct(Direccion_2)
            End Set
            Get
                Return _Tita
            End Get
        End Property

        Public ReadOnly Property ObtenerNodo(ByVal Nodo As Integer) As EVector3
            Get
                If 0 < Nodo <= XYZVP.Length Then
                    Return XYZVP(Nodo)
                Else
                    Return New EVector3
                End If
            End Get
        End Property

        Public Property ObtenerVelocidadInducida(ByVal Nodo As Integer) As EVector3
            Get
                If 0 < Nodo <= XYZVI.Length Then
                    Return XYZVI(Nodo)
                Else
                    Return New EVector3
                End If
            End Get
            Set(ByVal value As EVector3)
                If 0 < Nodo <= XYZVI.Length Then
                    XYZVI(Nodo) = value
                End If
            End Set
        End Property

        Public Property NodesInDirection_1 As Integer
            Set(ByVal value As Integer)
                If value >= 2 Then
                    _NodesInDirection_1 = value
                    GenerarMallado()
                End If
            End Set
            Get
                Return _NodesInDirection_1
            End Get
        End Property

        Public Property NodesInDirection_2 As Integer
            Set(ByVal value As Integer)
                If value >= 2 Then
                    _NodesInDirection_2 = value
                    GenerarMallado()
                End If
            End Set
            Get
                Return _NodesInDirection_2
            End Get
        End Property

        Public ReadOnly Property NumberOfNodes As Integer
            Get
                If Not IsNothing(XYZVP) Then
                    Return XYZVP.Length - 1
                Else
                    Return 0
                End If
            End Get
        End Property

        Private Sub AgregarPuntoDeControl(ByVal X As Double, ByVal Y As Double, ByVal Z As Double)

            Dim Dimension As Integer = XYZVP.Length

            ReDim Preserve Me.XYZVP(Dimension)
            ReDim Preserve Me.XYZVI(Dimension)

            Me.XYZVI(Dimension) = New EVector3

            Me.XYZVP(Dimension) = New EVector3
            Me.XYZVP(Dimension).X = X
            Me.XYZVP(Dimension).Y = Y
            Me.XYZVP(Dimension).Z = Z

        End Sub

        Private Sub AgregarPuntoDeControl(ByVal Punto As EVector3)

            Dim Dimension As Integer = XYZVP.Length

            ReDim Preserve Me.XYZVP(Dimension)
            ReDim Preserve Me.XYZVI(Dimension)

            Me.XYZVI(Dimension) = New EVector3

            Me.XYZVP(Dimension) = New EVector3
            Me.XYZVP(Dimension).X = Punto.X
            Me.XYZVP(Dimension).Y = Punto.Y
            Me.XYZVP(Dimension).Z = Punto.Z

        End Sub

        Public Sub GenerarMallado()

            ReDim Preserve Me.XYZVP(0)

            Direccion_1.X = Math.Cos(Psi)
            Direccion_1.Y = Math.Sin(Psi)
            Direccion_1.Z = 0.0#

            Direccion_2.X = -Math.Sin(Tita) * Math.Sin(Psi)
            Direccion_2.Y = Math.Sin(Tita) * Math.Cos(Psi)
            Direccion_2.Z = Math.Cos(Tita)

            Dim Coordenada1 As Double
            Dim Coordenada2 As Double
            Dim Punto As New EVector3

            Direccion_1.Normalize()
            Direccion_2.Normalize()

            For i = 1 To NodesInDirection_1

                Coordenada1 = Extension_1 * ((i - 1) / (NodesInDirection_1 - 1) - 0.5) 'De 0.5 a -0.5

                For j = 1 To NodesInDirection_2

                    Coordenada2 = Extension_2 * ((j - 1) / (NodesInDirection_2 - 1) - 0.5) 'De 0.5 a -0.5

                    Punto.X = Coordenada1 * Direccion_1.X + Coordenada2 * Direccion_2.X + Origin.X
                    Punto.Y = Coordenada1 * Direccion_1.Y + Coordenada2 * Direccion_2.Y + Origin.Y
                    Punto.Z = Coordenada1 * Direccion_1.Z + Coordenada2 * Direccion_2.Z + Origin.Z

                    Me.AgregarPuntoDeControl(Punto)

                Next

            Next

            Esquina1.X = 0.5 * Extension_1 * Direccion_1.X + 0.5 * Extension_2 * Direccion_2.X + Origin.X
            Esquina1.Y = 0.5 * Extension_1 * Direccion_1.Y + 0.5 * Extension_2 * Direccion_2.Y + Origin.Y
            Esquina1.Z = 0.5 * Extension_1 * Direccion_1.Z + 0.5 * Extension_2 * Direccion_2.Z + Origin.Z

            Esquina2.X = 0.5 * Extension_1 * Direccion_1.X - 0.5 * Extension_2 * Direccion_2.X + Origin.X
            Esquina2.Y = 0.5 * Extension_1 * Direccion_1.Y - 0.5 * Extension_2 * Direccion_2.Y + Origin.Y
            Esquina2.Z = 0.5 * Extension_1 * Direccion_1.Z - 0.5 * Extension_2 * Direccion_2.Z + Origin.Z

            Esquina3.X = -0.5 * Extension_1 * Direccion_1.X - 0.5 * Extension_2 * Direccion_2.X + Origin.X
            Esquina3.Y = -0.5 * Extension_1 * Direccion_1.Y - 0.5 * Extension_2 * Direccion_2.Y + Origin.Y
            Esquina3.Z = -0.5 * Extension_1 * Direccion_1.Z - 0.5 * Extension_2 * Direccion_2.Z + Origin.Z

            Esquina4.X = -0.5 * Extension_1 * Direccion_1.X + 0.5 * Extension_2 * Direccion_2.X + Origin.X
            Esquina4.Y = -0.5 * Extension_1 * Direccion_1.Y + 0.5 * Extension_2 * Direccion_2.Y + Origin.Y
            Esquina4.Z = -0.5 * Extension_1 * Direccion_1.Z + 0.5 * Extension_2 * Direccion_2.Z + Origin.Z

            VectorNormal = Direccion_1.VectorProduct(Direccion_2)

        End Sub

        Public Sub ActualizarModelo3D(ByRef gl As OpenGL)

            If Me.Visible Then

                gl.PointSize(Me.NodeSize)

                gl.Begin(OpenGL.GL_POINTS)
                gl.Color(ColorDeNodos.R / 255, ColorDeNodos.G / 255, ColorDeNodos.B / 255, 1)

                For i = 1 To NodesInDirection_1 * NodesInDirection_2
                    gl.Vertex(Me.ObtenerNodo(i).X, Me.ObtenerNodo(i).Y, Me.ObtenerNodo(i).Z)
                Next

                gl.End()

                gl.LineWidth(Me.VectorThickness)

                gl.Begin(OpenGL.GL_LINES)
                gl.Color(ColorDeVectores.R / 255, ColorDeVectores.G / 255, ColorDeVectores.B / 255, 1)

                For i = 1 To NodesInDirection_1 * NodesInDirection_2
                    gl.Vertex(Me.ObtenerNodo(i).X, Me.ObtenerNodo(i).Y, Me.ObtenerNodo(i).Z)
                    gl.Vertex(Me.ObtenerNodo(i).X + Me.Scale * Me.ObtenerVelocidadInducida(i).X, Me.ObtenerNodo(i).Y + Me.Scale * Me.ObtenerVelocidadInducida(i).Y, Me.ObtenerNodo(i).Z + Me.Scale * Me.ObtenerVelocidadInducida(i).Z)
                Next

                gl.End()

                gl.Color(ColorDePlano.R / 255, ColorDePlano.G / 255, ColorDePlano.B / 255, 0.3)

                gl.Begin(OpenGL.GL_QUADS)

                gl.Vertex(Esquina1.X, Esquina1.Y, Esquina1.Z)
                gl.Vertex(Esquina2.X, Esquina2.Y, Esquina2.Z)
                gl.Vertex(Esquina3.X, Esquina3.Y, Esquina3.Z)
                gl.Vertex(Esquina4.X, Esquina4.Y, Esquina4.Z)

                gl.End()

                If TreftSegments.Count > 0 Then

                    gl.Begin(OpenGL.GL_LINES)
                    gl.Color(ColorDeVectores.R / 255, ColorDeVectores.G / 255, ColorDeVectores.B / 255, 1)

                    For i = 0 To TreftSegments.Count - 1

                        gl.Vertex(TreftSegments(i).Point1.X, TreftSegments(i).Point1.Y, TreftSegments(i).Point1.Z)
                        gl.Vertex(TreftSegments(i).Point2.X, TreftSegments(i).Point2.Y, TreftSegments(i).Point2.Z)

                        gl.Vertex(TreftSegments(i).Point1.X, TreftSegments(i).Point1.Y, TreftSegments(i).Point1.Z)
                        gl.Vertex(TreftSegments(i).Point1.X + Scale * TreftSegments(i).Velocity.X, TreftSegments(i).Point1.Y + Scale * TreftSegments(i).Velocity.Y, TreftSegments(i).Point1.Z + Scale * TreftSegments(i).Velocity.Z)

                    Next

                    gl.End()

                End If

            End If

        End Sub

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Origin")
            writer.WriteAttributeString("X", String.Format("{0}", Origin.X))
            writer.WriteAttributeString("Y", String.Format("{0}", Origin.Y))
            writer.WriteAttributeString("Z", String.Format("{0}", Origin.Z))
            writer.WriteEndElement()

            writer.WriteStartElement("Exstension")
            writer.WriteAttributeString("Extension1", String.Format("{0}", Extension_1))
            writer.WriteAttributeString("Extension2", String.Format("{0}", Extension_2))
            writer.WriteAttributeString("Nodes1", String.Format("{0}", _NodesInDirection_1))
            writer.WriteAttributeString("Nodes2", String.Format("{0}", _NodesInDirection_2))
            writer.WriteEndElement()

            writer.WriteStartElement("Orientation")
            writer.WriteAttributeString("Psi", String.Format("{0}", Psi))
            writer.WriteAttributeString("Tita", String.Format("{0}", Tita))
            writer.WriteEndElement()

            writer.WriteStartElement("VisualProperties")
            writer.WriteAttributeString("Scale", String.Format("{0}", Scale))
            writer.WriteAttributeString("VectorThickess", String.Format("{0}", VectorThickness))
            writer.WriteAttributeString("NodeSize", String.Format("{0}", NodeSize))

            writer.WriteAttributeString("NodeColorR", String.Format("{0}", ColorDeNodos.R))
            writer.WriteAttributeString("NodeColorG", String.Format("{0}", ColorDeNodos.G))
            writer.WriteAttributeString("NodeColorB", String.Format("{0}", ColorDeNodos.B))

            writer.WriteAttributeString("VectorColorR", String.Format("{0}", ColorDeVectores.R))
            writer.WriteAttributeString("VectorColorG", String.Format("{0}", ColorDeVectores.G))
            writer.WriteAttributeString("VectorColorB", String.Format("{0}", ColorDeVectores.B))

            writer.WriteAttributeString("Show", String.Format("{0}", CInt(Visible)))
            writer.WriteAttributeString("InducedVelocity", String.Format("{0}", CInt(InducedVelocity)))
            writer.WriteEndElement()

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "Origin"

                            Origin.X = CDbl(reader.GetAttribute("X"))
                            Origin.Y = CDbl(reader.GetAttribute("Y"))
                            Origin.Z = CDbl(reader.GetAttribute("Z"))

                        Case "Extension"

                            Extension_1 = CDbl(reader.GetAttribute("Extension1"))
                            Extension_2 = CDbl(reader.GetAttribute("Extension2"))
                            NodesInDirection_1 = CInt(reader.GetAttribute("Nodes1"))
                            NodesInDirection_2 = CInt(reader.GetAttribute("Nodes2"))

                        Case "Orientation"

                            Psi = CDbl(reader.GetAttribute("Psi"))
                            Tita = CDbl(reader.GetAttribute("Tita"))

                        Case "VisualProperties"

                            Scale = CDbl(reader.GetAttribute("Escale"))
                            VectorThickness = CDbl(reader.GetAttribute("VectorThickess"))
                            NodeSize = CDbl(reader.GetAttribute("NodeSize"))

                            Dim R As Integer = CDbl(reader.GetAttribute("NodeColorR"))
                            Dim G As Integer = CDbl(reader.GetAttribute("NodeColorG"))
                            Dim B As Integer = CDbl(reader.GetAttribute("NodeColorB"))

                            ColorDeNodos = Color.FromArgb(R, G, B)

                            R = CDbl(reader.GetAttribute("VectorColorR"))
                            G = CDbl(reader.GetAttribute("VectorColorG"))
                            B = CDbl(reader.GetAttribute("VectorColorB"))

                            ColorDeVectores = Color.FromArgb(R, G, B)

                            Visible = CBool(CInt(reader.GetAttribute("Show")))
                            InducedVelocity = CBool(CInt(reader.GetAttribute("InducedVelocity")))

                    End Select

                End If

            End While

        End Sub

    End Class

    Public Class EmissionPoints

        Private XYZEP As New LinkedList(Of EVector3)

    End Class

#Region " Velocity profile "

    Public Class Perturbation

        Private _Start As Integer
        Private _ElapsedTime As Integer
        Private _PeakInstant As Integer

        Public Property Intensity As Double
        Public Property FinalDrop As Double

        Public Sub New()
            _Start = 1
            _PeakInstant = 2
            _ElapsedTime = 3
        End Sub

        Public Property Start As Integer
            Set(ByVal value As Integer)
                _Start = value
                If _PeakInstant <= _Start Then
                    _PeakInstant = _Start + 1
                End If
                If _Start + _ElapsedTime <= _PeakInstant Then
                    _ElapsedTime = _PeakInstant - _Start + 1
                End If
            End Set
            Get
                Return _Start
            End Get
        End Property

        Public Property ElapsedTime As Integer
            Set(ByVal value As Integer)
                _ElapsedTime = value
                If _Start + _ElapsedTime <= _PeakInstant Then
                    _ElapsedTime = _PeakInstant - _Start + 1
                End If
            End Set
            Get
                Return _ElapsedTime
            End Get
        End Property

        Public Property PeakInstant As Integer
            Set(ByVal value As Integer)
                If value > _Start Then
                    _PeakInstant = value
                Else
                    _PeakInstant = _Start + 1
                End If
                If _Start + _ElapsedTime <= _PeakInstant Then
                    _ElapsedTime = _PeakInstant - _Start + 1
                End If
            End Set
            Get
                Return _PeakInstant
            End Get
        End Property

        Public Sub Assign(ByRef Perturbacion As Perturbation)

            _Start = Perturbacion.Start
            _PeakInstant = Perturbacion.PeakInstant
            _ElapsedTime = Perturbacion.ElapsedTime
            Intensity = Perturbacion.Intensity
            FinalDrop = Perturbacion.FinalDrop

        End Sub

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Time")
            writer.WriteAttributeString("Start", String.Format("{0}", _Start))
            writer.WriteAttributeString("TimeSpan", String.Format("{0}", _ElapsedTime))
            writer.WriteAttributeString("PeakAt", String.Format("{0}", _PeakInstant))
            writer.WriteEndElement()

            writer.WriteStartElement("Amplitude")
            writer.WriteAttributeString("Intensity", String.Format("{0}", Intensity))
            writer.WriteAttributeString("FinalDrop", String.Format("{0}", FinalDrop))
            writer.WriteEndElement()

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "Time"
                            _Start = CInt(reader.GetAttribute("Start"))
                            _ElapsedTime = CInt(reader.GetAttribute("TimeSpan"))
                            _PeakInstant = CInt(reader.GetAttribute("PeakAt"))

                        Case "Amplitude"
                            Intensity = CDbl(reader.GetAttribute("Intensity"))
                            FinalDrop = CDbl(reader.GetAttribute("FinalDrop"))

                    End Select

                End If

            End While





        End Sub

    End Class

    Public Enum Axes As Integer
        X = 0
        Y = 1
        Z = 2
    End Enum

    Public Enum ProfileType As Integer
        Impulsivo = 0
        Perturbado = 1
    End Enum

    Public Class UnsteadyVelocity

        ''' <summary>
        ''' Base velocity.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BaseVelocity As New EVector3(0, 0, 0)

        Private _Velocity As New List(Of EVector3)
        Private _Intensity As New List(Of EVector3)

        ''' <summary>
        ''' Profile type.
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Type As ProfileType = ProfileType.Impulsivo

        Private _Perturbation(2) As Perturbation ' Representa una perturbación en cada eje de coordenadas.

        Public Sub New()
            _Perturbation(Axes.X) = New Perturbation
            _Perturbation(Axes.Y) = New Perturbation
            _Perturbation(Axes.Z) = New Perturbation
        End Sub

        ''' <summary>
        ''' 'Gets the velocity at a given time step
        ''' </summary>
        ''' <param name="Instant">Time stape (1-based)</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Velocity(ByVal Instant As Integer) As EVector3
            Get
                Return _Velocity(Instant - 1)
            End Get
        End Property

        Public Property Perturbation(ByVal Eje As Axes) As Perturbation
            Set(ByVal value As Perturbation)
                _Perturbation(Eje) = value
            End Set
            Get
                Return _Perturbation(Eje)
            End Get
        End Property

        ''' <summary>
        ''' Number of steps.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property nSteps As Integer
            Get
                Return _Velocity.Count
            End Get
        End Property

        Public Sub GeneratePerturbation(ByVal NumberOfSteps As Integer)

            _Velocity.Clear()
            _Intensity.Clear()

            If (Type = ProfileType.Perturbado) Then

                _Velocity.Capacity = NumberOfSteps - 1 ' El cero cuenta como primer indice
                _Intensity.Capacity = NumberOfSteps - 1

                Dim InstanteFinalX As Integer = Perturbation(Axes.X).Start + Perturbation(Axes.X).ElapsedTime
                Dim InstanteFinalY As Integer = Perturbation(Axes.Y).Start + Perturbation(Axes.Y).ElapsedTime
                Dim InstanteFinalZ As Integer = Perturbation(Axes.Z).Start + Perturbation(Axes.Z).ElapsedTime
                Dim Tita As Double
                Dim Parametro As Double
                Dim LIntensity As Double
                Dim LVelocity As Double = BaseVelocity.EuclideanNorm

                For i = 0 To NumberOfSteps - 1

                    _Velocity.Add(New EVector3)
                    _Intensity.Add(New EVector3)

                    LIntensity = 0
                    If i >= Perturbation(Axes.X).Start And i <= Perturbation(Axes.X).PeakInstant Then
                        Parametro = (i - Perturbation(Axes.X).Start) / (Perturbation(Axes.X).PeakInstant - Perturbation(Axes.X).Start)
                        Tita = Math.PI * (Parametro - 0.5)
                        LIntensity = Perturbation(Axes.X).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    ElseIf i > Perturbation(Axes.X).PeakInstant And i <= InstanteFinalX Then
                        Parametro = (i - Perturbation(Axes.X).PeakInstant) / (InstanteFinalX - Perturbation(Axes.X).PeakInstant)
                        Tita = Math.PI * (0.5 - Parametro)
                        LIntensity = Perturbation(Axes.X).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    End If

                    _Velocity(i).X = BaseVelocity.X + LVelocity * LIntensity
                    _Intensity(i).X = LIntensity

                    LIntensity = 0
                    If i >= Perturbation(Axes.Y).Start And i <= Perturbation(Axes.Y).PeakInstant Then
                        Parametro = (i - Perturbation(Axes.Y).Start) / (Perturbation(Axes.Y).PeakInstant - Perturbation(Axes.Y).Start)
                        Tita = Math.PI * (Parametro - 0.5)
                        LIntensity = Perturbation(Axes.Y).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    ElseIf i > Perturbation(Axes.Y).PeakInstant And i <= InstanteFinalY Then
                        Parametro = (i - Perturbation(Axes.Y).PeakInstant) / (InstanteFinalY - Perturbation(Axes.Y).PeakInstant)
                        Tita = Math.PI * (0.5 - Parametro)
                        LIntensity = Perturbation(Axes.Y).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    End If

                    _Velocity(i).Y = BaseVelocity.Y + LVelocity * LIntensity
                    _Intensity(i).Y = LIntensity

                    LIntensity = 0
                    If i >= Perturbation(Axes.Z).Start And i <= Perturbation(Axes.Z).PeakInstant Then
                        Parametro = (i - Perturbation(Axes.Z).Start) / (Perturbation(Axes.Z).PeakInstant - Perturbation(Axes.Z).Start)
                        Tita = Math.PI * (Parametro - 0.5)
                        LIntensity = Perturbation(Axes.Z).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    ElseIf i > Perturbation(Axes.Z).PeakInstant And i <= InstanteFinalZ Then
                        Parametro = (i - Perturbation(Axes.Z).PeakInstant) / (InstanteFinalZ - Perturbation(Axes.Z).PeakInstant)
                        Tita = Math.PI * (0.5 - Parametro)
                        LIntensity = Perturbation(Axes.Z).Intensity * 0.5 * (1 + Math.Sin(Tita))
                    End If

                    _Velocity(i).Z = BaseVelocity.Z + LVelocity * LIntensity
                    _Intensity(i).Z = LIntensity

                Next

            End If

            If (Type = ProfileType.Impulsivo) Then

                _Velocity.Capacity = NumberOfSteps - 1
                _Intensity.Capacity = NumberOfSteps - 1

                For i = 0 To NumberOfSteps - 1
                    _Velocity.Add(New EVector3(BaseVelocity))
                    _Intensity.Add(New EVector3)
                Next

            End If

        End Sub

        Public Sub PlotOnChart(ByRef Graph As Chart)

            Graph.Titles.Item(0).Text = "Unsteady velocity"
            Graph.ChartAreas.Item(0).AxisY.Title = "V(t)/Vo"
            Graph.ChartAreas.Item(0).AxisX.Title = "t/Δt"

            If Graph.Series("Vx").Points.Count >= 0 Then
                Graph.Series("Vx").Points.Clear()
            End If

            If Graph.Series("Vy").Points.Count >= 0 Then
                Graph.Series("Vy").Points.Clear()
            End If

            If Graph.Series("Vz").Points.Count >= 0 Then
                Graph.Series("Vz").Points.Clear()
            End If

            For i = 0 To Me.nSteps - 1

                Graph.Series("Vx").Points.AddXY(i + 1, Me._Intensity(i).X)
                Graph.Series("Vy").Points.AddXY(i + 1, Me._Intensity(i).Y)
                Graph.Series("Vz").Points.AddXY(i + 1, Me._Intensity(i).Z)

            Next

        End Sub

        Public Sub Assign(ByRef Profile As UnsteadyVelocity)

            Type = Profile.Type
            BaseVelocity.Assign(Profile.BaseVelocity)
            Perturbation(Axes.X).Assign(Profile.Perturbation(Axes.X))
            Perturbation(Axes.Y).Assign(Profile.Perturbation(Axes.Y))
            Perturbation(Axes.Z).Assign(Profile.Perturbation(Axes.Z))

        End Sub

        Public Sub SaveToXML(ByRef writer As XmlWriter)

            writer.WriteStartElement("Properties")
            writer.WriteAttributeString("Type", CInt(Type))
            writer.WriteEndElement()

            writer.WriteStartElement("X")
            Perturbation(Axes.X).SaveToXML(writer)
            writer.WriteEndElement()

            writer.WriteStartElement("Y")
            Perturbation(Axes.Y).SaveToXML(writer)
            writer.WriteEndElement()

            writer.WriteStartElement("Z")
            Perturbation(Axes.Z).SaveToXML(writer)
            writer.WriteEndElement()

        End Sub

        Public Sub ReadFromXML(ByRef reader As XmlReader)

            While reader.Read

                If reader.NodeType = XmlNodeType.Element Then

                    Select Case reader.Name

                        Case "X"
                            Perturbation(Axes.X).ReadFromXML(reader.ReadSubtree)

                        Case "Y"
                            Perturbation(Axes.Y).ReadFromXML(reader.ReadSubtree)

                        Case "Z"
                            Perturbation(Axes.Z).ReadFromXML(reader.ReadSubtree)

                        Case "Properties"
                            Type = IOXML.ReadInteger(reader, "Type", ProfileType.Impulsivo)

                    End Select

                End If

            End While

        End Sub

    End Class

#End Region

End Namespace
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

Imports System.Threading.Tasks
Imports MathTools.Algebra.EuclideanSpace
Imports AeroTools.VisualModel.Interface
Imports AeroTools.UVLM.SimulationTools

Public Class VelocityControl

    Public Event RefreshGL()

    Private Ready As Boolean = False
    Private ModyfingData As Boolean = True
    Private PlanoDeVelocidad As VelocityPlane
    Private Proyecto As AircraftProject

    Public Sub IniciarControl(ByRef Proyecto As AircraftProject)

        Me.Ready = False
        Me.PlanoDeVelocidad = Proyecto.VelocityPlane
        Me.Proyecto = Proyecto
        Me.Ready = True And (Not PlanoDeVelocidad Is Nothing) And (Not Proyecto Is Nothing)

        If Ready Then Me.AcomodarDatosAlForm()

    End Sub

    Private Sub AcomodarDatosAlForm()

        If Not Ready Then Exit Sub

        ModyfingData = True

        Me.PsiBox.Value = MathTools.Conversion.RadToDeg(PlanoDeVelocidad.Psi)
        Me.TitaBox.Value = MathTools.Conversion.RadToDeg(PlanoDeVelocidad.Tita)

        Me.NxBox.Value = PlanoDeVelocidad.NodesInDirection_1
        Me.NyBox.Value = PlanoDeVelocidad.NodesInDirection_2

        Me.ExtensionX.Value = PlanoDeVelocidad.Extension_1
        Me.ExtensionY.Value = PlanoDeVelocidad.Extension_2

        Me.OrigenX.Value = PlanoDeVelocidad.Origin.X
        Me.OrigenY.Value = PlanoDeVelocidad.Origin.Y
        Me.OrigenZ.Value = PlanoDeVelocidad.Origin.Z

        Me.VisualizarPlano.Checked = PlanoDeVelocidad.Visible

        Me.EscalaBox.Value = PlanoDeVelocidad.Scale

        Me.VectoresSampleColor.BackColor = PlanoDeVelocidad.ColorDeVectores
        Me.NodoSampleColor.BackColor = PlanoDeVelocidad.ColorDeNodos
        Me.NodeSize.Value = PlanoDeVelocidad.NodeSize
        Me.LineSize.Value = PlanoDeVelocidad.VectorThickness

        Me.rdInducedVelocity.Checked = PlanoDeVelocidad.InducedVelocity
        Me.rdTotalVelocity.Checked = Not PlanoDeVelocidad.InducedVelocity

        ModyfingData = False

    End Sub

    Public Sub AdquirirDatosDelForm()

        If Not Ready Then Exit Sub

        PlanoDeVelocidad.Psi = MathTools.Conversion.DegToRad(Me.PsiBox.Value)
        PlanoDeVelocidad.Tita = MathTools.Conversion.DegToRad(Me.TitaBox.Value)

        PlanoDeVelocidad.NodesInDirection_1 = Me.NxBox.Value
        PlanoDeVelocidad.NodesInDirection_2 = Me.NyBox.Value

        PlanoDeVelocidad.Extension_1 = Me.ExtensionX.Value
        PlanoDeVelocidad.Extension_2 = Me.ExtensionY.Value

        PlanoDeVelocidad.Origin.X = Me.OrigenX.Value
        PlanoDeVelocidad.Origin.Y = Me.OrigenY.Value
        PlanoDeVelocidad.Origin.Z = Me.OrigenZ.Value

        PlanoDeVelocidad.Scale = Me.EscalaBox.Value

        PlanoDeVelocidad.Visible = Me.VisualizarPlano.Checked
        PlanoDeVelocidad.InducedVelocity = Me.rdInducedVelocity.Checked

        Me.PlanoDeVelocidad.GenerarMallado()
        Me.Proyecto.RepresentOnGL()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub GenerarMallado()

        If Not Ready Then Exit Sub

        PlanoDeVelocidad.GenerarMallado()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub CalculateVelocity()

        If Not Ready Then Exit Sub

        Me.AdquirirDatosDelForm()
        Me.PlanoDeVelocidad.GenerarMallado()
        Dim CantidadDeNodos As Integer = PlanoDeVelocidad.NumberOfNodes
        Dim RefVelocity As New EVector3

        If Proyecto.CalculationCore Is Nothing Then Exit Sub

        Dim Total As Boolean = Not PlanoDeVelocidad.InducedVelocity

        Parallel.For(1, CantidadDeNodos + 1, Sub(i As Integer)
                                                 PlanoDeVelocidad.ObtenerVelocidadInducida(i).Assign(Proyecto.CalculationCore.CalculateVelocityAtPoint(PlanoDeVelocidad.ObtenerNodo(i), Total))
                                                 PlanoDeVelocidad.ObtenerVelocidadInducida(i).ProjectOnPlane(PlanoDeVelocidad.VectorNormal)
                                             End Sub)
        Proyecto.RepresentOnGL()

    End Sub

    Private Sub CalculateTreftzIntegral()

        Proyecto.CalculationCore.ComputeTrefftzIntegral(PlanoDeVelocidad.VectorNormal, PlanoDeVelocidad.Origin, PlanoDeVelocidad.TreftSegments)

    End Sub

    Private Sub Calcular_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Calcular.Click

        CalculateVelocity()
        'CalculateTreftzIntegral()

    End Sub

    Private Sub Formato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Formato.Click

        If Not Ready Then Exit Sub

        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            VectoresSampleColor.BackColor = Colores.Color
            PlanoDeVelocidad.ColorDeVectores = Colores.Color
        End If

    End Sub

    Private Sub LineSize_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineSize.ValueChanged
        If Not Ready Then Exit Sub
        PlanoDeVelocidad.VectorThickness = Me.LineSize.Value
    End Sub

    Private Sub NodeSize_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NodeSize.ValueChanged
        If Not Ready Then Exit Sub
        PlanoDeVelocidad.NodeSize = Me.NodeSize.Value
    End Sub

    Private Sub ColorNodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorNodo.Click
        If Not Ready Then Exit Sub
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            NodoSampleColor.BackColor = Colores.Color
            PlanoDeVelocidad.ColorDeNodos = Colores.Color
        End If
    End Sub

    Private Sub EscalaBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaBox.ValueChanged
        If Not Ready Then Exit Sub
        PlanoDeVelocidad.Scale = Me.EscalaBox.Value
        Proyecto.RepresentOnGL()
    End Sub

    Private Sub ControlDePlano_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Not Ready Then Exit Sub
        If Visible Then Me.AcomodarDatosAlForm()
    End Sub

    Private Sub VisualizarPlano_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VisualizarPlano.CheckedChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub PsiBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PsiBox.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub TitaBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TitaBox.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub OrigenX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrigenX.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub OrigenY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrigenY.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub OrigenZ_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrigenZ.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub ExtensionX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtensionX.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub ExtensionY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtensionY.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub NxBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NxBox.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub NyBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NyBox.ValueChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub cbParallelComputing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not ModyfingData Then AdquirirDatosDelForm()
    End Sub

    Private Sub rdTotalVelocity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdTotalVelocity.CheckedChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
        CalculateVelocity()
    End Sub

    Private Sub rdInducedVelocity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdInducedVelocity.CheckedChanged
        If Not ModyfingData Then AdquirirDatosDelForm()
        CalculateVelocity()
    End Sub

    Public Event OnClose()

    Private Sub Close(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        RaiseEvent OnClose()
        Visible = False
    End Sub

End Class

'Open VOGEL (https://en.wikibooks.org/wiki/Open_VOGEL)
'Open source software for aerodynamics
'Copyright (C) 2018 Guillermo Hazebrouck (gahazebrouck@gmail.com)

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
Imports OpenVOGEL.DesignTools.DataStore
Imports OpenVOGEL.DesignTools.VisualModel.Models.Components

Public Class VelocityControl

    Public Event RefreshGL()

    Private Ready As Boolean = False
    Private ModyfingData As Boolean = True
    Private Plane As VelocityPlane

    Public Sub Initialize()

        Me.Ready = False
        Me.Plane = ProjectRoot.VelocityPlane
        Me.Ready = True And (Not Plane Is Nothing) And (ProjectRoot.Initialized)

        If Ready Then Me.AcomodarDatosAlForm()

    End Sub

    Private Sub AcomodarDatosAlForm()

        If Not Ready Then Exit Sub

        ModyfingData = True

        Me.PsiBox.Value = MathTools.Conversion.RadToDeg(Plane.Psi)
        Me.TitaBox.Value = MathTools.Conversion.RadToDeg(Plane.Tita)

        Me.NxBox.Value = Plane.NodesInDirection1
        Me.NyBox.Value = Plane.NodesInDirection2

        Me.ExtensionX.Value = Plane.Extension1
        Me.ExtensionY.Value = Plane.Extension2

        Me.OrigenX.Value = Plane.Origin.X
        Me.OrigenY.Value = Plane.Origin.Y
        Me.OrigenZ.Value = Plane.Origin.Z

        Me.VisualizarPlano.Checked = Plane.Visible

        Me.EscalaBox.Value = Plane.Scale

        Me.VectoresSampleColor.BackColor = Plane.ColorVectors
        Me.NodoSampleColor.BackColor = Plane.ColorNodes
        Me.NodeSize.Value = Plane.NodeSize
        Me.LineSize.Value = Plane.VectorThickness

        Me.rdInducedVelocity.Checked = Plane.InducedVelocity
        Me.rdTotalVelocity.Checked = Not Plane.InducedVelocity

        ModyfingData = False

    End Sub

    Public Sub AdquirirDatosDelForm()

        If Not Ready Then Exit Sub

        Plane.Psi = MathTools.Conversion.DegToRad(Me.PsiBox.Value)
        Plane.Tita = MathTools.Conversion.DegToRad(Me.TitaBox.Value)

        Plane.NodesInDirection1 = Me.NxBox.Value
        Plane.NodesInDirection2 = Me.NyBox.Value

        Plane.Extension1 = Me.ExtensionX.Value
        Plane.Extension2 = Me.ExtensionY.Value

        Plane.Origin.X = Me.OrigenX.Value
        Plane.Origin.Y = Me.OrigenY.Value
        Plane.Origin.Z = Me.OrigenZ.Value

        Plane.Scale = Me.EscalaBox.Value

        Plane.Visible = Me.VisualizarPlano.Checked
        Plane.InducedVelocity = Me.rdInducedVelocity.Checked

        Me.Plane.GenerateMesh()
        ProjectRoot.RepresentOnGL()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub GenerarMallado()

        If Not Ready Then Exit Sub

        Plane.GenerateMesh()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub CalculateVelocity()

        If Not Ready Then Exit Sub

        AdquirirDatosDelForm()
        Plane.GenerateMesh()
        Dim CantidadDeNodos As Integer = Plane.NumberOfNodes
        Dim RefVelocity As New EVector3

        If ProjectRoot.CalculationCore Is Nothing Then Exit Sub

        Dim WithStreamOmega As Boolean = ProjectRoot.CalculationCore.Settings.Omega.EuclideanNorm > 0.0

        Dim Total As Boolean = Not Plane.InducedVelocity

        Parallel.For(1, CantidadDeNodos + 1, Sub(i As Integer)
                                                 Plane.GetInducedVelocity(i).Assign(ProjectRoot.CalculationCore.CalculateVelocityAtPoint(Plane.GetNode(i), Total, WithStreamOmega))
                                                 Plane.GetInducedVelocity(i).ProjectOnPlane(Plane.NormalVector)
                                             End Sub)
        ProjectRoot.RepresentOnGL()

    End Sub

    Private Sub CalculateTreftzIntegral()

        ProjectRoot.CalculationCore.ComputeTrefftzIntegral(Plane.NormalVector, Plane.Origin, Plane.TreftSegments)

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
            Plane.ColorVectors = Colores.Color
        End If

    End Sub

    Private Sub LineSize_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LineSize.ValueChanged
        If Not Ready Then Exit Sub
        Plane.VectorThickness = Me.LineSize.Value
    End Sub

    Private Sub NodeSize_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NodeSize.ValueChanged
        If Not Ready Then Exit Sub
        Plane.NodeSize = Me.NodeSize.Value
    End Sub

    Private Sub ColorNodo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ColorNodo.Click
        If Not Ready Then Exit Sub
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            NodoSampleColor.BackColor = Colores.Color
            Plane.ColorNodes = Colores.Color
        End If
    End Sub

    Private Sub EscalaBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaBox.ValueChanged
        If Not Ready Then Exit Sub
        Plane.Scale = Me.EscalaBox.Value
        ProjectRoot.RepresentOnGL()
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

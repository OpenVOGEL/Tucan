'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2020 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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
Imports OpenVOGEL.Tucan.Utility

Public Class VelocityControl

    Public Event RefreshGL()

    Private Ready As Boolean = False
    Private ModyfingData As Boolean = True
    Private Plane As VelocityPlane

    Public Sub Initialize()

        Ready = False
        Plane = ProjectRoot.VelocityPlane
        Ready = True And (Not Plane Is Nothing) And (ProjectRoot.Initialized)

        If Ready Then Me.LoadDataToControl()

    End Sub

    Private Sub LoadDataToControl()

        If Not Ready Then Exit Sub

        ModyfingData = True

        PsiBox.Value = MathTools.Conversion.RadToDeg(Plane.Psi)
        TitaBox.Value = MathTools.Conversion.RadToDeg(Plane.Tita)

        NxBox.Value = Plane.NodesInDirection1
        NyBox.Value = Plane.NodesInDirection2

        ExtensionX.Value = Plane.Extension1
        ExtensionY.Value = Plane.Extension2

        OrigenX.Value = Plane.Origin.X
        OrigenY.Value = Plane.Origin.Y
        OrigenZ.Value = Plane.Origin.Z

        cbxVisible.Checked = Plane.Visible

        nudScale.Value = Plane.Scale

        VectorsSampleColor.BackColor = Plane.ColorVectors
        NodeSampleColor.BackColor = Plane.ColorNodes
        nudNodeSize.Value = Plane.NodeSize
        nudLineSize.Value = Plane.VectorThickness

        rdInducedVelocity.Checked = Plane.InducedVelocity
        rdTotalVelocity.Checked = Not Plane.InducedVelocity

        ModyfingData = False

    End Sub

    Public Sub AquireDataFromControl()

        If Not Ready Then Exit Sub

        Plane.Psi = MathTools.Conversion.DegToRad(PsiBox.Value)
        Plane.Tita = MathTools.Conversion.DegToRad(TitaBox.Value)

        Plane.NodesInDirection1 = NxBox.Value
        Plane.NodesInDirection2 = NyBox.Value

        Plane.Extension1 = ExtensionX.Value
        Plane.Extension2 = ExtensionY.Value

        Plane.Origin.X = OrigenX.Value
        Plane.Origin.Y = OrigenY.Value
        Plane.Origin.Z = OrigenZ.Value

        Plane.Scale = nudScale.Value

        Plane.Visible = cbxVisible.Checked
        Plane.InducedVelocity = rdInducedVelocity.Checked

        Plane.GenerateMesh()
        ModelInterface.RepresentOnGL()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub GenerarMallado()

        If Not Ready Then Exit Sub

        Plane.GenerateMesh()

        RaiseEvent RefreshGL()

    End Sub

    Private Sub CalculateVelocity()

        If Not Ready Then Exit Sub

        AquireDataFromControl()
        Plane.GenerateMesh()

        If Plane.NumberOfNodes > 0 Then

            Dim Count As Integer = Plane.NumberOfNodes
            Dim RefVelocity As New Vector3

            If ProjectRoot.CalculationCore Is Nothing Then Exit Sub

            Dim WithStreamOmega As Boolean = ProjectRoot.CalculationCore.Settings.Omega.EuclideanNorm > 0.0

            Dim Total As Boolean = Not Plane.InducedVelocity

            Parallel.For(1, Count + 1,
                         Sub(i As Integer)
                             Plane.GetInducedVelocity(i).Assign(ProjectRoot.CalculationCore.CalculateVelocityAtPoint(Plane.GetNode(i), Total))
                             Plane.GetInducedVelocity(i).ProjectOnPlane(Plane.NormalVector)
                         End Sub)
            ModelInterface.RepresentOnGL()

        End If

    End Sub

    Private Sub btnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculate.Click

        CalculateVelocity()
        RaiseEvent RefreshGL()

    End Sub

    Private Sub btnFormat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFormat.Click

        If Not Ready Then Exit Sub

        Dim Result As MsgBoxResult = ColorsDialog.ShowDialog()
        If Result = MsgBoxResult.Ok Then
            VectorsSampleColor.BackColor = ColorsDialog.Color
            Plane.ColorVectors = ColorsDialog.Color
            RaiseEvent RefreshGL()
        End If

    End Sub

    Private Sub nudLineSize_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudLineSize.ValueChanged
        If Not Ready Then Exit Sub
        Plane.VectorThickness = Me.nudLineSize.Value
    End Sub

    Private Sub nudNodeSize_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNodeSize.ValueChanged
        If Not Ready Then Exit Sub
        Plane.NodeSize = Me.nudNodeSize.Value
    End Sub

    Private Sub nudNodeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudNodeColor.Click
        If Not Ready Then Exit Sub
        Dim Result As MsgBoxResult = ColorsDialog.ShowDialog()
        If Result = MsgBoxResult.Ok Then
            NodeSampleColor.BackColor = ColorsDialog.Color
            Plane.ColorNodes = ColorsDialog.Color
            RaiseEvent RefreshGL()
        End If
    End Sub

    Private Sub nudScale_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudScale.ValueChanged
        If Not Ready Then Exit Sub
        Plane.Scale = Me.nudScale.Value
        RaiseEvent RefreshGL()
    End Sub

    Private Sub ControlDePlano_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Not Ready Then Exit Sub
        If Visible Then Me.LoadDataToControl()
    End Sub

    Private Sub cbxVisible_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxVisible.CheckedChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub PsiBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PsiBox.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub TitaBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TitaBox.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub OrigenX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrigenX.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub OrigenY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrigenY.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub OrigenZ_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OrigenZ.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub ExtensionX_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtensionX.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub ExtensionY_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExtensionY.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub NxBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NxBox.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub NyBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NyBox.ValueChanged
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub cbParallelComputing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not ModyfingData Then AquireDataFromControl()
    End Sub

    Private Sub rdTotalVelocity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdTotalVelocity.CheckedChanged
        If Not ModyfingData Then AquireDataFromControl()
        CalculateVelocity()
    End Sub

    Private Sub rdInducedVelocity_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdInducedVelocity.CheckedChanged
        If Not ModyfingData Then AquireDataFromControl()
        CalculateVelocity()
    End Sub

    Public Event OnClose()

    Private Sub Close(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        RaiseEvent OnClose()
        Visible = False
    End Sub

End Class

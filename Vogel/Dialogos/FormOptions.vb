'Copyright (C) 2016 Guillermo Hazebrouck

Public Class FormOptions

    Public LoadingData As Boolean = True

    Private Sub Ocultar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ocultar.Click

        Call MainForm.Project.RepresentOnGL()
        Hide()

    End Sub

    Private Sub EstelasColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWakeColorSurface.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            MainForm.Project.Results.Wakes.VisualProps.ColorSurface = Colores.Color
            EstelasSampleColor.BackColor = Colores.Color
        End If
    End Sub

    Private Sub ModeloColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModeloColor.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            ModeloSampleColor.BackColor = Colores.Color
            MainForm.Project.Results.Model.VisualProps.ColorSurface = Colores.Color
            'Dim C As System.Drawing.Color = MainForm.Proyecto.Resultados.ObtenerModelo.ColorDeSuperficie
        End If
    End Sub

    Private Sub SuperficiesColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SuperficiesColor.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            SuperficieSampleColor.BackColor = Colores.Color
            If (Not IsNothing(MainForm.Project.Model.CurrentLiftingSurface)) Then
                MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ColorSurface = Colores.Color
            End If
        End If

    End Sub

    Private Sub EmpenajeColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MalladoPalaColor.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            MalladoSampleColor.BackColor = Colores.Color
            If (Not IsNothing(MainForm.Project.Model.CurrentLiftingSurface)) Then
                MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ColorMesh = Colores.Color
            End If
        End If
    End Sub

    Private Sub MalladoColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MalladoColor.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            MalladoModeloSampleColor.BackColor = Colores.Color
            MainForm.Project.Results.Model.VisualProps.ColorMesh = Colores.Color
        End If
    End Sub

    Private Sub LoadColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadColor.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            ColorCarga.BackColor = Colores.Color
            MainForm.Project.Results.Model.VisualProps.ColorLoads = Colores.Color
        End If
    End Sub

    Private Sub VmeanColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VmeanColor.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            VmediaColor.BackColor = Colores.Color
            MainForm.Project.Results.Model.VisualProps.ColorVelocity = Colores.Color
        End If
    End Sub

    Private Sub DVColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DVColor.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            SaltoVColor.BackColor = Colores.Color
        End If
    End Sub

    Private Sub IncMallado_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncMallado.CheckedChanged
        If (Not IsNothing(MainForm.Project.Model.CurrentLiftingSurface)) Then
            If Not LoadingData Then MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ShowMesh = IncMallado.Checked
        End If
    End Sub

    Private Sub TransparenciaSuperficie_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TransparenciaSuperficie.ValueChanged
        If (Not IsNothing(MainForm.Project.Model.CurrentLiftingSurface)) Then
            If Not LoadingData Then MainForm.Project.Model.CurrentLiftingSurface.VisualProps.Transparency = TransparenciaSuperficie.Value
        End If
    End Sub

    Private Sub Opciones_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        LoadingData = True

        ' Carga la lista de superficies
        If MainForm.Project.InterfaceMode = AeroTools.VisualModel.Interface.InterfaceModes.Design Then
            Me.GrupoPostProceso.Enabled = False
            Me.GrupoDiseño.Enabled = True
        End If

        If MainForm.Project.InterfaceMode = AeroTools.VisualModel.Interface.InterfaceModes.Postprocess Then
            Me.GrupoPostProceso.Enabled = True
            Me.GrupoDiseño.Enabled = False
        End If

        If MainForm.Project.Model.LiftingSurfaces.Count > 0 Then

            Me.ListeDeSuperfices.Items.Clear()

            Dim n As Integer = MainForm.Project.Model.LiftingSurfaces.Count - 1

            For i = 0 To n
                If Not IsNothing(MainForm.Project.Model.LiftingSurfaces(i)) Then
                    If Not IsNothing(MainForm.Project.Model.LiftingSurfaces(i).Name) Then
                        Me.ListeDeSuperfices.Items.Add(MainForm.Project.Model.LiftingSurfaces(i).Name)
                    Else
                        Me.ListeDeSuperfices.Items.Add(String.Format("Surface {0}", i))
                    End If

                End If
            Next

            n = MainForm.Project.Model.CurrentLiftingSurfaceID - 1
            If n >= 0 And n < MainForm.Project.Model.LiftingSurfaces.Count Then
                Me.ListeDeSuperfices.SelectedIndex = n
            End If

            ' Pone los colores de la superficie actual en los cuadros de muestra
            If (Not IsNothing(MainForm.Project.Model.CurrentLiftingSurface)) Then

                Me.SuperficieSampleColor.BackColor = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ColorSurface
                Me.MalladoSampleColor.BackColor = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ColorMesh
                Me.TransparenciaSuperficie.Value = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.Transparency
                Me.IncMallado.Checked = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ShowMesh

            End If

            ListeDeSuperfices.Enabled = True
            SuperficieSampleColor.Enabled = True
            MalladoSampleColor.Enabled = True
            TransparenciaSuperficie.Enabled = True
            SuperficiesColor.Enabled = True
            MalladoPalaColor.Enabled = True
            IncMallado.Enabled = True

        Else

            ListeDeSuperfices.Enabled = False
            SuperficieSampleColor.Enabled = False
            MalladoSampleColor.Enabled = False
            TransparenciaSuperficie.Enabled = False
            SuperficiesColor.Enabled = False
            MalladoPalaColor.Enabled = False
            IncMallado.Enabled = False

        End If

        ' Carga la lista de cuerpos

        If MainForm.Project.Model.Fuselages.Count > 0 Then

            Me.ListaDeCuerpos.Items.Clear()

            For i = 0 To MainForm.Project.Model.Fuselages.Count - 1

                If IsNothing(MainForm.Project.Model.Fuselages(i).Name) OrElse MainForm.Project.Model.Fuselages(i).Name = "" Then
                    Me.ListaDeCuerpos.Items.Add(String.Format("Body {0}", i))
                Else
                    Me.ListaDeCuerpos.Items.Add(MainForm.Project.Model.Fuselages(i).Name)

                End If
            Next

            Dim n As Integer = MainForm.Project.Model.CurrentBodyID - 1
            If n >= 0 And n < MainForm.Project.Model.Fuselages.Count Then
                Me.ListaDeCuerpos.SelectedIndex = n
            End If

            ' Pone los colores del cuerpo actual en los cuadros de muestra

            Me.CuerposSampleColor.BackColor = MainForm.Project.Model.CurrentBody.VisualProps.ColorSurface
            Me.MalladoCuerposSampleColor.BackColor = MainForm.Project.Model.CurrentBody.VisualProps.ColorMesh
            Me.TransparenciaCuerpos.Value = MainForm.Project.Model.CurrentBody.VisualProps.Transparency
            Me.IncMalladoCuerpos.Checked = MainForm.Project.Model.CurrentBody.VisualProps.ShowMesh

            ListaDeCuerpos.Enabled = True
            CuerposSampleColor.Enabled = True
            MalladoCuerposSampleColor.Enabled = True
            TransparenciaCuerpos.Enabled = True
            IncMalladoCuerpos.Enabled = True
            CuerposColor.Enabled = True
            MalladoCuerposColor.Enabled = True

        Else

            ListaDeCuerpos.Enabled = False
            CuerposSampleColor.Enabled = False
            MalladoCuerposSampleColor.Enabled = False
            TransparenciaCuerpos.Enabled = False
            IncMalladoCuerpos.Enabled = False
            CuerposColor.Enabled = False
            MalladoCuerposColor.Enabled = False

        End If

        ' Opciones del marco de referencia

        MarcacionFina.Checked = MainForm.Project.VisualizationParameters.ReferenceFrame.ConMarcacionFina
        MarcacionGruesa.Checked = MainForm.Project.VisualizationParameters.ReferenceFrame.ConMarcacionGruesa

        Me.ModeloSampleColor.BackColor = MainForm.Project.Results.Model.VisualProps.ColorSurface
        Me.MalladoModeloSampleColor.BackColor = MainForm.Project.Results.Model.VisualProps.ColorMesh
        Me.TransparenciaModelo.Value = MainForm.Project.Results.Model.VisualProps.Transparency
        Me.IncluirMalladoMod.Checked = MainForm.Project.Results.Model.VisualProps.ShowMesh

        Me.EstelasSampleColor.BackColor = MainForm.Project.Results.Wakes.VisualProps.ColorSurface
        Me.cbShowWakeSurface.Checked = MainForm.Project.Results.Wakes.VisualProps.ShowSurface
        Me.MalladoEstSampleColor.BackColor = MainForm.Project.Results.Wakes.VisualProps.ColorMesh
        Me.nudWakesSurfaceTrans.Value = MainForm.Project.Results.Wakes.VisualProps.Transparency
        Me.cbShowWakeMesh.Checked = MainForm.Project.Results.Wakes.VisualProps.ShowMesh
        Me.NodoEstSampleColor.BackColor = MainForm.Project.Results.Wakes.VisualProps.ColorNodes
        Me.cbShowWakeNodes.Checked = MainForm.Project.Results.Wakes.VisualProps.ShowNodes
        Me.nudWakeMeshSize.Value = MainForm.Project.Results.Wakes.VisualProps.ThicknessMesh
        Me.nudWakesNodesSize.Value = MainForm.Project.Results.Wakes.VisualProps.SizeNodes
        Me.nudWakeMeshSize.Value = MainForm.Project.Results.Wakes.VisualProps.ThicknessMesh

        Me.PantallaSample.BackColor = MainForm.Project.VisualizationParameters.ScreenColor
        Me.WireframeSample.BackColor = MainForm.Project.VisualizationParameters.ReferenceFrame.ColorDelWireFrame
        Me.Xmax_Box.Value = MainForm.Project.VisualizationParameters.ReferenceFrame.Xmax
        Me.Ymax_Box.Value = MainForm.Project.VisualizationParameters.ReferenceFrame.Ymax
        Me.Xmin_Box.Value = MainForm.Project.VisualizationParameters.ReferenceFrame.Xmin
        Me.Ymin_Box.Value = MainForm.Project.VisualizationParameters.ReferenceFrame.Ymin
        Me.Zmed_Box.Value = MainForm.Project.VisualizationParameters.ReferenceFrame.Z

        Me.VmediaColor.BackColor = MainForm.Project.Results.Model.VisualProps.ColorVelocity
        Me.EscalaVmedia.Value = MainForm.Project.Results.Model.VisualProps.ScaleVelocity
        Me.RepresentarVMedia.Checked = MainForm.Project.Results.Model.VisualProps.ShowVelocityVectors

        Me.ColorCarga.BackColor = MainForm.Project.Results.Model.VisualProps.ColorLoads
        Me.EscalaCarga.Value = MainForm.Project.Results.Model.VisualProps.ScalePressure
        Me.RepresentarCarga.Checked = MainForm.Project.Results.Model.VisualProps.ShowLoadVectors

        Me.MapaDePresión.Checked = MainForm.Project.Results.Model.VisualProps.ShowColormap

        Me.MaxPresBox.Value = MainForm.Project.Results.Model.PressureRange.Maximum
        Me.MinPresBox.Value = MainForm.Project.Results.Model.PressureRange.Minimum

        Me.AlphaBlendBox.Checked = MainForm.Project.VisualizationParameters.AllowAlphaBlending

        LoadingData = False

    End Sub

    Private Sub ListeDeSuperfices_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListeDeSuperfices.SelectedIndexChanged

        MainForm.Project.Model.CurrentLiftingSurfaceID = ListeDeSuperfices.SelectedIndex + 1

        If (Not IsNothing(MainForm.Project.Model.CurrentLiftingSurface)) Then
            Me.SuperficieSampleColor.BackColor = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ColorSurface
            Me.MalladoSampleColor.BackColor = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ColorMesh
            Me.TransparenciaSuperficie.Value = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.Transparency
            Me.IncMallado.Checked = MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ShowMesh
        End If

    End Sub

    Private Sub ReferenceFrame_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReferenceFrame.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            MainForm.Project.VisualizationParameters.ReferenceFrame.ColorDelWireFrame = Me.Colores.Color
            Me.WireframeSample.BackColor = Colores.Color
        End If
    End Sub


    Private Sub Xmax_Box_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Xmax_Box.ValueChanged
        If Not LoadingData Then
            MainForm.Project.VisualizationParameters.ReferenceFrame.Xmax = Xmax_Box.Value
        End If
    End Sub

    Private Sub Xmin_Box_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Xmin_Box.ValueChanged
        If Not LoadingData Then
            MainForm.Project.VisualizationParameters.ReferenceFrame.Xmin = Xmin_Box.Value
        End If
    End Sub

    Private Sub Ymax_Box_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ymax_Box.ValueChanged
        If Not LoadingData Then
            MainForm.Project.VisualizationParameters.ReferenceFrame.Ymax = Ymax_Box.Value
        End If
    End Sub

    Private Sub Ymin_Box_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ymin_Box.ValueChanged
        If Not LoadingData Then
            MainForm.Project.VisualizationParameters.ReferenceFrame.Ymin = Ymin_Box.Value
        End If
    End Sub

    Private Sub Zmed_Box_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Zmed_Box.ValueChanged
        If Not LoadingData Then
            MainForm.Project.VisualizationParameters.ReferenceFrame.Z = Zmed_Box.Value
        End If
    End Sub

    Private Sub PantallaButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PantallaButton.Click
        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            Me.PantallaSample.BackColor = Colores.Color
            MainForm.Project.VisualizationParameters.ScreenColor = Colores.Color
        End If
    End Sub

    Private Sub MarcacionGruesa_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarcacionGruesa.CheckedChanged
        If Not LoadingData Then MainForm.Project.VisualizationParameters.ReferenceFrame.ConMarcacionGruesa = Me.MarcacionGruesa.Checked
    End Sub

    Private Sub MarcacionFina_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MarcacionFina.CheckedChanged
        If Not LoadingData Then MainForm.Project.VisualizationParameters.ReferenceFrame.ConMarcacionFina = Me.MarcacionFina.Checked
    End Sub

    Private Sub CuerposColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CuerposColor.Click

        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            Me.CuerposSampleColor.BackColor = Me.Colores.Color
            MainForm.Project.Model.CurrentBody.VisualProps.ColorSurface = Me.Colores.Color
        End If

    End Sub

    Private Sub ListaDeCuerpos_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListaDeCuerpos.SelectedIndexChanged

        MainForm.Project.Model.CurrentBodyID = ListaDeCuerpos.SelectedIndex + 1

        Me.CuerposSampleColor.BackColor = MainForm.Project.Model.CurrentBody.VisualProps.ColorSurface
        Me.MalladoCuerposSampleColor.BackColor = MainForm.Project.Model.CurrentBody.VisualProps.ColorMesh
        Me.TransparenciaCuerpos.Value = MainForm.Project.Model.CurrentBody.VisualProps.Transparency
        Me.IncMalladoCuerpos.Checked = MainForm.Project.Model.CurrentBody.VisualProps.ShowMesh

    End Sub

    Private Sub MalladoCuerposColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MalladoCuerposColor.Click

        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            Me.MalladoCuerposSampleColor.BackColor = Me.Colores.Color
            MainForm.Project.Model.CurrentBody.VisualProps.ColorMesh = Me.Colores.Color
        End If

    End Sub

    Private Sub TransparenciaCuerpos_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TransparenciaCuerpos.ValueChanged
        If Not LoadingData Then MainForm.Project.Model.CurrentBody.VisualProps.Transparency = Me.TransparenciaCuerpos.Value
    End Sub

    Private Sub IncMalladoCuerpos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncMalladoCuerpos.CheckedChanged
        If Not LoadingData Then MainForm.Project.Model.CurrentBody.VisualProps.ShowMesh = Me.IncMalladoCuerpos.Checked
    End Sub

    Private Sub IncluirMalladoMod_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IncluirMalladoMod.CheckedChanged
        If Not LoadingData Then MainForm.Project.Results.Model.VisualProps.ShowMesh = IncluirMalladoMod.Checked
    End Sub

    Private Sub TransparenciaModelo_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TransparenciaModelo.ValueChanged
        If Not LoadingData Then MainForm.Project.Results.Model.VisualProps.Transparency = TransparenciaModelo.Value
    End Sub


    Private Sub MalladoEstelaSampleColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWakeColorMesh.Click

        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            Me.MalladoEstSampleColor.BackColor = Me.Colores.Color
            MainForm.Project.Results.Wakes.VisualProps.ColorMesh = Me.Colores.Color
        End If

    End Sub

    Private Sub NodosEstelaSampleColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWakeColorNodes.Click

        Dim Respuesta As MsgBoxResult = Colores.ShowDialog()
        If Respuesta = MsgBoxResult.Ok Then
            Me.NodoEstSampleColor.BackColor = Me.Colores.Color
            MainForm.Project.Results.Wakes.VisualProps.ColorNodes = Me.Colores.Color
        End If

    End Sub

    Private Sub MostrarEstela_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowWakeSurface.CheckedChanged
        If Not LoadingData Then MainForm.Project.Results.Wakes.VisualProps.ShowSurface = cbShowWakeSurface.Checked
    End Sub

    Private Sub IncluirMalladoEst_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowWakeMesh.CheckedChanged
        If Not LoadingData Then MainForm.Project.Results.Wakes.VisualProps.ShowMesh = cbShowWakeMesh.Checked
    End Sub

    Private Sub IncNodosEstela_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowWakeNodes.CheckedChanged
        If Not LoadingData Then MainForm.Project.Results.Wakes.VisualProps.ShowNodes = cbShowWakeNodes.Checked
    End Sub

    Private Sub TamañoDeNodosEstela_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudWakesNodesSize.ValueChanged
        If Not LoadingData Then MainForm.Project.Results.Wakes.VisualProps.SizeNodes = nudWakesNodesSize.Value
    End Sub

    Private Sub TamañoDeMalladoEstela_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudWakeMeshSize.ValueChanged
        If Not LoadingData Then MainForm.Project.Results.Wakes.VisualProps.ThicknessMesh = nudWakeMeshSize.Value
    End Sub

    Private Sub EscalaCarga_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaCarga.ValueChanged
        If Not LoadingData Then MainForm.Project.Results.Model.VisualProps.ScalePressure = EscalaCarga.Value
    End Sub

    Private Sub EscalaVmedia_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EscalaVmedia.ValueChanged
        If Not LoadingData Then MainForm.Project.Results.Model.VisualProps.ScaleVelocity = EscalaVmedia.Value
    End Sub

    Private Sub RepresentarCarga_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepresentarCarga.CheckedChanged
        If Not LoadingData Then MainForm.Project.Results.Model.VisualProps.ShowLoadVectors = RepresentarCarga.Checked
    End Sub

    Private Sub RepresentarVMedia_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RepresentarVMedia.CheckedChanged
        If Not LoadingData Then MainForm.Project.Results.Model.VisualProps.ShowVelocityVectors = RepresentarVMedia.Checked
    End Sub

    Private Sub MapaDePresión_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MapaDePresión.CheckedChanged
        If Not LoadingData Then MainForm.Project.Results.Model.VisualProps.ShowColormap = MapaDePresión.Checked
    End Sub

    Private Sub SmothLines_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SmothLines.CheckedChanged
        MainForm.Project.VisualizationParameters.AllowLineSmoothing = SmothLines.Checked
    End Sub

    Private Sub MaxPresBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaxPresBox.ValueChanged

        If Not LoadingData And MainForm.Project.Results.Loaded Then
            MainForm.Project.Results.Model.PressureRange.Maximum = Me.MaxPresBox.Value
            MainForm.Project.Results.Model.UpdateColormapWithPressure()
        End If

    End Sub

    Private Sub MinPresBox_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MinPresBox.ValueChanged

        If Not LoadingData And MainForm.Project.Results.Loaded Then
            MainForm.Project.Results.Model.PressureRange.Minimum = Me.MinPresBox.Value
            MainForm.Project.Results.Model.UpdateColormapWithPressure()
        End If

    End Sub

    Private Sub ResetMapaDePresiones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetMapaDePresiones.Click

        If MainForm.PostProcesoCargado Then
            MainForm.Project.Results.Model.FindPressureRange()
            MainForm.Project.Results.Model.UpdateColormapWithPressure()
            LoadingData = True
            Me.MaxPresBox.Value = MainForm.Project.Results.Model.PressureRange.Maximum
            Me.MinPresBox.Value = MainForm.Project.Results.Model.PressureRange.Minimum
            LoadingData = False
        End If

    End Sub

    Private Sub AlphaBlendBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlphaBlendBox.CheckedChanged
        If Not LoadingData Then MainForm.Project.VisualizationParameters.AllowAlphaBlending = AlphaBlendBox.Checked
    End Sub

End Class
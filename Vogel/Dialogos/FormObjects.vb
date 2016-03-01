Public Class FormObjects

    Private TableOfObjects As DataTable
    Private WithChanges As Boolean = False
    Private Reloaded As Boolean = True

    Private Sub ObjectsManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Reloaded = True

        TableOfObjects = New DataTable("Objects")

        Dim Column As DataColumn
        Dim View As DataView

        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.Boolean")
        Column.ColumnName = "Ver"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.String")
        Column.ColumnName = "ColorSuperficie"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.String")
        Column.ColumnName = "ColorMallado"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.String")
        Column.ColumnName = "Nombre"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.String")
        Column.ColumnName = "Tipo"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.Int32")
        Column.ColumnName = "Indice"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.Int32")
        Column.ColumnName = "NN"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.Int32")
        Column.ColumnName = "NP"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.Boolean")
        Column.ColumnName = "Incluir"
        TableOfObjects.Columns.Add(Column)
        Column = New DataColumn()
        Column.DataType = System.Type.GetType("System.Boolean")
        Column.ColumnName = "Bloquear"
        TableOfObjects.Columns.Add(Column)

        View = New DataView(TableOfObjects)
        dgvObjects.DataSource = View

        dgvObjects.AllowUserToOrderColumns = False
        dgvObjects.AllowUserToResizeColumns = False
        dgvObjects.AllowUserToResizeRows = False
        dgvObjects.ColumnHeadersHeightSizeMode = False

        dgvObjects.Columns(0).ReadOnly = False
        dgvObjects.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(0).HeaderText = "Show"
        dgvObjects.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(1).ReadOnly = True
        dgvObjects.Columns(1).HeaderText = "Surf."
        dgvObjects.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(2).ReadOnly = True
        dgvObjects.Columns(2).HeaderText = "Mesh"
        dgvObjects.Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(3).ReadOnly = True
        dgvObjects.Columns(3).HeaderText = "Name"
        dgvObjects.Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(4).ReadOnly = True
        dgvObjects.Columns(4).HeaderText = "Type"
        dgvObjects.Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(5).ReadOnly = True
        dgvObjects.Columns(5).HeaderText = "Index"
        dgvObjects.Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(6).ReadOnly = True
        dgvObjects.Columns(6).HeaderText = "Nodes"
        dgvObjects.Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(7).ReadOnly = True
        dgvObjects.Columns(7).HeaderText = "Elements"
        dgvObjects.Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(8).ReadOnly = False
        dgvObjects.Columns(8).HeaderText = "Include"
        dgvObjects.Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable
        dgvObjects.Columns(9).ReadOnly = False
        dgvObjects.Columns(9).HeaderText = "Lock"

        Reloaded = False

    End Sub

    Private Sub ObjectsManager_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        RefreshList()
    End Sub

    Private Sub btnAddObject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddObject.Click

        Dim Dialog As New SelectObject

        Dialog.ShowDialog()

        If Dialog.DialogResult = DialogResult.OK Then

            If Dialog.WingSelected Then

                MainForm.Project.Model.AddLiftingSurface()
                RefreshList()
                WithChanges = True

            ElseIf Dialog.FuselageSelected Then

                MainForm.Project.Model.AddExtrudedBody()
                RefreshList()
                WithChanges = True

            ElseIf Dialog.JetEngineSelected Then

                MainForm.Project.Model.AddJetEngine()
                RefreshList()
                WithChanges = True

            End If

            MainForm.Project.RefreshOnGL()

        End If

    End Sub

    Private Sub btnRemoveObject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveObject.Click

        Reloaded = True

        Dim SelectedRowIndex As Integer = dgvObjects.CurrentRow.Index

        If dgvObjects.Rows(SelectedRowIndex).Cells("Tipo").Value = "W" Then

            If MainForm.Project.Model.LiftingSurfaces.Count >= 0 Then

                Dim StartingSurfaceID As Integer = MainForm.Project.Model.CurrentLiftingSurfaceID
                Dim SurfaceIndex As Integer = dgvObjects.Rows(SelectedRowIndex).Cells("Indice").Value

                MainForm.Project.Model.CurrentLiftingSurfaceID = SurfaceIndex
                Dim Answer As MsgBoxResult = MsgBox("Are you sure you want to remove the surface """ & MainForm.Project.Model.CurrentLiftingSurface.Name & """ from the model?", MsgBoxStyle.YesNo, "Remove lifting surface")

                If Answer = MsgBoxResult.Yes Then
                    MainForm.Project.Model.RemoveLiftingSurface(SurfaceIndex)
                    RefreshList()
                    WithChanges = True
                End If

                MainForm.Project.Model.CurrentLiftingSurfaceID = StartingSurfaceID
                MainForm.Project.RepresentOnGL()

            End If

        ElseIf dgvObjects.Rows(SelectedRowIndex).Cells("Tipo").Value = "F" Then

            If MainForm.Project.Model.Fuselages.Count >= 0 Then

                Dim StartingSurfaceID As Integer = MainForm.Project.Model.CurrentBodyID
                Dim SurfaceIndex As Integer = GetIndexOfSelectedObject()

                MainForm.Project.Model.CurrentBodyID = SurfaceIndex
                Dim Answer As MsgBoxResult = MsgBox("Are you sure you want to remove the body """ & MainForm.Project.Model.CurrentBody.Name & """ from the model?", MsgBoxStyle.YesNo, "Remove body")

                If Answer = MsgBoxResult.Yes Then
                    MainForm.Project.Model.RemoveBody(SurfaceIndex)
                    RefreshList()
                    WithChanges = True
                End If

                MainForm.Project.Model.CurrentBodyID = StartingSurfaceID
                MainForm.Project.RepresentOnGL()

            End If

        ElseIf dgvObjects.Rows(SelectedRowIndex).Cells("Tipo").Value = "E" Then

            If MainForm.Project.Model.Fuselages.Count >= 0 Then

                Dim StartingSurfaceID As Integer = MainForm.Project.Model.CurrentJetEngineID
                Dim SurfaceIndex As Integer = GetIndexOfSelectedObject()

                MainForm.Project.Model.CurrentJetEngineID = SurfaceIndex
                Dim Answer As MsgBoxResult = MsgBox("Are you sure you want to remove the nacelle """ & MainForm.Project.Model.CurrentBody.Name & """ from the model?", MsgBoxStyle.YesNo, "Remove body")

                If Answer = MsgBoxResult.Yes Then
                    MainForm.Project.Model.RemoveJetEngine(SurfaceIndex)
                    RefreshList()
                    WithChanges = True
                End If

                MainForm.Project.Model.CurrentJetEngineID = StartingSurfaceID
                MainForm.Project.RepresentOnGL()

            End If

        End If

        Reloaded = False

    End Sub

    Private Sub tsmEditObject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmEditObject.Click

        Dim BodyIndex As Integer = dgvObjects.CurrentRow.Index
        Dim LiftingSurfaceIndex As Integer = dgvObjects.Rows(BodyIndex).Cells("Indice").Value
        Dim JetIndex As Integer = dgvObjects.CurrentRow.Index

        If dgvObjects.Rows(BodyIndex).Cells("Tipo").Value = "W" Then

            MainForm.Project.Model.CurrentLiftingSurfaceID = LiftingSurfaceIndex
            MainForm.ShowLiftingSurfaceEditor()
            RefreshList()
            WithChanges = True

        End If

        If dgvObjects.Rows(BodyIndex).Cells("Tipo").Value = "F" Then

            MainForm.Project.Model.CurrentBodyID = BodyIndex
            MainForm.ShowFuselageEditor()
            RefreshList()
            WithChanges = True

        End If

        If dgvObjects.Rows(BodyIndex).Cells("Tipo").Value = "E" Then

            MainForm.Project.Model.CurrentJetEngineID = JetIndex
            MainForm.ShowJetEngineDialog()
            RefreshList()
            WithChanges = True

        End If

    End Sub

    Private Sub btnCopyObject_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopyObject.Click

        Dim SelectedRowIndex As Integer = dgvObjects.CurrentRow.Index

        Dim SurfaceTypeIndex As Integer = dgvObjects.Rows(SelectedRowIndex).Cells("Indice").Value

        If dgvObjects.Rows(SelectedRowIndex).Cells("Tipo").Value = "W" Then

            MainForm.Project.Model.CloneLiftingSurface(GetIndexOfSelectedObject)
            MainForm.Project.RepresentOnGL()
            RefreshList()
            WithChanges = True

        ElseIf dgvObjects.Rows(SelectedRowIndex).Cells("Tipo").Value = "E" Then

            MainForm.Project.Model.CurrentJetEngineID = SurfaceTypeIndex
            Dim EngineToCopy = MainForm.Project.Model.CurrentJetEngine
            MainForm.Project.Model.AddJetEngine()
            MainForm.Project.Model.CurrentJetEngine.CopyFrom(EngineToCopy)
            MainForm.Project.RepresentOnGL()
            RefreshList()
            WithChanges = True


        End If

    End Sub

    Private Sub tsmChangeStyle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmChangeStyle.Click

        MainForm.Project.Model.CurrentLiftingSurfaceID = dgvObjects.CurrentRow.Index + 1
        If Not IsNothing(FormOptions) Then FormOptions.TabControl1.SelectTab(0)
        FormOptions.ShowDialog()
        RefreshList()
        WithChanges = True

    End Sub

    Private Sub dgvObjects_CellValueChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvObjects.CellValueChanged

        If e.ColumnIndex = 0 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Then
            If Not Reloaded Then

                Dim IndiceDeFilaSeleccionada As Integer = dgvObjects.CurrentRow.Index
                Dim IndiceDeSuperficie As Integer = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Indice").Value

                If dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Tipo").Value = "W" Then

                    Dim OriginalID As Integer = MainForm.Project.Model.CurrentLiftingSurfaceID

                    Dim Show As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Ver").Value 'LISTADEOBJETOS.Rows(IndiceDeSuperficie - 1).Item("Ver") 
                    Dim Include As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Incluir").Value
                    Dim Lock As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Bloquear").Value

                    MainForm.Project.Model.CurrentLiftingSurfaceID = IndiceDeSuperficie

                    If (Not IsNothing(MainForm.Project.Model.CurrentLiftingSurface)) Then

                        MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ShowSurface = Show
                        MainForm.Project.Model.CurrentLiftingSurface.VisualProps.ShowMesh = Show

                        MainForm.Project.Model.CurrentLiftingSurface.IncludeInCalculation = Include

                        MainForm.Project.Model.CurrentLiftingSurface.Lock = Lock

                        MainForm.Project.Model.CurrentLiftingSurfaceID = OriginalID

                    End If

                    WithChanges = True
                    dgvObjects.Select()

                End If

                If dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Tipo").Value = "F" Then

                    If e.ColumnIndex = 0 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Then
                        If Not Reloaded Then

                            Dim OriginalID As Integer = MainForm.Project.Model.CurrentBodyID

                            Dim Show As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Ver").Value 'LISTADEOBJETOS.Rows(IndiceDeSuperficie - 1).Item("Ver") 
                            Dim Include As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Incluir").Value
                            Dim Lock As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Bloquear").Value

                            MainForm.Project.Model.CurrentBodyID = IndiceDeSuperficie

                            MainForm.Project.Model.CurrentBody.VisualProps.ShowSurface = Show
                            MainForm.Project.Model.CurrentBody.VisualProps.ShowMesh = Show

                            MainForm.Project.Model.CurrentBody.IncludeInCalculation = Include

                            MainForm.Project.Model.CurrentBody.LockContent = Lock

                            MainForm.Project.Model.CurrentBodyID = OriginalID

                            WithChanges = True
                            dgvObjects.Select()
                        End If
                    End If
                End If

                If dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Tipo").Value = "E" Then

                    If e.ColumnIndex = 0 Or e.ColumnIndex = 8 Or e.ColumnIndex = 9 Then
                        If Not Reloaded Then

                            Dim OriginalID As Integer = MainForm.Project.Model.CurrentJetEngineID

                            Dim Show As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Ver").Value 'LISTADEOBJETOS.Rows(IndiceDeSuperficie - 1).Item("Ver") 
                            Dim Include As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Incluir").Value
                            Dim Lock As Boolean = dgvObjects.Rows(IndiceDeFilaSeleccionada).Cells("Bloquear").Value

                            MainForm.Project.Model.CurrentJetEngineID = IndiceDeSuperficie

                            MainForm.Project.Model.CurrentJetEngine.VisualProps.ShowSurface = Show
                            MainForm.Project.Model.CurrentJetEngine.VisualProps.ShowMesh = Show

                            MainForm.Project.Model.CurrentJetEngine.IncludeInCalculation = Include

                            MainForm.Project.Model.CurrentJetEngine.LockContent = Lock

                            MainForm.Project.Model.CurrentJetEngineID = OriginalID

                            WithChanges = True
                            dgvObjects.Select()

                        End If
                    End If
                End If

            End If
        End If

    End Sub

    Private Sub ObjectsManager_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed

        If WithChanges Then

            MainForm.Project.RepresentOnGL()

        End If

    End Sub

    Private Sub RefreshList()

        Dim Row As DataRow
        Dim RowIndex As Integer = 0

        Reloaded = True

        TableOfObjects.Rows.Clear()

        Dim SurfaceID As Integer = 0

        For Each LiftingSurface In MainForm.Project.Model.LiftingSurfaces

            Row = TableOfObjects.NewRow
            Row("ColorSuperficie") = " "
            Row("ColorMallado") = " "
            Row("Ver") = LiftingSurface.VisualProps.ShowSurface
            Row("Nombre") = LiftingSurface.Name
            Row("Tipo") = "W"
            Row("Indice") = SurfaceID + 1
            Row("NN") = LiftingSurface.nNodes
            Row("NP") = LiftingSurface.nPanels
            Row("Incluir") = LiftingSurface.IncludeInCalculation
            Row("Bloquear") = LiftingSurface.Lock

            TableOfObjects.Rows.Add(Row)

            dgvObjects.Rows(RowIndex).Cells("ColorSuperficie").Style.BackColor = LiftingSurface.VisualProps.ColorSurface
            dgvObjects.Rows(RowIndex).Cells("ColorSuperficie").Style.SelectionBackColor = LiftingSurface.VisualProps.ColorSurface

            dgvObjects.Rows(RowIndex).Cells("ColorMallado").Style.BackColor = LiftingSurface.VisualProps.ColorMesh
            dgvObjects.Rows(RowIndex).Cells("ColorMallado").Style.SelectionBackColor = LiftingSurface.VisualProps.ColorMesh

            SurfaceID += 1
            RowIndex += 1

        Next

        SurfaceID = 0

        If MainForm.Project.Model.Fuselages.Count > 0 Then

            For Each Body In MainForm.Project.Model.Fuselages

                Row = TableOfObjects.NewRow
                Row("ColorSuperficie") = " "
                Row("ColorMallado") = " "
                Row("Ver") = Body.VisualProps.ShowSurface
                Row("Nombre") = Body.Name
                Row("Tipo") = "F"
                Row("Indice") = SurfaceID + 1
                Row("NN") = Body.NumberOfNodes
                Row("NP") = Body.NumberOfPanels
                Row("Incluir") = Body.IncludeInCalculation
                Row("Bloquear") = Body.LockContent

                TableOfObjects.Rows.Add(Row)

                dgvObjects.Rows(RowIndex).Cells("ColorSuperficie").Style.BackColor = Body.VisualProps.ColorSurface
                dgvObjects.Rows(RowIndex).Cells("ColorSuperficie").Style.SelectionBackColor = Body.VisualProps.ColorSurface

                dgvObjects.Rows(RowIndex).Cells("ColorMallado").Style.BackColor = Body.VisualProps.ColorMesh
                dgvObjects.Rows(RowIndex).Cells("ColorMallado").Style.SelectionBackColor = Body.VisualProps.ColorMesh

                SurfaceID += 1
                RowIndex += 1

            Next

        End If

        SurfaceID = 0

        If MainForm.Project.Model.JetEngines.Count > 0 Then

            For Each JetEngine In MainForm.Project.Model.JetEngines

                Row = TableOfObjects.NewRow
                Row("ColorSuperficie") = " "
                Row("ColorMallado") = " "
                Row("Ver") = JetEngine.VisualProps.ShowSurface
                Row("Nombre") = JetEngine.Name
                Row("Tipo") = "E"
                Row("Indice") = SurfaceID + 1
                Row("NN") = JetEngine.NumberOfNodes
                Row("NP") = JetEngine.NumberOfPanels
                Row("Incluir") = JetEngine.IncludeInCalculation
                Row("Bloquear") = JetEngine.LockContent

                TableOfObjects.Rows.Add(Row)

                dgvObjects.Rows(RowIndex).Cells("ColorSuperficie").Style.BackColor = JetEngine.VisualProps.ColorSurface
                dgvObjects.Rows(RowIndex).Cells("ColorSuperficie").Style.SelectionBackColor = JetEngine.VisualProps.ColorSurface

                dgvObjects.Rows(RowIndex).Cells("ColorMallado").Style.BackColor = JetEngine.VisualProps.ColorMesh
                dgvObjects.Rows(RowIndex).Cells("ColorMallado").Style.SelectionBackColor = JetEngine.VisualProps.ColorMesh

                SurfaceID += 1
                RowIndex += 1

            Next

        End If

        dgvObjects.AutoResizeColumns()
        dgvObjects.Columns(1).Width = 40
        dgvObjects.Columns(2).Width = 40

        Reloaded = False

    End Sub

    Private Function GetIndexOfSelectedObject()

        Return dgvObjects.Rows(dgvObjects.CurrentRow.Index).Cells("Indice").Value

    End Function

End Class
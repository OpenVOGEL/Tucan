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

Imports AeroTools.CalculationModel.Models.Aero.Components
Imports System.Windows.Forms
Imports MathTools.Algebra.EuclideanSpace

Public Class CustomPolarControl

    Private _Polar As CustomPolar

    Public Property Polar As CustomPolar
        Set(value As CustomPolar)
            _Polar = value
            If _Polar IsNot Nothing Then
                SetUpTable()
                SetBindings()
            End If
        End Set
        Get
            Return _Polar
        End Get
    End Property

    Private Sub SetBindings()

        nudReynolds.DataBindings.Clear()
        nudReynolds.DataBindings.Add("Value", _Polar, "Reynolds", True, Windows.Forms.DataSourceUpdateMode.OnPropertyChanged)

        tbPolarName.DataBindings.Clear()
        tbPolarName.DataBindings.Add("Text", _Polar, "Name", True, Windows.Forms.DataSourceUpdateMode.OnPropertyChanged)

    End Sub

    Private _setupready = False

    Private Sub SetUpTable()

        If Not _setupready Then

            dgvNodes.AllowUserToResizeColumns = False
            dgvNodes.AllowUserToResizeRows = False
            dgvNodes.ScrollBars = ScrollBars.Vertical
            dgvNodes.RowHeadersVisible = False

            dgvNodes.Columns.Add("n", "#")
            dgvNodes.Columns(0).ReadOnly = True
            dgvNodes.Columns(0).Width = 25
            dgvNodes.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable

            dgvNodes.Columns.Add("X", "CL")
            dgvNodes.Columns(1).Width = 55
            dgvNodes.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable

            dgvNodes.Columns.Add("Y", "CDi")
            dgvNodes.Columns(2).Width = 55
            dgvNodes.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable

            _setupready = True

        End If

        RefreshTable()

    End Sub

    Public Sub RefreshTable()

        If _Polar IsNot Nothing Then

            If Not _setupready Then SetUpTable()

            dgvNodes.Rows.Clear()

            For i = 0 To _Polar.Nodes.Count - 1

                Dim row As Object() = {i, _Polar.Nodes(i).X, _Polar.Nodes(i).Y}
                dgvNodes.Rows.Add(row)

            Next

        End If

    End Sub

    Private Sub AddPoint() Handles btnAddPoint.Click

        If _Polar IsNot Nothing Then

            If _Polar.Nodes.Count = 0 Then

                _Polar.Nodes.Add(New EVector2(0, 0))

            ElseIf _Polar.Nodes.Count = 1 Then

                _Polar.Nodes.Add(New EVector2(_Polar.Nodes(0).X + 1, _Polar.Nodes(0).Y))

            ElseIf dgvNodes.CurrentRow.Index >= 0 And dgvNodes.CurrentRow.Index < _Polar.Nodes.Count - 1 Then

                Dim p1 As EVector2 = _Polar.Nodes(dgvNodes.CurrentRow.Index)
                Dim p2 As EVector2 = _Polar.Nodes(dgvNodes.CurrentRow.Index + 1)

                _Polar.Nodes.Insert(dgvNodes.CurrentRow.Index + 1, New EVector2(0.5 * (p1.X + p2.X), 0.5 * (p1.Y + p2.Y)))

            End If

            RefreshTable()

            RaiseEvent OnNodeChanged()

        End If

    End Sub

    Private Sub RemovePoint() Handles btnRemovePoint.Click

        Dim n As Integer = dgvNodes.CurrentRow.Index

        If _Polar.Nodes.Count > 1 And n > 0 And n < _Polar.Nodes.Count Then

            _Polar.Nodes.RemoveAt(n)

            RefreshTable()

            RaiseEvent OnNodeChanged()

        End If

    End Sub

    Private Sub EditNode(sender As Object, e As DataGridViewCellEventArgs) Handles dgvNodes.CellValueChanged

        If e.RowIndex >= 0 And e.RowIndex < _Polar.Nodes.Count Then

            If e.ColumnIndex = 1 Then

                Try
                    Dim value As Double = CDbl(dgvNodes.CurrentCell.Value)
                    _Polar.Nodes(e.RowIndex).X = value
                Catch
                    dgvNodes.CurrentCell.Value = _Polar.Nodes(e.RowIndex).X
                End Try

                Refresh()

            ElseIf e.ColumnIndex = 2 Then

                Try
                    Dim value As Double = CDbl(dgvNodes.CurrentCell.Value)
                    If value >= 0 Then _Polar.Nodes(e.RowIndex).Y = value
                Catch
                    dgvNodes.CurrentCell.Value = _Polar.Nodes(e.RowIndex).Y
                End Try

                Refresh()

            End If

            RaiseEvent OnNodeChanged()

        End If

    End Sub

    Private Sub GetFromExcell() Handles btnGetFromExcel.Click

        If Clipboard.ContainsText Then

            _Polar.Nodes.Clear()

            Dim data() As String = Clipboard.GetText.Split(vbNewLine)

            For Each line As String In data

                Dim points() As String = line.Split(vbTab)

                If points IsNot Nothing AndAlso points.Length = 2 Then

                    _Polar.Nodes.Add(New EVector2(CDbl(points(0)), CDbl(points(1))))

                End If

            Next

            RefreshTable()

        End If

    End Sub

    Public Event OnNodeChanged()

End Class

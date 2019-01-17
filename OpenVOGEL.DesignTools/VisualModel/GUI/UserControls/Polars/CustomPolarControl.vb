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

Imports OpenVOGEL.AeroTools.CalculationModel.Models.Aero.Components
Imports System.Windows.Forms
Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace

Public Class CustomPolarControl

    Private _Polar As CustomPolar

    Public Event OnNodeChanged()
    Public Event OnNameChanged()
    Public Event OnReynoldsChanged()

    Private AllowEvents As Boolean = True

    Public Sub New()

        InitializeComponent()

        AddHandler tbPolarName.TextChanged, AddressOf GetPolarName
        AddHandler nudReynolds.ValueChanged, AddressOf GetReynolds

    End Sub

    Private Sub GetPolarName()

        If Polar IsNot Nothing And AllowEvents Then

            Polar.Name = tbPolarName.Text

            RaiseEvent OnNameChanged()

        End If

    End Sub

    Private Sub GetReynolds()

        If _Polar IsNot Nothing And AllowEvents Then

            _Polar.Reynolds = nudReynolds.Value

            RaiseEvent OnReynoldsChanged()

        End If

    End Sub

    Public Property Polar As CustomPolar
        Set(value As CustomPolar)
            _Polar = value
            If _Polar IsNot Nothing Then
                SetUpTable()
                RefreshPolarData()
            End If
        End Set
        Get
            Return _Polar
        End Get
    End Property

    Private Sub RefreshPolarData()

        If Polar IsNot Nothing Then
            AllowEvents = False
            nudReynolds.Value = Polar.Reynolds
            tbPolarName.Text = Polar.Name
            AllowEvents = True
        End If

    End Sub

    Private _setupready = False

    Private Sub SetUpTable()

        If Not _setupready Then

            dgvNodes.AllowUserToResizeColumns = False
            dgvNodes.AllowUserToResizeRows = False
            dgvNodes.ScrollBars = ScrollBars.Vertical
            dgvNodes.RowHeadersVisible = False

            Dim dvgNodesFont As New Drawing.Font("Consolas", 8)
            dgvNodes.DefaultCellStyle.Font = dvgNodesFont
            dgvNodes.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            dgvNodes.Columns.Add("n", "#")
            dgvNodes.Columns(0).ReadOnly = True
            dgvNodes.Columns(0).Width = 25
            dgvNodes.Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable

            Dim ColumnWidth As Integer = (dgvNodes.Width - 28 - SystemInformation.VerticalScrollBarWidth) / 2

            dgvNodes.Columns.Add("X", "CL")
            dgvNodes.Columns(1).Width = ColumnWidth
            dgvNodes.Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable

            dgvNodes.Columns.Add("Y", "CDi")
            dgvNodes.Columns(2).Width = ColumnWidth
            dgvNodes.Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable

            _setupready = True

        End If

        RefreshTable()

    End Sub

    Public Sub RefreshTable()

        If _Polar IsNot Nothing Then

            If Not _setupready Then SetUpTable()

            Dim c As Integer = -1
            Dim r As Integer = -1

            If dgvNodes.CurrentCell IsNot Nothing Then
                c = dgvNodes.CurrentCell.ColumnIndex
                r = dgvNodes.CurrentCell.RowIndex
            End If

            dgvNodes.Rows.Clear()

                For i = 0 To _Polar.Nodes.Count - 1

                Dim row As Object() = {i,
                    Math.Round(100000 * _Polar.Nodes(i).X) / 100000,
                    Math.Round(100000 * _Polar.Nodes(i).Y) / 100000}

                dgvNodes.Rows.Add(row)

                If i = r Then
                    dgvNodes.CurrentCell = dgvNodes.Rows(r).Cells(c)
                End If

            Next

            End If

    End Sub

    Private Sub AddPoint() Handles btnAddPoint.Click

        If _Polar IsNot Nothing Then

            If _Polar.Nodes.Count = 0 Then

                _Polar.Nodes.Add(New EVector2(0, 0))

                RefreshTable()

                RaiseEvent OnNodeChanged()

            ElseIf _Polar.Nodes.Count = 1 Then

                _Polar.Nodes.Add(New EVector2(_Polar.Nodes(0).X + 1, _Polar.Nodes(0).Y))

                RefreshTable()

                RaiseEvent OnNodeChanged()

            ElseIf dgvNodes.CurrentRow.Index >= 0 And dgvNodes.CurrentRow.Index < _Polar.Nodes.Count - 1 Then

                Dim p1 As EVector2 = _Polar.Nodes(dgvNodes.CurrentRow.Index)
                Dim p2 As EVector2 = _Polar.Nodes(dgvNodes.CurrentRow.Index + 1)

                _Polar.Nodes.Insert(dgvNodes.CurrentRow.Index + 1, New EVector2(0.5 * (p1.X + p2.X), 0.5 * (p1.Y + p2.Y)))

                RefreshTable()

                RaiseEvent OnNodeChanged()

            End If

        End If

    End Sub

    Private Sub RemovePoint() Handles btnRemovePoint.Click

        Dim n As Integer = dgvNodes.CurrentRow.Index

        If _Polar.Nodes.Count > 1 And n >= 0 And n < _Polar.Nodes.Count Then

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

    Private Sub GetFromClipboard() Handles btnGetFromClipboard.Click

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

            RaiseEvent OnNodeChanged()

        End If

    End Sub

    Private Sub UpdateReynolds(sender As Object, e As EventArgs) Handles nudReynolds.ValueChanged

        If _Polar IsNot Nothing Then

            _Polar.Reynolds = nudReynolds.Value

            RaiseEvent OnReynoldsChanged()

        End If

    End Sub

End Class

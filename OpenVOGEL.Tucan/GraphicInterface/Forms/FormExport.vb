'OpenVOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2021 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Imports OpenVOGEL.DesignTools.DataStore

Public Class FormExport

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        cbxFormats.Items.Clear()
        cbxFormats.Items.Add("Stereolithography (stl)")
        cbxFormats.Items.Add("Native (dat)")
        cbxFormats.SelectedIndex = 0

    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

        If System.IO.File.Exists(FilePath) Then

            Select Case cbxFormats.SelectedIndex

                Case 0

                    ExportDesignModel(ExportTypes.ExportStl, cbxOneFile.Checked)

                Case 1

                    ExportDesignModel(ExportTypes.ExportCalculix, cbxOneFile.Checked)

            End Select

        Else
        MessageBox.Show("Please, save the project first")

        End If

        Me.Hide()

    End Sub

End Class
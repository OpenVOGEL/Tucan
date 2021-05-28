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
Imports OpenVOGEL.DesignTools.DataStore.ProjectRoot

''' <summary>
''' Represents a control that can display information about the active selection
''' </summary>
Public Class GroupPanel

    ''' <summary>
    ''' Reloads the data to the control
    ''' </summary>
    Public Sub UpdateData()

        lbxPanels.Items.Clear()

        If Results.ActiveFrame IsNot Nothing Then

            For Each Panel In Results.ActiveFrame.Model.Mesh.Panels

                If Panel.Active Then

                    lbxPanels.Items.Add("Panel " & Panel.GlobalIndex.ToString)

                End If

            Next

        End If

    End Sub


End Class

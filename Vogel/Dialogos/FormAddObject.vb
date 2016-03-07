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

Public Class SelectObject

    Public ReadOnly Property WingSelected As Boolean
        Get
            Return rbLiftingSurface.Enabled And rbLiftingSurface.Checked
        End Get
    End Property

    Public ReadOnly Property FuselageSelected As Boolean
        Get
            Return rbFuselage.Enabled And rbFuselage.Checked
        End Get
    End Property

    Public ReadOnly Property JetEngineSelected As Boolean
        Get
            Return rbJetEngine.Enabled And rbJetEngine.Checked
        End Get
    End Property

End Class
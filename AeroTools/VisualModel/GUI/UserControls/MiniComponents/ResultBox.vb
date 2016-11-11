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

Imports MathTools.Magnitudes

Public Class ResultBox

    Public Property Magnitude As IPhysicalMagnitude

    Public Sub New(Magnitude As Magnitudes)

        InitializeComponent()

        Me.Magnitude = Units.GetInstanceOf(Magnitude)

        lblUnit.Text = Me.Magnitude.Label

        SetUp()

    End Sub

    Public Sub New(Magnitude As IPhysicalMagnitude)

        InitializeComponent()

        Me.Magnitude = Units.GetInstanceOf(Magnitude.Magnitude)

        Me.Magnitude.Assign(Magnitude)

        lblUnit.Text = String.Format("{0}", Me.Magnitude.Label)

        SetUp()

    End Sub

    Private Sub SetUp()

        Decimals = 3
        lblName.Font = New Drawing.Font("Segoe UI", 8.25F)
        lblUnit.Font = lblName.Font

        BackColor = Drawing.Color.LightGray
        tbValue.BackColor = Drawing.Color.LightGray

        IsReadOnly = True

    End Sub

    Private _Decimals As Integer

    Public Property Decimals As Integer
        Set(value As Integer)
            _Decimals = value
            _FormatString = "{0:F" + _Decimals.ToString + "}"
        End Set
        Get
            Return _Decimals
        End Get
    End Property

    Private _FormatString As String = "{0:F3}"

    Public ReadOnly Property FormatString As String
        Get
            Return _FormatString
        End Get
    End Property

    ''' <summary>
    ''' Indicates if the value is read only.
    ''' </summary>
    ''' <returns></returns>
    Public Property IsReadOnly As Boolean
        Set(value As Boolean)
            tbValue.ReadOnly = value
        End Set
        Get
            Return tbValue.ReadOnly
        End Get
    End Property

    ''' <summary>
    ''' The value of the contained property.
    ''' </summary>
    ''' <returns></returns>
    Public Property Value As Double

        Set(value As Double)

            Magnitude.DefaultUnitValue = value

            If Double.IsNaN(value) Then
                tbValue.Text = "Nan"
            ElseIf Double.IsInfinity(value)
                tbValue.Text = "Inf"
            Else
                tbValue.Text = String.Format(FormatString, Magnitude.Value)
            End If

        End Set

        Get
            Return Magnitude.DefaultUnitValue
        End Get

    End Property

    ''' <summary>
    ''' The name of the contained property.
    ''' </summary>
    ''' <returns></returns>
    Public Overloads Property Name As String
        Set(value As String)
            lblName.Text = value
        End Set
        Get
            Return lblName.Text
        End Get
    End Property

    ''' <summary>
    ''' Indicates if the font is in greek characters.
    ''' </summary>
    ''' <returns></returns>
    Public Property GreekLetter As Boolean
        Set(value As Boolean)
            If value Then
                lblName.Font = New Drawing.Font("Symbol", lblName.Font.Size)
            End If
        End Set
        Get
            Return (lblName.Font.FontFamily.Name = "Symbol")
        End Get
    End Property

End Class

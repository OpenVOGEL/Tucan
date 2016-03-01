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

        Me.Magnitude.Assign(Magnitude)

        lblUnit.Text = String.Format("[{0}]", Me.Magnitude.Label)

        SetUp()

    End Sub

    Private Sub SetUp()

        lblName.Font = New Drawing.Font("Segoe UI", 8.25F)
        lblUnit.Font = lblName.Font

        BackColor = Drawing.Color.LightGray

        IsReadOnly = True

    End Sub

    Public Property FormatString As String = "{0:F3}"

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

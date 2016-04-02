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

Public Class FormProgress

    Public Sub New()

        InitializeComponent()
        pgCalculationProgress.Maximum = 100

    End Sub

    ''' <summary>
    ''' Publishes a message and updates the progress.
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <param name="Progreso"></param>
    ''' <remarks></remarks>
    Public Overloads Sub PushMessageWithProgress(ByVal Mensaje As String, ByVal Progreso As Integer)
        If InvokeRequired Then
            BeginInvoke(New PushMessageProgressCallback(AddressOf PushMessageWithProgress), Mensaje, Progreso)
        Else
            pgCalculationProgress.Value = Math.Min(100, Progreso)
            tbOperationsList.AppendText(String.Format("At {0}: {1}", Now, Mensaje) & vbNewLine)
        End If
    End Sub

    Delegate Sub PushMessageProgressCallback(ByVal Message As String, ByVal Progress As Integer)

    ''' <summary>
    ''' Publishes a message on the textbox.
    ''' </summary>
    ''' <param name="Mensaje"></param>
    ''' <remarks></remarks>
    Public Overloads Sub PushMessage(ByVal Mensaje As String)
        If InvokeRequired Then
            BeginInvoke(New PushMessageCallback(AddressOf PushMessage), Mensaje)
        Else
            tbOperationsList.AppendText(String.Format("At {0}: {1}", Now, Mensaje) & vbNewLine)
        End If
    End Sub

    Delegate Sub PushMessageCallback(ByVal Message As String)

    ''' <summary>
    ''' Publishes a progress on the progress bar.
    ''' </summary>
    ''' <param name="Progress"></param>
    ''' <remarks></remarks>
    Public Sub PushProgress(ByVal Progress As Single)
        If InvokeRequired Then
            BeginInvoke(New Action(Of Single)(AddressOf PushProgress), Progress)
        Else
            pgCalculationProgress.Value = Math.Min(Math.Min(100, Progress), 100)
        End If
    End Sub

    ''' <summary>
    ''' Publishes a state on the status label.
    ''' </summary>
    ''' <param name="State"></param>
    ''' <remarks></remarks>
    Public Sub PushState(ByVal State As String)
        If InvokeRequired Then
            BeginInvoke(New Action(Of String)(AddressOf PushState), State)
        Else
            lbState.Text = State
        End If
    End Sub

    ''' <summary>
    ''' Removes all messages.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearMessages()
        If InvokeRequired Then
            BeginInvoke(New Action(AddressOf ClearMessages))
        Else
            tbOperationsList.Text = ""
        End If
    End Sub

    ''' <summary>
    ''' Occurs when the user requests a cancelation.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event CancellationRequested()

    Private Sub btnCancel_Click() Handles btnCancel.Click

        RaiseEvent CancellationRequested()

        If _CloseModus Then

            Close()

        End If

    End Sub

    Private Sub WaitForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        pgCalculationProgress.Maximum = 100
        pgCalculationProgress.Value = 0
        tbOperationsList.AppendText(String.Format("At {0}: {1}", Now, "Starting...") & vbNewLine)
    End Sub

    Private _CloseModus As Boolean = False

    Public Sub ChangeToCloseModus()

        If InvokeRequired Then
            BeginInvoke(New Action(AddressOf ChangeToCloseModus))
        Else
            btnCancel.Text = "Close"
            _CloseModus = True
        End If

    End Sub

End Class
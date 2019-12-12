'Open VOGEL (openvogel.org)
'Open source software for aerodynamics
'Copyright (C) 2020 Guillermo Hazebrouck (guillermo.hazebrouck@openvogel.org)

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

Imports OpenVOGEL.MathTools.Algebra.EuclideanSpace
Imports SharpGL

Namespace VisualModel.Interface

    ''' <summary>
    ''' Interface that implements operations on objects
    ''' </summary>
    ''' <remarks></remarks>
    Public Interface IOperational

        Sub MoveTo(ByVal Vector As Vector3)
        Sub Orientate(ByVal Point As Vector3, ByVal Ori As EulerAngles)
        Sub Scale(ByVal Scale As Double)
        Sub Align(ByVal P1 As Vector3, ByVal P2 As Vector3, ByVal P3 As Vector3, ByVal P4 As Vector3)

    End Interface

    ''' <summary>
    ''' Operation types
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Operations As UInteger

        NoOperation = 0
        Translate = 1
        Rotate = 2
        Align = 3
        Scale = 4

    End Enum

    ''' <summary>
    ''' Provides methods to handle gometric operations such as translation and rotation of IOperational objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OperationsTool

        Public Event OnTaskReady()

        Private _Scalars As New List(Of Double)
        Private _Orientations As New List(Of EulerAngles)
        Private _DestinationObject As IOperational
        Private _StatusFlag As String
        Private _Operation As Operations = Operations.NoOperation
        Private _SurfaceLoaded As Boolean = False

        Public Points As New List(Of Vector3)

        ''' <summary>
        ''' Specifies the desired geometric operation.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Operation As Operations
            Set(ByVal value As Operations)
                CancelOperation()
                _Operation = value
            End Set
            Get
                Return _Operation
            End Get
        End Property

        ''' <summary>
        ''' Tells the user what the situation is inside the operations tool.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReadOnly Property StatusFlag As String
            Get
                Return _StatusFlag
            End Get
        End Property

        ''' <summary>
        ''' Loads a refference to the IOperational object that is going to be modified under the current task.
        ''' It will only work if there is an active operation and no loaded object.
        ''' </summary>
        ''' <param name="Obj"></param>
        ''' <remarks></remarks>
        Public Sub SetDestinationObject(ByRef Obj As IOperational)
            If _Operation <> Operations.NoOperation And Not _SurfaceLoaded Then
                _DestinationObject = Obj
                _SurfaceLoaded = True
            End If
        End Sub

        ''' <summary>
        ''' Adds an item that might be used on the current operation.
        ''' </summary>
        ''' <param name="Entity"></param>
        ''' <remarks></remarks>
        Public Sub SetEntityToQueue(ByVal Entity As Object)

            If _Operation = Operations.NoOperation Or Not _SurfaceLoaded Then Return

            If TypeOf Entity Is Vector3 Then
                Points.Add(Entity)
            End If

            If TypeOf Entity Is EulerAngles Then
                _Orientations.Add(Entity)
            End If

            If TypeOf Entity Is Double Then
                _Scalars.Add(Entity)
            End If

            TryOperation()

        End Sub

        ''' <summary>
        ''' Aborts the current operation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CancelOperation()

            _Operation = Operations.NoOperation
            _SurfaceLoaded = False
            Points.Clear()
            _Orientations.Clear()
            _Scalars.Clear()
            _StatusFlag = String.Format("Operation ""{0}"" has been cancelled", _Operation.ToString)

        End Sub

        Private Sub TryOperation()

            If IsNothing(_DestinationObject) Then Return

            Try

                Select Case _Operation

                    Case Operations.NoOperation

                        Return

                    Case Operations.Translate

                        If Points.Count >= 2 Then
                            _DestinationObject.MoveTo(Points(1) - Points(0))
                            RaiseTaskReady()
                        End If

                    Case Operations.Rotate

                        If _Orientations.Count >= 1 And Points.Count > 1 Then
                            _DestinationObject.Orientate(Points(0), _Orientations(0))
                            RaiseTaskReady()
                        End If

                    Case Operations.Scale

                        If _Scalars.Count >= 1 Then
                            _DestinationObject.Scale(_Scalars(0))
                            RaiseTaskReady()
                        End If

                    Case Operations.Align

                        If Points.Count >= 4 Then
                            _DestinationObject.Align(Points(0), Points(1), Points(2), Points(3))
                            RaiseTaskReady()
                        End If

                End Select

            Catch ex As Exception

                CancelOperation()
                _StatusFlag = String.Format("Operation ""{0}"" has been due to errors", _Operation.ToString)

            End Try

        End Sub

        Private Sub RaiseTaskReady()

            CancelOperation()
            RaiseEvent OnTaskReady()
            _StatusFlag = "Ready"

        End Sub

    End Class

End Namespace


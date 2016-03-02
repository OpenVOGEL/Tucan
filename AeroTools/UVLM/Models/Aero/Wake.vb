'Copyright (C) 2016 Guillermo Hazebrouck

Imports System.IO
Imports System.Threading.Tasks
Imports AeroTools.UVLM.Models.Aero.Components
Imports MathTools.Algebra.EuclideanSpace

Namespace UVLM.Models.Aero

    ''' <summary>
    ''' Represents a free lattice.
    ''' </summary>
    Public Class Wake

        Inherits Lattice

        ''' <summary>
        ''' Contains information about the sheding edge.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Primitive As New Primitive

        ''' <summary>
        ''' Indicates when the wake has to be trimmed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CuttingStep As Integer = 100

        ''' <summary>
        ''' Indicates if a fixed step has to be used (use this to model a prefixed wake).
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FixStep As Boolean = False

        ''' <summary>
        ''' Step to move the nodal points when the wake convection is prefixed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FidexedStep As New EVector3

        Public Sub Convect(ByVal Dt As Double)

            If FixStep Then

                Parallel.ForEach(Nodes, Sub(Node As Node)
                                            Node.Position.X += FidexedStep.X
                                            Node.Position.Y += FidexedStep.Y
                                            Node.Position.Z += FidexedStep.Z
                                        End Sub)

            Else

                Parallel.ForEach(Nodes, Sub(Node As Node)
                                            Node.Position.X += Dt * Node.Velocity.X
                                            Node.Position.Y += Dt * Node.Velocity.Y
                                            Node.Position.Z += Dt * Node.Velocity.Z
                                        End Sub)

            End If


        End Sub

        Public Overloads Sub AddVortexRing4(ByVal N1 As Integer, ByVal N2 As Integer, ByVal N3 As Integer, ByVal N4 As Integer, ByVal G As Double)

            Dim VortexRing As New VortexRing4(Nodes(N1), Nodes(N2), Nodes(N3), Nodes(N4), G, VortexRings.Count, False, True)

            VortexRings.Add(VortexRing)

        End Sub

#Region "I/O"

        Public Overrides Sub WriteBinary(ByRef w As BinaryWriter, Optional ByVal NodalVelocity As Boolean = False, Optional ByVal ReferencePosition As Boolean = False)

            MyBase.WriteBinary(w, NodalVelocity)

            w.Write(Primitive.Nodes.Count)

            For i = 0 To Primitive.Nodes.Count - 1
                w.Write(Primitive.Nodes(i))
            Next

            w.Write(Primitive.Rings.Count)

            For i = 0 To Primitive.Rings.Count - 1
                w.Write(Primitive.Rings(i))
            Next

            w.Write(CuttingStep)

        End Sub

        Public Overrides Sub ReadBinary(ByRef r As BinaryReader)

            MyBase.ReadBinary(r)

            Primitive.Nodes.Clear()
            Primitive.Rings.Clear()

            For i = 0 To r.ReadInt32 - 1
                Primitive.Nodes.Add(r.ReadInt32)
            Next

            For i = 0 To r.ReadInt32 - 1
                Primitive.Rings.Add(r.ReadInt32)
            Next

            CuttingStep = r.ReadInt32

        End Sub

#End Region

    End Class

End Namespace
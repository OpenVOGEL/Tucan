Imports MathTools.Algebra.EuclideanSpace

Namespace VisualModel.Models.Basics

    ''' <summary>
    ''' Represents an structural partition
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StructuralPartition

        ''' <summary>
        ''' Position of this partition
        ''' </summary>
        ''' <remarks></remarks>
        Public p As EVector3

        Public Basis As EBase3

        ''' <summary>
        ''' Section associated to this partition
        ''' </summary>
        ''' <remarks></remarks>
        Public LocalSection As UVLM.Models.Structural.Library.Section

        Public LocalChord As Double

        Public Sub New(ByVal p As EVector3, ByVal section As UVLM.Models.Structural.Library.Section)

            Me.p = New EVector3(p.X, p.Y, p.Z)
            LocalSection = New UVLM.Models.Structural.Library.Section
            LocalSection.Assign(section)

        End Sub

        Public Sub New()

            p = New EVector3()
            LocalSection = New UVLM.Models.Structural.Library.Section
            Basis = New EBase3()

        End Sub

    End Class

End Namespace


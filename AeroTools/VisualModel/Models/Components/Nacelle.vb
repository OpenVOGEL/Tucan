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

Imports MathTools.Algebra.EuclideanSpace
Imports AeroTools.VisualModel.Models.Basics

Namespace VisualModel.Models.Components

    Public Class FullNacelle

        Inherits GeneralSurface

        Private _ForwardSections As Integer = 3
        Private _RearSections As Integer = 3
        Private _ChordSections As Integer = 6
        Private _FlapRings As Integer = 3

        Private _ForwardLength As Single = 0.5#
        Private _RearLength As Single = 0.5#
        Private _Chord As Integer = 1.0#
        Private _LeadingEdgeElevation As Single = 0.0#
        Private _RearPointElevation = 0.0#
        Private _thetaUpper As Single = 0.25 * Math.PI
        Private _thetaLower As Single = 0.25 * Math.PI

        Private _ProfileChamber As ChamberedLine
        Private _UpperChamber As ChamberedLine
        Private _LowerChamber As ChamberedLine = New ChamberedLine(ChamberType.NACA4)
        Private _SideChamber As ChamberedLine = New ChamberedLine(ChamberType.NACA4)

        Private _IncludeLeftInterface As Boolean = True
        Private _IncludeRightInterface As Boolean = True
        Private _LeftInterfaceWidth As Single = 0.25
        Private _RightInterfaceWidth As Single = 0.25

        Private EightPanelsModel As Boolean = False

        Public Sub New()

            _ProfileChamber = New ChamberedLine(ChamberType.NACA4)
            _ProfileChamber.Dimension(ChamberDim.PosMaxChamber) = 0.4
            _ProfileChamber.Dimension(ChamberDim.MaxChamber) = 0.05

            _UpperChamber = New ChamberedLine(ChamberType.NACA4)
            _UpperChamber.Dimension(ChamberDim.PosMaxChamber) = 0.35
            _UpperChamber.Dimension(ChamberDim.MaxChamber) = 0.15

            _LowerChamber = New ChamberedLine(ChamberType.NACA4)
            _LowerChamber.Dimension(ChamberDim.PosMaxChamber) = 0.35
            _LowerChamber.Dimension(ChamberDim.MaxChamber) = 0.12

            _SideChamber = New ChamberedLine(ChamberType.NACA4)
            _SideChamber.Dimension(ChamberDim.PosMaxChamber) = 0.5
            _SideChamber.Dimension(ChamberDim.MaxChamber) = 0.1

        End Sub

        Public Overloads Sub GenerateLattice()

            If EightPanelsModel Then
                GenerateLattice_8()
            Else
                GenerateLattice_6()
            End If

        End Sub

        Public Sub GenerateLattice_6()

            _ProfileChamber.FlapChord = 0.2
            _ProfileChamber.FlapDeflection = -15 * Math.PI / 180
            _ProfileChamber.Flapped = True

            Mesh.NodalPoints.Clear()
            Mesh.NodalPoints.Add(New NodalPoint(0, 0, 0))

            Dim x As Double
            Dim l As Double = _ForwardLength + _Chord + _RearLength
            Dim xl As Double
            Dim nSections As Integer = 0

            Dim vu As New EVector2
            vu.X = Math.Sin(0.5 * _thetaUpper)
            vu.Y = Math.Cos(0.5 * _thetaUpper)

            Dim vl As New EVector2
            vl.X = Math.Sin(0.5 * _thetaLower)
            vl.Y = -Math.Cos(0.5 * _thetaLower)

            Dim RightChamberNodes(_ChordSections + 1) As Integer
            Dim LeftChamberNodes(_ChordSections + 1) As Integer

            ' Add forward sections:

            For i = 1 To _ForwardSections - 1

                x = i / _ForwardSections * _ForwardLength
                xl = x / l

                Dim SectionCenter As New EVector3(x, 0, xl * _RearPointElevation)

                Dim P0 As New EVector3(SectionCenter)
                P0.Y += vl.X * _LowerChamber.BasicLine(xl) * l
                P0.Z += vl.Y * _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, P0.Y, P0.Z))

                Dim P1 As New EVector3(SectionCenter)
                P1.Y += _SideChamber.BasicLine(xl) * l
                P1.Z = x / _ForwardLength * _LeadingEdgeElevation
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, P1.Y, P1.Z))

                Dim P2 As New EVector3(SectionCenter)
                P2.Y += vu.X * _UpperChamber.BasicLine(xl) * l
                P2.Z += vu.Y * _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P2.X, P2.Y, P2.Z))

                Mesh.NodalPoints.Add(New NodalPoint(P2.X, -P2.Y, P2.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, -P1.Y, P1.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, -P0.Y, P0.Z))

                nSections += 1

            Next

            ' Add chord sections:

            Dim _flapPosition As Integer = 0
            Dim _chamberPoint As New EVector2

            For i = 0 To _ChordSections

                If i < _ChordSections - _FlapRings Then
                    x = i / (_ChordSections - _FlapRings) * (_Chord - _ProfileChamber.FlapChord) + _ForwardLength
                Else
                    x = _Chord - _ProfileChamber.FlapChord * (_FlapRings - _flapPosition) / (_FlapRings) + _ForwardLength
                    _flapPosition += 1
                End If

                xl = x / l

                Dim SectionCenter As New EVector3(x, 0, xl * _RearPointElevation)

                Dim P0 As New EVector3(SectionCenter)
                P0.Y += vl.X * _LowerChamber.BasicLine(xl) * l
                P0.Z += vl.Y * _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, P0.Y, P0.Z))

                Dim P1 As New EVector3(SectionCenter)
                P1.Y += _SideChamber.BasicLine(xl) * l
                _ProfileChamber.EvaluatePoint(_chamberPoint, (x - _ForwardLength) / _Chord)
                P1.X = _chamberPoint.X * _Chord + _ForwardLength
                P1.Z = _chamberPoint.Y * _Chord + _LeadingEdgeElevation
                RightChamberNodes(i) = Mesh.NodalPoints.Count
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, P1.Y, P1.Z))

                Dim P2 As New EVector3(SectionCenter)
                P2.Y += vu.X * _UpperChamber.BasicLine(xl) * l
                P2.Z += vu.Y * _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P2.X, P2.Y, P2.Z))

                Mesh.NodalPoints.Add(New NodalPoint(P2.X, -P2.Y, P2.Z))
                LeftChamberNodes(i) = Mesh.NodalPoints.Count
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, -P1.Y, P1.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, -P0.Y, P0.Z))

                nSections += 1

            Next

            ' Add rear sections:

            For i = 1 To _RearSections - 1

                x = i / _RearSections * _RearLength + _ForwardLength + _Chord
                xl = x / l

                Dim SectionCenter As New EVector3(x, 0, xl * _RearPointElevation)

                Dim P0 As New EVector3(SectionCenter)
                P0.Y += vl.X * _LowerChamber.BasicLine(xl) * l
                P0.Z += vl.Y * _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, P0.Y, P0.Z))

                Dim P1 As New EVector3(SectionCenter)
                P1.Y += _SideChamber.BasicLine(xl) * l
                P1.Z = i / _RearSections * (_RearPointElevation - _LeadingEdgeElevation) + _LeadingEdgeElevation
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, P1.Y, P1.Z))

                Dim P2 As New EVector3(SectionCenter)
                P2.Y += vu.X * _UpperChamber.BasicLine(xl) * l
                P2.Z += vu.Y * _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P2.X, P2.Y, P2.Z))

                Mesh.NodalPoints.Add(New NodalPoint(P2.X, -P2.Y, P2.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, -P1.Y, P1.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, -P0.Y, P0.Z))

                nSections += 1

            Next

            Mesh.NodalPoints.Add(New NodalPoint(l, 0, _RearPointElevation))

            ' add panels (1-based indices!):

            Mesh.Panels.Clear()

            Mesh.Panels.Add(New Panel(1, 2, 3))
            Mesh.Panels.Add(New Panel(1, 3, 4))
            Mesh.Panels.Add(New Panel(1, 4, 5))
            Mesh.Panels.Add(New Panel(1, 5, 6))
            Mesh.Panels.Add(New Panel(1, 6, 7))
            Mesh.Panels.Add(New Panel(1, 7, 2))

            Dim nStart As Integer

            For i = 1 To nSections - 1

                nStart = (i - 1) * 6 + 1

                Mesh.Panels.Add(New Panel(nStart + 1, nStart + 7, nStart + 8, nStart + 2))
                Mesh.Panels.Add(New Panel(nStart + 2, nStart + 8, nStart + 9, nStart + 3))
                Mesh.Panels.Add(New Panel(nStart + 3, nStart + 9, nStart + 10, nStart + 4))
                Mesh.Panels.Add(New Panel(nStart + 4, nStart + 10, nStart + 11, nStart + 5))
                Mesh.Panels.Add(New Panel(nStart + 5, nStart + 11, nStart + 12, nStart + 6))
                Mesh.Panels.Add(New Panel(nStart + 6, nStart + 12, nStart + 7, nStart + 1))

            Next

            nStart = (nSections - 1) * 6 + 1

            Mesh.Panels.Add(New Panel(nStart + 1, nStart + 7, nStart + 2))
            Mesh.Panels.Add(New Panel(nStart + 2, nStart + 7, nStart + 3))
            Mesh.Panels.Add(New Panel(nStart + 3, nStart + 7, nStart + 4))
            Mesh.Panels.Add(New Panel(nStart + 4, nStart + 7, nStart + 5))
            Mesh.Panels.Add(New Panel(nStart + 5, nStart + 7, nStart + 6))
            Mesh.Panels.Add(New Panel(nStart + 6, nStart + 7, nStart + 1))

            ' Disable slenderness:

            For Each QuadPanel As Panel In Mesh.Panels
                QuadPanel.IsSlender = False
                QuadPanel.IsPrimitive = False
            Next

            ' Generate wing interface:

            If _IncludeRightInterface Then

                For i = 0 To _ChordSections
                    Mesh.NodalPoints.Add(New NodalPoint(Mesh.NodalPoints(RightChamberNodes(i)).Position.X, _RightInterfaceWidth, Mesh.NodalPoints(RightChamberNodes(i)).Position.Z))
                Next

                For i = 0 To _ChordSections - 1
                    Mesh.Panels.Add(New Panel(RightChamberNodes(i) + 1, Mesh.NodalPoints.Count - _ChordSections + i, Mesh.NodalPoints.Count - _ChordSections + i + 1, RightChamberNodes(i + 1) + 1))
                    Mesh.Panels(Mesh.Panels.Count - 1).IsSlender = True
                Next

                Mesh.Panels(Mesh.Panels.Count - 1).IsPrimitive = True

            End If

            If _IncludeLeftInterface Then

                For i = 0 To _ChordSections
                    Mesh.NodalPoints.Add(New NodalPoint(Mesh.NodalPoints(LeftChamberNodes(i)).Position.X, -_LeftInterfaceWidth, Mesh.NodalPoints(LeftChamberNodes(i)).Position.Z))
                Next

                For i = 0 To _ChordSections - 1
                    Mesh.Panels.Add(New Panel(LeftChamberNodes(i) + 1, Mesh.NodalPoints.Count - _ChordSections + i, Mesh.NodalPoints.Count - _ChordSections + i + 1, LeftChamberNodes(i + 1) + 1))
                    Mesh.Panels(Mesh.Panels.Count - 1).IsSlender = True
                Next

                Mesh.Panels(Mesh.Panels.Count - 1).IsPrimitive = True

            End If

            ' Generate mesh and geometric entities:

            GenerateLattice()

            GenerateControlPointsAndNormalVectors()

        End Sub

        Public Sub GenerateLattice_8()

            Mesh.NodalPoints.Clear()
            Mesh.NodalPoints.Add(New NodalPoint(0, 0, 0))

            Dim x As Double
            Dim l As Double = _ForwardLength + _Chord + _RearLength
            Dim xl As Double
            Dim nSections As Integer = 0

            Dim vu As New EVector2
            vu.X = Math.Sin(0.5 * _thetaUpper)
            vu.Y = Math.Cos(0.5 * _thetaUpper)

            Dim vl As New EVector2
            vl.X = Math.Sin(0.5 * _thetaLower)
            vl.Y = -Math.Cos(0.5 * _thetaLower)

            ' Add forward sections:

            For i = 1 To _ForwardSections - 1

                x = i / _ForwardSections * _ForwardLength
                xl = x / l

                Dim SectionCenter As New EVector3(x, 0, xl * _RearPointElevation)

                Dim PL As New EVector3(SectionCenter)
                PL.Z -= _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(PL.X, PL.Y, PL.Z))

                Dim P0 As New EVector3(SectionCenter)
                P0.Y += vl.X * _LowerChamber.BasicLine(xl) * l
                P0.Z += vl.Y * _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, P0.Y, P0.Z))

                Dim P1 As New EVector3(SectionCenter)
                P1.Y += _SideChamber.BasicLine(xl) * l
                P1.Z = x / _ForwardLength * _LeadingEdgeElevation
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, P1.Y, P1.Z))

                Dim P2 As New EVector3(SectionCenter)
                P2.Y += vu.X * _UpperChamber.BasicLine(xl) * l
                P2.Z += vu.Y * _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P2.X, P2.Y, P2.Z))

                Dim PU As New EVector3(SectionCenter)
                PU.Z += _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(PU.X, PU.Y, PU.Z))

                Mesh.NodalPoints.Add(New NodalPoint(P2.X, -P2.Y, P2.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, -P1.Y, P1.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, -P0.Y, P0.Z))

                nSections += 1

            Next

            ' Add chord sections:

            For i = 0 To _ChordSections

                x = i / _ChordSections * _Chord + _ForwardLength
                xl = x / l

                Dim SectionCenter As New EVector3(x, 0, xl * _RearPointElevation)

                Dim PL As New EVector3(SectionCenter)
                PL.Z -= _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(PL.X, PL.Y, PL.Z))

                Dim P0 As New EVector3(SectionCenter)
                P0.Y += vl.X * _LowerChamber.BasicLine(xl) * l
                P0.Z += vl.Y * _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, P0.Y, P0.Z))

                Dim P1 As New EVector3(SectionCenter)
                P1.Y += _SideChamber.BasicLine(xl) * l
                P1.Z = _Chord * _ProfileChamber.BasicLine(i / _ChordSections) + _LeadingEdgeElevation
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, P1.Y, P1.Z))

                Dim P2 As New EVector3(SectionCenter)
                P2.Y += vu.X * _UpperChamber.BasicLine(xl) * l
                P2.Z += vu.Y * _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P2.X, P2.Y, P2.Z))

                Dim PU As New EVector3(SectionCenter)
                PU.Z += _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(PU.X, PU.Y, PU.Z))

                Mesh.NodalPoints.Add(New NodalPoint(P2.X, -P2.Y, P2.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, -P1.Y, P1.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, -P0.Y, P0.Z))

                nSections += 1

            Next

            ' Add rear sections:

            For i = 1 To _RearSections - 1

                x = i / _RearSections * _RearLength + _ForwardLength + _Chord
                xl = x / l

                Dim SectionCenter As New EVector3(x, 0, xl * _RearPointElevation)

                Dim PL As New EVector3(SectionCenter)
                PL.Z -= _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(PL.X, PL.Y, PL.Z))

                Dim P0 As New EVector3(SectionCenter)
                P0.Y += vl.X * _LowerChamber.BasicLine(xl) * l
                P0.Z += vl.Y * _LowerChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, P0.Y, P0.Z))

                Dim P1 As New EVector3(SectionCenter)
                P1.Y += _SideChamber.BasicLine(xl) * l
                P1.Z = i / _RearSections * (_RearPointElevation - _LeadingEdgeElevation) + _LeadingEdgeElevation
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, P1.Y, P1.Z))

                Dim P2 As New EVector3(SectionCenter)
                P2.Y += vu.X * _UpperChamber.BasicLine(xl) * l
                P2.Z += vu.Y * _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(P2.X, P2.Y, P2.Z))

                Dim PU As New EVector3(SectionCenter)
                PU.Z += _UpperChamber.BasicLine(xl) * l
                Mesh.NodalPoints.Add(New NodalPoint(PU.X, PU.Y, PU.Z))

                Mesh.NodalPoints.Add(New NodalPoint(P2.X, -P2.Y, P2.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P1.X, -P1.Y, P1.Z))
                Mesh.NodalPoints.Add(New NodalPoint(P0.X, -P0.Y, P0.Z))

                nSections += 1

            Next

            Mesh.NodalPoints.Add(New NodalPoint(l, 0, _RearPointElevation))

            ' add panels (1-based indices!):

            Mesh.Panels.Clear()

            Mesh.Panels.Add(New Panel(1, 2, 3, 1))
            Mesh.Panels.Add(New Panel(1, 3, 4, 1))
            Mesh.Panels.Add(New Panel(1, 4, 5, 1))
            Mesh.Panels.Add(New Panel(1, 5, 6, 1))
            Mesh.Panels.Add(New Panel(1, 6, 7, 1))
            Mesh.Panels.Add(New Panel(1, 7, 8, 1))
            Mesh.Panels.Add(New Panel(1, 8, 9, 1))
            Mesh.Panels.Add(New Panel(1, 9, 2, 1))

            Dim nStart As Integer

            For i = 1 To nSections - 1

                nStart = (i - 1) * 8 + 1

                Mesh.Panels.Add(New Panel(nStart + 1, nStart + 9, nStart + 10, nStart + 2))
                Mesh.Panels.Add(New Panel(nStart + 2, nStart + 10, nStart + 11, nStart + 3))
                Mesh.Panels.Add(New Panel(nStart + 3, nStart + 11, nStart + 12, nStart + 4))
                Mesh.Panels.Add(New Panel(nStart + 4, nStart + 12, nStart + 13, nStart + 5))
                Mesh.Panels.Add(New Panel(nStart + 5, nStart + 13, nStart + 14, nStart + 6))
                Mesh.Panels.Add(New Panel(nStart + 6, nStart + 14, nStart + 15, nStart + 7))
                Mesh.Panels.Add(New Panel(nStart + 7, nStart + 15, nStart + 16, nStart + 8))
                Mesh.Panels.Add(New Panel(nStart + 8, nStart + 16, nStart + 9, nStart + 1))

            Next

            nStart = (nSections - 1) * 8 + 1

            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 1, nStart + 2, nStart + 9))
            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 2, nStart + 3, nStart + 9))
            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 3, nStart + 4, nStart + 9))
            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 4, nStart + 5, nStart + 9))
            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 5, nStart + 6, nStart + 9))
            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 6, nStart + 7, nStart + 9))
            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 7, nStart + 8, nStart + 9))
            Mesh.Panels.Add(New Panel(nStart + 9, nStart + 8, nStart + 1, nStart + 9))

            GenerateLattice()

            GenerateControlPointsAndNormalVectors()

        End Sub

    End Class

End Namespace
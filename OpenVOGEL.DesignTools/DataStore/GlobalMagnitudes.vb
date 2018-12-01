
Imports OpenVOGEL.MathTools.Magnitudes

Namespace DataStore

    Public Module GlobalMagnitudes

        Public UserMagnitudes As New Dictionary(Of Magnitudes, IPhysicalMagnitude)

        Public GlobalDecimals As New Dictionary(Of Magnitudes, Integer)

        Sub New()

            UserMagnitudes.Clear()

            Dim DeclaredUnits As Array
            DeclaredUnits = [Enum].GetValues(GetType(Magnitudes))

            For Each UnitType As Magnitudes In DeclaredUnits

                UserMagnitudes.Add(UnitType, Units.GetInstanceOf(UnitType))
                GlobalDecimals.Add(UnitType, 3)

            Next

            GlobalDecimals(Magnitudes.Force) = 0
            GlobalDecimals(Magnitudes.Moment) = 0
            GlobalDecimals(Magnitudes.Dimensionless) = 5

        End Sub

    End Module

End Namespace

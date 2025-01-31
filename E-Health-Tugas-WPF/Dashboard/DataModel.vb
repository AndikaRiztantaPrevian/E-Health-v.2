Public Class DataModel

    Public Property Id As Integer
    Public Property Name As String
    Public Property StartTime As TimeSpan
    Public Property EndTime As TimeSpan
    Public ReadOnly Property EllipseColor As Brush

        Get

            Dim currentTime = DateTime.Now.TimeOfDay

            If currentTime >= StartTime AndAlso currentTime <= EndTime Then

                Return New SolidColorBrush(ColorConverter.ConvertFromString("#00D440"))

            Else

                Return New SolidColorBrush(ColorConverter.ConvertFromString("#FFFFFF"))

            End If

        End Get

    End Property

End Class

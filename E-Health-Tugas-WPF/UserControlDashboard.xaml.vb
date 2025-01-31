Public Class UserControlDashboard

    Public Sub New()

        InitializeComponent()
        DataContext = New DashboardDataViewModel()

    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)

        Dim parentWindow As Window = Window.GetWindow(Me)

        If parentWindow IsNot Nothing Then

            parentWindow.Close()

        End If

    End Sub

End Class

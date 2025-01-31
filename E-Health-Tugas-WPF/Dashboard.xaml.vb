Public Class Dashboard

    Public Sub New()

        InitializeComponent()
        MainContent.Content = New UserControlDashboard()
        DataContext = New TimeViewModel()

        SetActiveButton(DashboardButton)

    End Sub

    Private Sub DashboardButton_Click(sender As Object, e As RoutedEventArgs)

        MainContent.Content = New UserControlDashboard()
        SetActiveButton(DashboardButton)

    End Sub

    Private Sub KonsultasiButton_Click(sender As Object, e As RoutedEventArgs)

        MainContent.Content = New UserControlKonsultasi()
        SetActiveButton(KonsultasiButton)

    End Sub

    Private Sub PemesananButton_Click(sender As Object, e As RoutedEventArgs)

        MainContent.Content = New UserControlPemesanan()
        SetActiveButton(PemesananButton)

    End Sub

    Private Sub SetActiveButton(activeButton As Button)

        DashboardButton.Background = Brushes.Transparent
        KonsultasiButton.Background = Brushes.Transparent
        PemesananButton.Background = Brushes.Transparent

        activeButton.Background = New SolidColorBrush(ColorConverter.ConvertFromString("#2E797E"))

    End Sub

End Class

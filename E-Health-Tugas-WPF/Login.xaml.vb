Imports MySql.Data.MySqlClient

Class Login

    Dim MysqlCon As MySqlConnection

    'Login Button
    Private Sub SubmitButton_Click(sender As Object, e As RoutedEventArgs) Handles SubmitButton.Click

        Dim email As String = EmailInput.Text.Trim()

        Dim password As String = PasswordInput.Password.Trim()


        If String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(password) Then

            MessageBox.Show("Semua field harus diisi.", "Info", MessageBoxButton.OK, MessageBoxImage.Warning)

            Return

        End If


        Dim MysqlCon As New MySqlConnection("server=localhost;userid=root;password=;database=e_health")


        Try

            MysqlCon.Open()


            Dim query As String = "SELECT COUNT(*) FROM users WHERE email = @Email AND password = @Password"

            Dim cmd As New MySqlCommand(query, MysqlCon)


            cmd.Parameters.AddWithValue("@Email", email)

            cmd.Parameters.AddWithValue("@Password", password)


            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())


            If count > 0 Then

                Dim dashboardWindow As New Dashboard()

                dashboardWindow.Show()

                Application.Current.MainWindow = dashboardWindow

                Me.Close()

                EmailInput.Text = ""

                PasswordInput.Password = ""

            Else

                MessageBox.Show("Email atau password salah.", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            End If

        Catch ex As MySqlException

            MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        Finally

            MysqlCon.Close()

            MysqlCon.Dispose()

        End Try

    End Sub

    'Close Button
    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click

        Close()

    End Sub

    'Open Register Window
    Private isRegisterWindowOpen As Boolean = False

    'Register Button
    Private Sub RegisterButton_Click(sender As Object, e As MouseButtonEventArgs) Handles RegisterButton.MouseLeftButtonDown

        If isRegisterWindowOpen Then

            Return

        End If

        isRegisterWindowOpen = True

        Dim registerWindow As New Register()
        AddHandler registerWindow.Closed, Sub() isRegisterWindowOpen = False
        registerWindow.Show()


        Close()

    End Sub

End Class

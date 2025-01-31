Imports MySql.Data.MySqlClient

Public Class Register

    'MySql Connection
    Dim MysqlCon As MySqlConnection

    'Register Button
    Private Sub SubmitButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles SubmitButton.Click


        Dim email As String = EmailInput.Text.Trim()

        Dim name As String = nameInput.Text.Trim()

        Dim password As String = PasswordInput.Password.Trim()

        Dim confirmPassword As String = PassworConfirmationInput.Password.Trim()


        If String.IsNullOrEmpty(email) OrElse String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(password) OrElse String.IsNullOrEmpty(confirmPassword) Then

            MessageBox.Show("Semua field harus diisi.", "Info", MessageBoxButton.OK, MessageBoxImage.Warning)

            Return

        End If


        If Not password.Equals(confirmPassword) Then

            MessageBox.Show("Password dan konfirmasi password tidak cocok.", "Info", MessageBoxButton.OK, MessageBoxImage.Warning)

            Return

        End If


        MysqlCon = New MySqlConnection("server=localhost;userid=root;password=;database=e_health")


        Try

            MysqlCon.Open()

            Dim query As String = "INSERT INTO users (email, name, password) VALUES (@Email, @Name, @Password)"
            Dim cmd As New MySqlCommand(query, MysqlCon)

            cmd.Parameters.AddWithValue("@Email", email)
            cmd.Parameters.AddWithValue("@Name", name)
            cmd.Parameters.AddWithValue("@Password", password)

            cmd.ExecuteNonQuery()

            MessageBox.Show("Pendaftaran berhasil!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information)

            EmailInput.Text = ""
            nameInput.Text = ""
            PasswordInput.Password = ""
            PassworConfirmationInput.Password = ""

        Catch ex As MySqlException

            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        Finally

            MysqlCon.Close()
            MysqlCon.Dispose()

        End Try


    End Sub

    'Cloase Button
    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs) Handles CloseButton.Click

        Close()

    End Sub

    'Open Login Window
    Private isLoginWindowOpen As Boolean = False

    Private Sub LoginButton_Click(sender As Object, e As MouseButtonEventArgs) Handles LoginButton.MouseLeftButtonDown

        If isLoginWindowOpen Then

            Return

        End If

        isLoginWindowOpen = True


        Dim loginWindow As New Login()

        AddHandler loginWindow.Closed, Sub() isLoginWindowOpen = False

        loginWindow.Show()


        Close()

    End Sub

End Class

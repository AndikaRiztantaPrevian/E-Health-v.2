Imports MySql.Data.MySqlClient

Public Class UserControlKonsultasi

    Public Sub New()

        InitializeComponent()
        Me.DataContext = New KonsultasiViewModel()

    End Sub
    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)

        Dim parentWindow As Window = Window.GetWindow(Me)

        If parentWindow IsNot Nothing Then

            parentWindow.Close()

        End If

    End Sub

    'Submit Button
    Private Sub SubmitButton_Click(sender As Object, e As RoutedEventArgs) Handles SubmitButton.Click

        Try

            Dim name As String = NameInput.Text.Trim()

            Dim nik As String = NikInput.Text.Trim()

            Dim phoneNumber As String = PhoneNumberInput.Text.Trim()

            Dim address As String = AddressInput.Text.Trim()

            Dim work As String = WorkInput.Text.Trim()

            Dim birthDate As Date = BirthDateInput.SelectedDate

            Dim gender As String = If(CType(GenderMaleRadioButton, RadioButton).IsChecked, "L", "P")

            Dim selectedSpecialist As SpecialistComboBox = CType(CategorySpecialistInput.SelectedItem, SpecialistComboBox)


            If selectedSpecialist Is Nothing Then

                MessageBox.Show("Harap pilih kategori spesialis.", "Info", MessageBoxButton.OK, MessageBoxImage.Warning)

                Return

            End If


            Dim categorySpecialist As String = selectedSpecialist.Id


            If String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(nik) OrElse String.IsNullOrEmpty(phoneNumber) OrElse
                String.IsNullOrEmpty(address) OrElse String.IsNullOrEmpty(work) OrElse birthDate = Date.MinValue Then

                MessageBox.Show("Harap lengkapi semua data.", "Info", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return
            End If

            ' Menghitung No. Antrian
            Dim noQueue As Integer = 0

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()

                    Dim queryQueue As String = "SELECT COUNT(*) FROM consultation_queue WHERE category_specialist_id = @CategorySpecialist AND DATE(created_at) = CURDATE()"


                    Dim commandQueue As New MySqlCommand(queryQueue, connection)

                    commandQueue.Parameters.AddWithValue("@CategorySpecialist", categorySpecialist)

                    noQueue = Convert.ToInt32(commandQueue.ExecuteScalar()) + 1

                End Using

            End Using

            ' Tampilkan No. Antrian di form
            NoQueueInput.Text = noQueue.ToString()

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()

                    Dim queryInsert As String = "INSERT INTO consultation_queue (name, nik, phone_number, address, work, birth_date, gender, category_specialist_id, no_queue, created_at) VALUES (@Name, @Nik, @PhoneNumber, @Address, @Work, @BirthDate, @Gender, @CategorySpecialist, @NoQueue, NOW())"

                    Dim commandInsert As New MySqlCommand(queryInsert, connection)

                    commandInsert.Parameters.AddWithValue("@Name", name)

                    commandInsert.Parameters.AddWithValue("@Nik", nik)

                    commandInsert.Parameters.AddWithValue("@PhoneNumber", phoneNumber)

                    commandInsert.Parameters.AddWithValue("@Address", address)

                    commandInsert.Parameters.AddWithValue("@Work", work)

                    commandInsert.Parameters.AddWithValue("@BirthDate", birthDate)

                    commandInsert.Parameters.AddWithValue("@Gender", gender)

                    commandInsert.Parameters.AddWithValue("@CategorySpecialist", categorySpecialist)

                    commandInsert.Parameters.AddWithValue("@NoQueue", noQueue)


                    commandInsert.ExecuteNonQuery()

                End Using

            End Using

            MessageBox.Show("Data berhasil disimpan.", "Info", MessageBoxButton.OK, MessageBoxImage.Information)

            ClearForm()

        Catch ex As Exception

            MessageBox.Show($"Terjadi kesalahan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

    End Sub

    ' Clear Data Form
    Private Sub ClearForm()

        NameInput.Text = String.Empty

        NikInput.Text = String.Empty

        PhoneNumberInput.Text = String.Empty

        AddressInput.Text = String.Empty

        WorkInput.Text = String.Empty

        BirthDateInput.SelectedDate = Nothing

        GenderMaleRadioButton.IsChecked = False

        GenderFemaleRadioButton.IsChecked = False

        CategorySpecialistInput.SelectedIndex = -1

        NoQueueInput.Text = String.Empty

    End Sub

    ' Button Reset Form
    Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs) Handles CancelButton.Click

        ClearForm()

    End Sub

End Class

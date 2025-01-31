Imports MySql.Data.MySqlClient
Imports System.Data

Public Class UserControlPemesanan

    Private ReadOnly medicinePrices As New Dictionary(Of String, Decimal) From {
        {"Paracetamol", 15000D},
        {"Mylanta", 12000D},
        {"Aclidinium", 18000D},
        {"Amaryl", 22000D},
        {"Ambroxol Indofarma", 25000D},
        {"Amlodipine", 20000D}
    }

    Private totalPrice As Decimal = 0

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)

        Dim parentWindow As Window = Window.GetWindow(Me)

        If parentWindow IsNot Nothing Then

            parentWindow.Close()

        End If

    End Sub

    Private Sub AddMedicine(medicineName As String)

        If medicinePrices.ContainsKey(medicineName) Then

            Dim price As Decimal = medicinePrices(medicineName)

            ListOrder.Items.Add($"{medicineName} - Rp {price:N0}")

            totalPrice += price

            TotalPriceOrder.Text = $"Rp {totalPrice:N0}"

        End If

    End Sub

    Private Sub ButtonMedician1_Click(sender As Object, e As RoutedEventArgs) Handles ButtonMedician1.Click
        AddMedicine("Paracetamol")
    End Sub

    Private Sub ButtonMedician2_Click(sender As Object, e As RoutedEventArgs) Handles ButtonMedician2.Click
        AddMedicine("Mylanta")
    End Sub

    Private Sub ButtonMedician3_Click(sender As Object, e As RoutedEventArgs) Handles ButtonMedician3.Click
        AddMedicine("Aclidinium")
    End Sub

    Private Sub ButtonMedician4_Click(sender As Object, e As RoutedEventArgs) Handles ButtonMedician4.Click
        AddMedicine("Amaryl")
    End Sub

    Private Sub ButtonMedician5_Click(sender As Object, e As RoutedEventArgs) Handles ButtonMedician5.Click
        AddMedicine("Ambroxol Indofarma")
    End Sub

    Private Sub ButtonMedician6_Click(sender As Object, e As RoutedEventArgs) Handles ButtonMedician6.Click
        AddMedicine("Amlodipine")
    End Sub

    Private Sub ButtonSubmit_Click(sender As Object, e As RoutedEventArgs) Handles ButtonSubmit.Click

        Dim name As String = NameInput.Text.Trim()

        Dim address As String = AddresInput.Text.Trim()

        Dim phoneNumber As String = PhoneNumberInput.Text.Trim()


        If String.IsNullOrEmpty(name) Then

            MessageBox.Show("Nama harus diisi!", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

            Return

        End If

        Try

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()

                    Dim query As String = "INSERT INTO order_detail (name, address, phone_number, total_payment, date) VALUES (@name, @address, @phoneNumber, @totalPayment, NOW())"

                    Using command As New MySqlCommand(query, connection)

                        command.Parameters.AddWithValue("@name", name)

                        command.Parameters.AddWithValue("@address", If(String.IsNullOrEmpty(address), DBNull.Value, address))

                        command.Parameters.AddWithValue("@phoneNumber", If(String.IsNullOrEmpty(phoneNumber), DBNull.Value, phoneNumber))

                        command.Parameters.AddWithValue("@totalPayment", $"Rp {totalPrice:N0}")

                        command.ExecuteNonQuery()

                    End Using

                End Using

            End Using

            MessageBox.Show("Pesanan berhasil disimpan!", "Success", MessageBoxButton.OK, MessageBoxImage.Information)

            ClearForm()

            LoadHistory()

        Catch ex As Exception

            MessageBox.Show($"Gagal menyimpan pesanan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

    End Sub

    Private Sub LoadHistory()

        Try

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()

                    Dim query As String = "SELECT name, address, phone_number, total_payment, date FROM order_detail ORDER BY date DESC"

                    Dim command As New MySqlCommand(query, connection)

                    Dim reader As MySqlDataReader = command.ExecuteReader()

                    Dim dt As New DataTable()

                    dt.Load(reader)

                    ConsultationQueueGrid.ItemsSource = dt.DefaultView

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show($"Gagal memuat riwayat pesanan: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error)

        End Try

    End Sub

    Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        LoadHistory()

    End Sub

    Private Sub ClearForm()

        NameInput.Text = String.Empty

        PhoneNumberInput.Text = String.Empty

        AddresInput.Text = String.Empty

        TotalPriceOrder.Text = String.Empty

        ListOrder.Items.Clear()

    End Sub

End Class
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class UserControlDashboardDetail

    Private _categorySpecialistId As Integer

    Public Sub New(categorySpecialistId As Integer)

        InitializeComponent()

        _categorySpecialistId = categorySpecialistId

        LoadData()

    End Sub

    Private Sub LoadData()

        Try

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()


                    Dim query As String = "SELECT * FROM consultation_queue WHERE category_specialist_id = @categorySpecialistId AND DATE(created_at) = CURDATE() AND status = 'waiting' ORDER BY no_queue ASC"

                    Dim command As New MySqlCommand(query, connection)

                    command.Parameters.AddWithValue("@categorySpecialistId", _categorySpecialistId)


                    Dim reader As MySqlDataReader = command.ExecuteReader()

                    Dim dt As New DataTable()


                    dt.Load(reader)


                    TotalQueue.Text = dt.Rows.Count.ToString()

                    If dt.Rows.Count > 0 Then

                        QueueNow.Text = dt.Rows(0)("no_queue").ToString()

                    End If

                    ConsultationQueueGrid.ItemsSource = dt.DefaultView

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show($"Error loading data: {ex.Message}")

        End Try

    End Sub

    Private Sub ButtonSkip_Click(sender As Object, e As RoutedEventArgs)

        Try

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()


                    Dim updateQuery As String = "UPDATE consultation_queue SET status = 'skipped' WHERE status = 'waiting' AND category_specialist_id = @categorySpecialistId ORDER BY no_queue ASC LIMIT 1"

                    Dim command As New MySqlCommand(updateQuery, connection)

                    command.Parameters.AddWithValue("@categorySpecialistId", _categorySpecialistId)


                    command.ExecuteNonQuery()

                    LoadData()

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show($"Error updating data: {ex.Message}")

        End Try

    End Sub

    Private Sub ButtonFinish_Click(sender As Object, e As RoutedEventArgs)

        Try

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()

                    Dim updateQuery As String = "UPDATE consultation_queue SET status = 'finished' WHERE status = 'waiting' AND category_specialist_id = @categorySpecialistId ORDER BY no_queue ASC LIMIT 1"

                    Dim command As New MySqlCommand(updateQuery, connection)

                    command.Parameters.AddWithValue("@categorySpecialistId", _categorySpecialistId)


                    command.ExecuteNonQuery()

                    LoadData()

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show($"Error updating data: {ex.Message}")

        End Try

    End Sub

    Private Sub CloseButton_Click(sender As Object, e As RoutedEventArgs)

        Dim parentWindow As Window = Window.GetWindow(Me)

        If parentWindow IsNot Nothing Then

            parentWindow.Close()

        End If

    End Sub

End Class

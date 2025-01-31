Imports MySql.Data.MySqlClient

Public Class ApplicationDbContext
    Implements IDisposable

    Private _connection As MySqlConnection

    Public Sub New()

        _connection = New MySqlConnection("server=localhost;userid=root;password=;database=e_health")

    End Sub

    Public Function GetConnection() As MySqlConnection

        Return _connection

    End Function

    Public Sub Dispose() Implements IDisposable.Dispose

        If _connection IsNot Nothing Then

            _connection.Dispose()

            _connection = Nothing

        End If

    End Sub

End Class

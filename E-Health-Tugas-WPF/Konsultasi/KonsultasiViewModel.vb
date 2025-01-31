Imports System.Collections.ObjectModel

Imports System.ComponentModel

Imports MySql.Data.MySqlClient

Public Class KonsultasiViewModel

    Implements INotifyPropertyChanged

    Private _dateTimeNow As String


    Private _comboBoxLists As ObservableCollection(Of SpecialistComboBox)

    Public Property ComboBoxLists As ObservableCollection(Of SpecialistComboBox)

        Get
            Return _comboBoxLists

        End Get

        Set(value As ObservableCollection(Of SpecialistComboBox))

            _comboBoxLists = value

            OnPropertyChanged(NameOf(ComboBoxLists))

        End Set

    End Property

    Private _selectedSpecialist As SpecialistComboBox



    Public Property SelectedSpecialist As SpecialistComboBox

        Get

            Return _selectedSpecialist

        End Get

        Set(value As SpecialistComboBox)

            _selectedSpecialist = value

            OnPropertyChanged(NameOf(SelectedSpecialist))

            If _selectedSpecialist IsNot Nothing Then

                MessageBox.Show($"Selected Specialist: {_selectedSpecialist.Name}")

            End If

        End Set

    End Property

    Public Sub New()

        ComboBoxLists = New ObservableCollection(Of SpecialistComboBox)()

        Load()

        DateTimeNow = DateTime.Now.ToString("dddd, dd MMMM yyyy | hh:mm tt")

    End Sub

    'Get data for ComboBox & No.Queue
    Private Sub Load()

        Try

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()

                    Dim query As String = "SELECT id, name FROM category_specialist"
                    Dim command As New MySqlCommand(query, connection)

                    Using reader As MySqlDataReader = command.ExecuteReader()

                        While reader.Read()

                            Dim specialist As New SpecialistComboBox With {
                                .Id = Convert.ToInt32(reader("id")),
                                .Name = reader("name").ToString()
                            }
                            ComboBoxLists.Add(specialist)

                        End While

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show($"Error loading data: {ex.Message}")

        End Try

    End Sub



    'ComboBox
    Private Sub MyComboBox_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        Dim comboBox As ComboBox = CType(sender, ComboBox)

        Dim selectedItem As ComboBoxItem = CType(comboBox.SelectedItem, ComboBoxItem)

        MessageBox.Show($"Selected: {selectedItem.Content}")

    End Sub

    'Time
    Public Property DateTimeNow As String

        Get

            Return _dateTimeNow

        End Get

        Set(value As String)

            _dateTimeNow = value

            OnPropertyChanged(NameOf(DateTimeNow))

        End Set

    End Property

    Private Sub UpdateDateTimeNow(sender As Object, e As EventArgs)

        DateTimeNow = DateTime.Now.ToString("dddd, dd MMMM yyyy | hh:mm tt")

    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(propertyName As String)

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))

    End Sub

End Class

Public Class SpecialistComboBox
    Public Property Id As Integer
    Public Property Name As String
End Class
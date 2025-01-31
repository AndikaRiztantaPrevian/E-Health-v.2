Imports System.Collections.ObjectModel

Imports System.ComponentModel

Imports System.Windows.Threading

Imports MySql.Data.MySqlClient

Public Class DashboardDataViewModel
    Implements INotifyPropertyChanged

    Public Property Specialists As ObservableCollection(Of SpecialistCard)
    Public ReadOnly Property ItemClickedCommand As ICommand

    Private _dateTimeNow As String
    Private ReadOnly _timer As DispatcherTimer

    Public Sub New()

        Specialists = New ObservableCollection(Of SpecialistCard)()

        LoadData()

        ItemClickedCommand = New RelayCommand(AddressOf OnItemClicked)


        DateTimeNow = DateTime.Now.ToString("dddd, dd MMMM yyyy | hh:mm tt")

        _timer = New DispatcherTimer With {.Interval = TimeSpan.FromSeconds(1)}

        AddHandler _timer.Tick, AddressOf UpdateDateTimeNow

        _timer.Start()

    End Sub

    Private Sub OnItemClicked(parameter As Object)

        Dim clickedSpecialist = TryCast(parameter, SpecialistCard)

        If clickedSpecialist IsNot Nothing Then

            Dim mainWindow = CType(Application.Current.MainWindow, Dashboard)

            mainWindow.MainContent.Content = New UserControlDashboardDetail(clickedSpecialist.SelectedIdCategory)

        End If

    End Sub

    Private Sub LoadData()

        Try

            Using context As New ApplicationDbContext()

                Using connection As MySqlConnection = context.GetConnection()

                    connection.Open()

                    Dim query As String = "SELECT id, name, start_time, end_time FROM category_specialist"
                    Dim command As New MySqlCommand(query, connection)

                    Using reader As MySqlDataReader = command.ExecuteReader()

                        While reader.Read()

                            Specialists.Add(New SpecialistCard With {
                                .SelectedIdCategory = reader("id"),
                                .Name = reader("name").ToString(),
                                .StartTime = TimeSpan.Parse(reader("start_time").ToString()),
                                .EndTime = TimeSpan.Parse(reader("end_time").ToString())
                            })

                        End While

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show($"Error loading data: {ex.Message}")

        End Try

    End Sub

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

Public Class SpecialistCard
    Public Property Name As String
    Public Property StartTime As TimeSpan
    Public Property EndTime As TimeSpan
    Public Property SelectedIdCategory As Integer

    Public ReadOnly Property StatusColor As Brush

        Get

            Dim now = DateTime.Now.TimeOfDay

            If now >= StartTime AndAlso now <= EndTime Then

                Return New SolidColorBrush(ColorConverter.ConvertFromString("#00D440"))

            Else

                Return New SolidColorBrush(ColorConverter.ConvertFromString("#FFFFFF"))

            End If

        End Get

    End Property

    Public ReadOnly Property TimeRange As String

        Get

            Return $"{StartTime:hh\:mm} - {EndTime:hh\:mm}"

        End Get

    End Property

End Class

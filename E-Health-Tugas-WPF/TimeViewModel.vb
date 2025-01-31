Imports System.ComponentModel

Imports System.Windows.Threading

Public Class TimeViewModel

    Implements INotifyPropertyChanged

    Private _dateTimeNow As String
    Public Property DateTimeNow As String

        Get

            Return _dateTimeNow

        End Get

        Set(value As String)

            _dateTimeNow = value

            OnPropertyChanged(NameOf(DateTimeNow))

        End Set

    End Property

    Private timer As DispatcherTimer

    Public Sub New()

        timer = New DispatcherTimer()

        timer.Interval = TimeSpan.FromSeconds(1)

        AddHandler timer.Tick, AddressOf UpdateDateTime

        timer.Start()


        UpdateDateTime(Nothing, Nothing)

    End Sub

    Private Sub UpdateDateTime(sender As Object, e As EventArgs)

        DateTimeNow = DateTime.Now.ToString("dddd, dd MMMM yyyy | hh:mm tt")

    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Protected Sub OnPropertyChanged(propertyName As String)

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))

    End Sub

End Class

Imports System.Globalization

Imports System.Threading

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    Protected Overrides Sub OnStartup(e As StartupEventArgs)

        MyBase.OnStartup(e)

        Thread.CurrentThread.CurrentCulture = New CultureInfo("id-ID")

        Thread.CurrentThread.CurrentUICulture = New CultureInfo("id-ID")

    End Sub

End Class

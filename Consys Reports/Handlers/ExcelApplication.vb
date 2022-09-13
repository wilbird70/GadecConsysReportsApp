'Gadec Engineerings Software (c) 2020

''' <summary>
''' <para><see cref="ExcelApplication"/> creates a new excel proces and provides methods create or open excel documents.</para>
''' </summary>
Public Class ExcelApplication
    Implements IDisposable
    Private _disposed As Boolean

    ''' <summary>
    ''' Contains the process of the present excel application.
    ''' </summary>
    Private ReadOnly _application As Object
    ''' <summary>
    ''' Contains the starttime of creating the process.
    ''' </summary>
    Private ReadOnly _startTime As Date
    ''' <summary>
    ''' Contains the endtime of creating the process.
    ''' </summary>
    Private ReadOnly _endTime As Date

    'class

    ''' <summary>
    ''' Initializes a new instance of <see cref="ExcelApplication"/>.
    ''' <para><see cref="ExcelApplication"/> creates a new process with the excel application and provides methods to create or open excel documents.</para>
    ''' </summary>
    Sub New()
        _startTime = Date.Now
        Try
            _application = CreateObject("Excel.Application")
        Catch ex As Exception
            _application = Nothing
        Finally
            _endTime = Date.Now
        End Try
    End Sub

    ''' <summary>
    ''' Disposing this <see cref="ExcelApplication"/> will close the excel application and stops any excel process started in it.
    ''' </summary>
    Protected Overridable Sub Dispose(disposing As Boolean)
        If _disposed Then Exit Sub

        If disposing Then
            'dispose managed state (managed objects) not needed
        End If

        _application.Quit()
        Dim processes = Process.GetProcessesByName("EXCEL")
        For Each p As Process In processes
            If p.StartTime >= _startTime And p.StartTime <= _endTime Then
                p.Kill()
                Exit For
            End If
        Next
        _disposed = True
    End Sub

    ''' <summary>
    ''' Overrides the finalize method.
    ''' </summary>
    Protected Overrides Sub Finalize()
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=False)
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' Implements the dispose method.
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub

    'functions

    ''' <summary>
    ''' Determines whether the process is started successfully.
    ''' </summary>
    ''' <returns>True process is available.</returns>
    Function StartedSuccessfully() As Boolean
        Return NotNothing(_application)
    End Function

    ''' <summary>
    ''' Gets the process of the excel application.
    ''' </summary>
    ''' <returns>The process.</returns>
    Function GetApplication() As Object
        Return _application
    End Function

    ''' <summary>
    ''' Creates a new instance of <see cref="ExcelDocument"/> that contains a new excel document.
    ''' </summary>
    ''' <returns></returns>
    Function NewDocument() As ExcelDocument
        Return New ExcelDocument(_application)
    End Function

    ''' <summary>
    ''' Creates a new instance of <see cref="ExcelDocument"/> that contains the excel document with the provided filename.
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <returns>A <see cref="ExcelDocument"/>.</returns>
    Function OpenDocument(fileName As String) As ExcelDocument
        Return New ExcelDocument(_application, fileName)
    End Function

End Class

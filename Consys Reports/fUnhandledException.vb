'Gadec Engineerings Software (c) 2020

Public Class fUnhandledException
    Private _Text As String
    Private _FormHeight As Integer
    Private _FormWidth As Integer
    Private _LoadedDia As Boolean

    'form
    Sub New(exception As Exception, methodeName As String, appBuild As String, fileName As String)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        Dim timeOccured = Format(Now, "dd-MM-yyyy - HH:mm:ss")
        Dim text = "#Occured on:{T}{0}$$$#Occured in:{T}{1}$$$#Catched in:{T}{2}$$$#Gadec build:{T}{3}$$$#Message:{T}{4}$$$#Active document:{T}{5}$$$#Stacktrace:$$${6}$$$"
        _Text = text.Param(timeOccured, exception.TargetSite.Name, methodeName, appBuild, exception.Message, fileName, exception.StackTrace)
        Try
            sCreateFolderIfNotExists("{AppData}".Param)
            Using streamWriter = IO.File.AppendText("{AppData}\LogMessageException.log".Param)
                streamWriter.WriteLine(_Text.Replace("$$$", vbCrLf))
            End Using
        Catch ex As IO.FileNotFoundException
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
        Select Case _Language
            Case "NL"
                lText1.Text = "Er Is een onverwerkte fout opgetreden:"
                lText2.Text = "Laten we een bericht maken voor de ontwikkelaar."
                ltSend.Text = "Verzenden"
                ltClose.Text = "Sluiten"
            Case "DE"
                lText1.Text = "Es ist ein nicht behandelter Fehler aufgetreten:"
                lText2.Text = "Lassen Sie uns eine Nachricht an den Entwickler senden."
                ltSend.Text = "Senden"
                ltClose.Text = "Schließen"
            Case "FR"
                lText1.Text = "Une erreur non gérée s'est produite:"
                lText2.Text = "Faisons un message pour le développeur."
                ltSend.Text = "Envoyer"
                ltClose.Text = "Fermer"
            Case Else
                lText1.Text = "Unhandled error has occurred:"
                lText2.Text = "Let's make a message for the developer."
                ltSend.Text = "Send"
                ltClose.Text = "Close"
        End Select
        tTextbox.Text = _Text.Replace("$$$", vbLf)
        _FormHeight = Me.Height
        _FormWidth = Me.Width
        _LoadedDia = True
        Me.Text = fApplicationVersion()
        Me.TopMost = True
        Me.Show()
        Beep()
    End Sub

    Private Sub Me_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
        Try
            sResizeForm(Me.Width, Me.Height)
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    'buttons

    Private Sub cOK_Click(sender As Object, e As EventArgs) Handles ltSend.Click
        Try
            Dim text = "{0}{P}{P}".Param(_Text.Replace("$$$", "{P}").Replace(vbLf, "{P}").Param)
            sStartMailMessage("Report Unhandled Exception {0}".Param(fApplicationVersion), text)
            Me.Hide()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub cCancel_Click(sender As Object, e As EventArgs) Handles ltClose.Click
        Try
            Me.Hide()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    'private subs

    Private Sub sResizeForm(width As Integer, height As Integer)
        If _LoadedDia Then
            Dim hgt = {158, height}.Max
            Dim wdt = {300, width}.Max
            Me.Height = hgt
            Me.Width = wdt

            Dim div1 = hgt - _FormHeight
            If Not div1 = 0 Then
                ltSend.Top += div1
                ltClose.Top += div1
                tTextbox.Height += div1
                lText2.Top += div1
            End If

            Dim div2 = wdt - _FormWidth
            If Not div2 = 0 Then
                ltClose.Left += div2
                ltSend.Left += div2
                tTextbox.Width += div2
                'tImage.Left += div2
            End If

            _FormHeight = Me.Height
            _FormWidth = Me.Width
        End If
    End Sub

End Class
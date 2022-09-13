'Gadec Engineerings Software (c) 2020

''' <summary>
''' <para><see cref="Main"/> provides the startup (main) dialogbox for this app.</para>
''' </summary>
Public Class Main
    ''' <summary>
    ''' Determines if is being translated, so no change of language (with dropdownbox) is accepted.
    ''' <para>Note: Especially important when loading the dialogbox.</para>
    ''' </summary>
    Private _translationIsBusy As Boolean = True

    'form

    ''' <summary>
    ''' EventHandler for the event that occurs when the dialogbox is loading.
    ''' <para>It initializes the instance of <see cref="Main"/>.</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub MyBase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim companyName = "Gadec"
            Dim appName = "ConsysReports App"
            Registerizer.Initialize(companyName, appName, "{0}\Settings".Compose(appName))
            Dim customCodes = New Dictionary(Of String, String) From {
                {"AppDir", Registerizer.MainSetting("AppDir")},
                {"Support", Registerizer.MainSetting("AppDir") & "\Support"},
                {"AppDataFolder", "{AppData}\{0}\{1}".Compose(companyName, appName)}
            }
            Composer.SetCustumCodes(customCodes)

            AddHandler Translator.LanguageChangedEvent, AddressOf LanguageChangedEventHandler
            Translator.Initialize("{Support}\SetLanguages.xml".Compose)

            Me.Text = Registerizer.GetApplicationVersion
            FileSystemHelper.CreateFolder("{AppDataFolder}".Compose)
        Catch ex As Exception
            MessageBoxException(ex)
        End Try
    End Sub

    'buttons

    ''' <summary>
    ''' EventHandler for the event that occurs when the user clicks the SelectFiles button.
    ''' <para>Allows the user to select report files and instantly creates and starts an excel report document.</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SelectFilesButton_Click(sender As Object, e As EventArgs) Handles ltSelectFiles.Click
        Try
            Dim initialFolder = Registerizer.UserSetting("InitialFolder")
            If Not IO.Directory.Exists(initialFolder) Then initialFolder = "{Desktop}".Compose

            ProgressLabel.Text = "lblOpeningCSV".Translate
            Dim dialog = New OpenFileDialog With {
                .Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*",
                .Multiselect = True,
                .InitialDirectory = initialFolder,
                .FilterIndex = 1,
                .RestoreDirectory = True
            }
            If Not dialog.ShowDialog() = DialogResult.OK Then ProgressLabel.Text = "..." : Exit Sub

            Dim folder = IO.Path.GetDirectoryName(dialog.FileName)
            Registerizer.UserSetting("InitialFolder", folder)

            Dim progress = New Progress(Of ProgressEventArgs)
            AddHandler progress.ProgressChanged, AddressOf ProgressImportEventHandler
            Dim reports = New Report(dialog.FileNames, progress)
            ProjectLabel.Text = "Project".Translate(reports.Project)
            reports.Create()

            If IO.File.Exists(reports.FileName) Then
                Dim process = ProcessHelper.StartDocument(reports.FileName)
                process.WaitForExit(2000)
            End If
            Me.Dispose()
        Catch ex As Exception
            MessageBoxException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' EventHandler for the event that occurs when the user clicks the close button.
    ''' <para>It closes the app.</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles ltClose.Click
        Try
            Me.Dispose()
        Catch ex As Exception
            MessageBoxException(ex)
        End Try
    End Sub

    'comboboxes

    ''' <summary>
    ''' EventHandler for the event that occurs when the user changes the selected language.
    ''' <para>It changes the language setting and starts translating (by raising an event).</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LanguageComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LanguageComboBox.SelectedIndexChanged
        Try
            If _translationIsBusy Then Exit Sub

            Translator.SetLanguange(LanguageComboBox.SelectedIndex)
        Catch ex As Exception
            MessageBoxException(ex)
        End Try
    End Sub

    'eventhandlers

    ''' <summary>
    ''' EventHandler for the event that occurs when the user changes the language.
    ''' <para>It will translate this dialog.</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LanguageChangedEventHandler(sender As Object, e As LanguageChangedEventArgs)
        'image
        Dim flagImageFileName = "{Support}\Lang_{0}.png".Compose(e.Selected)
        LanguagePictureBox.Image = Image.FromFile(flagImageFileName)
        'wireup combobox
        _translationIsBusy = True
        Dim languageKeys = e.AvialableLanguages
        If languageKeys.Count > 0 Then
            Dim languageIndex = 0
            LanguageComboBox.Items.Clear()
            For Each k In languageKeys
                LanguageComboBox.Items.Add(("Lang_{0}".Compose(k)).Translate)
                If k = e.Selected Then languageIndex = LanguageComboBox.Items.Count - 1
            Next
            If LanguageComboBox.Items.Count > 0 Then LanguageComboBox.SelectedIndex = languageIndex
        End If
        _translationIsBusy = False
        TranslateControles(Me)
    End Sub

    ''' <summary>
    ''' EventHandler for the event that occurs when the associated progress reports a change.
    ''' <para>It displays the provided text on the dialog.</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ProgressImportEventHandler(ByVal sender As Object, ByVal e As ProgressEventArgs)
        ProgressLabel.Text = e.Description
        ProgressLabel.Refresh()
    End Sub

    'private functions

    ''' <summary>
    ''' Translates the controls associated with the specified parent.
    ''' </summary>
    ''' <param name="parent">The parent control.</param>
    Private Sub TranslateControles(ByVal parent As Control)
        For Each control In parent.Controls.ToArray
            If control.Name.StartsWith("lt") Then control.Text = control.Name.Translate
            If control.HasChildren Then TranslateControles(control)
        Next
    End Sub

End Class
'Gadec Engineerings Software (c) 2020

''' <summary>
''' <para><see cref="Report"/> reads the report files and can create the reports document.</para>
''' </summary>
Public Class Report
    ''' <summary>
    ''' The projectname found in the first report.
    ''' </summary>
    ''' <returns>The projectname.</returns>
    Public Property Project As String = ""
    Public Property FileName As String = ""

    ''' <summary>
    ''' The present folder.
    ''' </summary>
    Private ReadOnly _folder As String
    ''' <summary>
    ''' The progress provider that tracks the progress of creating the reports document.
    ''' </summary>
    Private ReadOnly _progress As IProgress(Of ProgressEventArgs)
    ''' <summary>
    ''' A dictionary containing the report data.
    ''' </summary>
    ''' <returns>The dictionary.</returns>
    Private ReadOnly _reportData As New Dictionary(Of String, String)
    ''' <summary>
    ''' A database with the extract information.
    ''' </summary>
    Private ReadOnly _extractData As DataTable
    ''' <summary>
    ''' The list with all panelnumbers.
    ''' </summary>
    Private ReadOnly _panelNumbers As New List(Of String) From {" ", " "}
    ''' <summary>
    ''' A dictionary containing the totals of devices per panel.
    ''' </summary>
    Private ReadOnly _totalDevices As New Dictionary(Of String, Dictionary(Of String, Integer))
    ''' <summary>
    ''' A dictionary containing the totals of modes per panel.
    ''' </summary>
    Private ReadOnly _totalModes As New Dictionary(Of String, Dictionary(Of String, Integer))
    ''' <summary>
    ''' A dictionary containing the totals of devices per loop.
    ''' </summary>
    Private ReadOnly _loopDevices As New Dictionary(Of String, Integer())

    'class

    ''' <summary>
    ''' Initializes a new instance of <see cref="Report"/>.
    ''' <para><see cref="Report"/> reads the report files and can create the reports document.</para>
    ''' </summary>
    ''' <param name="fileNames">A list of fullnames of the reportfiles (csv).</param>
    ''' <param name="progress">The progress provider that tracks the progress of creating the reports document.</param>
    Public Sub New(fileNames As String(), progress As IProgress(Of ProgressEventArgs))
        If fileNames.Count = 0 Then Exit Sub

        _folder = IO.Path.GetDirectoryName(fileNames.FirstOrDefault)
        _progress = progress
        _extractData = DataSetHelper.LoadFromXml("{Support}\SetExtractInfo.xml".Compose).GetTable("LoopTypes", "Name")
        For Each file In fileNames
            ReadReport(file)
        Next
        If _totalDevices.Count > 0 Then
            Dim title = "Total Panel Pointlist - {0}".NotYetTranslated(_Project)
            _reportData.Add("#PanelPointslist;Total", String.Join(vbCr, CountingPage(title, _panelNumbers.ToArray, _totalDevices, _totalModes)))
        End If
        If _loopDevices.Count > 0 Then
            Dim title = "LoopConfigurationTitle".Translate(_Project)
            Dim header = "LoopConfigurationHeader".Translate
            _reportData.Add("#Loopconfiguration;Total", String.Join(vbCr, LoopConfiguration(title, header, _loopDevices)))
        End If
    End Sub

    ''' <summary>
    ''' Creates the reports document.
    ''' </summary>
    Public Sub Create()
        _FileName = "{0}\ConsysReports ({1}).xlsx".Compose(_folder, _Project)
        If FileHelper.BackUp(_FileName) = "" Then Exit Sub

        ReportProgress("lblStarting".Translate)
        Using excelApplication = New ExcelApplication
            If Not excelApplication.StartedSuccessfully Then Exit Sub

            ReportProgress("lblOpeningTemplate".Translate)
            Application.DoEvents()
            Using excelWorkbook = excelApplication.OpenDocument("{Support}\Template.xlsx".Compose)
                If Not excelWorkbook.OpenedSuccessfully Then Exit Sub

                ReportProgress("lblImporting".Translate)
                Application.DoEvents()
                excelWorkbook.ImportPages(_reportData, _progress)
                If Not excelWorkbook.ImportedSuccessfully Then Exit Sub

                ReportProgress("lblSaving".Translate(IO.Path.GetFileName(_FileName)))
                Application.DoEvents()
                excelWorkbook.Save(_FileName)
            End Using
            ReportProgress("lblQuiting".Translate)
            Application.DoEvents()
        End Using
        ReportProgress("lblOpeningReport".Translate(IO.Path.GetFileName(_FileName)))
        Application.DoEvents()
    End Sub

    'private subs

    ''' <summary>
    ''' Reports a change via the progress provider.
    ''' </summary>
    ''' <param name="description">The description to display in the main form.</param>
    Private Sub ReportProgress(description As String)
        _progress.Report(New ProgressEventArgs(description))
        Application.DoEvents()
    End Sub

    ''' <summary>
    ''' Reads the specified reportfile.
    ''' </summary>
    ''' <param name="fileName">The fullname of a reportfile (csv).</param>
    Private Sub ReadReport(fileName As String)
        Dim panelDevices = New Dictionary(Of String, Dictionary(Of String, Integer))
        Dim panelModes = New Dictionary(Of String, Dictionary(Of String, Integer))
        Dim sections = New List(Of String) From {" ", " "}
        Dim sourceData = IO.File.ReadAllLines(fileName).ToList
        Dim targetData = New List(Of String)
        Dim panel = "-"
        Dim region = "-"
        Dim category = "-"
        Dim type = "-"
        For i = 0 To sourceData.Count - 1
            Dim row = sourceData(i).Replace("{Q},{Q}".Compose, "$%$").Replace("{Q},".Compose, "").Replace("{Q}".Compose, "").Cut("$%$")
            Select Case True
                Case row.Count = 1
                    Continue For
                Case row.Count = 2
                    Select Case True
                        Case type = "-"
                            type = row(0).InStrResult("", " - ", "-")
                            If Not type = "-" Then
                                panel = row(0).InStrResult(" - Panel ", "(", "Gen")
                                If Not _panelNumbers.Contains("Panel".Translate(panel)) Then _panelNumbers.Add("Panel".Translate(panel))
                                If _Project = "" Then _Project = row(0).InStrResult(" - ", " - Panel ")
                            End If
                        Case Else
                            region = GetRegion(row(0))
                            category = GetCategory(row(0))
                            Select Case True
                                Case Not region = "Loop"
                                Case Not sections.Contains("Loop".Translate(category)) : sections.Add("Loop".Translate(category))
                            End Select
                    End Select
                Case row.Count = 4
                    If row(0).StartsWith("##") Then Continue For
                Case Else
                    If Not region = "-" Then
                        row(0) = region
                        row(2) = "{0}{1}".Compose(category, ("000{0}".Compose(row(2))).RightString(3))
                    End If
            End Select
            If Not (region = "" Or category = "") Then
                Dim newLine = New List(Of String)
                For j = 0 To row.Count - 1 Step 2
                    Select Case row(j)
                        Case "True" : row(j) = "■"
                        Case "False" : row(j) = "□"
                        Case "None" : row(j) = "-"
                    End Select
                    newLine.Add(row(j))
                Next
                For j = newLine.Count To 39
                    newLine.Add("")
                Next
                newLine.Add(" ")
                Select Case True
                    Case Not type = "Points Report"
                    Case newLine(1) = ""
                    Case Else
                        Dim channel = newLine(2)
                        Dim device = newLine(5)
                        'lijsten "per device"
                        If channel = "" Or channel = "1" Then
                            'per paneel
                            Select Case True
                                Case Not region = "Loop"
                                Case Not panelDevices.ContainsKey(device) : panelDevices.Add(device, New Dictionary(Of String, Integer) From {{"Loop".Translate(category), 1}})
                                Case panelDevices(device).ContainsKey("Loop".Translate(category)) : panelDevices(device)("Loop".Translate(category)) += 1
                                Case Else : panelDevices(device).Add("Loop".Translate(category), 1)
                            End Select
                            'totaal
                            Select Case True
                                Case Not _totalDevices.ContainsKey(device) : _totalDevices.Add(device, New Dictionary(Of String, Integer) From {{"Panel".Translate(panel), 1}})
                                Case _totalDevices(device).ContainsKey("Panel".Translate(panel)) : _totalDevices(device)("Panel".Translate(panel)) += 1
                                Case Else : _totalDevices(device).Add("Panel".Translate(panel), 1)
                            End Select
                            ''''Creëren looplijst per paneel
                            Dim extractRow = _extractData.Rows.Find(device)
                            Dim key = "P{0};{1};{2}".Compose(panel, category, "000{0}".Compose(newLine(13)).RightString(3))
                            Select Case True
                                Case Not region = "Loop"
                                Case IsNothing(extractRow)
                                Case Not _loopDevices.ContainsKey(key)
                                    _loopDevices.Add(key, {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0})
                                    For Each loopItem In extractRow.GetString("LoopItem").Cut
                                        _loopDevices(key)(loopItem.ToInteger) += 1
                                    Next
                                Case Else
                                    For Each loopItem In extractRow.GetString("LoopItem").Cut
                                        _loopDevices(key)(loopItem.ToInteger) += 1
                                    Next
                            End Select
                        End If
                        'lijsten "per address"
                        Dim mode = "{0};{1}".Compose(newLine(5), newLine(7))
                        If Not device = "Device" Then
                            'per paneel
                            Select Case True
                                Case Not region = "Loop"
                                Case Not panelModes.ContainsKey(mode) : panelModes.Add(mode, New Dictionary(Of String, Integer) From {{"Loop".Translate(category), 1}})
                                Case panelModes(mode).ContainsKey("Loop".Translate(category)) : panelModes(mode)("Loop".Translate(category)) += 1
                                Case Else : panelModes(mode).Add("Loop".Translate(category), 1)
                            End Select
                            'totaal
                            Select Case True
                                Case Not _totalModes.ContainsKey(mode) : _totalModes.Add(mode, New Dictionary(Of String, Integer) From {{"Panel".Translate(panel), 1}})
                                Case _totalModes(mode).ContainsKey("Panel".Translate(panel)) : _totalModes(mode)("Panel".Translate(panel)) += 1
                                Case Else : _totalModes(mode).Add("Panel".Translate(panel), 1)
                            End Select
                        End If
                End Select
                targetData.Add(String.Join(vbTab, newLine.ToArray))
            End If
        Next
        _reportData.Add("{0};P{1}".Compose(type, panel), String.Join(vbCr, targetData.ToArray))
        If panelDevices.Count > 0 Then
            Dim title = "Total Loopdevices - {0} - Panel {1}".Compose(_Project, panel)
            _reportData.Add("#LoopPointslist;P{0}".Compose(panel), String.Join(vbCr, CountingPage(title, sections.ToArray, panelDevices, panelModes)))
        End If
    End Sub

    'private functions

    ''' <summary>
    ''' Creates the data for a counting page (devices and modes per loop or panel).
    ''' </summary>
    ''' <param name="title">The page title.</param>
    ''' <param name="headers">The page headers.</param>
    ''' <param name="devices">A dictionary containing the totals of devices (per loop or panel).</param>
    ''' <param name="modes">A dictionary containing the totals of modes (per loop or panel).</param>
    ''' <returns>The page data.</returns>
    Private Function CountingPage(title As String, headers As String(),
                                 devices As Dictionary(Of String, Dictionary(Of String, Integer)),
                                 modes As Dictionary(Of String, Dictionary(Of String, Integer))) As String()
        Dim output = New List(Of String) From {title, String.Join(vbTab, headers), "PerDevice".Translate}
        Dim deviceCounterKeys = devices.Keys.ToList
        deviceCounterKeys.Sort()
        For Each k In deviceCounterKeys
            Dim deviceCounter = devices(k)
            Dim newLine = New List(Of String) From {k.Cut.ElementAt(0), " "}
            For i = 2 To headers.Count - 1
                Select Case deviceCounter.ContainsKey(headers(i))
                    Case True : newLine.Add(deviceCounter(headers(i)))
                    Case Else : newLine.Add(" ")
                End Select
            Next
            output.Add(String.Join(vbTab, newLine.ToArray))
        Next
        output.AddRange({"", "PerAddress".Translate})
        Dim modeCounterKeys = modes.Keys.ToList
        modeCounterKeys.Sort()
        For Each k In modeCounterKeys
            Dim modeCounter = modes(k)
            Dim newLine = New List(Of String) From {k.Cut.ElementAt(0), k.Cut.ElementAt(1)}
            For i = 2 To headers.Count - 1
                Select Case modeCounter.ContainsKey(headers(i))
                    Case True : newLine.Add(modeCounter(headers(i)))
                    Case Else : newLine.Add(" ")
                End Select
            Next
            output.Add(String.Join(vbTab, newLine.ToArray))
        Next
        Return output.ToArray
    End Function

    ''' <summary>
    ''' Creates the data for a loop configuration page (device types per loop).
    ''' </summary>
    ''' <param name="title">The page title.</param>
    ''' <param name="headers">The page headers.</param>
    ''' <param name="loopDevices">A dictionary containing the totals of devicestypes per loop.</param>
    ''' <returns>The page data.</returns>
    Private Function LoopConfiguration(title As String, headers As String,
                                       loopDevices As Dictionary(Of String, Integer())) As String()
        Dim output = New List(Of String) From {title, headers}
        Dim loopDeviceKeys = loopDevices.Keys.ToList
        loopDeviceKeys.Sort()
        Dim actPanel = ""
        Dim actLoop = ""

        For Each k In loopDeviceKeys
            Dim loopDevice = loopDevices(k)
            Dim newLine = k.Cut.ToList
            Select Case True
                Case actPanel = "" : actPanel = newLine(0)
                Case actPanel = newLine(0) : newLine(0) = ""
                Case Else : actPanel = newLine(0) : actLoop = ""
            End Select
            newLine(1) = newLine(1).Replace("-", "")
            Select Case True
                Case actLoop = "" : actLoop = newLine(1)
                Case actLoop = newLine(1) : newLine(1) = ""
                Case Else : actLoop = newLine(1)
            End Select
            newLine(2) = "Z{0}".Compose(newLine(2).ToInteger)
            For i = 0 To loopDevice.Count - 1
                Select Case loopDevice(i) > 0
                    Case True : newLine.Add(loopDevice(i))
                    Case Else : newLine.Add(" ")
                End Select
            Next
            output.Add(String.Join(vbTab, newLine.ToArray))
        Next
        Return output.ToArray
    End Function

    ''' <summary>
    ''' Gets the region of a in- or ouput.
    ''' </summary>
    ''' <param name="text">Input string.</param>
    ''' <returns>The sectionstring.</returns>
    Private Function GetRegion(text As String) As String
        Select Case True
            Case text.StartsWith("RBus") : Return text.LeftString(6)
            Case text.StartsWith("Local I/O") : Return "Local"
            Case text.StartsWith("Loop") : Return "Loop"
            Case Else : Return ""
        End Select
    End Function

    ''' <summary>
    ''' Gets the category within the region of a in- or ouput.
    ''' </summary>
    ''' <param name="text">Input string.</param>
    ''' <returns>The sectionstring.</returns>
    Private Function GetCategory(text As String) As String
        Select Case True
            Case text.StartsWith("Loop")
                Select Case "ABCDEFGH".Contains(text.MidString(6, 1))
                    Case True : Return text.MidString(6, 1)
                    Case Else : Return "L{0}-".Compose(text.MidString(6, 2))
                End Select
            Case text.Contains("Real") : Return "R"
            Case text.Contains("X-Bus") : Return "X"
            Case text.Contains("Pseudo") : Return "P"
            Case text.Contains("Timer") : Return "T"
            Case text.Contains("Menu") : Return "M"
            Case Else : Return ""
        End Select
    End Function

End Class

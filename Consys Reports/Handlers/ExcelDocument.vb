'Gadec Engineerings Software (c) 2020
Imports Gadec.ExcelAssistance

''' <summary>
''' <para><see cref="ExcelDocument"/> contains a new or existing excel document.</para>
''' </summary>
Public Class ExcelDocument
    Implements IDisposable
    Private _disposed As Boolean

    ''' <summary>
    ''' The present (excel) document.
    ''' </summary>
    Private _workbook As Object
    ''' <summary>
    ''' Determines whether the pages could not be imported.
    ''' </summary>
    Private _failed As Boolean = False

    'class

    ''' <summary>
    ''' Initializes a new instance of <see cref="ExcelDocument"/>.
    ''' <para><see cref="ExcelDocument"/> contains a new or existing excel document.</para>
    ''' </summary>
    ''' <param name="application"></param>
    ''' <param name="fileName"></param>
    Sub New(application As Object, Optional fileName As String = "")
        Try
            If fileName = "" Then
                _workbook = application.Workbooks.Add
            Else
                _workbook = application.Workbooks.Open(fileName)
            End If
        Catch ex As Exception
            _workbook = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Disposing this <see cref="ExcelDocument"/> will close the excel document.
    ''' </summary>
    Protected Overridable Sub Dispose(disposing As Boolean)
        If _disposed Then Exit Sub

        If disposing Then
            'dispose managed state (managed objects) not needed
        End If

        _workbook.Close()
        _workbook = Nothing
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
    ''' Determines whether the (new) document is open.
    ''' </summary>
    ''' <returns>True if document is available.</returns>
    Function OpenedSuccessfully() As Boolean
        Return NotNothing(_workbook)
    End Function

    ''' <summary>
    ''' Determines whether the pages have been successfully imported.
    ''' </summary>
    ''' <returns>True if successful.</returns>
    Function ImportedSuccessfully() As Boolean
        Return Not _failed
    End Function

    'subs

    ''' <summary>
    ''' Imports pages to the present document with the specified report data.
    ''' </summary>
    ''' <param name="reportData">A dictionary with all the report data.</param>
    ''' <param name="progress">The progress provider that tracks the progress of creating the reports document.</param>
    Sub ImportPages(reportData As Dictionary(Of String, String), progress As IProgress(Of ProgressEventArgs))
        Dim formatData = DataSetHelper.LoadFromXml("{Support}\SetFormatInfo.xml".Compose).GetTable("Format", "Name")
        Dim firstSheet As Object = Nothing
        Try
            Dim activeSheet = _workbook.ActiveSheet
            Dim pageKeys = reportData.Keys.ToList
            pageKeys.Sort()
            For Each key In pageKeys
                Dim eventArgs = New ProgressEventArgs("lblProcess".Translate(key.Replace(";", "-")))
                progress.Report(eventArgs)

                activeSheet.Copy(activeSheet)
                Dim workSheet = _workbook.ActiveSheet
                workSheet.Name = key.Replace(";", "-")
                If IsNothing(firstSheet) Then firstSheet = _workbook.ActiveSheet
                workSheet.Range("A1").Select()
                Clipboard.SetText(String.Join(vbCr, reportData(key)))
                Application.DoEvents()
                workSheet.Paste()
                Application.DoEvents()
                Dim formatRow = formatData.Rows.Find(key.Cut.Item(0))
                Dim sheetFormats = formatRow.GetString("FormatString").Cut.ToIniDictionary

                For Each sheetFormat In sheetFormats
                    Dim col = sheetFormat.Key.Replace("$", "")
                    Select Case True
                        Case sheetFormat.Value = "|"
                            workSheet.Columns(col).Borders(xlEdgeRight).LineStyle = xlContinuous
                            workSheet.Columns(col).Borders(xlEdgeRight).ColorIndex = 0
                            workSheet.Columns(col).Borders(xlEdgeRight).TintAndShade = 0
                            workSheet.Columns(col).Borders(xlEdgeRight).Weight = xlThin
                        Case Else : workSheet.Columns(col).ColumnWidth = Val(sheetFormat.Value)
                    End Select
                Next
                workSheet.Range("A1").Select()
            Next
            firstSheet.Select
            If firstSheet.Name = "#Loopconfiguration-Total" Then
                Dim eventArgs = New ProgressEventArgs("lblFormatLoopConfig".Translate)
                progress.Report(eventArgs)

                Dim currentRow = 4
                Dim panelStartRow = 3
                Dim loopStartRow = 3
                Do Until firstSheet.Cells(currentRow, 3).Value = ""
                    If Not firstSheet.Cells(currentRow, 1).Value = "" Then
                        Dim row = firstSheet.Range("A{0}:A{1}".Compose(panelStartRow, currentRow - 1))
                        row.Borders(xlInsideHorizontal).LineStyle = xlNone
                        panelStartRow = currentRow
                    End If
                    If Not firstSheet.Cells(currentRow, 2).Value = "" Then
                        Dim row = firstSheet.Range("B{0}:B{1}".Compose(loopStartRow, currentRow - 1))
                        row.Borders(xlInsideHorizontal).LineStyle = xlNone
                        loopStartRow = currentRow
                    End If
                    currentRow += 1
                Loop
                Dim range = firstSheet.Range("A{0}:A{1}".Compose(panelStartRow, currentRow - 1))
                range.Borders(xlInsideHorizontal).LineStyle = xlNone
                range = firstSheet.Range("B{0}:B{1}".Compose(loopStartRow, currentRow - 1))
                range.Borders(xlInsideHorizontal).LineStyle = xlNone
            End If
        Catch ex As Exception
            _failed = True
            MsgBox(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Saves the present document.
    ''' </summary>
    ''' <param name="fileName"></param>
    Sub Save(fileName As String)
        If IO.File.Exists(fileName) Then Kill(fileName)
        _workbook.SaveAs(fileName)
    End Sub

End Class

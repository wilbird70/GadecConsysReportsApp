'Gadec Engineerings Software (c) 2020

''' <summary>
''' Provides a method to backup a file (inserting 'old' in filename) if file exists.
''' </summary>
Public Class FileHelper

    ''' <summary>
    ''' Backups the specified file (inserting 'old' in filename) if file exists.
    ''' </summary>
    ''' <param name="fileName"></param>
    ''' <returns></returns>
    Shared Function BackUp(fileName As String) As String
        Dim output = "$$$"
        If IO.File.Exists(fileName) Then
            output = "{0}\{1}.old (1){2}".Compose(IO.Path.GetDirectoryName(fileName), IO.Path.GetFileNameWithoutExtension(fileName), IO.Path.GetExtension(fileName))
            Do While IO.File.Exists(output)
                output = output.AutoNumber
            Loop
            Do While FileSystemHelper.FileLocked(fileName)
                Dim msgRlt = MessageBoxQuestion("FileInUse".Translate(fileName), MessageBoxButtons.OKCancel)
                If msgRlt = Windows.Forms.DialogResult.Cancel Then output = "" : Exit Do
            Loop
            If Not output = "" Then
                Do
                    Try
                        IO.File.Move(fileName, output)
                        Exit Do
                    Catch ex As Exception
                        Dim msgRlt = MessageBoxQuestion("FileInUse".Translate(fileName), MessageBoxButtons.OKCancel)
                        If msgRlt = Windows.Forms.DialogResult.Cancel Then output = "" : Exit Do
                    End Try
                Loop
            End If
        End If
        Return output
    End Function

End Class

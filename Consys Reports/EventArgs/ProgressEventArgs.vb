'Gadec Engineerings Software (c) 2020

''' <summary>
''' <para><see cref="ProgressEventArgs"/> provides data for a <see cref="Progress"/> provider.</para>
''' <para>Tracks the progress of creating the reports document.</para>
''' </summary>
Public Class ProgressEventArgs
    ''' <summary>
    ''' The description to display in the main form.
    ''' </summary>
    ''' <returns></returns>
    Public Property Description As String

    ''' <summary>
    ''' Initializes a new instance of <see cref="ProgressEventArgs"/> with the specified properties.
    ''' <para><see cref="ProgressEventArgs"/> provides data for a <see cref="Progress"/> provider.</para>
    ''' <para>Tracks the progress of creating the reports document.</para>
    ''' </summary>
    ''' <param name="description">The description to display in the main form.</param>
    Sub New(description As String)
        _Description = description
    End Sub

End Class

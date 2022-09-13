<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.ltSelectFiles = New System.Windows.Forms.Button()
        Me.ProgressLabel = New System.Windows.Forms.Label()
        Me.ltClose = New System.Windows.Forms.Button()
        Me.ProjectLabel = New System.Windows.Forms.Label()
        Me.LanguagePictureBox = New System.Windows.Forms.PictureBox()
        Me.LanguageComboBox = New System.Windows.Forms.ComboBox()
        CType(Me.LanguagePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ltSelectFiles
        '
        Me.ltSelectFiles.Location = New System.Drawing.Point(12, 48)
        Me.ltSelectFiles.Name = "ltSelectFiles"
        Me.ltSelectFiles.Size = New System.Drawing.Size(140, 23)
        Me.ltSelectFiles.TabIndex = 0
        Me.ltSelectFiles.Text = "Select files"
        Me.ltSelectFiles.UseVisualStyleBackColor = True
        '
        'ProgressLabel
        '
        Me.ProgressLabel.AutoSize = True
        Me.ProgressLabel.Location = New System.Drawing.Point(12, 28)
        Me.ProgressLabel.Name = "ProgressLabel"
        Me.ProgressLabel.Size = New System.Drawing.Size(16, 13)
        Me.ProgressLabel.TabIndex = 1
        Me.ProgressLabel.Text = "..."
        '
        'ltClose
        '
        Me.ltClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ltClose.Location = New System.Drawing.Point(156, 48)
        Me.ltClose.Name = "ltClose"
        Me.ltClose.Size = New System.Drawing.Size(80, 23)
        Me.ltClose.TabIndex = 2
        Me.ltClose.Text = "Close"
        Me.ltClose.UseVisualStyleBackColor = True
        '
        'ProjectLabel
        '
        Me.ProjectLabel.AutoSize = True
        Me.ProjectLabel.Location = New System.Drawing.Point(12, 9)
        Me.ProjectLabel.Name = "ProjectLabel"
        Me.ProjectLabel.Size = New System.Drawing.Size(16, 13)
        Me.ProjectLabel.TabIndex = 3
        Me.ProjectLabel.Text = "..."
        '
        'LanguagePictureBox
        '
        Me.LanguagePictureBox.Location = New System.Drawing.Point(292, 12)
        Me.LanguagePictureBox.Name = "LanguagePictureBox"
        Me.LanguagePictureBox.Size = New System.Drawing.Size(30, 30)
        Me.LanguagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.LanguagePictureBox.TabIndex = 68
        Me.LanguagePictureBox.TabStop = False
        '
        'LanguageComboBox
        '
        Me.LanguageComboBox.FormattingEnabled = True
        Me.LanguageComboBox.Location = New System.Drawing.Point(242, 48)
        Me.LanguageComboBox.Name = "LanguageComboBox"
        Me.LanguageComboBox.Size = New System.Drawing.Size(80, 21)
        Me.LanguageComboBox.TabIndex = 67
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ltClose
        Me.ClientSize = New System.Drawing.Size(334, 83)
        Me.Controls.Add(Me.LanguagePictureBox)
        Me.Controls.Add(Me.LanguageComboBox)
        Me.Controls.Add(Me.ProjectLabel)
        Me.Controls.Add(Me.ltClose)
        Me.Controls.Add(Me.ProgressLabel)
        Me.Controls.Add(Me.ltSelectFiles)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Main"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XXX"
        CType(Me.LanguagePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ltSelectFiles As System.Windows.Forms.Button
    Friend WithEvents ProgressLabel As System.Windows.Forms.Label
    Friend WithEvents ltClose As System.Windows.Forms.Button
    Friend WithEvents ProjectLabel As System.Windows.Forms.Label
    Friend WithEvents LanguagePictureBox As PictureBox
    Friend WithEvents LanguageComboBox As ComboBox
End Class

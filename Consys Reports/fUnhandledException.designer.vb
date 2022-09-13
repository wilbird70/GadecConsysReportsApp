<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class fUnhandledException
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
    'Do not modify it using the code editor
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fUnhandledException))
        Me.ltSend = New System.Windows.Forms.Button()
        Me.ltClose = New System.Windows.Forms.Button()
        Me.lText1 = New System.Windows.Forms.Label()
        Me.tTextbox = New System.Windows.Forms.RichTextBox()
        Me.tImage = New System.Windows.Forms.PictureBox()
        Me.lText2 = New System.Windows.Forms.Label()
        CType(Me.tImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ltSend
        '
        Me.ltSend.Location = New System.Drawing.Point(458, 222)
        Me.ltSend.Name = "ltSend"
        Me.ltSend.Size = New System.Drawing.Size(86, 23)
        Me.ltSend.TabIndex = 10
        Me.ltSend.Text = "XXX"
        Me.ltSend.UseVisualStyleBackColor = True
        '
        'ltClose
        '
        Me.ltClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ltClose.Location = New System.Drawing.Point(550, 222)
        Me.ltClose.Name = "ltClose"
        Me.ltClose.Size = New System.Drawing.Size(85, 23)
        Me.ltClose.TabIndex = 11
        Me.ltClose.Text = "XXX"
        Me.ltClose.UseVisualStyleBackColor = True
        '
        'lText1
        '
        Me.lText1.AutoSize = True
        Me.lText1.Location = New System.Drawing.Point(115, 9)
        Me.lText1.Name = "lText1"
        Me.lText1.Size = New System.Drawing.Size(28, 13)
        Me.lText1.TabIndex = 12
        Me.lText1.Text = "XXX"
        '
        'tTextbox
        '
        Me.tTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tTextbox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tTextbox.Location = New System.Drawing.Point(118, 26)
        Me.tTextbox.Name = "tTextbox"
        Me.tTextbox.Size = New System.Drawing.Size(517, 190)
        Me.tTextbox.TabIndex = 13
        Me.tTextbox.Text = ""
        Me.tTextbox.WordWrap = False
        '
        'tImage
        '
        Me.tImage.Image = Global.Consys_Reports.My.Resources.Resources.warning_33364_960_720
        Me.tImage.Location = New System.Drawing.Point(12, 0)
        Me.tImage.Name = "tImage"
        Me.tImage.Size = New System.Drawing.Size(100, 100)
        Me.tImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.tImage.TabIndex = 14
        Me.tImage.TabStop = False
        '
        'lText2
        '
        Me.lText2.AutoSize = True
        Me.lText2.Location = New System.Drawing.Point(115, 227)
        Me.lText2.Name = "lText2"
        Me.lText2.Size = New System.Drawing.Size(28, 13)
        Me.lText2.TabIndex = 15
        Me.lText2.Text = "XXX"
        '
        'fUnhandledException
        '
        Me.AcceptButton = Me.ltSend
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ltClose
        Me.ClientSize = New System.Drawing.Size(650, 258)
        Me.Controls.Add(Me.lText1)
        Me.Controls.Add(Me.ltSend)
        Me.Controls.Add(Me.ltClose)
        Me.Controls.Add(Me.tImage)
        Me.Controls.Add(Me.tTextbox)
        Me.Controls.Add(Me.lText2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "fUnhandledException"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XXX"
        CType(Me.tImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ltSend As System.Windows.Forms.Button
    Friend WithEvents ltClose As System.Windows.Forms.Button
    Friend WithEvents lText1 As System.Windows.Forms.Label
    Friend WithEvents tTextbox As System.Windows.Forms.RichTextBox
    Friend WithEvents tImage As Windows.Forms.PictureBox
    Friend WithEvents lText2 As Windows.Forms.Label
End Class

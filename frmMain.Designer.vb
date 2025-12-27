<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        txtFilePath = New TextBox()
        btnLoadFile = New Button()
        btnExportSheet = New Button()
        btnGenerateCode = New Button()
        txtFontData = New TextBox()
        Label1 = New Label()
        Label2 = New Label()
        pnlPreviewContainer = New Panel()
        lblPreviewTitle = New Label()
        picFontPreview = New PictureBox()
        pnlPreviewContainer.SuspendLayout()
        CType(picFontPreview, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' txtFilePath
        ' 
        txtFilePath.Location = New Point(22, 56)
        txtFilePath.Margin = New Padding(6)
        txtFilePath.Name = "txtFilePath"
        txtFilePath.Size = New Size(704, 30)
        txtFilePath.TabIndex = 0
        ' 
        ' btnLoadFile
        ' 
        btnLoadFile.Location = New Point(740, 50)
        btnLoadFile.Margin = New Padding(6)
        btnLoadFile.Name = "btnLoadFile"
        btnLoadFile.Size = New Size(138, 42)
        btnLoadFile.TabIndex = 1
        btnLoadFile.Text = "Load File"
        btnLoadFile.UseVisualStyleBackColor = True
        ' 
        ' btnExportSheet
        ' 
        btnExportSheet.Location = New Point(22, 768)
        btnExportSheet.Margin = New Padding(6)
        btnExportSheet.Name = "btnExportSheet"
        btnExportSheet.Size = New Size(416, 42)
        btnExportSheet.TabIndex = 2
        btnExportSheet.Text = "Export Pixel Font Sheet to Target Path"
        btnExportSheet.UseVisualStyleBackColor = True
        ' 
        ' btnGenerateCode
        ' 
        btnGenerateCode.Location = New Point(462, 768)
        btnGenerateCode.Margin = New Padding(6)
        btnGenerateCode.Name = "btnGenerateCode"
        btnGenerateCode.Size = New Size(416, 42)
        btnGenerateCode.TabIndex = 3
        btnGenerateCode.Text = "Generate Encoding Code (VB.NET)"
        btnGenerateCode.UseVisualStyleBackColor = True
        ' 
        ' txtFontData
        ' 
        txtFontData.AcceptsReturn = True
        txtFontData.AcceptsTab = True
        txtFontData.Font = New Font("Consolas", 9.5F)
        txtFontData.Location = New Point(22, 138)
        txtFontData.Margin = New Padding(6)
        txtFontData.Multiline = True
        txtFontData.Name = "txtFontData"
        txtFontData.ScrollBars = ScrollBars.Both
        txtFontData.Size = New Size(854, 401)
        txtFontData.TabIndex = 4
        txtFontData.WordWrap = False
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(22, 26)
        Label1.Margin = New Padding(6, 0, 6, 0)
        Label1.Name = "Label1"
        Label1.Size = New Size(381, 24)
        Label1.TabIndex = 5
        Label1.Text = "Target Path to Pixel Font Sheet (PNG Only):"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(22, 107)
        Label2.Margin = New Padding(6, 0, 6, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(267, 24)
        Label2.TabIndex = 6
        Label2.Text = "Encoded Font Data (Editable):"
        ' 
        ' pnlPreviewContainer
        ' 
        pnlPreviewContainer.BorderStyle = BorderStyle.FixedSingle
        pnlPreviewContainer.Controls.Add(lblPreviewTitle)
        pnlPreviewContainer.Controls.Add(picFontPreview)
        pnlPreviewContainer.Location = New Point(22, 544)
        pnlPreviewContainer.Margin = New Padding(6)
        pnlPreviewContainer.Name = "pnlPreviewContainer"
        pnlPreviewContainer.Size = New Size(854, 211)
        pnlPreviewContainer.TabIndex = 7
        ' 
        ' lblPreviewTitle
        ' 
        lblPreviewTitle.AutoSize = True
        lblPreviewTitle.Font = New Font("Microsoft Sans Serif", 10.0F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPreviewTitle.Location = New Point(6, 6)
        lblPreviewTitle.Margin = New Padding(6, 0, 6, 0)
        lblPreviewTitle.Name = "lblPreviewTitle"
        lblPreviewTitle.Size = New Size(253, 25)
        lblPreviewTitle.TabIndex = 0
        lblPreviewTitle.Text = "Pixel Font Sheet Preview"
        ' 
        ' picFontPreview
        ' 
        picFontPreview.BackColor = Color.Black
        picFontPreview.BorderStyle = BorderStyle.FixedSingle
        picFontPreview.Location = New Point(334, 6)
        picFontPreview.Margin = New Padding(6)
        picFontPreview.Name = "picFontPreview"
        picFontPreview.Size = New Size(512, 192)
        picFontPreview.SizeMode = PictureBoxSizeMode.StretchImage
        picFontPreview.TabIndex = 1
        picFontPreview.TabStop = False
        ' 
        ' frmMain
        ' 
        AllowDrop = True
        AutoScaleDimensions = New SizeF(11.0F, 24.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 833)
        Controls.Add(pnlPreviewContainer)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(txtFontData)
        Controls.Add(btnGenerateCode)
        Controls.Add(btnExportSheet)
        Controls.Add(btnLoadFile)
        Controls.Add(txtFilePath)
        Margin = New Padding(6)
        Name = "frmMain"
        Text = "Pixel Font Codec 128x48"
        pnlPreviewContainer.ResumeLayout(False)
        pnlPreviewContainer.PerformLayout()
        CType(picFontPreview, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtFilePath As TextBox
    Friend WithEvents btnLoadFile As Button
    Friend WithEvents btnExportSheet As Button
    Friend WithEvents btnGenerateCode As Button
    Friend WithEvents txtFontData As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlPreviewContainer As Panel
    Friend WithEvents lblPreviewTitle As Label
    Friend WithEvents picFontPreview As PictureBox

End Class

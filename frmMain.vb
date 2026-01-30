Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text

#Disable Warning IDE1006
Public Class frmMain
    Private Shared Function EncodeFontData(bitmap As Bitmap) As StringBuilder
        If bitmap.Width <> 128 OrElse bitmap.Height <> 48 Then
            Throw New ArgumentException("Image must be 128x48 pixels (required for font sheet)")
        End If

        Dim data As New StringBuilder
        Dim px As Integer = 0
        Dim py As Integer = 0

        While px < 128
            Dim r As Integer = 0
            ' Read 24 pixels in a row, line by line
            For i As Integer = 0 To 23
                If px < 128 AndAlso py < 48 Then
                    Dim pixel As Color = bitmap.GetPixel(px, py)
                    ' Check if pixel is not close to black, with tolerance for grayscale
                    If Not (pixel.R < 50 AndAlso pixel.G < 50 AndAlso pixel.B < 50) Then r = r Or (1 << i)
                End If

                py += 1
                If py >= 48 Then
                    px += 1
                    py = 0
                End If
            Next i

            ' Split into 4 6-bit characters (original encoding logic)
            Dim sym1 As Integer = (r >> 18) And &H3F
            Dim sym2 As Integer = (r >> 12) And &H3F
            Dim sym3 As Integer = (r >> 6) And &H3F
            Dim sym4 As Integer = r And &H3F

            data.Append(ChrW(sym1 + 48))
            data.Append(ChrW(sym2 + 48))
            data.Append(ChrW(sym3 + 48))
            data.Append(ChrW(sym4 + 48))
        End While

        Return data
    End Function

    Private Shared Function DecodeFontData(data As String) As Bitmap
        ' Create a new 128x48 bitmap (core carrier)
        Dim bitmap As New Bitmap(128, 48, PixelFormat.Format32bppArgb)

        ' Graphics object must be disposed to avoid GDI+ resource leaks
        Using g As Graphics = Graphics.FromImage(bitmap)
            g.Clear(Color.Black) ' Black background (consistent with original logic)

            Dim px As Integer = 0
            Dim py As Integer = 0
            Dim b As Integer = 0
            ' Only process complete 4-character blocks to avoid index out of range errors
            Dim validData = data.Replace(vbCrLf, "").Replace(" ", "")
            Dim maxValidIndex = validData.Length - 4

            While b <= maxValidIndex
                Try
                    ' Parse 4 characters, with Try-Catch to avoid crashing
                    Dim sym1 As Integer = AscW(validData(b)) - 48
                    Dim sym2 As Integer = AscW(validData(b + 1)) - 48
                    Dim sym3 As Integer = AscW(validData(b + 2)) - 48
                    Dim sym4 As Integer = AscW(validData(b + 3)) - 48

                    ' Combine into 24-bit integer, and clamp values to avoid negative shifts
                    sym1 = Math.Clamp(sym1, 0, 63) ' 6-bit max value is 63
                    sym2 = Math.Clamp(sym2, 0, 63)
                    sym3 = Math.Clamp(sym3, 0, 63)
                    sym4 = Math.Clamp(sym4, 0, 63)
                    Dim r As Integer = sym1 << 18 Or sym2 << 12 Or sym3 << 6 Or sym4

                    ' Draw 24 pixels in a row (original encoding logic)
                    For i As Integer = 0 To 23
                        If px >= 128 Then Exit For ' Prevent X-axis overflow (fault tolerance)
                        Dim isSet As Boolean = (r And (1 << i)) <> 0
                        Dim color As Color = If(isSet, Color.White, Color.Black)
                        bitmap.SetPixel(px, py, color)

                        py += 1
                        If py >= 48 Then
                            px += 1
                            py = 0
                        End If
                    Next i
                Catch ex As Exception
                    ' Single character block errors are skipped
                    Debug.WriteLine($"Skip invalid data block at index {b}: {ex.Message}")
                End Try

                b += 4
            End While
        End Using

        Return bitmap
    End Function

    Private Sub btnLoadFile_Click(sender As Object, e As EventArgs) Handles btnLoadFile.Click
        Using ofd As New OpenFileDialog
            ofd.Filter = "PNG Images|*.png|All Files|*.*"
            ofd.Title = "Select PNG File (Font Sheet)"
            ofd.FileName = "font_sheet.png"
            ofd.CheckFileExists = True
            If ofd.ShowDialog() = DialogResult.OK Then
                txtFilePath.Text = ofd.FileName
                LoadPreviewFromFile(ofd.FileName)
            End If
        End Using
    End Sub

    ' ToDo: Write a drag event so that the font sheet can be loaded immediately
    Private Sub frmMain_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        e.Effect = If(
            e.Data.GetDataPresent(DataFormats.FileDrop), DragDropEffects.Copy, DragDropEffects.None
        )
    End Sub

    Private Sub frmMain_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files As String() = CType(e.Data.GetData(DataFormats.FileDrop), String())
            If files.Length > 0 Then
                txtFilePath.Text = files(0)
                LoadPreviewFromFile(files(0))
            End If
        End If
    End Sub

    Private Sub btnExportSheet_Click(sender As Object, e As EventArgs) Handles btnExportSheet.Click
        With txtFilePath.Text
            Dim validDrive = .StartsWith("C:\") OrElse .StartsWith("D:\") OrElse .StartsWith("E:\")
            If Not validDrive AndAlso Not .EndsWith(".png") Then
                ShowMessagePopup("For PNG output, please select or enter a save path first.

Note: Enter the PNG full path starting with 'C:\', 'D:\' or 'E:\' if you are going to enter the path manually.")
                Exit Sub
            End If
        End With
        If String.IsNullOrWhiteSpace(txtFontData.Text) Then
            ShowMessagePopup("Please enter valid font encoding data first")
            Exit Sub
        End If

        Try
            Dim data As String = txtFontData.Text.Replace(vbCrLf, "").Replace(" ", "")
            ' Bitmap must be disposed to avoid GDI+ resource leaks
            Using bitmap = DecodeFontData(data)
                ' Make sure the save directory exists to prevent path errors
                Dim saveDir = Path.GetDirectoryName(txtFilePath.Text)
                If Not Directory.Exists(saveDir) Then Directory.CreateDirectory(saveDir)
                bitmap.Save(txtFilePath.Text, ImageFormat.Png)
                ShowMessagePopup($"Pixel font sheet saved successfully to: {txtFilePath.Text}", "SUCCESS")
                LoadPreviewFromBitmap(bitmap)
            End Using
        Catch ex As Exception
            ShowMessagePopup($"Failed to generate font sheet: {ex.Message}")
        End Try
    End Sub

    Private Sub btnGenerateCode_Click(sender As Object, e As EventArgs) Handles btnGenerateCode.Click
        If String.IsNullOrWhiteSpace(txtFilePath.Text) Then
            ShowMessagePopup("Please select a PNG font sheet file first")
            Exit Sub
        ElseIf Not File.Exists(txtFilePath.Text) Then
            ShowMessagePopup($"Selected file not found: {txtFilePath.Text}")
            Exit Sub
        End If

        Dim varName = InputBox("Enter variable name for font encoding data:
(Note: Leave it *blank* for anonymous StringBuilder)", "Variable Name")
        Dim isAnonymous = String.IsNullOrWhiteSpace(varName)
        If Not (isAnonymous OrElse varName Like "[a-zA-Z_][a-zA-Z0-9_]*") Then
            ShowMessagePopup("Please enter a valid variable name")
            Exit Sub
        End If
        Try
            Dim firstLine = If(isAnonymous, "With New StringBuilder", $"Dim {varName} As New StringBuilder")
            ' Read PNG file with `Using` to ensure disposal of Bitmap object
            Using bitmap As New Bitmap(txtFilePath.Text)
                Dim data = EncodeFontData(bitmap)
                Dim strBdrCode As New StringBuilder
                Dim encodedText As New StringBuilder

                strBdrCode.AppendLine("' Auto-generated font encoding data as VB.NET code")
                strBdrCode.AppendLine(firstLine)
                ' Encode font data in chunks of 64 characters for readability
                For i As Integer = 0 To data.Length - 1 Step 64
                    Dim chunkLength As Integer = Math.Min(64, data.Length - i)
                    Dim chunk As String = data.ToString(i, chunkLength)
                    encodedText.AppendLine(chunk)
                    strBdrCode.AppendLine($"{varName}.Append(""{chunk}"")")
                Next i
                If isAnonymous Then strBdrCode.AppendLine($"End With")

                txtFontData.Text = encodedText.ToString()
                Clipboard.SetText(strBdrCode.ToString())
                ' Copy VB.NET StringBuilder code to clipboard for easy integration
                ShowMessagePopup("VB.NET StringBuilder code copied to clipboard!", "SUCCESS")
                LoadPreviewFromBitmap(bitmap)
            End Using ' Automatically dispose Bitmap object to avoid GDI+ resource leaks
        Catch ex As Exception
            ShowMessagePopup($"Failed to generate code: {ex.Message}")
        End Try
    End Sub

    Private Sub LoadPreviewFromFile(filePath As String)
        If Not File.Exists(filePath) Then
            ShowMessagePopup("Preview file not found. Please select a valid PNG.", "WARNING")
            ClearPreview()  ' Clear preview if file not found
            Exit Sub
        End If

        Try
            ' Load preview image, ensuring disposal to avoid memory leaks
            ClearPreview()
            Using bitmap As New Bitmap(filePath)
                Dim data = EncodeFontData(bitmap)
                Dim encodedText As New StringBuilder
                picFontPreview.Image = New Bitmap(bitmap)
                ' Encode font data in chunks of 64 characters (for readability)
                For i As Integer = 0 To data.Length - 1 Step 64
                    Dim chunkLength As Integer = Math.Min(64, data.Length - i)
                    Dim chunk As String = data.ToString(i, chunkLength)
                    encodedText.AppendLine(chunk)
                Next i
                txtFontData.Text = encodedText.ToString()
            End Using
        Catch ex As Exception
            ShowMessagePopup($"Failed to load preview: {ex.Message}")
            ClearPreview()
        End Try
    End Sub

    Private Sub LoadPreviewFromBitmap(sourceBmp As Bitmap)
        Try
            ' Note: Clear preview first to avoid memory leaks; copy Bitmap to avoid external 
            '       disposal issues.
            ClearPreview()
            picFontPreview.Image = New Bitmap(sourceBmp)
        Catch ex As Exception
            ShowMessagePopup($"Failed to update preview: {ex.Message}")
            ClearPreview()
        End Try
    End Sub

    ' Helper function: Clear preview and release GDI+ resources
    Private Sub ClearPreview()
        If picFontPreview.Image IsNot Nothing Then
            ' Must call Dispose to release GDI+ resources
            picFontPreview.Image.Dispose()
            picFontPreview.Image = Nothing
        End If
    End Sub

    Private WithEvents DebounceTimer As New Timer With {.Interval = 300, .Enabled = False}

    Private Sub txtFontData_TextChanged(sender As Object, e As EventArgs) Handles txtFontData.TextChanged
        ' Debounce: Delay preview update to avoid frequent re-renders
        DebounceTimer.Stop()
        DebounceTimer.Start()
    End Sub

    ' Debounce timer tick: Execute real-time preview after delay
    Private Sub DebounceTimer_Tick(sender As Object, e As EventArgs) Handles DebounceTimer.Tick
        DebounceTimer.Stop() ' Stop timer to avoid repeated executions

        ' Clear preview if text is empty
        If String.IsNullOrWhiteSpace(txtFontData.Text) Then
            ClearPreview()
            Exit Sub
        End If

        Try
            ' DecodeFontData returns a Bitmap that must be disposed to avoid GDI+ leaks
            Using tempBmp = DecodeFontData(txtFontData.Text)
                ' Copy a new Bitmap for preview to avoid empty reference after dispose
                LoadPreviewFromBitmap(tempBmp)
            End Using
        Catch ex As Exception
            ' On real-time preview error, show in debug output but don't block input
            Debug.WriteLine($"Real-time preview error: {ex.Message}")
            ClearPreview()
        End Try
    End Sub

    Private Sub ShowMessagePopup(message As String, Optional title As String = "ERROR")
        Dim iconMap As New Dictionary(Of String, MessageBoxIcon) From {
            {"ERROR", MessageBoxIcon.Error},
            {"SUCCESS", MessageBoxIcon.Information},
            {"WARNING", MessageBoxIcon.Warning}
        }
        MessageBox.Show(message, title, MessageBoxButtons.OK, iconMap(title))
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ClearPreview()
        DebounceTimer.Dispose()
    End Sub

    <STAThread()> Friend Shared Sub Main()
        Application.SetHighDpiMode(HighDpiMode.SystemAware)
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New frmMain)
    End Sub
End Class
Imports System.Data.SqlClient
Imports System.Diagnostics

Public Class Form7
    Private loggedInUser As String
    Public Property SelectedMode As String

    Private con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Project sem 5\my project\WindowsApp2\WindowsApp2\Database1.mdf;Integrated Security=True")

    Private words As List(Of String)
    Private random As New Random()
    Private stopwatch As New Stopwatch()

    Private totalScore As Integer = 0
    Private totalWords As Integer = 0
    Private correctWords As Integer = 0

    Private lblTitle As Label
    Private lblModeLevel As Label
    Private lblWord As Label
    Private txtInput As TextBox
    Private lblScore As Label

    Public Sub New(username As String, selectedMode As String)
        InitializeComponent()
        Me.loggedInUser = username
        Me.SelectedMode = selectedMode

        ' Flicker-free rendering
        Me.DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        UpdateStyles()
    End Sub

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000 ' WS_EX_COMPOSITED
            Return cp
        End Get
    End Property

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        Me.BackgroundImage = GlobalBackgroundImage
        Me.BackgroundImageLayout = ImageLayout.Stretch

        ' ===== FlowLayoutPanel Design =====
        FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown
        FlowLayoutPanel1.WrapContents = True
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.BackColor = Color.Transparent
        FlowLayoutPanel1.Padding = New Padding(0, 20, 0, 20)
        FlowLayoutPanel1.Controls.Clear()

        Dim titleColor As Color = Color.Cyan
        Dim textColor As Color = Color.White

        ' ===== Title =====
        lblTitle = New Label() With {
            .Text = "Typing Game",
            .Font = New Font("Segoe Script", 24, FontStyle.Bold Or FontStyle.Italic),
            .ForeColor = titleColor,
            .BackColor = Color.Transparent,
            .TextAlign = ContentAlignment.MiddleCenter,
            .AutoSize = True
        }
        FlowLayoutPanel1.Controls.Add(lblTitle)
        CenterControl(lblTitle)

        ' ===== Mode Label =====
        lblModeLevel = New Label() With {
            .Text = "Mode: " & SelectedMode,
            .Font = New Font("Rockwell", 14, FontStyle.Bold),
            .ForeColor = textColor,
            .BackColor = Color.Transparent,
            .TextAlign = ContentAlignment.MiddleCenter,
            .AutoSize = True
        }
        FlowLayoutPanel1.Controls.Add(lblModeLevel)
        CenterControl(lblModeLevel)

        ' ===== Word Label =====
        lblWord = New Label() With {
            .Text = "",
            .Font = New Font(GlobalGameFont, 22, FontStyle.Bold),
            .ForeColor = Color.Lime,
            .BackColor = Color.Transparent,
            .TextAlign = ContentAlignment.MiddleCenter,
            .AutoSize = True
        }
        FlowLayoutPanel1.Controls.Add(lblWord)
        CenterControl(lblWord, 45)

        ' ===== Input Box =====
        txtInput = New TextBox() With {
            .Width = 350,
            .Font = New Font("Rockwell", 18, FontStyle.Bold),
            .TextAlign = HorizontalAlignment.Center,
            .ForeColor = Color.Teal
        }
        AddHandler txtInput.TextChanged, AddressOf txtInput_TextChanged
        FlowLayoutPanel1.Controls.Add(txtInput)
        CenterControl(txtInput)

        ' ===== Score Label =====
        lblScore = New Label() With {
            .Text = "Score: 0",
            .Font = New Font("Rockwell", 14, FontStyle.Bold),
            .ForeColor = Color.Yellow,
            .BackColor = Color.Transparent,
            .TextAlign = ContentAlignment.MiddleCenter,
            .AutoSize = True
        }
        FlowLayoutPanel1.Controls.Add(lblScore)
        CenterControl(lblScore)

        words = GetWordsForMode(SelectedMode)
        ShuffleWords(words)
        totalWords = words.Count
        ShowNextWord()
    End Sub

    Private Sub CenterControl(ctrl As Control, Optional shiftLeft As Integer = 0)
        ctrl.Margin = New Padding(
        Math.Max(0, ((FlowLayoutPanel1.ClientSize.Width - ctrl.Width) \ 2) - shiftLeft),
        10, 0, 10)
    End Sub

    ' ======= Game Logic Code (unchanged) =======

    Private Function GetWordsForMode(mode As String) As List(Of String)
        Dim easyWords As New List(Of String) From {
            "apple", "ball", "chair", "desk", "earth", "fish", "goat", "hat", "ice", "jam",
            "kite", "lamp", "moon", "nest", "orange", "pen", "queen", "rose", "star", "tree",
            "umbrella", "van", "wolf", "xray", "yarn", "zebra"
        }

        Dim hardWords As New List(Of String) From {
            "amazing", "balance", "capture", "diamond", "elegant", "fiction", "gallery", "harvest",
            "include", "journey", "kingdom", "language", "measure", "network", "opinion", "precious",
            "quality", "regular", "science", "treasure", "upgrade", "victory", "welfare", "yelling", "zealous"
        }

        Dim expertWords As New List(Of String) From {
            "architecture", "benevolent", "configuration", "determination", "entrepreneur", "fascinating",
            "governance", "hypothesis", "infrastructure", "jurisdiction", "knowledgeable", "legislation",
            "metamorphosis", "neuroscience", "optimization", "phenomenology", "questionnaire", "rationalization",
            "sophisticated", "transformation", "unprecedented", "vulnerability", "withholding", "xenophobia",
            "yachtsmanship", "zoologically"
        }

        Select Case mode
            Case "Easy"
                Return easyWords
            Case "Hard"
                Return hardWords
            Case "Expert"
                Return expertWords
            Case Else
                Return easyWords
        End Select
    End Function

    Private Sub ShuffleWords(ByRef list As List(Of String))
        Dim n As Integer = list.Count
        While n > 1
            n -= 1
            Dim k As Integer = random.Next(n + 1)
            Dim value As String = list(k)
            list(k) = list(n)
            list(n) = value
        End While
    End Sub

    Private Sub ShowNextWord()
        If words.Count > 0 Then
            lblWord.Text = words(0)
            words.RemoveAt(0)
            txtInput.Text = ""
            txtInput.Focus()
            stopwatch.Restart()
        Else
            stopwatch.Stop()
            Dim accuracy As Double = (correctWords / totalWords) * 100
            Dim speed As Double = correctWords / (stopwatch.Elapsed.TotalSeconds / 60)

            lblWord.Text = "Completed!"
            txtInput.Enabled = False

            MessageBox.Show("Game Over." & vbCrLf &
                            "Your total score: " & totalScore & vbCrLf &
                            "Speed (WPM): " & Math.Round(speed, 2) & vbCrLf &
                            "Accuracy: " & Math.Round(accuracy, 2) & "%")

            SaveScore(loggedInUser, SelectedMode, speed, accuracy, totalScore, DateTime.Now)

            Dim form4 As New Form4(loggedInUser)
            form4.Show()
            Me.Close()
        End If
    End Sub

    Private Sub txtInput_TextChanged(sender As Object, e As EventArgs)
        If txtInput.Text.Trim().ToLower() = lblWord.Text.ToLower() Then
            stopwatch.Stop()
            Dim timeTaken As Integer = CInt(stopwatch.ElapsedMilliseconds)

            Dim wordScore As Integer
            If timeTaken <= 1000 Then
                wordScore = 10
            ElseIf timeTaken <= 3000 Then
                wordScore = 7
            ElseIf timeTaken <= 5000 Then
                wordScore = 5
            Else
                wordScore = 2
            End If

            totalScore += wordScore
            correctWords += 1
            lblScore.Text = "Score: " & totalScore

            ShowNextWord()
        End If
    End Sub

    Private Sub SaveScore(username As String, mode As String, speed As Double, accuracy As Double, score As Integer, datePlayed As DateTime)
        Try
            con.Open()
            Dim checkCmd As New SqlCommand("SELECT Score FROM Scores WHERE Username=@username AND Mode=@mode", con)
            checkCmd.Parameters.AddWithValue("@username", username)
            checkCmd.Parameters.AddWithValue("@mode", mode)

            Dim existingScoreObj As Object = checkCmd.ExecuteScalar()

            If existingScoreObj IsNot Nothing Then
                Dim existingScore As Integer = Convert.ToInt32(existingScoreObj)
                If score > existingScore Then
                    Dim updateCmd As New SqlCommand("UPDATE Scores SET Speed=@speed, Accuracy=@accuracy, Score=@score, DatePlayed=@date WHERE Username=@username AND Mode=@mode", con)
                    updateCmd.Parameters.AddWithValue("@speed", speed)
                    updateCmd.Parameters.AddWithValue("@accuracy", accuracy)
                    updateCmd.Parameters.AddWithValue("@score", score)
                    updateCmd.Parameters.AddWithValue("@date", datePlayed)
                    updateCmd.Parameters.AddWithValue("@username", username)
                    updateCmd.Parameters.AddWithValue("@mode", mode)
                    updateCmd.ExecuteNonQuery()
                End If
            Else
                Dim insertCmd As New SqlCommand("INSERT INTO Scores (Username, Mode, Speed, Accuracy, Score, DatePlayed) VALUES (@username, @mode, @speed, @accuracy, @score, @date)", con)
                insertCmd.Parameters.AddWithValue("@username", username)
                insertCmd.Parameters.AddWithValue("@mode", mode)
                insertCmd.Parameters.AddWithValue("@speed", speed)
                insertCmd.Parameters.AddWithValue("@accuracy", accuracy)
                insertCmd.Parameters.AddWithValue("@score", score)
                insertCmd.Parameters.AddWithValue("@date", datePlayed)
                insertCmd.ExecuteNonQuery()
            End If

        Catch ex As Exception
            MessageBox.Show("Error saving score: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

End Class

Imports System.Data.SqlClient

Public Class Form5

    Dim con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Project sem 5\my project\WindowsApp2\WindowsApp2\Database1.mdf;Integrated Security=True")
    Private loggedInUser As String

    Public Sub New(username As String)
        InitializeComponent()
        loggedInUser = username

        ' Enable flicker-free rendering
        Me.DoubleBuffered = True
        SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        UpdateStyles()
    End Sub

    ' ===== Flicker Fix with Extended Styles =====
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000 ' WS_EX_COMPOSITED
            Return cp
        End Get
    End Property

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None

        ' ===== LeaderBoard Title =====
        Dim lblTitle As New Label With {
            .Text = "LeaderBoard",
            .Font = New Font("Segoe Script", 24, FontStyle.Bold Or FontStyle.Italic),
            .ForeColor = Color.Teal,
            .AutoSize = False,
            .Height = 80,
            .Dock = DockStyle.Top,
            .TextAlign = ContentAlignment.MiddleCenter,
            .BackColor = Color.Transparent
        }
        Me.Controls.Add(lblTitle)
        lblTitle.BringToFront()

        ' ===== Setup FlowLayoutPanel =====
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.BackColor = Color.Transparent
        Me.Controls.Add(FlowLayoutPanel1)
        FlowLayoutPanel1.BringToFront()

        ' ===== Mode Buttons Panel at Bottom =====
        Dim buttonPanel As New Panel With {
            .Height = 80,
            .Dock = DockStyle.Bottom,
            .BackColor = Color.FromArgb(150, 0, 0, 0)
        }
        Me.Controls.Add(buttonPanel)
        buttonPanel.BringToFront()

        ' ===== Buttons =====
        Dim buttonFont As New Font("Rockwell", 10.2, FontStyle.Bold)
        Dim buttonBackColor As Color = Color.FromArgb(150, 0, 0, 0)
        Dim buttonForeColor As Color = Color.Cyan

        Dim btnEasy As New Button With {.Text = "Easy", .Width = 120, .Height = 50, .Font = buttonFont, .ForeColor = buttonForeColor, .BackColor = buttonBackColor, .FlatStyle = FlatStyle.Flat}
        Dim btnHard As New Button With {.Text = "Hard", .Width = 120, .Height = 50, .Font = buttonFont, .ForeColor = buttonForeColor, .BackColor = buttonBackColor, .FlatStyle = FlatStyle.Flat}
        Dim btnExpert As New Button With {.Text = "Expert", .Width = 120, .Height = 50, .Font = buttonFont, .ForeColor = buttonForeColor, .BackColor = buttonBackColor, .FlatStyle = FlatStyle.Flat}
        Dim btnBack As New Button With {.Text = "Back", .Width = 120, .Height = 50, .Font = buttonFont, .ForeColor = buttonForeColor, .BackColor = buttonBackColor, .FlatStyle = FlatStyle.Flat}

        btnEasy.FlatAppearance.BorderSize = 0
        btnHard.FlatAppearance.BorderSize = 0
        btnExpert.FlatAppearance.BorderSize = 0
        btnBack.FlatAppearance.BorderSize = 0

        Dim buttons = {btnEasy, btnHard, btnExpert, btnBack}
        Dim totalWidth As Integer = buttons.Sum(Function(b) b.Width) + (buttons.Length - 1) * 20
        Dim startX As Integer = (buttonPanel.Width - totalWidth) \ 2

        Dim currentX As Integer = startX
        For Each btn In buttons
            btn.Left = currentX
            btn.Top = 15
            currentX += btn.Width + 20
            buttonPanel.Controls.Add(btn)
        Next

        ' ===== Button Events =====
        AddHandler btnEasy.Click, Sub() LoadLeaderboard("Easy")
        AddHandler btnHard.Click, Sub() LoadLeaderboard("Hard")
        AddHandler btnExpert.Click, Sub() LoadLeaderboard("Expert")
        AddHandler btnBack.Click, AddressOf btnBack_Click

        ' ===== Load default leaderboard =====
        LoadLeaderboard("Easy")
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs)
        Dim form3 As New Form3(loggedInUser)
        form3.Show()
        Me.Close()
    End Sub

    Private Sub LoadLeaderboard(mode As String)
        Try
            con.Open()
            FlowLayoutPanel1.Controls.Clear()

            ' ==== Header Panel ====
            Dim headerPanel As New Panel With {
                .Width = FlowLayoutPanel1.ClientSize.Width - 30,
                .Height = 50,
                .BackColor = Color.FromArgb(150, 0, 0, 0),
                .Margin = New Padding(10)
            }

            Dim headerTable As New TableLayoutPanel With {
                .ColumnCount = 5,
                .RowCount = 1,
                .Dock = DockStyle.Fill
            }

            For i = 0 To 4
                headerTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
            Next

            headerTable.Controls.Add(CreateHeaderLabel("Username"), 0, 0)
            headerTable.Controls.Add(CreateHeaderLabel("Speed"), 1, 0)
            headerTable.Controls.Add(CreateHeaderLabel("Accuracy"), 2, 0)
            headerTable.Controls.Add(CreateHeaderLabel("Score"), 3, 0)
            headerTable.Controls.Add(CreateHeaderLabel("Mode"), 4, 0)

            headerPanel.Controls.Add(headerTable)
            FlowLayoutPanel1.Controls.Add(headerPanel)

            ' ==== Player Records ====
            Dim cmd As New SqlCommand("SELECT Username, Speed, Accuracy, Score, Mode FROM Scores WHERE Mode=@mode ORDER BY Score DESC", con)
            cmd.Parameters.AddWithValue("@mode", mode)
            Dim dr As SqlDataReader = cmd.ExecuteReader()

            Dim dataFound As Boolean = False

            While dr.Read()
                dataFound = True

                Dim recordPanel As New Panel With {
                    .Width = FlowLayoutPanel1.ClientSize.Width - 30,
                    .Height = 50,
                    .BackColor = Color.FromArgb(100, 0, 0, 0),
                    .Margin = New Padding(10)
                }

                Dim recordTable As New TableLayoutPanel With {
                    .ColumnCount = 5,
                    .RowCount = 1,
                    .Dock = DockStyle.Fill
                }

                For i = 0 To 4
                    recordTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 20))
                Next

                recordTable.Controls.Add(CreateRecordLabel(dr("Username").ToString()), 0, 0)
                recordTable.Controls.Add(CreateRecordLabel(Math.Round(Convert.ToDouble(dr("Speed")), 2).ToString()), 1, 0)
                recordTable.Controls.Add(CreateRecordLabel(Math.Round(Convert.ToDouble(dr("Accuracy")), 2).ToString() & "%"), 2, 0)
                recordTable.Controls.Add(CreateRecordLabel(dr("Score").ToString()), 3, 0)
                recordTable.Controls.Add(CreateRecordLabel(dr("Mode").ToString()), 4, 0)

                recordPanel.Controls.Add(recordTable)
                FlowLayoutPanel1.Controls.Add(recordPanel)
            End While

            dr.Close()

            If Not dataFound Then
                Dim noDataLabel As New Label With {
                    .Text = "No data available for this mode.",
                    .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                    .ForeColor = Color.Red,
                    .TextAlign = ContentAlignment.MiddleCenter,
                    .Dock = DockStyle.Top,
                    .Height = 50,
                    .BackColor = Color.Transparent
                }
                FlowLayoutPanel1.Controls.Add(noDataLabel)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading leaderboard: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Private Function CreateHeaderLabel(text As String) As Label
        Return New Label With {
            .Text = text,
            .ForeColor = Color.Cyan,
            .Font = New Font("Rockwell", 12, FontStyle.Bold),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Dock = DockStyle.Fill,
            .BackColor = Color.Transparent
        }
    End Function

    Private Function CreateRecordLabel(text As String) As Label
        Return New Label With {
            .Text = text,
            .ForeColor = Color.White,
            .Font = New Font("Rockwell", 11, FontStyle.Regular),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Dock = DockStyle.Fill,
            .BackColor = Color.Transparent
        }
    End Function

End Class

Imports System.Data.SqlClient
Imports System.Diagnostics

Public Class Form3
    Private loggedInUser As String

    ' ===== Constructor =====
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

    ' ===== Form Load =====
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ===== Fullscreen and background =====
        If GlobalBackgroundImage IsNot Nothing Then
            Me.BackgroundImage = GlobalBackgroundImage
            Me.BackgroundImageLayout = ImageLayout.Stretch
        End If

        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None

        ' ===== FlowLayoutPanel Settings =====
        FlowLayoutPanel1.BackColor = Color.Transparent
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown
        FlowLayoutPanel1.WrapContents = False
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.Padding = New Padding((Me.ClientSize.Width - 300) \ 2, 0, 0, 0) ' center controls horizontally

        ' ===== Determine text colours based on image =====
        Dim textColor As Color = Color.White
        Dim titleColor As Color = Color.Cyan
        Dim buttonBackColor As Color = Color.FromArgb(100, 0, 0, 0) ' semi-transparent black
        Dim buttonForeColor As Color = Color.White



        ' ===== Welcome Label =====
        Dim lblWelcome As New Label With {
            .Text = "Welcome " & loggedInUser & " to",
            .Font = New Font("Rockwell", 12, FontStyle.Bold),
            .ForeColor = textColor,
            .AutoSize = False,
            .Width = 400,
            .Height = 50,
            .TextAlign = ContentAlignment.MiddleRight,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(lblWelcome)

        ' ===== Title Label =====
        Dim lblTitle As New Label With {
            .Text = "Turbo Typist",
            .Font = New Font("Segoe Script", 24, FontStyle.Bold Or FontStyle.Italic),
            .ForeColor = titleColor,
            .AutoSize = False,
            .Width = 300,
            .Height = 100,
            .TextAlign = ContentAlignment.TopRight,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(lblTitle)

        ' ===== Spacer =====
        Dim spacer As New Label With {
            .Height = 30,
            .Width = 300,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(spacer)

        ' ===== Button Style =====
        Dim buttonFont As New Font("Rockwell", 10.2, FontStyle.Bold)

        ' ===== Buttons =====
        Dim btnPlay As New Button With {
            .Text = "Play",
            .Width = 300,
            .Height = 60,
            .Font = buttonFont,
            .ForeColor = buttonForeColor,
            .BackColor = buttonBackColor,
            .FlatStyle = FlatStyle.Flat
        }
        btnPlay.FlatAppearance.BorderSize = 0
        AddHandler btnPlay.Click, AddressOf btnPlay_Click

        Dim btnDashboard As New Button With {
            .Text = "Leaderboard",
            .Width = 300,
            .Height = 60,
            .Font = buttonFont,
            .ForeColor = buttonForeColor,
            .BackColor = buttonBackColor,
            .FlatStyle = FlatStyle.Flat
        }
        btnDashboard.FlatAppearance.BorderSize = 0
        AddHandler btnDashboard.Click, AddressOf btnDashboard_Click

        Dim btnSettings As New Button With {
            .Text = "Settings",
            .Width = 300,
            .Height = 60,
            .Font = buttonFont,
            .ForeColor = buttonForeColor,
            .BackColor = buttonBackColor,
            .FlatStyle = FlatStyle.Flat
        }
        btnSettings.FlatAppearance.BorderSize = 0
        AddHandler btnSettings.Click, AddressOf btnSettings_Click

        Dim btnQuit As New Button With {
            .Text = "Quit",
            .Width = 300,
            .Height = 60,
            .Font = buttonFont,
            .ForeColor = buttonForeColor,
            .BackColor = buttonBackColor,
            .FlatStyle = FlatStyle.Flat
        }
        btnQuit.FlatAppearance.BorderSize = 0
        AddHandler btnQuit.Click, AddressOf btnQuit_Click

        ' ===== Add buttons to FlowLayoutPanel =====
        FlowLayoutPanel1.Controls.Add(btnPlay)
        FlowLayoutPanel1.Controls.Add(btnDashboard)
        FlowLayoutPanel1.Controls.Add(btnSettings)
        FlowLayoutPanel1.Controls.Add(btnQuit)
    End Sub

    ' ===== Button Click Events =====
    Private Sub btnPlay_Click(sender As Object, e As EventArgs)
        Dim gameForm As New Form4(loggedInUser)
        gameForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnDashboard_Click(sender As Object, e As EventArgs)
        Dim dashboardForm As New Form5(loggedInUser)
        dashboardForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnSettings_Click(sender As Object, e As EventArgs)
        Dim settingsForm As New Form6(loggedInUser)
        settingsForm.Show()
        Me.Hide()
    End Sub

    Private Sub btnQuit_Click(sender As Object, e As EventArgs)
        Application.Exit()
    End Sub

End Class

Imports System.Data.SqlClient
Imports System.Diagnostics

Public Class Form4
    Private selectedMode As String = ""
    Private loggedInUser As String

    Private LabelLevelMode As Label
    Private btnPlay As Button
    Private btnBack As Button

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
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ===== Fullscreen settings =====
        If GlobalBackgroundImage IsNot Nothing Then
            Me.BackgroundImage = GlobalBackgroundImage
            Me.BackgroundImageLayout = ImageLayout.Stretch
        End If

        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None

        ' ===== Determine text colours based on image =====
        Dim textColor As Color = Color.White
        Dim titleColor As Color = Color.Cyan
        Dim buttonBackColor As Color = Color.FromArgb(100, 0, 0, 0) ' semi-transparent black
        Dim buttonForeColor As Color = Color.White


        ' ===== FlowLayoutPanel settings =====
        FlowLayoutPanel1.BackColor = Color.Transparent
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown
        FlowLayoutPanel1.WrapContents = False
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.Padding = New Padding((Me.ClientSize.Width - 600) \ 2, 0, 0, 0)

        ' ===== Welcome Label =====
        Dim lblWelcome As New Label With {
            .Text = "Welcome " & loggedInUser & ", select your mode:",
            .Font = New Font("Rockwell", 12, FontStyle.Bold),
            .ForeColor = textColor,
            .AutoSize = False,
            .Width = 600,
            .Height = 50,
            .TextAlign = ContentAlignment.MiddleCenter,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(lblWelcome)

        ' ===== Title Label =====
        Dim lblTitle As New Label With {
            .Text = "Select Mode",
            .Font = New Font("Segoe Script", 24, FontStyle.Bold Or FontStyle.Italic),
            .ForeColor = titleColor,
            .AutoSize = False,
            .Width = 600,
            .Height = 80,
            .TextAlign = ContentAlignment.MiddleCenter,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(lblTitle)

        ' ===== Spacer =====
        Dim spacer As New Label With {
            .Height = 30,
            .Width = 600,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(spacer)

        ' ===== Mode Buttons Panel =====
        Dim pnlModes As New Panel With {
            .Width = 600,
            .Height = 300,
            .BackColor = Color.Transparent
        }

        CreateModeButton("Easy", pnlModes, 20, buttonBackColor, buttonForeColor)
        CreateModeButton("Hard", pnlModes, 80, buttonBackColor, buttonForeColor)
        CreateModeButton("Expert", pnlModes, 140, buttonBackColor, buttonForeColor)

        FlowLayoutPanel1.Controls.Add(pnlModes)

        ' ===== Selected mode label =====
        LabelLevelMode = New Label With {
            .Text = "Mode: ",
            .Font = New Font("Rockwell", 12, FontStyle.Bold),
            .ForeColor = textColor,
            .AutoSize = False,
            .Width = 600,
            .Height = 30,
            .TextAlign = ContentAlignment.MiddleCenter,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(LabelLevelMode)

        ' ===== Buttons Panel =====
        Dim panelButtons As New Panel With {
            .Width = 600,
            .Height = 80,
            .BackColor = Color.Transparent
        }

        btnPlay = New Button With {
            .Text = "Play",
            .Width = 150,
            .Height = 50,
            .Font = New Font("Rockwell", 10.2, FontStyle.Bold),
            .ForeColor = buttonForeColor,
            .BackColor = buttonBackColor,
            .FlatStyle = FlatStyle.Flat,
            .Location = New Point(100, 15)
        }
        btnPlay.FlatAppearance.BorderSize = 0
        AddHandler btnPlay.Click, AddressOf btnPlay_Click
        panelButtons.Controls.Add(btnPlay)

        btnBack = New Button With {
            .Text = "Back",
            .Width = 150,
            .Height = 50,
            .Font = New Font("Rockwell", 10.2, FontStyle.Bold),
            .ForeColor = buttonForeColor,
            .BackColor = buttonBackColor,
            .FlatStyle = FlatStyle.Flat,
            .Location = New Point(350, 15)
        }
        btnBack.FlatAppearance.BorderSize = 0
        AddHandler btnBack.Click, AddressOf btnBack_Click
        panelButtons.Controls.Add(btnBack)

        FlowLayoutPanel1.Controls.Add(panelButtons)
    End Sub

    Private Sub CreateModeButton(modeName As String, panel As Panel, yPos As Integer, backColor As Color, foreColor As Color)
        Dim btnMode As New Button With {
            .Text = modeName,
            .Width = 200,
            .Height = 50,
            .Top = yPos,
            .Left = 200,
            .BackColor = backColor,
            .ForeColor = foreColor,
            .Font = New Font("Rockwell", 10.2, FontStyle.Bold),
            .FlatStyle = FlatStyle.Flat
        }
        btnMode.FlatAppearance.BorderSize = 0
        AddHandler btnMode.Click, AddressOf ModeButton_Click
        panel.Controls.Add(btnMode)
    End Sub

    Private Sub ModeButton_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        selectedMode = btn.Text
        LabelLevelMode.Text = "Mode: " & selectedMode
    End Sub

    Private Sub btnPlay_Click(sender As Object, e As EventArgs)
        If selectedMode <> "" Then
            Dim form7 As New Form7(loggedInUser, selectedMode)
            form7.Show()
            Me.Hide()
        Else
            MessageBox.Show("Please select a mode before playing.")
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs)
        Dim form3 As New Form3(loggedInUser)
        form3.Show()
        Me.Close()
    End Sub

End Class

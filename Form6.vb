Imports System.Data.SqlClient
Imports System.Diagnostics

Public Class Form6
    Private loggedInUser As String
    Private lblTitle As Label

    Public Sub New(username As String)
        InitializeComponent()
        loggedInUser = username

        ' Enable flicker-free rendering
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

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ===== Fullscreen and background =====
        If GlobalBackgroundImage IsNot Nothing Then
            Me.BackgroundImage = GlobalBackgroundImage
            Me.BackgroundImageLayout = ImageLayout.Stretch
        End If

        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None

        ' ===== FlowLayoutPanel settings =====
        FlowLayoutPanel1.BackColor = Color.Transparent
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown
        FlowLayoutPanel1.WrapContents = False
        FlowLayoutPanel1.AutoScroll = False

        ' ===== Title Label =====
        lblTitle = New Label With {
            .Text = "Settings",
            .Font = New Font("Segoe Script", 24, FontStyle.Bold Or FontStyle.Italic),
            .ForeColor = Color.Cyan,
            .AutoSize = False,
            .Width = 400,
            .Height = 100,
            .TextAlign = ContentAlignment.MiddleCenter,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(lblTitle)
        CenterControl(lblTitle)

        LoadMainButtons()
    End Sub

    Private Sub CenterControl(ctrl As Control)
        ctrl.Margin = New Padding((FlowLayoutPanel1.ClientSize.Width - ctrl.Width) \ 2, 10, 0, 10)
    End Sub

    Private Sub LoadMainButtons()
        FlowLayoutPanel1.Controls.Clear()
        FlowLayoutPanel1.Controls.Add(lblTitle)
        CenterControl(lblTitle)

        Dim buttonFont As New Font("Rockwell", 12, FontStyle.Bold)
        Dim buttonBackColor As Color = Color.FromArgb(150, 0, 0, 0)
        Dim buttonForeColor As Color = Color.Cyan

        ' ===== Sign Out Button =====
        Dim btnSignOut As New Button With {
        .Text = "Sign Out",
        .Width = 300,
        .Height = 60,
        .Font = buttonFont,
        .ForeColor = buttonForeColor,
        .BackColor = buttonBackColor,
        .FlatStyle = FlatStyle.Flat
    }
        btnSignOut.FlatAppearance.BorderSize = 0
        AddHandler btnSignOut.Click, AddressOf btnSignOut_Click
        FlowLayoutPanel1.Controls.Add(btnSignOut)
        CenterControl(btnSignOut)

        ' ===== Edit Game Interface Button =====
        Dim btnEditInterface As New Button With {
        .Text = "Edit Game Interface",
        .Width = 300,
        .Height = 60,
        .Font = buttonFont,
        .ForeColor = buttonForeColor,
        .BackColor = buttonBackColor,
        .FlatStyle = FlatStyle.Flat
    }
        btnEditInterface.FlatAppearance.BorderSize = 0
        AddHandler btnEditInterface.Click, AddressOf btnEditGameInterface_Click
        FlowLayoutPanel1.Controls.Add(btnEditInterface)
        CenterControl(btnEditInterface)

        ' ===== Change Game Font Button =====
        Dim btnChangeFont As New Button With {
        .Text = "Change Game Font",
        .Width = 300,
        .Height = 60,
        .Font = buttonFont,
        .ForeColor = buttonForeColor,
        .BackColor = buttonBackColor,
        .FlatStyle = FlatStyle.Flat
    }
        btnChangeFont.FlatAppearance.BorderSize = 0
        AddHandler btnChangeFont.Click, AddressOf ShowFontOptions
        FlowLayoutPanel1.Controls.Add(btnChangeFont)
        CenterControl(btnChangeFont)

        ' ===== Back Button =====
        Dim btnBack As New Button With {
        .Text = "Back",
        .Width = 300,
        .Height = 60,
        .Font = buttonFont,
        .ForeColor = buttonForeColor,
        .BackColor = buttonBackColor,
        .FlatStyle = FlatStyle.Flat
    }
        btnBack.FlatAppearance.BorderSize = 0
        AddHandler btnBack.Click, AddressOf btnBackToForm3_Click
        FlowLayoutPanel1.Controls.Add(btnBack)
        CenterControl(btnBack)
    End Sub

    Private Sub btnBackToForm3_Click(sender As Object, e As EventArgs)
        Dim form3 As New Form3(loggedInUser)
        form3.Show()
        Me.Close()
    End Sub

    Private Sub btnEditGameInterface_Click(sender As Object, e As EventArgs)
        FlowLayoutPanel1.Controls.Clear()
        FlowLayoutPanel1.Controls.Add(lblTitle)
        CenterControl(lblTitle)

        Dim lblSelect As New Label With {
            .Text = "Select Background Image:",
            .Font = New Font("Segoe UI", 14, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(lblSelect)
        CenterControl(lblSelect)

        ' ===== TableLayoutPanel for images =====
        Dim table As New TableLayoutPanel With {
            .RowCount = 2,
            .ColumnCount = 4,
            .AutoSize = True,
            .BackColor = Color.Transparent
        }

        ' ===== All 8 images =====
        Dim images As Image() = {
            My.Resources.img1, My.Resources.img2, My.Resources.img3, My.Resources.img4,
            My.Resources.im5, My.Resources.im6, My.Resources.im7, My.Resources.im8
        }

        Dim i As Integer = 0
        For Each img In images
            Dim pic As New PictureBox With {
                .Image = img,
                .Width = 200,
                .Height = 200,
                .SizeMode = PictureBoxSizeMode.StretchImage,
                .Margin = New Padding(10)
            }
            AddHandler pic.Click, Sub()
                                      GlobalBackgroundImage = img
                                      Me.BackgroundImage = img
                                      MessageBox.Show("Background changed.")
                                  End Sub
            table.Controls.Add(pic, i Mod 4, i \ 4)
            i += 1
        Next

        FlowLayoutPanel1.Controls.Add(table)
        CenterControl(table)

        ' ===== Back button =====
        Dim btnBack As New Button With {
            .Text = "Back",
            .Width = 300,
            .Height = 60,
            .Font = New Font("Rockwell", 12, FontStyle.Bold),
            .ForeColor = Color.Cyan,
            .BackColor = Color.FromArgb(150, 0, 0, 0),
            .FlatStyle = FlatStyle.Flat
        }
        btnBack.FlatAppearance.BorderSize = 0
        AddHandler btnBack.Click, AddressOf LoadMainButtons
        FlowLayoutPanel1.Controls.Add(btnBack)
        CenterControl(btnBack)
    End Sub

    Private Sub ShowFontOptions(sender As Object, e As EventArgs)
        FlowLayoutPanel1.Controls.Clear()
        FlowLayoutPanel1.Controls.Add(lblTitle)
        CenterControl(lblTitle)

        Dim lblSelectFont As New Label With {
            .Text = "Select Game Font Style:",
            .Font = New Font("Segoe UI", 14, FontStyle.Bold),
            .ForeColor = Color.White,
            .AutoSize = True,
            .BackColor = Color.Transparent
        }
        FlowLayoutPanel1.Controls.Add(lblSelectFont)
        CenterControl(lblSelectFont)

        ' ===== Font options =====
        Dim fonts As String() = {"Segoe UI", "Consolas", "Comic Sans MS", "Times New Roman", "Courier New"}
        For Each fnt In fonts
            Dim btnFont As New Button With {
                .Text = fnt,
                .Width = 250,
                .Height = 60,
                .Font = New Font(fnt, 12, FontStyle.Bold),
                .ForeColor = Color.Cyan,
                .BackColor = Color.FromArgb(150, 0, 0, 0),
                .FlatStyle = FlatStyle.Flat
            }
            btnFont.FlatAppearance.BorderSize = 0
            AddHandler btnFont.Click, Sub()
                                          GlobalGameFont = fnt
                                          MessageBox.Show("Game font changed to " & fnt)
                                      End Sub
            FlowLayoutPanel1.Controls.Add(btnFont)
            CenterControl(btnFont)
        Next

        ' ===== Back button =====
        Dim btnBack As New Button With {
            .Text = "Back",
            .Width = 300,
            .Height = 60,
            .Font = New Font("Rockwell", 12, FontStyle.Bold),
            .ForeColor = Color.Cyan,
            .BackColor = Color.FromArgb(150, 0, 0, 0),
            .FlatStyle = FlatStyle.Flat
        }
        btnBack.FlatAppearance.BorderSize = 0
        AddHandler btnBack.Click, AddressOf LoadMainButtons
        FlowLayoutPanel1.Controls.Add(btnBack)
        CenterControl(btnBack)
    End Sub
    Private Sub btnSignOut_Click(sender As Object, e As EventArgs)
        Dim form2 As New Form2()
        form2.Show()
        Me.Close()
    End Sub

End Class

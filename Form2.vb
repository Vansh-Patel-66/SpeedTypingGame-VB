Imports System.Data.SqlClient

Public Class Form2
    Dim con As New SqlConnection("Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Project sem 5\my project\WindowsApp2\WindowsApp2\Database1.mdf;Integrated Security=True")

    Private txtloginusername As TextBox
    Private txtloginpassword As TextBox
    Private lblTitle As Label

    ' Flicker fix
    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H2000000
            Return cp
        End Get
    End Property

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        Me.DoubleBuffered = True
        MyBase.OnHandleCreated(e)
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BackgroundImage = My.Resources.login
        Me.BackgroundImageLayout = ImageLayout.Stretch

        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        IsFullScreen = True

        FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown
        FlowLayoutPanel1.WrapContents = True
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.BackColor = Color.Transparent

        lblTitle = New Label()
        lblTitle.Text = "Login"
        lblTitle.Font = New Font("Segoe Script", 36, FontStyle.Bold Or FontStyle.Italic)
        lblTitle.ForeColor = Color.Cyan
        lblTitle.AutoSize = False
        lblTitle.Width = 500
        lblTitle.Height = 80
        lblTitle.TextAlign = ContentAlignment.MiddleCenter
        lblTitle.BackColor = Color.Transparent

        LoadLoginControls()
        lblTitle.BringToFront()
    End Sub

    Private Sub Form2_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        For Each ctrl As Control In FlowLayoutPanel1.Controls
            CenterControlInFlowPanel(ctrl)
        Next
    End Sub

    Private Sub CenterControlInFlowPanel(ctrl As Control)
        ctrl.Margin = New Padding((FlowLayoutPanel1.ClientSize.Width - ctrl.Width) \ 2, 10, 0, 10)
    End Sub

    Private Sub LoadLoginControls()
        FlowLayoutPanel1.Controls.Clear()
        FlowLayoutPanel1.Controls.Add(lblTitle)
        CenterControlInFlowPanel(lblTitle)

        ' Username Label
        Dim lblUsername As New Label()
        lblUsername.Text = "Username"
        lblUsername.Font = New Font("Consolas", 16, FontStyle.Bold)
        lblUsername.ForeColor = Color.Cyan
        lblUsername.Width = 400
        lblUsername.Height = 35
        lblUsername.BackColor = Color.Transparent
        FlowLayoutPanel1.Controls.Add(lblUsername)
        CenterControlInFlowPanel(lblUsername)

        ' Username TextBox
        txtloginusername = New TextBox()
        txtloginusername.Width = 400
        txtloginusername.Height = 35
        txtloginusername.Font = New Font("Consolas", 16, FontStyle.Regular)
        txtloginusername.ForeColor = Color.White
        txtloginusername.BackColor = Color.FromArgb(40, 40, 40)
        FlowLayoutPanel1.Controls.Add(txtloginusername)
        CenterControlInFlowPanel(txtloginusername)

        ' Password Label
        Dim lblPassword As New Label()
        lblPassword.Text = "Password"
        lblPassword.Font = New Font("Consolas", 16, FontStyle.Bold)
        lblPassword.ForeColor = Color.Cyan
        lblPassword.Width = 400
        lblPassword.Height = 35
        lblPassword.BackColor = Color.Transparent
        FlowLayoutPanel1.Controls.Add(lblPassword)
        CenterControlInFlowPanel(lblPassword)

        ' Password TextBox
        txtloginpassword = New TextBox()
        txtloginpassword.Width = 400
        txtloginpassword.Height = 35
        txtloginpassword.UseSystemPasswordChar = True
        txtloginpassword.Font = New Font("Consolas", 16, FontStyle.Regular)
        txtloginpassword.ForeColor = Color.White
        txtloginpassword.BackColor = Color.FromArgb(40, 40, 40)
        FlowLayoutPanel1.Controls.Add(txtloginpassword)
        CenterControlInFlowPanel(txtloginpassword)

        ' Login Button
        Dim btnLogin As New Button()
        btnLogin.Text = "Login"
        btnLogin.Width = 250
        btnLogin.Height = 60
        btnLogin.ForeColor = Color.Cyan
        btnLogin.BackColor = Color.FromArgb(50, 50, 50)
        btnLogin.Font = New Font("Consolas", 16, FontStyle.Bold)
        AddHandler btnLogin.Click, AddressOf btnLogin_Click
        FlowLayoutPanel1.Controls.Add(btnLogin)
        CenterControlInFlowPanel(btnLogin)

        ' Signup Link
        Dim linkSignup As New LinkLabel()
        linkSignup.Text = "Don't have an account?"
        linkSignup.Font = New Font("Segoe UI", 14)
        linkSignup.Width = 250
        linkSignup.Height = 35
        linkSignup.ForeColor = Color.Cyan
        linkSignup.BackColor = Color.Transparent
        AddHandler linkSignup.LinkClicked, AddressOf linkSignup_LinkClicked
        FlowLayoutPanel1.Controls.Add(linkSignup)
        CenterControlInFlowPanel(linkSignup)

        lblTitle.BringToFront()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs)
        If txtloginusername.Text = "" Or txtloginpassword.Text = "" Then
            MessageBox.Show("Please enter both username and password")
            Return
        End If

        Try
            con.Open()
            Dim checkCmd As New SqlCommand("SELECT COUNT(*) FROM [user] WHERE username=@username AND password=@password", con)
            checkCmd.Parameters.AddWithValue("@username", txtloginusername.Text)
            checkCmd.Parameters.AddWithValue("@password", txtloginpassword.Text)
            Dim userCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If userCount > 0 Then
                Dim form3 As New Form3(txtloginusername.Text)
                form3.Show()
                Me.Hide()
            Else
                MessageBox.Show("Invalid username or password.")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Private Sub linkSignup_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Dim signupForm As New Form1()
        signupForm.Show()
        Me.Hide()
    End Sub

End Class

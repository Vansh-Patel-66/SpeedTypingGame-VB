Imports System.Data.SqlClient
Imports System.Drawing

Public Class Form1

    Dim con As New SqlConnection("Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Project sem 5\my project\WindowsApp2\WindowsApp2\Database1.mdf;Integrated Security=True;Connect Timeout=30")

    Private txtnewusername As TextBox
    Private txtnewpassword As TextBox
    Private lblTitle As Label

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = cp.ExStyle Or &H02000000
            Return cp
        End Get
    End Property

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        Me.DoubleBuffered = True
        MyBase.OnHandleCreated(e)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.BackgroundImage = My.Resources.login
        Me.BackgroundImageLayout = ImageLayout.Stretch

        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None

        ' Ensure FlowLayoutPanel1 is added in Designer or dynamically here
        FlowLayoutPanel1.FlowDirection = FlowDirection.TopDown
        FlowLayoutPanel1.WrapContents = True
        FlowLayoutPanel1.AutoScroll = True
        FlowLayoutPanel1.Dock = DockStyle.Fill
        FlowLayoutPanel1.BackColor = Color.Transparent

        lblTitle = New Label()
        lblTitle.Text = "Signup"
        lblTitle.Font = New Font("Segoe Script", 36, FontStyle.Bold Or FontStyle.Italic)
        lblTitle.ForeColor = Color.Cyan
        lblTitle.AutoSize = False
        lblTitle.Width = 500
        lblTitle.Height = 80
        lblTitle.TextAlign = ContentAlignment.MiddleCenter
        lblTitle.BackColor = Color.Transparent

        LoadSignupControls()
        lblTitle.BringToFront()
    End Sub

    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Resize
        For Each ctrl As Control In FlowLayoutPanel1.Controls
            CenterControlInFlowPanel(ctrl)
        Next
    End Sub

    Private Sub CenterControlInFlowPanel(ByVal ctrl As Control)
        ctrl.Margin = New Padding((FlowLayoutPanel1.ClientSize.Width - ctrl.Width) \ 2, 10, 0, 10)
    End Sub

    Private Sub LoadSignupControls()
        FlowLayoutPanel1.Controls.Clear()
        FlowLayoutPanel1.Controls.Add(lblTitle)
        CenterControlInFlowPanel(lblTitle)

        Dim lblUsername As New Label()
        lblUsername.Text = "Username"
        lblUsername.Font = New Font("Consolas", 16, FontStyle.Bold)
        lblUsername.ForeColor = Color.Cyan
        lblUsername.Width = 400
        lblUsername.Height = 35
        lblUsername.BackColor = Color.Transparent
        FlowLayoutPanel1.Controls.Add(lblUsername)
        CenterControlInFlowPanel(lblUsername)

        txtnewusername = New TextBox()
        txtnewusername.Width = 400
        txtnewusername.Height = 35
        txtnewusername.Font = New Font("Consolas", 16, FontStyle.Regular)
        txtnewusername.ForeColor = Color.White
        txtnewusername.BackColor = Color.FromArgb(40, 40, 40)
        FlowLayoutPanel1.Controls.Add(txtnewusername)
        CenterControlInFlowPanel(txtnewusername)

        Dim lblPassword As New Label()
        lblPassword.Text = "Password"
        lblPassword.Font = New Font("Consolas", 16, FontStyle.Bold)
        lblPassword.ForeColor = Color.Cyan
        lblPassword.Width = 400
        lblPassword.Height = 35
        lblPassword.BackColor = Color.Transparent
        FlowLayoutPanel1.Controls.Add(lblPassword)
        CenterControlInFlowPanel(lblPassword)

        txtnewpassword = New TextBox()
        txtnewpassword.Width = 400
        txtnewpassword.Height = 35
        txtnewpassword.UseSystemPasswordChar = True
        txtnewpassword.Font = New Font("Consolas", 16, FontStyle.Regular)
        txtnewpassword.ForeColor = Color.White
        txtnewpassword.BackColor = Color.FromArgb(40, 40, 40)
        FlowLayoutPanel1.Controls.Add(txtnewpassword)
        CenterControlInFlowPanel(txtnewpassword)

        Dim btnSignup As New Button()
        btnSignup.Text = "Signup"
        btnSignup.Width = 250
        btnSignup.Height = 60
        btnSignup.ForeColor = Color.Cyan
        btnSignup.BackColor = Color.FromArgb(50, 50, 50)
        btnSignup.Font = New Font("Consolas", 16, FontStyle.Bold)
        AddHandler btnSignup.Click, AddressOf btnSignup_Click
        FlowLayoutPanel1.Controls.Add(btnSignup)
        CenterControlInFlowPanel(btnSignup)

        Dim linkLogin As New LinkLabel()
        linkLogin.Text = "Already have an account?"
        linkLogin.Font = New Font("Segoe UI", 14)
        linkLogin.Width = 250
        linkLogin.Height = 35
        linkLogin.ForeColor = Color.Cyan
        linkLogin.BackColor = Color.Transparent
        AddHandler linkLogin.LinkClicked, AddressOf linkLogin_LinkClicked
        FlowLayoutPanel1.Controls.Add(linkLogin)
        CenterControlInFlowPanel(linkLogin)

        lblTitle.BringToFront()
    End Sub

    Private Sub btnSignup_Click(ByVal sender As Object, ByVal e As EventArgs)
        If txtnewusername.Text = "" Or txtnewpassword.Text = "" Then
            MessageBox.Show("Please enter both username and password")
            Return
        End If
       
        Try
            con.Open()
            Dim checkCmd As New SqlCommand("SELECT COUNT(*) FROM [user] WHERE username=@username", con)
            checkCmd.Parameters.AddWithValue("@username", txtnewusername.Text)
            Dim userCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If userCount > 0 Then
                MessageBox.Show("User already exists. Please login.")
            Else
                Dim insertCmd As New SqlCommand("INSERT INTO [user] (username, password) VALUES (@username, @password)", con)
                insertCmd.Parameters.AddWithValue("@username", txtnewusername.Text)
                insertCmd.Parameters.AddWithValue("@password", txtnewpassword.Text)
                insertCmd.ExecuteNonQuery()
                MessageBox.Show("Signup successful. Please login.")
                Dim loginForm As New Form2()
                loginForm.Show()
                Me.Hide()
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Private Sub linkLogin_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs)
        Dim loginForm As New Form2()
        loginForm.Show()
        Me.Hide()
    End Sub

End Class

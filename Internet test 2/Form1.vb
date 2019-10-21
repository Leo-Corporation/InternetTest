Imports System.Net
Imports System.Threading.Thread
Imports System.Globalization
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If My.Computer.Network.IsAvailable = True Then
            If My.Settings.LanguageUsed = "FR" Then
                Label3.Text = "Vous êtes connecté à Internet"
            ElseIf My.Settings.LanguageUsed = "EN" Then
                Label3.Text = "You are connected to Internet"
            End If

            Label3.Visible = True
            Label3.Location = New Point(248, 130)
            If My.Settings.EnableNotif = True Then
                If My.Settings.LanguageUsed = "FR" Then
                    NotifyIcon1.BalloonTipText = "Votre connexion a été testée"
                    NotifyIcon1.ShowBalloonTip(1000)
                ElseIf My.Settings.LanguageUsed = "EN" Then
                    NotifyIcon1.BalloonTipText = "Your connexion was tested"
                    NotifyIcon1.ShowBalloonTip(1000)
                End If
            Else
            End If
        Else
            If My.Settings.LanguageUsed = "FR" Then
                Label3.Text = "Vous n'êtes pas connecté à Internet"
            ElseIf My.Settings.LanguageUsed = "EN" Then
                Label3.Text = "You are not connected to Internet"
            End If
            Label3.Location = New Point(225, 130)
            If My.Settings.EnableNotif = True Then
                If My.Settings.LanguageUsed = "FR" Then
                    NotifyIcon1.BalloonTipText = "Votre connexion a été testée"
                    NotifyIcon1.ShowBalloonTip(1000)
                ElseIf My.Settings.LanguageUsed = "EN" Then
                    NotifyIcon1.BalloonTipText = "Your connexion was tested"
                    NotifyIcon1.ShowBalloonTip(1000)
                End If
            Else
            End If
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        CheckUpdate()
    End Sub
    Public Sub Reload()
        Me.Controls.Clear()
        InitializeComponent()
        If My.Settings.DarkTheme = True Then
            Me.BackColor = Color.FromArgb(50, 50, 62)
            CheckBox1.Checked = True
            'PictureBox1.Image = My.Resources.logob | Logo transparent
            PictureBox2.Image = My.Resources.updateb
            PictureBox3.Image = My.Resources.refreshb
            PictureBox4.Image = My.Resources.sunb
            Button2.BackgroundImage = My.Resources.Browser_Black
            PictureBox6.Image = My.Resources.GitHub_logo_2013_White
            Button1.BackgroundImage = My.Resources.Test_Button_Black
        Else
            Me.BackColor = Color.White
            CheckBox1.Checked = False
            'PictureBox1.Image = My.Resources.logo
            PictureBox2.Image = My.Resources.update
            PictureBox3.Image = My.Resources.refresh
            PictureBox4.Image = My.Resources.moon
            Button2.BackgroundImage = My.Resources.Browser_White
            PictureBox6.Image = My.Resources.GitHub_logo_2013_svg
            Button1.BackgroundImage = My.Resources.Test_Button_White
            My.Settings.DarkTheme = False
        End If
    End Sub
    Sub CheckUpdate()
        Dim MAJ As New WebClient
        Dim Four As New WebClient
        Dim versionActuelle As String = "3.5.2.1910"
        Dim derniereVersion As String = MAJ.DownloadString("https://dl.dropboxusercontent.com/s/j45gbzsjlfv6v4q/Versions.txt")
        Dim FourMaj As String = Four.DownloadString("https://dl.dropboxusercontent.com/s/9fqb84pj4davna9/Fournisseur.txt")
        If versionActuelle = derniereVersion Then
            UN_Update.Show()
        Else
            AV_Update.Show()
        End If
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        If My.Computer.Network.IsAvailable = True Then
            If My.Settings.LanguageUsed = "FR" Then
                Label3.Text = "Vous êtes connecté à Internet"
            ElseIf My.Settings.LanguageUsed = "EN" Then
                Label3.Text = "You are connected to Internet"
            End If

            Label3.Visible = True
            Label3.Location = New Point(248, 130)
            If My.Settings.EnableNotif = True Then
                If My.Settings.LanguageUsed = "FR" Then
                    NotifyIcon1.BalloonTipText = "Votre connexion a été testée"
                    NotifyIcon1.ShowBalloonTip(1000)
                ElseIf My.Settings.LanguageUsed = "EN" Then
                    NotifyIcon1.BalloonTipText = "Your connexion was tested"
                    NotifyIcon1.ShowBalloonTip(1000)
                End If
            Else
            End If
        Else
            If My.Settings.LanguageUsed = "FR" Then
                Label3.Text = "Vous n'êtes pas connecté à Internet"
            ElseIf My.Settings.LanguageUsed = "EN" Then
                Label3.Text = "You are not connected to Internet"
            End If
            Label3.Location = New Point(225, 130)
            If My.Settings.EnableNotif = True Then
                If My.Settings.LanguageUsed = "FR" Then
                    NotifyIcon1.BalloonTipText = "Votre connexion a été testée"
                    NotifyIcon1.ShowBalloonTip(1000)
                ElseIf My.Settings.LanguageUsed = "EN" Then
                    NotifyIcon1.BalloonTipText = "Your connexion was tested"
                    NotifyIcon1.ShowBalloonTip(1000)
                End If
            Else
            End If
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        InternetTestBrowser.Show()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged

        If CheckBox1.Checked = True Then
            Me.BackColor = Color.FromArgb(50, 50, 62)
            My.Settings.DarkTheme = True
        Else
            Me.BackColor = Color.White
            My.Settings.DarkTheme = False
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.LanguageUsed = "EN" Then
            CurrentThread.CurrentUICulture = New CultureInfo("EN")
            Reload()
        ElseIf My.Settings.LanguageUsed = "FR" Then
            CurrentThread.CurrentUICulture = New CultureInfo("FR")
            Reload()
        End If
        If My.Settings.DarkTheme = True Then
            Me.BackColor = Color.FromArgb(50, 50, 62)
            CheckBox1.Checked = True
            'PictureBox1.Image = My.Resources.logob | Logo transparent
            PictureBox2.Image = My.Resources.updateb
            PictureBox3.Image = My.Resources.refreshb
            PictureBox4.Image = My.Resources.sunb
            Button2.BackgroundImage = My.Resources.Browser_Black
            PictureBox6.Image = My.Resources.GitHub_logo_2013_White
            Button1.BackgroundImage = My.Resources.Test_Button_Black
        Else
            Me.BackColor = Color.White
            CheckBox1.Checked = False
            'PictureBox1.Image = My.Resources.logo
            PictureBox2.Image = My.Resources.update
            PictureBox3.Image = My.Resources.refresh
            PictureBox4.Image = My.Resources.moon
            Button2.BackgroundImage = My.Resources.Browser_White
            PictureBox6.Image = My.Resources.GitHub_logo_2013_svg
            Button1.BackgroundImage = My.Resources.Test_Button_White
            My.Settings.DarkTheme = False
        End If
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        If My.Settings.DarkTheme = False Then
            Me.BackColor = Color.FromArgb(50, 50, 62)
            'PictureBox1.Image = My.Resources.logob
            PictureBox2.Image = My.Resources.updateb
            PictureBox3.Image = My.Resources.refreshb
            PictureBox4.Image = My.Resources.sunb
            Button2.BackgroundImage = My.Resources.Browser_Black
            PictureBox6.Image = My.Resources.GitHub_logo_2013_White
            Button1.BackgroundImage = My.Resources.Test_Button_Black
            My.Settings.DarkTheme = True
        Else
            Me.BackColor = Color.White
            'PictureBox1.Image = My.Resources.logo
            PictureBox2.Image = My.Resources.update
            PictureBox3.Image = My.Resources.refresh
            PictureBox4.Image = My.Resources.moon
            Button2.BackgroundImage = My.Resources.Browser_White
            PictureBox6.Image = My.Resources.GitHub_logo_2013_svg
            Button1.BackgroundImage = My.Resources.Test_Button_White
            My.Settings.DarkTheme = False
        End If
    End Sub

    Private Sub PictureBox5_Click(sender As Object, e As EventArgs)
        InternetTestBrowser.Show()
    End Sub

    Private Sub PictureBox6_Click(sender As Object, e As EventArgs) Handles PictureBox6.Click
        Process.Start("https://github.com/Leo-Corporation/InternetTest")
    End Sub

    Private Sub PictureBox7_Click(sender As Object, e As EventArgs) Handles PictureBox7.Click
        Paramètres.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        InternetTestBrowser.Show()
    End Sub
End Class

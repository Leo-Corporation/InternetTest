Imports System.Threading.Thread
Imports System.Globalization
Public Class Paramètres
    Private Sub Paramètres_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.DarkTheme = True Then
            If My.Settings.LanguageUsed = "FR" Then
                ComboBox1.Text = "Sombre"
            ElseIf My.Settings.LanguageUsed = "EN" Then
                ComboBox1.Text = "Dark"
            End If
            Me.BackColor = Color.FromArgb(50, 50, 62)
            ComboBox1.BackColor = Color.FromArgb(50, 50, 62)
            ComboBox2.BackColor = Color.FromArgb(50, 50, 62)
            Button1.BackgroundImage = My.Resources.Settings_Button_Main_Black
            Button2.BackgroundImage = My.Resources.Settings_Button_Second_Black
        Else
            If My.Settings.LanguageUsed = "FR" Then
                ComboBox1.Text = "Clair"
            ElseIf My.Settings.LanguageUsed = "EN" Then
                ComboBox1.Text = "White"
            End If

            Me.BackColor = Color.White
            ComboBox1.BackColor = Color.White
            ComboBox2.BackColor = Color.White
            Button1.BackgroundImage = My.Resources.Settings_Button_Main_White
            Button2.BackgroundImage = My.Resources.Settings_Button_Second_White
        End If
        If My.Settings.EnableNotif = True Then
            CheckBox1.Checked = True
        Else
            CheckBox1.Checked = False
        End If
        If My.Settings.EnableWarnMsgBrowser = True Then
            CheckBox2.Checked = True
        Else
            CheckBox2.Checked = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If My.Settings.LanguageUsed = "FR" Then
            If ComboBox1.Text = "Clair" Then
                My.Settings.DarkTheme = False
                Me.BackColor = Color.White
                ComboBox1.BackColor = Color.White
                ComboBox2.BackColor = Color.White
                Form1.PictureBox2.Image = My.Resources.update
                Form1.PictureBox3.Image = My.Resources.refresh
                Form1.PictureBox4.Image = My.Resources.moon
                Form1.Button2.BackgroundImage = My.Resources.Browser_White
                Form1.PictureBox6.Image = My.Resources.GitHub_logo_2013_svg
                Form1.Button1.BackgroundImage = My.Resources.Test_Button_White
                Form1.BackColor = Color.White
            Else
                My.Settings.DarkTheme = True
                Me.BackColor = Color.FromArgb(50, 50, 62)
                ComboBox1.BackColor = Color.FromArgb(50, 50, 62)
                ComboBox2.BackColor = Color.FromArgb(50, 50, 62)
                Form1.PictureBox2.Image = My.Resources.updateb
                Form1.PictureBox3.Image = My.Resources.refreshb
                Form1.PictureBox4.Image = My.Resources.sunb
                Form1.Button2.BackgroundImage = My.Resources.Browser_Black
                Form1.PictureBox6.Image = My.Resources.GitHub_logo_2013_White
                Form1.Button1.BackgroundImage = My.Resources.Test_Button_Black
                Form1.BackColor = Color.FromArgb(50, 50, 62)
            End If
        ElseIf My.Settings.LanguageUsed = "EN" Then
            If ComboBox1.Text = "White" Then
                My.Settings.DarkTheme = False
                Me.BackColor = Color.White
                ComboBox1.BackColor = Color.White
                ComboBox2.BackColor = Color.White
                Form1.PictureBox2.Image = My.Resources.update
                Form1.PictureBox3.Image = My.Resources.refresh
                Form1.PictureBox4.Image = My.Resources.moon
                Form1.Button2.BackgroundImage = My.Resources.Browser_White
                Form1.PictureBox6.Image = My.Resources.GitHub_logo_2013_svg
                Form1.Button1.BackgroundImage = My.Resources.Test_Button_White
                Form1.BackColor = Color.White
            Else
                My.Settings.DarkTheme = True
                Me.BackColor = Color.FromArgb(50, 50, 62)
                ComboBox1.BackColor = Color.FromArgb(50, 50, 62)
                ComboBox2.BackColor = Color.FromArgb(50, 50, 62)
                Form1.PictureBox2.Image = My.Resources.updateb
                Form1.PictureBox3.Image = My.Resources.refreshb
                Form1.PictureBox4.Image = My.Resources.sunb
                Form1.Button2.BackgroundImage = My.Resources.Browser_Black
                Form1.PictureBox6.Image = My.Resources.GitHub_logo_2013_White
                Form1.Button1.BackgroundImage = My.Resources.Test_Button_Black
                Form1.BackColor = Color.FromArgb(50, 50, 62)
            End If
        End If

        If CheckBox1.Checked = True Then
            My.Settings.EnableNotif = True
        Else
            My.Settings.EnableNotif = False
        End If
        If CheckBox2.Checked = True Then
            My.Settings.EnableWarnMsgBrowser = True
        Else
            My.Settings.EnableWarnMsgBrowser = False
        End If
        If ComboBox2.Text = "Français" Then
            CurrentThread.CurrentUICulture = New CultureInfo("fr-FR")
            My.Settings.LanguageUsed = "FR"
        ElseIf ComboBox2.Text = "English" Then
            CurrentThread.CurrentUICulture = New CultureInfo("EN")
            My.Settings.LanguageUsed = "EN"
        End If
        Form1.Reload()
        Me.Close()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        If ComboBox2.Text = "Français" Then
            ComboBox1.Text = "Clair"
            ComboBox2.Text = "Français"
            CheckBox1.Checked = True
            CheckBox2.Checked = True
        Else
            If ComboBox1.Text = "Clair" Then
                ComboBox2.Text = "Français"
                CheckBox1.Checked = True
                CheckBox2.Checked = True
            Else
                ComboBox1.Text = "White"
                ComboBox2.Text = "Français"
                CheckBox1.Checked = True
                CheckBox2.Checked = True
            End If
        End If
    End Sub
End Class
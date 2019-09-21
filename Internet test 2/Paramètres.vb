Public Class Paramètres
    Private Sub Paramètres_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.DarkTheme = True Then
            ComboBox1.Text = "Sombre"
            Me.BackColor = Color.FromArgb(50, 50, 62)
            ComboBox1.BackColor = Color.FromArgb(50, 50, 62)
        Else
            ComboBox1.Text = "Clair"
            Me.BackColor = Color.White
            ComboBox1.BackColor = Color.White
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
        If ComboBox1.Text = "Clair" Then
            My.Settings.DarkTheme = False
            Me.BackColor = Color.White
            ComboBox1.BackColor = Color.White
            Form1.PictureBox2.Image = My.Resources.update
            Form1.PictureBox3.Image = My.Resources.refresh
            Form1.PictureBox4.Image = My.Resources.moon
            Form1.PictureBox5.Image = My.Resources.Internet
            Form1.PictureBox6.Image = My.Resources.GitHub_logo_2013_svg
            Form1.Button1.BackgroundImage = My.Resources.Test_Button_White
            Form1.BackColor = Color.White
        Else
            My.Settings.DarkTheme = True
            Me.BackColor = Color.FromArgb(50, 50, 62)
            ComboBox1.BackColor = Color.FromArgb(50, 50, 62)
            Form1.PictureBox2.Image = My.Resources.updateb
            Form1.PictureBox3.Image = My.Resources.refreshb
            Form1.PictureBox4.Image = My.Resources.sunb
            Form1.PictureBox5.Image = My.Resources.Internetb
            Form1.PictureBox6.Image = My.Resources.GitHub_logo_2013_White
            Form1.Button1.BackgroundImage = My.Resources.Test_Button_Black
            Form1.BackColor = Color.FromArgb(50, 50, 62)
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
        Me.Close()
    End Sub
End Class
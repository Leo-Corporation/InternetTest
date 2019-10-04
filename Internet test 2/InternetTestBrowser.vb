Imports System.Threading.Thread
Imports System.Globalization
Public Class InternetTestBrowser

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Dim textUrl As String = TextBox1.Text
        Dim uriURL As New Uri(textUrl)

        WebBrowser1.Url = uriURL
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        WebBrowser1.GoBack()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        WebBrowser1.GoForward()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click

    End Sub

    Private Sub TextBox1_Click(sender As Object, e As EventArgs) Handles TextBox1.Click
        TextBox1.Text = "https://"
    End Sub

    Private Sub InternetTestBrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.EnableWarnMsgBrowser = True Then
            If My.Settings.LanguageUsed = "FR" Then
                MsgBox("Attention, ceci est un navigateur qui a juste pour but de tester votre connexion internet. Certains sites ne fonctionnerons pas.", MsgBoxStyle.Exclamation)
            ElseIf My.Settings.LanguageUsed = "EN" Then
                MsgBox("Warning, this is a browser is just here to test your internet connexion. Some sites won't work.", MsgBoxStyle.Exclamation)
            End If

        Else
        End If
        If My.Settings.DarkTheme = True Then
            PictureBox1.Image = My.Resources.Bouton_retour_DARK
            PictureBox2.Image = My.Resources.Bouton_avancer_DARK
            PictureBox3.Image = My.Resources.Barre_recherche_DARK
            PictureBox4.Image = My.Resources.Bouton_avancer_DARK
            TextBox1.BackColor = Color.FromArgb(50, 50, 62)
            TextBox1.ForeColor = Color.White
        Else
            PictureBox1.Image = My.Resources.Bouton_retour
            PictureBox2.Image = My.Resources.Bouton_avancer
            PictureBox3.Image = My.Resources.Barre_recherche
            PictureBox4.Image = My.Resources.Bouton_avancer
            TextBox1.BackColor = Color.White
            TextBox1.ForeColor = Color.Black
        End If
    End Sub
End Class
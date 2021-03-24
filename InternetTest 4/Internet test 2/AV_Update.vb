Imports System.Net
Public Class AV_Update
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim start = Application.StartupPath
        Process.Start(start & "\InternetTestUpdater.exe")
        Application.Exit()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub AV_Update_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.DarkTheme = True Then
            Me.BackColor = Color.FromArgb(50, 50, 62)
            Button1.BackgroundImage = My.Resources.Settings_Button_Main_Black
            Button2.BackgroundImage = My.Resources.Settings_Button_Second_Black
        Else
            Me.BackColor = Color.White
            Button1.BackgroundImage = My.Resources.Settings_Button_Main_White
            Button2.BackgroundImage = My.Resources.Settings_Button_Second_White
        End If
        Dim MAJ As New WebClient()
        Dim fourMAJ As New WebClient()
        Dim version = MAJ.DownloadString("https://dl.dropboxusercontent.com/s/j45gbzsjlfv6v4q/Versions.txt")
        Dim fournisseur = fourMAJ.DownloadString("https://dl.dropboxusercontent.com/s/9fqb84pj4davna9/Fournisseur.txt")
        Label4.Text += " " & version
        Label5.Text += " " & fournisseur
    End Sub
End Class
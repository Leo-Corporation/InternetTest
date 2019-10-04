Public Class UN_Update
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Me.Close()
    End Sub

    Private Sub UN_Update_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Settings.DarkTheme = True Then
            Me.BackColor = Color.FromArgb(50, 50, 62)
            Button1.BackgroundImage = My.Resources.Settings_Button_Main_Black
        Else
            Me.BackColor = Color.White
            Button1.BackgroundImage = My.Resources.Settings_Button_Main_White
        End If
    End Sub
End Class
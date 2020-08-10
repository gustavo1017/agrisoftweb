Imports System.Web.Services
Imports AgrisoftWeb.BL
Imports SendGrid
Imports SendGrid.Helpers.Mail

Public Class ResetPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    <WebMethod>
    Public Shared Function RequestResetPassword(ByVal email As String) As String
        Try
            Dim newToken As String = SaveRequest(email)
            If newToken = "" Then
                Return "EmailNotExists"
            End If

            Dim apiKey As String = ConfigurationManager.AppSettings("SendGrid_ApiKey")
            Dim client As New SendGridClient(apiKey)
            Dim emailFrom As String = ConfigurationManager.AppSettings("SendGrid_EmailFrom")
            Dim emailFromName As String = ConfigurationManager.AppSettings("SendGrid_EmailFromName")
            Dim fromEmail As New EmailAddress(emailFrom, emailFromName)
            Dim subject As String = ConfigurationManager.AppSettings("SendGrid_EmailSubject")
            Dim toEmail As New EmailAddress(email, "")
            Dim plainTextContent As String = ""
            Dim linkResetPwd As String = String.Format(ConfigurationManager.AppSettings("ResetPasswordLink"), newToken)
            Dim htmlContent As String = String.Format("Usted solicitó cambiar la contraseña para acceder a AgrisoftWeb.<br />Link:<br /><a href=""{0}"">Retablecer Contraseña</a><br /><br />Haga click en este link para restablecer su contraseña.", linkResetPwd)
            Dim msg As SendGridMessage = MailHelper.CreateSingleEmail(fromEmail, toEmail, subject, plainTextContent, htmlContent)
            Dim emailResponse = client.SendEmailAsync(msg)

            Return "OK"
        Catch ex As Exception
            Return "ERROR"
        End Try

    End Function

    Public Shared Function SaveRequest(ByVal email As String) As String
        Dim objGenericMethods As New GenericMethods()

        '' Get UserId from EMail
        'Dim idUsuario As Integer = objGenericMethods.GetUserIDByEmail(email)
        'If idUsuario = 0 Then
        '    Return ""
        'End If

        'Insertar en la nueva tabla la relación de UserID con requestID
        Dim newToken As Guid = Guid.NewGuid()
        'objGenericMethods.CreateUserToken(idUsuario, newToken)

        Return newToken.ToString()
    End Function

End Class
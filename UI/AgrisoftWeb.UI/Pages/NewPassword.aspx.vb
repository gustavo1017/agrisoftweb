Imports System.Web.Services
Imports AgrisoftWeb.BL

Public Class NewPassword
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            hdnToken.Value = Request.QueryString("TokenID")
        End If

    End Sub

    <WebMethod>
    Public Shared Function UpdatePassword(ByVal newPassword As String, ByVal token As String) As String
        Dim objGenericMethods As New GenericMethods()

        'Get UserID from Token on QueryString
        'Dim idUsuario As Integer = objGenericMethods.GetUserByToken(token)

        'Call to Repository to update User password
        'objGenericMethods.UpdateUserPassword(idUsuario, newPassword)

        Return "OK"
    End Function

End Class
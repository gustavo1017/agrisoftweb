Imports System.Threading
Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources
Imports Facebook

Public Class frmLoginServer
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Response.Redirect("frmMainMenu.aspx")
    End Sub

    Protected Sub btnFacebook_Click(sender As Object, e As ImageClickEventArgs) Handles btnFacebook.Click

    End Sub
End Class
Public Class frmCultivos_ordenarJS
    Inherits BasePage

    Private ListItemsJSValue As String
    Protected Property ListItemsJS() As String
        Get
            Return ListItemsJSValue
        End Get
        Set(ByVal value As String)
            ListItemsJSValue = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        Dim lstItems As New List(Of String)
        lstItems.Add("Item01")
        lstItems.Add("Item02")
        lstItems.Add("Item03")
        lstItems.Add("Item04")
        lstItems.Add("Item05")

        ListItemsJSValue = String.Format("['{0}']", String.Join("','", lstItems))

        ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "text", "fillList()", True)
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        ScriptManager.RegisterStartupScript(Me.Page, Page.GetType(), "text", "test()", True)
    End Sub

    Protected Sub btnProcess_Click(sender As Object, e As EventArgs)
        Dim lista As Object = hdnList.Value
        Label1.Text = "Llamando desde JS"
    End Sub
End Class
Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnArriba_Click(sender As Object, e As EventArgs)
        Dim str_Renamed As String
        Dim Index As Short

        Index = lstActividades.SelectedIndex
        str_Renamed = lstActividades.SelectedValue
        lstActividades.Items.RemoveAt(lstActividades.SelectedIndex)
        lstActividades.Items.Insert(Index - 1, str_Renamed)
        lstActividades.SelectedIndex = Index - 1
    End Sub
End Class
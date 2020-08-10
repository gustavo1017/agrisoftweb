Imports AgrisoftWeb.BL

Public Class frmActividades_ordenar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack() Then
            Dim RS As New ADODB.Recordset
            Dim objBL As New ActividadesOrdenar("Fundo0")
            RS = objBL.GetListaActividades()

            ' Si no encontro registros
            If RS.BOF Then
                Exit Sub
            End If

            ' Llena la lista
            RS.MoveFirst()
            While Not RS.EOF
                lstActividades.Items.Add(RS.Fields.Item(1).Value)
                RS.MoveNext()
            End While
            RS = New ADODB.Recordset
            btnUp.Enabled = False
            btnDown.Enabled = False
        End If
    End Sub

    Protected Sub lstActividades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstActividades.SelectedIndexChanged
        VerifyButtons()
    End Sub

    Private Sub VerifyButtons()
        If lstActividades.SelectedIndex = 0 Then
            btnUp.Enabled = False
            btnDown.Enabled = True
        Else
            btnUp.Enabled = True
        End If
        If lstActividades.SelectedIndex = lstActividades.Items.Count - 1 Then
            btnDown.Enabled = False
            btnUp.Enabled = True
        Else
            btnDown.Enabled = True
        End If
    End Sub

    Protected Sub btnUp_Click(sender As Object, e As EventArgs)
        Dim str_Renamed As String
        Dim Index As Short

        Index = lstActividades.SelectedIndex
        str_Renamed = lstActividades.SelectedValue
        lstActividades.Items.RemoveAt(lstActividades.SelectedIndex)
        lstActividades.Items.Insert(Index - 1, str_Renamed)
        lstActividades.SelectedIndex = Index - 1

        VerifyButtons()
    End Sub

    Protected Sub btnDown_Click(sender As Object, e As EventArgs)
        Dim str_Renamed As String
        Dim Index As Short

        Index = lstActividades.SelectedIndex
        str_Renamed = lstActividades.SelectedValue
        lstActividades.Items.RemoveAt(lstActividades.SelectedIndex)
        lstActividades.Items.Insert(Index + 1, str_Renamed)
        lstActividades.SelectedIndex = Index + 1

        VerifyButtons()
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        Dim objBl As New ActividadesOrdenar("Fundo0")
        Dim dctCultivos As New Dictionary(Of Integer, String)
        Dim i As Integer

        For i = 0 To lstActividades.Items.Count - 1
            dctCultivos.Add(i, lstActividades.Items(i).Value)
        Next

        objBl.UpdateActividadOrden(dctCultivos)

        lblResults.Text = Resources.Resource1.str543
    End Sub
End Class
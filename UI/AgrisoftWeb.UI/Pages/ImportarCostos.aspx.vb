Imports System.IO
Imports AgrisoftWeb.BL

Public Class ImportarCostos
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        If Not Page.IsPostBack() Then
            ' Dim gblstrIdUsuario As String = "Fundo0"
            currentModule = "Herramientas.Importar"
            Dim intAcceso As Integer = HabilitaFrame()
            If (intAcceso = -1) Then
                Response.Redirect("Unauthorized.aspx")
            End If
        End If
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            If fuImport.HasFile Then
                Dim rsCategorias As Object
                'Dim cnnText As ADODB.Connection
                'Dim strRt, strArchivo As String

                rsCategorias = New ADODB.Recordset
                'Dim ssql = "select * from categoria where id_categoria='OBRE'"
                'rsCategorias = DBconn.Execute(ssql)

                'strRt = fuImport.PostedFile.FileName.Substring(0, InStrRev(fuImport.PostedFile.FileName, "\") - 1)
                'strArchivo = fuImport.FileName
                'cnnText = New ADODB.Connection
                'With cnnText
                '    .Provider = "MSDASQL.1"
                '    .ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strRt & ";" & "Extended Properties='text;HDR=No;FMT=Delimited'"
                '    .Open()
                'End With

                ' Abrimos el objeto Recordset
                Dim lstContentRows As New List(Of String)
                Using reader As New StreamReader(fuImport.FileContent)
                    Do While reader.Peek() <> -1
                        lstContentRows.Add(reader.ReadLine())
                    Loop

                    reader.Close()
                End Using

                Dim objBL As New ImportarCostosBL("Fundo0")
                objBL.CargarTabla(lstContentRows, False)
                dvMessage.Visible = True
                lblResults.Text = Resources.Resource1.str999993 ' "Archivo cargado satisfactoriamente"
            End If
        Catch ex As Exception
            dvMessage.Visible = True
            lblResults.Text = Resources.Resource1.str999992 '"Hubo un problema al cargar el archivo"
        End Try

    End Sub

    Protected Sub btnUploadPlan_Click(sender As Object, e As EventArgs) Handles btnUploadPlan.Click
        Try
            If fuImportPlan.HasFile Then
                Dim rsCategorias As Object
                rsCategorias = New ADODB.Recordset

                ' Abrimos el objeto Recordset
                Dim lstContentRows As New List(Of String)
                Using reader As New StreamReader(fuImportPlan.FileContent)
                    Do While reader.Peek() <> -1
                        lstContentRows.Add(reader.ReadLine())
                    Loop

                    reader.Close()
                End Using

                Dim objBL As New ImportarCostosBL("Fundo0")
                objBL.CargarTabla(lstContentRows, True)
                dvMessage.Visible = True
                dvMessage.Attributes("class") = "alert alert-success"
                lblResults.Text = Resources.Resource1.str999993 ' "Archivo cargado satisfactoriamente"
            End If
        Catch ex As Exception
            dvMessage.Visible = True
            dvMessage.Attributes("class") = "alert alert-danger"
            lblResults.Text = Resources.Resource1.str999992 '"Hubo un problema al cargar el archivo"
        End Try
    End Sub
End Class
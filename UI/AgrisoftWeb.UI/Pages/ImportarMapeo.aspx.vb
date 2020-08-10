Imports System.IO
Imports AgrisoftWeb.BL

Public Class ImportarMapeo
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
            rbtnPlantacion.Checked = True
            fuImportPlan.Enabled = False
            btnUploadPlan.Enabled = False
            fuImport.Enabled = True
            btnUpload.Enabled = True
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

                Dim objBL As New ImportarMapeoBL("Fundo0")

                'Validar si es que se ha subido previamente el file
                Dim fileExists As Boolean = objBL.ValidateFileNameExists(fuImport.FileName, False)
                If fileExists Then
                    dvMessage.Visible = True
                    dvMessage.Attributes("class") = "alert alert-danger"
                    lblResults.Text = "El archivo ya se ha cargado anteriormente"
                    Return
                End If

                ' Abrimos el objeto Recordset
                Dim lstContentRows As New List(Of String)
                Using reader As New StreamReader(fuImport.FileContent)
                    Do While reader.Peek() <> -1
                        lstContentRows.Add(reader.ReadLine())
                    Loop

                    reader.Close()
                End Using

                ' Cargar Data en BD
                objBL.CargarTabla(lstContentRows, False, fuImport.FileName)
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

    Protected Sub btnUploadPlan_Click(sender As Object, e As EventArgs) Handles btnUploadPlan.Click
        Try
            If fuImportPlan.HasFile Then
                Dim rsCategorias As Object
                rsCategorias = New ADODB.Recordset

                Dim objBL As New ImportarMapeoBL("Fundo0")
                'Validar si es que se ha subido previamente el file
                Dim fileExists As Boolean = objBL.ValidateFileNameExists(fuImportPlan.FileName, True)
                If fileExists Then
                    dvMessage.Visible = True
                    dvMessage.Attributes("class") = "alert alert-danger"
                    lblResults.Text = "El archivo ya se ha cargado anteriormente"
                    Return
                End If

                ' Abrimos el objeto Recordset
                Dim lstContentRows As New List(Of String)
                Using reader As New StreamReader(fuImportPlan.FileContent)
                    Do While reader.Peek() <> -1
                        lstContentRows.Add(reader.ReadLine())
                    Loop

                    reader.Close()
                End Using

                objBL.CargarTabla(lstContentRows, True, fuImportPlan.FileName)
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

    Private Sub rbtnCosechas_CheckedChanged(sender As Object, e As EventArgs) Handles rbtnCosechas.CheckedChanged
        fuImportPlan.Enabled = True
        btnUploadPlan.Enabled = True
        fuImport.Enabled = False
        btnUpload.Enabled = False
    End Sub

    Private Sub rbtnPlantacion_CheckedChanged(sender As Object, e As EventArgs) Handles rbtnPlantacion.CheckedChanged
        fuImportPlan.Enabled = False
        btnUploadPlan.Enabled = False
        fuImport.Enabled = True
        btnUpload.Enabled = True
    End Sub
End Class
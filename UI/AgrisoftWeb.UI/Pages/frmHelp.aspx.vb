Imports System.IO

Public Class frmHelp
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
    End Sub

    Protected Sub lbtnBuenasPracticas_Click(sender As Object, e As EventArgs)
        Dim fileName As String = ConfigurationManager.AppSettings("AgriWeb_ManualPracticasName")
        DownloadFile(fileName)
    End Sub

    Protected Sub lbtnManualUsuario_Click(sender As Object, e As EventArgs)
        Dim fileName As String = ConfigurationManager.AppSettings("AgriWeb_ManualUsuarioName")
        DownloadFile(fileName)
    End Sub

    Protected Sub lbtnFAQ_Click(sender As Object, e As EventArgs)
        Dim fileName As String = ConfigurationManager.AppSettings("AgriWeb_FAQName")
        DownloadFile(fileName)
    End Sub

    Protected Sub lbtnFormatos_Click(sender As Object, e As EventArgs)
        Dim fileName As String = ConfigurationManager.AppSettings("AgriWeb_FormatosName")
        DownloadFile(fileName)
    End Sub

    Private Sub DownloadFile(ByVal fileName)
        Dim filesDirectory As String = ConfigurationManager.AppSettings("AgriWeb_FilesPath")
        Dim fullFileName As String = Path.Combine(filesDirectory, fileName)
        Dim fileFAQ As FileInfo = New FileInfo(fullFileName)

        If fileFAQ.Exists Then
            'Response.Clear()
            'Response.ClearHeaders()
            'Response.ClearContent()
            'Response.AddHeader("Content-Disposition", "attachment; filename=" + fileFAQ.Name.Replace("pdf", "PDF"))
            'Response.AddHeader("Content-Length", fileFAQ.Length.ToString())
            ''Response.ContentType = "text/plain"
            'Response.ContentType = "application/octet-stream"
            'Response.Flush()
            'Response.TransmitFile(fileFAQ.FullName)
            'Response.End()

            Dim client As New Net.WebClient()
            Dim buffer As Byte() = client.DownloadData(fileFAQ.FullName)
            Response.ContentType = "application/pdf"
            Response.AddHeader("Content-Length", fileFAQ.Length.ToString())
            Response.BinaryWrite(buffer)
        End If
    End Sub

    Protected Sub lblPlantillaExcel_Click(sender As Object, e As EventArgs) Handles lblPlantillaExcel.Click
        Dim fileFAQ As FileInfo = New FileInfo(HttpContext.Current.Server.MapPath("~/App_Data/plantillaExcel.xls"))

        If fileFAQ.Exists Then
            'Response.Clear()
            'Response.ClearHeaders()
            'Response.ClearContent()
            'Response.AddHeader("Content-Disposition", "attachment; filename=" + fileFAQ.Name.Replace("pdf", "PDF"))
            'Response.AddHeader("Content-Length", fileFAQ.Length.ToString())
            ''Response.ContentType = "text/plain"
            'Response.ContentType = "application/octet-stream"
            'Response.Flush()
            'Response.TransmitFile(fileFAQ.FullName)
            'Response.End()

            Dim client As New Net.WebClient()
            Dim buffer As Byte() = client.DownloadData(fileFAQ.FullName)
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=plantillaExcel.xls")
            Response.AddHeader("Content-Length", fileFAQ.Length.ToString())
            Response.BinaryWrite(buffer)
        End If
    End Sub
End Class
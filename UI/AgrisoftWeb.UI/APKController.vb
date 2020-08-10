Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Web.Http

Public Class APKController
    Inherits ApiController

    ' GET api/<controller>
    Public Function GetValues() As HttpResponseMessage

        Dim fileToDownload = HttpContext.Current.Server.MapPath("~/App_Data/tareopersonal.apk")
        Dim fileData() As Byte = System.IO.File.ReadAllBytes(fileToDownload)

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.Charset = ""
        HttpContext.Current.Response.ContentType = "application/vnd.android.package-archive"
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=tareopersonal.apk")

        HttpContext.Current.Response.OutputStream.Write(fileData, 0, fileData.Length)
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.End()

    End Function
End Class

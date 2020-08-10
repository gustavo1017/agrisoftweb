Imports System.Net.Http.Formatting
Imports System.Web.Http
Imports System.Web.Routing

Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started

        RouteTable.Routes.MapHttpRoute(
            "APKRoute",
            "{controller}")

        RouteTable.Routes.MapHttpRoute(
            "DefaultAPI",
            "api/{controller}/{id}",
            New With {.id = System.Web.Http.RouteParameter.Optional})

    End Sub
End Class
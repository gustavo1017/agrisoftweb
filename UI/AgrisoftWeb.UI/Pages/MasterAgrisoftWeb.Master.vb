Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports AgrisoftWeb.BL

Public Class MasterAgrisoftWeb
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Verify Login Status


        If Not Page.IsPostBack() Then
            'If ddlLanguage.Items.FindByValue(CultureInfo.CurrentCulture.Name) IsNot Nothing Then
            '    ddlLanguage.Items.FindByValue(CultureInfo.CurrentCulture.Name).Selected = True
            'End If
            If ddlLanguage.Items.FindByValue(Thread.CurrentThread.CurrentCulture.Name) IsNot Nothing Then
                ddlLanguage.Items.FindByValue(Thread.CurrentThread.CurrentCulture.Name).Selected = True
            End If

            'No se llamará porque se respeta los permisos que vienen desde la página Login
            'pHabilitarMenu()
        End If
    End Sub

    Protected Sub Menu1_MenuItemDataBound(sender As Object, e As MenuEventArgs)
        Dim currentMenu As Menu = CType(sender, Menu)
        Dim mapNode As SiteMapNode = CType(e.Item.DataItem, SiteMapNode)

        'If mapNode.Title = "Herramientas" Then
        '    Dim itemToRemove As MenuItem = currentMenu.FindItem("Home/" & mapNode.Title)
        '    itemToRemove.Parent.ChildItems.Remove(itemToRemove)
        'End If

        'If mapNode.Title = "Agricultura" Then
        '    Dim itemToRemove As MenuItem = currentMenu.FindItem("Home/" & mapNode.Title)
        '    itemToRemove.Enabled = False
        'End If

        'If mapNode.Title = "Cierre de Seguridad" Then
        '    e.Item.Target = mapNode("Target")
        '    e.Item.Parent.ChildItems.Remove(e.Item)
        'End If

        'If mapNode("menuKey") = "impCostos" Then
        '    e.Item.Target = mapNode("Target")
        '    e.Item.Enabled = False
        'End If

        Try
            Dim sessionName As String = String.Format("{0}.MenuEnabled", "Fundo0")
            Dim lstMenusEnabled As New List(Of String)
            If (Session(sessionName) IsNot Nothing) Then
                Dim objMenus As New MenuItemsPermission()
                objMenus = CType(Session(sessionName), MenuItemsPermission)

                For Each permisosMenu As KeyValuePair(Of String, List(Of String)) In objMenus.MenuPermissions
                    If permisosMenu.Value.Contains(mapNode("menuKey")) Then
                        e.Item.Target = mapNode("Target")

                        If permisosMenu.Key = "rutvisible" Then
                            e.Item.Parent.ChildItems.Remove(e.Item)
                        End If
                        If permisosMenu.Key = "ruthabilitar" Then
                            e.Item.Enabled = False
                        End If
                        If permisosMenu.Key = "ruthabilitarusuario" Then
                            e.Item.Enabled = True
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            Dim objBL As New GenericMethods()
            objBL.RegisterEvent(dctException)
        End Try


    End Sub

    Public Sub pHabilitarMenu()
        Dim rsFormEspe As ADODB.Recordset
        Dim lstMenus As New List(Of String)

        Dim objGenericMethods As New GenericMethods("Fundo0")
        rsFormEspe = objGenericMethods.getFormulariosEspeciales()

        While Not rsFormEspe.EOF
            lstMenus.Add(Trim(rsFormEspe.Fields("id_formulario").Value))
            rsFormEspe.MoveNext()
        End While

        Dim sessionName As String = String.Format("{0}.MenuEnabled", "Fundo0")

        Dim objMenuItems As New MenuItemsPermission()
        objMenuItems.MenuPermissions.Add("ruthabilitar", lstMenus)
        HttpContext.Current.Session.Add(sessionName, objMenuItems)

        rsFormEspe.Close()
    End Sub

    Public Sub RecorrerMenus(ByVal lstMenuItems As List(Of String), ByVal opcion As String)
        'Dim Nodes As SiteMapNodeCollection = SiteMap.RootNode.ChildNodes
    End Sub

End Class
Imports AgrisoftWeb.BL
Imports System.Web

'Namespace AgrisoftWeb.UI
Public Module UIModule
    Public Function CampoCampana(ByRef strCampana As String, ByVal cboSelectedIndex As Integer) As String
        If CDbl(strCampana) = Int(CDbl(strCampana)) Then
            If cboSelectedIndex = 0 Then
                CampoCampana = "cast(COSTOS.campana as int)"
            Else
                CampoCampana = "cast(COSTOSME.campana as int)"
            End If
        Else
            If cboSelectedIndex = 0 Then
                CampoCampana = "(COSTOS.campana)"
            Else
                CampoCampana = "(COSTOSME.campana)"
            End If
        End If
    End Function

    Public Sub pHabilitarMenuPorUsuario(ByVal pUsuario As String)
        Dim RS As New ADODB.Recordset
        Dim rsFormEspe As New ADODB.Recordset
        Dim lstMenus As New List(Of String)

        ' Get BusinessUser
        Dim objGenericMethods As New GenericMethods()
        Dim strBusinessUser As String = "DEMO01"

        objGenericMethods = New GenericMethods(pUsuario)
        RS = objGenericMethods.getModuloByUsuario(strBusinessUser)

        If Not RS.EOF Then
            Do While Not RS.EOF
                rsFormEspe = objGenericMethods.getFormulariosEspecialesByModulo(RS.Fields("id_modulo").Value)

                If Not rsFormEspe.EOF Then
                    Do While Not rsFormEspe.EOF
                        lstMenus.Add((Trim(rsFormEspe.Fields("id_formulario").Value)))
                        rsFormEspe.MoveNext()
                    Loop
                End If

                RS.MoveNext()
            Loop
        End If

        Dim lstMenusDisabled As New List(Of String)
        Dim RSDisabled As New ADODB.Recordset
        RSDisabled = objGenericMethods.getFormulariosEspecialesDeshabilitadoPorUsuario(strBusinessUser)

        If Not RSDisabled.EOF Then
            Do While Not RSDisabled.EOF
                lstMenusDisabled.Add((Trim(RSDisabled.Fields("id_formulario").Value)))
                RSDisabled.MoveNext()
            Loop
        End If

        Dim lstMenuToHide As New List(Of String)
        Dim RSMenuHide As New ADODB.Recordset
        RSMenuHide = objGenericMethods.ControlPermisos() 'TODO: Falta filtrar por Usuario.

        If Not RSMenuHide.EOF Then
            Do While Not RSMenuHide.EOF
                lstMenuToHide.Add((Trim(RSMenuHide.Fields("id_formulario").Value)))
                RSMenuHide.MoveNext()
            Loop
        End If

        Dim sessionName As String = String.Format("{0}.MenuEnabled", pUsuario)
        Dim objMenuItems As New MenuItemsPermission()
        objMenuItems.MenuPermissions.Add("ruthabilitarusuario", lstMenus)
        objMenuItems.MenuPermissions.Add("ruthabilitar", lstMenusDisabled)
        objMenuItems.MenuPermissions.Add("rutvisible", lstMenuToHide)
        HttpContext.Current.Session.Add(sessionName, objMenuItems)
    End Sub

End Module

Public Class MenuItemsPermission

    Sub New()
        _menuPermissions = New Dictionary(Of String, List(Of String))
    End Sub

    Private _menuPermissions As Dictionary(Of String, List(Of String))
    Public Property MenuPermissions() As Dictionary(Of String, List(Of String))
        Get
            Return _menuPermissions
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of String)))
            _menuPermissions = value
        End Set
    End Property
End Class
'End Namespace


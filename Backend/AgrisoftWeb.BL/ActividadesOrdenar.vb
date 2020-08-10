Imports System.Configuration
Imports AgrisoftWeb.BL

Public Class ActividadesOrdenar
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Function GetListaActividades() As ADODB.Recordset
        Dim ssql As Object
        Dim RS As New ADODB.Recordset
        'Dim DBconn As ADODB.Connection = objGenericMethods.SetupConnection(LoggedUser)

        ssql = "SELECT id_actividad, descripcion from ACTIVIDADES ORDER BY ubicacion_cc ASC"
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic

        RS.let_Source(ssql)
        RS.Open()

        Return RS
    End Function

    Public Sub UpdateActividadOrden(ByVal lstcultivos As Dictionary(Of Integer, String))
        Dim ssql As String
        Dim objGenericMethods As New GenericMethods("")
        'Dim DBconn As ADODB.Connection = objGenericMethods.SetupConnection(LoggedUser)

        For Each cultivo As KeyValuePair(Of Integer, String) In lstcultivos
            ssql = "UPDATE ACTIVIDADES SET ubicacion_cc=" & CStr(cultivo.Key) & " WHERE descripcion='" & cultivo.Value & "'"
            DBconn.Execute(ssql)
        Next
    End Sub
End Class

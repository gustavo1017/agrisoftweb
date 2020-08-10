Imports System.Configuration

Public Class CultivosOrdenar
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Function GetListaCultivos() As ADODB.Recordset
        Dim ssql As Object
        Dim RS As New ADODB.Recordset
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)

        ssql = "SELECT id_cultivo, descripcion from CULTIVOS ORDER BY orden ASC"
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic

        RS.let_Source(ssql)
        RS.Open()

        Return RS
    End Function

    Public Sub UpdateCultivoOrden(ByVal lstcultivos As Dictionary(Of Integer, String))
        Dim ssql As String
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)

        For Each cultivo As KeyValuePair(Of Integer, String) In lstcultivos
            ssql = "UPDATE CULTIVOS SET orden=" & CStr(cultivo.Key) & " WHERE descripcion='" & cultivo.Value & "'"
            DBconn.Execute(ssql)
        Next
    End Sub
End Class

Imports System.Configuration

Public Class ReporteZTCostosRecursosEtapasActividades
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Function getDataZonaTrabajo(ByVal IdUsuario As String, ByRef MatrizZ As Object, ByVal id As String, ByVal desc As String) As ADODB.Recordset
        Dim adoTabla As ADODB.Recordset
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)
        Dim ssql As String = "SELECT ZONA_TRABAJO.id_zonatrabajo,ZONA_TRABAJO.descripcion,campana,hectareas FROM ZONA_TRABAJO INNER JOIN USUARIOCCOSTOS ON ZONA_TRABAJO.id_fundo = USUARIOCCOSTOS.id_fundo WHERE (ZONA_TRABAJO.estado <> 0) AND (ZONA_TRABAJO.tipo = 'C') AND (USUARIOCCOSTOS.Id_usuario = '" & IdUsuario & "') ORDER BY ZONA_TRABAJO.descripcion"

        adoTabla = New ADODB.Recordset
        adoTabla.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

        Return adoTabla
    End Function

    Public Function Refresh_Query(ByRef strSQL As String) As ADODB.Recordset
        Dim rsReporte As New ADODB.Recordset
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)

        rsReporte.Open(strSQL, DBconn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        Return rsReporte
    End Function
End Class

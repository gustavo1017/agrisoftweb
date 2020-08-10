Imports System.Configuration

Public Class ResumenCostosEnTodosLosCultivosHABL
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Function Refresh_Query(ByRef pFilter As String, ByVal monedaIndex As Integer, ByVal IdUsuario As String) As ADODB.Recordset
        Dim rsReporte As New ADODB.Recordset
        Dim strSQL As String
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)

        If monedaIndex = 0 Then
            strSQL = " SELECT x.orden ,p.* FROM ( SELECT r.tipo_costo, r.cultivosdesc, x.zonatrabajodesc, r.id_cultivo, x.totalmontostandar2,SUM(r.monto_standar) AS totalmontostandar, AVG(CASE WHEN r.id_cultivo = 'COSTIN' OR substring(r.id_cultivo, 1, 4) = 'INVE' THEN 0 ELSE r.hectareas END) AS MáxDehectareas,X.costohectarea FROM ResumenDeCostosPorCultivo r INNER JOIN ( SELECT zonatrabajodesc, SUM(CASE WHEN r.[hectareas] <> 0 THEN [monto_standar] / r.[hectareas] ELSE 0 END) AS costohectarea , SUM(monto_standar) AS totalmontostandar2 From ResumenDeCostosPorCultivo as r INNER JOIN ZONA_TRABAJO ON r.id_zonatrabajo = ZONA_TRABAJO.id_zonatrabajo INNER JOIN USUARIOCCOSTOS ON ZONA_TRABAJO.id_fundo = USUARIOCCOSTOS.id_fundo "
            strSQL = strSQL & pFilter & " AND (tipo_costo IN ('I', 'M', 'R', 'H', 'O')) and USUARIOCCOSTOS.Id_usuario = '" & IdUsuario & "' GROUP BY zonatrabajodesc )x on x.zonatrabajodesc=r.zonatrabajodesc  "
            strSQL = strSQL & pFilter
            strSQL = strSQL & " AND (tipo_costo IN ('I', 'M', 'R', 'H', 'O'))  GROUP BY r.hectareas, r.tipo_costo, r.orden, r.cultivosdesc, x.zonatrabajodesc, r.id_cultivo,x.costohectarea,x.totalmontostandar2 ) t   PIVOT (SUM(totalmontostandar) FOR [TIPO_COSTO] IN ([I],[M],[H],[R],[O])) as p  LEFT JOIN CULTIVOS x ON x.id_cultivo=p.id_cultivo "
        Else
            strSQL = " SELECT x.orden ,p.* FROM ( SELECT r.tipo_costo, r.cultivosdesc, x.zonatrabajodesc, r.id_cultivo, x.totalmontostandar2,SUM(r.MS) AS totalmontostandar, AVG(CASE WHEN r.id_cultivo = 'COSTIN' OR substring(r.id_cultivo, 1, 4) = 'INVE' THEN 0 ELSE r.hectareas END) AS MáxDehectareas,X.costohectarea FROM ResumenDeCostosPorCultivoME r INNER JOIN ( SELECT zonatrabajodesc, SUM(CASE WHEN r.[hectareas] <> 0 THEN [MS] / r.[hectareas] ELSE 0 END) AS costohectarea , SUM(MS) AS totalmontostandar2 From ResumenDeCostosPorCultivoME as r INNER JOIN ZONA_TRABAJO ON r.id_zonatrabajo = ZONA_TRABAJO.id_zonatrabajo INNER JOIN USUARIOCCOSTOS ON ZONA_TRABAJO.id_fundo = USUARIOCCOSTOS.id_fundo "
            strSQL = strSQL & pFilter & " AND (tipo_costo IN ('I', 'M', 'R', 'H', 'O')) and USUARIOCCOSTOS.Id_usuario = '" & IdUsuario & "' GROUP BY zonatrabajodesc )x on x.zonatrabajodesc=r.zonatrabajodesc  "
            strSQL = strSQL & pFilter
            strSQL = strSQL & " AND (tipo_costo IN ('I', 'M', 'R', 'H', 'O'))  GROUP BY r.hectareas, r.tipo_costo, r.orden, r.cultivosdesc, x.zonatrabajodesc, r.id_cultivo,x.costohectarea,x.totalmontostandar2 ) t   PIVOT (SUM(totalmontostandar) FOR [TIPO_COSTO] IN ([I],[M],[H],[R],[O])) as p  LEFT JOIN CULTIVOS x ON x.id_cultivo=p.id_cultivo "

        End If
        rsReporte = New ADODB.Recordset
        rsReporte.Open(strSQL, DBconn, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)
        Return rsReporte
    End Function
End Class

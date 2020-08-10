Imports System.Data.OleDb

Public Class ZonaTrabajoView
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Sub New()

    End Sub

    Public Function cargarDatosGrilla(ByVal sField As String, ByVal sText As String) As DataTable
        Dim RS As New ADODB.Recordset
        'Dim ssql As String = "SELECT z.id_zonatrabajo, z.descripcion as zt , z.hectareas, c.descripcion AS cultivo, z.campana, z.Parametrodelsistema, FUNDOS.descripcion AS fundo, z.id_ccc, PLANCONTABLE_4.Descripcion AS Cuenta9, PLANCONTABLE.Descripcion AS CuentaActivo, PLANCONTABLE_2.Descripcion AS Cuenta7, PLANCONTABLE_1.Descripcion AS CuentaProductoTerminado, PLANCONTABLE_3.Descripcion AS CuentaCostoVentas, " & " SAP_ZONATRABAJO.id_zonatrabajoSap,z.fechasiembra,z.estado FROM ZONA_TRABAJO AS z INNER JOIN CCC ON z.id_ccc = CCC.id_ccc INNER JOIN PLANCONTABLE AS PLANCONTABLE_2 ON CCC.id_cuentacontable = PLANCONTABLE_2.Id_cuentacontable INNER JOIN PLANCONTABLE ON CCC.id_cuentacontable2 = PLANCONTABLE.Id_cuentacontable INNER JOIN PLANCONTABLE AS PLANCONTABLE_4 ON CCC.id_cuentacontable3 = PLANCONTABLE_4.Id_cuentacontable INNER JOIN  " & " PLANCONTABLE AS PLANCONTABLE_1 ON CCC.id_cuentacontable4 = PLANCONTABLE_1.Id_cuentacontable INNER JOIN PLANCONTABLE AS PLANCONTABLE_3 ON CCC.id_cuentacontable5 = PLANCONTABLE_3.Id_cuentacontable LEFT OUTER JOIN FUNDOS ON z.id_fundo = FUNDOS.Id_fundo LEFT OUTER JOIN CULTIVOS AS c ON z.id_cultivo = c.id_cultivo LEFT OUTER JOIN SAP_ZONATRABAJO ON z.id_zonatrabajo = SAP_ZONATRABAJO.id_zonatrabajo WHERE (z.id_cultivo = c.id_cultivo) AND (z.tipo = 'C') "
        Dim ssql As String = "SELECT z.id_zonatrabajo, z.descripcion as zt , z.hectareas, c.descripcion AS cultivo, z.campana, z.Parametrodelsistema, FUNDOS.descripcion AS fundo, z.id_ccc, PLANCONTABLE_4.Descripcion AS Cuenta9, PLANCONTABLE.Descripcion AS CuentaActivo, PLANCONTABLE_2.Descripcion AS Cuenta7, PLANCONTABLE_1.Descripcion AS CuentaProductoTerminado, PLANCONTABLE_3.Descripcion AS CuentaCostoVentas, " & " SAP_ZONATRABAJO.id_zonatrabajoSap,z.fechasiembra,z.estado FROM ZONA_TRABAJO AS z INNER JOIN CCC ON z.id_ccc = CCC.id_ccc INNER JOIN PLANCONTABLE AS PLANCONTABLE_2 ON CCC.id_cuentacontable = PLANCONTABLE_2.Id_cuentacontable INNER JOIN PLANCONTABLE ON CCC.id_cuentacontable2 = PLANCONTABLE.Id_cuentacontable INNER JOIN PLANCONTABLE AS PLANCONTABLE_4 ON CCC.id_cuentacontable3 = PLANCONTABLE_4.Id_cuentacontable INNER JOIN  " & " PLANCONTABLE AS PLANCONTABLE_1 ON CCC.id_cuentacontable4 = PLANCONTABLE_1.Id_cuentacontable INNER JOIN PLANCONTABLE AS PLANCONTABLE_3 ON CCC.id_cuentacontable5 = PLANCONTABLE_3.Id_cuentacontable LEFT OUTER JOIN FUNDOS ON z.id_fundo = FUNDOS.Id_fundo LEFT OUTER JOIN CULTIVOS AS c ON z.id_cultivo = c.id_cultivo LEFT OUTER JOIN SAP_ZONATRABAJO ON z.id_zonatrabajo = SAP_ZONATRABAJO.id_zonatrabajo WHERE (z.id_cultivo = c.id_cultivo) AND (z.tipo = 'C') "

        If sField <> "" Then
            ssql = ssql & " and " & sField & " like '%" & sText & "%';"
        Else
            ssql = ssql & ";"
        End If

        Dim dtData As New DataTable()
        dtData = cargarDatosGrilla(ssql)

        Return dtData
    End Function

    Public Function cargarDatosGrilla(ByVal sQuery As String) As DataTable
        Dim RS As New ADODB.Recordset
        Dim ssql As String = sQuery

        If RS.State = 1 Then
            RS.Close()
        End If

        RS.let_ActiveConnection(DBConn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        RS.LockType = ADODB.LockTypeEnum.adLockReadOnly
        RS.let_Source(ssql)
        RS.Open()

        Dim sqlAdapter As New OleDbDataAdapter()
        Dim dtData As New DataTable()
        sqlAdapter.Fill(dtData, RS)

        If RS.State = 1 Then
            RS.Close()
        End If

        Return dtData
    End Function

    Public Function GetSQLQuery(ByVal sField As String, ByVal sText As String) As String
        Dim ssql As String = "SELECT z.id_zonatrabajo, z.descripcion AS zt, z.hectareas, c.descripcion AS Cultivo, z.campana, z.Parametrodelsistema, FUNDOS.descripcion AS Fundo, z.id_ccc, PLANCONTABLE_4.Descripcion AS Cuenta9, PLANCONTABLE.Descripcion AS CuentaActivo, PLANCONTABLE_2.Descripcion AS Cuenta7, PLANCONTABLE_1.Descripcion AS CuentaProductoTerminado, PLANCONTABLE_3.Descripcion AS CuentaCostoVentas, " & " SAP_ZONATRABAJO.id_zonatrabajoSap,z.fechasiembra,z.estado FROM ZONA_TRABAJO AS z INNER JOIN CCC ON z.id_ccc = CCC.id_ccc INNER JOIN PLANCONTABLE AS PLANCONTABLE_2 ON CCC.id_cuentacontable = PLANCONTABLE_2.Id_cuentacontable INNER JOIN PLANCONTABLE ON CCC.id_cuentacontable2 = PLANCONTABLE.Id_cuentacontable INNER JOIN PLANCONTABLE AS PLANCONTABLE_4 ON CCC.id_cuentacontable3 = PLANCONTABLE_4.Id_cuentacontable INNER JOIN  " & " PLANCONTABLE AS PLANCONTABLE_1 ON CCC.id_cuentacontable4 = PLANCONTABLE_1.Id_cuentacontable INNER JOIN PLANCONTABLE AS PLANCONTABLE_3 ON CCC.id_cuentacontable5 = PLANCONTABLE_3.Id_cuentacontable LEFT OUTER JOIN FUNDOS ON z.id_fundo = FUNDOS.Id_fundo LEFT OUTER JOIN CULTIVOS AS c ON z.id_cultivo = c.id_cultivo LEFT OUTER JOIN SAP_ZONATRABAJO ON z.id_zonatrabajo = SAP_ZONATRABAJO.id_zonatrabajo WHERE (z.id_cultivo = c.id_cultivo) AND (z.tipo = 'C') "

        If sField <> "" Then
            ssql = ssql & " and " & sField & " like '%" & sText & "%';"
        Else
            ssql = ssql & ";"
        End If

        Return ssql
    End Function

    Public Function Delete(ByVal strIdZonaTrabajo As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False

        Try
            ssql = "delete from ZONA_TRABAJO where id_zonatrabajo='" & strIdZonaTrabajo & "';"
            DBConn.Execute(ssql)

            ssql = "DELETE  From SAP_ZONATRABAJO WHERE (((SAP_ZONATRABAJO.id_zonatrabajo)='" & strIdZonaTrabajo & "'));"
            DBConn.Execute(ssql)

            boolResult = True
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBConn.ConnectionString)

            RegisterEvent(dctException)
            boolResult = False
        End Try

        Return boolResult
    End Function

    Public Function VerifyZonaTrabajoDistribucion(ByVal strIdZonaTrabajo As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False
        Dim rsDistrib As ADODB.Recordset

        ssql = "SELECT DISTRIBUCIONINDIRECTOS.Id_zonatrabajoi, DISTRIBUCIONINDIRECTOS.Id_zonatrabajod " & "From DISTRIBUCIONINDIRECTOS " & "WHERE (((DISTRIBUCIONINDIRECTOS.Id_zonatrabajoi)='" & strIdZonaTrabajo & "')) " & "OR (((DISTRIBUCIONINDIRECTOS.Id_zonatrabajod)='" & strIdZonaTrabajo & "'));"
        rsDistrib = DBConn.Execute(ssql)
        If Not rsDistrib.EOF Then
            boolResult = True
        Else
            boolResult = False
        End If

        Return boolResult
    End Function

    Public Function VerifyZonaTrabajoVentas(ByVal strIdZonaTrabajo As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False
        Dim rsDistrib As ADODB.Recordset

        ssql = "SELECT COSTOS.id_zonatrabajo, COSTOS.tipo_costo " & "From COSTOS " & "WHERE (((COSTOS.id_zonatrabajo)='" & strIdZonaTrabajo & "') " & "AND ((COSTOS.tipo_costo)='v'));"
        rsDistrib = DBConn.Execute(ssql)
        If Not rsDistrib.EOF Then
            boolResult = True
        Else
            boolResult = False
        End If

        Return boolResult
    End Function
End Class

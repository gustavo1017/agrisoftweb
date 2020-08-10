Public Class ZonaTrabajo
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Function getRecord(ByVal Codigo As String) As ADODB.Recordset
        Dim rs As New ADODB.Recordset
        Dim ssql As String = "SELECT z.estado,z.fechasiembra AS FECS ,z.id_zonatrabajo, z.descripcion, z.hectareas, z.campana, c.id_cultivo, c.descripcion AS des_cultivo, z.id_fundo, FUNDOS.descripcion as FUNDO_des, CCC.id_ccc, CCC.descripcion as CCC_des, PLANCONTABLE.Descripcion AS CTA9 " & " FROM ((ZONA_TRABAJO AS z INNER JOIN CULTIVOS AS c ON z.id_cultivo = c.id_cultivo) INNER JOIN FUNDOS ON z.id_fundo = FUNDOS.Id_fundo) INNER JOIN CCC ON z.id_ccc = CCC.id_ccc INNER JOIN PLANCONTABLE ON CCC.id_cuentacontable3 = PLANCONTABLE.Id_cuentacontable " & " WHERE (((z.id_cultivo)=[c].[id_cultivo]) AND ((z.id_zonatrabajo)='" & Codigo & "'));"

        rs.let_ActiveConnection(DBConn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        Return rs
    End Function

    Public Function getZonaTrabajoByDescripcion(ByVal Codigo As String, ByVal strDescripcion As String) As ADODB.Recordset
        Dim rs As New ADODB.Recordset
        'Dim ssql As String = " SELECT ZONA_TRABAJO.descripcion From ZONA_TRABAJO WHERE (((ZONA_TRABAJO.descripcion)=ltrim(rtrim('" & strDescripcion & "')))); "
        Dim ssql As String = String.Format("SELECT ZONA_TRABAJO.descripcion From dbo.ZONA_TRABAJO (NOLOCK) WHERE ZONA_TRABAJO.descripcion = '{0}' AND id_zonatrabajo <> '{1}';", strDescripcion, Codigo)

        rs.let_ActiveConnection(DBConn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        Return rs
    End Function

    Public Function getZonaTrabajoByCodigo(ByVal strCodigo As String) As ADODB.Recordset
        Dim rs As New ADODB.Recordset
        Dim ssql As String = "SELECT zona_trabajo.id_zonatrabajo From zona_trabajo " & " WHERE (zona_trabajo.id_zonatrabajo) =ltrim((rtrim('" & strCodigo & "')));"

        rs.let_ActiveConnection(DBConn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        Return rs
    End Function

    Public Function Edit(ByVal strIdZonaTrabajo As String, ByVal strIdCultivo As String, ByVal strfecsiembra As String, ByVal strDescripcion As String, ByVal strHectareas As String, ByVal strCampana As String, ByVal sEstado As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False
        Dim rs As New ADODB.Recordset

        Try
            Dim fec As String = Convert.ToDateTime(strfecsiembra).ToString("yyyy/MM/dd")



            ssql = ""

            If rs.State = 1 Then
                rs.Close()
            End If

            rs.let_ActiveConnection(DBConn)
            rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
            rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
            rs.let_Source(ssql)
            rs.Open()

            If rs.EOF Then
                ssql = "insert into SAP_ZONATRABAJO (id_zonatrabajo,id_ZONATRABAJOSAP) "
                ssql = ssql & "values('" & strIdZonaTrabajo & "','" & strIdZonaTrabajo & "');"

                DBConn.Execute(ssql)
            End If

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
End Class

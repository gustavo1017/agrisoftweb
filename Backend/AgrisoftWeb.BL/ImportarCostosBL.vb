Imports System.Configuration
Imports System.Globalization

Public Class ImportarCostosBL
    Inherits BaseBL
    Dim LineapERSONAL As Object
    Dim StrActividad As Object
    Dim LineaAgrisoft As Object
    Dim strMaquina As Object

    Dim arrCampo() As Object
    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Sub CargarTabla(ByVal lstContentRows As List(Of String), ByVal isPlan As Boolean)

        Dim dateFile As DateTime
        Dim fec1 As String = String.Empty
        Dim I, X As Short
        Dim strReg As String
        Dim enUS = New CultureInfo("en-US")


        'Dim DBconn As ADODB.Connection

        'DBconn = GenericMethods.SetupConnection(LoggedUser)

        X = 1
        Dim index As Integer = 0
        ReDim Preserve arrCampo(X)
        For Each item As String In lstContentRows
            X = 1

            If index = 0 Then
                index += 1
                Continue For
            End If

            strReg = Trim(item)

            If String.IsNullOrEmpty(strReg) Then
                Continue For
            End If

            For I = 1 To Len(strReg)
                If Mid(strReg, I, 1) = Microsoft.VisualBasic.vbTab Then
                    arrCampo(X) = Trim(arrCampo(X))
                    X = X + 1
                    ReDim Preserve arrCampo(X)
                Else
                    arrCampo(X) = arrCampo(X) & Mid(strReg, I, 1)
                End If
            Next I

            strMaquina = NvaMAQUINA(Mid(arrCampo(4), 1, 6), Mid(arrCampo(4), 1, 6))
            arrCampo(7) = NvaZonaTrabajoSAP(Mid(arrCampo(7), 1, 7), Mid(arrCampo(7), 1, 7))
            LineaAgrisoft = NvaLineaSAP(Mid(arrCampo(8), 1, 11), Mid(arrCampo(8), 1, 11))
            arrCampo(3) = NvoProductoSAP(Mid(arrCampo(3), 1, 6), Mid(arrCampo(3), 1, 6), Mid(arrCampo(3), 1, 6))
            StrActividad = NvoLineaActividad(Mid(arrCampo(6), 1, 6), Mid(arrCampo(6), 1, 6))
            LineapERSONAL = NvOPERSONAL(Mid(arrCampo(5), 1, 8), Mid(arrCampo(5), 1, 8))

            If DateTime.TryParseExact(arrCampo(2), "yyyy-MM-dd", enUS,
                                 DateTimeStyles.None, dateFile) Then
                fec1 = dateFile.ToString("yyyyMMdd")
            Else

            End If

            Dim Sql As String


            DBConn.Execute(Sql, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

            For I = 1 To UBound(arrCampo) - 1
                arrCampo(I) = System.DBNull.Value
            Next I

            index += 1
        Next

        'If isPlan Then


        'End If

    End Sub

    Private Function NvaZonaTrabajoSAP(strValor As String, strDesc As String) As String
        Dim rs

        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)
        Dim Sql As String = "Select id_zonatrabajo from ZONA_TRABAJO where id_zonatrabajo='" & strValor & "';"
        rs = DBConn.Execute(Sql)

        If rs.EOF Then
            Dim objGenericMethods As New GenericMethods(LoggedUser)
            NvaZonaTrabajoSAP = CStr(objGenericMethods.Contador("ZonaTrabajo"))
            NvaZonaTrabajoSAP = NvaZonaTrabajoSAP.PadLeft(7, "0")

            Sql = "INSERT INTO ZONA_TRABAJO (TIPO,id_zonatrabajo, id_cultivo, descripcion, hectareas, campana, Parametrodelsistema, id_zona, depreciacion,ID_FUNDO,id_ccc ) " & "Values('C','" & strValor & "', 'COSTIN', " & IIf(strDesc = "", "'" & NvaZonaTrabajoSAP & "'", "'" & strDesc & "'") & ", 1, 1, 0, '0000001', 1, '000000','91');"
            DBConn.Execute(Sql)

            Sql = "INSERT INTO SAP_ZONATRABAJO (id_zonatrabajo, id_zonatrabajoSap) " & "Values( '" & strValor & "','" & NvaZonaTrabajoSAP & "');"
            DBConn.Execute(Sql)


            NvaZonaTrabajoSAP = strValor
        Else
            NvaZonaTrabajoSAP = strValor


        End If
        Return NvaZonaTrabajoSAP

    End Function

    Private Function NvaLineaSAP(ByVal strValor As String, ByVal strDesc As String) As String
        Dim strResult As String
        Dim rs As ADODB.Recordset
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)
        Dim Sql As String = "Select id_PROVEEDOR, DESCRIPCION from PROVEEDORES where id_PROVEEDOR='" & strValor & "'"
        rs = DBConn.Execute(Sql)
        strResult = strValor
        If rs.EOF Then
            Dim objGenericMethods As New GenericMethods(LoggedUser)
            Dim NvoProveedor As String = Mid(CStr(objGenericMethods.Contador("ZonaTrabajo")), 1, 11)
            NvoProveedor = NvoProveedor.PadLeft(11, "0")
            Dim ssql As String = "insert into PROVEEDORES (id_PROVEEDOR, DESCRIPCION) " & "values('" & strValor & "', '" & strValor & "')"
            DBConn.Execute(ssql)

            strResult = NvoProveedor
        Else
        End If

        Return strResult
    End Function
    Private Function NvaMAQUINA(ByVal strValor As String, ByVal strDesc As String) As String
        Dim strResult As String
        Dim rs As ADODB.Recordset
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)
        Dim Sql As String = "Select id_MAQUINARIA, DESCRIPCION from MAQUINAS where id_MAQUINARIA='" & strValor & "'"
        rs = DBConn.Execute(Sql)
        strResult = strValor

        If rs.EOF Then
            Dim objGenericMethods As New GenericMethods(LoggedUser)
            strResult = Mid(CStr(objGenericMethods.Contador("ZonaTrabajo")), 1, 6)
            Dim ssql As String = "insert into MAQUINAS (id_MAQUINARIA, DESCRIPCION,TIPO) " & "values('" & strValor & "', '" & strValor & "','M')"
            DBConn.Execute(ssql)
            strResult = strValor

        Else
            strResult = strValor

        End If

        Return strResult
    End Function
    Private Function NvoProductoSAP(ByVal strValor As String, ByVal strDesc As String, ByVal StrUnidad As String) As String

        Dim rsZT As Object
        Dim rs As ADODB.Recordset
        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)
        Dim Sql As String = "Select id_producto from PRODUCTOS where id_producto='" & strValor & "';"
        rs = DBConn.Execute(Sql)


        If rs.EOF Then

            Sql = "INSERT INTO PRODUCTOS (id_producto, descripcion, costo, tipo, tc, id_linea, Parametrodelsistema,id_unidad) " & "Values('" & strValor & "', " & IIf(strDesc = "", "'" & NvoProductoSAP & "'", "'" & LTrim(RTrim(strDesc)) & " " & LTrim(RTrim(StrUnidad)) & "'") & ",  1  , 'I', 1, 'ABO', 1,'KGS');"
            DBConn.Execute(Sql)

            Dim objGenericMethods As New GenericMethods(LoggedUser)
            NvoProductoSAP = CStr(objGenericMethods.Contador("ZonaTrabajo"))
            NvoProductoSAP = NvoProductoSAP.PadLeft(6, "0")

            Sql = "insert into SAP_PRODUCTOS (id_producto, id_productoSap) " & "values('" & NvoProductoSAP & "', '" & strValor & "');"
            DBConn.Execute(Sql)

            NvoProductoSAP = strValor


        Else
            NvoProductoSAP = strValor

        End If
        Return NvoProductoSAP

    End Function

    Private Function NvoLineaActividad(ByVal strValor As String, ByVal strDesc As String) As String
        Dim rs As ADODB.Recordset
        Dim strResult As String

        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)
        Dim Sql As String = "Select id_actividad, DESCRIPCION from ACTIVIDADES where id_ACTIVIDAD='" & strValor & "'"
        rs = DBConn.Execute(Sql)
        strResult = strValor

        If rs.EOF Then
            Sql = "insert into actividades (DESCRIPCION,id_actividad,id_etapa) " & "values('" & strDesc & "', '" & strValor & "','MA0')"
            DBConn.Execute(Sql)

        End If

        Return strResult
    End Function

    Private Function NvOPERSONAL(ByVal strValor As String, ByVal strDesc As String) As String
        Dim rs As ADODB.Recordset
        Dim strResult As String

        'Dim DBconn As ADODB.Connection = GenericMethods.SetupConnection(LoggedUser)
        Dim Sql As String = "Select id_PERSONAL, NOMBRE from PERSONAL where id_PERSONAL='" & strValor & "'"
        rs = DBConn.Execute(Sql)
        strResult = strValor

        If rs.EOF Then
            Dim ssql As String = "insert into PERSONAL (id_PERSONAL, NOMBRE,id_categoria,id_ciclopago,costo,costo_conta,horaes,horaed,estado) values('" & strValor & "', '" & strValor & "','OBRE','SEMA',1,1,1,1,1)"
            DBConn.Execute(ssql)
        Else

        End If

        Return strResult

    End Function
End Class

Imports System.Data.OleDb

Public Class ProveedoresView
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Sub New()

    End Sub

    Public Function cargarDatosGrilla(ByVal sField As String, ByVal sText As String) As DataTable
        Dim RS As New ADODB.Recordset
        Dim ssql As String = "select * from proveedores where id_proveedor<>'000001'"

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
        Dim ssql As String = "select * from Proveedores"

        'If sField <> "" Then
        '    ssql = ssql & " and " & sField & " like '%" & sText & "%';"
        'Else
        '    ssql = ssql & ";"
        'End If

        Return ssql
    End Function

    Public Function Delete(ByVal strIdCultivo As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False

        Try
            'Se comentó en coordinación con Juan, la tabla ya no es vigente
            'ssql = "delete from SYSCentros where id_cultivo='" & strIdCultivo & "';"
            'DBConn.Execute(ssql)

            ssql = "delete from proveedores where id_proveedor='" & strIdCultivo & "';"
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

End Class

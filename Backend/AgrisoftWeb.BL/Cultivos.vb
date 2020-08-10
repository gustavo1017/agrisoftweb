Public Class Cultivos
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Function getRecord(ByVal Codigo As String) As ADODB.Recordset
        Dim rs As New ADODB.Recordset
        Dim ssql As String = "select * from CULTIVOS where id_cultivo='" & Codigo & "';"

        rs.let_ActiveConnection(DBConn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        Return rs
    End Function

    Public Function getCultivosByDescripcion(ByVal Codigo As String, ByVal strDescripcion As String) As ADODB.Recordset
        Dim rs As New ADODB.Recordset
        Dim ssql As String = String.Format("SELECT DESCRIPCION From dbo.cultivos (NOLOCK) WHERE DESCRIPCION = '{0}' AND id_cultivo <> '{1}'", strDescripcion, Codigo)

        rs.let_ActiveConnection(DBConn)
        rs.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rs.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rs.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rs.let_Source(ssql)
        rs.Open()

        Return rs
    End Function

    Public Function Add_Renamed(ByVal strId As String, ByVal strDescripcion As String, ByVal strComponente As String, ByVal strModulo As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False

        Try


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

    Public Function Edit(ByVal strId As String, ByVal strDescripcion As String, ByVal strComponente As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False



        Return boolResult
    End Function

End Class

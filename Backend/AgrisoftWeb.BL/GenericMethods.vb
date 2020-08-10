Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web

Public Class GenericMethods
    Inherits BaseBL

    Public Sub New(strUsuario As String)
        MyBase.New(strUsuario)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function CheckAutorizacion(ByVal strIdModulo As String, ByVal strIdUsuario As String) As ADODB.Recordset
        Dim strSQL As String
        Dim RS As ADODB.Recordset
        'Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)

        strSQL = "SELECT bajo,medio,total FROM AUTORIZACION_USUARIOS WHERE id_modulo = '" & strIdModulo & "' and id_usuario ='" & strIdUsuario & "';"
        RS = New ADODB.Recordset
        If RS.State = 1 Then
            RS.Close()
        End If

        RS.let_ActiveConnection(DBConn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(strSQL)
        RS.Open()

        Return RS
    End Function

    Public Function GetEmpresaParametroValor(ByVal intIdEmpresa As Integer, ByVal strParametroID As String) As String
        Dim RS As New ADODB.Recordset
        Dim DBconn As ADODB.Connection = SetupAccessConnection()
        Dim strParametroValor As String = ""
        Dim strSQL As String
        strSQL = String.Format("SELECT ParametroValor FROM dbo.EmpresaParametro WHERE Id_empresa = {0} AND ParametroID = '{1}'", intIdEmpresa.ToString(), strParametroID.ToString())

        If RS.State = 1 Then
            RS.Close()
        End If

        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(strSQL)
        RS.Open()

        If RS.RecordCount > 0 Then
            If Not RS.EOF Then
                'Return RS
                strParametroValor = RS.Fields(0).Value.ToString()
            End If
        Else
            strParametroValor = ""
        End If

        Return strParametroValor
    End Function

    Public Function fnDevIdModulo(ByRef pDescripcion As String) As String
        Dim RS As New ADODB.Recordset
        ' Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)
        Dim ssql As String

        ssql = "SELECT ID_MODULO FROM AUTORIZACION WHERE DESCRIPCION = '" & pDescripcion & "';"
        RS = New ADODB.Recordset
        If RS.State = 1 Then
            RS.Close()
        End If
        RS.let_ActiveConnection(DBConn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(ssql)
        RS.Open()

        fnDevIdModulo = "*"
        If Not RS.EOF Then
            fnDevIdModulo = RS.Fields("ID_MODULO").Value
        End If
    End Function

    Public Function getFormulariosEspeciales() As ADODB.Recordset
        Dim strSQL As String
        Dim rsFormEspe As New ADODB.Recordset
        'Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)

        strSQL = " SELECT Menu, id_formulario, descripcion, estado, indice, indexid, id, id_modulo From FORMULARIOSESPECIALES where estado<>0 order by id"

        If rsFormEspe.State = 1 Then
            rsFormEspe.Close()
        End If

        rsFormEspe.let_ActiveConnection(DBConn)
        rsFormEspe.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rsFormEspe.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rsFormEspe.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rsFormEspe.let_Source(strSQL)
        rsFormEspe.Open()

        Return rsFormEspe
    End Function

    Public Function getModuloByUsuario(ByVal pUsuario As String) As ADODB.Recordset
        Try
            Dim strSQL As String
            Dim RS As New ADODB.Recordset

            strSQL = "SELECT * " & "FROM AUTORIZACION_USUARIOS WHERE id_usuario = '" & pUsuario & "' " & " AND Total <>0;"
            'Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)

            If RS.State = 1 Then
                RS.Close()
            End If

            RS.let_ActiveConnection(DBConn)
            RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
            RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
            RS.let_Source(strSQL)
            RS.Open()

            Return RS
        Catch ex As Exception
        End Try


    End Function

    Public Function getFormulariosEspecialesByModulo(ByVal idModulo As String) As ADODB.Recordset
        Dim rsFormEspe As New ADODB.Recordset
        Dim strSQL As String = "SELECT * " & "FROM FORMULARIOSESPECIALES WHERE estado <> 0 and id_modulo=ltrim(rtrim('" & idModulo & " ')) ; "
        'Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)

        If rsFormEspe.State = 1 Then
            rsFormEspe.Close()
        End If

        rsFormEspe.let_ActiveConnection(DBConn)
        rsFormEspe.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rsFormEspe.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
        rsFormEspe.LockType = ADODB.LockTypeEnum.adLockReadOnly
        rsFormEspe.let_Source(strSQL)
        rsFormEspe.Open()

        Return rsFormEspe
    End Function

    Public Function getFormulariosEspecialesDeshabilitadoPorUsuario(ByVal pUsuario As String) As ADODB.Recordset
        Dim strSQL As String
        Dim RS As New ADODB.Recordset

        strSQL = String.Format("SELECT id_formulario FROM dbo.AUTORIZACION_USUARIOS au INNER JOIN dbo.FORMULARIOSESPECIALES fe on au.id_modulo = fe.id_modulo and fe.estado <> 0 WHERE au.Total = 0 AND au.id_usuario = '{0}' ", pUsuario)
        'Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)

        If RS.State = 1 Then
            RS.Close()
        End If

        RS.let_ActiveConnection(DBConn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(strSQL)
        RS.Open()

        Return RS
    End Function

    Public Function ControlPermisos() As ADODB.Recordset
        Dim RS As New ADODB.Recordset
        Dim ssql As String
        'Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)

        ssql = " SELECT Permisos.menu, Permisos.id_formulario, Permisos.Autorizado, Permisos.significado, Permisos.indice, * From Permisos WHERE autorizado=0 ;"
        RS = New ADODB.Recordset
        If RS.State = 1 Then
            RS.Close()
        End If

        RS.Open(ssql, DBConn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockPessimistic)
        Return RS
    End Function

    Public Function Contador(ByRef strCampo As String, Optional ByRef ano As String = "") As Short
        Dim rsContador As ADODB.Recordset
        Dim intContador As Integer
        Dim ssql As String
        'Dim DBconn As ADODB.Connection = SetupConnection(LoggedUser)

        If Len(ano) = 0 Then

            ssql = "select * from CONTADOR where campo = '" & strCampo & "'"
            rsContador = DBConn.Execute(ssql)

            If Not rsContador.EOF Then
                intContador = rsContador.Fields(1).Value + 1
                ssql = "update CONTADOR set valor=" & intContador & " where campo = '" & strCampo & "'"
                DBConn.Execute(ssql)
                Contador = intContador
            Else
                Contador = 0
            End If

        Else
            ssql = "select * from CONTADOR where campo = '" & strCampo & "' And ano = '" & ano & " '"

            rsContador = DBConn.Execute(ssql)

            If Not rsContador.EOF Then
                ssql = "update CONTADOR set valor=" & intContador & " where campo = '" & strCampo & "'"
                DBConn.Execute(ssql)
                intContador = rsContador.Fields(1).Value + 1
                Contador = intContador
            Else
                ssql = "update CONTADOR set ano=" & ano & " where campo = '" & strCampo & "'"
                DBConn.Execute(ssql)
                intContador = 1
                Contador = intContador
            End If

        End If
    End Function

    Public Function UserExists(ByVal strUsuario As String) As Boolean
        Dim strSQL As String
        Dim RS As New ADODB.Recordset

        strSQL = "SELECT * FROM Usuario WHERE Login = '" & strUsuario & "'"
        DBConn = SetupAccessConnection()

        If RS.State = 1 Then
            RS.Close()
        End If

        RS.let_ActiveConnection(DBConn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(strSQL)
        RS.Open()

        Dim result As Boolean = True

        If RS.RecordCount = 0 Then Return False

        Return result
    End Function

    Public Function EmailExists(ByVal emailAddress As String) As Boolean
        Dim strSQL As String
        Dim RS As New ADODB.Recordset

        strSQL = "SELECT * FROM Usuario WHERE Email = '" & emailAddress & "'"
        DBConn = SetupAccessConnection()

        If RS.State = 1 Then
            RS.Close()
        End If

        RS.let_ActiveConnection(DBConn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(strSQL)
        RS.Open()

        Dim result As Boolean = True

        If RS.RecordCount = 0 Then Return False

        Return result
    End Function

    Public Function CreateUser(ByVal strLogin As String, ByVal strPassword As String, ByVal intIdEmpresa As Integer, ByVal strLoginFrom As String, ByRef strResultado As String, ByVal strFirstName As String, ByVal strLastName As String, ByVal emailAddress As String) As Integer
        Dim strSQL As String
        Dim RS As New ADODB.Recordset

        'Verify if User exists
        If UserExists(strLogin) Then
            strResultado = "CreateUser.NameAlreadyExists"
            Return -1
        End If

        DBConn = SetupAccessConnection()

        'Create User on AgriWebAccess DB
        strSQL = String.Format("INSERT dbo.Usuario (Login, Password, Activo, Id_Empresa, Estado, LoginFrom, FirstName, LastName, Email) values ('{0}', '{1}', 1, {2}, 0, '{3}', '{4}', '{5}', '{6}');", strLogin, strPassword, intIdEmpresa, strLoginFrom, strFirstName, strLastName, emailAddress)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
        RS = DBConn.Execute("SELECT SCOPE_IDENTITY() As NewIdUsuario")

        Dim id_usuario As Integer = RS("NewIdUsuario").Value
        Return id_usuario
    End Function

    Public Sub UpdateUserPassword(ByVal idUsuario As Integer, ByVal newPassword As String)
        Dim strSQL As String = "UPDATE Usuario SET Password = '" & newPassword & "' WHERE Id_Usuario = " & idUsuario.ToString()
        DBConn = SetupAccessConnection()
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
    End Sub

    Public Sub CreateUserBusinessDB(ByVal strUserName As String)
        Dim strSQL As String = ""

        Try
            Dim RS As New ADODB.Recordset

            'Create User on AgriWebAccess DB
            strSQL = String.Format("INSERT dbo.Usuarios (id_usuario, Descripcion, ParametroDelSistema, Estado, Pass) values ('{0}', '{1}', 0, 1, '{2}');", strUserName, strUserName, strUserName)
            RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
        Catch ex As Exception
            Dim sfullMessage As New StringBuilder()
            sfullMessage.AppendLine("Exception Message:")
            sfullMessage.AppendLine(ex.Message)
            sfullMessage.AppendLine()
            sfullMessage.AppendLine("Exception Stack Trace:")
            sfullMessage.AppendLine(ex.StackTrace)
            sfullMessage.AppendLine()

            If ex.InnerException IsNot Nothing Then
                sfullMessage.AppendLine("Inner Exception: ")
                sfullMessage.AppendLine(ex.InnerException.Message)
            End If

            sfullMessage.AppendLine("Additional Data: ")
            sfullMessage.AppendLine(strSQL)
            sfullMessage.AppendLine()
            'sfullMessage.AppendLine(DBConn.ConnectionString)

            RegisterEvent(sfullMessage.ToString())
            Throw
        End Try
    End Sub



    Public Sub UpdateUserAgrisoftAccess(ByVal strUserName As String, ByVal intUSerID As Integer)
        Dim strSQL As String
        DBConn = SetupAccessConnection()

        'Update User on AgrisoftWeb with BusinessDB User
        strSQL = String.Format("UPDATE dbo.Usuario SET Id_UsuarioBusiness = '{0}' WHERE Id_Usuario = {1}", strUserName, intUSerID)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
    End Sub

    Public Function CreateEmpresa(ByVal strDescripcion As String, ByVal strCodigo As String, ByRef strResult As String) As Integer
        Dim strSQL As String
        Dim RS As New ADODB.Recordset
        Dim RSEmpresaExiste As New ADODB.Recordset
        Dim RSEmpresas As New ADODB.Recordset

        ''Verify if this company has already registered
        'strSQL = String.Format("SELECT Id_Empresa FROM dbo.Empresa WHERE Descripcion = '{0}'", strDescripcion)
        'DBConn = SetupAccessConnection()

        'If RSEmpresaExiste.State = 1 Then
        '    RSEmpresaExiste.Close()
        'End If

        'RSEmpresaExiste.let_ActiveConnection(DBConn)
        'RSEmpresaExiste.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        'RSEmpresaExiste.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        'RSEmpresaExiste.LockType = ADODB.LockTypeEnum.adLockOptimistic
        'RSEmpresaExiste.let_Source(strSQL)
        'RSEmpresaExiste.Open()

        'If RSEmpresaExiste.RecordCount > 0 Then
        '    'RS.MoveFirst()
        '    strResult = "CreateEmpresa.NameAlreadyExists"
        '    'Return RS.Fields.Item("Id_Empresa").Value

        '    Return -1
        'End If

        strSQL = "SELECT Codigo from dbo.Empresa"
        ''DBConn = SetupAccessConnection()

        If RSEmpresas.State = 1 Then
            RSEmpresas.Close()
        End If

        RSEmpresas.let_ActiveConnection(DBConn)
        RSEmpresas.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RSEmpresas.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RSEmpresas.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RSEmpresas.let_Source(strSQL)
        RSEmpresas.Open()

        Dim lstCodigos As New List(Of String)
        If Not RSEmpresas.BOF Then
            RSEmpresas.MoveFirst()
            While Not RSEmpresas.EOF
                lstCodigos.Add(RSEmpresas.Fields("Codigo").Value.ToString())
                RSEmpresas.MoveNext()
            End While
        End If

        If String.IsNullOrEmpty(strCodigo) Then
            Dim newCodigo As String = GenerateRandomString(3)
            While lstCodigos.Contains(newCodigo)
                newCodigo = GenerateRandomString(3)
            End While

            strCodigo = newCodigo
        Else
            If lstCodigos.Contains(strCodigo) Then
                strResult = "CreateEmpresa.CodeAlreadyExists"
                Return -1
            End If
        End If

        strSQL = String.Format("INSERT dbo.Empresa (Descripcion, Codigo) values ('{0}', '{1}');", strDescripcion, strCodigo)
        DBConn.Execute(strSQL)
        RS = DBConn.Execute("SELECT SCOPE_IDENTITY() As NewIdEmpresa")

        Dim id_empresa As Integer = RS("NewIdEmpresa").Value
        strResult = "CreateEmpresa.Success"
        Return id_empresa
    End Function

    Public Sub CreateEmpresaParametrosDefault(ByVal intId_Empresa As Integer, ByVal strDBName As String)
        Dim strSQL As String
        Dim RS As New ADODB.Recordset
        DBConn = SetupAccessConnection()

        Dim strDatasource As String = ConfigurationManager.AppSettings("Default_DataSource")
        Dim strDBUser As String = ConfigurationManager.AppSettings("Default_DBUser")
        Dim strPassword As String = ConfigurationManager.AppSettings("Default_Password")
        Dim strCantUsers As String = ConfigurationManager.AppSettings("Default_CantUsuarios")
        Dim strCantPersonal As String = ConfigurationManager.AppSettings("Default_CantPersonal")
        Dim strCantRegistros As String = ConfigurationManager.AppSettings("Default_CantRegistros")
        Dim strProductsEnabled As String = ConfigurationManager.AppSettings("Default_ProductsEnabled")

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "BDServidor", strDatasource, "BD")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "BDName", strDBName, "BD")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "BDUser", strDBUser, "BD")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "BDPassword", strPassword, "BD")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "CantidadUsuarios", strCantUsers, "")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "CantidadPersonal", strCantPersonal, "")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "CantidadRegistros", strCantRegistros, "")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT EmpresaParametro(Id_empresa, ParametroID, ParametroValor, Module) values({0}, '{1}', '{2}', '{3}')", intId_Empresa.ToString, "ProductsEnabled", strProductsEnabled, "")
        RS = DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
    End Sub

    Public Function GenerateRandomString(ByVal codeLength As String) As String
        Dim sResult As String = ""
        Dim rdm As New Random()

        'For i As Integer = 1 To codeLength
        '    sResult &= ChrW(rdm.Next(32, 126))
        'Next

        Dim charsAllowed As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        sResult = Enumerable.Repeat(charsAllowed, codeLength).Select(Function(s) s(rdm.Next(s.Length))).ToArray()

        Return sResult
    End Function

    Public Function GenerateDB(ByVal strDatabaseName As String, ByVal strFilePath As String) As Boolean
    End Function

    Public Function VerifyDatabaseName(ByVal database As String) As Boolean

    End Function



    Public Sub RegisterUserModules(ByVal strBusinessUser As String)
        Dim strSQL As String

        strSQL = String.Format("INSERT AUTORIZACION_USUARIOS(id_usuario, id_modulo, bajo, Medio, Total, Parametrodelsistema) values('{0}', '{1}', {2}, {3}, {4}, {5})", strBusinessUser, "AGRREP", 0, 0, 1, 0)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT AUTORIZACION_USUARIOS(id_usuario, id_modulo, bajo, Medio, Total, Parametrodelsistema) values('{0}', '{1}', {2}, {3}, {4}, {5})", strBusinessUser, "AGRCOS", 0, 0, 1, 0)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT AUTORIZACION_USUARIOS(id_usuario, id_modulo, bajo, Medio, Total, Parametrodelsistema) values('{0}', '{1}', {2}, {3}, {4}, {5})", strBusinessUser, "HERIMP", 0, 0, 1, 0)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

    End Sub

    Public Sub RegisterUsuarioCategorias(ByVal strBusinessUser As String)
        Dim strSQL As String

        strSQL = String.Format("INSERT [dbo].[USUARIOCATEGORIAS] ([id_usuario], [id_categoria]) VALUES (N'{0}', N'EMPL')", strBusinessUser)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT [dbo].[USUARIOCATEGORIAS] ([id_usuario], [id_categoria]) VALUES (N'{0}', N'OBRE')", strBusinessUser)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT [dbo].[USUARIOCATEGORIAS] ([id_usuario], [id_categoria]) VALUES (N'{0}', N'PRUE')", strBusinessUser)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        strSQL = String.Format("INSERT [dbo].[USUARIOCATEGORIAS] ([id_usuario], [id_categoria]) VALUES (N'{0}', N'RECI')", strBusinessUser)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
    End Sub

    Public Sub RegisterUsuarioCCostos(ByVal strBusinessUser As String)
        Dim strSQL As String

        strSQL = String.Format("INSERT [dbo].[USUARIOCCOSTOS] ([Id_usuario], [id_fundo]) VALUES (N'{0}', N'000000')", strBusinessUser)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
    End Sub

    Public Sub RegisterUsuarioGrupos(ByVal strBusinessUser As String)
        Dim strSQL As String

        strSQL = String.Format("INSERT [dbo].[USUARIOGRUPOS] ([Id_usuario], [id_usuariogrupo]) VALUES (N'{0}', N'DEMO01')", strBusinessUser)
        DBConn.Execute(strSQL, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)
    End Sub

    Public Function ValidaPersonal() As Boolean
        Dim RS As New ADODB.Recordset
        Dim boolResult As Boolean = True
        Dim strSQL As String = "SELECT Count(PERSONAL.id_personal) AS Cuenta FROM PERSONAL;"
        Dim currentPersonal As Long = 0

        Try
            RS = New ADODB.Recordset
            If RS.State = 1 Then
                RS.Close()
            End If

            RS.let_ActiveConnection(DBConn)
            RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
            RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
            RS.let_Source(strSQL)
            RS.Open()

            currentPersonal = Convert.ToInt64(RS.Fields("CUENTA").Value)

            'Get the allowed users for this company
            strSQL = "SELECT Id_Empresa FROM USUARIO WHERE Login ='" & LoggedUser & "'"
            DBConn = SetupAccessConnection()
            RS = New ADODB.Recordset

            If RS.State = 1 Then
                RS.Close()
            End If

            RS.let_ActiveConnection(DBConn)
            RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
            RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
            RS.let_Source(strSQL)
            RS.Open()

            Dim idEmpresa As Integer = Convert.ToInt32(RS.Fields("Id_empresa").Value)
            Dim strPersonalTotal As String = GetEmpresaParametroValor(idEmpresa, "CantidadPersonal")
            Dim totalPersonal As Long = 0

            If Not String.IsNullOrEmpty(strPersonalTotal) Then
                totalPersonal = Int64.Parse(strPersonalTotal)
            End If

            If currentPersonal > totalPersonal Then
                boolResult = False
            End If
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", strSQL)
            dctException.Add("AdditionalData_Connection", DBConn.ConnectionString)

            RegisterEvent(dctException)
            Throw
        End Try

        Return boolResult
    End Function

    Public Function ValidateAccessCodeToUse(ByVal accessCode As String, ByRef messageValidation As String) As Boolean

        Dim RS As New ADODB.Recordset
        Dim boolResult As Boolean = True
        Dim strSQL As String = String.Format("SELECT ID, DateValidFrom, DateValidTo FROM UsuarioAccessCode WHERE AccessCode = '{0}' AND Id_Usuario IS NULL AND IsActive = 1;", accessCode)

        Try
            RS = New ADODB.Recordset
            If RS.State = 1 Then
                RS.Close()
            End If

            RS = ExecuteQueryAccessDB(strSQL)

            If RS.EOF Then
                messageValidation = "AccessCodeNotExists"
                Return False
            End If

            Dim dateFrom As DateTime = RS.Fields("DateValidFrom").Value
            Dim dateTo As DateTime = RS.Fields("DateValidTo").Value

            ' Get Date from DB Server 
            strSQL = "SELECT GETDATE() AS CurrentDate;"
            RS = ExecuteQueryAccessDB(strSQL)
            Dim currentDate As DateTime = Convert.ToDateTime(RS.Fields("CurrentDate").Value)

            If currentDate > dateTo Then
                messageValidation = "AccessCodeExpired"

                ' Update Status on table UserAccessCode


                Return False
            End If

        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", strSQL)
            dctException.Add("AdditionalData_Connection", DBConn.ConnectionString)
            RegisterEvent(dctException)
        End Try

        Return True
    End Function

    Public Sub RegisterUsuarioAccessCode(ByVal usuarioID As Integer, ByVal accessCode As String)
        Try
            Dim strSQL As String = String.Format("UPDATE dbo.UsuarioAccessCode SET Id_Usuario = {0}, DateUserEnabled = GETDATE() WHERE AccessCode = '{1}'", usuarioID, accessCode)
            DBConn.Execute(strSQL)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub InsertUsuarioAccessCode(ByVal usuarioID As Integer, ByVal accessCode As String)
        Try
            Dim strSQL As String = String.Format("INSERT dbo.UsuarioAccessCode (AccessCode, Id_Usuario, dateValidFrom, DateValidTo, DateUserEnabled, IsActive) VALUES ('{0}',{1}, GETDATE(), DATEADD(YEAR, 1, GETDATE()), GETDATE(), 1)", accessCode, usuarioID)
            DBConn.Execute(strSQL)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function ExecuteQueryAccessDB(ByVal queryToExecute As String) As ADODB.Recordset
        Dim RS As New ADODB.Recordset
        DBConn = SetupAccessConnection()

        RS.let_ActiveConnection(DBConn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(queryToExecute)
        RS.Open()

        Return RS
    End Function

    Public Function GetDatabaseDateTime() As DateTime
        ' Get Date from DB Server 
        Dim RS As New ADODB.Recordset
        Dim strSQL As String = "SELECT GETDATE() AS CurrentDate;"
        RS = ExecuteQueryAccessDB(strSQL)
        Dim currentDate As DateTime = Convert.ToDateTime(RS.Fields("CurrentDate").Value)
        Return currentDate
    End Function

End Class

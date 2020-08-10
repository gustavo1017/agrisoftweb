Imports System.Configuration
Imports System.Text

Public Class BaseBL
    Private _usuario As String
    Public Property LoggedUser() As String
        Get
            Return _usuario
        End Get
        Set(ByVal value As String)
            _usuario = value
        End Set
    End Property

    Private _conn As ADODB.Connection
    Public Property DBConn() As ADODB.Connection
        Get
            Return _conn
        End Get
        Set(ByVal value As ADODB.Connection)
            _conn = value
        End Set
    End Property

    Public Sub New(ByVal strUsuario As String)
        Dim sDBConnectionString As String = ""

        Try
            _usuario = strUsuario


            Dim objSQLCN = New ADODB.Connection()
            objSQLCN.CursorLocation = ADODB.CursorLocationEnum.adUseClient
            sDBConnectionString = GetSQLConnection()
            objSQLCN.Open(sDBConnectionString)
            _conn = objSQLCN
        Catch ex As Exception
            RegisterEvent(ex.Message)
        End Try
    End Sub

    Public Sub New()

    End Sub

    Public Function GetSQLConnection() As String
        'Get the User's DB connection Details

        'Build the Connection String
        Dim cnString As String
        'cnString = String.Format("Provider=SQLOLEDB.1; Data Source={0}; Initial Catalog={1}; User Id={2}; Password={3}", strDataSource, strInitialCatalog, strDBUser, strPassword)
        cnString = ConfigurationManager.ConnectionStrings("RomexConnection").ConnectionString

        Return cnString
    End Function

    Public Function SetupAccessConnection() As ADODB.Connection
        Dim objSQLCN = New ADODB.Connection()
        objSQLCN.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        objSQLCN.Open(ConfigurationManager.ConnectionStrings("AccessConnection").ConnectionString)
        Return objSQLCN
    End Function

    Public Sub RegisterEvent(ByVal sMessage As String)
        Dim sSource As String
        Dim sLog As String
        Dim sEvent As String
        Dim sMachine As String

        sSource = "Agrisoft Web"
        sLog = "Application"
        sEvent = sMessage
        sMachine = "Localhost"

        If Not EventLog.SourceExists(sSource) Then
            Dim objEvent As New EventSourceCreationData(sSource, sLog)

            EventLog.CreateEventSource(objEvent)
        End If

        EventLog.WriteEntry(sSource, sMessage, EventLogEntryType.Warning)
    End Sub

    Public Sub RegisterEvent(ByVal sExceptionData As Dictionary(Of String, String))
        Dim sSource As String
        Dim sLog As String
        'Dim sEvent As String
        Dim sMachine As String
        Dim sMessage As New StringBuilder

        'Build Exception Message
        For Each itemException As KeyValuePair(Of String, String) In sExceptionData
            sMessage.AppendLine(itemException.Key + ":")
            sMessage.AppendLine(itemException.Value)
            sMessage.AppendLine()
        Next

        sSource = "Agrisoft Web"
        sLog = "Application"
        'sEvent = sMessage.ToString()
        sMachine = "Localhost"

        If Not EventLog.SourceExists(sSource) Then
            Dim objEvent As New EventSourceCreationData(sSource, sLog)

            EventLog.CreateEventSource(objEvent)
        End If

        EventLog.WriteEntry(sSource, sMessage.ToString(), EventLogEntryType.Warning)
    End Sub

    Public Sub RegisterEvent(ByVal exception As Exception, ByVal sExceptionData As Dictionary(Of String, String))
        Dim sSource As String
        Dim sLog As String
        Dim sMachine As String
        Dim sMessage As New StringBuilder

        sMessage.AppendLine("ExceptionMessage: " + exception.Message)
        sMessage.AppendLine("StackTrace: " + exception.StackTrace)

        If exception.InnerException IsNot Nothing Then
            sMessage.AppendLine("InnerException: " + exception.InnerException.Message)
        End If

        'Build Exception Message
        For Each itemException As KeyValuePair(Of String, String) In sExceptionData
            sMessage.AppendLine(itemException.Key + ":")
            sMessage.AppendLine(itemException.Value)
            sMessage.AppendLine()
        Next

        sSource = "Agrisoft Web"
        sLog = "Application"
        sMachine = "Localhost"

        If Not EventLog.SourceExists(sSource) Then
            Dim objEvent As New EventSourceCreationData(sSource, sLog)

            EventLog.CreateEventSource(objEvent)
        End If

        EventLog.WriteEntry(sSource, sMessage.ToString(), EventLogEntryType.Warning)
    End Sub

    Public Function GetConnectionInfo() As Dictionary(Of String, String)
        Dim dctConnection As New Dictionary(Of String, String)

        'Get the User's DB connection Details
        Dim _DBconn As ADODB.Connection = SetupAccessConnection()
        Dim RS As New ADODB.Recordset
        Dim strSQL As String = String.Format("SELECT ep.ParametroID, ep.ParametroValor FROM dbo.Usuario u INNER JOIN dbo.EmpresaParametro ep ON u.Id_Empresa = ep.Id_Empresa AND ep.Module = 'BD' WHERE u.Login = '{0}'", _usuario)

        If RS.State = 1 Then
            RS.Close()
        End If

        RS.let_ActiveConnection(_DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(strSQL)
        RS.Open()

        Dim strDataSource As String = "", strInitialCatalog As String = "", strDBUser As String = "", strPassword As String = ""
        If RS.RecordCount > 0 Then
            RS.MoveFirst()
            While Not RS.EOF
                dctConnection.Add(RS.Fields("ParametroID").Value.ToString(), RS.Fields("ParametroValor").Value.ToString())
                RS.MoveNext()
            End While
        End If

        Return dctConnection
    End Function

    Public Function InsertAuditoriaGeneral(ByVal moduleTitle As String) As Boolean
        Dim ssql As String = ""
        Dim boolResult As Boolean = False

        Try
            ssql = "Insert into AUDITORIAGENERAL (ID_USUARIO,modulo,FECHAAUDITORIA) values ('" & _usuario & "','" & moduleTitle & "', getdate())"
            _conn.Execute(ssql)
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
        End Try

        Return boolResult
    End Function

End Class

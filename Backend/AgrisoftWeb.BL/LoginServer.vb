Public Class LoginServer

    Public Function ValidarUsuario(ByVal pUsuario As String, ByVal pPassword As String) As Short
        Dim strSQL As String
        Dim RS As ADODB.Recordset
        Dim objGenericMethods As New GenericMethods()
        Dim DBconn As ADODB.Connection = objGenericMethods.SetupAccessConnection()
        Dim CantUsuarios As Integer = 0

        ValidarUsuario = -1
        If pPassword = "" Then
            ValidarUsuario = 1
            Exit Function
        End If

        strSQL = "SELECT * FROM USUARIO WHERE Login ='" & pUsuario & "' AND Password = '" & pPassword & "';"
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

        If RS.EOF Then
            ValidarUsuario = 1 'Password malo
        Else
            'Close any existing open logins
            CambiaEstadoUsuario(pUsuario, 0)

            ' Validate AccessCode 
            Dim idUsuario As Integer = Convert.ToInt32(RS("Id_Usuario").Value)
            Dim accessCodeOK As Boolean = ValidateAccessCode(idUsuario)
            If Not accessCodeOK Then
                ValidarUsuario = 6  'User's Access Code not valid
                Return ValidarUsuario
            End If

            'Get Id Empresa
            Dim idEmpresa As Integer = Convert.ToInt32(RS.Fields("Id_empresa").Value)

            strSQL = String.Format("SELECT * FROM USUARIO WHERE Login ='{0}' AND ESTADO = 0 AND Id_Empresa = {1}", pUsuario, idEmpresa.ToString())
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
            If RS.EOF Then
                ValidarUsuario = 2 'Usuario activo en otra sesion
            Else
                strSQL = String.Format("SELECT * FROM USUARIO WHERE ESTADO <> 0 AND Id_Empresa = {0};", idEmpresa.ToString())
                objGenericMethods = New GenericMethods()
                CantUsuarios = IIf(String.IsNullOrEmpty(objGenericMethods.GetEmpresaParametroValor(idEmpresa, "CantidadUsuarios")), 0, Int32.Parse(objGenericMethods.GetEmpresaParametroValor(idEmpresa, "CantidadUsuarios")))
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

                If RS.RecordCount >= CantUsuarios Then
                    ValidarUsuario = 5 'Cantidad de usuarios excede
                Else
                    Call CambiaEstadoUsuario(pUsuario, -1)
                    ValidarUsuario = 0
                End If
            End If
        End If
    End Function

    Public Sub CambiaEstadoUsuario(ByRef pUsuario As String, ByRef pEstado As Short)
        Dim strSQL As String
        Dim objGenericMethods As New GenericMethods()
        Dim DBconn As ADODB.Connection = objGenericMethods.SetupAccessConnection()
        'On Error GoTo CtrlErr

        strSQL = "UPDATE USUARIO SET ESTADO =" & pEstado & " WHERE Login='" & pUsuario & "';"
        DBconn.Execute(strSQL)

        Exit Sub

        'CtrlErr:
        '        MsgBox(Err.Description, MsgBoxStyle.OkOnly, My.Application.Info.ProductName)
    End Sub

    Public Function ValidateAccessCode(ByVal idUsuario As Integer) As Boolean
        Dim objGenericMethods As New GenericMethods()
        Dim DBconn As ADODB.Connection = objGenericMethods.SetupAccessConnection()
        Dim strSQL As String = String.Format("SELECT Id_Usuario FROM dbo.UsuarioAccessCode WITH (NOLOCK) WHERE Id_Usuario = {0} AND DateValidTo > GETDATE()", idUsuario.ToString)

        Dim RS As New ADODB.Recordset

        If RS.State = 1 Then
            RS.Close()
        End If
        RS.let_ActiveConnection(DBconn)
        RS.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        RS.CursorType = ADODB.CursorTypeEnum.adOpenStatic
        RS.LockType = ADODB.LockTypeEnum.adLockOptimistic
        RS.let_Source(strSQL)
        RS.Open()

        Dim accessCodeOK As Boolean = Not RS.EOF
        Return accessCodeOK
    End Function
End Class

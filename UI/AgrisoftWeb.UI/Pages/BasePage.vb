Imports System.Data.OleDb
Imports System.Threading
Imports AgrisoftWeb.BL

Public Class BasePage
    Inherits System.Web.UI.Page

    Public currentModule As String
    Public currentPage As String

    Public Sub New()

    End Sub

    Protected Overrides Sub InitializeCulture()
        Dim language As String = "es-es"

        'Detect User's language
        If Request.UserLanguages IsNot Nothing Then
            'Set the language
            language = Request.UserLanguages(0)
        End If

        'Check if postback is caused by language dropdownlist
        If Request.Form("__EVENTTARGET") IsNot Nothing AndAlso Request.Form("__EVENTTARGET").Contains("ddlLanguage") Then
            'Set the language
            language = Request.Form(Request.Form("__EVENTTARGET"))
        Else
            If Session("AppLanguage") <> Nothing Then
                language = Session("AppLanguage")
            End If
        End If

        'Set the culture
        'language = "es-ES"
        Session.Add("AppLanguage", language)
        Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(language)
        Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo(language)
    End Sub

    Public Function HabilitaFrame() As Integer
        'Verify Login Status


        'First get the BusinessUser
        Dim objGenericMethods As New GenericMethods()
        Dim gblstrIdUsuario As String = "DEMO01"

        objGenericMethods = New GenericMethods("Fundo0")
        Dim gblstrIdModulo As String = objGenericMethods.fnDevIdModulo(currentModule.ToUpper())

        Dim RS As New ADODB.Recordset
        RS = objGenericMethods.CheckAutorizacion(gblstrIdModulo, gblstrIdUsuario)

        Dim result As Integer = -1
        If Not RS.EOF Then
            If RS.Fields("bajo").Value Then result = 0
            If RS.Fields("medio").Value Or RS.Fields("Total").Value Then result = 1

            'Verify if page is allowed to show on Menu or its an formulario Especial

        Else
            'Redirect to "Unauthorized" page
            result = -1
        End If

        Return result
    End Function

    Function Buscar(ByVal descripcion As String, ByRef Cbo As DropDownList) As String
        Dim textFound As String = ""

        For Each item As ListItem In Cbo.Items()
            If Trim(item.Text) = Trim(descripcion) Then
                textFound = Trim(item.Text)
                Exit For
            End If
        Next

        Return textFound


        'For i = 0 To Cbo.Items.Count - 1
        '    'Cbo.SelectedIndex = i
        '    Dim cboText = Cbo.SelectedValue '.GetItemText(Cbo.Items(i))

        '    'If Trim(Cbo.Text) = Trim(descripcion) Then
        '    If Trim(cboText) = Trim(descripcion) Then
        '        'Buscar = Cbo.Text
        '        textFound = Trim(cboText)
        '        Exit For
        '    End If
        'Next i

        'Return textFound
    End Function

    Public Sub AuditoriaBackup(ByVal audModulo As String, ByVal audUsuario As String, ByVal audFecha As String, ByVal audOperacion As String, Optional ByVal a As String = "", Optional ByVal B As String = "", Optional ByVal c As String = "", Optional ByVal D As String = "", Optional ByVal e As String = "", Optional ByVal F As String = "", Optional ByVal g As String = "", Optional ByVal H As String = "", Optional ByVal i As String = "", Optional ByVal j As String = "", Optional ByVal k As String = "", Optional ByVal L As String = "", Optional ByVal M As String = "", Optional ByVal n As String = "", Optional ByVal o As String = "", Optional ByVal p As String = "", Optional ByVal q As String = "", Optional ByVal r As String = "", Optional ByVal S As String = "", Optional ByVal t As String = "", Optional ByVal U As String = "", Optional ByVal V As String = "")
        Dim ssql As String = "INSERT INTO AUDITORIABACKUPT (id_modulo, id_usuario, fechaauditoria, Operacion, A, B, C, D, E, F, G, H, I, J, K,l,m,n,O,P,Q,R,S,T,U,V) " & "Values ('" & audModulo & "'," & "'" & audUsuario & "'," & "'" & audFecha & "'," & "'" & audOperacion & "'," & "'" & a & "'," & "'" & B & "'," & "'" & c & "'," & "'" & D & "'," & "'" & e & "'," & "'" & F & "'," & "'" & g & "'," & "'" & H & "'," & "'" & i & "'," & "'" & j & "'," & "'" & k & "'," & "'" & L & "'," & "'" & M & "'," & "'" & n & "'," & "'" & o & "'," & "'" & p & "'," & "'" & q & "'," & "'" & r & "', '" & S & "', '" & t & "', '" & U & "', '" & V & "')"
        Dim DBconn As New ADODB.Connection()
        Dim objBL As New GenericMethods("Fundo0")

        Try
            DBconn.Open(objBL.GetSQLConnection())
            DBconn.Execute(ssql)
        Catch ex As Exception
            Dim dctException As New Dictionary(Of String, String)
            dctException.Add("ExceptionMessage", ex.Message)
            dctException.Add("StackTrace", ex.StackTrace)

            If ex.InnerException IsNot Nothing Then
                dctException.Add("InnerException", ex.InnerException.Message)
            End If

            dctException.Add("AdditionalData_Query", ssql)
            dctException.Add("AdditionalData_Connection", DBconn.ConnectionString)

            objBL.RegisterEvent(dctException)
        End Try
    End Sub

    Public Function cargarDataTable(ByVal sQuery As String) As DataTable
        Dim RS As New ADODB.Recordset
        Dim ssql As String = sQuery
        Dim DBconn As New ADODB.Connection

        If RS.State = 1 Then
            RS.Close()
        End If

        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        RS.let_ActiveConnection(DBconn)
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

    Public Function fBuscarPosCodEnMatriz(ByVal Matriz As Object, ByVal Codigo As String) As Short
        Dim intReg As Short

        For intReg = 0 To UBound(Matriz, 1)

            If Matriz(intReg) = Codigo Then
                Return intReg
            End If
        Next

        Return intReg
    End Function

    Public Function GetEmpresaParametroValorByUserName(ByVal sUserName As String, ByVal sParametroKey As String) As String
        Dim strSQL As String = "SELECT Id_Empresa FROM USUARIO WHERE Login ='" & sUserName & "'"
        Dim objGenericBL As New GenericMethods()
        Dim DBConn As ADODB.Connection = objGenericBL.SetupAccessConnection()
        Dim RS As New ADODB.Recordset

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
        Dim parametroValor As String = objGenericBL.GetEmpresaParametroValor(idEmpresa, sParametroKey)

        Return parametroValor
    End Function

    Public Function GetProductsEnabled(ByVal sUserName As String) As List(Of String)
        Dim parameterValue As String = GetEmpresaParametroValorByUserName(sUserName, "ProductsEnabled")

        If (String.IsNullOrEmpty(parameterValue)) Then
            Return New List(Of String)
        End If

        Dim productsArray As String() = parameterValue.Split(New Char() {","c})
        Dim productsEnabled As New List(Of String)
        productsEnabled = productsArray.ToList()

        Return productsEnabled
    End Function

    Protected Sub CheckCurrentSession()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

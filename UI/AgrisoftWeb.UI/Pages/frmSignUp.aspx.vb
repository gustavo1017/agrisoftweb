Imports AgrisoftWeb.BL

Public Class frmSignUp
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'CheckCurrentSession()
    End Sub

    Protected Sub btnSignUp_Click(sender As Object, e As EventArgs)
        Dim userName As String = txtUsername.Text.Trim
        Dim businessName As String = txtEmpresa.Text.Trim
        Dim objGenericMethods As New GenericMethods()
        Dim existsOK As Boolean = objGenericMethods.UserExists(userName)
        Dim emailExistsOK As Boolean = objGenericMethods.EmailExists(txtEmail.Text.Trim)

        If existsOK Or emailExistsOK Then
            dvMessage.Visible = True

            If existsOK Then
                lblMessage.Text = Resources.Resource1.str999994 '"Lo sentimos, este usuario ya está registrado."
            End If

            If emailExistsOK Then
                lblMessage.Text = Resources.Resource1.str99996
            End If
        Else
            ' Validate AccessCode
            Dim validationMessage As String = ""
            Dim resultValidacion As Boolean = objGenericMethods.ValidateAccessCodeToUse(txtAccessCode.Text, validationMessage)

            If Not resultValidacion Then
                dvMessage.Visible = True

                Select Case validationMessage
                    Case "AccessCodeNotExists"
                        lblMessage.Text = Resources.Resource1.str99995
                    Case "AccessCodeExpired"
                        lblMessage.Text = Resources.Resource1.str99993
                    Case Else
                        lblMessage.Text = Resources.Resource1.str99994
                End Select

                Exit Sub
            End If

            'Create Empresa
            Dim strResultadoEmpresa As String = String.Empty
            If String.IsNullOrEmpty(businessName) Then
                'businessName = email.Substring(0, email.IndexOf("@"))
            End If

            Dim newId_empresa As Integer = objGenericMethods.CreateEmpresa(businessName, "", strResultadoEmpresa)

            If newId_empresa = -1 Then
                dvMessage.Visible = True
                lblMessage.Text = Resources.Resource1.str99997 '"Hubo un error al registrar la empresa. "

                If strResultadoEmpresa = "CreateEmpresa.NameAlreadyExists" Then
                    lblMessage.Text &= Resources.Resource1.str99998 & businessName ' empresa ya existe
                End If

                'TODO: Correr script Rollback nueva compañía
                Exit Sub
            End If

            'Validate if BD exists
            Dim newDatabaseName As String = objGenericMethods.GenerateRandomString(10)
            While Not objGenericMethods.VerifyDatabaseName(newDatabaseName)
                newDatabaseName = objGenericMethods.GenerateRandomString(10)
            End While

            'Create EmpresaParametro
            objGenericMethods.CreateEmpresaParametrosDefault(newId_empresa, newDatabaseName)

            'Create Usuario
            Dim strResultUser As String = String.Empty
            Dim newId_usuario As Integer = objGenericMethods.CreateUser(txtUsername.Text, txtPassword.Text.Trim, newId_empresa, "Email", strResultUser, txtFirstName.Text.Trim, txtLastName.Text.Trim, txtEmail.Text.Trim)

            If newId_usuario = -1 Then
                dvMessage.Visible = True
                lblMessage.Text = Resources.Resource1.str99999 'Resource1.str99999
                'TODO: Correr script Rollback nueva compañía
                Exit Sub
            End If

            'Dim userNameBusiness As String = email.Substring(0, 3) + objGenericMethods.GenerateRandomString(3)     'only 6 digits
            Dim userNameBusiness As String = txtUsername.Text.Trim()

            'Create Database
            Dim ruta As String = Server.MapPath("..\DBScript\AgrisoftDB.sql")
            Dim objresult As Boolean = objGenericMethods.GenerateDB(newDatabaseName, ruta)
            If objresult Then
                objGenericMethods = New GenericMethods(userName)
                objGenericMethods.CreateUserBusinessDB(userNameBusiness)
                objGenericMethods.RegisterUserModules(userNameBusiness)
                objGenericMethods.RegisterUsuarioCategorias(userNameBusiness)
                objGenericMethods.RegisterUsuarioCCostos(userNameBusiness)
                objGenericMethods.RegisterUsuarioGrupos(userNameBusiness)

                objGenericMethods = New GenericMethods()
                objGenericMethods.UpdateUserAgrisoftAccess(userNameBusiness, newId_usuario)
                objGenericMethods.RegisterUsuarioAccessCode(newId_usuario, txtAccessCode.Text.Trim())

                dvMessage.Visible = True
                lblMessage.Text = Resources.Resource1.str999991 ' Resource1.str999991
            End If

            Session.Add("UserLogged", userName)
            pHabilitarMenuPorUsuario(userName)
            'Response.Redirect(FormsAuthentication.DefaultUrl)
            FormsAuthentication.SetAuthCookie(userName, True)
            'Response.Redirect("Default.aspx")

            FormsAuthentication.RedirectFromLoginPage(userName, True)
        End If
    End Sub
End Class
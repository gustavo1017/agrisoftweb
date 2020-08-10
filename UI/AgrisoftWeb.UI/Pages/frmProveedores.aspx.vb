Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmproveedores
    Inherits BasePage

    Dim Openform As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckCurrentSession()
        setCaptionsLabels()
        setCaptionsButtons()

        If Not Page.IsPostBack() Then
            Dim strAction As String = Request.QueryString("Action")

            Select Case UCase(strAction)
                Case "NEW"
                    Call clearTextBox()
                    Session.Add("frmproveedores_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5007
                    End If

                    Session.Add("frmproveedores_Action", "EDIT")
            End Select

            hdnStr5008.Value = "El codigo debe ser de 11 caracteres"
        End If
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        If Validar() Then
            Dim objBL As New Proveedores("Fundo0")
            Dim strAction As String = Request.QueryString("Action")
            Dim strModulo As String = Resource1.str5004

            dvMessage.Visible = True

            Select Case UCase(strAction)
                Case "NEW"
                    If objBL.Add_Renamed(txtId.Text.Trim, descripcion.Text.Trim, strModulo) Then
                        Select Case UCase(Openform)
                            Case "FRMZONATRABAJO", "FRMPRESUPUESTOS"
                                'STRID = id.Text
                            Case Else
                                'El formulario padre se refrescará en el lado del cliente.
                                'frmproveedor_view.Refrescar()
                        End Select
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5004
                    End If

                Case "EDIT"
                    If objBL.Edit(txtId.Text.Trim, descripcion.Text.Trim) Then
                        'El formulario padre se refrescará en el lado del cliente.
                        'frmproveedor_view.Refrescar()
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5005
                    End If

            End Select
        End If
    End Sub



    Private Sub setCaptionsLabels()
        lblCodigo.Text = Resource1.str5002
        lblDescripcion.Text = Resource1.str5003
    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        'btnCancelar.Text = Resource1.str5
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
    End Sub

    Private Function getRecord() As Boolean
        Dim Codigo As String = ""
        Dim rs As New ADODB.Recordset
        Dim blGetRecord As Boolean = False

        If Session("frmproveedores_Codigo") IsNot Nothing Then
            Codigo = Session("frmproveedores_Codigo")
        Else
            blGetRecord = False
        End If

        Dim ssql As String = " SELECT * FROM Proveedores where id_Proveedor='" & Codigo & "'"

        Dim DBconn As New ADODB.Connection
        Dim objBL As New GenericMethods("Fundo0")
        DBconn.Open(objBL.GetSQLConnection())
        rs = DBconn.Execute(ssql)

        If Not rs.EOF Then
            txtId.Text = rs.Fields("id_proveedor").Value
            descripcion.Text = rs.Fields("descripcion").Value
            txtId.Enabled = False
            blGetRecord = True
        Else
            blGetRecord = False
        End If

        rs.Close()
        rs = Nothing

        Return blGetRecord
    End Function

    Function Validar() As Boolean
        Dim rs As New ADODB.Recordset
        Dim strAction As String = Request.QueryString("Action")

        'If strAction = "NEW" Then
        Dim DBconn As New ADODB.Connection
            Dim objBL As New GenericMethods("Fundo0")
            DBconn.Open(objBL.GetSQLConnection())
            'Dim ssql As String = " SELECT descripcion From Proveedores WHERE descripcion=ltrim(rtrim('" & descripcion.Text & "')); "
            Dim ssql As String = String.Format("SELECT DESCRIPCION From Proveedores WHERE DESCRIPCION = '{0}' AND id_proveedor <> '{1}'", descripcion.Text.Trim(), txtId.Text.Trim())
            rs.Open(ssql, DBconn, ADODB.CursorTypeEnum.adOpenStatic, ADODB.LockTypeEnum.adLockReadOnly)

            If Not rs.EOF Then
                dvMessage.Visible = True
                dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resources.Resource1.str999998 ' "Existe un proveedor con la misma descripcion. Pruebe otra descripcion o modifique algun caracter"
            rs.Close()
                Return False
            End If

            rs.Close()
        'End If

        txtId.Text = txtId.Text.ToUpper()
        descripcion.Text = descripcion.Text.ToUpper()
        Return True
    End Function
End Class
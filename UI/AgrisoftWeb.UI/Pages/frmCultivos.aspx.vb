Imports AgrisoftWeb.BL
Imports AgrisoftWeb.UI.Resources

Public Class frmCultivos
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
                    Session.Add("frmCultivos_Action", "NEW")
                Case "EDIT"
                    If Not getRecord() Then
                        dvMessage.Visible = True
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5007
                    End If

                    Session.Add("frmCultivos_Action", "EDIT")
            End Select

            hdnStr5008.Value = Resource1.str5008
        End If
    End Sub

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs)
        If Validar() Then
            Dim objBL As New Cultivos("Fundo0")
            Dim strAction As String = Request.QueryString("Action")
            Dim strModulo As String = Resource1.str5004

            dvMessage.Visible = True

            Select Case UCase(strAction)
                Case "NEW"
                    If objBL.Add_Renamed(txtId.Text.Trim, descripcion.Text.Trim, txtComponente.Text.Trim, strModulo) Then
                        Select Case UCase(Openform)
                            Case "FRMZONATRABAJO", "FRMPRESUPUESTOS"
                                'STRID = id.Text
                            Case Else
                                'El formulario padre se refrescará en el lado del cliente.
                                'frmCultivos_view.Refrescar()
                        End Select
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                        btnGrabar.Enabled = False
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str543
                    End If
                Case "EDIT"
                    If objBL.Edit(txtId.Text.Trim, descripcion.Text.Trim, txtComponente.Text.Trim) Then
                        'El formulario padre se refrescará en el lado del cliente.
                        'frmCultivos_view.Refrescar()
                        dvMessage.Attributes.Add("class", "alert alert-success")
                        lblResult.Text = Resource1.str543
                    Else
                        dvMessage.Attributes.Add("class", "alert alert-danger")
                        lblResult.Text = Resource1.str5005
                    End If
            End Select
        End If
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs)
        'El formulario padre se refrescará en el lado del cliente.
        'If Openform = "" Then frmCultivos_view.Refrescar()

        'El proceso 
        'Openform = "" : Me.Close()
    End Sub

    Private Sub setCaptionsLabels()
        lblCodigo.Text = Resource1.str5002
        lblDescripcion.Text = Resource1.str5003
    End Sub

    Private Sub setCaptionsButtons()
        btnGrabar.Text = Resource1.str4
        btnCancelar.Text = Resource1.str5
    End Sub

    Private Sub clearTextBox()
        txtId.Text = ""
        descripcion.Text = ""
    End Sub

    Private Function getRecord() As Boolean
        Dim Codigo As String = ""
        Dim blGetRecord As Boolean = False

        If Session("frmCultivos_Codigo") IsNot Nothing Then
            Codigo = Session("frmCultivos_Codigo")
        Else
            blGetRecord = False
        End If

        Dim rs As New ADODB.Recordset
        Dim objBL As New Cultivos("Fundo0")
        rs = objBL.getRecord(Codigo)

        If Not rs.EOF Then
            txtId.Text = rs.Fields("id_cultivo").Value
            descripcion.Text = rs.Fields("descripcion").Value
            txtComponente.Text = rs.Fields("id_componentecuenta9").Value
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
        Dim objBL As New Cultivos("Fundo0")
            rs = objBL.getCultivosByDescripcion(txtId.Text.Trim(), descripcion.Text.Trim())

        If Not rs.EOF Then
            dvMessage.Visible = True
            dvMessage.Attributes.Add("class", "alert alert-danger")
            lblResult.Text = Resource1.str99999987
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
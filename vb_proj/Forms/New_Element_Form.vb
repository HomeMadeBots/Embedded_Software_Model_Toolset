Public Class New_Element_Form
    Inherits ESMT_Form

    Private WithEvents Name_TextBox As TextBox
    Private Description_TextBox As RichTextBox
    Protected WithEvents Create_Button As Button

    Private Forbidden_Names As List(Of String)

    Public Sub New(
            default_name As String,
            default_description As String,
            forbidden_name_list As List(Of String))

        Me.Forbidden_Names = forbidden_name_list

        Dim item_y_pos As Integer = ESMT_Form.Marge
        Dim inner_item_y_pos As Integer = ESMT_Form.Marge

        '------------------------------------------------------------------------------------------'
        ' Add name panel
        Dim name_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, ESMT_Form.Marge),
            .BorderStyle = BorderStyle.FixedSingle}
        Me.Controls.Add(name_panel)

        Dim name_label As New Label With {
            .Text = "Name",
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        name_panel.Controls.Add(name_label)
        inner_item_y_pos += name_label.Height

        Me.Name_TextBox = New TextBox With {
            .Text = default_name,
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        name_panel.Controls.Add(Me.Name_TextBox)
        inner_item_y_pos += Me.Name_TextBox.Height + ESMT_Form.Marge

        name_panel.Size = New Size(Panel_Width, inner_item_y_pos)
        item_y_pos += name_panel.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' Add description panel
        inner_item_y_pos = Marge

        Dim desc_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, item_y_pos),
            .BorderStyle = BorderStyle.FixedSingle}
        Me.Controls.Add(desc_panel)

        Dim description_label As New Label With {
            .Text = "Description",
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        desc_panel.Controls.Add(description_label)
        inner_item_y_pos += description_label.Height

        Me.Description_TextBox = New RichTextBox With {
            .Text = default_description,
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = New Size(ESMT_Form.Label_Width, 100)}
        desc_panel.Controls.Add(Me.Description_TextBox)
        inner_item_y_pos += Me.Description_TextBox.Height + ESMT_Form.Marge

        desc_panel.Size = New Size(Panel_Width, inner_item_y_pos)
        item_y_pos += desc_panel.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' Design Create button
        Me.Create_Button = New Button With {
            .Text = "Create",
            .Size = ESMT_Form.Button_Size,
            .Location = New Point((Form_Width - Button_Width) \ 2, item_y_pos)}
        Me.Controls.Add(Me.Create_Button)

        item_y_pos += Me.Create_Button.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' Design Form
        Me.Text = "Create a new element"
        Me.ClientSize = New Size(Form_Width, item_y_pos)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog

        Me.Name_TextBox.Select()

    End Sub

    Public Function Get_Name() As String
        Return Me.Name_TextBox.Text
    End Function

    Public Function Get_Description() As String
        Return Me.Description_TextBox.Text
    End Function

    Private Sub Create() Handles Create_Button.Click

        If Not Software_Element.Is_Symbol_Valid(Me.Name_TextBox.Text) Then
            MsgBox("Invalid name", MsgBoxStyle.Exclamation)

        ElseIf Me.Forbidden_Names.Contains(Me.Name_TextBox.Text) Then
            MsgBox("A element with the same name is already aggregated", MsgBoxStyle.Exclamation)

        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

    End Sub

End Class

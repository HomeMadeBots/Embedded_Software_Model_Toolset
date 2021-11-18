Public Class Edition_Form
    Inherits ESMT_Form

    Protected WithEvents Apply_Button As Button

    Protected Name_TextBox As TextBox
    Protected UUID_TextBox As TextBox
    Protected Description_TextBox As RichTextBox

    Public Sub New(
            default_name As String,
            default_uuid As String,
            default_description As String)

        Dim item_y_pos As Integer = ESMT_Form.Marge
        Dim inner_item_y_pos As Integer = ESMT_Form.Marge

        '------------------------------------------------------------------------------------------'
        ' Add element name panel
        Dim elmt_name_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, ESMT_Form.Marge),
            .BorderStyle = BorderStyle.FixedSingle}
        Me.Controls.Add(elmt_name_panel)

        Dim elmt_name_label As New Label With {
            .Text = "Name",
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        elmt_name_panel.Controls.Add(elmt_name_label)
        inner_item_y_pos += elmt_name_label.Height

        Me.Name_TextBox = New TextBox With {
            .Text = default_name,
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        elmt_name_panel.Controls.Add(Me.Name_TextBox)
        inner_item_y_pos += Me.Name_TextBox.Height + ESMT_Form.Marge

        elmt_name_panel.Size = New Size(Panel_Width, inner_item_y_pos)
        item_y_pos += elmt_name_panel.Height + ESMT_Form.Marge

        '------------------------------------------------------------------------------------------'
        ' Add element UUID panel
        inner_item_y_pos = ESMT_Form.Marge

        Dim elmt_uuid_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, item_y_pos),
            .BorderStyle = BorderStyle.FixedSingle}
        Me.Controls.Add(elmt_uuid_panel)

        Dim elmt_uuid_label As New Label With {
            .Text = "UUID",
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        elmt_uuid_panel.Controls.Add(elmt_uuid_label)
        inner_item_y_pos += elmt_uuid_label.Height

        Me.UUID_TextBox = New TextBox With {
            .Text = default_uuid,
            .ReadOnly = True,
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        elmt_uuid_panel.Controls.Add(Me.UUID_TextBox)
        inner_item_y_pos += Me.UUID_TextBox.Height + ESMT_Form.Marge

        elmt_uuid_panel.Size = New Size(Panel_Width, inner_item_y_pos)
        item_y_pos += elmt_uuid_panel.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' Add element description panel
        inner_item_y_pos = Marge

        Dim elmt_desc_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, item_y_pos),
            .BorderStyle = BorderStyle.FixedSingle}
        Me.Controls.Add(elmt_desc_panel)

        Dim elmt_description_label As New Label With {
            .Text = "Description",
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        elmt_desc_panel.Controls.Add(elmt_description_label)
        inner_item_y_pos += elmt_description_label.Height

        Me.Description_TextBox = New RichTextBox With {
            .Text = default_description,
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = New Size(ESMT_Form.Label_Width, 100)}
        elmt_desc_panel.Controls.Add(Me.Description_TextBox)
        inner_item_y_pos += Me.Description_TextBox.Height + ESMT_Form.Marge

        elmt_desc_panel.Size = New Size(Panel_Width, inner_item_y_pos)
        item_y_pos += elmt_desc_panel.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' Design Apply button
        Me.Apply_Button = New Button With {
            .Text = "Apply",
            .Size = ESMT_Form.Button_Size,
            .Location = New Point((Form_Width - Button_Width) \ 2, item_y_pos)}
        Me.Controls.Add(Me.Apply_Button)

        item_y_pos += Me.Apply_Button.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' Design Form
        Me.Text = "Edit Software_Element"
        Me.ClientSize = New Size(Form_Width, item_y_pos)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.FormBorderStyle = FormBorderStyle.FixedDialog


    End Sub


    Public Function Get_Name() As String
        Return Me.Name_TextBox.Text
    End Function

    Public Function Get_Description() As String
        Return Me.Description_TextBox.Text
    End Function


    Private Sub Apply_Pressed() Handles Apply_Button.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

End Class

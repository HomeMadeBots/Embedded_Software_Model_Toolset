Public Class New_Element_With_Ref_Form
    Inherits New_Element_Form

    Protected WithEvents Referenced_Element_ComboBox As ComboBox

    Public Sub New(
            default_name As String,
            default_description As String,
            forbidden_name_list As List(Of String),
            ref_element_title As String,
            default_ref_element_path As String,
            ref_element_path_list As List(Of String))

        MyBase.New(default_name, default_description, forbidden_name_list)

        ' Get the current y position of Create_Button
        Dim item_y_pos As Integer = Me.ClientSize.Height - ESMT_Form.Marge - Button_Height

        Dim inner_item_y_pos As Integer = ESMT_Form.Marge

        '------------------------------------------------------------------------------------------'
        ' Add referenced element path panel
        Dim ref_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, item_y_pos),
            .BorderStyle = BorderStyle.FixedSingle}
        Me.Controls.Add(ref_panel)

        Dim ref_label As New Label With {
            .Text = ref_element_title,
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        ref_panel.Controls.Add(ref_label)
        inner_item_y_pos += ref_label.Height

        Me.Referenced_Element_ComboBox = New ComboBox
        If Not IsNothing(ref_element_path_list) Then
            For Each ref_element_path In ref_element_path_list
                Me.Referenced_Element_ComboBox.Items.Add(ref_element_path)
            Next
        Else
            Me.Referenced_Element_ComboBox.Items.Add(default_ref_element_path)
        End If
        With Me.Referenced_Element_ComboBox
            .DropDownStyle = ComboBoxStyle.DropDownList
            .Text = default_ref_element_path
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos)
            .Size = ESMT_Form.Label_Size
        End With
        ref_panel.Controls.Add(Me.Referenced_Element_ComboBox)
        inner_item_y_pos += Me.Referenced_Element_ComboBox.Height + ESMT_Form.Marge

        ref_panel.Size = New Size(Panel_Width, inner_item_y_pos)
        item_y_pos += ref_panel.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' (Re)design Create button
        Me.Create_Button.Location = New Point((Form_Width - Button_Width) \ 2, item_y_pos)
        item_y_pos += Me.Create_Button.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' (Re)design Form
        Me.ClientSize = New Size(Form_Width, item_y_pos)

    End Sub


    Public Function Get_Ref_Rerenced_Element_Path() As String
        Return Me.Referenced_Element_ComboBox.Text
    End Function

End Class

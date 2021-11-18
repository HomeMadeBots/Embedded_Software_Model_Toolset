Imports System.IO

Public Class New_Recordable_Element_Form

    Inherits ESMT_Form

    Private WithEvents Name_TextBox As TextBox
    Private Description_TextBox As RichTextBox
    Private Directory_TextBox As TextBox
    Private WithEvents Directory_Button As Button
    Private File_Name_TextBox As TextBox
    Private WithEvents Create_Button As Button

    Private Element_File_Extension As String

    Public Enum Recordable_Element_Kind
        PROJECT
        PACKAGE
    End Enum

    Public Sub New(
            default_name As String,
            default_description As String,
            default_directory As String,
            default_file_name As String,
            file_extension As String,
            element_kind As Recordable_Element_Kind)

        Me.Element_File_Extension = file_extension

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
        ' Add file directory selection panel
        inner_item_y_pos = Marge

        Dim dir_label As New Label With {
            .Text = "Directory",
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        inner_item_y_pos += dir_label.Height

        Me.Directory_TextBox = New TextBox With {
            .Location = New Point(Marge, inner_item_y_pos),
            .Size = Path_Text_Size,
            .Text = default_directory}

        Me.Directory_Button = New Button With {
            .Location = New Point(Path_Button_X_Pos, inner_item_y_pos),
            .Size = ESMT_Form.Path_Button_Size,
            .Text = "..."}
        inner_item_y_pos += Me.Directory_TextBox.Height + ESMT_Form.Marge

        Dim dir_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, item_y_pos),
            .BorderStyle = BorderStyle.FixedSingle,
            .Size = New Size(Panel_Width, inner_item_y_pos)}
        With dir_panel.Controls
            .Add(dir_label)
            .Add(Me.Directory_TextBox)
            .Add(Me.Directory_Button)
        End With
        Me.Controls.Add(dir_panel)
        item_y_pos += dir_panel.Height + ESMT_Form.Marge


        '------------------------------------------------------------------------------------------'
        ' Add file name panel
        inner_item_y_pos = Marge

        Dim file_label As New Label With {
            .Text = "File name",
            .Location = New Point(ESMT_Form.Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size}
        inner_item_y_pos += file_label.Height

        Me.File_Name_TextBox = New TextBox With {
            .Location = New Point(Marge, inner_item_y_pos),
            .Size = ESMT_Form.Label_Size,
            .Text = default_file_name & file_extension}
        inner_item_y_pos += Me.File_Name_TextBox.Height + ESMT_Form.Marge

        Dim file_panel As New Panel With {
            .Location = New Point(ESMT_Form.Marge, item_y_pos),
            .BorderStyle = BorderStyle.FixedSingle,
            .Size = New Size(Panel_Width, inner_item_y_pos)}
        With file_panel.Controls
            .Add(file_label)
            .Add(Me.File_Name_TextBox)
        End With
        Me.Controls.Add(file_panel)
        item_y_pos += file_panel.Height + ESMT_Form.Marge


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
        Select Case element_kind
            Case Recordable_Element_Kind.PACKAGE
                Me.Text = "Create a new package"
            Case Recordable_Element_Kind.PROJECT
                Me.Text = "Create a new project"
        End Select
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

    Public Function Get_File_Full_Path() As String
        Return Me.Directory_TextBox.Text _
            & Path.DirectorySeparatorChar _
            & Me.File_Name_TextBox.Text
    End Function

    Private Sub Name_Modified() Handles Name_TextBox.TextChanged
        Me.File_Name_TextBox.Text = Me.Name_TextBox.Text & Me.Element_File_Extension
    End Sub

    Private Sub Path_Button_Clicked() Handles Directory_Button.Click
        ESMT_Form.Select_Directory("Choose directory", Me.Directory_TextBox)
    End Sub

    Private Sub Create() Handles Create_Button.Click

        Dim file_name_wo_ext_length As Integer
        file_name_wo_ext_length = Me.File_Name_TextBox.Text.Length _
                                - Me.Element_File_Extension.Length

        Dim file_name_wo_ext As String
        file_name_wo_ext = Me.File_Name_TextBox.Text.Substring(0, file_name_wo_ext_length)

        If Not Software_Element.Is_Symbol_Valid(Me.Name_TextBox.Text) Then
            MsgBox("Invalid name", MsgBoxStyle.Exclamation)

        ElseIf Not Directory.Exists(Me.Directory_TextBox.Text) Then
            MsgBox("Invalid directory", MsgBoxStyle.Exclamation)

        ElseIf Not Me.File_Name_TextBox.Text.EndsWith(Me.Element_File_Extension) Then
            MsgBox("Invalid file extension", MsgBoxStyle.Exclamation)

        ElseIf Not Software_Element.Is_Symbol_Valid(file_name_wo_ext) Then
            MsgBox("Invalid file name", MsgBoxStyle.Exclamation)

        ElseIf File.Exists(Me.Directory_TextBox.Text & Path.DirectorySeparatorChar &
                Me.File_Name_TextBox.Text) Then
            MsgBox(Me.File_Name_TextBox.Text & " already exists in " & Me.Directory_TextBox.Text,
                MsgBoxStyle.Critical)

        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()

        End If

    End Sub

End Class
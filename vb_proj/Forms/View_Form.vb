Public Class View_Form
    Inherits Edition_Form

    Public Sub New(
            default_name As String,
            default_uuid As String,
            default_description As String)
        MyBase.New(default_name, default_uuid, default_description)
        Me.Name_TextBox.ReadOnly = True
        Me.Description_TextBox.ReadOnly = True
        Me.Apply_Button.Text = "OK"
        Me.Text = "View Software_Element"
    End Sub

End Class

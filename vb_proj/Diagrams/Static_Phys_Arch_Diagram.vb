Public Class Static_Physical_Architecture_Diagram
    Inherits Diagram

    Public Shared ReadOnly Metaclass_Name As String = "Diagram"

    ' -------------------------------------------------------------------------------------------- '
    ' Constructors
    ' -------------------------------------------------------------------------------------------- '

    Public Sub New()
    End Sub

    Public Sub New(
            name As String,
            description As String,
            owner As Software_Element,
            parent_node As TreeNode)
        MyBase.New(name, description, owner, parent_node)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods from Software_Element
    ' -------------------------------------------------------------------------------------------- '

    Protected Overrides Sub Create_Node()
        Me.Node = New TreeNode(Me.Name) With {
            .ImageKey = "Diagram",
            .SelectedImageKey = "Diagram",
            .ContextMenuStrip = Diagram.Context_Menu,
            .Tag = Me}
    End Sub

    Public Overrides Function Get_Metaclass_Name() As String
        Return Static_Physical_Architecture_Diagram.Metaclass_Name
    End Function

End Class

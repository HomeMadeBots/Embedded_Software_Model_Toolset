Public MustInherit Class Type
    Inherits Software_Element

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
    Public Overrides Function Is_Allowed_Parent(parent As Software_Element) As Boolean
        Dim is_allowed As Boolean = False
        If parent.GetType() = GetType(Top_Level_Package) _
            Or parent.GetType() = GetType(Package) Then
            is_allowed = True
        End If
        Return is_allowed
    End Function

End Class


Public MustInherit Class Basic_Type
    Inherits Type

    ' -------------------------------------------------------------------------------------------- '
    ' Methods from Software_Element
    ' -------------------------------------------------------------------------------------------- '
    Protected Overrides Sub Create_Node()
        Me.Node = New TreeNode(Me.Name) With {
            .ImageKey = "Basic_Type",
            .SelectedImageKey = "Basic_Type",
            .ContextMenuStrip = Software_Element.Read_Only_Context_Menu,
            .Tag = Me}
    End Sub

    Protected Overrides Sub Move_Me(new_parent As Software_Element)
        Throw New Exception("A Basic Type cannot be moved.")
    End Sub

    Protected Overrides Sub Remove_Me()
        Throw New Exception("A Basic Type cannot be removed.")
    End Sub

    Protected Overrides Function Get_Writable_Context_Menu() As ContextMenuStrip
        Return Software_Element.Read_Only_Context_Menu
    End Function

End Class


Public Class Basic_Integer_Type
    Inherits Basic_Type
End Class


Public Class Basic_Boolean_Type
    Inherits Basic_Type
End Class


Public Class Basic_Floating_Point_Type
    Inherits Basic_Type
End Class


Public Class Array_Type
    Inherits Type

    Public Multiplicity As UInteger
    Public Base_Type_Ref As Guid

    ' -------------------------------------------------------------------------------------------- '
    ' Constructors
    ' -------------------------------------------------------------------------------------------- '

    Public Sub New()
    End Sub

    Public Sub New(
            name As String,
            description As String,
            owner As Software_Element,
            parent_node As TreeNode,
            multiplicity As UInteger,
            base_type_ref As Guid)
        MyBase.New(name, description, owner, parent_node)
        Me.Multiplicity = multiplicity
        Me.Base_Type_Ref = base_type_ref
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods from Software_Element
    ' -------------------------------------------------------------------------------------------- '

    Protected Overrides Sub Create_Node()
        Me.Node = New TreeNode(Me.Name) With {
            .ImageKey = "Array_Type",
            .SelectedImageKey = "Array_Type",
            .ContextMenuStrip = Software_Element.Leaf_Context_Menu,
            .Tag = Me}
    End Sub

    Protected Overrides Sub Move_Me(new_parent As Software_Element)
        Throw New NotImplementedException()
    End Sub

    Protected Overrides Sub Remove_Me()
        Throw New NotImplementedException()
    End Sub

End Class
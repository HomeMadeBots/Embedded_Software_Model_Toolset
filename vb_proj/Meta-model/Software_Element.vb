Imports System.Text.RegularExpressions

Public MustInherit Class Software_Element

    Public Name As String
    Public UUID As Guid
    Public Description As String

    Protected Node As TreeNode

    Protected Owner As Software_Element = Nothing
    Protected Children As List(Of Software_Element) = Nothing

    Protected Shared Valid_Symbol_Regex As String = "^[a-zA-Z][a-zA-Z0-9_]+$"

    Protected Shared Read_Only_Context_Menu As New Read_Only_Context_Menu
    Protected Shared Leaf_Context_Menu As New Leaf_Context_Menu


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
        Me.Name = name
        Me.Description = description
        Me.UUID = Guid.NewGuid()
        Me.Owner = owner
        Me.Create_Node()
        parent_node.Nodes.Add(Me.Node)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Shared
    ' -------------------------------------------------------------------------------------------- '

    Public Shared Function Is_Symbol_Valid(symbol As String) As Boolean
        Dim result As Boolean = False
        If Regex.IsMatch(symbol, Software_Element.Valid_Symbol_Regex) Then
            result = True
        End If
        Return result
    End Function

    Public Shared Function Get_Default_Description() As String
        Return "A good description is always useful."
    End Function


    ' -------------------------------------------------------------------------------------------- '
    ' Generic methods
    ' -------------------------------------------------------------------------------------------- '

    Protected Overridable Function Get_Children() As List(Of Software_Element)
        Return Nothing
    End Function

    Protected MustOverride Sub Create_Node()

    Protected Sub Post_Treat_After_Deserialization(parent_node As TreeNode)
        Me.Create_Node()
        parent_node.Nodes.Add(Me.Node)
        If Not Me.Get_Top_Package().Is_Writable() Then
            Me.Node.ContextMenuStrip = Software_Element.Read_Only_Context_Menu
        End If
        Dim children As List(Of Software_Element) = Me.Get_Children()
        If Not IsNothing(children) Then
            For Each child In children
                child.Owner = Me
                child.Post_Treat_After_Deserialization(Me.Node)
            Next
        End If
    End Sub

    Public Function Get_Top_Package() As Top_Level_Package
        Dim top_pkg As Top_Level_Package
        Dim parent As Software_Element = Me.Owner
        If IsNothing(parent) Then
            top_pkg = CType(Me, Top_Level_Package)
        Else
            While Not IsNothing(parent.Owner)
                parent = parent.Owner
            End While
            top_pkg = CType(parent, Top_Level_Package)
        End If
        Return top_pkg
    End Function

    Public Sub Display_Package_Modified()
        Dim owner_pkg As Top_Level_Package = Me.Get_Top_Package()
        owner_pkg.Display_Modified()
    End Sub

    Protected Function Get_Children_Name() As List(Of String)
        Dim children_name As New List(Of String)
        For Each child In Me.Get_Children
            children_name.Add(child.Name)
        Next
        Return children_name
    End Function

    Public MustOverride Function Is_Allowed_Parent(parent As Software_Element) As Boolean

    Public Sub Move(new_parent As Software_Element)
        ' Manage top level packages
        Me.Display_Package_Modified()
        new_parent.Display_Package_Modified()

        ' Update model
        Me.Move_Me(new_parent)
        Me.Owner.Children.Remove(Me)
        Me.Owner = new_parent
        new_parent.Children.Add(Me)

        ' Manage TreeNode
        Me.Node.Remove()
        new_parent.Node.Nodes.Add(Me.Node)

    End Sub

    Public Sub Apply_Read_Only_Context_Menu()
        Me.Node.ContextMenuStrip = Software_Element.Read_Only_Context_Menu
        If Not IsNothing(Me.Children) Then
            For Each child In Me.Children
                child.Apply_Read_Only_Context_Menu()
            Next
        End If
    End Sub

    Public Sub Apply_Writable_Context_Menu()
        Me.Node.ContextMenuStrip = Me.Get_Writable_Context_Menu()
        If Not IsNothing(Me.Children) Then
            For Each child In Me.Children
                child.Apply_Writable_Context_Menu()
            Next
        End If
    End Sub

    Protected Overridable Function Get_Writable_Context_Menu() As ContextMenuStrip
        Return Software_Element.Leaf_Context_Menu
    End Function

    Protected MustOverride Sub Move_Me(new_parent As Software_Element)

    Protected MustOverride Sub Remove_Me()

    ' -------------------------------------------------------------------------------------------- '
    ' Methods for contextual menu
    ' -------------------------------------------------------------------------------------------- '

    Public Overridable Sub Edit()
        Dim elmt_edit_form As New Edition_Form(Me.Name, Me.UUID.ToString, Me.Description)
        Dim edit_result As DialogResult
        edit_result = elmt_edit_form.ShowDialog()
        If edit_result = DialogResult.OK Then
            Me.Name = elmt_edit_form.Get_Name()
            Me.Node.Text = Me.Name
            Me.Description = elmt_edit_form.Get_Description()
            Me.Display_Package_Modified()
        End If
    End Sub

    Public Sub Remove()
        Dim remove_dialog_result As MsgBoxResult
        remove_dialog_result = MsgBox(
            "Do you want to remove """ & Me.Name & """ and all its aggregated elements ?",
             MsgBoxStyle.OkCancel,
            "Remove element")
        If remove_dialog_result = MsgBoxResult.Ok Then
            Me.Remove_Me()
            Me.Display_Package_Modified()
        End If
    End Sub

    Public Overridable Sub View()
        Dim elmt_view_form As New View_Form(Me.Name, Me.UUID.ToString, Me.Description)
        elmt_view_form.ShowDialog()
    End Sub

End Class
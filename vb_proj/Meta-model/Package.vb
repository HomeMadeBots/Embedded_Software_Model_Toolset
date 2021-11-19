Public Class Package
    Inherits Software_Element

    Public Packages As New List(Of Package)

    Private Shared Context_Menu As New Package_Context_Menu()

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
        Me.Packages = New List(Of Package)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods from Software_Element
    ' -------------------------------------------------------------------------------------------- '

    Protected Overrides Function Get_Children() As List(Of Software_Element)
        If IsNothing(Me.Children) Then
            Me.Children = New List(Of Software_Element)
            Me.Children.AddRange(Me.Packages)
        End If
        Return Me.Children
    End Function

    Protected Overrides Sub Create_Node()
        Me.Node = New TreeNode(Me.Name) With {
            .ImageKey = "Package",
            .SelectedImageKey = "Package",
            .ContextMenuStrip = Package.Context_Menu,
            .Tag = Me}
    End Sub

    Public Overrides Function Is_Allowed_Parent(parent As Software_Element) As Boolean
        Dim is_allowed As Boolean = False
        If parent.GetType() = GetType(Top_Level_Package) _
            Or parent.GetType() = GetType(Package) Then
            is_allowed = True
        End If
        Return is_allowed
    End Function

    Protected Overrides Sub Move_Me(new_parent As Software_Element)
        Dim pkg_list As List(Of Package)
        pkg_list = CType(Me.Owner, Package).Packages
        Dim r As Boolean = pkg_list.Remove(Me)
        CType(new_parent, Package).Packages.Add(Me)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods for contextual menu
    ' -------------------------------------------------------------------------------------------- '

    Public Sub Add_Package()
        Dim creation_form As New New_Element_Form("Package", "", Me.Get_Children_Name())
        Dim creation_form_result As DialogResult = creation_form.ShowDialog()
        If creation_form_result = DialogResult.OK Then
            Dim new_pkg As New Package(
                creation_form.Get_Name(),
                creation_form.Get_Description(),
                Me,
                Me.Node)
            Me.Packages.Add(new_pkg)
            Me.Display_Package_Modified()
        End If
    End Sub

End Class
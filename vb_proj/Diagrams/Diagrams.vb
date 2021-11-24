Public MustInherit Class Diagram
    Inherits Software_Element

    Private Page As TabPage
    Protected Picture As PictureBox

    Protected Shared Context_Menu As New Diagram_Context_Menu

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
        Me.Create_Page_And_Picture()
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

    Protected Overrides Sub Manage_Diagrams()
        Me.Create_Page_And_Picture()
    End Sub

    Protected Overrides Sub Move_Me(new_parent As Software_Element)
        CType(Me.Owner, Package).Diagrams.Remove(Me)
        CType(new_parent, Package).Diagrams.Add(Me)
    End Sub

    Protected Overrides Sub Remove_Me()
        Dim parent_pkg As Package = CType(Me.Owner, Package)
        Me.Node.Remove()
        parent_pkg.Diagrams.Remove(Me)
    End Sub

    Public Overrides Function Is_Allowed_Parent(parent As Software_Element) As Boolean
        Dim is_allowed As Boolean = False
        If parent.GetType() = GetType(Top_Level_Package) _
            Or parent.GetType() = GetType(Package) Then
            is_allowed = True
        End If
        Return is_allowed
    End Function


    ' -------------------------------------------------------------------------------------------- '
    ' Methods for contextual menu
    ' -------------------------------------------------------------------------------------------- '

    Public Sub Show()
        Me.Get_Project().Add_Diagram_View(Me.Page)
    End Sub

    Public Sub Hide()
        Me.Get_Project().Hide_Diagram_View(Me.Page)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' 
    ' -------------------------------------------------------------------------------------------- '

    Public Overridable Sub Draw(ByVal g As Graphics)

    End Sub

    Private Sub Create_Page_And_Picture()
        Me.Page = New TabPage With {
            .Text = Me.Name,
            .ToolTipText = Me.Description,
            .Tag = Me}
        Me.Picture = New PictureBox With {
            .Dock = DockStyle.Fill,
            .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom _
                Or AnchorStyles.Left Or AnchorStyles.Right,
            .Name = "picture"} ' coupled with ESMT_Main_Window.Update_Active_Diagram
        Me.Page.Controls.Add(Me.Picture)
    End Sub


End Class

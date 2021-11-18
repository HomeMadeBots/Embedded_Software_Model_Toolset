Public Class Element_Context_Menu

    Inherits ContextMenuStrip

    Protected WithEvents Menu_Edit As New ToolStripMenuItem("Edit")

    Private Sub Edit(
            ByVal sender As Object,
            ByVal e As EventArgs) Handles Menu_Edit.Click
        Get_Selected_Element(sender).Edit()
    End Sub

    Protected Shared Function Get_Selected_Element(sender As Object) As Software_Element
        Dim tsmi As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim cms As ContextMenuStrip = CType(tsmi.Owner, ContextMenuStrip)
        Dim tv As TreeView = CType(cms.SourceControl, TreeView)
        Dim tn As TreeNode = tv.GetNodeAt(tv.PointToClient(cms.Location))
        Return CType(tn.Tag, Software_Element)
    End Function

    Protected Shared Function Get_Selected_Element_Owner(sender As Object) As Software_Element
        Dim tsmi As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim cms As ContextMenuStrip = CType(tsmi.Owner, ContextMenuStrip)
        Dim tv As TreeView = CType(cms.SourceControl, TreeView)
        Dim tn As TreeNode = tv.GetNodeAt(tv.PointToClient(cms.Location))
        Return CType(tn.Parent.Tag, Software_Element)
    End Function

End Class

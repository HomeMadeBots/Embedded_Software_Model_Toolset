Public Class Top_Package_Readable_Context_Menu

    Inherits Top_Package_Context_Menu

    Private WithEvents Menu_Make_Writable As New ToolStripMenuItem("Make writable")

    Public Sub New()
        Me.Items.AddRange(New ToolStripItem() {
            Me.Menu_Remove,
            New ToolStripSeparator,
            Me.Menu_Make_Writable})
    End Sub

End Class

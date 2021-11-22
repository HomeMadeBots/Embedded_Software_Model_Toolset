Public Class Diagram_Page_Context_Menu

    Inherits ContextMenuStrip

    Protected WithEvents Menu_Hide As New ToolStripMenuItem("Hide")

    Public Sub New()
        Me.Items.AddRange(New ToolStripItem() {
            Me.Menu_Hide})
    End Sub

    Private Sub Hide_Page(
        ByVal sender As Object,
        ByVal e As EventArgs) Handles Menu_Hide.Click
        Dim tsmi As ToolStripMenuItem = CType(sender, ToolStripMenuItem)
        Dim cms As ContextMenuStrip = CType(tsmi.Owner, ContextMenuStrip)
        Dim tb As TabPage = CType(cms.SourceControl, TabPage)
        CType(tb.Tag, Diagram).Hide()
    End Sub

End Class

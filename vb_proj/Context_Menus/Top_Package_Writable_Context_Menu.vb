Public Class Top_Package_Writable_Context_Menu

    Inherits Top_Package_Context_Menu

    Private WithEvents Menu_Save As New ToolStripMenuItem("Save")
    Private WithEvents Menu_Add_Package As New ToolStripMenuItem("Add Package")

    Public Sub New()
        Me.Items.AddRange(New ToolStripItem() {
            Me.Menu_Save,
            Me.Menu_Remove,
            New ToolStripSeparator,
            Me.Menu_Edit,
            New ToolStripSeparator,
            Me.Menu_Add_Package})
    End Sub

    Private Sub Save(
            ByVal sender As Object,
            ByVal e As EventArgs) Handles Menu_Save.Click
        Get_Top_Package(sender).Save()
    End Sub

    Private Sub Add_Package(
            ByVal sender As Object,
            ByVal e As EventArgs) Handles Menu_Add_Package.Click
        Get_Top_Package(sender).Add_Package()
    End Sub

End Class
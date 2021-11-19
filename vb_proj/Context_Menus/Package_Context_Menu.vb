Public Class Package_Context_Menu

    Inherits Element_Context_Menu

    Private WithEvents Menu_Add_Package As New ToolStripMenuItem("Add Package")

    Public Sub New()
        Me.Items.AddRange(New ToolStripItem() {
            Me.Menu_Edit,
            Me.Menu_View,
            Me.Menu_Remove,
            New ToolStripSeparator,
            Me.Menu_Add_Package})
    End Sub

    Private Sub Add_Package(
            ByVal sender As Object,
            ByVal e As EventArgs) Handles Menu_Add_Package.Click
        Dim pkg As Package = CType(Element_Context_Menu.Get_Selected_Element(sender), Package)
        pkg.Add_Package()
    End Sub

End Class

Public Class Diagram_Context_Menu
    Inherits Element_Context_Menu

    Protected WithEvents Menu_Show As New ToolStripMenuItem("Show")

    Public Sub New()
        Me.Items.AddRange(New ToolStripItem() {
            Me.Menu_Edit,
            Me.Menu_View,
            Me.Menu_Remove,
            Me.Menu_Show})
    End Sub

    Private Sub Show_Diagram(
        ByVal sender As Object,
        ByVal e As EventArgs) Handles Menu_Show.Click
        Dim diag As Diagram = CType(Element_Context_Menu.Get_Selected_Element(sender), Diagram)
        diag.Show()
    End Sub

End Class

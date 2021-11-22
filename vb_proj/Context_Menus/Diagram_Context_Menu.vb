Public Class Diagram_Context_Menu
    Inherits Element_Context_Menu

    Protected WithEvents Menu_Draw As New ToolStripMenuItem("Draw")

    Public Sub New()
        Me.Items.AddRange(New ToolStripItem() {
            Me.Menu_Edit,
            Me.Menu_View,
            Me.Menu_Remove,
            Me.Menu_Draw})
    End Sub

    Private Sub Draw(
        ByVal sender As Object,
        ByVal e As EventArgs) Handles Menu_Draw.Click
        Dim diag As Diagram = CType(Element_Context_Menu.Get_Selected_Element(sender), Diagram)
        diag.Draw()
    End Sub

End Class

Public Class Array_Type_Rectangle

    Public Top_Left_Corner_X_Pos As Integer
    Public Top_Left_Corner_Y_Pos As Integer
    Public Width As Integer
    Public Height As Integer
    Public Associated_Array_Type_Ref As Guid

    Public Sub Draw(ByVal graph As Graphics)
        graph.DrawRectangle(
            New Pen(Color.Green),
            Me.Top_Left_Corner_X_Pos,
            Me.Top_Left_Corner_Y_Pos,
            Me.Width,
            Me.Height)
    End Sub

End Class

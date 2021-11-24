Public Class Static_Physical_Architecture_Diagram
    Inherits Diagram

    Public Array_Type_Rectangle_List As List(Of Array_Type_Rectangle)

    Public Shared ReadOnly Metaclass_Name As String = "Diagram"

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
        Me.Array_Type_Rectangle_List = New List(Of Array_Type_Rectangle)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods from Software_Element
    ' -------------------------------------------------------------------------------------------- '

    Public Overrides Function Get_Metaclass_Name() As String
        Return Static_Physical_Architecture_Diagram.Metaclass_Name
    End Function


    ' -------------------------------------------------------------------------------------------- '
    ' 
    ' -------------------------------------------------------------------------------------------- '

    Public Overrides Sub Draw(ByVal graph As Graphics)
        MyBase.Draw(graph)
        For Each array_rect In Me.Array_Type_Rectangle_List
            array_rect.Draw(graph)
        Next

    End Sub

End Class

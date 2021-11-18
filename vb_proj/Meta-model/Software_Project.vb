﻿Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text

Public Class Software_Project

    Inherits Software_Element

    <XmlArrayItemAttribute(GetType(Package_Reference)), XmlArray("Packages_References")>
    Public Packages_References_List As List(Of Package_Reference)

    Private Xml_File_Path As String
    Private Top_Level_Packages_List As New List(Of Top_Level_Package)


    Public Shared ReadOnly Project_File_Extension As String = ".prjx"

    Private Shared Context_Menu As New Project_Context_Menu

    Private Shared Project_Serializer As New XmlSerializer(GetType(Software_Project))


    ' -------------------------------------------------------------------------------------------- '
    ' Constructors
    ' -------------------------------------------------------------------------------------------- '

    ' Default for deserialization
    Public Sub New()
    End Sub

    Public Sub New(
            name As String,
            desc As String,
            file_path As String,
            browser As TreeView)
        Me.Name = name
        Me.Description = desc
        Me.UUID = Guid.NewGuid()
        Me.Create_Node()
        browser.Nodes.Add(Me.Node)
        Me.Xml_File_Path = file_path
        Me.Packages_References_List = New List(Of Package_Reference)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods from Software_Element
    ' -------------------------------------------------------------------------------------------- '

    Protected Overrides Sub Create_Node()
        Me.Node = New TreeNode(Me.Name) With {
            .ImageKey = "Project",
            .SelectedImageKey = "Project",
            .ContextMenuStrip = Software_Project.Context_Menu,
            .Tag = Me}
    End Sub

    Protected Overrides Function Get_Children() As List(Of Software_Element)
        If IsNothing(Me.Children) Then
            Me.Children = New List(Of Software_Element)
            Me.Children.AddRange(Me.Top_Level_Packages_List)
        End If
        Return Me.Children
    End Function

    Public Overrides Function Is_Allowed_Parent(parent As Software_Element) As Boolean
        Return False
    End Function

    Protected Overrides Sub Move_Me(new_parent As Software_Element)
        ' Currently not needed, a package cannot be moved to a top level package
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Loading method
    ' -------------------------------------------------------------------------------------------- '

    Public Shared Function Load(
            project_file_path As String,
            browser As TreeView) As Software_Project

        Dim new_sw_proj As Software_Project = Nothing

        ' Deserialize xml file containing project data
        Dim reader As New XmlTextReader(project_file_path)
        Try
            new_sw_proj = CType(Software_Project.Project_Serializer.Deserialize(reader),
                Software_Project)
        Catch
            MsgBox("The project file is invalid !", MsgBoxStyle.Critical)
        End Try
        reader.Close()

        ' Check that deserialization is OK
        If Not IsNothing(new_sw_proj) Then

            ' Add project in browser
            new_sw_proj.Create_Node()
            browser.Nodes.Add(new_sw_proj.Node)

            ' Set or initialize private attributes
            new_sw_proj.Xml_File_Path = project_file_path

            ' Load the top level Packages aggregated by the project
            Environment.CurrentDirectory = Path.GetDirectoryName(project_file_path)
            For Each pkg_ref In new_sw_proj.Packages_References_List
                Dim new_pkg As Top_Level_Package = Nothing

                ' Compute full file path of the package to be loaded
                Dim new_pkg_path As String
                new_pkg_path = Path.GetFullPath(pkg_ref.Relative_Path)
                pkg_ref.Set_Full_Path(new_pkg_path)

                new_pkg = Top_Level_Package.Load(
                    pkg_ref.Last_Known_Name,
                    new_pkg_path,
                    new_sw_proj.Node,
                    pkg_ref.Is_Writable)

                If Not IsNothing(new_pkg) Then
                    new_sw_proj.Top_Level_Packages_List.Add(new_pkg)
                End If
            Next
        End If

        Return new_sw_proj
    End Function

    Public Sub Add_Predefined_Package()
        Top_Level_Package.Load_Basic_Types(Me.Node)
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods for contextual menu
    ' -------------------------------------------------------------------------------------------- '

    ' Need to be overloaded to manage "is modified" display
    Public Overloads Sub Edit()
        Dim prj_edit_form As New Edition_Form(Me.Name, Me.UUID.ToString, Me.Description)
        Dim edit_result As DialogResult
        edit_result = prj_edit_form.ShowDialog()
        If edit_result = DialogResult.OK Then
            Me.Name = prj_edit_form.Get_Name()
            Me.Node.Text = Me.Name
            Me.Description = prj_edit_form.Get_Description()
            Me.Display_Modified()
        End If
    End Sub

    Public Sub Save()

        ' Save top level packages
        For Each pkg In Me.Top_Level_Packages_List
            If pkg.Is_Writable() Then
                pkg.Save()
            End If
        Next

        ' Save Me
        Dim writer As New XmlTextWriter(Me.Xml_File_Path, Encoding.UTF8) With {
            .Indentation = 2,
            .IndentChar = " "c,
            .Formatting = Formatting.Indented}
        Software_Project.Project_Serializer.Serialize(writer, Me)
        writer.Close()

        ' Update model browser view
        Me.Display_Saved()

    End Sub

    Public Sub Load_Package(is_writable As Boolean)

        ' Display a form asking for package file
        Dim load_pkg_dialog = New OpenFileDialog With {
            .Title = "Select software model package file",
            .Filter = "Package file|*" & Top_Level_Package.Package_File_Extension,
            .CheckFileExists = True}
        Dim result As DialogResult = load_pkg_dialog.ShowDialog()

        ' Test the result from the form
        If result = DialogResult.OK Then
            ' The user has clicked on "Open"

            Dim pkg_file_path As String = load_pkg_dialog.FileName

            ' Check that the select file is not already loaded
            If Me.Is_Package_Loaded(pkg_file_path) Then
                ' File is already loaded
                ' Display an error message
                MsgBox("This package is already loaded", MsgBoxStyle.Exclamation)
            Else
                ' File is not already loaded
                ' Load the package from the file given by user
                Dim loaded_pkg As Top_Level_Package = Top_Level_Package.Load(
                    "temp",
                    pkg_file_path,
                    Me.Node,
                    is_writable)
                If Not IsNothing(loaded_pkg) Then
                    Me.Record_Package(loaded_pkg.Name, pkg_file_path, True)
                    Me.Top_Level_Packages_List.Add(loaded_pkg)
                    Me.Display_Modified()
                End If
            End If
        End If
    End Sub

    Public Sub Create_Package()

        Dim proposed_directory As String = Path.GetDirectoryName(Me.Xml_File_Path)

        Dim pkg_creation_form As New New_Recordable_Element_Form(
            "New_Package",
            "A good description is always useful.",
            proposed_directory,
            "New_Package",
            Top_Level_Package.Package_File_Extension,
            New_Recordable_Element_Form.Recordable_Element_Kind.PACKAGE)

        Dim creation_result As DialogResult = pkg_creation_form.ShowDialog()
        If creation_result = DialogResult.OK Then

            Dim pkg_file_path As String = pkg_creation_form.Get_File_Full_Path()

            ' Create the new package
            Dim created_pkg As Top_Level_Package = Nothing
            created_pkg = New Top_Level_Package(
                pkg_creation_form.Get_Name(),
                pkg_creation_form.Get_Description(),
                Me.Node,
                pkg_file_path)

            Me.Record_Package(created_pkg.Name, pkg_file_path, True)
            Me.Top_Level_Packages_List.Add(created_pkg)
            'created_pkg.Display_In_Browser(Me)

            Me.Display_Modified()

        End If
    End Sub

    Public Sub Remove_Package(pkg_name As String)
        ' Remove from packages list
        Dim pkg_idx As Integer = -1
        For Each pkg In Me.Top_Level_Packages_List
            If pkg.Name = pkg_name Then
                ' Remove node
                pkg.Delete()
                pkg_idx = Me.Top_Level_Packages_List.IndexOf(pkg)
                Me.Top_Level_Packages_List.Remove(pkg)
                Exit For
            End If
        Next

        ' Remove from package references list
        For Each ref In Me.Packages_References_List
            If ref.Last_Known_Name = pkg_name Then
                Packages_References_List.Remove(ref)
                Exit For
            End If
        Next

        Me.Display_Modified()

    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Private methods
    ' -------------------------------------------------------------------------------------------- '

    ' Create a new Package_Reference and add it to My list
    Private Sub Record_Package(pkg_name As String, pkg_file_path As String, is_writable As Boolean)
        Dim relative_pkg_path As String
        relative_pkg_path = Make_Relative_Path(Me.Xml_File_Path, pkg_file_path)
        Dim pkg_ref As New Package_Reference With {
            .Last_Known_Name = pkg_name,
            .Relative_Path = relative_pkg_path,
            .Is_Writable = is_writable}
        pkg_ref.Set_Full_Path(pkg_file_path)
        Me.Packages_References_List.Add(pkg_ref)
        Me.Save()
    End Sub

    Private Sub Display_Saved()
        Me.Node.Text = Me.Name
    End Sub

    Private Sub Display_Modified()
        Me.Node.Text = Me.Name & " *"
    End Sub

    Private Function Is_Package_Loaded(pkg_file_path As String) As Boolean
        Dim result As Boolean = False
        For Each ref In Me.Packages_References_List
            If ref.Get_Full_Path() = pkg_file_path Then
                result = True
                Exit For
            End If
        Next
        Return result
    End Function

End Class


Public Class Package_Reference

    Public Last_Known_Name As String
    Public Relative_Path As String
    Public Is_Writable As Boolean

    Private Full_Path As String

    Public Sub Set_Full_Path(path As String)
        Me.Full_Path = path
    End Sub

    Public Function Get_Full_Path() As String
        Return Me.Full_Path
    End Function

End Class

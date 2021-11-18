﻿Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Reflection

Public Class Top_Level_Package
    Inherits Package

    Private Enum E_PACKAGE_STATUS
        LOCKED
        NOT_FOUND
        CORRUPTED
        READABLE
        WRITABLE
    End Enum

    Private Xml_File_Path As String
    Private Status As E_PACKAGE_STATUS

    Private Shared Writable_Context_Menu As New Top_Package_Writable_Context_Menu
    Private Shared Readable_Context_Menu As New Top_Package_Readable_Context_Menu
    Private Shared Unloaded_Context_Menu As New Top_Package_Unloaded_Context_Menu


    Private Shared Pkg_Serializer As XmlSerializer = New XmlSerializer(GetType(Top_Level_Package))

    Public Shared ReadOnly Package_File_Extension As String = ".pkgx"


    ' -------------------------------------------------------------------------------------------- '
    ' Constructors
    ' -------------------------------------------------------------------------------------------- '

    Public Sub New()
    End Sub

    Public Sub New(
            name As String,
            description As String,
            parent_node As TreeNode,
            file_path As String)
        MyBase.New(name, description, Nothing, parent_node)
        Me.Xml_File_Path = file_path
        Me.UUID = Guid.NewGuid()
        Me.Status = E_PACKAGE_STATUS.WRITABLE
        Me.Packages = New List(Of Package)
        Me.Save()
    End Sub


    ' -------------------------------------------------------------------------------------------- '
    ' Methods from Software_Element
    ' -------------------------------------------------------------------------------------------- '

    Protected Overrides Sub Create_Node()
        MyBase.Create_Node()
        ' Set a specific contextual menu for top level packages (recordable file)
        ' Default choice as 3 kinds of ContextMenuStrip are defined.
        Me.Node.ContextMenuStrip = Top_Level_Package.Writable_Context_Menu
    End Sub

    Public Overrides Function Is_Allowed_Parent(parent As Software_Element) As Boolean
        Return False
    End Function

    ' -------------------------------------------------------------------------------------------- '
    ' Various method
    ' -------------------------------------------------------------------------------------------- '

    Public Shared Function Load(
            default_name As String, ' name to display if package not loaded
            file_path As String,
            parent_node As TreeNode,
            is_writable As Boolean) As Top_Level_Package

        Dim pkg As Top_Level_Package

        If Not File.Exists(file_path) Then
            pkg = Top_Level_Package.Create_Not_Found_Package(default_name, parent_node)
            MsgBox("Package file not found : " & vbCrLf & file_path,
                MsgBoxStyle.Critical)
        Else
            Dim reader As New XmlTextReader(file_path)
            Try
                pkg = CType(Top_Level_Package.Pkg_Serializer.Deserialize(reader), Top_Level_Package)
                With pkg
                    .Xml_File_Path = file_path
                End With
                If is_writable = True Then
                    pkg.Status = E_PACKAGE_STATUS.WRITABLE
                Else
                    pkg.Status = E_PACKAGE_STATUS.READABLE
                End If
                pkg.Post_Treat_After_Deserialization(parent_node)
                If is_writable = False Then
                    pkg.Node.ContextMenuStrip = Top_Level_Package.Readable_Context_Menu
                End If
            Catch
                pkg = Top_Level_Package.Create_Corrupted_Package(default_name, parent_node)
                MsgBox("Package file content is invalid : " & vbCrLf & file_path,
                    MsgBoxStyle.Critical)
            End Try
            reader.Close()
        End If

        pkg.Update_Display()

        Return pkg

    End Function

    Private Shared Function Create_Corrupted_Package(
            pkg_name As String,
            parent_node As TreeNode) As Top_Level_Package

        Dim pkg As New Top_Level_Package

        pkg.Create_Node()
        parent_node.Nodes.Add(pkg.Node)

        With pkg
            .Name = pkg_name
            .Node.ContextMenuStrip = Top_Level_Package.Unloaded_Context_Menu
            .Status = E_PACKAGE_STATUS.CORRUPTED
        End With

        Return pkg

    End Function

    Private Shared Function Create_Not_Found_Package(
            pkg_name As String,
            parent_node As TreeNode) As Top_Level_Package

        Dim pkg As New Top_Level_Package

        pkg.Create_Node()
        parent_node.Nodes.Add(pkg.Node)

        With pkg
            .Name = pkg_name
            .Node.ContextMenuStrip = Top_Level_Package.Unloaded_Context_Menu
            .Status = E_PACKAGE_STATUS.NOT_FOUND
        End With

        Return pkg

    End Function

    Public Shared Sub Load_Basic_Types(parent_node As TreeNode)
        Dim pkg As Top_Level_Package = Nothing
        Dim exe_assembly As Assembly = Assembly.GetExecutingAssembly()
        Dim ressource_name As String = "Embedded_Software_Model_Toolset.Basic_Types.pkgx"
        Dim file_stream As Stream = exe_assembly.GetManifestResourceStream(ressource_name)
        Dim reader As New XmlTextReader(file_stream)
        pkg = CType(Top_Level_Package.Pkg_Serializer.Deserialize(reader), Top_Level_Package)
        reader.Close()
        file_stream.Close()
        pkg.Post_Treat_After_Deserialization(parent_node)
        pkg.Status = E_PACKAGE_STATUS.LOCKED
        pkg.Update_Display()
    End Sub

    Private Sub Update_Display()
        Select Case Me.Status
            Case E_PACKAGE_STATUS.LOCKED
                Me.Node.Text = Me.Name & " (locked)"
            Case E_PACKAGE_STATUS.CORRUPTED
                Me.Node.Text = Me.Name & " (corrupted)"
            Case E_PACKAGE_STATUS.NOT_FOUND
                Me.Node.Text = Me.Name & " (not found)"
            Case E_PACKAGE_STATUS.READABLE
                Me.Node.Text = Me.Name & " (ref.)"
            Case E_PACKAGE_STATUS.WRITABLE
                Me.Node.Text = Me.Name
        End Select
    End Sub

    Public Sub Display_Saved()
        If Me.Status = E_PACKAGE_STATUS.WRITABLE Then
            Me.Node.Text = Me.Name
        Else
            Throw New System.Exception("Top level package should be writable.")
        End If
    End Sub

    Public Sub Display_Modified()
        If Me.Status = E_PACKAGE_STATUS.WRITABLE Then
            Me.Node.Text = Me.Name & " *"
        Else
            Throw New System.Exception("Top level package should be writable.")
        End If
    End Sub

    Public Function Is_Writable() As Boolean
        If Me.Status = E_PACKAGE_STATUS.WRITABLE Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub Delete()
        Me.Node.Remove()
    End Sub

    ' -------------------------------------------------------------------------------------------- '
    ' Methods for contextual menu
    ' -------------------------------------------------------------------------------------------- '

    Public Overrides Sub Edit()
        MyBase.Edit()
        ' TODO : modify last known name at project level
        Throw New System.Exception("TODO : modify last known name at project level")
    End Sub

    Public Sub Save()
        ' Initialize XML writer
        Dim writer As New XmlTextWriter(Me.Xml_File_Path, Encoding.UTF8) With {
            .Indentation = 2,
            .IndentChar = " "c,
            .Formatting = Formatting.Indented}

        ' Serialize Package
        Top_Level_Package.Pkg_Serializer.Serialize(writer, Me)

        ' Close writter
        writer.Close()

        Me.Display_Saved()
    End Sub

End Class
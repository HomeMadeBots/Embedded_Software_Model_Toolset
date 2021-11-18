Imports System.IO

Module Utilities

    Public Function Make_Relative_Path(from_path As String, to_path As String) As String
        Dim from_uri As Uri
        Dim to_uri As Uri
        Dim relative_path As String = to_path
        from_uri = New Uri(from_path)
        to_uri = New Uri(to_path)
        If from_uri.Scheme = to_uri.Scheme Then
            Dim relative_uri As Uri
            relative_uri = from_uri.MakeRelativeUri(to_uri)
            relative_path = Uri.UnescapeDataString(relative_uri.ToString())
            If to_uri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase) Then
                relative_path = relative_path.Replace(
                                Path.AltDirectorySeparatorChar,
                                Path.DirectorySeparatorChar)
            End If
        End If
        Return relative_path
    End Function

End Module

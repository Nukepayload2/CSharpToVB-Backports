' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Text

Partial Module StringExtensions
    <Extension>
    Public Function Replace(original As String, find As String, replacement As String, comparison As StringComparison) As String
        Select Case comparison
            Case StringComparison.Ordinal
                Return original.Replace(find, replacement)
            Case StringComparison.OrdinalIgnoreCase
                Return ReplaceOrdinalIgnoreCase(original, find, replacement)
            Case Else
                Throw New PlatformNotSupportedException(
                    $"Comparison `{comparison}` is not supported. Because `CompareInfo.IndexOf` doesn't return match length on this platform.")
        End Select
    End Function

    <ThreadStatic>
    Private t_stringBuilderCacheInReplace As StringBuilder

    ' The .NET Core runtime used ValueStringBuilder, which indicates that we need to reduce allocations here. 
    Private ReadOnly Property StringBuilderCacheInReplace As StringBuilder
        Get
            If t_stringBuilderCacheInReplace Is Nothing Then
                t_stringBuilderCacheInReplace = New StringBuilder
            End If
            Return t_stringBuilderCacheInReplace
        End Get
    End Property

    ' Copied something from https://github.com/dotnet/runtime/blob/57bfe474518ab5b7cfe6bf7424a79ce3af9d6657/src/libraries/System.Private.CoreLib/src/System/String.Manipulation.cs#L935
    Private Function ReplaceOrdinalIgnoreCase(original As String, find As String, replacement As String) As String
        Dim result = StringBuilderCacheInReplace
        result.Clear()

        Dim hasDoneAnyReplacements As Boolean = False

        Dim searchSpace As StringSegment = original
        Do
            Dim matchLength As Integer = find.Length
            Dim index As Integer = searchSpace.IndexOf(find, StringComparison.OrdinalIgnoreCase)

            ' There's the possibility that 'oldValue' has zero collation weight (empty string equivalent).
            ' If this is the case, we behave as if there are no more substitutions to be made.

            If index < 0 OrElse matchLength = 0 Then
                Exit Do
            End If

            ' append the unmodified portion of search space
            searchSpace.Slice(0, index).CopyToStringBuilder(result)

            ' append the replacement
            result.Append(replacement)

            searchSpace = searchSpace.Slice(index + matchLength)
            hasDoneAnyReplacements = True
        Loop

        ' Didn't find 'oldValue' in the remaining search space.

        If Not hasDoneAnyReplacements Then
            Return original
        End If

        ' Append what remains of the search space, then allocate the new string.
        searchSpace.CopyToStringBuilder(result)

        Return result.ToString()
    End Function

    <Extension>
    Public Function Contains(lookIn As String, find As String, comparion As StringComparison) As Boolean
        Return lookIn.IndexOf(find, comparion) >= 0
    End Function
End Module

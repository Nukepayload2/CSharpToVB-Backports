' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices

Public Module DictionaryExtension
    <Extension>
    Public Function TryAdd(Of TKey, TValue)(dict As Dictionary(Of TKey, TValue), key As TKey, value As TValue) As Boolean
        If Not dict.ContainsKey(key) Then
            dict(key) = value
            Return True
        End If
        Return False
    End Function
End Module

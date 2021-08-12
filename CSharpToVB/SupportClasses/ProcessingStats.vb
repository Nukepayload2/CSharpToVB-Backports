﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public NotInheritable Class ProcessingStats

    Public Sub New(lastFileNameWithPath As String)
        Me.LastFileNameWithPath = lastFileNameWithPath
        _elapsedTimer = New Diagnostics.Stopwatch
        _elapsedTimer.Start()
    End Sub

    Public ReadOnly _elapsedTimer As Diagnostics.Stopwatch
    Public Property FilesProcessed As Long
    Public Property LastFileNameWithPath As String
    Public Property TotalFilesToProcess As Long
End Class

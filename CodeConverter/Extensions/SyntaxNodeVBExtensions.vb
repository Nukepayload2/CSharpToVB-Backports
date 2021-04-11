﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports Microsoft.CodeAnalysis
Imports CS = Microsoft.CodeAnalysis.CSharp
Imports VB = Microsoft.CodeAnalysis.VisualBasic

Friend Module SyntaxNodeVbExtensions

    <Extension>
    Friend Function [With](Of T As VB.VisualBasicSyntaxNode)(node As T, leadingTrivia As IEnumerable(Of SyntaxTrivia), trailingTrivia As IEnumerable(Of SyntaxTrivia)) As T
        Return node.WithLeadingTrivia(leadingTrivia).WithTrailingTrivia(trailingTrivia)
    End Function

    <Extension>
    Friend Function WithUniqueLeadingTrivia(Of T As VB.VisualBasicSyntaxNode)(node As T, headerLeadingTrivia As SyntaxTriviaList) As T
        Dim nodeLeadingTrivia As SyntaxTriviaList = node.GetLeadingTrivia
        If nodeLeadingTrivia.Count = 0 Then
            Return node
        End If
        If nodeLeadingTrivia.First.Language = "C#" Then
            nodeLeadingTrivia = nodeLeadingTrivia.ConvertTriviaList
        End If
        If headerLeadingTrivia.Count = 0 Then
            Return node
        End If

        If Not nodeLeadingTrivia.ContainsCommentOrDirectiveTrivia Then
            Return node
        End If
        Dim index As Integer
        For index = 0 To headerLeadingTrivia.Count - 1
            If headerLeadingTrivia(index).RawKind <> nodeLeadingTrivia(index).RawKind Then
                Exit For
            End If
            If headerLeadingTrivia(index).ToString <> nodeLeadingTrivia(index).ToString Then
                Exit For
            End If
        Next
        Dim newLeadingTrivia As New SyntaxTriviaList
        For i As Integer = index To nodeLeadingTrivia.Count - 1
            If i <> 0 AndAlso i = index AndAlso nodeLeadingTrivia(i).IsKind(CS.SyntaxKind.EndOfLineTrivia) Then
                Continue For
            End If
            newLeadingTrivia = newLeadingTrivia.Add(nodeLeadingTrivia(i))
        Next
        Return node.WithLeadingTrivia(newLeadingTrivia)
    End Function

End Module

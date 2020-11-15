﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports CSharpToVBConverter.ToVisualBasic.CSharpConverter
Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.CSharp.Syntax

Imports CS = Microsoft.CodeAnalysis.CSharp
Imports Factory = Microsoft.CodeAnalysis.VisualBasic.SyntaxFactory
Imports VBS = Microsoft.CodeAnalysis.VisualBasic.Syntax

Namespace CSharpToVBConverter
    Friend Module BlockExtensions

        <Extension>
        Friend Function GetBodyStatements(block As BlockSyntax, visitor As MethodBodyVisitor) As SyntaxList(Of VBS.StatementSyntax)
            Dim statements As New List(Of VBS.StatementSyntax)
            For Each localFunction As LocalFunctionStatementSyntax In block.DescendantNodes().OfType(Of LocalFunctionStatementSyntax).ToList()
                Dim EmptyStatement As VBS.StatementSyntax = localFunction.Accept(visitor)(0)
                If EmptyStatement.GetLeadingTrivia.ContainsCommentOrDirectiveTrivia OrElse
                EmptyStatement.GetTrailingTrivia.ContainsCommentOrDirectiveTrivia Then
                    statements.Add(EmptyStatement)
                End If
            Next
            For Each s As StatementSyntax In block.statements
                If s.IsKind(CS.SyntaxKind.LocalFunctionStatement) Then
                    Continue For
                End If
                statements.AddRange(s.Accept(visitor))
                If statements.Any Then
                    If block.OpenBraceToken.leadingTrivia.ContainsCommentOrDirectiveTrivia Then
                        statements(0) = statements(0).WithPrependedLeadingTrivia(block.OpenBraceToken.leadingTrivia.ConvertTriviaList())
                    End If
                End If
            Next

            Return Factory.List(statements)
        End Function

    End Module
End Namespace

﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.IO
Imports System.Reflection
Imports System.Threading
Imports CSharpToVBApp

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Emit
Imports SupportClasses

Imports Xunit

Namespace Tests.ConvertDirectories

    Public Module TestSupport
        Friend LastFileProcessed As String

        ''' <summary>
        ''' Return False to skip test
        ''' </summary>
        Public ReadOnly Property EnableRoslynTests() As Boolean
            Get
                Return Directory.Exists(GetRoslynRootDirectory) AndAlso (Debugger.IsAttached OrElse Environment.GetEnvironmentVariable("EnableRoslynTests") IsNot Nothing)
            End Get
        End Property

        Friend Function TestProcessFileAsync(mainForm As Form1, pathWithFileName As String, targetDirectory As String, _0 As String, csPreprocessorSymbols As List(Of String), vbPreprocessorSymbols As List(Of KeyValuePair(Of String, Object)), optionalReferences() As MetadataReference, skipAutoGenerated As Boolean, cancelToken As CancellationToken) As Task(Of Boolean)
            ' Save to TargetDirectory not supported
            Assert.True(String.IsNullOrWhiteSpace(targetDirectory))
            ' Do not delete the next line or the parameter it is needed by other versions of this routine
            LastFileProcessed = pathWithFileName
            Using fs As FileStream = File.OpenRead(pathWithFileName)
                Dim requestToConvert As ConvertRequest = New ConvertRequest(mSkipAutoGenerated:=True, mProgress:=Nothing, mCancelToken:=cancelToken) With {
                    .SourceCode = fs.GetFileTextFromStream()
                }

                Dim resultOfConversion As ConversionResult = Utilities.ConvertInputRequest(requestToConvert, New DefaultVbOptions, csPreprocessorSymbols, vbPreprocessorSymbols, Utilities.CSharpReferences(Assembly.Load("System.Windows.Forms").Location, optionalReferences).ToArray, reportException:=Nothing, mProgress:=Nothing, cancelToken:=CancellationToken.None)
                If resultOfConversion.ResultStatus = ConversionResult.ResultTriState.Failure Then
                    Return Task.FromResult(False)
                End If
                Dim compileResult As (CompileSuccess As Boolean, EmitResult As EmitResult) = CompileVisualBasicString(stringToBeCompiled:=resultOfConversion.ConvertedCode, vbPreprocessorSymbols, DiagnosticSeverity.Error, resultOfConversion)
                If Not compileResult.CompileSuccess OrElse resultOfConversion.GetFilteredListOfFailures().Any Then
                    Dim msg As String = If(compileResult.CompileSuccess, resultOfConversion.GetFilteredListOfFailures()(0).GetMessage, "Fatal Compile error")
                    Throw New ApplicationException($"{pathWithFileName} failed to compile with error :{vbCrLf}{msg}")
                    Return Task.FromResult(False)
                End If
            End Using
            Return Task.FromResult(True)
        End Function

        Public Async Function TestProcessDirectoryAsync(sourceDirectory As String) As Task(Of Boolean)
            Return Await ProcessDirectoryAsync(mainForm:=Nothing, sourceDirectory, targetDirectory:="", stopButton:=Nothing, listBoxFileList:=Nothing, sourceLanguageExtension:="cs", New ProcessingStats(""), AddressOf TestProcessFileAsync, CancellationToken.None).ConfigureAwait(continueOnCapturedContext:=True)
        End Function

    End Module
End Namespace

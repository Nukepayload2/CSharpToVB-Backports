' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Runtime.CompilerServices
Imports System.Text

Friend Structure StringSegment
    Private ReadOnly _reference As String
    Private ReadOnly _start As Integer

    Public ReadOnly Length As Integer

    Public Sub New(reference As String, start As Integer, length As Integer)
        _reference = reference
        _start = start
        Me.Length = length
    End Sub

    Default Public ReadOnly Property Item(index As Integer) As Char
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return _reference(_start + index)
        End Get
    End Property

    Public Shared Function Slice(str As String, start As Integer, length As Integer) As StringSegment
        Dim seg As New StringSegment(str, start, length)
        Return seg
    End Function

    Public Function Slice(start As Integer, length As Integer) As StringSegment
        Dim seg As New StringSegment(_reference, start + _start, length)
        Return seg
    End Function

    Public Function Slice(start As Integer) As StringSegment
        Dim seg As New StringSegment(_reference, start + _start, Length - start)
        Return seg
    End Function

    ''' <summary>
    ''' Devirtualized version of <see cref="ToString()"/>.
    ''' </summary>
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function CopyToString() As String
        If _reference Is Nothing Then
            Return Nothing
        End If
        Return _reference.Substring(_start, Length)
    End Function

    Public Sub CopyToStringBuilder(sb As StringBuilder)
        If Length < 1 Then
            Return
        End If
        sb.Append(_reference, _start, Length)
    End Sub

    Public Shared Widening Operator CType(str As String) As StringSegment
        Return New StringSegment(str, 0, str.Length)
    End Operator

    Public Shared Widening Operator CType(str As StringSegment) As String
        Return str.CopyToString()
    End Operator

    Public ReadOnly Property IsNull As Boolean
        Get
            Return _reference Is Nothing
        End Get
    End Property

    Public Function IndexOf(find As String, comparison As StringComparison) As Integer
        Return _reference.IndexOf(find, _start, Length, comparison)
    End Function

    Public ReadOnly Property IsNullOrEmpty As Boolean
        Get
            Return Length = 0
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Me.CopyToString()
    End Function

    Public Shared ReadOnly Empty As New StringSegment(String.Empty, 0, 0)
End Structure

﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Imports System.Diagnostics
Imports System.Runtime.CompilerServices
Imports Microsoft.Win32

Friend Module TargetFrameworkUtilities

    ' Checking the version using >= will enable forward compatibility.
    Private Function CheckFor45PlusVersion(releaseKey As Integer) As String
        If releaseKey >= 528040 Then
            Return "NET48"
        ElseIf releaseKey >= 461808 Then
            Return "NET472"
        ElseIf releaseKey >= 461308 Then
            Return "NET471"
        ElseIf releaseKey >= 460798 Then
            Return "NET47"
        ElseIf releaseKey >= 394802 Then
            Return "NET462"
        ElseIf releaseKey >= 394254 Then
            Return "NET461"
        ElseIf releaseKey >= 393295 Then
            Return "NET46"
        ElseIf releaseKey >= 379893 Then
            Return "NET452"
        ElseIf releaseKey >= 378675 Then
            Return "NET451"
        ElseIf releaseKey >= 378389 Then
            Return "NET45"
        End If
        ' This code should never execute. A non-null release key should mean
        ' that 4.5 or later is installed.
        Return "No 4.5 or later version detected"
    End Function

    Private Function MapNameToFramework(base As String, separator As String, name As String) As String
        Dim nameSplit() As String = name.Split(".")
        Dim minor As String = nameSplit(1)
        Return $"{If(CInt(nameSplit(0)) >= 5, "NET", base) }{nameSplit(0)}{separator}{minor}"
    End Function

    <Extension>
    Friend Sub AddDropDownMenuItem(dropDownItems As ToolStripItemCollection, itemName As String)
        dropDownItems.Add(New ToolStripMenuItem With {
            .AutoSize = True,
            .CheckOnClick = True,
            .ImageScaling = ToolStripItemImageScaling.None,
            .Name = $"{itemName}ToolStripMenuItem",
            .Text = itemName
        })
    End Sub

    ''' <summary>
    ''' Converts a framework name from Project File format to Compile time variable
    ''' </summary>
    ''' <param name="framework"></param>
    ''' <returns></returns>
    Friend Function FrameworkNameToConstant(framework As String) As String
        If framework = "netcoreapp5.0" Then
            Return "NET5_0"
        End If
        Return framework.ToUpperInvariant.Replace(".", "_", StringComparison.OrdinalIgnoreCase)
    End Function

    Friend Function GetAllCoreVersions() As List(Of String)
        Dim versions As New List(Of String)
        Dim dotnetVersions As String() = RunCommand("Dotnet", "--list-sdks", showWindow:=False)
        For Each e As String In dotnetVersions
            Dim item As String = MapNameToFramework("NETCOREAPP", separator:="_", e.Split(" ")(0))
            If versions.Contains(item) Then
                Continue For
            End If
            versions.Add(item)
        Next
        Return versions
    End Function

    Friend Function GetAllFrameworkVersions() As List(Of String)
        Dim versions As New List(Of String)
        ' Opens the registry key for the .NET Framework entry.
        Using baseKey As RegistryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
            Using ndpKey As RegistryKey = baseKey.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\")
                For Each versionKeyName As String In ndpKey.GetSubKeyNames()
                    ' Skip .NET Framework 4.5 and later.
                    If versionKeyName = "v4" Then
                        Continue For
                    End If

                    If versionKeyName.StartsWith("v", StringComparison.OrdinalIgnoreCase) Then
                        Dim versionKey As RegistryKey = ndpKey.OpenSubKey(versionKeyName)
                        ' Get the .NET Framework version value.
                        Dim name As String = DirectCast(versionKey.GetValue("Version", ""), String)
                        ' Get the service pack (SP) number.
                        Dim sp As String = versionKey.GetValue("SP", "").ToString()

                        If Not String.IsNullOrEmpty(name) Then
                            versions.Add(MapNameToFramework("NET", separator:="", name))
                            Continue For
                        End If
                        For Each subKeyName As String In versionKey.GetSubKeyNames()
                            Dim subKey As RegistryKey = versionKey.OpenSubKey(subKeyName)
                            name = DirectCast(subKey.GetValue("Version", ""), String)
                            If Not String.IsNullOrEmpty(name) Then
                                sp = subKey.GetValue("SP", "").ToString()
                            End If
                            Dim install As String = subKey.GetValue("Install", "").ToString()
                            If String.IsNullOrEmpty(install) Then  ' No install info; it must be later.
                                versions.Add(MapNameToFramework("NET", separator:="", name))
                            Else
                                If Not String.IsNullOrEmpty(sp) AndAlso install = "1" Then
                                    versions.Add(MapNameToFramework("NET", separator:="", name))
                                ElseIf install = "1" Then
                                    versions.Add(MapNameToFramework("NET", separator:="", name))
                                End If
                            End If
                        Next
                    End If
                Next
            End Using
            Using ndpKey As RegistryKey = baseKey.OpenSubKey("SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\")
                If ndpKey IsNot Nothing AndAlso ndpKey.GetValue("Release") IsNot Nothing Then
                    versions.Add(CheckFor45PlusVersion(CInt(ndpKey.GetValue("Release"))))
                End If
            End Using
        End Using
        Return versions
    End Function

    ''' <summary>
    ''' Run a Windows command and return pipes text result to caller
    ''' </summary>
    ''' <param name="command"></param>
    ''' <param name="args"></param>
    ''' <param name="showWindow"></param>
    ''' <returns>Array of text lines returned by Command</returns>
    Friend Function RunCommand(command As String, args As String, Optional showWindow As Boolean = True) As String()
        Dim oProcess As New Process()
        Dim oStartInfo As New ProcessStartInfo(command, args) With {
            .CreateNoWindow = Not showWindow,
            .RedirectStandardOutput = True,
            .UseShellExecute = False
        }
        oProcess.StartInfo = oStartInfo
        oProcess.Start()

        Dim sOutput As String
        Using oStreamReader As IO.StreamReader = oProcess.StandardOutput
            sOutput = oStreamReader.ReadToEnd()
        End Using
        oProcess.Dispose()
        Return sOutput.SplitLines
    End Function

End Module

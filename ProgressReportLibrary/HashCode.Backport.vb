' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.

Public Structure HashCode
    Public Shared Function Combine(num1 As Integer, num2 As Integer) As Integer
        Return num1 Xor num2
    End Function
End Structure

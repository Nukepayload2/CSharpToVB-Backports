﻿' Licensed to the .NET Foundation under one or more agreements.
' The .NET Foundation licenses this file to you under the MIT license.
' See the LICENSE file in the project root for more information.
Imports System.IO
Imports System.Xml
Imports CSharpToVBApp
Imports Xunit

Namespace Tests.ProjectFile

    <TestClass()> Public NotInheritable Class SdkFileConvertTest

        <Fact>
        Public Shared Sub ConvertClassProjectFileTest()

            Dim originalProjectFile As XElement =
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <AssemblyName>System.Windows.Forms</AssemblyName>
        <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
        <CLSCompliant>true</CLSCompliant>
        <Nullable>enable</Nullable>

        <NoWarn>$(NoWarn);618</NoWarn>
        <DefineConstants>$(DefineConstants);OPTIMIZED_MEASUREMENTDC;</DefineConstants>
        <Win32Manifest>Resources\System\Windows\Forms\XPThemes.manifest</Win32Manifest>
        <Deterministic>true</Deterministic>
        <ProduceReferenceAssembly>true</ProduceReferenceAssembly>
        <UsePublicApiAnalyzers>true</UsePublicApiAnalyzers>
    </PropertyGroup>

    <ItemGroup>
        <ProjectReference Include="..\..\System.Windows.Forms.Primitives\src\System.Windows.Forms.Primitives.csproj"/>
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.Win32.Registry" Version="$(MicrosoftWin32RegistryPackageVersion)"/>
    </ItemGroup>

    <ItemGroup>
        <Compile Include="..\..\Common\src\RTLAwareMessageBox.cs" Link="Common\RTLAwareMessageBox.cs"/>
    </ItemGroup>

    <ItemGroup>
        <EmbeddedResource Update="Resources\SR.resx">
            <GenerateSource>true</GenerateSource>
            <Namespace>System</Namespace>
        </EmbeddedResource>
    </ItemGroup>

    <ItemGroup>
        <EmbeddedResource Include="Resources\System\Windows\Forms\Animation.ico">
            <LogicalName>System.Windows.Forms.Animation</LogicalName>
        </EmbeddedResource>
    </ItemGroup>
</Project>

            Dim expectedProjectFile As XElement =
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <AssemblyName>System.Windows.Forms</AssemblyName>
        <CLSCompliant>true</CLSCompliant>
        <Nullable>enable</Nullable>

        <DefineConstants>$(DefineConstants);OPTIMIZED_MEASUREMENTDC;</DefineConstants>
        <Win32Manifest>Resources\System\Windows\Forms\XPThemes.manifest</Win32Manifest>
        <Deterministic>true</Deterministic>
        <ProduceReferenceAssembly>true</ProduceReferenceAssembly>
        <UsePublicApiAnalyzers>true</UsePublicApiAnalyzers>
    </PropertyGroup>

    <ItemGroup>
        <ProjectReference Include="..\..\System.Windows.Forms.Primitives\src\System.Windows.Forms.Primitives.vbproj"/>
    </ItemGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.Win32.Registry" Version="$(MicrosoftWin32RegistryPackageVersion)"/>
    </ItemGroup>

    <ItemGroup>
        <Compile Include="..\..\Common\src\RTLAwareMessageBox.vb" Link="Common\RTLAwareMessageBox.vb"/>
    </ItemGroup>

    <ItemGroup>
        <EmbeddedResource Update="Resources\SR.resx">
            <GenerateSource>true</GenerateSource>
            <Namespace>System</Namespace>
        </EmbeddedResource>
    </ItemGroup>

    <ItemGroup>
        <EmbeddedResource Include="Resources\System\Windows\Forms\Animation.ico">
            <LogicalName>System.Windows.Forms.Animation</LogicalName>
        </EmbeddedResource>
    </ItemGroup>
</Project>
            Dim sourceXmlDoc As New XmlDocument With {
                                .PreserveWhitespace = True
                            }
            sourceXmlDoc.LoadXml(originalProjectFile.ToString)
            Dim tempDirectory As String = Path.GetTempPath() & "Test" & Guid.NewGuid().ToString()
            Dim sourceDirectory As String = Path.Combine(tempDirectory, "Source")
            Dim destinationDirectory As String = Path.Combine(tempDirectory, "Source_vb")
            Dim originalProjectFileName As String = sourceDirectory & Path.DirectorySeparatorChar & "Test.csproj"
            Dim destinationProjectFileName As String = destinationDirectory & Path.DirectorySeparatorChar & "Test.vbproj"
            Try
                Directory.CreateDirectory(tempDirectory)
                Directory.CreateDirectory(sourceDirectory)
                Directory.CreateDirectory(destinationDirectory)
                sourceXmlDoc.Save(originalProjectFileName)
                Assert.False(ConvertProjectFile(originalProjectFileName, destinationDirectory).Any)
                Dim resultXmlDoc As String = File.ReadAllText(destinationProjectFileName)
                Assert.Equal(expectedProjectFile.ToString, resultXmlDoc)
            Finally
                If Directory.Exists(tempDirectory) Then
                    Directory.Delete(tempDirectory, recursive:=True)
                End If
            End Try
        End Sub

        <Fact>
        Public Shared Sub ConvertSolutionFileTest()
            Dim originalSolutionFile As String =
                "Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 16
VisualStudioVersion = 16.0.28503.202
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""RoslynDeployment"", ""src\Deployment\RoslynDeployment.csproj"", ""{600AF682-E097-407B-AD85-EE3CED37E680}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Core"", ""Core"", ""{A41D1B99-F489-4C43-BBDF-96D61B19A6B9}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Compilers"", ""Compilers"", ""{3F40F71B-7DCF-44A1-B15C-38CA34824143}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""CSharp"", ""CSharp"", ""{32A48625-F0AD-419D-828B-A50BDABA38EA}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""VisualBasic"", ""VisualBasic"", ""{C65C6143-BED3-46E6-869E-9F0BE6E84C37}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Workspaces"", ""Workspaces"", ""{55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Tools"", ""Tools"", ""{FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""EditorFeatures"", ""EditorFeatures"", ""{EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""ExpressionEvaluator"", ""ExpressionEvaluator"", ""{235A3418-A3B0-4844-BCEB-F1CF45069232}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Features"", ""Features"", ""{3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Interactive"", ""Interactive"", ""{999FBDA2-33DA-4F74-B957-03AC72CCE5EC}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Scripting"", ""Scripting"", ""{38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""VisualStudio"", ""VisualStudio"", ""{8DBA5174-B0AA-4561-82B1-A46607697753}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""CSharp"", ""CSharp"", ""{913A4C08-898E-49C7-9692-0EF9DC56CF6E}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""VisualBasic"", ""VisualBasic"", ""{151F6994-AEB3-4B12-B746-2ACFF26C7BBB}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Setup"", ""Setup"", ""{4C81EBB2-82E1-4C81-80C4-84CC40FA281B}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Core"", ""Core"", ""{998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Setup"", ""Setup"", ""{19148439-436F-4CDA-B493-70AF4FFC13E9}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Hosts"", ""Hosts"", ""{5CA5F70E-0FDB-467B-B22C-3CD5994F0087}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Server"", ""Server"", ""{7E907718-0B33-45C8-851F-396CEFDC1AB6}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Test"", ""Test"", ""{CAD2965A-19AB-489F-BE2E-7649957F914A}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""IntegrationTest"", ""IntegrationTest"", ""{CC126D03-7EAC-493F-B187-DCDEE1EF6A70}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Dependencies"", ""Dependencies"", ""{C2D1346B-9665-4150-B644-075CF1636BAA}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Perf"", ""Perf"", ""{DD13507E-D5AF-4B61-B11A-D55D6F4A73A5}""
	ProjectSection(SolutionItems) = preProject
		src\Test\Perf\readme.md = src\Test\Perf\readme.md
	EndProjectSection
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""CodeStyle"", ""CodeStyle"", ""{DC014586-8D07-4DE6-B28E-C0540C59C085}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.UnitTests"", ""src\Compilers\Core\CodeAnalysisTest\Microsoft.CodeAnalysis.UnitTests.csproj"", ""{A4C99B85-765C-4C65-9C2A-BB609AAB09E6}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis"", ""src\Compilers\Core\Portable\Microsoft.CodeAnalysis.csproj"", ""{1EE8CAD3-55F9-4D91-96B2-084641DA9A6C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""VBCSCompiler"", ""src\Compilers\Server\VBCSCompiler\VBCSCompiler.csproj"", ""{9508F118-F62E-4C16-A6F4-7C3B56E166AD}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""VBCSCompiler.UnitTests"", ""src\Compilers\Server\VBCSCompilerTests\VBCSCompiler.UnitTests.csproj"", ""{F5CE416E-B906-41D2-80B9-0078E887A3F6}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""csc"", ""src\Compilers\CSharp\csc\csc.csproj"", ""{4B45CA0C-03A0-400F-B454-3D4BCB16AF38}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp"", ""src\Compilers\CSharp\Portable\Microsoft.CodeAnalysis.CSharp.csproj"", ""{B501A547-C911-4A05-AC6E-274A50DFF30E}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.CommandLine.UnitTests"", ""src\Compilers\CSharp\Test\CommandLine\Microsoft.CodeAnalysis.CSharp.CommandLine.UnitTests.csproj"", ""{50D26304-0961-4A51-ABF6-6CAD1A56D203}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Emit.UnitTests"", ""src\Compilers\CSharp\Test\Emit\Microsoft.CodeAnalysis.CSharp.Emit.UnitTests.csproj"", ""{4462B57A-7245-4146-B504-D46FDE762948}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.IOperation.UnitTests"", ""src\Compilers\CSharp\Test\IOperation\Microsoft.CodeAnalysis.CSharp.IOperation.UnitTests.csproj"", ""{1AF3672A-C5F1-4604-B6AB-D98C4DE9C3B1}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Semantic.UnitTests"", ""src\Compilers\CSharp\Test\Semantic\Microsoft.CodeAnalysis.CSharp.Semantic.UnitTests.csproj"", ""{B2C33A93-DB30-4099-903E-77D75C4C3F45}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Symbol.UnitTests"", ""src\Compilers\CSharp\Test\Symbol\Microsoft.CodeAnalysis.CSharp.Symbol.UnitTests.csproj"", ""{28026D16-EB0C-40B0-BDA7-11CAA2B97CCC}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Syntax.UnitTests"", ""src\Compilers\CSharp\Test\Syntax\Microsoft.CodeAnalysis.CSharp.Syntax.UnitTests.csproj"", ""{50D26304-0961-4A51-ABF6-6CAD1A56D202}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Compiler.Test.Resources"", ""src\Compilers\Test\Resources\Core\Microsoft.CodeAnalysis.Compiler.Test.Resources.csproj"", ""{7FE6B002-89D8-4298-9B1B-0B5C247DD1FD}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Test.Utilities"", ""src\Compilers\Test\Utilities\CSharp\Microsoft.CodeAnalysis.CSharp.Test.Utilities.csproj"", ""{4371944A-D3BA-4B5B-8285-82E5FFC6D1F9}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Test.Utilities"", ""src\Compilers\Test\Utilities\VisualBasic\Microsoft.CodeAnalysis.VisualBasic.Test.Utilities.vbproj"", ""{4371944A-D3BA-4B5B-8285-82E5FFC6D1F8}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic"", ""src\Compilers\VisualBasic\Portable\Microsoft.CodeAnalysis.VisualBasic.vbproj"", ""{2523D0E6-DF32-4A3E-8AE0-A19BFFAE2EF6}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.CommandLine.UnitTests"", ""src\Compilers\VisualBasic\Test\CommandLine\Microsoft.CodeAnalysis.VisualBasic.CommandLine.UnitTests.vbproj"", ""{E3B32027-3362-41DF-9172-4D3B623F42A5}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Emit.UnitTests"", ""src\Compilers\VisualBasic\Test\Emit\Microsoft.CodeAnalysis.VisualBasic.Emit.UnitTests.vbproj"", ""{190CE348-596E-435A-9E5B-12A689F9FC29}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Roslyn.Compilers.VisualBasic.IOperation.UnitTests"", ""src\Compilers\VisualBasic\Test\IOperation\Roslyn.Compilers.VisualBasic.IOperation.UnitTests.vbproj"", ""{9C9DABA4-0E72-4469-ADF1-4991F3CA572A}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Semantic.UnitTests"", ""src\Compilers\VisualBasic\Test\Semantic\Microsoft.CodeAnalysis.VisualBasic.Semantic.UnitTests.vbproj"", ""{BF180BD2-4FB7-4252-A7EC-A00E0C7A028A}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Symbol.UnitTests"", ""src\Compilers\VisualBasic\Test\Symbol\Microsoft.CodeAnalysis.VisualBasic.Symbol.UnitTests.vbproj"", ""{BDA5D613-596D-4B61-837C-63554151C8F5}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Syntax.UnitTests"", ""src\Compilers\VisualBasic\Test\Syntax\Microsoft.CodeAnalysis.VisualBasic.Syntax.UnitTests.vbproj"", ""{91F6F646-4F6E-449A-9AB4-2986348F329D}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.Test.PdbUtilities"", ""src\Test\PdbUtilities\Roslyn.Test.PdbUtilities.csproj"", ""{AFDE6BEA-5038-4A4A-A88E-DBD2E4088EED}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Workspaces"", ""src\Workspaces\Core\Portable\Microsoft.CodeAnalysis.Workspaces.csproj"", ""{5F8D2414-064A-4B3A-9B42-8E2A04246BE5}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""CompilersBoundTreeGenerator"", ""src\Tools\Source\CompilerGeneratorTools\Source\BoundTreeGenerator\CompilersBoundTreeGenerator.csproj"", ""{02459936-CD2C-4F61-B671-5C518F2A3DDC}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""CSharpErrorFactsGenerator"", ""src\Tools\Source\CompilerGeneratorTools\Source\CSharpErrorFactsGenerator\CSharpErrorFactsGenerator.csproj"", ""{288089C5-8721-458E-BE3E-78990DAB5E2E}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""CSharpSyntaxGenerator"", ""src\Tools\Source\CompilerGeneratorTools\Source\CSharpSyntaxGenerator\CSharpSyntaxGenerator.csproj"", ""{288089C5-8721-458E-BE3E-78990DAB5E2D}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""VisualBasicSyntaxGenerator"", ""src\Tools\Source\CompilerGeneratorTools\Source\VisualBasicSyntaxGenerator\VisualBasicSyntaxGenerator.vbproj"", ""{6AA96934-D6B7-4CC8-990D-DB6B9DD56E34}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Workspaces.UnitTests"", ""src\Workspaces\CoreTest\Microsoft.CodeAnalysis.Workspaces.UnitTests.csproj"", ""{C50166F1-BABC-40A9-95EB-8200080CD701}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Workspaces.UnitTests"", ""src\Workspaces\CSharpTest\Microsoft.CodeAnalysis.CSharp.Workspaces.UnitTests.csproj"", ""{E195A63F-B5A4-4C5A-96BD-8E7ED6A181B7}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Workspaces.UnitTests"", ""src\Workspaces\VisualBasicTest\Microsoft.CodeAnalysis.VisualBasic.Workspaces.UnitTests.vbproj"", ""{E3FDC65F-568D-4E2D-A093-5132FD3793B7}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""VisualBasicErrorFactsGenerator"", ""src\Tools\Source\CompilerGeneratorTools\Source\VisualBasicErrorFactsGenerator\VisualBasicErrorFactsGenerator.vbproj"", ""{909B656F-6095-4AC2-A5AB-C3F032315C45}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Workspaces.Desktop"", ""src\Workspaces\Core\Desktop\Microsoft.CodeAnalysis.Workspaces.Desktop.csproj"", ""{2E87FA96-50BB-4607-8676-46521599F998}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Workspaces.MSBuild"", ""src\Workspaces\Core\MSBuild\Microsoft.CodeAnalysis.Workspaces.MSBuild.csproj"", ""{96EB2D3B-F694-48C6-A284-67382841E086}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Workspaces"", ""src\Workspaces\CSharp\Portable\Microsoft.CodeAnalysis.CSharp.Workspaces.csproj"", ""{21B239D0-D144-430F-A394-C066D58EE267}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Workspaces"", ""src\Workspaces\VisualBasic\Portable\Microsoft.CodeAnalysis.VisualBasic.Workspaces.vbproj"", ""{57CA988D-F010-4BF2-9A2E-07D6DCD2FF2C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""RunTests"", ""src\Tools\Source\RunTests\RunTests.csproj"", ""{1A3941F1-1E1F-4EF7-8064-7729C4C2E2AA}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Features"", ""src\Features\VisualBasic\Portable\Microsoft.CodeAnalysis.VisualBasic.Features.vbproj"", ""{A1BCD0CE-6C2F-4F8C-9A48-D9D93928E26D}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Features"", ""src\Features\CSharp\Portable\Microsoft.CodeAnalysis.CSharp.Features.csproj"", ""{3973B09A-4FBF-44A5-8359-3D22CEB71F71}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Features"", ""src\Features\Core\Portable\Microsoft.CodeAnalysis.Features.csproj"", ""{EDC68A0E-C68D-4A74-91B7-BF38EC909888}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.EditorFeatures.Text"", ""src\EditorFeatures\Text\Microsoft.CodeAnalysis.EditorFeatures.Text.csproj"", ""{18F5FBB8-7570-4412-8CC7-0A86FF13B7BA}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.EditorFeatures"", ""src\EditorFeatures\VisualBasic\Microsoft.CodeAnalysis.VisualBasic.EditorFeatures.vbproj"", ""{49BFAE50-1BCE-48AE-BC89-78B7D90A3ECD}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.EditorFeatures"", ""src\EditorFeatures\CSharp\Microsoft.CodeAnalysis.CSharp.EditorFeatures.csproj"", ""{B0CE9307-FFDB-4838-A5EC-CE1F7CDC4AC2}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.EditorFeatures"", ""src\EditorFeatures\Core\Microsoft.CodeAnalysis.EditorFeatures.csproj"", ""{3CDEEAB7-2256-418A-BEB2-620B5CB16302}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.EditorFeatures.UnitTests"", ""src\EditorFeatures\VisualBasicTest\Microsoft.CodeAnalysis.VisualBasic.EditorFeatures.UnitTests.vbproj"", ""{0BE66736-CDAA-4989-88B1-B3F46EBDCA4A}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Scripting"", ""src\Scripting\VisualBasic\Microsoft.CodeAnalysis.VisualBasic.Scripting.vbproj"", ""{3E7DEA65-317B-4F43-A25D-62F18D96CFD7}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Scripting"", ""src\Scripting\Core\Microsoft.CodeAnalysis.Scripting.csproj"", ""{12A68549-4E8C-42D6-8703-A09335F97997}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Scripting.UnitTests"", ""src\Scripting\CoreTest\Microsoft.CodeAnalysis.Scripting.UnitTests.csproj"", ""{2DAE4406-7A89-4B5F-95C3-BC5472CE47CE}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Scripting"", ""src\Scripting\CSharp\Microsoft.CodeAnalysis.CSharp.Scripting.csproj"", ""{066F0DBD-C46C-4C20-AFEC-99829A172625}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Scripting.UnitTests"", ""src\Scripting\CSharpTest\Microsoft.CodeAnalysis.CSharp.Scripting.UnitTests.csproj"", ""{2DAE4406-7A89-4B5F-95C3-BC5422CE47CE}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.InteractiveHost"", ""src\Interactive\Host\Microsoft.CodeAnalysis.InteractiveHost.csproj"", ""{8E2A252E-A140-45A6-A81A-2652996EA589}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.EditorFeatures.UnitTests"", ""src\EditorFeatures\CSharpTest\Microsoft.CodeAnalysis.CSharp.EditorFeatures.UnitTests.csproj"", ""{AC2BCEFB-9298-4621-AC48-1FF5E639E48D}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.EditorFeatures2.UnitTests"", ""src\EditorFeatures\CSharpTest2\Microsoft.CodeAnalysis.CSharp.EditorFeatures2.UnitTests.csproj"", ""{16E93074-4252-466C-89A3-3B905ABAF779}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.EditorFeatures.UnitTests"", ""src\EditorFeatures\Test\Microsoft.CodeAnalysis.EditorFeatures.UnitTests.csproj"", ""{8CEE3609-A5A9-4A9B-86D7-33118F5D6B33}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.EditorFeatures2.UnitTests"", ""src\EditorFeatures\Test2\Microsoft.CodeAnalysis.EditorFeatures2.UnitTests.vbproj"", ""{3CEA0D69-00D3-40E5-A661-DC41EA07269B}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.Services.Test.Utilities"", ""src\EditorFeatures\TestUtilities\Roslyn.Services.Test.Utilities.csproj"", ""{76C6F005-C89D-4348-BB4A-39189DDBEB52}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.EditorFeatures.Wpf"", ""src\EditorFeatures\CSharp.Wpf\Microsoft.CodeAnalysis.CSharp.EditorFeatures.Wpf.csproj"", ""{FE2CBEA6-D121-4FAA-AA8B-FC9900BF8C83}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""InteractiveHost32"", ""src\Interactive\DesktopHost\InteractiveHost32.csproj"", ""{EBA4DFA1-6DED-418F-A485-A3B608978906}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""InteractiveHost.UnitTests"", ""src\Interactive\HostTest\InteractiveHost.UnitTests.csproj"", ""{8CEE3609-A5A9-4A9B-86D7-33118F5D6B34}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""csi"", ""src\Interactive\csi\csi.csproj"", ""{14118347-ED06-4608-9C45-18228273C712}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""vbi"", ""src\Interactive\vbi\vbi.vbproj"", ""{6E62A0FF-D0DC-4109-9131-AB8E60CDFF7B}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices"", ""src\VisualStudio\Core\Def\Microsoft.VisualStudio.LanguageServices.csproj"", ""{86FD5B9A-4FA0-4B10-B59F-CFAF077A859C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.Implementation"", ""src\VisualStudio\Core\Impl\Microsoft.VisualStudio.LanguageServices.Implementation.csproj"", ""{C0E80510-4FBE-4B0C-AF2C-4F473787722C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.SolutionExplorer"", ""src\VisualStudio\Core\SolutionExplorerShim\Microsoft.VisualStudio.LanguageServices.SolutionExplorer.csproj"", ""{7BE3DEEB-87F8-4E15-9C21-4F94B0B1C2D6}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.VisualStudio.LanguageServices.VisualBasic"", ""src\VisualStudio\VisualBasic\Impl\Microsoft.VisualStudio.LanguageServices.VisualBasic.vbproj"", ""{D49439D7-56D2-450F-A4F0-74CB95D620E6}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.CSharp"", ""src\VisualStudio\CSharp\Impl\Microsoft.VisualStudio.LanguageServices.CSharp.csproj"", ""{5DEFADBD-44EB-47A2-A53E-F1282CC9E4E9}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.CSharp.UnitTests"", ""src\VisualStudio\CSharp\Test\Microsoft.VisualStudio.LanguageServices.CSharp.UnitTests.csproj"", ""{91C574AD-0352-47E9-A019-EE02CC32A396}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.VisualStudio.LanguageServices.UnitTests"", ""src\VisualStudio\Core\Test\Microsoft.VisualStudio.LanguageServices.UnitTests.vbproj"", ""{A1455D30-55FC-45EF-8759-3AEBDB13D940}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.VisualStudio.Setup"", ""src\VisualStudio\Setup\Roslyn.VisualStudio.Setup.csproj"", ""{201EC5B7-F91E-45E5-B9F2-67A266CCE6FC}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.VisualStudio.InteractiveComponents"", ""src\VisualStudio\VisualStudioInteractiveComponents\Roslyn.VisualStudio.InteractiveComponents.csproj"", ""{2169F526-8A88-435D-8732-486ACA095A6A}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.VisualStudio.DiagnosticsWindow"", ""src\VisualStudio\VisualStudioDiagnosticsToolWindow\Roslyn.VisualStudio.DiagnosticsWindow.csproj"", ""{A486D7DE-F614-409D-BB41-0FFDF582E35C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""ExpressionEvaluatorPackage"", ""src\ExpressionEvaluator\Package\ExpressionEvaluatorPackage.csproj"", ""{B617717C-7881-4F01-AB6D-B1B6CC0483A0}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.ExpressionCompiler"", ""src\ExpressionEvaluator\CSharp\Source\ExpressionCompiler\Microsoft.CodeAnalysis.CSharp.ExpressionCompiler.csproj"", ""{FD6BA96C-7905-4876-8BCC-E38E2CA64F31}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.ExpressionCompiler.UnitTests"", ""src\ExpressionEvaluator\CSharp\Test\ExpressionCompiler\Microsoft.CodeAnalysis.CSharp.ExpressionCompiler.UnitTests.csproj"", ""{AE297965-4D56-4BA9-85EB-655AC4FC95A0}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.ResultProvider.UnitTests"", ""src\ExpressionEvaluator\CSharp\Test\ResultProvider\Microsoft.CodeAnalysis.CSharp.ResultProvider.UnitTests.csproj"", ""{60DB272A-21C9-4E8D-9803-FF4E132392C8}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.ExpressionCompiler"", ""src\ExpressionEvaluator\VisualBasic\Source\ExpressionCompiler\Microsoft.CodeAnalysis.VisualBasic.ExpressionCompiler.vbproj"", ""{73242A2D-6300-499D-8C15-FADF7ECB185C}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.ExpressionCompiler.UnitTests"", ""src\ExpressionEvaluator\VisualBasic\Test\ExpressionCompiler\Microsoft.CodeAnalysis.VisualBasic.ExpressionCompiler.UnitTests.vbproj"", ""{AC5E3515-482C-4C6A-92D9-D0CEA687DFC2}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExpressionCompiler"", ""src\ExpressionEvaluator\Core\Source\ExpressionCompiler\Microsoft.CodeAnalysis.ExpressionCompiler.csproj"", ""{B8DA3A90-A60C-42E3-9D8E-6C67B800C395}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.ResultProvider.UnitTests"", ""src\ExpressionEvaluator\VisualBasic\Test\ResultProvider\Microsoft.CodeAnalysis.VisualBasic.ResultProvider.UnitTests.vbproj"", ""{ACE53515-482C-4C6A-E2D2-4242A687DFEE}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExpressionCompiler.Utilities"", ""src\ExpressionEvaluator\Core\Test\ExpressionCompiler\Microsoft.CodeAnalysis.ExpressionCompiler.Utilities.csproj"", ""{21B80A31-8FF9-4E3A-8403-AABD635AEED9}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ResultProvider.Utilities"", ""src\ExpressionEvaluator\Core\Test\ResultProvider\Microsoft.CodeAnalysis.ResultProvider.Utilities.csproj"", ""{ABDBAC1E-350E-4DC3-BB45-3504404545EE}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""AnalyzerDriver"", ""src\Compilers\Core\AnalyzerDriver\AnalyzerDriver.shproj"", ""{D0BC9BE7-24F6-40CA-8DC6-FCB93BD44B34}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.WinRT.UnitTests"", ""src\Compilers\CSharp\Test\WinRT\Microsoft.CodeAnalysis.CSharp.WinRT.UnitTests.csproj"", ""{FCFA8808-A1B6-48CC-A1EA-0B8CA8AEDA8E}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.Build.Tasks.CodeAnalysis.UnitTests"", ""src\Compilers\Core\MSBuildTaskTests\Microsoft.Build.Tasks.CodeAnalysis.UnitTests.csproj"", ""{1DFEA9C5-973C-4179-9B1B-0F32288E1EF2}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""BasicResultProvider"", ""src\ExpressionEvaluator\VisualBasic\Source\ResultProvider\BasicResultProvider.shproj"", ""{3140FE61-0856-4367-9AA3-8081B9A80E35}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""BasicResultProvider.NetFX20"", ""src\ExpressionEvaluator\VisualBasic\Source\ResultProvider\NetFX20\BasicResultProvider.NetFX20.vbproj"", ""{76242A2D-2600-49DD-8C15-FEA07ECB1842}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.ResultProvider"", ""src\ExpressionEvaluator\VisualBasic\Source\ResultProvider\Portable\Microsoft.CodeAnalysis.VisualBasic.ResultProvider.vbproj"", ""{76242A2D-2600-49DD-8C15-FEA07ECB1843}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CSharpResultProvider"", ""src\ExpressionEvaluator\CSharp\Source\ResultProvider\CSharpResultProvider.shproj"", ""{3140FE61-0856-4367-9AA3-8081B9A80E36}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""CSharpResultProvider.NetFX20"", ""src\ExpressionEvaluator\CSharp\Source\ResultProvider\NetFX20\CSharpResultProvider.NetFX20.csproj"", ""{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D3}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.ResultProvider"", ""src\ExpressionEvaluator\CSharp\Source\ResultProvider\Portable\Microsoft.CodeAnalysis.CSharp.ResultProvider.csproj"", ""{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D4}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""ResultProvider"", ""src\ExpressionEvaluator\Core\Source\ResultProvider\ResultProvider.shproj"", ""{BB3CA047-5D00-48D4-B7D3-233C1265C065}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""ResultProvider.NetFX20"", ""src\ExpressionEvaluator\Core\Source\ResultProvider\NetFX20\ResultProvider.NetFX20.csproj"", ""{BEDC5A4A-809E-4017-9CFD-6C8D4E1847F0}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ResultProvider"", ""src\ExpressionEvaluator\Core\Source\ResultProvider\Portable\Microsoft.CodeAnalysis.ResultProvider.csproj"", ""{FA0E905D-EC46-466D-B7B2-3B5557F9428C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""vbc"", ""src\Compilers\VisualBasic\vbc\vbc.csproj"", ""{E58EE9D7-1239-4961-A0C1-F9EC3952C4C1}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.Test.Utilities"", ""src\Test\Utilities\Portable\Roslyn.Test.Utilities.csproj"", ""{CCBD3438-3E84-40A9-83AD-533F23BCFCA5}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Scripting.Desktop.UnitTests"", ""src\Scripting\CoreTest.Desktop\Microsoft.CodeAnalysis.Scripting.Desktop.UnitTests.csproj"", ""{6FD1CC3E-6A99-4736-9B8D-757992DDE75D}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.Scripting.Desktop.UnitTests"", ""src\Scripting\CSharpTest.Desktop\Microsoft.CodeAnalysis.CSharp.Scripting.Desktop.UnitTests.csproj"", ""{286B01F3-811A-40A7-8C1F-10C9BB0597F7}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Scripting.Desktop.UnitTests"", ""src\Scripting\VisualBasicTest.Desktop\Microsoft.CodeAnalysis.VisualBasic.Scripting.Desktop.UnitTests.vbproj"", ""{24973B4C-FD09-4EE1-97F4-EA03E6B12040}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.Scripting.UnitTests"", ""src\Scripting\VisualBasicTest\Microsoft.CodeAnalysis.VisualBasic.Scripting.UnitTests.vbproj"", ""{ABC7262E-1053-49F3-B846-E3091BB92E8C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.Hosting.Diagnostics"", ""src\Test\Diagnostics\Roslyn.Hosting.Diagnostics.csproj"", ""{E2E889A5-2489-4546-9194-47C63E49EAEB}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""BasicAnalyzerDriver"", ""src\Compilers\VisualBasic\BasicAnalyzerDriver\BasicAnalyzerDriver.shproj"", ""{E8F0BAA5-7327-43D1-9A51-644E81AE55F1}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CSharpAnalyzerDriver"", ""src\Compilers\CSharp\CSharpAnalyzerDriver\CSharpAnalyzerDriver.shproj"", ""{54E08BF5-F819-404F-A18D-0AB9EA81EA04}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CommandLine"", ""src\Compilers\Core\CommandLine\CommandLine.shproj"", ""{AD6F474E-E6D4-4217-91F3-B7AF1BE31CCC}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.Compilers.Extension"", ""src\Compilers\Extension\Roslyn.Compilers.Extension.csproj"", ""{43026D51-3083-4850-928D-07E1883D5B1A}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.IntegrationTest.Setup"", ""src\VisualStudio\IntegrationTest\TestSetup\Microsoft.VisualStudio.IntegrationTest.Setup.csproj"", ""{A88AB44F-7F9D-43F6-A127-83BB65E5A7E2}""
	ProjectSection(ProjectDependencies) = postProject
		{600AF682-E097-407B-AD85-EE3CED37E680} = {600AF682-E097-407B-AD85-EE3CED37E680}
		{A486D7DE-F614-409D-BB41-0FFDF582E35C} = {A486D7DE-F614-409D-BB41-0FFDF582E35C}
	EndProjectSection
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.IntegrationTests"", ""src\VisualStudio\IntegrationTest\IntegrationTests\Microsoft.VisualStudio.LanguageServices.IntegrationTests.csproj"", ""{E5A55C16-A5B9-4874-9043-A5266DC02F58}""
	ProjectSection(ProjectDependencies) = postProject
		{A88AB44F-7F9D-43F6-A127-83BB65E5A7E2} = {A88AB44F-7F9D-43F6-A127-83BB65E5A7E2}
	EndProjectSection
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.IntegrationTest.Utilities"", ""src\VisualStudio\IntegrationTest\TestUtilities\Microsoft.VisualStudio.IntegrationTest.Utilities.csproj"", ""{3BED15FD-D608-4573-B432-1569C1026F6D}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.PerformanceTests"", ""src\Test\Perf\tests\Roslyn.PerformanceTests.csproj"", ""{DA0D2A70-A2F9-4654-A99A-3227EDF54FF1}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.Xaml"", ""src\VisualStudio\Xaml\Impl\Microsoft.VisualStudio.LanguageServices.Xaml.csproj"", ""{971E832B-7471-48B5-833E-5913188EC0E4}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.Test.Performance.Utilities"", ""src\Test\Perf\Utilities\Roslyn.Test.Performance.Utilities.csproj"", ""{59AD474E-2A35-4E8A-A74D-E33479977FBF}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""Microsoft.CodeAnalysis.Debugging"", ""src\Dependencies\CodeAnalysis.Debugging\Microsoft.CodeAnalysis.Debugging.shproj"", ""{D73ADF7D-2C1C-42AE-B2AB-EDC9497E4B71}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""Microsoft.CodeAnalysis.PooledObjects"", ""src\Dependencies\PooledObjects\Microsoft.CodeAnalysis.PooledObjects.shproj"", ""{C1930979-C824-496B-A630-70F5369A636F}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Remote.Workspaces"", ""src\Workspaces\Remote\Core\Microsoft.CodeAnalysis.Remote.Workspaces.csproj"", ""{F822F72A-CC87-4E31-B57D-853F65CBEBF3}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Remote.ServiceHub"", ""src\Workspaces\Remote\ServiceHub\Microsoft.CodeAnalysis.Remote.ServiceHub.csproj"", ""{80FDDD00-9393-47F7-8BAF-7E87CE011068}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.Build.Tasks.CodeAnalysis"", ""src\Compilers\Core\MSBuildTask\Microsoft.Build.Tasks.CodeAnalysis.csproj"", ""{7AD4FE65-9A30-41A6-8004-AA8F89BCB7F3}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.VisualStudio.Next.UnitTests"", ""src\VisualStudio\Core\Test.Next\Roslyn.VisualStudio.Next.UnitTests.csproj"", ""{2E1658E2-5045-4F85-A64C-C0ECCD39F719}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""BuildBoss"", ""src\Tools\BuildBoss\BuildBoss.csproj"", ""{9C0660D9-48CA-40E1-BABA-8F6A1F11FE10}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Scripting.TestUtilities"", ""src\Scripting\CoreTestUtilities\Microsoft.CodeAnalysis.Scripting.TestUtilities.csproj"", ""{21A01C2D-2501-4619-8144-48977DD22D9C}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.Services.UnitTests.Utilities"", ""src\Workspaces\CoreTestUtilities\Roslyn.Services.UnitTests.Utilities.csproj"", ""{3F2FDC1C-DC6F-44CB-B4A1-A9026F25D66E}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.EditorFeatures.Test.Utilities"", ""src\EditorFeatures\TestUtilities2\Microsoft.CodeAnalysis.EditorFeatures.Test.Utilities.vbproj"", ""{3DFB4701-E3D6-4435-9F70-A6E35822C4F2}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.VisualStudio.LanguageServices.Test.Utilities2"", ""src\VisualStudio\TestUtilities2\Microsoft.VisualStudio.LanguageServices.Test.Utilities2.vbproj"", ""{69F853E5-BD04-4842-984F-FC68CC51F402}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.FunctionResolver"", ""src\ExpressionEvaluator\Core\Source\FunctionResolver\Microsoft.CodeAnalysis.FunctionResolver.csproj"", ""{6FC8E6F5-659C-424D-AEB5-331B95883E29}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.FunctionResolver.UnitTests"", ""src\ExpressionEvaluator\Core\Test\FunctionResolver\Microsoft.CodeAnalysis.FunctionResolver.UnitTests.csproj"", ""{DD317BE1-42A1-4795-B1D4-F370C40D649A}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.Razor.RemoteClient"", ""src\VisualStudio\Razor\Microsoft.VisualStudio.LanguageServices.Razor.RemoteClient.csproj"", ""{0C0EEB55-4B6D-4F2B-B0BB-B9EB2BA9E980}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Remote.Razor.ServiceHub"", ""src\Workspaces\Remote\Razor\Microsoft.CodeAnalysis.Remote.Razor.ServiceHub.csproj"", ""{B6FC05F2-0E49-4BE2-8030-ACBB82B7F431}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.VisualStudio.Setup.Dependencies"", ""src\VisualStudio\Setup.Dependencies\Roslyn.VisualStudio.Setup.Dependencies.csproj"", ""{1688E1E5-D510-4E06-86F3-F8DB10B1393D}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""StackDepthTest"", ""src\Test\Perf\StackDepthTest\StackDepthTest.csproj"", ""{F040CEC5-5E11-4DBD-9F6A-250478E28177}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CodeStyle"", ""src\CodeStyle\Core\Analyzers\Microsoft.CodeAnalysis.CodeStyle.csproj"", ""{275812EE-DEDB-4232-9439-91C9757D2AE4}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CodeStyle.Fixes"", ""src\CodeStyle\Core\CodeFixes\Microsoft.CodeAnalysis.CodeStyle.Fixes.csproj"", ""{5FF1E493-69CC-4D0B-83F2-039F469A04E1}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.CodeStyle"", ""src\CodeStyle\CSharp\Analyzers\Microsoft.CodeAnalysis.CSharp.CodeStyle.csproj"", ""{AA87BFED-089A-4096-B8D5-690BDC7D5B24}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.CodeStyle.Fixes"", ""src\CodeStyle\CSharp\CodeFixes\Microsoft.CodeAnalysis.CSharp.CodeStyle.Fixes.csproj"", ""{A07ABCF5-BC43-4EE9-8FD8-B2D77FD54D73}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.CodeStyle"", ""src\CodeStyle\VisualBasic\Analyzers\Microsoft.CodeAnalysis.VisualBasic.CodeStyle.vbproj"", ""{2531A8C4-97DD-47BC-A79C-B7846051E137}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.CodeStyle.Fixes"", ""src\CodeStyle\VisualBasic\CodeFixes\Microsoft.CodeAnalysis.VisualBasic.CodeStyle.Fixes.vbproj"", ""{0141285D-8F6C-42C7-BAF3-3C0CCD61C716}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CodeStyle.UnitTestUtilities"", ""src\CodeStyle\Core\Tests\Microsoft.CodeAnalysis.CodeStyle.UnitTestUtilities.csproj"", ""{9FF1205F-1D7C-4EE4-B038-3456FE6EBEAF}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CSharp.CodeStyle.UnitTests"", ""src\CodeStyle\CSharp\Tests\Microsoft.CodeAnalysis.CSharp.CodeStyle.UnitTests.csproj"", ""{5018D049-5870-465A-889B-C742CE1E31CB}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.VisualBasic.CodeStyle.UnitTests"", ""src\CodeStyle\VisualBasic\Tests\Microsoft.CodeAnalysis.VisualBasic.CodeStyle.UnitTests.vbproj"", ""{E512C6C1-F085-4AD7-B0D9-E8F1A0A2A510}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""RoslynPublish"", ""src\Tools\RoslynPublish\RoslynPublish.csproj"", ""{2D36C343-BB6A-4CB5-902B-E2145ACCB58F}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.EditorFeatures.Wpf"", ""src\EditorFeatures\Core.Wpf\Microsoft.CodeAnalysis.EditorFeatures.Wpf.csproj"", ""{FFB00FB5-8C8C-4A02-B67D-262B9D28E8B1}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""AnalyzerRunner"", ""src\Tools\AnalyzerRunner\AnalyzerRunner.csproj"", ""{60166C60-813C-46C4-911D-2411B4ABBC0F}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Debugging.Package"", ""src\Dependencies\CodeAnalysis.Debugging\Microsoft.CodeAnalysis.Debugging.Package.csproj"", ""{FC2AE90B-2E4B-4045-9FDD-73D4F5ED6C89}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.PooledObjects.Package"", ""src\Dependencies\PooledObjects\Microsoft.CodeAnalysis.PooledObjects.Package.csproj"", ""{49E7C367-181B-499C-AC2E-8E17C81418D6}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Workspaces.MSBuild.UnitTests"", ""src\Workspaces\MSBuildTest\Microsoft.CodeAnalysis.Workspaces.MSBuild.UnitTests.csproj"", ""{037F06F0-3BE8-42D0-801E-2F74FC380AB8}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""InteractiveHost64"", ""src\Interactive\DesktopHost\InteractiveHost64.csproj"", ""{2F11618A-9251-4609-B3D5-CE4D2B3D3E49}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.IntegrationTest.IntegrationService"", ""src\VisualStudio\IntegrationTest\IntegrationService\Microsoft.VisualStudio.IntegrationTest.IntegrationService.csproj"", ""{764D2C19-0187-4837-A2A3-96DDC6EF4CE2}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.Net.Compilers.Package"", ""src\NuGet\Microsoft.Net.Compilers\Microsoft.Net.Compilers.Package.csproj"", ""{9102ECF3-5CD1-4107-B8B7-F3795A52D790}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.NETCore.Compilers.Package"", ""src\NuGet\Microsoft.NETCore.Compilers\Microsoft.NETCore.Compilers.Package.csproj"", ""{50CF5D8F-F82F-4210-A06E-37CC9BFFDD49}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Compilers.Package"", ""src\NuGet\Microsoft.CodeAnalysis.Compilers.Package.csproj"", ""{CFA94A39-4805-456D-A369-FC35CCC170E9}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Packages"", ""Packages"", ""{C52D8057-43AF-40E6-A01B-6CDBB7301985}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Scripting.Package"", ""src\NuGet\Microsoft.CodeAnalysis.Scripting.Package.csproj"", ""{4A490CBC-37F4-4859-AFDB-4B0833D144AF}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.EditorFeatures.Package"", ""src\NuGet\Microsoft.CodeAnalysis.EditorFeatures.Package.csproj"", ""{34E868E9-D30B-4FB5-BC61-AFC4A9612A0F}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Packages"", ""Packages"", ""{BE25E872-1667-4649-9D19-96B83E75A44E}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""VS.ExternalAPIs.Roslyn.Package"", ""src\NuGet\VisualStudio\VS.ExternalAPIs.Roslyn.Package.csproj"", ""{0EB22BD1-B8B1-417D-8276-F475C2E190FF}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""VS.Tools.Roslyn.Package"", ""src\NuGet\VisualStudio\VS.Tools.Roslyn.Package.csproj"", ""{3636D3E2-E3EF-4815-B020-819F382204CD}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Package"", ""src\NuGet\Microsoft.CodeAnalysis.Package.csproj"", ""{B9843F65-262E-4F40-A0BC-2CBEF7563A44}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Compilers.Setup"", ""src\Setup\DevDivVsix\CompilersPackage\Microsoft.CodeAnalysis.Compilers.Setup.csproj"", ""{03607817-6800-40B6-BEAA-D6F437CD62B7}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Installer.Package"", ""src\Setup\Installer\Installer.Package.csproj"", ""{6A68FDF9-24B3-4CB6-A808-96BF50D1BCE5}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.Workspaces.Desktop.UnitTests"", ""src\Workspaces\DesktopTest\Microsoft.CodeAnalysis.Workspaces.Desktop.UnitTests.csproj"", ""{23405307-7EFF-4774-8B11-8F5885439761}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Insertion"", ""Insertion"", ""{AFA5F921-0650-45E8-B293-51A0BB89DEA0}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""DevDivInsertionFiles"", ""src\Setup\DevDivInsertionFiles\DevDivInsertionFiles.csproj"", ""{6362616E-6A47-48F0-9EE0-27800B306ACB}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""ExternalAccess"", ""ExternalAccess"", ""{8977A560-45C2-4EC2-A849-97335B382C74}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExternalAccess.FSharp"", ""src\Tools\ExternalAccess\FSharp\Microsoft.CodeAnalysis.ExternalAccess.FSharp.csproj"", ""{BD8CE303-5F04-45EC-8DCF-73C9164CD614}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExternalAccess.Razor"", ""src\Tools\ExternalAccess\Razor\Microsoft.CodeAnalysis.ExternalAccess.Razor.csproj"", ""{2FB6C157-DF91-4B1C-9827-A4D1C08C73EC}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.CodeLens"", ""src\VisualStudio\CodeLens\Microsoft.VisualStudio.LanguageServices.CodeLens.csproj"", ""{5E6E9184-DEC5-4EC5-B0A4-77CFDC8CDEBE}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.Net.Compilers.Toolset.Package"", ""src\NuGet\Microsoft.Net.Compilers.Toolset\Microsoft.Net.Compilers.Toolset.Package.csproj"", ""{A74C7D2E-92FA-490A-B80A-28BEF56B56FC}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.LanguageServer.Protocol"", ""src\Features\LanguageServer\Protocol\Microsoft.CodeAnalysis.LanguageServer.Protocol.csproj"", ""{686BF57E-A6FF-467B-AAB3-44DE916A9772}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.LanguageServer.Protocol.UnitTests"", ""src\Features\LanguageServer\ProtocolUnitTests\Microsoft.CodeAnalysis.LanguageServer.Protocol.UnitTests.csproj"", ""{1DDE89EE-5819-441F-A060-2FF4A986F372}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExternalAccess.Debugger"", ""src\Tools\ExternalAccess\Debugger\Microsoft.CodeAnalysis.ExternalAccess.Debugger.csproj"", ""{655A5B07-39B8-48CD-8590-8AC0C2B708D8}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExternalAccess.Xamarin.Remote"", ""src\Tools\ExternalAccess\Xamarin.Remote\Microsoft.CodeAnalysis.ExternalAccess.Xamarin.Remote.csproj"", ""{DE53934B-7FC1-48A0-85AB-C519FBBD02CF}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Roslyn.VisualStudio.Setup.ServiceHub"", ""src\Setup\DevDivVsix\ServiceHubConfig\Roslyn.VisualStudio.Setup.ServiceHub.csproj"", ""{3D33BBFD-EC63-4E8C-A714-0A48A3809A87}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExternalAccess.FSharp.UnitTests"", ""src\Tools\ExternalAccess\FSharpTest\Microsoft.CodeAnalysis.ExternalAccess.FSharp.UnitTests.csproj"", ""{BFFB5CAE-33B5-447E-9218-BDEBFDA96CB5}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.ExternalAccess.Apex"", ""src\Tools\ExternalAccess\Apex\Microsoft.CodeAnalysis.ExternalAccess.Apex.csproj"", ""{FC32EF16-31B1-47B3-B625-A80933CB3F29}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.LiveShare"", ""src\VisualStudio\LiveShare\Impl\Microsoft.VisualStudio.LanguageServices.LiveShare.csproj"", ""{453C8E28-81D4-431E-BFB0-F3D413346E51}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.VisualStudio.LanguageServices.LiveShare.UnitTests"", ""src\VisualStudio\LiveShare\Test\Microsoft.VisualStudio.LanguageServices.LiveShare.UnitTests.csproj"", ""{CE7F7553-DB2D-4839-92E3-F042E4261B4E}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""IdeBenchmarks"", ""src\Tools\IdeBenchmarks\IdeBenchmarks.csproj"", ""{FF38E9C9-7A25-44F0-B2C4-24C9BFD6A8F6}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Solution items"", ""Solution items"", ""{157EA250-2F28-4948-A8F2-D58EAEA05DC8}""
	ProjectSection(SolutionItems) = preProject
		.editorconfig = .editorconfig
		.vsconfig = .vsconfig
	EndProjectSection
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""CompilersIOperationGenerator"", ""src\Tools\Source\CompilerGeneratorTools\Source\IOperationGenerator\CompilersIOperationGenerator.csproj"", ""{D55FB2BD-CC9E-454B-9654-94AF5D910BF7}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.LanguageServerIndexFormat.Generator"", ""src\Features\Lsif\Generator\Microsoft.CodeAnalysis.LanguageServerIndexFormat.Generator.csproj"", ""{B9899CF1-E0EB-4599-9E24-6939A04B4979}""
EndProject
Project(""{778DAE3C-4631-46EA-AA77-85C1314464D9}"") = ""Microsoft.CodeAnalysis.LanguageServerIndexFormat.Generator.UnitTests"", ""src\Features\Lsif\GeneratorTest\Microsoft.CodeAnalysis.LanguageServerIndexFormat.Generator.UnitTests.vbproj"", ""{D15BF03E-04ED-4BEE-A72B-7620F541F4E2}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Analyzers"", ""Analyzers"", ""{4A49D526-1644-4819-AA4F-95B348D447D4}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CompilerExtensions"", ""src\Workspaces\SharedUtilitiesAndExtensions\Compiler\Core\CompilerExtensions.shproj"", ""{EC946164-1E17-410B-B7D9-7DE7E6268D63}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""WorkspaceExtensions"", ""src\Workspaces\SharedUtilitiesAndExtensions\Workspace\Core\WorkspaceExtensions.shproj"", ""{99F594B1-3916-471D-A761-A6731FC50E9A}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CSharpCompilerExtensions"", ""src\Workspaces\SharedUtilitiesAndExtensions\Compiler\CSharp\CSharpCompilerExtensions.shproj"", ""{699FEA05-AEA7-403D-827E-53CF4E826955}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CSharpWorkspaceExtensions"", ""src\Workspaces\SharedUtilitiesAndExtensions\Workspace\CSharp\CSharpWorkspaceExtensions.shproj"", ""{438DB8AF-F3F0-4ED9-80B5-13FDDD5B8787}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CSharpAnalyzers.UnitTests"", ""src\Analyzers\CSharp\Tests\CSharpAnalyzers.UnitTests.shproj"", ""{58969243-7F59-4236-93D0-C93B81F569B3}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""VisualBasicCompilerExtensions"", ""src\Workspaces\SharedUtilitiesAndExtensions\Compiler\VisualBasic\VisualBasicCompilerExtensions.shproj"", ""{CEC0DCE7-8D52-45C3-9295-FC7B16BD2451}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""VisualBasicWorkspaceExtensions"", ""src\Workspaces\SharedUtilitiesAndExtensions\Workspace\VisualBasic\VisualBasicWorkspaceExtensions.shproj"", ""{E9DBFA41-7A9C-49BE-BD36-FD71B31AA9FE}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""VisualBasicAnalyzers.UnitTests"", ""src\Analyzers\VisualBasic\Tests\VisualBasicAnalyzers.UnitTests.shproj"", ""{7B7F4153-AE93-4908-B8F0-430871589F83}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""Analyzers"", ""src\Analyzers\Core\Analyzers\Analyzers.shproj"", ""{76E96966-4780-4040-8197-BDE2879516F4}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CodeFixes"", ""src\Analyzers\Core\CodeFixes\CodeFixes.shproj"", ""{1B6C4A1A-413B-41FB-9F85-5C09118E541B}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CSharpAnalyzers"", ""src\Analyzers\CSharp\Analyzers\CSharpAnalyzers.shproj"", ""{EAFFCA55-335B-4860-BB99-EFCEAD123199}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""CSharpCodeFixes"", ""src\Analyzers\CSharp\CodeFixes\CSharpCodeFixes.shproj"", ""{DA973826-C985-4128-9948-0B445E638BDB}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""VisualBasicAnalyzers"", ""src\Analyzers\VisualBasic\Analyzers\VisualBasicAnalyzers.shproj"", ""{94FAF461-2E74-4DBB-9813-6B2CDE6F1880}""
EndProject
Project(""{D954291E-2A0B-460D-934E-DC6B0785DB48}"") = ""VisualBasicCodeFixes"", ""src\Analyzers\VisualBasic\CodeFixes\VisualBasicCodeFixes.shproj"", ""{9F9CCC78-7487-4127-9D46-DB23E501F001}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""SharedUtilitiesAndExtensions"", ""SharedUtilitiesAndExtensions"", ""{DF17AF27-AA02-482B-8946-5CA8A50D5A2B}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Compiler"", ""Compiler"", ""{7A69EA65-4411-4CD0-B439-035E720C1BD3}""
EndProject
Project(""{2150E333-8FDC-42A3-9474-1A3956D46DE8}"") = ""Workspace"", ""Workspace"", ""{9C1BE25C-5926-4E56-84AE-D2242CB0627E}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.EditorFeatures.DiagnosticsTests.Utilities"", ""src\EditorFeatures\DiagnosticsTestUtilities\Microsoft.CodeAnalysis.EditorFeatures.DiagnosticsTests.Utilities.csproj"", ""{B64766CD-1A1F-4C1B-B11F-C30F82B8E41E}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""Microsoft.CodeAnalysis.CodeStyle.LegacyTestFramework.UnitTestUtilities"", ""src\CodeStyle\Core\Tests\Microsoft.CodeAnalysis.CodeStyle.LegacyTestFramework.UnitTestUtilities.csproj"", ""{2D5E2DE4-5DA8-41C1-A14F-49855DCCE9C5}""
EndProject
Project(""{9A19103F-16F7-4668-BE54-9A1E7A4F7556}"") = ""IdeCoreBenchmarks"", ""src\Tools\IdeCoreBenchmarks\IdeCoreBenchmarks.csproj"", ""{CEA80C83-5848-4FF6-B4E8-CEEE9482E4AA}""
EndProject
Global
	GlobalSection(SharedMSBuildProjectFiles) = preSolution
		src\Analyzers\VisualBasic\CodeFixes\VisualBasicCodeFixes.projitems*{0141285d-8f6c-42c7-baf3-3c0ccd61c716}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\VisualBasic\VisualBasicWorkspaceExtensions.projitems*{0141285d-8f6c-42c7-baf3-3c0ccd61c716}*SharedItemsImports = 5
		src\Analyzers\VisualBasic\Tests\VisualBasicAnalyzers.UnitTests.projitems*{0be66736-cdaa-4989-88b1-b3f46ebdca4a}*SharedItemsImports = 5
		src\Analyzers\Core\CodeFixes\CodeFixes.projitems*{1b6c4a1a-413b-41fb-9f85-5c09118e541b}*SharedItemsImports = 13
		src\Compilers\Core\AnalyzerDriver\AnalyzerDriver.projitems*{1ee8cad3-55f9-4d91-96b2-084641da9a6c}*SharedItemsImports = 5
		src\Dependencies\CodeAnalysis.Debugging\Microsoft.CodeAnalysis.Debugging.projitems*{1ee8cad3-55f9-4d91-96b2-084641da9a6c}*SharedItemsImports = 5
		src\Dependencies\PooledObjects\Microsoft.CodeAnalysis.PooledObjects.projitems*{1ee8cad3-55f9-4d91-96b2-084641da9a6c}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\CSharp\CSharpCompilerExtensions.projitems*{21b239d0-d144-430f-a394-c066d58ee267}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\CSharp\CSharpWorkspaceExtensions.projitems*{21b239d0-d144-430f-a394-c066d58ee267}*SharedItemsImports = 5
		src\Compilers\VisualBasic\BasicAnalyzerDriver\BasicAnalyzerDriver.projitems*{2523d0e6-df32-4a3e-8ae0-a19bffae2ef6}*SharedItemsImports = 5
		src\Analyzers\VisualBasic\Analyzers\VisualBasicAnalyzers.projitems*{2531a8c4-97dd-47bc-a79c-b7846051e137}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\VisualBasic\VisualBasicCompilerExtensions.projitems*{2531a8c4-97dd-47bc-a79c-b7846051e137}*SharedItemsImports = 5
		src\Analyzers\Core\Analyzers\Analyzers.projitems*{275812ee-dedb-4232-9439-91c9757d2ae4}*SharedItemsImports = 5
		src\Dependencies\PooledObjects\Microsoft.CodeAnalysis.PooledObjects.projitems*{275812ee-dedb-4232-9439-91c9757d2ae4}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\Core\CompilerExtensions.projitems*{275812ee-dedb-4232-9439-91c9757d2ae4}*SharedItemsImports = 5
		src\ExpressionEvaluator\VisualBasic\Source\ResultProvider\BasicResultProvider.projitems*{3140fe61-0856-4367-9aa3-8081b9a80e35}*SharedItemsImports = 13
		src\ExpressionEvaluator\CSharp\Source\ResultProvider\CSharpResultProvider.projitems*{3140fe61-0856-4367-9aa3-8081b9a80e36}*SharedItemsImports = 13
		src\Analyzers\CSharp\Analyzers\CSharpAnalyzers.projitems*{3973b09a-4fbf-44a5-8359-3d22ceb71f71}*SharedItemsImports = 5
		src\Analyzers\CSharp\CodeFixes\CSharpCodeFixes.projitems*{3973b09a-4fbf-44a5-8359-3d22ceb71f71}*SharedItemsImports = 5
		src\Compilers\CSharp\CSharpAnalyzerDriver\CSharpAnalyzerDriver.projitems*{3973b09a-4fbf-44a5-8359-3d22ceb71f71}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\CSharp\CSharpWorkspaceExtensions.projitems*{438db8af-f3f0-4ed9-80b5-13fddd5b8787}*SharedItemsImports = 13
		src\Compilers\Core\CommandLine\CommandLine.projitems*{4b45ca0c-03a0-400f-b454-3d4bcb16af38}*SharedItemsImports = 5
		src\Analyzers\CSharp\Tests\CSharpAnalyzers.UnitTests.projitems*{5018d049-5870-465a-889b-c742ce1e31cb}*SharedItemsImports = 5
		src\Compilers\CSharp\CSharpAnalyzerDriver\CSharpAnalyzerDriver.projitems*{54e08bf5-f819-404f-a18d-0ab9ea81ea04}*SharedItemsImports = 13
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\VisualBasic\VisualBasicCompilerExtensions.projitems*{57ca988d-f010-4bf2-9a2e-07d6dcd2ff2c}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\VisualBasic\VisualBasicWorkspaceExtensions.projitems*{57ca988d-f010-4bf2-9a2e-07d6dcd2ff2c}*SharedItemsImports = 5
		src\Analyzers\CSharp\Tests\CSharpAnalyzers.UnitTests.projitems*{58969243-7f59-4236-93d0-c93b81f569b3}*SharedItemsImports = 13
		src\Dependencies\PooledObjects\Microsoft.CodeAnalysis.PooledObjects.projitems*{5f8d2414-064a-4b3a-9b42-8e2a04246be5}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\Core\CompilerExtensions.projitems*{5f8d2414-064a-4b3a-9b42-8e2a04246be5}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\Core\WorkspaceExtensions.projitems*{5f8d2414-064a-4b3a-9b42-8e2a04246be5}*SharedItemsImports = 5
		src\Analyzers\Core\CodeFixes\CodeFixes.projitems*{5ff1e493-69cc-4d0b-83f2-039f469a04e1}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\Core\WorkspaceExtensions.projitems*{5ff1e493-69cc-4d0b-83f2-039f469a04e1}*SharedItemsImports = 5
		src\ExpressionEvaluator\CSharp\Source\ResultProvider\CSharpResultProvider.projitems*{60db272a-21c9-4e8d-9803-ff4e132392c8}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\CSharp\CSharpCompilerExtensions.projitems*{699fea05-aea7-403d-827e-53cf4e826955}*SharedItemsImports = 13
		src\ExpressionEvaluator\VisualBasic\Source\ResultProvider\BasicResultProvider.projitems*{76242a2d-2600-49dd-8c15-fea07ecb1842}*SharedItemsImports = 5
		src\ExpressionEvaluator\VisualBasic\Source\ResultProvider\BasicResultProvider.projitems*{76242a2d-2600-49dd-8c15-fea07ecb1843}*SharedItemsImports = 5
		src\Analyzers\Core\Analyzers\Analyzers.projitems*{76e96966-4780-4040-8197-bde2879516f4}*SharedItemsImports = 13
		src\Compilers\Core\CommandLine\CommandLine.projitems*{7ad4fe65-9a30-41a6-8004-aa8f89bcb7f3}*SharedItemsImports = 5
		src\Analyzers\VisualBasic\Tests\VisualBasicAnalyzers.UnitTests.projitems*{7b7f4153-ae93-4908-b8f0-430871589f83}*SharedItemsImports = 13
		src\Analyzers\VisualBasic\Analyzers\VisualBasicAnalyzers.projitems*{94faf461-2e74-4dbb-9813-6b2cde6f1880}*SharedItemsImports = 13
		src\Compilers\Core\CommandLine\CommandLine.projitems*{9508f118-f62e-4c16-a6f4-7c3b56e166ad}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\Core\WorkspaceExtensions.projitems*{99f594b1-3916-471d-a761-a6731fc50e9a}*SharedItemsImports = 13
		src\Analyzers\VisualBasic\CodeFixes\VisualBasicCodeFixes.projitems*{9f9ccc78-7487-4127-9d46-db23e501f001}*SharedItemsImports = 13
		src\Analyzers\CSharp\CodeFixes\CSharpCodeFixes.projitems*{a07abcf5-bc43-4ee9-8fd8-b2d77fd54d73}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\CSharp\CSharpWorkspaceExtensions.projitems*{a07abcf5-bc43-4ee9-8fd8-b2d77fd54d73}*SharedItemsImports = 5
		src\Analyzers\VisualBasic\Analyzers\VisualBasicAnalyzers.projitems*{a1bcd0ce-6c2f-4f8c-9a48-d9d93928e26d}*SharedItemsImports = 5
		src\Analyzers\VisualBasic\CodeFixes\VisualBasicCodeFixes.projitems*{a1bcd0ce-6c2f-4f8c-9a48-d9d93928e26d}*SharedItemsImports = 5
		src\Compilers\VisualBasic\BasicAnalyzerDriver\BasicAnalyzerDriver.projitems*{a1bcd0ce-6c2f-4f8c-9a48-d9d93928e26d}*SharedItemsImports = 5
		src\Analyzers\CSharp\Analyzers\CSharpAnalyzers.projitems*{aa87bfed-089a-4096-b8d5-690bdc7d5b24}*SharedItemsImports = 5
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\CSharp\CSharpCompilerExtensions.projitems*{aa87bfed-089a-4096-b8d5-690bdc7d5b24}*SharedItemsImports = 5
		src\ExpressionEvaluator\Core\Source\ResultProvider\ResultProvider.projitems*{abdbac1e-350e-4dc3-bb45-3504404545ee}*SharedItemsImports = 5
		src\Analyzers\CSharp\Tests\CSharpAnalyzers.UnitTests.projitems*{ac2bcefb-9298-4621-ac48-1ff5e639e48d}*SharedItemsImports = 5
		src\ExpressionEvaluator\VisualBasic\Source\ResultProvider\BasicResultProvider.projitems*{ace53515-482c-4c6a-e2d2-4242a687dfee}*SharedItemsImports = 5
		src\Compilers\Core\CommandLine\CommandLine.projitems*{ad6f474e-e6d4-4217-91f3-b7af1be31ccc}*SharedItemsImports = 13
		src\Compilers\CSharp\CSharpAnalyzerDriver\CSharpAnalyzerDriver.projitems*{b501a547-c911-4a05-ac6e-274a50dff30e}*SharedItemsImports = 5
		src\ExpressionEvaluator\Core\Source\ResultProvider\ResultProvider.projitems*{bb3ca047-5d00-48d4-b7d3-233c1265c065}*SharedItemsImports = 13
		src\ExpressionEvaluator\Core\Source\ResultProvider\ResultProvider.projitems*{bedc5a4a-809e-4017-9cfd-6c8d4e1847f0}*SharedItemsImports = 5
		src\ExpressionEvaluator\CSharp\Source\ResultProvider\CSharpResultProvider.projitems*{bf9dac1e-3a5e-4dc3-bb44-9a64e0d4e9d3}*SharedItemsImports = 5
		src\ExpressionEvaluator\CSharp\Source\ResultProvider\CSharpResultProvider.projitems*{bf9dac1e-3a5e-4dc3-bb44-9a64e0d4e9d4}*SharedItemsImports = 5
		src\Dependencies\PooledObjects\Microsoft.CodeAnalysis.PooledObjects.projitems*{c1930979-c824-496b-a630-70f5369a636f}*SharedItemsImports = 13
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\VisualBasic\VisualBasicCompilerExtensions.projitems*{cec0dce7-8d52-45c3-9295-fc7b16bd2451}*SharedItemsImports = 13
		src\Compilers\Core\AnalyzerDriver\AnalyzerDriver.projitems*{d0bc9be7-24f6-40ca-8dc6-fcb93bd44b34}*SharedItemsImports = 13
		src\Dependencies\CodeAnalysis.Debugging\Microsoft.CodeAnalysis.Debugging.projitems*{d73adf7d-2c1c-42ae-b2ab-edc9497e4b71}*SharedItemsImports = 13
		src\Analyzers\CSharp\CodeFixes\CSharpCodeFixes.projitems*{da973826-c985-4128-9948-0b445e638bdb}*SharedItemsImports = 13
		src\Analyzers\VisualBasic\Tests\VisualBasicAnalyzers.UnitTests.projitems*{e512c6c1-f085-4ad7-b0d9-e8f1a0a2a510}*SharedItemsImports = 5
		src\Compilers\Core\CommandLine\CommandLine.projitems*{e58ee9d7-1239-4961-a0c1-f9ec3952c4c1}*SharedItemsImports = 5
		src\Compilers\VisualBasic\BasicAnalyzerDriver\BasicAnalyzerDriver.projitems*{e8f0baa5-7327-43d1-9a51-644e81ae55f1}*SharedItemsImports = 13
		src\Workspaces\SharedUtilitiesAndExtensions\Workspace\VisualBasic\VisualBasicWorkspaceExtensions.projitems*{e9dbfa41-7a9c-49be-bd36-fd71b31aa9fe}*SharedItemsImports = 13
		src\Analyzers\CSharp\Analyzers\CSharpAnalyzers.projitems*{eaffca55-335b-4860-bb99-efcead123199}*SharedItemsImports = 13
		src\Workspaces\SharedUtilitiesAndExtensions\Compiler\Core\CompilerExtensions.projitems*{ec946164-1e17-410b-b7d9-7de7e6268d63}*SharedItemsImports = 13
		src\Analyzers\Core\Analyzers\Analyzers.projitems*{edc68a0e-c68d-4a74-91b7-bf38ec909888}*SharedItemsImports = 5
		src\Analyzers\Core\CodeFixes\CodeFixes.projitems*{edc68a0e-c68d-4a74-91b7-bf38ec909888}*SharedItemsImports = 5
		src\Compilers\Core\AnalyzerDriver\AnalyzerDriver.projitems*{edc68a0e-c68d-4a74-91b7-bf38ec909888}*SharedItemsImports = 5
		src\Dependencies\CodeAnalysis.Debugging\Microsoft.CodeAnalysis.Debugging.projitems*{edc68a0e-c68d-4a74-91b7-bf38ec909888}*SharedItemsImports = 5
		src\ExpressionEvaluator\Core\Source\ResultProvider\ResultProvider.projitems*{fa0e905d-ec46-466d-b7b2-3b5557f9428c}*SharedItemsImports = 5
	EndGlobalSection
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{600AF682-E097-407B-AD85-EE3CED37E680}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{600AF682-E097-407B-AD85-EE3CED37E680}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{600AF682-E097-407B-AD85-EE3CED37E680}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{600AF682-E097-407B-AD85-EE3CED37E680}.Release|Any CPU.Build.0 = Release|Any CPU
		{A4C99B85-765C-4C65-9C2A-BB609AAB09E6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A4C99B85-765C-4C65-9C2A-BB609AAB09E6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A4C99B85-765C-4C65-9C2A-BB609AAB09E6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A4C99B85-765C-4C65-9C2A-BB609AAB09E6}.Release|Any CPU.Build.0 = Release|Any CPU
		{1EE8CAD3-55F9-4D91-96B2-084641DA9A6C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1EE8CAD3-55F9-4D91-96B2-084641DA9A6C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1EE8CAD3-55F9-4D91-96B2-084641DA9A6C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1EE8CAD3-55F9-4D91-96B2-084641DA9A6C}.Release|Any CPU.Build.0 = Release|Any CPU
		{9508F118-F62E-4C16-A6F4-7C3B56E166AD}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{9508F118-F62E-4C16-A6F4-7C3B56E166AD}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{9508F118-F62E-4C16-A6F4-7C3B56E166AD}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{9508F118-F62E-4C16-A6F4-7C3B56E166AD}.Release|Any CPU.Build.0 = Release|Any CPU
		{F5CE416E-B906-41D2-80B9-0078E887A3F6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{F5CE416E-B906-41D2-80B9-0078E887A3F6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{F5CE416E-B906-41D2-80B9-0078E887A3F6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{F5CE416E-B906-41D2-80B9-0078E887A3F6}.Release|Any CPU.Build.0 = Release|Any CPU
		{4B45CA0C-03A0-400F-B454-3D4BCB16AF38}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4B45CA0C-03A0-400F-B454-3D4BCB16AF38}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4B45CA0C-03A0-400F-B454-3D4BCB16AF38}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4B45CA0C-03A0-400F-B454-3D4BCB16AF38}.Release|Any CPU.Build.0 = Release|Any CPU
		{B501A547-C911-4A05-AC6E-274A50DFF30E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B501A547-C911-4A05-AC6E-274A50DFF30E}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B501A547-C911-4A05-AC6E-274A50DFF30E}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B501A547-C911-4A05-AC6E-274A50DFF30E}.Release|Any CPU.Build.0 = Release|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D203}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D203}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D203}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D203}.Release|Any CPU.Build.0 = Release|Any CPU
		{4462B57A-7245-4146-B504-D46FDE762948}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4462B57A-7245-4146-B504-D46FDE762948}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4462B57A-7245-4146-B504-D46FDE762948}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4462B57A-7245-4146-B504-D46FDE762948}.Release|Any CPU.Build.0 = Release|Any CPU
		{1AF3672A-C5F1-4604-B6AB-D98C4DE9C3B1}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1AF3672A-C5F1-4604-B6AB-D98C4DE9C3B1}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1AF3672A-C5F1-4604-B6AB-D98C4DE9C3B1}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1AF3672A-C5F1-4604-B6AB-D98C4DE9C3B1}.Release|Any CPU.Build.0 = Release|Any CPU
		{B2C33A93-DB30-4099-903E-77D75C4C3F45}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B2C33A93-DB30-4099-903E-77D75C4C3F45}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B2C33A93-DB30-4099-903E-77D75C4C3F45}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B2C33A93-DB30-4099-903E-77D75C4C3F45}.Release|Any CPU.Build.0 = Release|Any CPU
		{28026D16-EB0C-40B0-BDA7-11CAA2B97CCC}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{28026D16-EB0C-40B0-BDA7-11CAA2B97CCC}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{28026D16-EB0C-40B0-BDA7-11CAA2B97CCC}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{28026D16-EB0C-40B0-BDA7-11CAA2B97CCC}.Release|Any CPU.Build.0 = Release|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D202}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D202}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D202}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{50D26304-0961-4A51-ABF6-6CAD1A56D202}.Release|Any CPU.Build.0 = Release|Any CPU
		{7FE6B002-89D8-4298-9B1B-0B5C247DD1FD}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{7FE6B002-89D8-4298-9B1B-0B5C247DD1FD}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{7FE6B002-89D8-4298-9B1B-0B5C247DD1FD}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{7FE6B002-89D8-4298-9B1B-0B5C247DD1FD}.Release|Any CPU.Build.0 = Release|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F9}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F9}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F9}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F9}.Release|Any CPU.Build.0 = Release|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F8}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F8}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F8}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F8}.Release|Any CPU.Build.0 = Release|Any CPU
		{2523D0E6-DF32-4A3E-8AE0-A19BFFAE2EF6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2523D0E6-DF32-4A3E-8AE0-A19BFFAE2EF6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2523D0E6-DF32-4A3E-8AE0-A19BFFAE2EF6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2523D0E6-DF32-4A3E-8AE0-A19BFFAE2EF6}.Release|Any CPU.Build.0 = Release|Any CPU
		{E3B32027-3362-41DF-9172-4D3B623F42A5}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{E3B32027-3362-41DF-9172-4D3B623F42A5}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{E3B32027-3362-41DF-9172-4D3B623F42A5}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{E3B32027-3362-41DF-9172-4D3B623F42A5}.Release|Any CPU.Build.0 = Release|Any CPU
		{190CE348-596E-435A-9E5B-12A689F9FC29}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{190CE348-596E-435A-9E5B-12A689F9FC29}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{190CE348-596E-435A-9E5B-12A689F9FC29}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{190CE348-596E-435A-9E5B-12A689F9FC29}.Release|Any CPU.Build.0 = Release|Any CPU
		{9C9DABA4-0E72-4469-ADF1-4991F3CA572A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{9C9DABA4-0E72-4469-ADF1-4991F3CA572A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{9C9DABA4-0E72-4469-ADF1-4991F3CA572A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{9C9DABA4-0E72-4469-ADF1-4991F3CA572A}.Release|Any CPU.Build.0 = Release|Any CPU
		{BF180BD2-4FB7-4252-A7EC-A00E0C7A028A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{BF180BD2-4FB7-4252-A7EC-A00E0C7A028A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{BF180BD2-4FB7-4252-A7EC-A00E0C7A028A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{BF180BD2-4FB7-4252-A7EC-A00E0C7A028A}.Release|Any CPU.Build.0 = Release|Any CPU
		{BDA5D613-596D-4B61-837C-63554151C8F5}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{BDA5D613-596D-4B61-837C-63554151C8F5}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{BDA5D613-596D-4B61-837C-63554151C8F5}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{BDA5D613-596D-4B61-837C-63554151C8F5}.Release|Any CPU.Build.0 = Release|Any CPU
		{91F6F646-4F6E-449A-9AB4-2986348F329D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{91F6F646-4F6E-449A-9AB4-2986348F329D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{91F6F646-4F6E-449A-9AB4-2986348F329D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{91F6F646-4F6E-449A-9AB4-2986348F329D}.Release|Any CPU.Build.0 = Release|Any CPU
		{AFDE6BEA-5038-4A4A-A88E-DBD2E4088EED}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{AFDE6BEA-5038-4A4A-A88E-DBD2E4088EED}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{AFDE6BEA-5038-4A4A-A88E-DBD2E4088EED}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{AFDE6BEA-5038-4A4A-A88E-DBD2E4088EED}.Release|Any CPU.Build.0 = Release|Any CPU
		{5F8D2414-064A-4B3A-9B42-8E2A04246BE5}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{5F8D2414-064A-4B3A-9B42-8E2A04246BE5}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{5F8D2414-064A-4B3A-9B42-8E2A04246BE5}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{5F8D2414-064A-4B3A-9B42-8E2A04246BE5}.Release|Any CPU.Build.0 = Release|Any CPU
		{02459936-CD2C-4F61-B671-5C518F2A3DDC}.Debug|Any CPU.ActiveCfg = Debug|x64
		{02459936-CD2C-4F61-B671-5C518F2A3DDC}.Debug|Any CPU.Build.0 = Debug|x64
		{02459936-CD2C-4F61-B671-5C518F2A3DDC}.Release|Any CPU.ActiveCfg = Release|x64
		{02459936-CD2C-4F61-B671-5C518F2A3DDC}.Release|Any CPU.Build.0 = Release|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2E}.Debug|Any CPU.ActiveCfg = Debug|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2E}.Debug|Any CPU.Build.0 = Debug|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2E}.Release|Any CPU.ActiveCfg = Release|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2E}.Release|Any CPU.Build.0 = Release|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2D}.Debug|Any CPU.ActiveCfg = Debug|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2D}.Debug|Any CPU.Build.0 = Debug|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2D}.Release|Any CPU.ActiveCfg = Release|x64
		{288089C5-8721-458E-BE3E-78990DAB5E2D}.Release|Any CPU.Build.0 = Release|x64
		{6AA96934-D6B7-4CC8-990D-DB6B9DD56E34}.Debug|Any CPU.ActiveCfg = Debug|x64
		{6AA96934-D6B7-4CC8-990D-DB6B9DD56E34}.Debug|Any CPU.Build.0 = Debug|x64
		{6AA96934-D6B7-4CC8-990D-DB6B9DD56E34}.Release|Any CPU.ActiveCfg = Release|x64
		{6AA96934-D6B7-4CC8-990D-DB6B9DD56E34}.Release|Any CPU.Build.0 = Release|x64
		{C50166F1-BABC-40A9-95EB-8200080CD701}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{C50166F1-BABC-40A9-95EB-8200080CD701}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{C50166F1-BABC-40A9-95EB-8200080CD701}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{C50166F1-BABC-40A9-95EB-8200080CD701}.Release|Any CPU.Build.0 = Release|Any CPU
		{E195A63F-B5A4-4C5A-96BD-8E7ED6A181B7}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{E195A63F-B5A4-4C5A-96BD-8E7ED6A181B7}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{E195A63F-B5A4-4C5A-96BD-8E7ED6A181B7}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{E195A63F-B5A4-4C5A-96BD-8E7ED6A181B7}.Release|Any CPU.Build.0 = Release|Any CPU
		{E3FDC65F-568D-4E2D-A093-5132FD3793B7}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{E3FDC65F-568D-4E2D-A093-5132FD3793B7}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{E3FDC65F-568D-4E2D-A093-5132FD3793B7}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{E3FDC65F-568D-4E2D-A093-5132FD3793B7}.Release|Any CPU.Build.0 = Release|Any CPU
		{909B656F-6095-4AC2-A5AB-C3F032315C45}.Debug|Any CPU.ActiveCfg = Debug|x64
		{909B656F-6095-4AC2-A5AB-C3F032315C45}.Debug|Any CPU.Build.0 = Debug|x64
		{909B656F-6095-4AC2-A5AB-C3F032315C45}.Release|Any CPU.ActiveCfg = Release|x64
		{909B656F-6095-4AC2-A5AB-C3F032315C45}.Release|Any CPU.Build.0 = Release|x64
		{2E87FA96-50BB-4607-8676-46521599F998}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2E87FA96-50BB-4607-8676-46521599F998}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2E87FA96-50BB-4607-8676-46521599F998}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2E87FA96-50BB-4607-8676-46521599F998}.Release|Any CPU.Build.0 = Release|Any CPU
		{96EB2D3B-F694-48C6-A284-67382841E086}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{96EB2D3B-F694-48C6-A284-67382841E086}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{96EB2D3B-F694-48C6-A284-67382841E086}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{96EB2D3B-F694-48C6-A284-67382841E086}.Release|Any CPU.Build.0 = Release|Any CPU
		{21B239D0-D144-430F-A394-C066D58EE267}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{21B239D0-D144-430F-A394-C066D58EE267}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{21B239D0-D144-430F-A394-C066D58EE267}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{21B239D0-D144-430F-A394-C066D58EE267}.Release|Any CPU.Build.0 = Release|Any CPU
		{57CA988D-F010-4BF2-9A2E-07D6DCD2FF2C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{57CA988D-F010-4BF2-9A2E-07D6DCD2FF2C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{57CA988D-F010-4BF2-9A2E-07D6DCD2FF2C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{57CA988D-F010-4BF2-9A2E-07D6DCD2FF2C}.Release|Any CPU.Build.0 = Release|Any CPU
		{1A3941F1-1E1F-4EF7-8064-7729C4C2E2AA}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1A3941F1-1E1F-4EF7-8064-7729C4C2E2AA}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1A3941F1-1E1F-4EF7-8064-7729C4C2E2AA}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1A3941F1-1E1F-4EF7-8064-7729C4C2E2AA}.Release|Any CPU.Build.0 = Release|Any CPU
		{A1BCD0CE-6C2F-4F8C-9A48-D9D93928E26D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A1BCD0CE-6C2F-4F8C-9A48-D9D93928E26D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A1BCD0CE-6C2F-4F8C-9A48-D9D93928E26D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A1BCD0CE-6C2F-4F8C-9A48-D9D93928E26D}.Release|Any CPU.Build.0 = Release|Any CPU
		{3973B09A-4FBF-44A5-8359-3D22CEB71F71}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3973B09A-4FBF-44A5-8359-3D22CEB71F71}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3973B09A-4FBF-44A5-8359-3D22CEB71F71}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3973B09A-4FBF-44A5-8359-3D22CEB71F71}.Release|Any CPU.Build.0 = Release|Any CPU
		{EDC68A0E-C68D-4A74-91B7-BF38EC909888}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{EDC68A0E-C68D-4A74-91B7-BF38EC909888}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{EDC68A0E-C68D-4A74-91B7-BF38EC909888}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{EDC68A0E-C68D-4A74-91B7-BF38EC909888}.Release|Any CPU.Build.0 = Release|Any CPU
		{18F5FBB8-7570-4412-8CC7-0A86FF13B7BA}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{18F5FBB8-7570-4412-8CC7-0A86FF13B7BA}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{18F5FBB8-7570-4412-8CC7-0A86FF13B7BA}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{18F5FBB8-7570-4412-8CC7-0A86FF13B7BA}.Release|Any CPU.Build.0 = Release|Any CPU
		{49BFAE50-1BCE-48AE-BC89-78B7D90A3ECD}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{49BFAE50-1BCE-48AE-BC89-78B7D90A3ECD}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{49BFAE50-1BCE-48AE-BC89-78B7D90A3ECD}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{49BFAE50-1BCE-48AE-BC89-78B7D90A3ECD}.Release|Any CPU.Build.0 = Release|Any CPU
		{B0CE9307-FFDB-4838-A5EC-CE1F7CDC4AC2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B0CE9307-FFDB-4838-A5EC-CE1F7CDC4AC2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B0CE9307-FFDB-4838-A5EC-CE1F7CDC4AC2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B0CE9307-FFDB-4838-A5EC-CE1F7CDC4AC2}.Release|Any CPU.Build.0 = Release|Any CPU
		{3CDEEAB7-2256-418A-BEB2-620B5CB16302}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3CDEEAB7-2256-418A-BEB2-620B5CB16302}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3CDEEAB7-2256-418A-BEB2-620B5CB16302}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3CDEEAB7-2256-418A-BEB2-620B5CB16302}.Release|Any CPU.Build.0 = Release|Any CPU
		{0BE66736-CDAA-4989-88B1-B3F46EBDCA4A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0BE66736-CDAA-4989-88B1-B3F46EBDCA4A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0BE66736-CDAA-4989-88B1-B3F46EBDCA4A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0BE66736-CDAA-4989-88B1-B3F46EBDCA4A}.Release|Any CPU.Build.0 = Release|Any CPU
		{3E7DEA65-317B-4F43-A25D-62F18D96CFD7}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3E7DEA65-317B-4F43-A25D-62F18D96CFD7}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3E7DEA65-317B-4F43-A25D-62F18D96CFD7}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3E7DEA65-317B-4F43-A25D-62F18D96CFD7}.Release|Any CPU.Build.0 = Release|Any CPU
		{12A68549-4E8C-42D6-8703-A09335F97997}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{12A68549-4E8C-42D6-8703-A09335F97997}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{12A68549-4E8C-42D6-8703-A09335F97997}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{12A68549-4E8C-42D6-8703-A09335F97997}.Release|Any CPU.Build.0 = Release|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5472CE47CE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5472CE47CE}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5472CE47CE}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5472CE47CE}.Release|Any CPU.Build.0 = Release|Any CPU
		{066F0DBD-C46C-4C20-AFEC-99829A172625}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{066F0DBD-C46C-4C20-AFEC-99829A172625}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{066F0DBD-C46C-4C20-AFEC-99829A172625}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{066F0DBD-C46C-4C20-AFEC-99829A172625}.Release|Any CPU.Build.0 = Release|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5422CE47CE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5422CE47CE}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5422CE47CE}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2DAE4406-7A89-4B5F-95C3-BC5422CE47CE}.Release|Any CPU.Build.0 = Release|Any CPU
		{8E2A252E-A140-45A6-A81A-2652996EA589}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{8E2A252E-A140-45A6-A81A-2652996EA589}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{8E2A252E-A140-45A6-A81A-2652996EA589}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{8E2A252E-A140-45A6-A81A-2652996EA589}.Release|Any CPU.Build.0 = Release|Any CPU
		{AC2BCEFB-9298-4621-AC48-1FF5E639E48D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{AC2BCEFB-9298-4621-AC48-1FF5E639E48D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{AC2BCEFB-9298-4621-AC48-1FF5E639E48D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{AC2BCEFB-9298-4621-AC48-1FF5E639E48D}.Release|Any CPU.Build.0 = Release|Any CPU
		{16E93074-4252-466C-89A3-3B905ABAF779}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{16E93074-4252-466C-89A3-3B905ABAF779}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{16E93074-4252-466C-89A3-3B905ABAF779}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{16E93074-4252-466C-89A3-3B905ABAF779}.Release|Any CPU.Build.0 = Release|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B33}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B33}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B33}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B33}.Release|Any CPU.Build.0 = Release|Any CPU
		{3CEA0D69-00D3-40E5-A661-DC41EA07269B}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3CEA0D69-00D3-40E5-A661-DC41EA07269B}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3CEA0D69-00D3-40E5-A661-DC41EA07269B}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3CEA0D69-00D3-40E5-A661-DC41EA07269B}.Release|Any CPU.Build.0 = Release|Any CPU
		{76C6F005-C89D-4348-BB4A-39189DDBEB52}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{76C6F005-C89D-4348-BB4A-39189DDBEB52}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{76C6F005-C89D-4348-BB4A-39189DDBEB52}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{76C6F005-C89D-4348-BB4A-39189DDBEB52}.Release|Any CPU.Build.0 = Release|Any CPU
		{FE2CBEA6-D121-4FAA-AA8B-FC9900BF8C83}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FE2CBEA6-D121-4FAA-AA8B-FC9900BF8C83}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FE2CBEA6-D121-4FAA-AA8B-FC9900BF8C83}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FE2CBEA6-D121-4FAA-AA8B-FC9900BF8C83}.Release|Any CPU.Build.0 = Release|Any CPU
		{EBA4DFA1-6DED-418F-A485-A3B608978906}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{EBA4DFA1-6DED-418F-A485-A3B608978906}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{EBA4DFA1-6DED-418F-A485-A3B608978906}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{EBA4DFA1-6DED-418F-A485-A3B608978906}.Release|Any CPU.Build.0 = Release|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B34}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B34}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B34}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B34}.Release|Any CPU.Build.0 = Release|Any CPU
		{14118347-ED06-4608-9C45-18228273C712}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{14118347-ED06-4608-9C45-18228273C712}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{14118347-ED06-4608-9C45-18228273C712}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{14118347-ED06-4608-9C45-18228273C712}.Release|Any CPU.Build.0 = Release|Any CPU
		{6E62A0FF-D0DC-4109-9131-AB8E60CDFF7B}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{6E62A0FF-D0DC-4109-9131-AB8E60CDFF7B}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{6E62A0FF-D0DC-4109-9131-AB8E60CDFF7B}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{6E62A0FF-D0DC-4109-9131-AB8E60CDFF7B}.Release|Any CPU.Build.0 = Release|Any CPU
		{86FD5B9A-4FA0-4B10-B59F-CFAF077A859C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{86FD5B9A-4FA0-4B10-B59F-CFAF077A859C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{86FD5B9A-4FA0-4B10-B59F-CFAF077A859C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{86FD5B9A-4FA0-4B10-B59F-CFAF077A859C}.Release|Any CPU.Build.0 = Release|Any CPU
		{C0E80510-4FBE-4B0C-AF2C-4F473787722C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{C0E80510-4FBE-4B0C-AF2C-4F473787722C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{C0E80510-4FBE-4B0C-AF2C-4F473787722C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{C0E80510-4FBE-4B0C-AF2C-4F473787722C}.Release|Any CPU.Build.0 = Release|Any CPU
		{7BE3DEEB-87F8-4E15-9C21-4F94B0B1C2D6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{7BE3DEEB-87F8-4E15-9C21-4F94B0B1C2D6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{7BE3DEEB-87F8-4E15-9C21-4F94B0B1C2D6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{7BE3DEEB-87F8-4E15-9C21-4F94B0B1C2D6}.Release|Any CPU.Build.0 = Release|Any CPU
		{D49439D7-56D2-450F-A4F0-74CB95D620E6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{D49439D7-56D2-450F-A4F0-74CB95D620E6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{D49439D7-56D2-450F-A4F0-74CB95D620E6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{D49439D7-56D2-450F-A4F0-74CB95D620E6}.Release|Any CPU.Build.0 = Release|Any CPU
		{5DEFADBD-44EB-47A2-A53E-F1282CC9E4E9}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{5DEFADBD-44EB-47A2-A53E-F1282CC9E4E9}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{5DEFADBD-44EB-47A2-A53E-F1282CC9E4E9}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{5DEFADBD-44EB-47A2-A53E-F1282CC9E4E9}.Release|Any CPU.Build.0 = Release|Any CPU
		{91C574AD-0352-47E9-A019-EE02CC32A396}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{91C574AD-0352-47E9-A019-EE02CC32A396}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{91C574AD-0352-47E9-A019-EE02CC32A396}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{91C574AD-0352-47E9-A019-EE02CC32A396}.Release|Any CPU.Build.0 = Release|Any CPU
		{A1455D30-55FC-45EF-8759-3AEBDB13D940}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A1455D30-55FC-45EF-8759-3AEBDB13D940}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A1455D30-55FC-45EF-8759-3AEBDB13D940}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A1455D30-55FC-45EF-8759-3AEBDB13D940}.Release|Any CPU.Build.0 = Release|Any CPU
		{201EC5B7-F91E-45E5-B9F2-67A266CCE6FC}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{201EC5B7-F91E-45E5-B9F2-67A266CCE6FC}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{201EC5B7-F91E-45E5-B9F2-67A266CCE6FC}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{201EC5B7-F91E-45E5-B9F2-67A266CCE6FC}.Release|Any CPU.Build.0 = Release|Any CPU
		{2169F526-8A88-435D-8732-486ACA095A6A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2169F526-8A88-435D-8732-486ACA095A6A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2169F526-8A88-435D-8732-486ACA095A6A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2169F526-8A88-435D-8732-486ACA095A6A}.Release|Any CPU.Build.0 = Release|Any CPU
		{A486D7DE-F614-409D-BB41-0FFDF582E35C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A486D7DE-F614-409D-BB41-0FFDF582E35C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A486D7DE-F614-409D-BB41-0FFDF582E35C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A486D7DE-F614-409D-BB41-0FFDF582E35C}.Release|Any CPU.Build.0 = Release|Any CPU
		{B617717C-7881-4F01-AB6D-B1B6CC0483A0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B617717C-7881-4F01-AB6D-B1B6CC0483A0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B617717C-7881-4F01-AB6D-B1B6CC0483A0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B617717C-7881-4F01-AB6D-B1B6CC0483A0}.Release|Any CPU.Build.0 = Release|Any CPU
		{FD6BA96C-7905-4876-8BCC-E38E2CA64F31}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FD6BA96C-7905-4876-8BCC-E38E2CA64F31}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FD6BA96C-7905-4876-8BCC-E38E2CA64F31}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FD6BA96C-7905-4876-8BCC-E38E2CA64F31}.Release|Any CPU.Build.0 = Release|Any CPU
		{AE297965-4D56-4BA9-85EB-655AC4FC95A0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{AE297965-4D56-4BA9-85EB-655AC4FC95A0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{AE297965-4D56-4BA9-85EB-655AC4FC95A0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{AE297965-4D56-4BA9-85EB-655AC4FC95A0}.Release|Any CPU.Build.0 = Release|Any CPU
		{60DB272A-21C9-4E8D-9803-FF4E132392C8}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{60DB272A-21C9-4E8D-9803-FF4E132392C8}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{60DB272A-21C9-4E8D-9803-FF4E132392C8}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{60DB272A-21C9-4E8D-9803-FF4E132392C8}.Release|Any CPU.Build.0 = Release|Any CPU
		{73242A2D-6300-499D-8C15-FADF7ECB185C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{73242A2D-6300-499D-8C15-FADF7ECB185C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{73242A2D-6300-499D-8C15-FADF7ECB185C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{73242A2D-6300-499D-8C15-FADF7ECB185C}.Release|Any CPU.Build.0 = Release|Any CPU
		{AC5E3515-482C-4C6A-92D9-D0CEA687DFC2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{AC5E3515-482C-4C6A-92D9-D0CEA687DFC2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{AC5E3515-482C-4C6A-92D9-D0CEA687DFC2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{AC5E3515-482C-4C6A-92D9-D0CEA687DFC2}.Release|Any CPU.Build.0 = Release|Any CPU
		{B8DA3A90-A60C-42E3-9D8E-6C67B800C395}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B8DA3A90-A60C-42E3-9D8E-6C67B800C395}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B8DA3A90-A60C-42E3-9D8E-6C67B800C395}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B8DA3A90-A60C-42E3-9D8E-6C67B800C395}.Release|Any CPU.Build.0 = Release|Any CPU
		{ACE53515-482C-4C6A-E2D2-4242A687DFEE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{ACE53515-482C-4C6A-E2D2-4242A687DFEE}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{ACE53515-482C-4C6A-E2D2-4242A687DFEE}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{ACE53515-482C-4C6A-E2D2-4242A687DFEE}.Release|Any CPU.Build.0 = Release|Any CPU
		{21B80A31-8FF9-4E3A-8403-AABD635AEED9}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{21B80A31-8FF9-4E3A-8403-AABD635AEED9}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{21B80A31-8FF9-4E3A-8403-AABD635AEED9}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{21B80A31-8FF9-4E3A-8403-AABD635AEED9}.Release|Any CPU.Build.0 = Release|Any CPU
		{ABDBAC1E-350E-4DC3-BB45-3504404545EE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{ABDBAC1E-350E-4DC3-BB45-3504404545EE}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{ABDBAC1E-350E-4DC3-BB45-3504404545EE}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{ABDBAC1E-350E-4DC3-BB45-3504404545EE}.Release|Any CPU.Build.0 = Release|Any CPU
		{FCFA8808-A1B6-48CC-A1EA-0B8CA8AEDA8E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FCFA8808-A1B6-48CC-A1EA-0B8CA8AEDA8E}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FCFA8808-A1B6-48CC-A1EA-0B8CA8AEDA8E}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FCFA8808-A1B6-48CC-A1EA-0B8CA8AEDA8E}.Release|Any CPU.Build.0 = Release|Any CPU
		{1DFEA9C5-973C-4179-9B1B-0F32288E1EF2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1DFEA9C5-973C-4179-9B1B-0F32288E1EF2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1DFEA9C5-973C-4179-9B1B-0F32288E1EF2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1DFEA9C5-973C-4179-9B1B-0F32288E1EF2}.Release|Any CPU.Build.0 = Release|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1842}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1842}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1842}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1842}.Release|Any CPU.Build.0 = Release|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1843}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1843}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1843}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{76242A2D-2600-49DD-8C15-FEA07ECB1843}.Release|Any CPU.Build.0 = Release|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D3}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D3}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D3}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D3}.Release|Any CPU.Build.0 = Release|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D4}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D4}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D4}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D4}.Release|Any CPU.Build.0 = Release|Any CPU
		{BEDC5A4A-809E-4017-9CFD-6C8D4E1847F0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{BEDC5A4A-809E-4017-9CFD-6C8D4E1847F0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{BEDC5A4A-809E-4017-9CFD-6C8D4E1847F0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{BEDC5A4A-809E-4017-9CFD-6C8D4E1847F0}.Release|Any CPU.Build.0 = Release|Any CPU
		{FA0E905D-EC46-466D-B7B2-3B5557F9428C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FA0E905D-EC46-466D-B7B2-3B5557F9428C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FA0E905D-EC46-466D-B7B2-3B5557F9428C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FA0E905D-EC46-466D-B7B2-3B5557F9428C}.Release|Any CPU.Build.0 = Release|Any CPU
		{E58EE9D7-1239-4961-A0C1-F9EC3952C4C1}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{E58EE9D7-1239-4961-A0C1-F9EC3952C4C1}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{E58EE9D7-1239-4961-A0C1-F9EC3952C4C1}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{E58EE9D7-1239-4961-A0C1-F9EC3952C4C1}.Release|Any CPU.Build.0 = Release|Any CPU
		{CCBD3438-3E84-40A9-83AD-533F23BCFCA5}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{CCBD3438-3E84-40A9-83AD-533F23BCFCA5}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{CCBD3438-3E84-40A9-83AD-533F23BCFCA5}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{CCBD3438-3E84-40A9-83AD-533F23BCFCA5}.Release|Any CPU.Build.0 = Release|Any CPU
		{6FD1CC3E-6A99-4736-9B8D-757992DDE75D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{6FD1CC3E-6A99-4736-9B8D-757992DDE75D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{6FD1CC3E-6A99-4736-9B8D-757992DDE75D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{6FD1CC3E-6A99-4736-9B8D-757992DDE75D}.Release|Any CPU.Build.0 = Release|Any CPU
		{286B01F3-811A-40A7-8C1F-10C9BB0597F7}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{286B01F3-811A-40A7-8C1F-10C9BB0597F7}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{286B01F3-811A-40A7-8C1F-10C9BB0597F7}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{286B01F3-811A-40A7-8C1F-10C9BB0597F7}.Release|Any CPU.Build.0 = Release|Any CPU
		{24973B4C-FD09-4EE1-97F4-EA03E6B12040}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{24973B4C-FD09-4EE1-97F4-EA03E6B12040}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{24973B4C-FD09-4EE1-97F4-EA03E6B12040}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{24973B4C-FD09-4EE1-97F4-EA03E6B12040}.Release|Any CPU.Build.0 = Release|Any CPU
		{ABC7262E-1053-49F3-B846-E3091BB92E8C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{ABC7262E-1053-49F3-B846-E3091BB92E8C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{ABC7262E-1053-49F3-B846-E3091BB92E8C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{ABC7262E-1053-49F3-B846-E3091BB92E8C}.Release|Any CPU.Build.0 = Release|Any CPU
		{E2E889A5-2489-4546-9194-47C63E49EAEB}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{E2E889A5-2489-4546-9194-47C63E49EAEB}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{E2E889A5-2489-4546-9194-47C63E49EAEB}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{E2E889A5-2489-4546-9194-47C63E49EAEB}.Release|Any CPU.Build.0 = Release|Any CPU
		{43026D51-3083-4850-928D-07E1883D5B1A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{43026D51-3083-4850-928D-07E1883D5B1A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{43026D51-3083-4850-928D-07E1883D5B1A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{43026D51-3083-4850-928D-07E1883D5B1A}.Release|Any CPU.Build.0 = Release|Any CPU
		{A88AB44F-7F9D-43F6-A127-83BB65E5A7E2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A88AB44F-7F9D-43F6-A127-83BB65E5A7E2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A88AB44F-7F9D-43F6-A127-83BB65E5A7E2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A88AB44F-7F9D-43F6-A127-83BB65E5A7E2}.Release|Any CPU.Build.0 = Release|Any CPU
		{E5A55C16-A5B9-4874-9043-A5266DC02F58}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{E5A55C16-A5B9-4874-9043-A5266DC02F58}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{E5A55C16-A5B9-4874-9043-A5266DC02F58}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{E5A55C16-A5B9-4874-9043-A5266DC02F58}.Release|Any CPU.Build.0 = Release|Any CPU
		{3BED15FD-D608-4573-B432-1569C1026F6D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3BED15FD-D608-4573-B432-1569C1026F6D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3BED15FD-D608-4573-B432-1569C1026F6D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3BED15FD-D608-4573-B432-1569C1026F6D}.Release|Any CPU.Build.0 = Release|Any CPU
		{DA0D2A70-A2F9-4654-A99A-3227EDF54FF1}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{DA0D2A70-A2F9-4654-A99A-3227EDF54FF1}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{DA0D2A70-A2F9-4654-A99A-3227EDF54FF1}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{DA0D2A70-A2F9-4654-A99A-3227EDF54FF1}.Release|Any CPU.Build.0 = Release|Any CPU
		{971E832B-7471-48B5-833E-5913188EC0E4}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{971E832B-7471-48B5-833E-5913188EC0E4}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{971E832B-7471-48B5-833E-5913188EC0E4}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{971E832B-7471-48B5-833E-5913188EC0E4}.Release|Any CPU.Build.0 = Release|Any CPU
		{59AD474E-2A35-4E8A-A74D-E33479977FBF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{59AD474E-2A35-4E8A-A74D-E33479977FBF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{59AD474E-2A35-4E8A-A74D-E33479977FBF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{59AD474E-2A35-4E8A-A74D-E33479977FBF}.Release|Any CPU.Build.0 = Release|Any CPU
		{F822F72A-CC87-4E31-B57D-853F65CBEBF3}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{F822F72A-CC87-4E31-B57D-853F65CBEBF3}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{F822F72A-CC87-4E31-B57D-853F65CBEBF3}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{F822F72A-CC87-4E31-B57D-853F65CBEBF3}.Release|Any CPU.Build.0 = Release|Any CPU
		{80FDDD00-9393-47F7-8BAF-7E87CE011068}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{80FDDD00-9393-47F7-8BAF-7E87CE011068}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{80FDDD00-9393-47F7-8BAF-7E87CE011068}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{80FDDD00-9393-47F7-8BAF-7E87CE011068}.Release|Any CPU.Build.0 = Release|Any CPU
		{7AD4FE65-9A30-41A6-8004-AA8F89BCB7F3}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{7AD4FE65-9A30-41A6-8004-AA8F89BCB7F3}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{7AD4FE65-9A30-41A6-8004-AA8F89BCB7F3}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{7AD4FE65-9A30-41A6-8004-AA8F89BCB7F3}.Release|Any CPU.Build.0 = Release|Any CPU
		{2E1658E2-5045-4F85-A64C-C0ECCD39F719}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2E1658E2-5045-4F85-A64C-C0ECCD39F719}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2E1658E2-5045-4F85-A64C-C0ECCD39F719}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2E1658E2-5045-4F85-A64C-C0ECCD39F719}.Release|Any CPU.Build.0 = Release|Any CPU
		{9C0660D9-48CA-40E1-BABA-8F6A1F11FE10}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{9C0660D9-48CA-40E1-BABA-8F6A1F11FE10}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{9C0660D9-48CA-40E1-BABA-8F6A1F11FE10}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{9C0660D9-48CA-40E1-BABA-8F6A1F11FE10}.Release|Any CPU.Build.0 = Release|Any CPU
		{21A01C2D-2501-4619-8144-48977DD22D9C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{21A01C2D-2501-4619-8144-48977DD22D9C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{21A01C2D-2501-4619-8144-48977DD22D9C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{21A01C2D-2501-4619-8144-48977DD22D9C}.Release|Any CPU.Build.0 = Release|Any CPU
		{3F2FDC1C-DC6F-44CB-B4A1-A9026F25D66E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3F2FDC1C-DC6F-44CB-B4A1-A9026F25D66E}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3F2FDC1C-DC6F-44CB-B4A1-A9026F25D66E}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3F2FDC1C-DC6F-44CB-B4A1-A9026F25D66E}.Release|Any CPU.Build.0 = Release|Any CPU
		{3DFB4701-E3D6-4435-9F70-A6E35822C4F2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3DFB4701-E3D6-4435-9F70-A6E35822C4F2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3DFB4701-E3D6-4435-9F70-A6E35822C4F2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3DFB4701-E3D6-4435-9F70-A6E35822C4F2}.Release|Any CPU.Build.0 = Release|Any CPU
		{69F853E5-BD04-4842-984F-FC68CC51F402}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{69F853E5-BD04-4842-984F-FC68CC51F402}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{69F853E5-BD04-4842-984F-FC68CC51F402}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{69F853E5-BD04-4842-984F-FC68CC51F402}.Release|Any CPU.Build.0 = Release|Any CPU
		{6FC8E6F5-659C-424D-AEB5-331B95883E29}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{6FC8E6F5-659C-424D-AEB5-331B95883E29}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{6FC8E6F5-659C-424D-AEB5-331B95883E29}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{6FC8E6F5-659C-424D-AEB5-331B95883E29}.Release|Any CPU.Build.0 = Release|Any CPU
		{DD317BE1-42A1-4795-B1D4-F370C40D649A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{DD317BE1-42A1-4795-B1D4-F370C40D649A}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{DD317BE1-42A1-4795-B1D4-F370C40D649A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{DD317BE1-42A1-4795-B1D4-F370C40D649A}.Release|Any CPU.Build.0 = Release|Any CPU
		{0C0EEB55-4B6D-4F2B-B0BB-B9EB2BA9E980}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0C0EEB55-4B6D-4F2B-B0BB-B9EB2BA9E980}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0C0EEB55-4B6D-4F2B-B0BB-B9EB2BA9E980}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0C0EEB55-4B6D-4F2B-B0BB-B9EB2BA9E980}.Release|Any CPU.Build.0 = Release|Any CPU
		{B6FC05F2-0E49-4BE2-8030-ACBB82B7F431}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B6FC05F2-0E49-4BE2-8030-ACBB82B7F431}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B6FC05F2-0E49-4BE2-8030-ACBB82B7F431}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B6FC05F2-0E49-4BE2-8030-ACBB82B7F431}.Release|Any CPU.Build.0 = Release|Any CPU
		{1688E1E5-D510-4E06-86F3-F8DB10B1393D}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1688E1E5-D510-4E06-86F3-F8DB10B1393D}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1688E1E5-D510-4E06-86F3-F8DB10B1393D}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1688E1E5-D510-4E06-86F3-F8DB10B1393D}.Release|Any CPU.Build.0 = Release|Any CPU
		{F040CEC5-5E11-4DBD-9F6A-250478E28177}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{F040CEC5-5E11-4DBD-9F6A-250478E28177}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{F040CEC5-5E11-4DBD-9F6A-250478E28177}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{F040CEC5-5E11-4DBD-9F6A-250478E28177}.Release|Any CPU.Build.0 = Release|Any CPU
		{275812EE-DEDB-4232-9439-91C9757D2AE4}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{275812EE-DEDB-4232-9439-91C9757D2AE4}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{275812EE-DEDB-4232-9439-91C9757D2AE4}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{275812EE-DEDB-4232-9439-91C9757D2AE4}.Release|Any CPU.Build.0 = Release|Any CPU
		{5FF1E493-69CC-4D0B-83F2-039F469A04E1}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{5FF1E493-69CC-4D0B-83F2-039F469A04E1}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{5FF1E493-69CC-4D0B-83F2-039F469A04E1}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{5FF1E493-69CC-4D0B-83F2-039F469A04E1}.Release|Any CPU.Build.0 = Release|Any CPU
		{AA87BFED-089A-4096-B8D5-690BDC7D5B24}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{AA87BFED-089A-4096-B8D5-690BDC7D5B24}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{AA87BFED-089A-4096-B8D5-690BDC7D5B24}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{AA87BFED-089A-4096-B8D5-690BDC7D5B24}.Release|Any CPU.Build.0 = Release|Any CPU
		{A07ABCF5-BC43-4EE9-8FD8-B2D77FD54D73}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A07ABCF5-BC43-4EE9-8FD8-B2D77FD54D73}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A07ABCF5-BC43-4EE9-8FD8-B2D77FD54D73}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A07ABCF5-BC43-4EE9-8FD8-B2D77FD54D73}.Release|Any CPU.Build.0 = Release|Any CPU
		{2531A8C4-97DD-47BC-A79C-B7846051E137}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2531A8C4-97DD-47BC-A79C-B7846051E137}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2531A8C4-97DD-47BC-A79C-B7846051E137}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2531A8C4-97DD-47BC-A79C-B7846051E137}.Release|Any CPU.Build.0 = Release|Any CPU
		{0141285D-8F6C-42C7-BAF3-3C0CCD61C716}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0141285D-8F6C-42C7-BAF3-3C0CCD61C716}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0141285D-8F6C-42C7-BAF3-3C0CCD61C716}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0141285D-8F6C-42C7-BAF3-3C0CCD61C716}.Release|Any CPU.Build.0 = Release|Any CPU
		{9FF1205F-1D7C-4EE4-B038-3456FE6EBEAF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{9FF1205F-1D7C-4EE4-B038-3456FE6EBEAF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{9FF1205F-1D7C-4EE4-B038-3456FE6EBEAF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{9FF1205F-1D7C-4EE4-B038-3456FE6EBEAF}.Release|Any CPU.Build.0 = Release|Any CPU
		{5018D049-5870-465A-889B-C742CE1E31CB}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{5018D049-5870-465A-889B-C742CE1E31CB}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{5018D049-5870-465A-889B-C742CE1E31CB}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{5018D049-5870-465A-889B-C742CE1E31CB}.Release|Any CPU.Build.0 = Release|Any CPU
		{E512C6C1-F085-4AD7-B0D9-E8F1A0A2A510}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{E512C6C1-F085-4AD7-B0D9-E8F1A0A2A510}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{E512C6C1-F085-4AD7-B0D9-E8F1A0A2A510}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{E512C6C1-F085-4AD7-B0D9-E8F1A0A2A510}.Release|Any CPU.Build.0 = Release|Any CPU
		{2D36C343-BB6A-4CB5-902B-E2145ACCB58F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2D36C343-BB6A-4CB5-902B-E2145ACCB58F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2D36C343-BB6A-4CB5-902B-E2145ACCB58F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2D36C343-BB6A-4CB5-902B-E2145ACCB58F}.Release|Any CPU.Build.0 = Release|Any CPU
		{FFB00FB5-8C8C-4A02-B67D-262B9D28E8B1}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FFB00FB5-8C8C-4A02-B67D-262B9D28E8B1}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FFB00FB5-8C8C-4A02-B67D-262B9D28E8B1}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FFB00FB5-8C8C-4A02-B67D-262B9D28E8B1}.Release|Any CPU.Build.0 = Release|Any CPU
		{60166C60-813C-46C4-911D-2411B4ABBC0F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{60166C60-813C-46C4-911D-2411B4ABBC0F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{60166C60-813C-46C4-911D-2411B4ABBC0F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{60166C60-813C-46C4-911D-2411B4ABBC0F}.Release|Any CPU.Build.0 = Release|Any CPU
		{FC2AE90B-2E4B-4045-9FDD-73D4F5ED6C89}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FC2AE90B-2E4B-4045-9FDD-73D4F5ED6C89}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FC2AE90B-2E4B-4045-9FDD-73D4F5ED6C89}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FC2AE90B-2E4B-4045-9FDD-73D4F5ED6C89}.Release|Any CPU.Build.0 = Release|Any CPU
		{49E7C367-181B-499C-AC2E-8E17C81418D6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{49E7C367-181B-499C-AC2E-8E17C81418D6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{49E7C367-181B-499C-AC2E-8E17C81418D6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{49E7C367-181B-499C-AC2E-8E17C81418D6}.Release|Any CPU.Build.0 = Release|Any CPU
		{037F06F0-3BE8-42D0-801E-2F74FC380AB8}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{037F06F0-3BE8-42D0-801E-2F74FC380AB8}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{037F06F0-3BE8-42D0-801E-2F74FC380AB8}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{037F06F0-3BE8-42D0-801E-2F74FC380AB8}.Release|Any CPU.Build.0 = Release|Any CPU
		{2F11618A-9251-4609-B3D5-CE4D2B3D3E49}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2F11618A-9251-4609-B3D5-CE4D2B3D3E49}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2F11618A-9251-4609-B3D5-CE4D2B3D3E49}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2F11618A-9251-4609-B3D5-CE4D2B3D3E49}.Release|Any CPU.Build.0 = Release|Any CPU
		{764D2C19-0187-4837-A2A3-96DDC6EF4CE2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{764D2C19-0187-4837-A2A3-96DDC6EF4CE2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{764D2C19-0187-4837-A2A3-96DDC6EF4CE2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{764D2C19-0187-4837-A2A3-96DDC6EF4CE2}.Release|Any CPU.Build.0 = Release|Any CPU
		{9102ECF3-5CD1-4107-B8B7-F3795A52D790}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{9102ECF3-5CD1-4107-B8B7-F3795A52D790}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{9102ECF3-5CD1-4107-B8B7-F3795A52D790}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{9102ECF3-5CD1-4107-B8B7-F3795A52D790}.Release|Any CPU.Build.0 = Release|Any CPU
		{50CF5D8F-F82F-4210-A06E-37CC9BFFDD49}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{50CF5D8F-F82F-4210-A06E-37CC9BFFDD49}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{50CF5D8F-F82F-4210-A06E-37CC9BFFDD49}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{50CF5D8F-F82F-4210-A06E-37CC9BFFDD49}.Release|Any CPU.Build.0 = Release|Any CPU
		{CFA94A39-4805-456D-A369-FC35CCC170E9}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{CFA94A39-4805-456D-A369-FC35CCC170E9}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{CFA94A39-4805-456D-A369-FC35CCC170E9}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{CFA94A39-4805-456D-A369-FC35CCC170E9}.Release|Any CPU.Build.0 = Release|Any CPU
		{4A490CBC-37F4-4859-AFDB-4B0833D144AF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{4A490CBC-37F4-4859-AFDB-4B0833D144AF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{4A490CBC-37F4-4859-AFDB-4B0833D144AF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{4A490CBC-37F4-4859-AFDB-4B0833D144AF}.Release|Any CPU.Build.0 = Release|Any CPU
		{34E868E9-D30B-4FB5-BC61-AFC4A9612A0F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{34E868E9-D30B-4FB5-BC61-AFC4A9612A0F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{34E868E9-D30B-4FB5-BC61-AFC4A9612A0F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{34E868E9-D30B-4FB5-BC61-AFC4A9612A0F}.Release|Any CPU.Build.0 = Release|Any CPU
		{0EB22BD1-B8B1-417D-8276-F475C2E190FF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0EB22BD1-B8B1-417D-8276-F475C2E190FF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0EB22BD1-B8B1-417D-8276-F475C2E190FF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0EB22BD1-B8B1-417D-8276-F475C2E190FF}.Release|Any CPU.Build.0 = Release|Any CPU
		{3636D3E2-E3EF-4815-B020-819F382204CD}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3636D3E2-E3EF-4815-B020-819F382204CD}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3636D3E2-E3EF-4815-B020-819F382204CD}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3636D3E2-E3EF-4815-B020-819F382204CD}.Release|Any CPU.Build.0 = Release|Any CPU
		{B9843F65-262E-4F40-A0BC-2CBEF7563A44}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B9843F65-262E-4F40-A0BC-2CBEF7563A44}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B9843F65-262E-4F40-A0BC-2CBEF7563A44}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B9843F65-262E-4F40-A0BC-2CBEF7563A44}.Release|Any CPU.Build.0 = Release|Any CPU
		{03607817-6800-40B6-BEAA-D6F437CD62B7}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{03607817-6800-40B6-BEAA-D6F437CD62B7}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{03607817-6800-40B6-BEAA-D6F437CD62B7}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{03607817-6800-40B6-BEAA-D6F437CD62B7}.Release|Any CPU.Build.0 = Release|Any CPU
		{6A68FDF9-24B3-4CB6-A808-96BF50D1BCE5}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{6A68FDF9-24B3-4CB6-A808-96BF50D1BCE5}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{6A68FDF9-24B3-4CB6-A808-96BF50D1BCE5}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{6A68FDF9-24B3-4CB6-A808-96BF50D1BCE5}.Release|Any CPU.Build.0 = Release|Any CPU
		{23405307-7EFF-4774-8B11-8F5885439761}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{23405307-7EFF-4774-8B11-8F5885439761}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{23405307-7EFF-4774-8B11-8F5885439761}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{23405307-7EFF-4774-8B11-8F5885439761}.Release|Any CPU.Build.0 = Release|Any CPU
		{6362616E-6A47-48F0-9EE0-27800B306ACB}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{6362616E-6A47-48F0-9EE0-27800B306ACB}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{6362616E-6A47-48F0-9EE0-27800B306ACB}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{6362616E-6A47-48F0-9EE0-27800B306ACB}.Release|Any CPU.Build.0 = Release|Any CPU
		{BD8CE303-5F04-45EC-8DCF-73C9164CD614}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{BD8CE303-5F04-45EC-8DCF-73C9164CD614}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{BD8CE303-5F04-45EC-8DCF-73C9164CD614}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{BD8CE303-5F04-45EC-8DCF-73C9164CD614}.Release|Any CPU.Build.0 = Release|Any CPU
		{2FB6C157-DF91-4B1C-9827-A4D1C08C73EC}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2FB6C157-DF91-4B1C-9827-A4D1C08C73EC}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2FB6C157-DF91-4B1C-9827-A4D1C08C73EC}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2FB6C157-DF91-4B1C-9827-A4D1C08C73EC}.Release|Any CPU.Build.0 = Release|Any CPU
		{5E6E9184-DEC5-4EC5-B0A4-77CFDC8CDEBE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{5E6E9184-DEC5-4EC5-B0A4-77CFDC8CDEBE}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{5E6E9184-DEC5-4EC5-B0A4-77CFDC8CDEBE}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{5E6E9184-DEC5-4EC5-B0A4-77CFDC8CDEBE}.Release|Any CPU.Build.0 = Release|Any CPU
		{A74C7D2E-92FA-490A-B80A-28BEF56B56FC}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{A74C7D2E-92FA-490A-B80A-28BEF56B56FC}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{A74C7D2E-92FA-490A-B80A-28BEF56B56FC}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{A74C7D2E-92FA-490A-B80A-28BEF56B56FC}.Release|Any CPU.Build.0 = Release|Any CPU
		{686BF57E-A6FF-467B-AAB3-44DE916A9772}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{686BF57E-A6FF-467B-AAB3-44DE916A9772}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{686BF57E-A6FF-467B-AAB3-44DE916A9772}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{686BF57E-A6FF-467B-AAB3-44DE916A9772}.Release|Any CPU.Build.0 = Release|Any CPU
		{1DDE89EE-5819-441F-A060-2FF4A986F372}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1DDE89EE-5819-441F-A060-2FF4A986F372}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1DDE89EE-5819-441F-A060-2FF4A986F372}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1DDE89EE-5819-441F-A060-2FF4A986F372}.Release|Any CPU.Build.0 = Release|Any CPU
		{655A5B07-39B8-48CD-8590-8AC0C2B708D8}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{655A5B07-39B8-48CD-8590-8AC0C2B708D8}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{655A5B07-39B8-48CD-8590-8AC0C2B708D8}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{655A5B07-39B8-48CD-8590-8AC0C2B708D8}.Release|Any CPU.Build.0 = Release|Any CPU
		{DE53934B-7FC1-48A0-85AB-C519FBBD02CF}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{DE53934B-7FC1-48A0-85AB-C519FBBD02CF}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{DE53934B-7FC1-48A0-85AB-C519FBBD02CF}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{DE53934B-7FC1-48A0-85AB-C519FBBD02CF}.Release|Any CPU.Build.0 = Release|Any CPU
		{3D33BBFD-EC63-4E8C-A714-0A48A3809A87}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{3D33BBFD-EC63-4E8C-A714-0A48A3809A87}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{3D33BBFD-EC63-4E8C-A714-0A48A3809A87}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{3D33BBFD-EC63-4E8C-A714-0A48A3809A87}.Release|Any CPU.Build.0 = Release|Any CPU
		{BFFB5CAE-33B5-447E-9218-BDEBFDA96CB5}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{BFFB5CAE-33B5-447E-9218-BDEBFDA96CB5}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{BFFB5CAE-33B5-447E-9218-BDEBFDA96CB5}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{BFFB5CAE-33B5-447E-9218-BDEBFDA96CB5}.Release|Any CPU.Build.0 = Release|Any CPU
		{FC32EF16-31B1-47B3-B625-A80933CB3F29}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FC32EF16-31B1-47B3-B625-A80933CB3F29}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FC32EF16-31B1-47B3-B625-A80933CB3F29}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FC32EF16-31B1-47B3-B625-A80933CB3F29}.Release|Any CPU.Build.0 = Release|Any CPU
		{453C8E28-81D4-431E-BFB0-F3D413346E51}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{453C8E28-81D4-431E-BFB0-F3D413346E51}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{453C8E28-81D4-431E-BFB0-F3D413346E51}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{453C8E28-81D4-431E-BFB0-F3D413346E51}.Release|Any CPU.Build.0 = Release|Any CPU
		{CE7F7553-DB2D-4839-92E3-F042E4261B4E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{CE7F7553-DB2D-4839-92E3-F042E4261B4E}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{CE7F7553-DB2D-4839-92E3-F042E4261B4E}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{CE7F7553-DB2D-4839-92E3-F042E4261B4E}.Release|Any CPU.Build.0 = Release|Any CPU
		{FF38E9C9-7A25-44F0-B2C4-24C9BFD6A8F6}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{FF38E9C9-7A25-44F0-B2C4-24C9BFD6A8F6}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{FF38E9C9-7A25-44F0-B2C4-24C9BFD6A8F6}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{FF38E9C9-7A25-44F0-B2C4-24C9BFD6A8F6}.Release|Any CPU.Build.0 = Release|Any CPU
		{D55FB2BD-CC9E-454B-9654-94AF5D910BF7}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{D55FB2BD-CC9E-454B-9654-94AF5D910BF7}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{D55FB2BD-CC9E-454B-9654-94AF5D910BF7}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{D55FB2BD-CC9E-454B-9654-94AF5D910BF7}.Release|Any CPU.Build.0 = Release|Any CPU
		{B9899CF1-E0EB-4599-9E24-6939A04B4979}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B9899CF1-E0EB-4599-9E24-6939A04B4979}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B9899CF1-E0EB-4599-9E24-6939A04B4979}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B9899CF1-E0EB-4599-9E24-6939A04B4979}.Release|Any CPU.Build.0 = Release|Any CPU
		{D15BF03E-04ED-4BEE-A72B-7620F541F4E2}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{D15BF03E-04ED-4BEE-A72B-7620F541F4E2}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{D15BF03E-04ED-4BEE-A72B-7620F541F4E2}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{D15BF03E-04ED-4BEE-A72B-7620F541F4E2}.Release|Any CPU.Build.0 = Release|Any CPU
		{B64766CD-1A1F-4C1B-B11F-C30F82B8E41E}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{B64766CD-1A1F-4C1B-B11F-C30F82B8E41E}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{B64766CD-1A1F-4C1B-B11F-C30F82B8E41E}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{B64766CD-1A1F-4C1B-B11F-C30F82B8E41E}.Release|Any CPU.Build.0 = Release|Any CPU
		{2D5E2DE4-5DA8-41C1-A14F-49855DCCE9C5}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{2D5E2DE4-5DA8-41C1-A14F-49855DCCE9C5}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{2D5E2DE4-5DA8-41C1-A14F-49855DCCE9C5}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{2D5E2DE4-5DA8-41C1-A14F-49855DCCE9C5}.Release|Any CPU.Build.0 = Release|Any CPU
		{CEA80C83-5848-4FF6-B4E8-CEEE9482E4AA}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{CEA80C83-5848-4FF6-B4E8-CEEE9482E4AA}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{CEA80C83-5848-4FF6-B4E8-CEEE9482E4AA}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{CEA80C83-5848-4FF6-B4E8-CEEE9482E4AA}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(NestedProjects) = preSolution
		{A41D1B99-F489-4C43-BBDF-96D61B19A6B9} = {3F40F71B-7DCF-44A1-B15C-38CA34824143}
		{32A48625-F0AD-419D-828B-A50BDABA38EA} = {3F40F71B-7DCF-44A1-B15C-38CA34824143}
		{C65C6143-BED3-46E6-869E-9F0BE6E84C37} = {3F40F71B-7DCF-44A1-B15C-38CA34824143}
		{913A4C08-898E-49C7-9692-0EF9DC56CF6E} = {235A3418-A3B0-4844-BCEB-F1CF45069232}
		{151F6994-AEB3-4B12-B746-2ACFF26C7BBB} = {235A3418-A3B0-4844-BCEB-F1CF45069232}
		{4C81EBB2-82E1-4C81-80C4-84CC40FA281B} = {235A3418-A3B0-4844-BCEB-F1CF45069232}
		{998CAFE8-06E4-4683-A151-0F6AA4BFF6C6} = {235A3418-A3B0-4844-BCEB-F1CF45069232}
		{19148439-436F-4CDA-B493-70AF4FFC13E9} = {999FBDA2-33DA-4F74-B957-03AC72CCE5EC}
		{5CA5F70E-0FDB-467B-B22C-3CD5994F0087} = {999FBDA2-33DA-4F74-B957-03AC72CCE5EC}
		{7E907718-0B33-45C8-851F-396CEFDC1AB6} = {3F40F71B-7DCF-44A1-B15C-38CA34824143}
		{CC126D03-7EAC-493F-B187-DCDEE1EF6A70} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{DD13507E-D5AF-4B61-B11A-D55D6F4A73A5} = {CAD2965A-19AB-489F-BE2E-7649957F914A}
		{DC014586-8D07-4DE6-B28E-C0540C59C085} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{A4C99B85-765C-4C65-9C2A-BB609AAB09E6} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{1EE8CAD3-55F9-4D91-96B2-084641DA9A6C} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{9508F118-F62E-4C16-A6F4-7C3B56E166AD} = {7E907718-0B33-45C8-851F-396CEFDC1AB6}
		{F5CE416E-B906-41D2-80B9-0078E887A3F6} = {7E907718-0B33-45C8-851F-396CEFDC1AB6}
		{4B45CA0C-03A0-400F-B454-3D4BCB16AF38} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{B501A547-C911-4A05-AC6E-274A50DFF30E} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{50D26304-0961-4A51-ABF6-6CAD1A56D203} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{4462B57A-7245-4146-B504-D46FDE762948} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{1AF3672A-C5F1-4604-B6AB-D98C4DE9C3B1} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{B2C33A93-DB30-4099-903E-77D75C4C3F45} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{28026D16-EB0C-40B0-BDA7-11CAA2B97CCC} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{50D26304-0961-4A51-ABF6-6CAD1A56D202} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{7FE6B002-89D8-4298-9B1B-0B5C247DD1FD} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F9} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{4371944A-D3BA-4B5B-8285-82E5FFC6D1F8} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{2523D0E6-DF32-4A3E-8AE0-A19BFFAE2EF6} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{E3B32027-3362-41DF-9172-4D3B623F42A5} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{190CE348-596E-435A-9E5B-12A689F9FC29} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{9C9DABA4-0E72-4469-ADF1-4991F3CA572A} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{BF180BD2-4FB7-4252-A7EC-A00E0C7A028A} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{BDA5D613-596D-4B61-837C-63554151C8F5} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{91F6F646-4F6E-449A-9AB4-2986348F329D} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{AFDE6BEA-5038-4A4A-A88E-DBD2E4088EED} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{5F8D2414-064A-4B3A-9B42-8E2A04246BE5} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{02459936-CD2C-4F61-B671-5C518F2A3DDC} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{288089C5-8721-458E-BE3E-78990DAB5E2E} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{288089C5-8721-458E-BE3E-78990DAB5E2D} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{6AA96934-D6B7-4CC8-990D-DB6B9DD56E34} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{C50166F1-BABC-40A9-95EB-8200080CD701} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{E195A63F-B5A4-4C5A-96BD-8E7ED6A181B7} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{E3FDC65F-568D-4E2D-A093-5132FD3793B7} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{909B656F-6095-4AC2-A5AB-C3F032315C45} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{2E87FA96-50BB-4607-8676-46521599F998} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{96EB2D3B-F694-48C6-A284-67382841E086} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{21B239D0-D144-430F-A394-C066D58EE267} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{57CA988D-F010-4BF2-9A2E-07D6DCD2FF2C} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{1A3941F1-1E1F-4EF7-8064-7729C4C2E2AA} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{A1BCD0CE-6C2F-4F8C-9A48-D9D93928E26D} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{3973B09A-4FBF-44A5-8359-3D22CEB71F71} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{EDC68A0E-C68D-4A74-91B7-BF38EC909888} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{18F5FBB8-7570-4412-8CC7-0A86FF13B7BA} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{49BFAE50-1BCE-48AE-BC89-78B7D90A3ECD} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{B0CE9307-FFDB-4838-A5EC-CE1F7CDC4AC2} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{3CDEEAB7-2256-418A-BEB2-620B5CB16302} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{0BE66736-CDAA-4989-88B1-B3F46EBDCA4A} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{3E7DEA65-317B-4F43-A25D-62F18D96CFD7} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{12A68549-4E8C-42D6-8703-A09335F97997} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{2DAE4406-7A89-4B5F-95C3-BC5472CE47CE} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{066F0DBD-C46C-4C20-AFEC-99829A172625} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{2DAE4406-7A89-4B5F-95C3-BC5422CE47CE} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{8E2A252E-A140-45A6-A81A-2652996EA589} = {5CA5F70E-0FDB-467B-B22C-3CD5994F0087}
		{AC2BCEFB-9298-4621-AC48-1FF5E639E48D} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{16E93074-4252-466C-89A3-3B905ABAF779} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B33} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{3CEA0D69-00D3-40E5-A661-DC41EA07269B} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{76C6F005-C89D-4348-BB4A-39189DDBEB52} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{FE2CBEA6-D121-4FAA-AA8B-FC9900BF8C83} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{EBA4DFA1-6DED-418F-A485-A3B608978906} = {5CA5F70E-0FDB-467B-B22C-3CD5994F0087}
		{8CEE3609-A5A9-4A9B-86D7-33118F5D6B34} = {5CA5F70E-0FDB-467B-B22C-3CD5994F0087}
		{14118347-ED06-4608-9C45-18228273C712} = {5CA5F70E-0FDB-467B-B22C-3CD5994F0087}
		{6E62A0FF-D0DC-4109-9131-AB8E60CDFF7B} = {5CA5F70E-0FDB-467B-B22C-3CD5994F0087}
		{86FD5B9A-4FA0-4B10-B59F-CFAF077A859C} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{C0E80510-4FBE-4B0C-AF2C-4F473787722C} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{7BE3DEEB-87F8-4E15-9C21-4F94B0B1C2D6} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{D49439D7-56D2-450F-A4F0-74CB95D620E6} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{5DEFADBD-44EB-47A2-A53E-F1282CC9E4E9} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{91C574AD-0352-47E9-A019-EE02CC32A396} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{A1455D30-55FC-45EF-8759-3AEBDB13D940} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{201EC5B7-F91E-45E5-B9F2-67A266CCE6FC} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{2169F526-8A88-435D-8732-486ACA095A6A} = {19148439-436F-4CDA-B493-70AF4FFC13E9}
		{A486D7DE-F614-409D-BB41-0FFDF582E35C} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{B617717C-7881-4F01-AB6D-B1B6CC0483A0} = {4C81EBB2-82E1-4C81-80C4-84CC40FA281B}
		{FD6BA96C-7905-4876-8BCC-E38E2CA64F31} = {913A4C08-898E-49C7-9692-0EF9DC56CF6E}
		{AE297965-4D56-4BA9-85EB-655AC4FC95A0} = {913A4C08-898E-49C7-9692-0EF9DC56CF6E}
		{60DB272A-21C9-4E8D-9803-FF4E132392C8} = {913A4C08-898E-49C7-9692-0EF9DC56CF6E}
		{73242A2D-6300-499D-8C15-FADF7ECB185C} = {151F6994-AEB3-4B12-B746-2ACFF26C7BBB}
		{AC5E3515-482C-4C6A-92D9-D0CEA687DFC2} = {151F6994-AEB3-4B12-B746-2ACFF26C7BBB}
		{B8DA3A90-A60C-42E3-9D8E-6C67B800C395} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{ACE53515-482C-4C6A-E2D2-4242A687DFEE} = {151F6994-AEB3-4B12-B746-2ACFF26C7BBB}
		{21B80A31-8FF9-4E3A-8403-AABD635AEED9} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{ABDBAC1E-350E-4DC3-BB45-3504404545EE} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{D0BC9BE7-24F6-40CA-8DC6-FCB93BD44B34} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{FCFA8808-A1B6-48CC-A1EA-0B8CA8AEDA8E} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{1DFEA9C5-973C-4179-9B1B-0F32288E1EF2} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{3140FE61-0856-4367-9AA3-8081B9A80E35} = {151F6994-AEB3-4B12-B746-2ACFF26C7BBB}
		{76242A2D-2600-49DD-8C15-FEA07ECB1842} = {151F6994-AEB3-4B12-B746-2ACFF26C7BBB}
		{76242A2D-2600-49DD-8C15-FEA07ECB1843} = {151F6994-AEB3-4B12-B746-2ACFF26C7BBB}
		{3140FE61-0856-4367-9AA3-8081B9A80E36} = {913A4C08-898E-49C7-9692-0EF9DC56CF6E}
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D3} = {913A4C08-898E-49C7-9692-0EF9DC56CF6E}
		{BF9DAC1E-3A5E-4DC3-BB44-9A64E0D4E9D4} = {913A4C08-898E-49C7-9692-0EF9DC56CF6E}
		{BB3CA047-5D00-48D4-B7D3-233C1265C065} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{BEDC5A4A-809E-4017-9CFD-6C8D4E1847F0} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{FA0E905D-EC46-466D-B7B2-3B5557F9428C} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{E58EE9D7-1239-4961-A0C1-F9EC3952C4C1} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{CCBD3438-3E84-40A9-83AD-533F23BCFCA5} = {CAD2965A-19AB-489F-BE2E-7649957F914A}
		{6FD1CC3E-6A99-4736-9B8D-757992DDE75D} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{286B01F3-811A-40A7-8C1F-10C9BB0597F7} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{24973B4C-FD09-4EE1-97F4-EA03E6B12040} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{ABC7262E-1053-49F3-B846-E3091BB92E8C} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{E2E889A5-2489-4546-9194-47C63E49EAEB} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{E8F0BAA5-7327-43D1-9A51-644E81AE55F1} = {C65C6143-BED3-46E6-869E-9F0BE6E84C37}
		{54E08BF5-F819-404F-A18D-0AB9EA81EA04} = {32A48625-F0AD-419D-828B-A50BDABA38EA}
		{AD6F474E-E6D4-4217-91F3-B7AF1BE31CCC} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{43026D51-3083-4850-928D-07E1883D5B1A} = {C52D8057-43AF-40E6-A01B-6CDBB7301985}
		{A88AB44F-7F9D-43F6-A127-83BB65E5A7E2} = {CC126D03-7EAC-493F-B187-DCDEE1EF6A70}
		{E5A55C16-A5B9-4874-9043-A5266DC02F58} = {CC126D03-7EAC-493F-B187-DCDEE1EF6A70}
		{3BED15FD-D608-4573-B432-1569C1026F6D} = {CC126D03-7EAC-493F-B187-DCDEE1EF6A70}
		{DA0D2A70-A2F9-4654-A99A-3227EDF54FF1} = {DD13507E-D5AF-4B61-B11A-D55D6F4A73A5}
		{971E832B-7471-48B5-833E-5913188EC0E4} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{59AD474E-2A35-4E8A-A74D-E33479977FBF} = {DD13507E-D5AF-4B61-B11A-D55D6F4A73A5}
		{D73ADF7D-2C1C-42AE-B2AB-EDC9497E4B71} = {C2D1346B-9665-4150-B644-075CF1636BAA}
		{C1930979-C824-496B-A630-70F5369A636F} = {C2D1346B-9665-4150-B644-075CF1636BAA}
		{F822F72A-CC87-4E31-B57D-853F65CBEBF3} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{80FDDD00-9393-47F7-8BAF-7E87CE011068} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{7AD4FE65-9A30-41A6-8004-AA8F89BCB7F3} = {A41D1B99-F489-4C43-BBDF-96D61B19A6B9}
		{2E1658E2-5045-4F85-A64C-C0ECCD39F719} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{9C0660D9-48CA-40E1-BABA-8F6A1F11FE10} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{21A01C2D-2501-4619-8144-48977DD22D9C} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{3F2FDC1C-DC6F-44CB-B4A1-A9026F25D66E} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{3DFB4701-E3D6-4435-9F70-A6E35822C4F2} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{69F853E5-BD04-4842-984F-FC68CC51F402} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{6FC8E6F5-659C-424D-AEB5-331B95883E29} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{DD317BE1-42A1-4795-B1D4-F370C40D649A} = {998CAFE8-06E4-4683-A151-0F6AA4BFF6C6}
		{0C0EEB55-4B6D-4F2B-B0BB-B9EB2BA9E980} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{B6FC05F2-0E49-4BE2-8030-ACBB82B7F431} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{1688E1E5-D510-4E06-86F3-F8DB10B1393D} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{F040CEC5-5E11-4DBD-9F6A-250478E28177} = {DD13507E-D5AF-4B61-B11A-D55D6F4A73A5}
		{275812EE-DEDB-4232-9439-91C9757D2AE4} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{5FF1E493-69CC-4D0B-83F2-039F469A04E1} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{AA87BFED-089A-4096-B8D5-690BDC7D5B24} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{A07ABCF5-BC43-4EE9-8FD8-B2D77FD54D73} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{2531A8C4-97DD-47BC-A79C-B7846051E137} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{0141285D-8F6C-42C7-BAF3-3C0CCD61C716} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{9FF1205F-1D7C-4EE4-B038-3456FE6EBEAF} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{5018D049-5870-465A-889B-C742CE1E31CB} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{E512C6C1-F085-4AD7-B0D9-E8F1A0A2A510} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{2D36C343-BB6A-4CB5-902B-E2145ACCB58F} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{FFB00FB5-8C8C-4A02-B67D-262B9D28E8B1} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{60166C60-813C-46C4-911D-2411B4ABBC0F} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{FC2AE90B-2E4B-4045-9FDD-73D4F5ED6C89} = {C2D1346B-9665-4150-B644-075CF1636BAA}
		{49E7C367-181B-499C-AC2E-8E17C81418D6} = {C2D1346B-9665-4150-B644-075CF1636BAA}
		{037F06F0-3BE8-42D0-801E-2F74FC380AB8} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{2F11618A-9251-4609-B3D5-CE4D2B3D3E49} = {5CA5F70E-0FDB-467B-B22C-3CD5994F0087}
		{764D2C19-0187-4837-A2A3-96DDC6EF4CE2} = {CC126D03-7EAC-493F-B187-DCDEE1EF6A70}
		{9102ECF3-5CD1-4107-B8B7-F3795A52D790} = {C52D8057-43AF-40E6-A01B-6CDBB7301985}
		{50CF5D8F-F82F-4210-A06E-37CC9BFFDD49} = {C52D8057-43AF-40E6-A01B-6CDBB7301985}
		{CFA94A39-4805-456D-A369-FC35CCC170E9} = {C52D8057-43AF-40E6-A01B-6CDBB7301985}
		{C52D8057-43AF-40E6-A01B-6CDBB7301985} = {3F40F71B-7DCF-44A1-B15C-38CA34824143}
		{4A490CBC-37F4-4859-AFDB-4B0833D144AF} = {38940C5F-97FD-4B2A-B2CD-C4E4EF601B05}
		{34E868E9-D30B-4FB5-BC61-AFC4A9612A0F} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{BE25E872-1667-4649-9D19-96B83E75A44E} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{0EB22BD1-B8B1-417D-8276-F475C2E190FF} = {BE25E872-1667-4649-9D19-96B83E75A44E}
		{3636D3E2-E3EF-4815-B020-819F382204CD} = {BE25E872-1667-4649-9D19-96B83E75A44E}
		{B9843F65-262E-4F40-A0BC-2CBEF7563A44} = {C52D8057-43AF-40E6-A01B-6CDBB7301985}
		{03607817-6800-40B6-BEAA-D6F437CD62B7} = {BE25E872-1667-4649-9D19-96B83E75A44E}
		{6A68FDF9-24B3-4CB6-A808-96BF50D1BCE5} = {BE25E872-1667-4649-9D19-96B83E75A44E}
		{23405307-7EFF-4774-8B11-8F5885439761} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{AFA5F921-0650-45E8-B293-51A0BB89DEA0} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{6362616E-6A47-48F0-9EE0-27800B306ACB} = {AFA5F921-0650-45E8-B293-51A0BB89DEA0}
		{8977A560-45C2-4EC2-A849-97335B382C74} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{BD8CE303-5F04-45EC-8DCF-73C9164CD614} = {8977A560-45C2-4EC2-A849-97335B382C74}
		{2FB6C157-DF91-4B1C-9827-A4D1C08C73EC} = {8977A560-45C2-4EC2-A849-97335B382C74}
		{5E6E9184-DEC5-4EC5-B0A4-77CFDC8CDEBE} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{A74C7D2E-92FA-490A-B80A-28BEF56B56FC} = {C52D8057-43AF-40E6-A01B-6CDBB7301985}
		{686BF57E-A6FF-467B-AAB3-44DE916A9772} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{1DDE89EE-5819-441F-A060-2FF4A986F372} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{655A5B07-39B8-48CD-8590-8AC0C2B708D8} = {8977A560-45C2-4EC2-A849-97335B382C74}
		{DE53934B-7FC1-48A0-85AB-C519FBBD02CF} = {8977A560-45C2-4EC2-A849-97335B382C74}
		{3D33BBFD-EC63-4E8C-A714-0A48A3809A87} = {BE25E872-1667-4649-9D19-96B83E75A44E}
		{BFFB5CAE-33B5-447E-9218-BDEBFDA96CB5} = {8977A560-45C2-4EC2-A849-97335B382C74}
		{FC32EF16-31B1-47B3-B625-A80933CB3F29} = {8977A560-45C2-4EC2-A849-97335B382C74}
		{453C8E28-81D4-431E-BFB0-F3D413346E51} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{CE7F7553-DB2D-4839-92E3-F042E4261B4E} = {8DBA5174-B0AA-4561-82B1-A46607697753}
		{FF38E9C9-7A25-44F0-B2C4-24C9BFD6A8F6} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{D55FB2BD-CC9E-454B-9654-94AF5D910BF7} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
		{B9899CF1-E0EB-4599-9E24-6939A04B4979} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{D15BF03E-04ED-4BEE-A72B-7620F541F4E2} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{4A49D526-1644-4819-AA4F-95B348D447D4} = {3E5FE3DB-45F7-4D83-9097-8F05D3B3AEC6}
		{EC946164-1E17-410B-B7D9-7DE7E6268D63} = {7A69EA65-4411-4CD0-B439-035E720C1BD3}
		{99F594B1-3916-471D-A761-A6731FC50E9A} = {9C1BE25C-5926-4E56-84AE-D2242CB0627E}
		{699FEA05-AEA7-403D-827E-53CF4E826955} = {7A69EA65-4411-4CD0-B439-035E720C1BD3}
		{438DB8AF-F3F0-4ED9-80B5-13FDDD5B8787} = {9C1BE25C-5926-4E56-84AE-D2242CB0627E}
		{58969243-7F59-4236-93D0-C93B81F569B3} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{CEC0DCE7-8D52-45C3-9295-FC7B16BD2451} = {7A69EA65-4411-4CD0-B439-035E720C1BD3}
		{E9DBFA41-7A9C-49BE-BD36-FD71B31AA9FE} = {9C1BE25C-5926-4E56-84AE-D2242CB0627E}
		{7B7F4153-AE93-4908-B8F0-430871589F83} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{76E96966-4780-4040-8197-BDE2879516F4} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{1B6C4A1A-413B-41FB-9F85-5C09118E541B} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{EAFFCA55-335B-4860-BB99-EFCEAD123199} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{DA973826-C985-4128-9948-0B445E638BDB} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{94FAF461-2E74-4DBB-9813-6B2CDE6F1880} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{9F9CCC78-7487-4127-9D46-DB23E501F001} = {4A49D526-1644-4819-AA4F-95B348D447D4}
		{DF17AF27-AA02-482B-8946-5CA8A50D5A2B} = {55A62CFA-1155-46F1-ADF3-BEEE51B58AB5}
		{7A69EA65-4411-4CD0-B439-035E720C1BD3} = {DF17AF27-AA02-482B-8946-5CA8A50D5A2B}
		{9C1BE25C-5926-4E56-84AE-D2242CB0627E} = {DF17AF27-AA02-482B-8946-5CA8A50D5A2B}
		{B64766CD-1A1F-4C1B-B11F-C30F82B8E41E} = {EE97CB90-33BB-4F3A-9B3D-69375DEC6AC6}
		{2D5E2DE4-5DA8-41C1-A14F-49855DCCE9C5} = {DC014586-8D07-4DE6-B28E-C0540C59C085}
		{CEA80C83-5848-4FF6-B4E8-CEEE9482E4AA} = {FD0FAF5F-1DED-485C-99FA-84B97F3A8EEC}
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {604E6B91-7BC0-4126-AE07-D4D2FEFC3D29}
	EndGlobalSection
EndGlobal
"
            Dim tempDirectory As String = Path.GetTempPath() & "Test" & Guid.NewGuid().ToString()
            Dim sourceDirectory As String = Path.Combine(tempDirectory, "Source")
            Dim destinationDirectory As String = Path.Combine(tempDirectory, "Source_vb")
            Dim originalSolutionFileName As String = sourceDirectory & Path.DirectorySeparatorChar & "solution.sln"
            Dim destinationSolutionFileName As String = destinationDirectory & Path.DirectorySeparatorChar & "solution.sln"
            Try
                Directory.CreateDirectory(tempDirectory)
                Directory.CreateDirectory(sourceDirectory)
                Directory.CreateDirectory(destinationDirectory)
                Using sw As New StreamWriter(originalSolutionFileName)
                    sw.Write(originalSolutionFile)
                End Using
                ConvertSolutionFile(originalSolutionFileName, destinationDirectory, New List(Of String), testing:=True)
                Assert.True(File.Exists(destinationSolutionFileName))
                Assert.Equal(originalSolutionFile.Length, New FileInfo(destinationSolutionFileName).Length)
            Finally
                If Directory.Exists(tempDirectory) Then
                    Directory.Delete(tempDirectory, recursive:=True)
                End If
            End Try

        End Sub

        <Fact>
        Public Shared Sub ConvertWebProjectFileTest()

            Dim originalProjectFile As XElement =
        <Project Sdk="Microsoft.NET.Sdk.Web">
            <PropertyGroup>
                <TargetFramework>netcoreapp3.1</TargetFramework>
                <RootNamespace>Microsoft.eShopWeb.Web</RootNamespace>
                <UserSecretsId>aspnet-Web2-12345678-1234-5678-9E49-1CCCD7FE85F7</UserSecretsId>
                <LangVersion>latest</LangVersion>
                <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
            </PropertyGroup>

            <ItemGroup>
                <Content Remove="compilerconfig.json"/>
            </ItemGroup>

            <ItemGroup>
                <PackageReference Include="Ardalis.ListStartupServices" Version="1.1.3"/>

                <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="3.1.0">
                    <PrivateAssets>all</PrivateAssets>
                    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
                </PackageReference>
            </ItemGroup>
            <ItemGroup>
                <Folder Include="wwwroot\fonts\"/>
            </ItemGroup>
            <ItemGroup>
                <ProjectReference Include="..\ApplicationCore\ApplicationCore.csproj"/>
            </ItemGroup>
            <ItemGroup>
                <None Include="wwwroot\images\products\1.png"/>
            </ItemGroup>
            <ItemGroup>
                <Content Update="appsettings.json">
                    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
                </Content>
            </ItemGroup>
        </Project>

            Dim expectedProjectFile As XElement =
        <Project Sdk="Microsoft.NET.Sdk.Web">
            <PropertyGroup>
                <TargetFramework>netcoreapp3.1</TargetFramework>
                <RootNamespace>Microsoft.eShopWeb.Web</RootNamespace>
                <UserSecretsId>aspnet-Web2-12345678-1234-5678-9E49-1CCCD7FE85F7</UserSecretsId>
                <LangVersion>latest</LangVersion>
            </PropertyGroup>

            <ItemGroup>
                <Content Remove="compilerconfig.json"/>
            </ItemGroup>

            <ItemGroup>
                <PackageReference Include="Ardalis.ListStartupServices" Version="1.1.3"/>

                <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="3.1.0">
                    <PrivateAssets>all</PrivateAssets>
                    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
                </PackageReference>
            </ItemGroup>
            <ItemGroup>
                <Folder Include="wwwroot\fonts\"/>
            </ItemGroup>
            <ItemGroup>
                <ProjectReference Include="..\ApplicationCore\ApplicationCore.vbproj"/>
            </ItemGroup>
            <ItemGroup>
                <None Include="wwwroot\images\products\1.png"/>
            </ItemGroup>
            <ItemGroup>
                <Content Update="appsettings.json">
                    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
                </Content>
            </ItemGroup>
        </Project>

            Dim sourceXmlDoc As New XmlDocument With {
                                .PreserveWhitespace = True
                            }
            sourceXmlDoc.LoadXml(originalProjectFile.ToString)
            Dim tempDirectory As String = Path.GetTempPath() & "Test" & Guid.NewGuid().ToString()
            Dim sourceDirectory As String = Path.Combine(tempDirectory, "Source")
            Dim destinationDirectory As String = Path.Combine(tempDirectory, "Source_vb")
            Dim originalProjectFileName As String = sourceDirectory & Path.DirectorySeparatorChar & "Test.csproj"
            Dim destinationProjectFileName As String = destinationDirectory & Path.DirectorySeparatorChar & "Test.vbproj"
            Try
                Directory.CreateDirectory(tempDirectory)
                Directory.CreateDirectory(sourceDirectory)
                Directory.CreateDirectory(destinationDirectory)
                sourceXmlDoc.Save(originalProjectFileName)
                Assert.False(ConvertProjectFile(originalProjectFileName, destinationDirectory).Any)
                Dim resultXmlDoc As String = File.ReadAllText(destinationProjectFileName)
                Assert.Equal(expectedProjectFile.ToString, resultXmlDoc)
            Finally
                If Directory.Exists(tempDirectory) Then
                    Directory.Delete(tempDirectory, recursive:=True)
                End If
            End Try

        End Sub

        <Fact>
        Public Shared Sub ConvertWinFormsProjectFileTest()

            Dim originalProjectFile As XElement =
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">

    <PropertyGroup>
        <OutputType>WinExe</OutputType>
        <TargetFramework condition="'$(TargetFrameworkOverride)' == ''">netcoreapp5.0</TargetFramework>
        <TargetFramework condition="'$(TargetFrameworkOverride)' != ''">TargetFrameworkOverride</TargetFramework>
        <RootNamespace>Company.WinFormsApplication1</RootNamespace>
        <StartupObject>Company.WinFormsApplication1.Form1</StartupObject>
        <LangVersion condition="'$(langVersion)' != ''">$(ProjectLanguageVersion)</LangVersion>
        <UseWindowsForms>true</UseWindowsForms>
    </PropertyGroup>

    <ItemGroup>
        <Import Include="System.Data"/>
    </ItemGroup>

</Project>
            Dim expectedProjectFile As XElement =
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">

    <PropertyGroup>
        <OutputType>WinExe</OutputType>
        <TargetFramework condition="'$(TargetFrameworkOverride)' == ''">netcoreapp5.0</TargetFramework>
        <TargetFramework condition="'$(TargetFrameworkOverride)' != ''">TargetFrameworkOverride</TargetFramework>
        <RootNamespace>Company.WinFormsApplication1</RootNamespace>
        <StartupObject>Company.WinFormsApplication1.Form1</StartupObject>
        <LangVersion condition="'$(langVersion)' != ''">$(ProjectLanguageVersion)</LangVersion>
        <UseWindowsForms>true</UseWindowsForms>
    </PropertyGroup>

    <ItemGroup>
        <Import Include="System.Data"/>
    </ItemGroup>

</Project>
            Dim sourceXmlDoc As New XmlDocument With {
                                .PreserveWhitespace = True
                            }
            sourceXmlDoc.LoadXml(originalProjectFile.ToString)
            Dim tempDirectory As String = Path.GetTempPath() & "Test" & Guid.NewGuid().ToString()
            Dim sourceDirectory As String = Path.Combine(tempDirectory, "Source")
            Dim destinationDirectory As String = Path.Combine(tempDirectory, "Source_vb")
            Dim originalProjectFileName As String = sourceDirectory & Path.DirectorySeparatorChar & "Test.csproj"
            Dim destinationProjectFileName As String = destinationDirectory & Path.DirectorySeparatorChar & "Test.vbproj"
            Try
                Directory.CreateDirectory(tempDirectory)
                Directory.CreateDirectory(sourceDirectory)
                Directory.CreateDirectory(destinationDirectory)
                sourceXmlDoc.Save(originalProjectFileName)
                Assert.False(ConvertProjectFile(originalProjectFileName, destinationDirectory).Any)
                Dim resultXmlDoc As String = File.ReadAllText(destinationProjectFileName)
                Assert.Equal(expectedProjectFile.ToString, resultXmlDoc)
            Finally
                If Directory.Exists(tempDirectory) Then
                    Directory.Delete(tempDirectory, recursive:=True)
                End If
            End Try
        End Sub

    End Class

End Namespace

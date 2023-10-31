# ITNOA

##########################################################################
# This is the Cake bootstrapper script for PowerShell.
# This file was downloaded from https://github.com/cake-build/resources
# Feel free to change this file to fit your needs.
##########################################################################

<#

.SYNOPSIS
This is a Powershell script to bootstrap a Cake build.

.DESCRIPTION
This Powershell script will download NuGet if missing, restore NuGet tools (including Cake)
and execute your Cake build script with the parameters you provide.

.PARAMETER Script
The build script to execute.
.PARAMETER Target
The build script target to run.
.PARAMETER Configuration
The build configuration to use.
.PARAMETER Verbosity
Specifies the amount of information to be displayed.
.PARAMETER ShowDescription
Shows description about tasks.
.PARAMETER DryRun
Performs a dry run.
.PARAMETER SkipToolPackageRestore
Skips restoring of packages.
.PARAMETER ScriptArgs
Remaining arguments are added here.

.LINK
https://cakebuild.net

#>

[CmdletBinding()]
Param(
    # TODO: https://devblogs.microsoft.com/scripting/powertip-identify-which-platform-powershell-is-running-on/
    [string]$Script = "windows-build.cake",
    [string]$Target,
    [string]$Configuration,
    [ValidateSet("Quiet", "Minimal", "Normal", "Verbose", "Diagnostic")]
    [string]$Verbosity,
    [switch]$ShowDescription,
    [Alias("WhatIf", "Noop")]
    [switch]$DryRun,
    [switch]$SkipToolPackageRestore,
    [Parameter(Position=0,Mandatory=$false,ValueFromRemainingArguments=$true)]
    [string[]]$ScriptArgs
)

 # Build an array (not a string) of Cake arguments to be joined later
$cakeArguments = @()
if ($Script) { $cakeArguments += "`"$Script`"" }
if ($Target) { $cakeArguments += "--target=`"$Target`"" }
if ($Configuration) { $cakeArguments += "--configuration=$Configuration" }
if ($Verbosity) { $cakeArguments += "--verbosity=$Verbosity" }
if ($ShowDescription) { $cakeArguments += "--showdescription" }
if ($DryRun) { $cakeArguments += "--dryrun" }
$cakeArguments += $ScriptArgs

# TODO: Read port number from config and input
if ([System.Environment]::UserName -eq "Network Service") {
    if (-Not (Get-UrlAcl -Url "http://+:1372")) {
        Write-Verbose -Message "Add UrlAcl..."
        New-UrlAcl -Protocol http -HostName + -Port 1372 -SecurityContext "NT AUTHORITY\Network Service"
    }
}

# Start Cake
Write-Host "Running build script..."

Write-Host "CAKE_EXE: " + $CAKE_EXE_INVOCATION
Write-Host "CAKE Arguments: "
$cakeArguments

$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

Write-Host "Add nuget.org to sources"

# TODO: Check does not add duplicate
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org --configfile $env:APPDATA\NuGet\NuGet.Config

Write-Host "Start dotnet tool restore"

dotnet tool restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet cake $cakeArguments
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

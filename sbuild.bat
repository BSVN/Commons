::::::::::::::::::::::::::::::::::::::::::::
:: ITNOA
::
:: Elevate.cmd - Version 4
:: Automatically check & get admin rights
:: see "https://stackoverflow.com/a/12264592/1016343" for description
::
:: This Batch file is under https://softwareengineering.stackexchange.com/a/339375/69869 code style
::::::::::::::::::::::::::::::::::::::::::::
@cls
@title Build script for Resa solution
@echo off
setlocal EnableDelayedExpansion

rem If you want to run this script in silent mode, you need to disable UAC
set path=%path%;"C:\ProgramData\chocolatey\bin"
set path=%path%;"C:\ProgramData\chocolatey\lib\gsudo\bin\"

set ARGS=%*

rem Administrative permissions required. Detecting permissions...
if not defined RUNNING_ON_CI (
    net session >NUL 2>&1
    if %ERRORLEVEL% equ 0 (
        rem Administrative permissions confirmed.
        call :BUILD 0
        exit /B
    )
)
rem Check gsudo is exist?
where gsudo
if %ERRORLEVEL% neq 0 (
    rem gsudo Does not found, so we try to install it
    :: This command need UAC attention
    :: Check if choco is avalable use choco
    PowerShell -Command "Set-ExecutionPolicy RemoteSigned -scope Process; iwr -useb https://raw.githubusercontent.com/gerardog/gsudo/master/installgsudo.ps1 | iex"
)

call :BUILD 1

endlocal
exit /B

:BUILD VAL_NEED_GSUDO
    if %~1 equ 1 (
        echo "Run with gsudo"
        gsudo powershell.exe -NoProfile -ExecutionPolicy Bypass "& {& '%~dp0build\build.ps1' -Script Build/build.cake %ARGS%}"
    ) else (
        if %~1 equ 0 (
            powershell.exe -NoProfile -ExecutionPolicy Bypass "& {& '%~dp0build\build.ps1' -Script Build/build.cake %ARGS%}"
        ) else (
            echo "VAL_NEED_GSUDO must be valid value, the current value is" %~1
            exit /B 1
        )
    )

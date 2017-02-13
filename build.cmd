@echo off
SETLOCAL

@REM  ----------------------------------------------------------------------------
@REM  build.cmd
@REM
@REM  author: mario.moreno@live.com
@REM
@REM  ----------------------------------------------------------------------------

set start_time=%time%
set msbuild_bin_path="%ProgramFiles(x86)%\MSBuild\14.0\Bin\msbuild.exe"
set working_dir="%CD%"
set solution_name=IsoMount.sln
set default_build_type=Debug

if "%1"=="/?" goto help

cd %working_dir%

call %msbuild_bin_path% /m %solution_name% /t:Rebuild /p:Configuration=%default_build_type%
@if %errorlevel%  NEQ 0  goto :error

:error
@exit /b errorLevel

:help
echo This script runs a Debug build
echo usage: build.cmd [/?] 
echo /? shows this help text
echo.

:success
echo process successfully finished
echo start time: %start_time%
echo end time: %time%

ENDLOCAL
echo on
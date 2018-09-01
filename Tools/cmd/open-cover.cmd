@echo off
SETLOCAL

@REM  ----------------------------------------------------------------------------
@REM  open-cover.cmd
@REM
@REM  author: m4mc3r@gmail.com
@REM  ----------------------------------------------------------------------------

set start_time=%time%
set solution_dir=%CD%\..\..
set msbuild_bin_path="C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"
set mstest_bin_path="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\mstest.exe"
set opencover_bin_path=C:\root\bin\open-cover\tools\OpenCover.Console.exe
set opencover_proj_path=\open-cover.proj
set solution_name=\IsoMount.sln

if "%1"=="/?" goto help

@REM rebuild solution
echo %msbuild_bin_path% /m "%solution_dir%%solution_name%" /t:Rebuild /p:Configuration=Debug
@if %errorlevel% NEQ 0 GOTO error

@REM run opencover
%msbuild_bin_path% /p:WorkingDirectory="%solution_dir%" /p:OpenCoverBinPath="%opencover_bin_path%" /p:MSTestBinPath=%mstest_bin_path% "%solution_dir%%opencover_proj_path%"
@if %errorlevel% NEQ 0 goto error
goto success

:error
@exit /b errorLevel

:help
echo runs a code coverage analysis using opencover 
echo usage: open-cover.cmd [/?] 
echo "/?" shows this help test 

:success
echo process successfully finished
echo start time: %start_time%
echo end time: %time%

ENDLOCAL
echo on

echo off

rem author: mario.moreno@live.com

rem usage: build.cmd [build_configuration]
rem by default it runs a debug build configuration.

echo build script

pushd %~dp0
set start_time=%time%
set solution_path="..\..\Src"
set solution_name=ISOMount.sln

cd %solution_path%

if "%1" == "" goto build_defaults

%windir%\microsoft.net\framework\v4.0.30319\msbuild /m %solution_name% /p:Configuration=%1
@if %errorlevel% NEQ 0 GOTO error

GOTO success

:build_defaults
%windir%\microsoft.net\framework\v4.0.30319\msbuild /m %solution_name% /p:Configuration=Debug
@if %errorlevel% NEQ 0 GOTO error

GOTO success

:error
echo an error has occurred.
GOTO finish

:success
echo process successfully finished.
echo start time: %start_time%
echo end time: %Time%

:finish
popd
pause

echo on
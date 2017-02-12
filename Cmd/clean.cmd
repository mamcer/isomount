echo off

rem author: mario.moreno@live.com

rem usage: clean.cmd [build_configuration]
rem by default it runs a clean over Debug and Release configuration.

echo cleanup script

pushd %~dp0
set start_time=%time%
set solution_path="..\..\"
set solution_name=[solution_name].sln

cd %solution_path%

if "%1" == "" goto clean_defaults

%windir%\microsoft.net\framework\v4.0.30319\msbuild /m %solution_name% /t:clean  /p:Configuration=%1
@if %ERRORLEVEL% NEQ 0 GOTO error
GOTO success

:clean_defaults
rem debug clean
%windir%\microsoft.net\framework\v4.0.30319\msbuild /m %solution_name% /t:clean  /p:Configuration=Debug
@if %ERRORLEVEL% NEQ 0 GOTO error
GOTO success

rem release clean
%windir%\microsoft.net\framework\v4.0.30319\msbuild /m %solution_name% /t:clean  /p:Configuration=Release
@if %ERRORLEVEL% NEQ 0 GOTO error
GOTO success

:error
echo an error has occurred.
GOTO finish

cd %current_dir%

:success
echo process successfully finished.
echo start time: %start_time%
echo end time: %Time%

:finish
popd
pause

echo on
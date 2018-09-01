@echo off
SETLOCAL

@REM  ----------------------------------------------------------------------------
@REM  report-generator.cmd
@REM
@REM  author: m4mc3r@gmail.com
@REM  ----------------------------------------------------------------------------

set start_time=%time%
set working_dir=%CD%\..\..
set reportgenerator_bin=C:\root\bin\report-generator\tools\ReportGenerator.exe
set opencover_file=open-cover.xml
set target_dir=coverage-report

if "%1"=="/?" goto help

cd %working_dir%

@rem remove previous coverate-report directory if it exists  
IF NOT EXIST "%working_dir%\%target_dir%" GOTO NoCoverageReport
rmdir /s /q "%target_dir%"
:NoCoverageReport
md "%target_dir%"

rem run report generator
"%reportgenerator_bin%" -reports:"%CD%\%opencover_file%" -targetdir:"%CD%\%target_dir%" -reporttypes:Html
@if %errorlevel% NEQ 0 goto error
goto success

:error
@exit /b errorLevel

:help
echo generates a html report from an open-cover.xml file 
echo usage: report-generator.cmd [/?] 
echo "/?" shows this help test 

:success
echo process successfully finished
echo start time: %start_time%
echo end time: %time%

ENDLOCAL
echo on
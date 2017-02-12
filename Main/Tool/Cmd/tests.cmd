echo off

rem author: mario.moreno@live.com

echo tests run script

pushd %~dp0
set start_time=%time%
set solution_path="..\..\"
set vsmdi_file=[vsmdi_file_name].vsmdi
set testlist_name=[test_list_name]
set sqlserver_servicename="SQL Server (SQLEXPRESS)"

rem check if sql server service its running. If not start it.
net start | find %sqlserver_servicename%
@if %errorlevel% EQU 0 goto run_tests

net start %sqlserver_servicename%
@if %errorlevel% NEQ 0 goto error

:run_tests
cd %solution_path%
mstest /nologo /testmetadata:%vsmdi_file% /testlist:%testlist_name%
@if %errorlevel% NEQ 0 goto error
goto success

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
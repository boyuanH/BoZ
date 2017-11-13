
set slnPath=%~dp0
cd ../bin
xcopy /r /y WpfPageTransitions.dll ..\Setup\src\bin\
xcopy /r /y RCNdemo.exe ..\Setup\src\bin\
cd ../
xcopy /r /y .\resource\* .\Setup\src\resource\

where /q 7z

if not ERRORLEVEL 1 (7z a -sfx .\Setup\Setup.exe .\Setup\src\)

rmdir /s/q .\Setup\src

pause
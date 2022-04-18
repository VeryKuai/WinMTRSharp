@echo off
cd /d %~dp0
cd src

..\Tools\ILRepack.exe /union /ndebug /targetplatform:v4 /internalize ^
    /out:..\Temp\WinMTR.exe WinMTR.exe ^
    ARSoft.Tools.Net.dll BouncyCastle.Crypto.dll

..\Tools\ILStrip.exe -e WinMTRSharp.Program -h -u -import ..\Temp\WinMTR.exe ^
    ..\Temp\WinMTR.exe ..\Temp\WinMTR_Stripped.exe

copy /y ..\Temp\WinMTR_Stripped.exe ..\Temp\WinMTR.exe
del /f ..\Temp\WinMTR_Stripped.exe

..\Tools\Obfuscar.Console.exe ..\Tools\obfuscar_config.xml
del /f ..\Output\Mapping.txt
cd ..

pause

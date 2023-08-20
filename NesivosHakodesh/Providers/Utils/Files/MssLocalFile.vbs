Set WshShell = CreateObject("WScript.Shell" ) 
REM WshShell.Run "C:\inetpub\Projects\Nesivos\workflowFile\MssLocalFile.bat " & WScript.Arguments(0), 0
WshShell.Run "C:\Users\\User\source\repos\nesivoshakodeshapp\NesivosHakodesh\Providers\Utils\\Files\MssLocalFile.bat " & WScript.Arguments(0), 0
Set WshShell = Nothing
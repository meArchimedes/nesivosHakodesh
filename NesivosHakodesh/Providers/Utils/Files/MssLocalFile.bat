
@echo off 

setlocal enabledelayedexpansion
chcp 65001 >nul


::call :sub %1 > C:\\inetpub\\Projects\\Nesivos\\Files\\output.txt
::exit /b

:::sub

IF "%~1"=="" GOTO BLANK

REM SET basePath=C:\\inetpub\\Projects\\Nesivos\\Files\\
SET basePath=C:\\Users\\User\\source\\repos\\nesivoshakodeshapp\\NesivosHakodesh\\Providers\\Utils\\Files\\
SET file=%1

ECHO %1


SET file=!file:msslocalfile://=!
SET file=!file:"=!
SET file=!file://=\\!
SET file=!file:/=!
SET file=!file:%%D7%%90=א!
SET file=!file:%%D7%%91=ב!
SET file=!file:%%D7%%92=ג!
SET file=!file:%%D7%%93=ד!
SET file=!file:%%D7%%94=ה!
SET file=!file:%%D7%%95=ו!
SET file=!file:%%D7%%96=ז!
SET file=!file:%%D7%%97=ח!
SET file=!file:%%D7%%98=ט!
SET file=!file:%%D7%%99=י!
SET file=!file:%%D7%%9B=כ!
SET file=!file:%%D7%%9A=ך!
SET file=!file:%%D7%%9C=ל!
SET file=!file:%%D7%%9E=מ!
SET file=!file:%%D7%%9D=ם!
SET file=!file:%%D7%%A0=נ!
SET file=!file:%%D7%%9F=ן!
SET file=!file:%%D7%%A1=ס!
SET file=!file:%%D7%%A2=ע!
SET file=!file:%%D7%%A4=פ!
SET file=!file:%%D7%%A3=ף!
SET file=!file:%%D7%%A6=צ!
SET file=!file:%%D7%%A5=ץ!
SET file=!file:%%D7%%A7=ק!
SET file=!file:%%D7%%A8=ר!
SET file=!file:%%D7%%A9=ש!
SET file=!file:%%D7%%AA=ת!

SET fullPath=%basePath%%file%

ECHO %fullPath%

if not exist %fullPath% GOTO INVALID

start "" %fullPath%

GOTO DONE

:BLANK

ECHO No Parameter

GOTO DONE

:INVALID

ECHO Invalid Link

GOTO DONE
	
:DONE

ECHO Done!




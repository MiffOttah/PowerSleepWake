@echo off
cd %~dp0\bin\debug\net7.0
pwsh.exe -noexit -interactive -command "import-module .\powersleepwake.dll; $DebugPreference = 'Continue'"

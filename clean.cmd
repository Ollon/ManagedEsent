@echo off
md robo
robocopy robo /mir bin
rmdir /q /s bin
robocopy robo /mir .vs
rmdir /q /s .vs
rmdir /q /s robo
powershell -noprofile -executionPolicy RemoteSigned -file "%~dp0\build\scripts\clean.ps1"

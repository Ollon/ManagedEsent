@echo off
powershell -noprofile -executionPolicy RemoteSigned -file "%~dp0\build\scripts\rebuild.ps1"

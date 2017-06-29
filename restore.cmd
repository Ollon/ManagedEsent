@echo off
powershell -noprofile -executionPolicy RemoteSigned -file "%~dp0\build\scripts\restore.ps1"

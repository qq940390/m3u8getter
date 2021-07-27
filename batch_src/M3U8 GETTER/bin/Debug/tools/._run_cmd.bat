@echo off

set cmdto=cd  
set dirto=%~dp0

if "%1" == "dirto" (
    set cmdto=%cmdto%%dirto%
)

cmd /k "%cmdto%"

exit
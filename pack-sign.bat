MakeAppx pack /l /d ".\_build\x64\Release\App12Reunion (Package)\bin\App12Reunion (Package)\AppX" /p out.msix
if errorlevel 0 goto signAppx
goto end
:signAppx
Signtool.exe sign /a /v /fd SHA256 /f signingcert.pfx out.msix
:end
cd ..


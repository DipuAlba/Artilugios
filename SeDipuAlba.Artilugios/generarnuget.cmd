@echo off

REM Borra el nupkg anterior moviéndolo a oldnupkg
if not exist oldnupkg mkdir oldnupkg
for /f "delims=" %%x in ('dir /od /b *.nupkg') do move %%x oldnupkg\

REM Empaqueta el proyecto usando dotnet pack (esto funciona para proyectos SDK-style)
dotnet pack SeDipuAlba.Artilugios.csproj --configuration Release --output .

REM Busca el nupkg más reciente para asignarlo a la variable
for /f "delims=" %%x in ('dir /od /b *.nupkg') do set recent=%%x
echo %recent%
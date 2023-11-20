@echo off
for /f "delims=" %%x in ('dir /od /b *.nupkg') do move %%x oldnupkg\
nuget pack -Properties "Configuration=Release;mycommit=%build.vcs.number.1%"
for /f "delims=" %%x in ('dir /od /b *.nupkg') do set recent=%%x
echo %recent%
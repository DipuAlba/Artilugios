@echo off
for /f "delims=" %%x in ('dir /od /b *.nupkg') do move %%x oldnupkg\
nuget pack -Prop Configuration=Release
for /f "delims=" %%x in ('dir /od /b *.nupkg') do set recent=%%x
echo %recent%
nuget push %recent% Serpi2000 -Source http://liget:9011
nuget push %recent% -Source GitLab
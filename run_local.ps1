$ServerFolder = "$PSScriptRoot/src/server/eScratchLottery.Server/bin/Debug/net5.0"
$WwwRootPath  = "$PSScriptRoot/src/frontend/build"

if (Test-Path -Path $ServerFolder) {
    Write-Output "Running Server"
    Push-Location $ServerFolder
	
	$env:eSL_WwwRoot=$WwwRootPath
	Start-Process "http://localhost:2020";
    dotnet "$ServerFolder/eScratchLottery.Server.dll"
} else {
    Write-Output "Please run build.ps1 first"
}

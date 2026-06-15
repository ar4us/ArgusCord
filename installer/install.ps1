$link = "https://github.com/ArgusCord/Installer/releases/latest/download/ArgusCordInstallerCli.exe"

$outfile = "$env:TEMP\ArgusCordInstallerCli.exe"

Write-Output "Downloading installer to $outfile"

Invoke-WebRequest -Uri "$link" -OutFile "$outfile"

Write-Output ""

Start-Process -Wait -NoNewWindow -FilePath "$outfile"

# Cleanup
Remove-Item -Force "$outfile"

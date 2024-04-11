# Check if dotnet SDK is installed
if (!(Test-Path (Join-Path ${env:ProgramFiles(x86)} "\dotnet\dotnet.exe"))) {
    Write-Host ".NET SDK is not installed. Installing..."
    
    # Download and install the .NET SDK
    Invoke-WebRequest -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile "$env:TEMP\dotnet-install.ps1"
    & "$env:TEMP\dotnet-install.ps1" -InstallDir "$env:ProgramFiles(x86)\dotnet" -Channel LTS
    Remove-Item "$env:TEMP\dotnet-install.ps1"
}

# Install required NuGet packages
Write-Host "Restoring NuGet packages..."
dotnet restore

# Apply Entity Framework Core migrations
Write-Host "Applying Entity Framework Core migrations..."
dotnet ef database update

# Start the Blazor app in the background
Write-Host "Starting the Blazor app..."
$process = Start-Process "dotnet" -ArgumentList "run" -NoNewWindow -PassThru

# Wait for the app to start
Start-Sleep -Seconds 5

# Get the URL the app is listening on
$uri = $process.StartInfo.Arguments | Where-Object {$_ -like "https://*" }

if ($uri -eq $null) {
    Write-Host "Failed to determine app URL. Exiting."
    exit 1
}

# Launch the default web browser with the app URL
Write-Host "Launching default web browser..."
Invoke-Expression "$env:SystemRoot\system32\cmd.exe /c start $uri"

# Wait for the user to close the app
Write-Host "Press any key to stop the app..."
Read-Host

# Stop the app
Stop-Process -Id $process.Id

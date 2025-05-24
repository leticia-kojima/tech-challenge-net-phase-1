Write-Host "Running .NET tests..."
dotnet test ..\ `
    --configuration Release `
    --no-restore `
    --logger trx `
    --results-directory TestResults `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=opencover `
    --collect:"XPlat Code Coverage"

if (-not (Get-Command "reportgenerator" -ErrorAction SilentlyContinue)) {
    Write-Host "Installing ReportGenerator..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
}

Write-Host "Generating coverage report..."
reportgenerator `
    "-reports:TestResults/**/coverage.*.xml" `
    "-targetdir:Report" `
    -reporttypes:Html
Write-Host "Coverage report generated in 'Report' directory."

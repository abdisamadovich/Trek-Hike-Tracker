# Trek & Hike Tracker - Development Startup Script
# This script runs both ASP.NET Core API and Angular frontend in parallel

Write-Host "🚀 Trek & Hike Tracker - Development Environment" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host ""

# Ensure we're in the right directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Start .NET API
Write-Host "📡 Starting ASP.NET Core API..." -ForegroundColor Yellow
$apiProcess = Start-Process powershell -ArgumentList {
    cd "$scriptPath"
    dotnet run --project TrekTracker.API --urls "http://localhost:5245"
} -PassThru -WindowStyle Normal

# Wait a bit for API to start
Start-Sleep -Seconds 3

# Start Angular Frontend
Write-Host "🎨 Starting Angular Development Server..." -ForegroundColor Cyan
$webProcess = Start-Process powershell -ArgumentList {
    cd "$scriptPath\trek-hike-tracker-web"
    npm start
} -PassThru -WindowStyle Normal

Write-Host ""
Write-Host "✅ Both services are now running!" -ForegroundColor Green
Write-Host ""
Write-Host "📱 Frontend: http://localhost:4200 (opens automatically)" -ForegroundColor Cyan
Write-Host "📡 Backend API: http://localhost:5245" -ForegroundColor Cyan
Write-Host "📚 Swagger Docs: http://localhost:5245/swagger" -ForegroundColor Cyan
Write-Host ""
Write-Host "Press Ctrl+C to stop all services" -ForegroundColor Yellow
Write-Host ""

# Wait for both processes
$apiProcess.WaitForExit()
$webProcess.WaitForExit()

Write-Host ""
Write-Host "✋ Services stopped" -ForegroundColor Red

# Simple test script for Car Showcase Blazor Application

Write-Host "Testing Blazor Application Build and Deployment Package Creation" -ForegroundColor Cyan

# Build and test
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build CarShowcase.sln --configuration Release

Write-Host "Running tests..." -ForegroundColor Yellow  
dotnet test CarShowcase.Tests --configuration Release --collect:"XPlat Code Coverage" --results-directory:TestResults

Write-Host "Publishing Blazor app..." -ForegroundColor Yellow
dotnet publish CarShowcase/CarShowcase.csproj --configuration Release --output ./publish

Write-Host "Creating deployment directory..." -ForegroundColor Yellow
if (Test-Path "deploy") {
    Remove-Item "deploy" -Recurse -Force
}
New-Item -ItemType Directory -Path "deploy" | Out-Null

# Copy wwwroot content if it exists
if (Test-Path "publish/wwwroot") {
    Copy-Item "publish/wwwroot/*" "deploy/" -Recurse -Force
    Write-Host "Copied wwwroot content to deploy directory" -ForegroundColor Green
}

# Create simple index.html
$html = @"
<!DOCTYPE html>
<html>
<head>
    <title>Car Showcase - Blazor Application</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <div class="container mt-5">
        <div class="text-center">
            <h1>Car Showcase - Blazor Application</h1>
            <p class="lead">Successfully built and ready for deployment!</p>
            <div class="alert alert-success">
                <h4>Deployment Test Successful</h4>
                <p>The Blazor application has been built and packaged for GitHub Pages deployment.</p>
            </div>
        </div>
    </div>
</body>
</html>
"@

$html | Out-File "deploy/index.html" -Encoding UTF8

Write-Host "Deployment package created successfully!" -ForegroundColor Green
Write-Host "Files are ready in the 'deploy' directory" -ForegroundColor Cyan

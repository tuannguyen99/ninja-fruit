Write-Host "Ninja Fruit - Unity Project Status" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = "c:\Users\Admin\Desktop\ai\games\ninja-fruit"

if (Test-Path $projectPath)
{
    Write-Host "Unity project found" -ForegroundColor Green
}
else
{
    Write-Host "Unity project not found" -ForegroundColor Red
    exit 1
}

$keyFiles = @(
    "Assets\Scripts\Gameplay\GameManager.cs",
    "Assets\Scripts\UI\SwipeVisualizer.cs",
    "Assets\Editor\NinjaFruitSceneBuilder.cs",
    "Assets\Scripts\Gameplay\FruitSpawner.cs",
    "Assets\Scripts\Input\SwipeDetector.cs",
    "Assets\Scripts\Gameplay\CollisionManager.cs",
    "Assets\Scripts\Gameplay\ScoreManager.cs",
    "Assets\Scripts\Gameplay\GameStateController.cs",
    "Assets\Scripts\UI\HUDController.cs",
    "Assets\Scripts\CompilationTest.cs"
)

Write-Host "Checking game files..." -ForegroundColor Yellow
$allFilesExist = $true

foreach ($file in $keyFiles)
{
    $fullPath = Join-Path $projectPath $file
    if (Test-Path $fullPath)
    {
        Write-Host "  OK: $file" -ForegroundColor Green
    }
    else
    {
        Write-Host "  MISSING: $file" -ForegroundColor Red
        $allFilesExist = $false
    }
}

Write-Host ""

if ($allFilesExist)
{
    Write-Host "All files ready!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Next steps:" -ForegroundColor Yellow
    Write-Host "1. Open Unity Editor" -ForegroundColor White
    Write-Host "2. Wait for compilation" -ForegroundColor White
    Write-Host "3. Menu: Ninja Fruit > Build Gameplay Scene" -ForegroundColor White
    Write-Host "4. Press Play!" -ForegroundColor White
}
else
{
    Write-Host "Some files are missing!" -ForegroundColor Red
}

Write-Host ""

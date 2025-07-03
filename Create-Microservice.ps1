# Create-Microservice.ps1

param(
    [Parameter(Mandatory=$true)]
    [string]$MicroserviceName
)

$basePath = "$MicroserviceName"
$srcPath = "$basePath\src"
$testsPath = "$basePath\tests"
$projects = @("Api", "Application", "Domain", "Infrastructure", "SharedKernel")

Write-Host "Creating microservice structure for '$MicroserviceName'..."
New-Item -ItemType Directory -Path $srcPath -Force | Out-Null
New-Item -ItemType Directory -Path $testsPath -Force | Out-Null

foreach ($project in $projects) {
    $projectName = "GoodMarket.$MicroserviceName.$project"
    $folderPath = "$srcPath\$projectName"
    New-Item -ItemType Directory -Path $folderPath -Force | Out-Null
    if ($project -eq "Api") {
        dotnet new webapi -n $projectName -o $folderPath --framework net9.0
    } else {
        dotnet new classlib -n $projectName -o $folderPath --framework net9.0
        $classFile = Join-Path $folderPath "Class1.cs"
        if (Test-Path $classFile) { Remove-Item $classFile }
    }
    dotnet sln GoodMarket.sln add "$folderPath\$projectName.csproj"
}

$testProjectName = "GoodMarket.$MicroserviceName.Integration"
$testFolderPath = "$testsPath\$testProjectName"
New-Item -ItemType Directory -Path $testFolderPath -Force | Out-Null
dotnet new xunit -n $testProjectName -o $testFolderPath --framework net9.0
$testFile = Join-Path $testFolderPath "UnitTest1.cs"
if (Test-Path $testFile) { Remove-Item $testFile }
dotnet sln GoodMarket.sln add "$testFolderPath\$testProjectName.csproj"

Write-Host "✅ Microservice '$MicroserviceName' structure created successfully."

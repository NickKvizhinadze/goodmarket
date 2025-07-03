# 🛒 GoodMarket Ecommerce Microservices

This project is an Ecommerce solution for the GoodMarket store, designed with a modular microservices architecture for scalability, maintainability, and rapid feature development.

# 📦 GoodMarket Microservice Structure & Conventions

This document defines the **standard folder structure**, **naming conventions**, and includes a **PowerShell script** to scaffold new microservices within the `GoodMarket` solution. This ensures consistency, maintainability, and a smooth onboarding experience.

---

## 🔧 Folder Structure

Each microservice must follow the same folder layout:

```
GoodMarket/
└── {{MicroserviceName}}/
    ├── src/
    │   ├── GoodMarket.{{MicroserviceName}}.Api
    │   ├── GoodMarket.{{MicroserviceName}}.Application
    │   ├── GoodMarket.{{MicroserviceName}}.Domain
    │   ├── GoodMarket.{{MicroserviceName}}.Infrastructure
    │   └── GoodMarket.{{MicroserviceName}}.SharedKernel
    └── tests/
        └── GoodMarket.{{MicroserviceName}}.Integration
```

---

## 📛 Naming Conventions

All project and folder names must follow this format:

```
GoodMarket.{{MicroserviceName}}.{{ProjectType}}
```

Where:
- `{{MicroserviceName}}` is in **PascalCase**
- `{{ProjectType}}` is one of:
  - `Api`
  - `Application`
  - `Domain`
  - `Infrastructure`
  - `SharedKernel`
  - `Integration`

### Example (Customers microservice)

```
GoodMarket/
└── Customers/
    ├── src/
    │   ├── GoodMarket.Customers.Api
    │   ├── GoodMarket.Customers.Application
    │   ├── GoodMarket.Customers.Domain
    │   ├── GoodMarket.Customers.Infrastructure
    │   └── GoodMarket.Customers.SharedKernel
    └── tests/
        └── GoodMarket.Customers.Integration
```

---

## ✅ Required Projects per Microservice

Each microservice should include the following:

| Project Name                       | Purpose                                 |
|------------------------------------|------------------------------------------|
| Api                                | ASP.NET Core Web API entry point         |
| Domain                             | Business rules and core models           |
| Application                        | Use cases, CQRS handlers, DTOs           |
| Infrastructure                     | DB, file storage, APIs, etc.             |
| SharedKernel                       | Shared abstractions for reuse            |
| Integration (in `tests/`)          | Integration and end-to-end tests         |

---

## 🧪 Testing Strategy

- All tests go under the `tests/` folder.
- Test project must be named:  
  `GoodMarket.{{MicroserviceName}}.Integration`
- Keep test namespaces and folder structures aligned with the `src/` layout.

---

## 🛠️ Scaffold Script (PowerShell)

You can auto-generate the microservice folder and project structure using the script below.

```powershell
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
```

---

## ▶️ Usage Instructions

1. Save the script as `Create-Microservice.ps1`
2. Open a PowerShell terminal
3. Run:

```powershell
.\Create-Microservice.ps1 -MicroserviceName Customers
```

This creates the full folder structure and placeholder `.csproj` files for the **Customers** microservice.

---

## 📘 Optional Tips

- Use Visual Studio **solution folders** to mirror the physical structure.
- Maintain a global `GoodMarket.sln` to include all microservices.
- Use `dotnet sln add` to include projects in solution files.
- Consider adding `.editorconfig`, `.gitignore`, and default `README.md` per microservice folder for clarity.

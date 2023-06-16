Push-Location $PSScriptRoot

if (-not (Test-Path build)) { New-Item -ItemType Directory build }

dotnet clean TypeScriptConverter.sln -c Release -o build
dotnet build TypeScriptConverter.sln -c Release -o build

dotnet clean TypeScriptObject.sln -c Release -o build
dotnet build TypeScriptObject.sln -c Release -o build

if (-not (Test-Path build/lib)) { New-Item -ItemType Directory build/lib }
Copy-Item -Recurse -Verbose ./src/TypeScriptAstBuilder/lib/*.js ./build/lib/
Copy-Item -Recurse -Verbose ./src/TypeScriptAstBuilder/package.json ./build/lib/package.json

npm --prefix ./build/lib install ./build/lib

Pop-Location

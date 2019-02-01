if not exist build md build
dotnet clean -c Release
dotnet build -c Release
copy /y ".\src\Startup\bin\Release\netcoreapp2.1\*.dll" ".\build"
copy /y ".\src\Startup\bin\Release\netcoreapp2.1\*.json" ".\build"
copy /y ".\tscconfig.json" ".\build"
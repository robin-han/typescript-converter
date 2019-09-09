if not exist build md build
dotnet clean TypeScriptConverter.sln -c Release  -o build
dotnet build TypeScriptConverter.sln -c Release  -o build

dotnet clean TypeScriptObject.sln -c Release -o build
dotnet build TypeScriptObject.sln -c Release -o build

if not exist build\lib md build\lib
copy /y ".\src\TypeScriptAstBuilder\lib\*.js" ".\build\lib\"
copy /y ".\src\TypeScriptAstBuilder\package.json" ".\build\lib\package.json"

npm --prefix .\build\lib install .\build\lib

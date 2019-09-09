#!/bin/sh

mkdir -p build
dotnet clean TypeScriptConverter.sln -c Release  -o build
dotnet build TypeScriptConverter.sln -c Release  -o build

dotnet clean TypeScriptObject.sln -c Release -o build
dotnet build TypeScriptObject.sln -c Release -o build

mkdir -p build/lib
cp ./src/TypeScriptAstBuilder/lib/*.js ./build/lib
cp ./src/TypeScriptAstBuilder/package.json ./build/lib/package.json

npm --prefix ./build/lib install ./build/lib
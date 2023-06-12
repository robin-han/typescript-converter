
function normalize($path) {
    [System.IO.Path]::GetFullPath($path).TrimEnd([System.IO.Path]::DirectorySeparatorChar)
}

$dotnet = Split-Path -Parent (which dotnet)
$tools = Join-Path $PSScriptRoot 'tools'

$paths = ($env:Path -split [System.IO.Path]::PathSeparator) | `
    Where-Object { (normalize $_) -ne (normalize $dotnet) } | `
    Where-Object { (normalize $_) -ne (normalize $tools) }

$paths += $tools

$env:Path = ($paths -join [System.IO.Path]::PathSeparator)


[CmdletBinding()]
param (    
    [Parameter(ValueFromRemainingArguments=$true)]$args_app
)

$env:RUNC_MOUNT = $true

$vol = @(
    @{ 'node_modules' = './build/lib/node_modules' }
    @{ 'node_modules' = './src/TypeScriptAstBuilder/node_modules' }
)
runc 'node:current-alpine3.17' -volumes $vol -workdir . -entrypoint npm @args_app

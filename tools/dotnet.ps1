[CmdletBinding()]
param (    
    [Parameter(ParameterSetName="o")]$o,
    [Parameter(ValueFromRemainingArguments=$true)]$args_app
)

$env:RUNC_MOUNT = $true

if ( $o ) { $args_app += @( '-o', $o ) }

$vol = @(
    @{ 'dotnet_packages' = '/root/.local/share' }
)

runc 'mcr.microsoft.com/dotnet/sdk:7.0.302-alpine3.17' -volumes $vol -workdir . dotnet @args_app

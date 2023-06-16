[CmdletBinding()]
param (    
    [Parameter(ParameterSetName="pipe", ValueFromPipeline=$true, Position=0, Mandatory=$true)][string]$image,
    [Parameter()][switch]$exec,
    [Parameter()]$entrypoint,
    [Parameter()]$environment,
    [Parameter()]$workdir,
    [Parameter()]$mount,
    [Parameter()]$as,
    [Parameter()]$volumes,
    [Parameter()]$port,
    [Parameter(ValueFromRemainingArguments=$true)]$args_app
)

# relative to the root of the project
$base = (Get-Item (Join-Path $PSScriptRoot '..')).FullName

if ($env:RUNC_MOUNT) {
    $mount = $base
    $as = '/mnt'
}

$share = (Split-Path $mount -LeafBase)
$target = $(if ( $as ) { $as } else { $share })

if ($workdir) {
    Write-Verbose "workdir: $workdir"
    $workdir = (Join-Path $target ([System.IO.Path]::GetRelativePath($base, (Get-Item $workdir).FullName))).Replace("\", "/")
}

if ($mount) {
    if ((Get-Command vmrun -ErrorAction Ignore).Count -gt 0) {
        # outside mount
        if ($env:RUNC_VMMOUNT -ne $share) {
            $dm = (vmrun list | Select-String docker).Line
            Write-Verbose "vmware $dm"
            $folder = (Get-Item $mount).FullName
            vmrun -T ws removeSharedFolder $dm $share
            vmrun -T ws addSharedFolder $dm $share $folder
            $env:RUNC_VMMOUNT = $share
        }
        $mount = "/mnt/hgfs/$share"
    } else {
        # dind - docker in docker mount
        if ("$(df $mount)".Contains('-fuse')) {
            # vmware fuse support
            $fuse = ((df $mount --output=target) | Select-Object -Skip 1)
            $dc = ( docker ps --format json | ConvertFrom-Json ).Id | ForEach-Object { 
                docker inspect $_ --format json | ConvertFrom-Json 
            }
            $realpath = (($dc.Mounts | Where-Object Destination -eq $fuse).Source | Select-Object -First 1)
            $mount = (Join-Path $realpath $mount.Substring($fuse.Length))
        }
    }
    Write-Verbose "mounting $mount -> $target"
    $mnt = "type=bind,source=$mount,target=$target,consistency=cached"
    Write-Verbose "mount: $mnt"
}

if ($volumes) {
    $vol = $volumes | ForEach-Object { $v = $_; 
        $v.Keys | ForEach-Object { "$($_):$(if ($v[$_][0] -ne '/') { "$target/" })$($v[$_])" }
    }
    Write-Verbose "volumes: $vol"
}

$allports = @()
if ( -not $port ) { $allports += @( '--publish-all' ) }

$args_docker = @()
if ( $entrypoint ) { $args_docker += @( '--entrypoint', $entrypoint ) }
if ( $environment ) { $args_docker += ( $environment | ForEach-Object { @( '--env', $_ ) } ) }
if ( $workdir ) { $args_docker += @( '--workdir', $workdir ) }
if ( $mnt ) { $args_docker += @( '--mount', $mnt ) }
if ( $vol ) { $args_docker += @( $vol | ForEach-Object { @( '--volume', $_ ) } ) }
if ( $port ) { $args_docker += @( '--publish', "$($port):$($port)" ) }
Write-Verbose "docker args: $args_docker"

Write-Verbose "image: $image"

if ($exec) {
    $container = $images | Where-Object Image -eq $image | Select-Object -First 1
    if (-not $container) {
        Write-Error "Container running image $image not found!"
        return
    }
}

Write-Verbose "app args: $args_app"

if (-not $exec) {
    docker run --rm -it @allports @args_docker $image @args_app
} else {
    docker exec -it $container.Id @args_app
}

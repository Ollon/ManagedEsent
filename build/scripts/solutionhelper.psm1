

Function Get-VSLocation
{
    [CmdletBinding()]
    param
    (
        [ValidateSet('15.0','14.0','12.0')]
        [Parameter()]
        [System.String] $TargetVersion
    )

    return (Get-Item -Path 'HKLM:\SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7').GetValue($TargetVersion)
}

Function Get-MSBuildLocation
{
    [CmdletBinding()]
    param
    (
        [ValidateSet('15.0','14.0','12.0')]
        [Parameter()]
        [System.String] $TargetVsVersion
    )

    if ($TargetVsVersion -eq '15.0')
    {
        $VSLocation = Get-VSLocation -TargetVersion '15.0'
        return Join-Path $VSLocation -ChildPath 'MSBuild\15.0\Bin\msbuild.exe'
    }
    else
    {
        return Join-Path ${env:ProgramFiles(x86)} -ChildPath "MSBuild\$TargetVsVersion\Bin\msbuild.exe"
    }
}

Function Get-CurrentSolution
{
    [CmdletBinding()]
    param()

    return @((Get-ChildItem "$PSScriptRoot\..\..\" -Filter *.sln))[0].FullName   
}
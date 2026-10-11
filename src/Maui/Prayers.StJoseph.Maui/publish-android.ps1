$Project = "Prayers.StJoseph.Maui.csproj"

$xml = [xml](Get-Content $Project)
$AndroidTF = $xml.SelectNodes("//*[local-name()='TargetFrameworks']") |
    ForEach-Object { $_.InnerText -split '[;\s]+' } |
    Where-Object { $_ -match '^net\d+\.\d+-android' } |
    Sort-Object Length -Descending | Select-Object -First 1
if (-not $AndroidTF) { Write-Error "Android TF not found"; exit 1 }
$AndroidTF = $AndroidTF.Trim()
Write-Host "==> Target: $AndroidTF"

$OutDir = "bin/Release/$AndroidTF/publish"

# Java
if (-not $env:JAVA_HOME) {
    $j = "/opt/homebrew/opt/openjdk@21"
    if (Test-Path $j) { $env:JAVA_HOME = $j; $env:PATH = "$j/bin$([IO.Path]::PathSeparator)$env:PATH" }
}

# Android SDK
$SdkRoot = $env:ANDROID_HOME
if (-not $SdkRoot) {
    foreach ($c in @("$HOME/Library/Android/sdk", "$HOME/Android/Sdk")) {
        if (Test-Path $c) { $SdkRoot = $c; break }
    }
}
if (-not $SdkRoot) { Write-Error "Android SDK not found"; exit 1 }

Write-Host "==> Publishing release AAB..."
dotnet publish $Project -f $AndroidTF -c Release -p:AndroidSdkDirectory=$SdkRoot
if ($LASTEXITCODE -ne 0) { exit 1 }

$aab = Get-ChildItem $OutDir -Filter "*.aab" -ErrorAction SilentlyContinue | Select-Object -First 1
if (-not $aab) { Write-Error "AAB not found under $OutDir"; exit 1 }

Write-Host ""
Write-Host "==> Upload to Play Store:" -ForegroundColor Green
Write-Host "    $($aab.FullName)" -ForegroundColor Cyan

#!/bin/bash
set -e

PROJECT="Prayers.StJoseph.Maui.csproj"

IOS_TF=$(grep -oE 'net[0-9]+\.[0-9]+-ios[0-9.]+' "$PROJECT" | head -1)
[ -z "$IOS_TF" ] && { echo "ERROR: could not determine iOS TF"; exit 1; }
echo "==> Target: $IOS_TF"

[[ "$(uname)" != "Darwin" ]] && { echo "ERROR: requires macOS"; exit 1; }
xcode-select -p &>/dev/null || { echo "ERROR: Xcode tools not found"; exit 1; }

OUT_DIR="bin/Release/$IOS_TF/ios-arm64/publish"

echo "==> Publishing..."
dotnet publish "$PROJECT" -f "$IOS_TF" -c Release

IPA=$(find "$OUT_DIR" -name "*.ipa" 2>/dev/null | head -1)
[ -z "$IPA" ] && { echo "ERROR: IPA not found under $OUT_DIR"; exit 1; }

echo ""
echo "==> Upload this IPA to App Store Connect:"
echo "    $IPA"

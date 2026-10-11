#!/bin/zsh

# Usage:
#   ./publish-ios.sh

PROJECT="CatholicPrayers.Maui.csproj"
BUNDLE_ID="com.pj.catholicprayersapp"

# Check macOS + Xcode
if [[ "$(uname)" != "Darwin" ]]; then
  echo "ERROR: iOS publishing requires macOS"
  exit 1
fi

if ! command -v xcodebuild &>/dev/null; then
  echo "ERROR: Xcode not found"
  exit 1
fi

echo "==> Publishing iOS release (net9.0-ios18.0)..."
cd "$(dirname "$0")"
dotnet publish "$PROJECT" -f net9.0-ios18.0 -c Release || exit 1

IPA=$(find "bin/Release/net9.0-ios18.0" -name "*.ipa" 2>/dev/null | head -1)
if [ -z "$IPA" ]; then
  echo "WARNING: IPA not found (check ArchiveOnBuild setting)"
else
  echo ""
  echo "==> Done. Upload this IPA to App Store Connect:"
  echo "    $IPA"
fi

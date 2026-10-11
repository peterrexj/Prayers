#!/bin/zsh

# Usage:
#   ./run-ios.sh                         # iPhone 16 Pro (default)
#   ./run-ios.sh --ipad                  # iPad Pro 13-inch (M4)
#   ./run-ios.sh --device "iPhone 15"    # specific simulator name
#   ./run-ios.sh --clean                 # clean before build

PROJECT="CatholicPrayers.Maui.csproj"
APP_BUNDLE="bin/Debug/net9.0-ios18.0/iossimulator-arm64/CatholicPrayers.Maui.app"
BUNDLE_ID="com.pj.catholicprayersapp"

SIMULATOR_NAME="iPhone 16 Pro"
CLEAN=false
EXPLICIT_DEVICE=false

for arg in "$@"; do
  case "$arg" in
    --ipad)  SIMULATOR_NAME="iPad Pro 13-inch (M4)"; EXPLICIT_DEVICE=true ;;
    --clean) CLEAN=true ;;
    --device) EXPLICIT_DEVICE=true ;;
  esac
done

for i in $(seq 1 $#); do
  if [ "${@[$i]}" = "--device" ] && [ $((i+1)) -le $# ]; then
    SIMULATOR_NAME="${@[$((i+1))]}"
  fi
done

cd "$(dirname "$0")"

echo "==> Finding simulator: $SIMULATOR_NAME..."
# Two traps here, both of which silently give you the wrong simulator:
#   1. The trailing " (" anchors the name — without it "iPhone 16 Pro" also matches
#      "iPhone 16 Pro Max".
#   2. The same name exists on every installed iOS runtime, and simctl lists them oldest-first,
#      so `head -1` picks the OLDEST. Track the runtime header and keep the newest match instead.
SIMULATOR_ID=$(xcrun simctl list devices available | awk -v name="$SIMULATOR_NAME" '
  /^-- iOS /           { split($3, v, "."); cur = v[1] * 1000 + v[2]; next }
  index($0, name " (") { if (cur >= best) { best = cur; line = $0 } }
  END { if (line != "") { match(line, /[0-9A-F-]{36}/); print substr(line, RSTART, RLENGTH) } }')

if [ -z "$SIMULATOR_ID" ] && [ "$EXPLICIT_DEVICE" = "false" ]; then
  echo "    '$SIMULATOR_NAME' not installed; picking an available simulator instead."
  # Prefer an already-booted device (no boot wait); otherwise the newest-runtime
  # iPhone; otherwise the newest-runtime device of any kind (e.g. iPad-only machines).
  SIMULATOR_ID=$(xcrun simctl list devices available | awk '
    /^-- iOS /    { split($3, v, "."); cur = v[1] * 1000 + v[2]; next }
    /\(Booted\)/  { if (booted == "") { match($0, /[0-9A-F-]{36}/); booted = substr($0, RSTART, RLENGTH) } }
    /iPhone/      { if (cur >= bestPhone) { bestPhone = cur; phoneLine = $0 } }
    /iPhone|iPad/ { if (cur >= bestAny) { bestAny = cur; anyLine = $0 } }
    END {
      if (booted != "") { print booted; exit }
      line = (phoneLine != "") ? phoneLine : anyLine
      if (line != "") { match(line, /[0-9A-F-]{36}/); print substr(line, RSTART, RLENGTH) }
    }')
fi

if [ -z "$SIMULATOR_ID" ]; then
  echo "ERROR: No simulator found matching '$SIMULATOR_NAME'"
  echo "Available simulators:"
  xcrun simctl list devices available | grep -v "^==" | grep -v "^--" | grep -v "^$" | grep "iPhone\|iPad"
  exit 1
fi

echo "    Found: $SIMULATOR_ID"

echo "==> Booting simulator..."
STATUS=$(xcrun simctl list devices | grep "$SIMULATOR_ID" | grep -o "Booted")
if [ "$STATUS" != "Booted" ]; then
  xcrun simctl boot "$SIMULATOR_ID"
  open -a Simulator
else
  echo "    Simulator already booted"
fi

if [ "$CLEAN" = "true" ]; then
  echo "==> Cleaning..."
  dotnet clean "$PROJECT" -f net9.0-ios18.0 -c Debug
fi

echo "==> Building..."
dotnet build "$PROJECT" -f net9.0-ios18.0 -c Debug || exit 1

echo "==> Installing..."
xcrun simctl install "$SIMULATOR_ID" "$APP_BUNDLE" || exit 1

echo "==> Terminating existing instance..."
xcrun simctl terminate "$SIMULATOR_ID" "$BUNDLE_ID" 2>/dev/null || true

echo "==> Launching..."
xcrun simctl launch "$SIMULATOR_ID" "$BUNDLE_ID"

echo "==> Done"

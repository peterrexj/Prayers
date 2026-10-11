#!/bin/zsh

# Usage:
#   ./run-ios.sh                          # iPhone 16 Pro (default)
#   ./run-ios.sh --ipad                   # iPad Pro 13-inch (M4)
#   ./run-ios.sh --device "iPhone 15"     # specific simulator
#   ./run-ios.sh --logs                   # stream all output
#   ./run-ios.sh --nologs                 # launch silently

PROJECT="Prayers.StJoseph.Maui.csproj"
APP_BUNDLE="bin/Debug/net9.0-ios18.0/iossimulator-arm64/Prayers.StJoseph.Maui.app"
BUNDLE_ID="com.pj.stjosephnovenamalayalam"

SIMULATOR_NAME="iPhone 16 Pro"
LOG_MODE="filtered"
EXPLICIT_DEVICE=false

for arg in "$@"; do
  case "$arg" in
    --ipad)   SIMULATOR_NAME="iPad Pro 13-inch (M4)"; EXPLICIT_DEVICE=true ;;
    --logs)   LOG_MODE="full" ;;
    --nologs) LOG_MODE="none" ;;
    --device) EXPLICIT_DEVICE=true ;;
  esac
done

args=("$@")
for i in $(seq 0 $((${#args[@]}-1))); do
  if [ "${args[$i]}" = "--device" ] && [ $((i+1)) -lt ${#args[@]} ]; then
    SIMULATOR_NAME="${args[$((i+1))]}"
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
  echo "ERROR: No simulator matching '$SIMULATOR_NAME'"
  xcrun simctl list devices available | grep -E "iPhone|iPad"
  exit 1
fi
echo "    Found: $SIMULATOR_ID"

echo "==> Booting simulator..."
STATUS=$(xcrun simctl list devices | grep "$SIMULATOR_ID" | grep -o "Booted")
if [ "$STATUS" != "Booted" ]; then
  xcrun simctl boot "$SIMULATOR_ID"
  open -a Simulator
else
  echo "    Already booted"
fi

echo "==> Building..."
dotnet build "$PROJECT" -f net9.0-ios18.0 -c Debug || exit 1

echo "==> Installing..."
xcrun simctl install "$SIMULATOR_ID" "$APP_BUNDLE" || exit 1

echo "==> Terminating existing instance..."
xcrun simctl terminate "$SIMULATOR_ID" "$BUNDLE_ID" 2>/dev/null || true

if [ "$LOG_MODE" = "none" ]; then
  xcrun simctl launch "$SIMULATOR_ID" "$BUNDLE_ID"
  echo "==> Done."
elif [ "$LOG_MODE" = "full" ]; then
  echo "==> Launching (full logs, Ctrl+C to stop)..."
  xcrun simctl launch --console-pty "$SIMULATOR_ID" "$BUNDLE_ID" 2>&1
else
  echo "==> Launching (filtered logs, Ctrl+C to stop)..."
  xcrun simctl launch --console-pty "$SIMULATOR_ID" "$BUNDLE_ID" 2>&1 \
    | grep --line-buffered -E "error|Error|exception|Exception|Unhandled|fatal|Fatal|\[CRASH\]" \
    | grep -v "Sentry\|NSURLError\|TaskCancel"
fi

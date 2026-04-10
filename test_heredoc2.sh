#!/bin/bash
REPO_DIR=""
cat <<EOF_REMOTE
set -euo pipefail
REPO_PATH="${REPO_DIR:-/opt/tarotnow/TarotNowAI2}"
cd "\$REPO_PATH"
EOF_REMOTE

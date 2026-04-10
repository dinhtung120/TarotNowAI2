#!/usr/bin/env bash
set -euo pipefail

BACKEND_JSON="${BACKEND_JSON:-trivy-backend.json}"
FRONTEND_JSON="${FRONTEND_JSON:-trivy-frontend.json}"
FS_JSON="${FS_JSON:-trivy-fs.json}"
SONAR_STATUS="${SONAR_STATUS:-unknown}"
CODEQL_RESULT="${CODEQL_RESULT:-unknown}"
SECURITY_SCAN_RESULT="${SECURITY_SCAN_RESULT:-unknown}"
OUTPUT_FILE="${1:-security-quality-report.md}"

if ! command -v jq >/dev/null 2>&1; then
  echo "jq is required for summarize-findings" >&2
  exit 1
fi

count_severity() {
  local file="$1"
  local severity="$2"
  if [[ ! -f "$file" ]]; then
    echo 0
    return
  fi

  jq --arg sev "$severity" '
    [.Results[]? | .Vulnerabilities[]? | select(.Severity == $sev)] | length
  ' "$file"
}

total_vulns() {
  local file="$1"
  if [[ ! -f "$file" ]]; then
    echo 0
    return
  fi

  jq '[.Results[]? | .Vulnerabilities[]?] | length' "$file"
}

render_top_rows() {
  local file="$1"
  local max_rows="${2:-10}"

  if [[ ! -f "$file" ]]; then
    return
  fi

  jq -r --argjson limit "$max_rows" '
    [.Results[]? as $r
      | ($r.Target // "-") as $target
      | ($r.Vulnerabilities[]? // empty)
      | {
          target: $target,
          severity: (.Severity // "-"),
          pkg: (.PkgName // "-"),
          installed: (.InstalledVersion // "-"),
          fixed: (.FixedVersion // "-"),
          id: (.VulnerabilityID // "-"),
          title: (.Title // "-")
        }
    ]
    | sort_by(.severity)
    | reverse
    | .[:$limit]
    | .[]
    | "| \(.target) | \(.severity) | \(.pkg) | \(.installed) | \(.fixed) | \(.id) | \(.title | gsub("\\|"; "/")) |"
  ' "$file"
}

backend_critical="$(count_severity "$BACKEND_JSON" "CRITICAL")"
backend_high="$(count_severity "$BACKEND_JSON" "HIGH")"
backend_total="$(total_vulns "$BACKEND_JSON")"

frontend_critical="$(count_severity "$FRONTEND_JSON" "CRITICAL")"
frontend_high="$(count_severity "$FRONTEND_JSON" "HIGH")"
frontend_total="$(total_vulns "$FRONTEND_JSON")"

fs_critical="$(count_severity "$FS_JSON" "CRITICAL")"
fs_high="$(count_severity "$FS_JSON" "HIGH")"
fs_total="$(total_vulns "$FS_JSON")"

{
  echo "# Security and Quality Report"
  echo
  echo "Generated at: $(date -u +'%Y-%m-%dT%H:%M:%SZ')"
  echo
  echo "## Gate Status"
  echo
  echo "- CodeQL result: \`$CODEQL_RESULT\`"
  echo "- Sonar quality gate: \`$SONAR_STATUS\`"
  echo "- Security scan job: \`$SECURITY_SCAN_RESULT\`"
  echo
  echo "## Trivy Summary"
  echo
  echo "| Scan Target | Total Vulns | CRITICAL | HIGH |"
  echo "|---|---:|---:|---:|"
  echo "| backend image | $backend_total | $backend_critical | $backend_high |"
  echo "| frontend image | $frontend_total | $frontend_critical | $frontend_high |"
  echo "| repository fs | $fs_total | $fs_critical | $fs_high |"
  echo
  echo "## Top Backend Vulnerabilities"
  echo
  echo "| Target | Severity | Package | Installed | Fixed | ID | Title |"
  echo "|---|---|---|---|---|---|---|"
  render_top_rows "$BACKEND_JSON" 10
  echo
  echo "## Top Frontend Vulnerabilities"
  echo
  echo "| Target | Severity | Package | Installed | Fixed | ID | Title |"
  echo "|---|---|---|---|---|---|---|"
  render_top_rows "$FRONTEND_JSON" 10
  echo
  echo "## Top Filesystem Findings"
  echo
  echo "| Target | Severity | Package | Installed | Fixed | ID | Title |"
  echo "|---|---|---|---|---|---|---|"
  render_top_rows "$FS_JSON" 10
} > "$OUTPUT_FILE"

if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
  cat "$OUTPUT_FILE" >> "$GITHUB_STEP_SUMMARY"
fi

if [[ -n "${GITHUB_OUTPUT:-}" ]]; then
  {
    echo "backend_critical=$backend_critical"
    echo "backend_high=$backend_high"
    echo "frontend_critical=$frontend_critical"
    echo "frontend_high=$frontend_high"
    echo "fs_critical=$fs_critical"
    echo "fs_high=$fs_high"
    echo "sonar_status=$SONAR_STATUS"
    echo "codeql_result=$CODEQL_RESULT"
    echo "security_scan_result=$SECURITY_SCAN_RESULT"
  } >> "$GITHUB_OUTPUT"
fi

echo "[summary] report written to $OUTPUT_FILE"

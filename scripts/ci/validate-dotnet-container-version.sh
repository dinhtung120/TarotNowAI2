#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
global_json="$repo_root/Backend/global.json"
dockerfile="$repo_root/Backend/Dockerfile"

if [[ ! -f "$global_json" ]]; then
  echo "Missing file: $global_json" >&2
  exit 1
fi

if [[ ! -f "$dockerfile" ]]; then
  echo "Missing file: $dockerfile" >&2
  exit 1
fi

sdk_full_version="$(sed -nE 's/^[[:space:]]*"version"[[:space:]]*:[[:space:]]*"([0-9]+\.[0-9]+\.[0-9]+)".*$/\1/p' "$global_json" | head -n1)"
if [[ -z "$sdk_full_version" ]]; then
  echo "Cannot parse SDK version from $global_json" >&2
  exit 1
fi
expected_major_minor="$(awk -F. '{print $1"."$2}' <<<"$sdk_full_version")"

docker_arg_version="$(sed -nE 's/^ARG[[:space:]]+DOTNET_VERSION=([0-9]+\.[0-9]+).*$/\1/p' "$dockerfile" | head -n1)"

if [[ -z "$docker_arg_version" ]]; then
  echo "Cannot parse DOTNET_VERSION ARG from $dockerfile" >&2
  exit 1
fi

if ! grep -Eq '^FROM[[:space:]]+mcr\.microsoft\.com/dotnet/sdk:\$\{DOTNET_VERSION\}([[:alnum:]._-]*)?[[:space:]]+AS[[:space:]]+build$' "$dockerfile"; then
  echo "Dockerfile build base image must reference \${DOTNET_VERSION} (sdk stage)." >&2
  exit 1
fi

if ! grep -Eq '^FROM[[:space:]]+mcr\.microsoft\.com/dotnet/aspnet:\$\{DOTNET_VERSION\}([[:alnum:]._-]*)?[[:space:]]+AS[[:space:]]+runtime$' "$dockerfile"; then
  echo "Dockerfile runtime base image must reference \${DOTNET_VERSION} (aspnet stage)." >&2
  exit 1
fi

if [[ "$docker_arg_version" != "$expected_major_minor" ]]; then
  echo "Version mismatch: Backend/global.json SDK=$expected_major_minor, Dockerfile ARG DOTNET_VERSION=$docker_arg_version" >&2
  exit 1
fi

echo "Dotnet version check passed: SDK=$expected_major_minor, Dockerfile ARG=$docker_arg_version"

# Báo cáo quét file rác / không cần / phân vân

Ngày quét: 2026-05-13

Phạm vi: toàn repo `TarotNowAI2`, gồm root, Backend, Frontend, docs/config/test artifacts.

Nguyên tắc: báo cáo-only. Không xóa, không sửa file ngoài file báo cáo này.

## Tóm tắt

- Có nhiều artifact chắc chắn nên dọn: `.DS_Store`, logs local, cache/build output, cookie dump, output lỗi build cũ, Next starter SVG không dùng.
- Có nhiều file có thể không cần nhưng cần xác nhận mục đích lưu evidence: benchmark artifacts, performance docs/plans cũ, `tmp/notification-review/`, script probe performance.
- Có nhóm phân vân không nên xóa tự động: docs/review lớn, dev certs, tarot card images, CI/deploy/config, files dùng theo convention/framework.

## Rác chắc chắn

| Path | Lý do | Bằng chứng |
|---|---|---|
| `.DS_Store` | macOS artifact | `find` thấy file rác hệ điều hành |
| `Backend/.DS_Store` | macOS artifact | `find` thấy file rác hệ điều hành |
| `Frontend/public/.DS_Store` | macOS artifact | `find` thấy file rác hệ điều hành |
| `Frontend/public/images/.DS_Store` | macOS artifact | `find` thấy file rác hệ điều hành |
| `Frontend/public/images/gacha/.DS_Store` | macOS artifact | `find Frontend/public` thấy |
| `.playwright-mcp/console-*.log` | log Playwright MCP local | untracked, không source reference |
| `.playwright-mcp/page-*.yml` | snapshot Playwright MCP local | tracked, không source reference |
| `.superpowers/brainstorm/*` | cache/session artifact local | không thuộc app source |
| `tmux-log10.log` | log local | untracked |
| `Frontend/.next/` | Next.js build output/cache | build artifact, không nên track |
| `Frontend/.next-log` | Next.js log | tracked suspicious artifact |
| `Frontend/coverage/` | test coverage output | generated artifact |
| `Backend/src/build_errors.txt` | output lỗi build cũ | `find` thấy; không reference trong repo |
| `Backend/cookies.txt` | cookie dump local | `git grep` không thấy reference |
| `Backend/test-api.js` | script test thủ công root Backend | `git grep` không thấy reference |
| `Backend/test-paid.js` | script test thủ công root Backend | `git grep` không thấy reference |
| `Backend/src/TarotNow.Api/wwwroot/test.txt` | file test tĩnh trong API wwwroot | không có reference qua `git grep` |
| `Frontend/public/file.svg` | asset starter Next.js | không thấy reference |
| `Frontend/public/globe.svg` | asset starter Next.js | không thấy reference |
| `Frontend/public/next.svg` | asset starter Next.js | không thấy reference |
| `Frontend/public/vercel.svg` | asset starter Next.js | không thấy reference |
| `Frontend/public/window.svg` | asset starter Next.js | không thấy reference |

## Có thể không cần

| Path | Lý do | Bằng chứng |
|---|---|---|
| `Frontend/benchmark-results/benchmark/runs/*` | nhiều kết quả benchmark theo timestamp, giống artifact lịch sử | 127 tracked suspicious artifact; chỉ `index.json` reference nội bộ tới runs |
| `Frontend/benchmark-results/benchmark/latest/*` | generated latest benchmark output | `benchmark-results` chỉ thấy reference nội bộ và docs performance |
| `Frontend/benchmark-results/benchmark/*.json` | generated benchmark output | file lớn; script/docs dùng audit flow, không phải runtime |
| `perf-artifacts/` | benchmark artifact ngoài app source | file benchmark lớn ngoài cache; cần giữ nếu là bằng chứng performance |
| `tmp/notification-review/*.json` | dữ liệu inventory tạm | path nằm trong `tmp/`, generated inventory |
| `tmp/notification-review/appendix_domain_inventory.md` | report tạm từ inventory | chỉ tự chứa evidence, không app source |
| `PERFORMANCE-AUDIT.md` | generated report | `Frontend/scripts/generate-performance-audit.mjs` ghi file này; docs performance reference |
| `docs/superpowers/specs/*performance*` | spec/plan cũ cho performance work | không app runtime; chỉ lịch sử làm việc |
| `docs/superpowers/plans/*performance*` | plan cũ cho performance work | không app runtime; chỉ lịch sử làm việc |
| `docs/superpowers/reports/*performance*` | report cũ | không app runtime; chỉ lịch sử làm việc |
| `Frontend/scripts/probe-performance-headers.mjs` | script probe performance tạo cho plan cũ | chỉ reference trong `docs/superpowers/plans/2026-05-10-performance-bottleneck-optimization.md`; không có package script |
| `Frontend/tests/example.spec.ts` | e2e test tên generic, giống scaffold/example | vẫn có test thật; `docs/frontend-inventory-review.md` cũng ghi “tên file còn generic; nên đổi nếu tiếp tục giữ” |
| `Backend/src/TarotNow.Api/TarotNow.Api.http` | HTTP scratch file dev | không reference; có thể hữu ích thủ công trong IDE |
| `Frontend/public/img/*.avif` | 78 tarot card images, không thấy direct frontend reference | có thể backend/data trả path động, nên chưa xếp rác chắc chắn |

## Phân vân cần người xem

| Path | Lý do | Bằng chứng |
|---|---|---|
| `Frontend/certificates/localhost-key.pem` | cert dev HTTPS; có thể cần chạy local HTTPS | không kết luận xóa vì có thể dùng ngoài script |
| `Frontend/certificates/localhost.pem` | cert dev HTTPS; có thể cần chạy local HTTPS | không kết luận xóa vì có thể dùng ngoài script |
| `Review/` | bộ review kiến trúc lớn | có mục lục và kế hoạch nội bộ; không runtime |
| `Doccument/` | docs lớn, tên thư mục typo nhưng có thể là tài liệu chính | không app runtime; nhiều tài liệu thiết kế |
| `docs/backend-architecture-*.md` | review/fix plan kiến trúc | có liên quan commit gần đây; nên giữ nếu đang follow plan |
| `docs/frontend-deep-review-*.md` | review frontend | không runtime; giữ nếu còn dùng làm backlog |
| `docs/frontend-inventory-review.md` | inventory/review | không runtime; giữ nếu còn dùng làm audit |
| `.agents/rules/code-style-guide.md` | agent rules | có thể dùng bởi external agent tooling |
| `rule-code-day-du.md` | rule doc root | không thấy reference, nhưng có thể là hướng dẫn thủ công |
| `docker-compose*.yml`, `.github/workflows/*` | deploy/CI | không rác dù không runtime import; dùng ngoài app code |

## Không phải rác dù scan báo nghi vấn

| Path | Lý do giữ |
|---|---|
| `Frontend/public/images/collection/back-card.svg` | dùng ở `Frontend/src/features/collection/cards/useCollectionPageViewModel.ts` |
| `Frontend/public/lottie/*.json` | dùng ở `Frontend/src/features/gacha/result/hooks/useRareDropLottie.ts` |
| `Frontend/public/themes/*.css` | dùng động qua `Frontend/src/app/_shared/models/theme.ts` |
| `Frontend/public/locales/*/common.json` | localization public assets; không kết luận vì có thể load runtime |
| `Frontend/scripts/audit-shared-direct-imports.mjs` | docs inventory gọi là guard boundary; không package script nhưng có mục đích kiến trúc |
| `Frontend/src/app/[locale]/not-found.tsx` | Next.js convention file; basename scan false positive |
| `Frontend/src/**/*.test.*` không có sibling source | nhiều test target module/index/action không cùng basename; không kết luận rác |
| Backend controllers/handlers/partials “basename orphan” | C# framework/DI/partial class làm scan nhiễu; không dùng làm bằng chứng xóa |

## Gói dọn an toàn nhất nếu muốn làm bước sau

1. Xóa `.DS_Store` toàn repo.
2. Xóa local logs/cache: `.playwright-mcp/console-*.log`, `.superpowers/`, `tmux-log10.log`.
3. Xóa generated build/test output: `Frontend/.next/`, `Frontend/coverage/`, `Frontend/.next-log`.
4. Xóa file chắc rác: `Backend/src/build_errors.txt`, `Backend/cookies.txt`, `Backend/src/TarotNow.Api/wwwroot/test.txt`, Next starter SVGs.
5. Trước khi xóa nhóm “có thể không cần”: xác nhận `Frontend/public/img/*.avif`, benchmark artifacts, `tmp/notification-review/`, performance docs có còn cần làm evidence không.

## Lệnh đã dùng để kiểm tra

```bash
find . -maxdepth 3 -not -path './.git/*' -not -path './node_modules/*' -not -path './Frontend/node_modules/*' -not -path './Backend/**/bin/*' -not -path './Backend/**/obj/*' -print | sort | head -200

git ls-files | grep -E '(^|/)(\.DS_Store|\.next|coverage|benchmark-results|cookies\.txt|build_errors\.txt|test-api\.js|test-paid\.js|\.playwright-mcp|\.superpowers|certificates/|\.next-log)' | sort

git status --short --untracked-files=normal

find . -maxdepth 4 \( -path './.git' -o -path './Frontend/node_modules' -o -path './node_modules' \) -prune -o -type f \( -name '.DS_Store' -o -name '*.log' -o -name '*.tmp' -o -name '*.bak' -o -name 'build_errors.txt' -o -name 'cookies.txt' \) -print | sort

git grep -n "test-api\|test-paid\|cookies.txt\|build_errors.txt\|benchmark-results\|\.playwright-mcp\|\.next-log\|localhost-key.pem\|localhost.pem" -- . ':!Frontend/.next' ':!Frontend/coverage'

git ls-files 'Frontend/.next/*' 'Frontend/coverage/*' 'Backend/**/bin/*' 'Backend/**/obj/*' 'Frontend/certificates/*' 'tmux-log10.log' '.DS_Store' 'Backend/.DS_Store' 'Frontend/public/.DS_Store' 'Frontend/public/images/.DS_Store'

find . -maxdepth 3 -type f \( -name '*.csproj' -o -name 'package.json' -o -name '*.slnx' -o -name 'Dockerfile' -o -name 'docker-compose*.yml' -o -name '*.md' \) -not -path './.git/*' | sort

git ls-files | grep -E '(^tmp/|^Review/|^docs/|^Doccument/|^PERFORMANCE-AUDIT.md$|^rule-code-day-du.md$|^Backend/src/build_errors.txt$|^Backend/(test-api|test-paid)\.js$|^Backend/cookies\.txt$|^Frontend/benchmark-results/|^Frontend/\.next-log$|^\.playwright-mcp/)' | sort

git grep -n "tmp/notification-review\|PERFORMANCE-AUDIT\|rule-code-day-du\|Doccument\|Review/\|frontend-deep-review\|backend-architecture-fix-plan\|backend-test-review-findings\|frontend-inventory-review" -- . ':!Frontend/.next' ':!Frontend/coverage'

find . -maxdepth 5 \( -path './.git' -o -path './Frontend/node_modules' -o -path './Frontend/.next' -o -path './Frontend/coverage' \) -prune -o -type f -size +1M -print | sort

find Frontend/src Frontend/scripts -type f \( -name '*.ts' -o -name '*.tsx' -o -name '*.mjs' \) | while read -r f; do case "$f" in *.test.ts|*.test.tsx|*.spec.ts|*.spec.tsx) continue;; esac; b=$(basename "$f"); stem=${b%.*}; refs=$(git grep -l "$stem" -- Frontend/src Frontend/scripts Frontend/package.json Frontend/tsconfig.json Frontend/next.config.* 2>/dev/null | grep -v "^$f$" | wc -l | tr -d ' '); if [ "$refs" = "0" ]; then printf '%s\n' "$f"; fi; done | sort | head -250

find Frontend/src -type f \( -name '*.test.ts' -o -name '*.test.tsx' -o -name '*.spec.ts' -o -name '*.spec.tsx' \) | while read -r f; do prod=${f%.test.ts}; prod=${prod%.test.tsx}; prod=${prod%.spec.ts}; prod=${prod%.spec.tsx}; if [ ! -e "$prod.ts" ] && [ ! -e "$prod.tsx" ] && [ ! -e "$prod.mts" ] && [ ! -e "$prod.mjs" ]; then printf '%s\n' "$f"; fi; done | sort | head -200

git grep -n "audit-shared-direct-imports\|probe-performance-headers\|not-found" -- Frontend/package.json Frontend/scripts Frontend/src 'Frontend/next.config.*' 2>/dev/null || true

find Frontend/public -type f | sort | while read -r f; do b=$(basename "$f"); refs=$(git grep -l "$b" -- Frontend/src Frontend/public Frontend/scripts Frontend/package.json 2>/dev/null | grep -v "^$f$" | wc -l | tr -d ' '); if [ "$refs" = "0" ]; then printf '%s\n' "$f"; fi; done | head -200

git grep -n "file.svg\|globe.svg\|next.svg\|vercel.svg\|window.svg\|/img/\|public/img\|themes/\|common.json" -- Frontend/src Frontend/scripts Frontend/package.json 2>/dev/null | head -200 || true

git grep -n "01_The_Fool\|The_Fool\|public/img\|/img/\|back-card.svg\|epic-drop\|legendary-drop\|mythic-drop" -- Frontend/src Frontend/scripts Frontend/public 2>/dev/null | head -250 || true

git grep -n "Frontend/tests/example.spec\|example.spec\|@playwright/test\|playwright" -- Frontend/package.json Frontend/playwright.config.* Frontend/tests .github docs 2>/dev/null || true
```

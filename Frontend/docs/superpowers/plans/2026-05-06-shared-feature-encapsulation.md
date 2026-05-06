# Shared Feature Encapsulation Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Move code out of `Frontend/src/shared` unless it has direct consumers from at least two outside feature/app areas.

**Architecture:** Treat `src/shared` as a strict multi-owner boundary. Build a reproducible import inventory, manually classify every shared file or internal module group, then move one owner batch at a time so each feature owns its single-use helpers without behavior changes.

**Tech Stack:** Next.js 16, React 19, TypeScript strict mode, Vitest, ESLint, path alias `@/* -> ./src/*`.

---

## File Structure

**Create:**
- `Frontend/scripts/audit-shared-direct-imports.mjs` — repository-local audit script that reports direct imports of every `src/shared` file from outside `src/shared`.
- `Frontend/docs/superpowers/shared-inventory-2026-05-06.md` — working inventory used to record manual decisions for every shared file.

**Modify as classification requires:**
- `Frontend/src/shared/**` — remove or keep files according to the two-outside-area direct import rule.
- `Frontend/src/features/**` — receive helpers/components used by exactly one feature area.
- `Frontend/src/app/**` — receive helpers/components used only by app routes/layouts.
- `Frontend/src/proxy.ts` and adjacent root runtime files if they are the single outside owner of a shared helper.
- Existing imports in `Frontend/src/**/*.ts`, `Frontend/src/**/*.tsx`, and `Frontend/src/proxy.ts`.

**Do not create compatibility re-export shims.** Importers must point to the new owner path after a move.

---

### Task 1: Create the shared direct-import audit script

**Files:**
- Create: `Frontend/scripts/audit-shared-direct-imports.mjs`
- Modify: none
- Test: run the script and inspect output

- [ ] **Step 1: Add the audit script**

Create `Frontend/scripts/audit-shared-direct-imports.mjs` with this content:

```js
import { relative, resolve, sep } from 'node:path';
import { readdirSync, readFileSync } from 'node:fs';

const root = resolve(new URL('..', import.meta.url).pathname);
const srcRoot = resolve(root, 'src');
const sharedRoot = resolve(srcRoot, 'shared');
const extensions = ['.ts', '.tsx'];
const importPattern = /(?:import|export)\s+(?:type\s+)?(?:[^'\"]*?from\s+)?['\"]([^'\"]+)['\"]|import\s*\(\s*['\"]([^'\"]+)['\"]\s*\)/g;

function walk(dir) {
  const entries = readdirSync(dir, { withFileTypes: true });
  const files = [];

  for (const entry of entries) {
    const path = resolve(dir, entry.name);

    if (entry.isDirectory()) {
      if (entry.name === 'node_modules' || entry.name === '.next') continue;
      files.push(...walk(path));
      continue;
    }

    if (entry.isFile() && extensions.some((extension) => path.endsWith(extension))) {
      files.push(path);
    }
  }

  return files;
}

function stripExtension(path) {
  for (const extension of extensions) {
    if (path.endsWith(extension)) return path.slice(0, -extension.length);
  }

  return path;
}

function sharedSpecifiersFor(file) {
  const rel = relative(sharedRoot, file).split(sep).join('/');
  const withoutExtension = stripExtension(rel);

  return new Set([
    `@/shared/${withoutExtension}`,
    `@/shared/${withoutExtension}/index`,
  ]);
}

function areaFor(importer) {
  const rel = relative(srcRoot, importer).split(sep).join('/');
  const parts = rel.split('/');

  if (parts[0] === 'shared') return 'shared';
  if (parts[0] === 'features') return `features/${parts[1]}`;
  if (parts[0] === 'app') return 'app';

  return parts[0];
}

function importsBySpecifier(files) {
  const imports = new Map();

  for (const file of files) {
    const source = readFileSync(file, 'utf8');
    let match;

    while ((match = importPattern.exec(source))) {
      const specifier = match[1] ?? match[2];
      if (!specifier?.startsWith('@/shared/')) continue;

      if (!imports.has(specifier)) imports.set(specifier, []);
      imports.get(specifier).push(file);
    }
  }

  return imports;
}

const sourceFiles = walk(srcRoot);
const sharedFiles = walk(sharedRoot)
  .filter((file) => !file.endsWith('.test.ts') && !file.endsWith('.test.tsx'))
  .sort();
const imports = importsBySpecifier(sourceFiles);

console.log('| File | Outside areas | Outside importers | Shared importers | Decision |');
console.log('| --- | --- | --- | --- | --- |');

for (const file of sharedFiles) {
  const specifiers = sharedSpecifiersFor(file);
  const importers = [...specifiers].flatMap((specifier) => imports.get(specifier) ?? []);
  const outsideImporters = importers.filter((importer) => !relative(sharedRoot, importer).startsWith('..'));
  const actualOutsideImporters = importers.filter((importer) => relative(sharedRoot, importer).startsWith('..'));
  const areas = [...new Set(actualOutsideImporters.map(areaFor))].sort();
  const decision = areas.length >= 2 ? 'keep' : areas.length === 1 ? `move:${areas[0]}` : 'review-unused';

  const relFile = relative(root, file).split(sep).join('/');
  const outsideList = actualOutsideImporters.map((importer) => relative(root, importer).split(sep).join('/')).sort().join('<br>');
  const sharedList = outsideImporters.map((importer) => relative(root, importer).split(sep).join('/')).sort().join('<br>');

  console.log(`| ${relFile} | ${areas.join('<br>')} | ${outsideList} | ${sharedList} | ${decision} |`);
}
```

- [ ] **Step 2: Run the script**

Run from `Frontend`:

```bash
node scripts/audit-shared-direct-imports.mjs > /tmp/shared-inventory.md
```

Expected: command exits 0 and `/tmp/shared-inventory.md` starts with the markdown table header:

```markdown
| File | Outside areas | Outside importers | Shared importers | Decision |
```

- [ ] **Step 3: Fix the script if it fails due to path parsing**

If Node reports an invalid path derived from `import.meta.url`, replace the root line with this import and assignment:

```js
import { fileURLToPath } from 'node:url';

const root = resolve(fileURLToPath(new URL('..', import.meta.url)));
```

Then rerun:

```bash
node scripts/audit-shared-direct-imports.mjs > /tmp/shared-inventory.md
```

Expected: command exits 0.

- [ ] **Step 4: Commit the audit script**

```bash
git add scripts/audit-shared-direct-imports.mjs
git commit -m "chore: add shared import audit script"
```

Expected: new commit containing only the audit script.

---

### Task 2: Produce and manually classify the shared inventory

**Files:**
- Create: `Frontend/docs/superpowers/shared-inventory-2026-05-06.md`
- Modify: none
- Test: inventory has one row for every non-test file under `src/shared`

- [ ] **Step 1: Generate the initial inventory**

Run from `Frontend`:

```bash
node scripts/audit-shared-direct-imports.mjs > docs/superpowers/shared-inventory-2026-05-06.md
```

Expected: file exists and starts with the markdown table header.

- [ ] **Step 2: Count audited shared files**

Run from `Frontend`:

```bash
find src/shared -type f \( -name '*.ts' -o -name '*.tsx' \) ! -name '*.test.ts' ! -name '*.test.tsx' | wc -l
```

Expected: prints the number of shared source files excluding tests.

- [ ] **Step 3: Count inventory rows**

Run from `Frontend`:

```bash
grep -c '^| src/shared/' docs/superpowers/shared-inventory-2026-05-06.md
```

Expected: the row count equals the count from Step 2.

- [ ] **Step 4: Manually annotate move targets**

Open `docs/superpowers/shared-inventory-2026-05-06.md` and add a short note below the table with these sections:

```markdown

## Manual decisions

### keep

### move

### review-unused
```

For each generated table row, add exactly one bullet under one of those sections. Use this bullet format for keep rows:

```markdown
- `src/shared/<path>.ts` — direct outside consumers: `<area-a>`, `<area-b>`.
```

Use this bullet format for move rows:

```markdown
- `src/shared/<path>.ts` -> `src/features/<owner>/shared/<file>.ts` — only `<owner>` imports it.
```

Use this bullet format for review-unused rows:

```markdown
- `src/shared/<path>.ts` — no outside consumer in the audit; inspect internal imports before deletion or move.
```

Do not leave angle-bracket sample text in the file; replace every bullet with real paths and owners from the generated table.

- [ ] **Step 5: Verify no placeholder decisions remain**

Run from `Frontend`:

```bash
grep -nE '<[^>]+>|TBD|TODO' docs/superpowers/shared-inventory-2026-05-06.md
```

Expected: command exits 1 with no matches.

- [ ] **Step 6: Commit the inventory**

```bash
git add docs/superpowers/shared-inventory-2026-05-06.md
git commit -m "docs: classify shared module ownership"
```

Expected: new commit containing only the inventory file.

---

### Task 3: Move one-owner feature modules by owner batch

**Files:**
- Modify: files listed under `### move` in `Frontend/docs/superpowers/shared-inventory-2026-05-06.md`
- Modify: importers listed in the inventory rows for each moved file
- Test: relevant tests plus full lint/test after each batch

- [ ] **Step 1: Select a single owner batch**

Choose one owner from the `### move` section, such as `features/reader`, `features/auth`, or `app`. Only move files whose target owner is that same owner in this task pass.

Expected: the selected batch has a concrete list of source files and target paths in `docs/superpowers/shared-inventory-2026-05-06.md`.

- [ ] **Step 2: Move files with `mv`**

For each row in the selected batch, create the target directory and move the file. Example for a `features/reader` batch:

```bash
mkdir -p src/features/reader/shared
mv src/shared/path/to/file.ts src/features/reader/shared/file.ts
```

Expected: moved files no longer exist at their old `src/shared/...` paths and exist at the target owner paths.

- [ ] **Step 3: Move internal files belonging to moved entrypoints**

If a moved file imported internal files from `src/shared`, move those internal files into the same owner folder and preserve their relative import shape where possible. Example:

```bash
mkdir -p src/features/reader/shared/file-parts
mv src/shared/path/to/file-parts/*.tsx src/features/reader/shared/file-parts/
```

Expected: no moved entrypoint imports from its old internal `@/shared/...` module group.

- [ ] **Step 4: Update imports to the new owner path**

Use exact replacement for each moved file. Example:

```bash
python3 - <<'PY'
from pathlib import Path
root = Path('src')
replacements = {
    '@/shared/path/to/file': '@/features/reader/shared/file',
}
for path in root.rglob('*'):
    if path.suffix not in {'.ts', '.tsx'}:
        continue
    text = path.read_text()
    new = text
    for old, target in replacements.items():
        new = new.replace(old, target)
    if new != text:
        path.write_text(new)
PY
```

Expected: importers listed in the inventory now reference the new owner path.

- [ ] **Step 5: Verify old imports for the batch are gone**

Run one grep per moved old specifier. Example:

```bash
grep -R "@/shared/path/to/file" -n src proxy.ts
```

Expected: command exits 1 with no matches for moved specifiers. If matches remain in files that were also moved, update those imports to relative owner-local imports or the new alias path.

- [ ] **Step 6: Run focused tests for moved files that had tests**

If a moved file had a colocated test, move the test beside the new owner file and run it. Example:

```bash
npm run test -- src/features/reader/shared/file.test.ts
```

Expected: PASS.

- [ ] **Step 7: Run full frontend tests**

```bash
npm run test
```

Expected: Vitest exits 0.

- [ ] **Step 8: Run lint**

```bash
npm run lint
```

Expected: ESLint and custom lint scripts exit 0.

- [ ] **Step 9: Commit the owner batch**

```bash
git add src docs/superpowers/shared-inventory-2026-05-06.md
git commit -m "refactor: move single-owner shared modules into <owner>"
```

Replace `<owner>` with the selected owner, for example `reader` or `app routes`.

Expected: one commit per owner batch.

- [ ] **Step 10: Repeat for the next owner batch**

Repeat Steps 1-9 until every row in `### move` has been moved or deliberately reclassified in the inventory.

Expected: no remaining `move` row points to an unmoved `src/shared` file.

---

### Task 4: Resolve review-unused files

**Files:**
- Modify: files listed under `### review-unused` in `Frontend/docs/superpowers/shared-inventory-2026-05-06.md`
- Modify: related tests/imports as discovered
- Test: `npm run test`, `npm run lint`

- [ ] **Step 1: Inspect internal import chains**

For each `review-unused` file, search for its basename and alias path. Example for `src/shared/dom/scrollLock.ts`:

```bash
grep -R "scrollLock\|@/shared/dom/scrollLock" -n src
```

Expected: every `review-unused` file is classified as internal to a moved/kept entrypoint or genuinely unused.

- [ ] **Step 2: Reclassify internal files**

If a `review-unused` file is internal to an entrypoint already classified as `keep`, move its inventory note to `### keep`. If it is internal to an entrypoint classified as `move:<owner>`, move it with that owner batch and note the target path in `### move`.

Expected: no internal file remains ambiguous.

- [ ] **Step 3: Handle genuinely unused files conservatively**

If a file has no importers and is not a Next.js convention file, delete it only after confirming no dynamic import, route convention, or test reference uses it. Example for `src/shared/dom/scrollLock.ts`:

```bash
grep -R "scrollLock\|@/shared/dom/scrollLock" -n src tests 2>/dev/null
```

Expected: command exits 1 before deleting that file. If there is any match, do not delete; classify based on that owner.

- [ ] **Step 4: Run tests**

```bash
npm run test
```

Expected: Vitest exits 0.

- [ ] **Step 5: Run lint**

```bash
npm run lint
```

Expected: ESLint and custom lint scripts exit 0.

- [ ] **Step 6: Commit unused/internal cleanup**

```bash
git add src docs/superpowers/shared-inventory-2026-05-06.md
git commit -m "refactor: resolve unused shared module inventory"
```

Expected: commit contains only review-unused resolutions and inventory updates.

---

### Task 5: Final audit and UI verification

**Files:**
- Modify: none unless final audit finds missed imports
- Test: audit script, tests, lint, dev server browser smoke check when UI/app-shell moved

- [ ] **Step 1: Regenerate the audit**

```bash
node scripts/audit-shared-direct-imports.mjs > /tmp/shared-inventory-final.md
```

Expected: command exits 0.

- [ ] **Step 2: Confirm remaining shared files are keep or documented internal keep**

Compare final output with `docs/superpowers/shared-inventory-2026-05-06.md`:

```bash
grep '^| src/shared/' /tmp/shared-inventory-final.md
```

Expected: every remaining `src/shared` row either has at least two outside areas or is an internal file belonging to a kept shared entrypoint documented in the inventory.

- [ ] **Step 3: Run full tests**

```bash
npm run test
```

Expected: Vitest exits 0.

- [ ] **Step 4: Run full lint**

```bash
npm run lint
```

Expected: ESLint and custom lint scripts exit 0.

- [ ] **Step 5: Run production build if lint and tests pass**

```bash
npm run build
```

Expected: Next.js build exits 0.

- [ ] **Step 6: Browser smoke check UI-facing moves**

If any UI-facing app-shell, layout, navigation, theme, or route component moved, start the dev server:

```bash
npm run dev
```

Open the app in a browser and verify the affected golden paths: app shell loads, navigation renders, language/theme controls still work, and routes that imported moved components render without console errors.

Expected: affected pages render and browser console has no new runtime errors related to moved modules.

- [ ] **Step 7: Commit final audit fixes**

If Step 2-6 required any fixes, commit them:

```bash
git add src docs/superpowers/shared-inventory-2026-05-06.md
git commit -m "refactor: finalize shared feature encapsulation"
```

Expected: final commit exists only if additional fixes were needed.

---

## Self-Review

- Spec coverage: the plan creates a full shared file inventory, applies the direct two-outside-area rule, moves single-owner helpers into their owner, handles internal shared files through their entrypoint, and verifies with tests/lint/build/UI smoke checks.
- Placeholder scan: no `TBD`, `TODO`, `implement later`, or unspecified test steps remain in this plan.
- Type/path consistency: commands use the existing `Frontend` scripts from `package.json`, the `@/*` alias from `tsconfig.json`, and paths under `Frontend/src`.

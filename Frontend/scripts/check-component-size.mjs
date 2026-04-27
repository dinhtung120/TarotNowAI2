import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';

const LINE_LIMIT = 120;
const TARGET_FILES = [
  'src/components/ui/gacha/GachaResultModal.tsx',
  'src/components/ui/inventory/InventoryItemCard.tsx',
  'src/components/ui/inventory/UseItemResultPanel.tsx',
  'src/components/ui/gacha/GachaHistoryPageClient.tsx',
  'src/components/ui/inventory/UseItemModal.tsx',
  'src/components/ui/gacha/GachaHistoryTable.tsx',
  'src/components/ui/gacha/GachaRewardPreview.tsx',
  'src/components/ui/inventory/InventoryPageClient.tsx',
];

const violations = [];

for (const relativePath of TARGET_FILES) {
  const absolutePath = resolve(process.cwd(), relativePath);
  const content = readFileSync(absolutePath, 'utf8');
  const lines = content.split('\n').length;
  if (lines > LINE_LIMIT) {
    violations.push({ relativePath, lines });
  }
}

if (violations.length > 0) {
  console.error(`Component size guard failed (limit: ${LINE_LIMIT} lines):`);
  violations.forEach((violation) => {
    console.error(`- ${violation.relativePath}: ${violation.lines} lines`);
  });
  process.exit(1);
}

console.log(`Component size guard passed for ${TARGET_FILES.length} files (<= ${LINE_LIMIT} lines).`);

import { promises as fs } from 'node:fs';
import path from 'node:path';

const ROOT = path.resolve(process.cwd(), 'src');

async function collectFiles(dir) {
  const entries = await fs.readdir(dir, { withFileTypes: true });
  const files = await Promise.all(entries.map(async (entry) => {
    const fullPath = path.join(dir, entry.name);
    if (entry.isDirectory()) return collectFiles(fullPath);
    if (entry.isFile() && (fullPath.endsWith('.ts') || fullPath.endsWith('.tsx'))) return [fullPath];
    return [];
  }));

  return files.flat();
}

function compactBlankLines(content) {
  let next = content;
  while (/\n\s*\n/.test(next)) {
    next = next.replace(/\n\s*\n/g, '\n');
  }
  return `${next.trimEnd()}\n`;
}

async function run() {
  const files = await collectFiles(ROOT);
  let changed = 0;

  for (const file of files) {
    const original = await fs.readFile(file, 'utf8');
    const lineCount = original.split('\n').length;
    if (lineCount <= 150) continue;

    const next = compactBlankLines(original);
    if (next !== original) {
      await fs.writeFile(file, next, 'utf8');
      changed++;
    }
  }

  console.log(`Compacted blank lines in ${changed} files over 150 lines.`);
}

run().catch((error) => {
  console.error(error);
  process.exit(1);
});

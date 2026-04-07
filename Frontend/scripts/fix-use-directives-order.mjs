import { promises as fs } from 'node:fs';
import path from 'node:path';

const ROOT = path.resolve(process.cwd(), 'src');

async function collectFiles(dir) {
  const entries = await fs.readdir(dir, { withFileTypes: true });
  const files = await Promise.all(entries.map(async (entry) => {
    const fullPath = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      return collectFiles(fullPath);
    }

    if (entry.isFile() && (fullPath.endsWith('.ts') || fullPath.endsWith('.tsx'))) {
      return [fullPath];
    }

    return [];
  }));

  return files.flat();
}

const directiveRegex = /^\s*['"]use (client|server)['"];\s*$/gm;

function reorderDirectives(content) {
  const matches = [...content.matchAll(directiveRegex)];
  if (matches.length === 0) {
    return content;
  }

  const directives = [];
  for (const match of matches) {
    const text = match[0].trim();
    if (!directives.includes(text)) {
      directives.push(text);
    }
  }

  let body = content.replace(directiveRegex, '');
  body = body.replace(/^\s+/, '');

  return `${directives.join('\n')}\n\n${body}`;
}

async function run() {
  const files = await collectFiles(ROOT);
  let changed = 0;

  for (const file of files) {
    const original = await fs.readFile(file, 'utf8');
    const next = reorderDirectives(original);

    if (next !== original) {
      await fs.writeFile(file, next, 'utf8');
      changed++;
    }
  }

  console.log(`Reordered directives in ${changed} files.`);
}

run().catch((error) => {
  console.error(error);
  process.exit(1);
});

import { promises as fs } from 'node:fs';
import path from 'node:path';

const ROOT = path.resolve(process.cwd(), 'src');

async function collectTsxFiles(dir) {
  const entries = await fs.readdir(dir, { withFileTypes: true });
  const files = await Promise.all(entries.map(async (entry) => {
    const fullPath = path.join(dir, entry.name);
    if (entry.isDirectory()) {
      return collectTsxFiles(fullPath);
    }

    if (entry.isFile() && fullPath.endsWith('.tsx')) {
      return [fullPath];
    }

    return [];
  }));

  return files.flat();
}

function upsertCnImport(content) {
  const hasCnImport = /import\s*\{[^}]*\bcn\b[^}]*\}\s*from\s*['"]@\/lib\/utils['"];?/m.test(content);
  if (hasCnImport) {
    return content;
  }

  const namedUtilsImportRegex = /import\s*\{\s*([^}]*)\s*\}\s*from\s*['"]@\/lib\/utils['"];?/m;
  const namedMatch = content.match(namedUtilsImportRegex);
  if (namedMatch) {
    const specifiers = namedMatch[1]
      .split(',')
      .map((item) => item.trim())
      .filter(Boolean);

    if (!specifiers.includes('cn')) {
      const replaced = `import { ${['cn', ...specifiers].join(', ')} } from '@/lib/utils';`;
      return content.replace(namedUtilsImportRegex, replaced);
    }

    return content;
  }

  const importRegex = /^import\s.+;$/gm;
  const imports = [...content.matchAll(importRegex)];
  if (imports.length === 0) {
    return `import { cn } from '@/lib/utils';\n${content}`;
  }

  const lastImport = imports[imports.length - 1];
  const insertPos = lastImport.index + lastImport[0].length;
  return `${content.slice(0, insertPos)}\nimport { cn } from '@/lib/utils';${content.slice(insertPos)}`;
}

function wrapClassNameLiterals(content) {
  return content.replace(/className="([^"]*)"/g, (_match, classLiteral) => {
    const escaped = classLiteral.replace(/\\/g, '\\\\').replace(/"/g, '\\"');
    return `className={cn("${escaped}")}`;
  });
}

async function run() {
  const files = await collectTsxFiles(ROOT);
  let changed = 0;

  for (const file of files) {
    const original = await fs.readFile(file, 'utf8');
    let next = wrapClassNameLiterals(original);

    if (!next.includes('className={cn(')) {
      continue;
    }

    next = upsertCnImport(next);

    if (next !== original) {
      await fs.writeFile(file, next, 'utf8');
      changed++;
    }
  }

  console.log(`Updated ${changed} TSX files.`);
}

run().catch((error) => {
  console.error(error);
  process.exit(1);
});

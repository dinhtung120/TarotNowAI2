export const layerOrder = {
 domain: 0,
 application: 1,
 infrastructure: 2,
 presentation: 3,
};

const PUBLIC_API_ACCESS_ALLOWLIST = new Set([
 'src/shared/http/apiUrl.ts',
 'src/shared/http/clientJsonRequest.ts',
]);

const forbiddenFeatureLayerFolders = new Set(['application', 'presentation', 'domain', 'infrastructure']);

export function normalizePath(filePath) {
 return filePath.replace(/\\/g, '/');
}

export function isTestFile(filePath) {
 return filePath.includes('.test.')
  || filePath.includes('.spec.')
  || filePath.includes('/__tests__/');
}

export function isClassifiedRuntimeTarget(filePath) {
 const normalized = normalizePath(filePath);
 return normalized === 'src/proxy.ts'
  || normalized.startsWith('src/app/')
  || normalized.startsWith('src/features/')
  || normalized.startsWith('src/i18n/')
  || normalized.startsWith('src/lib/')
  || normalized.startsWith('src/shared/');
}

export function findForbiddenFeatureLayerFolder(filePath) {
 const normalized = normalizePath(filePath);
 const parts = normalized.split('/');
 const featuresIndex = parts.indexOf('features');
 if (featuresIndex < 0 || parts[0] !== 'src') return null;

 for (let index = featuresIndex + 2; index < parts.length - 1; index += 1) {
  if (forbiddenFeatureLayerFolders.has(parts[index])) return parts[index];
 }

 return null;
}

const forbiddenSharedLayerFolders = new Set(['application', 'components', 'domain', 'infrastructure']);

export function findForbiddenSharedLayerFolder(filePath) {
 const normalized = normalizePath(filePath);
 const parts = normalized.split('/');
 if (parts[0] !== 'src' || parts[1] !== 'shared' || parts.length < 4) return null;

 return forbiddenSharedLayerFolders.has(parts[2]) ? parts[2] : null;
}

export function isSharedComponentsPath(filePath) {
 return findForbiddenSharedLayerFolder(filePath) === 'components';
}

export function isSharedImportingFeature(filePath, importPath) {
 return normalizePath(filePath).startsWith('src/shared/') && importPath.startsWith('@/features/');
}

export function isAllowedSharedFeatureImport(filePath) {
 const normalized = normalizePath(filePath);

 return normalized.startsWith('src/shared/server/prefetch/')
  || normalized === 'src/shared/server/auth/sessionHandshake.ts'
  || normalized === 'src/shared/providers/AuthProvider.tsx'
  || normalized === 'src/shared/hooks/useAuth.ts'
  || normalized.startsWith('src/shared/auth/')
  || normalized.startsWith('src/shared/navigation/')
  || normalized.startsWith('src/shared/query/')
  || normalized.startsWith('src/shared/actions/')
  || normalized.startsWith('src/shared/gateways/')
  || normalized.startsWith('src/shared/hooks/usePresenceConnection')
  || normalized.startsWith('src/shared/app-shell/navigation/');
}

export function isAllowedFeatureRouteDirectImport(filePath, importPath) {
 const normalized = normalizePath(filePath);
 if (!/^src\/app\/\[locale\]\/((\(auth\)\/[^/]+)|admin(?:\/[^/]+)?)\/page\.(ts|tsx|mts)$/.test(normalized)) {
  return false;
 }

 return importPath.startsWith('@/features/auth/')
  || importPath.startsWith('@/features/admin/');
}

export function resolveLayer(filePath) {
 const normalized = normalizePath(filePath);

 if (normalized === 'src/proxy.ts') return 'presentation';
 if (normalized.startsWith('src/app/')) return 'presentation';
 if (normalized.startsWith('src/i18n/')) return 'application';
 if (normalized === 'src/lib/utils' || normalized === 'src/lib/utils.ts') return 'application';
 if (normalized.startsWith('src/lib/')) return 'presentation';

 if (normalized.startsWith('src/features/')) return 'application';

 if (normalized.startsWith('src/shared/models/')) return 'domain';
 if (normalized.startsWith('src/shared/auth/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/dom/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/error/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/http/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/logging/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/navigation/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/query/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/realtime/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/storage/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/theme/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/actions/')) return 'application';
 if (normalized.startsWith('src/shared/gateways/')) return 'application';
 if (normalized.startsWith('src/shared/ui/')) return 'presentation';
 if (normalized.startsWith('src/shared/app-shell/')) return 'presentation';
 if (normalized.startsWith('src/shared/providers/')) return 'presentation';
 if (normalized.startsWith('src/shared/hooks/')) return 'application';
 if (normalized.startsWith('src/shared/server/')) return 'infrastructure';
 if (normalized.startsWith('src/shared/seo/')) return 'presentation';
 if (normalized.startsWith('src/shared/config/')) return 'application';
 if (normalized.startsWith('src/shared/lib/')) return 'application';
 if (normalized.startsWith('src/shared/media-upload/')) return 'application';
 if (normalized.startsWith('src/shared/utils/')) return 'application';

 return null;
}

export function findLine(sourceCode, snippet) {
 const index = sourceCode.indexOf(snippet);
 if (index < 0) {
  return 1;
 }

 return sourceCode.slice(0, index).split('\n').length;
}

export function findClientBoundaryViolation(relativePath, sourceCode) {
 if (PUBLIC_API_ACCESS_ALLOWLIST.has(relativePath)) {
  return null;
 }

 if (sourceCode.includes('getPublicApiBaseUrl(')) {
  return {
   file: relativePath,
   line: findLine(sourceCode, 'getPublicApiBaseUrl'),
   message: 'imports getPublicApiBaseUrl directly instead of using a local BFF/shared client gateway.',
  };
 }

 if (sourceCode.includes('NEXT_PUBLIC_API_URL')) {
  return {
   file: relativePath,
   line: findLine(sourceCode, 'NEXT_PUBLIC_API_URL'),
   message: 'reads NEXT_PUBLIC_API_URL directly outside shared/http/apiUrl.ts.',
  };
 }

 const absoluteFetchMatch = sourceCode.match(/\b(?:fetch|EventSource)\s*\(\s*['"`]https?:\/\//);
 if (absoluteFetchMatch) {
  return {
   file: relativePath,
   line: findLine(sourceCode, absoluteFetchMatch[0]),
   message: 'uses an absolute-origin fetch/EventSource instead of going through the local BFF boundary.',
  };
 }

 return null;
}

export function findSensitiveStreamViolation(relativePath, sourceCode) {
 if (!sourceCode.includes('new EventSource(')) {
  return null;
 }

 const sensitiveParamPatterns = [
  'followupQuestion=',
  "set('followupQuestion'",
  'set("followupQuestion"',
 ];

 const matchedPattern = sensitiveParamPatterns.find((pattern) => sourceCode.includes(pattern));
 if (!matchedPattern) {
  return null;
 }

 return {
  file: relativePath,
  line: findLine(sourceCode, matchedPattern),
  message: 'builds an EventSource URL that exposes follow-up prompt data in the query string.',
 };
}

export function isAllowedLayerException(filePath, sourceLayer, targetLayer) {
 if (sourceLayer === 'application'
  && targetLayer === 'infrastructure'
  && filePath.includes('/application/gateways/')) {
  return true;
 }

 if (sourceLayer === 'application'
  && targetLayer === 'infrastructure'
  && filePath.startsWith('src/shared/media-upload/')) {
  return true;
 }

 return false;
}

export function findUnclassifiedRuntimeFiles(sourceFiles) {
 return sourceFiles.filter((filePath) => isClassifiedRuntimeTarget(filePath) && resolveLayer(filePath) === null);
}

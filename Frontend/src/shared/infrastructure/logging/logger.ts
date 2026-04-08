type LogMeta = Record<string, unknown> | undefined;
type LogLevel = 'error' | 'warn' | 'info' | 'debug';
const MAX_LOG_DEPTH = 4;

function shouldLog(level: LogLevel): boolean {
 if (process.env.NODE_ENV === 'test') return false;
 if (level === 'debug' && process.env.NODE_ENV === 'production') return false;
 return true;
}

function safeMeta(meta?: LogMeta): Record<string, unknown> | undefined {
 if (!meta) return undefined;
 const normalized = normalizeUnknown(meta);
 return isPlainRecord(normalized) ? normalized : { value: normalized };
}

function isPlainRecord(value: unknown): value is Record<string, unknown> {
 return typeof value === 'object' && value !== null && Array.isArray(value) === false;
}

function normalizeUnknown(value: unknown, depth = 0, seen?: WeakSet<object>): unknown {
 if (depth >= MAX_LOG_DEPTH) return '[MaxDepth]';

 if (value instanceof Error) {
  const candidate = value as Error & { code?: unknown; status?: unknown; cause?: unknown };
  const normalized: Record<string, unknown> = {
   name: value.name,
   message: value.message,
  };

  if (value.stack) normalized.stack = value.stack;
  if (candidate.code !== undefined) normalized.code = candidate.code;
  if (candidate.status !== undefined) normalized.status = candidate.status;
  if (candidate.cause !== undefined) {
   normalized.cause = normalizeUnknown(candidate.cause, depth + 1, seen);
  }

  return normalized;
 }

 if (Array.isArray(value)) {
  return value.map(item => normalizeUnknown(item, depth + 1, seen));
 }

 if (typeof value === 'object' && value !== null) {
  const nextSeen = seen ?? new WeakSet<object>();
  if (nextSeen.has(value)) return '[Circular]';
  nextSeen.add(value);

  const normalized: Record<string, unknown> = {};
  for (const [key, val] of Object.entries(value)) {
   normalized[key] = normalizeUnknown(val, depth + 1, nextSeen);
  }

  return normalized;
 }

 return value;
}

function writeLog(level: LogLevel, scope: string, payload: unknown, meta?: LogMeta): void {
 if (!shouldLog(level)) return;

 const normalizedPayload = normalizeUnknown(payload);
 const metaPayload = safeMeta(meta);
 const scopedPrefix = `[${scope}]`;

 if (level === 'error') {
  if (metaPayload) {
   console.error(scopedPrefix, normalizedPayload, metaPayload);
   return;
  }
  console.error(scopedPrefix, normalizedPayload);
  return;
 }

 if (level === 'warn') {
  if (metaPayload) {
   console.warn(scopedPrefix, normalizedPayload, metaPayload);
   return;
  }
  console.warn(scopedPrefix, normalizedPayload);
  return;
 }

 if (level === 'info') {
  if (metaPayload) {
   console.info(scopedPrefix, normalizedPayload, metaPayload);
   return;
  }
  console.info(scopedPrefix, normalizedPayload);
  return;
 }

 if (metaPayload) {
  console.debug(scopedPrefix, normalizedPayload, metaPayload);
  return;
 }
 console.debug(scopedPrefix, normalizedPayload);
}

export const logger = {
 error(scope: string, error: unknown, meta?: LogMeta): void {
  writeLog('error', scope, error, meta);
 },
 warn(scope: string, payload: unknown, meta?: LogMeta): void {
  writeLog('warn', scope, payload, meta);
 },
 info(scope: string, payload: unknown, meta?: LogMeta): void {
  writeLog('info', scope, payload, meta);
 },
 debug(scope: string, payload: unknown, meta?: LogMeta): void {
  writeLog('debug', scope, payload, meta);
 },
};

type LogMeta = Record<string, unknown> | undefined;
type LogLevel = 'error' | 'warn' | 'info' | 'debug';

function shouldLog(level: LogLevel): boolean {
 if (process.env.NODE_ENV === 'test') return false;
 if (level === 'debug' && process.env.NODE_ENV === 'production') return false;
 return true;
}

function safeMeta(meta?: LogMeta): Record<string, unknown> | undefined {
 if (!meta) return undefined;
 return meta;
}

function writeLog(level: LogLevel, scope: string, payload: unknown, meta?: LogMeta): void {
 if (!shouldLog(level)) return;

 const metaPayload = safeMeta(meta);
 const scopedPrefix = `[${scope}]`;

 if (level === 'error') {
  if (metaPayload) {
   console.error(scopedPrefix, payload, metaPayload);
   return;
  }
  console.error(scopedPrefix, payload);
  return;
 }

 if (level === 'warn') {
  if (metaPayload) {
   console.warn(scopedPrefix, payload, metaPayload);
   return;
  }
  console.warn(scopedPrefix, payload);
  return;
 }

 if (level === 'info') {
  if (metaPayload) {
   console.info(scopedPrefix, payload, metaPayload);
   return;
  }
  console.info(scopedPrefix, payload);
  return;
 }

 if (metaPayload) {
  console.debug(scopedPrefix, payload, metaPayload);
  return;
 }
 console.debug(scopedPrefix, payload);
}

export const logger = {
 error(scope: string, error: unknown, meta?: LogMeta): void {
  writeLog('error', scope, error, meta);
 },
 warn(scope: string, message: string, meta?: LogMeta): void {
  writeLog('warn', scope, message, meta);
 },
 info(scope: string, message: string, meta?: LogMeta): void {
  writeLog('info', scope, message, meta);
 },
 debug(scope: string, message: string, meta?: LogMeta): void {
  writeLog('debug', scope, message, meta);
 },
};

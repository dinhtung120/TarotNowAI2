type LogMeta = Record<string, unknown> | undefined;

function shouldLog(): boolean {
  return process.env.NODE_ENV !== 'test';
}

function safeMeta(meta?: LogMeta): Record<string, unknown> | undefined {
  if (!meta) return undefined;
  return meta;
}

export const logger = {
  error(scope: string, error: unknown, meta?: LogMeta): void {
    if (!shouldLog()) return;
    const payload = safeMeta(meta);
    if (payload) {
      console.error(`[${scope}]`, error, payload);
      return;
    }
    console.error(`[${scope}]`, error);
  },
};

export interface RetryOptions {
  baseDelayMs: number;
  maxAttempts: number;
  shouldRetry?: (error: unknown) => boolean;
}

export async function runWithRetry<T>(
  task: () => Promise<T>,
  options: RetryOptions,
): Promise<T> {
  const attempts = Math.max(1, options.maxAttempts);
  let lastError: unknown;

  for (let index = 0; index < attempts; index += 1) {
    try {
      return await task();
    } catch (error) {
      lastError = error;
      const canRetry = index < attempts - 1 && (options.shouldRetry?.(error) ?? true);
      if (!canRetry) {
        break;
      }

      await sleep(options.baseDelayMs * (index + 1));
    }
  }

  throw lastError instanceof Error ? lastError : new Error('Upload failed.');
}

function sleep(delayMs: number): Promise<void> {
  return new Promise((resolve) => setTimeout(resolve, delayMs));
}

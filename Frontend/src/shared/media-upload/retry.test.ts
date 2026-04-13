import { describe, expect, it, vi } from 'vitest';
import { runWithRetry } from '@/shared/media-upload/retry';

describe('runWithRetry', () => {
  it('retries until success', async () => {
    let attempts = 0;

    const result = await runWithRetry(
      async () => {
        attempts += 1;
        if (attempts < 3) {
          throw new Error('temporary');
        }

        return 'ok';
      },
      {
        maxAttempts: 3,
        baseDelayMs: 1,
      },
    );

    expect(result).toBe('ok');
    expect(attempts).toBe(3);
  });

  it('stops retry when shouldRetry returns false', async () => {
    const task = vi.fn(async () => {
      throw new Error('fatal');
    });

    await expect(runWithRetry(task, {
      maxAttempts: 5,
      baseDelayMs: 1,
      shouldRetry: () => false,
    })).rejects.toThrow('fatal');

    expect(task).toHaveBeenCalledTimes(1);
  });
});

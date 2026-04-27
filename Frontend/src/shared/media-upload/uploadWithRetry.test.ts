import { beforeEach, describe, expect, it, vi } from 'vitest';
import { runWithRetry } from '@/shared/media-upload/retry';
import { uploadToPresignedUrlViaXhr } from '@/shared/media-upload/uploadViaXhr';
import { uploadToR2WithRetry } from '@/shared/media-upload/uploadWithRetry';

vi.mock('@/shared/media-upload/retry', () => ({
 runWithRetry: vi.fn(),
}));

vi.mock('@/shared/media-upload/uploadViaXhr', () => ({
 uploadToPresignedUrlViaXhr: vi.fn(),
}));

const mockedRunWithRetry = vi.mocked(runWithRetry);
const mockedUploadToPresignedUrlViaXhr = vi.mocked(uploadToPresignedUrlViaXhr);

describe('uploadWithRetry', () => {
 beforeEach(() => {
  vi.clearAllMocks();
  mockedRunWithRetry.mockImplementation(async (fn) => {
   await fn();
  });
 });

 it('delegates upload through retry runner with configured callback', async () => {
  const file = new Blob(['hello'], { type: 'text/plain' });

  await uploadToR2WithRetry({
   uploadUrl: 'https://r2.example/file',
   contentType: 'text/plain',
   file,
   maxAttempts: 4,
  });

  expect(mockedRunWithRetry).toHaveBeenCalledTimes(1);
  expect(mockedUploadToPresignedUrlViaXhr).toHaveBeenCalledWith(
   expect.objectContaining({ uploadUrl: 'https://r2.example/file', contentType: 'text/plain', file }),
  );
  const retryOptions = mockedRunWithRetry.mock.calls[0]?.[1];
  expect(retryOptions?.maxAttempts).toBe(4);
 });

 it('does not retry for abort/cors errors in shouldRetry callback', async () => {
  const file = new Blob(['hello'], { type: 'text/plain' });

  await uploadToR2WithRetry({
   uploadUrl: 'https://r2.example/file',
   contentType: 'text/plain',
   file,
  });

  const retryOptions = mockedRunWithRetry.mock.calls[0]?.[1];
  expect(retryOptions?.shouldRetry(new Error('Upload media đã bị hủy.'))).toBe(false);
  expect(retryOptions?.shouldRetry(new Error('Upload bị chặn bởi CORS của R2.'))).toBe(false);
  expect(retryOptions?.shouldRetry(new Error('other'))).toBe(true);
 });

 it('retries by default for non-Error throwables', async () => {
  const file = new Blob(['hello'], { type: 'text/plain' });

  await uploadToR2WithRetry({
   uploadUrl: 'https://r2.example/file',
   contentType: 'text/plain',
   file,
  });

  const retryOptions = mockedRunWithRetry.mock.calls[0]?.[1];
  expect(retryOptions?.shouldRetry('string failure')).toBe(true);
 });
});

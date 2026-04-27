import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { uploadToPresignedUrlViaXhr } from '@/shared/media-upload/uploadViaXhr';

type UploadProgressHandler = ((event: { lengthComputable: boolean; loaded: number; total: number }) => void) | null;

type XhrScenario = 'success' | 'http-error' | 'cors-error' | 'timeout' | 'wait';

class MockXMLHttpRequest {
 static scenario: XhrScenario = 'success';
 status = 200;
 timeout = 0;
 upload: { onprogress: UploadProgressHandler } = { onprogress: null };
 onload: (() => void) | null = null;
 onerror: (() => void) | null = null;
 ontimeout: (() => void) | null = null;
 onabort: (() => void) | null = null;
 open = vi.fn();
 setRequestHeader = vi.fn();

 abort = vi.fn(() => {
  this.onabort?.();
 });

 send = vi.fn(() => {
  if (this.upload.onprogress) {
   this.upload.onprogress({ lengthComputable: true, loaded: 50, total: 100 });
  }

  queueMicrotask(() => {
   if (MockXMLHttpRequest.scenario === 'success') {
    this.status = 204;
    this.onload?.();
    return;
   }
   if (MockXMLHttpRequest.scenario === 'http-error') {
    this.status = 500;
    this.onload?.();
    return;
   }
   if (MockXMLHttpRequest.scenario === 'cors-error') {
    this.status = 0;
    this.onerror?.();
    return;
   }
   if (MockXMLHttpRequest.scenario === 'timeout') {
    this.ontimeout?.();
   }
  });
 });
}

describe('uploadViaXhr', () => {
 const originalXmlHttpRequest = globalThis.XMLHttpRequest;

 beforeEach(() => {
  MockXMLHttpRequest.scenario = 'success';
  globalThis.XMLHttpRequest = MockXMLHttpRequest as unknown as typeof XMLHttpRequest;
 });

 afterEach(() => {
  globalThis.XMLHttpRequest = originalXmlHttpRequest;
  vi.clearAllMocks();
 });

 it('resolves and reports upload progress for successful upload', async () => {
  const onProgress = vi.fn();

  await expect(
   uploadToPresignedUrlViaXhr({
    uploadUrl: 'https://files.example/upload',
    contentType: 'image/webp',
    file: new Blob(['data']),
    onProgress,
   }),
  ).resolves.toBeUndefined();

  expect(onProgress).toHaveBeenCalledWith(50);
 });

 it('rejects with server status message when upload responds with non-2xx', async () => {
  MockXMLHttpRequest.scenario = 'http-error';

  await expect(
   uploadToPresignedUrlViaXhr({
    uploadUrl: 'https://files.example/upload',
    contentType: 'image/webp',
    file: new Blob(['data']),
   }),
  ).rejects.toThrow('R2 upload failed (500).');
 });

 it('returns CORS-specific message for R2 status 0 network error', async () => {
  MockXMLHttpRequest.scenario = 'cors-error';

  await expect(
   uploadToPresignedUrlViaXhr({
    uploadUrl: 'https://bucket.r2.cloudflarestorage.com/upload',
    contentType: 'image/webp',
    file: new Blob(['data']),
   }),
  ).rejects.toThrow('CORS của R2');
 });

 it('returns generic network upload message for non-R2 status 0 error', async () => {
  MockXMLHttpRequest.scenario = 'cors-error';

  await expect(
   uploadToPresignedUrlViaXhr({
    uploadUrl: 'https://files.example/upload',
    contentType: 'image/webp',
    file: new Blob(['data']),
   }),
  ).rejects.toThrow('Không thể upload media lên R2.');
 });

 it('rejects with abort message when external signal aborts request', async () => {
  MockXMLHttpRequest.scenario = 'wait';
  const controller = new AbortController();

  const pending = uploadToPresignedUrlViaXhr({
   uploadUrl: 'https://files.example/upload',
   contentType: 'image/webp',
   file: new Blob(['data']),
   signal: controller.signal,
  });
  controller.abort();

  await expect(pending).rejects.toThrow('Upload media đã bị hủy.');
 });

 it('rejects with timeout message when xhr times out', async () => {
  MockXMLHttpRequest.scenario = 'timeout';

  await expect(
   uploadToPresignedUrlViaXhr({
    uploadUrl: 'https://files.example/upload',
    contentType: 'image/webp',
    file: new Blob(['data']),
   }),
  ).rejects.toThrow('Upload media bị timeout.');
 });
});

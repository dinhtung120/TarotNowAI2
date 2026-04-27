import { describe, expect, it } from 'vitest';
import * as mediaUpload from '@/shared/media-upload';

describe('media-upload index exports', () => {
 it('re-exports core upload APIs', () => {
  expect(typeof mediaUpload.uploadToR2WithRetry).toBe('function');
  expect(typeof mediaUpload.uploadToPresignedUrlViaXhr).toBe('function');
  expect(typeof mediaUpload.presignAvatarUpload).toBe('function');
  expect(typeof mediaUpload.confirmAvatarUpload).toBe('function');
 });
});

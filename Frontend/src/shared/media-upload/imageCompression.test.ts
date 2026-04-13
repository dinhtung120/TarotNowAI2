import { describe, expect, it } from 'vitest';
import {
  ImageUploadValidationError,
  validateImageForDirectUpload,
} from '@/shared/media-upload/imageCompression';

describe('validateImageForDirectUpload', () => {
  it('accepts image file within 10MB', () => {
    const file = new File([new Uint8Array(1024)], 'avatar.png', { type: 'image/png' });

    expect(() => validateImageForDirectUpload(file)).not.toThrow();
  });

  it('rejects non-image file', () => {
    const file = new File([new Uint8Array(1024)], 'sample.txt', { type: 'text/plain' });

    expect(() => validateImageForDirectUpload(file)).toThrow(ImageUploadValidationError);
  });

  it('rejects image above 10MB', () => {
    const file = new File([new Uint8Array(10 * 1024 * 1024 + 1)], 'large.jpg', { type: 'image/jpeg' });

    expect(() => validateImageForDirectUpload(file)).toThrow(ImageUploadValidationError);
  });
});

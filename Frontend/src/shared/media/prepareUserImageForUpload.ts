import { compressImageToWebpInWorker } from '@/shared/media/compressImageToWebpInWorker';

export type UserImagePreset = 'avatar' | 'community';

const PRESETS: Record<UserImagePreset, { maxEdge: number; quality: number }> = {
  /** FE giảm kích thước trước khi BE nén AVIF (cạnh dài tối đa px). */
  avatar: { maxEdge: 1024, quality: 0.85 },
  community: { maxEdge: 1600, quality: 0.85 },
};

/**
 * Luồng chung: validate MIME/size (trong worker entry) → nén WebP (Worker hoặc fallback).
 */
export async function prepareUserImageForUpload(file: File, preset: UserImagePreset): Promise<File> {
  const p = PRESETS[preset];
  return compressImageToWebpInWorker(file, p);
}

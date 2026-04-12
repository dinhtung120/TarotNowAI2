import { prepareUserImageForUpload } from '@/shared/media/prepareUserImageForUpload';

/** Nén avatar ở FE sang WebP (Web Worker), trước khi BE nén AVIF và upload R2/local. */
export async function compressAvatarImage(file: File): Promise<File> {
  return prepareUserImageForUpload(file, 'avatar');
}

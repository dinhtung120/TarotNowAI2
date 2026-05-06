import type { ReaderProfile } from '@/features/reader/shared';

export function getLocalizedReaderBio(
 reader: ReaderProfile,
 locale: string,
 fallback: string,
): string {
 if (locale === 'vi') {
  return reader.bioVi || fallback;
 }

 if (locale === 'en') {
  return reader.bioEn || reader.bioVi || fallback;
 }

 if (locale === 'zh') {
  return reader.bioZh || reader.bioVi || fallback;
 }

 return reader.bioVi || fallback;
}

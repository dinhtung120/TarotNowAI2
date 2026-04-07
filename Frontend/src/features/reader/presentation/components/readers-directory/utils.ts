import type { ReaderProfile } from '@/features/reader/application/actions';

export function getLocalizedReaderBio(
 reader: ReaderProfile,
 locale: string,
 fallback: string,
): string {
 if (locale === 'vi') {
  return reader.bioVi || reader.bioEn || reader.bioZh || fallback;
 }

 if (locale === 'en') {
  return reader.bioEn || reader.bioVi || reader.bioZh || fallback;
 }

 return reader.bioZh || reader.bioEn || reader.bioVi || fallback;
}

import type { Metadata } from 'next';
import { getTranslations } from 'next-intl/server';

interface LocaleMetadataParams {
 params: Promise<{ locale: string }>;
}

export async function generateLocaleMetadata({ params }: LocaleMetadataParams): Promise<Metadata> {
 const { locale } = await params;
 const t = await getTranslations({ locale, namespace: 'Common' });

 return {
  title: {
   default: t('app_title'),
   template: `%s | ${t('app_title')}`,
  },
  description: t('app_description'),
 };
}

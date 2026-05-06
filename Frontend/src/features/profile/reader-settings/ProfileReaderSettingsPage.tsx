'use client';

import { useTranslations } from 'next-intl';
import { useProfileReaderSettingsPage } from '@/features/profile/reader-settings/useProfileReaderSettingsPage';
import { ReaderSettingsFormCard } from '@/features/profile/reader-settings/components/ReaderSettingsFormCard';
import { ReaderSettingsHeader } from '@/features/profile/reader-settings/components/ReaderSettingsHeader';
import { ReaderSettingsLoadingState } from '@/features/profile/reader-settings/components/ReaderSettingsLoadingState';
import { cn } from '@/lib/utils';

interface ReaderSettingsPageProps {
 embedded?: boolean;
}

export default function ReaderSettingsPage({ embedded = false }: ReaderSettingsPageProps) {
 const t = useTranslations('Profile');
  const {
   loading,
   saving,
   bioVi,
   diamondPerQuestion,
   specialties,
   yearsOfExperience,
   minYearsOfExperience,
   facebookUrl,
   instagramUrl,
   tikTokUrl,
   minDiamondPerQuestion,
   readerPolicyReady,
  handleSave,
 } = useProfileReaderSettingsPage(t);

 if (loading) {
  return <ReaderSettingsLoadingState label={t('reader.loading')} />;
 }

 const content = (
  <div className={cn('space-y-8')}>
    <ReaderSettingsFormCard
    title={t('reader.public_profile_title')}
    bioLabel={t('reader.bio_label')}
    bioPlaceholder={t('reader.bio_placeholder')}
    bioValue={bioVi}
    specialtiesLabel={t('reader.specialties_label')}
    specialtiesValue={specialties}
    renderSpecialtyLabel={(value) => t(`reader.specialties.${value}`)}
    yearsLabel={t('reader.years_experience_label')}
    yearsValue={yearsOfExperience}
    minYearsValue={minYearsOfExperience}
    socialLinksLabel={t('reader.social_links_label')}
    socialLinksHint={t('reader.social_links_hint')}
    facebookPlaceholder={t('reader.facebook_placeholder')}
    instagramPlaceholder={t('reader.instagram_placeholder')}
    tikTokPlaceholder={t('reader.tiktok_placeholder')}
    facebookUrl={facebookUrl}
    instagramUrl={instagramUrl}
    tikTokUrl={tikTokUrl}
    priceLabel={t('reader.price_label')}
    priceHelp={t('reader.price_help')}
    priceValue={diamondPerQuestion}
    minPriceValue={minDiamondPerQuestion}
    validation={{
      bioMax: t('reader.validation.bio_max'),
      specialtiesMin: t('reader.validation.specialties_min'),
      yearsMin: t('reader.validation.years_min', { min: minYearsOfExperience }),
      priceMin: t('reader.validation.diamond_min', { min: minDiamondPerQuestion }),
      socialRequired: t('reader.validation.social_required'),
      socialTooLong: t('reader.validation.social_too_long'),
      facebookInvalid: t('reader.validation.facebook_invalid'),
      instagramInvalid: t('reader.validation.instagram_invalid'),
      tikTokInvalid: t('reader.validation.tiktok_invalid'),
    }}
    onSubmit={handleSave}
    saving={saving || !readerPolicyReady}
    savingLabel={t('reader.saving')}
    saveLabel={t('reader.save')}
   />
  </div>
 );

 if (embedded) {
  return content;
 }

 return (
  <div className={cn('max-w-3xl mx-auto tn-page-x pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000')}>
   <ReaderSettingsHeader tag={t('reader.tag')} title={t('reader.title')} subtitle={t('reader.subtitle')} />
   {content}
  </div>
 );
}

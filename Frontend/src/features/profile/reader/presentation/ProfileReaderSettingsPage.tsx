'use client';

import { useTranslations } from 'next-intl';
import { useProfileReaderSettingsPage } from '@/features/profile/reader/application/useProfileReaderSettingsPage';
import { ReaderSettingsFormCard } from '@/features/profile/reader/presentation/components/ReaderSettingsFormCard';
import { ReaderSettingsHeader } from '@/features/profile/reader/presentation/components/ReaderSettingsHeader';
import { ReaderSettingsLoadingState } from '@/features/profile/reader/presentation/components/ReaderSettingsLoadingState';
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
  setBioVi,
  diamondPerQuestion,
  setDiamondPerQuestion,
  specialtiesStr,
  setSpecialtiesStr,
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
    onChangeBio={setBioVi}
    specialtiesLabel={t('reader.specialties_label')}
    specialtiesPlaceholder={t('reader.specialties_placeholder')}
    specialtiesValue={specialtiesStr}
    onChangeSpecialties={setSpecialtiesStr}
    priceLabel={t('reader.price_label')}
    priceHelp={t('reader.price_help')}
    priceValue={diamondPerQuestion}
    onChangePrice={setDiamondPerQuestion}
    onSubmit={handleSave}
    saving={saving}
    savingLabel={t('reader.saving')}
    saveLabel={t('reader.save')}
   />
  </div>
 );

 if (embedded) {
  return content;
 }

 return (
  <div className={cn('max-w-3xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000')}>
   <ReaderSettingsHeader tag={t('reader.tag')} title={t('reader.title')} subtitle={t('reader.subtitle')} />
   {content}
  </div>
 );
}

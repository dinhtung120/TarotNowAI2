'use client';
import { useLocale, useTranslations } from 'next-intl';
import { useReaderPublicProfilePage } from '@/features/reader/application/useReaderPublicProfilePage';
import { cn } from '@/lib/utils';
import { ReaderProfileBackButton } from '@/features/reader/presentation/components/reader-profile/ReaderProfileBackButton';
import { ReaderProfileCard } from '@/features/reader/presentation/components/reader-profile/ReaderProfileCard';
import { ReaderProfileFooter } from '@/features/reader/presentation/components/reader-profile/ReaderProfileFooter';
import { ReaderProfileLoadingState } from '@/features/reader/presentation/components/ReaderProfileLoadingState';
import { ReaderProfileNotFoundState } from '@/features/reader/presentation/components/ReaderProfileNotFoundState';

export default function ReaderProfilePage() {
 const t = useTranslations('Readers');
 const locale = useLocale();
 const { router, profile, loading, startChat, startingChat } = useReaderPublicProfilePage(t);

 if (loading) {
  return <ReaderProfileLoadingState label={t('profile.loading')} />;
 }

 if (!profile) {
  return <ReaderProfileNotFoundState description={t('profile.not_found')} backLabel={t('profile.back_to_list')} onBack={() => router.push('/readers')} />;
 }

 const bio =
  (
   locale === 'vi'
    ? (profile.bioVi || profile.bioEn || profile.bioZh)
    : locale === 'en'
     ? (profile.bioEn || profile.bioVi || profile.bioZh)
     : (profile.bioZh || profile.bioEn || profile.bioVi)
  ) || t('directory.bio_fallback');

 return (
  <div className={cn('max-w-3xl mx-auto tn-page-x pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000')}>
   <ReaderProfileBackButton label={t('profile.back_to_list')} onBack={() => router.push('/readers')} />
   <ReaderProfileCard
    bio={bio}
    profile={profile}
    fallbackName={t('directory.reader_fallback')}
    diamondSuffix={t('profile.diamond_per_question_suffix')}
    ratingLabel={t('profile.rating_label')}
    reviewsLabel={t('profile.reviews_label')}
    soulLinkLabel={t('profile.soul_link')}
    specialtiesTitle={t('profile.specialties_title')}
    t={t}
   />
   <ReaderProfileFooter
    ctaLabel={t('profile.cta_send_question')}
    memberSinceLabel={t('profile.member_since', { date: new Date(profile.createdAt).toLocaleDateString(locale) })}
    onStartChat={startChat}
    startingChat={startingChat}
   />
  </div>
 );
}

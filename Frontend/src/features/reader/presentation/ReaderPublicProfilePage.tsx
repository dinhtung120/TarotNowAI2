'use client';
import { useLocale, useTranslations } from 'next-intl';
import { useReaderPublicProfilePage } from '@/features/reader/application/useReaderPublicProfilePage';
import { cn } from '@/lib/utils';
import { ReaderProfileBackButton } from '@/features/reader/presentation/components/reader-profile/ReaderProfileBackButton';
import { ReaderProfileCard } from '@/features/reader/presentation/components/reader-profile/ReaderProfileCard';
import { ReaderProfileFooter } from '@/features/reader/presentation/components/reader-profile/ReaderProfileFooter';
import { ReaderProfileLoadingState } from '@/features/reader/presentation/components/ReaderProfileLoadingState';
import { ReaderProfileNotFoundState } from '@/features/reader/presentation/components/ReaderProfileNotFoundState';
import { getLocalizedReaderBio } from '@/features/reader/presentation/components/readers-directory';

export default function ReaderProfilePage() {
 const t = useTranslations('Readers');
 const locale = useLocale();
 const { navigation, profile, loading, startChat, startingChat } = useReaderPublicProfilePage(t);

 if (loading) {
  return <ReaderProfileLoadingState label={t('profile.loading')} />;
 }

 if (!profile) {
  return <ReaderProfileNotFoundState description={t('profile.not_found')} backLabel={t('profile.back_to_list')} onBack={() => navigation.push('/readers')} />;
 }

 const bio = getLocalizedReaderBio(profile, locale, t('directory.bio_fallback'));

 return (
  <div className={cn('max-w-3xl mx-auto tn-page-x pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000')}>
   <ReaderProfileBackButton label={t('profile.back_to_list')} onBack={() => navigation.push('/readers')} />
   <ReaderProfileCard
    bio={bio}
    profile={profile}
    fallbackName={t('directory.reader_fallback')}
    diamondSuffix={t('profile.diamond_per_question_suffix')}
    yearsExperienceLabel={t('profile.years_experience_suffix')}
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

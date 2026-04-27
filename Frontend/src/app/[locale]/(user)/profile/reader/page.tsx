import { redirect } from 'next/navigation';
import ProfileReaderSettingsPage from '@/features/profile/reader/presentation/ProfileReaderSettingsPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchProfileReaderSettingsPage } from '@/shared/server/prefetch/runners';
import { requireSessionWithHandshake } from '@/shared/server/auth/sessionHandshake';

export default async function ProfileReaderRoutePage({
 params,
}: {
 params: Promise<{ locale: string }>;
}) {
 const { locale } = await params;
 const sessionSnapshot = await requireSessionWithHandshake({
  locale,
  nextPath: `/${locale}/profile/reader`,
 });

 if (sessionSnapshot.user.role !== 'tarot_reader') {
  redirect(`/${locale}/profile`);
 }

 const state = await dehydrateAppQueries(prefetchProfileReaderSettingsPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ProfileReaderSettingsPage />
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';

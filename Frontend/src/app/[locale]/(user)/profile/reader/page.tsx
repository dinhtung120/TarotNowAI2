import ProfileReaderSettingsPage from '@/features/profile/reader/presentation/ProfileReaderSettingsPage';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/shared/server/prefetch/appQueryDehydrate';
import { prefetchProfileReaderSettingsPage } from '@/shared/server/prefetch/runners';

export default async function ProfileReaderRoutePage() {
 const state = await dehydrateAppQueries(prefetchProfileReaderSettingsPage);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ProfileReaderSettingsPage />
  </AppQueryHydrationBoundary>
 );
}

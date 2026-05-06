import { ChatSegmentShell } from '@/features/chat/public';
import { AppQueryHydrationBoundary, dehydrateAppQueries } from '@/app/_shared/server/prefetch/appQueryDehydrate';
import { prefetchChatInboxShell } from '@/app/_shared/server/prefetch/runners';

export default async function ChatSegmentLayout({ children }: { children: React.ReactNode }) {
 const state = await dehydrateAppQueries(prefetchChatInboxShell);

 return (
  <AppQueryHydrationBoundary state={state}>
   <ChatSegmentShell>{children}</ChatSegmentShell>
  </AppQueryHydrationBoundary>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/app/_shared/seo/defaultMetadata';

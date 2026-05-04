import dynamic from 'next/dynamic';
import { Suspense } from 'react';
import LoadingSpinner from '@/shared/components/ui/LoadingSpinner';

const ChatRoomPage = dynamic(
 () => import('@/features/chat/public').then((m) => m.ChatRoomPage),
);

export default function ChatRoomRoutePage() {
 return (
  <Suspense fallback={<LoadingSpinner fullPage message="Loading chat room" />}>
   <ChatRoomPage />
  </Suspense>
 );
}

export { generateLocaleMetadata as generateMetadata } from '@/shared/seo/defaultMetadata';

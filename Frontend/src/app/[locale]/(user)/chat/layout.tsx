'use client';

import { usePathname } from 'next/navigation';
import ConversationSidebar from '@/features/chat/presentation/components/ConversationSidebar';
import { CallProvider, IncomingCallOverlay, ActiveCallOverlay } from '@/features/chat/presentation/call';
import { cn } from '@/lib/utils';

export default function ChatLayout({ children }: { children: React.ReactNode }) {
  const pathname = usePathname();
  
  const isChatRoom = pathname.split('/').filter(Boolean).length > 2;

  return (
    <CallProvider>
      <div className={cn("flex h-full w-full overflow-hidden bg-[#050505]/40 backdrop-blur-sm")}>
        {}
        <aside className={`
          ${isChatRoom ? 'hidden md:flex' : 'flex'} 
          w-full md:w-[350px] lg:w-[400px] border-r border-white/5 flex-col shrink-0
        `}>
          <ConversationSidebar />
        </aside>

        {}
        <main className={`
          ${isChatRoom ? 'flex' : 'hidden md:flex'} 
          flex-1 min-w-0 flex-col relative
        `}>
          {children}
        </main>
      </div>

      <IncomingCallOverlay />
      <ActiveCallOverlay />
    </CallProvider>
  );
}

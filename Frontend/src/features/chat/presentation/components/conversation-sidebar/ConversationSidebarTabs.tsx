import { cn } from '@/lib/utils';
import type { InboxTab } from '@/features/chat/application/actions';

interface ConversationSidebarTabsProps {
 tab: InboxTab;
 setTab: (tab: InboxTab) => void;
}

function tabClass(active: boolean) {
 return cn(
  'whitespace-nowrap px-3 py-1.5 rounded-full text-xs border transition-all',
  active
   ? 'bg-[var(--purple-accent)]/20 border-[var(--purple-accent)]/40 text-white'
   : 'bg-white/5 border-white/10 text-[var(--text-secondary)] hover:bg-white/10',
 );
}

export function ConversationSidebarTabs({ tab, setTab }: ConversationSidebarTabsProps) {
 return (
  <div className={cn('flex items-center gap-2 overflow-x-auto pb-1 scrollbar-hide')}>
   <button onClick={() => setTab('active')} className={tabClass(tab === 'active')}>Đang chat</button>
   <button onClick={() => setTab('pending')} className={tabClass(tab === 'pending')}>Pending</button>
   <button onClick={() => setTab('completed')} className={tabClass(tab === 'completed')}>Đã hoàn thành</button>
  </div>
 );
}

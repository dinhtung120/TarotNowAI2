import { cn } from '@/lib/utils';
import type { InboxTab } from '@/features/chat/shared/actions';

interface ConversationSidebarTabsProps {
 tab: InboxTab;
 setTab: (tab: InboxTab) => void;
}

function tabClass(active: boolean) {
 return cn(
  'whitespace-nowrap rounded-full border px-3 py-1.5 text-xs transition-all',
  active
   ? 'border-violet-500/40 bg-violet-500/20 text-white'
   : 'border-white/10 bg-white/5 text-slate-300',
 );
}

export function ConversationSidebarTabs({ tab, setTab }: ConversationSidebarTabsProps) {
 return (
  <div className={cn('flex items-center gap-2 overflow-x-auto pb-1 scrollbar-hide')}>
   <button type="button" onClick={() => setTab('active')} className={cn(tabClass(tab === 'active'))}>Đang chat</button>
   <button type="button" onClick={() => setTab('pending')} className={cn(tabClass(tab === 'pending'))}>Pending</button>
   <button type="button" onClick={() => setTab('completed')} className={cn(tabClass(tab === 'completed'))}>Đã hoàn thành</button>
  </div>
 );
}

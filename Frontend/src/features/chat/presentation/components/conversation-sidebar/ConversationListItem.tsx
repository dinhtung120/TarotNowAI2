import Image from 'next/image';
import { useRouter } from '@/i18n/routing';
import { cn } from '@/lib/utils';
import type { ConversationDto } from '@/features/chat/application/actions';
import { formatAgo, getOther, previewText, statusLabel } from './utils';

interface ConversationListItemProps {
 conversation: ConversationDto;
 currentId?: string;
 currentUserId: string;
 nowTs: number;
}

export function ConversationListItem({ conversation, currentId, currentUserId, nowTs }: ConversationListItemProps) {
 const router = useRouter();
 const other = getOther(conversation, currentUserId);
 const status = statusLabel(conversation.status);
 const active = conversation.id === currentId;

 return (
  <button
   type="button"
   onClick={() => router.push(`/chat/${conversation.id}`)}
   className={cn('w-full rounded-xl px-3 py-3 border text-left transition-all duration-200 group', active ? 'bg-[var(--purple-accent)]/15 border-[var(--purple-accent)]/35 shadow-[0_0_15px_rgba(168,85,247,0.1)]' : 'bg-white/5 border-white/10 hover:bg-white/10 hover:border-white/20')}
  >
   <div className={cn('flex items-center gap-3')}>
    <div className={cn('relative flex-shrink-0')}>
     {other.avatar ? <Image src={other.avatar} alt={other.name} width={44} height={44} unoptimized className={cn('w-11 h-11 rounded-full object-cover border border-white/10')} /> : <div className={cn('w-11 h-11 rounded-full bg-gradient-to-br from-[var(--purple-accent)]/20 to-indigo-500/20 flex items-center justify-center text-sm font-bold border border-white/10')}>{other.name.charAt(0)}</div>}
     {other.unread > 0 ? <div className={cn('absolute -top-1 -right-1 w-5 h-5 rounded-full bg-[var(--danger)] text-white text-[10px] flex items-center justify-center border-2 border-[#0a0a0a] font-bold')}>{other.unread > 9 ? '9+' : other.unread}</div> : null}
    </div>
    <div className={cn('min-w-0 flex-1')}>
     <div className={cn('flex items-center justify-between gap-2')}>
      <p className={cn('truncate font-semibold text-sm transition-colors', active ? 'text-white' : 'text-gray-200 group-hover:text-white')}>{other.name}</p>
      <span className={cn('text-[10px] text-[var(--text-secondary)] flex-shrink-0')}>{formatAgo(nowTs, conversation.lastMessageAt)}</span>
     </div>
     <p className={cn('mt-0.5 truncate text-xs text-[var(--text-secondary)]')}>{previewText(conversation)}</p>
     <div className={cn('mt-1.5 flex items-center gap-2')}>
      <span className={cn('px-2 py-0.5 rounded-full border text-[9px] font-medium uppercase tracking-wider', status.className)}>{status.text}</span>
      {conversation.escrowTotalFrozen && conversation.escrowTotalFrozen > 0 ? <span className={cn('text-[10px] text-[var(--warning)] font-medium')}>{conversation.escrowTotalFrozen} 💎</span> : null}
     </div>
    </div>
   </div>
  </button>
 );
}

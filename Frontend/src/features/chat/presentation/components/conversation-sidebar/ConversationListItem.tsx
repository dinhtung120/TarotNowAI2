import Image from 'next/image';
import { useRouter } from '@/i18n/routing';
import { cn } from '@/lib/utils';
import type { ConversationDto } from '@/features/chat/application/actions';
import { formatAgo, getOther, previewText, statusLabel } from './utils';
import { resolveAvatarUrl } from '@/shared/infrastructure/http/assetUrl';

interface ConversationListItemProps {
 conversation: ConversationDto;
 currentId?: string;
 currentUserId: string;
 nowTs: number;
}

export function ConversationListItem({ conversation, currentId, currentUserId, nowTs }: ConversationListItemProps) {
 const router = useRouter();
 const other = getOther(conversation, currentUserId);
 const avatarSrc = resolveAvatarUrl(other.avatar);
 const status = statusLabel(conversation.status);
 const active = conversation.id === currentId;

 return (
  <button
   type="button"
   onClick={() => router.push(`/chat/${conversation.id}`)}
   className={cn('w-full rounded-xl px-3 py-3 border text-left transition-all duration-200 group', active ? 'tn-bg-accent-20 tn-border-accent-30 tn-shadow-glow-accent-15' : 'bg-white/5 border-white/10 tn-conversation-item-hover')}
  >
   <div className={cn('flex items-center gap-3')}>
    <div className={cn('relative flex-shrink-0')}>
     {avatarSrc ? <Image src={avatarSrc} alt={other.name} width={44} height={44} unoptimized className={cn('w-11 h-11 rounded-full object-cover border border-white/10')} /> : <div className={cn('w-11 h-11 rounded-full tn-bg-accent-20 flex items-center justify-center text-sm font-bold border border-white/10')}>{other.name.charAt(0)}</div>}
     {other.unread > 0 ? <div className={cn('absolute -top-1 -right-1 w-5 h-5 rounded-full tn-bg-danger text-white tn-text-10 flex items-center justify-center border-2 tn-border-chat-bg font-bold')}>{other.unread > 9 ? '9+' : other.unread}</div> : null}
    </div>
    <div className={cn('min-w-0 flex-1')}>
     <div className={cn('flex items-center justify-between gap-2')}>
      <p className={cn('truncate font-semibold text-sm transition-colors', active ? 'text-white' : 'text-gray-200 tn-group-text-white')}>{other.name}</p>
      <span className={cn('tn-text-10 tn-text-secondary flex-shrink-0')}>{formatAgo(nowTs, conversation.lastMessageAt)}</span>
     </div>
     <p className={cn('mt-0.5 truncate text-xs tn-text-secondary')}>{previewText(conversation)}</p>
     <div className={cn('mt-1.5 flex items-center gap-2')}>
      <span className={cn('px-2 py-0.5 rounded-full border tn-text-9 font-medium uppercase tracking-wider', status.className)}>{status.text}</span>
      {conversation.escrowTotalFrozen && conversation.escrowTotalFrozen > 0 ? <span className={cn('tn-text-10 tn-text-warning font-medium')}>{conversation.escrowTotalFrozen} 💎</span> : null}
     </div>
    </div>
   </div>
  </button>
 );
}

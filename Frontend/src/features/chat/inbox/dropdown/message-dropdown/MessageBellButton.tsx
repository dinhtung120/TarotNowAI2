import { MessageCircle } from 'lucide-react';
import { cn } from '@/lib/utils';

interface MessageBellButtonProps {
 ariaLabel: string;
 buttonRef: React.RefObject<HTMLButtonElement | null>;
 isOpen: boolean;
 unreadCount: number;
 onToggle: () => void;
}

export default function MessageBellButton({ ariaLabel, buttonRef, isOpen, unreadCount, onToggle }: MessageBellButtonProps) {
 return (
  <button
   ref={buttonRef}
   type="button"
   onClick={onToggle}
   className={cn(
    'relative inline-flex min-h-11 min-w-11 items-center justify-center rounded-xl p-2 transition-all cursor-pointer',
    isOpen ? 'tn-notification-bell-open' : 'bg-transparent tn-notification-bell-closed',
   )}
   aria-label={ariaLabel}
  >
   <MessageCircle className={cn('h-5 w-5')} />
   {unreadCount > 0 ? (
    <span className={cn('absolute right-1.5 top-1.5 flex tn-notification-bell-badge items-center justify-center rounded-full px-1 tn-text-9 font-black text-white shadow-md animate-in zoom-in')}>
     {unreadCount > 99 ? '99+' : unreadCount}
    </span>
   ) : null}
  </button>
 );
}

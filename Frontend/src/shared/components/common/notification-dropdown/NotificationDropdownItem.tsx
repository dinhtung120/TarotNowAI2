'use client';

import { memo, useState } from 'react';
import { Bell, Flame, Info, Star, Wallet, Zap } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { NotificationItem } from '@/features/notifications/application/actions';

const typeIconMap: Record<string, { icon: typeof Bell; colorClass: string }> = {
 system: { icon: Info, colorClass: 'text-blue-400' },
 quest: { icon: Star, colorClass: 'text-purple-400' },
 streak: { icon: Flame, colorClass: 'text-orange-400' },
 escrow: { icon: Wallet, colorClass: 'text-emerald-400' },
 payment: { icon: Zap, colorClass: 'text-yellow-400' },
};

interface NotificationDropdownItemProps {
 item: NotificationItem;
 title: string;
 timeLabel: string;
 onMarkRead: (id: string) => Promise<unknown>;
}

export const NotificationDropdownItem = memo(function NotificationDropdownItem({
 item,
 title,
 timeLabel,
 onMarkRead,
}: NotificationDropdownItemProps) {
 const typeConfig = typeIconMap[item.type] ?? typeIconMap.system;
 const Icon = typeConfig.icon;
 const [isMarking, setIsMarking] = useState(false);

 const handleClick = async () => {
  if (!item.isRead && !isMarking) {
   setIsMarking(true);
   await onMarkRead(item.id);
   setIsMarking(false);
  }
 };

 return (
  <div
   onClick={() => void handleClick()}
   className={cn(
    'group flex items-start gap-3 px-4 py-3 cursor-pointer transition-colors',
    !item.isRead
     ? 'bg-[var(--purple-accent)]/5 hover:bg-[var(--purple-accent)]/10 border-l-[3px] border-l-[var(--purple-accent)]'
     : 'bg-transparent hover:bg-[var(--bg-surface-hover)] opacity-80 border-l-[3px] border-l-transparent',
   )}
  >
   <div className={cn('w-8 h-8 rounded-full bg-[var(--bg-glass)] flex items-center justify-center shrink-0 border border-[var(--border-subtle)] group-hover:border-[var(--border-default)] transition-colors')}>
    <Icon className={cn('w-4 h-4', typeConfig.colorClass)} />
   </div>
   <div className={cn('flex-1 min-w-0')}>
    <p className={cn('text-[13px] leading-tight line-clamp-2', !item.isRead ? 'font-bold text-[var(--text-ink)]' : 'font-medium text-[var(--text-secondary)]')}>
     {title}
    </p>
    <p className={cn('mt-1 text-[11px] font-medium text-[var(--purple-muted)]')}>{timeLabel}</p>
   </div>
   {!item.isRead ? <div className={cn('w-2 h-2 rounded-full bg-[var(--danger)] shrink-0 self-center')} /> : null}
  </div>
 );
});

'use client';

import { useCallback, useState } from 'react';
import { Bell, Flame, Info, Star, Wallet, Zap } from 'lucide-react';
import type { NotificationItem } from '@/features/notifications/application/actions';

const typeIconMap: Record<string, { icon: typeof Bell; colorClass: string }> = {
  system: { icon: Info, colorClass: 'text-blue-400' },
  quest: { icon: Star, colorClass: 'text-purple-400' },
  streak: { icon: Flame, colorClass: 'text-orange-400' },
  escrow: { icon: Wallet, colorClass: 'text-emerald-400' },
  payment: { icon: Zap, colorClass: 'text-yellow-400' },
};

interface UseNotificationDropdownItemArgs {
  item: NotificationItem;
  onMarkRead: (id: string) => Promise<unknown>;
}

export function useNotificationDropdownItem({ item, onMarkRead }: UseNotificationDropdownItemArgs) {
  const typeConfig = typeIconMap[item.type] ?? typeIconMap.system;
  const [isMarking, setIsMarking] = useState(false);

  const handleClick = useCallback(async () => {
    if (item.isRead || isMarking) return;
    setIsMarking(true);
    await onMarkRead(item.id);
    setIsMarking(false);
  }, [isMarking, item.id, item.isRead, onMarkRead]);

  return {
    typeConfig,
    handleClick,
  };
}

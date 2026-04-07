'use client';

import { Coins, Gem } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/application/useAdminUsers';
import { cn } from '@/lib/utils';

interface AdminUserBalanceCellProps {
 locale: string;
 user: AdminUsersViewUser;
}

export function AdminUserBalanceCell({
 locale,
 user,
}: AdminUserBalanceCellProps) {
 return (
  <td className={cn('px-8 py-5')}>
   <div className={cn('space-y-1')}>
    <div className={cn('flex items-center gap-2 text-[11px] font-black tn-text-primary italic')}>
     <Gem className={cn('w-3 h-3 text-[var(--purple-accent)]')} />
     {user.diamondBalance.toLocaleString(locale)}
    </div>
    <div className={cn('flex items-center gap-2 text-[10px] font-bold text-[var(--warning)] italic')}>
     <Coins className={cn('w-3 h-3')} />
     {user.goldBalance.toLocaleString(locale)}
    </div>
   </div>
  </td>
 );
}

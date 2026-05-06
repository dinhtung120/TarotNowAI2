'use client';

import { Coins, Gem } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/hooks/useAdminUsers';
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
    <div className={cn('flex items-center gap-2 tn-text-11 font-black tn-text-primary italic')}>
     <Gem className={cn('w-3 h-3 tn-text-accent')} />
     {user.diamondBalance.toLocaleString(locale)}
    </div>
    <div className={cn('flex items-center gap-2 tn-text-10 font-bold tn-text-warning italic')}>
     <Coins className={cn('w-3 h-3')} />
     {user.goldBalance.toLocaleString(locale)}
    </div>
   </div>
  </td>
 );
}

'use client';

import { Mail } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/hooks/useAdminUsers';
import { cn } from '@/lib/utils';

interface AdminUserIdentityCellProps {
 user: AdminUsersViewUser;
}

export function AdminUserIdentityCell({ user }: AdminUserIdentityCellProps) {
 return (
  <td className={cn('px-8 py-5')}>
   <div className={cn('flex items-center gap-4')}>
    <div className={cn('w-10 h-10 rounded-xl tn-grad-accent-info-soft border tn-border flex items-center justify-center tn-text-primary font-black text-sm relative overflow-hidden group-row-hover-scale-110 transition-transform shadow-inner')}>
     {user.displayName?.charAt(0).toUpperCase() || 'U'}
     <div className={cn('absolute inset-0 tn-surface-strong opacity-0 group-row-hover-opacity-100 transition-opacity')} />
    </div>
    <div>
     <div className={cn('tn-text-11 font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm')}>{user.displayName}</div>
     <div className={cn('flex items-center gap-1.5 tn-text-9 font-bold tn-text-tertiary')}>
      <Mail className={cn('w-2.5 h-2.5')} />
      {user.email}
     </div>
    </div>
   </div>
  </td>
 );
}

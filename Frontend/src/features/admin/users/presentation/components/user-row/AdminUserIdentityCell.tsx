'use client';

import { Mail } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/application/useAdminUsers';
import { cn } from '@/lib/utils';

interface AdminUserIdentityCellProps {
 user: AdminUsersViewUser;
}

export function AdminUserIdentityCell({ user }: AdminUserIdentityCellProps) {
 return (
  <td className={cn('px-8 py-5')}>
   <div className={cn('flex items-center gap-4')}>
    <div className={cn('w-10 h-10 rounded-xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--info)]/20 border tn-border flex items-center justify-center tn-text-primary font-black text-sm relative overflow-hidden group-hover/row:scale-110 transition-transform shadow-inner')}>
     {user.displayName?.charAt(0).toUpperCase() || 'U'}
     <div className={cn('absolute inset-0 tn-surface-strong opacity-0 group-hover/row:opacity-100 transition-opacity')} />
    </div>
    <div>
     <div className={cn('text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm')}>{user.displayName}</div>
     <div className={cn('flex items-center gap-1.5 text-[9px] font-bold text-[var(--text-tertiary)]')}>
      <Mail className={cn('w-2.5 h-2.5')} />
      {user.email}
     </div>
    </div>
   </div>
  </td>
 );
}

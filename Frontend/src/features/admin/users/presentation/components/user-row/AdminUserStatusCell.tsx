'use client';

import { Activity, Lock } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/application/useAdminUsers';
import { cn } from '@/lib/utils';
import type { AdminUsersTranslateFn } from '../types';

interface AdminUserStatusCellProps {
 t: AdminUsersTranslateFn;
 user: AdminUsersViewUser;
}

export function AdminUserStatusCell({ t, user }: AdminUserStatusCellProps) {
 return (
  <td className={cn('px-8 py-5 text-center')}>
   <div className={cn('inline-flex items-center justify-center gap-2 text-[9px] font-black uppercase tracking-widest', user.isLocked ? 'text-[var(--danger)]' : 'text-[var(--success)]')}>
    {user.isLocked ? <Lock className={cn('w-3 h-3')} /> : <Activity className={cn('w-3 h-3 animate-pulse')} />}
    {user.isLocked ? t('users.status.locked') : t('users.status.active')}
   </div>
  </td>
 );
}

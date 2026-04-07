'use client';

import type { AdminUsersViewUser } from '@/features/admin/users/application/useAdminUsers';
import { cn } from '@/lib/utils';
import type { AdminUsersTranslateFn } from '../types';

interface AdminUserLevelCellProps {
 t: AdminUsersTranslateFn;
 user: AdminUsersViewUser;
}

export function AdminUserLevelCell({ t, user }: AdminUserLevelCellProps) {
 return (
  <td className={cn('px-8 py-5')}>
   <div className={cn('flex items-center gap-2')}>
    <div className={cn('px-2 py-0.5 rounded-md tn-bg-warning-10 border tn-border-warning-20 tn-text-9 font-black tn-text-warning shadow-inner')}>
     {t('users.row.level', { level: user.level })}
    </div>
    <div className={cn('tn-text-10 font-bold tn-text-tertiary')}>
     {t('users.row.exp', { exp: user.exp })}
    </div>
   </div>
  </td>
 );
}

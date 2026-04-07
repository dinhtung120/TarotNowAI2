'use client';

import { Star } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/application/useAdminUsers';
import { cn } from '@/lib/utils';
import type { AdminUsersTranslateFn } from '../types';
import { roleClassName, roleLabel } from './userRole';

interface AdminUserRoleCellProps {
 t: AdminUsersTranslateFn;
 user: AdminUsersViewUser;
}

export function AdminUserRoleCell({ t, user }: AdminUserRoleCellProps) {
 return (
  <td className={cn('px-8 py-5')}>
   <div className={cn('inline-flex items-center gap-1.5 px-3 py-1 rounded-full text-[9px] font-black uppercase tracking-widest border shadow-inner', roleClassName(user.role))}>
    <Star className={cn('w-2.5 h-2.5 fill-current')} />
    {roleLabel(user.role, t)}
   </div>
  </td>
 );
}

'use client';

import { Edit2 } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/application/useAdminUsers';
import type { AdminUsersTranslateFn } from '../types';
import { cn } from '@/lib/utils';

interface AdminUserActionsCellProps {
 onEdit: (user: AdminUsersViewUser) => void;
 t: AdminUsersTranslateFn;
 user: AdminUsersViewUser;
}

export function AdminUserActionsCell({ onEdit, t, user }: AdminUserActionsCellProps) {
 return (
  <td className={cn('px-8 py-5 text-right')}>
   <div className={cn('flex items-center justify-end gap-2 opacity-0 group-row-hover-opacity-100')}>
    <button type="button" onClick={() => onEdit(user)} className={cn('p-2.5 min-h-11 min-w-11 rounded-xl border tn-admin-action-btn transition-all shadow-md group')} title={t('users.actions.edit_title')}>
     <Edit2 className={cn('w-4 h-4')} />
    </button>
   </div>
  </td>
 );
}

'use client';

import { Edit2 } from 'lucide-react';
import type { AdminUsersViewUser } from '@/features/admin/users/application/useAdminUsers';
import { cn } from '@/lib/utils';

interface AdminUserActionsCellProps {
 onEdit: (user: AdminUsersViewUser) => void;
 user: AdminUsersViewUser;
}

export function AdminUserActionsCell({ onEdit, user }: AdminUserActionsCellProps) {
 return (
  <td className={cn('px-8 py-5 text-right')}>
   <div className={cn('flex items-center justify-end gap-2 opacity-0 group-row-hover-opacity-100')}>
    <button type="button" onClick={() => onEdit(user)} className={cn('p-2.5 min-h-11 min-w-11 rounded-xl border tn-admin-action-btn transition-all shadow-md group')} title="Sửa Thông Tin Thiết Lập">
     <Edit2 className={cn('w-4 h-4')} />
    </button>
   </div>
  </td>
 );
}

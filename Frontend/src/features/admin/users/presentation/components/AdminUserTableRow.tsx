import { cn } from '@/lib/utils';
import type { AdminUserTableRowProps } from './types';
import { AdminUserActionsCell } from './user-row/AdminUserActionsCell';
import { AdminUserBalanceCell } from './user-row/AdminUserBalanceCell';
import { AdminUserIdentityCell } from './user-row/AdminUserIdentityCell';
import { AdminUserLevelCell } from './user-row/AdminUserLevelCell';
import { AdminUserRoleCell } from './user-row/AdminUserRoleCell';
import { AdminUserStatusCell } from './user-row/AdminUserStatusCell';

export function AdminUserTableRow({ locale, onEdit, t, user }: AdminUserTableRowProps) {
 return (
  <tr className={cn('group-row tn-hover-surface-strong transition-colors')}>
   <AdminUserIdentityCell user={user} />
   <AdminUserLevelCell t={t} user={user} />
   <AdminUserBalanceCell locale={locale} user={user} />
   <AdminUserRoleCell t={t} user={user} />
   <AdminUserStatusCell t={t} user={user} />
   <AdminUserActionsCell onEdit={onEdit} user={user} />
  </tr>
 );
}

import type { AdminUserItem } from '@/features/admin/application/actions';

export interface AdminUsersViewUser extends AdminUserItem {
 isLocked: boolean;
}

export interface AdminUsersEditModalState {
 isOpen: boolean;
 user: AdminUsersViewUser | null;
}

export interface AdminUsersAddModalState {
 isOpen: boolean;
}

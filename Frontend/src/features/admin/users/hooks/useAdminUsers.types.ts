import type { AdminUserItem } from '@/features/admin/users/actions/users';

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

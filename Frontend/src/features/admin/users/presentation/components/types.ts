import type { Dispatch, SetStateAction } from "react";
import type { CreateUserParams, UpdateUserParams } from "@/features/admin/application/actions";
import type { AdminUsersAddModalState, AdminUsersEditModalState, AdminUsersViewUser } from "@/features/admin/users/application/useAdminUsers";

export type AdminUsersTranslateFn = (
 key: string,
 values?: Record<string, string | number>,
) => string;

export type SetAddUserForm = Dispatch<SetStateAction<CreateUserParams>>;
export type SetEditUserForm = Dispatch<SetStateAction<UpdateUserParams>>;

export interface AdminUsersAddModalProps {
 addModal: AdminUsersAddModalState;
 addForm: CreateUserParams;
 closeAddModal: () => void;
 createLoading: boolean;
 onCreateUser: () => void;
 setAddForm: SetAddUserForm;
 t: AdminUsersTranslateFn;
}

export interface AdminUsersEditModalProps {
 actionLoading: boolean;
 closeEditModal: () => void;
 editForm: UpdateUserParams;
 editModal: AdminUsersEditModalState;
 onSaveUser: () => void;
 setEditForm: SetEditUserForm;
 t: AdminUsersTranslateFn;
}

export interface AdminUserTableRowProps {
 locale: string;
 onEdit: (user: AdminUsersViewUser) => void;
 t: AdminUsersTranslateFn;
 user: AdminUsersViewUser;
}

export interface AdminUsersTableProps {
 loading: boolean;
 locale: string;
 onNextPage: () => void;
 onEdit: (user: AdminUsersViewUser) => void;
 onPrevPage: () => void;
 page: number;
 t: AdminUsersTranslateFn;
 totalCount: number;
 users: AdminUsersViewUser[];
}

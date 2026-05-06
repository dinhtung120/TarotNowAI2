'use client';

import { useMemo, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { useLocale, useTranslations } from 'next-intl';
import { listUsers } from '@/features/admin/users/actions/users';
import { ADMIN_QUERY_POLICY } from '@/features/admin/shared/adminQueryPolicy';
import { adminQueryKeys } from '@/features/admin/shared/adminQueryKeys';
import { useAdminUsersModalForms } from '@/features/admin/users/hooks/useAdminUsersModalForms';
import { useAdminUsersMutations } from '@/features/admin/users/hooks/useAdminUsersMutations';
import { queryFnOrThrow, useDebouncedQueryInput } from '@/shared/utils/queryPolicy';

export type {
 AdminUsersAddModalState,
 AdminUsersEditModalState,
 AdminUsersViewUser,
} from '@/features/admin/users/hooks/useAdminUsers.types';

const SEARCH_DEBOUNCE_MS = 300;

export function useAdminUsers() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const queryClient = useQueryClient();
 const [page, setPage] = useState(1);
 const [searchTerm, setSearchTerm] = useState('');
 const debouncedSearchTerm = useDebouncedQueryInput(searchTerm.trim(), SEARCH_DEBOUNCE_MS);
 const {
  addUserForm,
  editUserForm,
  addModal,
  editModal,
  addForm,
  editForm,
  setAddForm,
  setEditForm,
  addFormErrors,
  editFormErrors,
  handleOpenEdit,
  closeEditModal,
  handleOpenAdd,
  closeAddModal,
 } = useAdminUsersModalForms();

 const queryKey = adminQueryKeys.users(page, debouncedSearchTerm);
 const usersQuery = useQuery({
  queryKey,
 queryFn: async () => {
  const result = await listUsers(page, 10, debouncedSearchTerm);
  const payload = queryFnOrThrow(result, 'Failed to load admin users');
  return {
    users: payload.users.map((item) => ({
     ...item,
     isLocked: item.status?.toLowerCase() === 'locked',
    })),
   totalCount: payload.totalCount,
  };
 },
  ...ADMIN_QUERY_POLICY.list,
 });

 const users = useMemo(() => usersQuery.data?.users ?? [], [usersQuery.data]);
 const totalCount = usersQuery.data?.totalCount ?? 0;
 const loading = usersQuery.isPending || usersQuery.isFetching;
 const listError = usersQuery.error instanceof Error ? usersQuery.error.message : '';

 const { handleCreateUser, handleSaveUser, actionLoading, createLoading } = useAdminUsersMutations({
  t,
  queryClient,
  editModal,
  addUserForm,
  editUserForm,
  closeAddModal,
  closeEditModal,
 });

 return {
  t,
  locale,
  users,
  totalCount,
  page,
  setPage,
  searchTerm,
  setSearchTerm,
  loading,
  listError,
  addModal,
  addForm,
  setAddForm,
  addFormErrors,
  editModal,
  editForm,
  setEditForm,
  editFormErrors,
  actionLoading,
  createLoading,
  handleOpenAdd,
  closeAddModal,
  handleCreateUser,
  handleOpenEdit,
  closeEditModal,
  handleSaveUser,
 };
}

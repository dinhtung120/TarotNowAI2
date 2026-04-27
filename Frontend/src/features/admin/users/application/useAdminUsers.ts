'use client';

import { useCallback, useMemo, useState, type Dispatch, type SetStateAction } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { useForm, useWatch } from 'react-hook-form';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import { z } from 'zod';
import {
 createUser,
 listUsers,
 updateUser,
 type AdminUserItem,
 type CreateUserParams,
 type UpdateUserParams,
} from '@/features/admin/application/actions';
import { queryFnOrThrow, useDebouncedQueryInput } from '@/shared/application/utils/queryPolicy';

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

const EMPTY_ADD_FORM: CreateUserParams = {
 email: '',
 username: '',
 displayName: '',
 password: '',
 role: 'user',
};

const EMPTY_EDIT_FORM: UpdateUserParams = {
 role: 'user',
 status: 'active',
 diamondBalance: 0,
 goldBalance: 0,
};

const SEARCH_DEBOUNCE_MS = 300;

const createUserSchema = z.object({
 email: z.string().trim().email(),
 username: z.string().trim().min(3).max(32).regex(/^[a-zA-Z0-9_]+$/),
 displayName: z.string().trim().min(2).max(50),
 password: z.string().min(8).max(100),
 role: z.enum(['user', 'tarot_reader', 'admin']),
});

const updateUserSchema = z.object({
 role: z.enum(['user', 'tarot_reader', 'admin']),
 status: z.enum(['active', 'locked']),
 diamondBalance: z.number().int().min(0),
 goldBalance: z.number().int().min(0),
});

type SetStateForm<T> = Dispatch<SetStateAction<T>>;

export function useAdminUsers() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const queryClient = useQueryClient();
 const [page, setPage] = useState(1);
 const [searchTerm, setSearchTerm] = useState('');
 const debouncedSearchTerm = useDebouncedQueryInput(searchTerm.trim(), SEARCH_DEBOUNCE_MS);
 const [editModal, setEditModal] = useState<AdminUsersEditModalState>({
  isOpen: false,
  user: null,
 });
 const [addModal, setAddModal] = useState<AdminUsersAddModalState>({
  isOpen: false,
 });

 const addUserForm = useForm<CreateUserParams>({
  resolver: zodResolver(createUserSchema),
  defaultValues: EMPTY_ADD_FORM,
 });
 const editUserForm = useForm<UpdateUserParams>({
  resolver: zodResolver(updateUserSchema),
  defaultValues: EMPTY_EDIT_FORM,
 });

 const queryKey = ['admin', 'users', page, debouncedSearchTerm] as const;
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
 });

 const users = useMemo(() => usersQuery.data?.users ?? [], [usersQuery.data]);
 const totalCount = usersQuery.data?.totalCount ?? 0;
 const loading = usersQuery.isPending || usersQuery.isFetching;
 const listError = usersQuery.error instanceof Error ? usersQuery.error.message : '';

 const updateMutation = useMutation({
  mutationFn: async (payload: { userId: string; values: UpdateUserParams; enforceMoneyEvent: boolean }) =>
   updateUser(payload.userId, payload.values, { enforceMoneyEvent: payload.enforceMoneyEvent }),
 });
 const createMutation = useMutation({
  mutationFn: async (values: CreateUserParams) => createUser(values),
 });

 const handleOpenEdit = useCallback((user: AdminUsersViewUser) => {
  editUserForm.reset({
   role: user.role as UpdateUserParams['role'],
   status: user.isLocked ? 'locked' : 'active',
   diamondBalance: user.diamondBalance,
   goldBalance: user.goldBalance,
  });
  setEditModal({ isOpen: true, user });
 }, [editUserForm]);

 const closeEditModal = useCallback(() => {
  setEditModal({ isOpen: false, user: null });
  editUserForm.reset(EMPTY_EDIT_FORM);
 }, [editUserForm]);

 const resetAddForm = useCallback(() => {
  addUserForm.reset(EMPTY_ADD_FORM);
 }, [addUserForm]);

 const handleOpenAdd = useCallback(() => {
  resetAddForm();
  setAddModal({ isOpen: true });
 }, [resetAddForm]);

 const closeAddModal = useCallback(() => {
  setAddModal({ isOpen: false });
  resetAddForm();
 }, [resetAddForm]);

 const handleCreateUser = addUserForm.handleSubmit(async (values) => {
  try {
   const payload: CreateUserParams = {
    email: values.email.trim(),
    username: values.username.trim(),
    displayName: values.displayName.trim(),
    password: values.password,
    role: values.role,
   };
   const result = await createMutation.mutateAsync(payload);
   if (!result.success) {
    toast.error(result.error || t('users.toast.create_failed'));
    return;
   }

   closeAddModal();
   toast.success(t('users.toast.create_success'));
   await queryClient.invalidateQueries({ queryKey: ['admin', 'users'] });
  } catch {
   toast.error(t('users.toast.system_error'));
  }
 }, () => {
  toast.error(t('users.toast.create_failed'));
 });

 const handleSaveUser = editUserForm.handleSubmit(async (values) => {
  if (!editModal.user) return;

  const hasBalanceChanged = values.diamondBalance !== editModal.user.diamondBalance
   || values.goldBalance !== editModal.user.goldBalance;

  try {
   const result = await updateMutation.mutateAsync({
    userId: editModal.user.id,
    values,
    enforceMoneyEvent: hasBalanceChanged,
   });
   if (!result.success) {
    toast.error(result.error || t('users.toast.update_failed'));
    return;
   }

   closeEditModal();
   toast.success(t('users.toast.update_success'));
   await queryClient.invalidateQueries({ queryKey: ['admin', 'users'] });
  } catch {
   toast.error(t('users.toast.system_error'));
  }
 }, () => {
  toast.error(t('users.toast.update_failed'));
 });

 const watchedAddForm = useWatch({ control: addUserForm.control });
 const watchedEditForm = useWatch({ control: editUserForm.control });
 const addForm: CreateUserParams = { ...EMPTY_ADD_FORM, ...watchedAddForm };
 const editForm: UpdateUserParams = { ...EMPTY_EDIT_FORM, ...watchedEditForm };

 const setAddForm = useMemo<SetStateForm<CreateUserParams>>(() => {
  return (nextValueOrUpdater) => {
   const current = addUserForm.getValues();
   const next = typeof nextValueOrUpdater === 'function'
    ? nextValueOrUpdater(current)
    : nextValueOrUpdater;

   for (const [key, value] of Object.entries(next)) {
    addUserForm.setValue(key as keyof CreateUserParams, value, {
     shouldDirty: true,
     shouldValidate: true,
    });
   }
  };
 }, [addUserForm]);

 const setEditForm = useMemo<SetStateForm<UpdateUserParams>>(() => {
  return (nextValueOrUpdater) => {
   const current = editUserForm.getValues();
   const next = typeof nextValueOrUpdater === 'function'
    ? nextValueOrUpdater(current)
    : nextValueOrUpdater;

   for (const [key, value] of Object.entries(next)) {
    editUserForm.setValue(key as keyof UpdateUserParams, value, {
     shouldDirty: true,
     shouldValidate: true,
    });
   }
  };
 }, [editUserForm]);

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
  addFormErrors: addUserForm.formState.errors,
  editModal,
  editForm,
  setEditForm,
  editFormErrors: editUserForm.formState.errors,
  actionLoading: updateMutation.isPending,
  createLoading: createMutation.isPending,
  handleOpenAdd,
  closeAddModal,
  handleCreateUser,
  handleOpenEdit,
  closeEditModal,
  handleSaveUser,
 };
}

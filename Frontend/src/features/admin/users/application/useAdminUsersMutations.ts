'use client';

import { useMutation, type QueryClient } from '@tanstack/react-query';
import type { UseFormReturn } from 'react-hook-form';
import toast from 'react-hot-toast';
import {
 createUser,
 updateUser,
 type CreateUserParams,
 type UpdateUserParams,
} from '@/features/admin/application/actions';
import type { AdminUsersEditModalState } from './useAdminUsers.types';

interface UseAdminUsersMutationsOptions {
 t: (key: string) => string;
 queryClient: QueryClient;
 editModal: AdminUsersEditModalState;
 addUserForm: UseFormReturn<CreateUserParams>;
 editUserForm: UseFormReturn<UpdateUserParams>;
 closeAddModal: () => void;
 closeEditModal: () => void;
}

export function useAdminUsersMutations(options: UseAdminUsersMutationsOptions) {
 const { t, queryClient, editModal, addUserForm, editUserForm, closeAddModal, closeEditModal } = options;
 const updateMutation = useMutation({
  mutationFn: async (payload: { userId: string; values: UpdateUserParams }) =>
   updateUser(payload.userId, payload.values),
 });
 const createMutation = useMutation({
  mutationFn: async (values: CreateUserParams) => createUser(values),
 });

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
  if (!editModal.user) {
   return;
  }

  try {
   const result = await updateMutation.mutateAsync({
    userId: editModal.user.id,
    values,
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

 return {
  handleCreateUser,
  handleSaveUser,
  actionLoading: updateMutation.isPending,
  createLoading: createMutation.isPending,
 };
}

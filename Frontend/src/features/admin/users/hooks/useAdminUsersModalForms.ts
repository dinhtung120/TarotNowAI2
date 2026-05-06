'use client';

import { useCallback, useMemo, useState } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm, useWatch } from 'react-hook-form';
import type { CreateUserParams, UpdateUserParams } from '@/features/admin/users/actions/users';
import {
 EMPTY_ADD_FORM,
 EMPTY_EDIT_FORM,
 createSetStateForm,
 createUserSchema,
 updateUserSchema,
} from '@/features/admin/users/hooks/useAdminUsers.config';
import type {
 AdminUsersAddModalState,
 AdminUsersEditModalState,
 AdminUsersViewUser,
} from '@/features/admin/users/hooks/useAdminUsers.types';

export function useAdminUsersModalForms() {
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

 const watchedAddForm = useWatch({ control: addUserForm.control });
 const watchedEditForm = useWatch({ control: editUserForm.control });
 const addForm: CreateUserParams = { ...EMPTY_ADD_FORM, ...watchedAddForm };
 const editForm: UpdateUserParams = { ...EMPTY_EDIT_FORM, ...watchedEditForm };
 const setAddForm = useMemo(() => createSetStateForm(addUserForm), [addUserForm]);
 const setEditForm = useMemo(() => createSetStateForm(editUserForm), [editUserForm]);

 return {
  addUserForm,
  editUserForm,
  addModal,
  editModal,
  addForm,
  editForm,
  setAddForm,
  setEditForm,
  addFormErrors: addUserForm.formState.errors,
  editFormErrors: editUserForm.formState.errors,
  handleOpenEdit,
  closeEditModal,
  handleOpenAdd,
  closeAddModal,
 };
}

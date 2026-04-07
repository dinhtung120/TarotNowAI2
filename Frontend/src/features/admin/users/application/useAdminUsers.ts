'use client';
import { useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import {
 createUser,
 listUsers,
 updateUser,
 type AdminUserItem,
 type CreateUserParams,
 type UpdateUserParams,
} from '@/features/admin/application/actions';
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
export function useAdminUsers() {
 const t = useTranslations('Admin');
 const locale = useLocale();
 const queryClient = useQueryClient();
 const [page, setPage] = useState(1);
 const [searchTerm, setSearchTerm] = useState('');
 const [editModal, setEditModal] = useState<AdminUsersEditModalState>({
  isOpen: false,
  user: null,
 });
 const [addModal, setAddModal] = useState<AdminUsersAddModalState>({
  isOpen: false,
 });
 const [addForm, setAddForm] = useState<CreateUserParams>(EMPTY_ADD_FORM);
 const [editForm, setEditForm] = useState<UpdateUserParams>({
  role: 'user',
  status: 'active',
  diamondBalance: 0,
  goldBalance: 0,
 });
 const queryKey = ['admin', 'users', page, searchTerm] as const;
 const { data, isLoading, isFetching } = useQuery({
  queryKey,
  queryFn: async () => {
   const result = await listUsers(page, 10, searchTerm);
   if (!result.success || !result.data) return { users: [] as AdminUsersViewUser[], totalCount: 0 };
   return {
    users: result.data.users.map((item) => ({
     ...item,
     isLocked: item.status?.toLowerCase() === 'locked',
    })),
    totalCount: result.data.totalCount,
   };
  },
 });
 const users = useMemo(() => data?.users ?? [], [data]);
 const totalCount = data?.totalCount ?? 0;
 const loading = isLoading || isFetching;
 const updateMutation = useMutation({
  mutationFn: async (payload: { userId: string; values: UpdateUserParams }) =>
   updateUser(payload.userId, payload.values),
 });
 const createMutation = useMutation({
  mutationFn: async (values: CreateUserParams) => createUser(values),
 });
 const handleOpenEdit = (user: AdminUsersViewUser) => {
  setEditForm({
   role: user.role,
   status: user.isLocked ? 'locked' : 'active',
   diamondBalance: user.diamondBalance,
   goldBalance: user.goldBalance,
  });
  setEditModal({ isOpen: true, user });
 };
 const closeEditModal = () => {
  setEditModal({ isOpen: false, user: null });
 };
 const resetAddForm = () => setAddForm(EMPTY_ADD_FORM);
 const handleOpenAdd = () => { resetAddForm(); setAddModal({ isOpen: true }); };
 const closeAddModal = () => { setAddModal({ isOpen: false }); resetAddForm(); };
 const handleCreateUser = async () => {
  try {
   const result = await createMutation.mutateAsync(addForm);
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
 };
 const handleSaveUser = async () => {
  if (!editModal.user) return;
  try {
   const result = await updateMutation.mutateAsync({
    userId: editModal.user.id,
    values: editForm,
   });
   if (!result.success) {
    toast.error(t('users.toast.update_failed'));
    return;
   }
   closeEditModal();
   toast.success(t('users.toast.update_success'));
   await queryClient.invalidateQueries({ queryKey: ['admin', 'users'] });
  } catch {
   toast.error(t('users.toast.system_error'));
  }
 };
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
  addModal,
  addForm,
  setAddForm,
  editModal,
  editForm,
  setEditForm,
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

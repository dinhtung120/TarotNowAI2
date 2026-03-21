'use client';

import { useCallback, useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import { useLocale, useTranslations } from 'next-intl';
import {
 listUsers,
 updateUser,
 type AdminUserItem,
 type UpdateUserParams,
} from '@/actions/adminActions';

interface User extends AdminUserItem {
 isLocked: boolean;
}

export function useAdminUsers() {
 const t = useTranslations('Admin');
 const locale = useLocale();

 const [users, setUsers] = useState<User[]>([]);
 const [totalCount, setTotalCount] = useState(0);
 const [page, setPage] = useState(1);
 const [searchTerm, setSearchTerm] = useState('');
 const [loading, setLoading] = useState(true);
 const [editModal, setEditModal] = useState<{ isOpen: boolean; user: User | null }>({
  isOpen: false,
  user: null,
 });
 const [editForm, setEditForm] = useState<UpdateUserParams>({
  role: 'user',
  status: 'active',
  diamondBalance: 0,
  goldBalance: 0,
 });
 const [actionLoading, setActionLoading] = useState(false);

 const fetchUsers = useCallback(async () => {
  setLoading(true);
  try {
   const data = await listUsers(page, 10, searchTerm);
   if (!data) return;

   const mappedUsers: User[] = data.users.map((item) => ({
    ...item,
    isLocked: item.status === 'Locked',
   }));
   setUsers(mappedUsers);
   setTotalCount(data.totalCount);
  } finally {
   setLoading(false);
  }
 }, [page, searchTerm]);

 useEffect(() => {
  const timer = window.setTimeout(() => {
   void fetchUsers();
  }, 0);

  return () => {
   window.clearTimeout(timer);
  };
 }, [fetchUsers]);

 const handleOpenEdit = (user: User) => {
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

 const handleSaveUser = async () => {
  if (!editModal.user) return;

  setActionLoading(true);
  try {
   const success = await updateUser(editModal.user.id, editForm);
   if (!success) {
    toast.error('Cập nhật thất bại. Xin vui lòng thử lại.');
    return;
   }

   closeEditModal();
   toast.success('Đã cập nhật thông tin người dùng thành công.');
   await fetchUsers();
  } catch {
   toast.error(t('users.toast.system_error'));
  } finally {
   setActionLoading(false);
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
  editModal,
  setEditModal,
  editForm,
  setEditForm,
  actionLoading,
  handleOpenEdit,
  closeEditModal,
  handleSaveUser,
 };
}

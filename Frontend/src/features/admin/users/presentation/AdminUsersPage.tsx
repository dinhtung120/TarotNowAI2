'use client';

import { useAdminUsers } from '@/features/admin/users/application/useAdminUsers';
import { cn } from '@/lib/utils';
import { AdminUsersPageHeader } from './components/AdminUsersPageHeader';
import { AdminUsersPageModals } from './components/AdminUsersPageModals';
import { AdminUsersTable } from './components/AdminUsersTable';
import type { AdminUsersTranslateFn } from './components/types';

export default function AdminUsersPage() {
 const {
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
  actionLoading,
  createLoading,
  handleOpenAdd,
  closeAddModal,
  handleCreateUser,
  handleOpenEdit,
  closeEditModal,
  handleSaveUser,
 } = useAdminUsers();
 const translate = t as AdminUsersTranslateFn;

 return (
  <div className={cn('space-y-8 pb-20 animate-in fade-in duration-700')}>
   <AdminUsersPageModals
    addModalProps={{ addModal, addForm, closeAddModal, createLoading, onCreateUser: handleCreateUser, setAddForm, t: translate }}
    editModalProps={{ actionLoading, closeEditModal, editForm, editModal, onSaveUser: handleSaveUser, setEditForm, t: translate }}
   />
   <AdminUsersPageHeader
    t={translate}
    totalCount={totalCount}
    searchTerm={searchTerm}
    onOpenAdd={handleOpenAdd}
    onSearchTermChange={(value) => {
     setSearchTerm(value);
     setPage(1);
    }}
   />
   <AdminUsersTable
    loading={loading}
    locale={locale}
    onEdit={handleOpenEdit}
    onNextPage={() => setPage((currentPage) => currentPage + 1)}
    onPrevPage={() => setPage((currentPage) => Math.max(1, currentPage - 1))}
    page={page}
    t={translate}
    totalCount={totalCount}
    users={users}
   />
  </div>
 );
}

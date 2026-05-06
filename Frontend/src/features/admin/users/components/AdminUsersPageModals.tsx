'use client';

import { AdminUsersAddModal } from './AdminUsersAddModal';
import { AdminUsersEditModal } from './AdminUsersEditModal';
import type { AdminUsersAddModalProps, AdminUsersEditModalProps } from './types';

interface AdminUsersPageModalsProps {
 addModalProps: AdminUsersAddModalProps;
 editModalProps: AdminUsersEditModalProps;
}

export function AdminUsersPageModals({
 addModalProps,
 editModalProps,
}: AdminUsersPageModalsProps) {
 return (
  <>
   <AdminUsersAddModal {...addModalProps} />
   <AdminUsersEditModal {...editModalProps} />
  </>
 );
}

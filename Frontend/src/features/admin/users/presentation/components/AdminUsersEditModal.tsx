import { Edit2 } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { AdminUsersEditModalProps } from './types';
import { AdminUsersEditFields } from './user-modals/AdminUsersEditFields';
import { AdminUsersModalActions } from './user-modals/AdminUsersModalActions';
import { AdminUsersModalHeader } from './user-modals/AdminUsersModalHeader';

export function AdminUsersEditModal({
 actionLoading,
 closeEditModal,
 editForm,
 editModal,
 onSaveUser,
 setEditForm,
 t,
}: AdminUsersEditModalProps) {
 if (!editModal.isOpen || !editModal.user) {
  return null;
 }

 return (
  <div className={cn('fixed inset-0 z-[150] flex items-center justify-center p-4 md:p-6 animate-in fade-in duration-300')}>
   <button type="button" className={cn('absolute inset-0 tn-overlay-strong')} onClick={closeEditModal} aria-label={t('users.lock_modal.cancel')} />
   <div className={cn('relative z-10 w-full max-w-lg tn-panel rounded-[3rem] overflow-hidden shadow-[0_0_100px_var(--c-168-85-247-15)] animate-in zoom-in-95 duration-300')}>
    <AdminUsersModalHeader cancelLabel={t('users.lock_modal.cancel')} Icon={Edit2} iconClassName={cn('bg-[var(--warning)]/10 border-[var(--warning)]/20 text-[var(--warning)]')} onClose={closeEditModal} title="Sửa Thông Tin" subtitle={`${editModal.user.displayName} / ${editModal.user.email}`} />
    <AdminUsersEditFields editForm={editForm} setEditForm={setEditForm} />
    <AdminUsersModalActions cancelLabel={t('users.lock_modal.cancel')} confirmLabel="Xác Nhận & Lưu" confirmIcon={Edit2} isLoading={actionLoading} onCancel={closeEditModal} onConfirm={onSaveUser} />
   </div>
  </div>
 );
}

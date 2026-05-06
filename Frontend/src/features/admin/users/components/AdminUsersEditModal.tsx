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
 editFormErrors,
 editModal,
 onSaveUser,
 setEditForm,
 t,
}: AdminUsersEditModalProps) {
 if (!editModal.isOpen || !editModal.user) {
  return null;
 }

 return (
  <div className={cn('fixed inset-0 tn-z-150 flex items-center justify-center tn-pad-4-6-md animate-in fade-in duration-300')}>
   <button type="button" className={cn('absolute inset-0 tn-overlay-strong')} onClick={closeEditModal} aria-label={t('users.lock_modal.cancel')} />
   <div className={cn('relative z-10 w-full max-w-lg tn-panel tn-rounded-3xl overflow-hidden tn-shadow-accent-100-soft animate-in zoom-in-95 duration-300')}>
    <AdminUsersModalHeader cancelLabel={t('users.lock_modal.cancel')} Icon={Edit2} iconClassName={cn('tn-icon-chip-warning')} onClose={closeEditModal} title={t('users.edit_user.title')} subtitle={`${editModal.user.displayName} / ${editModal.user.email}`} />
    <AdminUsersEditFields editForm={editForm} editFormErrors={editFormErrors} setEditForm={setEditForm} t={t} />
    <AdminUsersModalActions cancelLabel={t('users.lock_modal.cancel')} confirmLabel={t('users.edit_user.confirm')} confirmIcon={Edit2} isLoading={actionLoading} onCancel={closeEditModal} onConfirm={onSaveUser} />
   </div>
  </div>
 );
}

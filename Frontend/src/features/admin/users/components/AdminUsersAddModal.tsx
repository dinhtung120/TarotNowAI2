import { UserPlus } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { AdminUsersAddModalProps } from './types';
import { AdminUsersAddFields } from './user-modals/AdminUsersAddFields';
import { AdminUsersModalActions } from './user-modals/AdminUsersModalActions';
import { AdminUsersModalHeader } from './user-modals/AdminUsersModalHeader';

export function AdminUsersAddModal({
 addModal,
 addForm,
 addFormErrors,
 closeAddModal,
 createLoading,
 onCreateUser,
 setAddForm,
 t,
}: AdminUsersAddModalProps) {
 if (!addModal.isOpen) {
  return null;
 }

 return (
  <div className={cn('fixed inset-0 tn-z-160 flex items-center justify-center tn-pad-4-6-md animate-in fade-in duration-300')}>
   <button type="button" className={cn('absolute inset-0 tn-overlay-strong')} onClick={closeAddModal} aria-label={t('users.add_user.cancel')} />
   <div className={cn('relative z-10 w-full max-w-xl tn-panel tn-rounded-3xl overflow-hidden tn-shadow-accent-100-soft animate-in zoom-in-95 duration-300')}>
    <AdminUsersModalHeader cancelLabel={t('users.add_user.cancel')} Icon={UserPlus} iconClassName={cn('tn-icon-chip-info')} onClose={closeAddModal} title={t('users.add_user.title')} subtitle={t('users.add_user.subtitle')} />
    <AdminUsersAddFields addForm={addForm} addFormErrors={addFormErrors} setAddForm={setAddForm} t={t} />
    <AdminUsersModalActions cancelLabel={t('users.add_user.cancel')} confirmLabel={t('users.add_user.submit')} confirmIcon={UserPlus} isLoading={createLoading} onCancel={closeAddModal} onConfirm={onCreateUser} />
   </div>
  </div>
 );
}

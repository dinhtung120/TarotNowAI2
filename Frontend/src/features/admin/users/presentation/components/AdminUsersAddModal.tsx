import { UserPlus } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { AdminUsersAddModalProps } from './types';
import { AdminUsersAddFields } from './user-modals/AdminUsersAddFields';
import { AdminUsersModalActions } from './user-modals/AdminUsersModalActions';
import { AdminUsersModalHeader } from './user-modals/AdminUsersModalHeader';

export function AdminUsersAddModal({
 addModal,
 addForm,
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
  <div className={cn('fixed inset-0 z-[160] flex items-center justify-center p-4 md:p-6 animate-in fade-in duration-300')}>
   <button type="button" className={cn('absolute inset-0 tn-overlay-strong')} onClick={closeAddModal} aria-label={t('users.add_user.cancel')} />
   <div className={cn('relative z-10 w-full max-w-xl tn-panel rounded-[3rem] overflow-hidden shadow-[0_0_100px_var(--c-168-85-247-15)] animate-in zoom-in-95 duration-300')}>
    <AdminUsersModalHeader cancelLabel={t('users.add_user.cancel')} Icon={UserPlus} iconClassName={cn('bg-[var(--info)]/10 border-[var(--info)]/20 text-[var(--info)]')} onClose={closeAddModal} title={t('users.add_user.title')} subtitle={t('users.add_user.subtitle')} />
    <AdminUsersAddFields addForm={addForm} setAddForm={setAddForm} t={t} />
    <AdminUsersModalActions cancelLabel={t('users.add_user.cancel')} confirmLabel={t('users.add_user.submit')} confirmIcon={UserPlus} isLoading={createLoading} onCancel={closeAddModal} onConfirm={onCreateUser} />
   </div>
  </div>
 );
}

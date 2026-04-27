'use client';

import { Coins, Gem } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { FieldErrors } from 'react-hook-form';
import type { UpdateUserParams } from '@/features/admin/application/actions';
import type { AdminUsersTranslateFn, SetEditUserForm } from '../types';
import { AdminUsersLedgerNotice } from './AdminUsersLedgerNotice';

interface AdminUsersEditFieldsProps {
 editForm: UpdateUserParams;
 editFormErrors: FieldErrors<UpdateUserParams>;
 setEditForm: SetEditUserForm;
 t: AdminUsersTranslateFn;
}

const SELECT_CLASS = 'w-full tn-field tn-field-accent tn-border-soft tn-text-primary rounded-xl px-4 py-3 tn-surface font-bold italic shadow-inner';

function getErrorMessage(errors: FieldErrors<UpdateUserParams>, field: keyof UpdateUserParams): string {
 const value = errors[field]?.message;
 return typeof value === 'string' ? value : '';
}

export function AdminUsersEditFields({ editForm, editFormErrors, setEditForm, t }: AdminUsersEditFieldsProps) {
 const update = <K extends keyof UpdateUserParams>(key: K, value: UpdateUserParams[K]) => setEditForm((prev) => ({ ...prev, [key]: value }));
 return (
  <div className={cn('space-y-6 p-5 sm:p-8')}>
   <div className={cn('grid grid-cols-1 gap-6 md:grid-cols-2')}>
    <label className={cn('space-y-3 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>{t('users.edit_user.role_label')}</span>
     <select value={editForm.role} onChange={(event) => update('role', event.target.value as UpdateUserParams['role'])} className={cn(SELECT_CLASS)}>
      <option value="user">{t('users.edit_user.role_user')}</option>
      <option value="tarot_reader">{t('users.edit_user.role_reader')}</option>
      <option value="admin">{t('users.edit_user.role_admin')}</option>
     </select>
     {getErrorMessage(editFormErrors, 'role') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(editFormErrors, 'role')}</p> : null}
    </label>
    <label className={cn('space-y-3 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>{t('users.edit_user.status_label')}</span>
     <select value={editForm.status} onChange={(event) => update('status', event.target.value as UpdateUserParams['status'])} className={cn(SELECT_CLASS)}>
      <option value="active">{t('users.edit_user.status_active')}</option>
      <option value="locked">{t('users.edit_user.status_locked')}</option>
     </select>
     {getErrorMessage(editFormErrors, 'status') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(editFormErrors, 'status')}</p> : null}
    </label>
   </div>
   <div className={cn('grid grid-cols-1 gap-6 border-t pt-4 tn-border-soft md:grid-cols-2')}>
    <label className={cn('space-y-3 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-warning flex items-center gap-1.5 align-middle')}><Coins className={cn('w-3.5 h-3.5')} /> {t('users.edit_user.gold_balance_label')}</span>
     <input type="number" min={0} value={editForm.goldBalance} onChange={(event) => update('goldBalance', Number(event.target.value))} className={cn('w-full tn-field tn-field-accent tn-border-warning-30 tn-text-warning rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter')} />
     {getErrorMessage(editFormErrors, 'goldBalance') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(editFormErrors, 'goldBalance')}</p> : null}
    </label>
    <label className={cn('space-y-3 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent flex items-center gap-1.5 align-middle')}><Gem className={cn('w-3.5 h-3.5')} /> {t('users.edit_user.diamond_balance_label')}</span>
     <input type="number" min={0} value={editForm.diamondBalance} onChange={(event) => update('diamondBalance', Number(event.target.value))} className={cn('w-full tn-field tn-field-accent tn-border-accent-30 tn-text-accent rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter')} />
     {getErrorMessage(editFormErrors, 'diamondBalance') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(editFormErrors, 'diamondBalance')}</p> : null}
    </label>
   </div>
   <AdminUsersLedgerNotice />
  </div>
 );
}

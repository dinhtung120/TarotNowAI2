'use client';

import { cn } from '@/lib/utils';
import type { FieldErrors } from 'react-hook-form';
import type { CreateUserParams } from '@/features/admin/application/actions';
import type { AdminUsersTranslateFn, SetAddUserForm } from '../types';

interface AdminUsersAddFieldsProps {
 addForm: CreateUserParams;
 addFormErrors: FieldErrors<CreateUserParams>;
 setAddForm: SetAddUserForm;
 t: AdminUsersTranslateFn;
}

const FIELD_CLASS = 'w-full tn-field tn-field-accent tn-border-soft tn-text-primary rounded-xl px-4 py-3 tn-surface font-bold shadow-inner';

function getErrorMessage(errors: FieldErrors<CreateUserParams>, field: keyof CreateUserParams): string {
 const value = errors[field]?.message;
 return typeof value === 'string' ? value : '';
}

export function AdminUsersAddFields({ addForm, addFormErrors, setAddForm, t }: AdminUsersAddFieldsProps) {
 const update = <K extends keyof CreateUserParams>(key: K, value: CreateUserParams[K]) =>
  setAddForm((prev) => ({ ...prev, [key]: value }));
 return (
  <div className={cn('p-8 space-y-5')}>
   <div className={cn('tn-grid-cols-1-2-md gap-5')}>
    <label className={cn('space-y-2 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>{t('users.add_user.email_label')}</span>
     <input type="email" value={addForm.email} onChange={(event) => update('email', event.target.value)} placeholder={t('users.add_user.email_placeholder')} className={cn(FIELD_CLASS)} />
     {getErrorMessage(addFormErrors, 'email') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(addFormErrors, 'email')}</p> : null}
    </label>
    <label className={cn('space-y-2 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>{t('users.add_user.username_label')}</span>
     <input type="text" value={addForm.username} onChange={(event) => update('username', event.target.value)} placeholder={t('users.add_user.username_placeholder')} className={cn(FIELD_CLASS)} />
     {getErrorMessage(addFormErrors, 'username') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(addFormErrors, 'username')}</p> : null}
    </label>
   </div>
   <div className={cn('tn-grid-cols-1-2-md gap-5')}>
    <label className={cn('space-y-2 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>{t('users.add_user.display_name_label')}</span>
     <input type="text" value={addForm.displayName} onChange={(event) => update('displayName', event.target.value)} placeholder={t('users.add_user.display_name_placeholder')} className={cn(FIELD_CLASS)} />
     {getErrorMessage(addFormErrors, 'displayName') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(addFormErrors, 'displayName')}</p> : null}
    </label>
    <label className={cn('space-y-2 block')}>
     <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>{t('users.add_user.password_label')}</span>
     <input type="password" value={addForm.password} onChange={(event) => update('password', event.target.value)} placeholder={t('users.add_user.password_placeholder')} className={cn(FIELD_CLASS)} />
     {getErrorMessage(addFormErrors, 'password') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(addFormErrors, 'password')}</p> : null}
    </label>
   </div>
   <label className={cn('space-y-2 block')}>
    <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>{t('users.add_user.role_label')}</span>
    <select value={addForm.role} onChange={(event) => update('role', event.target.value as CreateUserParams['role'])} className={cn(FIELD_CLASS)}>
     <option value="user">{t('users.roles.user')}</option>
     <option value="tarot_reader">{t('users.roles.tarot_reader')}</option>
     <option value="admin">{t('users.roles.admin')}</option>
    </select>
    {getErrorMessage(addFormErrors, 'role') ? <p className={cn('text-xs font-semibold text-red-300')}>{getErrorMessage(addFormErrors, 'role')}</p> : null}
   </label>
  </div>
 );
}

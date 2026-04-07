'use client';

import { cn } from '@/lib/utils';
import type { CreateUserParams } from '@/features/admin/application/actions';
import type { AdminUsersTranslateFn, SetAddUserForm } from '../types';

interface AdminUsersAddFieldsProps {
 addForm: CreateUserParams;
 setAddForm: SetAddUserForm;
 t: AdminUsersTranslateFn;
}

const FIELD_CLASS = 'w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold shadow-inner';

export function AdminUsersAddFields({ addForm, setAddForm, t }: AdminUsersAddFieldsProps) {
 const update = (key: keyof CreateUserParams, value: string) => setAddForm((prev) => ({ ...prev, [key]: value }));
 return (
  <div className={cn('p-8 space-y-5')}>
   <div className={cn('grid grid-cols-1 md:grid-cols-2 gap-5')}>
    <label className={cn('space-y-2 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left')}>{t('users.add_user.email_label')}</span><input type="email" value={addForm.email} onChange={(event) => update('email', event.target.value)} placeholder={t('users.add_user.email_placeholder')} className={cn(FIELD_CLASS)} /></label>
    <label className={cn('space-y-2 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left')}>{t('users.add_user.username_label')}</span><input type="text" value={addForm.username} onChange={(event) => update('username', event.target.value)} placeholder={t('users.add_user.username_placeholder')} className={cn(FIELD_CLASS)} /></label>
   </div>
   <div className={cn('grid grid-cols-1 md:grid-cols-2 gap-5')}>
    <label className={cn('space-y-2 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left')}>{t('users.add_user.display_name_label')}</span><input type="text" value={addForm.displayName} onChange={(event) => update('displayName', event.target.value)} placeholder={t('users.add_user.display_name_placeholder')} className={cn(FIELD_CLASS)} /></label>
    <label className={cn('space-y-2 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left')}>{t('users.add_user.password_label')}</span><input type="password" value={addForm.password} onChange={(event) => update('password', event.target.value)} placeholder={t('users.add_user.password_placeholder')} className={cn(FIELD_CLASS)} /></label>
   </div>
   <label className={cn('space-y-2 block')}>
    <span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left')}>{t('users.add_user.role_label')}</span>
    <select value={addForm.role} onChange={(event) => update('role', event.target.value)} className={cn(FIELD_CLASS)}>
     <option value="user">{t('users.roles.user')}</option>
     <option value="tarot_reader">{t('users.roles.tarot_reader')}</option>
     <option value="admin">{t('users.roles.admin')}</option>
    </select>
   </label>
  </div>
 );
}

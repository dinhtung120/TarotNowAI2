'use client';

import { Coins, Gem } from 'lucide-react';
import { cn } from '@/lib/utils';
import type { UpdateUserParams } from '@/features/admin/application/actions';
import type { SetEditUserForm } from '../types';
import { AdminUsersLedgerNotice } from './AdminUsersLedgerNotice';

interface AdminUsersEditFieldsProps {
 editForm: UpdateUserParams;
 setEditForm: SetEditUserForm;
}

const SELECT_CLASS = 'w-full tn-field tn-field-accent tn-border-soft tn-text-primary rounded-xl px-4 py-3 tn-surface font-bold italic shadow-inner';

export function AdminUsersEditFields({ editForm, setEditForm }: AdminUsersEditFieldsProps) {
 const update = <K extends keyof UpdateUserParams>(key: K, value: UpdateUserParams[K]) => setEditForm((prev) => ({ ...prev, [key]: value }));
 return (
  <div className={cn('space-y-6 p-5 sm:p-8')}>
   <div className={cn('grid grid-cols-1 gap-6 md:grid-cols-2')}>
    <label className={cn('space-y-3 block')}><span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>Chức Vụ Hệ Thống</span><select value={editForm.role} onChange={(event) => update('role', event.target.value)} className={cn(SELECT_CLASS)}><option value="user">USER (Khách)</option><option value="tarot_reader">TAROT READER (Thầy)</option><option value="admin">ADMIN (Quản trị)</option></select></label>
    <label className={cn('space-y-3 block')}><span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-secondary block text-left')}>Trạng Thái (Status)</span><select value={editForm.status} onChange={(event) => update('status', event.target.value)} className={cn(SELECT_CLASS)}><option value="active">ACTIVE (Đang Hoạt Động)</option><option value="locked">LOCKED (Bị Khóa Cấm)</option></select></label>
   </div>
   <div className={cn('grid grid-cols-1 gap-6 border-t pt-4 tn-border-soft md:grid-cols-2')}>
    <label className={cn('space-y-3 block')}><span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-warning flex items-center gap-1.5 align-middle')}><Coins className={cn('w-3.5 h-3.5')} /> Vàng Hiện Có</span><input type="number" value={editForm.goldBalance} onChange={(event) => update('goldBalance', Number(event.target.value))} className={cn('w-full tn-field tn-field-accent tn-border-warning-30 tn-text-warning rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter')} /></label>
    <label className={cn('space-y-3 block')}><span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent flex items-center gap-1.5 align-middle')}><Gem className={cn('w-3.5 h-3.5')} /> KC Hiện Có</span><input type="number" value={editForm.diamondBalance} onChange={(event) => update('diamondBalance', Number(event.target.value))} className={cn('w-full tn-field tn-field-accent tn-border-accent-30 tn-text-accent rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter')} /></label>
   </div>
   <AdminUsersLedgerNotice />
  </div>
 );
}

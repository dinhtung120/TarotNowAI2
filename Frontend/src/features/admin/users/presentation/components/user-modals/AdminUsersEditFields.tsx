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

const SELECT_CLASS = 'w-full tn-field tn-field-accent border-[var(--text-tertiary)]/20 tn-text-primary rounded-xl px-4 py-3 bg-[var(--surface-color)] font-bold italic shadow-inner';

export function AdminUsersEditFields({ editForm, setEditForm }: AdminUsersEditFieldsProps) {
 const update = <K extends keyof UpdateUserParams>(key: K, value: UpdateUserParams[K]) => setEditForm((prev) => ({ ...prev, [key]: value }));
 return (
  <div className={cn('p-8 space-y-6')}>
   <div className={cn('grid grid-cols-2 gap-6')}>
    <label className={cn('space-y-3 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left')}>Chức Vụ Hệ Thống</span><select value={editForm.role} onChange={(event) => update('role', event.target.value)} className={cn(SELECT_CLASS)}><option value="user">USER (Khách)</option><option value="tarot_reader">TAROT READER (Thầy)</option><option value="admin">ADMIN (Quản trị)</option></select></label>
    <label className={cn('space-y-3 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] block text-left')}>Trạng Thái (Status)</span><select value={editForm.status} onChange={(event) => update('status', event.target.value)} className={cn(SELECT_CLASS)}><option value="active">ACTIVE (Đang Hoạt Động)</option><option value="locked">LOCKED (Bị Khóa Cấm)</option></select></label>
   </div>
   <div className={cn('grid grid-cols-2 gap-6 pt-4 border-t tn-border-soft')}>
    <label className={cn('space-y-3 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--warning)] flex items-center gap-1.5 align-middle')}><Coins className={cn('w-3.5 h-3.5')} /> Vàng Hiện Có</span><input type="number" value={editForm.goldBalance} onChange={(event) => update('goldBalance', Number(event.target.value))} className={cn('w-full tn-field tn-field-accent border-[var(--warning)]/30 text-[var(--warning)] rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter')} /></label>
    <label className={cn('space-y-3 block')}><span className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] flex items-center gap-1.5 align-middle')}><Gem className={cn('w-3.5 h-3.5')} /> KC Hiện Có</span><input type="number" value={editForm.diamondBalance} onChange={(event) => update('diamondBalance', Number(event.target.value))} className={cn('w-full tn-field tn-field-accent border-[var(--purple-accent)]/30 text-[var(--purple-accent)] rounded-2xl px-5 py-3 text-lg font-black italic tracking-tighter')} /></label>
   </div>
   <AdminUsersLedgerNotice />
  </div>
 );
}

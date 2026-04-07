'use client';

import { ShieldCheck } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderRequestAdminNoteProps {
 note: string;
 title: string;
}

export function ReaderRequestAdminNote({
 note,
 title,
}: ReaderRequestAdminNoteProps) {
 if (!note) return null;
 return (
  <div className={cn('p-4 rounded-2xl bg-[var(--purple-accent)]/5 border border-[var(--purple-accent)]/20 shadow-inner')}>
   <div className={cn('text-[10px] font-black uppercase tracking-widest text-[var(--purple-accent)] mb-2 flex items-center gap-2')}>
    <ShieldCheck className={cn('w-3.5 h-3.5')} />
    {title}
   </div>
   <p className={cn('text-xs font-medium text-[var(--text-secondary)]')}>{note}</p>
  </div>
 );
}

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
  <div className={cn('p-4 rounded-2xl tn-bg-accent-5 border tn-border-accent-20 shadow-inner')}>
   <div className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent mb-2 flex items-center gap-2')}>
    <ShieldCheck className={cn('w-3.5 h-3.5')} />
    {title}
   </div>
   <p className={cn('text-xs font-medium tn-text-secondary')}>{note}</p>
  </div>
 );
}

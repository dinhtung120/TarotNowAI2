'use client';

import { ArrowLeft } from 'lucide-react';
import { cn } from '@/lib/utils';

interface ReaderProfileBackButtonProps {
 label: string;
 onBack: () => void;
}

export function ReaderProfileBackButton({
 label,
 onBack,
}: ReaderProfileBackButtonProps) {
 return (
  <button type="button" onClick={onBack} className={cn('flex items-center gap-2 text-[11px] font-black uppercase tracking-widest text-[var(--text-secondary)] hover:tn-text-primary transition-colors group')}>
   <ArrowLeft className={cn('w-4 h-4 text-[var(--purple-accent)] group-hover:-translate-x-1 transition-transform')} />
   <span>{label}</span>
  </button>
 );
}

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
  <button type="button" onClick={onBack} className={cn('flex items-center gap-2 tn-text-11 font-black uppercase tracking-widest tn-text-secondary tn-hover-text-primary transition-colors group')}>
   <ArrowLeft className={cn('w-4 h-4 tn-wallet-back-icon tn-group-shift-left-1')} />
   <span>{label}</span>
  </button>
 );
}

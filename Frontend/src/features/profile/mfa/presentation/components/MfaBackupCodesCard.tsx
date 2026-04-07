'use client';

import { AlertTriangle } from 'lucide-react';
import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface MfaBackupCodesCardProps {
 backupCodes: string[];
 subtitle: string;
 title: string;
}

export function MfaBackupCodesCard({
 backupCodes,
 subtitle,
 title,
}: MfaBackupCodesCardProps) {
 return (
  <GlassCard className={cn('!p-8 !bg-[var(--danger)]/5 border-[var(--danger)]/20 shadow-[0_0_40px_var(--c-239-68-68-05)] text-center space-y-8')}>
   <div className={cn('space-y-3')}>
    <h3 className={cn('text-xl font-black uppercase italic tracking-tight text-[var(--danger)] flex items-center justify-center gap-2')}>
     <AlertTriangle className={cn('w-5 h-5')} />
     {title}
    </h3>
    <p className={cn('text-[var(--danger)]/80 text-sm font-medium')}>{subtitle}</p>
   </div>
   <div className={cn('grid grid-cols-2 gap-4 max-w-sm mx-auto')}>
    {backupCodes.map((code, idx) => (
     <div key={idx} className={cn('tn-surface border border-[var(--danger)]/20 py-3 rounded-xl text-center font-mono tn-text-secondary font-bold select-all cursor-text shadow-inner')}>
      {code}
     </div>
    ))}
   </div>
  </GlassCard>
 );
}

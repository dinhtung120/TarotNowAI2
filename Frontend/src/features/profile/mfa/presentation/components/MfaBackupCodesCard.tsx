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
  <GlassCard className={cn('tn-mfa-backup-card !p-6 text-center space-y-8 sm:!p-8')}>
   <div className={cn('space-y-3')}>
    <h3 className={cn('text-xl font-black uppercase italic tracking-tight tn-text-danger flex items-center justify-center gap-2')}>
     <AlertTriangle className={cn('w-5 h-5')} />
     {title}
    </h3>
    <p className={cn('tn-text-danger-80 text-sm font-medium')}>{subtitle}</p>
   </div>
   <div className={cn('mx-auto grid max-w-sm grid-cols-1 gap-4 sm:grid-cols-2')}>
    {backupCodes.map((code, idx) => (
     <div key={idx} className={cn('tn-surface border tn-border-danger py-3 rounded-xl text-center font-mono tn-text-secondary font-bold select-all cursor-text shadow-inner')}>
      {code}
     </div>
    ))}
   </div>
  </GlassCard>
 );
}

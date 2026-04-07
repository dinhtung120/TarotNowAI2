'use client';

import type { FormEvent } from 'react';
import { AlertTriangle, CheckCircle2, Loader2 } from 'lucide-react';
import { Button, GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface MfaVerifyCodeCardProps {
 code: string;
 codePlaceholder: string;
 errorMessage: string;
 onChangeCode: (value: string) => void;
 onSubmit: (event: FormEvent) => void;
 subtitle: string;
 title: string;
 verifyLabel: string;
 verifyLoading: boolean;
}

export function MfaVerifyCodeCard({
 code,
 codePlaceholder,
 errorMessage,
 onChangeCode,
 onSubmit,
 subtitle,
 title,
 verifyLabel,
 verifyLoading,
}: MfaVerifyCodeCardProps) {
 return (
  <GlassCard className={cn('!p-8 text-center space-y-8')}>
   <div className={cn('space-y-3')}>
    <h3 className={cn('text-xl font-black tn-text-primary uppercase italic tracking-tight')}>{title}</h3>
    <p className={cn('text-[var(--text-secondary)] text-sm font-medium')}>{subtitle}</p>
   </div>
   <form onSubmit={onSubmit} className={cn('space-y-8')}>
    <input type="text" value={code} onChange={(event) => onChangeCode(event.target.value.replace(/\D/g, '').slice(0, 6))} placeholder={codePlaceholder} className={cn('w-full max-w-[240px] mx-auto block text-center text-4xl tracking-widest font-mono py-6 tn-field rounded-2xl tn-text-primary tn-field-success transition-all placeholder:text-[color:var(--c-154-144-171-58)] shadow-inner')} autoFocus />
    {errorMessage ? <div className={cn('text-[var(--danger)] text-xs font-bold uppercase tracking-widest flex justify-center items-center gap-2 bg-[var(--danger-bg)] border border-[var(--danger)]/30 p-4 rounded-xl max-w-xs mx-auto')}><AlertTriangle className={cn('w-4 h-4')} /> {errorMessage}</div> : null}
    <Button variant="primary" type="submit" disabled={code.length !== 6 || verifyLoading} className={cn('w-full max-w-[240px] mx-auto h-14 !bg-[var(--success)] hover:!bg-[var(--success)]/80 tn-text-primary border-none shadow-[0_0_20px_var(--c-16-185-129-30)] disabled:opacity-50 disabled:shadow-none')}>
     {verifyLoading ? <Loader2 className={cn('w-5 h-5 animate-spin mr-2')} /> : <CheckCircle2 className={cn('w-5 h-5 mr-2')} />}
     {verifyLabel}
    </Button>
   </form>
  </GlassCard>
 );
}

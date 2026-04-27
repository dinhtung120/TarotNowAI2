'use client';

import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { AlertTriangle, CheckCircle2, Loader2 } from 'lucide-react';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { Button, GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import type { TypedSubmitHandler } from '@/shared/application/utils/typedSubmit';

interface MfaVerifyCodeCardProps {
 code: string;
 codePlaceholder: string;
 errorMessage: string;
 onChangeCode: (value: string) => void;
 onSubmit: TypedSubmitHandler<{ code: string }>;
 subtitle: string;
 title: string;
 verifyLabel: string;
 verifyLoading: boolean;
}

const mfaVerifyCodeCardSchema = z.object({
 code: z.string().regex(/^\d{6}$/),
});

type MfaVerifyCodeCardFormValues = z.infer<typeof mfaVerifyCodeCardSchema>;

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
 const { handleSubmit, setValue, control } = useForm<MfaVerifyCodeCardFormValues>({
  resolver: zodResolver(mfaVerifyCodeCardSchema),
  defaultValues: {
   code,
  },
 });

 const watchedCode = useWatch({ control, name: 'code' }) ?? '';

 useEffect(() => {
  setValue('code', code, { shouldDirty: false, shouldValidate: false });
 }, [code, setValue]);

 useEffect(() => {
  onChangeCode(watchedCode);
 }, [onChangeCode, watchedCode]);

 const submitWithValidation = handleSubmit(async (values) => {
  await onSubmit({ code: values.code });
 });

 return (
  <GlassCard className={cn('!p-8 text-center space-y-8')}>
   <div className={cn('space-y-3')}>
   <h3 className={cn('text-xl font-black tn-text-primary uppercase italic tracking-tight')}>{title}</h3>
   <p className={cn('tn-text-secondary text-sm font-medium')}>{subtitle}</p>
  </div>
   <form onSubmit={submitWithValidation} className={cn('space-y-8')}>
    <input type="text" value={watchedCode} onChange={(event) => setValue('code', event.target.value.replace(/\D/g, '').slice(0, 6), { shouldDirty: true, shouldValidate: true })} placeholder={codePlaceholder} className={cn('w-full tn-maxw-240 mx-auto block text-center text-4xl tracking-widest font-mono py-6 tn-field rounded-2xl tn-text-primary tn-field-success transition-all tn-placeholder shadow-inner')} autoFocus />
    {errorMessage ? <div className={cn('tn-text-danger text-xs font-bold uppercase tracking-widest flex justify-center items-center gap-2 tn-bg-danger-soft border tn-border-danger p-4 rounded-xl max-w-xs mx-auto')}><AlertTriangle className={cn('w-4 h-4')} /> {errorMessage}</div> : null}
    <Button variant="primary" type="submit" disabled={watchedCode.length !== 6 || verifyLoading} className={cn('tn-mfa-verify-submit w-full tn-maxw-240 mx-auto h-14 tn-text-primary border-none')}>
     {verifyLoading ? <Loader2 className={cn('w-5 h-5 animate-spin mr-2')} /> : <CheckCircle2 className={cn('w-5 h-5 mr-2')} />}
     {verifyLabel}
    </Button>
   </form>
  </GlassCard>
 );
}

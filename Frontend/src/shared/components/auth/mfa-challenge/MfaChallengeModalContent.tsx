import { useEffect } from 'react';
import type { FormEvent } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import { Loader2, ShieldAlert } from 'lucide-react';
import { useForm, useWatch } from 'react-hook-form';
import { z } from 'zod';
import { cn } from '@/lib/utils';

interface MfaChallengeModalContentProps {
  actionTitleText: string;
  code: string;
  error: string;
  loading: boolean;
  title: string;
  description: string;
  placeholder: string;
  submitLabel: string;
  onCodeChange: (value: string) => void;
  onSubmit: (event: FormEvent) => Promise<void>;
}

const mfaChallengeModalContentSchema = z.object({
  code: z.string().regex(/^\d{6}$/),
});

type MfaChallengeModalContentFormValues = z.infer<typeof mfaChallengeModalContentSchema>;

export default function MfaChallengeModalContent({ actionTitleText, code, error, loading, title, description, placeholder, submitLabel, onCodeChange, onSubmit }: MfaChallengeModalContentProps) {
  const { handleSubmit, setValue, control } = useForm<MfaChallengeModalContentFormValues>({
    resolver: zodResolver(mfaChallengeModalContentSchema),
    defaultValues: {
      code,
    },
  });
  const watchedCode = useWatch({ control, name: 'code' }) ?? '';

  useEffect(() => {
    setValue('code', code, { shouldDirty: false, shouldValidate: false });
  }, [code, setValue]);

  useEffect(() => {
    onCodeChange(watchedCode);
  }, [onCodeChange, watchedCode]);

  const submitWithValidation = handleSubmit(async () => {
    await onSubmit({
      preventDefault: () => undefined,
      stopPropagation: () => undefined,
    } as unknown as FormEvent);
  });

 return (
  <div className={cn('space-y-6 text-center')}>
   <div className={cn('tn-mfa-challenge-icon-shell mx-auto flex h-16 w-16 items-center justify-center rounded-full')}><ShieldAlert className={cn('h-8 w-8 tn-text-success')} /></div>
   <div className={cn('space-y-2')}><h3 className={cn('text-xl font-black tn-text-primary')}>{title}</h3><p className={cn('text-sm tn-text-secondary')}>{description.replace('{actionTitle}', actionTitleText)}</p></div>
   <form onSubmit={submitWithValidation} className={cn('space-y-4')}>
    <input type="text" value={watchedCode} onChange={(event) => setValue('code', event.target.value.replace(/\D/g, '').slice(0, 6), { shouldDirty: true, shouldValidate: true })} placeholder={placeholder} className={cn('w-full rounded-2xl py-4 text-center font-mono text-3xl tn-tracking-05 tn-text-success tn-field tn-field-success transition-colors tn-placeholder')} autoFocus />
    {error ? <p className={cn('text-xs tn-text-danger')}>{error}</p> : null}
    <button type="submit" disabled={watchedCode.length !== 6 || loading} className={cn('tn-mfa-challenge-submit flex h-12 w-full items-center justify-center gap-2 rounded-xl text-xs font-black uppercase tracking-widest tn-text-primary transition-all')}>
     {loading ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : submitLabel}
    </button>
   </form>
    </div>
  );
}

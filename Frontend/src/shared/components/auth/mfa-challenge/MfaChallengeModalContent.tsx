import { Loader2, ShieldAlert } from 'lucide-react';
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
  onSubmit: (event: React.FormEvent) => Promise<void>;
}

export default function MfaChallengeModalContent({ actionTitleText, code, error, loading, title, description, placeholder, submitLabel, onCodeChange, onSubmit }: MfaChallengeModalContentProps) {
  return (
    <div className={cn('space-y-6 text-center')}>
      <div className={cn('mx-auto flex h-16 w-16 items-center justify-center rounded-full border border-[var(--success)]/20 bg-[var(--success)]/10')}><ShieldAlert className={cn('h-8 w-8 text-[var(--success)]')} /></div>
      <div className={cn('space-y-2')}><h3 className={cn('text-xl font-black tn-text-primary')}>{title}</h3><p className={cn('text-sm tn-text-secondary')}>{description.replace('{actionTitle}', actionTitleText)}</p></div>
      <form onSubmit={(event) => void onSubmit(event)} className={cn('space-y-4')}>
        <input type="text" value={code} onChange={(event) => onCodeChange(event.target.value.replace(/\D/g, '').slice(0, 6))} placeholder={placeholder} className={cn('w-full rounded-2xl py-4 text-center font-mono text-3xl tracking-[0.5em] text-[var(--success)] tn-field tn-field-success transition-colors placeholder:tn-text-muted')} autoFocus />
        {error ? <p className={cn('text-xs text-[var(--danger)]')}>{error}</p> : null}
        <button type="submit" disabled={code.length !== 6 || loading} className={cn('flex h-12 w-full items-center justify-center gap-2 rounded-xl bg-[var(--success)] text-xs font-black uppercase tracking-widest tn-text-primary transition-all disabled:tn-surface-strong disabled:opacity-50')}>
          {loading ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : submitLabel}
        </button>
      </form>
    </div>
  );
}

import { cn } from '@/lib/utils';

interface InputFieldMetaProps {
  error?: string;
  hint?: string;
  label?: string;
}

export default function InputFieldMeta({ error, hint, label }: InputFieldMetaProps) {
  return (
    <>
      {label ? <label className={cn('ml-1 block text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)]')}>{label}</label> : null}
      {error ? <p className={cn('ml-1 text-[11px] font-medium text-[var(--danger)]')}>{error}</p> : hint ? <p className={cn('ml-1 text-[11px] font-medium text-[var(--text-muted)]')}>{hint}</p> : null}
    </>
  );
}

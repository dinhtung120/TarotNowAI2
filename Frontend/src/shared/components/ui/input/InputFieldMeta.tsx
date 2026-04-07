import { cn } from '@/lib/utils';

interface InputFieldMetaProps {
  error?: string;
  hint?: string;
  label?: string;
}

export default function InputFieldMeta({ error, hint, label }: InputFieldMetaProps) {
  return (
    <>
      {label ? <label className={cn('ml-1 block tn-text-10 font-black uppercase tn-tracking-02 tn-text-secondary')}>{label}</label> : null}
      {error ? <p className={cn('ml-1 tn-text-11 font-medium tn-text-danger')}>{error}</p> : hint ? <p className={cn('ml-1 tn-text-11 font-medium tn-text-muted')}>{hint}</p> : null}
    </>
  );
}

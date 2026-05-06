'use client';

import { cn } from '@/lib/utils';

interface AdminDisputeNoteInputProps {
 note: string;
 onChange: (value: string) => void;
 placeholder: string;
}

export function AdminDisputeNoteInput({
 note,
 onChange,
 placeholder,
}: AdminDisputeNoteInputProps) {
 return (
  <textarea
   value={note}
   onChange={(event) => onChange(event.target.value)}
   rows={2}
   placeholder={placeholder}
   className={cn('w-full rounded-xl bg-white/5 border border-white/10 px-3 py-2 text-sm tn-text-primary')}
  />
 );
}

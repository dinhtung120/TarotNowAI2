'use client';

import { cn } from '@/lib/utils';

interface InlineErrorAlertProps {
 message?: string | null;
 title?: string;
 className?: string;
}

export default function InlineErrorAlert({ message, title, className }: InlineErrorAlertProps) {
 if (!message) {
  return null;
 }

 return (
  <div className={cn('rounded-2xl border border-red-500/20 bg-red-500/5 px-6 py-4 text-sm tn-text-danger', className)}>
   {title ? <p className={cn('mb-1 font-bold uppercase tracking-widest')}>{title}</p> : null}
   {message}
  </div>
 );
}

'use client';

import { Loader2, Send } from 'lucide-react';
import type { UseFormRegisterReturn } from 'react-hook-form';
import { cn } from '@/lib/utils';

interface CommentComposerProps {
 content: string;
 isAuthenticated: boolean;
 isPending: boolean;
 loginRequiredLabel: string;
 placeholder: string;
 registerField: UseFormRegisterReturn;
 submit: () => void;
}

export function CommentComposer({
 content,
 isAuthenticated,
 isPending,
 loginRequiredLabel,
 placeholder,
 registerField,
 submit,
}: CommentComposerProps) {
 if (!isAuthenticated) {
  return <div className={cn('text-center py-3 text-xs text-gray-500 tn-surface rounded-xl border tn-border-soft')}>{loginRequiredLabel}</div>;
 }

 return (
  <form onSubmit={(event) => { event.preventDefault(); submit(); }} className={cn('flex gap-2 items-end relative')}>
   <textarea {...registerField} placeholder={placeholder} maxLength={1000} className={cn('flex-1 tn-field rounded-xl px-4 py-3 text-sm text-gray-200 resize-none h-12 min-h-12 custom-scrollbar transition-all')} rows={1} disabled={isPending} onKeyDown={(event) => { if (event.key === 'Enter' && !event.shiftKey) { event.preventDefault(); submit(); } }} />
   {content.length > 800 ? <span className={cn('absolute -top-5 right-14 text-xs', content.length >= 1000 ? 'text-red-400' : 'text-gray-500')}>{content.length}/1000</span> : null}
   <button type="submit" disabled={!content.trim() || isPending} className={cn('tn-size-46 shrink-0 rounded-xl bg-gradient-to-tr from-violet-600 to-indigo-900 flex items-center justify-center text-white transition-opacity tn-disabled-dim')}>
    {isPending ? <Loader2 className={cn('w-5 h-5 animate-spin')} /> : <Send className={cn('w-5 h-5 ml-1')} />}
   </button>
  </form>
 );
}

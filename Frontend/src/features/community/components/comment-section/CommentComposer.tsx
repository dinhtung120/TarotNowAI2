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
  return <div className={cn('text-center py-3 text-xs text-gray-500 bg-[#0f0f16] rounded-xl border border-[#2a2b3d]/50')}>{loginRequiredLabel}</div>;
 }

 return (
  <form onSubmit={(event) => { event.preventDefault(); submit(); }} className={cn('flex gap-2 items-end relative')}>
   <textarea {...registerField} placeholder={placeholder} maxLength={1000} className={cn('flex-1 bg-[#0f0f16] border border-[#2a2b3d] rounded-xl px-4 py-3 text-sm text-gray-200 focus:outline-none focus:border-[#8a2be2] resize-none h-[46px] min-h-[46px] custom-scrollbar focus:ring-1 focus:ring-[#8a2be2]/50 transition-all')} rows={1} disabled={isPending} onKeyDown={(event) => { if (event.key === 'Enter' && !event.shiftKey) { event.preventDefault(); submit(); } }} />
   {content.length > 800 ? <span className={cn('absolute -top-5 right-14 text-xs', content.length >= 1000 ? 'text-red-400' : 'text-gray-500')}>{content.length}/1000</span> : null}
   <button type="submit" disabled={!content.trim() || isPending} className={cn('w-[46px] h-[46px] shrink-0 rounded-xl bg-gradient-to-tr from-[#8a2be2] to-[#4b0082] flex items-center justify-center text-white hover:opacity-90 transition-opacity disabled:opacity-50 disabled:cursor-not-allowed')}>
    {isPending ? <Loader2 className={cn('w-5 h-5 animate-spin')} /> : <Send className={cn('w-5 h-5 ml-1')} />}
   </button>
  </form>
 );
}

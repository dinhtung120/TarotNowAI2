'use client';

import { useRef, type ChangeEvent } from 'react';
import { Image as ImageIcon, Loader2, Send } from 'lucide-react';
import type { UseFormRegisterReturn } from 'react-hook-form';
import { cn } from '@/lib/utils';

interface CommentComposerProps {
  attachImageLabel: string;
  content: string;
  isAuthenticated: boolean;
  isPending: boolean;
  isUploadingImage: boolean;
  loginRequiredLabel: string;
  onAttachImage: (file: File) => Promise<void>;
  placeholder: string;
  registerField: UseFormRegisterReturn;
  submit: () => void;
  uploadingProgressLabel: string;
}

export function CommentComposer({
  attachImageLabel,
  content,
  isAuthenticated,
  isPending,
  isUploadingImage,
  loginRequiredLabel,
  onAttachImage,
  placeholder,
  registerField,
  submit,
  uploadingProgressLabel,
}: CommentComposerProps) {
  const fileInputRef = useRef<HTMLInputElement>(null);

  if (!isAuthenticated) {
    return <div className={cn('text-center py-3 text-xs text-gray-500 tn-surface rounded-xl border tn-border-soft')}>{loginRequiredLabel}</div>;
  }

  const handleFileChange = async (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    event.target.value = '';
    if (!file) {
      return;
    }

    await onAttachImage(file);
  };

  return (
    <form onSubmit={(event) => { event.preventDefault(); submit(); }} className={cn('relative mt-2 flex items-end gap-2')}>
      <textarea
        {...registerField}
        placeholder={placeholder}
        maxLength={1000}
        className={cn('flex-1 tn-field rounded-xl px-4 py-3 text-sm text-gray-200 resize-none h-12 min-h-12 custom-scrollbar transition-all')}
        rows={1}
        disabled={isPending}
        onKeyDown={(event) => {
          if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
            submit();
          }
        }}
      />
      <button
        type="button"
        disabled={isPending || isUploadingImage}
        onClick={() => fileInputRef.current?.click()}
        className={cn('flex h-11 w-11 shrink-0 items-center justify-center rounded-xl border border-slate-600 bg-slate-900 text-slate-200 transition-opacity tn-disabled-dim')}
        title={attachImageLabel}
      >
        {isUploadingImage ? <Loader2 className={cn('h-4 w-4 animate-spin')} /> : <ImageIcon className={cn('h-4 w-4')} />}
      </button>
      <input ref={fileInputRef} type="file" accept="image/*" className={cn('hidden')} onChange={(event) => void handleFileChange(event)} />
      <button type="submit" disabled={!content.trim() || isPending || isUploadingImage} className={cn('tn-size-46 shrink-0 rounded-xl bg-gradient-to-tr from-violet-600 to-indigo-900 flex items-center justify-center text-white transition-opacity tn-disabled-dim')}>
        {isPending ? <Loader2 className={cn('w-5 h-5 animate-spin')} /> : <Send className={cn('w-5 h-5 ml-1')} />}
      </button>
      {content.length > 800 ? <span className={cn('absolute -top-5 right-14 text-xs', content.length >= 1000 ? 'text-red-400' : 'text-gray-500')}>{content.length}/1000</span> : null}
      {isUploadingImage ? <span className={cn('absolute -bottom-5 left-1 text-[11px] text-gray-400')}>{uploadingProgressLabel}</span> : null}
    </form>
  );
}

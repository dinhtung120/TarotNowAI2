'use client';

import type { ChangeEvent, RefObject } from 'react';
import { Image as ImageIcon, Loader2 } from 'lucide-react';
import type { UseFormRegisterReturn } from 'react-hook-form';
import { cn } from '@/lib/utils';

interface PostComposerActionsProps {
 content: string;
 fileInputRef: RefObject<HTMLInputElement | null>;
 isPending: boolean;
 isUploading: boolean;
 onChangeImage: (event: ChangeEvent<HTMLInputElement>) => void;
 t: (key: string, values?: Record<string, string | number | Date>) => string;
 visibilityField: UseFormRegisterReturn;
}

export function PostComposerActions({
 content,
 fileInputRef,
 isPending,
 isUploading,
 onChangeImage,
 t,
 visibilityField,
}: PostComposerActionsProps) {
 return (
  <div className={cn('flex justify-between items-center mt-3')}>
   <div className={cn('flex items-center gap-4')}>
    <select {...visibilityField} className={cn('tn-surface tn-text-secondary text-sm border-none outline-none rounded-md py-1 px-2 cursor-pointer')} disabled={isPending}>
     <option value="public">{t('composer.visibility_public')}</option>
     <option value="private">{t('composer.visibility_private')}</option>
    </select>
    <button type="button" disabled={isUploading || isPending} onClick={() => fileInputRef.current?.click()} className={cn('tn-text-accent tn-hover-text-primary transition-colors p-2 rounded-full tn-hover-surface-soft')} title={t('composer.attach_image')}>
     {isUploading ? <Loader2 className={cn('w-5 h-5 animate-spin')} /> : <ImageIcon className={cn('w-5 h-5')} />}
    </button>
    <input type="file" accept="image/*" className={cn('hidden')} ref={fileInputRef} onChange={onChangeImage} />
   </div>
   <button type="submit" disabled={isPending || isUploading || !content.trim()} className={cn('bg-gradient-to-r from-violet-600 to-indigo-900 text-white px-6 py-2 rounded-lg font-medium transition-opacity tn-disabled-dim')}>
    {isPending ? t('composer.posting') : t('composer.submit')}
   </button>
  </div>
 );
}

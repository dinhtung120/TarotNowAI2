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
    <select {...visibilityField} className={cn('bg-[#0f0f16] text-gray-400 text-sm border-none focus:ring-0 rounded-md py-1 px-2 cursor-pointer')} disabled={isPending}>
     <option value="public">{t('composer.visibility_public')}</option>
     <option value="private">{t('composer.visibility_private')}</option>
    </select>
    <button type="button" disabled={isUploading} onClick={() => fileInputRef.current?.click()} className={cn('text-[#8a2be2] hover:text-[#ff00ff] transition-colors p-2 rounded-full hover:bg-[#8a2be2]/10')} title={t('composer.attach_image')}>
     {isUploading ? <Loader2 className={cn('w-5 h-5 animate-spin')} /> : <ImageIcon className={cn('w-5 h-5')} />}
    </button>
    <input type="file" accept="image/*" className={cn('hidden')} ref={fileInputRef} onChange={onChangeImage} />
   </div>
   <button type="submit" disabled={isPending || (!content.trim() && !isUploading)} className={cn('bg-gradient-to-r from-[#8a2be2] to-[#4b0082] text-white px-6 py-2 rounded-lg font-medium hover:opacity-90 disabled:opacity-50 disabled:cursor-not-allowed transition-opacity')}>
    {isPending ? t('composer.posting') : t('composer.submit')}
   </button>
  </div>
 );
}

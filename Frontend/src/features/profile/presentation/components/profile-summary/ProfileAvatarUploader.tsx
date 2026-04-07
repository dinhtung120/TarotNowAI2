'use client';

import type { ChangeEvent } from 'react';
import Image from 'next/image';
import { Camera, Trophy } from 'lucide-react';
import { cn } from '@/lib/utils';
import { ProfileAvatarSpinner } from './ProfileAvatarSpinner';

interface ProfileAvatarUploaderProps {
 avatarAlt: string;
 avatarPreview: string | null;
 avatarUploading: boolean;
 displayName: string;
 isSubmitting: boolean;
 onAvatarSelect: (event: ChangeEvent<HTMLInputElement>) => void;
 uploadLabel: string;
}

export function ProfileAvatarUploader({
 avatarAlt,
 avatarPreview,
 avatarUploading,
 displayName,
 isSubmitting,
 onAvatarSelect,
 uploadLabel,
}: ProfileAvatarUploaderProps) {
 return (
  <div className={cn('relative w-28 h-28 shrink-0 group/avatar cursor-pointer')}>
   <input type="file" accept="image/jpeg,image/png,image/webp" onChange={onAvatarSelect} disabled={avatarUploading || isSubmitting} className={cn('absolute inset-0 w-full h-full opacity-0 z-50 cursor-pointer disabled:cursor-not-allowed')} title={uploadLabel} />
   <div className={cn('absolute inset-[-6px] rounded-full bg-gradient-to-tr from-[var(--purple-accent)]/30 via-transparent to-[var(--warning)]/20 blur-xl group-hover:inset-[-12px] transition-all duration-700 opacity-70')} />
   <div className={cn('w-full h-full rounded-full border-2 tn-border overflow-hidden relative z-10 shadow-2xl tn-surface group-hover/avatar:border-[var(--purple-accent)] transition-colors')}>
    <Image src={avatarPreview || `https://ui-avatars.com/api/?background=111&color=fff&name=${encodeURIComponent(displayName)}`} alt={avatarAlt} fill sizes="112px" unoptimized className={cn('object-cover transition-transform duration-1000 group-hover/avatar:scale-110', avatarUploading && 'opacity-50 blur-sm')} />
    <div className={cn('absolute inset-0 bg-black/40 flex flex-col justify-center items-center opacity-0 group-hover/avatar:opacity-100 transition-opacity duration-300')}>
     {avatarUploading ? <ProfileAvatarSpinner /> : <><Camera className={cn('w-6 h-6 text-white mb-1')} /><span className={cn('text-[8px] font-bold text-white uppercase tracking-wider')}>{uploadLabel}</span></>}
    </div>
   </div>
   <div className={cn('absolute -bottom-1 -right-1 w-8 h-8 tn-panel rounded-xl flex items-center justify-center text-[var(--warning)] shadow-xl z-20')}>
    <Trophy className={cn('w-4 h-4')} />
   </div>
  </div>
 );
}

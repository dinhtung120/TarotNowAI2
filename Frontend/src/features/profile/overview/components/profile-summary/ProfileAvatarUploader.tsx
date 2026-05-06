'use client';

import type { ChangeEvent } from 'react';
import Image from 'next/image';
import { Camera, Trophy } from 'lucide-react';
import { cn } from '@/lib/utils';
import { ProfileAvatarSpinner } from './ProfileAvatarSpinner';
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from '@/shared/http/assetUrl';

interface ProfileAvatarUploaderProps {
 avatarAlt: string;
 avatarPreview: string | null;
 avatarUploadProgress: number;
 avatarUploading: boolean;
 displayName: string;
 isSubmitting: boolean;
 onAvatarSelect: (event: ChangeEvent<HTMLInputElement>) => void;
 uploadLabel: string;
}

export function ProfileAvatarUploader({
 avatarAlt,
 avatarPreview,
 avatarUploadProgress,
 avatarUploading,
 displayName,
 isSubmitting,
 onAvatarSelect,
 uploadLabel,
}: ProfileAvatarUploaderProps) {
 const avatarSrc = resolveAvatarUrl(avatarPreview);
 const resolvedAvatarSrc = avatarSrc || `https://ui-avatars.com/api/?background=111&color=fff&name=${encodeURIComponent(displayName)}`;
 const unoptimizedAvatar = shouldUseUnoptimizedImage(resolvedAvatarSrc);

 return (
  <div className={cn('relative w-28 h-28 shrink-0 group cursor-pointer')}>
   <input type="file" accept="image/jpeg,image/png,image/webp,image/heic,image/heif" onChange={onAvatarSelect} disabled={avatarUploading || isSubmitting} className={cn('tn-disabled-dim absolute inset-0 w-full h-full opacity-0 z-50 cursor-pointer')} title={uploadLabel} />
   <div className={cn('tn-profile-avatar-glow absolute rounded-full transition-all duration-700 opacity-70')} />
   <div className={cn('w-full h-full rounded-full border-2 tn-border overflow-hidden relative z-10 shadow-2xl tn-surface tn-group-border-accent-50 transition-colors')}>
    <Image src={resolvedAvatarSrc} alt={avatarAlt} fill sizes="112px" unoptimized={unoptimizedAvatar} priority className={cn('tn-group-scale-110 object-cover transition-transform duration-1000', avatarUploading && 'opacity-50 blur-sm')} />
    <div className={cn('tn-group-fade-in-overlay absolute inset-0 bg-black/40 flex flex-col justify-center items-center transition-opacity duration-300')}>
     {avatarUploading ? <ProfileAvatarSpinner /> : <><Camera className={cn('w-6 h-6 text-white mb-1')} /><span className={cn('tn-text-2xs font-bold text-white uppercase tracking-wider')}>{uploadLabel}</span></>}
    </div>
   </div>
   {avatarUploading ? (
    <div className={cn('absolute -bottom-7 left-1/2 w-28 -translate-x-1/2')}>
     <progress
      className={cn('tn-progress tn-progress-sm tn-progress-success')}
      max={100}
      value={Math.max(5, Math.min(100, avatarUploadProgress))}
     />
     <p className={cn('mt-1 text-center text-[10px] font-medium text-emerald-200')}>{Math.max(0, Math.min(100, avatarUploadProgress))}%</p>
    </div>
   ) : null}
   <div className={cn('absolute -bottom-1 -right-1 w-8 h-8 tn-panel rounded-xl flex items-center justify-center tn-text-warning shadow-xl z-20')}>
    <Trophy className={cn('w-4 h-4')} />
   </div>
  </div>
 );
}

import Image from 'next/image';
import { cn } from '@/lib/utils';
import { resolveAvatarUrl, shouldUseUnoptimizedImage } from '@/shared/http/assetUrl';
import { ReaderStatusIndicator } from '@/features/reader/directory/components/readers-directory/ReaderStatusIndicator';

interface ReaderDetailHeaderProps {
 displayName: string;
 avatarUrl: string | null | undefined;
 status: string;
 fallbackLabel: string;
 labels: {
  online: string;
  busy: string;
  offline: string;
 };
}

export function ReaderDetailHeader({
 displayName,
 avatarUrl,
 status,
 fallbackLabel,
 labels,
}: ReaderDetailHeaderProps) {
 const avatarSrc = resolveAvatarUrl(avatarUrl);
 const unoptimizedAvatar = shouldUseUnoptimizedImage(avatarSrc);

 return (
  <div className={cn('flex items-center gap-4')}>
   <div className={cn('relative h-16 w-16 flex-shrink-0 overflow-hidden rounded-full border-2 tn-border tn-surface-strong')}>
    {avatarSrc ? (
     <Image src={avatarSrc} alt={displayName} fill sizes="64px" loading="lazy" unoptimized={unoptimizedAvatar} className={cn('h-full w-full object-cover bg-white')} />
    ) : (
     <div className={cn('flex h-full w-full items-center justify-center text-xl font-black tn-text-primary')}>
      {displayName.charAt(0)?.toUpperCase() || '?'}
     </div>
    )}
   </div>
   <div className={cn('min-w-0')}>
    <h3 className={cn('truncate text-xl font-black italic tracking-tight')}>{displayName || fallbackLabel}</h3>
    <div className={cn('mt-1')}>
     <ReaderStatusIndicator status={status} labels={labels} />
    </div>
   </div>
  </div>
 );
}

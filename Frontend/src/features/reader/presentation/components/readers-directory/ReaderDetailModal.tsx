import Image from 'next/image';
import { Gem, Loader2, MessageCircle, Star, X } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';
import { ReaderStatusIndicator } from './ReaderStatusIndicator';
import { resolveAvatarUrl } from '@/shared/infrastructure/http/assetUrl';

interface ReaderDetailModalProps {
 reader: ReaderProfile | null;
 bio: string;
 isStartingConversation: boolean;
 onClose: () => void;
 onStartConversation: () => Promise<void>;
 labels: {
  readerFallback: string;
  perQuestionSuffix: string;
  startConversationCta: string;
  online: string;
  busy: string;
  offline: string;
 };
}

export function ReaderDetailModal({
 reader,
 bio,
 isStartingConversation,
 onClose,
 onStartConversation,
 labels,
}: ReaderDetailModalProps) {
 if (!reader) return null;
 const avatarSrc = resolveAvatarUrl(reader.avatarUrl);

 return (
  <div className={cn('tn-reader-modal-root fixed inset-0 flex items-center justify-center animate-in fade-in duration-500')}>
   <div className={cn('absolute inset-0 tn-overlay-strong')} onClick={onClose} />
   <div className={cn('tn-reader-modal-panel relative z-10 max-w-3xl w-full tn-panel animate-in zoom-in-95 slide-in-from-bottom-10 duration-500 overflow-hidden')}>
    <div className={cn('absolute top-4 right-4')}>
     <button type="button" onClick={onClose} className={cn('w-9 h-9 rounded-xl tn-surface border tn-border-soft flex items-center justify-center tn-hover-surface-strong')}>
      <X className={cn('w-4 h-4')} />
     </button>
     </div>
    <div className={cn('space-y-6')}>
     <div className={cn('flex items-center gap-4')}>
      <div className={cn('w-16 h-16 rounded-full tn-surface-strong border-2 tn-border flex items-center justify-center text-xl font-black tn-text-primary overflow-hidden relative')}>
       {avatarSrc ? <Image src={avatarSrc} alt={reader.displayName} fill sizes="64px" unoptimized className={cn('w-full h-full object-cover bg-white')} /> : reader.displayName?.charAt(0)?.toUpperCase() || '?'}
      </div>
      <div className={cn('min-w-0')}>
       <h3 className={cn('text-2xl font-black italic tracking-tight line-clamp-1')}>{reader.displayName || labels.readerFallback}</h3>
       <div className={cn('mt-1')}><ReaderStatusIndicator status={reader.status} labels={{ online: labels.online, busy: labels.busy, offline: labels.offline }} /></div>
      </div>
     </div>
     <p className={cn('text-sm tn-text-secondary leading-relaxed')}>{bio}</p>
     <div className={cn('flex flex-wrap items-center gap-3')}>
      <div className={cn('flex items-center gap-1.5 tn-surface px-3 py-1.5 rounded-lg border tn-border-soft')}><Star className={cn('w-3.5 h-3.5 tn-text-warning')} fill="currentColor" /><span className={cn('text-xs font-black tn-text-primary')}>{reader.avgRating > 0 ? reader.avgRating.toFixed(1) : '--'}</span><span className={cn('tn-text-10 tn-text-tertiary font-bold')}>({reader.totalReviews})</span></div>
      <div className={cn('flex items-center gap-1.5 tn-bg-accent-10 px-3 py-1.5 rounded-lg border tn-border-accent-20')}><Gem className={cn('w-3.5 h-3.5 tn-text-accent')} /><span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent')}>{reader.diamondPerQuestion} {labels.perQuestionSuffix}</span></div>
     </div>
     {reader.specialties.length > 0 ? <div className={cn('flex flex-wrap gap-2')}>{reader.specialties.map((spec) => <span key={spec} className={cn('px-3 py-1 rounded-lg tn-surface tn-text-secondary tn-text-10 font-black uppercase tracking-wider border tn-border')}>{spec}</span>)}</div> : null}
     <div className={cn('pt-4 border-t tn-border-soft flex justify-end')}>
      <Button onClick={onStartConversation} disabled={isStartingConversation} className={cn('px-6 py-3')}>
       {isStartingConversation ? <Loader2 className={cn('w-4 h-4 animate-spin mr-2')} /> : <MessageCircle className={cn('w-4 h-4 mr-2')} />}
       {labels.startConversationCta}
      </Button>
     </div>
    </div>
   </div>
  </div>
 );
}

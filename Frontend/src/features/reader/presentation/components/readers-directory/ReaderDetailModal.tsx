import Image from 'next/image';
import { BriefcaseBusiness, Gem, Loader2, MessageCircle, Star } from 'lucide-react';
import { Button } from '@/shared/components/ui';
import Modal from '@/shared/components/ui/Modal';
import type { ReaderProfile } from '@/features/reader/application/actions';
import { cn } from '@/lib/utils';
import { ReaderStatusIndicator } from './ReaderStatusIndicator';
import { resolveAvatarUrl } from '@/shared/infrastructure/http/assetUrl';
import ReaderSocialLinksInline from '@/features/reader/presentation/components/ReaderSocialLinksInline';

interface ReaderDetailModalProps {
 reader: ReaderProfile | null;
 bio: string;
 isStartingConversation: boolean;
 onClose: () => void;
 onStartConversation: () => Promise<void>;
 labels: {
  readerFallback: string;
  perQuestionSuffix: string;
  yearsExperienceLabel: string;
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
  <Modal
   isOpen={Boolean(reader)}
   onClose={onClose}
   title={reader.displayName || labels.readerFallback}
   size="lg"
   className={cn('tn-reader-modal-panel')}
  >
   <div className={cn('custom-scrollbar max-h-[72vh] space-y-6 overflow-y-auto pr-1')}>
    <div className={cn('flex items-center gap-4')}>
     <div className={cn('relative h-16 w-16 flex-shrink-0 overflow-hidden rounded-full border-2 tn-border tn-surface-strong')}>
      {avatarSrc ? (
       <Image src={avatarSrc} alt={reader.displayName} fill sizes="64px" unoptimized className={cn('h-full w-full object-cover bg-white')} />
      ) : (
       <div className={cn('flex h-full w-full items-center justify-center text-xl font-black tn-text-primary')}>
        {reader.displayName?.charAt(0)?.toUpperCase() || '?'}
       </div>
      )}
     </div>
     <div className={cn('min-w-0')}>
      <h3 className={cn('truncate text-xl font-black italic tracking-tight')}>
       {reader.displayName || labels.readerFallback}
      </h3>
      <div className={cn('mt-1')}>
       <ReaderStatusIndicator
        status={reader.status}
        labels={{ online: labels.online, busy: labels.busy, offline: labels.offline }}
       />
      </div>
     </div>
    </div>

    <p className={cn('whitespace-pre-wrap text-sm leading-relaxed tn-text-primary md:text-base')}>{bio}</p>

    <div className={cn('flex flex-wrap items-center gap-3')}>
     <div className={cn('flex items-center gap-1.5 rounded-lg border tn-border-soft tn-surface px-3 py-1.5')}>
      <Star className={cn('h-3.5 w-3.5 tn-text-warning')} fill="currentColor" />
      <span className={cn('text-xs font-black tn-text-primary')}>
       {reader.avgRating > 0 ? reader.avgRating.toFixed(1) : '--'}
      </span>
      <span className={cn('tn-text-10 font-bold tn-text-tertiary')}>({reader.totalReviews})</span>
     </div>

     <div className={cn('flex items-center gap-1.5 rounded-lg border tn-border-accent-20 tn-bg-accent-10 px-3 py-1.5')}>
      <Gem className={cn('h-3.5 w-3.5 tn-text-accent')} />
      <span className={cn('tn-text-10 font-black uppercase tracking-widest tn-text-accent')}>
       {reader.diamondPerQuestion} {labels.perQuestionSuffix}
      </span>
     </div>

     <div className={cn('flex items-center gap-1.5 rounded-lg border tn-border-soft tn-surface px-3 py-1.5')}>
      <BriefcaseBusiness className={cn('h-3.5 w-3.5 tn-text-secondary')} />
      <span className={cn('tn-text-10 font-black uppercase tn-text-secondary')}>
       {reader.yearsOfExperience}+ {labels.yearsExperienceLabel}
      </span>
     </div>
    </div>

    {reader.specialties.length > 0 ? (
     <div className={cn('flex flex-wrap gap-2')}>
      {reader.specialties.map((spec) => (
       <span key={spec} className={cn('rounded-lg border tn-border px-3 py-1 tn-surface tn-text-10 font-black uppercase tracking-wider tn-text-secondary')}>
        {spec}
       </span>
      ))}
     </div>
    ) : null}

    <ReaderSocialLinksInline
     facebookUrl={reader.facebookUrl}
     instagramUrl={reader.instagramUrl}
     tikTokUrl={reader.tikTokUrl}
     size="md"
    />

    <div className={cn('flex justify-end border-t pt-6 tn-border-soft')}>
     <Button onClick={onStartConversation} disabled={isStartingConversation} className={cn('w-full px-6 py-3 font-bold sm:w-auto')}>
      {isStartingConversation ? <Loader2 className={cn('mr-2 h-4 w-4 animate-spin')} /> : <MessageCircle className={cn('mr-2 h-4 w-4')} />}
      {labels.startConversationCta}
     </Button>
    </div>
   </div>
  </Modal>
 );
}
